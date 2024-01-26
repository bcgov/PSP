using PIMS.Tests.Automation.Classes;
using PIMS.Tests.Automation.Data;
using PIMS.Tests.Automation.PageObjects;
using System.Data;

namespace PIMS.Tests.Automation.StepDefinitions
{
    [Binding]
    public class DispositionFileSteps
    {
        private readonly LoginSteps loginSteps;
        private readonly DispositionFileDetails dispositionFileDetails;
        private readonly SearchDispositionFiles searchDispositionFiles;
        private readonly SharedFileProperties sharedFileProperties;
        private readonly PropertyInformation propertyInformation;
        private readonly Notes notes;

        private readonly string userName = "TRANPSP1";
        private string dispositionFileCode = "";
        private DispositionFile dispositionFile;

        public DispositionFileSteps(BrowserDriver driver)
        {
            loginSteps = new LoginSteps(driver);
            dispositionFileDetails = new DispositionFileDetails(driver.Current);
            searchDispositionFiles = new SearchDispositionFiles(driver.Current);
            sharedFileProperties = new SharedFileProperties(driver.Current);
            propertyInformation = new PropertyInformation(driver.Current);
            notes = new Notes(driver.Current);
        }

        [StepDefinition(@"I create a new Disposition File from row number (.*)")]
        public void CreateDispositionFile(int rowNumber)
        {
            /* TEST COVERAGE: PSP-7504, PSP-7507 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Acquisition File
            PopulateDispositionFile(rowNumber);
            dispositionFileDetails.NavigateToCreateNewDipositionFile();

            //Validate Acquisition File Details Create Form
            dispositionFileDetails.VerifyDispositionFileCreate();

            //Create basic Acquisition File
            dispositionFileDetails.CreateMinimumDispositionFile(dispositionFile);

            //Save Acquisition File
            dispositionFileDetails.SaveDispositionFileDetails();

            //Get Disposition File code
            dispositionFileCode = dispositionFileDetails.GetDispositionFileCode();
        }

        [StepDefinition(@"I add additional information to the Disposition File Details")]
        public void AddAdditionalInfoDispositionFile()
        {
            /* TEST COVERAGE:  PSP-7505, PSP-7558 */

            //Enter to Edit mode of Disposition File
            dispositionFileDetails.EditDispositionFileBttn();

            //Add Additional Optional information to the disposition file
            dispositionFileDetails.AddAdditionalInformation(dispositionFile);

            //Save Disposition File
            dispositionFileDetails.SaveDispositionFileDetails();

            //Validate View File Details View Mode
            dispositionFileDetails.VerifyDispositionFileView(dispositionFile);

            //Verify automatic note created when the status is changed
            if (dispositionFile.DispositionFileStatus != "Active")
            {
                notes.NavigateNotesTab();
                notes.VerifyAutomaticNotes("Disposition File", "Active", dispositionFile.DispositionFileStatus);
            }
        }

        [StepDefinition(@"I update the File details from an existing Disposition File from row number (.*)")]
        public void UpdateDispositionFileDetails(int rowNumber)
        {
            /* TEST COVERAGE: PSP-7506, PSP-7507, PSP-7522, PSP-7559 */

            PopulateDispositionFile(rowNumber);

            //Search for an existing Disposition File
            searchDispositionFiles.NavigateToSearchDispositionFile();
            searchDispositionFiles.SearchDispositionFileByDFile(dispositionFileCode);
            searchDispositionFiles.SelectFirstOption();

            //Update existing Disposition File
            dispositionFileDetails.EditDispositionFileBttn();
            dispositionFileDetails.UpdateDispositionFile(dispositionFile);

            //Cancel changes
            dispositionFileDetails.CancelDispositionFile();

            //Edit Disposition File
            dispositionFileDetails.EditDispositionFileBttn();
            dispositionFileDetails.UpdateDispositionFile(dispositionFile);

            //Save Disposition File
            dispositionFileDetails.SaveDispositionFileDetails();

            //Get Disposition File code
            dispositionFileCode = dispositionFileDetails.GetDispositionFileCode();

            //Validate View File Details View Mode
            dispositionFileDetails.VerifyDispositionFileView(dispositionFile);

            //Verify automatic note created when the status changes
            if (dispositionFile.DispositionFileStatus != "Active")
            {
                notes.NavigateNotesTab();
                notes.VerifyAutomaticNotes("Disposition File", "Hold", dispositionFile.DispositionFileStatus);
            }
        }

