using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Property
{
    public class ManagementActivityPropertyMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsManagementActivityProperty, ManagementActivityPropertyModel>()
                .Map(dest => dest.Id, src => src.ManagementActivityPropertyId)
                .Map(dest => dest.ManagementActivityId, src => src.ManagementActivityId)
                .Map(dest => dest.ManagementActivity, src => src.ManagementActivity)
                .Map(dest => dest.PropertyId, src => src.PropertyId)
                .Map(dest => dest.Property, src => src.Property)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<ManagementActivityPropertyModel, Entity.PimsManagementActivityProperty>()
                .Map(dest => dest.ManagementActivityPropertyId, src => src.Id)
                .Map(dest => dest.ManagementActivityId, src => src.ManagementActivityId)
                .Map(dest => dest.PropertyId, src => src.PropertyId)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();
        }
    }
}
