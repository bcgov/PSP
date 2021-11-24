using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Models.AccessRequest;

namespace Pims.Api.Mapping.AccessRequest
{
    public class AccessRequestOrganizationMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsAccessRequestOrganization, Model.AccessRequestOrganizationModel>()
                .Map(dest => dest.Id, src => src.OrganizationId)
                .Map(dest => dest.Name, src => src.Organization == null ? null : src.Organization.OrganizationName)
                .Inherits<Entity.IDisableBaseAppEntity, Models.BaseAppModel>();

            config.NewConfig<Model.AccessRequestOrganizationModel, Entity.PimsAccessRequestOrganization>()
                .Map(dest => dest.OrganizationId, src => src.Id)
                .Inherits<Models.BaseAppModel, Entity.IDisableBaseAppEntity>();


            config.NewConfig<Entity.PimsOrganization, Model.AccessRequestOrganizationModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.OrganizationName)
                .Inherits<Entity.IDisableBaseAppEntity, Models.BaseAppModel>();

            config.NewConfig<Model.AccessRequestOrganizationModel, Entity.PimsOrganization>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.OrganizationName, src => src.Name)
                .Inherits<Models.BaseAppModel, Entity.IDisableBaseAppEntity>();
        }
    }
}
