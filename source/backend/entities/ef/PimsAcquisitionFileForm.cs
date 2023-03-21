using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_ACQUISITION_FILE_FORM")]
    [Index(nameof(AcquisitionFileId), Name = "ACQFRM_ACQUISITION_FILE_ID_IDX")]
    [Index(nameof(FormTypeCode), Name = "ACQFRM_FORM_TYPE_CODE_IDX")]
    public partial class PimsAcquisitionFileForm
    {
        [Key]
        [Column("ACQUISITION_FILE_FORM_ID")]
        public long AcquisitionFileFormId { get; set; }
        [Column("ACQUISITION_FILE_ID")]
        public long AcquisitionFileId { get; set; }
        [Required]
        [Column("FORM_TYPE_CODE")]
        [StringLength(20)]
        public string FormTypeCode { get; set; }
        [Column("FORM_JSON")]
        public string FormJson { get; set; }
        [Column("IS_DISABLED")]
        public bool? IsDisabled { get; set; }
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

        [ForeignKey(nameof(AcquisitionFileId))]
        [InverseProperty(nameof(PimsAcquisitionFile.PimsAcquisitionFileForms))]
        public virtual PimsAcquisitionFile AcquisitionFile { get; set; }
        [ForeignKey(nameof(FormTypeCode))]
        [InverseProperty(nameof(PimsFormType.PimsAcquisitionFileForms))]
        public virtual PimsFormType FormTypeCodeNavigation { get; set; }
    }
}
