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

        PimsPropertyImprovement Add(PimsPropertyImprovement propertyImprovement);

        PimsPropertyImprovement Get(long propertyId, long propertyImprovementId);

        PimsPropertyImprovement Update(long propertyId, PimsPropertyImprovement propertyImprovement);

        bool TryDelete(long propertyId, long propertyImprovementId);
    }
}
