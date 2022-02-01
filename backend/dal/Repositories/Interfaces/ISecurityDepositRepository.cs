using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// ISecurityDepositRepository interface, provides functions to interact with leases within the datasource.
    /// </summary>
    public interface ISecurityDepositRepository : IRepository<PimsSecurityDeposit>
    {
        IEnumerable<PimsSecurityDeposit> GetByLeaseId(long leaseId);        
        PimsSecurityDeposit GetById(long id);
        PimsSecurityDeposit Add(PimsSecurityDeposit securityDeposit);
        PimsSecurityDeposit Update(PimsSecurityDeposit securityDeposit);
        void Delete(long id);
    }
}
