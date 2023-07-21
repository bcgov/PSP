using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// ILeaseTenantRepository interface, provides functions to interact with lease tenants within the datasource.
    /// </summary>
    public interface ILeaseTenantRepository : IRepository<PimsLeaseTenant>
    {

        IEnumerable<PimsLeaseTenant> GetByLeaseId(long leaseId);

        IEnumerable<PimsLeaseTenant> Update(long leaseId, IEnumerable<PimsLeaseTenant> pimsLeaseTenants);
    }
}
