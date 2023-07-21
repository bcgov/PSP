using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public class LeaseTermRepository : BaseRepository<PimsLeaseTerm>, ILeaseTermRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a LeaseService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public LeaseTermRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<LeaseRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        public void Delete(long leaseTermId)
        {
            PimsLeaseTerm term = this.Context.PimsLeaseTerms.Find(leaseTermId) ?? throw new KeyNotFoundException();
            this.Context.Remove(term);
        }

        public PimsLeaseTerm Update(PimsLeaseTerm pimsLeaseTerm)
        {
            this.Context.Entry(pimsLeaseTerm).Collection(t => t.PimsLeasePayments).IsModified = false;
            var updatedEntity = this.Context.Update(pimsLeaseTerm);
            return updatedEntity.Entity;
        }

        public PimsLeaseTerm Add(PimsLeaseTerm pimsLeaseTerm)
        {
            var updatedEntity = this.Context.Add(pimsLeaseTerm);
            return updatedEntity.Entity;
        }

        public IEnumerable<PimsLeaseTerm> GetAllByLeaseId(long leaseId)
        {
            var terms = this.Context.PimsLeaseTerms.AsNoTracking()
                .Include(t => t.LeasePmtFreqTypeCodeNavigation)
                .Include(t => t.LeaseTermStatusTypeCodeNavigation)
                .Include(t => t.PimsLeasePayments)
                    .ThenInclude(p => p.LeasePaymentMethodTypeCodeNavigation)
                .Include(t => t.PimsLeasePayments)
                    .ThenInclude(p => p.LeasePaymentStatusTypeCodeNavigation)
                .Where(t => t.LeaseId == leaseId).ToArray();

            terms = terms.OrderBy(t => t.TermStartDate).ThenBy(t => t.LeaseTermId).Select(t =>
            {
                t.PimsLeasePayments = t.PimsLeasePayments.OrderBy(p => p.PaymentReceivedDate).ThenBy(p => p.LeasePaymentId).ToArray();
                return t;
            }).ToArray();

            return terms;
        }

        public PimsLeaseTerm GetById(long leaseTermId, bool loadPayments = false)
        {
            var query = this.Context.PimsLeaseTerms.AsNoTracking().Where(t => t.LeaseTermId == leaseTermId);
            if (loadPayments)
            {
                query.Include(t => t.PimsLeasePayments);
            }
            return query.FirstOrDefault() ?? throw new KeyNotFoundException();
        }
    }
}
