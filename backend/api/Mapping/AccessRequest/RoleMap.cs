using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Models.AccessRequest;

namespace Pims.Api.Mapping.AccessRequest
{
    public class AccessRequestRoleMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsRole, Model.RoleModel>()
                .Map(dest => dest.Id, src => src.RoleId)
                .Map(dest => dest.Name, src => src.Name)
                .Inherits<Entity.IBaseEntity, Models.BaseModel>();

            config.NewConfig<Model.RoleModel, Entity.PimsRole>()
                .Map(dest => dest.RoleId, src => src.Id)
                .Inherits<Models.BaseModel, Entity.IBaseEntity>();
        }
    }
}
