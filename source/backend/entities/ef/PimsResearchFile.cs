using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_RESEARCH_FILE")]
    [Index(nameof(RequestorName), Name = "RESRCH_REQUESTOR_NAME_IDX")]
    [Index(nameof(RequestorOrganization), Name = "RESRCH_REQUESTOR_ORGANIZATION_IDX")]
    [Index(nameof(RequestSourceTypeCode), Name = "RESRCH_REQUEST_SOURCE_TYPE_CODE_IDX")]
    [Index(nameof(ResearchFileStatusTypeCode), Name = "RESRCH_RESEARCH_FILE_STATUS_TYPE_CODE_IDX")]
    public partial class PimsResearchFile
    {
        public PimsResearchFile()
        {
            PimsPropertyResearchFiles = new HashSet<PimsPropertyResearchFile>();
            PimsResearchFileDocuments = new HashSet<PimsResearchFileDocument>();
            PimsResearchFileNotes = new HashSet<PimsResearchFileNote>();
            PimsResearchFileProjects = new HashSet<PimsResearchFileProject>();
            PimsResearchFilePurposes = new HashSet<PimsResearchFilePurpose>();
        }

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
        [Column("REQUEST_DATE", TypeName = "date")]
        public DateTime? RequestDate { get; set; }
        [Column("ROAD_NAME")]
        public string RoadName { get; set; }
        [Column("ROAD_ALIAS")]
        public string RoadAlias { get; set; }
        [Column("REQUEST_DESCRIPTION")]
        public string RequestDescription { get; set; }
        [Column("REQUEST_SOURCE_DESCRIPTION")]
        [StringLength(2000)]
        public string RequestSourceDescription { get; set; }
        [Column("RESEARCH_RESULT")]
        public string ResearchResult { get; set; }
        [Column("IS_EXPROPRIATION")]
        public bool? IsExpropriation { get; set; }
        [Column("EXPROPRIATION_NOTES")]
        public string ExpropriationNotes { get; set; }
        [Column("RESEARCH_COMPLETION_DATE", TypeName = "date")]
        public DateTime? ResearchCompletionDate { get; set; }
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

        [ForeignKey(nameof(RequestSourceTypeCode))]
        [InverseProperty(nameof(PimsRequestSourceType.PimsResearchFiles))]
        public virtual PimsRequestSourceType RequestSourceTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(RequestorName))]
        [InverseProperty(nameof(PimsPerson.PimsResearchFiles))]
        public virtual PimsPerson RequestorNameNavigation { get; set; }
        [ForeignKey(nameof(RequestorOrganization))]
        [InverseProperty(nameof(PimsOrganization.PimsResearchFiles))]
        public virtual PimsOrganization RequestorOrganizationNavigation { get; set; }
        [ForeignKey(nameof(ResearchFileStatusTypeCode))]
        [InverseProperty(nameof(PimsResearchFileStatusType.PimsResearchFiles))]
        public virtual PimsResearchFileStatusType ResearchFileStatusTypeCodeNavigation { get; set; }
        [InverseProperty(nameof(PimsPropertyResearchFile.ResearchFile))]
        public virtual ICollection<PimsPropertyResearchFile> PimsPropertyResearchFiles { get; set; }
        [InverseProperty(nameof(PimsResearchFileDocument.ResearchFile))]
        public virtual ICollection<PimsResearchFileDocument> PimsResearchFileDocuments { get; set; }
        [InverseProperty(nameof(PimsResearchFileNote.ResearchFile))]
        public virtual ICollection<PimsResearchFileNote> PimsResearchFileNotes { get; set; }
        [InverseProperty(nameof(PimsResearchFileProject.ResearchFile))]
        public virtual ICollection<PimsResearchFileProject> PimsResearchFileProjects { get; set; }
        [InverseProperty(nameof(PimsResearchFilePurpose.ResearchFile))]
        public virtual ICollection<PimsResearchFilePurpose> PimsResearchFilePurposes { get; set; }
    }
}
