using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.Data.SqlClient;
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
        #region Constructors

        /// <summary>
        /// Creates a new instance of a ResearchFileRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public ResearchFileRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<ResearchFileRepository> logger)
            : base(dbContext, user, logger)
        {
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
            SqlParameter result = new SqlParameter("@result", System.Data.SqlDbType.BigInt)
            {
                Direction = System.Data.ParameterDirection.Output,
            };
            this.Context.Database.ExecuteSqlRaw("set @result = next value for dbo.PIMS_RESEARCH_FILE_ID_SEQ;", result);

            return (long)result.Value;
        }
        #endregion
    }
}
