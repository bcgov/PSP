using System;
using Pims.Api.Models.Base;

namespace Pims.Api.Models.Concepts.Notification
{
    public class NotificationModel : BaseConcurrentModel
    {
        public long? NotificationId { get; set; }

        public string NotificationTypeCode { get; set; }

        public DateOnly NotificationTriggerDate { get; set; }

        public string NotificationMessage { get; set; }

        // Only one of these should be set at a time:
        public long? AcquisitionFileId { get; set; }

        public long? DispositionFileId { get; set; }

        public long? ResearchFileId { get; set; }

        public long? ManagementFileId { get; set; }

        public long? LeaseId { get; set; }

        public long? TakeId { get; set; }

        public long? InsuranceId { get; set; }

        public long? LeaseConsultationId { get; set; }

        public long? NoticeOfClaimId { get; set; }

        public long? LeaseRenewalId { get; set; }

        public long? ExpropOwnerHistoryId { get; set; }

        public long? AgreementId { get; set; }
    }
}