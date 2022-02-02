using Pims.Dal.Entities;

namespace Pims.Dal.Services
{
    public interface ILeaseService
    {
        bool IsRowVersionEqual(long leaseId, long rowVersion);
        PimsLease GetById(long leaseId);
    }
}
