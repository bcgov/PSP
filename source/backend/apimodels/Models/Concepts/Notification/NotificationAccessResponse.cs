using Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Notification
{
    public class NotificationAccessResponse
    {
        public NotificationAccessResult Result { get; set; }

        public PimsNotification Notification { get; set; }
    }
}
