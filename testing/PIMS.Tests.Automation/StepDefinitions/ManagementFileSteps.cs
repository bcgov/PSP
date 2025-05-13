using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;
using PIMS.Tests.Automation.Data;
using PIMS.Tests.Automation.PageObjects;

namespace PIMS.Tests.Automation.StepDefinitions
{
    public class ManagementFileSteps
    {
        private readonly LoginSteps loginSteps;

        private readonly ManagementDetails managementFilesDetails;
        private readonly SearchManagement searchManagement;

        private readonly string userName = "TRANPSP1";

        private ManagementFile managementFile;

        public ManagementFileSteps(IWebDriver driver)
        {
            loginSteps = new LoginSteps(driver);
            managementFilesDetails = new ManagementDetails(driver);
            searchManagement = new SearchManagement(driver);
        }

        [StepDefinition(@"I create a new Management File from row number (.*)")]
        public void CreateManagementFile(int rowNumber)
        {
            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Acquisition File
            PopulateAcquisitionFile(rowNumber);
            acquisitionFilesDetails.NavigateToCreateNewAcquisitionFile();

            //Validate Acquisition File Details Create Form
            acquisitionFilesDetails.VerifyAcquisitionFileCreate("Main");

            //Create basic Acquisition File
            acquisitionFilesDetails.CreateMinimumAcquisitionFile(acquisitionFile);

            //Save Acquisition File
            acquisitionFilesDetails.SaveAcquisitionFileDetails();

            //Get Acquisition File code
            acquisitionFileCode = acquisitionFilesDetails.GetAcquisitionFileCode();
        }

        [StepDefinition(@"I add additional information to the Management File Details")]
        public void AddAdditionalInfoManagementFile()
        {
            /* TEST COVERAGE:  PSP-4469, PSP-4471, PSP-4553, PSP-5308, PSP-5590, PSP-5634, PSP-5637, PSP-5790, PSP-5979, PSP-6041 */

            //Enter to Edit mode of Acquisition File
            acquisitionFilesDetails.EditAcquisitionFileBttn();

            //Verify Maximum fields
            acquisitionFilesDetails.VerifyMaximumFields();

            //Add Additional Optional information to the acquisition file
            acquisitionFilesDetails.UpdateAcquisitionFile(acquisitionFile, "Main");

            //Save Acquisition File
            acquisitionFilesDetails.SaveAcquisitionFileDetails();

            //Validate View File Details View Mode
            acquisitionFilesDetails.VerifyAcquisitionFileView(acquisitionFile, "Main");

            //Verify automatic note created when
            if (acquisitionFile.AcquisitionStatus != "Active")
            {
                notes.NavigateNotesTab();
                notes.VerifyAutomaticNotes("Acquisition File", "Active", acquisitionFile.AcquisitionStatus);
            }
        }

        [StepDefinition(@"I add additional information to complete the Management File")]
        public void AddAdditionalInfoCompleteManagementFile()
        {
            //Go to File Summary
            acquisitionFilesDetails.NavigateToFileSummary();

            //Go to File Details
            acquisitionFilesDetails.NavigateToFileDetailsTab();

            //Enter to Edit mode of Acquisition File
            acquisitionFilesDetails.EditAcquisitionFileBttn();

            //Add Additional Optional information to the acquisition file
            acquisitionFilesDetails.UpdateAcquisitionFile(acquisitionFile, "Main");

            //Save Acquisition File
            acquisitionFilesDetails.SaveAcquisitionFileDetails();
        }

