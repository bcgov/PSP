using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface IExpropriationEventService
    {
        IEnumerable<PimsExpropOwnerHistory> GetExpropriationEvents(long acquisitionFileId);

        PimsExpropOwnerHistory GetExpropriationEventById(long expropriationHistoryId);

        PimsExpropOwnerHistory AddExpropriationEvent(long acquisitionFileId, PimsExpropOwnerHistory expropriationEvent);

        PimsExpropOwnerHistory UpdateExpropriationEvent(long acquisitionFileId, PimsExpropOwnerHistory expropriationEvent);

        bool DeleteExpropriationEvent(long acquisitionFileId, long expropriationEventId);
    }
}
