using Mapster;
using Pims.Dal.Entities.Helpers;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Lease.Models.Lease;

namespace Pims.Api.Areas.Lease.Mapping.Lease
{
    public class OrganizationMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsOrganization, Model.OrganizationModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.OrganizationName)
                .Map(dest => dest.Address, src => src.GetSingleAddress())
                .Map(dest => dest.ContactName, src => src.GetFirstPersonFullName())
                .Map(dest => dest.Landline, src => src.GetLandlinePhoneNumber())
                .Map(dest => dest.Mobile, src => src.GetMobilePhoneNumber())
                .Map(dest => dest.Email, src => src.GetEmail());

        }
    }
}
