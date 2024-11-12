using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Description of the surplus property type.
/// </summary>
[Table("PIMS_SURPLUS_DECLARATION_TYPE")]
public partial class PimsSurplusDeclarationType
{
    /// <summary>
    /// Code value of the surplus property type
    /// </summary>
    [Key]
    [Column("SURPLUS_DECLARATION_TYPE_CODE")]
    [StringLength(20)]
    public string SurplusDeclarationTypeCode { get; set; }

    /// <summary>
    /// Code description of the surplus property type
    /// </summary>
    [Required]
    [Column("DESCRIPTION")]
    [StringLength(200)]
    public string Description { get; set; }

    /// <summary>
    /// Indicates that the code value is disabled
    /// </summary>
    [Column("IS_DISABLED")]
    public bool IsDisabled { get; set; }

    [Column("DISPLAY_ORDER")]
    public int? DisplayOrder { get; set; }

    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long? ConcurrencyControlNumber { get; set; }

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

    [InverseProperty("SurplusDeclarationTypeCodeNavigation")]
    public virtual ICollection<PimsProperty> PimsProperties { get; set; } = new List<PimsProperty>();
}
