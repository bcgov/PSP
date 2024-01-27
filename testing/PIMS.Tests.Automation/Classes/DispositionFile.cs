

namespace PIMS.Tests.Automation.Classes
{
    public class DispositionFile
    {
        public string? DispositionFileStatus { get; set; } = String.Empty;

        public string? AssignedDate { get; set; } = String.Empty;

        public string? DispositionCompletedDate { get; set; } = String.Empty;

        public string? DispositionFileName { get; set; } = String.Empty;

        public string? ReferenceNumber { get; set; } = String.Empty;

        public string? DispositionStatus { get; set; } = null!;

        public string? DispositionType { get; set; } = null!;

        public string? InitiatingDocument { get; set; } = String.Empty;

        public string? OtherInitiatingDocument { get; set; } = String.Empty;

        public string? InitiatingDocumentDate { get; set; } = String.Empty;

        public string? PhysicalFileStatus { get; set; } = String.Empty;

        public string? InitiatingBranch { get; set; } = String.Empty;

        public string DispositionMOTIRegion { get; set; } = null!;


    }
    public class DispositionOfferAndSale
    {
        public string AppraisalAndAssessmentAppraisalValue { get; set; } = String.Empty;
        public string? AppraisalAndAssessmentAppraisalDate { get; set; } = String.Empty;
        public string AppraisalAndAssessmentBcAssessmentValue { get; set; } = String.Empty;
        public string? AppraisalAndAssessmentBcAssessmentRollYear { get; set; } = String.Empty;
        public string? AppraisalAndAssessmentListPrice { get; set; } = String.Empty;
        public string? OfferOfferStatus { get; set; } = String.Empty;
        public string? OfferOfferName{ get; set; } = null!;
        public string? OfferOfferDate { get; set; } = null!;
        public string? OfferOfferExpiryDate { get; set; } = String.Empty;
        public string? OfferPrice { get; set; } = null!;
        public string? OfferNotes { get; set; } = String.Empty;
    }
}
