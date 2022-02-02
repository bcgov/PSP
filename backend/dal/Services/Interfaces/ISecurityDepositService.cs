using Pims.Dal.Entities;

namespace Pims.Dal.Services
{
    public interface ISecurityDepositService
    {
        PimsLease AddLeaseDeposit(long leaseId, long leaseRowVersion, PimsSecurityDeposit deposit);
        PimsLease UpdateLeaseDeposit(long leaseId, long leaseRowVersion, PimsSecurityDeposit deposit);
        PimsLease DeleteLeaseDeposit(long leaseId, long leaseRowVersion, PimsSecurityDeposit deposit);
    }
}
