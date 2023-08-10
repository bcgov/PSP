using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_EXPROP_PMT_PMT_ITEM")]
    [Index(nameof(ExpropriationPaymentId), Name = "XPMTITY_EXPROPRIATION_PAYMENT_ID_IDX")]
    [Index(nameof(PaymentItemTypeCode), nameof(ExpropriationPaymentId), Name = "XPMTITY_EXPROP_PMT_PMT_TYPE_TUC", IsUnique = true)]
    [Index(nameof(PaymentItemTypeCode), Name = "XPMTITY_PAYMENT_ITEM_TYPE_CODE_IDX")]
    public partial class PimsExpropPmtPmtItem
    {
        [Key]
        [Column("EXPROP_PMT_PMT_ITEM_ID")]
        public long ExpropPmtPmtItemId { get; set; }
        [Column("EXPROPRIATION_PAYMENT_ID")]
        public long ExpropriationPaymentId { get; set; }
        [Required]
        [Column("PAYMENT_ITEM_TYPE_CODE")]
        [StringLength(20)]
        public string PaymentItemTypeCode { get; set; }
        [Column("IS_GST_REQUIRED")]
        public bool? IsGstRequired { get; set; }
        [Column("PRETAX_AMT", TypeName = "money")]
        public decimal? PretaxAmt { get; set; }
        [Column("TAX_AMT", TypeName = "money")]
        public decimal? TaxAmt { get; set; }
        [Column("TOTAL_AMT", TypeName = "money")]
        public decimal? TotalAmt { get; set; }
        [Column("IS_DISABLED")]
        public bool? IsDisabled { get; set; }
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

        [ForeignKey(nameof(ExpropriationPaymentId))]
        [InverseProperty(nameof(PimsExpropriationPayment.PimsExpropPmtPmtItems))]
        public virtual PimsExpropriationPayment ExpropriationPayment { get; set; }
        [ForeignKey(nameof(PaymentItemTypeCode))]
        [InverseProperty(nameof(PimsPaymentItemType.PimsExpropPmtPmtItems))]
        public virtual PimsPaymentItemType PaymentItemTypeCodeNavigation { get; set; }
    }
}
