using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;
using PIMS.Tests.Automation.Data;

namespace PIMS.Tests.Automation.StepDefinitions
{
    [Binding]
    public class ManagementFileSteps(IWebDriver driver)
    {
        private readonly LoginSteps loginSteps = new LoginSteps(driver);
        private readonly GenericSteps genericSteps = new GenericSteps(driver);

        private readonly ManagementDetails managementFilesDetails = new ManagementDetails(driver);
        private readonly ManagementActivities managementActivities = new ManagementActivities(driver);
        private readonly SearchManagement searchManagementFiles = new SearchManagement(driver);
        private readonly SharedFileProperties sharedFileProperties = new SharedFileProperties(driver);
        private readonly SharedPagination sharedPagination = new SharedPagination(driver);
        private readonly SharedActivities sharedActivities = new SharedActivities(driver);
        private readonly Notes notes = new Notes(driver);

        private readonly string userName = "TRANPSP1";

        private ManagementFile managementFile;
        private string managementFileCode = string.Empty;

        [StepDefinition(@"I create a new Management File from row number (.*)")]
        public void CreateManagementFile(int rowNumber)
        {
            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Management File
            PopulateManagementFile(rowNumber);
            managementFilesDetails.NavigateToCreateNewManagementFile();

            //Validate Management File Details Create Form
            managementFilesDetails.VerifyManagementFileInitCreateForm();

            //Create basic Management File
            managementFilesDetails.CreateMinimumManagementDetails(managementFile);

            //Save Management File
            managementFilesDetails.SaveManagementFile();

            //Get Acquisition File code
            managementFileCode = managementFilesDetails.GetManagementCode();
        }

        [StepDefinition(@"I add additional information to the Management File Details")]
        public void AddAdditionalInfoManagementFile()
        {
            //Enter to Edit mode of Management File
            managementFilesDetails.EditMgmtFileDetailsBttn();

            //Verify Management file update init form
            managementFilesDetails.VerifyManagementUpdateForm();

            //Add Additional Optional information to the management file
            managementFilesDetails.UpdateManagementFileDetails(managementFile);

            //Save Acquisition File
            managementFilesDetails.SaveManagementFile();

            //Validate View File Details View Mode
            managementFilesDetails.VerifyManagementDetailsViewForm(managementFile);

            //Verify automatic note created when status changes
            if (managementFile.ManagementStatus != "Active")
            {
                notes.NavigateNotesTab();
                notes.VerifyAutomaticNotes("Management File", "Active", managementFile.ManagementStatus);
            }
        }

        [StepDefinition(@"I update the File details from an existing Management File from row number (.*)")]
        public void UpdateFileManagementDetails(int rowNumber)
        {
            PopulateManagementFile(rowNumber);

            //Search for an existing Management File
            searchManagementFiles.NavigateToSearchManagement();
            searchManagementFiles.FilterManagementFiles("", "", "", managementFileCode, "", "", "", "");
            searchManagementFiles.SelectFirstOption();

            //Update existing Acquisition file
            managementFilesDetails.EditMgmtFileDetailsBttn();
            managementFilesDetails.UpdateManagementFileDetails(managementFile);

            //Cancel changes
            managementFilesDetails.CancelManagementFile();

            //Edit Acquisition File
            managementFilesDetails.EditMgmtFileDetailsBttn();
            managementFilesDetails.UpdateManagementFileDetails(managementFile);

            //Save Acquisition File
            managementFilesDetails.SaveManagementFile();

            //Get Acquisition File code
            managementFileCode = managementFilesDetails.GetManagementCode();

            //Validate View File Details View Mode
            managementFilesDetails.VerifyManagementDetailsViewForm(managementFile);

            //Verify automatic note created when
            if (managementFile.ManagementStatus != "Active")
            {
                notes.NavigateNotesTab();
                notes.VerifyAutomaticNotes("Management File", "Hold", managementFile.ManagementStatus);
            }
        }

