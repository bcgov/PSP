using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Mapping
{
    public class BaseAppMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.BaseAppEntity, Models.BaseAppModel>()
                .Map(dest => dest.CreatedOn, src => src.CreatedOn)
                .Map(dest => dest.UpdatedOn, src => src.UpdatedOn)
                .Map(dest => dest.UpdatedByName, src => src.UpdatedByName ?? src.CreatedByName)
                .Map(dest => dest.UpdatedByEmail, src => src.UpdatedByEmail ?? src.CreatedByEmail)
                .Inherits<Entity.BaseEntity, Models.BaseModel>();

            config.NewConfig<Models.BaseAppModel, Entity.BaseAppEntity>()
                .Map(dest => dest.CreatedOn, src => src.CreatedOn)
                .Map(dest => dest.UpdatedOn, src => src.UpdatedOn)
                .Inherits<Models.BaseModel, Entity.BaseEntity>();
        }
    }
}
