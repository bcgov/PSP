using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IAcquisitionFileChecklistRepository : IRepository
    {

        IEnumerable<PimsAcqChklstItemType> GetAllChecklistItemTypes();

        List<PimsAcquisitionChecklistItem> GetAllChecklistItemsByAcquisitionFileId(long acquisitionFileId);

        PimsAcquisitionChecklistItem Update(PimsAcquisitionChecklistItem checklistItem);

        PimsAcquisitionChecklistItem Add(PimsAcquisitionChecklistItem checklistItem);
    }
}
