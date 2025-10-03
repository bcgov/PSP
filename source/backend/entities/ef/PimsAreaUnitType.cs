using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// The area unit used for measuring Properties.  The units must be in metric: square metres or hectares.
/// </summary>
[Table("PIMS_AREA_UNIT_TYPE")]
public partial class PimsAreaUnitType
{
    /// <summary>
    /// Code value of the  area unit type.  The area unit used for measuring Properties.  The units must be in metric: square metres or hectares.
    /// </summary>
    [Key]
    [Column("AREA_UNIT_TYPE_CODE")]
    [StringLength(20)]
    public string AreaUnitTypeCode { get; set; }

    /// <summary>
    /// Description of the  area unit type.  Translation of the code value into a description that can be displayed to the user.
    /// </summary>
    [Required]
    [Column("DESCRIPTION")]
    [StringLength(200)]
    public string Description { get; set; }

    /// <summary>
    /// Indicates if the record is disabled and therefore not selectable or displayed.
    /// </summary>
    [Column("IS_DISABLED")]
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Designates a preferred presentation order of the code descriptions.
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

    [InverseProperty("PropertyAreaUnitTypeCodeNavigation")]
    public virtual ICollection<PimsProperty> PimsProperties { get; set; } = new List<PimsProperty>();

    [InverseProperty("AreaUnitTypeCodeNavigation")]
    public virtual ICollection<PimsPropertyLease> PimsPropertyLeases { get; set; } = new List<PimsPropertyLease>();

    [InverseProperty("AreaUnitTypeCodeNavigation")]
    public virtual ICollection<PimsTake> PimsTakes { get; set; } = new List<PimsTake>();
}
