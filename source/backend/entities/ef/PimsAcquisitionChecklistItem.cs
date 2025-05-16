using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

[Table("PIMS_ACQUISITION_CHECKLIST_ITEM")]
[Index("AcquisitionFileId", Name = "ACQCKI_ACQUISITION_FILE_ID_IDX")]
[Index("AcqChklstItemTypeCode", Name = "ACQCKI_ACQ_CHKLST_ITEM_TYPE_CODE_IDX")]
[Index("AcquisitionFileId", "AcqChklstItemTypeCode", Name = "ACQCKI_ACQ_FILE_CHKLST_ITEM_UK_IDX", IsUnique = true)]
[Index("ChklstItemStatusTypeCode", Name = "ACQCKI_CHKLST_ITEM_STATUS_TYPE_CODE_IDX")]
public partial class PimsAcquisitionChecklistItem
{
    [Key]
    [Column("ACQUISITION_CHECKLIST_ITEM_ID")]
    public long AcquisitionChecklistItemId { get; set; }

    [Column("ACQUISITION_FILE_ID")]
    public long AcquisitionFileId { get; set; }

    [Column("ACQ_CHKLST_ITEM_TYPE_CODE")]
    [StringLength(20)]
    public string AcqChklstItemTypeCode { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_CHKLST_ITEM_STATUS_TYPE table.
    /// </summary>
    [Required]
    [Column("CHKLST_ITEM_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string ChklstItemStatusTypeCode { get; set; }

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

    [Required]
    [Column("DB_CREATE_USERID")]
    [StringLength(30)]
    public string DbCreateUserid { get; set; }

    [Column("DB_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime DbLastUpdateTimestamp { get; set; }

    [Required]
    [Column("DB_LAST_UPDATE_USERID")]
    [StringLength(30)]
    public string DbLastUpdateUserid { get; set; }

    [ForeignKey("AcqChklstItemTypeCode")]
    [InverseProperty("PimsAcquisitionChecklistItems")]
    public virtual PimsAcqChklstItemType AcqChklstItemTypeCodeNavigation { get; set; }

    [ForeignKey("AcquisitionFileId")]
    [InverseProperty("PimsAcquisitionChecklistItems")]
    public virtual PimsAcquisitionFile AcquisitionFile { get; set; }

    [ForeignKey("ChklstItemStatusTypeCode")]
    [InverseProperty("PimsAcquisitionChecklistItems")]
    public virtual PimsChklstItemStatusType ChklstItemStatusTypeCodeNavigation { get; set; }
}
