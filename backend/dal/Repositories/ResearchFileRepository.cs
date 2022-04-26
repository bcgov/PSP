using System;
using System.Linq;
using System.Security.Claims;
using MapsterMapper;
using Microsoft.Data.SqlClient;
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
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public ResearchFileRepository(PimsContext dbContext, ClaimsPrincipal user, IPimsRepository service, ILogger<ResearchFileRepository> logger, IMapper mapper) : base(dbContext, user, service, logger, mapper) { }
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
                .Include(r => r.PimsPropertyResearchFiles).FirstOrDefault();
        }

        /// <summary>
        /// Add the specified research file to the datasource.
        /// </summary>
        /// <param name="researchFile"></param>
        /// <returns></returns>
        public PimsResearchFile Add(PimsResearchFile researchFile)
        {
            researchFile.ThrowIfNull(nameof(researchFile));

            researchFile.RfileNumber = GenerateRFileNumber();

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

            researchFile.RfileNumber = GenerateRFileNumber();

            this.Context.PimsResearchFiles.Update(researchFile);
            return researchFile;
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
        /// Generate a new R File in format RFile-XXXXXXXXXX using the research id.
        /// </summary>
        private string GenerateRFileNumber()
        {
            long nextResearchFileId = this.GetNextResearchSequenceValue();
            return $"RFile-{nextResearchFileId.ToString().PadLeft(10, '0')}";
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
