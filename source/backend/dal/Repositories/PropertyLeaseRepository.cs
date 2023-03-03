using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;

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
        /// Get the property lease for the specified property id.
        /// </summary>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        public IEnumerable<PimsPropertyLease> GetAllByPropertyId(long propertyId)
        {
            return this.Context.PimsPropertyLeases.AsNoTracking().Include(p => p.Property).Include(l => l.Lease).Where(p => p.PropertyId == propertyId);
        }

        /// <summary>
        /// Get the property lease for the specified lease id.
        /// </summary>
        /// <param name="leaseId"></param>
        /// <returns></returns>
        public IEnumerable<PimsPropertyLease> GetAllByLeaseId(long leaseId)
        {
            return this.Context.PimsPropertyLeases.AsNoTracking().Include(p => p.Property).Include(l => l.Lease).Where(p => p.LeaseId == leaseId);
        }

        /// <summary>
        /// Delete a propertyLeaseFile. Note that this method will fail unless all dependencies are removed first.
        /// </summary>
        /// <param name="propertyLeaseFile"></param>
        public void Delete(PimsPropertyLease propertyLeaseFile)
        {
            Context.Remove(new PimsPropertyResearchFile() { Internal_Id = propertyLeaseFile.Internal_Id });
        }

        #endregion
    }
}
