using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

[Table("PIMS_DOCUMENT_QUEUE_HIST")]
[Index("DocumentQueueHistId", "EndDateHist", Name = "PIMS_DOCQUE_H_UK", IsUnique = true)]
public partial class PimsDocumentQueueHist
{
    [Key]
    [Column("_DOCUMENT_QUEUE_HIST_ID")]
    public long DocumentQueueHistId { get; set; }

    [Column("EFFECTIVE_DATE_HIST", TypeName = "datetime")]
    public DateTime EffectiveDateHist { get; set; }

    [Column("END_DATE_HIST", TypeName = "datetime")]
    public DateTime? EndDateHist { get; set; }

    [Column("DOCUMENT_QUEUE_ID")]
    public long DocumentQueueId { get; set; }

    [Column("DOCUMENT_ID")]
    public long? DocumentId { get; set; }

    [Required]
    [Column("DOCUMENT_QUEUE_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string DocumentQueueStatusTypeCode { get; set; }

    [Required]
    [Column("DATA_SOURCE_TYPE_CODE")]
    [StringLength(20)]
    public string DataSourceTypeCode { get; set; }

    [Column("DOCUMENT_EXTERNAL_ID")]
    [StringLength(1000)]
    public string DocumentExternalId { get; set; }

    [Column("DOCUMENT_METADATA")]
    [StringLength(4000)]
    public string DocumentMetadata { get; set; }

    [Column("DOC_PROCESS_START_DT", TypeName = "datetime")]
    public DateTime? DocProcessStartDt { get; set; }

    [Column("DOC_PROCESS_END_DT", TypeName = "datetime")]
    public DateTime? DocProcessEndDt { get; set; }

    [Column("DOC_PROCESS_RETRIES")]
    public int? DocProcessRetries { get; set; }

    [Column("MAYAN_ERROR")]
    [StringLength(4000)]
    public string MayanError { get; set; }

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
