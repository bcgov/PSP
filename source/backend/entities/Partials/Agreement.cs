using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsAgreement class, provides an entity for the datamodel to manage agreements.
    /// </summary>
    public partial class PimsAgreement : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.AgreementId; set => this.AgreementId = value; }
        #endregion

        public bool IsEqual(PimsAgreement other)
        {
            return AgreementId == other.AgreementId &&
                AcquisitionFileId == other.AcquisitionFileId &&
                AgreementTypeCode == other.AgreementTypeCode &&
                AgreementStatusTypeCode == other.AgreementStatusTypeCode &&
                AgreementDate == other.AgreementDate &&
                CompletionDate == other.CompletionDate &&
                TerminationDate == other.TerminationDate &&
                CommencementDate == other.CommencementDate &&
                DepositAmount == other.DepositAmount &&
                NoLaterThanDays == other.NoLaterThanDays &&
                PurchasePrice == other.PurchasePrice &&
                LegalSurveyPlanNum == other.LegalSurveyPlanNum &&
                OfferDate == other.OfferDate &&
                ExpiryTs == other.ExpiryTs &&
                SignedDate == other.SignedDate &&
                InspectionDate == other.InspectionDate &&
                ExpropriationDate == other.ExpropriationDate &&
                PossessionDate == other.PossessionDate &&
                CancellationNote == other.CancellationNote;
        }
    }
}
