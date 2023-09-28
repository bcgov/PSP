

using PIMS.Tests.Automation.Classes;
using PIMS.Tests.Automation.Data;
using PIMS.Tests.Automation.PageObjects;
using System.Data;

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

        private readonly string userName = "TRANPSP1";
        //private readonly string userName = "sutairak";

        private Lease lease;
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
            lease = new Lease();
        }

        [StepDefinition(@"I create a new minimum Lease from row number (.*)")]
        public void CreateMinimumLeaseLicense(int rowNumber)
        {
            /* TEST COVERAGE: PSP-1966, PSP-2550, PSP-4558, PSP-5100 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Create a new Lease/License
            PopulateLeaseLicense(rowNumber);
            leaseDetails.NavigateToCreateNewLicense();

            //Create minimum Lease file
            leaseDetails.VerifyLicenseDetailsCreateForm();
            leaseDetails.CreateMinimumLicenseDetails(lease);

            //Save minimum lease file
            leaseDetails.SaveLicense();

            //Get new lease's code
            leaseCode = leaseDetails.GetLeaseCode();
        }

        [StepDefinition(@"I add additional Information to the Lease Details")]
        public void AddAdditionalInfoLicenseDetails()
        {
            /* TEST COVERAGE:  PSP-1966, PSP-2550, PSP-2644, PSP-4558, PSP-5334, PSP-5335, PSP-5336, PSP-5337, PSP-5338, PSP-5340, PSP-5654, PSP-5668, PSP-5923, PSP-6266 */

            //Add Additional information to the lease
            leaseDetails.EditLeaseFileDetailsBttn();
            leaseDetails.AddAdditionalLicenseDetailsInformation(lease);

            //Add Several Properties
            //Verify UI/UX from Search By Component
            sharedSearchProperties.NavigateToSearchTab();
            sharedSearchProperties.VerifySearchPropertiesFeature();

            //Search for a property by PID
            if (lease.SearchProperties.PID != "")
            {
                sharedSearchProperties.SelectPropertyByPID(lease.SearchProperties.PID);
                sharedSearchProperties.SelectFirstOption();
            }

            //Search for a property by PIN
            if (lease.SearchProperties.PIN != "")
            {
                sharedSearchProperties.SelectPropertyByPIN(lease.SearchProperties.PIN);
                sharedSearchProperties.SelectFirstOption();
            }

            //Search for a property by Plan
            if (lease.SearchProperties.PlanNumber != "")
            {
                sharedSearchProperties.SelectPropertyByPlan(lease.SearchProperties.PlanNumber);
                sharedSearchProperties.SelectFirstOption();
            }

            //Search for a property by Address
            if (lease.SearchProperties.Address != "")
            {
                sharedSearchProperties.SelectPropertyByAddress(lease.SearchProperties.Address);
                sharedSearchProperties.SelectFirstOption();
            }

            //Search for a property by Legal Description
            if (lease.SearchProperties.LegalDescription != "")
            {
                sharedSearchProperties.SelectPropertyByLegalDescription(lease.SearchProperties.LegalDescription);
                sharedSearchProperties.SelectFirstOption();
            }

            //Search for a duplicate property
            if (lease.SearchProperties.PID != "")
            {
                sharedSearchProperties.SelectPropertyByPID(lease.SearchProperties.PID);
                sharedSearchProperties.SelectFirstOption();
            }

            //Save the new license details
            leaseDetails.SaveLicense();
            leaseDetails.VerifyLicenseDetailsViewForm(lease);
        }

        [StepDefinition(@"I update a Lease's Details from row number (.*)")]
        public void UpdateLeaseDetails(int rowNumber)
        {
            /* TEST COVERAGE: PSP-2096, PSP-2642, PSP-4558, PSP-5161, PSP-5245, PSP-5342, PSP-5667, PSP-5924 */

            //Navigate to Search Leases
            PopulateLeaseLicense(rowNumber);
            searchLease.NavigateToSearchLicense();

            //Look for the previously created lease
            searchLease.SearchLicenseByLFile(leaseCode);
            searchLease.SelectFirstOption();

            //Edit File Details Section
            leaseDetails.EditLeaseFileDetailsBttn();

            //Verify the edit File details form
            leaseDetails.VerifyLicenseDetailsUpdateForm();

            //Make some changes on the Details Form
            leaseDetails.UpdateLeaseFileDetails(lease);

            //Verify Properties section
            sharedSearchProperties.VerifyLocateOnMapFeature();

            //Delete 1st property
            sharedSearchProperties.DeleteProperty();

            //Save the new license details
            leaseDetails.SaveLicense();

            //Verify File Details Form
            leaseDetails.VerifyLicenseDetailsViewForm(lease);
        }

        [StepDefinition(@"I add Tenants to the Lease")]
        public void CreateTenants()
        {
            /* TEST COVERAGE: PSP-3492, PSP-3494, PSP-3495, PSP-3496, PSP-3498, PSP-3499 */

            //TENANTS
            //Navigate to Tenants
            tenant.NavigateToTenantSection();

            //Edit Tenants Section
            tenant.EditTenant();

            //Verify Tenants Initial Form
            tenant.VerifyTenantsInitForm();

            //Go back to initial view form
            leaseDetails.CancelLicense();

            //Adding an individual Tenant
            if (lease.LeaseTenants.Count > 0)
            {
                for (var i = 0; i < lease.LeaseTenants.Count; i++)
                {
                    //Edit Tenants Section
                    tenant.EditTenant();

                    if (lease.LeaseTenants[i].ContactType == "Individual")
                        tenant.AddIndividualTenant(lease.LeaseTenants[i]);
                    else
                        tenant.AddOrganizationTenant(lease.LeaseTenants[i]);

                    //Saving Tenants
                    tenant.SaveTenant();
                }

            }

            //Assert quantity of tenants
            Assert.True(tenant.TotalTenants().Equals(lease.TenantsNumber));
            Assert.True(tenant.TotalRepresentatives().Equals(lease.RepresentativeNumber));
            Assert.True(tenant.TotalManagers().Equals(lease.PropertyManagerNumber));
            Assert.True(tenant.TotalUnknown().Equals(lease.UnknownNumber));
        }

        [StepDefinition(@"I update a Lease's Tenants from row number (.*)")]
        public void UpdateTenants(int rowNumber)
        {
            /* TEST COVERAGE:  PSP-4558 */

            //Navigate to Search Leases
            PopulateLeaseLicense(rowNumber);
            searchLease.NavigateToSearchLicense();

            //Look for the previously created lease
            searchLease.SearchLicenseByLFile(leaseCode);
            searchLease.SelectFirstOption();

            //Navigate to Tenants section
            tenant.NavigateToTenantSection();

            //Edit Tenants
            if (lease.TenantsQuantity > 0)
            {
                //Delete last Tenant
                tenant.EditTenant();
                tenant.DeleteLastTenant();

                //Save tenants changes
                tenant.SaveTenant();

                for (int i = 0; i < lease.LeaseTenants.Count; i++)
                {
                    //Edit last Tenant
                    tenant.EditTenant();
                    tenant.EditTenant(lease.LeaseTenants[i]);

                    //Save tenants changes
                    tenant.SaveTenant();
                }

                //Assert quantity of tenants
                Assert.True(tenant.TotalTenants().Equals(lease.TenantsNumber));
                Assert.True(tenant.TotalRepresentatives().Equals(lease.RepresentativeNumber));
                Assert.True(tenant.TotalManagers().Equals(lease.PropertyManagerNumber));
                Assert.True(tenant.TotalUnknown().Equals(lease.UnknownNumber));
            }
        }

        [StepDefinition(@"I add Improvements to the Lease")]
        public void CreateImprovements()
        {
            /* TEST COVERAGE: PSP-2637, PSP-2640 */

            //Navigate to Improvements
            improvements.NavigateToImprovementSection();

            //Edit Improvement Section
            improvements.EditImprovements();

            //Add Commercial Improvements
            if (lease.CommercialImprovementUnit != "")
                improvements.AddCommercialImprovement(lease);

            //Add Commercial Improvements
            if (lease.ResidentialImprovementUnit != "")
                improvements.AddResidentialImprovement(lease);

            //Add Commercial Improvements
            if (lease.OtherImprovementUnit != "")
                improvements.AddOtherImprovement(lease);

            //Save Improvements
            leaseDetails.SaveLicense();

            //Verify Improvements View
            improvements.VerifyImprovementView(lease);
        }

        [StepDefinition(@"I update a Lease's Improvements from row number (.*)")]
        public void UpdateImprovements(int rowNumber)
        {
            /* TEST COVERAGE:  PSP-2638, PSP-2640 */

            //Navigate to Search Leases
            PopulateLeaseLicense(rowNumber);
            searchLease.NavigateToSearchLicense();

            //Look for the previously created lease
            searchLease.SearchLicenseByLFile(leaseCode);
            searchLease.SelectFirstOption();

           
            //Navigate to the improvements section
            improvements.NavigateToImprovementSection();

            //Edit Improvements
            improvements.EditImprovements();
            if (lease.CommercialImprovementUnit != "")
                improvements.AddCommercialImprovement(lease);

            if (lease.ResidentialImprovementUnit != "")
                improvements.AddResidentialImprovement(lease);

            if (lease.OtherImprovementUnit != "")
                improvements.AddOtherImprovement(lease);

            //Save Improvements
            leaseDetails.SaveLicense();

            //Verify Improvement Changes
            improvements.VerifyImprovementView(lease);

            //Verify improvements count
            Assert.True(improvements.ImprovementTotal().Equals(lease.TotalImprovementCount));
        }

        [StepDefinition(@"I add Insurance to the Lease")]
        public void CreateInsurance()
        {
            /* TEST COVERAGE:  PSP-2099, PSP-2677 */

            //Navigate to Improvements
            insurance.NavigateToInsuranceSection();

            //Edit Improvement Section
            insurance.EditInsuranceButton();

            //Add Aircraft Insurance
            insurance.VerifyInsuranceInitForm();

            if (lease.AircraftDescriptionCoverage != "")
                insurance.AddAircraftInsurance(lease);

            //Add CGL Insurance
            if (lease.CGLDescriptionCoverage != "")
                insurance.AddCGLInsurance(lease);

            //Add Marine Insurance
            if (lease.MarineDescriptionCoverage != "")
                insurance.AddMarineInsurance(lease);

            //Add Vehicle Insurance
            if (lease.VehicleDescriptionCoverage != "")
                insurance.AddVehicleInsurance(lease);

            //Add Other Insurance
            if (lease.OtherDescriptionCoverage != "")
                insurance.AddOtherInsurance(lease);

            //Save Insurances
            leaseDetails.SaveLicense();

            //Verify Insurance View Form
            insurance.VerifyInsuranceViewForm(lease);
        }

        [StepDefinition(@"I update a Lease's Insurance from row number (.*)")]
        public void UpdateInsurance(int rowNumber)
        {
            /* TEST COVERAGE: PSP-2099, PSP-4195 */

            //Navigate to Search Leases
            PopulateLeaseLicense(rowNumber);
            searchLease.NavigateToSearchLicense();

            //Look for the previously created lease
            searchLease.SearchLicenseByLFile(leaseCode);
            searchLease.SelectFirstOption();

            //Navigate to Insurance
            insurance.NavigateToInsuranceSection();

            //Edit Insurance Section
            insurance.EditInsuranceButton();

            if (lease.TotalInsuranceCount > 0)
                insurance.DeleteLastInsurance();

            //Save Insurance
            leaseDetails.SaveLicense();

            //Verify Insurance changes
            insurance.VerifyInsuranceViewForm(lease);

            //Verify Insurance Total count
            Assert.True(insurance.TotalInsuranceCount().Equals(lease.TotalInsuranceCount));
        }

        [StepDefinition(@"I add Deposits to the Lease")]
        public void CreateDeposits()
        {
            /* TEST COVERAGE: PSP-2094, PSP-2921, PSP-2922 */

            //Navigate to Deposits
            deposits.NavigateToDepositSection();

            //Verify Deposits initial Tab
            deposits.VerifyDepositInitForm();

            //Verify Create a new deposit form
            deposits.AddDepositBttn();
            deposits.VerifyCreateDepositForm();
            leaseDetails.CancelLicense();

            if (lease.DepositsStartRow != 0)
            {
                for (var i = 0; i < lease.LeaseDeposits.Count; i++)
                {
                    //Add a new deposit
                    deposits.AddDepositBttn();
                    deposits.AddDeposit(lease.LeaseDeposits[i]);

                    //Verify new Deposit UI/UX on Table
                    deposits.VerifyCreatedDepositTable(lease.LeaseDeposits[i]);

                    //Verify Create Return Form
                    deposits.AddReturnBttn();
                    deposits.VerifyCreateReturnForm(lease.LeaseDeposits[i]);

                    //Add Return
                    deposits.AddReturn(lease.LeaseDeposits[i]);

                    //Verify new return UI/UX on Table
                    deposits.VerifyCreatedReturnTable(lease.LeaseDeposits[i]);
                }
            }

            //Add Deposit Notes
            if (lease.DepositNotes != "")
            {
                deposits.AddNotes(lease.DepositNotes);
                leaseDetails.SaveLicense();
            }
        }

        [StepDefinition(@"I update a Lease's Deposits from row number (.*)")]
        public void UpdateDeposits(int rowNumber)
        {
            /* TEST COVERAGE: PSP-2923, PSP-4196 */

            //Navigate to Search Leases
            PopulateLeaseLicense(rowNumber);
            searchLease.NavigateToSearchLicense();

            //Look for the last created lease
            searchLease.SearchLicenseByLFile(leaseCode);
            searchLease.SelectFirstOption();

            //Navigate to Deposits
            deposits.NavigateToDepositSection();

            //Delete first return
            deposits.DeleteFirstReturn();

            //Edit last deposit
            deposits.EditLastDeposit(lease.LeaseDeposits[0]);

            //Verify Deposit changes
            deposits.VerifyCreatedDepositTable(lease.LeaseDeposits[0]);

            //Delete first deposit
            deposits.DeleteFirstDeposit();
        }

        [StepDefinition(@"I add Payments to the Lease")]
        public void CreatePayments()
        {
            /* TEST COVERAGE: PSP-2755, PSP-2815, PSP-2915, PSP-2918, PSP-2927 */

            //Navigating to Payments section
            payments.NavigateToPaymentSection();

            //Verify initial Payments screen
            payments.VerifyPaymentsInitForm();

            for (var i = 0; i < lease.LeaseTerms.Count; i++)
            {
                //Verify Create Term Form
                payments.AddTermBttn();
                payments.VerifyCreateTermForm();

                //Inserting first term
                payments.AddTerm(lease.LeaseTerms[i]);

                //Verify inserted Term
                payments.VerifyInsertedTermTable(lease.LeaseTerms[i]);
            }

            for (var i = 0; i < lease.TermPayments.Count; i++)
            {
                //Add Payments
                payments.OpenPaymentTab(lease.TermPayments[i].ParentTerm);

                //Verify Payment Form
                payments.AddPaymentBttn();
                payments.VerifyCreatePaymentForm();

                //Inserting Payment for first term
                payments.AddPayment(lease.TermPayments[i]);

                //Verify Header for Payments Table
                payments.VerifyPaymentTableHeader();
                //payments.VerifyInsertedPayment(lease.TermPayments[i]);

                //Close Payment Tab
                payments.OpenPaymentTab(lease.TermPayments[i].ParentTerm);
            }
        }

        [StepDefinition(@"I update a Lease's Payments from row number (.*)")]
        public void UpdatePayments(int rowNumber)
        {
            /* TEST COVERAGE: PSP-4558 */

            //Navigate to Search Leases
            PopulateLeaseLicense(rowNumber);
            searchLease.NavigateToSearchLicense();

            //Look for the last created lease
            searchLease.SearchLastLease();
            searchLease.SelectFirstOption();

            //Navigate to Payments
            payments.NavigateToPaymentSection();

            //Delete last term
            payments.DeleteLastTerm();

            //Navigate to first term payments
            payments.OpenPaymentTab(lease.TermPayments[0].ParentTerm);

            //Delete last term last payment
            payments.DeleteLastPayment();
        }

        [StepDefinition(@"I verify the Surplus section")]
        public void VerifySurplus()
        {
            /* TEST COVERAGE: PSP-2157 */
           
            //Navigate to Surplus Declaration Section
            surplus.NavigateToSurplusSection();
            surplus.VerifySurplusForm();
        }

        [StepDefinition(@"I create a new Lease through a Property Pin from row number (.*)")]
        public void CreateLeaseGreenPin(int rowNumber)
        {
            /* TEST COVERAGE: PSP-4978, PSP-2648, PSP-2641 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Look for a Inventory Property
            PopulateLeaseLicense(rowNumber);
            searchProperties.SearchPropertyByPINPID(lease.SearchProperties.PID);

            //Choose the given result
            searchProperties.SelectFoundPin();

            //Close Main Information Window
            propertyInformation.ClosePropertyInfoModal();

            //Start a new lease from pop-up
            propertyInformation.OpenMoreOptionsPopUp();
            propertyInformation.ChooseCreationOptionFromPin("Lease/License - Create new");

            //Fill basic information on the form
            leaseDetails.CreateMinimumLicenseDetails(lease);

            //Save Lease Details
            leaseDetails.SaveLicense();

            //Verify Header with Expired Flag
            leaseDetails.VerifyLicenseHeader();

            //Insert an update
            leaseDetails.EditLeaseFileDetailsBttn();
            leaseDetails.UpdateLeaseFileDetails(lease);

            //Cancel changes
            leaseDetails.CancelLicense();

            //Get new lease's code
            leaseCode = leaseDetails.GetLeaseCode();
        }

        [StepDefinition(@"I create a new Lease through a Property of Interest from row number (.*)")]
        public void CreateLeaseBluePin(int rowNumber)
        {
            /* TEST COVERAGE: PSP-4979, PSP-2551 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Look for a Inventory Property
            PopulateLeaseLicense(rowNumber);
            searchProperties.SearchPropertyByPINPID(lease.SearchProperties.PID);

            //Choose the given result
            searchProperties.SelectFoundPin();

            //Close Main Information Window
            propertyInformation.ClosePropertyInfoModal();

            //Start a new lease from pop-up
            propertyInformation.OpenMoreOptionsPopUp();
            propertyInformation.ChooseCreationOptionFromPin("Lease/License - Create new");

            //Fill basic information on the form
            leaseDetails.CreateMinimumLicenseDetails(lease);

            //Save Lease Details
            leaseDetails.CancelLicense();

            //Start a new lease from pop-up
            propertyInformation.OpenMoreOptionsPopUp();
            propertyInformation.ChooseCreationOptionFromPin("Lease/License - Create new");

            //Fill basic information on the form
            leaseDetails.CreateMinimumLicenseDetails(lease);

            //Save changes
            leaseDetails.SaveLicense();

            //Get new lease's code
            leaseCode = leaseDetails.GetLeaseCode();
        }

        [StepDefinition(@"I create a new Lease through a Payable Marker from row number (.*)")]
        public void CreateLeasePurplePin(int rowNumber)
        {
            /* TEST COVERAGE: PSP-5158 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Look for a Inventory Property
            PopulateLeaseLicense(rowNumber);
            searchProperties.SearchPropertyByPINPID(lease.SearchProperties.PID);

            //Choose the given result
            searchProperties.SelectFoundPin();

            //Close Main Information Window
            propertyInformation.ClosePropertyInfoModal();

            //Start a new lease from pop-up
            propertyInformation.OpenMoreOptionsPopUp();
            propertyInformation.ChooseCreationOptionFromPin("Lease/License - Create new");

            //Fill basic information on the form
            leaseDetails.CreateMinimumLicenseDetails(lease);

            //Save Lease Details
            leaseDetails.SaveLicense();

            //Get new lease's code
            leaseCode = leaseDetails.GetLeaseCode();
        }

        [StepDefinition(@"I search for an existing Lease or License from row number (.*)")]
        public void SearchExistingLicense(int rowNumber)
        {
            /* TEST COVERAGE: PSP-2466  */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Leases Search
            searchLease.NavigateToSearchLicense();

            //Filter leases Files
            PopulateLeaseLicense(rowNumber);
            searchLease.FilterLeasesFiles(lease.SearchProperties.PID, lease.LeaseExpiryDate, lease.LeaseTenants[0].Summary,  lease.LeaseStatus);
            Assert.True(searchLease.SearchFoundResults());

            searchLease.FilterLeasesFiles("003-549-551", "05/12/1987", "Jonathan Doe", "Discarded");
            Assert.False(searchLease.SearchFoundResults());

            //Look for the last created research file
            searchLease.SearchLastLease();
        }

        [StepDefinition(@"A new lease is created successfully")]
        public void NewLeaseCreated()
        {
            //TEST COVERAGE: PSP-2466, PSP-2993

            searchLease.NavigateToSearchLicense();
            searchLease.SearchLicenseByLFile(leaseCode);

            Assert.True(searchLease.SearchFoundResults());
        }

        [StepDefinition(@"Expected Lease File Content is displayed on Leases Table")]
        public void VerifyAcquisitionFileTableContent()
        {
            /* TEST COVERAGE: PSP-1833 */

            //Verify List View
            searchLease.VerifySearchLeasesView();
            searchLease.VerifyLeaseTableContent(lease.LeaseExpiryDate, lease.Program, lease.LeaseStatus);

        }

        private void PopulateLeaseLicense(int rowNumber)
        {
            DataTable leaseSheet = ExcelDataContext.GetInstance().Sheets["Leases"];
            ExcelDataContext.PopulateInCollection(leaseSheet);

            //Lease Details
            lease.MinistryProjectCode = ExcelDataContext.ReadData(rowNumber, "MinistryProjectCode");
            lease.MinistryProject = ExcelDataContext.ReadData(rowNumber, "MinistryProject");
            lease.LeaseStatus = ExcelDataContext.ReadData(rowNumber, "LeaseStatus");
            lease.AccountType = ExcelDataContext.ReadData(rowNumber, "AccountType");
            lease.LeaseStartDate = ExcelDataContext.ReadData(rowNumber, "LeaseStartDate");
            lease.LeaseExpiryDate = ExcelDataContext.ReadData(rowNumber, "LeaseExpiryDate");

            lease.MOTIContact = ExcelDataContext.ReadData(rowNumber, "MOTIContact");
            lease.MOTIRegion = ExcelDataContext.ReadData(rowNumber, "MOTIRegion");
            lease.Program = ExcelDataContext.ReadData(rowNumber, "Program");
            lease.ProgramOther = ExcelDataContext.ReadData(rowNumber, "ProgramOther");
            lease.AdminType = ExcelDataContext.ReadData(rowNumber, "AdminType");
            lease.TypeOther = ExcelDataContext.ReadData(rowNumber, "TypeOther");
            lease.Category = ExcelDataContext.ReadData(rowNumber, "Category");
            lease.CategoryOther = ExcelDataContext.ReadData(rowNumber, "CategoryOther");
            lease.Purpose = ExcelDataContext.ReadData(rowNumber, "Purpose");
            lease.PurposeOther = ExcelDataContext.ReadData(rowNumber, "PurposeOther");
            lease.Initiator = ExcelDataContext.ReadData(rowNumber, "Initiator");
            lease.Responsibility = ExcelDataContext.ReadData(rowNumber, "Responsibility");
            lease.EffectiveDate = ExcelDataContext.ReadData(rowNumber, "EffectiveDate");
            lease.IntendedUse = ExcelDataContext.ReadData(rowNumber, "IntendedUse");

            lease.FirstNation = ExcelDataContext.ReadData(rowNumber, "FirstNation");
            lease.StrategicRealEstate = ExcelDataContext.ReadData(rowNumber, "StrategicRealEstate");
            lease.RegionalPlanning = ExcelDataContext.ReadData(rowNumber, "RegionalPlanning");
            lease.RegionalPropertyService = ExcelDataContext.ReadData(rowNumber, "RegionalPropertyService");
            lease.District = ExcelDataContext.ReadData(rowNumber, "District");
            lease.Headquarter = ExcelDataContext.ReadData(rowNumber, "Headquarter");
            lease.ConsultationOther = ExcelDataContext.ReadData(rowNumber, "ConsultationOther");
            lease.ConsultationOtherDetails = ExcelDataContext.ReadData(rowNumber, "ConsultationOtherDetails");

            lease.ProgramOther = ExcelDataContext.ReadData(rowNumber, "ProgramOther");
            lease.AdminType = ExcelDataContext.ReadData(rowNumber, "AdminType");
            lease.TypeOther = ExcelDataContext.ReadData(rowNumber, "TypeOther");
            lease.Purpose = ExcelDataContext.ReadData(rowNumber, "Purpose");
            lease.PurposeOther = ExcelDataContext.ReadData(rowNumber, "PurposeOther");
            lease.Initiator = ExcelDataContext.ReadData(rowNumber, "Initiator");
            lease.Responsibility = ExcelDataContext.ReadData(rowNumber, "Responsibility");
            lease.EffectiveDate = ExcelDataContext.ReadData(rowNumber, "EffectiveDate");
            lease.IntendedUse = ExcelDataContext.ReadData(rowNumber, "IntendedUse");

            lease.PhysicalLeaseExist = ExcelDataContext.ReadData(rowNumber, "PhysicalLeaseExist");
            lease.DigitalLeaseExist = ExcelDataContext.ReadData(rowNumber, "DigitalLeaseExist");
            lease.DocumentLocation = ExcelDataContext.ReadData(rowNumber, "DocumentLocation");
            lease.LISNumber = ExcelDataContext.ReadData(rowNumber, "LISNumber");
            lease.PSNumber = ExcelDataContext.ReadData(rowNumber, "PSNumber");
            lease.LeaseNotes = ExcelDataContext.ReadData(rowNumber, "LeaseNotes");
            lease.SearchPropertiesIndex = int.Parse(ExcelDataContext.ReadData(rowNumber, "SearchPropertiesIndex"));

            if (lease.SearchPropertiesIndex > 0)
            {
                DataTable searchPropertiesSheet = ExcelDataContext.GetInstance().Sheets["SearchProperties"];
                ExcelDataContext.PopulateInCollection(searchPropertiesSheet);

                lease.SearchProperties.PID = ExcelDataContext.ReadData(lease.SearchPropertiesIndex, "PID");
                lease.SearchProperties.PIN = ExcelDataContext.ReadData(lease.SearchPropertiesIndex, "PIN");
                lease.SearchProperties.Address = ExcelDataContext.ReadData(lease.SearchPropertiesIndex, "Address");
                lease.SearchProperties.PlanNumber = ExcelDataContext.ReadData(lease.SearchPropertiesIndex, "PlanNumber");
                lease.SearchProperties.LegalDescription = ExcelDataContext.ReadData(lease.SearchPropertiesIndex, "LegalDescription");
            }

            //Tenants
            lease.TenantsStartRow = int.Parse(ExcelDataContext.ReadData(rowNumber, "TenantsStartRow"));
            lease.TenantsQuantity = int.Parse(ExcelDataContext.ReadData(rowNumber, "TenantsQuantity"));
            lease.TenantsNumber = int.Parse(ExcelDataContext.ReadData(rowNumber, "TenantsNumber"));
            lease.RepresentativeNumber = int.Parse(ExcelDataContext.ReadData(rowNumber, "RepresentativeNumber"));
            lease.PropertyManagerNumber = int.Parse(ExcelDataContext.ReadData(rowNumber, "PropertyManagerNumber"));
            lease.UnknownNumber = int.Parse(ExcelDataContext.ReadData(rowNumber, "UnknownNumber"));
            if (lease.TenantsStartRow != 0 && lease.TenantsQuantity != 0)
            {
                PopulateTenantsCollection(lease.TenantsStartRow, lease.TenantsQuantity);
            }

            //Improvements
            lease.CommercialImprovementUnit = ExcelDataContext.ReadData(rowNumber, "CommercialImprovementUnit");
            lease.CommercialImprovementBuildingSize = ExcelDataContext.ReadData(rowNumber, "CommercialImprovementBuildingSize");
            lease.CommercialImprovementDescription = ExcelDataContext.ReadData(rowNumber, "CommercialImprovementDescription");

            lease.ResidentialImprovementUnit = ExcelDataContext.ReadData(rowNumber, "ResidentialImprovementUnit");
            lease.ResidentialImprovementBuildingSize = ExcelDataContext.ReadData(rowNumber, "ResidentialImprovementBuildingSize");
            lease.ResidentialImprovementDescription = ExcelDataContext.ReadData(rowNumber, "ResidentialImprovementDescription");

            lease.OtherImprovementUnit = ExcelDataContext.ReadData(rowNumber, "OtherImprovementUnit");
            lease.OtherImprovementBuildingSize = ExcelDataContext.ReadData(rowNumber, "OtherImprovementBuildingSize");
            lease.OtherImprovementDescription = ExcelDataContext.ReadData(rowNumber, "OtherImprovementDescription");

            lease.TotalImprovementCount = int.Parse(ExcelDataContext.ReadData(rowNumber, "TotalImprovementCount"));

            //Insurance
            lease.AircraftInsuranceInPlace = ExcelDataContext.ReadData(rowNumber, "AircraftInsuranceInPlace");
            lease.AircraftLimit = ExcelDataContext.ReadData(rowNumber, "AircraftLimit");
            lease.AircraftPolicyExpiryDate = ExcelDataContext.ReadData(rowNumber, "AircraftPolicyExpiryDate");
            lease.AircraftDescriptionCoverage = ExcelDataContext.ReadData(rowNumber, "AircraftDescriptionCoverage");

            lease.CGLInsuranceInPlace = ExcelDataContext.ReadData(rowNumber, "CGLInsuranceInPlace");
            lease.CGLLimit = ExcelDataContext.ReadData(rowNumber, "CGLLimit");
            lease.CGLPolicyExpiryDate = ExcelDataContext.ReadData(rowNumber, "CGLPolicyExpiryDate");
            lease.CGLDescriptionCoverage = ExcelDataContext.ReadData(rowNumber, "CGLDescriptionCoverage");

            lease.MarineInsuranceInPlace = ExcelDataContext.ReadData(rowNumber, "MarineInsuranceInPlace");
            lease.MarineLimit = ExcelDataContext.ReadData(rowNumber, "MarineLimit");
            lease.MarinePolicyExpiryDate = ExcelDataContext.ReadData(rowNumber, "MarinePolicyExpiryDate");
            lease.MarineDescriptionCoverage = ExcelDataContext.ReadData(rowNumber, "MarineDescriptionCoverage");

            lease.VehicleInsuranceInPlace = ExcelDataContext.ReadData(rowNumber, "VehicleInsuranceInPlace");
            lease.VehicleLimit = ExcelDataContext.ReadData(rowNumber, "VehicleLimit");
            lease.VehiclePolicyExpiryDate = ExcelDataContext.ReadData(rowNumber, "VehiclePolicyExpiryDate");
            lease.VehicleDescriptionCoverage = ExcelDataContext.ReadData(rowNumber, "VehicleDescriptionCoverage");

            lease.OtherInsuranceType = ExcelDataContext.ReadData(rowNumber, "OtherInsuranceType");
            lease.OtherInsuranceInPlace = ExcelDataContext.ReadData(rowNumber, "OtherInsuranceInPlace");
            lease.OtherLimit = ExcelDataContext.ReadData(rowNumber, "OtherLimit");
            lease.OtherPolicyExpiryDate = ExcelDataContext.ReadData(rowNumber, "OtherPolicyExpiryDate");
            lease.OtherDescriptionCoverage = ExcelDataContext.ReadData(rowNumber, "OtherDescriptionCoverage");

            lease.TotalInsuranceCount = int.Parse(ExcelDataContext.ReadData(rowNumber, "TotalInsuranceCount"));

            //Deposits
            lease.DepositNotes = ExcelDataContext.ReadData(rowNumber, "DepositNotes");
            lease.DepositsStartRow = int.Parse(ExcelDataContext.ReadData(rowNumber, "DepositsStartRow"));
            lease.DepositsCount = int.Parse(ExcelDataContext.ReadData(rowNumber, "DepositsCount"));
            if (lease.DepositsStartRow != 0 && lease.DepositsCount != 0)
            {
                PopulateDepositsCollection(lease.DepositsStartRow, lease.DepositsCount);
            }

            //Terms
            lease.TermsStartRow = int.Parse(ExcelDataContext.ReadData(rowNumber, "TermsStartRow"));
            lease.TermsCount = int.Parse(ExcelDataContext.ReadData(rowNumber, "TermsCount"));
            if (lease.TermsStartRow != 0 && lease.TermsCount != 0)
            {
                PopulateTermsCollection(lease.TermsStartRow, lease.TermsCount);
            }

            //Payments
            lease.PaymentsStartRow = int.Parse(ExcelDataContext.ReadData(rowNumber, "PaymentsStartRow"));
            lease.PaymentsCount = int.Parse(ExcelDataContext.ReadData(rowNumber, "PaymentsCount"));
            if (lease.PaymentsStartRow != 0 && lease.PaymentsCount != 0)
            {
                PopulatePaymentsCollection(lease.PaymentsStartRow, lease.PaymentsCount);
            }
        }

        private void PopulateTenantsCollection(int startRow, int rowsCount)
        {
            DataTable leasesTenantsSheet = ExcelDataContext.GetInstance().Sheets["LeasesTenants"];
            ExcelDataContext.PopulateInCollection(leasesTenantsSheet);

            for (int i = startRow; i < startRow + rowsCount; i++)
            {
                Tenant tenant = new Tenant();
                tenant.ContactType = ExcelDataContext.ReadData(i, "ContactType");
                tenant.Summary = ExcelDataContext.ReadData(i, "Summary");
                tenant.PrimaryContact = ExcelDataContext.ReadData(i, "PrimaryContact");
                tenant.TenantType = ExcelDataContext.ReadData(i, "TenantType");

                lease.LeaseTenants.Add(tenant);
            }
        }

        private void PopulateDepositsCollection(int startRow, int rowsCount)
        {
            DataTable leasesDepositsSheet = ExcelDataContext.GetInstance().Sheets["LeasesDeposits"];
            ExcelDataContext.PopulateInCollection(leasesDepositsSheet);

            for (int i = startRow; i < startRow + rowsCount; i++)
            {
                Deposit deposit = new Deposit();
                deposit.DepositType = ExcelDataContext.ReadData(i, "DepositType");
                deposit.DepositTypeOther = ExcelDataContext.ReadData(i, "DepositTypeOther");

                deposit.DepositDescription = ExcelDataContext.ReadData(i, "DepositDescription");
                deposit.DepositAmount = ExcelDataContext.ReadData(i, "DepositAmount");
                deposit.DepositPaidDate = ExcelDataContext.ReadData(i, "DepositPaidDate");
                deposit.DepositHolder = ExcelDataContext.ReadData(i, "DepositHolder");

                deposit.ReturnTerminationDate = ExcelDataContext.ReadData(i, "ReturnTerminationDate");
                deposit.TerminationClaimDeposit = ExcelDataContext.ReadData(i, "TerminationClaimDeposit");
                deposit.ReturnedAmount = ExcelDataContext.ReadData(i, "ReturnedAmount");
                deposit.ReturnInterestPaid = ExcelDataContext.ReadData(i, "ReturnInterestPaid");
                deposit.ReturnedDate = ExcelDataContext.ReadData(i, "ReturnedDate");
                deposit.ReturnPayeeName = ExcelDataContext.ReadData(i, "ReturnPayeeName");

                lease.LeaseDeposits.Add(deposit);
            }
        }

        private void PopulateTermsCollection(int startRow, int rowsCount)
        {
            DataTable leasesTermsSheet = ExcelDataContext.GetInstance().Sheets["LeasesTerms"];
            ExcelDataContext.PopulateInCollection(leasesTermsSheet);

            for (int i = startRow; i < startRow + rowsCount; i++)
            {
                Term term = new Term();
                term.TermStartDate = ExcelDataContext.ReadData(i, "TermStartDate");
                term.TermEndDate = ExcelDataContext.ReadData(i, "TermEndDate");
                term.TermPaymentFrequency = ExcelDataContext.ReadData(i, "TermPaymentFrequency");
                term.TermAgreedPayment = ExcelDataContext.ReadData(i, "TermAgreedPayment");
                term.TermPaymentsDue = ExcelDataContext.ReadData(i, "TermPaymentsDue");
                term.IsGSTEligible = bool.Parse(ExcelDataContext.ReadData(i, "IsGSTEligible"));
                term.TermStatus = ExcelDataContext.ReadData(i, "TermStatus");

                lease.LeaseTerms.Add(term);
            }
        }

        private void PopulatePaymentsCollection(int startRow, int rowsCount)
        {
            DataTable leasesDepositsPaymentsSheet = ExcelDataContext.GetInstance().Sheets["LeasesPayments"];
            ExcelDataContext.PopulateInCollection(leasesDepositsPaymentsSheet);

            for (int i = startRow; i < startRow + rowsCount; i++)
            {
                Payment payment = new Payment();
                payment.PaymentSentDate = ExcelDataContext.ReadData(i, "PaymentSentDate");
                payment.PaymentMethod = ExcelDataContext.ReadData(i, "PaymentMethod");
                payment.PaymentTotalReceived = ExcelDataContext.ReadData(i, "PaymentTotalReceived");
                payment.PaymentExpectedPayment = ExcelDataContext.ReadData(i, "PaymentExpectedPayment");
                payment.PaymentGST = ExcelDataContext.ReadData(i, "PaymentGST");
                payment.PaymentStatus = ExcelDataContext.ReadData(i, "PaymentStatus");
                payment.ParentTerm = int.Parse(ExcelDataContext.ReadData(i, "ParentTerm"));

                lease.TermPayments.Add(payment);
            }
        }
    }
}
