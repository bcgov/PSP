using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IDispositionFilePropertyRepository : IRepository
    {
        List<PimsPropertyDispositionFile> GetPropertiesByDispositionFileId(long dispositionFileId);

        int GetDispositionFilePropertyRelatedCount(long propertyId);
    }
}