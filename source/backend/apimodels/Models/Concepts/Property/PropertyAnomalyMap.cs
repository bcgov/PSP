using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Property
{
    public class PropertyAnomalyMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPropPropAnomalyTyp, PropertyAnomalyModel>()
                .Map(dest => dest.Id, src => src.PropPropAnomalyTypeId)
                .Map(dest => dest.PropertyId, src => src.PropertyId)
                .Map(dest => dest.PropertyAnomalyTypeCode, src => src.PropertyAnomalyTypeCodeNavigation)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<PropertyAnomalyModel, Entity.PimsPropPropAnomalyTyp>()
                .Map(dest => dest.PropPropAnomalyTypeId, src => src.Id)
                .Map(dest => dest.PropertyId, src => src.PropertyId)
                .Map(dest => dest.PropertyAnomalyTypeCode, src => src.PropertyAnomalyTypeCode.Id)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();
        }
    }
}
