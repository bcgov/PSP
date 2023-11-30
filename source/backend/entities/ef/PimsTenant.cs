using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Deprecated table to support legacy CITZ-PIMS application code.  This table will be removed once the code dependency is removed from the system.
/// </summary>
[Table("PIMS_TENANT")]
public partial class PimsTenant
{
    /// <summary>
    /// Auto-sequenced unique key value
    /// </summary>
    [Key]
    [Column("TENANT_ID")]
    public long TenantId { get; set; }

    /// <summary>
    /// Code value for entry
    /// </summary>
    [Required]
    [Column("CODE")]
    [StringLength(6)]
    public string Code { get; set; }

    /// <summary>
    /// Name of the entry for display purposes
    /// </summary>
    [Required]
    [Column("NAME")]
    [StringLength(150)]
    public string Name { get; set; }

    /// <summary>
    /// Description of the entry for display purposes
    /// </summary>
    [Column("DESCRIPTION")]
    [StringLength(500)]
    public string Description { get; set; }

    /// <summary>
    /// Serialized JSON value for the configuration
    /// </summary>
    [Required]
    [Column("SETTINGS")]
    [StringLength(2000)]
    public string Settings { get; set; }

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
}
