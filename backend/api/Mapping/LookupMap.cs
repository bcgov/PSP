using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Mapping
{
    public class LookupMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.LookupEntity, Models.LookupModel>()
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.SortOrder, src => src.SortOrder)
                .Map(dest => dest.Type, src => src.GetType().Name)
                .Inherits<Entity.BaseEntity, Models.BaseModel>();

            config.NewConfig<Models.LookupModel, Entity.LookupEntity>()
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.SortOrder, src => src.SortOrder)
                .Inherits<Models.BaseModel, Entity.BaseEntity>();
        }
    }
}
