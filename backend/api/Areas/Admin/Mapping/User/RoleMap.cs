using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Admin.Models.User;

namespace Pims.Api.Areas.Admin.Mapping.User
{
    public class RoleMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsRole, Model.RoleModel>()
                .Map(dest => dest.Id, src => src.RoleId)
                .Map(dest => dest.KeycloakGroupId, src => src.KeycloakGroupId)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsPublic, src => src.IsPublic)
                .Inherits<Entity.IBaseEntity, Api.Models.BaseModel>();

            config.NewConfig<Model.RoleModel, Entity.PimsRole>()
                .Map(dest => dest.RoleId, src => src.Id)
                .Map(dest => dest.KeycloakGroupId, src => src.KeycloakGroupId)
                .Map(dest => dest.RoleUid, src => src.KeycloakGroupId)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsPublic, src => src.IsPublic)
                .Inherits<Api.Models.BaseModel, Entity.IBaseEntity>();

            config.NewConfig<Entity.PimsUserRole, Model.RoleModel>()
                .Map(dest => dest.Id, src => src.RoleId)
                .Map(dest => dest.KeycloakGroupId, src => src.Role.KeycloakGroupId)
                .Map(dest => dest.Name, src => src.Role.Name);

            config.NewConfig<Model.RoleModel, Entity.PimsUserRole>()
                .Map(dest => dest.RoleId, src => src.Id)
                .Map(dest => dest.Role, src => src)
                .Map(dest => dest.ConcurrencyControlNumber, src => src.RowVersion);
        }
    }
}
