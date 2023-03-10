

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
        private readonly SearchProperties searchProperties;
        private readonly PropertyInformation propertyInformation;
        private readonly SharedSearchProperties sharedSearchProperties;

        //private readonly string userName = "TRANPSP1";
        private readonly string userName = "sutairak";

        private readonly string leaseStartDate = "02/22/2022";
        private readonly string leaseExpiryDate = "03/22/2024";
        private readonly string leaseExpiryPastDate = "12/24/2022";
        private readonly string motiContact = "Automation Test";
        private readonly string responsibilityDate = "02/23/2022";
        private readonly string locationDoc = "Automation Test Location of Documents";
        private readonly string description = "Automation Test Intention of use";
        private readonly string lisDocumentNbr = "1948-0000-1";
        private readonly string psDocumentNbr = "135-256-001";
        private readonly string notes = "Automation Test Notes";
        private readonly string editDescription = "Automation Test Intented use - Edited by automation";
        private readonly string editNotes = "Automation Test Notes - Edited by automation";

        private readonly string PID = "001-192-396";
        private readonly string PIN = "10114481";
        private readonly string address = "2525 Mill Bay Rd";
        private readonly string planNbr = "VAP7495";
        private readonly string legalDescription = "LOT 3 DISTRICT LOT 118 GROUP 1";

        private readonly string individualTenant = "John Smith";
        private readonly string organizationTenant1 = "District of Saanich";
        private readonly string organizationTenant2 = "Sakwi Creek Hydro LP";
        private readonly string organizationTenant3 = "Bishop of Victoria";

        private readonly string firstTermEndDate = "04/22/2022";
        private readonly string firstTermFrequency = "Weekly";
        private readonly string firstTermAgreedPayment = "2500";

        private readonly string firstTermPaymentSentDate = "02/23/2022";
        private readonly string firstTermPaymentStatus = "Paid";
        private readonly string firstTermPaymentReceived = "2625";
        private readonly string firstTerm2ndPaymentSentDate = "03/09/2022";
        private readonly string firstTerm2ndPaymentReceived = "300";
        private readonly string firstTerm2ndPaymentStatus = "Partial";
        private readonly string secondTermStartDate = "05/22/2022";
        private readonly string secondTermEndDate = "06/30/2022";
        private readonly string secondTermFrequency = "Monthly";
        private readonly string secondTermAgreedPayment = "3500";
        private readonly string secondTermPaymentSentDate = "04/23/2022";
        private readonly string secondTermPaymentReceived = "3000";


        private readonly string termPaymentDue = "Automation Test Due";
        private readonly string noGST = "false";
        private readonly string yesGST = "true";
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

        private readonly string insuranceAircraftInPlace = "yes";
        private readonly string insuranceAircraftLimit = "25000";
        private readonly string insuranceAircraftExpiryDate = "12/12/2025";
        private readonly string insuranceAircraftDescription = "Automation Test - Aircraft Insurance Description";
        private readonly string insuranceCGLInPlace = "yes";
        private readonly string insuranceCGLLimit = "205000";
        private readonly string insuranceCGLExpiryDate = "12/12/2026";
        private readonly string insuranceCGLDescription = "Automation Test - Commercial General Liability Insurance Description";
        private readonly string insuranceMarineInPlace = "no";
        private readonly string insuranceMarineLimit = "125000";
        private readonly string insuranceMarineExpiryDate = "12/12/2027";
        private readonly string insuranceMarineDescription = "Automation Test - Marine Insurance Description";
        private readonly string insuranceVehicleInPlace = "yes";
        private readonly string insuranceVehicleLimit = "11000";
        private readonly string insuranceVehicleExpiryDate = "12/12/2023";
        private readonly string insuranceVehicleDescription = "Automation Test - Vehicle Insurance Description";
        private readonly string insuranceOtherType = "Pets Insurance";
        private readonly string insuranceOtherInPlace = "no";
        private readonly string insuranceOtherLimit = "100000";
        private readonly string insuranceOtherExpiryDate = "12/12/2030";
        private readonly string insuranceOtherDescription = "Automation Test - Aircraft Insurance Description";

        private readonly string depositType = "Security deposit";
        private readonly string depositType2 = "Other deposit";
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

        private readonly string greenPropertyPID = "015-254-241";
        private readonly string bluePropertyPID = "001-505-360";
        private readonly string purplePropertyPID = "000-228-729";

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
            searchProperties = new SearchProperties(driver.Current);
            propertyInformation = new PropertyInformation(driver.Current);
            sharedSearchProperties = new SharedSearchProperties(driver.Current);
        }

        [StepDefinition(@"I create a new Lease with minimum fields")]
        public void MinimumLeaseLicense()
        {
            /* TEST COVERAGE: PSP-2637, PSP-2677, PSP-2755, PSP-2915, PSP-2918, PSP-2921, PSP-2922, PSP-3494, PSP-3498, PSP-3499
             * PSP-4558 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Create a new Lease/License
            leaseDetails.NavigateToCreateNewLicense();

            //LEASE DETAILS
            //Create a new Lease/ License Details with minimum fields
            leaseDetails.LicenseDetailsMinFields(leaseStartDate, leaseExpiryDate);

            //Save the new license details
            leaseDetails.SaveLicense();

            //TENANTS
            //Navigate to Tenants
            tenant.NavigateToTenantSection();

            //Edit Tenants Section
            tenant.EditTenant();

            //Adding an individual Tenant
            tenant.AddIndividualTenant(individualTenant, "Tenant");

            //Saving Tenant
            leaseDetails.SaveLicense();

            //IMPROVEMENTS
            //Navigate to Improvements
            improvements.NavigateToImprovementSection();

            //Edit Improvement Section
            improvements.EditImprovements();

            //Add Commercial Improvements
            improvements.AddCommercialImprovement(improvementCommercialAddress, improvementCommercialSize, improvementCommercialSize);

            //Save Improvements
            leaseDetails.SaveLicense();

            //INSURANCE
            //Navigate to Insurance Section
            insurance.NavigateToInsuranceSection();

            //Edit Insurance Section
            insurance.EditInsurance();

            //Add Vehicle Insurance
            insurance.AddVehicleInsurance(insuranceVehicleInPlace, insuranceVehicleLimit, insuranceVehicleExpiryDate, insuranceVehicleDescription);

            //Add Other Insurance
            insurance.AddOtherInsurance(insuranceOtherInPlace, insuranceOtherType, insuranceOtherLimit, insuranceOtherExpiryDate, insuranceOtherDescription);

            //Save Insurances
            leaseDetails.SaveLicense();

            //DEPOSITS
            //Navigate to Deposits
            deposits.NavigateToDepositSection();

            //Add Deposit
            deposits.AddDepositBttn();
            deposits.AddDeposit(depositType2, depositDescription, depositAmount, depositPaidDate, depositHolder);

            //Verify new Deposit UI/UX on Table
            deposits.VerifyCreatedDepositTable(depositType2, depositDescription, depositAmount, depositPaidDate, depositHolder);

            //Add Return
            deposits.AddReturnBttn();
            deposits.AddReturn(depositReturnTerminationDate, depositReturnClaims, depositRetunedAmount, depositReturnInterestPaid, depositReturnDate, depositReturnPayeeName);

            //PAYMENTS
            //Navigating to Payments section
            payments.NavigateToPaymentSection();

            //Inserting first term
            payments.AddTermBttn();
            payments.AddTerm(leaseStartDate, firstTermEndDate, firstTermFrequency, firstTermAgreedPayment, termPaymentDue, noGST, termExercised);

            //Add Payments
            payments.OpenLastPaymentTab();

            //Inserting Payment for first term
            payments.AddPaymentBttn();
            payments.AddPayment(firstTermPaymentSentDate, firstTermPaymentReceived, firstTermPaymentStatus);

            //Get new lease's code
            leaseCode = leaseDetails.GetLeaseCode();

        }

        [StepDefinition(@"I create a new Lease with all fields and Properties")]
        public void MaximumLeaseLicense()
        {
            /* TEST COVERAGE: 
             * Lease Details: PSP-2550, PSP-1966, PSP-5100, PSP-5334, PSP-5335, PSP-5336, PSP-5337, PSP-5338, PSP-5340, PSP-5340, PSP-4558, PSP-2644
             * Tenants: PSP-3495, PSP-3496, PSP-3498, PSP-3499
             * Improvements: PSP-2637, PSP-2640
             * Insurance: PSP-2677, PSP-2099
             * Deposits: PSP-2094, PSP-2921, PSP-2922
             * Payments: PSP-2755, PSP-2915, PSP-2918, PSP-2927, PSP-2815
             * Surplus:
             */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Create a new Lease/License
            leaseDetails.NavigateToCreateNewLicense();

            //LEASE DETAILS
            //Verify Create new Lease Form
            leaseDetails.VerifyLicenseDetailsCreateForm();

            //Create a new Lease/ License Details with maximum fields
            leaseDetails.LicenseDetailsMaxFields(leaseStartDate, leaseExpiryDate, motiContact, responsibilityDate, locationDoc, description, lisDocumentNbr, psDocumentNbr, notes);

            //Add Several Properties
            //Verify UI/UX from Search By Component
            sharedSearchProperties.NavigateToSearchTab();
            sharedSearchProperties.VerifySearchPropertiesFeature();

            //Search for a property by PID
            sharedSearchProperties.SelectPropertyByPID(PID);
            sharedSearchProperties.SelectFirstOption();

            //Search for a property by PIN
            sharedSearchProperties.SelectPropertyByPIN(PIN);
            sharedSearchProperties.SelectFirstOption();

            //Search for a property by Plan
            sharedSearchProperties.SelectPropertyByPlan(planNbr);
            sharedSearchProperties.SelectFirstOption();

            //Search for a property by Address
            sharedSearchProperties.SelectPropertyByAddress(address);
            sharedSearchProperties.SelectFirstOption();

            //Search for a property by Legal Description
            sharedSearchProperties.SelectPropertyByLegalDescription(legalDescription);
            sharedSearchProperties.SelectFirstOption();

            //Search for a duplicate property
            sharedSearchProperties.SelectPropertyByPID(PID);
            sharedSearchProperties.SelectFirstOption();

            //Save the new license details
            leaseDetails.SaveLicense();

            //TENANTS
            //Navigate to Tenants
            tenant.NavigateToTenantSection();

            //Edit Tenants Section
            tenant.EditTenant();

            //Verify Tenants Initial Form
            tenant.VerifyTenantsInitForm();

            //Adding an individual Tenant
            tenant.AddIndividualTenant(individualTenant, "Tenant");

            //Saving Tenants
            tenant.SaveTenant();

            //Edit Tenants Section
            tenant.EditTenant();

            //Adding an organization Tenant
            tenant.AddOrganizationTenant(organizationTenant1, "Property manager");

            //Saving Tenants
            tenant.SaveTenant();

            //Edit Tenants Section
            tenant.EditTenant();

            //Adding another Organization Tenant
            tenant.AddOrganizationTenant(organizationTenant2, "Representative");

            //Saving Tenants
            tenant.SaveTenant();

            //Edit Tenants Section
            tenant.EditTenant();

            //Adding another Organization Tenant
            tenant.AddOrganizationTenant(organizationTenant3, "Tenant");

            //Saving Tenants
            tenant.SaveTenant();

            //Assert quantity of tenants
            Assert.True(tenant.TotalTenants() == 2);
            Assert.True(tenant.TotalRepresentatives() == 1);
            Assert.True(tenant.TotalManagers() == 1);
            Assert.True(tenant.TotalUnknown() == 0);

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
            leaseDetails.SaveLicense();

            //Verify Improvements View
            improvements.VerifyImprovementView(improvementCommercialAddress, improvementCommercialSize, improvementCommercialSize, improvementOtherAddress, improvementOtherSize, improvementOtherDescription,
                improvementResidentialAddress, improvementResidentialSize, improvementResidentialDescription);

            //INSURANCE
            //Navigate to Improvements
            insurance.NavigateToInsuranceSection();

            //Edit Improvement Section
            insurance.EditInsurance();

            //Add Aircraft Insurance
            insurance.VerifyInsuranceInitForm();
            insurance.AddAircraftInsurance(insuranceAircraftInPlace, insuranceAircraftLimit, insuranceAircraftExpiryDate, insuranceAircraftDescription);

            //Add CGL Insurance
            insurance.AddCGLInsurance(insuranceCGLInPlace, insuranceCGLLimit, insuranceCGLExpiryDate, insuranceCGLDescription);

            //Add Marine Insurance
            insurance.AddMarineInsurance(insuranceMarineInPlace, insuranceMarineLimit, insuranceMarineExpiryDate, insuranceMarineDescription);

            //Add Vehicle Insurance
            insurance.AddVehicleInsurance(insuranceVehicleInPlace, insuranceVehicleLimit, insuranceVehicleExpiryDate, insuranceVehicleDescription);

            //Add Other Insurance
            insurance.AddOtherInsurance(insuranceOtherInPlace, insuranceOtherType, insuranceOtherLimit, insuranceOtherExpiryDate, insuranceOtherDescription);

            //Save Insurances
            leaseDetails.SaveLicense();

            //Verify Insurance View Form
            insurance.VerifyInsuranceViewForm(insuranceAircraftInPlace, insuranceAircraftLimit, insuranceAircraftExpiryDate, insuranceAircraftDescription, insuranceCGLInPlace, insuranceCGLLimit, insuranceCGLExpiryDate, insuranceCGLDescription,
                insuranceMarineInPlace, insuranceMarineLimit, insuranceMarineExpiryDate, insuranceMarineDescription, insuranceVehicleInPlace, insuranceVehicleLimit, insuranceVehicleExpiryDate, insuranceVehicleDescription,
                insuranceOtherInPlace, insuranceOtherLimit, insuranceOtherExpiryDate, insuranceOtherDescription);

            //DEPOSITS
            //Navigate to Deposits
            deposits.NavigateToDepositSection();

            //Verify Deposits initial Tab
            deposits.VerifyDepositInitForm();

            //Verify Create a new deposit form
            deposits.AddDepositBttn();
            deposits.VerifyCreateDepositForm();

            //Add a new deposit
            deposits.AddDeposit(depositType, depositDescription, depositAmount, depositPaidDate, depositHolder);

            //Verify new Deposit UI/UX on Table
            deposits.VerifyCreatedDepositTable(depositType, depositDescription, depositAmount, depositPaidDate, depositHolder);

            //Verify Create Return Form
            deposits.AddReturnBttn();
            deposits.VerifyCreateReturnForm(depositType, depositAmount);

            //Add Return
            deposits.AddReturn(depositReturnTerminationDate, depositReturnClaims, depositRetunedAmount, depositReturnInterestPaid, depositReturnDate, depositReturnPayeeName);

            //Verify new return UI/UX on Table
            deposits.VerifyCreatedReturnTable(depositType, depositReturnTerminationDate, depositAmount, depositReturnClaims, depositRetunedAmount, depositReturnInterestPaid, depositReturnDate, depositReturnPayeeName);

            //Add Deposit Notes
            deposits.AddNotes(depositNotes);
            leaseDetails.SaveLicense();

            //PAYMENTS
            //Navigating to Payments section
            payments.NavigateToPaymentSection();

            //Verify initial Payments screen
            payments.VerifyPaymentsInitForm();

            //Verify Create Term Form
            payments.AddTermBttn();
            payments.VerifyCreateTermForm();

            //Inserting first term
            payments.AddTerm(leaseStartDate, firstTermEndDate, firstTermFrequency, firstTermAgreedPayment, termPaymentDue, yesGST, termExercised);

            //Verify inserted Term
            payments.VerifyInsertedTermTable("Initial term", leaseStartDate, firstTermEndDate, firstTermFrequency, termPaymentDue, firstTermAgreedPayment, yesGST, termExercised);

            //Add Payments
            payments.OpenLastPaymentTab();

            //Verify Payment Form
            payments.AddPaymentBttn();
            payments.VerifyCreatePaymentForm();

            //Inserting Payment for first term
            payments.AddPayment(firstTermPaymentSentDate, firstTermPaymentReceived, firstTermPaymentStatus);

            //Verify Header for Payments Table
            payments.VerifyPaymentTableHeader();

            //Inserting second Payment
            payments.AddPaymentBttn();
            payments.AddPayment(firstTerm2ndPaymentSentDate, firstTerm2ndPaymentReceived, firstTerm2ndPaymentStatus);

            //Inserting second term
            payments.AddTerm(secondTermStartDate, secondTermEndDate, secondTermFrequency, secondTermAgreedPayment, termPaymentDue, noGST, termNotExercised);

            //Verify inserted Term
            payments.VerifyInsertedTermTable("Renewal 1", secondTermStartDate, secondTermEndDate, secondTermFrequency, secondTermAgreedPayment, termPaymentDue, noGST, termNotExercised);

            //SURPLUS
            //Navigate to Surplus Declaration Section
            //surplus.NavigateToSurplusSection();

            //Get new lease's code
            leaseCode = leaseDetails.GetLeaseCode();
        }

        [StepDefinition(@"I create a new Lease through a Property Pin")]
        public void CreateLeaseGreenPin()
        {
            /* TEST COVERAGE: PSP-4978, PSP-2648, PSP-2641 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Look for a Inventory Property
            searchProperties.SearchPropertyByPINPID(greenPropertyPID);

            //Choose the given result
            searchProperties.SelectFoundPin();

            //Close Main Information Window
            propertyInformation.ClosePropertyInfoModal();

            //Start a new lease from pop-up
            propertyInformation.OpenMoreOptionsPopUp();
            propertyInformation.ChooseCreationOptionFromPin("Lease/License - Create new");

            //Fill basic information on the form
            leaseDetails.LicenseDetailsMinFields(leaseStartDate, leaseExpiryPastDate);

            //Save Lease Details
            leaseDetails.SaveLicense();

            //Verify Header with Expired Flag
            leaseDetails.VerifyLicenseHeader();

            //Insert an update
            leaseDetails.EditLeaseFileDetails();
            leaseDetails.UpdateLeaseFileDetails(editDescription, editNotes);

            //Cancel changes
            leaseDetails.CancelLicense();

            //Get new lease's code
            leaseCode = leaseDetails.GetLeaseCode();
        }

        [StepDefinition(@"I create a new Lease through a Property of Interest")]
        public void CreateLeaseBluePin()
        {
            /* TEST COVERAGE: PSP-4979, PSP-2551 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Look for a Inventory Property
            searchProperties.SearchPropertyByPINPID(bluePropertyPID);

            //Choose the given result
            searchProperties.SelectFoundPin();

            //Close Main Information Window
            propertyInformation.ClosePropertyInfoModal();

            //Start a new lease from pop-up
            propertyInformation.OpenMoreOptionsPopUp();
            propertyInformation.ChooseCreationOptionFromPin("Lease/License - Create new");

            //Fill basic information on the form
            leaseDetails.LicenseDetailsMinFields(leaseStartDate, leaseExpiryPastDate);

            //Save Lease Details
            leaseDetails.CancelLicense();

            //Start a new lease from pop-up
            propertyInformation.ChooseCreationOptionFromPin("Lease/License - Create new");

            //Fill basic information on the form
            leaseDetails.LicenseDetailsMinFields(leaseStartDate, leaseExpiryPastDate);

            //Save changes
            leaseDetails.SaveLicense();

            //Get new lease's code
            leaseCode = leaseDetails.GetLeaseCode();
        }

        [StepDefinition(@"I create a new Lease through a Payable Marker")]
        public void CreateLeasePurplePin()
        {
            /* TEST COVERAGE: PSP-5158 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Look for a Inventory Property
            searchProperties.SearchPropertyByPINPID(purplePropertyPID);

            //Choose the given result
            searchProperties.SelectFoundPin();

            //Close Main Information Window
            propertyInformation.ClosePropertyInfoModal();

            //Start a new lease from pop-up
            propertyInformation.OpenMoreOptionsPopUp();
            propertyInformation.ChooseCreationOptionFromPin("Lease/License - Create new");

            //Fill basic information on the form
            leaseDetails.LicenseDetailsMinFields(leaseStartDate, leaseExpiryPastDate);

            //Save Lease Details
            leaseDetails.SaveLicense();

            //Get new lease's code
            leaseCode = leaseDetails.GetLeaseCode();
        }

        [StepDefinition(@"I update an existing lease")]
        public void EditExistingLease()
        {
            /* TEST COVERAGE: PSP-2096, PSP-2642, PSP-5161, PSP-5342, PSP-2637, PSP-2638, PSP-2923, PSP-4195, PSP-4196, PSP-4558 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Search Leases
            searchLease.NavigateToSearchLicense();

            //Look for the last created lease
            searchLease.SearchLastLease();

            //FILE DETAILS
            //Verify File Details Form
            leaseDetails.VerifyLicenseDetailsViewForm();

            //Edit File Details Section
            leaseDetails.EditLeaseFileDetails();

            //Verify the edit File details form
            leaseDetails.VerifyLicenseDetailsUpdateForm();

            //Make some changes on the Details Form
            leaseDetails.UpdateLeaseFileDetails(editDescription, editNotes);

            //Verify Properties section
            sharedSearchProperties.VerifyLocateOnMapFeature();

            //Delete 1st property
            sharedSearchProperties.DeleteProperty();

            //Save the new license details
            leaseDetails.SaveLicense();

            //PAYMENTS
            //Navigate to Payments
            //payments.NavigateToPaymentSection();

            ////Delete last term
            //payments.DeleteLastTerm();

            ////Navigate to first term payments
            //payments.OpenLastPaymentTab();

            ////Delete first term last payment
            //payments.DeleteLastPayment();


            ////INSURANCE
            ////Navigate to Improvements
            //insurance.NavigateToInsuranceSection();

            ////Edit Improvement Section
            //insurance.EditInsurance();

            ////Add Vehicle Insurance
            //insurance.AddVehicleInsurance(insuranceVechicleLimit, insuranceVehicleExpiryDate, insuranceVehicleDescription);

            ////Add Other Insurance
            //insurance.AddOtherInsurance(insuranceOtherType, insuranceOtherLimit, insuranceOtherExpiryDate, insuranceOtherDescription);

            ////Save Insurances
            //insurance.SaveInsurance();

            ////DEPOSITS
            ////Navigate to Deposits
            //deposits.NavigateToDepositSection();

            ////Delete last return
            //deposits.DeleteLastReturn();

            ////Delete last deposit
            //deposits.DeleteLastDeposit();
        }

        [StepDefinition(@"A new lease is created successfully")]
        public void NewLeaseCreated()
        {
            //TEST COVERAGE: PSP-2466, PSP-2993

            searchLease.NavigateToSearchLicense();
            searchLease.SearchLicenseByLFile(leaseCode);

            Assert.True(searchLease.SearchFoundResults());
        }

        [StepDefinition(@"An existing lease is updated successfully")]
        public void LeaseEditedSuccessfully()
        {
            //Assert.True(deposits.LeaseDespositExist());
        }
    }
}
