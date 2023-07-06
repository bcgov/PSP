using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface ISecurityDepositReturnService
    {
        PimsSecurityDepositReturn AddLeaseDepositReturn(long leaseId, PimsSecurityDepositReturn deposit);

        PimsSecurityDepositReturn UpdateLeaseDepositReturn(long leaseId, PimsSecurityDepositReturn deposit);

        bool DeleteLeaseDepositReturn(long leaseId, PimsSecurityDepositReturn deposit);
    }
}
