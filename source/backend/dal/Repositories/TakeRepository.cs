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
        /// Get take by id.
        /// </summary>
        /// <param name="takeId"></param>
        /// <returns></returns>
        public PimsTake GetById(long takeId)
        {
            return Context.PimsTakes

                .Include(t => t.PropertyAcquisitionFile)
                .Include(t => t.TakeSiteContamTypeCodeNavigation)
                .Include(t => t.TakeStatusTypeCodeNavigation)
                .Include(t => t.TakeTypeCodeNavigation)
                .Include(t => t.LandActTypeCodeNavigation)
                .AsNoTracking()
                .FirstOrDefault(t => t.TakeId == takeId) ?? throw new KeyNotFoundException($"Unable to find take with id {takeId}");
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
                .Where(t => t.PropertyAcquisitionFile.AcquisitionFileId == fileId)
                .AsNoTracking();
        }

        /// <summary>
        /// Get all Takes for a Property in the Acquisition File.
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        public IEnumerable<PimsTake> GetAllByAcqPropertyId(long fileId, long propertyId)
        {
            return Context.PimsTakes
                .Include(t => t.PropertyAcquisitionFile)
                .Include(t => t.TakeSiteContamTypeCodeNavigation)
                .Include(t => t.TakeStatusTypeCodeNavigation)
                .Include(t => t.TakeTypeCodeNavigation)
                .Include(t => t.LandActTypeCodeNavigation)
                .Where(t => t.PropertyAcquisitionFile.AcquisitionFileId == fileId
                        && t.PropertyAcquisitionFile.PropertyId == propertyId)
                .AsNoTracking();
        }

        /// <summary>
        /// Get all Takes for a Property.
        /// </summary>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        public IEnumerable<PimsTake> GetAllByPropertyId(long propertyId)
        {
            return Context.PimsTakes
                .Where(t => t.PropertyAcquisitionFile.PropertyId == propertyId)
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

        public PimsTake AddTake(PimsTake take)
        {
            using var scope = Logger.QueryScope();

            Context.PimsTakes.Add(take);

            return take;
        }

        /// <summary>
        /// Update the passed take.
        /// </summary>
        /// <param name="take"></param>
        public PimsTake UpdateTake(PimsTake take)
        {
            using var scope = Logger.QueryScope();

            var existingTake = Context.PimsTakes.FirstOrDefault(x => x.TakeId == take.TakeId) ?? throw new KeyNotFoundException();

            take.PropertyAcquisitionFileId = existingTake.PropertyAcquisitionFileId; // A take cannot be migrated between properties.
            Context.Entry(existingTake).CurrentValues.SetValues(take);

            return existingTake;
        }

        public bool TryDeleteTake(long takeId)
        {
            using var scope = Logger.QueryScope();

            var deletedEntity = Context.PimsTakes.Where(x => x.TakeId == takeId).FirstOrDefault();
            if (deletedEntity is not null)
            {
                Context.PimsTakes.Remove(deletedEntity);

                return true;
            }

            return false;
        }

        public IEnumerable<PimsTake> GetAllByPropertyAcquisitionFileId(long acquisitionFilePropertyId)
        {
            return Context.PimsTakes.Include(t => t.PropertyAcquisitionFile).Where(pf => pf.PropertyAcquisitionFileId == acquisitionFilePropertyId).ToList();
        }
    }
}
