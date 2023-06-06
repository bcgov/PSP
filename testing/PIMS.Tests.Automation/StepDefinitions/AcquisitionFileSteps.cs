

using PIMS.Tests.Automation.Classes;
using PIMS.Tests.Automation.Data;
using PIMS.Tests.Automation.PageObjects;
using System.Data;

namespace PIMS.Tests.Automation.StepDefinitions
{
    [Binding]
    public class AcquisitionFileSteps
    {
        private readonly LoginSteps loginSteps;
        private readonly AcquisitionFiles acquisitionFiles;
        private readonly SearchAcquisitionFiles searchAcquisitionFiles;
        private readonly SharedSearchProperties sharedSearchProperties;
        private readonly SearchProperties searchProperties;
        private readonly PropertyInformation propertyInformation;
        private readonly Notes notes;

        private readonly string userName = "TRANPSP1";
        //private readonly string userName = "sutairak";

        private AcquisitionFile acquisitionFile;
        protected string acquisitionFileCode = "";

        public AcquisitionFileSteps(BrowserDriver driver)
        {
            loginSteps = new LoginSteps(driver);
            acquisitionFiles = new AcquisitionFiles(driver.Current);
            searchAcquisitionFiles = new SearchAcquisitionFiles(driver.Current);
            sharedSearchProperties = new SharedSearchProperties(driver.Current);
            searchProperties = new SearchProperties(driver.Current);
            propertyInformation = new PropertyInformation(driver.Current);
            acquisitionFile = new AcquisitionFile();
        }

        [StepDefinition(@"I create a new Acquisition File from row number (.*)")]
        public void CreateAcquisitionFile(int rowNumber)
        {
            /* TEST COVERAGE: PSP-4163, PSP-4164, PSP-4323, PSP-4553  */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Acquisition File
            PopulateAcquisitionFile(rowNumber);
            acquisitionFiles.NavigateToCreateNewAcquisitionFile();

            //Validate Acquisition File Details Create Form
            acquisitionFiles.VerifyAcquisitionFileCreate();

            //Create basic Acquisition File
            acquisitionFiles.CreateMinimumAcquisitionFile(acquisitionFile);

            //Save Acquisition File
            acquisitionFiles.SaveAcquisitionFile();

            //Get Research File code
            acquisitionFileCode = acquisitionFiles.GetAcquisitionFileCode();
        }

        [StepDefinition(@"I add additional information to the Acquisition File")]
        public void AddAdditionalInfoAcquisitionFile()
        {
            /* TEST COVERAGE: PSP-4163, PSP-4323, PSP-4471, PSP-4553, PSP-4331, PSP-4469, PSP-5308, PSP-4327, PSP-4163, PSP-4593, PSP-4334, PSP-4329, PSP-4328, PSP-4327, PSP-4326, PSP-4325 */

            //Enter to Edit mode of Acquisition File
            acquisitionFiles.EditAcquisitionFileDetails();

            //Add Additional Optional information to the acquisition file
            acquisitionFiles.AddAdditionalInformation(acquisitionFile);

            //Save Research File
            acquisitionFiles.SaveAcquisitionFile();

            //Validate View File Details View Mode
            acquisitionFiles.VerifyAcquisitionFileView(acquisitionFile);

            //Navigate to Edit Research File
            acquisitionFiles.NavigateToAddPropertiesAcquisitionFile();

            //Navigate to Add Properties by search and verify Add Properties UI/UX
            sharedSearchProperties.NavigateToSearchTab();
            sharedSearchProperties.VerifySearchPropertiesFeature();

            //Search for a property by PID
            if (acquisitionFile.SearchProperties.PID != "")
            {
                sharedSearchProperties.SelectPropertyByPID(acquisitionFile.SearchProperties.PID);
                sharedSearchProperties.SelectFirstOption();
            }

            //Search for a property by PIN
            if (acquisitionFile.SearchProperties.PIN != "")
            {
                sharedSearchProperties.SelectPropertyByPIN(acquisitionFile.SearchProperties.PIN);
                sharedSearchProperties.SelectFirstOption();
            }

            //Search for a property by Plan
            if (acquisitionFile.SearchProperties.PlanNumber != "")
            {
                sharedSearchProperties.SelectPropertyByPlan(acquisitionFile.SearchProperties.PlanNumber);
                sharedSearchProperties.SelectFirstOption();
            }

            //Search for a property by Address
            if (acquisitionFile.SearchProperties.Address != "")
            {
                sharedSearchProperties.SelectPropertyByAddress(acquisitionFile.SearchProperties.Address);
                sharedSearchProperties.SelectFirstOption();
            }

            //Search for a property by Legal Description
            if (acquisitionFile.SearchProperties.LegalDescription != "")
            {
                sharedSearchProperties.SelectPropertyByLegalDescription(acquisitionFile.SearchProperties.LegalDescription);
                sharedSearchProperties.SelectFirstOption();
            }

            //Search for a duplicate property
            if (acquisitionFile.SearchProperties.PID != "")
            {
                sharedSearchProperties.SelectPropertyByPID(acquisitionFile.SearchProperties.PID);
                sharedSearchProperties.SelectFirstOption();
            }

            //Save Research File
            acquisitionFiles.SaveAcquisitionFileProperties();
        }

