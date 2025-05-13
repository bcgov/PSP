using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Defines the activities that are associated with this property.
/// </summary>
[Table("PIMS_PROPERTY_ACTIVITY_INVOICE")]
[Index("PimsPropertyActivityId", Name = "PRACIN_PIMS_PROPERTY_ACTIVITY_ID_IDX")]
public partial class PimsPropertyActivityInvoice
{
    [Key]
    [Column("PROPERTY_ACTIVITY_INVOICE_ID")]
    public long PropertyActivityInvoiceId { get; set; }

    [Column("PIMS_PROPERTY_ACTIVITY_ID")]
    public long PimsPropertyActivityId { get; set; }

    /// <summary>
    /// Date of the invoice.
    /// </summary>
    [Column("INVOICE_DT")]
    public DateOnly InvoiceDt { get; set; }

    /// <summary>
    /// Number assigned to the invoice.
    /// </summary>
    [Column("INVOICE_NUM")]
    [StringLength(50)]
    public string InvoiceNum { get; set; }

    /// <summary>
    /// Description of the invoice.
    /// </summary>
    [Column("DESCRIPTION")]
    [StringLength(1000)]
    public string Description { get; set; }

    /// <summary>
    /// Subtotal of the invoice,.
    /// </summary>
    [Column("PRETAX_AMT", TypeName = "money")]
    public decimal PretaxAmt { get; set; }

    /// <summary>
    /// GST on the invoice.
    /// </summary>
    [Column("GST_AMT", TypeName = "money")]
    public decimal? GstAmt { get; set; }

    /// <summary>
    /// PST on the invoice.
    /// </summary>
    [Column("PST_AMT", TypeName = "money")]
    public decimal? PstAmt { get; set; }

    /// <summary>
    /// Total cost of the invoice.
    /// </summary>
    [Column("TOTAL_AMT", TypeName = "money")]
    public decimal? TotalAmt { get; set; }

    /// <summary>
    /// Indicates if the invoice requires PST.
    /// </summary>
    [Column("IS_PST_REQUIRED")]
    public bool IsPstRequired { get; set; }

    /// <summary>
    /// Indicates if the invoice is disabled.
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

    [ForeignKey("PimsPropertyActivityId")]
    [InverseProperty("PimsPropertyActivityInvoices")]
    public virtual PimsPropertyActivity PimsPropertyActivity { get; set; }
}
