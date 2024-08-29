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
    /// Provides a repository to interact with research files within the datasource.
    /// </summary>
    public class ResearchFileRepository : BaseRepository<PimsResearchFile>, IResearchFileRepository
    {
        private readonly ISequenceRepository _sequenceRepository;

        #region Constructors

        /// <summary>
        /// Creates a new instance of a ResearchFileRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public ResearchFileRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<ResearchFileRepository> logger, ISequenceRepository sequenceRepository)
            : base(dbContext, user, logger)
        {
            _sequenceRepository = sequenceRepository;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Retrieves the research file with the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PimsResearchFile GetById(long id)
        {
            return this.Context.PimsResearchFiles.AsNoTracking()
                .Where(x => x.ResearchFileId == id)
                .Include(r => r.ResearchFileStatusTypeCodeNavigation)
                .Include(r => r.RequestSourceTypeCodeNavigation)
                .Include(r => r.RequestorNameNavigation)
                    .ThenInclude(p => p.PimsPersonOrganizations)
                    .ThenInclude(o => o.Organization)
                .Include(r => r.RequestorOrganizationNavigation)
                .Include(r => r.PimsPropertyResearchFiles)
                    .ThenInclude(rp => rp.PimsPrfPropResearchPurposeTypes)
                    .ThenInclude(p => p.PropResearchPurposeTypeCodeNavigation)
                .Include(r => r.PimsResearchFilePurposes)
                    .ThenInclude(rp => rp.ResearchPurposeTypeCodeNavigation)
                .Include(r => r.PimsResearchFileProjects)
                    .ThenInclude(rp => rp.Project)
                .FirstOrDefault() ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Add the specified research file to the datasource.
        /// </summary>
        /// <param name="researchFile"></param>
        /// <returns></returns>
        public PimsResearchFile Add(PimsResearchFile researchFile)
        {
            researchFile.ThrowIfNull(nameof(researchFile));

            // Existing properties should not be added.
            foreach (var researchProperty in researchFile.PimsPropertyResearchFiles)
            {
                if (researchProperty.Property.PropertyId != 0)
                {
                    researchProperty.Property = null;
                }
            }

            long nextResearchFileId = this.GetNextResearchSequenceValue();

            researchFile.RfileNumber = GenerateRFileNumber(nextResearchFileId);

            researchFile.ResearchFileId = nextResearchFileId;
            this.Context.PimsResearchFiles.Add(researchFile);
            return researchFile;
        }

        /// <summary>
        /// Update the specified research file.
        /// </summary>
        /// <param name="researchFile"></param>
        /// <returns></returns>
        public PimsResearchFile Update(PimsResearchFile researchFile)
        {
            researchFile.ThrowIfNull(nameof(researchFile));

            var existingResearchFile = Context.PimsResearchFiles
                .FirstOrDefault(x => x.ResearchFileId == researchFile.Internal_Id) ?? throw new KeyNotFoundException();

            var currentPurposes = Context.PimsResearchFiles
                .SelectMany(x => x.PimsResearchFilePurposes)
                .Where(x => x.ResearchFileId == researchFile.Internal_Id)
                .AsNoTracking()
                .ToList();

            List<PimsResearchFilePurpose> purposes = new List<PimsResearchFilePurpose>();

            foreach (var selectedPurpose in researchFile.PimsResearchFilePurposes)
            {
                var currentPurpose = currentPurposes.FirstOrDefault(x => x.ResearchPurposeTypeCode == selectedPurpose.ResearchPurposeTypeCode);

                // If the code is already on the list, add the existing one, otherwise add the incoming one
                if (currentPurpose != null)
                {
                    purposes.Add(currentPurpose);
                    Context.Entry(currentPurpose).State = EntityState.Unchanged;
                }
                else
                {
                    purposes.Add(selectedPurpose);
                    Context.Entry(selectedPurpose).State = EntityState.Added;
                }
            }

            // The ones not on the new set should be deleted
            List<PimsResearchFilePurpose> differenceSet = currentPurposes.Where(x => !purposes.Any(y => y.ResearchPurposeTypeCode == x.ResearchPurposeTypeCode)).ToList();
            foreach (var deletedPurpose in differenceSet)
            {
                purposes.Add(deletedPurpose);
                Context.Entry(deletedPurpose).State = EntityState.Deleted;
            }

            existingResearchFile.PimsResearchFilePurposes = purposes;

            Context.Entry(existingResearchFile).CurrentValues.SetValues(researchFile);
            Context.UpdateChild<PimsResearchFile, long, PimsResearchFileProject, long>(p => p.PimsResearchFileProjects, researchFile.Internal_Id, researchFile.PimsResearchFileProjects.ToArray());

            return researchFile;
        }

        /// <summary>
        /// Retrieves the research file with the specified id last update information.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public LastUpdatedByModel GetLastUpdateBy(long id)
        {
            var lastUpdatedByAggregate = new List<LastUpdatedByModel>();
            var fileLastUpdatedBy = this.Context.PimsResearchFiles.AsNoTracking()
                .Where(r => r.ResearchFileId == id)
                .Select(r => new LastUpdatedByModel()
                {
                    ParentId = id,
                    AppLastUpdateUserid = r.AppLastUpdateUserid,
                    AppLastUpdateUserGuid = r.AppLastUpdateUserGuid,
                    AppLastUpdateTimestamp = r.AppLastUpdateTimestamp,
                })
                .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
                .Take(1)
                .ToList();
            lastUpdatedByAggregate.AddRange(fileLastUpdatedBy);

            var documentsLastUpdatedBy = this.Context.PimsResearchFileDocuments.AsNoTracking()
                .Where(rd => rd.ResearchFileId == id)
                .Include(rd => rd.Document)
                .Select(rd => new LastUpdatedByModel()
                {
                    ParentId = id,
                    AppLastUpdateUserid = rd.Document.AppLastUpdateUserid,
                    AppLastUpdateUserGuid = rd.Document.AppLastUpdateUserGuid,
                    AppLastUpdateTimestamp = rd.Document.AppLastUpdateTimestamp,
                })
                .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
                .Take(1)
                .ToList();
            lastUpdatedByAggregate.AddRange(documentsLastUpdatedBy);

            // This is needed to get the document last-updated-by from the document that where deleted
            var deletedDocuments = this.Context.PimsResearchFileDocumentHists.AsNoTracking()
                .Where(rdh => rdh.ResearchFileId == id)
                .GroupBy(rdh => rdh.ResearchFileDocumentId)
                .Select(grdh => grdh.OrderByDescending(a => a.EffectiveDateHist).FirstOrDefault()).ToList();

            var documentsHistoryLastUpdatedBy = deletedDocuments
            .Select(rdh => new LastUpdatedByModel()
            {
                ParentId = id,
                AppLastUpdateUserid = rdh.AppLastUpdateUserid, // TODO: Update this once the DB tracks the user
                AppLastUpdateUserGuid = rdh.AppLastUpdateUserGuid, // TODO: Update this once the DB tracks the user
                AppLastUpdateTimestamp = rdh.EndDateHist ?? DateTime.UnixEpoch,
            })
            .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
            .Take(1)
            .ToList();
            lastUpdatedByAggregate.AddRange(documentsHistoryLastUpdatedBy);

            var notesLastUpdatedBy = this.Context.PimsResearchFileNotes.AsNoTracking()
                .Where(rn => rn.ResearchFileId == id)
                .Include(rn => rn.Note)
                .Select(rn => new LastUpdatedByModel()
                {
                    ParentId = id,
                    AppLastUpdateUserid = rn.Note.AppLastUpdateUserid,
                    AppLastUpdateUserGuid = rn.Note.AppLastUpdateUserGuid,
                    AppLastUpdateTimestamp = rn.Note.AppLastUpdateTimestamp,
                })
                .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
                .Take(1)
                .ToList();
            lastUpdatedByAggregate.AddRange(notesLastUpdatedBy);

            // This is needed to get the notes last-updated-by from the notes that where deleted
            var deletedNotes = this.Context.PimsResearchFileNoteHists.AsNoTracking()
                .Where(aph => aph.ResearchFileId == id)
                .GroupBy(aph => aph.ResearchFileNoteId)
                .Select(gaph => gaph.OrderByDescending(a => a.EffectiveDateHist).FirstOrDefault()).ToList();

            var notesHistoryLastUpdatedBy = deletedNotes
                .Select(anh => new LastUpdatedByModel()
                {
                    ParentId = id,
                    AppLastUpdateUserid = anh.AppLastUpdateUserid, // TODO: Update this once the DB tracks the user
                    AppLastUpdateUserGuid = anh.AppLastUpdateUserGuid, // TODO: Update this once the DB tracks the user
                    AppLastUpdateTimestamp = anh.EndDateHist ?? DateTime.UnixEpoch,
                })
                .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
                .Take(1)
                .ToList();
            lastUpdatedByAggregate.AddRange(notesHistoryLastUpdatedBy);

            var propertiesLastUpdatedBy = this.Context.PimsPropertyResearchFiles.AsNoTracking()
                .Where(rp => rp.ResearchFileId == id)
                .Select(rp => new LastUpdatedByModel()
                {
                    ParentId = id,
                    AppLastUpdateUserid = rp.AppLastUpdateUserid,
                    AppLastUpdateUserGuid = rp.AppLastUpdateUserGuid,
                    AppLastUpdateTimestamp = rp.AppLastUpdateTimestamp,
                })
                .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
                .Take(1)
                .ToList();
            lastUpdatedByAggregate.AddRange(propertiesLastUpdatedBy);

            // This is needed to get the properties last-updated-by from the notes that where deleted
            var deletedProperties = this.Context.PimsPropertyResearchFileHists.AsNoTracking()
                .Where(aph => aph.ResearchFileId == id)
                .GroupBy(aph => aph.PropertyResearchFileId)
                .Select(gaph => gaph.OrderByDescending(a => a.EffectiveDateHist).FirstOrDefault()).ToList();

            var propertiesHistoryLastUpdatedBy = deletedProperties
            .Select(rph => new LastUpdatedByModel()
            {
                ParentId = id,
                AppLastUpdateUserid = rph.AppLastUpdateUserid, // TODO: Update this once the DB tracks the user
                AppLastUpdateUserGuid = rph.AppLastUpdateUserGuid, // TODO: Update this once the DB tracks the user
                AppLastUpdateTimestamp = rph.EndDateHist ?? DateTime.UnixEpoch,
            })
            .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
            .Take(1)
            .ToList();
            lastUpdatedByAggregate.AddRange(propertiesHistoryLastUpdatedBy);

            return lastUpdatedByAggregate.OrderByDescending(x => x.AppLastUpdateTimestamp).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves the version of the research file with the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public long GetRowVersion(long id)
        {
            return this.Context.PimsResearchFiles.AsNoTracking()
                .Where(p => p.ResearchFileId == id)?
                .Select(p => p.ConcurrencyControlNumber)?
                .FirstOrDefault() ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Get a page with an array of research files within the specified filters.
        /// Note that the 'filter' will control the 'page' and 'quantity'.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Paged<PimsResearchFile> GetPage(ResearchFilter filter)
        {
            filter.ThrowIfNull(nameof(filter));
            if (!filter.IsValid())
            {
                throw new ArgumentException("Argument must have a valid filter", nameof(filter));
            }

            var skip = (filter.Page - 1) * filter.Quantity;
            var query = this.Context.GenerateResearchQuery(filter);
            var items = query
                .Skip(skip)
                .Take(filter.Quantity)
                .ToArray();

            return new Paged<PimsResearchFile>(items, filter.Page, filter.Quantity, query.Count());
        }

        /// <summary>
        /// Generate a new R File in format R-X using the research id.
        /// </summary>
        private static string GenerateRFileNumber(long id)
        {
            return $"R-{id}";
        }

        /// <summary>
        /// Get the next available id from the PIMS_RESEARCH_FILE_ID_SEQ.
        /// </summary>
        /// <param name="context"></param>
        private long GetNextResearchSequenceValue()
        {
            return _sequenceRepository.GetNextSequenceValue("dbo.PIMS_RESEARCH_FILE_ID_SEQ");
        }
        #endregion
    }
}
