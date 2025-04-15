using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
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

        public IEnumerable<PimsExpropOwnerHistory> GetExpropriationEventsByAcquisitionFile(long acquisitionFileId)
        {
            using var scope = Logger.QueryScope();

            return Context.PimsExpropOwnerHistories.AsNoTracking()
                .Include(eoh => eoh.AcquisitionFile)
                .Include(x => x.InterestHolder)
                    .ThenInclude(y => y.Person)
                .Include(x => x.InterestHolder)
                    .ThenInclude(y => y.Organization)
                .Include(x => x.InterestHolder)
                    .ThenInclude(y => y.InterestHolderTypeCodeNavigation)
                .Include(eoh => eoh.AcquisitionOwner)
                .Include(eoh => eoh.ExpropOwnerHistoryTypeCodeNavigation)
                .Where(eoh => eoh.AcquisitionFileId == acquisitionFileId);
        }

        public PimsExpropOwnerHistory GetExpropriationEventById(long expropriationEventId)
        {
            using var scope = Logger.QueryScope();

            return Context.PimsExpropOwnerHistories.AsNoTracking()
                .Include(eoh => eoh.AcquisitionFile)
                .Include(x => x.InterestHolder)
                    .ThenInclude(y => y.Person)
                .Include(x => x.InterestHolder)
                    .ThenInclude(y => y.Organization)
                .Include(x => x.InterestHolder)
                    .ThenInclude(y => y.InterestHolderTypeCodeNavigation)
                .Include(eoh => eoh.AcquisitionOwner)
                .Include(eoh => eoh.ExpropOwnerHistoryTypeCodeNavigation)
                .FirstOrDefault(eoh => eoh.ExpropOwnerHistoryId == expropriationEventId) ?? throw new KeyNotFoundException();
        }

        public PimsExpropOwnerHistory AddExpropriationEvent(PimsExpropOwnerHistory expropriationEvent)
        {
            using var scope = Logger.QueryScope();

            Context.PimsExpropOwnerHistories.Add(expropriationEvent);

            return expropriationEvent;
        }

        public PimsExpropOwnerHistory UpdateExpropriationEvent(PimsExpropOwnerHistory expropriationEvent)
        {
            using var scope = Logger.QueryScope();

            var existingEvent = Context.PimsExpropOwnerHistories
                .FirstOrDefault(x => x.ExpropOwnerHistoryId == expropriationEvent.ExpropOwnerHistoryId) ?? throw new KeyNotFoundException();

            Context.Entry(existingEvent).CurrentValues.SetValues(expropriationEvent);

            return existingEvent;
        }

        public bool TryDeleteExpropriationEvent(long acquisitionFileId, long expropriationEventId)
        {
            using var scope = Logger.QueryScope();

            var eventToDelete = Context.PimsExpropOwnerHistories.FirstOrDefault(x => x.ExpropOwnerHistoryId == expropriationEventId && x.AcquisitionFileId == acquisitionFileId);
            if (eventToDelete is not null)
            {
                Context.PimsExpropOwnerHistories.Remove(eventToDelete);
                return true;
            }

            return false;
        }
    }
}
