using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Codified values for the acquistion file staff profile (role).
/// </summary>
[Table("PIMS_ACQ_FL_TEAM_PROFILE_TYPE")]
public partial class PimsAcqFlTeamProfileType
{
    /// <summary>
    /// Code value for the acquistion file staff/org profile (role).
    /// </summary>
    [Key]
    [Column("ACQ_FL_TEAM_PROFILE_TYPE_CODE")]
    [StringLength(20)]
    public string AcqFlTeamProfileTypeCode { get; set; }

    /// <summary>
    /// Description of the acquistion file staff/org profile (role).
    /// </summary>
    [Required]
    [Column("DESCRIPTION")]
    [StringLength(200)]
    public string Description { get; set; }

    /// <summary>
    /// Indicates if the code value is inactive.
    /// </summary>
    [Column("IS_DISABLED")]
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Designates a preferred presentation order of the code descriptions.
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

    [InverseProperty("AcqFlTeamProfileTypeCodeNavigation")]
    public virtual ICollection<PimsAcquisitionFileTeam> PimsAcquisitionFileTeams { get; set; } = new List<PimsAcquisitionFileTeam>();
}
