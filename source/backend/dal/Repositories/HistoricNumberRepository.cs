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
    /// HistoricalNumberRepository class, provides a service layer to interact with property Historic Numbers within the datasource.
    /// </summary>
    public class HistoricalNumberRepository : BaseRepository<PimsHistoricalFileNumber>, IHistoricalNumberRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a HistoricalNumberRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public HistoricalNumberRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<HistoricalNumberRepository> logger)
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
        public IList<PimsHistoricalFileNumber> GetAllByPropertyId(long propertyId)
        {
            var historicalFileNumbers = Context.PimsHistoricalFileNumbers.AsNoTracking()
                .Include(p => p.HistoricalFileNumberTypeCodeNavigation)
                .Where(p => p.PropertyId == propertyId)
                .OrderBy(p => p.HistoricalFileNumberTypeCodeNavigation.DisplayOrder)
                .ToList();
            return historicalFileNumbers;
        }

        public IList<PimsHistoricalFileNumber> UpdateHistoricalFileNumbers(long propertyId, IEnumerable<PimsHistoricalFileNumber> pimsHistoricalNumbers)
        {
            using var scope = Logger.QueryScope();
            Context.UpdateChild<PimsProperty, long, PimsHistoricalFileNumber, long>(l => l.PimsHistoricalFileNumbers, propertyId, pimsHistoricalNumbers.ToArray());
            return GetAllByPropertyId(propertyId);
        }
        #endregion
    }
}
