using System;
using Pims.Api.Models.Base;

namespace Pims.Api.Models.Concepts.Notification
{
    public class NotificationUserOutputModel : BaseConcurrentModel
    {
        public long NotificationUserOutputId { get; set; }

        public long NotificationUserId { get; set; }

        public string NotificationOutputTypeCode { get; set; }

        public DateTime? NotificationSentDt { get; set; }

        public DateTime? NotificationReadDt { get; set; }

        public short? NotificationRetryCnt { get; set; }

        public string? NotificationErrorReason { get; set; }

        public DateOnly? NotificationErrorDt { get; set; }
    }
}
