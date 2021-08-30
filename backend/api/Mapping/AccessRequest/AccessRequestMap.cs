using System.Collections.Generic;
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
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.Status, src => src.StatusId)
                .AfterMapping((src, dest) =>
                {
                    if (src.OrganizationsManyToMany.FirstOrDefault()?.Organization != null)
                        dest.Organization = new Model.AccessRequestOrganizationModel() { Id = src.OrganizationsManyToMany.FirstOrDefault().Organization.Id, Name = src.OrganizationsManyToMany.FirstOrDefault().Organization.Name };
                    else if (src.Organizations.Any())
                        dest.Organization = new Model.AccessRequestOrganizationModel() { Id = src.Organizations.FirstOrDefault().Id, Name = src.Organizations.FirstOrDefault().Name };
                })
                .Inherits<Entity.BaseAppEntity, Models.BaseAppModel>();

            config.NewConfig<Model.AccessRequestModel, Entity.AccessRequest>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.User, src => src.User)
                .Map(dest => dest.RoleId, src => src.Role.Id)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.StatusId, src => src.Status)
                .AfterMapping((src, dest) =>
                {
                    dest.OrganizationsManyToMany.Add(new Entity.AccessRequestOrganization() { OrganizationId = src.Organization.Id });
                })
                .Inherits<Models.BaseAppModel, Entity.BaseAppEntity>();
        }
    }
}
