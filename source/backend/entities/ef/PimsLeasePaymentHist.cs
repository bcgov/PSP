using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_LEASE_PAYMENT_HIST")]
    [Index(nameof(LeasePaymentHistId), nameof(EndDateHist), Name = "PIMS_LSPYMT_H_UK", IsUnique = true)]
    public partial class PimsLeasePaymentHist
    {
        [Key]
        [Column("_LEASE_PAYMENT_HIST_ID")]
        public long LeasePaymentHistId { get; set; }
        [Column("EFFECTIVE_DATE_HIST", TypeName = "datetime")]
        public DateTime EffectiveDateHist { get; set; }
        [Column("END_DATE_HIST", TypeName = "datetime")]
        public DateTime? EndDateHist { get; set; }
        [Column("LEASE_PAYMENT_ID")]
        public long LeasePaymentId { get; set; }
        [Column("LEASE_TERM_ID")]
        public long LeaseTermId { get; set; }
        [Required]
        [Column("LEASE_PAYMENT_METHOD_TYPE_CODE")]
        [StringLength(20)]
        public string LeasePaymentMethodTypeCode { get; set; }
        [Column("LEASE_PAYMENT_STATUS_TYPE_CODE")]
        [StringLength(20)]
        public string LeasePaymentStatusTypeCode { get; set; }
        [Column("PAYMENT_RECEIVED_DATE", TypeName = "datetime")]
        public DateTime PaymentReceivedDate { get; set; }
        [Column("PAYMENT_AMOUNT_PRE_TAX", TypeName = "money")]
        public decimal PaymentAmountPreTax { get; set; }
        [Column("PAYMENT_AMOUNT_PST", TypeName = "money")]
        public decimal? PaymentAmountPst { get; set; }
        [Column("PAYMENT_AMOUNT_GST", TypeName = "money")]
        public decimal? PaymentAmountGst { get; set; }
        [Column("PAYMENT_AMOUNT_TOTAL", TypeName = "money")]
        public decimal PaymentAmountTotal { get; set; }
        [Column("NOTE")]
        [StringLength(2000)]
        public string Note { get; set; }
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
}
