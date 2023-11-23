using Mapster;
using Pims.Api.Concepts.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Concepts.Models.Concepts.Property
{
    public class PropertyRoadMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPropPropRoadType, PropertyRoadModel>()
                .Map(dest => dest.Id, src => src.PropPropRoadTypeId)
                .Map(dest => dest.PropertyId, src => src.PropertyId)
                .Map(dest => dest.PropertyRoadTypeCode, src => src.PropertyRoadTypeCodeNavigation)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<PropertyRoadModel, Entity.PimsPropPropRoadType>()
                .Map(dest => dest.PropPropRoadTypeId, src => src.Id)
                .Map(dest => dest.PropertyId, src => src.PropertyId)
                .Map(dest => dest.PropertyRoadTypeCode, src => src.PropertyRoadTypeCode.Id)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();
        }
    }
}
