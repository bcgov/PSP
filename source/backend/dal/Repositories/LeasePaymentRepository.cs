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
            PimsLeasePayment payment = this.Context.PimsLeasePayments.Find(leasePaymentId) ?? throw new KeyNotFoundException();
            this.Context.Remove(payment);
        }

        public PimsLeasePayment Update(PimsLeasePayment pimsLeasePayment)
        {
            var updatedEntity = this.Context.Update(pimsLeasePayment);
            return updatedEntity.Entity;
        }

        public PimsLeasePayment Add(PimsLeasePayment pimsLeasePayment)
        {
            var updatedEntity = this.Context.Add(pimsLeasePayment);
            return updatedEntity.Entity;
        }

        public IEnumerable<PimsLeasePayment> GetAllTracking(DateTime startDate, DateTime endDate)
        {
            return this.Context.PimsLeasePayments
                .AsSplitQuery()
                .Include(p => p.LeasePaymentCategoryTypeCodeNavigation)
                .Include(p => p.LeasePeriod)
                    .ThenInclude(t => t.LeasePeriodStatusTypeCodeNavigation)
                .Include(p => p.LeasePeriod)
                    .ThenInclude(t => t.LeasePmtFreqTypeCodeNavigation)
                .Where(p => p.PaymentReceivedDate <= endDate && p.PaymentReceivedDate >= startDate);
        }
    }
}
