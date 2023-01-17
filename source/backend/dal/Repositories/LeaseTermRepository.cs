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
            this.Context.Update(pimsLeaseTerm);
            return pimsLeaseTerm;
        }

        public PimsLeaseTerm Add(PimsLeaseTerm pimsLeaseTerm)
        {
            this.Context.Add(pimsLeaseTerm);
            return pimsLeaseTerm;
        }

        public IEnumerable<PimsLeaseTerm> GetAllByLeaseId(long leaseId)
        {
            return this.Context.PimsLeaseTerms.AsNoTracking().Where(t => t.LeaseId == leaseId).ToArray();
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