        [StepDefinition(@"I update the File details from an existing Management File from row number (.*)")]
        public void UpdateFileManagementDetails(int rowNumber)
        {
            /* TEST COVERAGE: PSP-4331, PSP-4544, PSP-4545, PSP-5638, PSP-5639, PSP-5970, PSP-5979s */

            PopulateAcquisitionFile(rowNumber);

            //Search for an existing Acquisition File
            searchAcquisitionFiles.NavigateToSearchAcquisitionFile();
            searchAcquisitionFiles.SearchAcquisitionFileByAFile(acquisitionFileCode);
            searchAcquisitionFiles.SelectFirstOption();

            //Update existing Acquisition file
            acquisitionFilesDetails.EditAcquisitionFileBttn();
            acquisitionFilesDetails.UpdateAcquisitionFile(acquisitionFile, "Main");

            //Cancel changes
            acquisitionFilesDetails.CancelAcquisitionFile();

            //Edit Acquisition File
            acquisitionFilesDetails.EditAcquisitionFileBttn();
            acquisitionFilesDetails.UpdateAcquisitionFile(acquisitionFile, "Main");

            //Save Acquisition File
            acquisitionFilesDetails.SaveAcquisitionFileDetails();

            //Get Acquisition File code
            acquisitionFileCode = acquisitionFilesDetails.GetAcquisitionFileCode();

            //Validate View File Details View Mode
            acquisitionFilesDetails.VerifyAcquisitionFileView(acquisitionFile, "Main");

            //Verify automatic note created when
            if (acquisitionFile.AcquisitionStatus != "Active")
            {
                notes.NavigateNotesTab();
                notes.VerifyAutomaticNotes("Acquisition File", "Hold", acquisitionFile.AcquisitionStatus);
            }
        }

        [StepDefinition(@"I add Properties to the Management File")]
        public void AddProperties()
        {
            /* TEST COVERAGE: PSP-4163, PSP-4325, PSP-4326, PSP-4327, PSP-4328, PSP-4329, PSP-4334, PSP-4593, PSP-6268 */

            //Navigate to Properties for Acquisition File
            sharedFileProperties.NavigateToAddPropertiesToFile();

            //Navigate to Add Properties by search and verify Add Properties UI/UX
            sharedFileProperties.NavigateToSearchTab();
            sharedFileProperties.VerifySearchPropertiesFeature();

            //Search for a property by PID
            if (acquisitionFile.AcquisitionSearchProperties.PID != "")
            {
                sharedFileProperties.SelectPropertyByPID(acquisitionFile.AcquisitionSearchProperties.PID);
                sharedFileProperties.SelectFirstOptionFromSearch();
                sharedFileProperties.ResetSearch();
            }

            //Search for a property by PIN
            if (acquisitionFile.AcquisitionSearchProperties.PIN != "")
            {
                sharedFileProperties.SelectPropertyByPIN(acquisitionFile.AcquisitionSearchProperties.PIN);
                sharedFileProperties.SelectFirstOptionFromSearch();
                sharedFileProperties.ResetSearch();
            }

            //Search for a property by Plan
            if (acquisitionFile.AcquisitionSearchProperties.PlanNumber != "")
            {
                sharedFileProperties.SelectPropertyByPlan(acquisitionFile.AcquisitionSearchProperties.PlanNumber);
                sharedFileProperties.SelectFirstOptionFromSearch();
                sharedFileProperties.ResetSearch();
            }

            //Search for a property by Address
            if (acquisitionFile.AcquisitionSearchProperties.Address != "")
            {
                sharedFileProperties.SelectPropertyByAddress(acquisitionFile.AcquisitionSearchProperties.Address);
                sharedFileProperties.SelectFirstOptionFromSearch();
                sharedFileProperties.ResetSearch();
            }

            //Search for a property by Legal Description
            if (acquisitionFile.AcquisitionSearchProperties.LegalDescription != "")
            {
                sharedFileProperties.SelectPropertyByLegalDescription(acquisitionFile.AcquisitionSearchProperties.LegalDescription);
                sharedFileProperties.SelectFirstOptionFromSearch();
                sharedFileProperties.ResetSearch();
            }

            //Search for Multiple PIDs
            if (acquisitionFile.AcquisitionSearchProperties.MultiplePIDS.First() != "")
            {
                foreach (string prop in acquisitionFile.AcquisitionSearchProperties.MultiplePIDS)
                {
                    sharedFileProperties.SelectPropertyByPID(prop);
                    sharedFileProperties.SelectFirstOptionFromSearch();
                    sharedFileProperties.ResetSearch();
                }
            }

            //Search for a duplicate property
            if (acquisitionFile.AcquisitionSearchProperties.PID != "")
            {
                sharedFileProperties.SelectPropertyByPID(acquisitionFile.AcquisitionSearchProperties.PID);
                sharedFileProperties.SelectFirstOptionFromSearch();
                sharedFileProperties.ResetSearch();
            }

            //Save Research File
            sharedFileProperties.SaveFileProperties();
        }

