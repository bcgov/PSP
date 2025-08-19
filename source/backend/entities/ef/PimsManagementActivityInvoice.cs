using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Defines the activities that are associated with this property.
/// </summary>
[Table("PIMS_MANAGEMENT_ACTIVITY_INVOICE")]
[Index("ManagementActivityId", Name = "MAAINV_MANAGEMENT_ACTIVITY_ID_IDX")]
public partial class PimsManagementActivityInvoice
{
    /// <summary>
    /// Generated surrogate primary key.
    /// </summary>
    [Key]
    [Column("MANAGEMENT_ACTIVITY_INVOICE_ID")]
    public long ManagementActivityInvoiceId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_MANAGEMENT_ACTIVITY table.
    /// </summary>
    [Column("MANAGEMENT_ACTIVITY_ID")]
    public long ManagementActivityId { get; set; }

    /// <summary>
    /// Date of the invoice
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
    /// Subtotal of the invoice,
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

    [ForeignKey("ManagementActivityId")]
    [InverseProperty("PimsManagementActivityInvoices")]
    public virtual PimsManagementActivity ManagementActivity { get; set; }
}
