using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Code table describing the type of lessor on a lease.
/// </summary>
[Table("PIMS_LESSOR_TYPE")]
public partial class PimsLessorType
{
    /// <summary>
    /// Code representing the types of lessors on a lease.
    /// </summary>
    [Key]
    [Column("LESSOR_TYPE_CODE")]
    [StringLength(20)]
    public string LessorTypeCode { get; set; }

    /// <summary>
    /// Description of the types of lessors on a lease.
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

    [InverseProperty("LessorTypeCodeNavigation")]
    public virtual ICollection<PimsLeaseStakeholder> PimsLeaseStakeholders { get; set; } = new List<PimsLeaseStakeholder>();
}
