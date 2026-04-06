using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Notification
{
    public class NotificationUserOutputMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsNotificationUserOutput, NotificationUserOutputModel>()
                .Map(dest => dest.NotificationUserOutputId, src => src.NotificationUserOutputId)
                .Map(dest => dest.NotificationUserId, src => src.NotificationUserId)
                .Map(dest => dest.NotificationOutputTypeCode, src => src.NotificationOutputTypeCode)
                .Map(dest => dest.NotificationSentDt, src => src.NotificationSentDt)
                .Map(dest => dest.NotificationReadDt, src => src.NotificationReadDt)
                .Map(dest => dest.NotificationRetryCnt, src => src.NotificationRetryCnt)
                .Map(dest => dest.NotificationErrorReason, src => src.NotificationErrorReason)
                .Map(dest => dest.NotificationErrorDt, src => src.NotificationErrorDt)
                .Inherits<Entity.IBaseAppEntity, BaseConcurrentModel>();

            config.NewConfig<NotificationUserOutputModel, Entity.PimsNotificationUserOutput>()
                .Map(dest => dest.NotificationUserOutputId, src => src.NotificationUserOutputId)
                .Map(dest => dest.NotificationUserId, src => src.NotificationUserId)
                .Map(dest => dest.NotificationOutputTypeCode, src => src.NotificationOutputTypeCode)
                .Map(dest => dest.NotificationSentDt, src => src.NotificationSentDt)
                .Map(dest => dest.NotificationReadDt, src => src.NotificationReadDt)
                .Map(dest => dest.NotificationRetryCnt, src => src.NotificationRetryCnt)
                .Map(dest => dest.NotificationErrorReason, src => src.NotificationErrorReason)
                .Map(dest => dest.NotificationErrorDt, src => src.NotificationErrorDt)
                .Inherits<BaseConcurrentModel, Entity.IBaseAppEntity>();
        }
    }
}