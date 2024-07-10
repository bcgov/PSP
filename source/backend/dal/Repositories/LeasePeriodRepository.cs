using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public class LeasePeriodRepository : BaseRepository<PimsLeasePeriod>, ILeasePeriodRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a LeaseService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public LeasePeriodRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<LeaseRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        public void Delete(long leasePeriodId)
        {
            PimsLeasePeriod period = this.Context.PimsLeasePeriods.Find(leasePeriodId) ?? throw new KeyNotFoundException();
            this.Context.Remove(period);
        }

        public PimsLeasePeriod Update(PimsLeasePeriod pimsLeasePeriod)
        {
            this.Context.Entry(pimsLeasePeriod).Collection(t => t.PimsLeasePayments).IsModified = false;
            var updatedEntity = this.Context.Update(pimsLeasePeriod);
            return updatedEntity.Entity;
        }

        public PimsLeasePeriod Add(PimsLeasePeriod pimsLeasePeriod)
        {
            var updatedEntity = this.Context.Add(pimsLeasePeriod);
            return updatedEntity.Entity;
        }

        public IEnumerable<PimsLeasePeriod> GetAllByLeaseId(long leaseId)
        {
            var periods = this.Context.PimsLeasePeriods.AsNoTracking()
                .Include(t => t.LeasePmtFreqTypeCodeNavigation)
                .Include(t => t.VblRentFreqNavigation)
                .Include(t => t.AddlRentFreqNavigation)
                .Include(t => t.LeasePeriodStatusTypeCodeNavigation)
                .Include(t => t.PimsLeasePayments)
                    .ThenInclude(p => p.LeasePaymentMethodTypeCodeNavigation)
                .Include(t => t.PimsLeasePayments)
                    .ThenInclude(p => p.LeasePaymentStatusTypeCodeNavigation)
                .Include(t => t.PimsLeasePayments)
                    .ThenInclude(p => p.LeasePaymentCategoryTypeCodeNavigation)
                .Where(t => t.LeaseId == leaseId).ToArray();

            periods = periods.OrderBy(t => t.PeriodStartDate).ThenBy(t => t.LeasePeriodId).Select(t =>
            {
                t.PimsLeasePayments = t.PimsLeasePayments.OrderBy(p => p.PaymentReceivedDate).ThenBy(p => p.LeasePaymentId).ToArray();
                return t;
            }).ToArray();

            return periods;
        }

        public PimsLeasePeriod GetById(long leasePeriodId, bool loadPayments = false)
        {
            var query = this.Context.PimsLeasePeriods.AsNoTracking().Where(t => t.LeasePeriodId == leasePeriodId);
            if (loadPayments)
            {
                query.Include(t => t.PimsLeasePayments);
            }
            return query.FirstOrDefault() ?? throw new KeyNotFoundException();
        }
    }
}
