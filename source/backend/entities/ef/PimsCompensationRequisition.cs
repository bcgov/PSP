using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Table containing the compensation requisition data for the acquisition file.
/// </summary>
[Table("PIMS_COMPENSATION_REQUISITION")]
[Index("AcquisitionFileId", Name = "CMPREQ_ACQUISITION_FILE_ID_IDX")]
[Index("AcquisitionFileTeamId", Name = "CMPREQ_ACQUISITION_FILE_PERSON_ID_IDX")]
[Index("AcquisitionOwnerId", Name = "CMPREQ_ACQUISITION_OWNER_ID_IDX")]
[Index("AlternateProjectId", Name = "CMPREQ_ALTERNATE_PROJECT_ID_IDX")]
[Index("ChartOfAccountsId", Name = "CMPREQ_CHART_OF_ACCOUNTS_ID_IDX")]
[Index("InterestHolderId", Name = "CMPREQ_INTEREST_HOLDER_ID_IDX")]
[Index("LeaseId", Name = "CMPREQ_LEASE_ID_IDX")]
[Index("ResponsibilityId", Name = "CMPREQ_RESPONSIBILITY_ID_IDX")]
[Index("YearlyFinancialId", Name = "CMPREQ_YEARLY_FINANCIAL_ID_IDX")]
public partial class PimsCompensationRequisition
{
    [Key]
    [Column("COMPENSATION_REQUISITION_ID")]
    public long CompensationRequisitionId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_ACQUISITION_FILE table.
    /// </summary>
    [Column("ACQUISITION_FILE_ID")]
    public long? AcquisitionFileId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_LEASE table.
    /// </summary>
    [Column("LEASE_ID")]
    public long? LeaseId { get; set; }

    [Column("ACQUISITION_OWNER_ID")]
    public long? AcquisitionOwnerId { get; set; }

    [Column("INTEREST_HOLDER_ID")]
    public long? InterestHolderId { get; set; }

    [Column("ACQUISITION_FILE_TEAM_ID")]
    public long? AcquisitionFileTeamId { get; set; }

    [Column("CHART_OF_ACCOUNTS_ID")]
    public long? ChartOfAccountsId { get; set; }

    [Column("RESPONSIBILITY_ID")]
    public long? ResponsibilityId { get; set; }

    [Column("YEARLY_FINANCIAL_ID")]
    public long? YearlyFinancialId { get; set; }

    /// <summary>
    /// Link a file to an &quot;Alternate Project&quot;, so the user can make alternate payments that may be due after the original file&apos;s project closes.
    /// </summary>
    [Column("ALTERNATE_PROJECT_ID")]
    public long? AlternateProjectId { get; set; }

    /// <summary>
    /// Payee where only the name is known from the PAIMS system,
    /// </summary>
    [Column("LEGACY_PAYEE")]
    [StringLength(1000)]
    public string LegacyPayee { get; set; }

    /// <summary>
    /// Indicates if the agreement is in draft format.
    /// </summary>
    [Column("IS_DRAFT")]
    public bool? IsDraft { get; set; }

    /// <summary>
    /// Indicates if the payment was made in trust.
    /// </summary>
    [Column("IS_PAYMENT_IN_TRUST")]
    public bool? IsPaymentInTrust { get; set; }

    /// <summary>
    /// GST number of the organization receiving the payment.
    /// </summary>
    [Column("GST_NUMBER")]
    [StringLength(50)]
    public string GstNumber { get; set; }

    /// <summary>
    /// Fiscal year of the compensation requisition.
    /// </summary>
    [Column("FISCAL_YEAR")]
    [StringLength(9)]
    public string FiscalYear { get; set; }

    /// <summary>
    /// Agreement date.
    /// </summary>
    [Column("AGREEMENT_DT")]
    public DateOnly? AgreementDt { get; set; }

    /// <summary>
    /// Expropriation notice served date.
    /// </summary>
    [Column("EXPROP_NOTICE_SERVED_DT")]
    public DateOnly? ExpropNoticeServedDt { get; set; }

