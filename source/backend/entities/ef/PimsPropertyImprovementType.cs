using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Description of the types of improvements made to a property during the lease.
/// </summary>
[Table("PIMS_PROPERTY_IMPROVEMENT_TYPE")]
public partial class PimsPropertyImprovementType
{
    /// <summary>
    /// Code value of the types of improvements made to a property during the lease.
    /// </summary>
    [Key]
    [Column("PROPERTY_IMPROVEMENT_TYPE_CODE")]
    [StringLength(20)]
    public string PropertyImprovementTypeCode { get; set; }

    /// <summary>
    /// Code description of the types of improvements made to a property during the lease.
    /// </summary>
    [Required]
    [Column("DESCRIPTION")]
    [StringLength(200)]
    public string Description { get; set; }

    /// <summary>
    /// Indicates if the code is disabled.
    /// </summary>
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

    [InverseProperty("PropertyImprovementTypeCodeNavigation")]
    public virtual ICollection<PimsPropertyImprovement> PimsPropertyImprovements { get; set; } = new List<PimsPropertyImprovement>();
}
