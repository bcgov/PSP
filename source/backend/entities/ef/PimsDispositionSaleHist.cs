using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

[Table("PIMS_DISPOSITION_SALE_HIST")]
[Index("DispositionSaleHistId", "EndDateHist", Name = "PIMS_DSPSAL_H_UK", IsUnique = true)]
public partial class PimsDispositionSaleHist
{
    [Key]
    [Column("_DISPOSITION_SALE_HIST_ID")]
    public long DispositionSaleHistId { get; set; }

    [Column("EFFECTIVE_DATE_HIST", TypeName = "datetime")]
    public DateTime EffectiveDateHist { get; set; }

    [Column("END_DATE_HIST", TypeName = "datetime")]
    public DateTime? EndDateHist { get; set; }

    [Column("DISPOSITION_SALE_ID")]
    public long DispositionSaleId { get; set; }

    [Column("DISPOSITION_FILE_ID")]
    public long DispositionFileId { get; set; }

    [Column("DSP_PURCH_AGENT_ID")]
    public long? DspPurchAgentId { get; set; }

    [Column("DSP_PURCH_SOLICITOR_ID")]
    public long? DspPurchSolicitorId { get; set; }

    [Column("FINAL_CONDITION_REMOVAL_DT")]
    public DateOnly? FinalConditionRemovalDt { get; set; }

    [Column("SALE_COMPLETION_DT")]
    public DateOnly? SaleCompletionDt { get; set; }

    [Column("SALE_FISCAL_YEAR")]
    public short? SaleFiscalYear { get; set; }

    [Column("SALE_FINAL_AMT", TypeName = "money")]
    public decimal? SaleFinalAmt { get; set; }

    [Column("REALTOR_COMMISSION_AMT", TypeName = "money")]
    public decimal? RealtorCommissionAmt { get; set; }

    [Column("IS_GST_REQUIRED")]
    public bool IsGstRequired { get; set; }

    [Column("GST_COLLECTED_AMT", TypeName = "money")]
    public decimal? GstCollectedAmt { get; set; }

    [Column("NET_BOOK_AMT", TypeName = "money")]
    public decimal? NetBookAmt { get; set; }

    [Column("TOTAL_COST_AMT", TypeName = "money")]
    public decimal? TotalCostAmt { get; set; }

    [Column("SPP_AMT", TypeName = "money")]
    public decimal? SppAmt { get; set; }

    [Column("REMEDIATION_AMT", TypeName = "money")]
    public decimal? RemediationAmt { get; set; }

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
