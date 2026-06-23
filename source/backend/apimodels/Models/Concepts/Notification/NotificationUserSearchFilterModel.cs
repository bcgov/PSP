using System;
using Pims.Dal.Entities.Models;

namespace Pims.Api.Models.Models.Concepts.Notification
{
    public class NotificationUserSearchFilterModel : PageFilter
    {
        public string NotificationOutputTypeCode { get; set; }

        public int? MaxRetries { get; set; }

        public DateTime? NotificationSentDateTime { get; set; } = null;

        public DateOnly? NotificationTriggerDate { get; set; } = null;
    }
}
