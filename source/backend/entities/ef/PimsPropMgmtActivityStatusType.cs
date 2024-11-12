using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Code table to describe the status of the property management activity.
/// </summary>
[Table("PIMS_PROP_MGMT_ACTIVITY_STATUS_TYPE")]
public partial class PimsPropMgmtActivityStatusType
{
    /// <summary>
    /// Code representing the status of the property management activity.
    /// </summary>
    [Key]
    [Column("PROP_MGMT_ACTIVITY_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string PropMgmtActivityStatusTypeCode { get; set; }

    /// <summary>
    /// Description of the status of the property management status.
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

    [InverseProperty("PropMgmtActivityStatusTypeCodeNavigation")]
    public virtual ICollection<PimsPropertyActivity> PimsPropertyActivities { get; set; } = new List<PimsPropertyActivity>();
}
