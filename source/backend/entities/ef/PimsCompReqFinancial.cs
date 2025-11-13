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
    /// <summary>
    /// System-generated unique surrogate primary key.
    /// </summary>
    [Key]
    [Column("COMP_REQ_FINANCIAL_ID")]
    public long CompReqFinancialId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_COMPENSATION_REQUISITION table.
    /// </summary>
    [Column("COMPENSATION_REQUISITION_ID")]
    public long CompensationRequisitionId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_FINANCIAL_ACTIVITY table.
    /// </summary>
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
    /// Indicates if the record is disabled and therefore not selectable or displayed.
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

    [ForeignKey("CompensationRequisitionId")]
    [InverseProperty("PimsCompReqFinancials")]
    public virtual PimsCompensationRequisition CompensationRequisition { get; set; }

    [ForeignKey("FinancialActivityCodeId")]
    [InverseProperty("PimsCompReqFinancials")]
    public virtual PimsFinancialActivityCode FinancialActivityCode { get; set; }
}
