using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Keycloak.Models.Role;

namespace Pims.Api.Areas.Admin.Keycloak.Role
{
    public class UpdateRoleMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsRole, Model.Update.RoleModel>()
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsPublic, src => src.IsPublic)
                .Inherits<Entity.IBaseEntity, BaseConcurrentModel>();

            config.NewConfig<Model.Update.RoleModel, Entity.PimsRole>()
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsPublic, src => src.IsPublic)
                .Inherits<BaseConcurrentModel, Entity.IBaseEntity>();

            config.NewConfig<Entity.IDisableBaseAppEntity, BaseAuditModel>()
                .Map(dest => dest.RowVersion, src => src.ConcurrencyControlNumber);
        }
    }
}
