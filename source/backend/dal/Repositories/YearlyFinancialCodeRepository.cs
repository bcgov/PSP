using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Provides a repository to interact with financial codes within the datasource.
    /// </summary>
    public class YearlyFinancialCodeRepository : BaseRepository<PimsYearlyFinancialCode>, IYearlyFinancialCodeRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a YearlyFinancialCodeRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public YearlyFinancialCodeRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<YearlyFinancialCodeRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Retrieves all yearly financial codes.
        /// </summary>
        /// <returns></returns>
        public IList<PimsYearlyFinancialCode> GetAllYearlyFinancialCodes()
        {
            return this.Context.PimsYearlyFinancialCodes.AsNoTracking()
                .ToList();
        }

        #endregion
    }
}
