using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Api.Services
{
    public interface INotificationInboxService
    {
        /// <summary>
        /// Returns a paged list of sent in-app deliveries for the given user.
        /// Only deliveries where NotificationSentDt IS NOT NULL are included.
        /// </summary>
        Paged<PimsNotificationUserOutput> GetUserInbox(string username, int page, int quantity);

        /// <summary>
        /// Returns the count of unread in-app notifications for the given user.
        /// </summary>
        int GetUnreadCount(string username);

        /// <summary>
        /// Returns a single in-app user notification by id, scoped to the given user.
        /// Only includes rows where NotificationSentDt IS NOT NULL and NotificationOutputTypeCode == PIMS.
        /// </summary>
        PimsNotificationUserOutput GetNotificationOutputById(long outputId, string username);

        /// <summary>
        /// Updates the read status of a user notification.
        /// </summary>
        PimsNotificationUserOutput UpdateReadStatus(long outputId, string username, bool isRead);

        /// <summary>
        /// Marks all sent in-app deliveries as read for the given user.
        /// </summary>
        void MarkAllRead(string username);

        /// <summary>
        /// Deletes a single in-app delivery for the given user.
        /// Returns false if the delivery was not found or does not belong to the user.
        /// </summary>
        bool DeleteNotificationOutput(long outputId, string username);
    }
}
