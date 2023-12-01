using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Helpers.Extensions;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Provides a repository to interact with disposition files within the datasource.
    /// </summary>
    public class DispositionFileRepository : BaseRepository<PimsDispositionFile>, IDispositionFileRepository
    {
        private const string FILENUMBERPREFIX = "D";
        private const string FILENUMBERSEQUENCETABLE = "dbo.PIMS_DISPOSITION_FILE_NO_SEQ";

        private readonly ISequenceRepository _sequenceRepository;

        #region Constructors

        /// <summary>
        /// Creates a new instance of a DispositionFileRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public DispositionFileRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<DispositionFileRepository> logger, ISequenceRepository sequenceRepository)
            : base(dbContext, user, logger)
        {
            _sequenceRepository = sequenceRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Retrieves the disposition file with the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PimsDispositionFile GetById(long id)
        {
            using var scope = Logger.QueryScope();

            return this.Context.PimsDispositionFiles.AsNoTracking()
                .Include(d => d.DispositionFileStatusTypeCodeNavigation)
                .Include(d => d.DispositionFundingTypeCodeNavigation)
                .Include(d => d.DispositionInitiatingDocTypeCodeNavigation)
                .Include(d => d.DispositionStatusTypeCodeNavigation)
                .Include(d => d.DispositionTypeCodeNavigation)
                .Include(d => d.DspInitiatingBranchTypeCodeNavigation)
                .Include(d => d.RegionCodeNavigation)
                .Include(d => d.DspPhysFileStatusTypeCodeNavigation)
                .Include(d => d.PimsDispositionFileTeams)
                    .ThenInclude(d => d.Organization)
                .Include(d => d.PimsDispositionFileTeams)
                    .ThenInclude(d => d.Person)
                .Include(d => d.PimsDispositionFileTeams)
                    .ThenInclude(d => d.PrimaryContact)
                .Include(d => d.PimsDispositionFileTeams)
                    .ThenInclude(d => d.DspFlTeamProfileTypeCodeNavigation)
                .FirstOrDefault(d => d.DispositionFileId == id) ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Add the new Disposition File to Context.
        /// </summary>
        /// <param name="disposition"></param>
        /// <returns></returns>
        public PimsDispositionFile Add(PimsDispositionFile disposition)
        {
            using var scope = Logger.QueryScope();
            disposition.ThrowIfNull(nameof(disposition));

            disposition.FileNumber = GeneratetDispositionFileNumber();

            Context.PimsDispositionFiles.Add(disposition);

            return disposition;
        }

        /// <summary>
        /// Retrieves the disposition file with the specified id last update information.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public LastUpdatedByModel GetLastUpdateBy(long id)
        {
            // Disposition File
            var lastUpdatedByAggregate = new List<LastUpdatedByModel>();
            var fileLastUpdatedBy = this.Context.PimsDispositionFiles.AsNoTracking()
                .Where(d => d.DispositionFileId == id)
                .Select(d => new LastUpdatedByModel()
                {
                    ParentId = id,
                    AppLastUpdateUserid = d.AppLastUpdateUserid,
                    AppLastUpdateUserGuid = d.AppLastUpdateUserGuid,
                    AppLastUpdateTimestamp = d.AppLastUpdateTimestamp,
                })
                .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
                .Take(1)
                .ToList();
            lastUpdatedByAggregate.AddRange(fileLastUpdatedBy);

            // Disposition Team
            var teamLastUpdatedBy = this.Context.PimsDispositionFileTeams.AsNoTracking()
              .Where(dp => dp.DispositionFileId == id)
              .Select(dp => new LastUpdatedByModel()
              {
                  ParentId = id,
                  AppLastUpdateUserid = dp.AppLastUpdateUserid,
                  AppLastUpdateUserGuid = dp.AppLastUpdateUserGuid,
                  AppLastUpdateTimestamp = dp.AppLastUpdateTimestamp,
              })
              .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
              .Take(1)
              .ToList();
            lastUpdatedByAggregate.AddRange(teamLastUpdatedBy);

            // Disposition Deleted Team
            // This is needed to get the disposition team last-updated-by when deleted
            var teamHistLastUpdatedBy = this.Context.PimsDispositionFileTeamHists.AsNoTracking()
              .Where(dph => dph.DispositionFileId == id)
              .Select(dph => new LastUpdatedByModel()
              {
                  ParentId = id,
                  AppLastUpdateUserid = dph.AppLastUpdateUserid, // TODO: Update this once the DB tracks the user
                  AppLastUpdateUserGuid = dph.AppLastUpdateUserGuid, // TODO: Update this once the DB tracks the user
                  AppLastUpdateTimestamp = dph.EndDateHist ?? DateTime.UnixEpoch,
              })
              .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
              .Take(1)
              .ToList();
            lastUpdatedByAggregate.AddRange(teamHistLastUpdatedBy);

            // Disposition Properties
            var propertiesLastUpdatedBy = this.Context.PimsPropertyDispositionFiles.AsNoTracking()
                .Where(dp => dp.DispositionFileId == id)
                .Select(dp => new LastUpdatedByModel()
                {
                    ParentId = id,
                    AppLastUpdateUserid = dp.AppLastUpdateUserid,
                    AppLastUpdateUserGuid = dp.AppLastUpdateUserGuid,
                    AppLastUpdateTimestamp = dp.AppLastUpdateTimestamp,
                })
                .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
                .Take(1)
                .ToList();
            lastUpdatedByAggregate.AddRange(propertiesLastUpdatedBy);

            // Disposition Deleted Properties
            // This is needed to get the notes last-updated-by from the notes that where deleted
            var propertiesHistoryLastUpdatedBy = this.Context.PimsPropertyDispositionFileHists.AsNoTracking()
            .Where(dph => dph.DispositionFileId == id)
            .Select(dph => new LastUpdatedByModel()
            {
                ParentId = id,
                AppLastUpdateUserid = dph.AppLastUpdateUserid, // TODO: Update this once the DB tracks the user
                AppLastUpdateUserGuid = dph.AppLastUpdateUserGuid, // TODO: Update this once the DB tracks the user
                AppLastUpdateTimestamp = dph.EndDateHist ?? DateTime.UnixEpoch,
            })
            .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
            .Take(1)
            .ToList();
            lastUpdatedByAggregate.AddRange(propertiesHistoryLastUpdatedBy);

            return lastUpdatedByAggregate.OrderByDescending(x => x.AppLastUpdateTimestamp).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves the version of the disposition file with the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The file row version.</returns>
        public long GetRowVersion(long id)
        {
            using var scope = Logger.QueryScope();

            var result = this.Context.PimsDispositionFiles.AsNoTracking()
                .Where(p => p.DispositionFileId == id)?
                .FirstOrDefault() ?? throw new KeyNotFoundException();
            return result.ConcurrencyControlNumber;
        }

        /// <summary>
        /// Get the next available value from PIMS_DISPOSITION_FILE_NO_SEQ.
        /// </summary>
        /// <returns>The next value for the sequence.</returns>
        private string GeneratetDispositionFileNumber()
        {
            int fileNumberSequence = (int)_sequenceRepository.GetNextSequenceValue(FILENUMBERSEQUENCETABLE);

            return $"{FILENUMBERPREFIX}-{fileNumberSequence}";
        }
        #endregion
    }
}
