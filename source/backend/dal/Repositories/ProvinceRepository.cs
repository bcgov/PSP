using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// ProvinceRepository class, provides a service layer to administrate provinces within the datasource.
    /// </summary>
    public class ProvinceRepository : BaseRepository<PimsProvinceState>, IProvinceRepository
    {
        #region Variables
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a ProvinceRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public ProvinceRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<ProvinceRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get all of provinces from the datasource.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PimsProvinceState> GetAll()
        {
            var query = this.Context.PimsProvinceStates.AsNoTracking();
            return query.OrderBy(p => p.DisplayOrder).OrderBy(p => p.ProvinceStateId).ToArray();
        }
        #endregion
    }
}
