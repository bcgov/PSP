using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Keycloak.Models.AccessRequest;

namespace Pims.Api.Areas.Keycloak.Mapping.AccessRequest
{
    public class AccessRequestOrganizationMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.AccessRequestOrganization, Model.OrganizationModel>()
                .Map(dest => dest.Id, src => src.OrganizationId)
                .Map(dest => dest.Name, src => src.Organization == null ? null : src.Organization.Name)
                .Inherits<Entity.BaseAppEntity, Api.Models.BaseAppModel>();

            config.NewConfig<Model.OrganizationModel, Entity.AccessRequestOrganization>()
                .Map(dest => dest.OrganizationId, src => src.Id)
                .Inherits<Api.Models.BaseAppModel, Entity.BaseAppEntity>();
        }
    }
}
