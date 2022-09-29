using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IAcquisitionFilePropertyRepository : IRepository
    {
        List<PimsPropertyAcquisitionFile> GetByAcquisitionFileId(long acquisitionFileId);

        int GetAcquisitionFilePropertyRelatedCount(long propertyId);

        PimsPropertyAcquisitionFile Add(PimsPropertyAcquisitionFile propertyAcquisitionFile);

        PimsPropertyAcquisitionFile Update(PimsPropertyAcquisitionFile propertyAcquisitionFile);

        void Delete(PimsPropertyAcquisitionFile propertyAcquisitionFile);
    }
}
