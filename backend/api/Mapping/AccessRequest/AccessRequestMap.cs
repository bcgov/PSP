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
            config.NewConfig<Entity.PimsAccessRequest, Model.AccessRequestModel>()
                .Map(dest => dest.Id, src => src.AccessRequestId)
                .Map(dest => dest.User, src => src.User)
                .Map(dest => dest.RoleId, src => src.RoleId)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.Status, src => src.AccessRequestStatusTypeCode)
                .AfterMapping((src, dest) =>
                {
                    Entity.PimsOrganization organization = src.PimsAccessRequestOrganizations.FirstOrDefault()?.Organization;
                    if (organization != null)
                    {
                        dest.OrganizationId = organization?.Id;
                    }
                    else if (src.GetOrganizations().Any())
                    {
                        dest.OrganizationId = organization?.Id;
                    }
                })
                .Inherits<Entity.IBaseAppEntity, Models.BaseAppModel>();

            config.NewConfig<Model.AccessRequestModel, Entity.PimsAccessRequest>()
                .Map(dest => dest.AccessRequestId, src => src.Id)
                .Map(dest => dest.User, src => src.User)
                .Map(dest => dest.RoleId, src => src.RoleId)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.AccessRequestStatusTypeCode, src => src.Status)
                .AfterMapping((src, dest) =>
                {
                    if (src.OrganizationId != null)
                    {
                        dest.PimsAccessRequestOrganizations.Add(new Entity.PimsAccessRequestOrganization() { OrganizationId = (long)src.OrganizationId });
                    }
                })
                .Inherits<Models.BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
