using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_LEASE_TERM_HIST")]
    [Index(nameof(LeaseTermHistId), nameof(EndDateHist), Name = "PIMS_LSTERM_H_UK", IsUnique = true)]
    public partial class PimsLeaseTermHist
    {
        [Key]
        [Column("_LEASE_TERM_HIST_ID")]
        public long LeaseTermHistId { get; set; }
        [Column("EFFECTIVE_DATE_HIST", TypeName = "datetime")]
        public DateTime EffectiveDateHist { get; set; }
        [Column("END_DATE_HIST", TypeName = "datetime")]
        public DateTime? EndDateHist { get; set; }
        [Column("LEASE_TERM_ID")]
        public long LeaseTermId { get; set; }
        [Column("LEASE_ID")]
        public long LeaseId { get; set; }
        [Column("LEASE_TERM_STATUS_TYPE_CODE")]
        [StringLength(20)]
        public string LeaseTermStatusTypeCode { get; set; }
        [Column("LEASE_PMT_FREQ_TYPE_CODE")]
        [StringLength(20)]
        public string LeasePmtFreqTypeCode { get; set; }
        [Column("TERM_START_DATE", TypeName = "datetime")]
        public DateTime TermStartDate { get; set; }
        [Column("TERM_EXPIRY_DATE", TypeName = "datetime")]
        public DateTime? TermExpiryDate { get; set; }
        [Column("TERM_RENEWAL_DATE", TypeName = "datetime")]
        public DateTime? TermRenewalDate { get; set; }
        [Column("PAYMENT_AMOUNT", TypeName = "money")]
        public decimal? PaymentAmount { get; set; }
        [Column("PAYMENT_DUE_DATE")]
        [StringLength(200)]
        public string PaymentDueDate { get; set; }
        [Column("PAYMENT_NOTE")]
        [StringLength(2000)]
        public string PaymentNote { get; set; }
        [Column("IS_GST_ELIGIBLE")]
        public bool? IsGstEligible { get; set; }
        [Column("GST_AMOUNT", TypeName = "money")]
        public decimal? GstAmount { get; set; }
        [Column("IS_TERM_EXERCISED")]
        public bool? IsTermExercised { get; set; }
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
