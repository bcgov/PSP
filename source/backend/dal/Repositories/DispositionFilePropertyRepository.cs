using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
                .Where(x => x.DispositionFileId == dispositionFileId)
                .ToList();
        }

        public int GetDispositionFilePropertyRelatedCount(long propertyId)
        {
            return Context.PimsDispositionFileProperties
                .Where(x => x.PropertyId == propertyId)
                .AsNoTracking()
                .Count();
        }
        #endregion
    }
}
