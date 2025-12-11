using Mapster;
using Pims.Api.Models.Base;
using Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Person
{
    public class PersonMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config
                .NewConfig<PimsPerson, PersonModel>()
                .Map(dest => dest.Id, src => src.PersonId)
                .Map(dest => dest.Surname, src => src.Surname)
                .Map(dest => dest.FirstName, src => src.FirstName)
                .Map(dest => dest.MiddleNames, src => src.MiddleNames)
                .Map(dest => dest.NameSuffix, src => src.NameSuffix)
                .Map(dest => dest.PreferredName, src => src.PreferredName)
                .Map(dest => dest.BirthDate, src => src.BirthDate)
                .Map(dest => dest.Comment, src => src.Comment)
                .Map(dest => dest.AddressComment, src => src.AddressComment)
                .Map(dest => dest.UseOrganizationAddress, src => src.UseOrganizationAddress)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.ManagementActivityId, src => src.PimsPropertyActivityId)
                .Map(dest => dest.ContactMethods, src => src.PimsContactMethods)
                .Map(dest => dest.PersonAddresses, src => src.PimsPersonAddresses)
                .Map(dest => dest.PersonOrganizations, src => src.PimsPersonOrganizations)
                .Inherits<IBaseEntity, BaseConcurrentModel>();

            config
                .NewConfig<PersonModel, PimsPerson>()
                .Map(dest => dest.PersonId, src => src.Id)
                .Map(dest => dest.Surname, src => src.Surname)
                .Map(dest => dest.FirstName, src => src.FirstName)
                .Map(dest => dest.MiddleNames, src => src.MiddleNames)
                .Map(dest => dest.NameSuffix, src => src.NameSuffix)
                .Map(dest => dest.PreferredName, src => src.PreferredName)
                .Map(dest => dest.BirthDate, src => src.BirthDate)
                .Map(dest => dest.Comment, src => src.Comment)
                .Map(dest => dest.AddressComment, src => src.AddressComment)
                .Map(dest => dest.UseOrganizationAddress, src => src.UseOrganizationAddress)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.PimsPropertyActivityId, src => src.ManagementActivityId)
                .Map(dest => dest.PimsContactMethods, src => src.ContactMethods)
                .Map(dest => dest.PimsPersonAddresses, src => src.PersonAddresses)
                .Map(dest => dest.PimsPersonOrganizations, src => src.PersonOrganizations)
                .Inherits<BaseConcurrentModel, IBaseEntity>();

            config
                .NewConfig<PimsPersonHist, PimsPerson>()
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.Surname, src => src.Surname)
                .Map(dest => dest.FirstName, src => src.FirstName)
                .Map(dest => dest.MiddleNames, src => src.MiddleNames)
                .Map(dest => dest.NameSuffix, src => src.NameSuffix)
                .Map(dest => dest.PreferredName, src => src.PreferredName)
                .Map(dest => dest.BirthDate, src => src.BirthDate)
                .Map(dest => dest.Comment, src => src.Comment)
                .Map(dest => dest.AddressComment, src => src.AddressComment)
                .Map(dest => dest.UseOrganizationAddress, src => src.UseOrganizationAddress)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.PimsPropertyActivityId, src => src.PimsPropertyActivityId);
        }
    }
}
