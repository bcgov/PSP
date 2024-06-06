﻿namespace PIMS.Tests.Automation.Classes
{
    public class Lease
    {
        public string MinistryProjectCode { get; set; } = null!;
        public string MinistryProject { get; set; } = null!;    
        public string LeaseStatus { get; set; } = null!;
        public string LeaseTerminationReason { get; set; } = null!;
        public string LeaseCancellationReason { get; set; } = null!;
        public string AccountType { get; set; } = null!;
        public string LeaseStartDate { get; set; } = null!;
        public string LeaseExpiryDate { get; set; } = null!;
        public string MOTIContact { get; set; } = null!;
        public string MOTIRegion { get; set; } = null!;
        public string Program { get; set; } = null!;
        public string ProgramOther { get; set; } = null!;
        public string AdminType { get; set; } = null!;
        public string TypeOther { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string CategoryOther { get; set; } = null!;
        public string LeasePurpose { get; set; } = null!;
        public string PurposeOther { get; set; } = null!;
        public string Initiator { get; set; } = null!;
        public string Responsibility { get; set; } = null!;
        public string EffectiveDate { get; set; } = null!;
        public string IntendedUse { get; set; } = null!;
        public string FirstNation { get; set; } = null!;
        public string StrategicRealEstate { get; set; } = null!;
        public string RegionalPlanning { get; set; } = null!;
        public string RegionalPropertyService { get; set; } = null!;
        public string District { get; set; } = null!;
        public string Headquarter { get; set; } = null!;
        public string ConsultationOther { get; set; } = null!;
        public string ConsultationOtherDetails { get; set; } = null!;
        public string PhysicalLeaseExist { get; set; } = null!;
        public string DigitalLeaseExist { get; set; } = null!;
        public string DocumentLocation { get; set; } = null!;
        public string LeaseNotes { get; set; } = null!;
        public int SearchPropertiesIndex { get; set; } = 0;
        public SearchProperty SearchProperties { get; set; } = new SearchProperty();
        public int TenantsStartRow { get; set; } = 0;
        public int TenantsQuantity { get; set; } = 0;
        public int TenantsNumber { get; set; } = 0;
        public int RepresentativeNumber { get; set; } = 0;
        public int PropertyManagerNumber { get; set; } = 0;
        public int UnknownNumber { get; set; } = 0;
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

    public class Term
    {
        public string TermStartDate { get; set; } = null!;
        public string TermEndDate { get; set; } = null!;
        public string TermPaymentFrequency { get; set;} = null!;
        public string TermAgreedPayment { get; set; } = null!;
        public string TermPaymentsDue { get; set; } = null!;
        public bool IsGSTEligible { get; set; } = false;
        public string TermStatus { get; set; } = null!;
    }

    public class Payment
    {
        public string PaymentSentDate { get; set; } = null!;
        public string PaymentMethod { get; set; } = null!;
        public string PaymentTotalReceived { get; set; } = null!;
        public string PaymentExpectedPayment { get; set; } = null!;
        public string PaymentGST { get; set; } = null!;
        public string PaymentStatus { get; set; } = null!;
        public int ParentTerm { get; set; } = 0;
    }

}
