using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface ISecurityDepositService
    {
        public IEnumerable<PimsSecurityDeposit> GetLeaseDeposits(long leaseId);

        PimsSecurityDeposit AddLeaseDeposit(long leaseId, PimsSecurityDeposit deposit);

        PimsSecurityDeposit UpdateLeaseDeposit(long leaseId, PimsSecurityDeposit deposit);

        void UpdateLeaseDepositNote(long leaseId, string note);

        bool DeleteLeaseDeposit(PimsSecurityDeposit deposit);
    }
}
