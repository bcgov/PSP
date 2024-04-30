using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface ITakeService
    {

        PimsTake GetById(long takeId);

        PimsTake AddAcquisitionPropertyTake(long acquisitionFilePropertyId, PimsTake take);

        PimsTake UpdateAcquisitionPropertyTake(long acquisitionFilePropertyId, PimsTake take);

        bool DeleteAcquisitionPropertyTake(long takeId);

        IEnumerable<PimsTake> GetByFileId(long fileId);

        IEnumerable<PimsTake> GetByPropertyId(long fileId, long acquisitionFilePropertyId);

        int GetCountByPropertyId(long propertyId);
    }
}
