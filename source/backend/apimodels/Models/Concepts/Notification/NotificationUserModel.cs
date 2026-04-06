using Pims.Api.Models.Base;

namespace Pims.Api.Models.Concepts.Notification
{
    public class NotificationUserModel : BaseConcurrentModel
    {
        public long NotificationUserId { get; set; }

        public long NotificationId { get; set; }

        public long UserId { get; set; }
    }
}