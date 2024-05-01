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

        IEnumerable<PimsTake> GetAllByAcqPropertyId(long fileId, long propertyId);

        IEnumerable<PimsTake> GetAllByPropertyId(long propertyId);

        PimsTake GetById(long takeId);

        PimsTake AddTake(PimsTake take);

        PimsTake UpdateTake(PimsTake take);

        bool TryDeleteTake(long takeId);

        int GetCountByPropertyId(long propertyId);

        IEnumerable<PimsTake> GetAllByPropertyAcquisitionFileId(long acquisitionFilePropertyId);
    }
}