        [StepDefinition(@"I add Properties to the Management File")]
        public void AddProperties()
        {
            //Navigate to Properties for Management File
            sharedFileProperties.NavigateToAddPropertiesToFile();

            //Navigate to Add Properties by search and verify Add Properties UI/UX
            sharedFileProperties.NavigateToSearchTab();
            sharedFileProperties.VerifySearchPropertiesFeature();

            //Search for a property by PID
            if (managementFile.ManagementSearchProperties.PID != "")
            {
                sharedFileProperties.SelectPropertyByPID(managementFile.ManagementSearchProperties.PID);
                sharedFileProperties.SelectFirstOptionFromSearch();
                sharedFileProperties.ResetSearch();
            }

            //Search for a property by PIN
            if (managementFile.ManagementSearchProperties.PIN != "")
            {
                sharedFileProperties.SelectPropertyByPIN(managementFile.ManagementSearchProperties.PIN);
                sharedFileProperties.SelectFirstOptionFromSearch();
                sharedFileProperties.ResetSearch();
            }

            //Search for a property by Plan
            if (managementFile.ManagementSearchProperties.PlanNumber != "")
            {
                sharedFileProperties.SelectPropertyByPlan(managementFile.ManagementSearchProperties.PlanNumber);
                sharedFileProperties.SelectFirstOptionFromSearch();
                sharedFileProperties.ResetSearch();
            }

            //Search for a property by Address
            if (managementFile.ManagementSearchProperties.Address != "")
            {
                sharedFileProperties.SelectPropertyByAddress(managementFile.ManagementSearchProperties.Address);
                sharedFileProperties.SelectFirstOptionFromSearch();
                sharedFileProperties.ResetSearch();
            }

            //Search for a property by Legal Description
            if (managementFile.ManagementSearchProperties.LegalDescription != "")
            {
                sharedFileProperties.SelectPropertyByLegalDescription(managementFile.ManagementSearchProperties.LegalDescription);
                sharedFileProperties.SelectFirstOptionFromSearch();
                sharedFileProperties.ResetSearch();
            }

            //Search for a property by Latitude and Longitude
            if (managementFile.ManagementSearchProperties.LatitudeLongitude.LatitudeDegree != "")
            {
                sharedFileProperties.SelectPropertyByLongLant(managementFile.ManagementSearchProperties.LatitudeLongitude);
                sharedFileProperties.SelectFirstOptionFromSearch();
                sharedFileProperties.ResetSearch();
            }

            //Search for Multiple PIDs
            if (managementFile.ManagementSearchProperties.MultiplePIDS.First() != "")
            {
                foreach (string prop in managementFile.ManagementSearchProperties.MultiplePIDS)
                {
                    sharedFileProperties.SelectPropertyByPID(prop);
                    sharedFileProperties.SelectFirstOptionFromSearch();
                    sharedFileProperties.ResetSearch();
                }
            }

            //Search for a duplicate property
            if (managementFile.ManagementSearchProperties.PID != "")
            {
                sharedFileProperties.SelectPropertyByPID(managementFile.ManagementSearchProperties.PID);
                sharedFileProperties.SelectFirstOptionFromSearch();
                sharedFileProperties.ResetSearch();
            }

            //Save Research File
            sharedFileProperties.SaveFileProperties();
        }

        [StepDefinition(@"I update a Management File's Properties from row number (.*)")]
        public void UpdateProperties(int rowNumber)
        {
            PopulateManagementFile(rowNumber);

            //Search for an existing Management File
            searchManagementFiles.NavigateToSearchManagement();
            searchManagementFiles.FilterManagementFiles(mgmtfile: managementFileCode);
            searchManagementFiles.SelectFirstOption();

            //Navigate to Edit Management File's Properties
            sharedFileProperties.NavigateToAddPropertiesToFile();

            //Search for a property by Legal Description
            sharedFileProperties.NavigateToSearchTab();
            sharedFileProperties.SelectPropertyByLegalDescription(managementFile.ManagementSearchProperties.LegalDescription);
            sharedFileProperties.SelectFirstOptionFromSearch();

            //Save changes
            sharedFileProperties.SaveFileProperties();

            //Delete Property
            sharedFileProperties.NavigateToAddPropertiesToFile();
            sharedFileProperties.DeleteLastPropertyFromFile();

            //Save Acquisition File changes
            sharedFileProperties.SaveFileProperties();

            //Select 1st Property
            sharedFileProperties.SelectFirstPropertyOptionFromFile();
        }

