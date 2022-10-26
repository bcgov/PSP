

namespace PIMS.Tests.Automation.StepDefinitions
{
    [Binding]
    public class ResearchFileSteps
    {
        private readonly LoginSteps loginSteps;
        private readonly ResearchFile researchFile;
        private readonly SharedSearchProperties sharedSearchProperties;
        private readonly SearchResearchFiles searchResearchFile;

        private readonly string userName = "TRANPSP1";

        private readonly string researchFileName = "Automated Research File";
        private readonly string researchFileRoadName = "The Automated Road";
        private readonly string researchFileAliasName = "The Automated Happy Path";
        private readonly int researchFilePurposesNbr = 2;
        private readonly string researchFileRequestDate = "03/05/2022";
        private readonly string researchFileRequester = "John Smith";
        private readonly string researchFileDescriptionRequest = "Automation Test - Description for Research File request";
        private readonly string researchFileResearchCompletedDate = "03/11/2022";
        private readonly string researchFileResultRequest = "Automation Test - Description for the Result of the Research File request";
        private readonly Boolean researchFileExpropiationNo = false;
        private readonly string researchFileExpropiationNotes = "Automation Test - Expropiation Notes";

        private readonly string PID1Search = "001-505-360";
        private readonly string PID2Search = "028-753-054";
        private readonly string PID3Search = "099-123-677";
        private readonly string PIN1Search = "8157500";
        private readonly string Plan1Search = "48TR1_QUEEN_CHARLOTTE";
        private readonly string address1Search = "1818 Cornwall";
        private readonly string legalDescription1Search = "DISTRICT LOT 2405";
        private readonly string legalDescription2Search = "LOT 97";

        protected string researchFileCode = "";


        public ResearchFileSteps(BrowserDriver driver)
        {
            loginSteps = new LoginSteps(driver);
            researchFile = new ResearchFile(driver.Current);
            sharedSearchProperties = new SharedSearchProperties(driver.Current);
            searchResearchFile = new SearchResearchFiles(driver.Current);
        }

        [StepDefinition(@"I create a new Research File")]
        public void CreateResearchFile()
        {
            /* TEST COVERAGE: PSP-3266, PSP-3267, PSP-4556 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Research File
            researchFile.NavigateToCreateNewResearchFile();

            //Create basic Research File
            researchFile.CreateMinimumResearchFile(researchFileName);

            //Save Research File
            researchFile.SaveResearchFile();

            //Get Research File code
            researchFileCode = researchFile.GetResearchFileCode();

        }

        [StepDefinition(@"I add additional information to an existing Research File")]
        public void AddInfoToResearchFile()
        {
            /* TEST COVERAGE: PSP-3358, PSP-3357 */

            //Add additional info to the reseach File
            researchFile.AddAdditionalResearchFileInfo(researchFileRoadName, researchFileAliasName, researchFilePurposesNbr, researchFileRequestDate, researchFileRequester,
                researchFileDescriptionRequest, researchFileResearchCompletedDate, researchFileResultRequest, researchFileExpropiationNo, researchFileExpropiationNotes);

            //Save Research File
            researchFile.SaveResearchFile();
        }

        [StepDefinition(@"I add a Property by PID to the Research File")]
        public void AddOnePIDPropertyResearchFile()
        {
            /* TEST COVERAGE: PSP-3721, PSP-4556, PSP-3596 */

            //Navigate to Edit Research File
            researchFile.NavigateToAddPropertiesReseachFile();

            //Search for a property by PID
            sharedSearchProperties.NavigateToSearchTab();
            sharedSearchProperties.SelectPropertyByPID(PID1Search);

            //Choose 1st option
            sharedSearchProperties.SelectFirstOption();

            //Save Research File
            researchFile.SaveResearchFile();

            //Confirm saving changes
            researchFile.ConfirmChangesResearchFile();

        }

        [StepDefinition(@"I look for a Property by Legal Description")]
        public void LookPlanPropertyResearchFile()
        {
            /* TEST COVERAGE: PSP-3721, PSP-3597 */

            //Navigate to Edit Research File
            researchFile.NavigateToAddPropertiesReseachFile();

            //Search for a property by PID
            sharedSearchProperties.NavigateToSearchTab();
            sharedSearchProperties.SelectPropertyByLegalDescription(legalDescription2Search);
        }

        [StepDefinition(@"I look for an incorrect PID")]
        public void LookIncorrectPID()
        {
            /* TEST COVERAGE: PSP-3596, PSP-3598 */

            //Navigate to Edit Research File
            researchFile.NavigateToAddPropertiesReseachFile();

            //Search for a property by PID
            sharedSearchProperties.NavigateToSearchTab();
            sharedSearchProperties.SelectPropertyByPID(PID3Search);
        }

        [StepDefinition(@"I add several Properties to the research File")]
        public void AddAllPropertiesResearchFile()
        {
            /* TEST COVERAGE: PSP-3266, PSP-3721, PSP-4333, PSP-3597, PSP-3596, PSP-3595, PSP-3849, PSP-3598 */

            //Navigate to Edit Research File
            researchFile.NavigateToAddPropertiesReseachFile();

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
            sharedSearchProperties.SelectPropertyByAddress(address1Search);
            sharedSearchProperties.SelectFirstOption();

            //Search for a property by Legal Description
            sharedSearchProperties.SelectPropertyByLegalDescription(legalDescription1Search);
            sharedSearchProperties.SelectFirstOption();

            //Save Research File
            researchFile.SaveResearchFile();

            //Confirm saving changes
            researchFile.ConfirmChangesResearchFile();
        }

        [StepDefinition(@"A new research file is created successfully")]
        public void NewResearchFileCreated()
        {
            /* TEST COVERAGE: PSP-4556 */

            searchResearchFile.NavigateToSearchResearchFile();
            searchResearchFile.SearchResearchFileByRFile(researchFileCode);

            Assert.True(searchResearchFile.SearchFoundResults());
        }

        [StepDefinition(@"More than 15 results are found")]
        public void TooManyResultsFound()
        {
            /* TEST COVERAGE: PSP-3598 */

            Assert.True(sharedSearchProperties.noRowsResultsMessage().Equals("Too many results (more than 15) match this criteria. Please refine your search."));
        }

        [StepDefinition(@"No results are found")]
        public void NoResultsFound()
        {
            /* TEST COVERAGE: PSP-3598 */

            Assert.True(sharedSearchProperties.noRowsResultsMessage().Equals("No results found for your search criteria."));
        }
    }
}
