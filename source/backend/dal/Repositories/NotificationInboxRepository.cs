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
        public Paged<PimsNotificationUserOutput> GetUserInboxDeep(long userId, int page, int quantity)
        {
            using var scope = Logger.QueryScope();

            var baseQuery = BuildUserNotificationsQuery(userId, deep: true);
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
            return BuildUnreadNotificationsQuery(userId, deep: false).Count();
        }

        /// <inheritdoc />
        public PimsNotificationUserOutput GetNotificationOutputById(long outputId, long userId)
        {
            using var scope = Logger.QueryScope();

            var query = BuildUserNotificationsQuery(userId, deep: false);
            return query.FirstOrDefault(o => o.NotificationUserOutputId == outputId) ?? throw new KeyNotFoundException();
        }

        /// <inheritdoc />
        public PimsNotificationUserOutput GetNotificationOutputByIdDeep(long outputId, long userId)
        {
            using var scope = Logger.QueryScope();

            var query = BuildUserNotificationsQuery(userId, deep: true);
            return query.FirstOrDefault(o => o.NotificationUserOutputId == outputId) ?? throw new KeyNotFoundException();
        }

        /// <inheritdoc />
        public PimsNotificationUserOutput UpdateReadStatus(long outputId, long userId, bool isRead)
        {
            var existing = GetNotificationOutputById(outputId, userId);
            existing.NotificationReadDt = isRead ? DateTime.UtcNow : (DateTime?)null;

            // This attaches the entity and marks ONLY the main row as modified, leaving the rest of the graph untouched.
            Context.Entry(existing).State = EntityState.Modified;
            Context.SaveChanges();

            return existing;
        }

        /// <inheritdoc />
        public void MarkAllRead(long userId)
        {
            using var scope = Logger.QueryScope();

            var unread = BuildUnreadNotificationsQuery(userId, deep: false).ToList();

            foreach (var notificationOutput in unread)
            {
                notificationOutput.NotificationReadDt = DateTime.UtcNow;
                Context.Entry(notificationOutput).State = EntityState.Modified;
            }

            Context.SaveChanges();
        }

        /// <inheritdoc />
        public bool DeleteNotificationOutput(long outputId, long userId)
        {
            var existing = GetNotificationOutputById(outputId, userId);

            if (existing is null)
            {
                return false;
            }

            Context.PimsNotificationUserOutputs.Remove(
                new PimsNotificationUserOutput
                {
                    NotificationUserOutputId = outputId,
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
        private IQueryable<PimsNotificationUserOutput> BuildUserNotificationsQuery(long userId, bool deep = false)
        {
            IQueryable<PimsNotificationUserOutput> query = Context.PimsNotificationUserOutputs
                .AsNoTracking()
                .Include(o => o.NotificationUser)
                    .ThenInclude(nu => nu.Notification)
                    .ThenInclude(n => n.NotificationTypeCodeNavigation);

            if (deep)
            {
                query = query.AsSplitQuery()
                    .Include(o => o.NotificationUser)
                        .ThenInclude(nu => nu.Notification)
                            .ThenInclude(n => n.AcquisitionFile)
                    .Include(o => o.NotificationUser)
                        .ThenInclude(nu => nu.Notification)
                            .ThenInclude(n => n.DispositionFile)
                    .Include(o => o.NotificationUser)
                        .ThenInclude(nu => nu.Notification)
                            .ThenInclude(n => n.ResearchFile)
                    .Include(o => o.NotificationUser)
                        .ThenInclude(nu => nu.Notification)
                            .ThenInclude(n => n.ManagementFile)
                    .Include(o => o.NotificationUser)
                        .ThenInclude(nu => nu.Notification)
                            .ThenInclude(n => n.Lease)
                    .Include(o => o.NotificationUser)
                        .ThenInclude(nu => nu.Notification)
                            .ThenInclude(n => n.Take)
                    .Include(o => o.NotificationUser)
                        .ThenInclude(nu => nu.Notification)
                            .ThenInclude(n => n.Insurance)
                    .Include(o => o.NotificationUser)
                        .ThenInclude(nu => nu.Notification)
                            .ThenInclude(n => n.LeaseConsultation)
                    .Include(o => o.NotificationUser)
                        .ThenInclude(nu => nu.Notification)
                            .ThenInclude(n => n.NoticeOfClaim)
                    .Include(o => o.NotificationUser)
                        .ThenInclude(nu => nu.Notification)
                            .ThenInclude(n => n.LeaseRenewal)
                    .Include(o => o.NotificationUser)
                        .ThenInclude(nu => nu.Notification)
                            .ThenInclude(n => n.ExpropOwnerHistory)
                    .Include(o => o.NotificationUser)
                        .ThenInclude(nu => nu.Notification)
                            .ThenInclude(n => n.Agreement);
            }

            query = query.Where(o =>
                    o.NotificationUser.UserId == userId &&
                    o.NotificationOutputTypeCode == NotificationOutputTypeCode.PIMS.ToString() &&
                    o.NotificationSentDt != null);
            return query;
        }

        private IQueryable<PimsNotificationUserOutput> BuildUnreadNotificationsQuery(long userId, bool deep = false)
        {
            return BuildUserNotificationsQuery(userId, deep)
                .Where(o => o.NotificationReadDt == null);
        }
    }
}
