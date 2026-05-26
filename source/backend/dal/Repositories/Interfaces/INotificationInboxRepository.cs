using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Repositories
{
    public interface INotificationInboxRepository : IRepository
    {
        /// <summary>
        /// Returns a paged list of sent in-app notification deliveries for the given user.
        /// Only rows where NotificationSentDt IS NOT NULL and NotificationOutputTypeCode == PIMS are included.
        /// </summary>
        Paged<PimsNotificationUserOutput> GetUserInbox(long userId, int page, int quantity);

        /// <summary>
        /// Returns the count of unread in-app notifications for the given user.
        /// </summary>
        int GetUnreadCount(long userId);

        /// <summary>
        /// Returns a single in-app user notification by id, scoped to the given user.
        /// Only includes rows where NotificationSentDt IS NOT NULL and NotificationOutputTypeCode == PIMS.
        /// </summary>
        PimsNotificationUserOutput GetDeliveredUserNotification(long outputId, long userId);

        /// <summary>
        /// Updates the read status of a user notification.
        /// </summary>
        PimsNotificationUserOutput UpdateReadStatus(long outputId, long userId, bool isRead);

        /// <summary>
        /// Marks all unread in-app notifications as read for the given user.
        /// </summary>
        void MarkAllRead(long userId);

        /// <summary>
        /// Removes a user notification. It will no longer appear in the user's inbox.
        /// Returns false if the notification delivery was not found or does not belong to the user.
        /// </summary>
        bool DeleteUserNotification(long outputId, long userId);
    }
}
