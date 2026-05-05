namespace Pims.Dal.Entities.Models
{
    public class NotificationSearchCriteria
    {
        /// <summary>
        /// get/set - The type of notification to search by.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// get/set - The acquisition file id to search by.
        /// </summary>
        public long? AcquisitionFileId { get; set; }

        /// <summary>
        /// get/set - The disposition file id to search by.
        /// </summary>
        public long? DispositionFileId { get; set; }

        /// <summary>
        /// get/set - The research file id to search by.
        /// </summary>
        public long? ResearchFileId { get; set; }

        /// <summary>
        /// get/set - The management file id to search by.
        /// </summary>
        public long? ManagementFileId { get; set; }

        /// <summary>
        /// get/set - The lease id to search by.
        /// </summary>
        public long? LeaseId { get; set; }

        /// <summary>
        /// get/set - The take id to search by.
        /// </summary>
        public long? TakeId { get; set; }

        /// <summary>
        /// get/set - The lease insurance id to search by.
        /// </summary>
        public long? InsuranceId { get; set; }

        /// <summary>
        /// get/set - The lease consultation id to search by.
        /// </summary>
        public long? LeaseConsultationId { get; set; }

        /// <summary>
        /// get/set - The notice of claim id to search by.
        /// </summary>
        public long? NoticeOfClaimId { get; set; }

        /// <summary>
        /// get/set - The lease renewal id to search by.
        /// </summary>
        public long? LeaseRenewalId { get; set; }

        /// <summary>
        /// get/set - The expropriation owner history id to search by.
        /// </summary>
        public long? ExpropOwnerHistoryId { get; set; }

        /// <summary>
        /// get/set - The agreement id to search by.
        /// </summary>
        public long? AgreementId { get; set; }
    }
}
