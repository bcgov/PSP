using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_AGREEMENT")]
    [Index(nameof(AcquisitionFileId), Name = "AGRMNT_ACQUISITION_FILE_ID_IDX")]
    [Index(nameof(AgreementTypeCode), Name = "AGRMNT_AGREEMENT_TYPE_CODE_IDX")]
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
        [Column("AGREEMENT_DATE", TypeName = "date")]
        public DateTime? AgreementDate { get; set; }
        [Column("IS_DRAFT")]
        public bool? IsDraft { get; set; }
        [Column("COMPLETION_DATE", TypeName = "date")]
        public DateTime? CompletionDate { get; set; }
        [Column("TERMINATION_DATE", TypeName = "date")]
        public DateTime? TerminationDate { get; set; }
        [Column("COMMENCEMENT_DATE", TypeName = "date")]
        public DateTime? CommencementDate { get; set; }
        [Column("DEPOSIT_AMOUNT", TypeName = "money")]
        public decimal? DepositAmount { get; set; }
        [Column("NO_LATER_THAN_DAYS")]
        public int? NoLaterThanDays { get; set; }
        [Column("PURCHASE_PRICE", TypeName = "money")]
        public decimal? PurchasePrice { get; set; }
        [Column("LEGAL_SURVEY_PLAN_NUM")]
        [StringLength(250)]
        public string LegalSurveyPlanNum { get; set; }
        [Column("OFFER_DATE", TypeName = "date")]
        public DateTime? OfferDate { get; set; }
        [Column("EXPIRY_TS", TypeName = "datetime")]
        public DateTime? ExpiryTs { get; set; }
        [Column("SIGNED_DATE", TypeName = "date")]
        public DateTime? SignedDate { get; set; }
        [Column("INSPECTION_DATE", TypeName = "date")]
        public DateTime? InspectionDate { get; set; }
        [Column("EXPROPRIATION_DATE", TypeName = "date")]
        public DateTime? ExpropriationDate { get; set; }
        [Column("POSSESSION_DATE", TypeName = "date")]
        public DateTime? PossessionDate { get; set; }
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

        [ForeignKey(nameof(AcquisitionFileId))]
        [InverseProperty(nameof(PimsAcquisitionFile.PimsAgreements))]
        public virtual PimsAcquisitionFile AcquisitionFile { get; set; }
        [ForeignKey(nameof(AgreementTypeCode))]
        [InverseProperty(nameof(PimsAgreementType.PimsAgreements))]
        public virtual PimsAgreementType AgreementTypeCodeNavigation { get; set; }
    }
}
