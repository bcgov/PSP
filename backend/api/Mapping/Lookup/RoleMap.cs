using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Models.Lookup;

namespace Pims.Api.Mapping.Lookup
{
    public class RoleMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.Role, Model.RoleModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Key, src => src.Key)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsPublic, src => src.IsPublic)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.SortOrder, src => src.SortOrder)
                .Map(dest => dest.Type, src => src.GetType().Name)
                .Inherits<Entity.BaseAppEntity, Models.BaseAppModel>();

            config.NewConfig<Model.RoleModel, Entity.Role>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Key, src => src.Key)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsPublic, src => src.IsPublic)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.SortOrder, src => src.SortOrder)
                .Inherits<Models.BaseAppModel, Entity.BaseAppEntity>();
        }
    }
}