        [StepDefinition(@"I search for an existing Management File from row number (.*)")]
        public void SearchExistingManagementFile(int rowNumber)
        {
            /* TEST COVERAGE: PSP-4252, PSP-4255, PSP-5589 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Management File Search
            PopulateManagementFile(rowNumber);
            searchManagementFiles.NavigateToSearchManagement();

            //Verify Pagination
            sharedPagination.ChoosePaginationOption(5);
            Assert.Equal(5, searchManagementFiles.MgmtTableResultNumber());

            sharedPagination.ChoosePaginationOption(10);
            Assert.Equal(10, searchManagementFiles.MgmtTableResultNumber());

            sharedPagination.ChoosePaginationOption(20);
            Assert.True(searchManagementFiles.MgmtTableResultNumber() <= 20);

            //sharedPagination.ChoosePaginationOption(50);
            //Assert.Equal(50, searchManagementFiles.MgmtTableResultNumber());

            //sharedPagination.ChoosePaginationOption(100);
            //Assert.Equal(100, searchManagementFiles.MgmtTableResultNumber());

            //Verify Column Sorting by File Name
            searchManagementFiles.OrderByMgmtFileName();
            var firstFileNameDescResult = searchManagementFiles.FirstMgmtFileName();

            searchManagementFiles.OrderByMgmtFileName();
            var firstFileNameAscResult = searchManagementFiles.FirstMgmtFileName();

            Assert.NotEqual(firstFileNameDescResult, firstFileNameAscResult);

            //Verify Column Sorting by Historical File Number
            searchManagementFiles.OrderByMgmtHistoricalFileNbr();
            var firstHistoricalDescResult = searchManagementFiles.FirstMgmtHistoricalFile();

            searchManagementFiles.OrderByMgmtHistoricalFileNbr();
            var firstHistoricalAscResult = searchManagementFiles.FirstMgmtHistoricalFile();

            Assert.NotEqual(firstHistoricalDescResult, firstHistoricalAscResult);

            //Verify Column Sorting by Purpose
            searchManagementFiles.OrderByMgmtPurpose();
            var firstFilePurposeDescResult = searchManagementFiles.FirstMgmtPurpose();

            searchManagementFiles.OrderByMgmtPurpose();
            var firstFilePurposeAscResult = searchManagementFiles.FirstMgmtPurpose();

            Assert.NotEqual(firstFilePurposeDescResult, firstFilePurposeAscResult);

            //Verify Column Sorting by Status
            searchManagementFiles.OrderByMgmtStatus();
            var firstFileStatusDescResult = searchManagementFiles.FirstMgmtStatus();

            searchManagementFiles.OrderByMgmtStatus();
            var firstFileStatusAscResult = searchManagementFiles.FirstMgmtStatus();

            Assert.NotEqual(firstFileStatusDescResult, firstFileStatusAscResult);

            //Verify Pagination display different set of results
            sharedPagination.ResetSearch();
            sharedPagination.ChoosePaginationOption(5);

            var firstAcquisitionPage1 = searchManagementFiles.FirstMgmtFileName();
            sharedPagination.GoNextPage();
            var firstAcquisitionPage2 = searchManagementFiles.FirstMgmtFileName();
            Assert.NotEqual(firstAcquisitionPage1, firstAcquisitionPage2);

            sharedPagination.ResetSearch();

            //Filter Acquisition Files
            searchManagementFiles.FilterManagementFiles(pid: "003-549-551", mgmtfile: "Management from Jonathan Doe", status: "Cancelled");
            Assert.False(searchManagementFiles.SearchFoundResults());

            //Look for the last created Acquisition File
            searchManagementFiles.FilterManagementFiles(mgmtfile: managementFile.ManagementName, status: managementFile.ManagementStatus);
        }

        [StepDefinition(@"I insert activities to the Management Activities Tab")]
        public void InsertActivityTab()
        {
            //Go to the Management Activity Tab
            managementActivities.NavigateActivitiesTab();
            managementActivities.VerifyActivitiesInitListsView();

            //Insert Activities
            for (int j = 0; j < managementFile.ManagementPropertyActivities.Count; j++)
            {
                managementActivities.AddActivityBttn();
                sharedActivities.VerifyCreateActivityInitForm("Management File", managementFile.ManagementPropertyActivities[j].PropertyActivityPropsCount);
                sharedActivities.InsertNewPropertyActivity(managementFile.ManagementPropertyActivities[j]);
                managementActivities.SaveActivity();
                sharedActivities.VerifyInsertedActivity(managementFile.ManagementPropertyActivities[j], "Management File");
                managementActivities.ViewLastActivityFromList();
                managementActivities.VerifyLastInsertedActivityTable(managementFile.ManagementPropertyActivities[j]);
            }

        }

        [StepDefinition(@"I update an activity from the Activities Tab")]
        public void UpdateManagementPropertyTab()
        {
            //Update an activity
            managementActivities.ViewLastActivityFromList();
            managementActivities.ViewLastActivityButton();
            sharedActivities.UpdateSelectedActivityBttn();
            sharedActivities.InsertNewPropertyActivity(managementFile.ManagementPropertyActivities[0]);
            sharedActivities.SaveManagementActivity();
            sharedActivities.VerifyInsertedActivity(managementFile.ManagementPropertyActivities[0], "Management File");
            managementActivities.ViewLastActivityFromList();
            managementActivities.VerifyLastInsertedActivityTable(managementFile.ManagementPropertyActivities[0]);
        }

        [StepDefinition(@"A new Management file is created or updated successfully")]
        public void NewManagementFileCreated()
        {
            searchManagementFiles.NavigateToSearchManagement();
            searchManagementFiles.FilterManagementFiles("", "", "", managementFileCode, "", "", "", "");

            Assert.True(searchManagementFiles.SearchFoundResults());
        }

        [StepDefinition(@"Expected Management File Content is displayed on Management File Table")]
        public void VerifyManagementFileTableContent()
        {
            /* TEST COVERAGE: PSP-4253 */

            //Verify List View
            searchManagementFiles.VerifySearchManagementListView();
            searchManagementFiles.VerifyManagementTableContent(managementFile);
        }

