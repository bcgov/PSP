using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

[Table("PIMS_PROPERTY_ACTIVITY_DOCUMENT")]
[Index("DocumentId", Name = "PRACDO_DOCUMENT_ID_IDX")]
[Index("PimsPropertyActivityId", Name = "PRACDO_PIMS_PROPERTY_ACTIVITY_ID_IDX")]
public partial class PimsPropertyActivityDocument
{
    [Key]
    [Column("PROPERTY_ACTIVITY_DOCUMENT_ID")]
    public long PropertyActivityDocumentId { get; set; }

    [Column("PIMS_PROPERTY_ACTIVITY_ID")]
    public long PimsPropertyActivityId { get; set; }

    [Column("DOCUMENT_ID")]
    public long DocumentId { get; set; }

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

    [ForeignKey("DocumentId")]
    [InverseProperty("PimsPropertyActivityDocuments")]
    public virtual PimsDocument Document { get; set; }

    [InverseProperty("PropertyActivityDocument")]
    public virtual ICollection<PimsDocumentQueue> PimsDocumentQueues { get; set; } = new List<PimsDocumentQueue>();

    [ForeignKey("PimsPropertyActivityId")]
    [InverseProperty("PimsPropertyActivityDocuments")]
    public virtual PimsPropertyActivity PimsPropertyActivity { get; set; }
}
