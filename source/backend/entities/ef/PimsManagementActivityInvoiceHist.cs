using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

[Table("PIMS_MANAGEMENT_ACTIVITY_INVOICE_HIST")]
[Index("ManagementActivityInvoiceHistId", "EndDateHist", Name = "PIMS_MAAINV_H_UK", IsUnique = true)]
public partial class PimsManagementActivityInvoiceHist
{
    [Key]
    [Column("_MANAGEMENT_ACTIVITY_INVOICE_HIST_ID")]
    public long ManagementActivityInvoiceHistId { get; set; }

    [Column("EFFECTIVE_DATE_HIST", TypeName = "datetime")]
    public DateTime EffectiveDateHist { get; set; }

    [Column("END_DATE_HIST", TypeName = "datetime")]
    public DateTime? EndDateHist { get; set; }

    [Column("MANAGEMENT_ACTIVITY_INVOICE_ID")]
    public long ManagementActivityInvoiceId { get; set; }

    [Column("MANAGEMENT_ACTIVITY_ID")]
    public long ManagementActivityId { get; set; }

    [Column("INVOICE_DT")]
    public DateOnly InvoiceDt { get; set; }

    [Column("INVOICE_NUM")]
    [StringLength(50)]
    public string InvoiceNum { get; set; }

    [Column("DESCRIPTION")]
    [StringLength(1000)]
    public string Description { get; set; }

    [Column("PRETAX_AMT", TypeName = "money")]
    public decimal PretaxAmt { get; set; }

    [Column("GST_AMT", TypeName = "money")]
    public decimal? GstAmt { get; set; }

    [Column("PST_AMT", TypeName = "money")]
    public decimal? PstAmt { get; set; }

    [Column("TOTAL_AMT", TypeName = "money")]
    public decimal? TotalAmt { get; set; }

    [Column("IS_PST_REQUIRED")]
    public bool IsPstRequired { get; set; }

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
