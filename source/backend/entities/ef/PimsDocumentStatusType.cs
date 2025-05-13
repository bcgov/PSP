using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Table describing the available document types.
/// </summary>
[Table("PIMS_DOCUMENT_STATUS_TYPE")]
public partial class PimsDocumentStatusType
{
    /// <summary>
    /// Code value of the document status type.
    /// </summary>
    [Key]
    [Column("DOCUMENT_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string DocumentStatusTypeCode { get; set; }

    /// <summary>
    /// Descriptive translation of the document status type code.
    /// </summary>
    [Required]
    [Column("DESCRIPTION")]
    [StringLength(200)]
    public string Description { get; set; }

    /// <summary>
    /// Indicates if the code is still in use.
    /// </summary>
    [Column("IS_DISABLED")]
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Determines the default display order of the code descriptions.
    /// </summary>
    [Column("DISPLAY_ORDER")]
    public int? DisplayOrder { get; set; }

    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long ConcurrencyControlNumber { get; set; }

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

    [InverseProperty("DocumentStatusTypeCodeNavigation")]
    public virtual ICollection<PimsDocument> PimsDocuments { get; set; } = new List<PimsDocument>();
}
