using Microsoft.Extensions.DependencyInjection;
using Pims.Dal.Services;
using System;
using System.Security.Claims;

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
        /// get - The property services.
        /// </summary>
        public IPropertyService Property { get { return _serviceProvider.GetService<IPropertyService>(); } }

        /// <summary>
        /// get - The building services.
        /// </summary>
        public IBuildingService Building { get { return _serviceProvider.GetService<IBuildingService>(); } }

        /// <summary>
        /// get - The lookup services.
        /// </summary>
        public ILookupService Lookup { get { return _serviceProvider.GetService<ILookupService>(); } }

        /// <summary>
        /// get - The parcel services.
        /// </summary>
        public IParcelService Parcel { get { return _serviceProvider.GetService<IParcelService>(); } }

        /// <summary>
        /// get - The user services.
        /// </summary>
        public IUserService User { get { return _serviceProvider.GetService<IUserService>(); } }

        /// <summary>
        /// get - The notification template services.
        /// </summary>
        public INotificationTemplateService NotificationTemplate { get { return _serviceProvider.GetService<INotificationTemplateService>(); } }

        /// <summary>
        /// get - The notification queue services.
        /// </summary>
        public INotificationQueueService NotificationQueue { get { return _serviceProvider.GetService<INotificationQueueService>(); } }

        /// <summary>
        /// get - The tenant service.
        /// </summary>
        /// <typeparam name="ITenantService"></typeparam>
        public ITenantService Tenant { get { return _serviceProvider.GetService<ITenantService>(); } }

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
        #endregion

        #region Methods
        #endregion
    }
}
