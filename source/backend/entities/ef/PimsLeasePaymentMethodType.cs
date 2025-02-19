﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Describes the type of payment method for a lease.
/// </summary>
[Table("PIMS_LEASE_PAYMENT_METHOD_TYPE")]
public partial class PimsLeasePaymentMethodType
{
    /// <summary>
    /// Payment method type code
    /// </summary>
    [Key]
    [Column("LEASE_PAYMENT_METHOD_TYPE_CODE")]
    [StringLength(20)]
    public string LeasePaymentMethodTypeCode { get; set; }

    /// <summary>
    /// Payment method type description
    /// </summary>
    [Required]
    [Column("DESCRIPTION")]
    [StringLength(200)]
    public string Description { get; set; }

    /// <summary>
    /// Is this code disabled?
    /// </summary>
    [Column("IS_DISABLED")]
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Display order of the descriptions
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

    [InverseProperty("LeasePaymentMethodTypeCodeNavigation")]
    public virtual ICollection<PimsLeasePayment> PimsLeasePayments { get; set; } = new List<PimsLeasePayment>();
}
