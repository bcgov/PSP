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
    [Key]
    [Column("DOCUMENT_CATEGORY_SUBTYPE_ID")]
    public long DocumentCategorySubtypeId { get; set; }

    [Required]
    [Column("DOCUMENT_CATEGORY_TYPE_CODE")]
    [StringLength(20)]
    public string DocumentCategoryTypeCode { get; set; }

    [Column("DOCUMENT_TYPE_ID")]
    public long DocumentTypeId { get; set; }

    /// <summary>
    /// Order in which to display the code values, if required.
    /// </summary>
    [Column("DISPLAY_ORDER")]
    public int? DisplayOrder { get; set; }

    /// <summary>
    /// Indicates if the code value is still active or is now disabled.
    /// </summary>
    [Column("IS_DISABLED")]
    public bool IsDisabled { get; set; }

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

    [ForeignKey("DocumentCategoryTypeCode")]
    [InverseProperty("PimsDocumentCategorySubtypes")]
    public virtual PimsDocumentCategoryType DocumentCategoryTypeCodeNavigation { get; set; }

    [ForeignKey("DocumentTypeId")]
    [InverseProperty("PimsDocumentCategorySubtypes")]
    public virtual PimsDocumentTyp DocumentType { get; set; }
}
