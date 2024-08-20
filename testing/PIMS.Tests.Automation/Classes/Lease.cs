namespace PIMS.Tests.Automation.Classes
{
    public class Lease
    {
        public string MinistryProjectCode { get; set; } = null!;
        public string MinistryProject { get; set; } = null!;    
        public string LeaseStatus { get; set; } = null!;
        public string LeaseTerminationDate { get; set; } = null!;
        public string LeaseTerminationReason { get; set; } = null!;
        public string LeaseCancellationReason { get; set; } = null!;
        public string AccountType { get; set; } = null!;
        public string LeaseStartDate { get; set; } = null!;
        public string LeaseExpiryDate { get; set; } = null!;

        public int LeaseRenewalStartRow { get; set; } = 0;
        public int LeaseRenewalQuantity { get; set; } = 0;
        public List<LeaseRenewal> LeaseRenewals { get; set; } = new List<LeaseRenewal>() { };

        public string MOTIContact { get; set; } = null!;
        public string MOTIRegion { get; set; } = null!;
        public string Program { get; set; } = null!;
        public string ProgramOther { get; set; } = null!;
        public string AdminType { get; set; } = null!;
        public string TypeOther { get; set; } = null!;
        public List<string> LeasePurpose { get; set; } = new List<string>() { };
        public string PurposeOther { get; set; } = null!;
        public string Initiator { get; set; } = null!;
        public string Responsibility { get; set; } = null!;
        public string EffectiveDate { get; set; } = null!;
        public string IntendedUse { get; set; } = null!;
        public string ArbitrationCity { get; set; } = null!;

        public string FirstNation { get; set; } = null!;
        public string StrategicRealEstate { get; set; } = null!;
        public string RegionalPlanning { get; set; } = null!;
        public string RegionalPropertyService { get; set; } = null!;
        public string District { get; set; } = null!;
        public string Headquarter { get; set; } = null!;
        public string ConsultationOther { get; set; } = null!;
        public string ConsultationOtherDetails { get; set; } = null!;

        public string FeeDeterminationPublicBenefit { get; set; } = null!;
        public string FeeDeterminationFinancialGain { get; set; } = null!;
        public string FeeDeterminationSuggestedFee { get; set; } = null!;
        public string FeeDeterminationNotes { get; set; } = null!;

        public string PhysicalLeaseExist { get; set; } = null!;
        public string DigitalLeaseExist { get; set; } = null!;
        public string DocumentLocation { get; set; } = null!;
        public string LeaseNotes { get; set; } = null!;

        public int SearchPropertiesIndex { get; set; } = 0;
        public SearchProperty SearchProperties { get; set; } = new SearchProperty() { };
        public int LeasePropertyDetailsStartRow { get; set; } = 0;
        public int LeasePropertyDetailsQuantity { get; set; } = 0;
        public List<LeaseProperty> LeasePropertiesDetails { get; set; } = new List<LeaseProperty>() { };

        public int LeaseChecklistIndex { get; set; } = 0;
        public LeaseChecklist LeaseChecklist { get; set; } = new LeaseChecklist() { };

        public int TenantsStartRow { get; set; } = 0;
        public int TenantsQuantity { get; set; } = 0;
        public int TenantsNumber { get; set; } = 0;
        public int RepresentativeNumber { get; set; } = 0;
        public int PropertyManagerNumber { get; set; } = 0;
        public int UnknownTenantNumber { get; set; } = 0;
        public List<Tenant> LeaseTenants { get; set; } = new List<Tenant>();

        public string CommercialImprovementUnit { get; set; } = null!;
        public string CommercialImprovementBuildingSize { get; set; } = null!;
        public string CommercialImprovementDescription { get; set; } = null!;
        public string ResidentialImprovementUnit { get; set; } = null!;
        public string ResidentialImprovementBuildingSize { get; set; } = null!;
        public string ResidentialImprovementDescription { get; set; } = null!;
        public string OtherImprovementUnit { get; set; } = null!;
        public string OtherImprovementBuildingSize { get; set; } = null!;
        public string OtherImprovementDescription { get; set; } = null!;
        public int TotalImprovementCount { get; set; } = 0;

        public string AccidentalInsuranceInPlace { get; set; } = null!;
        public string AccidentalLimit { get; set; } = null!;
        public string AccidentalPolicyExpiryDate { get; set; } = null!;
        public string AccidentalDescriptionCoverage { get; set; } = null!;
        public string AircraftInsuranceInPlace { get; set; } = null!;
        public string AircraftLimit { get; set; } = null!;
        public string AircraftPolicyExpiryDate { get; set; } = null!;
        public string AircraftDescriptionCoverage { get; set; } = null!;
        public string CGLInsuranceInPlace { get; set; } = null!;
        public string CGLLimit { get; set; } = null!;
        public string CGLPolicyExpiryDate { get; set; } = null!;
        public string CGLDescriptionCoverage { get; set; } = null!;
        public string MarineInsuranceInPlace { get; set; } = null!;
        public string MarineLimit { get; set; } = null!;
        public string MarinePolicyExpiryDate { get; set; } = null!;
        public string MarineDescriptionCoverage { get; set; } = null!;
        public string UnmannedAirVehicleInsuranceInPlace { get; set; } = null!;
        public string UnmannedAirVehicleLimit { get; set; } = null!;
        public string UnmannedAirVehiclePolicyExpiryDate { get; set; } = null!;
        public string UnmannedAirVehicleDescriptionCoverage { get; set; } = null!;
        public string VehicleInsuranceInPlace { get; set; } = null!;
        public string VehicleLimit { get; set; } = null!;
        public string VehiclePolicyExpiryDate { get; set; } = null!;
        public string VehicleDescriptionCoverage { get; set; } = null!;
        public string OtherInsuranceType { get; set; } = null!;
        public string OtherInsuranceInPlace { get; set; } = null!;
        public string OtherLimit { get; set; } = null!;
        public string OtherPolicyExpiryDate { get; set; } = null!;
        public string OtherDescriptionCoverage { get; set; } = null!;
        public int TotalInsuranceCount { get; set; } = 0;

        public string DepositNotes { get; set; } = null!;
        public int DepositsStartRow { get; set; } = 0;
        public int DepositsCount { get; set; } = 0;
        public List<Deposit> LeaseDeposits { get; set; } = new List<Deposit>();

        public int PeriodsStartRow { get; set; } = 0;
        public int PeriodsCount { get; set; } = 0;
        public List<Period> LeaseTerms { get; set; } = new List<Period>();

        public int PeriodPaymentsStartRow { get; set; } = 0;
        public int PeriodPaymentsCount { get; set; } = 0;
        public List<Payment> PeriodPayments { get; set; } = new List<Payment>();
    }

    public class LeaseRenewal
    {
        public string RenewalIsExercised { get; set; } = null!;
        public string RenewalCommencementDate { get; set; } = null!;
        public string RenewalExpiryDate { get; set; } = null!;
        public string RenewalNotes { get; set; } = null!;
    }

    public class LeaseChecklist
    {
        public string FileInitiationSelect1 { get; set; } = null!;
        public string FileInitiationSelect2 { get; set; } = null!;
        public string FileInitiationSelect3 { get; set; } = null!;
        public string FileInitiationSelect4 { get; set; } = null!;
        public string FileInitiationSelect5 { get; set; } = null!;
        public string FileInitiationSelect6 { get; set; } = null!;

        public string ReferralsApprovalsSelect1 { get; set; } = null!;
        public string ReferralsApprovalsSelect2 { get; set; } = null!;
        public string ReferralsApprovalsSelect3 { get; set; } = null!;
        public string ReferralsApprovalsSelect4 { get; set; } = null!;
        public string ReferralsApprovalsSelect5 { get; set; } = null!;
        public string ReferralsApprovalsSelect6 { get; set; } = null!;
        public string ReferralsApprovalsSelect7 { get; set; } = null!;
        public string ReferralsApprovalsSelect8 { get; set; } = null!;

        public string AgreementPreparationSelect1 { get; set; } = null!;
        public string AgreementPreparationSelect2 { get; set; } = null!;
        public string AgreementPreparationSelect3 { get; set; } = null!;
        public string AgreementPreparationSelect4 { get; set; } = null!;
        public string AgreementPreparationSelect5 { get; set; } = null!;
        public string AgreementPreparationSelect6 { get; set; } = null!;
        public string AgreementPreparationSelect7 { get; set; } = null!;
        public string AgreementPreparationSelect8 { get; set; } = null!;
        public string AgreementPreparationSelect9 { get; set; } = null!;
        public string AgreementPreparationSelect10 { get; set; } = null!;
        public string AgreementPreparationSelect11 { get; set; } = null!;
        public string AgreementPreparationSelect12 { get; set; } = null!;
        public string AgreementPreparationSelect13 { get; set; } = null!;

        public string LeaseLicenceCompletionSelect1 { get; set; } = null!;
        public string LeaseLicenceCompletionSelect2 { get; set; } = null!;        
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
        public string DepositType { get; set; } = null!;
        public string DepositTypeOther { get; set; } = null!;
        public string DepositDescription { get; set; } = null!;
        public string DepositAmount { get; set; } = null!;
        public string DepositPaidDate { get; set; } = null!;
        public string DepositHolder{ get; set; } = null!;
        public string ReturnTerminationDate { get; set; } = null!;
        public string TerminationClaimDeposit { get; set; } = null!;
        public string ReturnedAmount { get; set; } = null!;
        public string ReturnInterestPaid { get; set; } = null!;
        public string ReturnedDate { get; set; } = null!;
        public string ReturnPayeeName { get; set; } = null!;
    }

    public class Period
    {
        public string PeriodPaymentType { get; set; } = null!;
        public string PeriodDuration { get; set; } = null!;
        public string PeriodStartDate { get; set; } = null!;
        public string PeriodEndDate { get; set; } = null!;
        public string PeriodPaymentsDue { get; set; } = null!;
        public string PeriodStatus { get; set; } = null!;
        public string PeriodBasePaymentFrequency { get; set;} = null!;
        public string PeriodBaseAgreedPayment { get; set; } = null!;
        public string PeriodBaseIsGSTEligible { get; set; } = null!;
        public string PeriodBaseGSTAmount { get; set; } = null!;
        public string PeriodBaseTotalPaymentAmount { get; set; } = null!;
        public string PeriodAdditionalPaymentFrequency { get; set; } = null!;
        public string PeriodAdditionalAgreedPayment { get; set; } = null!;
        public string PeriodAdditionalIsGSTEligible { get; set; } = null!;
        public string PeriodAdditionalGSTAmount { get; set; } = null!;
        public string PeriodAdditionalTotalPaymentAmount { get; set; } = null!;
        public string PeriodVariablePaymentFrequency { get; set; } = null!;
        public string PeriodVariableAgreedPayment { get; set; } = null!;
        public string PeriodVariableIsGSTEligible { get; set; } = null!;
        public string PeriodVariableGSTAmount { get; set; } = null!;
        public string PeriodVariableTotalPaymentAmount { get; set; } = null!;
    }

    public class Payment
    {
        public string PaymentSentDate { get; set; } = null!;
        public string PaymentMethod { get; set; } = null!;
        public string PaymentCategory { get; set; } = null!;
        public string PaymentTotalReceived { get; set; } = null!;
        public string PaymentExpectedPayment { get; set; } = null!;
        public string PaymentIsGSTApplicable { get; set; } = null!; 
        public string PaymentGST { get; set; } = null!;
        public string PaymentStatus { get; set; } = null!;
        public int PeriodParentIndex { get; set; } = 0;
        public string ParentPeriodPaymentType { get; set; } = null!;

    }

}
