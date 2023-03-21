using System.Collections.Generic;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// ILeaseRepository interface, provides functions to interact with leases within the datasource.
    /// </summary>
    public interface ILeaseRepository : IRepository<PimsLease>
    {
        int Count();

        IEnumerable<PimsLease> GetAllByFilter(LeaseFilter filter, bool loadPayments = false);

        long GetRowVersion(long id);

        PimsLease Get(long id);

        PimsLease GetNoTracking(long id);

        Paged<PimsLease> GetPage(LeaseFilter filter);

        PimsLease Add(PimsLease lease, bool userOverride = false);

        PimsLease Update(PimsLease lease, bool commitTransaction = true);

        PimsLease UpdateLeaseTenants(long leaseId, long rowVersion, ICollection<PimsLeaseTenant> pimsLeaseTenants);

        PimsLease UpdateLeaseImprovements(long leaseId, long rowVersion, ICollection<PimsPropertyImprovement> pimsPropertyImprovements);

        PimsLease UpdatePropertyLeases(long leaseId, long rowVersion, ICollection<PimsPropertyLease> pimsPropertyLeases, bool userOverride = false);
    }
}
