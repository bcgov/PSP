using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface INotificationService
    {
        IEnumerable<PimsNotification> GetByUser(string username);

        PimsNotification GetById(long notificationId);

        PimsNotification Add(PimsNotification notification, string username);

        PimsNotification Update(PimsNotification notification, string username);

        bool Delete(long notificationId, string username);
    }
}