    /// <summary>
    /// Expropriation vesting date.
    /// </summary>
    [Column("EXPROP_VESTING_DT")]
    public DateOnly? ExpropVestingDt { get; set; }

    /// <summary>
    /// Document generation date.
    /// </summary>
    [Column("GENERATION_DT")]
    public DateOnly? GenerationDt { get; set; }

    /// <summary>
    /// Date that the draft Compensation Req changed from Draft to Final status.
    /// </summary>
    [Column("FINALIZED_DATE")]
    public DateOnly? FinalizedDate { get; set; }

    /// <summary>
    /// Date that the advanced payment was made.
    /// </summary>
    [Column("ADV_PMT_SERVED_DT")]
    public DateOnly? AdvPmtServedDt { get; set; }

    /// <summary>
    /// Special instructions for the compensation requisition.
    /// </summary>
    [Column("SPECIAL_INSTRUCTION")]
    [StringLength(2000)]
    public string SpecialInstruction { get; set; }

    /// <summary>
    /// Detailed remarks for the compensation requisition.
    /// </summary>
    [Column("DETAILED_REMARKS")]
    [StringLength(2000)]
    public string DetailedRemarks { get; set; }

    /// <summary>
    /// Indicates if the requisition is inactive.
    /// </summary>
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

    [ForeignKey("AcquisitionFileId")]
    [InverseProperty("PimsCompensationRequisitions")]
    public virtual PimsAcquisitionFile AcquisitionFile { get; set; }

    [ForeignKey("AcquisitionFileTeamId")]
    [InverseProperty("PimsCompensationRequisitions")]
    public virtual PimsAcquisitionFileTeam AcquisitionFileTeam { get; set; }

    [ForeignKey("AcquisitionOwnerId")]
    [InverseProperty("PimsCompensationRequisitions")]
    public virtual PimsAcquisitionOwner AcquisitionOwner { get; set; }

    [ForeignKey("AlternateProjectId")]
    [InverseProperty("PimsCompensationRequisitions")]
    public virtual PimsProject AlternateProject { get; set; }

    [ForeignKey("ChartOfAccountsId")]
    [InverseProperty("PimsCompensationRequisitions")]
    public virtual PimsChartOfAccountsCode ChartOfAccounts { get; set; }

    [ForeignKey("InterestHolderId")]
    [InverseProperty("PimsCompensationRequisitions")]
    public virtual PimsInterestHolder InterestHolder { get; set; }

    [ForeignKey("LeaseId")]
    [InverseProperty("PimsCompensationRequisitions")]
    public virtual PimsLease Lease { get; set; }

    [InverseProperty("CompensationRequisition")]
    public virtual ICollection<PimsCompReqFinancial> PimsCompReqFinancials { get; set; } = new List<PimsCompReqFinancial>();

    [InverseProperty("CompensationRequisition")]
    public virtual ICollection<PimsLeaseStakeholderCompReq> PimsLeaseStakeholderCompReqs { get; set; } = new List<PimsLeaseStakeholderCompReq>();

    [InverseProperty("CompensationRequisition")]
    public virtual ICollection<PimsPropAcqFlCompReq> PimsPropAcqFlCompReqs { get; set; } = new List<PimsPropAcqFlCompReq>();

    [InverseProperty("CompensationRequisition")]
    public virtual ICollection<PimsPropLeaseCompReq> PimsPropLeaseCompReqs { get; set; } = new List<PimsPropLeaseCompReq>();

    [ForeignKey("ResponsibilityId")]
    [InverseProperty("PimsCompensationRequisitions")]
    public virtual PimsResponsibilityCode Responsibility { get; set; }

    [ForeignKey("YearlyFinancialId")]
    [InverseProperty("PimsCompensationRequisitions")]
    public virtual PimsYearlyFinancialCode YearlyFinancial { get; set; }
}
