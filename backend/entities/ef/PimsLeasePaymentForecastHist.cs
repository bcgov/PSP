using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_LEASE_PAYMENT_FORECAST_HIST")]
    [Index(nameof(LeasePaymentForecastHistId), nameof(EndDateHist), Name = "PIMS_LPFCST_H_UK", IsUnique = true)]
    public partial class PimsLeasePaymentForecastHist
    {
        [Key]
        [Column("_LEASE_PAYMENT_FORECAST_HIST_ID")]
        public long LeasePaymentForecastHistId { get; set; }
        [Column("EFFECTIVE_DATE_HIST", TypeName = "datetime")]
        public DateTime EffectiveDateHist { get; set; }
        [Column("END_DATE_HIST", TypeName = "datetime")]
        public DateTime? EndDateHist { get; set; }
        [Column("LEASE_PAYMENT_FORECAST_ID")]
        public long LeasePaymentForecastId { get; set; }
        [Column("LEASE_TERM_ID")]
        public long LeaseTermId { get; set; }
        [Column("LEASE_PAYMENT_PERIOD_ID")]
        public long LeasePaymentPeriodId { get; set; }
        [Required]
        [Column("LEASE_PAYMENT_STATUS_TYPE_CODE")]
        [StringLength(20)]
        public string LeasePaymentStatusTypeCode { get; set; }
        [Column("PAYMENT_DUE_DATE", TypeName = "datetime")]
        public DateTime PaymentDueDate { get; set; }
        [Column("FORECAST_PAYMENT_PRE_TAX", TypeName = "money")]
        public decimal ForecastPaymentPreTax { get; set; }
        [Column("FORECAST_PAYMENT_PST", TypeName = "money")]
        public decimal? ForecastPaymentPst { get; set; }
        [Column("FORECAST_PAYMENT_GST", TypeName = "money")]
        public decimal ForecastPaymentGst { get; set; }
        [Column("FORECAST_PAYMENT_TOTAL", TypeName = "money")]
        public decimal ForecastPaymentTotal { get; set; }
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
