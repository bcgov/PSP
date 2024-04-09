namespace PIMS.Tests.Automation.Classes
{
    public class AcquisitionFile
    {
        public string AcquisitionStatus { get; set; } = null!;
        public string AcquisitionProjCode { get; set; } = null!;
        public string AcquisitionProject { get; set; } = null!;
        public string AcquisitionProjProductCode { get; set; } = null!;
        public string AcquisitionProjProduct { get; set; } = null!;
        public string AcquisitionProjFunding { get; set; } = null!;
        public string AcquisitionFundingOther { get; set; } = null!;
        public string AssignedDate { get; set; } = null!;
        public string DeliveryDate { get; set; } = null!;
        public string AcquisitionCompletedDate { get; set; } = null!;
        public string AcquisitionFileName { get; set; } = null!;
        public string HistoricalFileNumber { get; set; } = null!;
        public string PhysicalFileStatus { get; set; } = null!;
        public string AcquisitionType { get; set; } = null!;
        public string AcquisitionMOTIRegion { get; set; } = null!;
        public int AcquisitionTeamStartRow { get; set; } = 0;
        public int AcquisitionTeamCount { get; set; } = 0;
        public List<TeamMember> AcquisitionTeam { get; set; } = new List<TeamMember>() { };
        public int OwnerStartRow { get; set; } = 0; 
        public int OwnerCount { get; set; } = 0;
        public List<AcquisitionOwner> AcquisitionOwners { get; set; } = new List<AcquisitionOwner>() { };
        public string OwnerSolicitor { get; set; } = null!;
        public string OwnerRepresentative { get; set; } = null!;
        public string OwnerComment { get; set; } = null!;
        public int AcquisitionSearchPropertiesIndex { get; set; } = 0;
        public SearchProperty AcquisitionSearchProperties { get; set; } = new SearchProperty() { };
        public int TakesStartRow { get; set; } = 0;
        public int TakesCount { get; set; } = 0;
        public List<Take> AcquisitionTakes { get; set; } = new List<Take>() { };
        public int AcquisitionFileChecklistIndex { get; set; } = 0;
        public AcquisitionFileChecklist AcquisitionFileChecklist { get; set; } = new AcquisitionFileChecklist() { };
        public int AgreementStartRow { get; set; } = 0;
        public int AgreementCount { get; set; } = 0;
        public List<AcquisitionAgreement> AcquisitionAgreements { get; set; } = new List<AcquisitionAgreement>() { };
        public int StakeholderStartRow { get; set; } = 0;
        public int StakeholderCount { get; set; } = 0;
        public List<AcquisitionStakeholder> AcquisitionStakeholders { get; set; } = new List<AcquisitionStakeholder>();
        public int CompensationStartRow { get; set; } = 0;
        public int CompensationCount { get; set; } = 0;
        public string CompensationTotalAllowableAmount { get; set; } = null!;
        public List<AcquisitionCompensation> AcquisitionCompensations { get; set; } = new List<AcquisitionCompensation>() { };
        public int ExpropriationStartRow { get; set; } = 0;
        public int ExpropriationCount { get; set; } = 0;
        public List<AcquisitionExpropriationForm8> AcquisitionExpropriationForm8s { get; set; } = new List<AcquisitionExpropriationForm8>() { };
    }

    public class TeamMember
    {
        public string TeamMemberRole { get; set; } = null!;
        public string TeamMemberContactName { get; set; } = null!;
        public string TeamMemberContactType { get; set; } = null!;
        public string TeamMemberPrimaryContact { get; set; } = null!;
    }

    public class AcquisitionOwner
    {
        public string OwnerContactType { get; set; } = null!;
        public bool OwnerIsPrimary { get; set; } = false;
        public string OwnerGivenNames { get; set; } = null!;
        public string OwnerLastName { get; set; } = null!;
        public string OwnerOtherName { get; set; } = null!;
        public string OwnerCorporationName { get; set; } = null!;
        public string OwnerIncorporationNumber { get; set; } = null!;
        public string OwnerRegistrationNumber { get; set; } = null!;
        public Address OwnerMailAddress { get; set; } = new Address();
        public string OwnerEmail { get; set; } = null!;
        public string OwnerPhone { get; set; } = null!;
    }

    public class AcquisitionFileChecklist
    {
        public string FileInitiationSelect1 { get; set; } = null!;
        public string FileInitiationSelect2 { get; set; } = null!;
        public string FileInitiationSelect3 { get; set; } = null!;
        public string FileInitiationSelect4 { get; set; } = null!;
        public string FileInitiationSelect5 { get; set; } = null!;

        public string ActiveFileManagementSelect1 { get; set; } = null!;
        public string ActiveFileManagementSelect2 { get; set; } = null!;
        public string ActiveFileManagementSelect3 { get; set; } = null!;
        public string ActiveFileManagementSelect4 { get; set; } = null!;
        public string ActiveFileManagementSelect5 { get; set; } = null!;
        public string ActiveFileManagementSelect6 { get; set; } = null!;
        public string ActiveFileManagementSelect7 { get; set; } = null!;
        public string ActiveFileManagementSelect8 { get; set; } = null!;
        public string ActiveFileManagementSelect9 { get; set; } = null!;
        public string ActiveFileManagementSelect10 { get; set; } = null!;
        public string ActiveFileManagementSelect11 { get; set; } = null!;
        public string ActiveFileManagementSelect12 { get; set; } = null!;
        public string ActiveFileManagementSelect13 { get; set; } = null!;
        public string ActiveFileManagementSelect14 { get; set; } = null!;
        public string ActiveFileManagementSelect15 { get; set; } = null!;
        public string ActiveFileManagementSelect16 { get; set; } = null!;
        public string ActiveFileManagementSelect17 { get; set; } = null!;

        public string CrownLandSelect1 { get; set; } = null!;
        public string CrownLandSelect2 { get; set; } = null!;
        public string CrownLandSelect3 { get; set; } = null!;

        public string Section3AgreementSelect1 { get; set; } = null!;
        public string Section3AgreementSelect2 { get; set; } = null!;
        public string Section3AgreementSelect3 { get; set; } = null!;
        public string Section3AgreementSelect4 { get; set; } = null!;
        public string Section3AgreementSelect5 { get; set; } = null!;
        public string Section3AgreementSelect6 { get; set; } = null!;
        public string Section3AgreementSelect7 { get; set; } = null!;
        public string Section3AgreementSelect8 { get; set; } = null!;
        public string Section3AgreementSelect9 { get; set; } = null!;

        public string Section6ExpropriationSelect1 { get; set; } = null!;
        public string Section6ExpropriationSelect2 { get; set; } = null!;
        public string Section6ExpropriationSelect3 { get; set; } = null!;
        public string Section6ExpropriationSelect4 { get; set; } = null!;
        public string Section6ExpropriationSelect5 { get; set; } = null!;
        public string Section6ExpropriationSelect6 { get; set; } = null!;
        public string Section6ExpropriationSelect7 { get; set; } = null!;
        public string Section6ExpropriationSelect8 { get; set; } = null!;
        public string Section6ExpropriationSelect9 { get; set; } = null!;
        public string Section6ExpropriationSelect10 { get; set; } = null!;
        public string Section6ExpropriationSelect11 { get; set; } = null!;
        public string Section6ExpropriationSelect12 { get; set; } = null!;

        public string AcquisitionCompletionSelect1 { get; set; } = null!;
    }

    public class AcquisitionAgreement
    {
        public string AgreementStatus { get; set; } = null!;
        public string AgreementCancellationReason { get; set; } = null!;
        public string AgreementLegalSurveyPlan { get; set; } = null!;
        public string AgreementType { get; set; } = null!;
        public string AgreementDate { get; set; } = null!;
        public string AgreementCompletionDate { get; set; } = null!;
        public string AgreementCommencementDate { get; set; } = null!;
        public string AgreementTerminationDate { get; set; } = null!;
        public string AgreementPossessionDate { get; set; } = null!;
        public string AgreementPurchasePrice { get; set; } = null!;
        public string AgreementDepositDue { get; set; } = null!;
        public string AgreementDepositAmount { get; set; } = null!;
    }

    public class AcquisitionStakeholder
    {
        public string StakeholderType { get; set; } = null!;
        public string InterestHolder { get; set; } = null!;
        public string InterestType { get; set; } = null!;
        public string StakeholderContactType { get; set; } = null!;
        public string PrimaryContact { get; set; } = null!;
        public string PayeeName { get; set; } = null!;
        public int StakeholderIndex { get; set; } = 0;
    }

    public class AcquisitionCompensation
    {
        public string CompensationAmount { get; set; } = null!;
        public string CompensationGSTAmount { get; set; } = null!;
        public string CompensationTotalAmount { get; set; } = null!;
        public string CompensationStatus { get; set; } = null!;
        public string CompensationAlternateProject { get; set; } = null!;
        public string CompensationAgreementDate { get; set; } = null!;
        public string CompensationExpropriationNoticeDate { get; set; } = null!;
        public string CompensationExpropriationVestingDate { get; set; } = null!;
        public string CompensationAdvancedPaymentDate { get; set; } = null!;
        public string CompensationSpecialInstructions { get; set; } = null!;
        public string CompensationFiscalYear { get; set; } = null!;
        public string CompensationSTOB { get; set; } = null!;
        public string CompensationServiceLine { get; set; } = null!;
        public string CompensationResponsibilityCentre { get; set; } = null!;
        public string CompensationPayee { get; set; } = null!;
        public string CompensationPayeeDisplay { get; set; } = null!;
        public Boolean CompensationPaymentInTrust { get; set; } = false;
        public string CompensationGSTNumber { get; set; } = null!;
        public string CompensationDetailedRemarks { get; set; } = null!;
        public int ActivitiesStartRow { get; set; } = 0;
        public int ActivitiesCount { get; set; } = 0;
        public List<CompensationActivity> CompensationActivities { get; set; } = new List<CompensationActivity>();
    }

    public class CompensationActivity
    {
        public string ActCodeDescription { get; set; } = null!;
        public string ActAmount { get; set; } = null!;
        public string ActGSTEligible { get; set; } = null!;
        public string ActGSTAmount { get; set; } = null!;
        public string ActTotalAmount { get; set; } = null!;
    }

    public class Take
    {
        public string TakeType { get; set; } = null!;
        public string TakeStatus { get; set; } = null!;
        public string SiteContamination { get; set; } = null!;
        public string TakeDescription { get; set; } = null!;
        public string IsNewHighwayDedication { get; set; } = null!;
        public string IsNewHighwayDedicationArea { get; set; } = null!;
        public string IsMotiInventory { get; set; } = null!;
        public string IsNewInterestLand { get; set; } = null!;
        public string IsNewInterestLandArea { get; set; } = null!;
        public string IsNewInterestLandDate { get; set; } = null!;
        public string IsLandActTenure { get; set; } = null!;
        public string IsLandActTenureDetail { get; set; } = null!;
        public string IsLandActTenureArea { get; set; } = null!;
        public string IsLandActTenureDate { get; set; } = null!;
        public string IsLicenseConstruct { get; set; } = null!;
        public string IsLicenseConstructArea { get; set; } = null!;
        public string IsLicenseConstructDate { get; set; } = null!;
        public string IsSurplus { get; set; } = null!;
        public string IsSurplusArea { get; set; } = null!;
        public int FromProperty { get; set; } = 0;
        public int TakeCounter { get; set; } = 0;
    }

    public class AcquisitionExpropriationForm8
    {
        public string Form8Payee { get; set; } = null!;
        public string Form8PayeeDisplay { get; set; } = null!;
        public string Form8ExpropriationAuthority { get; set; } = null!;
        public string Form8Description { get; set; } = null!;
        public int ExpPaymentStartRow { get; set; } = 0;
        public int ExpPaymentCount { get; set; } = 0;
        public List<ExpropriationPayment> ExpropriationPayments { get; set; } = new List<ExpropriationPayment>() { };
    }

    public class ExpropriationPayment
    {
        public string ExpPaymentItem { get; set; } = null!;
        public string ExpPaymentAmount { get; set; } = null!;
        public string ExpPaymentGSTApplicable { get; set; } = null!;
        public string ExpPaymentGSTAmount { get; set; } = null!;
        public string ExpPaymentTotalAmount { get; set; } = null!;
    }
}
