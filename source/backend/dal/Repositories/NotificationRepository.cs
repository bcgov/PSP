using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Enums;
using Pims.Dal.Helpers.Extensions;

namespace Pims.Dal.Repositories
{
    public class NotificationRepository : BaseRepository<PimsNotification>, INotificationRepository
    {

        #region Constructors

        /// <summary>
        /// Creates a new instance of a NotificationRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        /// <param name="sequenceRepository"></param>
        /// <param name="mapper"></param>
        public NotificationRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<NotificationRepository> logger, ISequenceRepository sequenceRepository, IMapper mapper)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Retrieves all notifications for a given user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<PimsNotification> GetByUser(long userId)
        {
            using var scope = Logger.QueryScope();
            var response = Context.PimsNotifications
                .Where(n => n.PimsNotificationUsers.Any(u => u.UserId == userId))
                .ToList();
            return response;
        }

        /// <summary>
        /// Retrieves a notification by its id.
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        public PimsNotification GetById(long notificationId)
        {
            using var scope = Logger.QueryScope();
            var response = Context.PimsNotifications
                .Include(n => n.PimsNotificationUsers)
                    .ThenInclude(nu => nu.User)
                .FirstOrDefault(n => n.NotificationId == notificationId);

            return response;
        }

        /// <summary>
        /// Adds a notification for a given user.
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public PimsNotification Add(PimsNotification notification, long userId)
        {
            using var scope = Logger.QueryScope();
            notification.ThrowIfNull(nameof(notification));

            Logger.LogInformation("Adding notification to context: {@Notification}", notification);
            Context.PimsNotifications.Add(notification);
            Context.SaveChanges();

            // Add the notification user
            var notificationUser = new PimsNotificationUser
            {
                NotificationId = notification.NotificationId,
                UserId = userId,
            };
            Logger.LogInformation("Adding notification user to context: {@NotificationUser}", notificationUser);
            Context.PimsNotificationUsers.Add(notificationUser);

            // Add two notification user outputs (EMAIL and PIMS)
            var outputEmail = new PimsNotificationUserOutput
            {
                NotificationUserId = notificationUser.NotificationUserId, // will be set after SaveChanges
                NotificationOutputTypeCode = NotificationOutputTypeCode.EMAIL.ToString(),
                NotificationSentDt = null,
                NotificationReadDt = null,
            };
            var outputPims = new PimsNotificationUserOutput
            {
                NotificationUserId = notificationUser.NotificationUserId, // will be set after SaveChanges
                NotificationOutputTypeCode = NotificationOutputTypeCode.PIMS.ToString(),
                NotificationSentDt = null,
                NotificationReadDt = null,
            };
            Context.SaveChanges();

            // Now add outputs with correct NotificationUserId
            outputEmail.NotificationUserId = notificationUser.NotificationUserId;
            outputPims.NotificationUserId = notificationUser.NotificationUserId;
            Context.PimsNotificationUserOutputs.Add(outputEmail);
            Context.PimsNotificationUserOutputs.Add(outputPims);
            Context.SaveChanges();

            return notification;
        }

        /// <summary>
        /// Updates a notification for a given user.
        /// </summary>
        /// <param name="notification">The notification to update.</param>
        /// <param name="userId">The ID of the user associated with the notification.</param>
        /// <returns>The updated notification.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the notification with the specified ID is not found.</exception>
        public PimsNotification Update(PimsNotification notification, long userId)
        {
            notification.ThrowIfNull(nameof(notification));

            var existing = this.Context.PimsNotifications.FirstOrDefault(n => n.NotificationId == notification.NotificationId) ?? throw new KeyNotFoundException($"Notification {notification.NotificationId} not found");
            Context.Entry(existing).CurrentValues.SetValues(notification);
            Context.SaveChanges();
            return existing;
        }

        /// <summary>
        /// Deletes a notification for a given user.
        /// </summary>
        /// <param name="notificationId">The ID of the notification to delete.</param>
        /// <param name="userId">The ID of the user associated with the notification.</param>
        /// <returns>True if the notification was deleted successfully, otherwise false.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the notification with the specified ID is not found.</exception>
        public bool Delete(long notificationId, long userId)
        {
            var notification = Context.PimsNotifications.FirstOrDefault(n => n.NotificationId == notificationId) ?? throw new KeyNotFoundException($"Notification {notificationId} not found");

            var notificationUserIds = Context.PimsNotificationUsers
                .Where(nu => nu.NotificationId == notificationId && nu.UserId == userId)
                .Select(nu => nu.NotificationUserId)
                .ToList();

            if (notificationUserIds.Count == 0)
            {
                return false;
            }

            // Remove related notification-user-output for userId
            var notificationUserOutputs = Context.PimsNotificationUserOutputs
                .Where(nuo => notificationUserIds.Contains(nuo.NotificationUserId));
            Context.PimsNotificationUserOutputs.RemoveRange(notificationUserOutputs);

            // Remove notification-user for userId
            var notificationUsers = Context.PimsNotificationUsers
                .Where(nu => notificationUserIds.Contains(nu.NotificationUserId));
            Context.PimsNotificationUsers.RemoveRange(notificationUsers);

            Context.SaveChanges();

            // Check if any other user uses this notification
            bool hasOtherLinks = Context.PimsNotificationUsers.Any(nu => nu.NotificationId == notificationId);

            // Only remove the notification if no other user links remain
            if (!hasOtherLinks)
            {
                Context.PimsNotifications.Remove(notification);
                Context.SaveChanges();
            }

            return true;
        }

        /// <summary>
        /// Retrieves the concurrency control number (version) of the notification with the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The notification row version.</returns>
        public long GetRowVersion(long id)
        {
            using var scope = Logger.QueryScope();

            var notification = Context.PimsNotifications.AsNoTracking()
                .FirstOrDefault(n => n.NotificationId == id) ?? throw new KeyNotFoundException();
            return notification.ConcurrencyControlNumber;
        }
        #endregion
    }
}