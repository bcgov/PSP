using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MapsterMapper;
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
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public SecurityDepositReturnRepository(PimsContext dbContext,
            ClaimsPrincipal user,
            IPimsRepository service,
            ILogger<LeaseRepository> logger,
            IMapper mapper) : base(dbContext, user, service, logger, mapper) { }

        #endregion

        public IEnumerable<PimsSecurityDepositReturn> GetByLeaseId(long leaseId)
        {
            return this.Context.PimsSecurityDepositReturns.AsNoTracking().Where(t => t.LeaseId == leaseId).ToArray();
        }

        public PimsSecurityDepositReturn GetById(long id)
        {
            var query = this.Context.PimsSecurityDepositReturns.AsNoTracking().Where(t => t.SecurityDepositId == id);
            return query.FirstOrDefault() ?? throw new KeyNotFoundException();
        }


        public IEnumerable<PimsSecurityDepositReturn> GetByDepositId(long id)
        {
            var depositReturns = this.Context.PimsSecurityDepositReturns.AsNoTracking().Where(t => t.SecurityDepositId == id);
            return depositReturns ?? throw new KeyNotFoundException();
        }

        public PimsSecurityDepositReturn Add(PimsSecurityDepositReturn securityDeposit)
        {
            this.Context.Add(securityDeposit);
            return securityDeposit;
        }


        public PimsSecurityDepositReturn Update(PimsSecurityDepositReturn securityDeposit)
        {
            this.Context.Update(securityDeposit);
            return securityDeposit;
        }

        public void Delete(long id)
        {
            PimsSecurityDepositReturn term = this.Context.PimsSecurityDepositReturns.Find(id) ?? throw new KeyNotFoundException();
            this.Context.Remove(term);
        }

        /*private PimsSecurityDepositReturn AssociateHolders(PimsSecurityDepositReturn securityDeposit)
        {
            foreach (PimsSecurityDepositReturnHolder holder in securityDeposit.PimsSecurityDepositReturnHolders)
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
