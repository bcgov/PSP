using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Admin.Models.User;

namespace Pims.Api.Areas.Admin.Mapping.User
{
    public class OrganizationMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsOrganization, Model.OrganizationModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.OrganizationName)
                .Map(dest => dest.ParentId, src => src.PrntOrganizationId)
                .Inherits<Entity.IDisableBaseAppEntity, Api.Models.BaseAppModel>();

            config.NewConfig<Model.OrganizationModel, Entity.PimsOrganization>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.PrntOrganizationId, src => src.ParentId)
                .Inherits<Api.Models.BaseAppModel, Entity.IDisableBaseAppEntity>();

            config.NewConfig<Entity.PimsUserOrganization, Model.OrganizationModel>()
                .Map(dest => dest.Id, src => src.OrganizationId)
                .Map(dest => dest.ParentId, src => src.Organization != null ? src.Organization.PrntOrganizationId : null)
                .Map(dest => dest.Name, src => src.Organization != null ? src.Organization.OrganizationName : null);

            config.NewConfig<Model.OrganizationModel, Entity.PimsUserOrganization>()
                .Map(dest => dest.OrganizationId, src => src.Id);
        }
    }
}
