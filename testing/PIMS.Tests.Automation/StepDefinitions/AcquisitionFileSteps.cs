

namespace PIMS.Tests.Automation.StepDefinitions
{
    [Binding]
    public class AcquisitionFileSteps
    {
        private readonly LoginSteps loginSteps;
        private readonly AcquisitionFile acquisitionFile;
        private readonly SharedSearchProperties sharedSearchProperties;

        private readonly string userName = "sutairak";

        private readonly string acquisitionFileName = "Automated Acquisition File";

        private readonly string acquisitionFileDeliveryDate = "12/27/2023";
        private readonly string teamMember1 = "Alma Mosley Addison";
        private readonly string teamMember2 = "Tao Sebastian Karkabe";

        private readonly string PID1Search = "014-083-736";
        private readonly string PID2Search = "023-212-047";
        private readonly string PID3Search = "099-123-677";
        private readonly string PIN1Search = "34444321";
        private readonly string Plan1Search = "EPP92028";
        private readonly string address1Search = "1818 Cornwall";
        private readonly string legalDescription1Search = "65 VICTORIA DISTRICT PLAN 33395";
        private readonly string legalDescription2Search = "LOT 97";

        public AcquisitionFileSteps(BrowserDriver driver)
        {
            loginSteps = new LoginSteps(driver);
            acquisitionFile = new AcquisitionFile(driver.Current);
            sharedSearchProperties = new SharedSearchProperties(driver.Current);
        }

        [StepDefinition(@"I create a new Acquisition File")]
        public void CreateAcquisitionFile()
        {
            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Research File
            acquisitionFile.NavigateToCreateNewAcquisitionFile();

            //Create basic Research File
            acquisitionFile.CreateMinimumAcquisitionFile(acquisitionFileName);

            //Save Research File
            acquisitionFile.SaveAcquisitionFile();

        }

        [StepDefinition(@"I add additional information to the Acquisition File")]
        public void AddAdditionalInfoAcquisitionFile()
        {
            //Add Additional Optional information to the acquisition file
            acquisitionFile.AddAdditionalInformation(acquisitionFileDeliveryDate, teamMember1, teamMember2);

            //Save Research File
            acquisitionFile.SaveAcquisitionFile();

        }

        [StepDefinition(@"I add several Properties to the Acquisition File")]
        public void AddAllPropertiesResearchFile()
        {
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
    }
}
