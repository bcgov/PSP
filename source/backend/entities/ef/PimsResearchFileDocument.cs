using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_RESEARCH_FILE_DOCUMENT")]
    [Index(nameof(DocumentId), Name = "RFLDOC_DOCUMENT_ID_IDX")]
    [Index(nameof(DocumentId), nameof(ResearchFileId), Name = "RFLDOC_RESEARCH_FILE_DOCUMENT_TUC", IsUnique = true)]
    [Index(nameof(ResearchFileId), Name = "RFLDOC_RESEARCH_FILE_ID_IDX")]
    public partial class PimsResearchFileDocument
    {
        [Key]
        [Column("RESEARCH_FILE_DOCUMENT_ID")]
        public long ResearchFileDocumentId { get; set; }
        [Column("RESEARCH_FILE_ID")]
        public long ResearchFileId { get; set; }
        [Column("DOCUMENT_ID")]
        public long DocumentId { get; set; }
        [Column("IS_DISABLED")]
        public bool? IsDisabled { get; set; }
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
        [Column("DB_CREATE_USERID")]
        [StringLength(30)]
        public string DbCreateUserid { get; set; }
        [Column("DB_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
        public DateTime DbLastUpdateTimestamp { get; set; }
        [Required]
        [Column("DB_LAST_UPDATE_USERID")]
        [StringLength(30)]
        public string DbLastUpdateUserid { get; set; }

        [ForeignKey(nameof(DocumentId))]
        [InverseProperty(nameof(PimsDocument.PimsResearchFileDocuments))]
        public virtual PimsDocument Document { get; set; }
        [ForeignKey(nameof(ResearchFileId))]
        [InverseProperty(nameof(PimsResearchFile.PimsResearchFileDocuments))]
        public virtual PimsResearchFile ResearchFile { get; set; }
    }
}
