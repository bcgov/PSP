using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Mapping
{
    public class BaseAppMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.IDisableBaseAppEntity, Models.BaseAppModel>()
                .Inherits<Entity.IBaseAppEntity, Models.BaseAppModel>();

            config.NewConfig<Entity.IBaseAppEntity, Models.BaseAppModel>()
                .Map(dest => dest.AppCreateTimestamp, src => src.AppCreateTimestamp)
                .Map(dest => dest.AppLastUpdateTimestamp, src => src.AppLastUpdateTimestamp)
                .Map(dest => dest.AppLastUpdateUserid, src => src.AppLastUpdateUserid)
                .Map(dest => dest.AppCreateUserid, src => src.AppCreateUserid)
                .Map(dest => dest.AppLastUpdateUserGuid, src => src.AppLastUpdateUserGuid)
                .Map(dest => dest.AppCreateUserGuid, src => src.AppCreateUserGuid)
                .Inherits<Entity.IBaseEntity, Models.BaseModel>();

            config.NewConfig<Models.BaseAppModel, Entity.IDisableBaseAppEntity>()
                .Inherits<Models.BaseAppModel, Entity.IBaseAppEntity>();

            config.NewConfig<Models.BaseAppModel, Entity.IBaseAppEntity>()
                .Map(dest => dest.AppCreateTimestamp, src => src.AppCreateTimestamp)
                .Map(dest => dest.AppLastUpdateTimestamp, src => src.AppLastUpdateTimestamp)
                .Map(dest => dest.AppLastUpdateUserid, src => src.AppLastUpdateUserid)
                .Map(dest => dest.AppCreateUserid, src => src.AppCreateUserid)
                .Map(dest => dest.AppLastUpdateUserGuid, src => src.AppLastUpdateUserGuid)
                .Map(dest => dest.AppCreateUserGuid, src => src.AppCreateUserGuid)
                .Inherits<Models.BaseModel, Entity.IBaseEntity>();
        }
    }
}
