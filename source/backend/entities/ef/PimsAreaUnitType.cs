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
    /// The area unit used for measuring Properties.  The units must be in metric: square metres or hectares.
    /// </summary>
    [Key]
    [Column("AREA_UNIT_TYPE_CODE")]
    [StringLength(20)]
    public string AreaUnitTypeCode { get; set; }

    /// <summary>
    /// Translation of the code value into a description that can be displayed to the user.
    /// </summary>
    [Required]
    [Column("DESCRIPTION")]
    [StringLength(200)]
    public string Description { get; set; }

    /// <summary>
    /// Indicates if the code value is still active or is now disabled.
    /// </summary>
    [Column("IS_DISABLED")]
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Order in which to display the code values, if required.
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

    [InverseProperty("PropertyAreaUnitTypeCodeNavigation")]
    public virtual ICollection<PimsProperty> PimsProperties { get; set; } = new List<PimsProperty>();

    [InverseProperty("AreaUnitTypeCodeNavigation")]
    public virtual ICollection<PimsPropertyLease> PimsPropertyLeases { get; set; } = new List<PimsPropertyLease>();

    [InverseProperty("AreaUnitTypeCodeNavigation")]
    public virtual ICollection<PimsTake> PimsTakes { get; set; } = new List<PimsTake>();
}
