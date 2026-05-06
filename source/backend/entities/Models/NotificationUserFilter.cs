using System;

namespace Pims.Dal.Entities.Models
{
    public class NotificationUserFilter : PageFilter
    {
        public string NotificationOutputTypeCode { get; set; }

        public int? MaxRetries { get; set; }

        public DateTime? NotificationSentDateTime { get; set; } = null;
    }
}
