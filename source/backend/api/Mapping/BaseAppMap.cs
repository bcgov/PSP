using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Mapping
{
    public class BaseAppMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.IDisableBaseAppEntity, BaseAuditModel>()
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<Entity.IBaseAppEntity, BaseAuditModel>()
                .Map(dest => dest.AppCreateTimestamp, src => src.AppCreateTimestamp)
                .Map(dest => dest.AppLastUpdateTimestamp, src => src.AppLastUpdateTimestamp)
                .Map(dest => dest.AppLastUpdateUserid, src => src.AppLastUpdateUserid)
                .Map(dest => dest.AppCreateUserid, src => src.AppCreateUserid)
                .Map(dest => dest.AppLastUpdateUserGuid, src => src.AppLastUpdateUserGuid)
                .Map(dest => dest.AppCreateUserGuid, src => src.AppCreateUserGuid)
                .Inherits<Entity.IBaseEntity, BaseConcurrentModel>();

            config.NewConfig<BaseAuditModel, Entity.IDisableBaseAppEntity>()
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();

            config.NewConfig<BaseAuditModel, Entity.IBaseAppEntity>()
                .Map(dest => dest.AppCreateTimestamp, src => src.AppCreateTimestamp)
                .Map(dest => dest.AppLastUpdateTimestamp, src => src.AppLastUpdateTimestamp)
                .Map(dest => dest.AppLastUpdateUserid, src => src.AppLastUpdateUserid)
                .Map(dest => dest.AppCreateUserid, src => src.AppCreateUserid)
                .Map(dest => dest.AppLastUpdateUserGuid, src => src.AppLastUpdateUserGuid)
                .Map(dest => dest.AppCreateUserGuid, src => src.AppCreateUserGuid)
                .Inherits<BaseConcurrentModel, Entity.IBaseEntity>();
        }
    }
}
