using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Keycloak.Models.User;

namespace Pims.Api.Areas.Keycloak.Mapping.User
{
    public class AccessRequestRoleMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.AccessRequestRole, Model.AccessRequestRoleModel>()
                .Map(dest => dest.Id, src => src.RoleId)
                .Map(dest => dest.Name, src => src.Role == null ? null : src.Role.Name)
                .Inherits<Entity.BaseAppEntity, Api.Models.BaseAppModel>();

            config.NewConfig<Model.AccessRequestRoleModel, Entity.AccessRequestRole>()
                .Map(dest => dest.RoleId, src => src.Id)
                .Map(dest => dest.Role, src => new Entity.Role()
                {
                    Id = src.Id,
                    Name = src.Name
                })
                .Inherits<Api.Models.BaseAppModel, Entity.BaseAppEntity>();
        }
    }
}
