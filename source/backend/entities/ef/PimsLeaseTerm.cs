using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Describes a term for the associated lease.
/// </summary>
[Table("PIMS_LEASE_TERM")]
[Index("LeaseId", Name = "LSTERM_LEASE_ID_IDX")]
[Index("LeasePmtFreqTypeCode", Name = "LSTERM_LEASE_PMT_FREQ_TYPE_CODE_IDX")]
[Index("LeaseTermStatusTypeCode", Name = "LSTERM_LEASE_TERM_STATUS_TYPE_CODE_IDX")]
public partial class PimsLeaseTerm
{
    [Key]
    [Column("LEASE_TERM_ID")]
    public long LeaseTermId { get; set; }

    [Column("LEASE_ID")]
    public long LeaseId { get; set; }

    [Column("LEASE_TERM_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string LeaseTermStatusTypeCode { get; set; }

    /// <summary>
    /// Foreign key to payment frequency values
    /// </summary>
    [Column("LEASE_PMT_FREQ_TYPE_CODE")]
    [StringLength(20)]
    public string LeasePmtFreqTypeCode { get; set; }

    /// <summary>
    /// Start date of the current term of the lease/licence
    /// </summary>
    [Column("TERM_START_DATE", TypeName = "datetime")]
    public DateTime TermStartDate { get; set; }

    /// <summary>
    /// Expiry date of the current term of the lease/licence
    /// </summary>
    [Column("TERM_EXPIRY_DATE", TypeName = "datetime")]
    public DateTime? TermExpiryDate { get; set; }

    /// <summary>
    /// Renewal date of the current term of the lease/licence
    /// </summary>
    [Column("TERM_RENEWAL_DATE", TypeName = "datetime")]
    public DateTime? TermRenewalDate { get; set; }

    /// <summary>
    /// Agreed-to payment amount (exclusive of GST)
    /// </summary>
    [Column("PAYMENT_AMOUNT", TypeName = "money")]
    public decimal? PaymentAmount { get; set; }

    /// <summary>
    /// Anecdotal description of payment due date (e.g. 1st of month, end of month)
    /// </summary>
    [Column("PAYMENT_DUE_DATE")]
    [StringLength(200)]
    public string PaymentDueDate { get; set; }

    /// <summary>
    /// Notes regarding payment status for the lease term
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
    /// Has the lease term been exercised?
    /// </summary>
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

    [ForeignKey("LeaseId")]
    [InverseProperty("PimsLeaseTerms")]
    public virtual PimsLease Lease { get; set; }

    [ForeignKey("LeasePmtFreqTypeCode")]
    [InverseProperty("PimsLeaseTerms")]
    public virtual PimsLeasePmtFreqType LeasePmtFreqTypeCodeNavigation { get; set; }

    [ForeignKey("LeaseTermStatusTypeCode")]
    [InverseProperty("PimsLeaseTerms")]
    public virtual PimsLeaseTermStatusType LeaseTermStatusTypeCodeNavigation { get; set; }

    [InverseProperty("LeaseTerm")]
    public virtual ICollection<PimsLeasePayment> PimsLeasePayments { get; set; } = new List<PimsLeasePayment>();
}
