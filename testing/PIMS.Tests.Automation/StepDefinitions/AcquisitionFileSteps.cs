

namespace PIMS.Tests.Automation.StepDefinitions
{
    [Binding]
    public class AcquisitionFileSteps
    {
        private readonly LoginSteps loginSteps;
        private readonly AcquisitionFile acquisitionFile;
        private readonly SearchAcquisitionFiles searchAcquisitionFiles;
        private readonly SharedSearchProperties sharedSearchProperties;
        private readonly SearchProperties searchProperties;
        private readonly PropertyInformation propertyInformation;
        private readonly SharedNotesTab sharedNotesTab;

        private readonly string userName = "TRANPSP1";
        //private readonly string userName = "sutairak";

        private readonly string acquisitionFileName = "Automated Acquisition File";
        private readonly string acquisitionFileNameNotes = "Automated Acquisition File - Testing Notes Tab";
        private readonly string acquisitionFileNameNotes2 = "Automated Acquisition File - Testing Notes Tab Update";

        private readonly string acquisitionFileProject = "Super Test Project";
        private readonly string acquisitionFileProduct = "33-001 Test Product 1";

        private readonly string acquisitionFileDeliveryDate = "12/27/2023";
        private readonly string teamMember1 = "Alejandro Sanchez";
        private readonly string teamMember2 = "Devin Smith";

        private readonly string PID1Search = "014-083-736";
        private readonly string PID2Search = "014-929-791";
        private readonly string PID3Search = "000-750-166";
        private readonly string PID4Search = "025-710-176";
        private readonly string PIN1Search = "90077451";
        private readonly string Plan1Search = "EPP92028";
        private readonly string address1Search = "1818 Cornwall";
        private readonly string legalDescription1Search = "65 VICTORIA DISTRICT PLAN 33395";

        private readonly string propertyDetailsAddressLine1 = "1239 Automated St.";
        private readonly string propertyDetailsAddressLine2 = "Office 4566";
        private readonly string propertyDetailsCity = "Victoria";
        private readonly string propertyDetailsPostalCode = "V8P 1A1";

        private readonly string propertyDetailsmunicipalZoning = "Automated Acquisition zone";
        private readonly string propertyDetailssqMts = "89.87";
        private readonly string propertyDetailscubeMts = "125.78";
        private readonly string propertyDetailsNotes = "Automated Acquisition files - Notes for Property Information";
        private readonly string propertyDetailsNotes2 = "  - Edited note";

        private readonly string notesTabNote1 = "Testing notes tab from Acquisition File";

        protected string acquisitionFileCode = "";

        public AcquisitionFileSteps(BrowserDriver driver)
        {
            loginSteps = new LoginSteps(driver);
            acquisitionFile = new AcquisitionFile(driver.Current);
            searchAcquisitionFiles = new SearchAcquisitionFiles(driver.Current);
            sharedSearchProperties = new SharedSearchProperties(driver.Current);
            searchProperties = new SearchProperties(driver.Current);
            propertyInformation = new PropertyInformation(driver.Current);
            sharedNotesTab = new SharedNotesTab(driver.Current);
        }

        [StepDefinition(@"I navigate to create new Acquisition File")]
        public void NavigateCreateNewAcquisitionFile()
        {
            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Create Acquisition File
            acquisitionFile.NavigateToCreateNewAcquisitionFile();
        }

        [StepDefinition(@"I create and cancel new Acquisition Files")]
        public void CreateCancelAcquisitionFile()
        {
            /* TEST COVERAGE: PSP-4167 */

            //Cancel empty acquisition file
            acquisitionFile.CancelAcquisitionFile();

            //Verify Form is no longer visible
            Assert.True(acquisitionFile.IsCreateAcquisitionFileFormVisible() == 0);

            //Navigate to Create Acquisition File
            acquisitionFile.NavigateToCreateNewAcquisitionFile();

            //Add basic Information
            acquisitionFile.CreateMinimumAcquisitionFile(acquisitionFileName);

            //Cancel Creation
            acquisitionFile.CancelAcquisitionFile();
        }

        [StepDefinition(@"I create a new Acquisition File")]
        public void CreateAcquisitionFile()
        {
            /* TEST COVERAGE: PSP-4163, PSP-4164, PSP-4323, PSP-4553  */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Acquisition File
            acquisitionFile.NavigateToCreateNewAcquisitionFile();

            //Validate Acquisition File Details Create Form
            acquisitionFile.VerifyAcquisitionFileCreate();

            //Create basic Acquisition File
            acquisitionFile.CreateMinimumAcquisitionFile(acquisitionFileName);

            //Save Acquisition File
            acquisitionFile.SaveAcquisitionFile();

            //Get Research File code
            acquisitionFileCode = acquisitionFile.GetAcquisitionFileCode();

        }

