using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using LinqKit;
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
                .Include(d => d.PimsDispositionSales)
                .Include(d => d.PimsDispositionAppraisals)
                .Include(d => d.PimsDispositionOffers)
                    .ThenInclude(o => o.DispositionOfferStatusTypeCodeNavigation)
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

            disposition.FileNumber = _sequenceRepository.GetNextSequenceValue(FILENUMBERSEQUENCETABLE).ToString();

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
            var propertiesLastUpdatedBy = this.Context.PimsDispositionFileProperties.AsNoTracking()
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
            var propertiesHistoryLastUpdatedBy = Context.PimsDispositionFilePropertyHists.AsNoTracking()
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
        /// Retrieves a page with an array of disposition files within the specified filters.
        /// Note that the 'filter' will control the 'page' and 'quantity'.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Paged<PimsDispositionFile> GetPageDeep(DispositionFilter filter)
        {
            using var scope = Logger.QueryScope();

            filter.ThrowIfNull(nameof(filter));
            if (!filter.IsValid())
            {
                throw new ArgumentException("Argument must have a valid filter", nameof(filter));
            }

            var query = GetCommonDispositionFileQueryDeep(filter);

            var skip = (filter.Page - 1) * filter.Quantity;
            var pageItems = query.Skip(skip).Take(filter.Quantity).ToList();

            return new Paged<PimsDispositionFile>(pageItems, filter.Page, filter.Quantity, query.Count());
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

        public List<PimsDispositionFileTeam> GetTeamMembers()
        {
            return Context.PimsDispositionFileTeams.AsNoTracking()
                .Include(x => x.DispositionFile)
                .Include(x => x.Person)
                .Include(x => x.Organization)
                .ToList();
        }

        public List<PimsDispositionOffer> GetDispositionOffers(long dispositionId)
        {
            return Context.PimsDispositionOffers.AsNoTracking()
                .Include(x => x.DispositionOfferStatusTypeCodeNavigation)
                .Where(x => x.DispositionFileId == dispositionId).ToList();
        }

        public PimsDispositionOffer GetDispositionOfferById(long dispositionId, long dispositionOfferId)
        {
            return Context.PimsDispositionOffers.AsNoTracking()
                .Where(x => x.DispositionOfferId == dispositionOfferId && x.DispositionFileId == dispositionId)
                .FirstOrDefault() ?? throw new KeyNotFoundException();
        }

        public PimsDispositionOffer AddDispositionOffer(PimsDispositionOffer dispositionOffer)
        {
            Context.PimsDispositionOffers.Add(dispositionOffer);

            return dispositionOffer;
        }

        public PimsDispositionOffer UpdateDispositionOffer(PimsDispositionOffer dispositionOffer)
        {
            var existingOffer = Context.PimsDispositionOffers
                .FirstOrDefault(x => x.DispositionOfferId.Equals(dispositionOffer.DispositionOfferId)) ?? throw new KeyNotFoundException();

            Context.Entry(existingOffer).CurrentValues.SetValues(dispositionOffer);

            return existingOffer;
        }

        public bool TryDeleteDispositionOffer(long dispositionId, long dispositionOfferId)
        {
            var deletedEntity = Context.PimsDispositionOffers.Where(x => x.DispositionFileId == dispositionId && x.DispositionOfferId == dispositionOfferId).FirstOrDefault();

            if (deletedEntity is not null)
            {
                Context.PimsDispositionOffers.Remove(deletedEntity);

                return true;
            }

            return false;
        }

        public PimsDispositionSale GetDispositionFileSale(long dispositionId)
        {
            return Context.PimsDispositionSales.AsNoTracking()
                .Include(x => x.PimsDispositionPurchasers)
                    .ThenInclude(y => y.Person)
                .Include(x => x.PimsDispositionPurchasers)
                    .ThenInclude(y => y.Organization)
                .Include(x => x.PimsDispositionPurchasers)
                    .ThenInclude(y => y.PrimaryContact)
                .Include(x => x.PimsDspPurchAgents)
                    .ThenInclude(y => y.Person)
                .Include(x => x.PimsDspPurchAgents)
                    .ThenInclude(y => y.Organization)
                .Include(x => x.PimsDspPurchAgents)
                    .ThenInclude(y => y.PrimaryContact)
                .Include(x => x.PimsDspPurchSolicitors)
                    .ThenInclude(y => y.Person)
                .Include(x => x.PimsDspPurchSolicitors)
                    .ThenInclude(y => y.Organization)
                .Include(x => x.PimsDspPurchSolicitors)
                    .ThenInclude(y => y.PrimaryContact)
                .Where(x => x.DispositionFileId == dispositionId).FirstOrDefault();
        }

        /// <summary>
        /// Generate a Common IQueryable for Disposition Files.
        /// </summary>
        /// <param name="filter">The filter to apply.</param>
        /// <returns></returns>
        private IQueryable<PimsDispositionFile> GetCommonDispositionFileQueryDeep(DispositionFilter filter)
        {
            var predicate = PredicateBuilder.New<PimsDispositionFile>(disp => true);
            if (!string.IsNullOrWhiteSpace(filter.Pid))
            {
                var pidValue = filter.Pid.Replace("-", string.Empty).Trim().TrimStart('0');
                predicate = predicate.And(disp => disp.PimsDispositionFileProperties.Any(pd => pd != null && EF.Functions.Like(pd.Property.Pid.ToString(), $"%{pidValue}%")));
            }

            if (!string.IsNullOrWhiteSpace(filter.Pin))
            {
                var pinValue = filter.Pin.Replace("-", string.Empty).Trim().TrimStart('0');
                predicate = predicate.And(acq => acq.PimsDispositionFileProperties.Any(pd => pd != null && EF.Functions.Like(pd.Property.Pin.ToString(), $"%{pinValue}%")));
            }

            if (!string.IsNullOrWhiteSpace(filter.Address))
            {
                predicate = predicate.And(disp => disp.PimsDispositionFileProperties.Any(pd => pd != null &&
                    (EF.Functions.Like(pd.Property.Address.StreetAddress1, $"%{filter.Address}%") ||
                    EF.Functions.Like(pd.Property.Address.StreetAddress2, $"%{filter.Address}%") ||
                    EF.Functions.Like(pd.Property.Address.StreetAddress3, $"%{filter.Address}%") ||
                    EF.Functions.Like(pd.Property.Address.MunicipalityName, $"%{filter.Address}%"))));
            }

            if (!string.IsNullOrWhiteSpace(filter.FileNameOrNumberOrReference))
            {
                predicate = predicate.And(r => EF.Functions.Like(r.FileName, $"%{filter.FileNameOrNumberOrReference}%")
                || EF.Functions.Like(r.FileNumber, $"%{filter.FileNameOrNumberOrReference}%")
                || EF.Functions.Like(r.FileReference, $"%{filter.FileNameOrNumberOrReference}%"));
            }

            // filter by various statuses
            if (!string.IsNullOrWhiteSpace(filter.DispositionFileStatusCode))
            {
                predicate = predicate.And(disp => disp.DispositionFileStatusTypeCode == filter.DispositionFileStatusCode);
            }

            if (!string.IsNullOrWhiteSpace(filter.DispositionStatusCode))
            {
                predicate = predicate.And(disp => disp.DispositionStatusTypeCode == filter.DispositionStatusCode);
            }

            if (!string.IsNullOrWhiteSpace(filter.DispositionTypeCode))
            {
                predicate = predicate.And(disp => disp.DispositionTypeCode == filter.DispositionTypeCode);
            }

            // filter by team members
            if (filter.TeamMemberPersonId.HasValue)
            {
                predicate = predicate.And(disp => disp.PimsDispositionFileTeams.Any(x => x.PersonId == filter.TeamMemberPersonId.Value));
            }

            if (filter.TeamMemberOrganizationId.HasValue)
            {
                predicate = predicate.And(disp => disp.PimsDispositionFileTeams.Any(x => x.OrganizationId == filter.TeamMemberOrganizationId.Value));
            }

            var query = Context.PimsDispositionFiles.AsNoTracking()
                .Include(d => d.RegionCodeNavigation)
                .Include(d => d.DspPhysFileStatusTypeCodeNavigation)
                .Include(d => d.DispositionFileStatusTypeCodeNavigation)
                .Include(d => d.DispositionStatusTypeCodeNavigation)
                .Include(d => d.DispositionTypeCodeNavigation)
                .Include(d => d.DispositionFundingTypeCodeNavigation)
                .Include(d => d.DispositionInitiatingDocTypeCodeNavigation)
                .Include(d => d.PimsDispositionAppraisals)
                .Include(d => d.PimsDispositionOffers)
                .Include(d => d.PimsDispositionSales)
                    .ThenInclude(s => s.PimsDispositionPurchasers)
                        .ThenInclude(p => p.Person)
                .Include(d => d.PimsDispositionSales)
                    .ThenInclude(s => s.PimsDispositionPurchasers)
                        .ThenInclude(p => p.Organization)
                .Include(d => d.PimsDispositionSales)
                    .ThenInclude(s => s.PimsDispositionPurchasers)
                        .ThenInclude(p => p.PrimaryContact)
                .Include(d => d.PimsDispositionFileTeams)
                    .ThenInclude(c => c.Person)
                .Include(d => d.PimsDispositionFileTeams)
                    .ThenInclude(c => c.Organization)
                .Include(d => d.PimsDispositionFileTeams)
                    .ThenInclude(c => c.PrimaryContact)
                .Include(tm => tm.PimsDispositionFileTeams)
                    .ThenInclude(c => c.DspFlTeamProfileTypeCodeNavigation)
                .Include(fp => fp.PimsDispositionFileProperties)
                    .ThenInclude(prop => prop.Property)
                    .ThenInclude(ad => ad.Address)
                    .ThenInclude(x => x.ProvinceState)
                .Where(predicate);

            // As per Confluence - default sort to show chronological, newest first; based upon File Assigned Date
            query = (filter.Sort?.Any() == true) ? query.OrderByProperty(filter.Sort) : query.OrderByDescending(disp => disp.AssignedDt ?? DateOnly.FromDateTime(DateTime.MinValue));

            return query;
        }

        /// <summary>
        /// Get Disposition Files for Export.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<PimsDispositionFile> GetDispositionFileExportDeep(DispositionFilter filter)
        {
            // RECOMMENDED - use a log scope to group all potential SQL statements generated by EF for this method call
            using var scope = Logger.QueryScope();

            filter.ThrowIfNull(nameof(filter));
            if (!filter.IsValid())
            {
                throw new ArgumentException("Argument must have a valid filter", nameof(filter));
            }

            return GetCommonDispositionFileQueryDeep(filter).ToList();
        }

        #endregion
    }
}
