using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Security;

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
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public InsuranceRepository(PimsContext dbContext, ClaimsPrincipal user, IPimsRepository service, ILogger<InsuranceRepository> logger, IMapper mapper)
            : base(dbContext, user, service, logger, mapper)
        {
        }
        #endregion

        #region Methods
        public PimsInsurance Get(long id)
        {
            this.User.ThrowIfNotAuthorized(Permissions.LeaseView);
            return this.Context.PimsInsurances
                .FirstOrDefault(i => i.InsuranceId == id) ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Add the passed lease to the database assuming the user has the require claims.
        /// </summary>
        /// <param name="insurance"></param>
        /// <param name="commit"></param>
        /// <returns></returns>
        public PimsInsurance Add(PimsInsurance insurance, bool commit = true)
        {
            if (insurance == null)
            {
                throw new ArgumentNullException(nameof(insurance), "insurance cannot be null.");
            }
            this.User.ThrowIfNotAuthorized(Permissions.LeaseEdit);

            var entityEntry = this.Context.PimsInsurances.Add(insurance);
            if (commit)
            {
                this.Context.CommitTransaction();
            }
            return entityEntry.Entity;
        }

        public PimsInsurance Update(PimsInsurance insurance, bool commit = true)
        {
            if (insurance == null)
            {
                throw new ArgumentNullException(nameof(insurance), "insurance cannot be null.");
            }
            this.User.ThrowIfNotAuthorized(Permissions.LeaseEdit);

            this.Context.PimsInsurances.Update(insurance);
            if (commit)
            {
                this.Context.CommitTransaction();
            }
            return Get(insurance.InsuranceId);
        }

        public PimsInsurance Delete(PimsInsurance insurance, bool commit = true)
        {
            if (insurance == null)
            {
                throw new ArgumentNullException(nameof(insurance), "insurance cannot be null.");
            }

            this.User.ThrowIfNotAuthorized(Permissions.LeaseEdit);

            this.Context.PimsInsurances.Remove(insurance);
            if (commit)
            {
                this.Context.CommitTransaction();
            }
            return insurance;
        }
        #endregion
    }
}