        [StepDefinition(@"I create an Acquisition File from a pin on map from row number (.*)")]
        public void CreateAcquisitionFileFromPin(int rowNumber)
        {
            /* TEST COVERAGE: PSP-1546, PSP-1556, PSP-4164, PSP-4167, PSP-4601, PSP-4704, PSP-5308  */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Search for a property
            PopulateAcquisitionFile(rowNumber);
            searchProperties.SearchPropertyByPINPID(acquisitionFile.SearchProperties.PID);

            //Select Found Pin on map
            searchProperties.SelectFoundPin();

            //Close Property Information Modal
            propertyInformation.ClosePropertyInfoModal();

            //Open elipsis option
            propertyInformation.OpenMoreOptionsPopUp();
            propertyInformation.ChooseCreationOptionFromPin("Acquisition File - Create new");

            //Validate Acquisition File Details Create Form
            acquisitionFiles.VerifyAcquisitionFileCreate();

            //Cancel empty acquisition file
            acquisitionFiles.CancelAcquisitionFile();

            //Verify Form is no longer visible
            Assert.True(acquisitionFiles.IsCreateAcquisitionFileFormVisible() == 0);

            //Search for a property
            searchProperties.SearchPropertyByPINPID(acquisitionFile.SearchProperties.PID);

            //Select Found Pin on map
            searchProperties.SelectFoundPin();

            //Close Property Information Modal
            propertyInformation.ClosePropertyInfoModal();

            //Open elipsis option
            propertyInformation.OpenMoreOptionsPopUp();
            propertyInformation.ChooseCreationOptionFromPin("Acquisition File - Create new");

            //Fill basic Acquisition File information
            acquisitionFiles.CreateMinimumAcquisitionFile(acquisitionFile);

            //Cancel Creation
            acquisitionFiles.CancelAcquisitionFile();

            //Search for a property
            searchProperties.SearchPropertyByPINPID(acquisitionFile.SearchProperties.PID);

            //Select Found Pin on map
            searchProperties.SelectFoundPin();

            //Close Property Information Modal
            propertyInformation.ClosePropertyInfoModal();

            //Open elipsis option
            propertyInformation.OpenMoreOptionsPopUp();
            propertyInformation.ChooseCreationOptionFromPin("Acquisition File - Create new");

            //Fill basic Acquisition File information
            acquisitionFiles.CreateMinimumAcquisitionFile(acquisitionFile);

            //Save Acquisition File
            acquisitionFiles.SaveAcquisitionFile();

            //Get Research File code
            acquisitionFileCode = acquisitionFiles.GetAcquisitionFileCode();

            //Edit Acquisition File
            acquisitionFiles.EditAcquisitionFileBttn();

            //Add additional information
            acquisitionFiles.AddAdditionalInformation(acquisitionFile);

            //Save Acquisition File
            acquisitionFiles.SaveAcquisitionFile();
        }

