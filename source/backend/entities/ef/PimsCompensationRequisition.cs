﻿using System;
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
[Index("AlternateProjectId", Name = "CMPREQ_ALTERNATE_PROJECT_ID_IDX")]
[Index("ChartOfAccountsId", Name = "CMPREQ_CHART_OF_ACCOUNTS_ID_IDX")]
[Index("LeaseId", Name = "CMPREQ_LEASE_ID_IDX")]
[Index("ResponsibilityId", Name = "CMPREQ_RESPONSIBILITY_ID_IDX")]
[Index("YearlyFinancialId", Name = "CMPREQ_YEARLY_FINANCIAL_ID_IDX")]
public partial class PimsCompensationRequisition
{
    /// <summary>
    /// Generated surrogate primary key.
    /// </summary>
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

    /// <summary>
    /// Foreign key to the PIMS_CHART_OF_ACCOUNTS table.
    /// </summary>
    [Column("CHART_OF_ACCOUNTS_ID")]
    public long? ChartOfAccountsId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_RESPONSIBILITY table.
    /// </summary>
    [Column("RESPONSIBILITY_ID")]
    public long? ResponsibilityId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_YEARLY_FINANCIAL table.
    /// </summary>
    [Column("YEARLY_FINANCIAL_ID")]
    public long? YearlyFinancialId { get; set; }

    /// <summary>
    /// Link a file to an &quot;Alternate Project&quot;, so the user can make alternate payments that may be due after the original file&apos;s project closes.
    /// </summary>
    [Column("ALTERNATE_PROJECT_ID")]
    public long? AlternateProjectId { get; set; }

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
    /// The user account that created the record.
    /// </summary>
    [Required]
    [Column("APP_CREATE_USERID")]
    [StringLength(30)]
    public string AppCreateUserid { get; set; }

    /// <summary>
    /// The GUID of the user account that created the record.
    /// </summary>
    [Column("APP_CREATE_USER_GUID")]
    public Guid? AppCreateUserGuid { get; set; }

    /// <summary>
    /// The directory of the user account that created the record.
    /// </summary>
    [Required]
    [Column("APP_CREATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppCreateUserDirectory { get; set; }

    /// <summary>
    /// The date and time the user updated the record.
    /// </summary>
    [Column("APP_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppLastUpdateTimestamp { get; set; }

    /// <summary>
    /// The user account that updated the record.
    /// </summary>
    [Required]
    [Column("APP_LAST_UPDATE_USERID")]
    [StringLength(30)]
    public string AppLastUpdateUserid { get; set; }

    /// <summary>
    /// The GUID of the user account that updated the record.
    /// </summary>
    [Column("APP_LAST_UPDATE_USER_GUID")]
    public Guid? AppLastUpdateUserGuid { get; set; }

    /// <summary>
    /// The directory of the user account that updated the record.
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

    [ForeignKey("AcquisitionFileId")]
    [InverseProperty("PimsCompensationRequisitions")]
    public virtual PimsAcquisitionFile AcquisitionFile { get; set; }

    [ForeignKey("AlternateProjectId")]
    [InverseProperty("PimsCompensationRequisitions")]
    public virtual PimsProject AlternateProject { get; set; }

    [ForeignKey("ChartOfAccountsId")]
    [InverseProperty("PimsCompensationRequisitions")]
    public virtual PimsChartOfAccountsCode ChartOfAccounts { get; set; }

    [ForeignKey("LeaseId")]
    [InverseProperty("PimsCompensationRequisitions")]
    public virtual PimsLease Lease { get; set; }

    [InverseProperty("CompensationRequisition")]
    public virtual ICollection<PimsCompReqAcqPayee> PimsCompReqAcqPayees { get; set; } = new List<PimsCompReqAcqPayee>();

    [InverseProperty("CompensationRequisition")]
    public virtual ICollection<PimsCompReqFinancial> PimsCompReqFinancials { get; set; } = new List<PimsCompReqFinancial>();

    [InverseProperty("CompensationRequisition")]
    public virtual ICollection<PimsCompReqLeasePayee> PimsCompReqLeasePayees { get; set; } = new List<PimsCompReqLeasePayee>();

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
