using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_DOCUMENT_TYP")]
    public partial class PimsDocumentTyp
    {
        public PimsDocumentTyp()
        {
            PimsDocumentCategorySubtypes = new HashSet<PimsDocumentCategorySubtype>();
            PimsDocuments = new HashSet<PimsDocument>();
        }

        [Key]
        [Column("DOCUMENT_TYPE_ID")]
        public long DocumentTypeId { get; set; }
        [Column("MAYAN_ID")]
        public long MayanId { get; set; }
        [Required]
        [Column("DOCUMENT_TYPE")]
        [StringLength(20)]
        public string DocumentType { get; set; }
        [Required]
        [Column("DOCUMENT_TYPE_DESCRIPTION")]
        [StringLength(200)]
        public string DocumentTypeDescription { get; set; }
        [Required]
        [Column("IS_DISABLED")]
        public bool? IsDisabled { get; set; }
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

        [InverseProperty(nameof(PimsDocumentCategorySubtype.DocumentType))]
        public virtual ICollection<PimsDocumentCategorySubtype> PimsDocumentCategorySubtypes { get; set; }
        [InverseProperty(nameof(PimsDocument.DocumentType))]
        public virtual ICollection<PimsDocument> PimsDocuments { get; set; }
    }
}
