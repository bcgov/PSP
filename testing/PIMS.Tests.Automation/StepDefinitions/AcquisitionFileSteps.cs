

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

        private readonly string userName = "TRANPSP1";
        //private readonly string userName = "sutairak";

        private readonly string acquisitionFileName = "Automated Acquisition File";

        private readonly string acquisitionFileDeliveryDate = "12/27/2023";
        private readonly string teamMember1 = "Alejandro Sanchez";
        private readonly string teamMember2 = "Mahesh Babu";

        private readonly string PID1Search = "014-083-736";
        private readonly string PID2Search = "023-212-047";
        private readonly string PID3Search = "099-123-677";
        private readonly string PID4Search = "025-710-176";
        private readonly string PIN1Search = "34444321";
        private readonly string Plan1Search = "EPP92028";
        private readonly string address1Search = "1818 Cornwall";
        private readonly string legalDescription1Search = "65 VICTORIA DISTRICT PLAN 33395";
        private readonly string legalDescription2Search = "LOT 97";

        protected string acquisitionFileCode = "";

        public AcquisitionFileSteps(BrowserDriver driver)
        {
            loginSteps = new LoginSteps(driver);
            acquisitionFile = new AcquisitionFile(driver.Current);
            searchAcquisitionFiles = new SearchAcquisitionFiles(driver.Current);
            sharedSearchProperties = new SharedSearchProperties(driver.Current);
            searchProperties = new SearchProperties(driver.Current);
            propertyInformation = new PropertyInformation(driver.Current);
        }

        [StepDefinition(@"I create a new Acquisition File")]
        public void CreateAcquisitionFile()
        {
            /* TEST COVERAGE: PSP-4163, PSP-4323, PSP-4553 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Acquisition File
            acquisitionFile.NavigateToCreateNewAcquisitionFile();

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
            /* TEST COVERAGE: PSP-4163, PSP-4323, PSP-4471, PSP-4553 */

            //Add Additional Optional information to the acquisition file
            acquisitionFile.AddAdditionalInformation(acquisitionFileDeliveryDate, teamMember1, teamMember2);

            //Save Research File
            acquisitionFile.SaveAcquisitionFile();

        }

        [StepDefinition(@"I add several Properties to the Acquisition File")]
        public void AddAllPropertiesAcquisitionFile()
        {
            /* TEST COVERAGE: PSP-4327, PSP-4163, PSP-4593, PSP-4334, PSP-4329, PSP-4328, PSP-4327, PSP-4326 */

            //Navigate to Edit Research File
            acquisitionFile.NavigateToAddPropertiesAcquisitionFile();

            //Search for a property by PID
            sharedSearchProperties.NavigateToSearchTab();
            sharedSearchProperties.SelectPropertyByPID(PID2Search);
            sharedSearchProperties.SelectFirstOption();

            //Search for a property by PIN

            sharedSearchProperties.SelectPropertyByPIN(PIN1Search);
            sharedSearchProperties.SelectFirstOption();

            //Search for a property by Plan
            sharedSearchProperties.SelectPropertyByPlan(Plan1Search);
            sharedSearchProperties.SelectFirstOption();

            //Search for a property by Address
            //sharedSearchProperties.SelectPropertyByAddress(address1Search);
            //sharedSearchProperties.SelectFirstOption();

            //Search for a property by Legal Description
            sharedSearchProperties.SelectPropertyByLegalDescription(legalDescription1Search);
            sharedSearchProperties.SelectFirstOption();

            //Save Research File
            acquisitionFile.SaveAcquisitionFileProperties();
        }

        [StepDefinition(@"I create an Acquisition File from a pin on map")]
        public void CreateAcquisitionFileFromPin()
        {
            /* TEST COVERAGE: PSP-4601, PSP-1546, PSP-1556 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Search for a property
            searchProperties.SearchPropertyByPINPID(PID4Search);

            //Select Found Pin on map
            searchProperties.SelectFoundPin();

            //Close Property Information Modal
            propertyInformation.ClosePropertyInfoModal();

            //Open elipsis option
            propertyInformation.ChooseCreationOptionFromPin("Acquisition File - Create new");

            //Fill basic Acquisition File information
            acquisitionFile.CreateMinimumAcquisitionFile(acquisitionFileName);

            //Save Acquisition File
            acquisitionFile.SaveAcquisitionFile();

            //Get Research File code
            acquisitionFileCode = acquisitionFile.GetAcquisitionFileCode();

            //Add additional information
            acquisitionFile.AddAdditionalInformation(acquisitionFileDeliveryDate, teamMember1, teamMember2);

            //Save Acquisition File
            acquisitionFile.SaveAcquisitionFile();

        }

        [StepDefinition(@"I edit an existing Acquisition File")]
        public void EditAcquisitionFile()
        {
            /* TEST COVERAGE: PSP-4600, PSP-4591, PSP-4548, PSP-4545 */

            //Search for an acquisition file
            searchProperties.SearchPropertyByPINPID(PID4Search);

            //Select foound pin on map
            searchProperties.SelectFoundPin();


        }

        [StepDefinition(@"A new Acquisition file is created successfully")]
        public void NewAcquisitionFileCreated()
        {
            /* TEST COVERAGE: */

            searchAcquisitionFiles.NavigateToSearchAcquisitionFile();
            searchAcquisitionFiles.SearchAcquisitionFileByAFile(acquisitionFileCode);

            Assert.True(searchAcquisitionFiles.SearchFoundResults());
        }

    }
}
