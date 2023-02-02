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
    public class CostTypeCodeRepository : BaseRepository<PimsCostTypeCode>, ICostTypeCodeRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a CostTypeCodeRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public CostTypeCodeRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<CostTypeCodeRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Retrieves all cost type codes.
        /// </summary>
        /// <returns></returns>
        public IList<PimsCostTypeCode> GetAllCostTypeCodes()
        {
            return this.Context.PimsCostTypeCodes.AsNoTracking()
                .ToList();
        }

        #endregion
    }
}