        [StepDefinition(@"I search for an existing acquisition file")]
        public void SearchLastCreatedAcquisitionFile()
        {
            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Manage Acquisition Files
            searchAcquisitionFiles.NavigateToSearchAcquisitionFile();

            //Look for the last acquisition file
            searchAcquisitionFiles.SearchLastAcquisitionFile();

            //Select 1st option from search
            searchAcquisitionFiles.SelectFirstOption();
        }

        [StepDefinition(@"I edit an existing Acquisition File from row number (.*)")]
        public void EditAcquisitionFile(int rowNumber)
        {
            /* TEST COVERAGE: PSP-4544, PSP-4545, PSP-4590, PSP-4591, PSP-4600, PSP-4689, PSP-5003, PSP-5006, PSP-5007  */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Acquisition File Search
            PopulateAcquisitionFile(rowNumber);
            searchAcquisitionFiles.NavigateToSearchAcquisitionFile();

            //Filter research Files
            searchAcquisitionFiles.FilterAcquisitionFiles(acquisitionFile.SearchProperties.PID, acquisitionFile.AcquisitionFileName, acquisitionFile.AcquisitionStatus);

            //Look for the last created research file
            searchAcquisitionFiles.SearchLastAcquisitionFile();

            //Choose first found option
            searchAcquisitionFiles.SelectFirstOption();

            //Edit Acquisition File
            acquisitionFiles.EditAcquisitionFileBttn();

            //Update Acquisition File main form
            acquisitionFiles.UpdateAcquisitionFile(acquisitionFile);

            //Cancel changes
            acquisitionFiles.CancelAcquisitionFile();

            //Edit Acquisition File
            acquisitionFiles.EditAcquisitionFileBttn();

            //Update Acquisition File main form
            acquisitionFiles.UpdateAcquisitionFile(acquisitionFile);

            //Save Staff changes
            acquisitionFiles.SaveAcquisitionFile();

            //Navigate to Edit Research File
            acquisitionFiles.NavigateToAddPropertiesAcquisitionFile();

            //Search for a property by Legal Description
            sharedSearchProperties.NavigateToSearchTab();
            sharedSearchProperties.SelectPropertyByLegalDescription(acquisitionFile.SearchProperties.LegalDescription);
            sharedSearchProperties.SelectFirstOption();

            //Save changes
            acquisitionFiles.SaveAcquisitionFileProperties();

            //Select 1st Property
            acquisitionFiles.ChooseFirstPropertyOption();

            //Verify its Property Details
            propertyInformation.NavigatePropertyDetailsTab();
            propertyInformation.VerifyPropertyDetailsView();

            //Edit Acquisition File
            acquisitionFiles.NavigateToAddPropertiesAcquisitionFile();

            //Delete Property
            acquisitionFiles.DeleteLastProperty();

            //Save Acquisition File changes
            acquisitionFiles.SaveAcquisitionFileProperties();

            //Select 1st Property
            acquisitionFiles.ChooseFirstPropertyOption();
        }

        [StepDefinition(@"I navigate back to the Acquisition File Summary")]
        public void NavigateMainResearchFileSection()
        {
            //Navigate back to File Summary
            acquisitionFiles.NavigateToFileSummary();
        }

        [StepDefinition(@"I search for an existing Acquisition File from row number (.*)")]
        public void SearchExistingAcquisitionFile(int rowNumber)
        {
            /* TEST COVERAGE: PSP-4252  */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Acquisition File Search
            PopulateAcquisitionFile(rowNumber);
            searchAcquisitionFiles.NavigateToSearchAcquisitionFile();

            //Filter research Files
            searchAcquisitionFiles.FilterAcquisitionFiles(acquisitionFile.SearchProperties.PID, acquisitionFile.AcquisitionFileName, acquisitionFile.AcquisitionStatus);
            Assert.True(searchAcquisitionFiles.SearchFoundResults());

            searchAcquisitionFiles.FilterAcquisitionFiles("003-549-551", "Acquisition from Jonathan Doe", "Cancelled");
            Assert.False(searchAcquisitionFiles.SearchFoundResults());

            //Look for the last created research file
            searchAcquisitionFiles.FilterAcquisitionFiles(acquisitionFile.SearchProperties.PID, acquisitionFile.AcquisitionFileName, acquisitionFile.AcquisitionStatus);
        }

