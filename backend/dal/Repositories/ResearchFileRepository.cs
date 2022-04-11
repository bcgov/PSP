using System;
using System.Security.Claims;
using MapsterMapper;
using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;

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
        /// Generate a new L File in format RFile-XXXXXXXXXX using the lease id. Add the lease id and lfileno to the passed lease.
        /// </summary>
        private string GenerateRFileNumber()
        {
            long nextResearchFileId = this.GetNextLeaseSequenceValue();
            return $"RFile-{nextResearchFileId.ToString().PadLeft(10, '0')}";
        }

        /// <summary>
        /// Get the next available id from the PIMS_RESEARCH_FILE_ID_SEQ
        /// </summary>
        /// <param name="context"></param>
        private long GetNextLeaseSequenceValue()
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
