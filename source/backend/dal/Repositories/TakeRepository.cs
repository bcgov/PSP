using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Provides a repository to interact with takes within the datasource.
    /// </summary>
    public class TakeRepository : BaseRepository<PimsTake>, ITakeRepository
    {
        /// <summary>
        /// Creates a new instance of a TakeRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public TakeRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<TakeRepository> logger)
            : base(dbContext, user, logger)
        {
        }

        /// <summary>
        /// Get all of the takes that are associated to a given acquisition file by its id.
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public IEnumerable<PimsTake> GetAllByAcquisitionFileId(long fileId)
        {
            return Context.PimsTakes
                .Include(t => t.PropertyAcquisitionFile)
                .Include(t => t.TakeSiteContamTypeCodeNavigation)
                .Include(t => t.TakeStatusTypeCodeNavigation)
                .Include(t => t.TakeTypeCodeNavigation)
                .Include(t => t.LandActTypeCodeNavigation)
                .Where(t => t.PropertyAcquisitionFile.AcquisitionFileId == fileId);
        }

        /// <summary>
        /// Get all Takes for a Property in the Acquisition File.
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="acquisitionFilePropertyId"></param>
        /// <returns></returns>
        public IEnumerable<PimsTake> GetAllByPropertyId(long fileId, long acquisitionFilePropertyId)
        {
            return Context.PimsTakes
                .Include(t => t.PropertyAcquisitionFile)
                .Include(t => t.TakeSiteContamTypeCodeNavigation)
                .Include(t => t.TakeStatusTypeCodeNavigation)
                .Include(t => t.TakeTypeCodeNavigation)
                .Include(t => t.LandActTypeCodeNavigation)
                .Where(t => t.PropertyAcquisitionFile.AcquisitionFileId == fileId
                        && t.PropertyAcquisitionFile.PropertyId == acquisitionFilePropertyId)
                .AsNoTracking();
        }

        /// <summary>
        /// Returns the Take Counts for a Property.
        /// </summary>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        public int GetCountByPropertyId(long propertyId)
        {
            return Context.PimsTakes
                .Include(t => t.PropertyAcquisitionFile)
                .Where(x => x.PropertyAcquisitionFile.PropertyId == propertyId)
                .AsNoTracking()
                .Count();
        }

        /// <summary>
        /// Sets the passed list of takes as the takes associated to the given acquisition property, adding, deleting and updating as necessary.
        /// </summary>
        /// <param name="acquisitionFilePropertyId"></param>
        /// <param name="takes"></param>
        public void UpdateAcquisitionPropertyTakes(long acquisitionFilePropertyId, IEnumerable<PimsTake> takes)
        {
            Context.UpdateChild<PimsPropertyAcquisitionFile, long, PimsTake, long>(p => p.PimsTakes, acquisitionFilePropertyId, takes.ToArray(), true);
        }

        public IEnumerable<PimsTake> GetAllByPropertyAcquisitionFileId(long acquisitionFilePropertyId)
        {
            return Context.PimsTakes.Include(t => t.PropertyAcquisitionFile).Where(pf => pf.PropertyAcquisitionFileId == acquisitionFilePropertyId);
        }
    }
}
