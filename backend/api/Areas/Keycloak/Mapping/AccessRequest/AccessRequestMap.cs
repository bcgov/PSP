using Mapster;
using System.Linq;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Keycloak.Models.AccessRequest;

namespace Pims.Api.Areas.Keycloak.Mapping.AccessRequest
{
    public class AccessRequestMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.AccessRequest, Model.AccessRequestModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.User, src => src.User)
                .Map(dest => dest.Status, src => src.StatusId)
                .Map(dest => dest.Role, src => src.Role)
                .Map(dest => dest.Note, src => src.Note)
                .AfterMapping((src, dest) =>
                {
                    if (src.OrganizationsManyToMany.FirstOrDefault()?.Organization != null)
                        dest.Organization = new Model.OrganizationModel() { Id = src.OrganizationsManyToMany.FirstOrDefault().Organization.Id, Name = src.OrganizationsManyToMany.FirstOrDefault().Organization.Name };
                    else if (src.Organizations.Any())
                        dest.Organization = new Model.OrganizationModel() { Id = src.Organizations.FirstOrDefault().Id, Name = src.Organizations.FirstOrDefault().Name };
                })
                .Inherits<Entity.BaseAppEntity, Api.Models.BaseAppModel>();

            config.NewConfig<Model.AccessRequestModel, Entity.AccessRequest>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.StatusId, src => src.Status)
                .Map(dest => dest.UserId, src => src.User.Id)
                .Map(dest => dest.RoleId, src => src.Role.Id)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.OrganizationsManyToMany, src => src.Organization)
                .AfterMapping((src, dest) =>
                {
                    dest.OrganizationsManyToMany.Add(new Entity.AccessRequestOrganization() { OrganizationId = src.Organization.Id });
                })
                .Inherits<Api.Models.BaseAppModel, Entity.BaseAppEntity>();
        }
    }
}
