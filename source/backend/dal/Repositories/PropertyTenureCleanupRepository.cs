using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// PropertyTenureCleanupRepository class, provides a service layer to interact with property tenure cleanups within the datasource.
    /// </summary>
    public class PropertyTenureCleanupRepository : BaseRepository<PimsPropTenureCleanup>, IPropertyTenureCleanupRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a PropertyTenureCleanupRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public PropertyTenureCleanupRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<PropertyTenureCleanupRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get all historical file numbers by property id.
        /// </summary>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        public IList<PimsPropTenureCleanup> GetAllByPropertyId(long propertyId)
        {
            var tenureCleanups = Context.PimsPropTenureCleanups.AsNoTracking()
                .Include(p => p.TenureCleanupTypeCodeNavigation)
                .Where(p => p.PropertyId == propertyId)
                .OrderBy(p => p.TenureCleanupTypeCodeNavigation.DisplayOrder)
                .ToList();
            return tenureCleanups;
        }

        #endregion
    }
}
