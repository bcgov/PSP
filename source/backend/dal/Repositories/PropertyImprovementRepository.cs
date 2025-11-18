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
        /// Creates a new instance of a PropertyImprovementRepository, and initializes it with the specified arguments.
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
        /// get the improvements on a property.
        /// </summary>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        public IEnumerable<PimsPropertyImprovement> GetByPropertyId(long propertyId)
        {
            using var scope = Logger.QueryScope();

            return Context.PimsPropertyImprovements.AsNoTracking()
                .Include(pi => pi.PropertyImprovementTypeCodeNavigation)
                .Where(x => x.PropertyId == propertyId)
                .OrderBy(i => i.PropertyImprovementTypeCode) ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// update the improvements on a property.
        /// </summary>
        /// <param name="propertyId"></param>
        /// <param name="pimsPropertyImprovements"></param>
        /// <returns></returns>
        public IEnumerable<PimsPropertyImprovement> Update(long propertyId, IEnumerable<PimsPropertyImprovement> pimsPropertyImprovements)
        {
            using var scope = Logger.QueryScope();

            Context.UpdateChild<PimsProperty, long, PimsPropertyImprovement, long>(x => x.PimsPropertyImprovements, propertyId, pimsPropertyImprovements.ToArray());

            return pimsPropertyImprovements;
        }
        #endregion
    }
}
