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
        private readonly SearchManagement searchManagementFiles = new SearchManagement(driver);
        private readonly SharedFileProperties sharedFileProperties = new SharedFileProperties(driver);
        private readonly SharedPagination sharedPagination = new SharedPagination(driver);
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
            searchManagementFiles.FilterManagementFiles("", "", "", managementFileCode, "", "", "", "");
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

        //[StepDefinition(@"I create an Management File from a pin on map from row number (.*)")]
        //public void CreateManagementFileFromPin(int rowNumber)
        //{
        //    //Login to PIMS
        //    loginSteps.Idir(userName);

        //    //Search for a property
        //    PopulateAcquisitionFile(rowNumber);
        //    searchProperties.SearchPropertyByPID(acquisitionFile.AcquisitionSearchProperties.PID);

        //    //Select Found Pin on map
        //    searchProperties.SelectFoundPin();

        //    //Close Left Side Forms
        //    propertyInformation.HideLeftSideForms();

        //    //Open elipsis option
        //    propertyInformation.OpenMoreOptionsPopUp();
        //    propertyInformation.ChooseCreationOptionFromPin("Acquisition File");

        //    //Open Left Side Forms
        //    propertyInformation.ShowLeftSideForms();

        //    //Validate Acquisition File Details Create Form
        //    acquisitionFilesDetails.VerifyAcquisitionFileCreate("Main");

        //    //Cancel empty acquisition file
        //    acquisitionFilesDetails.CancelAcquisitionFile();

        //    //Verify Form is no longer visible
        //    Assert.Equal(0, acquisitionFilesDetails.IsCreateAcquisitionFileFormVisible());

        //    //Search for a property
        //    searchProperties.SearchPropertyByPID(acquisitionFile.AcquisitionSearchProperties.PID);

        //    //Select Found Pin on map
        //    searchProperties.SelectFoundPin();

        //    //Close Property Information Modal
        //    propertyInformation.HideLeftSideForms();

        //    //Open elipsis option
        //    propertyInformation.OpenMoreOptionsPopUp();
        //    propertyInformation.ChooseCreationOptionFromPin("Acquisition File");

        //    //Open Left Side Forms
        //    propertyInformation.ShowLeftSideForms();

        //    //Fill basic Acquisition File information
        //    acquisitionFilesDetails.CreateMinimumAcquisitionFile(acquisitionFile);

        //    //Cancel Creation
        //    acquisitionFilesDetails.CancelAcquisitionFile();

        //    //Search for a property
        //    searchProperties.SearchPropertyByPID(acquisitionFile.AcquisitionSearchProperties.PID);

        //    //Select Found Pin on map
        //    searchProperties.SelectFoundPin();

        //    //Close Property Information Modal
        //    propertyInformation.HideLeftSideForms();

        //    //Open elipsis option
        //    propertyInformation.OpenMoreOptionsPopUp();
        //    propertyInformation.ChooseCreationOptionFromPin("Acquisition File");

        //    //Open Left Side Forms
        //    propertyInformation.ShowLeftSideForms();

        //    //Fill basic Acquisition File information
        //    acquisitionFilesDetails.CreateMinimumAcquisitionFile(acquisitionFile);

        //    //Save Acquisition File
        //    acquisitionFilesDetails.SaveAcquisitionFileDetails();

        //    //Get Acquisition File code
        //    acquisitionFileCode = acquisitionFilesDetails.GetAcquisitionFileCode();

        //    //Edit Acquisition File
        //    acquisitionFilesDetails.EditAcquisitionFileBttn();

        //    //Add additional information
        //    acquisitionFilesDetails.UpdateAcquisitionFile(acquisitionFile, "Main");

        //    //Save Acquisition File
        //    acquisitionFilesDetails.SaveAcquisitionFileDetails();
        //}

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
            Assert.Equal(20, searchManagementFiles.MgmtTableResultNumber());

            sharedPagination.ChoosePaginationOption(50);
            Assert.Equal(50, searchManagementFiles.MgmtTableResultNumber());

            sharedPagination.ChoosePaginationOption(100);
            Assert.Equal(100, searchManagementFiles.MgmtTableResultNumber());

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

            searchManagementFiles.OrderByMgmtPurpose();
            var firstFileStatusAscResult = searchManagementFiles.FirstMgmtStatus();

            Assert.NotEqual(firstFileStatusDescResult, firstFileStatusAscResult);

            //Verify Pagination display different set of results
            sharedPagination.ResetSearch();

            var firstAcquisitionPage1 = searchManagementFiles.FirstMgmtFileName();
            sharedPagination.GoNextPage();
            var firstAcquisitionPage2 = searchManagementFiles.FirstMgmtFileName();
            Assert.NotEqual(firstAcquisitionPage1, firstAcquisitionPage2);

            sharedPagination.ResetSearch();

            //Filter Acquisition Files
            searchManagementFiles.FilterManagementFiles("003-549-551", "", "", "Management from Jonathan Doe", "", "Cancelled", "", "");
            Assert.False(searchManagementFiles.SearchFoundResults());

            //Look for the last created Acquisition File
            searchManagementFiles.FilterManagementFiles("", "", "", managementFile.ManagementName, "", managementFile.ManagementStatus, "");
        }

        [StepDefinition(@"A new Management file is created successfully")]
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
            }
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
    }
}
