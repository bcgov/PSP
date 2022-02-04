using Pims.Dal.Entities;

namespace Pims.Dal.Services
{
    public interface ISecurityDepositReturnService
    {
        PimsLease AddLeaseDepositReturn(long leaseId, long leaseRowVersion, PimsSecurityDepositReturn deposit);
        PimsLease UpdateLeaseDepositReturn(long leaseId, long leaseRowVersion, PimsSecurityDepositReturn deposit);
        PimsLease DeleteLeaseDepositReturn(long leaseId, long leaseRowVersion, PimsSecurityDepositReturn deposit);
    }
}
