using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Describes the status of the lease term
/// </summary>
[Table("PIMS_LEASE_TERM_STATUS_TYPE")]
public partial class PimsLeaseTermStatusType
{
    /// <summary>
    /// Code value of the status of the lease term
    /// </summary>
    [Key]
    [Column("LEASE_TERM_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string LeaseTermStatusTypeCode { get; set; }

    /// <summary>
    /// Description of the status of the lease term
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

    [InverseProperty("LeaseTermStatusTypeCodeNavigation")]
    public virtual ICollection<PimsLeaseTerm> PimsLeaseTerms { get; set; } = new List<PimsLeaseTerm>();
}
