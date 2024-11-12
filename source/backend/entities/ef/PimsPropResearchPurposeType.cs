using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Code table to describe the purpose ot the property research
/// </summary>
[Table("PIMS_PROP_RESEARCH_PURPOSE_TYPE")]
public partial class PimsPropResearchPurposeType
{
    /// <summary>
    /// Code indicating the purpose of the property research
    /// </summary>
    [Key]
    [Column("PROP_RESEARCH_PURPOSE_TYPE_CODE")]
    [StringLength(20)]
    public string PropResearchPurposeTypeCode { get; set; }

    /// <summary>
    /// Description of the code indicating the purpose of the property research
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

    [InverseProperty("PropResearchPurposeTypeCodeNavigation")]
    public virtual ICollection<PimsPrfPropResearchPurposeType> PimsPrfPropResearchPurposeTypes { get; set; } = new List<PimsPrfPropResearchPurposeType>();
}
