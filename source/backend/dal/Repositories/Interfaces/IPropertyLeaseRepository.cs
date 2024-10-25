using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// IPropertyLeaseRepository interface, provides functions to interact with property leases within the datasource.
    /// </summary>
    public interface IPropertyLeaseRepository : IRepository<PimsPropertyLease>
    {
        IEnumerable<PimsPropertyLease> GetAllByPropertyId(long propertyId);

        IEnumerable<PimsPropertyLease> GetAllByLeaseId(long leaseId);

        IEnumerable<PimsPropertyLease> UpdatePropertyLeases(long leaseId, ICollection<PimsPropertyLease> pimsPropertyLeases);

        bool LeaseFilePropertyInCompensationReq(long propertyLeaseFileId);
    }
}
