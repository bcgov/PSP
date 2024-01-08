using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IDispositionFileChecklistRepository : IRepository
    {

        IEnumerable<PimsDspChklstItemType> GetAllChecklistItemTypes();

        List<PimsDispositionChecklistItem> GetAllChecklistItemsByDispositionFileId(long dispositionFileId);

        PimsDispositionChecklistItem Update(PimsDispositionChecklistItem checklistItem);

        PimsDispositionChecklistItem Add(PimsDispositionChecklistItem checklistItem);
    }
}
