using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Describes which organization is responsible for this lease.
/// </summary>
[Table("PIMS_LEASE_RESPONSIBILITY_TYPE")]
public partial class PimsLeaseResponsibilityType
{
    /// <summary>
    /// Code value of the organization responsible for this lease.
    /// </summary>
    [Key]
    [Column("LEASE_RESPONSIBILITY_TYPE_CODE")]
    [StringLength(20)]
    public string LeaseResponsibilityTypeCode { get; set; }

    /// <summary>
    /// Description of the organization responsible for this lease.
    /// </summary>
    [Required]
    [Column("DESCRIPTION")]
    [StringLength(200)]
    public string Description { get; set; }

    [Column("IS_DISABLED")]
    public bool IsDisabled { get; set; }

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

    [InverseProperty("LeaseResponsibilityTypeCodeNavigation")]
    public virtual ICollection<PimsLease> PimsLeases { get; set; } = new List<PimsLease>();
}
