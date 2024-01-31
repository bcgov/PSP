namespace PIMS.Tests.Automation.Classes
{
    public class AcquisitionFile
    {
        public string? AcquisitionStatus { get; set; } = String.Empty;
        public string? AcquisitionProjCode { get; set; } = String.Empty;
        public string? AcquisitionProject { get; set; } = String.Empty;
        public string? AcquisitionProjProductCode { get; set; } = String.Empty; 
        public string? AcquisitionProjProduct { get; set; } = String.Empty;
        public string? AcquisitionProjFunding { get; set; } = String.Empty;
        public string? AcquisitionFundingOther { get; set; } = String.Empty;
        public string? AssignedDate { get; set; } = String.Empty;
        public string? DeliveryDate { get; set; } = String.Empty;
        public string? AcquisitionCompletedDate { get; set; } = String.Empty;
        public string AcquisitionFileName { get; set; } = null!;
        public string? HistoricalFileNumber { get; set; } = String.Empty;
        public string? PhysicalFileStatus { get; set; } = String.Empty;
        public string AcquisitionType { get; set; } = null!;
        public string AcquisitionMOTIRegion { get; set; } = null!;
        public int AcquisitionTeamStartRow { get; set; } = 0;
        public int AcquisitionTeamCount { get; set; } = 0;
        public List<TeamMember>? AcquisitionTeam { get; set; } = new List<TeamMember>();
        public int OwnerStartRow { get; set; } = 0; 
        public int OwnerCount { get; set; } = 0;
        public List<AcquisitionOwner>? AcquisitionOwners { get; set; } = new List<AcquisitionOwner>();
        public string? OwnerSolicitor { get; set; } = String.Empty;
        public string? OwnerRepresentative { get; set; } = String.Empty;
        public string? OwnerComment { get; set; } = String.Empty;
        public int AcquisitionSearchPropertiesIndex { get; set; } = 0;
        public SearchProperty? AcquisitionSearchProperties { get; set; } = new SearchProperty();
        public int TakesStartRow { get; set; } = 0;
        public int TakesCount { get; set; } = 0;
        public List<Take>? AcquisitionTakes { get; set; } = new List<Take>();
        public int AcquisitionFileChecklistIndex { get; set; } = 0;
        public AcquisitionFileChecklist? AcquisitionFileChecklist { get; set; } = new AcquisitionFileChecklist();
        public int AgreementStartRow { get; set; } = 0;
        public int AgreementCount { get; set; } = 0;
        public List<AcquisitionAgreement>? AcquisitionAgreements { get; set; } = new List<AcquisitionAgreement>();
        public int StakeholderStartRow { get; set; } = 0;
        public int StakeholderCount { get; set; } = 0;
        public List<AcquisitionStakeholder>? AcquisitionStakeholders { get; set; } = new List<AcquisitionStakeholder>();
        public int CompensationStartRow { get; set; } = 0;
        public int CompensationCount { get; set; } = 0;
        public string? CompensationTotalAllowableAmount { get; set; } = String.Empty;
        public List<AcquisitionCompensation>? AcquisitionCompensations { get; set; } = new List<AcquisitionCompensation>();
        public int ExpropriationStartRow { get; set; } = 0;
        public int ExpropriationCount { get; set; } = 0;
        public List<AcquisitionExpropriationForm8>? AcquisitionExpropriationForm8s { get; set; } = new List<AcquisitionExpropriationForm8>();
    }

    public class TeamMember
    {
        public string TeamMemberRole { get; set; } = null!;
        public string TeamMemberContactName { get; set; } = null!;
        public string TeamMemberContactType { get; set; } = null!;
        public string? TeamMemberPrimaryContact { get; set; } = String.Empty;
    }

    public class AcquisitionOwner
    {
        public string? ContactType { get; set; } = String.Empty;
        public bool isPrimary { get; set; } = false;
        public string? GivenNames { get; set; } = String.Empty;
        public string? LastName { get; set; } = String.Empty;
        public string? OtherName { get; set; } = String.Empty;
        public string? CorporationName { get; set; } = String.Empty;
        public string? IncorporationNumber { get; set; } = String.Empty;
        public string? RegistrationNumber { get; set; } = String.Empty;
        public string? MailAddressLine1 { get; set; } = String.Empty;
        public string? MailAddressLine2 { get; set; } = String.Empty;
        public string? MailAddressLine3 { get; set; } = String.Empty;
        public string? MailCity { get; set; } = String.Empty;
        public string? MailProvince { get; set; } = String.Empty;
        public string? MailCountry { get; set; } = String.Empty;
        public string? MailOtherCountry { get; set; } = String.Empty;
        public string? MailPostalCode { get; set; } = String.Empty;
        public string? Email { get; set; } = String.Empty;
        public string? Phone { get; set; } = String.Empty;
    }

    public class AcquisitionFileChecklist
    {
        public string? FileInitiationSelect1 { get; set; } = String.Empty;
        public string? FileInitiationSelect2 { get; set; } = String.Empty;
        public string? FileInitiationSelect3 { get; set; } = String.Empty;
        public string? FileInitiationSelect4 { get; set; } = String.Empty;
        public string? FileInitiationSelect5 { get; set; } = String.Empty;

        public string? ActiveFileManagementSelect1 { get; set; } = String.Empty;
        public string? ActiveFileManagementSelect2 { get; set; } = String.Empty;
        public string? ActiveFileManagementSelect3 { get; set; } = String.Empty;
        public string? ActiveFileManagementSelect4 { get; set; } = String.Empty;
        public string? ActiveFileManagementSelect5 { get; set; } = String.Empty;
        public string? ActiveFileManagementSelect6 { get; set; } = String.Empty;
        public string? ActiveFileManagementSelect7 { get; set; } = String.Empty;
        public string? ActiveFileManagementSelect8 { get; set; } = String.Empty;
        public string? ActiveFileManagementSelect9 { get; set; } = String.Empty;
        public string? ActiveFileManagementSelect10 { get; set; } = String.Empty;
        public string? ActiveFileManagementSelect11 { get; set; } = String.Empty;
        public string? ActiveFileManagementSelect12 { get; set; } = String.Empty;
        public string? ActiveFileManagementSelect13 { get; set; } = String.Empty;
        public string? ActiveFileManagementSelect14 { get; set; } = String.Empty;
        public string? ActiveFileManagementSelect15 { get; set; } = String.Empty;
        public string? ActiveFileManagementSelect16 { get; set; } = String.Empty;
        public string? ActiveFileManagementSelect17 { get; set; } = String.Empty;

        public string? CrownLandSelect1 { get; set; } = String.Empty;
        public string? CrownLandSelect2 { get; set; } = String.Empty;
        public string? CrownLandSelect3 { get; set; } = String.Empty;

        public string? Section3AgreementSelect1 { get; set; } = String.Empty;
        public string? Section3AgreementSelect2 { get; set; } = String.Empty;
        public string? Section3AgreementSelect3 { get; set; } = String.Empty;
        public string? Section3AgreementSelect4 { get; set; } = String.Empty;
        public string? Section3AgreementSelect5 { get; set; } = String.Empty;
        public string? Section3AgreementSelect6 { get; set; } = String.Empty;
        public string? Section3AgreementSelect7 { get; set; } = String.Empty;
        public string? Section3AgreementSelect8 { get; set; } = String.Empty;
        public string? Section3AgreementSelect9 { get; set; } = String.Empty;

        public string? Section6ExpropriationSelect1 { get; set; } = String.Empty;
        public string? Section6ExpropriationSelect2 { get; set; } = String.Empty;
        public string? Section6ExpropriationSelect3 { get; set; } = String.Empty;
        public string? Section6ExpropriationSelect4 { get; set; } = String.Empty;
        public string? Section6ExpropriationSelect5 { get; set; } = String.Empty;
        public string? Section6ExpropriationSelect6 { get; set; } = String.Empty;
        public string? Section6ExpropriationSelect7 { get; set; } = String.Empty;
        public string? Section6ExpropriationSelect8 { get; set; } = String.Empty;
        public string? Section6ExpropriationSelect9 { get; set; } = String.Empty;
        public string? Section6ExpropriationSelect10 { get; set; } = String.Empty;
        public string? Section6ExpropriationSelect11 { get; set; } = String.Empty;
        public string? Section6ExpropriationSelect12 { get; set; } = String.Empty;

        public string? AcquisitionCompletionSelect1 { get; set; } = String.Empty;
    }

    public class AcquisitionAgreement
    {
        public string AgreementStatus { get; set; } = null!;
        public string? AgreementLegalSurveyPlan { get; set; } = String.Empty;
        public string AgreementType { get; set; } = null!;
        public string? AgreementDate { get; set; } = String.Empty;
        public string? AgreementCompletionDate { get; set; } = String.Empty;
        public string? AgreementCommencementDate { get; set; } = String.Empty;
        public string? AgreementTerminationDate { get; set; } = String.Empty;
        public string? AgreementPurchasePrice { get; set; } = String.Empty;
        public string? AgreementDepositDue { get; set; } = String.Empty;
        public string? AgreementDepositAmount { get; set; } = String.Empty;
    }

    public class AcquisitionStakeholder
    {
        public string StakeholderType { get; set; } = null!;
        public string? InterestHolder { get; set; } = String.Empty;
        public string? InterestType { get; set; } = String.Empty;
        public string? StakeholderContactType { get; set; } = String.Empty;
        public string? PrimaryContact { get; set; } = String.Empty;
        public string? PayeeName { get; set; } = String.Empty;
        public int StakeholderIndex { get; set; } = 0;
    }

    public class AcquisitionCompensation
    {
        public string? CompensationAmount { get; set; } = String.Empty;
        public string? CompensationGSTAmount { get; set; } = String.Empty;
        public string? CompensationTotalAmount { get; set; } = String.Empty;
        public string? CompensationStatus { get; set; } = String.Empty;
        public string? CompensationAlternateProject { get; set; } = String.Empty;
        public string? CompensationAgreementDate { get; set; } = String.Empty;
        public string? CompensationExpropriationNoticeDate { get; set; } = String.Empty;
        public string? CompensationExpropriationVestingDate { get; set; } = String.Empty;
        public string? CompensationAdvancedPaymentDate { get; set; } = String.Empty;
        public string? CompensationSpecialInstructions { get; set; } = String.Empty;
        public string? CompensationFiscalYear { get; set; } = String.Empty;
        public string? CompensationSTOB { get; set; } = String.Empty;
        public string? CompensationServiceLine { get; set; } = String.Empty;
        public string? CompensationResponsibilityCentre { get; set; } = String.Empty;
        public string? CompensationPayee { get; set; } = String.Empty;
        public string? CompensationPayeeDisplay { get; set; } = String.Empty;
        public Boolean CompensationPaymentInTrust { get; set; } = false;
        public string? CompensationGSTNumber { get; set; } = String.Empty;
        public string? CompensationDetailedRemarks { get; set; } = String.Empty;
        public int ActivitiesStartRow { get; set; } = 0;
        public int ActivitiesCount { get; set; } = 0;
        public List<CompensationActivity>? CompensationActivities { get; set; } = new List<CompensationActivity>();
    }

    public class CompensationActivity
    {
        public string? ActCodeDescription { get; set; } = String.Empty;
        public string? ActAmount { get; set; } = String.Empty;
        public string? ActGSTEligible { get; set; } = String.Empty;
        public string? ActGSTAmount { get; set; } = String.Empty;
        public string? ActTotalAmount { get; set; } = String.Empty;
    }

    public class Take
    {
        public string TakeType { get; set; } = null!;
        public string TakeStatus { get; set; } = null!;
        public string? SiteContamination { get; set; } = String.Empty;
        public string? TakeDescription { get; set; } = String.Empty;
        public string? IsNewHighwayDedication { get; set; } = String.Empty;
        public string? IsNewHighwayDedicationArea { get; set; } = String.Empty;
        public string? IsMotiInventory { get; set; } = String.Empty;
        public string? IsNewInterestLand { get; set; } = String.Empty;
        public string? IsNewInterestLandArea { get; set; } = String.Empty;
        public string? IsLandActTenure { get; set; } = String.Empty;
        public string? IsLandActTenureDetail { get; set; } = String.Empty;
        public string? IsLandActTenureArea { get; set; } = String.Empty;
        public string? IsLandActTenureDate { get; set; } = String.Empty;
        public string? IsLicenseConstruct { get; set; } = String.Empty;
        public string? IsLicenseConstructArea { get; set; } = String.Empty;
        public string IsLicenseConstructDate { get; set; } = String.Empty;
        public string IsSurplus { get; set; } = String.Empty;
        public string? IsSurplusArea { get; set; } = String.Empty;
        public int FromProperty { get; set; } = 0;
        public int TakeCounter { get; set; } = 0;
    }

    public class AcquisitionExpropriationForm8
    {
        public string Form8Payee { get; set; } = null!;
        public string Form8PayeeDisplay { get; set; } = null!;
        public string? Form8ExpropriationAuthority { get; set; } = String.Empty;
        public string? Form8Description { get; set; } = String.Empty;
        public int ExpPaymentStartRow { get; set; } = 0;
        public int ExpPaymentCount { get; set; } = 0;
        public List<ExpropriationPayment>? ExpropriationPayments { get; set; } = new List<ExpropriationPayment>();
    }

    public class ExpropriationPayment
    {
        public string ExpPaymentItem { get; set; } = null!;
        public string ExpPaymentAmount { get; set; } = null!;
        public string? ExpPaymentGSTApplicable { get; set; } = String.Empty;
        public string? ExpPaymentGSTAmount { get; set; } = String.Empty;
        public string? ExpPaymentTotalAmount { get; set; } = String.Empty;
    }
}
