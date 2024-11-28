
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
        #region Constructors

        /// <summary>
        /// Creates a new instance of a ConsultationRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public ConsultationRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<ConsultationRepository> logger)
            : base(dbContext, user, logger)
        {
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

            return existingConsultation;
        }

        public bool TryDeleteConsultation(long consultationId)
        {
            using var scope = Logger.QueryScope();

            var deletedEntity = Context.PimsLeaseConsultations.Where(x => x.LeaseConsultationId == consultationId).FirstOrDefault();
            if (deletedEntity is not null)
            {
                Context.PimsLeaseConsultations.Remove(deletedEntity);

                return true;
            }

            return false;
        }

        #endregion
    }
}
