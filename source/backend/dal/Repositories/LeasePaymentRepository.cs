using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

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

        public IEnumerable<PimsLeasePayment> GetAllByDateRange(DateTime startDate, DateTime endDate, UserContextModel userContext = null)
        {
            var query = Context.PimsLeasePayments.AsSplitQuery().AsNoTracking()
                .Include(p => p.LeasePaymentCategoryTypeCodeNavigation)
                .Include(p => p.LeasePaymentStatusTypeCodeNavigation)
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

            // Contractor access is limited by region and team membership.
            if (userContext is not null && userContext.IsContractor)
            {
                query = query.Where(p => p.LeasePeriod.Lease.RegionCode.HasValue && userContext.Regions.Contains(p.LeasePeriod.Lease.RegionCode.Value));
                query = query.Where(p => p.LeasePeriod.Lease.PimsLeaseLicenseTeams.Any(lt => lt.PersonId == userContext.PersonId) ||
                    (p.LeasePeriod.Lease.Project != null && p.LeasePeriod.Lease.Project.PimsProjectPeople.Any(pp => pp.PersonId == userContext.PersonId)));
            }

            return query;
        }
    }
}
