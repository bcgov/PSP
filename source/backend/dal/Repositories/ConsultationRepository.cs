using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Provides a repository to interact with consultations within the datasource.
    /// </summary>
    public class ConsultationRepository : BaseRepository<PimsLeaseConsultation>, IConsultationRepository
    {
        private readonly INotificationRepository _notificationRepository;

        #region Constructors

        /// <summary>
        /// Creates a new instance of a ConsultationRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public ConsultationRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<ConsultationRepository> logger, INotificationRepository notificationRepository)
            : base(dbContext, user, logger)
        {
            _notificationRepository = notificationRepository;
        }

        #endregion

        #region Methods

        public List<PimsLeaseConsultation> GetConsultationsByLease(long leaseId)
        {
            using var scope = Logger.QueryScope();

            return Context.PimsLeaseConsultations
                .Where(lc => lc.LeaseId == leaseId)
                .Include(lc => lc.ConsultationTypeCodeNavigation)
                .Include(lc => lc.ConsultationOutcomeTypeCodeNavigation)
                .AsNoTracking()
                .ToList();
        }

        public PimsLeaseConsultation AddConsultation(PimsLeaseConsultation consultation)
        {
            using var scope = Logger.QueryScope();

            Context.PimsLeaseConsultations.Add(consultation);

            return consultation;
        }

        public PimsLeaseConsultation GetConsultationById(long consultationId)
        {
            using var scope = Logger.QueryScope();

            return Context.PimsLeaseConsultations.Where(x => x.LeaseConsultationId == consultationId)
                .AsNoTracking()
                .Include(x => x.ConsultationTypeCodeNavigation)
                .Include(x => x.ConsultationOutcomeTypeCodeNavigation)
                .FirstOrDefault() ?? throw new KeyNotFoundException();
        }

        public PimsLeaseConsultation UpdateConsultation(PimsLeaseConsultation consultation)
        {
            using var scope = Logger.QueryScope();

            var existingConsultation = Context.PimsLeaseConsultations.FirstOrDefault(x => x.LeaseConsultationId == consultation.LeaseConsultationId) ?? throw new KeyNotFoundException();

            Context.Entry(existingConsultation).CurrentValues.SetValues(consultation);

            if(!consultation.RequestedOn.HasValue)
            {
                var existingNotification = Context.PimsNotifications.AsNoTracking()
                    .Where(n => n.LeaseId == consultation.LeaseId && n.LeaseConsultationId == consultation.LeaseConsultationId)
                    .FirstOrDefault();

                if (existingNotification is not null)
                {
                    _notificationRepository.Delete(existingNotification.NotificationId);
                }
            }

            return existingConsultation;
        }

        public bool TryDeleteConsultation(long consultationId)
        {
            using var scope = Logger.QueryScope();

            var deletedEntity = Context.PimsLeaseConsultations.Where(x => x.LeaseConsultationId == consultationId).FirstOrDefault();
            if (deletedEntity is not null)
            {
                var existingNotification = Context.PimsNotifications.AsNoTracking()
                    .Where(n => n.LeaseId == deletedEntity.LeaseId && n.LeaseConsultationId == deletedEntity.LeaseConsultationId)
                    .FirstOrDefault();

                if (existingNotification is not null)
                {
                    _notificationRepository.Delete(existingNotification.NotificationId);
                }

                Context.PimsLeaseConsultations.Remove(deletedEntity);

                return true;
            }

            return false;
        }

        #endregion
    }
}
