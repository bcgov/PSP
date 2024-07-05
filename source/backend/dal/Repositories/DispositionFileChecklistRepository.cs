using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Provides a repository to interact with disposition file checklist items within the datasource.
    /// </summary>
    public class DispositionFileChecklistRepository : BaseRepository<PimsDispositionChecklistItem>, IDispositionFileChecklistRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a DispositionFileChecklistRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public DispositionFileChecklistRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<DispositionFileChecklistRepository> logger)
            : base(dbContext, user, logger)
        {
        }

        #endregion

        #region Methods

        public IEnumerable<PimsDspChklstItemType> GetAllChecklistItemTypes()
        {
            return Context.PimsDspChklstItemTypes
                .Include(it => it.DspChklstSectionTypeCodeNavigation)
                .OrderBy(it => it.DisplayOrder)
                .AsNoTracking()
                .ToArray();
        }

        public List<PimsDispositionChecklistItem> GetAllChecklistItemsByDispositionFileId(long dispositionFileId)
        {
            using var scope = Logger.QueryScope();

            return Context.PimsDispositionChecklistItems
                .Where(ci => ci.DispositionFileId == dispositionFileId)
                .Include(ci => ci.ChklstItemStatusTypeCodeNavigation)
                .Include(ci => ci.DspChklstItemTypeCodeNavigation)
                    .ThenInclude(it => it.DspChklstSectionTypeCodeNavigation)
                .AsNoTracking()
                .ToList();
        }

        public PimsDispositionChecklistItem Add(PimsDispositionChecklistItem checklistItem)
        {
            using var scope = Logger.QueryScope();
            Context.PimsDispositionChecklistItems.Add(checklistItem);
            return checklistItem;
        }

        public PimsDispositionChecklistItem Update(PimsDispositionChecklistItem checklistItem)
        {
            checklistItem.ThrowIfNull(nameof(checklistItem));

            Context.Entry(checklistItem).CurrentValues.SetValues(checklistItem);
            Context.Entry(checklistItem).State = EntityState.Modified;
            return checklistItem;
        }

        #endregion
    }
}
