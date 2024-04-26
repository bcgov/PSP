using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// ITakeRepository interface, provides a service layer to administer takes within the datasource.
    /// </summary>
    public interface ITakeRepository : IRepository<PimsTake>
    {
        IEnumerable<PimsTake> GetAllByAcquisitionFileId(long fileId);

        IEnumerable<PimsTake> GetAllByAcqPropertyId(long fileId, long acquisitionFilePropertyId);

        IEnumerable<PimsTake> GetAllByPropertyId(long propertyId);

        int GetCountByPropertyId(long propertyId);

        void UpdateAcquisitionPropertyTakes(long acquisitionFilePropertyId, IEnumerable<PimsTake> takes);

        IEnumerable<PimsTake> GetAllByPropertyAcquisitionFileId(long acquisitionFilePropertyId);
    }
}
