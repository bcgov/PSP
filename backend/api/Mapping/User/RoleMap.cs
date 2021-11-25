using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Models.User;

namespace Pims.Api.Mapping.User
{
    public class RoleMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsRole, Model.RoleModel>()
                .Map(dest => dest.Id, src => src.RoleId)
                .Map(dest => dest.KeycloakGroupId, src => src.KeycloakGroupId)
                .Map(dest => dest.IsPublic, src => src.IsPublic)
                .Map(dest => dest.Users, src => src.GetUsers())
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Inherits<Entity.IBaseEntity, Models.BaseModel>();

            config.NewConfig<Model.RoleModel, Entity.PimsRole>()
                .Map(dest => dest.RoleId, src => src.Id)
                .Map(dest => dest.KeycloakGroupId, src => src.KeycloakGroupId)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsPublic, src => src.IsPublic)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Inherits<Models.BaseModel, Entity.IBaseEntity>();

            config.NewConfig<Model.RoleModel, Entity.PimsUserRole>()
                .Map(dest => dest.RoleId, src => src.Id)
                .Map(dest => dest.Role, src => src);
        }
    }
}
