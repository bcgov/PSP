using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public class LeaseRenewalRepostory : BaseRepository<PimsLeaseRenewal>, ILeaseRenewalRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a LeaseRenewalRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public LeaseRenewalRepostory(PimsContext dbContext, ClaimsPrincipal user, ILogger<LeaseRenewalRepostory> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        public IEnumerable<PimsLeaseRenewal> GetByLeaseId(long leaseId)
        {
            var renewals = this.Context.PimsLeaseRenewals.AsNoTracking()
                .Where(t => t.LeaseId == leaseId).ToArray();

            return renewals;
        }
    }
}

