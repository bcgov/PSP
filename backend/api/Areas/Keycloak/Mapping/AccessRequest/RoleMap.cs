using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Keycloak.Models.AccessRequest;

namespace Pims.Api.Areas.Keycloak.Mapping.AccessRequest
{
    public class AccessRequestRoleMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsRole, Model.RoleModel>()
                .Map(dest => dest.Id, src => src.RoleId)
                .Map(dest => dest.Name, src => src.Name)
                .Inherits<Entity.IBaseEntity, Api.Models.BaseModel>();
        }
    }
}