        private void PopulateManagementFile(int rowNumber)
        {
            System.Data.DataTable acquisitionSheet = ExcelDataContext.GetInstance().Sheets["ManagementFiles"]!;
            ExcelDataContext.PopulateInCollection(acquisitionSheet);
            managementFile = new ManagementFile();

            //Management Status
            managementFile.ManagementStatus = ExcelDataContext.ReadData(rowNumber, "ManagementStatus");

            //Project
            managementFile.ManagementMinistryProjectCode = ExcelDataContext.ReadData(rowNumber, "ManagementMinistryProjectCode");
            managementFile.ManagementMinistryProject = ExcelDataContext.ReadData(rowNumber, "ManagementMinistryProject");
            managementFile.ManagementMinistryProduct = ExcelDataContext.ReadData(rowNumber, "ManagementMinistryProduct");
            managementFile.ManagementMinistryFunding = ExcelDataContext.ReadData(rowNumber, "ManagementMinistryFunding");

            //Acquisition Details
            managementFile.ManagementName = ExcelDataContext.ReadData(rowNumber, "ManagementName");
            managementFile.ManagementHistoricalFile = ExcelDataContext.ReadData(rowNumber, "ManagementHistoricalFile");
            managementFile.ManagementPurpose = ExcelDataContext.ReadData(rowNumber, "ManagementPurpose");
            managementFile.ManagementAdditionalDetails = ExcelDataContext.ReadData(rowNumber, "ManagementAdditionalDetails");

            //Acquisition Team
            managementFile.ManagementTeamStartRow = int.Parse(ExcelDataContext.ReadData(rowNumber, "ManagementTeamStartRow"));
            managementFile.ManagementTeamCount = int.Parse(ExcelDataContext.ReadData(rowNumber, "ManagementTeamCount"));

            if (managementFile.ManagementTeamStartRow != 0 && managementFile.ManagementTeamCount != 0)
                PopulateTeamsCollection(managementFile.ManagementTeamStartRow, managementFile.ManagementTeamCount);

            //Properties Search
            managementFile.ManagementSearchPropertiesIndex = int.Parse(ExcelDataContext.ReadData(rowNumber, "ManagementSearchPropertiesIndex"));
            if (managementFile.ManagementSearchPropertiesIndex > 0)
            {
                System.Data.DataTable searchPropertiesSheet = ExcelDataContext.GetInstance().Sheets["SearchProperties"]!;
                ExcelDataContext.PopulateInCollection(searchPropertiesSheet);

                managementFile.ManagementSearchProperties.PID = ExcelDataContext.ReadData(managementFile.ManagementSearchPropertiesIndex, "PID");
                managementFile.ManagementSearchProperties.PIN = ExcelDataContext.ReadData(managementFile.ManagementSearchPropertiesIndex, "PIN");
                managementFile.ManagementSearchProperties.Address = ExcelDataContext.ReadData(managementFile.ManagementSearchPropertiesIndex, "Address");
                managementFile.ManagementSearchProperties.PlanNumber = ExcelDataContext.ReadData(managementFile.ManagementSearchPropertiesIndex, "PlanNumber");
                managementFile.ManagementSearchProperties.LegalDescription = ExcelDataContext.ReadData(managementFile.ManagementSearchPropertiesIndex, "LegalDescription");
                managementFile.ManagementSearchProperties.MultiplePIDS = genericSteps.PopulateLists(ExcelDataContext.ReadData(managementFile.ManagementSearchPropertiesIndex, "MultiplePIDS"));
                managementFile.ManagementSearchProperties.LatitudeLongitude.LatitudeDegree = ExcelDataContext.ReadData(managementFile.ManagementSearchPropertiesIndex, "LatitudeDegree");
                managementFile.ManagementSearchProperties.LatitudeLongitude.LatitudeMinutes = ExcelDataContext.ReadData(managementFile.ManagementSearchPropertiesIndex, "LatitudeMinutes");
                managementFile.ManagementSearchProperties.LatitudeLongitude.LatitudeSeconds = ExcelDataContext.ReadData(managementFile.ManagementSearchPropertiesIndex, "LatitudeSeconds");
                managementFile.ManagementSearchProperties.LatitudeLongitude.LatitudeDirection = ExcelDataContext.ReadData(managementFile.ManagementSearchPropertiesIndex, "LatitudeDirection");
                managementFile.ManagementSearchProperties.LatitudeLongitude.LongitudeDegree = ExcelDataContext.ReadData(managementFile.ManagementSearchPropertiesIndex, "LongitudeDegree");
                managementFile.ManagementSearchProperties.LatitudeLongitude.LongitudeMinutes = ExcelDataContext.ReadData(managementFile.ManagementSearchPropertiesIndex, "LongitudeMinutes");
                managementFile.ManagementSearchProperties.LatitudeLongitude.LongitudeSeconds = ExcelDataContext.ReadData(managementFile.ManagementSearchPropertiesIndex, "LongitudeSeconds");
                managementFile.ManagementSearchProperties.LatitudeLongitude.LongitudeDirection = ExcelDataContext.ReadData(managementFile.ManagementSearchPropertiesIndex, "LongitudeDirection");

            }

            //Management Activities
            managementFile.ManagementActivityStartRow = int.Parse(ExcelDataContext.ReadData(rowNumber, "ManagementActivityStartRow"));
            managementFile.ManagementActivityCount = int.Parse(ExcelDataContext.ReadData(rowNumber, "ManagementActivityCount"));

            if (managementFile.ManagementActivityStartRow != 0 && managementFile.ManagementActivityCount != 0)
                PopulateManagementActivitiesCollection(managementFile.ManagementActivityStartRow, managementFile.ManagementActivityCount);
        }

