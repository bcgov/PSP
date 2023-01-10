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
    public class WorkActivityCodeRepository : BaseRepository<PimsWorkActivityCode>, IWorkActivityCodeRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a WorkActivityCodeRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public WorkActivityCodeRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<WorkActivityCodeRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Retrieves all work activity codes.
        /// </summary>
        /// <returns></returns>
        public IList<PimsWorkActivityCode> GetAllWorkActivityCodes()
        {
            return this.Context.PimsWorkActivityCodes.AsNoTracking()
                .ToList();
        }

        #endregion
    }
}
