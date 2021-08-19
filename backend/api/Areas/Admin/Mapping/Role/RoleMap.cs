using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Admin.Models.Role;

namespace Pims.Api.Areas.Admin.Mapping.Role
{
    public class RoleMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.Role, Model.RoleModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Key, src => src.Key)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.KeycloakGroupId, src => src.KeycloakGroupId)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsPublic, src => src.IsPublic)
                .Map(dest => dest.Claims, src => src.ClaimsManyToMany)
                .Inherits<Entity.BaseEntity, Api.Models.BaseModel>();

            config.NewConfig<Model.RoleModel, Entity.Role>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Key, src => src.Key)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.KeycloakGroupId, src => src.KeycloakGroupId)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsPublic, src => src.IsPublic)
                .Map(dest => dest.ClaimsManyToMany, src => src.Claims)
                .Inherits<Api.Models.BaseModel, Entity.BaseEntity>();
        }
    }
}
