using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Admin.Models.Organization;

namespace Pims.Api.Areas.Admin.Mapping.Organization
{
    public class OrganizationMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsOrganization, Model.OrganizationModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.ParentId, src => src.PrntOrganizationId)
                .Inherits<Entity.IDisableBaseAppEntity, Api.Models.BaseAppModel>();

            config.NewConfig<Model.OrganizationModel, Entity.PimsOrganization>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.PrntOrganizationId, src => src.ParentId)
                .Inherits<Api.Models.BaseAppModel, Entity.IDisableBaseAppEntity>();
        }
    }
}
