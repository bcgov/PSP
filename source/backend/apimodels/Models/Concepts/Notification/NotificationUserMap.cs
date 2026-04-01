using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Areas.Notification.Models
{
    public class NotificationUserMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsNotificationUser, NotificationUserModel>()
                .Map(dest => dest.NotificationUserId, src => src.NotificationUserId)
                .Map(dest => dest.NotificationId, src => src.NotificationId)
                .Map(dest => dest.UserId, src => src.UserId)
                .Inherits<Entity.IBaseAppEntity, BaseConcurrentModel>();

            config.NewConfig<NotificationUserModel, Entity.PimsNotificationUser>()
                .Map(dest => dest.NotificationUserId, src => src.NotificationUserId)
                .Map(dest => dest.NotificationId, src => src.NotificationId)
                .Map(dest => dest.UserId, src => src.UserId)
                .Inherits<BaseConcurrentModel, Entity.IBaseAppEntity>();
        }
    }
}