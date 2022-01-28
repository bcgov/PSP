using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

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
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public LeasePaymentRepository(PimsContext dbContext, ClaimsPrincipal user, IPimsRepository service, ILogger<LeasePaymentRepository> logger, IMapper mapper) : base(dbContext, user, service, logger, mapper) { }
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
