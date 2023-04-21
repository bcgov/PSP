using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class PropertyAnomalyMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPropPropAnomalyType, PropertyAnomalyModel>()
                .Map(dest => dest.Id, src => src.PropPropAnomalyTypeId)
                .Map(dest => dest.PropertyId, src => src.PropertyId)
                .Map(dest => dest.PropertyAnomalyTypeCode, src => src.PropertyAnomalyTypeCodeNavigation)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<PropertyAnomalyModel, Entity.PimsPropPropAnomalyType>()
                .Map(dest => dest.PropPropAnomalyTypeId, src => src.Id)
                .Map(dest => dest.PropertyId, src => src.PropertyId)
                .Map(dest => dest.PropertyAnomalyTypeCode, src => src.PropertyAnomalyTypeCode.Id)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
