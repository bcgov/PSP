using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Table containing the details of the acquisition agreement.
/// </summary>
[Table("PIMS_AGREEMENT")]
[Index("AcquisitionFileId", Name = "AGRMNT_ACQUISITION_FILE_ID_IDX")]
[Index("AgreementStatusTypeCode", Name = "AGRMNT_AGREEMENT_STATUS_TYPE_CODE_IDX")]
[Index("AgreementTypeCode", Name = "AGRMNT_AGREEMENT_TYPE_CODE_IDX")]
public partial class PimsAgreement
{
    [Key]
    [Column("AGREEMENT_ID")]
    public long AgreementId { get; set; }

    [Column("ACQUISITION_FILE_ID")]
    public long AcquisitionFileId { get; set; }

    [Required]
    [Column("AGREEMENT_TYPE_CODE")]
    [StringLength(20)]
    public string AgreementTypeCode { get; set; }

    [Required]
    [Column("AGREEMENT_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string AgreementStatusTypeCode { get; set; }

    /// <summary>
    /// Date of the agreement.
    /// </summary>
    [Column("AGREEMENT_DATE")]
    public DateOnly? AgreementDate { get; set; }

    /// <summary>
    /// Date of completion of the agreement.
    /// </summary>
    [Column("COMPLETION_DATE")]
    public DateOnly? CompletionDate { get; set; }

    /// <summary>
    /// Date of termination of the agreement.
    /// </summary>
    [Column("TERMINATION_DATE")]
    public DateOnly? TerminationDate { get; set; }

    /// <summary>
    /// Date of commencement of the agreement.
    /// </summary>
    [Column("COMMENCEMENT_DATE")]
    public DateOnly? CommencementDate { get; set; }

    /// <summary>
    /// Amount of the deposit on the agreement.
    /// </summary>
    [Column("DEPOSIT_AMOUNT", TypeName = "money")]
    public decimal? DepositAmount { get; set; }

    /// <summary>
    /// Deposit due date
    /// </summary>
    [Column("NO_LATER_THAN_DAYS")]
    public int? NoLaterThanDays { get; set; }

    /// <summary>
    /// Amount of the purchase price of the agreement.
    /// </summary>
    [Column("PURCHASE_PRICE", TypeName = "money")]
    public decimal? PurchasePrice { get; set; }

    /// <summary>
    /// Legal survey plan number,
    /// </summary>
    [Column("LEGAL_SURVEY_PLAN_NUM")]
    [StringLength(250)]
    public string LegalSurveyPlanNum { get; set; }

    /// <summary>
    /// Date of acquisition offer.
    /// </summary>
    [Column("OFFER_DATE")]
    public DateOnly? OfferDate { get; set; }

    /// <summary>
    /// Expiry date and time of acquisition offer.
    /// </summary>
    [Column("EXPIRY_TS", TypeName = "datetime")]
    public DateTime? ExpiryTs { get; set; }

    /// <summary>
    /// Signed date of acquisition offer.
    /// </summary>
    [Column("SIGNED_DATE")]
    public DateOnly? SignedDate { get; set; }

    /// <summary>
    /// Date of inspection.
    /// </summary>
    [Column("INSPECTION_DATE")]
    public DateOnly? InspectionDate { get; set; }

    /// <summary>
    /// Date of expropriation of the property.
    /// </summary>
    [Column("EXPROPRIATION_DATE")]
    public DateOnly? ExpropriationDate { get; set; }

    /// <summary>
    /// Date of possession of the property.
    /// </summary>
    [Column("POSSESSION_DATE")]
    public DateOnly? PossessionDate { get; set; }

    /// <summary>
    /// Note pertaining to the cancellation of the agreement.
    /// </summary>
    [Column("CANCELLATION_NOTE")]
    [StringLength(2000)]
    public string CancellationNote { get; set; }

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

    [ForeignKey("AcquisitionFileId")]
    [InverseProperty("PimsAgreements")]
    public virtual PimsAcquisitionFile AcquisitionFile { get; set; }

    [ForeignKey("AgreementStatusTypeCode")]
    [InverseProperty("PimsAgreements")]
    public virtual PimsAgreementStatusType AgreementStatusTypeCodeNavigation { get; set; }

    [ForeignKey("AgreementTypeCode")]
    [InverseProperty("PimsAgreements")]
    public virtual PimsAgreementType AgreementTypeCodeNavigation { get; set; }
}
