using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Provides a repository to interact with projects within the datasource.
    /// </summary>
    public class ProjectRepository : BaseRepository<PimsProject>, IProjectRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a ProjectRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public ProjectRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<ProjectRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Retrieves the activities with the specified research file id.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="maxResult"></param>
        /// <returns></returns>
        public IList<PimsProject> GetProjectPrediction(string filter, int maxResult)
        {
            return this.Context.PimsProjects.AsNoTracking()
                .Where(o => EF.Functions.Like(o.Description, $"%{filter}%"))
                .OrderBy(a => a.Code)
                .Take(maxResult)
                .ToArray();
        }

        #endregion
    }
}
