using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface ISecurityDepositService
    {
        PimsLease AddLeaseDeposit(long leaseId, long leaseRowVersion, PimsSecurityDeposit deposit);

        PimsLease UpdateLeaseDeposit(long leaseId, long leaseRowVersion, PimsSecurityDeposit deposit);

        PimsLease UpdateLeaseDepositNote(long leaseId, long leaseRowVersion, string note);

        PimsLease DeleteLeaseDeposit(long leaseId, long leaseRowVersion, PimsSecurityDeposit deposit);
    }
}
