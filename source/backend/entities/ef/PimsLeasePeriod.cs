using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Describes a duration period for the associated lease.
/// </summary>
[Table("PIMS_LEASE_PERIOD")]
[Index("AddlRentFreq", Name = "LSPERD_ADDL_RENT_FREQ_IDX")]
[Index("LeaseId", Name = "LSPERD_LEASE_ID_IDX")]
[Index("LeasePeriodStatusTypeCode", Name = "LSPERD_LEASE_PERIOD_STATUS_TYPE_CODE_IDX")]
[Index("LeasePmtFreqTypeCode", Name = "LSPERD_LEASE_PMT_FREQ_TYPE_CODE_IDX")]
[Index("VblRentFreq", Name = "LSPERD_VBL_RENT_FREQ_IDX")]
public partial class PimsLeasePeriod
{
    /// <summary>
    /// Generated surrogate primary key.
    /// </summary>
    [Key]
    [Column("LEASE_PERIOD_ID")]
    public long LeasePeriodId { get; set; }

    /// <summary>
    /// Foreign key reference to the PIMS_LEASE table.
    /// </summary>
    [Column("LEASE_ID")]
    public long LeaseId { get; set; }

    /// <summary>
    /// Foreign key reference to the PIMS_LEASE_PERIOD_STATUS_TYPE table.
    /// </summary>
    [Column("LEASE_PERIOD_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string LeasePeriodStatusTypeCode { get; set; }

    /// <summary>
    /// Foreign key reference to the PIMS_LEASE_PMT_FREQ_TYPE table.
    /// </summary>
    [Column("LEASE_PMT_FREQ_TYPE_CODE")]
    [StringLength(20)]
    public string LeasePmtFreqTypeCode { get; set; }

    /// <summary>
    /// Foreign key reference to the PIMS_LEASE_PMT_FREQ_TYPE table.
    /// </summary>
    [Column("ADDL_RENT_FREQ")]
    [StringLength(20)]
    public string AddlRentFreq { get; set; }

    /// <summary>
    /// Foreign key reference to the PIMS_LEASE_PMT_FREQ_TYPE table.
    /// </summary>
    [Column("VBL_RENT_FREQ")]
    [StringLength(20)]
    public string VblRentFreq { get; set; }

    /// <summary>
    /// Start date of the current period of the lease/licence
    /// </summary>
    [Column("PERIOD_START_DATE", TypeName = "datetime")]
    public DateTime PeriodStartDate { get; set; }

    /// <summary>
    /// Expiry date of the current period of the lease/licence
    /// </summary>
    [Column("PERIOD_EXPIRY_DATE", TypeName = "datetime")]
    public DateTime? PeriodExpiryDate { get; set; }

    /// <summary>
    /// Renewal date of the current period of the lease/licence
    /// </summary>
    [Column("PERIOD_RENEWAL_DATE", TypeName = "datetime")]
    public DateTime? PeriodRenewalDate { get; set; }

    /// <summary>
    /// Agreed-to payment amount (exclusive of GST)
    /// </summary>
    [Column("PAYMENT_AMOUNT", TypeName = "money")]
    public decimal? PaymentAmount { get; set; }

    /// <summary>
    /// Anecdotal description of payment due date (e.g. 1st of month, end of month)
    /// </summary>
    [Column("PAYMENT_DUE_DATE")]
    [StringLength(2000)]
    public string PaymentDueDate { get; set; }

    /// <summary>
    /// Notes regarding payment status for the lease period
    /// </summary>
    [Column("PAYMENT_NOTE")]
    [StringLength(2000)]
    public string PaymentNote { get; set; }

    /// <summary>
    /// Is the lease subject to GST?
    /// </summary>
    [Column("IS_GST_ELIGIBLE")]
    public bool? IsGstEligible { get; set; }

    /// <summary>
    /// Calculated/entered GST portion of the payment.  Can be overridden by the user.
    /// </summary>
    [Column("GST_AMOUNT", TypeName = "money")]
    public decimal? GstAmount { get; set; }

    /// <summary>
    /// Has the lease period been exercised?
    /// </summary>
    [Column("IS_PERIOD_EXERCISED")]
    public bool? IsPeriodExercised { get; set; }

    /// <summary>
    /// Indicates whether the payment type is predetermined (FALSE) or variable (TRUE).  Predetermined (FALSE) is the default value.
    /// </summary>
    [Column("IS_VARIABLE_PAYMENT")]
    public bool IsVariablePayment { get; set; }

    /// <summary>
    /// Indicates whether the period duration is fixed (FALSE) or flexible (TRUE).  Fixed (FALSE) is the default value.
    /// </summary>
    [Column("IS_FLEXIBLE_DURATION")]
    public bool IsFlexibleDuration { get; set; }

    /// <summary>
    /// Indicates the agreed-to variable additional rent payment amount.
    /// </summary>
    [Column("ADDL_RENT_AGREED_PMT", TypeName = "money")]
    public decimal? AddlRentAgreedPmt { get; set; }

    /// <summary>
    /// GST dollar amount for the additional rent.
    /// </summary>
    [Column("ADDL_RENT_GST_AMOUNT", TypeName = "money")]
    public decimal? AddlRentGstAmount { get; set; }

    /// <summary>
    /// Is the variable additional rent payment subject to GST?
    /// </summary>
    [Column("IS_ADDL_RENT_SUBJECT_TO_GST")]
    public bool? IsAddlRentSubjectToGst { get; set; }

    /// <summary>
    /// Indicates the agreed-to variable rent payment amount.
    /// </summary>
    [Column("VBL_RENT_AGREED_PMT", TypeName = "money")]
    public decimal? VblRentAgreedPmt { get; set; }

    /// <summary>
    /// GST dollar amount for the variable rent.
    /// </summary>
    [Column("VBL_RENT_GST_AMOUNT", TypeName = "money")]
    public decimal? VblRentGstAmount { get; set; }

    /// <summary>
    /// Is the variable rent payment subject to GST?
    /// </summary>
    [Column("IS_VBL_RENT_SUBJECT_TO_GST")]
    public bool? IsVblRentSubjectToGst { get; set; }

    /// <summary>
    /// Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o
    /// </summary>
    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long ConcurrencyControlNumber { get; set; }

    /// <summary>
    /// The date and time the user created the record.
    /// </summary>
    [Column("APP_CREATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppCreateTimestamp { get; set; }

    /// <summary>
    /// The user account that created the record.
    /// </summary>
    [Required]
    [Column("APP_CREATE_USERID")]
    [StringLength(30)]
    public string AppCreateUserid { get; set; }

    /// <summary>
    /// The GUID of the user account that created the record.
    /// </summary>
    [Column("APP_CREATE_USER_GUID")]
    public Guid? AppCreateUserGuid { get; set; }

    /// <summary>
    /// The directory of the user account that created the record.
    /// </summary>
    [Required]
    [Column("APP_CREATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppCreateUserDirectory { get; set; }

    /// <summary>
    /// The date and time the user updated the record.
    /// </summary>
    [Column("APP_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppLastUpdateTimestamp { get; set; }

    /// <summary>
    /// The user account that updated the record.
    /// </summary>
    [Required]
    [Column("APP_LAST_UPDATE_USERID")]
    [StringLength(30)]
    public string AppLastUpdateUserid { get; set; }

    /// <summary>
    /// The GUID of the user account that updated the record.
    /// </summary>
    [Column("APP_LAST_UPDATE_USER_GUID")]
    public Guid? AppLastUpdateUserGuid { get; set; }

    /// <summary>
    /// The directory of the user account that updated the record.
    /// </summary>
    [Required]
    [Column("APP_LAST_UPDATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppLastUpdateUserDirectory { get; set; }

    /// <summary>
    /// The date and time the record was created.
    /// </summary>
    [Column("DB_CREATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime DbCreateTimestamp { get; set; }

    /// <summary>
    /// The user or proxy account that created the record.
    /// </summary>
    [Required]
    [Column("DB_CREATE_USERID")]
    [StringLength(30)]
    public string DbCreateUserid { get; set; }

    /// <summary>
    /// The date and time the record was created or last updated.
    /// </summary>
    [Column("DB_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime DbLastUpdateTimestamp { get; set; }

    /// <summary>
    /// The user or proxy account that created or last updated the record.
    /// </summary>
    [Required]
    [Column("DB_LAST_UPDATE_USERID")]
    [StringLength(30)]
    public string DbLastUpdateUserid { get; set; }

    [ForeignKey("AddlRentFreq")]
    [InverseProperty("PimsLeasePeriodAddlRentFreqNavigations")]
    public virtual PimsLeasePmtFreqType AddlRentFreqNavigation { get; set; }

    [ForeignKey("LeaseId")]
    [InverseProperty("PimsLeasePeriods")]
    public virtual PimsLease Lease { get; set; }

    [ForeignKey("LeasePeriodStatusTypeCode")]
    [InverseProperty("PimsLeasePeriods")]
    public virtual PimsLeasePeriodStatusType LeasePeriodStatusTypeCodeNavigation { get; set; }

    [ForeignKey("LeasePmtFreqTypeCode")]
    [InverseProperty("PimsLeasePeriodLeasePmtFreqTypeCodeNavigations")]
    public virtual PimsLeasePmtFreqType LeasePmtFreqTypeCodeNavigation { get; set; }

    [InverseProperty("LeasePeriod")]
    public virtual ICollection<PimsLeasePayment> PimsLeasePayments { get; set; } = new List<PimsLeasePayment>();

    [ForeignKey("VblRentFreq")]
    [InverseProperty("PimsLeasePeriodVblRentFreqNavigations")]
    public virtual PimsLeasePmtFreqType VblRentFreqNavigation { get; set; }
}
