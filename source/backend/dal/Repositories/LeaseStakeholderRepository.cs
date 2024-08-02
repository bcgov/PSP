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
    /// LeaseStakeholderRepository class, provides a service layer to interact with lease stakeholders within the datasource.
    /// </summary>
    public class LeaseStakeholderRepository : BaseRepository<PimsLeaseStakeholder>, ILeaseStakeholderRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a LeaseStakeholderRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public LeaseStakeholderRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<LeaseRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// get the stakeholders on a lease.
        /// </summary>
        /// <param name="leaseId"></param>
        /// <returns></returns>
        public IEnumerable<PimsLeaseStakeholder> GetByLeaseId(long leaseId)
        {
            using var scope = Logger.QueryScope();
            return this.Context.PimsLeaseStakeholders
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
                .Include(t => t.LeaseStakeholderTypeCodeNavigation)
                .Where(l => l.LeaseId == leaseId).AsNoTracking()
                 ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// update the stakeholders on a lease.
        /// </summary>
        /// <param name="leaseId"></param>
        /// <param name="pimsLeaseStakeholders"></param>
        /// <returns></returns>
        public IEnumerable<PimsLeaseStakeholder> Update(long leaseId, IEnumerable<PimsLeaseStakeholder> pimsLeaseStakeholders)
        {
            using var scope = Logger.QueryScope();
            this.Context.UpdateChild<PimsLease, long, PimsLeaseStakeholder, long>(l => l.PimsLeaseStakeholders, leaseId, pimsLeaseStakeholders.ToArray());

            return GetByLeaseId(leaseId);
        }
        #endregion
    }
}
