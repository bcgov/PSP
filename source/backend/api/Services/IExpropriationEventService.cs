using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface IExpropriationEventService
    {
        IEnumerable<PimsExpropOwnerHistory> GetExpropriationEvents(long acquisitionFileId);

        PimsExpropOwnerHistory GetExpropriationEventById(long expropriationHistoryId);

        PimsExpropOwnerHistory AddExpropriationEvent(long acquisitionFileId, PimsExpropOwnerHistory expropriationHistory);

        PimsExpropOwnerHistory UpdateExpropriationEvent(long acquisitionFileId, PimsExpropOwnerHistory expropriationHistory);

        bool DeleteExpropriationEvent(long acquisitionFileId, long expropriationHistoryId);
    }
}
