using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Models.User;

namespace Pims.Api.Mapping.User
{
    public class OrganizationMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsOrganization, Model.OrganizationModel>()
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Map(dest => dest.Parent, src => src.PrntOrganization)
                .Map(dest => dest.Children, src => src.InversePrntOrganization)
                .Map(dest => dest.Users, src => src.GetUsers())
                .Inherits<Entity.IDisableBaseAppEntity, Models.BaseAppModel>();

            config.NewConfig<Model.OrganizationModel, Entity.PimsOrganization>()
                .Map(dest => dest.Internal_Id, src => src.Id)
                .Map(dest => dest.PrntOrganizationId, src => src.Parent == null ? (long?)null : src.Parent.Id)
                .Map(dest => dest.PrntOrganization, src => src.Parent)
                .Map(dest => dest.InversePrntOrganization, src => src.Children)
                .Map(dest => dest.PimsUserOrganizations, src => src.Users)
                .Inherits<Models.BaseAppModel, Entity.IDisableBaseAppEntity>();

            config.NewConfig<Model.OrganizationModel, Entity.PimsUserOrganization>()
                .Map(dest => dest.OrganizationId, src => src.Id)
                .Map(dest => dest.Organization, src => src);
        }
    }
}
