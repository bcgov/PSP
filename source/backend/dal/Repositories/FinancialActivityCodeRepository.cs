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
    public class FinancialActivityCodeRepository : BaseRepository<PimsFinancialActivityCode>, IFinancialActivityCodeRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a FinancialActivityCodeRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public FinancialActivityCodeRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<FinancialActivityCodeRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Retrieves all financial activity codes.
        /// </summary>
        /// <returns></returns>
        public IList<PimsFinancialActivityCode> GetAllFinancialActivityCodes()
        {
            return this.Context.PimsFinancialActivityCodes.AsNoTracking()
                .ToList();
        }

        #endregion
    }
}
