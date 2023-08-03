using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// LeaseTenantRepository class, provides a service layer to interact with lease tenants within the datasource.
    /// </summary>
    public class LeaseTenantRepository : BaseRepository<PimsLeaseTenant>, ILeaseTenantRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a LeaseTenantRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public LeaseTenantRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<LeaseRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// get the tenants on a lease.
        /// </summary>
        /// <param name="leaseId"></param>
        /// <returns></returns>
        public IEnumerable<PimsLeaseTenant> GetByLeaseId(long leaseId)
        {
            using var scope = Logger.QueryScope();
            return this.Context.PimsLeaseTenants
                .Include(t => t.LessorTypeCodeNavigation)
                .Include(t => t.Organization)
                    .ThenInclude(o => o.PimsPersonOrganizations)
                    .ThenInclude(op => op.Person)
                .Include(t => t.Organization)
                    .ThenInclude(o => o.PimsOrganizationAddresses)
                    .ThenInclude(oa => oa.Address)
                .Include(t => t.Organization)
                    .ThenInclude(o => o.PimsOrganizationAddresses)
                    .ThenInclude(oa => oa.AddressUsageTypeCodeNavigation)
                .Include(t => t.Organization)
                    .ThenInclude(o => o.PimsContactMethods)
                    .ThenInclude(c => c.ContactMethodTypeCodeNavigation)
                .Include(t => t.Person)
                    .ThenInclude(p => p.PimsPersonAddresses)
                    .ThenInclude(pa => pa.Address)
                .Include(t => t.Person)
                    .ThenInclude(p => p.PimsPersonAddresses)
                    .ThenInclude(pa => pa.AddressUsageTypeCodeNavigation)
                .Include(t => t.Person)
                    .ThenInclude(p => p.PimsContactMethods)
                    .ThenInclude(c => c.ContactMethodTypeCodeNavigation)
                .Include(t => t.PrimaryContact)
                .Include(t => t.TenantTypeCodeNavigation)
                .Where(l => l.LeaseId == leaseId).AsNoTracking()
                 ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// update the tenants on a lease.
        /// </summary>
        /// <param name="leaseId"></param>
        /// <param name="pimsLeaseTenants"></param>
        /// <returns></returns>
        public IEnumerable<PimsLeaseTenant> Update(long leaseId, IEnumerable<PimsLeaseTenant> pimsLeaseTenants)
        {
            using var scope = Logger.QueryScope();
            this.Context.UpdateChild<PimsLease, long, PimsLeaseTenant, long>(l => l.PimsLeaseTenants, leaseId, pimsLeaseTenants.ToArray());

            return GetByLeaseId(leaseId);
        }
        #endregion
    }
}
