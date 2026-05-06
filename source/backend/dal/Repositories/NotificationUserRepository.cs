using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.Models.Concepts.Notification;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;

namespace Pims.Dal.Repositories
{
    public class NotificationUserRepository : BaseRepository<PimsNotificationUserOutput>, INotificationUserRepository
    {
        public NotificationUserRepository(
            PimsContext dbContext,
            ClaimsPrincipal user,
            ILogger<DocumentRepository> logger)
            : base(dbContext, user, logger)
        {
        }

        public PimsNotificationUserOutput GetById(long notificationUserOutputId)
        {
            using var scope = Logger.QueryScope();

            return Context.PimsNotificationUserOutputs
                .AsNoTracking()
                .Include(x => x.NotificationUser)
                    .ThenInclude(y => y.Notification)
                        .ThenInclude(z => z.AcquisitionFile)
                .Include(x => x.NotificationUser)
                    .ThenInclude(y => y.Notification)
                        .ThenInclude(z => z.DispositionFile)
                .Include(x => x.NotificationUser)
                    .ThenInclude(y => y.Notification)
                        .ThenInclude(z => z.ResearchFile)
                .Include(x => x.NotificationUser)
                    .ThenInclude(y => y.Notification)
                        .ThenInclude(z => z.ManagementFile)
                .Include(x => x.NotificationUser)
                    .ThenInclude(y => y.Notification)
                        .ThenInclude(z => z.Lease)
                .Include(x => x.NotificationUser)
                    .ThenInclude(y => y.Notification)
                        .ThenInclude(z => z.Take)
                            .ThenInclude(z1 => z1.PropertyAcquisitionFile)
                .Include(x => x.NotificationUser)
                    .ThenInclude(y => y.Notification)
                        .ThenInclude(z => z.Insurance)
                .Include(x => x.NotificationUser)
                    .ThenInclude(y => y.Notification)
                        .ThenInclude(z => z.LeaseConsultation)
                .Include(x => x.NotificationUser)
                    .ThenInclude(y => y.Notification)
                        .ThenInclude(z => z.NoticeOfClaim)
                .Include(x => x.NotificationUser)
                    .ThenInclude(y => y.Notification)
                        .ThenInclude(z => z.LeaseRenewal)
                .Include(x => x.NotificationUser)
                    .ThenInclude(y => y.Notification)
                        .ThenInclude(z => z.ExpropOwnerHistory)
                .Include(x => x.NotificationUser)
                    .ThenInclude(y => y.Notification)
                        .ThenInclude(z => z.Agreement)
                .Include(x => x.NotificationUser)
                    .ThenInclude(y => y.Notification)
                        .ThenInclude(z => z.NotificationTypeCodeNavigation)
                 .Include(x => x.NotificationUser)
                    .ThenInclude(y => y.User)
                        .ThenInclude(z => z.Person)
                            .ThenInclude(z1 => z1.PimsContactMethods)
                .FirstOrDefault(x => x.NotificationUserOutputId == notificationUserOutputId) ?? throw new KeyNotFoundException();
        }

        public PimsNotificationUserOutput Update(PimsNotificationUserOutput userNotification)
        {
            try
            {
                using var scope = Logger.QueryScope();
                userNotification.ThrowIfNull(nameof(userNotification));

                var existingUserNotification = Context.PimsNotificationUserOutputs
                    .FirstOrDefault(x => x.NotificationUserOutputId == userNotification.NotificationUserOutputId);

                Context.Entry(existingUserNotification).CurrentValues.SetValues(userNotification);

                return existingUserNotification;
            }
            catch(Exception ex)
            {
                Logger.LogError(ex.Message);
                return null;
            }
        }

        public IEnumerable<PimsNotificationUserOutput> GetAllByFilter(NotificationUserSearchFilterModel filter)
        {
            using var scope = Logger.QueryScope();

            var predicateBuilder = PredicateBuilder.New<PimsNotificationUserOutput>(p => true);
            if (!string.IsNullOrEmpty(filter.NotificationOutputTypeCode))
            {
                predicateBuilder.And(x => x.NotificationOutputTypeCode == filter.NotificationOutputTypeCode);
            }

            if(filter.MaxRetries.HasValue)
            {
                predicateBuilder.And(x => x.NotificationRetryCnt < filter.MaxRetries || x.NotificationRetryCnt == null);
            }

            if(filter.NotificationSentDateTime is null)
            {
                predicateBuilder.And(x => x.NotificationSentDt == null);
            }
            else
            {
                predicateBuilder.And(x => x.NotificationSentDt.Value.Date == filter.NotificationSentDateTime.Value.Date);
            }

            if(filter.NotificationTriggerDate is not null)
            {
                predicateBuilder.And(x => x.NotificationUser.Notification.NotificationTriggerDate == filter.NotificationTriggerDate);
            }

            var query = Context.PimsNotificationUserOutputs
                .Include(x => x.NotificationUser)
                    .ThenInclude(y => y.Notification)
                .Include(x => x.NotificationOutputTypeCodeNavigation)
                .Where(predicateBuilder);

            return query.ToList();
        }
    }
}
