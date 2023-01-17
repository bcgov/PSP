using Mapster;
using Pims.Api.Models;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Mapping
{
    public class RegionMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsRegion, RegionModel>()
                .Map(dest => dest.Id, src => src.RegionCode)
                .Map(dest => dest.Description, src => src.RegionName);
        }
    }
}
