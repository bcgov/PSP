namespace PIMS.Tests.Automation.Classes
{
    public class DispositionFile
    {
        public string DispositionFileStatus { get; set; } = null!;
        public string DispositionProjFunding { get; set; } = null!;
        public string DispositionAssignedDate { get; set; } = null!;
        public string DispositionCompletedDate { get; set; } = null!;
        public string DispositionFileName { get; set; } = null!;
        public string DispositionReferenceNumber { get; set; } = null!;
        public string DispositionStatus { get; set; } = null!;
        public string DispositionType { get; set; } = null!;
        public string DispositionOtherTransferType { get; set; } = null!;
        public string InitiatingDocument { get; set; } = null!;
        public string OtherInitiatingDocument { get; set; } = null!;
        public string InitiatingDocumentDate { get; set; } = null!;
        public string DispositionPhysicalFileStatus { get; set; } = null!;
        public string InitiatingBranch { get; set; } = null!;
        public string DispositionMOTIRegion { get; set; } = null!;
        public int DispositionTeamStartRow { get; set; } = 0;
        public int DispositionTeamCount { get; set; } = 0;
        public List<TeamMember> DispositionTeam { get; set; } = new List<TeamMember>() { };
        public int DispositionSearchPropertiesIndex { get; set; } = 0;
        public SearchProperty DispositionSearchProperties { get; set; } = new SearchProperty() { };
        public int DispositionFileChecklistIndex { get; set; } = 0;
        public DispositionFileChecklist DispositionFileChecklist { get; set; } = new DispositionFileChecklist() { };
        public string AppraisalAndAssessmentValue { get; set; } = null!;
        public string AppraisalAndAssessmentDate { get; set; } = null!;
        public string AppraisalAndAssessmentBcAssessmentValue { get; set; } = null!;
        public string AppraisalAndAssessmentBcAssessmentRollYear { get; set; } = null!;
        public string AppraisalAndAssessmentListPrice { get; set; } = null!;
        public int OfferSaleStartRow { get; set; } = 0;
        public int OfferSaleTotalCount { get; set; } = 0;
        public List<DispositionOfferAndSale> DispositionOfferAndSale { get; set; } = new List<DispositionOfferAndSale>();
        public int PurchaseNameStartRow { get; set; } = 0;
        public int PurchaseNameTotalCount { get; set; } = 0;
        public List<PurchaseMember> PurchaserNames { get; set; } = new List<PurchaseMember>();
        public string PurchaserAgent { get; set; } = null!;
        public string PurchaserAgentType { get; set; } = null!;
        public string PurchaserAgentPrimaryContact { get; set; } = null!;
        public string PurchaserSolicitor { get; set; } = null!;
        public string PurchaserSolicitorType { get; set; } = null!;
        public string PurchaserSolicitorPrimaryContact { get; set; } = null!;
        public string LastConditionRemovalDate { get; set; } = null!;
        public string SaleCompletionDate { get; set; } = null!;
        public string FiscalYearOfSale { get; set; } = null!;
        public string FinalSalePrice { get; set; } = null!;
        public string RealtorCommission { get; set; } = null!;
        public string GSTRequired { get; set; } = null!;
        public string GSTCollected { get; set; } = null!;
        public string NetBookValue { get; set; } = null!;
        public string TotalCostOfSales { get; set; } = null!;
        public string NetProceedsBeforeSPP { get; set; } = null!;
        public string SPPAmount { get; set; } = null!;
        public string NetProceedsAfterSPP { get; set; } = null!;
        public string RemediationCost { get; set; } = null!;

    }
    public class DispositionFileChecklist
    {
        public string FileInitiationSelect1 { get; set; } = null!;
        public string FileInitiationSelect2 { get; set; } = null!;
        public string FileInitiationSelect3 { get; set; } = null!;
        public string FileInitiationSelect4 { get; set; } = null!;
        public string FileInitiationSelect5 { get; set; } = null!;

        public string DispositionPreparationSelect1 { get; set; } = null!;
        public string DispositionPreparationSelect2 { get; set; } = null!;
        public string DispositionPreparationSelect3 { get; set; } = null!;
        public string DispositionPreparationSelect4 { get; set; } = null!;
        public string ReferralsAndConsultationsSelect1 { get; set; } = null!;
        public string ReferralsAndConsultationsSelect2 { get; set; } = null!;
        public string ReferralsAndConsultationsSelect3 { get; set; } = null!;
        public string ReferralsAndConsultationsSelect4 { get; set; } = null!;
        public string ReferralsAndConsultationsSelect5 { get; set; } = null!;
        public string ReferralsAndConsultationsSelect6 { get; set; } = null!;
        public string ReferralsAndConsultationsSelect7 { get; set; } = null!;

        public string DirectSaleRoadClosureSelect1 { get; set; } = null!;
        public string DirectSaleRoadClosureSelect2 { get; set; } = null!;
        public string DirectSaleRoadClosureSelect3 { get; set; } = null!;
        public string DirectSaleRoadClosureSelect4 { get; set; } = null!;
        public string DirectSaleRoadClosureSelect5 { get; set; } = null!;
        public string DirectSaleRoadClosureSelect6 { get; set; } = null!;
        public string DirectSaleRoadClosureSelect7 { get; set; } = null!;
        public string DirectSaleRoadClosureSelect8 { get; set; } = null!;
        public string DirectSaleRoadClosureSelect9 { get; set; } = null!;

        public string SaleInformationSelect1 { get; set; } = null!;
        public string SaleInformationSelect2 { get; set; } = null!;
        public string SaleInformationSelect3 { get; set; } = null!;
        public string SaleInformationSelect4 { get; set; } = null!;
        public string SaleInformationSelect5 { get; set; } = null!;
        public string SaleInformationSelect6 { get; set; } = null!;
        public string SaleInformationSelect7 { get; set; } = null!;
        public string SaleInformationSelect8 { get; set; } = null!;
        public string SaleInformationSelect9 { get; set; } = null!;
        public string SaleInformationSelect10 { get; set; } = null!;
        public string SaleInformationSelect11 { get; set; } = null!;
    }
    public class DispositionOfferAndSale
    {
        public string OfferOfferStatus { get; set; } = null!;
        public string OfferOfferName{ get; set; } = null!;
        public string OfferOfferDate { get; set; } = null!;
        public string OfferOfferExpiryDate { get; set; } = null!;
        public string OfferPrice { get; set; } = null!;
        public string OfferNotes { get; set; } = null!;
    }

    public class PurchaseMember
    {
        public string PurchaserName { get; set; } = null!;
        public string PurchaseMemberContactType { get; set; } = null!;
        public string PurchaseMemberPrimaryContact { get; set; } = null!;
    }
}
