using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_DOCUMENT_CATEGORY_SUBTYPE")]
    [Index(nameof(DocumentTypeId), nameof(DocumentCategoryTypeCode), Name = "DCCTSB_DOCUMENT_CATEGORY_SUBTYPE_TUC", IsUnique = true)]
    [Index(nameof(DocumentCategoryTypeCode), Name = "DCCTSB_DOCUMENT_CATEGORY_TYPE_CODE_IDX")]
    [Index(nameof(DocumentTypeId), Name = "DCCTSB_DOCUMENT_TYPE_ID_IDX")]
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
        [Column("DISPLAY_ORDER")]
        public int? DisplayOrder { get; set; }
        [Required]
        [Column("IS_DISABLED")]
        public bool? IsDisabled { get; set; }
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

        [ForeignKey(nameof(DocumentCategoryTypeCode))]
        [InverseProperty(nameof(PimsDocumentCategoryType.PimsDocumentCategorySubtypes))]
        public virtual PimsDocumentCategoryType DocumentCategoryTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(DocumentTypeId))]
        [InverseProperty(nameof(PimsDocumentTyp.PimsDocumentCategorySubtypes))]
        public virtual PimsDocumentTyp DocumentType { get; set; }
    }
}
