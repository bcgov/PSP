using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

[Table("PIMS_DISPOSITION_AGREEMENT_HIST")]
[Index("DispositionAgreementHistId", "EndDateHist", Name = "PIMS_DSPAGR_H_UK", IsUnique = true)]
public partial class PimsDispositionAgreementHist
{
    [Key]
    [Column("_DISPOSITION_AGREEMENT_HIST_ID")]
    public long DispositionAgreementHistId { get; set; }

    [Column("EFFECTIVE_DATE_HIST", TypeName = "datetime")]
    public DateTime EffectiveDateHist { get; set; }

    [Column("END_DATE_HIST", TypeName = "datetime")]
    public DateTime? EndDateHist { get; set; }

    [Column("DISPOSITION_AGREEMENT_ID")]
    public long DispositionAgreementId { get; set; }

    [Column("DISPOSITION_FILE_ID")]
    public long DispositionFileId { get; set; }

    [Required]
    [Column("DISPOSITION_AGREEMENT_TYPE_CODE")]
    [StringLength(20)]
    public string DispositionAgreementTypeCode { get; set; }

    [Required]
    [Column("DSP_AGREEMENT_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string DspAgreementStatusTypeCode { get; set; }

    [Column("AGREEMENT_DATE")]
    public DateOnly? AgreementDate { get; set; }

    [Column("COMPLETION_DATE")]
    public DateOnly? CompletionDate { get; set; }

    [Column("TERMINATION_DATE")]
    public DateOnly? TerminationDate { get; set; }

    [Column("COMMENCEMENT_DATE")]
    public DateOnly? CommencementDate { get; set; }

    [Column("DEPOSIT_AMOUNT", TypeName = "money")]
    public decimal? DepositAmount { get; set; }

    [Column("NO_LATER_THAN_DAYS")]
    public int? NoLaterThanDays { get; set; }

    [Column("PURCHASE_PRICE", TypeName = "money")]
    public decimal? PurchasePrice { get; set; }

    [Column("LEGAL_SURVEY_PLAN_NUM")]
    [StringLength(250)]
    public string LegalSurveyPlanNum { get; set; }

    [Column("OFFER_DATE")]
    public DateOnly? OfferDate { get; set; }

    [Column("EXPIRY_TS", TypeName = "datetime")]
    public DateTime? ExpiryTs { get; set; }

    [Column("SIGNED_DATE")]
    public DateOnly? SignedDate { get; set; }

    [Column("INSPECTION_DATE")]
    public DateOnly? InspectionDate { get; set; }

    [Column("EXPROPRIATION_DATE")]
    public DateOnly? ExpropriationDate { get; set; }

    [Column("POSSESSION_DATE")]
    public DateOnly? PossessionDate { get; set; }

    [Column("CANCELLATION_NOTE")]
    [StringLength(2000)]
    public string CancellationNote { get; set; }

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
