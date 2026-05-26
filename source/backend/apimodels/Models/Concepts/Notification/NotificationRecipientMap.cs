using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Notification
{
    public class NotificationRecipientMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsNotificationUser, NotificationRecipientModel>()
                .Map(dest => dest.Id, src => src.NotificationUserId)
                .Map(dest => dest.NotificationId, src => src.NotificationId)
                .Map(dest => dest.UserId, src => src.UserId)
                .Map(dest => dest.Notification, src => src.Notification)
                .Inherits<Entity.IBaseAppEntity, BaseConcurrentModel>();

            config.NewConfig<NotificationRecipientModel, Entity.PimsNotificationUser>()
                .Map(dest => dest.NotificationUserId, src => src.Id)
                .Map(dest => dest.NotificationId, src => src.NotificationId)
                .Map(dest => dest.UserId, src => src.UserId)
                .Inherits<BaseConcurrentModel, Entity.IBaseAppEntity>();
        }
    }
}
