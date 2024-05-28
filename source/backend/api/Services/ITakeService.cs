using System.Collections.Generic;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;

namespace Pims.Api.Services
{
    public interface ITakeService
    {

        PimsTake GetById(long takeId);

        PimsTake AddAcquisitionPropertyTake(long acquisitionFilePropertyId, PimsTake take);

        PimsTake UpdateAcquisitionPropertyTake(long acquisitionFilePropertyId, PimsTake take);

        bool DeleteAcquisitionPropertyTake(long takeId, IEnumerable<UserOverrideCode> userOverrides);

        IEnumerable<PimsTake> GetByFileId(long fileId);

        IEnumerable<PimsTake> GetByPropertyId(long fileId, long acquisitionFilePropertyId);

        int GetCountByPropertyId(long propertyId);
    }
}
