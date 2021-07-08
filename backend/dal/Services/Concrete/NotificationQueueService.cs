using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Pims.Dal.Services
{
    /// <summary>
    /// NotificationQueueService class, provides a service layer to interact with notification templates within the datasource.
    /// </summary>
    public class NotificationQueueService : BaseService<NotificationQueue>, INotificationQueueService
    {
        #region Variables
        private readonly INotificationService _notifyService;
        private readonly PimsOptions _options;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a NotificationQueueService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="options"></param>
        /// <param name="service"></param>
        /// <param name="notifyService"></param>
        /// <param name="logger"></param>
        public NotificationQueueService(PimsContext dbContext, ClaimsPrincipal user, IOptions<PimsOptions> options, IPimsService service, INotificationService notifyService, ILogger<NotificationQueueService> logger) : base(dbContext, user, service, logger)
        {
            _options = options.Value;
            _notifyService = notifyService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get an array of notifications within the specified filter.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Paged<NotificationQueue> GetPage(NotificationQueueFilter filter)
        {
            this.User.ThrowIfNotAuthorized(Permissions.SystemAdmin);

            var query = this.Context.GenerateQuery(this.User, filter);
            var total = query.Count();
            var items = query.Skip((filter.Page - 1) * filter.Quantity).Take(filter.Quantity);

            return new Paged<NotificationQueue>(items, filter.Page, filter.Quantity, total);
        }

        /// <summary>
        /// Get the notification in the queue for the specified 'id'.
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="KeyNotFoundException">The notification does not exist in the queue for the specified 'id'.</exception>
        /// <returns></returns>
        public NotificationQueue Get(long id)
        {
            this.User.ThrowIfNotAuthorized(Permissions.SystemAdmin);

            return this.Context.NotificationQueue
                .FirstOrDefault(t => t.Id == id) ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Add the specified 'notification' to the data source.
        /// </summary>
        /// <param name="notification"></param>
        public void Add(NotificationQueue notification)
        {
            this.User.ThrowIfNotAuthorized(Permissions.SystemAdmin);

            this.Context.NotificationQueue.Add(notification);
            this.Context.CommitTransaction();
        }

        /// <summary>
        /// Add the array of notifications to the data source.
        /// </summary>
        /// <param name="notifications"></param>
        public void Add(IEnumerable<NotificationQueue> notifications)
        {
            this.User.ThrowIfNotAuthorized(Permissions.SystemAdmin);

            this.Context.NotificationQueue.AddRange(notifications);
            this.Context.CommitTransaction();
        }

        /// <summary>
        /// Update the speicified 'notification' in the data source.
        /// </summary>
        /// <param name="notification"></param>
        public void Update(NotificationQueue notification)
        {
            this.Context.Update(notification);
            this.Context.CommitTransaction();
        }

        /// <summary>
        /// Update the speicified 'notifications' in the data source.
        /// </summary>
        /// <param name="notifications"></param>
        public void Update(IEnumerable<NotificationQueue> notifications)
        {
            this.Context.Update(notifications);
            this.Context.CommitTransaction();
        }

        /// <summary>
        /// Cancel the notification for the specified 'id'.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<NotificationQueue> CancelNotificationAsync(long id)
        {
            var notification = Get(id);
            await _notifyService.CancelAsync(notification);
            Update(notification);

            return notification;
        }

        /// <summary>
        /// Send the notifications to CHES.
        /// Update the queue with the latest information.
        /// </summary>
        /// <param name="notifications"></param>
        /// <param name="saveChanges"></param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task SendNotificationsAsync(IEnumerable<NotificationQueue> notifications, bool saveChanges = true)
        {
            if (notifications == null) throw new ArgumentNullException(nameof(notifications));

            foreach (var notification in notifications)
            {
                if (this.Context.Entry(notification).State == EntityState.Detached) this.Context.NotificationQueue.Add(notification);
                try
                {
                    await _notifyService.SendAsync(notification);
                }
                catch (Exception ex)
                {
                    this.Logger.LogError(ex, $"Failed to send notification '{notification.Id}'.");
                    if (_options.Notifications.ThrowExceptions)
                        throw;
                }
            }

            // Update the notification queue to include the CHES references.
            if (notifications.Any() && saveChanges) this.Context.CommitTransaction();
        }

        /// <summary>
        /// Generates the notification for the specified 'model' and 'template'.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="to"></param>
        /// <param name="template"></param>
        /// <param name="model"></param>
        /// <param name="sendOn"></param>
        /// <returns></returns>
        public NotificationQueue GenerateNotification<T>(string to, NotificationTemplate template, T model, DateTime? sendOn = null)
        {
            if (template == null) throw new ArgumentNullException(nameof(template));
            if (model == null) throw new ArgumentNullException(nameof(model));

            var notification = new NotificationQueue(template, to, template.Subject, template.Body)
            {
                Key = Guid.NewGuid(),
                Status = NotificationStatus.Pending,
                SendOn = sendOn ?? DateTime.UtcNow,
                To = String.Join(";", new[] { to, template.To }),
                Cc = template.Cc,
                Bcc = template.Bcc
            };

            try
            {
                _notifyService.Generate(notification, model);
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, $"Failed to generate notification for template '{template.Id}'.");
                if (_options.Notifications.ThrowExceptions)
                    throw;
                return null;
            }
            return notification;
        }
        #endregion
    }
}
