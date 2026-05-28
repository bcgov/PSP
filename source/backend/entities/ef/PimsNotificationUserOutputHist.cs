using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

[Table("PIMS_NOTIFICATION_USER_OUTPUT_HIST")]
[Index("NotificationUserOutputHistId", "EndDateHist", Name = "PIMS_NUTOUT_H_UK", IsUnique = true)]
public partial class PimsNotificationUserOutputHist
{
    [Key]
    [Column("_NOTIFICATION_USER_OUTPUT_HIST_ID")]
    public long NotificationUserOutputHistId { get; set; }

    [Column("EFFECTIVE_DATE_HIST", TypeName = "datetime")]
    public DateTime EffectiveDateHist { get; set; }

    [Column("END_DATE_HIST", TypeName = "datetime")]
    public DateTime? EndDateHist { get; set; }

    [Column("NOTIFICATION_USER_OUTPUT_ID")]
    public long NotificationUserOutputId { get; set; }

    [Column("NOTIFICATION_USER_ID")]
    public long NotificationUserId { get; set; }

    [Required]
    [Column("NOTIFICATION_OUTPUT_TYPE_CODE")]
    [StringLength(20)]
    public string NotificationOutputTypeCode { get; set; }

    [Column("NOTIFICATION_SENT_DT", TypeName = "datetime")]
    public DateTime? NotificationSentDt { get; set; }

    [Column("NOTIFICATION_READ_DT", TypeName = "datetime")]
    public DateTime? NotificationReadDt { get; set; }

    [Column("NOTIFICATION_RETRY_CNT")]
    public short? NotificationRetryCnt { get; set; }

    [Column("NOTIFICATION_ERROR_REASON")]
    [StringLength(500)]
    public string NotificationErrorReason { get; set; }

    [Column("NOTIFICATION_ERROR_DT")]
    public DateOnly? NotificationErrorDt { get; set; }

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
