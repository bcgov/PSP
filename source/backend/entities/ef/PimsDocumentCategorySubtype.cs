using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

[Table("PIMS_DOCUMENT_CATEGORY_SUBTYPE")]
[Index("DocumentTypeId", "DocumentCategoryTypeCode", Name = "DCCTSB_DOCUMENT_CATEGORY_SUBTYPE_TUC", IsUnique = true)]
[Index("DocumentCategoryTypeCode", Name = "DCCTSB_DOCUMENT_CATEGORY_TYPE_CODE_IDX")]
[Index("DocumentTypeId", Name = "DCCTSB_DOCUMENT_TYPE_ID_IDX")]
public partial class PimsDocumentCategorySubtype
{
    /// <summary>
    /// System-generated unique surrogate primary key.
    /// </summary>
    [Key]
    [Column("DOCUMENT_CATEGORY_SUBTYPE_ID")]
    public long DocumentCategorySubtypeId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_DOCUMENT_CATEGORY_TYPE table.
    /// </summary>
    [Required]
    [Column("DOCUMENT_CATEGORY_TYPE_CODE")]
    [StringLength(20)]
    public string DocumentCategoryTypeCode { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_DOCUMENT_TYPE table.
    /// </summary>
    [Column("DOCUMENT_TYPE_ID")]
    public long DocumentTypeId { get; set; }

    /// <summary>
    /// Designates a preferred presentation order of the code descriptions.
    /// </summary>
    [Column("DISPLAY_ORDER")]
    public int? DisplayOrder { get; set; }

    /// <summary>
    /// Indicates if the record is disabled and therefore not selectable or displayed.
    /// </summary>
    [Column("IS_DISABLED")]
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o
    /// </summary>
    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long ConcurrencyControlNumber { get; set; }

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

    [ForeignKey("DocumentCategoryTypeCode")]
    [InverseProperty("PimsDocumentCategorySubtypes")]
    public virtual PimsDocumentCategoryType DocumentCategoryTypeCodeNavigation { get; set; }

    [ForeignKey("DocumentTypeId")]
    [InverseProperty("PimsDocumentCategorySubtypes")]
    public virtual PimsDocumentTyp DocumentType { get; set; }
}
