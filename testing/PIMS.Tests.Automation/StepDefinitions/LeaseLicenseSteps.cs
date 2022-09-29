using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMS.Tests.Automation.StepDefinitions
{
    [Binding]
    public class LeaseLicenseSteps
    {
        private readonly LoginSteps loginSteps;
        private readonly LeaseDetails leaseDetails;
        private readonly LeaseTenants tenant;
        private readonly LeasePayments payments;
        private readonly LeaseImprovements improvements;
        private readonly LeaseInsurance insurance;
        private readonly LeaseDeposits deposits;
        private readonly LeaseSurplus surplus;
        private readonly SearchLease searchLease;

        private readonly string userName = "sutairak";

        private readonly string leaseStartDate = "02/22/2022";
        private readonly string leaseExpiryDate = "03/22/2024";
        private readonly string motiContact = "Automation Test";
        private readonly string responsibilityDate = "02/23/2022";
        private readonly string locationDoc = "Automation Test Location of Documents";
        private readonly string pid1 = "23915803";
        private readonly string description = "Automation Test Description";
        private readonly string notes = "Automation Test Notes";

        private readonly string individualTenant = "John Smith";
        private readonly string organizationTenant1 = "Astro Vancouver";
        private readonly string organizationTenant2 = "Yahoo Ventures";
        private readonly string organizationTenant3 = "Bishop of Victoria";

        private readonly string firstTermEndDate = "04/22/2022";
        private readonly string firstTermAgreedPayment = "2500";
        private readonly string firstTermPaymentSentDate = "02/23/2022";
        private readonly string firstTermPaymentStatus = "Paid";
        private readonly string firstTermPaymentReceived = "2625";
        private readonly string firstTerm2ndPaymentSentDate = "03/09/2022";
        private readonly string firstTerm2ndPaymentReceived = "300";
        private readonly string firstTerm2ndPaymentStatus = "Partial";
        private readonly string secondTermStartDate = "05/22/2022";
        private readonly string secondTermEndDate = "06/30/2022";
        private readonly string secondTermAgreedPayment = "3500";
        private readonly string termPaymentDue = "Automation Test Due";
        private readonly string noGST = "N";
        private readonly string yesGST = "Y";
        private readonly string termExercised = "Exercised";
        private readonly string termNotExercised = "Not Exercised";

        private readonly string improvementCommercialAddress = "1688 Blanshard St. Victoria, BC, V7C 1B7";
        private readonly string improvementCommercialSize = "256 sqft";
        private readonly string improvementCommercialDescription = "Automation Test - Commercial Improvement Description";
        private readonly string improvementResidentialAddress = "2301-1155 Nanaimo St. Vancouver, BC V6Z 2C7";
        private readonly string improvementResidentialSize = "175.69 sqft";
        private readonly string improvementResidentialDescription = "Automation Test - Residential Improvement Description";
        private readonly string improvementOtherAddress = "Stanley Park, Vancouver, BC, V6Z 8J9";
        private readonly string improvementOtherSize = "2256 sqft";
        private readonly string improvementOtherDescription = "Automation Test - Other Improvement Description";

        private readonly string insuranceAircraftLimit = "25000";
        private readonly string insuranceAircraftExpiryDate = "12/12/2025";
        private readonly string insuranceAircraftDescription = "Automation Test - Aircraft Insurance Description";
        private readonly string insuranceCGLLimit = "205000";
        private readonly string insuranceCGLExpiryDate = "12/12/2026";
        private readonly string insuranceCGLDescription = "Automation Test - Commercial General Liability Insurance Description";
        private readonly string insuranceMarineLimit = "125000";
        private readonly string insuranceMarineExpiryDate = "12/12/2027";
        private readonly string insuranceMarineDescription = "Automation Test - Marine Insurance Description";
        private readonly string insuranceVechicleLimit = "11000";
        private readonly string insuranceVehicleExpiryDate = "12/12/2023";
        private readonly string insuranceVehicleDescription = "Automation Test - Vehicle Insurance Description";
        private readonly string insuranceOtherType = "Pets Insurance";
        private readonly string insuranceOtherLimit = "100000";
        private readonly string insuranceOtherExpiryDate = "12/12/2030";
        private readonly string insuranceOtherDescription = "Automation Test - Aircraft Insurance Description";

        private readonly string depositDescription = "Automation Test - Deposit Description";
        private readonly string depositAmount = "250";
        private readonly string depositPaidDate = "02/25/2022";
        private readonly string depositHolder = "John Smith";
        private readonly string depositReturnTerminationDate = "03/25/2022";
        private readonly string depositReturnClaims = "75";
        private readonly string depositRetunedAmount = "50";
        private readonly string depositReturnInterestPaid = "25";
        private readonly string depositReturnDate = "03/30/2022";
        private readonly string depositReturnPayeeName = "John Smith";
        private readonly string depositNotes = "Automation Test - Deposit Notes";

        protected string leaseCode = "";

        public LeaseLicenseSteps(BrowserDriver driver)
        {
            loginSteps = new LoginSteps(driver);
            leaseDetails = new LeaseDetails(driver.Current);
            tenant = new LeaseTenants(driver.Current);
            payments = new LeasePayments(driver.Current);
            improvements = new LeaseImprovements(driver.Current);
            insurance = new LeaseInsurance(driver.Current);
            deposits = new LeaseDeposits(driver.Current);
            surplus = new LeaseSurplus(driver.Current);
            searchLease = new SearchLease(driver.Current);
        }

        [StepDefinition(@"I create a new Lease with minimum fields")]
        public void MinimumLeaseLicense()
        {
            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Create a new Lease/License
            leaseDetails.NavigateToCreateNewLicense();

            //LEASE DETAILS
            //Create a new Lease/ License Details with minimum fields
            leaseDetails.LicenseDetailsMinFields(leaseStartDate, pid1);

            //Save the new license details
            leaseDetails.SaveLicense();

            //TENANTS
            //Navigate to Tenants
            tenant.NavigateToTenantSection();

            //Edit Tenants Section
            tenant.EditTenant();

            //Adding an individual Tenant
            tenant.AddIndividualTenant(individualTenant);

            //Saving Tenant
            tenant.SaveTenant();

            //PAYMENTS
            //Navigating to Payments section
            payments.NavigateToPaymentSection();

            //Inserting first term
            payments.AddTerm(leaseStartDate, firstTermEndDate, firstTermAgreedPayment, termPaymentDue, yesGST, termExercised);

            //Add Payments
            payments.OpenLastPaymentTab();

            //Inserting Payment for first term
            payments.AddPayment(firstTermPaymentSentDate, firstTermPaymentReceived, firstTermPaymentStatus);

            //INSURANCE
            //Add Vehicle Insurance
            insurance.AddVehicleInsurance(insuranceVechicleLimit, insuranceVehicleExpiryDate, insuranceVehicleDescription);

            //Add Other Insurance
            insurance.AddOtherInsurance(insuranceOtherType, insuranceOtherLimit, insuranceOtherExpiryDate, insuranceOtherDescription);

            //Get new lease's code
            leaseCode = surplus.GetLeaseCode();

        }

        [StepDefinition(@"I create a new Lease with all fields")]
        public void MaximumLeaseLicense()
        {
            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Create a new Lease/License
            leaseDetails.NavigateToCreateNewLicense();

            //LEASE DETAILS
            //Create a new Lease/ License Details with maximum fields
            leaseDetails.LicenseDetailsMaxFields(leaseStartDate, leaseExpiryDate, motiContact, responsibilityDate, locationDoc, pid1, description, notes);

            //Save the new license details
            leaseDetails.SaveLicense();

            //TENANTS
            //Navigate to Tenants
            tenant.NavigateToTenantSection();

            //Edit Tenants Section
            tenant.EditTenant();

            //Adding an individual Tenant
            tenant.AddIndividualTenant(individualTenant);

            //Saving Tenant 1
            tenant.SaveTenant();

            //Edit Tenants Section
            tenant.EditTenant();

            //Adding an organization Tenant
            tenant.AddOrganizationTenant(organizationTenant1);

            //Saving Tenant 2
            tenant.SaveTenant();

            //Edit Tenants Section
            tenant.EditTenant();

            //Adding another Organization Tenant
            tenant.AddOrganizationTenant(organizationTenant2);

            //Saving Tenant 3
            tenant.SaveTenant();

            //Edit Tenants Section
            tenant.EditTenant();

            //Adding another Organization Tenant
            tenant.AddOrganizationTenant(organizationTenant3);

            //Saving Tenant 4
            tenant.SaveTenant();

            //Assert quantity of tenants
            Assert.True(tenant.TotalTenants());

            //PAYMENTS
            //Navigating to Payments section
            payments.NavigateToPaymentSection();

            //Inserting first term
            payments.AddTerm(leaseStartDate, firstTermEndDate, firstTermAgreedPayment, termPaymentDue, yesGST, termExercised);

            //Add Payments
            payments.OpenLastPaymentTab();

            //Inserting Payment for first term
            payments.AddPayment(firstTermPaymentSentDate, firstTermPaymentReceived, firstTermPaymentStatus);

            //Inserting second Payment
            payments.AddPayment(firstTerm2ndPaymentSentDate, firstTerm2ndPaymentReceived, firstTerm2ndPaymentStatus);

            //Inserting second term
            payments.AddTerm(secondTermStartDate, secondTermEndDate, secondTermAgreedPayment, termPaymentDue, noGST, termNotExercised);

            //IMPROVEMENTS
            //Navigate to Improvements
            improvements.NavigateToImprovementSection();

            //Edit Improvement Section
            improvements.EditImprovements();

            //Add Commercial Improvements
            improvements.AddCommercialImprovement(improvementCommercialAddress, improvementCommercialSize, improvementCommercialSize);

            //Add Commercial Improvements
            improvements.AddResidentialImprovement(improvementResidentialAddress, improvementResidentialSize, improvementResidentialDescription);

            //Add Commercial Improvements
            improvements.AddOtherImprovement(improvementOtherAddress, improvementOtherSize, improvementOtherDescription);

            //Save Improvements
            improvements.SaveImproments();

            //INSURANCE
            //Navigate to Improvements
            insurance.NavigateToInsuranceSection();

            //Edit Improvement Section
            insurance.EditInsurance();

            //Add Aircraft Insurance
            insurance.AddAircraftInsurance(insuranceAircraftLimit, insuranceAircraftExpiryDate, insuranceAircraftDescription);

            //Add CGL Insurance
            insurance.AddCGLInsurance(insuranceCGLLimit, insuranceCGLExpiryDate, insuranceCGLDescription);

            //Add Marine Insurance
            insurance.AddMarineInsurance(insuranceMarineLimit, insuranceMarineExpiryDate, insuranceMarineDescription);

            //Save Insurances
            insurance.SaveInsurance();

            //DEPOSITS
            //Navigate to Deposits
            deposits.NavigateToDepositSection();

            //Add Deposit
            deposits.AddDeposit(depositDescription, depositAmount, depositPaidDate, depositHolder);

            //Add Return
            deposits.AddReturn(depositReturnTerminationDate, depositReturnClaims, depositRetunedAmount, depositReturnInterestPaid, depositReturnDate, depositReturnPayeeName);

            //Add Deposit Notes
            deposits.AddNotes(depositNotes);

            //SURPLUS
            //Navigate to Surplus Declaration Section
            surplus.NavigateToSurplusSection();

            //Get new lease's code
            leaseCode = surplus.GetLeaseCode();

        }

        [StepDefinition(@"A new lease is created successfully")]
        public void NewLeaseCreated()
        {
            searchLease.NavigateToSearchLicense();
            searchLease.SearchLicenseByLFile(leaseCode);

            Assert.True(searchLease.SearchFoundResults());
        }
    }
}
