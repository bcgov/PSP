using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Describes the user notification and related field type (e.g. acquisition, disposition, management) provided to the user.
/// </summary>
[Table("PIMS_NOTIFICATION")]
[Index("AcquisitionFileId", Name = "NOTIFY_ACQUISITION_FILE_ID_IDX")]
[Index("AgreementId", Name = "NOTIFY_AGREEMENT_ID_IDX")]
[Index("DispositionFileId", Name = "NOTIFY_DISPOSITION_FILE_ID_IDX")]
[Index("ExpropOwnerHistoryId", Name = "NOTIFY_EXPROP_OWNER_HISTORY_ID_IDX")]
[Index("InsuranceId", Name = "NOTIFY_INSURANCE_ID_IDX")]
[Index("LeaseConsultationId", Name = "NOTIFY_LEASE_CONSULTATION_ID_IDX")]
[Index("LeaseId", Name = "NOTIFY_LEASE_ID_IDX")]
[Index("LeaseRenewalId", Name = "NOTIFY_LEASE_RENEWAL_ID_IDX")]
[Index("ManagementFileId", Name = "NOTIFY_MANAGEMENT_FILE_ID_IDX")]
[Index("NoticeOfClaimId", Name = "NOTIFY_NOTICE_OF_CLAIM_ID_IDX")]
[Index("NotificationTypeCode", Name = "NOTIFY_NOTIFICATION_TYPE_CODE_IDX")]
[Index("ResearchFileId", Name = "NOTIFY_RESEARCH_FILE_ID_IDX")]
[Index("TakeId", Name = "NOTIFY_TAKE_ID_IDX")]
public partial class PimsNotification
{
    /// <summary>
    /// System-generated unique surrogate primary key.
    /// </summary>
    [Key]
    [Column("NOTIFICATION_ID")]
    public long NotificationId { get; set; }

