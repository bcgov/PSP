using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Code table describing the type of tenant on a lease.
/// </summary>
[Table("PIMS_TENANT_TYPE")]
public partial class PimsTenantType
{
    /// <summary>
    /// Code representing the types of tenants on a lease.
    /// </summary>
    [Key]
    [Column("TENANT_TYPE_CODE")]
    [StringLength(20)]
    public string TenantTypeCode { get; set; }

    /// <summary>
    /// Description of the types of tenants on a lease.
    /// </summary>
    [Required]
    [Column("DESCRIPTION")]
    [StringLength(200)]
    public string Description { get; set; }

    /// <summary>
    /// Indicates if the code is currently active.
    /// </summary>
    [Column("IS_DISABLED")]
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Specifies a specific order to visually present the code.
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

    [InverseProperty("TenantTypeCodeNavigation")]
    public virtual ICollection<PimsLeaseTenant> PimsLeaseTenants { get; set; } = new List<PimsLeaseTenant>();
}
