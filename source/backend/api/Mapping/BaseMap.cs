using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Mapping
{
    public class BaseMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.IBaseEntity, Models.BaseModel>()
                .Map(dest => dest.RowVersion, src => src.ConcurrencyControlNumber);

            config.NewConfig<Models.BaseModel, Entity.IBaseEntity>()
                .Map(dest => dest.ConcurrencyControlNumber, src => src.RowVersion);
        }
    }
}
