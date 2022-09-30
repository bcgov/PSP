using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_ACTIVITY_TEMPLATE_DOCUMENT")]
    [Index(nameof(ActivityTemplateId), Name = "ACTMDO_ACTIVITY_TEMPLATE_ID_IDX")]
    [Index(nameof(ActivityTemplateId), nameof(DocumentId), Name = "ACTMDO_ACT_TMP_DOC_TUC", IsUnique = true)]
    [Index(nameof(DocumentId), Name = "ACTMDO_DOCUMENT_ID_IDX")]
    public partial class PimsActivityTemplateDocument
    {
        [Key]
        [Column("ACTIVITY_TEMPLATE_DOCUMENT_ID")]
        public long ActivityTemplateDocumentId { get; set; }
        [Column("DOCUMENT_ID")]
        public long DocumentId { get; set; }
        [Column("ACTIVITY_TEMPLATE_ID")]
        public long ActivityTemplateId { get; set; }
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

        [ForeignKey(nameof(ActivityTemplateId))]
        [InverseProperty(nameof(PimsActivityTemplate.PimsActivityTemplateDocuments))]
        public virtual PimsActivityTemplate ActivityTemplate { get; set; }
        [ForeignKey(nameof(DocumentId))]
        [InverseProperty(nameof(PimsDocument.PimsActivityTemplateDocuments))]
        public virtual PimsDocument Document { get; set; }
    }
}
