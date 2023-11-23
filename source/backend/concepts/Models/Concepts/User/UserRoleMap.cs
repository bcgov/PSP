using Mapster;
using Pims.Api.Concepts.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Concepts.Models.Concepts.User
{
    public class UserRoleMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsUserRole, UserRoleModel>()
                .MaxDepth(3)
                .Map(dest => dest.Id, src => src.UserRoleId)
                .Map(dest => dest.UserId, src => src.UserId)
                .Map(dest => dest.RoleId, src => src.RoleId)
                .Map(dest => dest.Role, src => src.Role)
                .Map(dest => dest.User, src => src.User)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<UserRoleModel, Entity.PimsUserRole>()
                .Map(dest => dest.UserRoleId, src => src.Id)
                .Map(dest => dest.UserId, src => src.UserId)
                .Map(dest => dest.RoleId, src => src.RoleId)
                .Map(dest => dest.Role, src => src.Role)
                .Map(dest => dest.User, src => src.User)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();
        }
    }
}
