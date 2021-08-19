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
                .Map(dest => dest.Role, src => src.Role)
                .AfterMapping((src, dest) =>
                {
                    if (src.OrganizationsManyToMany.Any())
                        dest.Organizations = src.OrganizationsManyToMany.Select(o => new Model.AccessRequestOrganizationModel() { Id = o.OrganizationId, Name = o.Organization?.Name });
                    else if (src.Organizations.Any())
                        dest.Organizations = src.Organizations.Select(o => new Model.AccessRequestOrganizationModel() { Id = o.Id, Name = o.Name });
                })
                .Inherits<Entity.BaseAppEntity, Models.BaseAppModel>();

            config.NewConfig<Model.AccessRequestModel, Entity.AccessRequest>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.User, src => src.User)
                .Map(dest => dest.Role, src => src.Role)
                .Map(dest => dest.Organizations, src => src.Organizations)
                .Inherits<Models.BaseAppModel, Entity.BaseAppEntity>();
        }
    }
}
