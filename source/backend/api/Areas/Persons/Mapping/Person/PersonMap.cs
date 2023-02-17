using System.Linq;
using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Persons.Models.Person;

namespace Pims.Api.Areas.Persons.Mapping.Person
{
    public class PersonMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPerson, Model.PersonModel>()
                .Map(dest => dest.Id, src => src.PersonId)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.Surname, src => src.Surname)
                .Map(dest => dest.FirstName, src => src.FirstName)
                .Map(dest => dest.MiddleNames, src => src.MiddleNames)
                .Map(dest => dest.PreferredName, src => src.PreferredName)
                .Map(dest => dest.Comment, src => src.Comment)
                .Map(dest => dest.UseOrganizationAddress, src => src.UseOrganizationAddress ?? false)
                .Map(dest => dest.Addresses, src => src.PimsPersonAddresses)
                .Map(dest => dest.ContactMethods, src => src.PimsContactMethods)
                .Map(dest => dest.PersonOrganizationId, src => src.GetPersonOrganizationId())
                .Map(dest => dest.PersonOrganizationRowVersion, src => src.GetPersonOrganizationRowVersion())
                .Inherits<Entity.IBaseAppEntity, Api.Models.BaseAppModel>()
                .AfterMapping((src, dest) =>
                {
                    // The database supports many organizations for a person but the app currently supports only one linked organization per person.
                    var linkedOrganization = src.PimsPersonOrganizations?.FirstOrDefault(p => p != null && p.Organization != null)?.Organization;
                    if (linkedOrganization != null)
                    {
                        dest.Organization = new Model.OrganizationLinkModel { Id = linkedOrganization.Internal_Id, Text = linkedOrganization.Name };
                    }
                });

            config.NewConfig<Model.PersonModel, Entity.PimsPerson>()
                .Map(dest => dest.PersonId, src => src.Id)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.Surname, src => src.Surname)
                .Map(dest => dest.FirstName, src => src.FirstName)
                .Map(dest => dest.MiddleNames, src => src.MiddleNames)
                .Map(dest => dest.PreferredName, src => src.PreferredName)
                .Map(dest => dest.Comment, src => src.Comment)
                .Map(dest => dest.UseOrganizationAddress, src => src.UseOrganizationAddress)
                .Map(dest => dest.PimsPersonAddresses, src => src.Addresses)
                .Map(dest => dest.PimsContactMethods, src => src.ContactMethods)
                .Map(dest => dest.PimsPersonOrganizations, src => src.Organization != null ? new[] { src } : null)
                .Inherits<Api.Models.BaseAppModel, Entity.IBaseAppEntity>()
                .IgnoreNullValues(true)
                .AfterMapping((src, dest) =>
                {
                    // ensure many-to-many PersonAddress entities have set the proper FK to owning Person
                    foreach (var pa in dest.PimsPersonAddresses)
                    {
                        pa.PersonId = dest.PersonId;
                    }
                    foreach (var cm in dest.PimsContactMethods)
                    {
                        cm.PersonId = dest.PersonId;
                    }
                });
        }
    }
}
