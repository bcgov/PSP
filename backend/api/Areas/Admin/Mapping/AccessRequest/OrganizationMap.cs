using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Admin.Models.AccessRequest;

namespace Pims.Api.Areas.Admin.Mapping.AccessRequest
{
    public class AccessRequestOrganizationMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsAccessRequestOrganization, Model.OrganizationModel>()
                .Map(dest => dest.Id, src => src.OrganizationId)
                .Map(dest => dest.Name, src => src.Organization.OrganizationName)
                .Inherits<Entity.IDisableBaseAppEntity, Api.Models.BaseAppModel>();

            config.NewConfig<Model.OrganizationModel, Entity.PimsAccessRequestOrganization>()
                .Map(dest => dest.OrganizationId, src => src.Id)
                .Inherits<Api.Models.BaseAppModel, Entity.IDisableBaseAppEntity>();
        }
    }
}
