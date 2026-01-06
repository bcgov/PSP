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
        /// Add Property Improvement.
        /// </summary>
        /// <param name="propertyImprovement"></param>
        /// <returns></returns>
        public PimsPropertyImprovement Add(PimsPropertyImprovement propertyImprovement)
        {
            using var scope = Logger.QueryScope();

            Context.PimsPropertyImprovements.Add(propertyImprovement);

            return propertyImprovement;
        }

        /// <summary>
        /// Update the Property Improvement.
        /// </summary>
        /// <param name="propertyId"></param>
        /// <param name="propertyImprovement"></param>
        /// <returns></returns>
        public PimsPropertyImprovement Update(long propertyId, PimsPropertyImprovement propertyImprovement)
        {
            using var scope = Logger.QueryScope();

            PimsPropertyImprovement existingImprovement = Context.PimsPropertyImprovements
                .FirstOrDefault(x => x.PropertyId == propertyId && x.PropertyImprovementId == propertyImprovement.PropertyImprovementId);

            Context.Entry(existingImprovement).CurrentValues.SetValues(propertyImprovement);

            return existingImprovement;
        }

        /// <summary>
        /// Get improvement by Id.
        /// </summary>
        /// <param name="propertyId"></param>
        /// <param name="propertyImprovementId"></param>
        /// <returns></returns>
        public PimsPropertyImprovement Get(long propertyId, long propertyImprovementId)
        {
            using var scope = Logger.QueryScope();

            return Context.PimsPropertyImprovements.AsNoTracking()
                .Include(x => x.PropertyImprovementTypeCodeNavigation)
                .FirstOrDefault(x => x.PropertyImprovementId == propertyImprovementId && x.PropertyId == propertyId) ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Delete the Property Improvement.
        /// </summary>
        /// <param name="propertyId"></param>
        /// <param name="propertyImprovementId"></param>
        /// <returns></returns>
        public bool TryDelete(long propertyId, long propertyImprovementId)
        {
            using var scope = Logger.QueryScope();

            var deletedEntity = Context.PimsPropertyImprovements.Where(x => x.PropertyId == propertyId && x.PropertyImprovementId == propertyImprovementId).FirstOrDefault();
            if (deletedEntity is not null)
            {
                Context.PimsPropertyImprovements.Remove(deletedEntity);

                return true;
            }

            return false;
        }
        #endregion
    }
}
