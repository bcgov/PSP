using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IExpropriationEventRepository : IRepository<PimsExpropOwnerHistory>
    {
        IEnumerable<PimsExpropOwnerHistory> GetExpropriationEventsByAcquisitionFile(long acquisitionFileId);

        PimsExpropOwnerHistory GetExpropriationEventById(long expropriationHistoryId);

        PimsExpropOwnerHistory AddExpropriationEvent(PimsExpropOwnerHistory expropriationHistory);

        PimsExpropOwnerHistory UpdateExpropriationEvent(PimsExpropOwnerHistory expropriationHistory);

        bool TryDeleteExpropriationEvent(long acquisitionFileId, long expropriationHistoryId);
    }
}
