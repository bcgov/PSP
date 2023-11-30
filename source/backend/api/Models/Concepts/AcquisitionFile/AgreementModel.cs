using System;

namespace Pims.Api.Models.Concepts
{
    public class AgreementModel : BaseAppModel
    {
        public long AgreementId { get; set; }

        public long AcquisitionFileId { get; set; }

        public TypeModel<string> AgreementType { get; set; }

        public TypeModel<string> AgreementStatusType { get; set; }

        public DateOnly? AgreementDate { get; set; }

        public bool? IsDraft { get; set; }

        public DateOnly? CompletionDate { get; set; }

        public DateOnly? TerminationDate { get; set; }

        public DateOnly? CommencementDate { get; set; }

        public DateOnly? PossessionDate { get; set; }

        public decimal? DepositAmount { get; set; }

        public int? NoLaterThanDays { get; set; }

        public decimal? PurchasePrice { get; set; }

        public string LegalSurveyPlanNum { get; set; }

        public DateOnly? OfferDate { get; set; }

        public DateTime? ExpiryDateTime { get; set; }

        public DateOnly? SignedDate { get; set; }

        public DateOnly? InspectionDate { get; set; }

        public string CancellationNote { get; set; }
    }
}
