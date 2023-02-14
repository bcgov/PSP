using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Provides a repository to interact with acquisition file properties within the datasource.
    /// </summary>
    public class AcquisitionFilePropertyRepository : BaseRepository<PimsPropertyAcquisitionFile>, IAcquisitionFilePropertyRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a AcquisitionFilePropertyRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public AcquisitionFilePropertyRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<AcquisitionFilePropertyRepository> logger)
            : base(dbContext, user, logger)
        {
        }

        #endregion

        #region Methods

        public List<PimsPropertyAcquisitionFile> GetPropertiesByAcquisitionFileId(long acquisitionFileId)
        {
            return Context.PimsPropertyAcquisitionFiles
                .Where(x => x.AcquisitionFileId == acquisitionFileId)
                .Include(rp => rp.PimsActInstPropAcqFiles)
                .Include(rp => rp.Property)
                .ThenInclude(rp => rp.RegionCodeNavigation)
                .Include(rp => rp.Property)
                .ThenInclude(rp => rp.DistrictCodeNavigation)
                .Include(rp => rp.Property)
                .ThenInclude(rp => rp.Address)
                .AsNoTracking()
                .ToList();
        }

        public int GetAcquisitionFilePropertyRelatedCount(long propertyId)
        {
            return Context.PimsPropertyAcquisitionFiles
                .Where(x => x.PropertyId == propertyId)
                .AsNoTracking()
                .Count();
        }

        public PimsPropertyAcquisitionFile Add(PimsPropertyAcquisitionFile propertyAcquisitionFile)
        {
            propertyAcquisitionFile.ThrowIfNull(nameof(propertyAcquisitionFile));

            // Mark the property not to be changed if it did not exist already.
            if (propertyAcquisitionFile.PropertyId != 0)
            {
                Context.Entry(propertyAcquisitionFile.Property).State = EntityState.Unchanged;
            }

            Context.PimsPropertyAcquisitionFiles.Add(propertyAcquisitionFile);
            return propertyAcquisitionFile;
        }

        public void Delete(PimsPropertyAcquisitionFile propertyAcquisitionFile)
        {
            propertyAcquisitionFile.ThrowIfNull(nameof(propertyAcquisitionFile));

            var propertyAcquisitionFileToDelete = Context.PimsPropertyAcquisitionFiles
                .Where(x => x.PropertyAcquisitionFileId == propertyAcquisitionFile.PropertyAcquisitionFileId)
                .Include(rp => rp.PimsActInstPropAcqFiles)
                .FirstOrDefault() ?? throw new KeyNotFoundException();

            propertyAcquisitionFileToDelete.PimsActInstPropAcqFiles.ForEach(s => Context.PimsActInstPropAcqFiles.Remove(s));

            Context.PimsPropertyAcquisitionFiles.Remove(propertyAcquisitionFileToDelete);
        }

        public PimsPropertyAcquisitionFile Update(PimsPropertyAcquisitionFile propertyAcquisitionFile)
        {
            propertyAcquisitionFile.ThrowIfNull(nameof(propertyAcquisitionFile));

            Context.Entry(propertyAcquisitionFile).CurrentValues.SetValues(propertyAcquisitionFile);
            Context.Entry(propertyAcquisitionFile).State = EntityState.Modified;
            return propertyAcquisitionFile;
        }

        #endregion
    }
}
