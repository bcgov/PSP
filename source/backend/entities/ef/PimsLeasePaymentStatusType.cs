using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Describes the status of forecast payments.
/// </summary>
[Table("PIMS_LEASE_PAYMENT_STATUS_TYPE")]
public partial class PimsLeasePaymentStatusType
{
    /// <summary>
    /// Payment status type code.
    /// </summary>
    [Key]
    [Column("LEASE_PAYMENT_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string LeasePaymentStatusTypeCode { get; set; }

    /// <summary>
    /// Payment status type description.
    /// </summary>
    [Required]
    [Column("DESCRIPTION")]
    [StringLength(200)]
    public string Description { get; set; }

    /// <summary>
    /// Is this code disabled?.
    /// </summary>
    [Column("IS_DISABLED")]
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Display order of the descriptions.
    /// </summary>
    [Column("DISPLAY_ORDER")]
    public int? DisplayOrder { get; set; }

    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long ConcurrencyControlNumber { get; set; }

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

    [InverseProperty("LeasePaymentStatusTypeCodeNavigation")]
    public virtual ICollection<PimsLeasePayment> PimsLeasePayments { get; set; } = new List<PimsLeasePayment>();
}
