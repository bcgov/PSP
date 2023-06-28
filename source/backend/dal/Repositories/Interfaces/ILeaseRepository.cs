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

        IEnumerable<PimsLease> GetAllByFilter(LeaseFilter filter, HashSet<short> regionCodes, bool loadPayments = false);

        long GetRowVersion(long id);

        PimsLease Get(long id);

        PimsLease GetNoTracking(long id);

        Paged<PimsLease> GetPage(LeaseFilter filter, HashSet<short> regions);

        IList<PimsLeaseDocument> GetAllLeaseDocuments(long leaseId);

        PimsLease Add(PimsLease lease);

        PimsLeaseDocument AddLeaseDocument(PimsLeaseDocument leaseDocument);

        void DeleteLeaseDocument(long leaseDocumentId);

        PimsLease Update(PimsLease lease, bool commitTransaction = true);

        PimsLease UpdateLeaseConsultations(long leaseId, long? rowVersion, ICollection<PimsLeaseConsultation> pimsLeaseConsultations);
    }
}
