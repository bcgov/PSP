using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// IPropertyTenureCleanupRepository interface, provides functions to interact with property's tenure cleanups within the datasource.
    /// </summary>
    public interface IPropertyTenureCleanupRepository : IRepository<PimsPropTenureCleanup>
    {
        IList<PimsPropTenureCleanup> GetAllByPropertyId(long propertyId);
    }
}
