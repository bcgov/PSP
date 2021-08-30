using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Keycloak.Models.User;

namespace Pims.Api.Areas.Keycloak.Mapping.User
{
    public class RoleMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.Role, Model.RoleModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Inherits<Entity.BaseEntity, Api.Models.BaseModel>();

            config.NewConfig<Model.RoleModel, Entity.Role>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Inherits<Api.Models.BaseModel, Entity.BaseEntity>();

            config.NewConfig<Entity.UserRole, Model.RoleModel>()
                .Map(dest => dest.Id, src => src.UserId);

            config.NewConfig<Model.RoleModel, Entity.UserRole>()
                .Map(dest => dest.UserId, src => src.Id);
        }
    }
}
