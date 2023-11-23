using Mapster;
using Pims.Api.Concepts.Models.Base;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Keycloak.Models.User;

namespace Pims.Api.Areas.Keycloak.Mapping.User
{
    public class OrganizationMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsOrganization, Model.OrganizationModel>()
                .IgnoreNonMapped(true)
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Map(dest => dest.Name, src => src.OrganizationName)
                .Map(dest => dest.ParentId, src => src.PrntOrganizationId)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<Model.OrganizationModel, Entity.PimsOrganization>()
                .IgnoreNonMapped(true)
                .Map(dest => dest.Internal_Id, src => src.Id)
                .Map(dest => dest.OrganizationName, src => src.Name)
                .Map(dest => dest.PrntOrganizationId, src => src.ParentId)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();

            config.NewConfig<Entity.PimsUserOrganization, Model.OrganizationModel>()
                .IgnoreNonMapped(true)
                .Map(dest => dest.Id, src => src.OrganizationId)
                .Map(dest => dest.ParentId, src => src.Organization.PrntOrganizationId)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<Model.OrganizationModel, Entity.PimsUserOrganization>()
                .IgnoreNonMapped(true)
                .Map(dest => dest.OrganizationId, src => src.Id)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();
        }
    }
}
