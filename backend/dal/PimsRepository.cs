using System;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using Pims.Dal.Repositories;

namespace Pims.Dal
{
    /// <summary>
    /// PimsRepository class, provides a encapsulated way to references all the independent repositories.
    /// </summary>
    public class PimsRepository : IPimsRepository
    {
        #region Variables
        private readonly IServiceProvider _serviceProvider;
        private readonly PimsContext _pimsContext;
        #endregion

        #region Properties
        /// <summary>
        /// get - The person repository.
        /// </summary>
        public IPersonService Person { get { return _serviceProvider.GetService<IPersonService>(); } }

        /// <summary>
        /// get - The organization repository.
        /// </summary>
        public IOrganizationService Organization { get { return _serviceProvider.GetService<IOrganizationService>(); } }

        /// <summary>
        /// get - The user organization service (legacy).
        /// </summary>
        public IUserOrganizationService UserOrganization { get { return _serviceProvider.GetService<IUserOrganizationService>(); } }

        /// <summary>
        /// get - The user calling the repository.
        /// </summary>
        public ClaimsPrincipal Principal { get; }

        /// <summary>
        /// get - The property repository.
        /// </summary>
        public IPropertyService Property { get { return _serviceProvider.GetService<IPropertyService>(); } }

        /// <summary>
        /// get - The lookup repository.
        /// </summary>
        public ILookupService Lookup { get { return _serviceProvider.GetService<ILookupService>(); } }

        /// <summary>
        /// get - The system constant repository.
        /// </summary>
        public ISystemConstantService SystemConstant { get { return _serviceProvider.GetService<ISystemConstantService>(); } }

        /// <summary>
        /// get - The user repository.
        /// </summary>
        public IUserService User { get { return _serviceProvider.GetService<IUserService>(); } }

        /// <summary>
        /// get - The role repository.
        /// </summary>
        public IRoleService Role { get { return _serviceProvider.GetService<IRoleService>(); } }

        /// <summary>
        /// get - The claim repository.
        /// </summary>
        public IClaimService Claim { get { return _serviceProvider.GetService<IClaimService>(); } }

        /// <summary>
        /// get - The access request repository.
        /// </summary>
        public IAccessRequestService AccessRequest { get { return _serviceProvider.GetService<IAccessRequestService>(); } }

        /// <summary>
        /// get - The tenant repository.
        /// </summary>
        public ITenantService Tenant { get { return _serviceProvider.GetService<ITenantService>(); } }

        /// <summary>
        /// get - The lease repository.
        /// </summary>
        public ILeaseRepository Lease { get { return _serviceProvider.GetService<ILeaseRepository>(); } }

        /// <summary>
        /// get - The lease term repository.
        /// </summary>
        public ILeaseTermRepository LeaseTerm { get { return _serviceProvider.GetService<ILeaseTermRepository>(); } }

        /// <summary>
        /// get - The security deposit repository.
        /// </summary>
        public ISecurityDepositRepository SecurityDeposit { get { return _serviceProvider.GetService<ISecurityDepositRepository>(); } }

        /// <summary>
        /// get - The contact repository.
        /// </summary>
        public IContactService Contact { get { return _serviceProvider.GetService<IContactService>(); } }

        /// <summary>
        /// get - The insurance repository.
        /// </summary>
        public IInsuranceService Insurance { get { return _serviceProvider.GetService<IInsuranceService>(); } }

        /// <summary>
        /// get - The autocomplete repository.
        /// </summary>
        public IAutocompleteService Autocomplete { get { return _serviceProvider.GetService<IAutocompleteService>(); } }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a PimsService class, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="serviceProvider"></param>
        public PimsRepository(PimsContext dbContext, ClaimsPrincipal user, IServiceProvider serviceProvider)
        {
            this.Principal = user;
            _pimsContext = dbContext;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Get the original value of the specified 'entity'.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public T OriginalValue<T>(object entity, string propertyName)
        {
            return this.User.OriginalValue<T>(entity, propertyName);
        }

        /// <summary>
        /// Commit all saved changes as a single transaction.
        /// </summary>
        public void CommitTransaction()
        {
            _pimsContext.CommitTransaction();
        }
        #endregion
    }
}
