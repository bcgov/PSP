using System;
using Pims.Api.Models.Base;

namespace Pims.Api.Models.Concepts.AcquisitionFile
{
    public class AgreementModel : BaseAuditModel
    {
        public long AgreementId { get; set; }

        public long AcquisitionFileId { get; set; }

        public TypeModel<string> AgreementType { get; set; }

        public TypeModel<string> AgreementStatusType { get; set; }

        public DateTime? AgreementDate { get; set; }

        public bool? IsDraft { get; set; }

        public DateTime? CompletionDate { get; set; }

        public DateTime? TerminationDate { get; set; }

        public DateTime? CommencementDate { get; set; }

        public DateTime? PossessionDate { get; set; }

        public decimal? DepositAmount { get; set; }

        public int? NoLaterThanDays { get; set; }

        public decimal? PurchasePrice { get; set; }

        public string LegalSurveyPlanNum { get; set; }

        public DateTime? OfferDate { get; set; }

        public DateTime? ExpiryDateTime { get; set; }

        public DateTime? SignedDate { get; set; }

        public DateTime? InspectionDate { get; set; }

        public string CancellationNote { get; set; }
    }
}
