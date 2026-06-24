using System.Collections.Generic;
using System.Threading.Tasks;
using Pims.Api.Models.Models.Concepts.Notification;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface INotificationUserService
    {
        public IEnumerable<PimsNotificationUserOutput> SearchNotificationUser(NotificationUserSearchFilterModel filter);

        Task PushNotificationUser(long notificationUserId);
    }
}
