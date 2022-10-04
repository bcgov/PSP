using System.Security.Claims;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Provides a repository to interact with sequences within the datasource.
    /// </summary>
    public class SequenceRepository : BaseRepository, ISequenceRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a SequenceRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public SequenceRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<SequenceRepository> logger)
            : base(dbContext, user, logger)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the next available value of sequence matching the supplied sequence name.
        /// </summary>
        public long GetNextSequenceValue(string sequenceName)
        {
            var result = new SqlParameter("@result", System.Data.SqlDbType.BigInt)
            {
                Direction = System.Data.ParameterDirection.Output,
            };

            Context.Database.ExecuteSqlRaw($"SET @result = NEXT VALUE FOR {sequenceName};", result);
            return (long)result.Value;
        }

        #endregion
    }
}
