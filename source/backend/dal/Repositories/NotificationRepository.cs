using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Enums;
using Pims.Dal.Entities.Models;
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

        public IEnumerable<PimsNotification> Search(NotificationSearchCriteria criteria, long userId)
        {
            using var scope = Logger.QueryScope();

            var predicate = PredicateBuilder.New<PimsNotification>(n => n.PimsNotificationUsers.Any(u => u.UserId == userId));

            if (!string.IsNullOrWhiteSpace(criteria.Type))
            {
                predicate = predicate.And(n => n.NotificationTypeCode == criteria.Type);
            }
            if (criteria.AcquisitionFileId.HasValue)
            {
                predicate = predicate.And(n => n.AcquisitionFileId == criteria.AcquisitionFileId.Value);
            }
            if (criteria.DispositionFileId.HasValue)
            {
                predicate = predicate.And(n => n.DispositionFileId == criteria.DispositionFileId.Value);
            }
            if (criteria.ResearchFileId.HasValue)
            {
                predicate = predicate.And(n => n.ResearchFileId == criteria.ResearchFileId.Value);
            }
            if (criteria.ManagementFileId.HasValue)
            {
                predicate = predicate.And(n => n.ManagementFileId == criteria.ManagementFileId.Value);
            }
            if (criteria.LeaseId.HasValue)
            {
                predicate = predicate.And(n => n.LeaseId == criteria.LeaseId.Value);
            }
            if (criteria.TakeId.HasValue)
            {
                predicate = predicate.And(n => n.TakeId == criteria.TakeId.Value);
            }
            if (criteria.InsuranceId.HasValue)
            {
                predicate = predicate.And(n => n.InsuranceId == criteria.InsuranceId.Value);
            }
            if (criteria.LeaseConsultationId.HasValue)
            {
                predicate = predicate.And(n => n.LeaseConsultationId == criteria.LeaseConsultationId.Value);
            }
            if (criteria.NoticeOfClaimId.HasValue)
            {
                predicate = predicate.And(n => n.NoticeOfClaimId == criteria.NoticeOfClaimId.Value);
            }
            if (criteria.LeaseRenewalId.HasValue)
            {
                predicate = predicate.And(n => n.LeaseRenewalId == criteria.LeaseRenewalId.Value);
            }
            if (criteria.ExpropOwnerHistoryId.HasValue)
            {
                predicate = predicate.And(n => n.ExpropOwnerHistoryId == criteria.ExpropOwnerHistoryId.Value);
            }
            if (criteria.AgreementId.HasValue)
            {
                predicate = predicate.And(n => n.AgreementId == criteria.AgreementId.Value);
            }

            return Context.PimsNotifications
                .AsNoTracking()
                .Where(predicate)
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
