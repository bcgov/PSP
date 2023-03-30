using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface ITakeService
    {
        IEnumerable<PimsTake> GetByFileId(long fileId);

        int GetTakesCountForAcquisitionProperty(long acquisitionFileId, long propertyId);

        IEnumerable<PimsTake> UpdateAcquisitionPropertyTakes(long acquisitionFilePropertyId, IEnumerable<PimsTake> takes);
    }
}
