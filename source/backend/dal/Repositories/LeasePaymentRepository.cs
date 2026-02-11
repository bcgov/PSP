using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public class LeasePaymentRepository : BaseRepository<PimsLeasePayment>, ILeasePaymentRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a LeaseService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public LeasePaymentRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<LeasePaymentRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        public void Delete(long leasePaymentId)
        {
            PimsLeasePayment payment = Context.PimsLeasePayments.Find(leasePaymentId) ?? throw new KeyNotFoundException();
            Context.Remove(payment);
        }

        public PimsLeasePayment Update(PimsLeasePayment pimsLeasePayment)
        {
            var updatedEntity = Context.Update(pimsLeasePayment);
            return updatedEntity.Entity;
        }

        public PimsLeasePayment Add(PimsLeasePayment pimsLeasePayment)
        {
            var updatedEntity = Context.Add(pimsLeasePayment);
            return updatedEntity.Entity;
        }

        public IEnumerable<PimsLeasePayment> GetAllByDateRange(DateTime startDate, DateTime endDate, long? contractorPersonId = null)
        {
            var query = Context.PimsLeasePayments.AsSplitQuery().AsNoTracking()
                .Include(p => p.LeasePaymentCategoryTypeCodeNavigation)
                .Include(p => p.LeasePeriod)
                    .ThenInclude(t => t.LeasePeriodStatusTypeCodeNavigation)
                .Include(p => p.LeasePeriod)
                    .ThenInclude(t => t.LeasePmtFreqTypeCodeNavigation)
                .Include(p => p.LeasePeriod)
                    .ThenInclude(t => t.Lease)
                    .ThenInclude(l => l.PimsLeaseLicenseTeams)
                .Include(p => p.LeasePeriod)
                    .ThenInclude(t => t.Lease)
                    .ThenInclude(l => l.Project)
                    .ThenInclude(lp => lp.PimsProjectPeople)
                .Where(p => p.PaymentReceivedDate <= endDate && p.PaymentReceivedDate >= startDate);

            // Enforce contractor access to only their leases
            if (contractorPersonId is not null)
            {
                query = query.Where(p => p.LeasePeriod.Lease.PimsLeaseLicenseTeams.Any(lt => lt.PersonId == contractorPersonId) ||
                    p.LeasePeriod.Lease.Project.PimsProjectPeople.Any(pp => pp.PersonId == contractorPersonId));
            }

            return query;
        }
    }
}
