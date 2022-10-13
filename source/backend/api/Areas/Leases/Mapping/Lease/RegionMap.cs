using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Lease.Models.Lease;

namespace Pims.Api.Areas.Lease.Mapping.Lease
{
    public class RegionMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsRegion, Model.RegionModel>()
                .Map(dest => dest.RegionCode, src => src.RegionCode)
                .Map(dest => dest.RegionName, src => src.RegionName);
        }
    }
}
