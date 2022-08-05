using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Provides a repository to interact with acquisition files within the datasource.
    /// </summary>
    public class AcquisitionFileRepository : BaseRepository<PimsAcquisitionFile>, IAcquisitionFileRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a AcquisitionFileRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public AcquisitionFileRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<AcquisitionFileRepository> logger, IMapper mapper)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Retrieves the acquisition file with the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PimsAcquisitionFile GetById(long id)
        {
            return this.Context.PimsAcquisitionFiles.AsNoTracking()
                .Include(r => r.AcquisitionFileStatusTypeCodeNavigation)
                .Include(r => r.AcqPhysFileStatusTypeCodeNavigation)
                .Include(r => r.AcquisitionTypeCodeNavigation)
                .Include(r => r.RegionCodeNavigation)
                .FirstOrDefault(x => x.AcquisitionFileId == id) ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Adds the specified acquisition file to the datasource.
        /// </summary>
        /// <param name="acquisitionFile"></param>
        /// <returns></returns>
        public PimsAcquisitionFile Add(PimsAcquisitionFile acquisitionFile)
        {
            acquisitionFile.ThrowIfNull(nameof(acquisitionFile));
            this.Context.PimsAcquisitionFiles.Add(acquisitionFile);
            return acquisitionFile;
        }

        /// <summary>
        /// Updates the specified acquisition file.
        /// </summary>
        /// <param name="acquisitionFile"></param>
        /// <returns></returns>
        public PimsAcquisitionFile Update(PimsAcquisitionFile acquisitionFile)
        {
            acquisitionFile.ThrowIfNull(nameof(acquisitionFile));

            var existingAcqFile = this.Context.PimsAcquisitionFiles
                .FirstOrDefault(x => x.AcquisitionFileId == acquisitionFile.Id) ?? throw new KeyNotFoundException();

            // TODO: Implementation pending
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Retrieves the version of the acquisition file with the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public long GetRowVersion(long id)
        {
            return this.Context.PimsAcquisitionFiles.AsNoTracking()
                .Where(p => p.AcquisitionFileId == id)?
                .Select(p => p.ConcurrencyControlNumber)?
                .FirstOrDefault() ?? throw new KeyNotFoundException();
        }

        #endregion
    }
}
