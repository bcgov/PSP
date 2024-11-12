using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Exceptions;
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
                .Include(d => d.Project)
                .Include(d => d.Product)
                .Include(d => d.DispositionFundingTypeCodeNavigation)
                .Include(d => d.DispositionInitiatingDocTypeCodeNavigation)
                .Include(d => d.DispositionStatusTypeCodeNavigation)
                .Include(d => d.DispositionTypeCodeNavigation)
                .Include(d => d.DspInitiatingBranchTypeCodeNavigation)
                .Include(d => d.RegionCodeNavigation)
                .Include(d => d.DspPhysFileStatusTypeCodeNavigation)
                .Include(d => d.PimsDispositionSales)
                .Include(d => d.PimsDispositionAppraisals)
                .Include(d => d.PimsDispositionFileProperties)
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

            if (disposition.PimsDispositionFileProperties.Any(x => x.Property != null && x.Property.IsRetired.HasValue && x.Property.IsRetired.Value))
            {
                throw new BusinessRuleViolationException("Retired property can not be selected.");
            }

            // Existing properties should not be added.
            foreach (var dispositionProperty in disposition.PimsDispositionFileProperties)
            {
                if (dispositionProperty.Property.Internal_Id != 0)
                {
                    dispositionProperty.Property = null;
                }
            }

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
            var deletedTeams = this.Context.PimsDispositionFileTeamHists.AsNoTracking()
               .Where(aph => aph.DispositionFileId == id)
               .GroupBy(aph => aph.DispositionFileTeamId)
               .Select(gaph => gaph.OrderByDescending(a => a.EffectiveDateHist).FirstOrDefault()).ToList();

            var teamHistLastUpdatedBy = deletedTeams
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
            var deletedProperties = this.Context.PimsDispositionFilePropertyHists.AsNoTracking()
               .Where(aph => aph.DispositionFileId == id)
               .GroupBy(aph => aph.DispositionFilePropertyId)
               .Select(gaph => gaph.OrderByDescending(a => a.EffectiveDateHist).FirstOrDefault()).ToList();

            var propertiesHistoryLastUpdatedBy = deletedProperties
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

            // Disposition Sales
            var salesLastUpdatedBy = this.Context.PimsDispositionSales.AsNoTracking()
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
            lastUpdatedByAggregate.AddRange(salesLastUpdatedBy);

            // Disposition Deleted Sales
            var salesHistoryLastUpdatedBy = Context.PimsDispositionSaleHists.AsNoTracking()
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
            lastUpdatedByAggregate.AddRange(salesHistoryLastUpdatedBy);

            // Disposition Offers
            var offerLastUpdatedBy = this.Context.PimsDispositionOffers.AsNoTracking()
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
            lastUpdatedByAggregate.AddRange(offerLastUpdatedBy);

            // Disposition Deleted Offers
            var offerHistoryLastUpdatedBy = Context.PimsDispositionOfferHists.AsNoTracking()
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
            lastUpdatedByAggregate.AddRange(offerHistoryLastUpdatedBy);

            // Disposition Values
            var valueLastUpdatedBy = this.Context.PimsDispositionAppraisals.AsNoTracking()
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
            lastUpdatedByAggregate.AddRange(valueLastUpdatedBy);

            // Disposition Deleted Values
            var valueHistoryLastUpdatedBy = Context.PimsDispositionAppraisalHists.AsNoTracking()
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
            lastUpdatedByAggregate.AddRange(valueHistoryLastUpdatedBy);

            // Disposition Checklist
            var checklistLastUpdatedBy = this.Context.PimsDispositionChecklistItems.AsNoTracking()
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
            lastUpdatedByAggregate.AddRange(checklistLastUpdatedBy);

            // Disposition Deleted Checklists
            var checklistHistoryLastUpdatedBy = Context.PimsDispositionChecklistItemHists.AsNoTracking()
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
            lastUpdatedByAggregate.AddRange(checklistHistoryLastUpdatedBy);

            // Disposition Document
            var documentLastUpdatedBy = this.Context.PimsDispositionFileDocuments.AsNoTracking()
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
            lastUpdatedByAggregate.AddRange(documentLastUpdatedBy);

            // Disposition Deleted Documents
            var documentHistoryLastUpdatedBy = Context.PimsDispositionFileDocumentHists.AsNoTracking()
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
            lastUpdatedByAggregate.AddRange(documentHistoryLastUpdatedBy);

            // Disposition Notes
            var notesLastUpdatedBy = this.Context.PimsDispositionFileNotes.AsNoTracking()
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
            lastUpdatedByAggregate.AddRange(notesLastUpdatedBy);

            // Disposition Deleted Notes
            var notesHistoryLastUpdatedBy = Context.PimsDispositionFileNoteHists.AsNoTracking()
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
            lastUpdatedByAggregate.AddRange(notesHistoryLastUpdatedBy);

            return lastUpdatedByAggregate.OrderByDescending(x => x.AppLastUpdateTimestamp).FirstOrDefault();
        }

        public PimsDispositionFile Update(long dispositionFileId, PimsDispositionFile dispositionFile)
        {
            using var scope = Logger.QueryScope();
            dispositionFile.ThrowIfNull(nameof(dispositionFile));

            var existingFile = Context.PimsDispositionFiles
                .FirstOrDefault(x => x.DispositionFileId == dispositionFileId) ?? throw new KeyNotFoundException();

            dispositionFile.FileNumber = existingFile.FileNumber;

            Context.Entry(existingFile).CurrentValues.SetValues(dispositionFile);
            Context.UpdateChild<PimsDispositionFile, long, PimsDispositionFileTeam, long>(p => p.PimsDispositionFileTeams, dispositionFile.Internal_Id, dispositionFile.PimsDispositionFileTeams.ToArray());

            return existingFile;
        }

        /// <summary>
        /// Retrieves a page with an array of disposition files within the specified filters.
        /// Note that the 'filter' will control the 'page' and 'quantity'.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="contractorPersonId"></param>
        /// <returns></returns>
        public Paged<PimsDispositionFile> GetPageDeep(DispositionFilter filter, long? contractorPersonId = null)
        {
            using var scope = Logger.QueryScope();

            filter.ThrowIfNull(nameof(filter));
            if (!filter.IsValid())
            {
                throw new ArgumentException("Argument must have a valid filter", nameof(filter));
            }

            var query = GetCommonDispositionFileQueryDeep(filter, contractorPersonId);

            var skip = (filter.Page - 1) * filter.Quantity;
            var pageItems = query.Skip(skip).Take(filter.Quantity).ToList();

            return new Paged<PimsDispositionFile>(pageItems, filter.Page, filter.Quantity, query.Count());
        }

        /// <summary>
        /// Retrieves the region of the disposition file with the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The file region.</returns>
        public short GetRegion(long id)
        {
            using var scope = Logger.QueryScope();

            return this.Context.PimsDispositionFiles.AsNoTracking()
                .Where(p => p.DispositionFileId == id)?
                .Select(p => p.RegionCode)?
                .FirstOrDefault() ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Retrieves the version of the disposition file with the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The file row version.</returns>
        public long? GetRowVersion(long id)
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
                .Include(x => x.DspPurchAgent)
                    .ThenInclude(y => y.Person)
                .Include(x => x.DspPurchAgent)
                    .ThenInclude(y => y.Organization)
                .Include(x => x.DspPurchAgent)
                    .ThenInclude(y => y.PrimaryContact)
                .Include(x => x.DspPurchSolicitor)
                    .ThenInclude(y => y.Person)
                .Include(x => x.DspPurchSolicitor)
                    .ThenInclude(y => y.Organization)
                .Include(x => x.DspPurchSolicitor)
                    .ThenInclude(y => y.PrimaryContact)
                .Where(x => x.DispositionFileId == dispositionId).FirstOrDefault();
        }

        public PimsDispositionSale AddDispositionFileSale(PimsDispositionSale dispositionSale)
        {
            Context.PimsDispositionSales.Add(dispositionSale);

            return dispositionSale;
        }

        public PimsDispositionSale UpdateDispositionFileSale(PimsDispositionSale dispositionSale)
        {
            var existingSale = Context.PimsDispositionSales
                .Include(x => x.PimsDispositionPurchasers)
                .Include(x => x.DspPurchAgent)
                .Include(x => x.DspPurchSolicitor)
                .FirstOrDefault(x => x.DispositionSaleId.Equals(dispositionSale.DispositionSaleId)) ?? throw new KeyNotFoundException();

            if (existingSale.DspPurchAgent != null && dispositionSale.DspPurchAgent == null)
            {
                Context.Remove(existingSale.DspPurchAgent);
                dispositionSale.DspPurchAgentId = null;
            }
            else if (existingSale.DspPurchAgent != null && dispositionSale.DspPurchAgentId.HasValue && dispositionSale.DspPurchAgent != null)
            {
                Context.Entry(existingSale.DspPurchAgent).CurrentValues.SetValues(dispositionSale.DspPurchAgent);
            }
            else if (existingSale.DspPurchAgent == null && dispositionSale.DspPurchAgent != null)
            {
                Context.PimsDspPurchAgents.Add(dispositionSale.DspPurchAgent);
                Context.SaveChanges();

                dispositionSale.DspPurchAgentId = dispositionSale.DspPurchAgent.DspPurchAgentId;
            }

            if (existingSale.DspPurchSolicitor != null && dispositionSale.DspPurchSolicitor == null)
            {
                Context.Remove(existingSale.DspPurchSolicitor);
                dispositionSale.DspPurchSolicitorId = null;
            }
            else if (existingSale.DspPurchSolicitor != null && dispositionSale.DspPurchSolicitorId.HasValue && dispositionSale.DspPurchSolicitor != null)
            {
                Context.Entry(existingSale.DspPurchSolicitor).CurrentValues.SetValues(dispositionSale.DspPurchSolicitor);
            }
            else if (existingSale.DspPurchSolicitor == null && dispositionSale.DspPurchSolicitor != null)
            {
                Context.PimsDspPurchSolicitors.Add(dispositionSale.DspPurchSolicitor);
                Context.SaveChanges();

                dispositionSale.DspPurchSolicitorId = dispositionSale.DspPurchSolicitor.DspPurchSolicitorId;
            }

            Context.Entry(existingSale).CurrentValues.SetValues(dispositionSale);
            Context.UpdateChild<PimsDispositionSale, long, PimsDispositionPurchaser, long>(p => p.PimsDispositionPurchasers, dispositionSale.Internal_Id, dispositionSale.PimsDispositionPurchasers.ToArray());

            return existingSale;
        }

        public PimsDispositionAppraisal GetDispositionFileAppraisal(long dispositionId)
        {
            return Context.PimsDispositionAppraisals.AsNoTracking()
                    .Where(x => x.DispositionFileId == dispositionId).FirstOrDefault();
        }

        public PimsDispositionAppraisal AddDispositionFileAppraisal(PimsDispositionAppraisal dispositionAppraisal)
        {
            Context.PimsDispositionAppraisals.Add(dispositionAppraisal);

            return dispositionAppraisal;
        }

        public PimsDispositionAppraisal UpdateDispositionFileAppraisal(long id, PimsDispositionAppraisal dispositionAppraisal)
        {
            var existingAppraisal = Context.PimsDispositionAppraisals
                .FirstOrDefault(x => x.DispositionAppraisalId.Equals(id)) ?? throw new KeyNotFoundException();

            Context.Entry(existingAppraisal).CurrentValues.SetValues(dispositionAppraisal);

            return existingAppraisal;
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

        /// <summary>
        /// Generate a Common IQueryable for Disposition Files.
        /// </summary>
        /// <param name="filter">The filter to apply.</param>
        /// <param name="contractorPersonId">Filter for Contractors.</param>
        /// <returns></returns>
        private IQueryable<PimsDispositionFile> GetCommonDispositionFileQueryDeep(DispositionFilter filter, long? contractorPersonId = null)
        {
            filter.FileNameOrNumberOrReference = Regex.Replace(filter.FileNameOrNumberOrReference ?? string.Empty, @"^[d,D]-", string.Empty);
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

            if (contractorPersonId is not null)
            {
                predicate = predicate.And(x => x.PimsDispositionFileTeams.Any(x => x.PersonId == contractorPersonId));
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
                .Include(p => p.Project)
                .Where(predicate);

            // As per Confluence - default sort to show chronological, newest first; based upon File Assigned Date
            query = (filter.Sort?.Any() == true) ? query.OrderByProperty(true, filter.Sort) : query.OrderByDescending(disp => disp.AssignedDt ?? DateOnly.FromDateTime(DateTime.MinValue));

            return query;
        }
        #endregion
    }
}
