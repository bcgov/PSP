using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_PROPERTY_TAX")]
    [Index(nameof(PropertyId), Name = "PRPTAX_PROPERTY_ID_IDX")]
    [Index(nameof(PropertyTaxRemitTypeCode), Name = "PRPTAX_PROPERTY_TAX_REMIT_TYPE_CODE_IDX")]
    public partial class PimsPropertyTax
    {
        [Key]
        [Column("PROPERTY_TAX_ID")]
        public long PropertyTaxId { get; set; }
        [Column("PROPERTY_ID")]
        public long PropertyId { get; set; }
        [Required]
        [Column("PROPERTY_TAX_REMIT_TYPE_CODE")]
        [StringLength(20)]
        public string PropertyTaxRemitTypeCode { get; set; }
        [Required]
        [Column("TAX_FOLIO_NO")]
        [StringLength(50)]
        public string TaxFolioNo { get; set; }
        [Column("PAYMENT_AMOUNT", TypeName = "money")]
        public decimal PaymentAmount { get; set; }
        [Column("LAST_PAYMENT_DATE", TypeName = "datetime")]
        public DateTime? LastPaymentDate { get; set; }
        [Column("PAYMENT_NOTES", TypeName = "money")]
        public decimal? PaymentNotes { get; set; }
        [Column("BCTFA_NOTIFICATION_DATE", TypeName = "datetime")]
        public DateTime? BctfaNotificationDate { get; set; }
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

        [ForeignKey(nameof(PropertyId))]
        [InverseProperty(nameof(PimsProperty.PimsPropertyTaxes))]
        public virtual PimsProperty Property { get; set; }
        [ForeignKey(nameof(PropertyTaxRemitTypeCode))]
        [InverseProperty(nameof(PimsPropertyTaxRemitType.PimsPropertyTaxes))]
        public virtual PimsPropertyTaxRemitType PropertyTaxRemitTypeCodeNavigation { get; set; }
    }
}
