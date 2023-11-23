using Mapster;
using Pims.Api.Concepts.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Concepts.Models.Concepts.Property
{
    public class PropertyTenureMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPropPropTenureType, PropertyTenureModel>()
                .Map(dest => dest.Id, src => src.PropPropTenureTypeId)
                .Map(dest => dest.PropertyId, src => src.PropertyId)
                .Map(dest => dest.PropertyTenureTypeCode, src => src.PropertyTenureTypeCodeNavigation)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<PropertyTenureModel, Entity.PimsPropPropTenureType>()
                .Map(dest => dest.PropPropTenureTypeId, src => src.Id)
                .Map(dest => dest.PropertyId, src => src.PropertyId)
                .Map(dest => dest.PropertyTenureTypeCode, src => src.PropertyTenureTypeCode.Id)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();
        }
    }
}
