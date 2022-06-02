using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.AccessRequest
{
    public class UserRoleMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsUserRole, UserRoleModel>()
                .PreserveReference(true)
                .Map(dest => dest.UserId, src => src.UserId)
                .Map(dest => dest.RoleId, src => src.RoleId)
                .Map(dest => dest.Role, src => src.Role)
                .Map(dest => dest.User, src => src.User)
                .Inherits<Entity.IBaseAppEntity, Api.Models.BaseAppModel>();

            config.NewConfig<UserRoleModel, Entity.PimsUserRole>()
                .PreserveReference(true)
                .Map(dest => dest.UserId, src => src.UserId)
                .Map(dest => dest.RoleId, src => src.RoleId)
                .Map(dest => dest.Role, src => src.Role)
                .Map(dest => dest.User, src => src.User)
                .Inherits<Api.Models.BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
