using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Describes the details of the notification sent the user including the sent date and read date.
/// </summary>
[Table("PIMS_NOTIFICATION_USER_OUTPUT")]
[Index("NotificationOutputTypeCode", Name = "NUTOUT_NOTIFICATION_OUTPUT_TYPE_CODE_IDX")]
[Index("NotificationUserId", Name = "NUTOUT_NOTIFICATION_USER_ID_IDX")]
public partial class PimsNotificationUserOutput
{
    /// <summary>
    /// System-generated unique surrogate primary key.
    /// </summary>
    [Key]
    [Column("NOTIFICATION_USER_OUTPUT_ID")]
    public long NotificationUserOutputId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_NOTIFICATION_USER table.
    /// </summary>
    [Column("NOTIFICATION_USER_ID")]
    public long NotificationUserId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_NOTIFICATION_OUTPUT_TYPE table.
    /// </summary>
    [Required]
    [Column("NOTIFICATION_OUTPUT_TYPE_CODE")]
    [StringLength(20)]
    public string NotificationOutputTypeCode { get; set; }

    /// <summary>
    /// Date and time that the notification was sent.
    /// </summary>
    [Column("NOTIFICATION_SENT_DT", TypeName = "datetime")]
    public DateTime? NotificationSentDt { get; set; }

    /// <summary>
    /// Date and time that the notification was read.
    /// </summary>
    [Column("NOTIFICATION_READ_DT", TypeName = "datetime")]
    public DateTime? NotificationReadDt { get; set; }

    /// <summary>
    /// Number of times to retry sending the notification.
    /// </summary>
    [Column("NOTIFICATION_RETRY_CNT")]
    public short? NotificationRetryCnt { get; set; }

    /// <summary>
    /// Reason the notification failed.
    /// </summary>
    [Column("NOTIFICATION_ERROR_REASON")]
    [StringLength(500)]
    public string NotificationErrorReason { get; set; }

    /// <summary>
    /// The date the notification failed.
    /// </summary>
    [Column("NOTIFICATION_ERROR_DT")]
    public DateOnly? NotificationErrorDt { get; set; }

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

    [ForeignKey("NotificationOutputTypeCode")]
    [InverseProperty("PimsNotificationUserOutputs")]
    public virtual PimsNotificationOutputType NotificationOutputTypeCodeNavigation { get; set; }

    [ForeignKey("NotificationUserId")]
    [InverseProperty("PimsNotificationUserOutputs")]
    public virtual PimsNotificationUser NotificationUser { get; set; }
}
