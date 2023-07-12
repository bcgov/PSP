using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// InsuranceService class, provides a service layer to interact with insurances within the datasource.
    /// </summary>
    public class InsuranceRepository : BaseRepository<PimsInsurance>, IInsuranceRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a InsuranceService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public InsuranceRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<InsuranceRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods
        IEnumerable<PimsInsurance> IInsuranceRepository.GetByLeaseId(long leaseId)
        {
            using var scope = Logger.QueryScope();
            return this.Context.PimsInsurances
                .Include(i => i.InsuranceTypeCodeNavigation)
                .Where(l => l.LeaseId == leaseId).AsNoTracking() ?? throw new KeyNotFoundException();
        }

        IEnumerable<PimsInsurance> IInsuranceRepository.UpdateLeaseInsurance(long leaseId, IEnumerable<PimsInsurance> insurances)
        {
            using var scope = Logger.QueryScope();
            this.Context.UpdateChild<PimsLease, long, PimsInsurance, long>(l => l.PimsInsurances, leaseId, insurances.ToArray());
            return insurances;
        }
        #endregion
    }
}