    /// <summary>
    /// Foreign key to thje PIMS_NOTIFICATION_TYPE table.
    /// </summary>
    [Required]
    [Column("NOTIFICATION_TYPE_CODE")]
    [StringLength(20)]
    public string NotificationTypeCode { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_ACQUISITION_FILE table.
    /// </summary>
    [Column("ACQUISITION_FILE_ID")]
    public long? AcquisitionFileId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_DISPOSITION_FILE table.
    /// </summary>
    [Column("DISPOSITION_FILE_ID")]
    public long? DispositionFileId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_RESEARCH_FILE table.
    /// </summary>
    [Column("RESEARCH_FILE_ID")]
    public long? ResearchFileId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_MANAGEMENT_FILE table.
    /// </summary>
    [Column("MANAGEMENT_FILE_ID")]
    public long? ManagementFileId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_LEASE table.
    /// </summary>
    [Column("LEASE_ID")]
    public long? LeaseId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_TAKE table.
    /// </summary>
    [Column("TAKE_ID")]
    public long? TakeId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_INSURANCE table.
    /// </summary>
    [Column("INSURANCE_ID")]
    public long? InsuranceId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_LEASE_CONSULTATION table.
    /// </summary>
    [Column("LEASE_CONSULTATION_ID")]
    public long? LeaseConsultationId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_NOTICE_OF_CLAIM table.
    /// </summary>
    [Column("NOTICE_OF_CLAIM_ID")]
    public long? NoticeOfClaimId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_LEASE_RENEWAL table.
    /// </summary>
    [Column("LEASE_RENEWAL_ID")]
    public long? LeaseRenewalId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_EXPROP_OWNER_HISTORY table.
    /// </summary>
    [Column("EXPROP_OWNER_HISTORY_ID")]
    public long? ExpropOwnerHistoryId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_AGREEMENT table.
    /// </summary>
    [Column("AGREEMENT_ID")]
    public long? AgreementId { get; set; }

    /// <summary>
    /// Date the notification was transmitted to the user.
    /// </summary>
    [Column("NOTIFICATION_TRIGGER_DATE")]
    public DateOnly NotificationTriggerDate { get; set; }

    /// <summary>
    /// Message text of the notification.
    /// </summary>
    [Column("NOTIFICATION_MESSAGE")]
    [StringLength(2000)]
    public string NotificationMessage { get; set; }

    /// <summary>
    /// Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o
    /// </summary>
    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long ConcurrencyControlNumber { get; set; }

    /// <summary>
    /// The date and time the user created the record.
    /// </summary>
    [Column("APP_CREATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppCreateTimestamp { get; set; }

    /// <summary>
    /// The user that created the record.
    /// </summary>
    [Required]
    [Column("APP_CREATE_USERID")]
    [StringLength(30)]
    public string AppCreateUserid { get; set; }

    /// <summary>
    /// GUID of the user that created the record.
    /// </summary>
    [Column("APP_CREATE_USER_GUID")]
    public Guid? AppCreateUserGuid { get; set; }

    /// <summary>
    /// User directory of the user that created the record.
    /// </summary>
    [Required]
    [Column("APP_CREATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppCreateUserDirectory { get; set; }

    /// <summary>
    /// The date and time the record was updated by the user.
    /// </summary>
    [Column("APP_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppLastUpdateTimestamp { get; set; }

    /// <summary>
    /// The user that updated the record.
    /// </summary>
    [Required]
    [Column("APP_LAST_UPDATE_USERID")]
    [StringLength(30)]
    public string AppLastUpdateUserid { get; set; }

    /// <summary>
    /// GUID of the user that updated the record.
    /// </summary>
    [Column("APP_LAST_UPDATE_USER_GUID")]
    public Guid? AppLastUpdateUserGuid { get; set; }

    /// <summary>
    /// User directory of the user that updated the record.
    /// </summary>
    [Required]
    [Column("APP_LAST_UPDATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppLastUpdateUserDirectory { get; set; }

    /// <summary>
    /// The date and time the record was created.
    /// </summary>
    [Column("DB_CREATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime DbCreateTimestamp { get; set; }

    /// <summary>
    /// The user or proxy account that created the record.
    /// </summary>
    [Column("DB_CREATE_USERID")]
    [StringLength(30)]
    public string DbCreateUserid { get; set; }

    /// <summary>
    /// The date and time the record was created or last updated.
    /// </summary>
    [Column("DB_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime DbLastUpdateTimestamp { get; set; }

    /// <summary>
    /// The user or proxy account that created or last updated the record.
    /// </summary>
    [Required]
    [Column("DB_LAST_UPDATE_USERID")]
    [StringLength(30)]
    public string DbLastUpdateUserid { get; set; }

    [ForeignKey("AcquisitionFileId")]
    [InverseProperty("PimsNotifications")]
    public virtual PimsAcquisitionFile AcquisitionFile { get; set; }

    [ForeignKey("AgreementId")]
    [InverseProperty("PimsNotifications")]
    public virtual PimsAgreement Agreement { get; set; }

    [ForeignKey("DispositionFileId")]
    [InverseProperty("PimsNotifications")]
    public virtual PimsDispositionFile DispositionFile { get; set; }

    [ForeignKey("ExpropOwnerHistoryId")]
    [InverseProperty("PimsNotifications")]
    public virtual PimsExpropOwnerHistory ExpropOwnerHistory { get; set; }

    [ForeignKey("InsuranceId")]
    [InverseProperty("PimsNotifications")]
    public virtual PimsInsurance Insurance { get; set; }

    [ForeignKey("LeaseId")]
    [InverseProperty("PimsNotifications")]
    public virtual PimsLease Lease { get; set; }

    [ForeignKey("LeaseConsultationId")]
    [InverseProperty("PimsNotifications")]
    public virtual PimsLeaseConsultation LeaseConsultation { get; set; }

    [ForeignKey("LeaseRenewalId")]
    [InverseProperty("PimsNotifications")]
    public virtual PimsLeaseRenewal LeaseRenewal { get; set; }

    [ForeignKey("ManagementFileId")]
    [InverseProperty("PimsNotifications")]
    public virtual PimsManagementFile ManagementFile { get; set; }

    [ForeignKey("NoticeOfClaimId")]
    [InverseProperty("PimsNotifications")]
    public virtual PimsNoticeOfClaim NoticeOfClaim { get; set; }

    [ForeignKey("NotificationTypeCode")]
    [InverseProperty("PimsNotifications")]
    public virtual PimsNotificationType NotificationTypeCodeNavigation { get; set; }

    [InverseProperty("Notification")]
    public virtual ICollection<PimsNotificationUser> PimsNotificationUsers { get; set; } = new List<PimsNotificationUser>();

    [ForeignKey("ResearchFileId")]
    [InverseProperty("PimsNotifications")]
    public virtual PimsResearchFile ResearchFile { get; set; }

    [ForeignKey("TakeId")]
    [InverseProperty("PimsNotifications")]
    public virtual PimsTake Take { get; set; }
}
