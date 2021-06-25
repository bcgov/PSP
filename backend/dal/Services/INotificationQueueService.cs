using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using System;
using System.Collections.Generic;

namespace Pims.Dal.Services
{
    /// <summary>
    /// INotificationQueueService interface, provides functions to interact with the notification queue within the datasource.
    /// </summary>
    public interface INotificationQueueService : IService
    {
        Paged<NotificationQueue> GetPage(NotificationQueueFilter filter);
        NotificationQueue Get(long id);
        void Add(IEnumerable<NotificationQueue> notifications);
        void Add(NotificationQueue notification);
        void Update(NotificationQueue notification);
        void Update(IEnumerable<NotificationQueue> notifications);
        System.Threading.Tasks.Task<NotificationQueue> CancelNotificationAsync(long id);
        NotificationQueue GenerateNotification<T>(string to, NotificationTemplate template, T model, DateTime? sendOn = null);
        System.Threading.Tasks.Task SendNotificationsAsync(IEnumerable<NotificationQueue> notifications, bool saveChanges = true);
    }
}
