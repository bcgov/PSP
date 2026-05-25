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
        Paged<PimsNotificationUserOutput> GetInboxPage(long userId, int page, int quantity);

        /// <summary>
        /// Returns the count of unread in-app notifications for the given user.
        /// </summary>
        int GetUnreadCount(long userId);

        /// <summary>
        /// Returns a single in-app notification delivery by id, scoped to the given user.
        /// Returns null when not found or the notification delivery does not belong to the user.
        /// </summary>
        PimsNotificationUserOutput GetNotificationOutput(long notificationOutputId, long userId);

        /// <summary>
        /// Updates the read status of a notification delivery.
        /// </summary>
        PimsNotificationUserOutput UpdateReadStatus(long notificationUserOutputId, long userId, bool isRead);

        /// <summary>
        /// Marks all unread in-app notifications as read for the given user.
        /// </summary>
        void MarkAllRead(long userId);

        /// <summary>
        /// Removes a notification delivery for a given user.
        /// </summary>
        bool Delete(long notificationOutputId, long userId);
    }
}
