namespace PIMS.Tests.Automation.Classes
{
    public class Lease
    {
        public string? MinistryProjectCode { get; set; } = String.Empty;
        public string? MinistryProject { get; set; } = String.Empty;
        public string LeaseStatus { get; set; } = null!;
        public string AccountType { get; set; } = null!;
        public string LeaseStartDate { get; set; } = null!;
        public string? LeaseExpiryDate { get; set; } = String.Empty;
        public string? MOTIContact { get; set; } = String.Empty;
        public string MOTIRegion { get; set; } = null!;
        public string Program { get; set; } = null!;
        public string? ProgramOther { get; set; } = String.Empty;
        public string AdminType { get; set; } = null!;
        public string? TypeOther { get; set; } = String.Empty;
        public string? Category { get; set; } = String.Empty;
        public string? CategoryOther { get; set; } = String.Empty;
        public string Purpose { get; set; } = null!;
        public string? PurposeOther { get; set; } = String.Empty;
        public string? Initiator { get; set; } = String.Empty;
        public string? Responsibility { get; set; } = String.Empty;
        public string? EffectiveDate { get; set; } = String.Empty;
        public string? IntendedUse { get; set; } = String.Empty;
        public string? FirstNation { get; set; } = String.Empty;
        public string? StrategicRealEstate { get; set; } = String.Empty;
        public string? RegionalPlanning { get; set; } = String.Empty;
        public string? RegionalPropertyService { get; set; } = String.Empty;
        public string? District { get; set; } = String.Empty;
        public string? Headquarter { get; set; } = String.Empty;
        public string? ConsultationOther { get; set; } = String.Empty;
        public string? ConsultationOtherDetails { get; set; } = String.Empty;
        public string? PhysicalLeaseExist { get; set; } = String.Empty;
        public string? DigitalLeaseExist { get; set; } = String.Empty;
        public string? DocumentLocation { get; set; } = String.Empty;
        public string? LISNumber { get; set; } = String.Empty;
        public string? PSNumber { get; set; } = String.Empty;
        public string? LeaseNotes { get; set; } = String.Empty;
        public int SearchPropertiesIndex { get; set; } = 0;
        public SearchProperty? SearchProperties { get; set; } = new SearchProperty();
        public int TenantsStartRow { get; set; } = 0;
        public int TenantsQuantity { get; set; } = 0;
        public int TenantsNumber { get; set; } = 0;
        public int RepresentativeNumber { get; set; } = 0;
        public int PropertyManagerNumber { get; set; } = 0;
        public int UnknownNumber { get; set; } = 0;
        public List<Tenant>? LeaseTenants { get; set; } = new List<Tenant>();
        public string? CommercialImprovementUnit { get; set; } = String.Empty;
        public string? CommercialImprovementBuildingSize { get; set; } = String.Empty;
        public string? CommercialImprovementDescription { get; set; } = String.Empty;
        public string? ResidentialImprovementUnit { get; set; } = String.Empty;
        public string? ResidentialImprovementBuildingSize { get; set; } = String.Empty;
        public string? ResidentialImprovementDescription { get; set; } = String.Empty;
        public string? OtherImprovementUnit { get; set; } = String.Empty;
        public string? OtherImprovementBuildingSize { get; set; } = String.Empty;
        public string? OtherImprovementDescription { get; set; } = String.Empty;
        public int TotalImprovementCount { get; set; } = 0;

        public string? AircraftInsuranceInPlace { get; set; } = String.Empty;

        public string? AircraftLimit { get; set; } = String.Empty;
        public string? AircraftPolicyExpiryDate { get; set; } = String.Empty;
        public string? AircraftDescriptionCoverage { get; set; } = String.Empty;
        public string? CGLInsuranceInPlace { get; set; } = String.Empty;
        public string? CGLLimit { get; set; } = String.Empty;
        public string? CGLPolicyExpiryDate { get; set; } = String.Empty;
        public string? CGLDescriptionCoverage { get; set; } = String.Empty;
        public string? MarineInsuranceInPlace { get; set; } = String.Empty;
        public string? MarineLimit { get; set; } = String.Empty;
        public string? MarinePolicyExpiryDate { get; set; } = String.Empty;
        public string? MarineDescriptionCoverage { get; set; } = String.Empty;
        public string? VehicleInsuranceInPlace { get; set; } = String.Empty;
        public string? VehicleLimit { get; set; } = String.Empty;
        public string? VehiclePolicyExpiryDate { get; set; } = String.Empty;
        public string? VehicleDescriptionCoverage { get; set; } = String.Empty;
        public string? OtherInsuranceType { get; set; } = String.Empty;
        public string? OtherInsuranceInPlace { get; set; } = String.Empty;
        public string? OtherLimit { get; set; } = String.Empty;
        public string? OtherPolicyExpiryDate { get; set; } = String.Empty;
        public string? OtherDescriptionCoverage { get; set; } = String.Empty;
        public int TotalInsuranceCount { get; set; } = 0;
        public string? DepositNotes { get; set; } = String.Empty;
        public int DepositsStartRow { get; set; } = 0;
        public int DepositsCount { get; set; } = 0;
        public List<Deposit> LeaseDeposits { get; set; } = new List<Deposit>();
        public int TermsStartRow { get; set; } = 0;
        public int TermsCount { get; set; } = 0;
        public List<Term> LeaseTerms { get; set; } = new List<Term>();
        public int PaymentsStartRow { get; set; } = 0;
        public int PaymentsCount { get; set; } = 0;
        public List<Payment> TermPayments { get; set; } = new List<Payment>();

    }

    public class Tenant
    {
        public string ContactType { get; set; } = null!;
        public string Summary { get; set;} = null!;
        public string PrimaryContact { get; set; } = null!;
        public string TenantType { get; set; } = null!;
    }

    public class Deposit
    {
        public string? DepositType { get; set; } = String.Empty;
        public string? DepositTypeOther { get; set; } = String.Empty;
        public string? DepositDescription { get; set; } = String.Empty;
        public string? DepositAmount { get; set; } = String.Empty;
        public string? DepositPaidDate { get; set; } = String.Empty;
        public string? DepositHolder{ get; set; } = String.Empty;
        public string? ReturnTerminationDate { get; set; } = String.Empty;
        public string? TerminationClaimDeposit { get; set; } = String.Empty;
        public string? ReturnedAmount { get; set; } = String.Empty;
        public string? ReturnInterestPaid { get; set; } = String.Empty;
        public string? ReturnedDate { get; set; } = String.Empty;
        public string? ReturnPayeeName { get; set; } = String.Empty;
    }

    public class Term
    {
        public string TermStartDate { get; set; } = null!;
        public string? TermEndDate { get; set; } = String.Empty;
        public string? TermPaymentFrequency { get; set;} = String.Empty;
        public string? TermAgreedPayment { get; set; } = String.Empty;
        public string? TermPaymentsDue { get; set; } = String.Empty;
        public bool IsGSTEligible { get; set; } = false;
        public string? TermStatus { get; set; } = String.Empty;
    }

    public class Payment
    {
        public string PaymentSentDate { get; set; } = null!;
        public string? PaymentMethod { get; set; } = String.Empty;
        public string? PaymentTotalReceived { get; set; } = String.Empty;
        public string? PaymentExpectedPayment { get; set; } = String.Empty;
        public string? PaymentGST { get; set; } = String.Empty;
        public string? PaymentStatus { get; set; } = String.Empty;
        public int ParentTerm { get; set; } = 0;
    }

}
