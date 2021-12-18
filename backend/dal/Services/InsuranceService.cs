using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Security;

namespace Pims.Dal.Services
{
    /// <summary>
    /// InsuranceService class, provides a service layer to interact with insurances within the datasource.
    /// </summary>
    public class InsuranceService : BaseService<PimsInsurance>, IInsuranceService
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of a InsuranceService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public InsuranceService(PimsContext dbContext, ClaimsPrincipal user, IPimsService service, ILogger<InsuranceService> logger, IMapper mapper) : base(dbContext, user, service, logger, mapper) { }
        #endregion

        #region Methods
        public PimsInsurance Get(long id)
        {
            this.User.ThrowIfNotAuthorized(Permissions.LeaseView);
            return this.Context.PimsInsurances
                .Where(i => i.InsuranceId == id)
                .FirstOrDefault() ?? throw new KeyNotFoundException();
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
