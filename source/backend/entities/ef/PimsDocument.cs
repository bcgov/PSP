using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_DOCUMENT")]
    [Index(nameof(DocumentId), Name = "DOCMNT_DOCUMENT_ID_IDX")]
    [Index(nameof(DocumentStatusTypeCode), Name = "DOCMNT_DOCUMENT_STATUS_TYPE_CODE_IDX")]
    public partial class PimsDocument
    {
        public PimsDocument()
        {
            PimsAcquisitionFileDocuments = new HashSet<PimsAcquisitionFileDocument>();
            PimsFormTypes = new HashSet<PimsFormType>();
            PimsLeaseDocuments = new HashSet<PimsLeaseDocument>();
            PimsProjectDocuments = new HashSet<PimsProjectDocument>();
            PimsResearchFileDocuments = new HashSet<PimsResearchFileDocument>();
        }

        [Key]
        [Column("DOCUMENT_ID")]
        public long DocumentId { get; set; }
        [Column("DOCUMENT_TYPE_ID")]
        public long DocumentTypeId { get; set; }
        [Required]
        [Column("DOCUMENT_STATUS_TYPE_CODE")]
        [StringLength(20)]
        public string DocumentStatusTypeCode { get; set; }
        [Required]
        [Column("FILE_NAME")]
        [StringLength(500)]
        public string FileName { get; set; }
        [Column("MAYAN_ID")]
        public long MayanId { get; set; }
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

        [ForeignKey(nameof(DocumentStatusTypeCode))]
        [InverseProperty(nameof(PimsDocumentStatusType.PimsDocuments))]
        public virtual PimsDocumentStatusType DocumentStatusTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(DocumentTypeId))]
        [InverseProperty(nameof(PimsDocumentTyp.PimsDocuments))]
        public virtual PimsDocumentTyp DocumentType { get; set; }
        [InverseProperty(nameof(PimsAcquisitionFileDocument.Document))]
        public virtual ICollection<PimsAcquisitionFileDocument> PimsAcquisitionFileDocuments { get; set; }
        [InverseProperty(nameof(PimsFormType.Document))]
        public virtual ICollection<PimsFormType> PimsFormTypes { get; set; }
        [InverseProperty(nameof(PimsLeaseDocument.Document))]
        public virtual ICollection<PimsLeaseDocument> PimsLeaseDocuments { get; set; }
        [InverseProperty(nameof(PimsProjectDocument.Document))]
        public virtual ICollection<PimsProjectDocument> PimsProjectDocuments { get; set; }
        [InverseProperty(nameof(PimsResearchFileDocument.Document))]
        public virtual ICollection<PimsResearchFileDocument> PimsResearchFileDocuments { get; set; }
    }
}
