using Mapster;
using Pims.Api.Models;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Keycloak.Models.Role;

namespace Pims.Api.Areas.Admin.Keycloak.Role
{
    public class UpdateRoleMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.Role, Model.Update.RoleModel>()
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsPublic, src => src.IsPublic)
                .Inherits<Entity.BaseEntity, BaseModel>();

            config.NewConfig<Model.Update.RoleModel, Entity.Role>()
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsPublic, src => src.IsPublic)
                .Inherits<BaseModel, Entity.BaseEntity>();

            config.NewConfig<Entity.BaseAppEntity, BaseAppModel>()
                .Map(dest => dest.RowVersion, src => src.RowVersion);
        }
    }
}
