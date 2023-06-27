using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        /// <param name="logger"></param>
        public SecurityDepositRepository(
            PimsContext dbContext,
            ClaimsPrincipal user,
            ILogger<LeaseRepository> logger)
            : base(dbContext, user, logger)
        {
        }

        #endregion

        public IEnumerable<PimsSecurityDeposit> GetAllByLeaseId(long leaseId)
        {
            return this.Context.PimsSecurityDeposits
                .Include(s => s.PimsSecurityDepositHolder)
                    .ThenInclude(h => h.Person)
                .Include(s => s.PimsSecurityDepositHolder)
                    .ThenInclude(h => h.Organization)
                .Include(s => s.SecurityDepositTypeCodeNavigation)
                .Include(s => s.PimsSecurityDepositReturns)
                    .ThenInclude(s => s.PimsSecurityDepositReturnHolder)
                        .ThenInclude(s => s.Person)
                .Include(s => s.PimsSecurityDepositReturns)
                    .ThenInclude(s => s.PimsSecurityDepositReturnHolder)
                        .ThenInclude(s => s.Organization)
                    .AsNoTracking().Where(t => t.LeaseId == leaseId).ToArray();
        }

        public PimsSecurityDeposit GetById(long id)
        {
            var query = this.Context.PimsSecurityDeposits.AsNoTracking()
                .Where(t => t.SecurityDepositId == id)
                .Include(s => s.PimsSecurityDepositHolder)
                    .ThenInclude(h => h.Person)
                .Include(s => s.PimsSecurityDepositHolder)
                    .ThenInclude(h => h.Organization);
            return query.FirstOrDefault() ?? throw new KeyNotFoundException();
        }

        public PimsSecurityDeposit Add(PimsSecurityDeposit securityDeposit)
        {
            var addedSecurityDeposit = this.Context.Add(securityDeposit);
            return addedSecurityDeposit.Entity;
        }

        public PimsSecurityDeposit Update(PimsSecurityDeposit securityDeposit)
        {
            var updatedSecurityDeposit = this.Context.Update(securityDeposit);
            return updatedSecurityDeposit.Entity;
        }

        public bool Delete(long id)
        {
            PimsSecurityDeposit deposit = this.Context.PimsSecurityDeposits
                .Where(x => x.SecurityDepositId == id)
                .Include(s => s.PimsSecurityDepositHolder).FirstOrDefault() ?? throw new KeyNotFoundException();
            if (deposit.PimsSecurityDepositHolder != null)
            {
                this.Context.Remove(deposit.PimsSecurityDepositHolder);
            }
            this.Context.Remove(deposit);

            return true;
        }
    }
}
