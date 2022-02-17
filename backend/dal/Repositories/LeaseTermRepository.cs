using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MapsterMapper;
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
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public LeaseTermRepository(PimsContext dbContext, ClaimsPrincipal user, IPimsRepository service, ILogger<LeaseRepository> logger, IMapper mapper) : base(dbContext, user, service, logger, mapper) { }
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

        public IEnumerable<PimsLeaseTerm> GetByLeaseId(long leaseId)
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
