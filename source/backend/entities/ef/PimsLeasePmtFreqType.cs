using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Describes the frequency of payments for a lease.
/// </summary>
[Table("PIMS_LEASE_PMT_FREQ_TYPE")]
public partial class PimsLeasePmtFreqType
{
    /// <summary>
    /// Payment frequency type code
    /// </summary>
    [Key]
    [Column("LEASE_PMT_FREQ_TYPE_CODE")]
    [StringLength(20)]
    public string LeasePmtFreqTypeCode { get; set; }

    /// <summary>
    /// Payment frequency type code description
    /// </summary>
    [Required]
    [Column("DESCRIPTION")]
    [StringLength(200)]
    public string Description { get; set; }

    /// <summary>
    /// Indicates that the record is disabled.
    /// </summary>
    [Column("IS_DISABLED")]
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Display order of the descriptions.
    /// </summary>
    [Column("DISPLAY_ORDER")]
    public int? DisplayOrder { get; set; }

    /// <summary>
    /// Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o
    /// </summary>
    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long ConcurrencyControlNumber { get; set; }

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

    [InverseProperty("LeasePmtFreqTypeCodeNavigation")]
    public virtual ICollection<PimsLeasePayment> PimsLeasePayments { get; set; } = new List<PimsLeasePayment>();

    [InverseProperty("AddlRentFreqNavigation")]
    public virtual ICollection<PimsLeasePeriod> PimsLeasePeriodAddlRentFreqNavigations { get; set; } = new List<PimsLeasePeriod>();

    [InverseProperty("LeasePmtFreqTypeCodeNavigation")]
    public virtual ICollection<PimsLeasePeriod> PimsLeasePeriodLeasePmtFreqTypeCodeNavigations { get; set; } = new List<PimsLeasePeriod>();

    [InverseProperty("VblRentFreqNavigation")]
    public virtual ICollection<PimsLeasePeriod> PimsLeasePeriodVblRentFreqNavigations { get; set; } = new List<PimsLeasePeriod>();
}
