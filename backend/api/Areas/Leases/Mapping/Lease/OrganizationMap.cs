using Mapster;
using Pims.Dal.Entities.Helpers;
using Pims.Dal.Helpers.Extensions;
using System.Linq;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Lease.Models.Lease;

namespace Pims.Api.Areas.Lease.Mapping.Lease
{
    public class OrganizationMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.Organization, Model.OrganizationModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Address, src => src.Address)
                .Map(dest => dest.ContactName, src => src.GetFullName())
                .Map(dest => dest.Landline, src => src.GetLandlinePhoneNumber())
                .Map(dest => dest.Mobile, src => src.GetMobilePhoneNumber())
                .Map(dest => dest.Email, src => src.GetEmail());
                
        }
    }
}
