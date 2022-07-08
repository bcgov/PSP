using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Provides a service layer to administrate static variables within the datasource.
    /// </summary>
    public class SystemConstantService : BaseRepository<PimsStaticVariable>, ISystemConstantService
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a SystemConstantService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="repository"></param>
        /// <param name="logger"></param>
        public SystemConstantService(PimsContext dbContext, ClaimsPrincipal user, IPimsRepository repository, ILogger<SystemConstantService> logger, IMapper mapper)
            : base(dbContext, user, repository, logger, mapper) { }
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
