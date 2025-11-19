using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// IPropertyImprovementRepository interface, provides functions to interact with leases within the datasource.
    /// </summary>
    public interface IPropertyImprovementRepository : IRepository<PimsPropertyImprovement>
    {

        IEnumerable<PimsPropertyImprovement> GetByPropertyId(long propertyId);

        IEnumerable<PimsPropertyImprovement> Update(long propertyId, IEnumerable<PimsPropertyImprovement> pimsPropertyImprovements);
    }
}
