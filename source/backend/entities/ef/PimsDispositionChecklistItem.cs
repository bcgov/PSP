using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

[Table("PIMS_DISPOSITION_CHECKLIST_ITEM")]
[Index("ChklstItemStatusTypeCode", Name = "DSPCKI_CHKLST_ITEM_STATUS_TYPE_CODE_IDX")]
[Index("DispositionFileId", Name = "DSPCKI_DISPOSITION_FILE_ID_IDX")]
[Index("DispositionFileId", "DspChklstItemTypeCode", Name = "DSPCKI_DISPOSITION_FILE_ID_UK_IDX", IsUnique = true)]
[Index("DspChklstItemTypeCode", Name = "DSPCKI_DSP_CHKLST_ITEM_TYPE_CODE_IDX")]
public partial class PimsDispositionChecklistItem
{
    /// <summary>
    /// Unique auto-generated surrogate primary key
    /// </summary>
    [Key]
    [Column("DISPOSITION_CHECKLIST_ITEM_ID")]
    public long DispositionChecklistItemId { get; set; }

    /// <summary>
    /// Foreign key of the disposition file.
    /// </summary>
    [Column("DISPOSITION_FILE_ID")]
    public long DispositionFileId { get; set; }

    /// <summary>
    /// Code value for the checklist item.
    /// </summary>
    [Column("DSP_CHKLST_ITEM_TYPE_CODE")]
    [StringLength(20)]
    public string DspChklstItemTypeCode { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_CHKLST_ITEM_STATUS_TYPE table.
    /// </summary>
    [Required]
    [Column("CHKLST_ITEM_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string ChklstItemStatusTypeCode { get; set; }

    /// <summary>
    /// Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update.  If this is done then the update will succeed, provided that the row was not updated by any
    /// </summary>
    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long ConcurrencyControlNumber { get; set; }

    /// <summary>
    /// The date and time the record was created by the user.
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
    [Required]
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

    [ForeignKey("ChklstItemStatusTypeCode")]
    [InverseProperty("PimsDispositionChecklistItems")]
    public virtual PimsChklstItemStatusType ChklstItemStatusTypeCodeNavigation { get; set; }

    [ForeignKey("DispositionFileId")]
    [InverseProperty("PimsDispositionChecklistItems")]
    public virtual PimsDispositionFile DispositionFile { get; set; }

    [ForeignKey("DspChklstItemTypeCode")]
    [InverseProperty("PimsDispositionChecklistItems")]
    public virtual PimsDspChklstItemType DspChklstItemTypeCodeNavigation { get; set; }
}
