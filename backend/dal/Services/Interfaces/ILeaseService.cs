using Pims.Dal.Entities.Models;
using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Services
{
    /// <summary>
    /// ILeaseService interface, provides functions to interact with leases within the datasource.
    /// </summary>
    public interface ILeaseService : IService<PimsLease>
    {
        int Count();
        IEnumerable<PimsLease> Get(LeaseFilter filter);
        PimsLease Get(long id);
        Paged<PimsLease> GetPage(LeaseFilter filter);
        PimsLease Add(PimsLease lease, bool userOverride = false);
        PimsLease Update(PimsLease lease, bool commitTransaction = true);
        PimsLease UpdateLeaseTenants(long leaseId, long rowVersion, ICollection<PimsLeaseTenant> pimsLeaseTenants);
        PimsLease UpdateLeaseImprovements(long leaseId, long rowVersion, ICollection<PimsPropertyImprovement> pimsPropertyImprovements);
        PimsLease UpdatePropertyLeases(long leaseId, long rowVersion, ICollection<PimsPropertyLease> pimsPropertyLeases, bool userOverride = false);
    }
}
