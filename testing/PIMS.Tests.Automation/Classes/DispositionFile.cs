namespace PIMS.Tests.Automation.Classes
{
    public class DispositionFile
    {
        public string? DispositionFileStatus { get; set; } = String.Empty;
        public string? DispositionProjFunding { get; set; } = String.Empty;
        public string? DispositionAssignedDate { get; set; } = String.Empty;
        public string? DispositionCompletedDate { get; set; } = String.Empty;
        public string? DispositionFileName { get; set; } = String.Empty;
        public string? DispositionReferenceNumber { get; set; } = String.Empty;
        public string? DispositionStatus { get; set; } = null!;
        public string? DispositionType { get; set; } = null!;
        public string? DispositionOtherTransferType { get; set; } = String.Empty;
        public string? InitiatingDocument { get; set; } = String.Empty;
        public string? OtherInitiatingDocument { get; set; } = String.Empty;
        public string? InitiatingDocumentDate { get; set; } = String.Empty;
        public string? PhysicalFileStatus { get; set; } = String.Empty;
        public string? InitiatingBranch { get; set; } = String.Empty;
        public string DispositionMOTIRegion { get; set; } = null!;
        public int DispositionTeamStartRow { get; set; } = 0;
        public int DispositionTeamCount { get; set; } = 0;
        public List<TeamMember>? DispositionTeam { get; set; } = new List<TeamMember>();
        public int DispositionSearchPropertiesIndex { get; set; } = 0;
        public SearchProperty? DispositionSearchProperties { get; set; } = new SearchProperty();
        public int DispositionFileChecklistIndex { get; set; } = 0;
        public DispositionFileChecklist? DispositionFileChecklist { get; set; } = new DispositionFileChecklist();
        public string? AppraisalAndAssessmentValue { get; set; } = String.Empty;
        public string? AppraisalAndAssessmentDate { get; set; } = String.Empty;
        public string? AppraisalAndAssessmentBcAssessmentValue { get; set; } = String.Empty;
        public string? AppraisalAndAssessmentBcAssessmentRollYear { get; set; } = String.Empty;
        public string? AppraisalAndAssessmentListPrice { get; set; } = String.Empty;
        public int OfferSaleStartRow { get; set; } = 0;
        public int OfferSaleTotalCount { get; set; } = 0;
        public List<DispositionOfferAndSale>? DispositionOfferAndSale { get; set; } = new List<DispositionOfferAndSale>();
    }
    public class DispositionFileChecklist
    {
        public string? FileInitiationSelect1 { get; set; } = String.Empty;
        public string? FileInitiationSelect2 { get; set; } = String.Empty;
        public string? FileInitiationSelect3 { get; set; } = String.Empty;
        public string? FileInitiationSelect4 { get; set; } = String.Empty;
        public string? FileInitiationSelect5 { get; set; } = String.Empty;

        public string? DispositionPreparationSelect1 { get; set; } = String.Empty;
        public string? DispositionPreparationSelect2 { get; set; } = String.Empty;
        public string? DispositionPreparationSelect3 { get; set; } = String.Empty;
        public string? DispositionPreparationSelect4 { get; set; } = String.Empty;
        public string? ReferralsAndConsultationsSelect1 { get; set; } = String.Empty;
        public string? ReferralsAndConsultationsSelect2 { get; set; } = String.Empty;
        public string? ReferralsAndConsultationsSelect3 { get; set; } = String.Empty;
        public string? ReferralsAndConsultationsSelect4 { get; set; } = String.Empty;
        public string? ReferralsAndConsultationsSelect5 { get; set; } = String.Empty;
        public string? ReferralsAndConsultationsSelect6 { get; set; } = String.Empty;
        public string? ReferralsAndConsultationsSelect7 { get; set; } = String.Empty;

        public string? DirectSaleRoadClosureSelect1 { get; set; } = String.Empty;
        public string? DirectSaleRoadClosureSelect2 { get; set; } = String.Empty;
        public string? DirectSaleRoadClosureSelect3 { get; set; } = String.Empty;
        public string? DirectSaleRoadClosureSelect4 { get; set; } = String.Empty;
        public string? DirectSaleRoadClosureSelect5 { get; set; } = String.Empty;
        public string? DirectSaleRoadClosureSelect6 { get; set; } = String.Empty;
        public string? DirectSaleRoadClosureSelect7 { get; set; } = String.Empty;
        public string? DirectSaleRoadClosureSelect8 { get; set; } = String.Empty;
        public string? DirectSaleRoadClosureSelect9 { get; set; } = String.Empty;

        public string? SaleInformationSelect1 { get; set; } = String.Empty;
        public string? SaleInformationSelect2 { get; set; } = String.Empty;
        public string? SaleInformationSelect3 { get; set; } = String.Empty;
        public string? SaleInformationSelect4 { get; set; } = String.Empty;
        public string? SaleInformationSelect5 { get; set; } = String.Empty;
        public string? SaleInformationSelect6 { get; set; } = String.Empty;
        public string? SaleInformationSelect7 { get; set; } = String.Empty;
        public string? SaleInformationSelect8 { get; set; } = String.Empty;
        public string? SaleInformationSelect9 { get; set; } = String.Empty;
        public string? SaleInformationSelect10 { get; set; } = String.Empty;
        public string? SaleInformationSelect11 { get; set; } = String.Empty;
    }
    public class DispositionOfferAndSale
    {
        public string? OfferOfferStatus { get; set; } = String.Empty;
        public string? OfferOfferName{ get; set; } = null!;
        public string? OfferOfferDate { get; set; } = null!;
        public string? OfferOfferExpiryDate { get; set; } = String.Empty;
        public string? OfferPrice { get; set; } = null!;
        public string? OfferNotes { get; set; } = String.Empty;
    }
}