        [StepDefinition(@"I update a Management File's Properties from row number (.*)")]
        public void UpdateProperties(int rowNumber)
        {
            /* TEST COVERAGE:  PSP-4590, PSP-4591, PSP-4600, PSP-4689, PSP-5003, PSP-5006, PSP-5007  */

            PopulateAcquisitionFile(rowNumber);

            //Search for an existing Acquisition File
            searchAcquisitionFiles.NavigateToSearchAcquisitionFile();
            searchAcquisitionFiles.SearchAcquisitionFileByAFile(acquisitionFileCode);
            searchAcquisitionFiles.SelectFirstOption();

            //Navigate to Edit Acquisition File's Properties
            sharedFileProperties.NavigateToAddPropertiesToFile();

            //Search for a property by Legal Description
            sharedFileProperties.NavigateToSearchTab();
            sharedFileProperties.SelectPropertyByLegalDescription(acquisitionFile.AcquisitionSearchProperties.LegalDescription);
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

        [StepDefinition(@"I create an Acquisition File from a pin on map from row number (.*)")]
        public void CreateManagementFileFromPin(int rowNumber)
        {
            /* TEST COVERAGE: PSP-1546, PSP-1556, PSP-4164, PSP-4165, PSP-4167, PSP-4472, PSP-4601, PSP-4704, PSP-5308  */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Search for a property
            PopulateAcquisitionFile(rowNumber);
            searchProperties.SearchPropertyByPID(acquisitionFile.AcquisitionSearchProperties.PID);

            //Select Found Pin on map
            searchProperties.SelectFoundPin();

            //Close Left Side Forms
            propertyInformation.HideLeftSideForms();

            //Open elipsis option
            propertyInformation.OpenMoreOptionsPopUp();
            propertyInformation.ChooseCreationOptionFromPin("Acquisition File");

            //Open Left Side Forms
            propertyInformation.ShowLeftSideForms();

            //Validate Acquisition File Details Create Form
            acquisitionFilesDetails.VerifyAcquisitionFileCreate("Main");

            //Cancel empty acquisition file
            acquisitionFilesDetails.CancelAcquisitionFile();

            //Verify Form is no longer visible
            Assert.Equal(0, acquisitionFilesDetails.IsCreateAcquisitionFileFormVisible());

            //Search for a property
            searchProperties.SearchPropertyByPID(acquisitionFile.AcquisitionSearchProperties.PID);

            //Select Found Pin on map
            searchProperties.SelectFoundPin();

            //Close Property Information Modal
            propertyInformation.HideLeftSideForms();

            //Open elipsis option
            propertyInformation.OpenMoreOptionsPopUp();
            propertyInformation.ChooseCreationOptionFromPin("Acquisition File");

            //Open Left Side Forms
            propertyInformation.ShowLeftSideForms();

            //Fill basic Acquisition File information
            acquisitionFilesDetails.CreateMinimumAcquisitionFile(acquisitionFile);

            //Cancel Creation
            acquisitionFilesDetails.CancelAcquisitionFile();

            //Search for a property
            searchProperties.SearchPropertyByPID(acquisitionFile.AcquisitionSearchProperties.PID);

            //Select Found Pin on map
            searchProperties.SelectFoundPin();

            //Close Property Information Modal
            propertyInformation.HideLeftSideForms();

            //Open elipsis option
            propertyInformation.OpenMoreOptionsPopUp();
            propertyInformation.ChooseCreationOptionFromPin("Acquisition File");

            //Open Left Side Forms
            propertyInformation.ShowLeftSideForms();

            //Fill basic Acquisition File information
            acquisitionFilesDetails.CreateMinimumAcquisitionFile(acquisitionFile);

            //Save Acquisition File
            acquisitionFilesDetails.SaveAcquisitionFileDetails();

            //Get Acquisition File code
            acquisitionFileCode = acquisitionFilesDetails.GetAcquisitionFileCode();

            //Edit Acquisition File
            acquisitionFilesDetails.EditAcquisitionFileBttn();

            //Add additional information
            acquisitionFilesDetails.UpdateAcquisitionFile(acquisitionFile, "Main");

            //Save Acquisition File
            acquisitionFilesDetails.SaveAcquisitionFileDetails();
        }

        [StepDefinition(@"I search for an existing Management File from row number (.*)")]
        public void SearchExistingManagementFile(int rowNumber)
        {
            /* TEST COVERAGE: PSP-4252, PSP-4255, PSP-5589 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Acquisition File Search
            PopulateAcquisitionFile(rowNumber);
            searchAcquisitionFiles.NavigateToSearchAcquisitionFile();

            //Verify Pagination
            sharedPagination.ChoosePaginationOption(5);
            Assert.Equal(5, searchAcquisitionFiles.AcquisitionFileTableResultNumber());

            sharedPagination.ChoosePaginationOption(10);
            Assert.Equal(10, searchAcquisitionFiles.AcquisitionFileTableResultNumber());

            sharedPagination.ChoosePaginationOption(20);
            Assert.Equal(20, searchAcquisitionFiles.AcquisitionFileTableResultNumber());

            sharedPagination.ChoosePaginationOption(50);
            Assert.Equal(50, searchAcquisitionFiles.AcquisitionFileTableResultNumber());

            sharedPagination.ChoosePaginationOption(100);
            Assert.Equal(100, searchAcquisitionFiles.AcquisitionFileTableResultNumber());

            //Verify Column Sorting by File Number
            searchAcquisitionFiles.OrderByAcquisitionFileNumber();
            var firstFileNbrDescResult = searchAcquisitionFiles.FirstAcquisitionFileNumber();

            searchAcquisitionFiles.OrderByAcquisitionFileNumber();
            var firstFileNbrAscResult = searchAcquisitionFiles.FirstAcquisitionFileNumber();

            Assert.NotEqual(firstFileNbrDescResult, firstFileNbrAscResult);

            //Verify Column Sorting by Historical File Number
            searchAcquisitionFiles.OrderByAcquisitionFileHistoricalNumber();
            var firstHistoricalDescResult = searchAcquisitionFiles.FirstAcquisitionLegacyFileNumber();

            searchAcquisitionFiles.OrderByAcquisitionFileHistoricalNumber();
            var firstHistoricalAscResult = searchAcquisitionFiles.FirstAcquisitionLegacyFileNumber();

            Assert.NotEqual(firstHistoricalDescResult, firstHistoricalAscResult);

            //Verify Column Sorting by File Name
            searchAcquisitionFiles.OrderByAcquisitionFileName();
            var firstFileNameDescResult = searchAcquisitionFiles.FirstAcquisitionFileName();

            searchAcquisitionFiles.OrderByAcquisitionFileName();
            var firstFileNameAscResult = searchAcquisitionFiles.FirstAcquisitionFileName();

            Assert.NotEqual(firstFileNameDescResult, firstFileNameAscResult);

            //Verify Pagination display different set of results
            sharedPagination.ResetSearch();

            var firstAcquisitionPage1 = searchAcquisitionFiles.FirstAcquisitionFileNumber();
            sharedPagination.GoNextPage();
            var firstAcquisitionPage2 = searchAcquisitionFiles.FirstAcquisitionFileNumber();
            Assert.NotEqual(firstAcquisitionPage1, firstAcquisitionPage2);

            sharedPagination.ResetSearch();

            //Filter Acquisition Files
            searchAcquisitionFiles.FilterAcquisitionFiles("003-549-551", "", "", "Acquisition from Jonathan Doe", "", "Cancelled", "");
            Assert.False(searchAcquisitionFiles.SearchFoundResults());

            //Look for the last created Acquisition File
            searchAcquisitionFiles.FilterAcquisitionFiles("", "", "", acquisitionFile.AcquisitionFileName, "", acquisitionFile.AcquisitionStatus, "");
        }

        [StepDefinition(@"A new Management file is created successfully")]
        public void NewManagementFileCreated()
        {
            searchAcquisitionFiles.NavigateToSearchAcquisitionFile();
            searchAcquisitionFiles.SearchAcquisitionFileByAFile(acquisitionFileCode);

            Assert.True(searchAcquisitionFiles.SearchFoundResults());
        }

        [StepDefinition(@"An existing Management file has been edited successfully")]
        public void EditManagementFileSuccess()
        {
            acquisitionFilesDetails.NavigateToFileDetailsTab();
            acquisitionFilesDetails.VerifyAcquisitionFileView(acquisitionFile, "Main");
        }

        [StepDefinition(@"Expected Management File Content is displayed on Management File Table")]
        public void VerifyManagementFileTableContent()
        {
            /* TEST COVERAGE: PSP-4253 */

            //Verify List View
            searchAcquisitionFiles.VerifyAcquisitionFileListView();
            searchAcquisitionFiles.VerifyAcquisitionFileTableContent(acquisitionFile);
        }

        private void PopulateManagementFile(int rowNumber)
        {
            System.Data.DataTable acquisitionSheet = ExcelDataContext.GetInstance().Sheets["AcquisitionFiles"]!;
            ExcelDataContext.PopulateInCollection(acquisitionSheet);
            acquisitionFile = new AcquisitionFile();

            //Acquisition Status
            acquisitionFile.AcquisitionStatus = ExcelDataContext.ReadData(rowNumber, "AcquisitionStatus");

            //Project
            acquisitionFile.AcquisitionProject = ExcelDataContext.ReadData(rowNumber, "AcquisitionProject");
            acquisitionFile.AcquisitionProjCode = ExcelDataContext.ReadData(rowNumber, "AcquisitionProjCode");
            acquisitionFile.AcquisitionProjProductCode = ExcelDataContext.ReadData(rowNumber, "AcquisitionProjProductCode");
            acquisitionFile.AcquisitionProjProduct = ExcelDataContext.ReadData(rowNumber, "AcquisitionProjProduct");
            acquisitionFile.AcquisitionProjFunding = ExcelDataContext.ReadData(rowNumber, "AcquisitionProjFunding");
            acquisitionFile.AcquisitionFundingOther = ExcelDataContext.ReadData(rowNumber, "AcquisitionFundingOther");

            //Progress Statuses
            acquisitionFile.AcquisitionFileProgressStatuses = genericSteps.PopulateLists(ExcelDataContext.ReadData(rowNumber, "AcquisitionFileProgressStatuses"));
            acquisitionFile.AcquisitionAppraisalStatus = ExcelDataContext.ReadData(rowNumber, "AcquisitionAppraisalStatus");
            acquisitionFile.AcquisitionLegalSurveyStatus = ExcelDataContext.ReadData(rowNumber, "AcquisitionLegalSurveyStatus");
            acquisitionFile.AcquisitionTypeTakingStatuses = genericSteps.PopulateLists(ExcelDataContext.ReadData(rowNumber, "AcquisitionTypeTakingStatuses"));
            acquisitionFile.AcquisitionExpropriationRiskStatus = ExcelDataContext.ReadData(rowNumber, "AcquisitionExpropriationRiskStatus");

            //Schedule
            acquisitionFile.AcquisitionAssignedDate = ExcelDataContext.ReadData(rowNumber, "AcquisitionAssignedDate");
            acquisitionFile.AcquisitionDeliveryDate = ExcelDataContext.ReadData(rowNumber, "AcquisitionDeliveryDate");
            acquisitionFile.AcquisitionEstimatedDate = ExcelDataContext.ReadData(rowNumber, "AcquisitionEstimatedDate");
            acquisitionFile.AcquisitionPossesionDate = ExcelDataContext.ReadData(rowNumber, "AcquisitionPossesionDate");

            //Acquisition Details
            acquisitionFile.AcquisitionFileName = ExcelDataContext.ReadData(rowNumber, "AcquisitionFileName");
            acquisitionFile.HistoricalFileNumber = ExcelDataContext.ReadData(rowNumber, "HistoricalFileNumber");
            acquisitionFile.PhysicalFileStatus = ExcelDataContext.ReadData(rowNumber, "PhysicalFileStatus");
            acquisitionFile.AcquisitionType = ExcelDataContext.ReadData(rowNumber, "AcquisitionType");
            acquisitionFile.AcquisitionSubfileInterest = ExcelDataContext.ReadData(rowNumber, "AcquisitionSubfileInterest");
            acquisitionFile.AcquisitionSubfileInterestOther = ExcelDataContext.ReadData(rowNumber, "AcquisitionSubfileInterestOther");
            acquisitionFile.AcquisitionMOTIRegion = ExcelDataContext.ReadData(rowNumber, "AcquisitionMOTIRegion");

            acquisitionFile.AcquisitionTeamStartRow = int.Parse(ExcelDataContext.ReadData(rowNumber, "AcquisitionTeamStartRow"));
            acquisitionFile.AcquisitionTeamCount = int.Parse(ExcelDataContext.ReadData(rowNumber, "AcquisitionTeamCount"));

            //Acquisition Team
            if (acquisitionFile.AcquisitionTeamStartRow != 0 && acquisitionFile.AcquisitionTeamCount != 0)
                PopulateTeamsCollection(acquisitionFile.AcquisitionTeamStartRow, acquisitionFile.AcquisitionTeamCount);


            //Owner
            acquisitionFile.OwnerStartRow = int.Parse(ExcelDataContext.ReadData(rowNumber, "OwnerStartRow"));
            acquisitionFile.OwnerCount = int.Parse(ExcelDataContext.ReadData(rowNumber, "OwnerCount"));
            if (acquisitionFile.OwnerStartRow != 0 && acquisitionFile.OwnerCount != 0)
                PopulateOwnersCollection(acquisitionFile.OwnerStartRow, acquisitionFile.OwnerCount);

            acquisitionFile.OwnerSolicitor = ExcelDataContext.ReadData(rowNumber, "OwnerSolicitor");
            acquisitionFile.OwnerRepresentative = ExcelDataContext.ReadData(rowNumber, "OwnerRepresentative");
            acquisitionFile.OwnerComment = ExcelDataContext.ReadData(rowNumber, "OwnerComment");

            //Properties Search
            acquisitionFile.AcquisitionSearchPropertiesIndex = int.Parse(ExcelDataContext.ReadData(rowNumber, "AcqSearchPropertiesIndex"));
            if (acquisitionFile.AcquisitionSearchPropertiesIndex > 0)
            {
                System.Data.DataTable searchPropertiesSheet = ExcelDataContext.GetInstance().Sheets["SearchProperties"]!;
                ExcelDataContext.PopulateInCollection(searchPropertiesSheet);

                acquisitionFile.AcquisitionSearchProperties.PID = ExcelDataContext.ReadData(acquisitionFile.AcquisitionSearchPropertiesIndex, "PID");
                acquisitionFile.AcquisitionSearchProperties.PIN = ExcelDataContext.ReadData(acquisitionFile.AcquisitionSearchPropertiesIndex, "PIN");
                acquisitionFile.AcquisitionSearchProperties.Address = ExcelDataContext.ReadData(acquisitionFile.AcquisitionSearchPropertiesIndex, "Address");
                acquisitionFile.AcquisitionSearchProperties.PlanNumber = ExcelDataContext.ReadData(acquisitionFile.AcquisitionSearchPropertiesIndex, "PlanNumber");
                acquisitionFile.AcquisitionSearchProperties.LegalDescription = ExcelDataContext.ReadData(acquisitionFile.AcquisitionSearchPropertiesIndex, "LegalDescription");
                acquisitionFile.AcquisitionSearchProperties.MultiplePIDS = genericSteps.PopulateLists(ExcelDataContext.ReadData(acquisitionFile.AcquisitionSearchPropertiesIndex, "MultiplePIDS"));
            }

            //Acquisition's Properties' Takes
            acquisitionFile.TakesStartRow = int.Parse(ExcelDataContext.ReadData(rowNumber, "TakesStartRow"));
            acquisitionFile.TakesCount = int.Parse(ExcelDataContext.ReadData(rowNumber, "TakesCount"));
            if (acquisitionFile.TakesStartRow != 0 && acquisitionFile.TakesCount != 0)
                PopulateTakesCollection(acquisitionFile.TakesStartRow, acquisitionFile.TakesCount);

            //Acquisition File Checklist
            acquisitionFile.AcquisitionFileChecklistIndex = int.Parse(ExcelDataContext.ReadData(rowNumber, "AcquisitionFileChecklistIndex"));
            if (acquisitionFile.AcquisitionFileChecklistIndex > 0)
            {
                System.Data.DataTable acquisitionFileChecklistSheet = ExcelDataContext.GetInstance().Sheets["AcquisitionChecklist"]!;
                ExcelDataContext.PopulateInCollection(acquisitionFileChecklistSheet);

                acquisitionFile.AcquisitionFileChecklist.FileInitiationSelect1 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqFileInitiationSelect1");
                acquisitionFile.AcquisitionFileChecklist.FileInitiationSelect2 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqFileInitiationSelect2");
                acquisitionFile.AcquisitionFileChecklist.FileInitiationSelect3 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqFileInitiationSelect3");
                acquisitionFile.AcquisitionFileChecklist.FileInitiationSelect4 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqFileInitiationSelect4");
                acquisitionFile.AcquisitionFileChecklist.FileInitiationSelect5 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqFileInitiationSelect5");

                acquisitionFile.AcquisitionFileChecklist.ActiveFileManagementSelect1 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqActiveFileManagementSelect1");
                acquisitionFile.AcquisitionFileChecklist.ActiveFileManagementSelect2 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqActiveFileManagementSelect2");
                acquisitionFile.AcquisitionFileChecklist.ActiveFileManagementSelect3 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqActiveFileManagementSelect3");
                acquisitionFile.AcquisitionFileChecklist.ActiveFileManagementSelect4 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqActiveFileManagementSelect4");
                acquisitionFile.AcquisitionFileChecklist.ActiveFileManagementSelect5 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqActiveFileManagementSelect5");
                acquisitionFile.AcquisitionFileChecklist.ActiveFileManagementSelect6 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqActiveFileManagementSelect6");
                acquisitionFile.AcquisitionFileChecklist.ActiveFileManagementSelect7 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqActiveFileManagementSelect7");
                acquisitionFile.AcquisitionFileChecklist.ActiveFileManagementSelect8 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqActiveFileManagementSelect8");
                acquisitionFile.AcquisitionFileChecklist.ActiveFileManagementSelect9 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqActiveFileManagementSelect9");
                acquisitionFile.AcquisitionFileChecklist.ActiveFileManagementSelect10 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqActiveFileManagementSelect10");
                acquisitionFile.AcquisitionFileChecklist.ActiveFileManagementSelect11 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqActiveFileManagementSelect11");
                acquisitionFile.AcquisitionFileChecklist.ActiveFileManagementSelect12 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqActiveFileManagementSelect12");
                acquisitionFile.AcquisitionFileChecklist.ActiveFileManagementSelect13 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqActiveFileManagementSelect13");
                acquisitionFile.AcquisitionFileChecklist.ActiveFileManagementSelect14 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqActiveFileManagementSelect14");
                acquisitionFile.AcquisitionFileChecklist.ActiveFileManagementSelect15 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqActiveFileManagementSelect15");
                acquisitionFile.AcquisitionFileChecklist.ActiveFileManagementSelect16 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqActiveFileManagementSelect16");
                acquisitionFile.AcquisitionFileChecklist.ActiveFileManagementSelect17 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqActiveFileManagementSelect17");

                acquisitionFile.AcquisitionFileChecklist.CrownLandSelect1 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqCrownLandSelect1");
                acquisitionFile.AcquisitionFileChecklist.CrownLandSelect2 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqCrownLandSelect2");
                acquisitionFile.AcquisitionFileChecklist.CrownLandSelect3 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqCrownLandSelect3");

                acquisitionFile.AcquisitionFileChecklist.Section3AgreementSelect1 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqSection3AgreementSelect1");
                acquisitionFile.AcquisitionFileChecklist.Section3AgreementSelect2 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqSection3AgreementSelect2");
                acquisitionFile.AcquisitionFileChecklist.Section3AgreementSelect3 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqSection3AgreementSelect3");
                acquisitionFile.AcquisitionFileChecklist.Section3AgreementSelect4 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqSection3AgreementSelect4");
                acquisitionFile.AcquisitionFileChecklist.Section3AgreementSelect5 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqSection3AgreementSelect5");
                acquisitionFile.AcquisitionFileChecklist.Section3AgreementSelect6 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqSection3AgreementSelect6");
                acquisitionFile.AcquisitionFileChecklist.Section3AgreementSelect7 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqSection3AgreementSelect7");
                acquisitionFile.AcquisitionFileChecklist.Section3AgreementSelect8 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqSection3AgreementSelect8");
                acquisitionFile.AcquisitionFileChecklist.Section3AgreementSelect9 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqSection3AgreementSelect9");

                acquisitionFile.AcquisitionFileChecklist.Section6ExpropriationSelect1 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqSection6ExpropriationSelect1");
                acquisitionFile.AcquisitionFileChecklist.Section6ExpropriationSelect2 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqSection6ExpropriationSelect2");
                acquisitionFile.AcquisitionFileChecklist.Section6ExpropriationSelect3 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqSection6ExpropriationSelect3");
                acquisitionFile.AcquisitionFileChecklist.Section6ExpropriationSelect4 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqSection6ExpropriationSelect4");
                acquisitionFile.AcquisitionFileChecklist.Section6ExpropriationSelect5 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqSection6ExpropriationSelect5");
                acquisitionFile.AcquisitionFileChecklist.Section6ExpropriationSelect6 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqSection6ExpropriationSelect6");
                acquisitionFile.AcquisitionFileChecklist.Section6ExpropriationSelect7 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqSection6ExpropriationSelect7");
                acquisitionFile.AcquisitionFileChecklist.Section6ExpropriationSelect8 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqSection6ExpropriationSelect8");
                acquisitionFile.AcquisitionFileChecklist.Section6ExpropriationSelect9 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqSection6ExpropriationSelect9");
                acquisitionFile.AcquisitionFileChecklist.Section6ExpropriationSelect10 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqSection6ExpropriationSelect10");
                acquisitionFile.AcquisitionFileChecklist.Section6ExpropriationSelect11 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqSection6ExpropriationSelect11");
                acquisitionFile.AcquisitionFileChecklist.Section6ExpropriationSelect12 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcqSection6ExpropriationSelect12");

                acquisitionFile.AcquisitionFileChecklist.AcquisitionCompletionSelect1 = ExcelDataContext.ReadData(acquisitionFile.AcquisitionFileChecklistIndex, "AcquisitionCompletionSelect1");
            }

            //Acquisition Agreements
            acquisitionFile.AgreementStartRow = int.Parse(ExcelDataContext.ReadData(rowNumber, "AgreementStartRow"));
            acquisitionFile.AgreementCount = int.Parse(ExcelDataContext.ReadData(rowNumber, "AgreementCount"));
            if (acquisitionFile.AgreementStartRow != 0 && acquisitionFile.AgreementCount != 0)
                PopulateAgreementsCollection(acquisitionFile.AgreementStartRow, acquisitionFile.AgreementCount);

            //Acquisition Stakeholders
            acquisitionFile.StakeholderStartRow = int.Parse(ExcelDataContext.ReadData(rowNumber, "StakeholderStartRow"));
            acquisitionFile.StakeholderCount = int.Parse(ExcelDataContext.ReadData(rowNumber, "StakeholderCount"));
            if (acquisitionFile.StakeholderStartRow != 0 && acquisitionFile.StakeholderCount != 0)
                PopulateStakeholdersCollection(acquisitionFile.StakeholderStartRow, acquisitionFile.StakeholderCount);

            //Acquisition Compensation Requisition
            acquisitionFile.AcquisitionCompensationStartRow = int.Parse(ExcelDataContext.ReadData(rowNumber, "AcquisitionCompensationStartRow"));
            acquisitionFile.AcquisitionCompensationCount = int.Parse(ExcelDataContext.ReadData(rowNumber, "AcquisitionCompensationCount"));
            acquisitionFile.AcquisitionCompensationTotalAllowableAmount = ExcelDataContext.ReadData(rowNumber, "AcquisitionCompensationTotalAllowableAmount");
            acquisitionFile.AcquisitionCompensationMainFileTotal = ExcelDataContext.ReadData(rowNumber, "AcquisitionCompensationMainFileTotal");
            acquisitionFile.AcquisitionCompensationSubfilesMainFileTotal = ExcelDataContext.ReadData(rowNumber, "AcquisitionCompensationSubfilesMainFileTotal");
            acquisitionFile.AcquisitionCompensationDraftTotal = ExcelDataContext.ReadData(rowNumber, "AcquisitionCompensationDraftTotal");
            if (acquisitionFile.AcquisitionCompensationStartRow != 0 && acquisitionFile.AcquisitionCompensationCount != 0)
                PopulateCompensationsCollection(acquisitionFile.AcquisitionCompensationStartRow, acquisitionFile.AcquisitionCompensationCount);

            //Acquisition Expropriation
            acquisitionFile.ExpropriationStartRow = int.Parse(ExcelDataContext.ReadData(rowNumber, "ExpropriationStartRow"));
            acquisitionFile.ExpropriationCount = int.Parse(ExcelDataContext.ReadData(rowNumber, "ExpropriationCount"));
            if (acquisitionFile.ExpropriationStartRow != 0 && acquisitionFile.ExpropriationCount != 0)
                PopulateExpropriationCollection(acquisitionFile.ExpropriationStartRow, acquisitionFile.ExpropriationCount);
        }
    }
}
