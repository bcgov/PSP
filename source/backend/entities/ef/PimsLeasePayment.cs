using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Describes a payment associated with a lease term.
/// </summary>
[Table("PIMS_LEASE_PAYMENT")]
[Index("LeasePaymentCategoryTypeCode", Name = "LSPYMT_LEASE_PAYMENT_CATEGORY_TYPE_CODE_IDX")]
[Index("LeasePaymentMethodTypeCode", Name = "LSPYMT_LEASE_PAYMENT_METHOD_TYPE_CODE_IDX")]
[Index("LeasePaymentStatusTypeCode", Name = "LSPYMT_LEASE_PAYMENT_STATUS_TYPE_CODE_IDX")]
[Index("LeasePeriodId", Name = "LSPYMT_LEASE_PERIOD_ID_IDX")]
[Index("LeasePmtFreqTypeCode", Name = "LSPYMT_LEASE_PMT_FREQ_TYPE_CODE_IDX")]
public partial class PimsLeasePayment
{
    /// <summary>
    /// Generated surrogate primary key.
    /// </summary>
    [Key]
    [Column("LEASE_PAYMENT_ID")]
    public long LeasePaymentId { get; set; }

    /// <summary>
    /// Foreign key reference to the PIMS_LEASE_PERIOD table.
    /// </summary>
    [Column("LEASE_PERIOD_ID")]
    public long LeasePeriodId { get; set; }

    /// <summary>
    /// Foreign key reference to the PIMS_LEASE_PAYMENT_METHOD_TYPE_CODE table.
    /// </summary>
    [Required]
    [Column("LEASE_PAYMENT_METHOD_TYPE_CODE")]
    [StringLength(20)]
    public string LeasePaymentMethodTypeCode { get; set; }

    /// <summary>
    /// Foreign key reference to the PIMS_LEASE_PAYMENT_STATUS_TYPE_CODE table.
    /// </summary>
    [Column("LEASE_PAYMENT_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string LeasePaymentStatusTypeCode { get; set; }

    /// <summary>
    /// Foreign key reference to the PIMS_LEASE_PAYMENT_CATEGORY_TYPE_CODE table.
    /// </summary>
    [Column("LEASE_PAYMENT_CATEGORY_TYPE_CODE")]
    [StringLength(20)]
    public string LeasePaymentCategoryTypeCode { get; set; }

    /// <summary>
    /// Foreign key reference to the PIMS_LEASE_PMT_FREQ_TYPE_CODE table.
    /// </summary>
    [Column("LEASE_PMT_FREQ_TYPE_CODE")]
    [StringLength(20)]
    public string LeasePmtFreqTypeCode { get; set; }

    /// <summary>
    /// Date the payment was received or sent
    /// </summary>
    [Column("PAYMENT_RECEIVED_DATE", TypeName = "datetime")]
    public DateTime PaymentReceivedDate { get; set; }

    /// <summary>
    /// Principal amount of the payment before applicable taxes
    /// </summary>
    [Column("PAYMENT_AMOUNT_PRE_TAX", TypeName = "money")]
    public decimal PaymentAmountPreTax { get; set; }

    /// <summary>
    /// PST owing on payment if applicable
    /// </summary>
    [Column("PAYMENT_AMOUNT_PST", TypeName = "money")]
    public decimal? PaymentAmountPst { get; set; }

    /// <summary>
    /// GST owing on payment if applicable
    /// </summary>
    [Column("PAYMENT_AMOUNT_GST", TypeName = "money")]
    public decimal? PaymentAmountGst { get; set; }

    /// <summary>
    /// Total amount of payment including principal plus all applicable taxes
    /// </summary>
    [Column("PAYMENT_AMOUNT_TOTAL", TypeName = "money")]
    public decimal PaymentAmountTotal { get; set; }

    /// <summary>
    /// Notes regarding this payment
    /// </summary>
    [Column("NOTE")]
    [StringLength(2000)]
    public string Note { get; set; }

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

    [ForeignKey("LeasePaymentCategoryTypeCode")]
    [InverseProperty("PimsLeasePayments")]
    public virtual PimsLeasePaymentCategoryType LeasePaymentCategoryTypeCodeNavigation { get; set; }

    [ForeignKey("LeasePaymentMethodTypeCode")]
    [InverseProperty("PimsLeasePayments")]
    public virtual PimsLeasePaymentMethodType LeasePaymentMethodTypeCodeNavigation { get; set; }

    [ForeignKey("LeasePaymentStatusTypeCode")]
    [InverseProperty("PimsLeasePayments")]
    public virtual PimsLeasePaymentStatusType LeasePaymentStatusTypeCodeNavigation { get; set; }

    [ForeignKey("LeasePeriodId")]
    [InverseProperty("PimsLeasePayments")]
    public virtual PimsLeasePeriod LeasePeriod { get; set; }

    [ForeignKey("LeasePmtFreqTypeCode")]
    [InverseProperty("PimsLeasePayments")]
    public virtual PimsLeasePmtFreqType LeasePmtFreqTypeCodeNavigation { get; set; }
}