        private void PopulateTeamsCollection(int startRow, int rowsCount)
        {
            System.Data.DataTable teamsSheet = ExcelDataContext.GetInstance().Sheets["TeamMembers"]!;
            ExcelDataContext.PopulateInCollection(teamsSheet);

            for (int i = startRow; i < startRow + rowsCount; i++)
            {
                TeamMember teamMember = new TeamMember();
                teamMember.TeamMemberRole = ExcelDataContext.ReadData(i, "TeamMemberRole");
                teamMember.TeamMemberContactName = ExcelDataContext.ReadData(i, "TeamMemberContactName");
                teamMember.TeamMemberContactType = ExcelDataContext.ReadData(i, "TeamMemberContactType");
                teamMember.TeamMemberPrimaryContact = ExcelDataContext.ReadData(i, "TeamMemberPrimaryContact");

                managementFile.ManagementTeam.Add(teamMember);
            }
        }

        private void PopulateManagementActivitiesCollection(int startRow, int rowsCount)
        {
            System.Data.DataTable managementActivitesSheet = ExcelDataContext.GetInstance().Sheets["PropertyManagementActivity"]!;
            ExcelDataContext.PopulateInCollection(managementActivitesSheet);

            for (int i = startRow; i < startRow + rowsCount; i++)
            {
                PropertyActivity propertyActivity = new PropertyActivity();

                propertyActivity.PropertyActivityPropsCount = int.Parse(ExcelDataContext.ReadData(i, "PropertyActivityPropsCount"));
                propertyActivity.PropertyActivityType = ExcelDataContext.ReadData(i, "PropertyActivityType");
                propertyActivity.PropertyActivitySubTypeList = genericSteps.PopulateLists(ExcelDataContext.ReadData(i, "PropertyActivitySubType"));
                propertyActivity.PropertyActivityStatus = ExcelDataContext.ReadData(i, "PropertyActivityStatus");
                propertyActivity.PropertyActivityRequestedCommenceDate = ExcelDataContext.ReadData(i, "PropertyActivityRequestedCommenceDate");
                propertyActivity.PropertyActivityCompletionDate = ExcelDataContext.ReadData(i, "PropertyActivityCompletionDate");
                propertyActivity.PropertyActivityDescription = ExcelDataContext.ReadData(i, "PropertyActivityDescription");
                propertyActivity.PropertyActivityMinistryContactList = genericSteps.PopulateLists(ExcelDataContext.ReadData(i, "PropertyActivityMinistryContact"));
                propertyActivity.PropertyActivityRequestorContactMngr = ExcelDataContext.ReadData(i, "PropertyActivityRequestorContactMngr");
                propertyActivity.PropertyActivityInvolvedPartiesExtContactsList = genericSteps.PopulateLists(ExcelDataContext.ReadData(i, "PropertyActivityInvolvedPartiesExtContacts"));
                propertyActivity.PropertyActivityServiceProvider = ExcelDataContext.ReadData(i, "PropertyActivityServiceProvider");
                propertyActivity.ManagementPropertyActivityInvoicesStartRow = int.Parse(ExcelDataContext.ReadData(i, "ManagementPropertyActivityInvoicesStartRow"));
                propertyActivity.ManagementPropertyActivityInvoicesCount = int.Parse(ExcelDataContext.ReadData(i, "ManagementPropertyActivityInvoicesCount"));
                propertyActivity.ManagementPropertyActivityTotalPreTax = ExcelDataContext.ReadData(i, "ManagementPropertyActivityTotalPreTax");
                propertyActivity.ManagementPropertyActivityTotalGST = ExcelDataContext.ReadData(i, "ManagementPropertyActivityTotalGST");
                propertyActivity.ManagementPropertyActivityTotalPST = ExcelDataContext.ReadData(i, "ManagementPropertyActivityTotalPST");
                propertyActivity.ManagementPropertyActivityGrandTotal = ExcelDataContext.ReadData(i, "ManagementPropertyActivityGrandTotal");

                if (propertyActivity.ManagementPropertyActivityInvoicesStartRow != 0 && propertyActivity.ManagementPropertyActivityInvoicesCount != 0)
                    PopulateManagementActivitiesInvoiceCollection(propertyActivity.ManagementPropertyActivityInvoicesStartRow, propertyActivity.ManagementPropertyActivityInvoicesCount, propertyActivity.ManagementPropertyActivityInvoices);
                else
                    propertyActivity.ManagementPropertyActivityInvoices = new List<ManagementPropertyActivityInvoice>();

                managementFile.ManagementPropertyActivities.Add(propertyActivity);
            }
        }