        [StepDefinition(@"I add Properties to the Disposition File")]
        public void AddProperties()
        {
            /* TEST COVERAGE:  */

            //Navigate to Properties for Disposition File
            sharedFileProperties.NavigateToAddPropertiesToFile();

            //Navigate to Add Properties by search and verify Add Properties UI/UX
            sharedFileProperties.NavigateToSearchTab();
            sharedFileProperties.VerifySearchPropertiesFeature();

            //Search for a property by PID
            if (dispositionFile.DispositionSearchProperties.PID != "")
            {
                sharedFileProperties.SelectPropertyByPID(dispositionFile.DispositionSearchProperties.PID);
                sharedFileProperties.SelectFirstOptionFromSearch();
            }

            //Search for a property by PIN
            if (dispositionFile.DispositionSearchProperties.PIN != "")
            {
                sharedFileProperties.SelectPropertyByPIN(dispositionFile.DispositionSearchProperties.PIN);
                sharedFileProperties.SelectFirstOptionFromSearch();
            }

            //Search for a property by Plan
            if (dispositionFile.DispositionSearchProperties.PlanNumber != "")
            {
                sharedFileProperties.SelectPropertyByPlan(dispositionFile.DispositionSearchProperties.PlanNumber);
                sharedFileProperties.SelectFirstOptionFromSearch();
            }

            //Search for a property by Address
            if (dispositionFile.DispositionSearchProperties.Address != "")
            {
                sharedFileProperties.SelectPropertyByAddress(dispositionFile.DispositionSearchProperties.Address);
                sharedFileProperties.SelectFirstOptionFromSearch();
            }

            //Search for a property by Legal Description
            //if (dispositionFile.DispositionSearchProperties.LegalDescription != "")
            //{
            //    sharedSearchProperties.SelectPropertyByLegalDescription(acquisitionFile.SearchProperties.LegalDescription);
            //    sharedSearchProperties.SelectFirstOption();
            //}

            //Search for a duplicate property
            if (dispositionFile.DispositionSearchProperties.PID != "")
            {
                sharedFileProperties.SelectPropertyByPID(dispositionFile.DispositionSearchProperties.PID);
                sharedFileProperties.SelectFirstOptionFromSearch();
            }

            //Save Research File
            sharedFileProperties.SaveFileProperties();
        }

        [StepDefinition(@"I update a Disposition File's Properties from row number (.*)")]
        public void UpdateProperties(int rowNumber)
        {
            /* TEST COVERAGE:  */

            PopulateDispositionFile(rowNumber);

            //Search for an existing Acquisition File
            searchDispositionFiles.NavigateToSearchDispositionFile();
            searchDispositionFiles.SearchDispositionFileByDFile(dispositionFileCode);
            searchDispositionFiles.SelectFirstOption();

            //Navigate to Edit Acquisition File's Properties
            sharedFileProperties.NavigateToAddPropertiesToFile();

            //Search for a property by PIN
            sharedFileProperties.NavigateToSearchTab();
            sharedFileProperties.SelectPropertyByPIN(dispositionFile.DispositionSearchProperties.PIN);
            sharedFileProperties.SelectFirstOptionFromSearch();

            //Delete last Property
            sharedFileProperties.DeleteLastPropertyFromFile();

            //Save changes
            sharedFileProperties.SaveFileProperties();

            //Select 1st Property
            sharedFileProperties.SelectFirstPropertyOptionFromFile();
        }

        [StepDefinition(@"A new Disposition file is created successfully")]
        public void NewDispositionFileCreated()
        {

            searchDispositionFiles.NavigateToSearchDispositionFile();
            searchDispositionFiles.SearchDispositionFileByDFile(dispositionFileCode);

            Assert.True(searchDispositionFiles.SearchFoundResults());
        }

