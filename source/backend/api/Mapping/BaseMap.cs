using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Mapping
{
    public class BaseMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.IBaseEntity, BaseConcurrentModel>()
                .Map(dest => dest.RowVersion, src => src.ConcurrencyControlNumber);

            config.NewConfig<BaseConcurrentModel, Entity.IBaseEntity>()
                .Map(dest => dest.ConcurrencyControlNumber, src => src.RowVersion);
        }
    }
}
