using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MapsterMapper;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// TenantService class, provides a service layer to interact with tenants within the datasource.
    /// </summary>
    public class TenantService : BaseRepository<PimsTenant>, ITenantService
    {
        #region Variables
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a TenantService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="service"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public TenantService(PimsContext dbContext, ClaimsPrincipal user, IPimsRepository service, ILogger<TenantService> logger, IMapper mapper) : base(dbContext, user, service, logger, mapper) { }
        #endregion

        #region Methods

        /// <summary>
        /// Get the tenant for the specified 'code'.
        /// </summary>
        /// <returns></returns>
        public PimsTenant GetTenant(string code)
        {
            return this.Context.PimsTenants.FirstOrDefault(t => t.Code == code);
        }

        /// <summary>
        /// Update the specified tenant in the datasource.
        /// </summary>
        /// <param name="tenant"></param>
        /// <returns></returns>
        public PimsTenant UpdateTenant(PimsTenant tenant)
        {
            if (tenant == null) throw new ArgumentNullException(nameof(tenant));
            this.User.ThrowIfNotAuthorized(Permissions.SystemAdmin);
            var originalTenant = this.Context.PimsTenants.FirstOrDefault(t => t.Code == tenant.Code);

            if (originalTenant == null) throw new KeyNotFoundException();

            originalTenant.Name = tenant.Name;
            originalTenant.Description = tenant.Description;
            originalTenant.Settings = tenant.Settings;
            originalTenant.ConcurrencyControlNumber = tenant.ConcurrencyControlNumber;

            this.Context.PimsTenants.Update(originalTenant);
            this.Context.CommitTransaction();

            return originalTenant;
        }
        #endregion
    }
}
