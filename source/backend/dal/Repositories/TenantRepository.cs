using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// TenantRepository class, provides a service layer to interact with tenants within the datasource.
    /// </summary>
    public class TenantRepository : BaseRepository<PimsTenant>, ITenantRepository
    {
        #region Variables
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a TenantRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public TenantRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<TenantRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get the tenant for the specified 'code'.
        /// </summary>
        /// <returns></returns>
        public PimsTenant TryGetTenantByCode(string code)
        {
            return this.Context.PimsTenants.FirstOrDefault(t => t.Code == code);
        }
        #endregion
    }
}
