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
    public class ChartOfAccountsCodeRepository : BaseRepository<PimsChartOfAccountsCode>, IChartOfAccountsCodeRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a ChartOfAccountsCodeRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public ChartOfAccountsCodeRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<ChartOfAccountsCodeRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Retrieves all chart of account codes.
        /// </summary>
        /// <returns></returns>
        public IList<PimsChartOfAccountsCode> GetAllChartOfAccountCodes()
        {
            return this.Context.PimsChartOfAccountsCodes.AsNoTracking()
                .ToList();
        }

        #endregion
    }
}
