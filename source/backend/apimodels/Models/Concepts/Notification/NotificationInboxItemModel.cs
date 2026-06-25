using System;
using Pims.Api.Models.Base;

namespace Pims.Api.Models.Concepts.Notification
{
    /// <summary>
    /// A single item in the user-facing notification inbox popup.
    ///
    /// This is a read projection — not the full delivery (output) record. It carries only
    /// what the popup renders: the subject line, the notification type, the tracked date,
    /// and read state.
    ///
    /// Delivery diagnostics (retry count, error reason, error date) are  intentionally excluded,
    /// they belong to the email/in-app dispatch concern, not the user inbox.
    /// </summary>
    public class NotificationInboxItemModel : BaseConcurrentModel
    {
        public long Id { get; set; }

        public long NotificationUserId { get; set; }

        /// <summary>
        /// The subject line of the notification delivery.
        /// </summary>
        public string Subject { get; set; }

        public CodeTypeModel<string> NotificationType { get; set; }

        /// <summary>
        /// The domain date this notification is tracking.
        /// This is NOT the trigger date (when the item starts appearing) nor the sent date.
        /// It is the due/effective date of the underlying event, resolved by the repository
        /// via the notification's FK into the relevant table (e.g. lease renewal -> PIMS_LEASE_RENEWAL.EXPIRY_DT).
        /// </summary>
        public DateTime? TrackedDate { get; set; }

        /// <summary>
        /// Whether the user has read this delivery. It is derived from the presence of a NotificationReadDt in the delivery record.
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// When the delivery was sent. Always populated for inbox items (the inbox only surfaces sent deliveries).
        /// </summary>
        public DateTime? NotificationSentDt { get; set; }

        public NotificationUserModel NotificationUser { get; set; }
    }
}
