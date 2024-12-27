using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Table providing progress tracking of document inclusion into the MAYAN EDMS.
/// </summary>
[Table("PIMS_DOCUMENT_QUEUE")]
[Index("DataSourceTypeCode", Name = "DOCQUE_DATA_SOURCE_TYPE_CODE_IDX")]
[Index("DocumentId", Name = "DOCQUE_DOCUMENT_ID_IDX")]
[Index("DocumentQueueStatusTypeCode", Name = "DOCQUE_DOCUMENT_QUEUE_STATUS_TYPE_CODE_IDX")]
public partial class PimsDocumentQueue
{
    /// <summary>
    /// Generated surrogate primary key.
    /// </summary>
    [Key]
    [Column("DOCUMENT_QUEUE_ID")]
    public long DocumentQueueId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_DOCUMENT table.
    /// </summary>
    [Column("DOCUMENT_ID")]
    public long? DocumentId { get; set; }

    /// <summary>
    /// Code value that represents the current status of the document as it is processed by PIMS/MAYAN.
    /// </summary>
    [Required]
    [Column("DOCUMENT_QUEUE_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string DocumentQueueStatusTypeCode { get; set; }

    /// <summary>
    /// Code value that refers to the source system the document originated in.
    /// </summary>
    [Required]
    [Column("DATA_SOURCE_TYPE_CODE")]
    [StringLength(20)]
    public string DataSourceTypeCode { get; set; }

    /// <summary>
    /// Fluid key used to uniquely identify document in external system.
    /// </summary>
    [Column("DOCUMENT_EXTERNAL_ID")]
    [StringLength(1000)]
    public string DocumentExternalId { get; set; }

    /// <summary>
    /// Used to store JSON-encoded metadata that needs to be added to the document during upload.
    /// </summary>
    [Column("DOCUMENT_METADATA")]
    [StringLength(4000)]
    public string DocumentMetadata { get; set; }

    /// <summary>
    /// When the document is sent to the backend for processing, this will be populated.
    /// </summary>
    [Column("DOC_PROCESS_START_DT", TypeName = "datetime")]
    public DateTime? DocProcessStartDt { get; set; }

    /// <summary>
    /// When the document?s processing finishes, this will be populated.
    /// </summary>
    [Column("DOC_PROCESS_END_DT", TypeName = "datetime")]
    public DateTime? DocProcessEndDt { get; set; }

    /// <summary>
    /// The number of times that this document has been queued for upload.
    /// </summary>
    [Column("DOC_PROCESS_RETRIES")]
    public int? DocProcessRetries { get; set; }

    /// <summary>
    /// If the upload process fails, the error corresponding to the failure will be displayed here.
    /// </summary>
    [Column("MAYAN_ERROR")]
    [StringLength(4000)]
    public string MayanError { get; set; }

    /// <summary>
    /// The actual document blob, stored temporarily until after processing completes.
    /// </summary>
    [Column("DOCUMENT")]
    public byte[] Document { get; set; }

    /// <summary>
    /// Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o.
    /// </summary>
    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long ConcurrencyControlNumber { get; set; }

    /// <summary>
    /// The date and time the user created the record.
    /// </summary>
    [Column("APP_CREATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppCreateTimestamp { get; set; }

    /// <summary>
    /// The user account that created the record.
    /// </summary>
    [Required]
    [Column("APP_CREATE_USERID")]
    [StringLength(30)]
    public string AppCreateUserid { get; set; }

    /// <summary>
    /// The GUID of the user account that created the record.
    /// </summary>
    [Column("APP_CREATE_USER_GUID")]
    public Guid? AppCreateUserGuid { get; set; }

    /// <summary>
    /// The directory of the user account that created the record.
    /// </summary>
    [Required]
    [Column("APP_CREATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppCreateUserDirectory { get; set; }

    /// <summary>
    /// The date and time the user updated the record.
    /// </summary>
    [Column("APP_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppLastUpdateTimestamp { get; set; }

    /// <summary>
    /// The user account that updated the record.
    /// </summary>
    [Required]
    [Column("APP_LAST_UPDATE_USERID")]
    [StringLength(30)]
    public string AppLastUpdateUserid { get; set; }

    /// <summary>
    /// The GUID of the user account that updated the record.
    /// </summary>
    [Column("APP_LAST_UPDATE_USER_GUID")]
    public Guid? AppLastUpdateUserGuid { get; set; }

    /// <summary>
    /// The directory of the user account that updated the record.
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

    [ForeignKey("DataSourceTypeCode")]
    [InverseProperty("PimsDocumentQueues")]
    public virtual PimsDataSourceType DataSourceTypeCodeNavigation { get; set; }

    [ForeignKey("DocumentId")]
    [InverseProperty("PimsDocumentQueues")]
    public virtual PimsDocument DocumentNavigation { get; set; }

    [ForeignKey("DocumentQueueStatusTypeCode")]
    [InverseProperty("PimsDocumentQueues")]
    public virtual PimsDocumentQueueStatusType DocumentQueueStatusTypeCodeNavigation { get; set; }
}
