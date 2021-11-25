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
            config.NewConfig<Entity.PimsAccessRequest, Model.AccessRequestModel>()
                .Map(dest => dest.Id, src => src.AccessRequestId)
                .Map(dest => dest.Status, src => src.AccessRequestStatusTypeCode)
                .Map(dest => dest.User, src => src.User)
                .Map(dest => dest.Role, src => src.Role)
                .Map(dest => dest.Note, src => src.Note)
                .Inherits<Entity.IBaseAppEntity, Api.Models.BaseAppModel>()
                .AfterMapping((src, dest) =>
                {
                    if (src.PimsAccessRequestOrganizations.FirstOrDefault()?.Organization != null)
                        dest.Organization = new Model.OrganizationModel() { Id = src.PimsAccessRequestOrganizations.FirstOrDefault().Organization.Id, Name = src.PimsAccessRequestOrganizations.FirstOrDefault().Organization.OrganizationName };
                    else if (src.GetOrganizations().Any())
                        dest.Organization = new Model.OrganizationModel() { Id = src.GetOrganizations().FirstOrDefault().Id, Name = src.GetOrganizations().FirstOrDefault().OrganizationName };
                });

            config.NewConfig<Model.AccessRequestModel, Entity.PimsAccessRequest>()
                .Map(dest => dest.AccessRequestId, src => src.Id)
                .Map(dest => dest.AccessRequestStatusTypeCode, src => src.Status)
                .Map(dest => dest.UserId, src => src.User.Id)
                .Map(dest => dest.RoleId, src => src.Role.Id)
                .Map(dest => dest.Note, src => src.Note)
                .Inherits<Api.Models.BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
