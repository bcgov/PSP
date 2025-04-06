using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;

namespace Pims.Dal.Repositories
{
    public class ExpropriationEventRepository : BaseRepository<PimsExpropOwnerHistory>, IExpropriationEventRepository
    {
        public ExpropriationEventRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<ExpropriationEventRepository> logger)
            : base(dbContext, user, logger)
        {
        }

        public List<PimsExpropOwnerHistory> GetExpropriationEventsByAcquisitionFile(long acquisitionFileId)
        {
            using var scope = Logger.QueryScope();

            // TODO: Implement
            throw new System.NotImplementedException();
        }

        public PimsExpropOwnerHistory GetExpropriationEventById(long expropriationHistoryId)
        {
            using var scope = Logger.QueryScope();

            // TODO: Implement
            throw new System.NotImplementedException();
        }

        public PimsExpropOwnerHistory AddExpropriationEvent(PimsExpropOwnerHistory expropriationHistory)
        {
            using var scope = Logger.QueryScope();

            // TODO: Implement
            throw new System.NotImplementedException();
        }

        public PimsExpropOwnerHistory UpdateExpropriationEvent(PimsExpropOwnerHistory expropriationHistory)
        {
            using var scope = Logger.QueryScope();

            // TODO: Implement
            throw new System.NotImplementedException();
        }

        public bool TryDeleteExpropriationEvent(long acquisitionFileId, long expropriationHistoryId)
        {
            using var scope = Logger.QueryScope();

            // TODO: Implement
            throw new System.NotImplementedException();
        }
    }
}
