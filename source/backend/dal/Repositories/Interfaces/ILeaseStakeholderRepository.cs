using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// ILeaseStakeholderRepository interface, provides functions to interact with lease stakeholder within the datasource.
    /// </summary>
    public interface ILeaseStakeholderRepository : IRepository<PimsLeaseStakeholder>
    {

        IEnumerable<PimsLeaseStakeholder> GetByLeaseId(long leaseId);

        IEnumerable<PimsLeaseStakeholder> Update(long leaseId, IEnumerable<PimsLeaseStakeholder> pimsLeaseStakeholders);
    }
}
