using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Mapping
{
    public class BaseMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.BaseEntity, Models.BaseModel>()
                .Map(dest => dest.RowVersion, src => src.RowVersion);

            config.NewConfig<Models.BaseModel, Entity.BaseEntity>()
                .Map(dest => dest.RowVersion, src => src.RowVersion);
        }
    }
}
