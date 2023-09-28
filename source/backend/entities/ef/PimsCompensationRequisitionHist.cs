using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_COMPENSATION_REQUISITION_HIST")]
    [Index(nameof(CompensationRequisitionHistId), nameof(EndDateHist), Name = "PIMS_CMPREQ_H_UK", IsUnique = true)]
    public partial class PimsCompensationRequisitionHist
    {
        [Key]
        [Column("_COMPENSATION_REQUISITION_HIST_ID")]
        public long CompensationRequisitionHistId { get; set; }
        [Column("EFFECTIVE_DATE_HIST", TypeName = "datetime")]
        public DateTime EffectiveDateHist { get; set; }
        [Column("END_DATE_HIST", TypeName = "datetime")]
        public DateTime? EndDateHist { get; set; }
        [Column("COMPENSATION_REQUISITION_ID")]
        public long CompensationRequisitionId { get; set; }
        [Column("ACQUISITION_FILE_ID")]
        public long AcquisitionFileId { get; set; }
        [Column("ACQUISITION_OWNER_ID")]
        public long? AcquisitionOwnerId { get; set; }
        [Column("INTEREST_HOLDER_ID")]
        public long? InterestHolderId { get; set; }
        [Column("ACQUISITION_FILE_PERSON_ID")]
        public long? AcquisitionFilePersonId { get; set; }
        [Column("CHART_OF_ACCOUNTS_ID")]
        public long? ChartOfAccountsId { get; set; }
        [Column("RESPONSIBILITY_ID")]
        public long? ResponsibilityId { get; set; }
        [Column("YEARLY_FINANCIAL_ID")]
        public long? YearlyFinancialId { get; set; }
        [Column("ALTERNATE_PROJECT_ID")]
        public long? AlternateProjectId { get; set; }
        [Column("LEGACY_PAYEE")]
        [StringLength(1000)]
        public string LegacyPayee { get; set; }
        [Column("IS_DRAFT")]
        public bool? IsDraft { get; set; }
        [Column("IS_PAYMENT_IN_TRUST")]
        public bool? IsPaymentInTrust { get; set; }
        [Column("GST_NUMBER")]
        [StringLength(50)]
        public string GstNumber { get; set; }
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
        [Column("FINALIZED_DATE", TypeName = "date")]
        public DateTime? FinalizedDate { get; set; }
        [Column("ADV_PMT_SERVED_DT", TypeName = "date")]
        public DateTime? AdvPmtServedDt { get; set; }
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
    }
}
