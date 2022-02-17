using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public class SecurityDepositRepository : BaseRepository<PimsSecurityDeposit>, ISecurityDepositRepository
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of a SecurityDepositRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public SecurityDepositRepository(PimsContext dbContext,
            ClaimsPrincipal user,
            IPimsRepository service,
            ILogger<LeaseRepository> logger,
            IMapper mapper) : base(dbContext, user, service, logger, mapper) { }

        #endregion

        public IEnumerable<PimsSecurityDeposit> GetByLeaseId(long leaseId)
        {
            return this.Context.PimsSecurityDeposits.AsNoTracking().Where(t => t.LeaseId == leaseId).ToArray();
        }

        public PimsSecurityDeposit GetById(long id)
        {
            var query = this.Context.PimsSecurityDeposits.AsNoTracking().Where(t => t.SecurityDepositId == id);
            return query.FirstOrDefault() ?? throw new KeyNotFoundException();
        }

        public PimsSecurityDeposit Add(PimsSecurityDeposit securityDeposit)
        {
            this.Context.Add(securityDeposit);
            return securityDeposit;
        }


        public PimsSecurityDeposit Update(PimsSecurityDeposit securityDeposit)
        {
            this.Context.Update(securityDeposit);
            return securityDeposit;
        }

        public void Delete(long id)
        {
            PimsSecurityDeposit term = this.Context.PimsSecurityDeposits.Find(id) ?? throw new KeyNotFoundException();
            this.Context.Remove(term);
        }
    }
}
