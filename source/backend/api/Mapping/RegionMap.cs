using Mapster;
using Pims.Api.Concepts.Models.Concepts.Address;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Mapping
{
    public class RegionMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsRegion, CodeTypeModel>()
                .Map(dest => dest.Code, src => src.RegionCode)
                .Map(dest => dest.Description, src => src.RegionName)
                .Map(dest => dest.DisplayOrder, src => src.DisplayOrder);
        }
    }
}
