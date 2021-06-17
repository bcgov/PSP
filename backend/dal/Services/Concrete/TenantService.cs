using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Pims.Dal.Services
{
    /// <summary>
    /// TenantService class, provides a service layer to interact with tenants within the datasource.
    /// </summary>
    public class TenantService : BaseService<Tenant>, ITenantService
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
        public TenantService(PimsContext dbContext, ClaimsPrincipal user, IPimsService service, ILogger<TenantService> logger) : base(dbContext, user, service, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get the tenant for the specified 'code'.
        /// </summary>
        /// <returns></returns>
        public Tenant GetTenant(string code)
        {
            return this.Context.Tenants.FirstOrDefault(t => t.Code == code);
        }

        /// <summary>
        /// Update the specified tenant in the datasource.
        /// </summary>
        /// <param name="tenant"></param>
        /// <returns></returns>
        public Tenant UpdateTenant(Tenant tenant)
        {
            if (tenant == null) throw new ArgumentNullException(nameof(tenant));
            this.User.ThrowIfNotAuthorized(Permissions.SystemAdmin);
            var originalTenant = this.Context.Tenants.FirstOrDefault(t => t.Code == tenant.Code);

            if (originalTenant == null) throw new KeyNotFoundException();

            originalTenant.Name = tenant.Name;
            originalTenant.Description = tenant.Description;
            originalTenant.Settings = tenant.Settings;
            originalTenant.RowVersion = tenant.RowVersion;

            this.Context.Tenants.Update(originalTenant);
            this.Context.SaveChanges();

            return originalTenant;
        }
        #endregion
    }
}
