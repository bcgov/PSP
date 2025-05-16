using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Property research file
/// </summary>
[Table("PIMS_RESEARCH_FILE")]
[Index("RequestorName", Name = "RESRCH_REQUESTOR_NAME_IDX")]
[Index("RequestorOrganization", Name = "RESRCH_REQUESTOR_ORGANIZATION_IDX")]
[Index("RequestSourceTypeCode", Name = "RESRCH_REQUEST_SOURCE_TYPE_CODE_IDX")]
[Index("ResearchFileStatusTypeCode", Name = "RESRCH_RESEARCH_FILE_STATUS_TYPE_CODE_IDX")]
public partial class PimsResearchFile
{
    [Key]
    [Column("RESEARCH_FILE_ID")]
    public long ResearchFileId { get; set; }

    [Required]
    [Column("RESEARCH_FILE_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string ResearchFileStatusTypeCode { get; set; }

    [Column("REQUEST_SOURCE_TYPE_CODE")]
    [StringLength(20)]
    public string RequestSourceTypeCode { get; set; }

    /// <summary>
    /// Name of the research requestor.
    /// </summary>
    [Column("REQUESTOR_NAME")]
    public long? RequestorName { get; set; }

    /// <summary>
    /// Organization associated with the research requestor.
    /// </summary>
    [Column("REQUESTOR_ORGANIZATION")]
    public long? RequestorOrganization { get; set; }

    /// <summary>
    /// Name given to the research file.
    /// </summary>
    [Required]
    [Column("NAME")]
    [StringLength(250)]
    public string Name { get; set; }

    /// <summary>
    /// R-File number assigned to the research file, formatted value from PIMS_RFILE_NUMBER_SEQ sequence generator
    /// </summary>
    [Required]
    [Column("RFILE_NUMBER")]
    [StringLength(25)]
    public string RfileNumber { get; set; }

    /// <summary>
    /// Date of the research request.
    /// </summary>
    [Column("REQUEST_DATE")]
    public DateOnly? RequestDate { get; set; }

    /// <summary>
    /// Name(s) of roads associated with this research request.
    /// </summary>
    [Column("ROAD_NAME")]
    [StringLength(200)]
    public string RoadName { get; set; }

    /// <summary>
    /// Alias(es) of roads associated with this research request.
    /// </summary>
    [Column("ROAD_ALIAS")]
    [StringLength(200)]
    public string RoadAlias { get; set; }

    /// <summary>
    /// Description of the research request.
    /// </summary>
    [Column("REQUEST_DESCRIPTION")]
    [StringLength(3000)]
    public string RequestDescription { get; set; }

    [Column("REQUEST_SOURCE_DESCRIPTION")]
    [StringLength(2000)]
    public string RequestSourceDescription { get; set; }

    /// <summary>
    /// Result of the research request.
    /// </summary>
    [Column("RESEARCH_RESULT")]
    [StringLength(2000)]
    public string ResearchResult { get; set; }

    /// <summary>
    /// Is this an expropriation?
    /// </summary>
    [Column("IS_EXPROPRIATION")]
    public bool? IsExpropriation { get; set; }

    /// <summary>
    /// Notes associated with an expropriation.
    /// </summary>
    [Column("EXPROPRIATION_NOTES")]
    [StringLength(1000)]
    public string ExpropriationNotes { get; set; }

    /// <summary>
    /// Date the research request was completed.
    /// </summary>
    [Column("RESEARCH_COMPLETION_DATE")]
    public DateOnly? ResearchCompletionDate { get; set; }

    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long ConcurrencyControlNumber { get; set; }

    [Column("APP_CREATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppCreateTimestamp { get; set; }

    [Required]
    [Column("APP_CREATE_USERID")]
    [StringLength(30)]
    public string AppCreateUserid { get; set; }

    [Column("APP_CREATE_USER_GUID")]
    public Guid? AppCreateUserGuid { get; set; }

    [Required]
    [Column("APP_CREATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppCreateUserDirectory { get; set; }

    [Column("APP_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppLastUpdateTimestamp { get; set; }

    [Required]
    [Column("APP_LAST_UPDATE_USERID")]
    [StringLength(30)]
    public string AppLastUpdateUserid { get; set; }

    [Column("APP_LAST_UPDATE_USER_GUID")]
    public Guid? AppLastUpdateUserGuid { get; set; }

    [Required]
    [Column("APP_LAST_UPDATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppLastUpdateUserDirectory { get; set; }

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

    [InverseProperty("ResearchFile")]
    public virtual ICollection<PimsPropertyResearchFile> PimsPropertyResearchFiles { get; set; } = new List<PimsPropertyResearchFile>();

    [InverseProperty("ResearchFile")]
    public virtual ICollection<PimsResearchFileDocument> PimsResearchFileDocuments { get; set; } = new List<PimsResearchFileDocument>();

    [InverseProperty("ResearchFile")]
    public virtual ICollection<PimsResearchFileNote> PimsResearchFileNotes { get; set; } = new List<PimsResearchFileNote>();

    [InverseProperty("ResearchFile")]
    public virtual ICollection<PimsResearchFileProject> PimsResearchFileProjects { get; set; } = new List<PimsResearchFileProject>();

    [InverseProperty("ResearchFile")]
    public virtual ICollection<PimsResearchFilePurpose> PimsResearchFilePurposes { get; set; } = new List<PimsResearchFilePurpose>();

    [ForeignKey("RequestSourceTypeCode")]
    [InverseProperty("PimsResearchFiles")]
    public virtual PimsRequestSourceType RequestSourceTypeCodeNavigation { get; set; }

    [ForeignKey("RequestorName")]
    [InverseProperty("PimsResearchFiles")]
    public virtual PimsPerson RequestorNameNavigation { get; set; }

    [ForeignKey("RequestorOrganization")]
    [InverseProperty("PimsResearchFiles")]
    public virtual PimsOrganization RequestorOrganizationNavigation { get; set; }

    [ForeignKey("ResearchFileStatusTypeCode")]
    [InverseProperty("PimsResearchFiles")]
    public virtual PimsResearchFileStatusType ResearchFileStatusTypeCodeNavigation { get; set; }
}
