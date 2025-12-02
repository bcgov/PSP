using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Property
{
    public class PropertyTenureCleanupMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config
                .NewConfig<Entity.PimsPropTenureCleanup, PropertyTenureCleanupModel>()
                .Map(dest => dest.Id, src => src.PropTenureCleanupId)
                .Map(dest => dest.PropertyId, src => src.PropertyId)
                .Map(dest => dest.TenureCleanupTypeCode, src => src.TenureCleanupTypeCodeNavigation)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config
                .NewConfig<PropertyTenureCleanupModel, Entity.PimsPropTenureCleanup>()
                .Map(dest => dest.PropTenureCleanupId, src => src.Id)
                .Map(dest => dest.PropertyId, src => src.PropertyId)
                .Map(dest => dest.TenureCleanupTypeCode, src => src.TenureCleanupTypeCode.Id)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();
        }
    }
}
