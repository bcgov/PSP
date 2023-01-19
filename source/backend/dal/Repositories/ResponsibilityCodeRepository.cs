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
    public class ResponsibilityCodeRepository : BaseRepository<PimsResponsibilityCode>, IResponsibilityCodeRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a ResponsibilityCodeRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public ResponsibilityCodeRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<ResponsibilityCodeRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Retrieves all responsibility codes.
        /// </summary>
        /// <returns></returns>
        public IList<PimsResponsibilityCode> GetAllResponsibilityCodes()
        {
            return this.Context.PimsResponsibilityCodes.AsNoTracking()
                .ToList();
        }

        #endregion
    }
}
