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
                .Map(dest => dest.RowVersion, src => src.ConcurrencyControlNumber)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.Surname, src => src.Surname)
                .Map(dest => dest.FirstName, src => src.FirstName)
                .Map(dest => dest.MiddleNames, src => src.MiddleNames)
                .Map(dest => dest.PreferredName, src => src.PreferredName)
                .Map(dest => dest.Comment, src => src.Comment)
                .Map(dest => dest.Addresses, src => src.PimsPersonAddresses)
                .Map(dest => dest.ContactMethods, src => src.PimsContactMethods)
                .Map(dest => dest.OrganizationId, src => src.GetLinkedOrganizationId())
                .Map(dest => dest.PersonOrganizationId, src => src.GetPersonOrganizationId())
                .Map(dest => dest.PersonOrganizationRowVersion, src => src.GetPersonOrganizationRowVersion());

            config.NewConfig<Model.PersonModel, Entity.PimsPerson>()
                .Map(dest => dest.PersonId, src => src.Id)
                .Map(dest => dest.ConcurrencyControlNumber, src => src.RowVersion)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.Surname, src => src.Surname)
                .Map(dest => dest.FirstName, src => src.FirstName)
                .Map(dest => dest.MiddleNames, src => src.MiddleNames)
                .Map(dest => dest.PreferredName, src => src.PreferredName)
                .Map(dest => dest.Comment, src => src.Comment)
                .Map(dest => dest.PimsPersonAddresses, src => src.Addresses)
                .Map(dest => dest.PimsContactMethods, src => src.ContactMethods)
                .Map(dest => dest.PimsPersonOrganizations, src => src.OrganizationId != null ? new[] { src } : null)
                .IgnoreNullValues(true);
        }
    }
}
