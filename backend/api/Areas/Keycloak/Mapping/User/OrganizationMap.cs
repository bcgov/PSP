using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Keycloak.Models.User;

namespace Pims.Api.Areas.Keycloak.Mapping.User
{
    public class OrganizationMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.Organization, Model.OrganizationModel>()
                .IgnoreNonMapped(true)
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.ParentId, src => src.ParentId)
                .Inherits<Entity.BaseAppEntity, Api.Models.BaseAppModel>();

            config.NewConfig<Model.OrganizationModel, Entity.Organization>()
                .IgnoreNonMapped(true)
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.ParentId, src => src.ParentId)
                .Inherits<Api.Models.BaseAppModel, Entity.BaseAppEntity>();

            config.NewConfig<Entity.UserOrganization, Model.OrganizationModel>()
                .IgnoreNonMapped(true)
                .Map(dest => dest.Id, src => src.OrganizationId)
                .Map(dest => dest.ParentId, src => src.Organization.ParentId)
                .Inherits<Entity.BaseAppEntity, Api.Models.BaseAppModel>();

            config.NewConfig<Model.OrganizationModel, Entity.UserOrganization>()
                .IgnoreNonMapped(true)
                .Map(dest => dest.OrganizationId, src => src.Id)
                .Inherits<Api.Models.BaseAppModel, Entity.BaseAppEntity>();
        }
    }
}
