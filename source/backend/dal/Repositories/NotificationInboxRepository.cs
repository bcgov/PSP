using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Enums;
using Pims.Dal.Entities.Models;
using Pims.Dal.Helpers.Extensions;

namespace Pims.Dal.Repositories
{
    public class NotificationInboxRepository : BaseRepository<PimsNotificationUserOutput>, INotificationInboxRepository
    {
        /// <summary>
        /// Creates a new instance of a NotificationInboxRepository, and initializes it with the specified arguments.
        /// </summary>
        public NotificationInboxRepository(
            PimsContext dbContext,
            ClaimsPrincipal user,
            ILogger<NotificationInboxRepository> logger)
            : base(dbContext, user, logger)
        {
        }

        /// <inheritdoc />
        public Paged<PimsNotificationUserOutput> GetInboxPage(long userId, int page, int quantity)
        {
            using var scope = Logger.QueryScope();

            var baseQuery = BuildSentNotificationsQuery(userId);
            var total = baseQuery.Count();

            page = page < 1 ? 1 : page;
            quantity = quantity < 1 ? 10 : quantity;
            int skip = (page - 1) * quantity;

            var items = baseQuery
                .OrderByDescending(o => o.NotificationUser.Notification.NotificationTriggerDate)
                .ThenByDescending(o => o.NotificationUserOutputId)
                .Skip(skip)
                .Take(quantity)
                .ToList();

            return new Paged<PimsNotificationUserOutput>(items, page, quantity, total);
        }

        /// <inheritdoc />
        public int GetUnreadCount(long userId)
        {
            using var scope = Logger.QueryScope();
            return BuildUnreadNotificationsQuery(userId).Count();
        }

        /// <inheritdoc />
        public PimsNotificationUserOutput GetNotificationOutput(long notificationOutputId, long userId)
        {
            using var scope = Logger.QueryScope();

            var query = BuildSentNotificationsQuery(userId);
            return query.FirstOrDefault(o => o.NotificationUserOutputId == notificationOutputId) ?? throw new KeyNotFoundException();
        }

        /// <inheritdoc />
        public PimsNotificationUserOutput UpdateReadStatus(long notificationUserOutputId, long userId, bool isRead)
        {
            var existing = GetNotificationOutput(notificationUserOutputId, userId);
            existing.NotificationReadDt = isRead ? DateTime.UtcNow : (DateTime?)null;
            Context.SaveChanges();
            return existing;
        }

        /// <inheritdoc />
        public void MarkAllRead(long userId)
        {
            using var scope = Logger.QueryScope();

            var unread = BuildUnreadNotificationsQuery(userId).ToList();

            foreach (var notificationOutput in unread)
            {
                notificationOutput.NotificationReadDt = DateTime.UtcNow;
            }

            Context.SaveChanges();
        }

        /// <inheritdoc />
        public bool Delete(long notificationOutputId, long userId)
        {
            var existing = GetNotificationOutput(notificationOutputId, userId);

            if (existing is null)
            {
                return true;
            }

            Context.PimsNotificationUserOutputs.Remove(new PimsNotificationUserOutput
            {
                NotificationUserOutputId = notificationOutputId,
            });
            Context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Base query shared by all inbox reads:
        /// - scoped to the calling user
        /// - in-app channel only (PIMS output type)
        /// - sent only (NotificationSentDt IS NOT NULL).
        /// </summary>
        private IQueryable<PimsNotificationUserOutput> BuildSentNotificationsQuery(long userId)
        {
            return Context.PimsNotificationUserOutputs
                .AsNoTracking()
                .Include(o => o.NotificationUser)
                    .ThenInclude(nu => nu.Notification)
                .Where(o =>
                    o.NotificationUser.UserId == userId &&
                    o.NotificationOutputTypeCode == NotificationOutputTypeCode.PIMS.ToString() &&
                    o.NotificationSentDt != null);
        }

        private IQueryable<PimsNotificationUserOutput> BuildUnreadNotificationsQuery(long userId)
        {
            return BuildSentNotificationsQuery(userId)
                .Where(o => o.NotificationReadDt == null);
        }
    }
}
