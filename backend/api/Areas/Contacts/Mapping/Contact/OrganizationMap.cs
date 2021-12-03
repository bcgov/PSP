using Mapster;
using Pims.Dal.Entities.Helpers;
using Pims.Dal.Helpers.Extensions;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Contact.Models.Contact;

namespace Pims.Api.Areas.Contact.Mapping.Contact
{
    public class OrganizationMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsOrganization, Model.OrganizationModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.OrganizationName)
                .Map(dest => dest.Address, src => src.GetSingleAddress())
                .Map(dest => dest.ContactName, src => src.GetFullName())
                .Map(dest => dest.Landline, src => src.GetLandlinePhoneNumber())
                .Map(dest => dest.Mobile, src => src.GetMobilePhoneNumber())
                .Map(dest => dest.Email, src => src.GetEmail());

        }
    }
}
