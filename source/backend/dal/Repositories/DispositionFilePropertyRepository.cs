using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Exceptions;
using Pims.Core.Extensions;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Provides a repository to interact with disposition file properties within the datasource.
    /// </summary>
    public class DispositionFilePropertyRepository : BaseRepository<PimsDispositionFileProperty>, IDispositionFilePropertyRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a DispositionFilePropertyRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public DispositionFilePropertyRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<DispositionFilePropertyRepository> logger)
            : base(dbContext, user, logger)
        {
        }

        #endregion

        #region Methods

        public List<PimsDispositionFileProperty> GetPropertiesByDispositionFileId(long dispositionFileId)
        {
            return Context.PimsDispositionFileProperties
                .AsNoTracking()
                .Include(rp => rp.Property)
                    .ThenInclude(rp => rp.RegionCodeNavigation)
                .Include(rp => rp.Property)
                    .ThenInclude(rp => rp.DistrictCodeNavigation)
                .Include(rp => rp.Property)
                    .ThenInclude(rp => rp.Address)
                    .ThenInclude(a => a.Country)
                .Include(rp => rp.Property)
                    .ThenInclude(rp => rp.Address)
                    .ThenInclude(a => a.ProvinceState)
                .Where(dp => dp.DispositionFileId == dispositionFileId)
                .OrderBy(dp => dp.DisplayOrder)
                .ToList();
        }

        public int GetDispositionFilePropertyRelatedCount(long propertyId)
        {
            return Context.PimsDispositionFileProperties
                .Where(x => x.PropertyId == propertyId)
                .AsNoTracking()
                .Count();
        }

        public PimsDispositionFileProperty Add(PimsDispositionFileProperty propertyDispositionFile)
        {
            propertyDispositionFile.ThrowIfNull(nameof(propertyDispositionFile));

            if (propertyDispositionFile.Property.IsRetired.HasValue && propertyDispositionFile.Property.IsRetired.Value)
            {
                throw new BusinessRuleViolationException("Retired property can not be selected.");
            }

            // Mark the property not to be changed if it did not exist already.
            if (propertyDispositionFile.PropertyId != 0)
            {
                propertyDispositionFile.Property = null;
            }

            Context.PimsDispositionFileProperties.Add(propertyDispositionFile);
            return propertyDispositionFile;
        }

        public PimsDispositionFileProperty Update(PimsDispositionFileProperty propertyDispositionFile)
        {
            propertyDispositionFile.ThrowIfNull(nameof(propertyDispositionFile));

            Context.Entry(propertyDispositionFile).CurrentValues.SetValues(propertyDispositionFile);
            Context.Entry(propertyDispositionFile).State = EntityState.Modified;
            return propertyDispositionFile;
        }

        public void Delete(PimsDispositionFileProperty propertyDispositionFile)
        {
            propertyDispositionFile.ThrowIfNull(nameof(propertyDispositionFile));

            var propertyDispositionFileToDelete = Context.PimsDispositionFileProperties
                .Where(x => x.DispositionFilePropertyId == propertyDispositionFile.Internal_Id)
                .FirstOrDefault() ?? throw new KeyNotFoundException();

            Context.PimsDispositionFileProperties.Remove(propertyDispositionFileToDelete);
        }
        #endregion
    }
}
