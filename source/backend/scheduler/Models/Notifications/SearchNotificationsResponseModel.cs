using System.Collections.Generic;
using Pims.Api.Models.Concepts.Notification;
using Pims.Api.Models.Requests.Http;
using Pims.Scheduler.Models.Base;

namespace Pims.Scheduler.Models
{
    public class SearchNotificationsResponseModel
    {
        public ExternalResponse<List<NotificationUserOutputModel>> SearchResults { get; set; }

        public BaseTaskResponseModel ScheduledTaskResponseModel { get; set; }
    }
}
