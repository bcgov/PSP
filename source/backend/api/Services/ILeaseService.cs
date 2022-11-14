using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface ILeaseService
    {
        bool IsRowVersionEqual(long leaseId, long rowVersion);

        PimsLease GetById(long leaseId);

        PimsLease Add(PimsLease lease, bool userOverride = false);

        PimsLease Update(PimsLease lease, bool userOverride = false);
    }
}
