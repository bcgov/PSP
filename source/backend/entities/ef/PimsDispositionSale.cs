using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Entity containing information regarding an disposition sale.
/// </summary>
[Table("PIMS_DISPOSITION_SALE")]
[Index("DispositionFileId", Name = "DSPSAL_DISPOSITION_FILE_ID_IDX")]
[Index("DspPurchAgentId", Name = "DSPSAL_DSP_PURCH_AGENT_ID_IDX")]
[Index("DspPurchSolicitorId", Name = "DSPSAL_DSP_PURCH_SOLICITOR_ID_IDX")]
public partial class PimsDispositionSale
{
    /// <summary>
    /// Unique auto-generated surrogate primary key.
    /// </summary>
    [Key]
    [Column("DISPOSITION_SALE_ID")]
    public long DispositionSaleId { get; set; }

    /// <summary>
    /// Foreign key value for the dispostion file.
    /// </summary>
    [Column("DISPOSITION_FILE_ID")]
    public long DispositionFileId { get; set; }

    /// <summary>
    /// Foreign key to the agent associated with the sale of the disposition.
    /// </summary>
    [Column("DSP_PURCH_AGENT_ID")]
    public long? DspPurchAgentId { get; set; }

    /// <summary>
    /// Foreign key to the solicitor associated with the sale of the disposition.
    /// </summary>
    [Column("DSP_PURCH_SOLICITOR_ID")]
    public long? DspPurchSolicitorId { get; set; }

    /// <summary>
    /// For general sales, provide the date when the last condition(s) are to be removed. For road closures enter the condition precedent date.
    /// </summary>
    [Column("FINAL_CONDITION_REMOVAL_DT")]
    public DateOnly? FinalConditionRemovalDt { get; set; }

    /// <summary>
    /// The date the disposition was completed.
    /// </summary>
    [Column("SALE_COMPLETION_DT")]
    public DateOnly? SaleCompletionDt { get; set; }

    /// <summary>
    /// The fiscal year in which the sale was completed.
    /// </summary>
    [Column("SALE_FISCAL_YEAR")]
    public short? SaleFiscalYear { get; set; }

    /// <summary>
    /// Value of the final sale.
    /// </summary>
    [Column("SALE_FINAL_AMT", TypeName = "money")]
    public decimal? SaleFinalAmt { get; set; }

    /// <summary>
    /// Amount paid to the realtor managing the sale.
    /// </summary>
    [Column("REALTOR_COMMISSION_AMT", TypeName = "money")]
    public decimal? RealtorCommissionAmt { get; set; }

    /// <summary>
    /// Is GST required for this sale?.
    /// </summary>
    [Column("IS_GST_REQUIRED")]
    public bool IsGstRequired { get; set; }

    /// <summary>
    /// GST collected is calculated based upon Final Sales Price.
    /// </summary>
    [Column("GST_COLLECTED_AMT", TypeName = "money")]
    public decimal? GstCollectedAmt { get; set; }

    /// <summary>
    /// The net book value of the disposition sale.
    /// </summary>
    [Column("NET_BOOK_AMT", TypeName = "money")]
    public decimal? NetBookAmt { get; set; }

    /// <summary>
    /// The sum of all costs incurred to prepare property for sale (e.g., appraisal, environmental and other consultants, legal fees, First Nations accommodation, etc.).
    /// </summary>
    [Column("TOTAL_COST_AMT", TypeName = "money")]
    public decimal? TotalCostAmt { get; set; }

    /// <summary>
    /// Surplus Property Program (SPP) fee to be paid to CITZ.
    /// </summary>
    [Column("SPP_AMT", TypeName = "money")]
    public decimal? SppAmt { get; set; }

    /// <summary>
    /// Cost of propery remediation.
    /// </summary>
    [Column("REMEDIATION_AMT", TypeName = "money")]
    public decimal? RemediationAmt { get; set; }

    /// <summary>
    /// Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update.  If this is done then the update will succeed, provided that the row was not updated by any.
    /// </summary>
    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long ConcurrencyControlNumber { get; set; }

    /// <summary>
    /// The date and time the record was created by the user.
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

    [ForeignKey("DispositionFileId")]
    [InverseProperty("PimsDispositionSales")]
    public virtual PimsDispositionFile DispositionFile { get; set; }

    [ForeignKey("DspPurchAgentId")]
    [InverseProperty("PimsDispositionSales")]
    public virtual PimsDspPurchAgent DspPurchAgent { get; set; }

    [ForeignKey("DspPurchSolicitorId")]
    [InverseProperty("PimsDispositionSales")]
    public virtual PimsDspPurchSolicitor DspPurchSolicitor { get; set; }

    [InverseProperty("DispositionSale")]
    public virtual ICollection<PimsDispositionPurchaser> PimsDispositionPurchasers { get; set; } = new List<PimsDispositionPurchaser>();
}
