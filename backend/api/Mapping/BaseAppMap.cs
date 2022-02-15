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
                .Map(dest => dest.UpdatedOn, src => src.AppLastUpdateTimestamp)
                .Map(dest => dest.UpdatedByName, src => src.AppLastUpdateUserid)
                .Inherits<Entity.IBaseEntity, Models.BaseModel>();

            config.NewConfig<Models.BaseAppModel, Entity.IDisableBaseAppEntity>()
                .Inherits<Models.BaseAppModel, Entity.IBaseAppEntity>();

            config.NewConfig<Models.BaseAppModel, Entity.IBaseAppEntity>()
                .Map(dest => dest.AppCreateTimestamp, src => src.AppCreateTimestamp)
                .Map(dest => dest.AppLastUpdateTimestamp, src => src.UpdatedOn)
                .Inherits<Models.BaseModel, Entity.IBaseEntity>();
        }
    }
}
