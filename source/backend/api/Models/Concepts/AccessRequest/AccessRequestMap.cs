using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.AccessRequest
{
    public class AccessRequestMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsAccessRequest, AccessRequestModel>()
                .Map(dest => dest.Id, src => src.AccessRequestId)
                .Map(dest => dest.AccessRequestStatusTypeCode, src => src.AccessRequestStatusTypeCodeNavigation)
                .Map(dest => dest.RegionCode, src => src.RegionCodeNavigation)
                .Map(dest => dest.User, src => src.User)
                .Map(dest => dest.UserId, src => src.UserId)
                .Map(dest => dest.Role, src => src.Role)
                .Map(dest => dest.RoleId, src => src.RoleId)
                .Map(dest => dest.Note, src => src.Note)
                .Inherits<Entity.IBaseAppEntity, Api.Models.BaseAppModel>();

            config.NewConfig<AccessRequestModel, Entity.PimsAccessRequest>()
                .Map(dest => dest.AccessRequestId, src => src.Id)
                .Map(dest => dest.AccessRequestStatusTypeCode, src => src.AccessRequestStatusTypeCode.Id)
                .Map(dest => dest.RegionCode, src => src.RegionCode.Id)
                .Map(dest => dest.UserId, src => src.UserId)
                .Map(dest => dest.User, src => src.User)
                .Map(dest => dest.RoleId, src => src.RoleId)
                .Map(dest => dest.Note, src => src.Note)
                .Inherits<Api.Models.BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
