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
    /// Provides a repository to interact with management file properties within the datasource.
    /// </summary>
    public class ManagementFilePropertyRepository : BaseRepository<PimsManagementFileProperty>, IManagementFilePropertyRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a ManagementFilePropertyRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public ManagementFilePropertyRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<ManagementFilePropertyRepository> logger)
            : base(dbContext, user, logger)
        {
        }

        #endregion

        #region Methods

        public List<PimsManagementFileProperty> GetPropertiesByManagementFileId(long managementFileId)
        {
            return Context.PimsManagementFileProperties
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
                .Where(mp => mp.ManagementFileId == managementFileId)
                .OrderBy(mp => mp.DisplayOrder)
                .ToList();
        }

        public int GetManagementFilePropertyRelatedCount(long propertyId)
        {
            return Context.PimsManagementFileProperties
                .Where(x => x.PropertyId == propertyId)
                .AsNoTracking()
                .Count();
        }

        public PimsManagementFileProperty Add(PimsManagementFileProperty propertyManagementFile)
        {
            propertyManagementFile.ThrowIfNull(nameof(propertyManagementFile));

            if (propertyManagementFile.Property.IsRetired.HasValue && propertyManagementFile.Property.IsRetired.Value)
            {
                throw new BusinessRuleViolationException("Retired property can not be selected.");
            }

            // Mark the property not to be changed if it did not exist already.
            if (propertyManagementFile.PropertyId != 0)
            {
                propertyManagementFile.Property = null;
            }

            Context.PimsManagementFileProperties.Add(propertyManagementFile);
            return propertyManagementFile;
        }

        public PimsManagementFileProperty Update(PimsManagementFileProperty propertyManagementFile)
        {
            propertyManagementFile.ThrowIfNull(nameof(propertyManagementFile));

            Context.Entry(propertyManagementFile).CurrentValues.SetValues(propertyManagementFile);
            Context.Entry(propertyManagementFile).State = EntityState.Modified;
            return propertyManagementFile;
        }

        public void Delete(PimsManagementFileProperty propertyManagementFile)
        {
            propertyManagementFile.ThrowIfNull(nameof(propertyManagementFile));

            var propertyManagementFileToDelete = Context.PimsManagementFileProperties
                .Where(x => x.ManagementFilePropertyId == propertyManagementFile.Internal_Id)
                .FirstOrDefault() ?? throw new KeyNotFoundException();

            Context.PimsManagementFileProperties.Remove(propertyManagementFileToDelete);
        }
        #endregion
    }
}
