using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Table describing the available document types.
/// </summary>
[Table("PIMS_DOCUMENT")]
[Index("DocumentId", Name = "DOCMNT_DOCUMENT_ID_IDX")]
[Index("DocumentStatusTypeCode", Name = "DOCMNT_DOCUMENT_STATUS_TYPE_CODE_IDX")]
public partial class PimsDocument
{
    [Key]
    [Column("DOCUMENT_ID")]
    public long DocumentId { get; set; }

    [Column("DOCUMENT_TYPE_ID")]
    public long DocumentTypeId { get; set; }

    [Required]
    [Column("DOCUMENT_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string DocumentStatusTypeCode { get; set; }

    /// <summary>
    /// Name of the file stored on Mayan EDMS.
    /// </summary>
    [Required]
    [Column("FILE_NAME")]
    [StringLength(500)]
    public string FileName { get; set; }

    /// <summary>
    /// Mayan-generated document number used for retrieval from Mayan EDMS.
    /// </summary>
    [Column("MAYAN_ID")]
    public long? MayanId { get; set; }

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

    /// <summary>
    /// Fluid key used to uniquely identify document in external system.
    /// </summary>
    [Column("DOCUMENT_EXTERNAL_ID")]
    [StringLength(1000)]
    public string DocumentExternalId { get; set; }

    [ForeignKey("DocumentStatusTypeCode")]
    [InverseProperty("PimsDocuments")]
    public virtual PimsDocumentStatusType DocumentStatusTypeCodeNavigation { get; set; }

    [ForeignKey("DocumentTypeId")]
    [InverseProperty("PimsDocuments")]
    public virtual PimsDocumentTyp DocumentType { get; set; }

    [InverseProperty("Document")]
    public virtual ICollection<PimsAcquisitionFileDocument> PimsAcquisitionFileDocuments { get; set; } = new List<PimsAcquisitionFileDocument>();

    [InverseProperty("Document")]
    public virtual ICollection<PimsDispositionFileDocument> PimsDispositionFileDocuments { get; set; } = new List<PimsDispositionFileDocument>();

    [InverseProperty("DocumentNavigation")]
    public virtual ICollection<PimsDocumentQueue> PimsDocumentQueues { get; set; } = new List<PimsDocumentQueue>();

    [InverseProperty("Document")]
    public virtual ICollection<PimsFormType> PimsFormTypes { get; set; } = new List<PimsFormType>();

    [InverseProperty("Document")]
    public virtual ICollection<PimsLeaseDocument> PimsLeaseDocuments { get; set; } = new List<PimsLeaseDocument>();

    [InverseProperty("Document")]
    public virtual ICollection<PimsProjectDocument> PimsProjectDocuments { get; set; } = new List<PimsProjectDocument>();

    [InverseProperty("Document")]
    public virtual ICollection<PimsPropertyActivityDocument> PimsPropertyActivityDocuments { get; set; } = new List<PimsPropertyActivityDocument>();

    [InverseProperty("Document")]
    public virtual ICollection<PimsResearchFileDocument> PimsResearchFileDocuments { get; set; } = new List<PimsResearchFileDocument>();
}
