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
            /*if (loadPayments)
            {
                query.Include(t => t.PimsLeasePayments);
            }*/
            return query.FirstOrDefault() ?? throw new KeyNotFoundException();
        }

        public PimsSecurityDeposit Add(PimsSecurityDeposit securityDeposit)
        {
            //securityDeposit = AssociateHolders(securityDeposit);
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

        /*private PimsSecurityDeposit AssociateHolders(PimsSecurityDeposit securityDeposit)
        {
            foreach (PimsSecurityDepositHolder holder in securityDeposit.PimsSecurityDepositHolders)
            {
                if (holder.PersonId != null)
                {
                    PimsPerson person = this.Context.PimsPeople
                   .AsNoTracking()
                   .FirstOrDefault(p => p.PersonId == holder.PersonId);

                }
                else if (holder.OrganizationId != null)
                { }
                PimsPerson property = this.Context.PimsProperties
                    .Include(p => p.PimsPropertyLeases)
                    .ThenInclude(l => l.Lease)
                    .AsNoTracking()
                    .FirstOrDefault(p => (propertyLease.Property != null && p.Pid == propertyLease.Property.Pid) ||
                        (propertyLease.Property != null && propertyLease.Property.Pin != null && p.Pin == propertyLease.Property.Pin));
                if (property?.PropertyId == null)
                {
                    throw new InvalidOperationException($"Property with PID {propertyLease?.Property?.Pid.ToString() ?? ""} does not exist");
                }
                if (property?.PimsPropertyLeases.Any(p => p.LeaseId != lease.Id) == true && !userOverride && newLeaseProperties)
                {
                    var genericOverrideErrorMsg = $"is attached to L-File # {property.PimsPropertyLeases.FirstOrDefault().Lease.LFileNo}";
                    if (propertyLease?.Property?.Pin != null)
                    {
                        throw new UserOverrideException($"PIN {propertyLease?.Property?.Pin.ToString() ?? ""} {genericOverrideErrorMsg}");
                    }
                    throw new UserOverrideException($"PID {propertyLease?.Property?.Pid.ToString() ?? ""} {genericOverrideErrorMsg}");
                }
                propertyLease.PropertyId = property.PropertyId;
                propertyLease.Property = null; //Do not attempt to update the associated property, just refer to it by id.
            }
            return deposit;
        }*/

    }
}
