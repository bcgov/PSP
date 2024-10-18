using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Table describing the available document types.
/// </summary>
[Table("PIMS_DOCUMENT_TYP")]
public partial class PimsDocumentTyp
{
    [Key]
    [Column("DOCUMENT_TYPE_ID")]
    public long DocumentTypeId { get; set; }

    /// <summary>
    /// Mayan-generated document type number used for retrieval from Mayan EDMS.
    /// </summary>
    [Column("MAYAN_ID")]
    public long MayanId { get; set; }

    /// <summary>
    /// Code value of the available document types.
    /// </summary>
    [Required]
    [Column("DOCUMENT_TYPE")]
    [StringLength(20)]
    public string DocumentType { get; set; }

    /// <summary>
    /// Description of the available document types.
    /// </summary>
    [Required]
    [Column("DOCUMENT_TYPE_DESCRIPTION")]
    [StringLength(200)]
    public string DocumentTypeDescription { get; set; }

    /// <summary>
    /// Describes the purpose of the document.
    /// </summary>
    [Column("DOCUMENT_TYPE_DEFINITION")]
    [StringLength(500)]
    public string DocumentTypeDefinition { get; set; }

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

    [InverseProperty("DocumentType")]
    public virtual ICollection<PimsDocumentCategorySubtype> PimsDocumentCategorySubtypes { get; set; } = new List<PimsDocumentCategorySubtype>();

    [InverseProperty("DocumentType")]
    public virtual ICollection<PimsDocument> PimsDocuments { get; set; } = new List<PimsDocument>();
}
