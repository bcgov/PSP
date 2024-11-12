using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Code table to describe property adjacent land type.
/// </summary>
[Table("PIMS_RESEARCH_FILE_STATUS_TYPE")]
public partial class PimsResearchFileStatusType
{
    /// <summary>
    /// Code indicating the status of the research file.
    /// </summary>
    [Key]
    [Column("RESEARCH_FILE_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string ResearchFileStatusTypeCode { get; set; }

    /// <summary>
    /// Description of the code indicating the status of the research file.
    /// </summary>
    [Required]
    [Column("DESCRIPTION")]
    [StringLength(200)]
    public string Description { get; set; }

    /// <summary>
    /// Indicates if the code is disabled.
    /// </summary>
    [Column("IS_DISABLED")]
    public bool? IsDisabled { get; set; }

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

    [InverseProperty("ResearchFileStatusTypeCodeNavigation")]
    public virtual ICollection<PimsResearchFile> PimsResearchFiles { get; set; } = new List<PimsResearchFile>();
}
