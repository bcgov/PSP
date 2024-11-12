using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Associative entity to connect expropriation forms (Form 8) to payment item types.  The supports the ability to associate multiple payment item types to a single expropriation form (Form 8).
/// </summary>
[Table("PIMS_EXPROP_PMT_PMT_ITEM")]
[Index("ExpropriationPaymentId", Name = "XPMTITY_EXPROPRIATION_PAYMENT_ID_IDX")]
[Index("PaymentItemTypeCode", "ExpropriationPaymentId", Name = "XPMTITY_EXPROP_PMT_PMT_TYPE_TUC", IsUnique = true)]
[Index("PaymentItemTypeCode", Name = "XPMTITY_PAYMENT_ITEM_TYPE_CODE_IDX")]
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

    /// <summary>
    /// Indicates if GST is required for this transaction.
    /// </summary>
    [Column("IS_GST_REQUIRED")]
    public bool? IsGstRequired { get; set; }

    /// <summary>
    /// Subtotal of the Form 8 payment.
    /// </summary>
    [Column("PRETAX_AMT", TypeName = "money")]
    public decimal? PretaxAmt { get; set; }

    /// <summary>
    /// GST on the Form 8 oayment.
    /// </summary>
    [Column("TAX_AMT", TypeName = "money")]
    public decimal? TaxAmt { get; set; }

    /// <summary>
    /// Total amount of the Form 8 payment.
    /// </summary>
    [Column("TOTAL_AMT", TypeName = "money")]
    public decimal? TotalAmt { get; set; }

    /// <summary>
    /// Indicates if the relationship is active.
    /// </summary>
    [Column("IS_DISABLED")]
    public bool? IsDisabled { get; set; }

    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long? ConcurrencyControlNumber { get; set; }

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

    [ForeignKey("ExpropriationPaymentId")]
    [InverseProperty("PimsExpropPmtPmtItems")]
    public virtual PimsExpropriationPayment ExpropriationPayment { get; set; }

    [ForeignKey("PaymentItemTypeCode")]
    [InverseProperty("PimsExpropPmtPmtItems")]
    public virtual PimsPaymentItemType PaymentItemTypeCodeNavigation { get; set; }
}
