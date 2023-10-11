using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// IPropertyImprovementRepository interface, provides functions to interact with leases within the datasource.
    /// </summary>
    public interface IPropertyImprovementRepository : IRepository<PimsPropertyImprovement>
    {

        IEnumerable<PimsPropertyImprovement> GetByLeaseId(long leaseId);

        IEnumerable<PimsPropertyImprovement> Update(long leaseId, IEnumerable<PimsPropertyImprovement> pimsPropertyImprovements);
    }
}
