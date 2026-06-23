using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Notification
{
    public class NotificationInboxItemMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsNotificationUserOutput, NotificationInboxItemModel>()
                .Map(dest => dest.Id, src => src.NotificationUserOutputId)
                .Map(dest => dest.NotificationUserId, src => src.NotificationUserId)
                .Map(dest => dest.NotificationSentDt, src => src.NotificationSentDt)
                .Map(dest => dest.IsRead, src => src.NotificationReadDt != null)
                .Map(dest => dest.NotificationType, src => NotificationInboxItemResolver.GetNotificationType(src))
                .Map(dest => dest.TrackedDate, src => NotificationInboxItemResolver.GetTrackedDate(src))
                .Map(dest => dest.Subject, src => NotificationInboxItemResolver.GetSubject(src))
                .Map(dest => dest.NotificationUser, src => src.NotificationUser)
                .Inherits<Entity.IBaseAppEntity, BaseConcurrentModel>();
        }
    }
}
