
using PIMS.Tests.Automation.Classes;
using PIMS.Tests.Automation.Data;
using System.Data;

namespace PIMS.Tests.Automation.StepDefinitions
{
    [Binding]
    public class LeaseLicenseSteps
    {
        private readonly LoginSteps loginSteps;
        private readonly LeaseDetails leaseDetails;
        private readonly LeaseConsultations leaseConsultation;
        private readonly LeasesChecklist checklist;
        private readonly LeaseTenants tenant;
        private readonly LeasePeriodPayments periodPayments;
        private readonly LeaseImprovements improvements;
        private readonly LeaseInsurance insurance;
        private readonly LeaseDeposits deposits;
        private readonly LeaseSurplus surplus;
        private readonly SearchLease searchLeases;
        private readonly SearchProperties searchProperties;
        private readonly PropertyInformation propertyInformation;
        private readonly SharedFileProperties sharedSearchProperties;
        private readonly SharedPagination sharedPagination;
        private readonly SharedCompensations h120;
        private readonly Notes notes;
        private readonly GenericSteps genericSteps;

        private readonly string userName = "TRANPSP1";

        private Lease lease;
        protected string leaseCode = "";
        protected string compensationNumber = "";

        public LeaseLicenseSteps(BrowserDriver driver)
        {
            loginSteps = new LoginSteps(driver);
            genericSteps = new GenericSteps(driver);
            leaseDetails = new LeaseDetails(driver.Current);
            leaseConsultation = new LeaseConsultations(driver.Current);
            checklist = new LeasesChecklist(driver.Current);
            tenant = new LeaseTenants(driver.Current);
            periodPayments = new LeasePeriodPayments(driver.Current);
            improvements = new LeaseImprovements(driver.Current);
            insurance = new LeaseInsurance(driver.Current);
            deposits = new LeaseDeposits(driver.Current);
            surplus = new LeaseSurplus(driver.Current);
            searchLeases = new SearchLease(driver.Current);
            searchProperties = new SearchProperties(driver.Current);
            propertyInformation = new PropertyInformation(driver.Current);
            sharedSearchProperties = new SharedFileProperties(driver.Current);
            sharedPagination = new SharedPagination(driver.Current);
            h120 = new SharedCompensations(driver.Current);
            notes = new Notes(driver.Current);

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
            leaseDetails.VerifyLicenseDetailsInitCreateForm();
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
            leaseDetails.UpdateLeaseFileDetails(lease);

            //Save the new license details
            leaseDetails.SaveLicense();
            leaseDetails.VerifyLicenseDetailsViewForm(lease);
        }

        [StepDefinition(@"I add Properties to the Lease Details")]
        public void AddPropertiesLicenseDetails()
        {
            /* TEST COVERAGE:  PSP-1966, PSP-2550, PSP-2644, PSP-4558, PSP-5334, PSP-5335, PSP-5336, PSP-5337, PSP-5338, PSP-5340, PSP-5654, PSP-5668, PSP-5923, PSP-6266 */

            //Add Additional information to the lease
            leaseDetails.EditLeaseFileDetailsBttn();

            //Add Several Properties
            //Verify UI/UX from Search By Component
            sharedSearchProperties.NavigateToSearchTab();
            sharedSearchProperties.VerifySearchPropertiesFeature();

            //Search for a property by PID
            if (lease.SearchProperties.PID != "")
            {
                sharedSearchProperties.SelectPropertyByPID(lease.SearchProperties.PID);
                sharedSearchProperties.SelectFirstOptionFromSearch();
            }

            //Search for a property by PIN
            if (lease.SearchProperties.PIN != "")
            {
                sharedSearchProperties.SelectPropertyByPIN(lease.SearchProperties.PIN);
                sharedSearchProperties.SelectFirstOptionFromSearch();
            }

            //Search for a property by Address
            if (lease.SearchProperties.Address != "")
            {
                sharedSearchProperties.SelectPropertyByAddress(lease.SearchProperties.Address);
                sharedSearchProperties.SelectFirstOptionFromSearch();
            }

            //Search for a property by Legal Description
            if (lease.SearchProperties.LegalDescription != "")
            {
                sharedSearchProperties.SelectPropertyByLegalDescription(lease.SearchProperties.LegalDescription);
                sharedSearchProperties.SelectFirstOptionFromSearch();
            }

            //Search for a property by Plan
            if (lease.SearchProperties.PlanNumber != "")
            {
                sharedSearchProperties.SelectPropertyByPlan(lease.SearchProperties.PlanNumber);
                sharedSearchProperties.SelectFirstOptionFromSearch();
            }

            //Search for a duplicate property
            if (lease.SearchProperties.PID != "")
            {
                sharedSearchProperties.SelectPropertyByPID(lease.SearchProperties.PID);
                sharedSearchProperties.SelectFirstOptionFromSearch();
            }

            //Update Properties
            for (var i = 0; i < lease.LeasePropertiesDetails.Count; i++)
                leaseDetails.UpdateLicensePropertiesForm(lease.LeasePropertiesDetails[i], i);

            //Save the new license details
            leaseDetails.SaveLicense();

            //Verify File Details Form
            //leaseDetails.VerifyLicensePropertyViewForm(lease.LeasePropertiesDetails, lease.AccountType);
        }

        [StepDefinition(@"I update a Lease's Details from row number (.*)")]
        public void UpdateLeaseDetails(int rowNumber)
        {
            /* TEST COVERAGE: PSP-2096, PSP-2642, PSP-4558, PSP-5161, PSP-5245, PSP-5342, PSP-5667, PSP-5924 */

            //Navigate to Search Leases
            PopulateLeaseLicense(rowNumber);
            searchLeases.NavigateToSearchLicense();

            //Look for the previously created lease
            searchLeases.SearchLicenseByLFile(leaseCode);
            searchLeases.SelectFirstOption();

            //Edit File Details Section
            leaseDetails.EditLeaseFileDetailsBttn();

            //Make some changes on the Details Form
            leaseDetails.UpdateLeaseFileDetails(lease);

            //Save the new license details
            leaseDetails.SaveLicense();

            //Verify File Details Form
            leaseDetails.VerifyLicenseDetailsViewForm(lease);
        }

        [StepDefinition(@"I update a Lease's Properties from row number (.*)")]
        public void UpdateLeaseProperties(int rowNumber)
        {
            /* TEST COVERAGE: PSP-2096, PSP-2642, PSP-4558, PSP-5161, PSP-5245, PSP-5342, PSP-5667, PSP-5924 */

            //Navigate to Search Leases
            PopulateLeaseLicense(rowNumber);
            searchLeases.NavigateToSearchLicense();

            //Look for the previously created lease
            searchLeases.SearchLicenseByLFile(leaseCode);
            searchLeases.SelectFirstOption();

            //Edit File Details Section
            leaseDetails.EditLeaseFileDetailsBttn();

            //Verify Properties section
            sharedSearchProperties.VerifyLocateOnMapFeature();

            //Update Properties
            leaseDetails.UpdateLicensePropertiesForm(lease.LeasePropertiesDetails[0], 0);

            //Save the new license details
            leaseDetails.CancelLicense();

            //Edit File Details Section
            leaseDetails.EditLeaseFileDetailsBttn();

            //Update Properties
            leaseDetails.UpdateLicensePropertiesForm(lease.LeasePropertiesDetails[0], 0);

            //Save the new license details
            leaseDetails.SaveLicense();

            //Verify File Details Form
            leaseDetails.VerifyLicensePropertyViewForm(lease.LeasePropertiesDetails, lease.AccountType);

            //Edit File Details Section
            leaseDetails.EditLeaseFileDetailsBttn();

            //Delete last property
            sharedSearchProperties.DeleteLastPropertyFromLease();

            //Save the new license details
            leaseDetails.SaveLicense();
        }