        [StepDefinition(@"I add additional information to the Acquisition File")]
        public void AddAdditionalInfoAcquisitionFile()
        {
            /* TEST COVERAGE: PSP-4163, PSP-4323, PSP-4471, PSP-4553, PSP-4331, PSP-4469, PSP-5308 */

            //Enter to Edit mode of Acquisition File
            acquisitionFile.EditAcquisitionFileDetails();

            //Validate Acquisition File Details Update Form
            acquisitionFile.VerifyAcquisitionFileUpdate();

            //Add Additional Optional information to the acquisition file
            acquisitionFile.AddAdditionalInformation(acquisitionFileProject, acquisitionFileProduct, acquisitionFileDeliveryDate, teamMember1, teamMember2);

            //Save Research File
            acquisitionFile.SaveAcquisitionFile();

            //Validate View File Details View Mode
            acquisitionFile.VerifyAcquisitionFileView();
        }

        [StepDefinition(@"I add several Properties to the Acquisition File")]
        public void AddAllPropertiesAcquisitionFile()
        {
            /* TEST COVERAGE: PSP-4327, PSP-4163, PSP-4593, PSP-4334, PSP-4329, PSP-4328, PSP-4327, PSP-4326, PSp-4325 */

            //Navigate to Edit Research File
            acquisitionFile.NavigateToAddPropertiesAcquisitionFile();

            //Navigate to Add Properties by search and verify Add Properties UI/UX
            sharedSearchProperties.NavigateToSearchTab();
            sharedSearchProperties.VerifySearchPropertiesFeature();

            //Search for a property by PID
            sharedSearchProperties.SelectPropertyByPID(PID2Search);
            sharedSearchProperties.SelectFirstOption();

            //Search for a property by PIN
            sharedSearchProperties.SelectPropertyByPIN(PIN1Search);
            sharedSearchProperties.SelectFirstOption();

            //Search for a property by Plan
            sharedSearchProperties.SelectPropertyByPlan(Plan1Search);
            sharedSearchProperties.SelectFirstOption();

            //Search for a property by Address
            sharedSearchProperties.SelectPropertyByAddress(address1Search);
            sharedSearchProperties.SelectFirstOption();

            //Search for a property by Legal Description
            sharedSearchProperties.SelectPropertyByLegalDescription(legalDescription1Search);
            sharedSearchProperties.SelectFirstOption();

            //Search for a duplicate property
            sharedSearchProperties.SelectPropertyByPID(PID2Search);
            sharedSearchProperties.SelectFirstOption();

            //Save Research File
            acquisitionFile.SaveAcquisitionFileProperties();
        }

        [StepDefinition(@"I create an Acquisition File from a pin on map")]
        public void CreateAcquisitionFileFromPin()
        {
            /* TEST COVERAGE: PSP-4601, PSP-1546, PSP-1556, PSP-4704, PSP-4164, PSP-5308 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Search for a property
            searchProperties.SearchPropertyByPINPID(PID4Search);

            //Select Found Pin on map
            searchProperties.SelectFoundPin();

            //Close Property Information Modal
            propertyInformation.ClosePropertyInfoModal();

            //Open elipsis option
            propertyInformation.OpenMoreOptionsPopUp();
            propertyInformation.ChooseCreationOptionFromPin("Acquisition File - Create new");

            //Validate Acquisition File Details Create Form
            acquisitionFile.VerifyAcquisitionFileCreate();

            //Fill basic Acquisition File information
            acquisitionFile.CreateMinimumAcquisitionFile(acquisitionFileName);

            //Save Acquisition File
            acquisitionFile.SaveAcquisitionFile();

            //Get Research File code
            acquisitionFileCode = acquisitionFile.GetAcquisitionFileCode();

            //Edit Acquisition File
            acquisitionFile.EditAcquisitionFile();

            //Add additional information
            acquisitionFile.AddAdditionalInformation(acquisitionFileProject, acquisitionFileProduct, acquisitionFileDeliveryDate, teamMember1, teamMember2);

            //Save Acquisition File
            acquisitionFile.SaveAcquisitionFile();
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

        [StepDefinition(@"I edit an existing Acquisition File")]
        public void EditAcquisitionFile()
        {
            /* TEST COVERAGE: PSP-4544, PSP-4545, PSP-4590, PSP-4591, PSP-4600, PSP-4689, PSP-5003, PSP-5006, PSP-5007  */

            //Edit Acquisition File
            acquisitionFile.EditAcquisitionFile();

            //Delete a staff member
            acquisitionFile.DeleteLastStaffMember();

            //Save Staff changes
            acquisitionFile.SaveAcquisitionFile();

            //Navigate to Edit Research File
            acquisitionFile.NavigateToAddPropertiesAcquisitionFile();

            //Search for a property by PID
            sharedSearchProperties.NavigateToSearchTab();
            sharedSearchProperties.SelectPropertyByPID(PID3Search);
            sharedSearchProperties.SelectFirstOption();

            //Save changes
            acquisitionFile.SaveAcquisitionFileProperties();

            //Select 1st Property
            acquisitionFile.ChooseFirstPropertyOption();

            //Verify its Property Details
            propertyInformation.NavigatePropertyDetailsTab();
            propertyInformation.VerifyPropertyDetailsView("Acquisition File");

            //Edit Property Details
            propertyInformation.EditPropertyInfoBttn();
            propertyInformation.UpdateMaxPropertyDetails(propertyDetailsAddressLine1, propertyDetailsAddressLine2, propertyDetailsCity, propertyDetailsPostalCode, propertyDetailsmunicipalZoning,
                propertyDetailssqMts, propertyDetailscubeMts, propertyDetailsNotes);

            //Save Property Details
            propertyInformation.SavePropertyDetails();

            //Edit Property Details
            propertyInformation.EditPropertyInfoBttn();
            propertyInformation.UpdateMinPropertyDetails(propertyDetailsNotes2);

            //Cancel Property Details
            acquisitionFile.CancelAcquisitionFile();

            //Verify PIMS File Tab
            propertyInformation.VerifyPimsFiles();

            //Edit Acquisition File
            acquisitionFile.NavigateToAddPropertiesAcquisitionFile();

            //Delete Property
            acquisitionFile.DeleteLastProperty();

            //Save Acquisition File changes
            acquisitionFile.SaveAcquisitionFileProperties();

            //Navigate to Edit Research File
            acquisitionFile.EditAcquisitionFile(); ;

            //Change Status
            acquisitionFile.ChangeStatus("Cancelled");

            //Cancel changes
            acquisitionFile.CancelAcquisitionFile();

        }

