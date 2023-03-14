using System.Collections.Generic;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Repositories
{
    public interface IAcquisitionFileRepository : IRepository
    {
        Paged<PimsAcquisitionFile> GetPage(AcquisitionFilter filter, HashSet<short> regions);

        PimsAcquisitionFile GetById(long id);

        PimsAcquisitionFile Add(PimsAcquisitionFile acquisitionFile);

        PimsAcquisitionFile Update(PimsAcquisitionFile acquisitionFile);

        long GetRowVersion(long id);

        short GetRegion(long id);

        List<PimsAcquisitionFile> GetByProductId(long productId);

        IEnumerable<PimsAcqChklstItemType> GetAllAcquisitionChecklistItemTypes();

        List<PimsAcquisitionChecklistItem> GetChecklistItemsByAcquisitionFileId(long acquisitionFileId);

        PimsAcquisitionFile UpdateChecklistItems(PimsAcquisitionFile acquisitionFile);
    }
}
