using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public class SecurityDepositReturnRepository : BaseRepository<PimsSecurityDepositReturn>, ISecurityDepositReturnRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a SecurityDepositReturnRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public SecurityDepositReturnRepository(
            PimsContext dbContext,
            ClaimsPrincipal user,
            ILogger<LeaseRepository> logger)
            : base(dbContext, user, logger)
        {
        }

        #endregion

        public PimsSecurityDepositReturn GetById(long id)
        {
            var query = this.Context.PimsSecurityDepositReturns.AsNoTracking()
                .Where(t => t.SecurityDepositReturnId == id)
                .Include(r => r.PimsSecurityDepositReturnHolder);
            return query.FirstOrDefault() ?? throw new KeyNotFoundException();
        }

        public IEnumerable<PimsSecurityDepositReturn> GetAllByDepositId(long id)
        {
            var depositReturns = this.Context.PimsSecurityDepositReturns.AsNoTracking().Where(t => t.SecurityDepositId == id);
            return depositReturns ?? throw new KeyNotFoundException();
        }

        public PimsSecurityDepositReturn Add(PimsSecurityDepositReturn depositReturn)
        {
            var addedReturn = this.Context.Add(depositReturn);
            return addedReturn.Entity;
        }

        public PimsSecurityDepositReturn Update(PimsSecurityDepositReturn depositReturn)
        {
            var updatedReturn = this.Context.Update(depositReturn);
            return updatedReturn.Entity;
        }

        public void Delete(long id)
        {
            PimsSecurityDepositReturn depositReturn = this.Context.PimsSecurityDepositReturns
                .Where(x => x.SecurityDepositReturnId == id)
                .Include(s => s.PimsSecurityDepositReturnHolder).FirstOrDefault() ?? throw new KeyNotFoundException();
            if (depositReturn.PimsSecurityDepositReturnHolder != null)
            {
                this.Context.Remove(depositReturn.PimsSecurityDepositReturnHolder);
            }
            this.Context.Remove(depositReturn);
        }
    }
}
