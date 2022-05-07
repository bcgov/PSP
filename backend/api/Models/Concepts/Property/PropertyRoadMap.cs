using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class PropertyRoadMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPropPropRoadType, PropertyRoadModel>()
                .PreserveReference(true)
                .Map(dest => dest.Id, src => src.PropPropRoadTypeId)
                .Map(dest => dest.PropertyRoadTypeCode, src => src.PropertyRoadTypeCodeNavigation)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<PropertyRoadModel, Entity.PimsPropPropRoadType>()
                .Map(dest => dest.PropPropRoadTypeId, src => src.Id)
                .Map(dest => dest.PropertyRoadTypeCode, src => src.PropertyRoadTypeCode.Id)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
