using System.Collections.Generic;
using Pims.Api.Models.Concepts.Notification;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface INotificationService
    {
        IEnumerable<PimsNotification> GetByUser(string username);

        NotificationAccessResponse GetByIdForUser(long notificationId, string username);

        PimsNotification Add(PimsNotification notification, string username);

        PimsNotification Update(PimsNotification notification, string username);

        bool Delete(long notificationId, string username);
    }
}