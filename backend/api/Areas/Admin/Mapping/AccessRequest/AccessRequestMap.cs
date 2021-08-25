using Mapster;
using System.Linq;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Admin.Models.AccessRequest;

namespace Pims.Api.Areas.Admin.Mapping.AccessRequest
{
    public class AccessRequestMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.AccessRequest, Model.AccessRequestModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Status, src => src.StatusId)
                .Map(dest => dest.User, src => src.User)
                .Map(dest => dest.Role, src => src.Role)
                .Map(dest => dest.Note, src => src.Note)
                .Inherits<Entity.BaseAppEntity, Api.Models.BaseAppModel>()
                .AfterMapping((src, dest) =>
                {
                    if (src.OrganizationsManyToMany.FirstOrDefault()?.Organization != null)
                        dest.Organization = new Model.OrganizationModel() { Id = src.OrganizationsManyToMany.FirstOrDefault().Organization.Id, Name = src.OrganizationsManyToMany.FirstOrDefault().Organization.Name };
                    else if (src.Organizations.Any())
                        dest.Organization = new Model.OrganizationModel() { Id = src.Organizations.FirstOrDefault().Id, Name = src.Organizations.FirstOrDefault().Name };
                });

            config.NewConfig<Model.AccessRequestModel, Entity.AccessRequest>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.StatusId, src => src.Status)
                .Map(dest => dest.UserId, src => src.User.Id)
                .Map(dest => dest.RoleId, src => src.Role.Id)
                .Map(dest => dest.Note, src => src.Note)
                .Inherits<Api.Models.BaseAppModel, Entity.BaseAppEntity>();
        }
    }
}
