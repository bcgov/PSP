using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        public NotificationRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<NotificationRepository> logger)
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
            return Context.PimsNotifications
                .AsNoTracking()
                .Where(n => n.PimsNotificationUsers.Any(u => u.UserId == userId))
                .ToList();
        }

        /// <summary>
        /// Retrieves a notification by its id.
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        public PimsNotification GetById(long notificationId)
        {
            using var scope = Logger.QueryScope();
            return Context.PimsNotifications
                .AsNoTracking()
                .Include(n => n.PimsNotificationUsers)
                    .ThenInclude(nu => nu.User)
                .FirstOrDefault(n => n.NotificationId == notificationId) ?? throw new KeyNotFoundException();
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

            notification.PimsNotificationUsers = new List<PimsNotificationUser>
            {
                new PimsNotificationUser
                {
                    UserId = userId,
                    PimsNotificationUserOutputs = new List<PimsNotificationUserOutput>
                    {
                        new PimsNotificationUserOutput
                        {
                            NotificationOutputTypeCode = NotificationOutputTypeCode.EMAIL.ToString(),
                        },
                        new PimsNotificationUserOutput
                        {
                            NotificationOutputTypeCode = NotificationOutputTypeCode.PIMS.ToString(),
                        },
                    },
                },
            };

            Logger.LogInformation("Adding notification to context: {@Notification}", notification);
            Context.Add(notification);
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

            // Only update allowed fields
            existing.NotificationTriggerDate = notification.NotificationTriggerDate;
            existing.NotificationMessage = notification.NotificationMessage;

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
            var deletedEntity = Context.PimsNotifications
                .Include(n => n.PimsNotificationUsers)
                    .ThenInclude(nu => nu.PimsNotificationUserOutputs)
                .AsNoTracking()
                .FirstOrDefault(n => n.NotificationId == notificationId &&
                                     n.PimsNotificationUsers.Any(nu => nu.UserId == userId));

            if (deletedEntity == null)
            {
                return false;
            }

            foreach (var notificationUser in deletedEntity.PimsNotificationUsers.Where(nu => nu.UserId == userId))
            {
                foreach (var output in notificationUser.PimsNotificationUserOutputs)
                {
                    Context.PimsNotificationUserOutputs.Remove(
                        new PimsNotificationUserOutput
                        {
                            NotificationUserOutputId = output.NotificationUserOutputId,
                        });
                }
                Context.PimsNotificationUsers.Remove(
                    new PimsNotificationUser
                    {
                        NotificationUserId = notificationUser.NotificationUserId,
                    });
            }

            // Only remove the parent if no other users are linked
            bool hasOtherLinks = deletedEntity.PimsNotificationUsers.Any(nu => nu.UserId != userId);
            if (!hasOtherLinks)
            {
                Context.PimsNotifications.Remove(
                    new PimsNotification
                    {
                        NotificationId = notificationId,
                    });
            }

            Context.SaveChanges();
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