using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

[Table("PIMS_NOTIFICATION_HIST")]
[Index("NotificationHistId", "EndDateHist", Name = "PIMS_NOTIFY_H_UK", IsUnique = true)]
public partial class PimsNotificationHist
{
    [Key]
    [Column("_NOTIFICATION_HIST_ID")]
    public long NotificationHistId { get; set; }

    [Column("EFFECTIVE_DATE_HIST", TypeName = "datetime")]
    public DateTime EffectiveDateHist { get; set; }

    [Column("END_DATE_HIST", TypeName = "datetime")]
    public DateTime? EndDateHist { get; set; }

    [Column("NOTIFICATION_ID")]
    public long NotificationId { get; set; }

    [Required]
    [Column("NOTIFICATION_TYPE_CODE")]
    [StringLength(20)]
    public string NotificationTypeCode { get; set; }

    [Column("ACQUISITION_FILE_ID")]
    public long? AcquisitionFileId { get; set; }

    [Column("DISPOSITION_FILE_ID")]
    public long? DispositionFileId { get; set; }

    [Column("RESEARCH_FILE_ID")]
    public long? ResearchFileId { get; set; }

    [Column("MANAGEMENT_FILE_ID")]
    public long? ManagementFileId { get; set; }

    [Column("LEASE_ID")]
    public long? LeaseId { get; set; }

    [Column("TAKE_ID")]
    public long? TakeId { get; set; }

    [Column("INSURANCE_ID")]
    public long? InsuranceId { get; set; }

    [Column("LEASE_CONSULTATION_ID")]
    public long? LeaseConsultationId { get; set; }

    [Column("NOTICE_OF_CLAIM_ID")]
    public long? NoticeOfClaimId { get; set; }

    [Column("LEASE_RENEWAL_ID")]
    public long? LeaseRenewalId { get; set; }

    [Column("EXPROP_OWNER_HISTORY_ID")]
    public long? ExpropOwnerHistoryId { get; set; }

    [Column("AGREEMENT_ID")]
    public long? AgreementId { get; set; }

    [Column("NOTIFICATION_TRIGGER_DATE")]
    public DateOnly NotificationTriggerDate { get; set; }

    [Column("NOTIFICATION_MESSAGE")]
    [StringLength(2000)]
    public string NotificationMessage { get; set; }

    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long ConcurrencyControlNumber { get; set; }

    [Column("APP_CREATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppCreateTimestamp { get; set; }

    [Required]
    [Column("APP_CREATE_USERID")]
    [StringLength(30)]
    public string AppCreateUserid { get; set; }

    [Column("APP_CREATE_USER_GUID")]
    public Guid? AppCreateUserGuid { get; set; }

    [Required]
    [Column("APP_CREATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppCreateUserDirectory { get; set; }

    [Column("APP_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppLastUpdateTimestamp { get; set; }

    [Required]
    [Column("APP_LAST_UPDATE_USERID")]
    [StringLength(30)]
    public string AppLastUpdateUserid { get; set; }

    [Column("APP_LAST_UPDATE_USER_GUID")]
    public Guid? AppLastUpdateUserGuid { get; set; }

    [Required]
    [Column("APP_LAST_UPDATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppLastUpdateUserDirectory { get; set; }

    [Column("DB_CREATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime DbCreateTimestamp { get; set; }

    [Column("DB_CREATE_USERID")]
    [StringLength(30)]
    public string DbCreateUserid { get; set; }

    [Column("DB_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime DbLastUpdateTimestamp { get; set; }

    [Required]
    [Column("DB_LAST_UPDATE_USERID")]
    [StringLength(30)]
    public string DbLastUpdateUserid { get; set; }
}
