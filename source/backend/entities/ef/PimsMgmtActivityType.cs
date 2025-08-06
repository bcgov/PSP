using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Code table to describe the type of property management.
/// </summary>
[Table("PIMS_MGMT_ACTIVITY_TYPE")]
public partial class PimsMgmtActivityType
{
    /// <summary>
    /// Code representing the type of property management.
    /// </summary>
    [Key]
    [Column("MGMT_ACTIVITY_TYPE_CODE")]
    [StringLength(20)]
    public string MgmtActivityTypeCode { get; set; }

    /// <summary>
    /// Description of the type of property management.
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

    [InverseProperty("MgmtActivityTypeCodeNavigation")]
    public virtual ICollection<PimsManagementActivity> PimsManagementActivities { get; set; } = new List<PimsManagementActivity>();

    [InverseProperty("MgmtActivityTypeCodeNavigation")]
    public virtual ICollection<PimsMgmtActivitySubtype> PimsMgmtActivitySubtypes { get; set; } = new List<PimsMgmtActivitySubtype>();
}