        [StepDefinition(@"I insert new consultations to the Lease")]
        public void CreateConsultations()
        {
            //Navigate to the Approvals/Consultations Tab
            leaseConsultation.NavigateToConsultationsTab();

            //Verify Initial View Form
            leaseConsultation.VerifyInitConsultationTab();

            //Verify Create Consultation Form
            leaseConsultation.AddConsultationBttn();
            leaseConsultation.VerifyConsultationCreateForm();
            leaseDetails.CancelLicense();

            for (var i = 0; i < lease.LeaseConsultations.Count; i++)
            {
                //Click on Add new consultation Button
                leaseConsultation.AddConsultationBttn();

                //Create a new consultation
                leaseConsultation.AddUpdateConsultation(lease.LeaseConsultations[i]);

                //Save changes
                leaseDetails.SaveLicense();

                //Verify Inserted consultation
                leaseConsultation.VerifyLastInsertedConsultationView(lease.LeaseConsultations[i]);
            }
        }

        [StepDefinition(@"I update a Lease's consultation from row number (.*)")]
        public void UpdateConsultations(int rowNumber)
        {
            //Navigate to Search Leases
            PopulateLeaseLicense(rowNumber);
            searchLeases.NavigateToSearchLicense();

            //Look for the previously created lease
            searchLeases.SearchLicenseByLFile(leaseCode);
            searchLeases.SelectFirstOption();

            //Navigate to the consultations tab
            leaseConsultation.NavigateToConsultationsTab();

            //Edit specific consultation
            leaseConsultation.EditLastConsultationByType(lease.LeaseConsultations[0].leaseConsultationType);
            leaseConsultation.AddUpdateConsultation(lease.LeaseConsultations[0]);

            //Save changes
            leaseDetails.SaveLicense();

            //Verify changes
            leaseConsultation.VerifyLastInsertedConsultationView(lease.LeaseConsultations[0]);

            //Delete last "Other" consultation
            leaseConsultation.DeleteLastConsultationByType("Other");
        }

        [StepDefinition(@"I insert Checklist information to a Lease")]
        public void CreateChecklist()
        {
            /* TEST COVERAGE: PSP-5899, PSP-5900, PSP-5904, PSP-5921 */

            //Navigate to Checklist Tab
            checklist.NavigateChecklistTab();

            //Verify View Checklist form
            checklist.VerifyChecklistInitViewForm();

            //Edit Checklist button
            checklist.EditChecklistButton();

            //Verify Edit Checklist form
            checklist.VerifyChecklistEditForm();

            //Update Checklist Form
            checklist.UpdateChecklist(lease.LeaseChecklist);

            //Save changes
            checklist.SaveLeaseChecklist();
        }

        [StepDefinition(@"I add Tenants to the Lease")]
        public void CreateTenants()
        {
            /* TEST COVERAGE: PSP-3492, PSP-3494, PSP-3495, PSP-3496, PSP-3498, PSP-3499 */

            //TENANTS
            //Navigate to Tenants
            tenant.NavigateToStakeholderSection(lease.AccountType);

            //Edit stakeholders Section
            tenant.EditStakeholderButton();

            //Verify stakeholders Initial Form
            tenant.VerifyStakeholdersInitForm(lease.AccountType);

            //Go back to initial view form
            leaseDetails.CancelLicense();

            //Adding an individual stakeholder
            if (lease.LeaseTenants.Count > 0)
            {
                for (var i = 0; i < lease.LeaseTenants.Count; i++)
                {
                    //Edit stakeholders Section
                    tenant.EditStakeholderButton();

                    if (lease.LeaseTenants[i].ContactType == "Individual")
                        tenant.AddIndividualStakeholder(lease.AccountType, lease.LeaseTenants[i]);
                    else
                        tenant.AddOrganizationTenant(lease.AccountType, lease.LeaseTenants[i]);

                    //Saving stakeholders
                    tenant.SaveTenant();
                }

            }

            //Assert quantity of stakeholders
            if (lease.AccountType == "Receivable")
            {
                Assert.Equal(lease.AssigneeNumber, tenant.TotalAssignees());
                Assert.Equal(lease.TenantsNumber, tenant.TotalTenants());
                Assert.Equal(lease.RepresentativeNumber, tenant.TotalRepresentatives());
                Assert.Equal(lease.PropertyManagerNumber, tenant.TotalManagers());
                Assert.Equal(lease.UnknownTenantNumber, tenant.TotalUnknown());
            }
            else
            {
                Assert.Equal(lease.OwnerRepresentativeNumber, tenant.TotalOwnerRepresentatives());
                Assert.Equal(lease.OwnerPayeeNumber, tenant.TotalOwners());
            }
        }

