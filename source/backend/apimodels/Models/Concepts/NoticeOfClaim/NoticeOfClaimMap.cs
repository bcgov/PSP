using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.NoticeOfClaim
{
    public class NoticeOfClaimMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsNoticeOfClaim, NoticeOfClaimModel>()
                .Map(dest => dest.Id, src => src.NoticeOfClaimId)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Map(dest => dest.ManagementFileId, src => src.ManagementFileId)
                .Map(dest => dest.Comment, src => src.Comment)
                .Map(dest => dest.ReceivedDate, src => src.ReceivedDt)
                .Inherits<Entity.IBaseAppEntity, BaseConcurrentModel>();

            config.NewConfig<NoticeOfClaimModel, Entity.PimsNoticeOfClaim>()
                .Map(dest => dest.NoticeOfClaimId, src => src.Id)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Map(dest => dest.ManagementFileId, src => src.ManagementFileId)
                .Map(dest => dest.Comment, src => src.Comment)
                .Map(dest => dest.ReceivedDt, src => src.ReceivedDate)
                .Inherits<BaseConcurrentModel, Entity.IBaseAppEntity>();
        }
    }
}
