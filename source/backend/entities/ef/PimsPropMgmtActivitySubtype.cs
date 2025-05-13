using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Code table to describe the subtype of property management.
/// </summary>
[Table("PIMS_PROP_MGMT_ACTIVITY_SUBTYPE")]
[Index("PropMgmtActivityTypeCode", Name = "PRACST_PROP_MGMT_ACTIVITY_TYPE_CODE_IDX")]
public partial class PimsPropMgmtActivitySubtype
{
    /// <summary>
    /// Code representing the subtype of property management.
    /// </summary>
    [Key]
    [Column("PROP_MGMT_ACTIVITY_SUBTYPE_CODE")]
    [StringLength(20)]
    public string PropMgmtActivitySubtypeCode { get; set; }

    /// <summary>
    /// Code representing the type of property management.
    /// </summary>
    [Required]
    [Column("PROP_MGMT_ACTIVITY_TYPE_CODE")]
    [StringLength(20)]
    public string PropMgmtActivityTypeCode { get; set; }

    /// <summary>
    /// Description of the subtype of property management.
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

    /// <summary>
    /// Force the display order of the codes.
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

    [InverseProperty("PropMgmtActivitySubtypeCodeNavigation")]
    public virtual ICollection<PimsPropertyActivity> PimsPropertyActivities { get; set; } = new List<PimsPropertyActivity>();

    [ForeignKey("PropMgmtActivityTypeCode")]
    [InverseProperty("PimsPropMgmtActivitySubtypes")]
    public virtual PimsPropMgmtActivityType PropMgmtActivityTypeCodeNavigation { get; set; }
}
