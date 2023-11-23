using Mapster;
using Pims.Api.Concepts.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Concepts.Models.Concepts.Role.AccessRequest
{
    public class RoleMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsRole, RoleModel>()
                .Map(dest => dest.Id, src => src.RoleId)
                .Map(dest => dest.RoleUid, src => src.RoleUid)
                .Map(dest => dest.KeycloakGroupId, src => src.KeycloakGroupId)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsPublic, src => src.IsPublic)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.RoleClaims, src => src.PimsRoleClaims)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<RoleModel, Entity.PimsRole>()
                .Map(dest => dest.RoleId, src => src.Id)
                .Map(dest => dest.RoleUid, src => src.RoleUid)
                .Map(dest => dest.KeycloakGroupId, src => src.KeycloakGroupId)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsPublic, src => src.IsPublic)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.PimsRoleClaims, src => src.RoleClaims)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();
        }
    }
}
