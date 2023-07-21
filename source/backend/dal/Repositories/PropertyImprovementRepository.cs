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
    /// PropertyImprovementRepository class, provides a service layer to interact with property improvements within the datasource.
    /// </summary>
    public class PropertyImprovementRepository : BaseRepository<PimsPropertyImprovement>, IPropertyImprovementRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a LeaseRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public PropertyImprovementRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<LeaseRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// get the improvements on a lease.
        /// </summary>
        /// <param name="leaseId"></param>
        /// <returns></returns>
        public IEnumerable<PimsPropertyImprovement> GetByLeaseId(long leaseId)
        {
            using var scope = Logger.QueryScope();
            return this.Context.PimsPropertyImprovements
                .Include(pi => pi.PropertyImprovementTypeCodeNavigation)
                .Where(l => l.LeaseId == leaseId).AsNoTracking()
                .OrderBy(i => i.PropertyImprovementTypeCode)
                 ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// update the improvements on a lease.
        /// </summary>
        /// <param name="leaseId"></param>
        /// <param name="pimsPropertyImprovements"></param>
        /// <returns></returns>
        public IEnumerable<PimsPropertyImprovement> Update(long leaseId, IEnumerable<PimsPropertyImprovement> pimsPropertyImprovements)
        {
            using var scope = Logger.QueryScope();
            this.Context.UpdateChild<PimsLease, long, PimsPropertyImprovement, long>(l => l.PimsPropertyImprovements, leaseId, pimsPropertyImprovements.ToArray());

            return pimsPropertyImprovements;
        }
        #endregion
    }
}
