using System;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using Pims.Dal.Services;

namespace Pims.Dal
{
    /// <summary>
    /// PimsService class, provides a encapsulated way to references all the independent services.
    /// </summary>
    public class PimsService : IPimsService
    {
        #region Variables
        private readonly IServiceProvider _serviceProvider;
        #endregion

        #region Properties
        /// <summary>
        /// get - The user calling the service.
        /// </summary>
        public ClaimsPrincipal Principal { get; }

        /// <summary>
        /// get - The lease term service.
        /// </summary>
        public ILeaseTermService LeaseTermService { get { return _serviceProvider.GetService<ILeaseTermService>(); } }
        /// <summary>
        /// get - The lease payment service.
        /// </summary>
        public ILeasePaymentService LeasePaymentService { get { return _serviceProvider.GetService<ILeasePaymentService>(); } }
        /// <summary>
        /// get - The lease service.
        /// </summary>
        public ILeaseService LeaseService { get { return _serviceProvider.GetService<ILeaseService>(); } }
        /// <summary>
        /// get - The security deposit service.
        /// </summary>
        public ISecurityDepositService SecurityDepositService { get { return _serviceProvider.GetService<ISecurityDepositService>(); } }
        /// <summary>
        /// get - The security deposit return service.
        /// </summary>
        public ISecurityDepositReturnService SecurityDepositReturnService { get { return _serviceProvider.GetService<ISecurityDepositReturnService>(); } }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a PimsService class, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="serviceProvider"></param>
        public PimsService(ClaimsPrincipal user, IServiceProvider serviceProvider)
        {
            this.Principal = user;
            _serviceProvider = serviceProvider;
        }
        #endregion
    }
}
