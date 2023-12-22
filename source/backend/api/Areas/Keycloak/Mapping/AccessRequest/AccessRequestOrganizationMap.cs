using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Keycloak.Models.AccessRequest;

namespace Pims.Api.Areas.Keycloak.Mapping.AccessRequest
{
    public class AccessRequestOrganizationMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsAccessRequestOrganization, Model.OrganizationModel>()
                .Map(dest => dest.Id, src => src.OrganizationId)
                .Map(dest => dest.Name, src => src.Organization == null ? null : src.Organization.OrganizationName)
                .Inherits<Entity.IDisableBaseAppEntity<bool?>, BaseAuditModel>();

            config.NewConfig<Model.OrganizationModel, Entity.PimsAccessRequestOrganization>()
                .Map(dest => dest.OrganizationId, src => src.Id)
                .Inherits<BaseAuditModel, Entity.IDisableBaseAppEntity<bool?>>();
        }
    }
}
