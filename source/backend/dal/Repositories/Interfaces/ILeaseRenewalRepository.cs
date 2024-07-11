
using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// ILeaseRenewalRepository interface, provides functions to interact with lease renewals within the datasource.
    /// </summary>
    public interface ILeaseRenewalRepository : IRepository<PimsLeaseRenewal>
    {

        IEnumerable<PimsLeaseRenewal> GetByLeaseId(long leaseId);
    }
}
