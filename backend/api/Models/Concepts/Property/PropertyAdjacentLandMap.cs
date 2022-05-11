using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class PropertyAdjacentLandMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPropPropAdjacentLandType, PropertyAdjacentLandModel>()
                .PreserveReference(true)
                .Map(dest => dest.Id, src => src.PropPropAdjacentLandTypeId)
                .Map(dest => dest.PropertyId, src => src.PropertyId)
                .Map(dest => dest.PropertyAdjacentLandTypeCode, src => src.PropertyAdjacentLandTypeCodeNavigation)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<PropertyAdjacentLandModel, Entity.PimsPropPropAdjacentLandType>()
                .Map(dest => dest.PropPropAdjacentLandTypeId, src => src.Id)
                .Map(dest => dest.PropertyId, src => src.PropertyId)
                .Map(dest => dest.PropertyAdjacentLandTypeCode, src => src.PropertyAdjacentLandTypeCode.Id)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
