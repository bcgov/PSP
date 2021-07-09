using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Admin.Models.User;

namespace Pims.Api.Areas.Admin.Mapping.User
{
    public class RoleMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.Role, Model.RoleModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Key, src => src.Key)
                .Map(dest => dest.KeycloakGroupId, src => src.KeycloakGroupId)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsPublic, src => src.IsPublic)
                .Inherits<Entity.BaseAppEntity, Api.Models.BaseAppModel>();

            config.NewConfig<Model.RoleModel, Entity.Role>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Key, src => src.Key)
                .Map(dest => dest.KeycloakGroupId, src => src.KeycloakGroupId)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsPublic, src => src.IsPublic)
                .Inherits<Api.Models.BaseAppModel, Entity.BaseAppEntity>();

            config.NewConfig<Entity.UserRole, Model.RoleModel>()
                .Map(dest => dest.Id, src => src.RoleId)
                .Map(dest => dest.Key, src => src.Role.Key)
                .Map(dest => dest.Name, src => src.Role.Name);

            config.NewConfig<Model.RoleModel, Entity.UserRole>()
                .Map(dest => dest.RoleId, src => src.Id);
        }
    }
}
