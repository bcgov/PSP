using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_DISPOSITION_CHECKLIST_ITEM")]
    [Index(nameof(DispositionFileId), Name = "DSPCKI_DISPOSITION_FILE_ID_IDX")]
    [Index(nameof(DispositionFileId), nameof(DspChklstItemTypeCode), Name = "DSPCKI_DISPOSITION_FILE_ID_UK_IDX", IsUnique = true)]
    [Index(nameof(DspChklstItemStatusTypeCode), Name = "DSPCKI_DSP_CHKLST_ITEM_STATUS_TYPE_CODE_IDX")]
    [Index(nameof(DspChklstItemTypeCode), Name = "DSPCKI_DSP_CHKLST_ITEM_TYPE_CODE_IDX")]
    public partial class PimsDispositionChecklistItem
    {
        [Key]
        [Column("DISPOSITION_CHECKLIST_ITEM_ID")]
        public long DispositionChecklistItemId { get; set; }
        [Column("DISPOSITION_FILE_ID")]
        public long DispositionFileId { get; set; }
        [Column("DSP_CHKLST_ITEM_TYPE_CODE")]
        [StringLength(20)]
        public string DspChklstItemTypeCode { get; set; }
        [Required]
        [Column("DSP_CHKLST_ITEM_STATUS_TYPE_CODE")]
        [StringLength(20)]
        public string DspChklstItemStatusTypeCode { get; set; }
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

        [ForeignKey(nameof(DispositionFileId))]
        [InverseProperty(nameof(PimsDispositionFile.PimsDispositionChecklistItems))]
        public virtual PimsDispositionFile DispositionFile { get; set; }
        [ForeignKey(nameof(DspChklstItemStatusTypeCode))]
        [InverseProperty(nameof(PimsDspChklstItemStatusType.PimsDispositionChecklistItems))]
        public virtual PimsDspChklstItemStatusType DspChklstItemStatusTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(DspChklstItemTypeCode))]
        [InverseProperty(nameof(PimsDspChklstItemType.PimsDispositionChecklistItems))]
        public virtual PimsDspChklstItemType DspChklstItemTypeCodeNavigation { get; set; }
    }
}
