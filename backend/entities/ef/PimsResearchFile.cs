using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_RESEARCH_FILE")]
    [Index(nameof(ResearchFileStatusTypeCode), Name = "RESRCH_RESEARCH_FILE_STATUS_TYPE_CODE_IDX")]
    public partial class PimsResearchFile
    {
        public PimsResearchFile()
        {
            PimsPropertyResearchFiles = new HashSet<PimsPropertyResearchFile>();
        }

        [Key]
        [Column("RESEARCH_FILE_ID")]
        public long ResearchFileId { get; set; }
        [Required]
        [Column("RESEARCH_FILE_STATUS_TYPE_CODE")]
        [StringLength(20)]
        public string ResearchFileStatusTypeCode { get; set; }
        [Required]
        [Column("NAME")]
        [StringLength(250)]
        public string Name { get; set; }
        [Required]
        [Column("RFILE_NUMBER")]
        [StringLength(25)]
        public string RfileNumber { get; set; }
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

        [ForeignKey(nameof(ResearchFileStatusTypeCode))]
        [InverseProperty(nameof(PimsResearchFileStatusType.PimsResearchFiles))]
        public virtual PimsResearchFileStatusType ResearchFileStatusTypeCodeNavigation { get; set; }
        [InverseProperty(nameof(PimsPropertyResearchFile.ResearchFile))]
        public virtual ICollection<PimsPropertyResearchFile> PimsPropertyResearchFiles { get; set; }
    }
}
