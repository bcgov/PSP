using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

[Table("PIMS_LEASE_PERIOD_HIST")]
[Index("LeasePeriodHistId", "EndDateHist", Name = "PIMS_LSPERD_H_UK", IsUnique = true)]
public partial class PimsLeasePeriodHist
{
    [Key]
    [Column("_LEASE_PERIOD_HIST_ID")]
    public long LeasePeriodHistId { get; set; }

    [Column("EFFECTIVE_DATE_HIST", TypeName = "datetime")]
    public DateTime EffectiveDateHist { get; set; }

    [Column("END_DATE_HIST", TypeName = "datetime")]
    public DateTime? EndDateHist { get; set; }

    [Column("LEASE_PERIOD_ID")]
    public long LeasePeriodId { get; set; }

    [Column("LEASE_ID")]
    public long LeaseId { get; set; }

    [Column("LEASE_PERIOD_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string LeasePeriodStatusTypeCode { get; set; }

    [Column("LEASE_PMT_FREQ_TYPE_CODE")]
    [StringLength(20)]
    public string LeasePmtFreqTypeCode { get; set; }

    [Column("ADDL_RENT_FREQ")]
    [StringLength(20)]
    public string AddlRentFreq { get; set; }

    [Column("VBL_RENT_FREQ")]
    [StringLength(20)]
    public string VblRentFreq { get; set; }

    [Column("PERIOD_START_DATE", TypeName = "datetime")]
    public DateTime PeriodStartDate { get; set; }

    [Column("PERIOD_EXPIRY_DATE", TypeName = "datetime")]
    public DateTime? PeriodExpiryDate { get; set; }

    [Column("PERIOD_RENEWAL_DATE", TypeName = "datetime")]
    public DateTime? PeriodRenewalDate { get; set; }

    [Column("PAYMENT_AMOUNT", TypeName = "money")]
    public decimal? PaymentAmount { get; set; }

    [Column("PAYMENT_DUE_DATE")]
    [StringLength(2000)]
    public string PaymentDueDate { get; set; }

    [Column("PAYMENT_NOTE")]
    [StringLength(2000)]
    public string PaymentNote { get; set; }

    [Column("IS_GST_ELIGIBLE")]
    public bool? IsGstEligible { get; set; }

    [Column("GST_AMOUNT", TypeName = "money")]
    public decimal? GstAmount { get; set; }

    [Column("IS_PERIOD_EXERCISED")]
    public bool? IsPeriodExercised { get; set; }

    [Column("IS_VARIABLE_PAYMENT")]
    public bool IsVariablePayment { get; set; }

    [Column("IS_FLEXIBLE_DURATION")]
    public bool IsFlexibleDuration { get; set; }

    [Column("ADDL_RENT_AGREED_PMT", TypeName = "money")]
    public decimal? AddlRentAgreedPmt { get; set; }

    [Column("ADDL_RENT_GST_AMOUNT", TypeName = "money")]
    public decimal? AddlRentGstAmount { get; set; }

    [Column("IS_ADDL_RENT_SUBJECT_TO_GST")]
    public bool? IsAddlRentSubjectToGst { get; set; }

    [Column("VBL_RENT_AGREED_PMT", TypeName = "money")]
    public decimal? VblRentAgreedPmt { get; set; }

    [Column("VBL_RENT_GST_AMOUNT", TypeName = "money")]
    public decimal? VblRentGstAmount { get; set; }

    [Column("IS_VBL_RENT_SUBJECT_TO_GST")]
    public bool? IsVblRentSubjectToGst { get; set; }

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
