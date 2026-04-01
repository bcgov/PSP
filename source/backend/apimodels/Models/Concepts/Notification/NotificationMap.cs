using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Notification
{
    public class NotificationMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsNotification, NotificationModel>()
                .Map(dest => dest.NotificationId, src => src.NotificationId)
                .Map(dest => dest.NotificationTypeCode, src => src.NotificationTypeCode)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Map(dest => dest.DispositionFileId, src => src.DispositionFileId)
                .Map(dest => dest.ResearchFileId, src => src.ResearchFileId)
                .Map(dest => dest.ManagementFileId, src => src.ManagementFileId)
                .Map(dest => dest.LeaseId, src => src.LeaseId)
                .Map(dest => dest.TakeId, src => src.TakeId)
                .Map(dest => dest.InsuranceId, src => src.InsuranceId)
                .Map(dest => dest.LeaseConsultationId, src => src.LeaseConsultationId)
                .Map(dest => dest.NoticeOfClaimId, src => src.NoticeOfClaimId)
                .Map(dest => dest.LeaseRenewalId, src => src.LeaseRenewalId)
                .Map(dest => dest.ExpropOwnerHistoryId, src => src.ExpropOwnerHistoryId)
                .Map(dest => dest.AgreementId, src => src.AgreementId)
                .Map(dest => dest.NotificationTriggerDate, src => src.NotificationTriggerDate)
                .Map(dest => dest.NotificationMessage, src => src.NotificationMessage)
                .Inherits<Entity.IBaseAppEntity, BaseConcurrentModel>();

            config.NewConfig<NotificationModel, Entity.PimsNotification>()
                .Map(dest => dest.NotificationId, src => src.NotificationId)
                .Map(dest => dest.NotificationTypeCode, src => src.NotificationTypeCode)
                .Map(dest => dest.NotificationTriggerDate, src => src.NotificationTriggerDate)
                .Map(dest => dest.NotificationMessage, src => src.NotificationMessage)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Map(dest => dest.DispositionFileId, src => src.DispositionFileId)
                .Map(dest => dest.ResearchFileId, src => src.ResearchFileId)
                .Map(dest => dest.ManagementFileId, src => src.ManagementFileId)
                .Inherits<BaseConcurrentModel, Entity.IBaseAppEntity>();
        }
    }
}