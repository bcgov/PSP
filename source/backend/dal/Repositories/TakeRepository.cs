using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
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
                .Where(t => t.PropertyAcquisitionFile.AcquisitionFileId == fileId);
        }

        /// <summary>
        /// Get the count of all takes for this property on any acquisition file.
        /// </summary>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        public int GetCountByPropertyId(long propertyId)
        {
            return Context.PimsTakes
                .Include(t => t.PropertyAcquisitionFile)
                .Count(t => t.PropertyAcquisitionFile.PropertyId == propertyId);
        }

        /// <summary>
        /// Sets the passed list of takes as the takes associated to the given acquisition property, adding, deleting and updating as necessary.
        /// </summary>
        /// <param name="acquisitionFilePropertyId"></param>
        /// <param name="takes"></param>
        public void UpdateAcquisitionPropertyTakes(long acquisitionFilePropertyId, IEnumerable<PimsTake> takes)
        {
            Context.UpdateChild<PimsPropertyAcquisitionFile, long, PimsTake>(p => p.PimsTakes, acquisitionFilePropertyId, takes.ToArray(), true);
            
        }

        /// <summary>
        /// Returns the total number of takes in the database.
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return Context.PimsTakes.Count();
        }

        public IEnumerable<PimsTake> GetAllByPropertyAcquisitionFileId(long acquisitionFilePropertyId)
        {
            return Context.PimsTakes.Include(t => t.PropertyAcquisitionFile).Where(pf => pf.PropertyAcquisitionFileId == acquisitionFilePropertyId);
        }
    }
}
