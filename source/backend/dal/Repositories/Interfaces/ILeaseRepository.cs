using System;
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

        IEnumerable<PimsLease> GetAllByFilter(LeaseFilter filter, UserContextModel userContext, bool loadPayments = false);

        long GetRowVersion(long id);

        PimsLease Get(long id);

        IEnumerable<PimsLease> GetAllByIds(IEnumerable<long> leaseIds, UserContextModel userContext);

        PimsLease GetNoTracking(long id);

        LastUpdatedByModel GetLastUpdateBy(long leaseId);

        Paged<PimsLease> GetPage(LeaseFilter filter, UserContextModel userContext);

        PimsLease Add(PimsLease lease);

        PimsLease Update(PimsLease lease, bool commitTransaction = true);

        PimsLease UpdateLeaseRenewals(long leaseId, long? rowVersion, ICollection<PimsLeaseRenewal> renewals);

        IEnumerable<PimsLeaseChklstItemType> GetAllChecklistItemTypes();

        List<PimsLeaseChecklistItem> GetAllChecklistItemsByLeaseId(long leaseId);

        PimsLeaseChecklistItem AddChecklistItem(PimsLeaseChecklistItem checklistItem);

        PimsLeaseChecklistItem UpdateChecklistItem(PimsLeaseChecklistItem checklistItem);

        IEnumerable<PimsLeaseStakeholderType> GetAllLeaseStakeholderTypes();

        List<PimsLeaseLicenseTeam> GetTeamMembers(UserContextModel userContext);

        PimsLease GetLeaseAtTime(long leaseId, DateTime time);
    }
}