        [StepDefinition(@"A new Acquisition file is created successfully")]
        public void NewAcquisitionFileCreated()
        {

            searchAcquisitionFiles.NavigateToSearchAcquisitionFile();
            searchAcquisitionFiles.SearchAcquisitionFileByAFile(acquisitionFileCode);

            Assert.True(searchAcquisitionFiles.SearchFoundResults());
        }

        [StepDefinition(@"An existing Acquisition file has been edited successfully")]
        public void EditAcquisitionFileSuccess()
        {
            acquisitionFiles.VerifyAcquisitionFileView(acquisitionFile);
        }

        [StepDefinition(@"Expected Acquisition File Content is displayed on Acquisition File Table")]
        public void VerifyAcquisitionFileTableContent()
        {
            /* TEST COVERAGE: PSP-4253 */

            //Verify List View
            searchAcquisitionFiles.VerifyAcquisitionFileListView();
            searchAcquisitionFiles.VerifyAcquisitionFileTableContent(acquisitionFile);

        }

        private void PopulateAcquisitionFile(int rowNumber)
        {
            DataTable acquisitionSheet = ExcelDataContext.GetInstance().Sheets["AcquisitionFiles"];
            ExcelDataContext.PopulateInCollection(acquisitionSheet);

            //Lease Details
            acquisitionFile.AcquisitionStatus = ExcelDataContext.ReadData(rowNumber, "AcquisitionStatus");

            acquisitionFile.AcquisitionProject = ExcelDataContext.ReadData(rowNumber, "AcquisitionProject");
            acquisitionFile.AcquisitionProjCode = ExcelDataContext.ReadData(rowNumber, "AcquisitionProjCode");
            acquisitionFile.AcquisitionProjProduct = ExcelDataContext.ReadData(rowNumber, "AcquisitionProjProduct");
            acquisitionFile.AcquisitionProjFunding = ExcelDataContext.ReadData(rowNumber, "AcquisitionProjFunding");
            acquisitionFile.AcquisitionFundingOther = ExcelDataContext.ReadData(rowNumber, "AcquisitionFundingOther");

            acquisitionFile.DeliveryDate = ExcelDataContext.ReadData(rowNumber, "DeliveryDate");
            acquisitionFile.AcquisitionCompletedDate = ExcelDataContext.ReadData(rowNumber, "AcquisitionCompletedDate");

            acquisitionFile.AcquisitionFileName = ExcelDataContext.ReadData(rowNumber, "AcquisitionFileName");
            acquisitionFile.HistoricalFileNumber = ExcelDataContext.ReadData(rowNumber, "HistoricalFileNumber");
            acquisitionFile.PhysicalFileStatus = ExcelDataContext.ReadData(rowNumber, "PhysicalFileStatus");
            acquisitionFile.AcquisitionType = ExcelDataContext.ReadData(rowNumber, "AcquisitionType");
            acquisitionFile.MOTIRegion = ExcelDataContext.ReadData(rowNumber, "MOTIRegion");

            acquisitionFile.AcquisitionTeamStartRow = int.Parse(ExcelDataContext.ReadData(rowNumber, "AcquisitionTeamStartRow"));
            acquisitionFile.AcquisitionTeamCount = int.Parse(ExcelDataContext.ReadData(rowNumber, "AcquisitionTeamCount"));

            //Acquisition Team
            if (acquisitionFile.AcquisitionTeamStartRow != 0 && acquisitionFile.AcquisitionTeamCount != 0)
            {
                PopulateTeamsCollection(acquisitionFile.AcquisitionTeamStartRow, acquisitionFile.AcquisitionTeamCount);
            }

            //Owner
            acquisitionFile.OwnerStartRow = int.Parse(ExcelDataContext.ReadData(rowNumber, "OwnerStartRow"));
            acquisitionFile.OwnerCount = int.Parse(ExcelDataContext.ReadData(rowNumber, "OwnerCount"));
            if (acquisitionFile.OwnerStartRow != 0 && acquisitionFile.OwnerCount != 0)
            {
                PopulateOwnersCollection(acquisitionFile.OwnerStartRow, acquisitionFile.OwnerCount);
            }

            acquisitionFile.OwnerSolicitor = ExcelDataContext.ReadData(rowNumber, "OwnerSolicitor");

            //Properties Search
            acquisitionFile.SearchPropertiesIndex = int.Parse(ExcelDataContext.ReadData(rowNumber, "SearchPropertiesIndex"));
            if (acquisitionFile.SearchPropertiesIndex > 0)
            {
                DataTable searchPropertiesSheet = ExcelDataContext.GetInstance().Sheets["SearchProperties"];
                ExcelDataContext.PopulateInCollection(searchPropertiesSheet);

                acquisitionFile.SearchProperties.PID = ExcelDataContext.ReadData(acquisitionFile.SearchPropertiesIndex, "PID");
                acquisitionFile.SearchProperties.PIN = ExcelDataContext.ReadData(acquisitionFile.SearchPropertiesIndex, "PIN");
                acquisitionFile.SearchProperties.Address = ExcelDataContext.ReadData(acquisitionFile.SearchPropertiesIndex, "Address");
                acquisitionFile.SearchProperties.PlanNumber = ExcelDataContext.ReadData(acquisitionFile.SearchPropertiesIndex, "PlanNumber");
                acquisitionFile.SearchProperties.LegalDescription = ExcelDataContext.ReadData(acquisitionFile.SearchPropertiesIndex, "LegalDescription");
            }
        }