        [StepDefinition(@"I update a Lease's Tenants from row number (.*)")]
        public void UpdateTenants(int rowNumber)
        {
            /* TEST COVERAGE:  PSP-4558 */

            //Navigate to Search Leases
            PopulateLeaseLicense(rowNumber);
            searchLeases.NavigateToSearchLicense();

            //Look for the previously created lease
            searchLeases.SearchLicenseByLFile(leaseCode);
            searchLeases.SelectFirstOption();

            //Navigate to Tenants section
            tenant.NavigateToStakeholderSection(lease.AccountType);

            //Edit stakeholders
            if (lease.TenantsQuantity > 0)
            {
                //Delete last stakeholder
                tenant.EditStakeholderButton();
                tenant.DeleteLastStakeholder();

                //Save stakeholders changes
                tenant.SaveTenant();

                for (int i = 0; i < lease.LeaseTenants.Count; i++)
                {
                    //Edit last stakeholder
                    tenant.EditStakeholderButton();
                    tenant.EditStakeholder(lease.LeaseTenants[i]);

                    //Save stakeholders changes
                    tenant.SaveTenant();
                }


                //Assert quantity of stakeholders
                if (lease.AccountType == "Receivable")
                {
                    Assert.Equal(lease.AssigneeNumber, tenant.TotalAssignees());
                    Assert.Equal(lease.TenantsNumber, tenant.TotalTenants());
                    Assert.Equal(lease.RepresentativeNumber, tenant.TotalRepresentatives());
                    Assert.Equal(lease.PropertyManagerNumber, tenant.TotalManagers());
                    Assert.Equal(lease.UnknownTenantNumber, tenant.TotalUnknown());
                }
                else
                {
                    Assert.Equal(lease.OwnerRepresentativeNumber, tenant.TotalOwnerRepresentatives());
                    Assert.Equal(lease.OwnerPayeeNumber, tenant.TotalOwners());
                }
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
            searchLeases.NavigateToSearchLicense();

            //Look for the previously created lease
            searchLeases.SearchLicenseByLFile(leaseCode);
            searchLeases.SelectFirstOption();
           
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
            insurance.VerifyInsuranceInitForm();

            //Add Accidental Insurance
            if (lease.AccidentalDescriptionCoverage != "")
                insurance.AddAccidentalInsurance(lease);

            //Add Aircraft Insurance
            if (lease.AircraftDescriptionCoverage != "")
                insurance.AddAircraftInsurance(lease);

            //Add CGL Insurance
            if (lease.CGLDescriptionCoverage != "")
                insurance.AddCGLInsurance(lease);

            //Add Marine Insurance
            if (lease.MarineDescriptionCoverage != "")
                insurance.AddMarineInsurance(lease);

            //Add Unmmaned Air Vehicle Insurance
            if (lease.UnmannedAirVehicleDescriptionCoverage != "")
                insurance.AddUnmannedAirVehicleInsurance(lease);

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
            searchLeases.NavigateToSearchLicense();

            //Look for the previously created lease
            searchLeases.SearchLicenseByLFile(leaseCode);
            searchLeases.SelectFirstOption();

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
            Assert.Equal(lease.TotalInsuranceCount, insurance.TotalInsuranceCount());
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
            deposits.CancelDeposit();

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
            searchLeases.NavigateToSearchLicense();

            //Look for the last created lease
            searchLeases.SearchLicenseByLFile(leaseCode);
            searchLeases.SelectFirstOption();

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

        [StepDefinition(@"I add Periods and Payments to the Lease")]
        public void CreatePayments()
        {
            /* TEST COVERAGE: PSP-2755, PSP-2815, PSP-2915, PSP-2918, PSP-2927 */

            //Navigating to Payments section
            periodPayments.NavigateToPaymentSection();

            //Verify init Periods Tab
            periodPayments.VerifyPeriodsTabInit();

            // Insert Periods
            for (var i = 0; i < lease.LeaseTerms.Count; i++)
            {
                //Verify Create Period Form
                periodPayments.AddPeriodBttn();

                //Inserting the period's information
                periodPayments.AddPeriod(lease.LeaseTerms[i]);

                //Open new period's details
                periodPayments.OpenClosePeriodCategoryPayments(i+1);

                //Verify inserted Term
                periodPayments.VerifyInsertedPeriodTable(lease.LeaseTerms[i]);

                //Close new period's details
                periodPayments.OpenClosePeriodCategoryPayments(i+1);
            }

            //Insert Payments
            for (var j = 0; j < lease.PeriodPayments.Count; j++)
            {
                //Add Payments
                periodPayments.OpenClosePeriodCategoryPayments(lease.PeriodPayments[j].PeriodParentIndex);

                //Verify Payment Form
                periodPayments.AddPaymentBttn(lease.PeriodPayments[j].PeriodParentIndex);

                //Inserting Payment for first term
                periodPayments.AddPayment(lease.PeriodPayments[j], lease.PeriodPayments[j].ParentPeriodPaymentType);

                //Verify inserted Payments Table
                periodPayments.VerifyInsertedPaymentTable(lease.PeriodPayments[j], lease.PeriodPayments[j].PeriodParentIndex, lease.AccountType) ;

                //Close Payment Tab
                periodPayments.OpenClosePeriodCategoryPayments(lease.PeriodPayments[j].PeriodParentIndex);
            }
        }

        [StepDefinition(@"I update a Lease's Payments from row number (.*)")]
        public void UpdatePayments(int rowNumber)
        {
            /* TEST COVERAGE: PSP-4558 */

            //Navigate to Search Leases
            PopulateLeaseLicense(rowNumber);
            searchLeases.NavigateToSearchLicense();

            //Look for the last created lease
            searchLeases.SearchLastLease();
            searchLeases.SelectFirstOption();

            //Navigate to Payments
            periodPayments.NavigateToPaymentSection();

            //Delete last term
            periodPayments.DeleteLastPeriod();

            //Navigate to first term payments
            periodPayments.OpenClosePeriodCategoryPayments(lease.PeriodPayments[0].PeriodParentIndex);

            //Delete last term last payment
            periodPayments.DeleteLastPayment(lease.PeriodPayments[0].PeriodParentIndex);
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
            searchProperties.SearchPropertyByPID(lease.SearchProperties.PID);

            //Choose the given result
            searchProperties.SelectFoundPin();

            //Close Main Information Window
            propertyInformation.HideLeftSideForms();

            //Start a new lease from pop-up
            propertyInformation.OpenMoreOptionsPopUp();
            propertyInformation.ChooseCreationOptionFromPin("Lease/License");

            //Open Left Side Forms
            propertyInformation.ShowLeftSideForms();

            //Fill basic information on the form
            leaseDetails.CreateMinimumLicenseDetails(lease);

            //Save Lease Details
            leaseDetails.SaveLicense();

            //Verify Header with Expired Flag
            leaseDetails.VerifyLicenseHeader(lease);

            //Insert an update
            leaseDetails.EditLeaseFileDetailsBttn();
            leaseDetails.UpdateLeaseFileDetails(lease);

            //Cancel changes
            leaseDetails.CancelLicense();

            //Get new lease's code
            leaseCode = leaseDetails.GetLeaseCode();
        }

        [StepDefinition(@"I search for an existing Lease or License from row number (.*)")]
        public void SearchExistingLicense(int rowNumber)
        {
            /* TEST COVERAGE: PSP-2466 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Leases Search
            searchLeases.NavigateToSearchLicense();

            //Verify Pagination
            sharedPagination.ChoosePaginationOption(5);
            Assert.Equal(5, searchLeases.LeasesTableResultNumber());

            sharedPagination.ChoosePaginationOption(10);
            Assert.Equal(10, searchLeases.LeasesTableResultNumber());

            sharedPagination.ChoosePaginationOption(20);
            Assert.Equal(20, searchLeases.LeasesTableResultNumber());

            sharedPagination.ChoosePaginationOption(50);
            Assert.Equal(50, searchLeases.LeasesTableResultNumber());

            sharedPagination.ChoosePaginationOption(100);
            Assert.Equal(100, searchLeases.LeasesTableResultNumber());

            //Delete "Active" status from list view
            searchLeases.SearchAllLeases();

            //Verify Column Sorting by File Number
            searchLeases.OrderByLeaseFileNumber();
            var firstFileNbrDescResult = searchLeases.FirstLeaseFileNumber();

            searchLeases.OrderByLeaseFileNumber();
            var firstFileNbrAscResult = searchLeases.FirstLeaseFileNumber();

            Assert.NotEqual(firstFileNbrDescResult, firstFileNbrAscResult);

            //Verify Column Sorting by Expiry Date
            searchLeases.OrderByLeaseExpiryDate();
            var firstExpiryDateDescResult = searchLeases.FirstLeaseExpiryDate();

            searchLeases.OrderByLeaseExpiryDate();
            var firstExpiryDatelAscResult = searchLeases.FirstLeaseExpiryDate();

            Assert.NotEqual(firstExpiryDateDescResult, firstExpiryDatelAscResult);

            //Verify Column Sorting by Program Name
            searchLeases.OrderByLeaseProgramName();
            var firstProgramNameDescResult = searchLeases.FirstLeaseProgramName();

            searchLeases.OrderByLeaseProgramName();
            var firstProgramNameAscResult = searchLeases.FirstLeaseProgramName();

            Assert.NotEqual(firstProgramNameDescResult, firstProgramNameAscResult);

            //Verify Column Sorting by File Name
            searchLeases.OrderByLeaseStatus();
            var firstStatusDescResult = searchLeases.FirstLeaseStatus();

            searchLeases.OrderByLeaseStatus();
            var firstStatusAscResult = searchLeases.FirstLeaseStatus();

            Assert.NotEqual(firstStatusDescResult, firstStatusAscResult);

            //Verify Pagination display different set of results
            sharedPagination.ResetSearch();

            var firstLeasePage1 = searchLeases.FirstLeaseFileNumber();
            sharedPagination.GoNextPage();
            var firstLeasePage2 = searchLeases.FirstLeaseFileNumber();
            Assert.NotEqual(firstLeasePage1, firstLeasePage2);

            sharedPagination.ResetSearch();

            //Filter leases Files
            PopulateLeaseLicense(rowNumber);
            searchLeases.FilterLeasesFiles("", "", "", "", "", "", lease.LeaseStatus, "", lease.LeaseExpiryDate, "", "", "");
            Assert.True(searchLeases.SearchFoundResults());

            searchLeases.FilterLeasesFiles("", "", "", "", "", "", "", "Progressive Motor Sports", "", "", "", "");
            Assert.True(searchLeases.SearchFoundResults());

            searchLeases.FilterLeasesFiles("003-549-551", "", "", "", "", "", "Duplicate", "Jonathan Doe", "05/12/1987", "", "", "");
            Assert.False(searchLeases.SearchFoundResults());

            //searchLeases.FilterLeasesFiles("", "", "", "", "TestPN654", "", "", "", "", "", "", "");
            //Assert.True(searchLeases.SearchFoundResults());

            searchLeases.FilterLeasesFiles("", "", "", "", "", "", "Terminated", "", "03/22/2024", "", "", "");
            searchLeases.OrderByLastLease();
        }

        [StepDefinition(@"A new lease is created successfully")]
        public void NewLeaseCreated()
        {
            //TEST COVERAGE: PSP-2466, PSP-2993

            searchLeases.NavigateToSearchLicense();
            searchLeases.SearchLicenseByLFile(leaseCode);

            Assert.True(searchLeases.SearchFoundResults());
            searchLeases.Dispose();
        }

        [StepDefinition(@"Expected Lease File Content is displayed on Leases Table")]
        public void VerifyAcquisitionFileTableContent()
        {
            /* TEST COVERAGE: PSP-1833 */

            //Verify List View
            searchLeases.VerifySearchLeasesView();
            searchLeases.VerifyLeaseTableContent(lease);
            searchLeases.Dispose();
        }

        [StepDefinition(@"I create Compensation Requisition within an Lease/Licence")]
        public void CreateCompensationRequisition()
        {
            /* TEST COVERAGE: PSP-6066, PSP-6067, PSP-6274, PSP-6277, PSP-6355 */

            //Navigate to Compensation Requisition Tab
            h120.NavigateCompensationTab();

            //Verify initial Compensation Tab List View
            h120.VerifyCompensationInitTabView("Lease");

            //Update Allowable Compensation Amount
            h120.UpdateTotalAllowableCompensation(lease.LeaseCompensationTotalAllowableAmount);

            //Create Compensation Requisition Forms
            if (lease.LeaseCompensations.Count > 0)
            {
                for (int i = 0; i < lease.LeaseCompensations.Count; i++)
                {
                    //Click on Add new Compensation
                    h120.AddCompensationBttn();

                    //Open the created Compensation Requisition details
                    h120.OpenCompensationDetails(i);

                    //Verify Initial View Form
                    h120.VerifyCompensationDetailsInitViewForm("Lease");

                    //Add Details to the Compensation Requisition
                    h120.EditCompensationDetails();
                    //h120.VerifyCompensationDetailsInitCreateForm();
                    h120.UpdateCompensationDetails(lease.LeaseCompensations[i]);

                    //Save new Compensation Requisition Details
                    h120.SaveAcquisitionFileCompensation();

                    //Verify added Compensation Requisition List View and Details
                    h120.VerifyCompensationDetailsViewForm(lease.LeaseCompensations[i], "Lease");
                    h120.VerifyCompensationListView(lease.LeaseCompensations[i]);
                }
            }
        }

        [StepDefinition(@"I update Compensation Requisition within an Lease from row number (.*)")]
        public void UpdateCompensationRequisition(int rowNumber)
        {
            /* TEST COVERAGE:  PSP-6275, PSP-6282, PSP-6356, PSP-6360, PSP-6483, PSP-6484 */

            //Populate data
            PopulateLeaseLicense(rowNumber);

            searchLeases.NavigateToSearchLicense();

            //Look for the last created lease
            searchLeases.SearchLicenseByLFile(leaseCode);
            searchLeases.SelectFirstOption();

            //Navigate to Compensation Tab
            h120.NavigateCompensationTab();

            //Select first created compensation requisition
            h120.OpenCompensationDetails(0);

            //Edit Compensation button
            h120.EditCompensationDetails();

            //Make changes on created Compensation Requisition Form
            h120.UpdateCompensationDetails(lease.LeaseCompensations[0]);

            //Cancel changes
            h120.CancelAcquisitionFileCompensation();

            //Make changes on created Compensation Requisition Form
            h120.EditCompensationDetails();
            h120.UpdateCompensationDetails(lease.LeaseCompensations[0]);

            //Save changes
            h120.SaveAcquisitionFileCompensation();

            //Get updated compensation number
            compensationNumber = h120.GetCompensationFileNumber(1);

            //Verify automatic note
            notes.NavigateNotesTab();
            notes.VerifyAutomaticNotesCompensation(compensationNumber, "Draft", "Final");

            //Navigate to Leases stakeholder
            tenant.NavigateToStakeholderSection(lease.AccountType);

            //Edit stakeholders Section
            tenant.EditStakeholderButton();

            //Delete the stakeholder that is associated to a compensation requisition
            tenant.DeleteFirstStakeholder();

            //Save Acquisition File Details changes
            leaseDetails.SaveLicenseWithExpectedErrors();

            //Cancel Acquisition File changes
            leaseDetails.CancelLicense();

            //Navigate back to Compensation Tab
            h120.NavigateCompensationTab();

            //Open Requisition File
            h120.OpenCompensationDetails(0);

            //Edit Compensation Button
            h120.EditCompensationDetails();

            //Delete Financial Activity
            var activitiesBeforeDelete = h120.TotalActivitiesCount();
            h120.DeleteFirstActivity();

            var activitiesAfterDelete = h120.TotalActivitiesCount();
            Assert.True(activitiesBeforeDelete - activitiesAfterDelete == 1);

            //Save Compensation changes
            h120.SaveAcquisitionFileCompensation();

            //Create a new Compensation Requisition
            h120.AddCompensationBttn();

            var compensationsBeforeDelete = h120.TotalCompensationCount();
            h120.DeleteCompensationRequisition(1);

            var compensationsAfterDelete = h120.TotalCompensationCount();

            Assert.True(compensationsBeforeDelete - compensationsAfterDelete == 1);
        }

        private void PopulateLeaseLicense(int rowNumber)
        {
            DataTable leaseSheet = ExcelDataContext.GetInstance().Sheets["Leases"]!;
            ExcelDataContext.PopulateInCollection(leaseSheet);

            lease = new Lease
            {
                //Lease Details
                MinistryProjectCode = ExcelDataContext.ReadData(rowNumber, "MinistryProjectCode"),
                MinistryProject = ExcelDataContext.ReadData(rowNumber, "MinistryProject"),
                MinistryProduct = ExcelDataContext.ReadData(rowNumber, "MinistryProduct"),
                LeaseStatus = ExcelDataContext.ReadData(rowNumber, "LeaseStatus"),
                LeaseTerminationDate = ExcelDataContext.ReadData(rowNumber, "LeaseTerminationDate"),
                LeaseTerminationReason = ExcelDataContext.ReadData(rowNumber, "LeaseTerminationReason"),
                LeaseCancellationReason = ExcelDataContext.ReadData(rowNumber, "LeaseCancellationReason"),
                AccountType = ExcelDataContext.ReadData(rowNumber, "AccountType"),
                LeaseStartDate = ExcelDataContext.ReadData(rowNumber, "LeaseStartDate"),
                LeaseExpiryDate = ExcelDataContext.ReadData(rowNumber, "LeaseExpiryDate"),
                LeaseRenewalStartRow = int.Parse(ExcelDataContext.ReadData(rowNumber, "LeaseRenewalStartRow")),
                LeaseRenewalQuantity = int.Parse(ExcelDataContext.ReadData(rowNumber, "LeaseRenewalQuantity"))
            };

            if (lease.LeaseRenewalStartRow != 0 && lease.LeaseRenewalQuantity != 0)
                PopulateRenewalsCollection(lease.LeaseRenewalStartRow, lease.LeaseRenewalQuantity);

            lease.MOTIContact = ExcelDataContext.ReadData(rowNumber, "MOTIContact");
            lease.MOTIRegion = ExcelDataContext.ReadData(rowNumber, "MOTIRegion");
            lease.Program = ExcelDataContext.ReadData(rowNumber, "Program");
            lease.ProgramOther = ExcelDataContext.ReadData(rowNumber, "ProgramOther");
            lease.AdminType = ExcelDataContext.ReadData(rowNumber, "AdminType");
            lease.TypeOther = ExcelDataContext.ReadData(rowNumber, "TypeOther");
            lease.LeasePurpose = genericSteps.PopulateLists(ExcelDataContext.ReadData(rowNumber, "LeasePurpose"));
            lease.PurposeOther = ExcelDataContext.ReadData(rowNumber, "PurposeOther");
            lease.Initiator = ExcelDataContext.ReadData(rowNumber, "Initiator");
            lease.Responsibility = ExcelDataContext.ReadData(rowNumber, "Responsibility");
            lease.EffectiveDate = ExcelDataContext.ReadData(rowNumber, "EffectiveDate");
            lease.IntendedUse = ExcelDataContext.ReadData(rowNumber, "IntendedUse");
            lease.ArbitrationCity = ExcelDataContext.ReadData(rowNumber, "ArbitrationCity");

            lease.FirstNation = ExcelDataContext.ReadData(rowNumber, "FirstNation");
            lease.StrategicRealEstate = ExcelDataContext.ReadData(rowNumber, "StrategicRealEstate");
            lease.RegionalPlanning = ExcelDataContext.ReadData(rowNumber, "RegionalPlanning");
            lease.RegionalPropertyService = ExcelDataContext.ReadData(rowNumber, "RegionalPropertyService");
            lease.District = ExcelDataContext.ReadData(rowNumber, "District");
            lease.Headquarter = ExcelDataContext.ReadData(rowNumber, "Headquarter");
            lease.ConsultationOther = ExcelDataContext.ReadData(rowNumber, "ConsultationOther");
            lease.ConsultationOtherDetails = ExcelDataContext.ReadData(rowNumber, "ConsultationOtherDetails");

            lease.FeeDeterminationPublicBenefit = ExcelDataContext.ReadData(rowNumber, "FeeDeterminationPublicBenefit");
            lease.FeeDeterminationFinancialGain = ExcelDataContext.ReadData(rowNumber, "FeeDeterminationFinancialGain");
            lease.FeeDeterminationSuggestedFee = ExcelDataContext.ReadData(rowNumber, "FeeDeterminationSuggestedFee");
            lease.FeeDeterminationNotes = ExcelDataContext.ReadData(rowNumber, "FeeDeterminationNotes");

            lease.SearchPropertiesIndex = int.Parse(ExcelDataContext.ReadData(rowNumber, "LeaseSearchPropertiesIndex"));

            if (lease.SearchPropertiesIndex > 0)
            {
                DataTable searchPropertiesSheet = ExcelDataContext.GetInstance().Sheets["SearchProperties"]!;
                ExcelDataContext.PopulateInCollection(searchPropertiesSheet);

                lease.SearchProperties.PID = ExcelDataContext.ReadData(lease.SearchPropertiesIndex, "PID");
                lease.SearchProperties.PIN = ExcelDataContext.ReadData(lease.SearchPropertiesIndex, "PIN");
                lease.SearchProperties.Address = ExcelDataContext.ReadData(lease.SearchPropertiesIndex, "Address");
                lease.SearchProperties.PlanNumber = ExcelDataContext.ReadData(lease.SearchPropertiesIndex, "PlanNumber");
                lease.SearchProperties.LegalDescription = ExcelDataContext.ReadData(lease.SearchPropertiesIndex, "LegalDescription");
            }

            lease.LeasePropertyDetailsStartRow = int.Parse(ExcelDataContext.ReadData(rowNumber, "LeasePropertyDetailsStartRow"));
            lease.LeasePropertyDetailsQuantity = int.Parse(ExcelDataContext.ReadData(rowNumber, "LeasePropertyDetailsQuantity"));
            if (lease.LeasePropertyDetailsStartRow != 0 && lease.LeasePropertyDetailsQuantity != 0)
                PopulatePropertiesCollection(lease.LeasePropertyDetailsStartRow, lease.LeasePropertyDetailsQuantity);

            //Leases Approval/Consultations
            lease.LeaseConsultationStartRow = int.Parse(ExcelDataContext.ReadData(rowNumber, "LeaseConsultationStartRow"));
            lease.LeaseConsultationQuantity = int.Parse(ExcelDataContext.ReadData(rowNumber, "LeaseConsultationQuantity"));
            if (lease.LeaseConsultationStartRow != 0 && lease.LeaseConsultationQuantity != 0)
                PopulateConsultationsCollection(lease.LeaseConsultationStartRow, lease.LeaseConsultationQuantity);

            //Leases File Checklist
            lease.LeaseChecklistIndex = int.Parse(ExcelDataContext.ReadData(rowNumber, "LeaseChecklistIndex"));
            if (lease.LeaseChecklistIndex > 0)
            {
                DataTable leaseChecklistSheet = ExcelDataContext.GetInstance().Sheets["LeasesChecklist"]!;
                ExcelDataContext.PopulateInCollection(leaseChecklistSheet);

                lease.LeaseChecklist.FileInitiationSelect1 = ExcelDataContext.ReadData(lease.LeaseChecklistIndex, "FileInitiationSelect1");
                lease.LeaseChecklist.FileInitiationSelect2 = ExcelDataContext.ReadData(lease.LeaseChecklistIndex, "FileInitiationSelect2");
                lease.LeaseChecklist.FileInitiationSelect3 = ExcelDataContext.ReadData(lease.LeaseChecklistIndex, "FileInitiationSelect3");
                lease.LeaseChecklist.FileInitiationSelect4 = ExcelDataContext.ReadData(lease.LeaseChecklistIndex, "FileInitiationSelect4");
                lease.LeaseChecklist.FileInitiationSelect5 = ExcelDataContext.ReadData(lease.LeaseChecklistIndex, "FileInitiationSelect5");
                lease.LeaseChecklist.FileInitiationSelect6 = ExcelDataContext.ReadData(lease.LeaseChecklistIndex, "FileInitiationSelect6");

                lease.LeaseChecklist.ReferralsApprovalsSelect1 = ExcelDataContext.ReadData(lease.LeaseChecklistIndex, "ReferralsApprovalsSelect1");
                lease.LeaseChecklist.ReferralsApprovalsSelect2 = ExcelDataContext.ReadData(lease.LeaseChecklistIndex, "ReferralsApprovalsSelect2");
                lease.LeaseChecklist.ReferralsApprovalsSelect3 = ExcelDataContext.ReadData(lease.LeaseChecklistIndex, "ReferralsApprovalsSelect3");
                lease.LeaseChecklist.ReferralsApprovalsSelect4 = ExcelDataContext.ReadData(lease.LeaseChecklistIndex, "ReferralsApprovalsSelect4");
                lease.LeaseChecklist.ReferralsApprovalsSelect5 = ExcelDataContext.ReadData(lease.LeaseChecklistIndex, "ReferralsApprovalsSelect5");
                lease.LeaseChecklist.ReferralsApprovalsSelect6 = ExcelDataContext.ReadData(lease.LeaseChecklistIndex, "ReferralsApprovalsSelect6");
                lease.LeaseChecklist.ReferralsApprovalsSelect7 = ExcelDataContext.ReadData(lease.LeaseChecklistIndex, "ReferralsApprovalsSelect7");
                lease.LeaseChecklist.ReferralsApprovalsSelect8 = ExcelDataContext.ReadData(lease.LeaseChecklistIndex, "ReferralsApprovalsSelect8");

                lease.LeaseChecklist.AgreementPreparationSelect1 = ExcelDataContext.ReadData(lease.LeaseChecklistIndex, "AgreementPreparationSelect1");
                lease.LeaseChecklist.AgreementPreparationSelect2 = ExcelDataContext.ReadData(lease.LeaseChecklistIndex, "AgreementPreparationSelect2");
                lease.LeaseChecklist.AgreementPreparationSelect3 = ExcelDataContext.ReadData(lease.LeaseChecklistIndex, "AgreementPreparationSelect3");
                lease.LeaseChecklist.AgreementPreparationSelect4 = ExcelDataContext.ReadData(lease.LeaseChecklistIndex, "AgreementPreparationSelect4");
                lease.LeaseChecklist.AgreementPreparationSelect5 = ExcelDataContext.ReadData(lease.LeaseChecklistIndex, "AgreementPreparationSelect5");
                lease.LeaseChecklist.AgreementPreparationSelect6 = ExcelDataContext.ReadData(lease.LeaseChecklistIndex, "AgreementPreparationSelect6");
                lease.LeaseChecklist.AgreementPreparationSelect7 = ExcelDataContext.ReadData(lease.LeaseChecklistIndex, "AgreementPreparationSelect7");
                lease.LeaseChecklist.AgreementPreparationSelect8 = ExcelDataContext.ReadData(lease.LeaseChecklistIndex, "AgreementPreparationSelect8");
                lease.LeaseChecklist.AgreementPreparationSelect9 = ExcelDataContext.ReadData(lease.LeaseChecklistIndex, "AgreementPreparationSelect9");
                lease.LeaseChecklist.AgreementPreparationSelect10 = ExcelDataContext.ReadData(lease.LeaseChecklistIndex, "AgreementPreparationSelect10");
                lease.LeaseChecklist.AgreementPreparationSelect11 = ExcelDataContext.ReadData(lease.LeaseChecklistIndex, "AgreementPreparationSelect11");
                lease.LeaseChecklist.AgreementPreparationSelect12 = ExcelDataContext.ReadData(lease.LeaseChecklistIndex, "AgreementPreparationSelect12");
                lease.LeaseChecklist.AgreementPreparationSelect13 = ExcelDataContext.ReadData(lease.LeaseChecklistIndex, "AgreementPreparationSelect13");

                lease.LeaseChecklist.LeaseLicenceCompletionSelect1 = ExcelDataContext.ReadData(lease.LeaseChecklistIndex, "LeaseLicenceCompletionSelect1");
                lease.LeaseChecklist.LeaseLicenceCompletionSelect2 = ExcelDataContext.ReadData(lease.LeaseChecklistIndex, "LeaseLicenceCompletionSelect2");
            }

            //Tenants
            lease.TenantsStartRow = int.Parse(ExcelDataContext.ReadData(rowNumber, "TenantsStartRow"));
            lease.TenantsQuantity = int.Parse(ExcelDataContext.ReadData(rowNumber, "TenantsQuantity"));
            lease.TenantsNumber = int.Parse(ExcelDataContext.ReadData(rowNumber, "TenantsNumber"));
            lease.AssigneeNumber = int.Parse(ExcelDataContext.ReadData(rowNumber, "AssigneeNumber"));
            lease.RepresentativeNumber = int.Parse(ExcelDataContext.ReadData(rowNumber, "RepresentativeNumber"));
            lease.PropertyManagerNumber = int.Parse(ExcelDataContext.ReadData(rowNumber, "PropertyManagerNumber"));
            lease.UnknownTenantNumber = int.Parse(ExcelDataContext.ReadData(rowNumber, "UnknownTenantNumber"));
            lease.OwnerPayeeNumber = int.Parse(ExcelDataContext.ReadData(rowNumber, "OwnerPayeeNumber"));
            lease.OwnerRepresentativeNumber = int.Parse(ExcelDataContext.ReadData(rowNumber, "OwnerRepresentativeNumber"));
            if (lease.TenantsStartRow != 0 && lease.TenantsQuantity != 0)
                PopulateTenantsCollection(lease.TenantsStartRow, lease.TenantsQuantity);
            
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
            lease.AccidentalInsuranceInPlace = ExcelDataContext.ReadData(rowNumber, "AccidentalInsuranceInPlace");
            lease.AccidentalLimit = ExcelDataContext.ReadData(rowNumber, "AccidentalLimit");
            lease.AccidentalPolicyExpiryDate = ExcelDataContext.ReadData(rowNumber, "AccidentalPolicyExpiryDate");
            lease.AccidentalDescriptionCoverage = ExcelDataContext.ReadData(rowNumber, "AccidentalDescriptionCoverage");

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

            lease.UnmannedAirVehicleInsuranceInPlace = ExcelDataContext.ReadData(rowNumber, "UnmannedAirVehicleInsuranceInPlace");
            lease.UnmannedAirVehicleLimit = ExcelDataContext.ReadData(rowNumber, "UnmannedAirVehicleLimit");
            lease.UnmannedAirVehiclePolicyExpiryDate = ExcelDataContext.ReadData(rowNumber, "UnmannedAirVehiclePolicyExpiryDate");
            lease.UnmannedAirVehicleDescriptionCoverage = ExcelDataContext.ReadData(rowNumber, "UnmannedAirVehicleDescriptionCoverage");

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
                PopulateDepositsCollection(lease.DepositsStartRow, lease.DepositsCount);
            
            //Periods
            lease.PeriodsStartRow = int.Parse(ExcelDataContext.ReadData(rowNumber, "PeriodsStartRow"));
            lease.PeriodsCount = int.Parse(ExcelDataContext.ReadData(rowNumber, "PeriodsCount"));
            if (lease.PeriodsStartRow != 0 && lease.PeriodsCount != 0)
                PopulatePeriodsCollection(lease.PeriodsStartRow, lease.PeriodsCount);

            //Payments
            lease.PeriodPaymentsStartRow = int.Parse(ExcelDataContext.ReadData(rowNumber, "PeriodPaymentsStartRow"));
            lease.PeriodPaymentsCount = int.Parse(ExcelDataContext.ReadData(rowNumber, "PeriodPaymentsCount"));

            if (lease.PeriodPaymentsStartRow != 0 && lease.PeriodPaymentsCount != 0)
                PopulatePaymentsCollection(lease.PeriodPaymentsStartRow, lease.PeriodPaymentsCount);

            //Compensations
            lease.LeaseCompensationStartRow = int.Parse(ExcelDataContext.ReadData(rowNumber, "LeaseCompensationStartRow"));
            lease.LeaseCompensationCount = int.Parse(ExcelDataContext.ReadData(rowNumber, "LeaseCompensationCount"));
            lease.LeaseCompensationTotalAllowableAmount = ExcelDataContext.ReadData(rowNumber, "LeaseCompensationTotalAllowableAmount");
            if (lease.LeaseCompensationStartRow != 0 && lease.LeaseCompensationCount != 0)
                PopulateCompensationsCollection(lease.LeaseCompensationStartRow, lease.LeaseCompensationCount);
        }

        private void PopulateRenewalsCollection(int startRow, int rowsCount)
        {
            DataTable leasesRenewalsSheet = ExcelDataContext.GetInstance().Sheets["LeasesRenewals"]!;
            ExcelDataContext.PopulateInCollection(leasesRenewalsSheet);

            for (int i = startRow; i < startRow + rowsCount; i++)
            {
                LeaseRenewal renewal = new()
                {
                    RenewalIsExercised = ExcelDataContext.ReadData(i, "RenewalIsExercised"),
                    RenewalCommencementDate = ExcelDataContext.ReadData(i, "RenewalCommencementDate"),
                    RenewalExpiryDate = ExcelDataContext.ReadData(i, "RenewalExpiryDate"),
                    RenewalNotes = ExcelDataContext.ReadData(i, "RenewalNotes")
                };

                lease.LeaseRenewals.Add(renewal);
            }
        }

        private void PopulatePropertiesCollection(int startRow, int rowsCount)
        {
            DataTable propertiesSheet = ExcelDataContext.GetInstance().Sheets["LeasesProperties"]!;
            ExcelDataContext.PopulateInCollection(propertiesSheet);

            for (int i = startRow; i < startRow + rowsCount; i++)
            {

                var property = new LeaseProperty
                {
                    PID = ExcelDataContext.ReadData(i, "LeasePropPID"),
                    HistoricalFile = ExcelDataContext.ReadData(i, "LeasePropHistoricalFile"),
                    DescriptiveName = ExcelDataContext.ReadData(i, "LeasePropDescriptiveName"),
                    Area = ExcelDataContext.ReadData(i, "LeasePropArea")
                };

                property.Address.AddressLine1 = ExcelDataContext.ReadData(i, "LeasePropAddressLine1");
                property.Address.AddressLine2 = ExcelDataContext.ReadData(i, "LeasePropAddressLine2");
                property.Address.AddressLine3 = ExcelDataContext.ReadData(i, "LeasePropAddressLine3");
                property.Address.City = ExcelDataContext.ReadData(i, "LeasePropAddressCity");
                property.Address.PostalCode = ExcelDataContext.ReadData(i, "LeasePropPostalCode");
                property.LegalDescription = ExcelDataContext.ReadData(i, "LeasePropLegalDescription");

                lease.LeasePropertiesDetails.Add(property);
            }
        }

        private void PopulateConsultationsCollection(int startRow, int rowsCount)
        {
            DataTable leasesConsultationsSheet = ExcelDataContext.GetInstance().Sheets["LeasesConsultations"]!;
            ExcelDataContext.PopulateInCollection(leasesConsultationsSheet);

            lease.LeaseConsultations = new List<LeaseConsultation>();

            for (int i = startRow; i < startRow + rowsCount; i++)
            {
                LeaseConsultation consultation = new()
                {
                    leaseConsultationType = ExcelDataContext.ReadData(i, "leaseConsultationType"),
                    leaseConsultationOtherDescription = ExcelDataContext.ReadData(i, " leaseConsultationOtherDescription"),
                    leaseConsultationRequestedOn = ExcelDataContext.ReadData(i, "leaseConsultationRequestedOn"),
                    leaseConsultationContactType = ExcelDataContext.ReadData(i, "leaseConsultationContactType"),
                    leaseConsultationContact = ExcelDataContext.ReadData(i, "leaseConsultationContact"),
                    leaseConsultationContactPrimaryContact = ExcelDataContext.ReadData(i, "leaseConsultationContactPrimaryContact"),
                    leaseConsultationReceived = ExcelDataContext.ReadData(i, "leaseConsultationReceived"),
                    leaseConsultationReceivedOn = ExcelDataContext.ReadData(i, "leaseConsultationReceivedOn"),
                    leaseConsultationOutcome = ExcelDataContext.ReadData(i, "leaseConsultationOutcome"),
                    leaseConsultationComment = ExcelDataContext.ReadData(i, "leaseConsultationComment"), 
                };

                lease.LeaseConsultations.Add(consultation);
            }
        }

        private void PopulateTenantsCollection(int startRow, int rowsCount)
        {
            DataTable leasesTenantsSheet = ExcelDataContext.GetInstance().Sheets["LeasesTenants"]!;
            ExcelDataContext.PopulateInCollection(leasesTenantsSheet);

            for (int i = startRow; i < startRow + rowsCount; i++)
            {
                Stakeholder tenant = new()
                {
                    ContactType = ExcelDataContext.ReadData(i, "ContactType"),
                    Summary = ExcelDataContext.ReadData(i, "Summary"),
                    PrimaryContact = ExcelDataContext.ReadData(i, "LeaseTenantsPrimaryContact"),
                    StakeholderType = ExcelDataContext.ReadData(i, "TenantType")
                };

                lease.LeaseTenants.Add(tenant);
            }
        }

        private void PopulateDepositsCollection(int startRow, int rowsCount)
        {
            DataTable leasesDepositsSheet = ExcelDataContext.GetInstance().Sheets["LeasesDeposits"]!;
            ExcelDataContext.PopulateInCollection(leasesDepositsSheet);

            for (int i = startRow; i < startRow + rowsCount; i++)
            {
                Deposit deposit = new()
                {
                    DepositType = ExcelDataContext.ReadData(i, "DepositType"),
                    DepositTypeOther = ExcelDataContext.ReadData(i, "DepositTypeOther"),

                    DepositDescription = ExcelDataContext.ReadData(i, "DepositDescription"),
                    DepositAmount = ExcelDataContext.ReadData(i, "DepositAmount"),
                    DepositPaidDate = ExcelDataContext.ReadData(i, "DepositPaidDate"),
                    DepositHolder = ExcelDataContext.ReadData(i, "DepositHolder"),

                    ReturnTerminationDate = ExcelDataContext.ReadData(i, "ReturnTerminationDate"),
                    TerminationClaimDeposit = ExcelDataContext.ReadData(i, "TerminationClaimDeposit"),
                    ReturnedAmount = ExcelDataContext.ReadData(i, "ReturnedAmount"),
                    ReturnInterestPaid = ExcelDataContext.ReadData(i, "ReturnInterestPaid"),
                    ReturnedDate = ExcelDataContext.ReadData(i, "ReturnedDate"),
                    ReturnPayeeName = ExcelDataContext.ReadData(i, "ReturnPayeeName")
                };

                lease.LeaseDeposits.Add(deposit);
            }
        }

        private void PopulatePeriodsCollection(int startRow, int rowsCount)
        {
            DataTable leasesPeriodsSheet = ExcelDataContext.GetInstance().Sheets["LeasesPeriods"]!;
            ExcelDataContext.PopulateInCollection(leasesPeriodsSheet);

            for (int i = startRow; i < startRow + rowsCount; i++)
            {
                Period period = new()
                {
                    PeriodPaymentType = ExcelDataContext.ReadData(i, "PeriodPaymentType"),
                    PeriodDuration = ExcelDataContext.ReadData(i, "PeriodDuration"),
                    PeriodStartDate = ExcelDataContext.ReadData(i, "PeriodStartDate"),
                    PeriodEndDate = ExcelDataContext.ReadData(i, "PeriodEndDate"),
                    PeriodPaymentsDue = ExcelDataContext.ReadData(i, "PeriodPaymentsDue"),
                    PeriodStatus = ExcelDataContext.ReadData(i, "PeriodStatus"),

                    PeriodBasePaymentFrequency = ExcelDataContext.ReadData(i, "PeriodBasePaymentFrequency"),
                    PeriodBaseAgreedPayment = ExcelDataContext.ReadData(i, "PeriodBaseAgreedPayment"),
                    PeriodBaseIsGSTEligible = ExcelDataContext.ReadData(i, "PeriodBaseIsGSTEligible"),
                    PeriodBaseGSTAmount = ExcelDataContext.ReadData(i, "PeriodBaseGSTAmount"),
                    PeriodBaseTotalPaymentAmount = ExcelDataContext.ReadData(i, "PeriodBaseTotalPaymentAmount"),

                    PeriodAdditionalPaymentFrequency = ExcelDataContext.ReadData(i, "PeriodAdditionalPaymentFrequency"),
                    PeriodAdditionalAgreedPayment = ExcelDataContext.ReadData(i, "PeriodAdditionalAgreedPayment"),
                    PeriodAdditionalIsGSTEligible = ExcelDataContext.ReadData(i, "PeriodAdditionalIsGSTEligible"),
                    PeriodAdditionalGSTAmount = ExcelDataContext.ReadData(i, "PeriodAdditionalGSTAmount"),
                    PeriodAdditionalTotalPaymentAmount = ExcelDataContext.ReadData(i, "PeriodAdditionalTotalPaymentAmount"),

                    PeriodVariablePaymentFrequency = ExcelDataContext.ReadData(i, "PeriodVariablePaymentFrequency"),
                    PeriodVariableAgreedPayment = ExcelDataContext.ReadData(i, "PeriodVariableAgreedPayment"),
                    PeriodVariableIsGSTEligible = ExcelDataContext.ReadData(i, "PeriodVariableIsGSTEligible"),
                    PeriodVariableGSTAmount = ExcelDataContext.ReadData(i, "PeriodVariableGSTAmount"),
                    PeriodVariableTotalPaymentAmount = ExcelDataContext.ReadData(i, "PeriodVariableTotalPaymentAmount")
                };

                lease.LeaseTerms.Add(period);
            }
        }

        private void PopulatePaymentsCollection(int startRow, int rowsCount)
        {
            DataTable leasesDepositsPaymentsSheet = ExcelDataContext.GetInstance().Sheets["LeasesPayments"]!;
            ExcelDataContext.PopulateInCollection(leasesDepositsPaymentsSheet);

            for (int i = startRow; i < startRow + rowsCount; i++)
            {
                Payment payment = new()
                {
                    PaymentSentDate = ExcelDataContext.ReadData(i, "PaymentSentDate"),
                    PaymentMethod = ExcelDataContext.ReadData(i, "PaymentMethod"),
                    PaymentCategory = ExcelDataContext.ReadData(i, "PaymentCategory"),
                    PaymentTotalReceived = ExcelDataContext.ReadData(i, "PaymentTotalReceived"),
                    PaymentExpectedPayment = ExcelDataContext.ReadData(i, "PaymentExpectedPayment"),
                    PaymentIsGSTApplicable = ExcelDataContext.ReadData(i, "PaymentIsGSTApplicable"),
                    PaymentGST = ExcelDataContext.ReadData(i, "PaymentGST"),
                    PaymentStatus = ExcelDataContext.ReadData(i, "PaymentStatus"),
                    PeriodParentIndex = int.Parse(ExcelDataContext.ReadData(i, "PeriodParentIndex")),
                    ParentPeriodPaymentType = ExcelDataContext.ReadData(i, "ParentPeriodPaymentType")
                };

                lease.PeriodPayments.Add(payment);
            }
        }

        private void PopulateCompensationsCollection(int startRow, int rowsCount)
        {
            DataTable compensationSheet = ExcelDataContext.GetInstance().Sheets["Compensation"]!;
            ExcelDataContext.PopulateInCollection(compensationSheet);

            for (int i = startRow; i < startRow + rowsCount; i++)
            {
                Compensation compensation = new Compensation();

                compensation.CompensationAmount = ExcelDataContext.ReadData(i, "CompensationAmount");
                compensation.CompensationGSTAmount = ExcelDataContext.ReadData(i, "CompensationGSTAmount");
                compensation.CompensationTotalAmount = ExcelDataContext.ReadData(i, "CompensationTotalAmount");
                compensation.CompensationStatus = ExcelDataContext.ReadData(i, "CompensationStatus");
                compensation.CompensationAlternateProject = ExcelDataContext.ReadData(i, "CompensationAlternateProject");
                compensation.CompensationAgreementDate = ExcelDataContext.ReadData(i, "CompensationAgreementDate");
                compensation.CompensationExpropriationNoticeDate = ExcelDataContext.ReadData(i, "CompensationExpropriationNoticeDate");
                compensation.CompensationExpropriationVestingDate = ExcelDataContext.ReadData(i, "CompensationExpropriationVestingDate");
                compensation.CompensationAdvancedPaymentDate = ExcelDataContext.ReadData(i, "CompensationAdvancedPaymentDate");
                compensation.CompensationSpecialInstructions = ExcelDataContext.ReadData(i, "CompensationSpecialInstructions");
                compensation.CompensationFiscalYear = ExcelDataContext.ReadData(i, "CompensationFiscalYear");
                compensation.CompensationSTOB = ExcelDataContext.ReadData(i, "CompensationSTOB");
                compensation.CompensationServiceLine = ExcelDataContext.ReadData(i, "CompensationServiceLine");
                compensation.CompensationResponsibilityCentre = ExcelDataContext.ReadData(i, "CompensationResponsibilityCentre");
                compensation.CompensationPayee = ExcelDataContext.ReadData(i, "CompensationPayee");
                compensation.CompensationPayeeDisplay = ExcelDataContext.ReadData(i, "CompensationPayeeDisplay");
                compensation.CompensationPaymentInTrust = Boolean.Parse(ExcelDataContext.ReadData(i, "CompensationPaymentInTrust"));
                compensation.CompensationGSTNumber = ExcelDataContext.ReadData(i, "CompensationGSTNumber");
                compensation.CompensationDetailedRemarks = ExcelDataContext.ReadData(i, "CompensationDetailedRemarks");
                compensation.ActivitiesStartRow = int.Parse(ExcelDataContext.ReadData(i, "ActivitiesStartRow"));
                compensation.ActivitiesCount = int.Parse(ExcelDataContext.ReadData(i, "ActivitiesCount"));

                if (compensation.ActivitiesStartRow != 0 && compensation.ActivitiesCount != 0)
                {
                    PopulateActivitiesCollection(compensation.ActivitiesStartRow, compensation.ActivitiesCount, compensation.CompensationActivities);
                }

                lease.LeaseCompensations.Add(compensation);
            }
        }

        private void PopulateActivitiesCollection(int startRow, int rowsCount, List<CompensationActivity> activities)
        {
            DataTable activitiesSheet = ExcelDataContext.GetInstance().Sheets["CompensationActivities"]!;
            ExcelDataContext.PopulateInCollection(activitiesSheet);

            for (int i = startRow; i < startRow + rowsCount; i++)
            {
                CompensationActivity activity = new CompensationActivity();

                activity.ActCodeDescription = ExcelDataContext.ReadData(i, "ActCodeDescription");
                activity.ActAmount = ExcelDataContext.ReadData(i, "ActAmount");
                activity.ActGSTEligible = ExcelDataContext.ReadData(i, "ActGSTEligible");
                activity.ActGSTAmount = ExcelDataContext.ReadData(i, "ActGSTAmount");
                activity.ActTotalAmount = ExcelDataContext.ReadData(i, "ActTotalAmount");

                activities.Add(activity);
            }
        }
    }
}
