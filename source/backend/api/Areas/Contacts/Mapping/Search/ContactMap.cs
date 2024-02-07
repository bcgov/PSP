using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Contact.Models.Search;

namespace Pims.Api.Areas.Contact.Mapping.Search
{
    public class ContactMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsContactMgrVw, Model.ContactSummaryModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.Organization, src => src.Organization)
                .Map(dest => dest.Summary, src => src.Summary)
                .Map(dest => dest.Surname, src => src.Surname)
                .Map(dest => dest.FirstName, src => src.FirstName)
                .Map(dest => dest.MiddleNames, src => src.MiddleNames)
                .Map(dest => dest.OrganizationName, src => src.OrganizationName)
                .Map(dest => dest.Email, src => src.EmailAddress)
                .Map(dest => dest.MailingAddress, src => src.MailingAddress)
                .Map(dest => dest.MunicipalityName, src => src.MunicipalityName)
                .Map(dest => dest.ProvinceState, src => src.ProvinceState)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled);
        }
    }
}
