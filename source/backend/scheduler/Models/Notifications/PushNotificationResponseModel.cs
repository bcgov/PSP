using Pims.Api.Models.CodeTypes;

namespace Pims.Scheduler.Models.Notifications
{
    public class PushNotificationResponseModel
    {
        public ExternalResponseStatus ResponseStatus { get; set; }

        public string Message { get; set; }

    }
}