        private void PopulateDispositionFile(int rowNumber)
        {
            DataTable dispositionSheet = ExcelDataContext.GetInstance().Sheets["DispositionFiles"];
            ExcelDataContext.PopulateInCollection(dispositionSheet);
            dispositionFile = new DispositionFile();

            //Disposition File Status
            dispositionFile.DispositionFileStatus = ExcelDataContext.ReadData(rowNumber, "DispositionFileStatus");

            //Project
            dispositionFile.DispositionProjFunding = ExcelDataContext.ReadData(rowNumber, "DispositionProjFunding");

            //Schedule
            dispositionFile.DispositionAssignedDate = ExcelDataContext.ReadData(rowNumber, "AssignedDate");
            dispositionFile.DispositionCompletedDate = ExcelDataContext.ReadData(rowNumber, "DispositionCompletedDate");

            //Disposition Details
            dispositionFile.DispositionFileName = ExcelDataContext.ReadData(rowNumber, "DispositionFileName");
            dispositionFile.DispositionReferenceNumber = ExcelDataContext.ReadData(rowNumber, "ReferenceNumber");
            dispositionFile.DispositionStatus = ExcelDataContext.ReadData(rowNumber, "DispositionStatus"); 
            dispositionFile.DispositionType = ExcelDataContext.ReadData(rowNumber, "DispositionType");
            dispositionFile.DispositionOtherTransferType = ExcelDataContext.ReadData(rowNumber, "DispositionOtherTransferType");
            dispositionFile.InitiatingDocument = ExcelDataContext.ReadData(rowNumber, "InitiatingDocument");
            dispositionFile.OtherInitiatingDocument = ExcelDataContext.ReadData(rowNumber, "OtherInitiatingDocument");
            dispositionFile.InitiatingDocumentDate = ExcelDataContext.ReadData(rowNumber, "InitiatingDocumentDate");
            dispositionFile.PhysicalFileStatus = ExcelDataContext.ReadData(rowNumber, "PhysicalFileStatus");
            dispositionFile.InitiatingBranch = ExcelDataContext.ReadData(rowNumber, "InitiatingBranch");
            dispositionFile.DispositionMOTIRegion = ExcelDataContext.ReadData(rowNumber, "DispositionMOTIRegion");

            //Disposition Team
            dispositionFile.DispositionTeamStartRow = int.Parse(ExcelDataContext.ReadData(rowNumber, "DispositionTeamStartRow"));
            dispositionFile.DispositionTeamCount = int.Parse(ExcelDataContext.ReadData(rowNumber, "DispositionTeamCount"));

            if (dispositionFile.DispositionTeamStartRow != 0 && dispositionFile.DispositionTeamCount != 0)
                PopulateTeamsCollection(dispositionFile.DispositionTeamStartRow, dispositionFile.DispositionTeamCount);

            //Properties Search
            dispositionFile.DispositionSearchPropertiesIndex = int.Parse(ExcelDataContext.ReadData(rowNumber, "DisSearchPropertiesIndex"));
            if (dispositionFile.DispositionSearchPropertiesIndex > 0)
            {
                DataTable searchPropertiesSheet = ExcelDataContext.GetInstance().Sheets["SearchProperties"];
                ExcelDataContext.PopulateInCollection(searchPropertiesSheet);

                dispositionFile.DispositionSearchProperties.PID = ExcelDataContext.ReadData(dispositionFile.DispositionSearchPropertiesIndex, "PID");
                dispositionFile.DispositionSearchProperties.PIN = ExcelDataContext.ReadData(dispositionFile.DispositionSearchPropertiesIndex, "PIN");
                dispositionFile.DispositionSearchProperties.Address = ExcelDataContext.ReadData(dispositionFile.DispositionSearchPropertiesIndex, "Address");
                dispositionFile.DispositionSearchProperties.PlanNumber = ExcelDataContext.ReadData(dispositionFile.DispositionSearchPropertiesIndex, "PlanNumber");
                dispositionFile.DispositionSearchProperties.LegalDescription = ExcelDataContext.ReadData(dispositionFile.DispositionSearchPropertiesIndex, "LegalDescription");
            }
        }

        private void PopulateTeamsCollection(int startRow, int rowsCount)
        {
            DataTable teamsSheet = ExcelDataContext.GetInstance().Sheets["TeamMembers"];
            ExcelDataContext.PopulateInCollection(teamsSheet);

            for (int i = startRow; i < startRow + rowsCount; i++)
            {
                TeamMember teamMember = new TeamMember();
                teamMember.TeamMemberRole = ExcelDataContext.ReadData(i, "TeamMemberRole");
                teamMember.TeamMemberContactName = ExcelDataContext.ReadData(i, "TeamMemberContactName");
                teamMember.TeamMemberContactType = ExcelDataContext.ReadData(i, "TeamMemberContactType");
                teamMember.TeamMemberPrimaryContact = ExcelDataContext.ReadData(i, "TeamMemberPrimaryContact");

                dispositionFile.DispositionTeam.Add(teamMember);
            }
        }
    }
}
