using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_RESEARCH_FILE_PURPOSE")]
    [Index(nameof(ResearchFileId), Name = "RSFLPR_RESEARCH_FILE_ID_IDX")]
    [Index(nameof(ResearchPurposeTypeCode), Name = "RSFLPR_RESEARCH_PURPOSE_TYPE_CODE_IDX")]
    [Index(nameof(ResearchPurposeTypeCode), nameof(ResearchFileId), Name = "RSFLPR_RSRCH_FL_RSRCH_FL_PURP_TUC", IsUnique = true)]
    public partial class PimsResearchFilePurpose
    {
        [Key]
        [Column("RESEARCH_FILE_PURPOSE_ID")]
        public long ResearchFilePurposeId { get; set; }
        [Column("RESEARCH_FILE_ID")]
        public long ResearchFileId { get; set; }
        [Required]
        [Column("RESEARCH_PURPOSE_TYPE_CODE")]
        [StringLength(20)]
        public string ResearchPurposeTypeCode { get; set; }
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

        [ForeignKey(nameof(ResearchFileId))]
        [InverseProperty(nameof(PimsResearchFile.PimsResearchFilePurposes))]
        public virtual PimsResearchFile ResearchFile { get; set; }
        [ForeignKey(nameof(ResearchPurposeTypeCode))]
        [InverseProperty(nameof(PimsResearchPurposeType.PimsResearchFilePurposes))]
        public virtual PimsResearchPurposeType ResearchPurposeTypeCodeNavigation { get; set; }
    }
}
