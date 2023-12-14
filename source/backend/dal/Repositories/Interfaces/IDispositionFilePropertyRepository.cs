using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IDispositionFilePropertyRepository : IRepository
    {
        List<PimsDispositionFileProperty> GetPropertiesByDispositionFileId(long dispositionFileId);

        int GetDispositionFilePropertyRelatedCount(long propertyId);
    }
}
