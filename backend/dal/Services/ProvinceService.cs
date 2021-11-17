using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MapsterMapper;

namespace Pims.Dal.Services
{
    /// <summary>
    /// ProvinceService class, provides a service layer to administrate provinces within the datasource.
    /// </summary>
    public class ProvinceService : BaseService<PimsProvinceState>, IProvinceService
    {
        #region Variables
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ProvinceService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public ProvinceService(PimsContext dbContext, ClaimsPrincipal user, IPimsService service, ILogger<ProvinceService> logger, IMapper mapper) : base(dbContext, user, service, logger, mapper) { }
        #endregion

        #region Methods
        /// <summary>
        /// Get all of provinces from the datasource.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PimsProvinceState> Get()
        {
            var query = this.Context.PimsProvinceStates.AsNoTracking();
            return query.OrderBy(p => p.DisplayOrder).OrderBy(p => p.ProvinceStateId).ToArray();
        }
        #endregion
    }
}
