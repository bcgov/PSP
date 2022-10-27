using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Provides a service layer to administrate static variables within the datasource.
    /// </summary>
    public class SystemConstantRepository : BaseRepository<PimsStaticVariable>, ISystemConstantRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a SystemConstantRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public SystemConstantRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<SystemConstantRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get all static variables.
        /// </summary>
        public IEnumerable<PimsStaticVariable> GetAll()
        {
            return this.Context.PimsStaticVariables.AsNoTracking().ToArray();
        }
        #endregion
    }
}
