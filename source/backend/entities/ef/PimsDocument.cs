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
    /// <summary>
    /// System-generated unique surrogate primary key.
    /// </summary>
    [Key]
    [Column("DOCUMENT_ID")]
    public long DocumentId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_DOCUMENT_TYPE table.
    /// </summary>
    [Column("DOCUMENT_TYPE_ID")]
    public long DocumentTypeId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_DOCUMENT_STATUS_TYPE table.
    /// </summary>
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
    /// The user that created the record.
    /// </summary>
    [Required]
    [Column("APP_CREATE_USERID")]
    [StringLength(30)]
    public string AppCreateUserid { get; set; }

    /// <summary>
    /// GUID of the user that created the record.
    /// </summary>
    [Column("APP_CREATE_USER_GUID")]
    public Guid? AppCreateUserGuid { get; set; }

    /// <summary>
    /// User directory of the user that created the record.
    /// </summary>
    [Required]
    [Column("APP_CREATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppCreateUserDirectory { get; set; }

    /// <summary>
    /// The date and time the record was updated by the user.
    /// </summary>
    [Column("APP_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppLastUpdateTimestamp { get; set; }

    /// <summary>
    /// The user that updated the record.
    /// </summary>
    [Required]
    [Column("APP_LAST_UPDATE_USERID")]
    [StringLength(30)]
    public string AppLastUpdateUserid { get; set; }

    /// <summary>
    /// GUID of the user that updated the record.
    /// </summary>
    [Column("APP_LAST_UPDATE_USER_GUID")]
    public Guid? AppLastUpdateUserGuid { get; set; }

    /// <summary>
    /// User directory of the user that updated the record.
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
    public virtual ICollection<PimsManagementFileDocument> PimsManagementFileDocuments { get; set; } = new List<PimsManagementFileDocument>();

    [InverseProperty("Document")]
    public virtual ICollection<PimsMgmtActivityDocument> PimsMgmtActivityDocuments { get; set; } = new List<PimsMgmtActivityDocument>();

    [InverseProperty("Document")]
    public virtual ICollection<PimsProjectDocument> PimsProjectDocuments { get; set; } = new List<PimsProjectDocument>();

    [InverseProperty("Document")]
    public virtual ICollection<PimsPropertyDocument> PimsPropertyDocuments { get; set; } = new List<PimsPropertyDocument>();

    [InverseProperty("Document")]
    public virtual ICollection<PimsResearchFileDocument> PimsResearchFileDocuments { get; set; } = new List<PimsResearchFileDocument>();
}
