using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Models.User;

namespace Pims.Api.Mapping.User
{
    public class AccessRequestRoleMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.AccessRequestRole, Model.AccessRequestRoleModel>()
                .Map(dest => dest.Id, src => src.RoleId)
                .Map(dest => dest.Name, src => src.Role != null ? src.Role.Name : null)
                .Inherits<Entity.BaseAppEntity, Models.BaseAppModel>();

            config.NewConfig<Model.AccessRequestRoleModel, Entity.AccessRequestRole>()
                .Map(dest => dest.RoleId, src => src.Id)
                .Inherits<Models.BaseAppModel, Entity.BaseAppEntity>();
        }
    }
}
