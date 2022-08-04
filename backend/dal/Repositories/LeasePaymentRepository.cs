using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MapsterMapper;
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
        public LeasePaymentRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<LeasePaymentRepository> logger) : base(dbContext, user, logger) { }
        #endregion

        public void Delete(long leasePaymentId)
        {
            PimsLeasePayment payment = this.Context.PimsLeasePayments.Find(leasePaymentId) ?? throw new KeyNotFoundException();
            this.Context.Remove(payment);
        }

        public PimsLeasePayment Update(PimsLeasePayment pimsLeasePayment)
        {
            this.Context.Update(pimsLeasePayment);
            return pimsLeasePayment;
        }

        public PimsLeasePayment Add(PimsLeasePayment pimsLeasePayment)
        {
            this.Context.Add(pimsLeasePayment);
            return pimsLeasePayment;
        }

        public PimsLeasePayment GetById(long leasePaymentId)
        {
            var query = this.Context.PimsLeasePayments.AsNoTracking().Where(p => p.LeasePaymentId == leasePaymentId);
            return query.FirstOrDefault() ?? throw new KeyNotFoundException();
        }
    }
}
