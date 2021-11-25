using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Admin.Models.AccessRequest;

namespace Pims.Api.Areas.Admin.Mapping.AccessRequest
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
                .Inherits<Api.Models.BaseModel, Entity.IBaseEntity>();
        }
    }
}
