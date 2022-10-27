using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Keycloak.Models.AccessRequest;

namespace Pims.Api.Areas.Keycloak.Mapping.AccessRequest
{
    public class AccessRequestMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsAccessRequest, Model.AccessRequestModel>()
                .Map(dest => dest.Id, src => src.AccessRequestId)
                .Map(dest => dest.User, src => src.User)
                .Map(dest => dest.Status, src => src.AccessRequestStatusTypeCode)
                .Map(dest => dest.Role, src => src.Role)
                .Map(dest => dest.Note, src => src.Note)
                .Inherits<Entity.IBaseAppEntity, Api.Models.BaseAppModel>();

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
