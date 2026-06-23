using System.Collections.Generic;
using System.Threading.Tasks;
using Pims.Api.Models.Concepts.Notification;
using Pims.Api.Models.Models.Concepts.Notification;
using Pims.Api.Models.Requests.Http;

namespace Pims.Scheduler.Repositories
{
    public interface IPimsNotificationUserRepository
    {
        Task<ExternalResponse<List<NotificationOutputModel>>> SearchUserNotificationsAsync(NotificationUserSearchFilterModel filter);

        Task<ExternalResponse<NotificationOutputModel>> PushUserNotificationsAsync(NotificationOutputModel userNotification);
    }
}
