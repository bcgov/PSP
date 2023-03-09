using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_ACQUISITION_CHECKLIST_ITEM")]
    [Index(nameof(AcquisitionFileId), Name = "ACQCKI_ACQUISITION_FILE_ID_IDX")]
    [Index(nameof(AcqChklstItemStatusTypeCode), Name = "ACQCKI_ACQ_CHKLST_ITEM_STATUS_TYPE_CODE_IDX")]
    [Index(nameof(AcqChklstItemTypeCode), Name = "ACQCKI_ACQ_CHKLST_ITEM_TYPE_CODE_IDX")]
    [Index(nameof(AcquisitionFileId), nameof(AcqChklstItemTypeCode), Name = "ACQCKI_ACQ_FILE_CHKLST_ITEM_UK_IDX", IsUnique = true)]
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
        [Required]
        [Column("ACQ_CHKLST_ITEM_STATUS_TYPE_CODE")]
        [StringLength(20)]
        public string AcqChklstItemStatusTypeCode { get; set; }
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

        [ForeignKey(nameof(AcqChklstItemStatusTypeCode))]
        [InverseProperty(nameof(PimsAcqChklstItemStatusType.PimsAcquisitionChecklistItems))]
        public virtual PimsAcqChklstItemStatusType AcqChklstItemStatusTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(AcqChklstItemTypeCode))]
        [InverseProperty(nameof(PimsAcqChklstItemType.PimsAcquisitionChecklistItems))]
        public virtual PimsAcqChklstItemType AcqChklstItemTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(AcquisitionFileId))]
        [InverseProperty(nameof(PimsAcquisitionFile.PimsAcquisitionChecklistItems))]
        public virtual PimsAcquisitionFile AcquisitionFile { get; set; }
    }
}
