using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IExpropriationEventRepository : IRepository<PimsExpropOwnerHistory>
    {
        IEnumerable<PimsExpropOwnerHistory> GetExpropriationEventsByAcquisitionFile(long acquisitionFileId);

        PimsExpropOwnerHistory GetExpropriationEventById(long expropriationEventId);

        PimsExpropOwnerHistory AddExpropriationEvent(PimsExpropOwnerHistory expropriationEvent);

        PimsExpropOwnerHistory UpdateExpropriationEvent(PimsExpropOwnerHistory expropriationEvent);

        bool TryDeleteExpropriationEvent(long acquisitionFileId, long expropriationEventId);
    }
}
