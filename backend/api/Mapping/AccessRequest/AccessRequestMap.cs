using System.Linq;
using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Models.AccessRequest;

namespace Pims.Api.Mapping.AccessRequest
{
    public class AccessRequestMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.AccessRequest, Model.AccessRequestModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.User, src => src.User)
                .Map(dest => dest.RoleId, src => src.RoleId)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.Status, src => src.StatusId)
                .AfterMapping((src, dest) =>
                {
                    Entity.Organization organization = src.OrganizationsManyToMany.FirstOrDefault()?.Organization;
                    if (organization != null)
                    {
                        dest.OrganizationId = organization?.Id;
                    }
                    else if (src.Organizations.Any())
                    {
                        dest.OrganizationId = organization?.Id;
                    }
                })
                .Inherits<Entity.BaseAppEntity, Models.BaseAppModel>();

            config.NewConfig<Model.AccessRequestModel, Entity.AccessRequest>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.User, src => src.User)
                .Map(dest => dest.RoleId, src => src.RoleId)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.StatusId, src => src.Status)
                .AfterMapping((src, dest) =>
                {
                    if (src.OrganizationId != null)
                    {
                        dest.OrganizationsManyToMany.Add(new Entity.AccessRequestOrganization() { OrganizationId = (long)src.OrganizationId });
                    }
                })
                .Inherits<Models.BaseAppModel, Entity.BaseAppEntity>();
        }
    }
}
