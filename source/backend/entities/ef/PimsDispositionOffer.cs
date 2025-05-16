using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Entity containing information regarding an disposition offer.
/// </summary>
[Table("PIMS_DISPOSITION_OFFER")]
[Index("DispositionFileId", Name = "DSPOFR_DISPOSITION_FILE_ID_IDX")]
[Index("DispositionOfferStatusTypeCode", Name = "DSPOFR_DISPOSITION_OFFER_STATUS_TYPE_CODE_IDX")]
public partial class PimsDispositionOffer
{
    /// <summary>
    /// Unique auto-generated surrogate primary key
    /// </summary>
    [Key]
    [Column("DISPOSITION_OFFER_ID")]
    public long DispositionOfferId { get; set; }

    /// <summary>
    /// Foreign key value for the dispostion file
    /// </summary>
    [Column("DISPOSITION_FILE_ID")]
    public long DispositionFileId { get; set; }

    /// <summary>
    /// Code value for the dispostion offer status.
    /// </summary>
    [Column("DISPOSITION_OFFER_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string DispositionOfferStatusTypeCode { get; set; }

    /// <summary>
    /// The name(s) associated with this disposition offer.
    /// </summary>
    [Required]
    [Column("OFFER_NAME")]
    [StringLength(1000)]
    public string OfferName { get; set; }

    /// <summary>
    /// The date the disposition offer was made.
    /// </summary>
    [Column("OFFER_DT")]
    public DateOnly OfferDt { get; set; }

    /// <summary>
    /// The date the disposition offer expires.
    /// </summary>
    [Column("OFFER_EXPIRY_DT")]
    public DateOnly? OfferExpiryDt { get; set; }

    /// <summary>
    /// The monetary value of the disposition offer.
    /// </summary>
    [Column("OFFER_AMT", TypeName = "money")]
    public decimal OfferAmt { get; set; }

    /// <summary>
    /// Provide any additional details such as offer terms or conditions, and any commentary on why the offer was accepted/countered/rejected.
    /// </summary>
    [Column("OFFER_NOTE")]
    [StringLength(2000)]
    public string OfferNote { get; set; }

    /// <summary>
    /// Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update.  If this is done then the update will succeed, provided that the row was not updated by any
    /// </summary>
    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long ConcurrencyControlNumber { get; set; }

    /// <summary>
    /// The date and time the record was created by the user.
    /// </summary>
    [Column("APP_CREATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppCreateTimestamp { get; set; }

    /// <summary>
    /// The user that created the record.
    /// </summary>
    [Required]
    [Column("APP_CREATE_USERID")]
    [StringLength(30)]
    public string AppCreateUserid { get; set; }

    /// <summary>
    /// GUID of the user that created the record.
    /// </summary>
    [Column("APP_CREATE_USER_GUID")]
    public Guid? AppCreateUserGuid { get; set; }

    /// <summary>
    /// User directory of the user that created the record.
    /// </summary>
    [Required]
    [Column("APP_CREATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppCreateUserDirectory { get; set; }

    /// <summary>
    /// The date and time the record was updated by the user.
    /// </summary>
    [Column("APP_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppLastUpdateTimestamp { get; set; }

    /// <summary>
    /// The user that updated the record.
    /// </summary>
    [Required]
    [Column("APP_LAST_UPDATE_USERID")]
    [StringLength(30)]
    public string AppLastUpdateUserid { get; set; }

    /// <summary>
    /// GUID of the user that updated the record.
    /// </summary>
    [Column("APP_LAST_UPDATE_USER_GUID")]
    public Guid? AppLastUpdateUserGuid { get; set; }

    /// <summary>
    /// User directory of the user that updated the record.
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

    [ForeignKey("DispositionFileId")]
    [InverseProperty("PimsDispositionOffers")]
    public virtual PimsDispositionFile DispositionFile { get; set; }

    [ForeignKey("DispositionOfferStatusTypeCode")]
    [InverseProperty("PimsDispositionOffers")]
    public virtual PimsDispositionOfferStatusType DispositionOfferStatusTypeCodeNavigation { get; set; }
}
