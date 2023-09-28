using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Security;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// PropertyLeaseRepository class, provides a service layer to interact with property leases within the datasource.
    /// </summary>
    public class PropertyLeaseRepository : BaseRepository<PimsPropertyLease>, IPropertyLeaseRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a PropertyLeaseRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public PropertyLeaseRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<PropertyLeaseRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get the associated property leases for the specified property id.
        /// </summary>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        public IEnumerable<PimsPropertyLease> GetAllByPropertyId(long propertyId)
        {
            return Context.PimsPropertyLeases.AsNoTracking()
                .Include(pl => pl.Property)
                    .ThenInclude(p => p.Address)
                .Include(pl => pl.Lease)
                    .ThenInclude(l => l.LeaseStatusTypeCodeNavigation)
                .Include(pl => pl.Lease)
                    .ThenInclude(l => l.PimsLeaseTerms)
                .Where(p => p.PropertyId == propertyId);
        }

        /// <summary>
        /// Get the property lease for the specified lease id.
        /// </summary>
        /// <param name="leaseId"></param>
        /// <returns></returns>
        public IEnumerable<PimsPropertyLease> GetAllByLeaseId(long leaseId)
        {
            return this.Context.PimsPropertyLeases.AsNoTracking()
                .Include(p => p.Property)
                    .ThenInclude(p => p.Address)
                .Include(l => l.Lease)
                .Where(p => p.LeaseId == leaseId);
        }

        /// <summary>
        /// Delete a propertyLeaseFile. Note that this method will fail unless all dependencies are removed first.
        /// </summary>
        /// <param name="propertyLeaseFile"></param>
        public void Delete(PimsPropertyLease propertyLeaseFile)
        {
            Context.Remove(new PimsPropertyResearchFile() { Internal_Id = propertyLeaseFile.Internal_Id });
        }

        /// <summary>
        /// update the properties on the lease.
        /// </summary>
        /// <param name="leaseId"></param>
        /// <param name="pimsPropertyLeases"></param>
        /// <returns></returns>
        public IEnumerable<PimsPropertyLease> UpdatePropertyLeases(long leaseId, ICollection<PimsPropertyLease> pimsPropertyLeases)
        {
            this.User.ThrowIfNotAuthorized(Permissions.LeaseEdit);

            this.Context.UpdateChild<PimsLease, long, PimsPropertyLease, long>(l => l.PimsPropertyLeases, leaseId, pimsPropertyLeases.ToArray());

            return GetAllByLeaseId(leaseId);
        }

        #endregion
    }
}
