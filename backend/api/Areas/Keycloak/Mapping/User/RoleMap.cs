using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Keycloak.Models.User;

namespace Pims.Api.Areas.Keycloak.Mapping.User
{
    public class RoleMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsRole, Model.RoleModel>()
                .Map(dest => dest.Id, src => src.RoleId)
                .Map(dest => dest.Name, src => src.Name)
                .Inherits<Entity.IBaseEntity, Api.Models.BaseModel>();

            config.NewConfig<Model.RoleModel, Entity.PimsRole>()
                .Map(dest => dest.RoleId, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Inherits<Api.Models.BaseModel, Entity.IBaseEntity>();

            config.NewConfig<Entity.PimsUserRole, Model.RoleModel>()
                .Map(dest => dest.Id, src => src.UserId);

            config.NewConfig<Model.RoleModel, Entity.PimsUserRole>()
                .Map(dest => dest.UserId, src => src.Id);
        }
    }
}
