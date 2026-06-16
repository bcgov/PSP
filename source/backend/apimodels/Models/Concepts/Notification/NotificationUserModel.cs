using Pims.Api.Models.Base;

namespace Pims.Api.Models.Concepts.Notification
{
    public class NotificationUserModel : BaseConcurrentModel
    {
        public long Id { get; set; }

        public long NotificationId { get; set; }

        public long UserId { get; set; }

        public NotificationModel Notification { get; set; }
    }
}
