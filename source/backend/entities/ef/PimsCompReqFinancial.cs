using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Table associating compensation requisitions related to work activities.
/// </summary>
[Table("PIMS_COMP_REQ_FINANCIAL")]
[Index("CompensationRequisitionId", Name = "CRH120_COMPENSATION_REQUISITION_ID_IDX")]
[Index("FinancialActivityCodeId", Name = "CRH120_FINANCIAL_ACTIVITY_CODE_ID_IDX")]
public partial class PimsCompReqFinancial
{
    [Key]
    [Column("COMP_REQ_FINANCIAL_ID")]
    public long CompReqFinancialId { get; set; }

    [Column("COMPENSATION_REQUISITION_ID")]
    public long CompensationRequisitionId { get; set; }

    [Column("FINANCIAL_ACTIVITY_CODE_ID")]
    public long FinancialActivityCodeId { get; set; }

    /// <summary>
    /// Subtotal of the requisition&apos;s work activity.
    /// </summary>
    [Column("PRETAX_AMT", TypeName = "money")]
    public decimal? PretaxAmt { get; set; }

    /// <summary>
    /// Taxes on the requisition&apos;s work activity.
    /// </summary>
    [Column("TAX_AMT", TypeName = "money")]
    public decimal? TaxAmt { get; set; }

    /// <summary>
    /// Total value of the requisition&apos;s work activity.
    /// </summary>
    [Column("TOTAL_AMT", TypeName = "money")]
    public decimal? TotalAmt { get; set; }

    /// <summary>
    /// Indicates if GST is required for this transaction.
    /// </summary>
    [Column("IS_GST_REQUIRED")]
    public bool? IsGstRequired { get; set; }

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

    [ForeignKey("CompensationRequisitionId")]
    [InverseProperty("PimsCompReqFinancials")]
    public virtual PimsCompensationRequisition CompensationRequisition { get; set; }

    [ForeignKey("FinancialActivityCodeId")]
    [InverseProperty("PimsCompReqFinancials")]
    public virtual PimsFinancialActivityCode FinancialActivityCode { get; set; }
}
