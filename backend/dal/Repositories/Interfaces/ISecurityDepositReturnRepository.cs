using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// ISecurityDepositReturnRepository interface, provides functions to interact with deposit returns within the datasource.
    /// </summary>
    public interface ISecurityDepositReturnRepository : IRepository<PimsSecurityDepositReturn>
    {
        IEnumerable<PimsSecurityDepositReturn> GetByLeaseId(long leaseId);
        PimsSecurityDepositReturn GetById(long id);
        IEnumerable<PimsSecurityDepositReturn> GetByDepositId(long id);
        PimsSecurityDepositReturn Add(PimsSecurityDepositReturn securityDeposit);
        PimsSecurityDepositReturn Update(PimsSecurityDepositReturn securityDeposit);
        void Delete(long id);
    }
}
