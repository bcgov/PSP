using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_LEASE_PAYMENT_PERIOD")]
    public partial class PimsLeasePaymentPeriod
    {
        public PimsLeasePaymentPeriod()
        {
            PimsLeasePaymentForecasts = new HashSet<PimsLeasePaymentForecast>();
            PimsLeasePayments = new HashSet<PimsLeasePayment>();
        }

        [Key]
        [Column("LEASE_PAYMENT_PERIOD_ID")]
        public long LeasePaymentPeriodId { get; set; }
        [Column("PERIOD_START_DATE", TypeName = "date")]
        public DateTime PeriodStartDate { get; set; }
        [Required]
        [Column("IS_PERIOD_CLOSED")]
        public bool? IsPeriodClosed { get; set; }
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

        [InverseProperty(nameof(PimsLeasePaymentForecast.LeasePaymentPeriod))]
        public virtual ICollection<PimsLeasePaymentForecast> PimsLeasePaymentForecasts { get; set; }
        [InverseProperty(nameof(PimsLeasePayment.LeasePaymentPeriod))]
        public virtual ICollection<PimsLeasePayment> PimsLeasePayments { get; set; }
    }
}