        private void PopulateTeamsCollection(int startRow, int rowsCount)
        {
            DataTable teamsSheet = ExcelDataContext.GetInstance().Sheets["AcquisitionTeams"];
            ExcelDataContext.PopulateInCollection(teamsSheet);

            for (int i = startRow; i < startRow + rowsCount; i++)
            {
                AcquisitionTeamMember teamMember = new AcquisitionTeamMember();
                teamMember.TeamRole = ExcelDataContext.ReadData(i, "TeamRole");
                teamMember.ContactName = ExcelDataContext.ReadData(i, "ContactName");

                acquisitionFile.AcquisitionTeam.Add(teamMember);
            }
        }

        private void PopulateOwnersCollection(int startRow, int rowsCount)
        {
            DataTable ownersSheet = ExcelDataContext.GetInstance().Sheets["AcquisitionOwners"];
            ExcelDataContext.PopulateInCollection(ownersSheet);

            for (int i = startRow; i < startRow + rowsCount; i++)
            {
                Owner owner = new Owner();
                owner.ContactType = ExcelDataContext.ReadData(i, "ContactType");
                owner.isPrimary = bool.Parse(ExcelDataContext.ReadData(i, "isPrimary"));
                owner.GivenNames = ExcelDataContext.ReadData(i, "GivenNames");
                owner.LastName = ExcelDataContext.ReadData(i, "LastName");
                owner.OtherName = ExcelDataContext.ReadData(i, "OtherName");
                owner.CorporationName = ExcelDataContext.ReadData(i, "CorporationName");
                owner.IncorporationNumber = ExcelDataContext.ReadData(i, "IncorporationNumber");
                owner.RegistrationNumber = ExcelDataContext.ReadData(i, "RegistrationNumber");
                owner.MailAddressLine1 = ExcelDataContext.ReadData(i, "MailAddressLine1");
                owner.MailAddressLine2 = ExcelDataContext.ReadData(i, "MailAddressLine2");
                owner.MailAddressLine3 = ExcelDataContext.ReadData(i, "MailAddressLine3");
                owner.MailCity = ExcelDataContext.ReadData(i, "MailCity");
                owner.MailProvince = ExcelDataContext.ReadData(i, "MailProvince");
                owner.MailCountry = ExcelDataContext.ReadData(i, "MailCountry");
                owner.MailOtherCountry = ExcelDataContext.ReadData(i, "MailOtherCountry");
                owner.MailPostalCode = ExcelDataContext.ReadData(i, "MailPostalCode");
                owner.Email = ExcelDataContext.ReadData(i, "Email");
                owner.Phone = ExcelDataContext.ReadData(i, "Phone");

                acquisitionFile.AcquisitionOwners.Add(owner);
            }
        }

    }
}
