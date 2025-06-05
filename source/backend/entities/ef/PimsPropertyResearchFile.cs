using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Pims.Dal.Entities;

/// <summary>
/// Associates a property with a research file.
/// </summary>
[Table("PIMS_PROPERTY_RESEARCH_FILE")]
[Index("PropertyId", Name = "PRSCRC_PROPERTY_ID_IDX")]
[Index("ResearchFileId", Name = "PRSCRC_RESEARCH_FILE_ID_IDX")]
[Index("ResearchFileId", "PropertyId", Name = "PRSCRC_RSRCH_FILE_PROP_TUC", IsUnique = true)]
public partial class PimsPropertyResearchFile
{
    /// <summary>
    /// Generated surrogate primary key.
    /// </summary>
    [Key]
    [Column("PROPERTY_RESEARCH_FILE_ID")]
    public long PropertyResearchFileId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_PROPERTY table.
    /// </summary>
    [Column("PROPERTY_ID")]
    public long PropertyId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_RESEARCH_FILE table.
    /// </summary>
    [Column("RESEARCH_FILE_ID")]
    public long ResearchFileId { get; set; }

    /// <summary>
    /// Descriptive reference for the property being researched.
    /// </summary>
    [Column("PROPERTY_NAME")]
    [StringLength(500)]
    public string PropertyName { get; set; }

    /// <summary>
    /// Specifies the display order of the property (PSP-10521).
    /// </summary>
    [Column("DISPLAY_ORDER")]
    public int? DisplayOrder { get; set; }

    /// <summary>
    /// Indicates whether a legal opinion is required (0 = No, 1 = Yes, null = Unknown)
    /// </summary>
    [Column("IS_LEGAL_OPINION_REQUIRED")]
    public bool? IsLegalOpinionRequired { get; set; }

    /// <summary>
    /// Indicates whether a legal opinion was obtained (0 = No, 1 = Yes, null = Unknown)
    /// </summary>
    [Column("IS_LEGAL_OPINION_OBTAINED")]
    public bool? IsLegalOpinionObtained { get; set; }

    /// <summary>
    /// URL / reference to a LAN Drive
    /// </summary>
    [Column("DOCUMENT_REFERENCE")]
    [StringLength(2000)]
    public string DocumentReference { get; set; }

    /// <summary>
    /// Summary of the property research.
    /// </summary>
    [Column("RESEARCH_SUMMARY")]
    [StringLength(4000)]
    public string ResearchSummary { get; set; }

    /// <summary>
    /// Geospatial location (pin) of property
    /// </summary>
    [Column("LOCATION", TypeName = "geometry")]
    public Geometry Location { get; set; }

    /// <summary>
    /// Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o
    /// </summary>
    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long ConcurrencyControlNumber { get; set; }

    /// <summary>
    /// The date and time the user created the record.
    /// </summary>
    [Column("APP_CREATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppCreateTimestamp { get; set; }

    /// <summary>
    /// The user account that created the record.
    /// </summary>
    [Required]
    [Column("APP_CREATE_USERID")]
    [StringLength(30)]
    public string AppCreateUserid { get; set; }

    /// <summary>
    /// The GUID of the user account that created the record.
    /// </summary>
    [Column("APP_CREATE_USER_GUID")]
    public Guid? AppCreateUserGuid { get; set; }

    /// <summary>
    /// The directory of the user account that created the record.
    /// </summary>
    [Required]
    [Column("APP_CREATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppCreateUserDirectory { get; set; }

    /// <summary>
    /// The date and time the user updated the record.
    /// </summary>
    [Column("APP_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppLastUpdateTimestamp { get; set; }

    /// <summary>
    /// The user account that updated the record.
    /// </summary>
    [Required]
    [Column("APP_LAST_UPDATE_USERID")]
    [StringLength(30)]
    public string AppLastUpdateUserid { get; set; }

    /// <summary>
    /// The GUID of the user account that updated the record.
    /// </summary>
    [Column("APP_LAST_UPDATE_USER_GUID")]
    public Guid? AppLastUpdateUserGuid { get; set; }

    /// <summary>
    /// The directory of the user account that updated the record.
    /// </summary>
    [Required]
    [Column("APP_LAST_UPDATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppLastUpdateUserDirectory { get; set; }

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

    [InverseProperty("PropertyResearchFile")]
    public virtual ICollection<PimsPrfPropResearchPurposeTyp> PimsPrfPropResearchPurposeTyps { get; set; } = new List<PimsPrfPropResearchPurposeTyp>();

    [ForeignKey("PropertyId")]
    [InverseProperty("PimsPropertyResearchFiles")]
    public virtual PimsProperty Property { get; set; }

    [ForeignKey("ResearchFileId")]
    [InverseProperty("PimsPropertyResearchFiles")]
    public virtual PimsResearchFile ResearchFile { get; set; }
}