        private void PopulateManagementActivitiesInvoiceCollection(int startRow, int rowsCount, List<ManagementPropertyActivityInvoice> invoices)
        {
            System.Data.DataTable invoicesSheet = ExcelDataContext.GetInstance().Sheets["ManagementPropActivityInvoice"]!;
            ExcelDataContext.PopulateInCollection(invoicesSheet);

            for (int i = startRow; i < startRow + rowsCount; i++)
            {
                ManagementPropertyActivityInvoice invoice = new ManagementPropertyActivityInvoice();

                invoice.PropertyActivityInvoiceNumber = ExcelDataContext.ReadData(i, "PropertyActivityInvoiceNumber");
                invoice.PropertyActivityInvoiceDate = ExcelDataContext.ReadData(i, "PropertyActivityInvoiceDate");
                invoice.PropertyActivityInvoiceDescription = ExcelDataContext.ReadData(i, "PropertyActivityInvoiceDescription");
                invoice.PropertyActivityInvoicePretaxAmount = ExcelDataContext.ReadData(i, "PropertyActivityInvoicePretaxAmount");
                invoice.PropertyActivityInvoiceGSTAmount = ExcelDataContext.ReadData(i, "PropertyActivityInvoiceGSTAmount");
                invoice.PropertyActivityInvoicePSTApplicable = ExcelDataContext.ReadData(i, "PropertyActivityInvoicePSTApplicable");
                invoice.PropertyActivityInvoicePSTAmount = ExcelDataContext.ReadData(i, "PropertyActivityInvoicePSTAmount");
                invoice.PropertyActivityInvoiceTotalAmount = ExcelDataContext.ReadData(i, "PropertyActivityInvoiceTotalAmount");

                invoices.Add(invoice);
            }
        }
    }
}
