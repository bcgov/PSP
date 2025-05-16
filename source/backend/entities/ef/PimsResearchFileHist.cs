using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

[Table("PIMS_RESEARCH_FILE_HIST")]
[Index("ResearchFileHistId", "EndDateHist", Name = "PIMS_RESRCH_H_UK", IsUnique = true)]
public partial class PimsResearchFileHist
{
    [Key]
    [Column("_RESEARCH_FILE_HIST_ID")]
    public long ResearchFileHistId { get; set; }

    [Column("EFFECTIVE_DATE_HIST", TypeName = "datetime")]
    public DateTime EffectiveDateHist { get; set; }

    [Column("END_DATE_HIST", TypeName = "datetime")]
    public DateTime? EndDateHist { get; set; }

    [Column("RESEARCH_FILE_ID")]
    public long ResearchFileId { get; set; }

    [Required]
    [Column("RESEARCH_FILE_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string ResearchFileStatusTypeCode { get; set; }

    [Column("REQUEST_SOURCE_TYPE_CODE")]
    [StringLength(20)]
    public string RequestSourceTypeCode { get; set; }

    [Column("REQUESTOR_NAME")]
    public long? RequestorName { get; set; }

    [Column("REQUESTOR_ORGANIZATION")]
    public long? RequestorOrganization { get; set; }

    [Required]
    [Column("NAME")]
    [StringLength(250)]
    public string Name { get; set; }

    [Required]
    [Column("RFILE_NUMBER")]
    [StringLength(25)]
    public string RfileNumber { get; set; }

    [Column("REQUEST_DATE")]
    public DateOnly? RequestDate { get; set; }

    [Column("ROAD_NAME")]
    [StringLength(200)]
    public string RoadName { get; set; }

    [Column("ROAD_ALIAS")]
    [StringLength(200)]
    public string RoadAlias { get; set; }

    [Column("REQUEST_DESCRIPTION")]
    [StringLength(3000)]
    public string RequestDescription { get; set; }

    [Column("REQUEST_SOURCE_DESCRIPTION")]
    [StringLength(2000)]
    public string RequestSourceDescription { get; set; }

    [Column("RESEARCH_RESULT")]
    [StringLength(2000)]
    public string ResearchResult { get; set; }

    [Column("IS_EXPROPRIATION")]
    public bool? IsExpropriation { get; set; }

    [Column("EXPROPRIATION_NOTES")]
    [StringLength(1000)]
    public string ExpropriationNotes { get; set; }

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
}
