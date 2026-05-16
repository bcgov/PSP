using Pims.Api.Models.Base;

namespace Pims.Api.Models.Concepts.Notification
{
    public class NotificationUserModel : BaseConcurrentModel
    {
        public long NotificationUserId { get; set; }

        public long NotificationId { get; set; }

        public long UserId { get; set; }

        /// <summary>
        /// Navigation to the parent notification. Populated when the row is fetched with
        /// the PIMS_NOTIFICATION join loaded; null on writes.
        /// </summary>
        public NotificationModel Notification { get; set; }
    }
}