        [StepDefinition(@"I search for an existing Acquisition File")]
        public void SearchExistingAcquisitionFile()
        {
            /* TEST COVERAGE: PSP-4252  */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Acquisition File Search
            searchAcquisitionFiles.NavigateToSearchAcquisitionFile();

            //Filter research Files
            searchAcquisitionFiles.FilterAcquisitionFiles(PID2Search, "Automated", "Active");
            Assert.True(searchAcquisitionFiles.SearchFoundResults());

            searchAcquisitionFiles.FilterAcquisitionFiles(PID1Search, "Automated", "Cancelled");
            Assert.False(searchAcquisitionFiles.SearchFoundResults());

            //Look for the last created research file
            searchAcquisitionFiles.SearchLastAcquisitionFile();
        }

        [StepDefinition(@"I create a Acquisition File with a new Note on the Notes Tab")]
        public void CreateNotesTab()
        {
            /* TEST COVERAGE: PSP-5332, PSP-5505, PSP-5506, PSP-5507  */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Acquisition File
            acquisitionFile.NavigateToCreateNewAcquisitionFile();

            //Create basic Acquisition File
            acquisitionFile.CreateMinimumAcquisitionFile(acquisitionFileNameNotes);

            //Save Acquisition File
            acquisitionFile.SaveAcquisitionFile();

            //Navigate to the Notes Tab
            sharedNotesTab.NavigateNotesTab();

            //Create a new note
            sharedNotesTab.CreateNotesTabButton();
            sharedNotesTab.AddNewNoteDetails(notesTabNote1);

            //Cancel new note
            sharedNotesTab.CancelNote();

            //Create a new note
            sharedNotesTab.CreateNotesTabButton();
            sharedNotesTab.AddNewNoteDetails(notesTabNote1);

            //Save note
            sharedNotesTab.SaveNote();

            //Edit note
            sharedNotesTab.ViewFirstNoteDetails();
            sharedNotesTab.EditNote(acquisitionFileNameNotes2);

            //Cancel note's update
            sharedNotesTab.CancelNote();

            //Edit note
            sharedNotesTab.ViewFirstNoteDetails();
            sharedNotesTab.EditNote(acquisitionFileNameNotes2);

            //Save changes
            sharedNotesTab.SaveNote();

            //Verify Notes quantity
            Assert.True(sharedNotesTab.NotesTabCount() == 1);

            //Delete Note
            sharedNotesTab.DeleteFirstNote();
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
            acquisitionFile.VerifyAcquisitionFileView();
        }

        [StepDefinition(@"Expected Acquisition File Content is displayed on Acquisition File Table")]
        public void VerifyAcquisitionFileTableContent()
        {
            /* TEST COVERAGE: PSP-4253 */

            //Verify List View
            searchAcquisitionFiles.VerifyAcquisitionFileListView();
            searchAcquisitionFiles.VerifyAcquisitionFileTableContent(acquisitionFileName);

        }

        [StepDefinition(@"The creation of an Acquisition File is cancelled successfully")]
        public void CancelSuccessful()
        {
            Assert.True(acquisitionFile.IsCreateAcquisitionFileFormVisible() == 0);
        }

        [StepDefinition(@"The Notes Tab rendered successfully")]
        public void NotesTanSuccessful()
        {
            sharedNotesTab.VerifyNotesTabListView();
        }

    }
}
