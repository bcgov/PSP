using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_COMPENSATION_REQUISITION")]
    [Index(nameof(AcquisitionFileId), Name = "CMPREQ_ACQUISITION_FILE_ID_IDX")]
    public partial class PimsCompensationRequisition
    {
        public PimsCompensationRequisition()
        {
            PimsCompReqH120s = new HashSet<PimsCompReqH120>();
        }

        [Key]
        [Column("COMPENSATION_REQUISITION_ID")]
        public long CompensationRequisitionId { get; set; }
        [Column("ACQUISITION_FILE_ID")]
        public long AcquisitionFileId { get; set; }
        [Column("IS_DRAFT")]
        public bool? IsDraft { get; set; }
        [Column("FISCAL_YEAR")]
        [StringLength(9)]
        public string FiscalYear { get; set; }
        [Column("AGREEMENT_DT", TypeName = "date")]
        public DateTime? AgreementDt { get; set; }
        [Column("EXPROP_NOTICE_SERVED_DT", TypeName = "date")]
        public DateTime? ExpropNoticeServedDt { get; set; }
        [Column("EXPROP_VESTING_DT", TypeName = "date")]
        public DateTime? ExpropVestingDt { get; set; }
        [Column("GENERATION_DT", TypeName = "date")]
        public DateTime? GenerationDt { get; set; }
        [Column("SPECIAL_INSTRUCTION")]
        [StringLength(2000)]
        public string SpecialInstruction { get; set; }
        [Column("DETAILED_REMARKS")]
        [StringLength(2000)]
        public string DetailedRemarks { get; set; }
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
        [InverseProperty(nameof(PimsAcquisitionFile.PimsCompensationRequisitions))]
        public virtual PimsAcquisitionFile AcquisitionFile { get; set; }
        [InverseProperty(nameof(PimsCompReqH120.CompensationRequisition))]
        public virtual ICollection<PimsCompReqH120> PimsCompReqH120s { get; set; }
    }
}
