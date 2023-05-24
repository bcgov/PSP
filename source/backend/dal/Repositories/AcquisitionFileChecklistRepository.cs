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
    /// Provides a repository to interact with acquisition file checklist items within the datasource.
    /// </summary>
    public class AcquisitionFileChecklistRepository : BaseRepository<PimsAcquisitionChecklistItem>, IAcquisitionFileChecklistRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a AcquisitionFileChecklistRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public AcquisitionFileChecklistRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<AcquisitionFileChecklistRepository> logger)
            : base(dbContext, user, logger)
        {
        }

        #endregion

        #region Methods

        public IEnumerable<PimsAcqChklstItemType> GetAllChecklistItemTypes()
        {
            return Context.PimsAcqChklstItemTypes
                .Include(it => it.AcqChklstSectionTypeCodeNavigation)
                .OrderBy(it => it.DisplayOrder)
                .AsNoTracking()
                .ToArray();
        }

        public List<PimsAcquisitionChecklistItem> GetAllChecklistItemsByAcquisitionFileId(long acquisitionFileId)
        {
            using var scope = Logger.QueryScope();

            return Context.PimsAcquisitionChecklistItems
                .Where(ci => ci.AcquisitionFileId == acquisitionFileId)
                .Include(ci => ci.AcqChklstItemStatusTypeCodeNavigation)
                .Include(ci => ci.AcqChklstItemTypeCodeNavigation)
                    .ThenInclude(it => it.AcqChklstSectionTypeCodeNavigation)
                .AsNoTracking()
                .ToList();
        }

        public PimsAcquisitionChecklistItem Add(PimsAcquisitionChecklistItem checklistItem)
        {
            using var scope = Logger.QueryScope();
            Context.PimsAcquisitionChecklistItems.Add(checklistItem);
            return checklistItem;
        }

        public PimsAcquisitionChecklistItem Update(PimsAcquisitionChecklistItem checklistItem)
        {
            checklistItem.ThrowIfNull(nameof(checklistItem));

            Context.Entry(checklistItem).CurrentValues.SetValues(checklistItem);
            Context.Entry(checklistItem).State = EntityState.Modified;
            return checklistItem;
        }

        #endregion
    }
}
