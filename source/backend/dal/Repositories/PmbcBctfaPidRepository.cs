using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Provides a repository to interact with bctfa ownership list in PSP db.
    /// </summary>
    public class PmbcBctfaPidRepository : BaseRepository<Pims.Dal.Entities.PmbcBctfaPid>, IPmbcBctfaPidRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a BctfaOwnershipRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public PmbcBctfaPidRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<PmbcBctfaPidRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        public IEnumerable<PmbcBctfaPid> GetAll()
        {
            return this.Context.PmbcBctfaPids
                .ToArray();
        }

        public void AddRange(IEnumerable<PmbcBctfaPid> pids)
        {
            if (pids != null && pids.Any())
            {
                this.Context.PmbcBctfaPids.AddRange(pids);
            }
        }

        public void UpdateRange(IEnumerable<PmbcBctfaPid> pids)
        {
            if (pids != null && pids.Any())
            {
                this.Context.UpdateRange(pids);
            }
        }

        #endregion
    }
}
