

namespace PIMS.Tests.Automation.StepDefinitions
{
    [Binding]
    public class ResearchFileSteps
    {
        private readonly LoginSteps loginSteps;
        private readonly ResearchFile researchFile;
        private readonly SharedSearchProperties sharedSearchProperties;
        private readonly SearchResearchFiles searchResearchFile;
        private readonly PropertyInformation propertyInformation;
        private readonly SearchProperties searchProperties;

        private readonly string userName = "TRANPSP1";
        //private readonly string userName = "sutairak";

        private readonly string researchFileName = "Automated Research File";
        private readonly string researchFileRoadName = "The Automated Road";
        private readonly string researchFileAliasName = "The Automated Happy Path";
        private readonly int researchFilePurposesNbr = 2;
        private readonly string researchFileRequestDate = "03/05/2022";
        private readonly string researchFileVerifyRequestDate = "Mar 5, 2022";
        private readonly string researchFileRequester = "Devin Smith";
        private readonly string researchFileDescriptionRequest = "Automation Test - Description for Research File request";
        private readonly string researchFileResearchCompletedDate = "03/11/2022";
        private readonly string researchFileVerifyResearchCompletedDate = "Mar 11, 2022";
        private readonly string researchFileResultRequest = "Automation Test - Description for the Result of the Research File request";
        private readonly Boolean researchFileExpropiationNo = false;
        private readonly string researchFileExpropiationNotes = "Automation Test - Expropiation Notes";

        private readonly string PID1Search = "001-505-360";
        private readonly string PID2Search = "028-753-054";
        private readonly string PID3Search = "099-123-677";
        private readonly string PID4Search = "004-537-360";
        private readonly string PIN1Search = "8157500";
        private readonly string Plan1Search = "18TR2_RUPERT";
        private readonly string address1Search = "1818 Cornwall";
        private readonly string legalDescription1Search = "DISTRICT LOT 2405";
        private readonly string legalDescription2Search = "LOT 97";

        private readonly string propertyResearchPropName = "Automation Property";
        private readonly string propertyResearchDocRef = "Automation test - Document reference";
        private readonly string propertyResearchNotes = "Automation test - Testing Property Research adding information";
        private readonly string propertyResearchEditNotes = "Automation test - Testing Property Research adding information - Automation Edit";

        private readonly string propertyDetailsMunicipalZoning = "Automated Test zone";
        private readonly string propertyDetailsAreaSqMts = "50";
        private readonly string propertyDetailsAreaCubeMts = "100";
        private readonly string propertyDetailsNotes = "Automated updates on Property Information from Research File";
        private readonly string propertyDetailsNotesChanges = " Changes added automatically";


        protected string researchFileCode = "";


        public ResearchFileSteps(BrowserDriver driver)
        {
            loginSteps = new LoginSteps(driver);
            researchFile = new ResearchFile(driver.Current);
            sharedSearchProperties = new SharedSearchProperties(driver.Current);
            searchResearchFile = new SearchResearchFiles(driver.Current);
            propertyInformation = new PropertyInformation(driver.Current);
            searchProperties = new SearchProperties(driver.Current);
        }

        [StepDefinition(@"I start creating and cancel a new Research File")]
        public void CreateCancelFile()
        {
            /* TEST COVERAGE: PSP-3268 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Research File
            researchFile.NavigateToCreateNewResearchFile();

            //Create basic Research File
            researchFile.CreateMinimumResearchFile(researchFileName);

            //Save Research File
            researchFile.CancelResearchFile();

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

        [StepDefinition(@"I create a Research File from a pin on map")]
        public void CreateResearchFileFromPin()
        {
            /* TEST COVERAGE: PSP-3371, PSP-1546, PSP-1556 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Search for a property
            searchProperties.SearchPropertyByPINPID(PID4Search);

            //Select found property on Map
            searchProperties.SelectFoundPin();

            //Close Property Information Modal
            propertyInformation.ClosePropertyInfoModal();

            //Open elipsis option
            propertyInformation.ChooseCreationOptionFromPin("Research File - Create new");

            //Fill basic Research File information
            researchFile.CreateMinimumResearchFile(researchFileName);

            //Fill name to selected property
            //sharedSearchProperties.AddNameSelectedProperty("Automated Property from Pin");

            //Save Research File
            researchFile.SaveResearchFile();

            //Get Research File code
            researchFileCode = researchFile.GetResearchFileCode();

            //Add additional info to the reseach File
            researchFile.AddAdditionalResearchFileInfo(researchFileRoadName, researchFileAliasName, researchFilePurposesNbr, researchFileRequestDate, researchFileRequester,
                researchFileDescriptionRequest, researchFileResearchCompletedDate, researchFileResultRequest, researchFileExpropiationNo, researchFileExpropiationNotes);

            //Save Research File
            researchFile.SaveResearchFile();

        }

        [StepDefinition(@"I add additional information to an existing Research File")]
        public void AddInfoToResearchFile()
        {
            /* TEST COVERAGE: PSP-3358, PSP-3357, PSP-3367 */

            //Add additional info to the reseach File
            researchFile.AddAdditionalResearchFileInfo(researchFileRoadName, researchFileAliasName, researchFilePurposesNbr, researchFileRequestDate, researchFileRequester,
                researchFileDescriptionRequest, researchFileResearchCompletedDate, researchFileResultRequest, researchFileExpropiationNo, researchFileExpropiationNotes);

            //Save Research File
            researchFile.SaveResearchFile();

            //Verify Research File Details View Form
            researchFile.VerifyResearchFileMainFormView(researchFileRoadName, researchFileAliasName, researchFileVerifyRequestDate, researchFileRequester, researchFileVerifyResearchCompletedDate, researchFileExpropiationNo);
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
            /* TEST COVERAGE: PSP-3266, PSP-3721, PSP-4333, PSP-3597, PSP-3596, PSP-3595, PSP-3849, PSP-3598, PSP-3600 */

            //Navigate to Edit Research File
            researchFile.NavigateToAddPropertiesReseachFile();

            //Verify UI/UX from Search By Component
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

            //Save Research File
            researchFile.SaveResearchFile();

            //Confirm saving changes
            researchFile.ConfirmChangesResearchFile();

        }

        [StepDefinition(@"I search for an existing Research File")]
        public void SearchExistingResearchFileProperties()
        {
            /* TEST COVERAGE: PSP-3294, PSP-4197 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Research File Search
            searchResearchFile.NavigateToSearchResearchFile();

            //Filter research Files
            searchResearchFile.FilterResearchFiles("South Coast Region", "Automated", "Active", "Happy", "Tranpsp1");
            Assert.True(searchResearchFile.SearchFoundResults());

            searchResearchFile.FilterResearchFiles("Cannot determine", "Automated", "Closed", "Happy", "dsmith");
            Assert.False(searchResearchFile.SearchFoundResults());

            //Look for the last created research file
            searchResearchFile.SearchLastResearchFile();

        }

        [StepDefinition(@"I update and cancel existing research file form")]
        public void UpdateCancelExistingResearchFileForm()
        {
            /* TEST COVERAGE: PSP-3359 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Research File Search and look for the last research file created
            searchResearchFile.NavigateToSearchResearchFile();
            searchResearchFile.SearchLastResearchFile();

            //Select first result
            searchResearchFile.SelectFirstResult();

            //Edit the research file
            researchFile.EditResearchFileForm();

            //Cancel changes
            researchFile.CancelResearchFile();
        }

        [StepDefinition(@"I update an existing research file properties")]
        public void UpdateExistingResearchFileProperties()
        {
            /* TEST COVERAGE: PSP-3460, PSP-3599, PSP-3600, PSP-3612, PSP-3722, PSP-3462 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Research File Search and look for the last research file created
            searchResearchFile.NavigateToSearchResearchFile();
            searchResearchFile.SearchLastResearchFile();

            //Select 1st Search Result
            searchResearchFile.SelectFirstResult();

            //Navigate to Edit Research File
            researchFile.NavigateToAddPropertiesReseachFile();

            //Add existing property again
            sharedSearchProperties.NavigateToSearchTab();

            sharedSearchProperties.VerifySearchPropertiesFeature();
            sharedSearchProperties.SelectPropertyByPID(PID1Search);
            sharedSearchProperties.SelectFirstOption();

            //Delete first property
            sharedSearchProperties.DeleteProperty();

            //Save changes
            researchFile.SaveResearchFile();

            //Confirm changes
            researchFile.ConfirmChangesResearchFile();

            //Select 1st Property attached
            researchFile.ChooseFirstPropertyOption();

            //Add information
            researchFile.AddPropertyResearchMaxInfo(propertyResearchPropName, propertyResearchDocRef, propertyResearchNotes);

            //Save changes
            researchFile.SaveResearchFile();

            //Edit 1st Property Prop Research Tab
            researchFile.EditPropertyResearch(propertyResearchEditNotes);

            //Save changes
            researchFile.SaveResearchFile();

        }

        [StepDefinition(@"I update and cancel changes on existing research file properties")]
        public void UpdateCancelExistingResearchFileProperties()
        {
            /* TEST COVERAGE: PSP-3720, PSP-3463, PSP-3601  */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Research File Search and look for the last research file created
            searchResearchFile.NavigateToSearchResearchFile();
            searchResearchFile.SearchLastResearchFile();

            //Select 1st Search Result
            searchResearchFile.SelectFirstResult();

            //Select 1st Property attached
            researchFile.ChooseFirstPropertyOption();

            //Insert some changes
            researchFile.AddPropertyResearchMinInfo(propertyResearchPropName);

            //Cancel changes
            researchFile.CancelResearchFile();

            //Verify PIMS Files Tab
            propertyInformation.VerifyPimsFiles();

            //Navigate to Edit Research File
            researchFile.NavigateToAddPropertiesReseachFile();

            //Delete first property
            sharedSearchProperties.DeleteProperty();

            //Cancel changes
            researchFile.CancelResearchFileProps();

        }

        [StepDefinition(@"I update Property Details from a Research File")]
        public void UpdatePropertyDetails()
        {
            /* TEST COVERAGE: PSP-4791 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Research File Search and look for the last research file created
            searchResearchFile.NavigateToSearchResearchFile();
            searchResearchFile.SearchLastResearchFile();

            //Select 1st Search Result
            searchResearchFile.SelectFirstResult();

            //Select 1st Property attached
            researchFile.ChooseFirstPropertyOption();

            //Navigate to Property Information Tab
            propertyInformation.NavigatePropertyDetailsTab();

            //Click on the Edit Property Information Button
            propertyInformation.EditPropertyInfoResearchBttn();

            //Insert some changes
            propertyInformation.UpdateMaxPropertyDetails(propertyDetailsMunicipalZoning, propertyDetailsAreaSqMts, propertyDetailsAreaCubeMts, propertyDetailsNotes);

            //Save changes
            propertyInformation.SavePropertyDetails();

        }

        [StepDefinition(@"I cancel changes on a Property Details from a Research File")]
        public void CancelChangesPropertyDetails()
        {
            /* TEST COVERAGE: PSP-5005 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Research File Search and look for the last research file created
            searchResearchFile.NavigateToSearchResearchFile();
            searchResearchFile.SearchLastResearchFile();

            //Select 1st Search Result
            searchResearchFile.SelectFirstResult();

            //Select 1st Property attached
            researchFile.ChooseFirstPropertyOption();

            //Navigate to Property Information Tab
            propertyInformation.NavigatePropertyDetailsTab();

            //Click on the Edit Property Information Button
            propertyInformation.EditPropertyInfoResearchBttn();

            //Insert some changes
            propertyInformation.UpdateMinPropertyDetails(propertyDetailsNotesChanges);

            //Cancel changes
            researchFile.CancelResearchFilePropertyDetails();
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

        [StepDefinition(@"Property Research Tab has been updated successfully")]
        public void PropertyResearchEditSuccess()
        {
            

            //Verify Property Research Tab View
            researchFile.VerifyPropResearchTabFormView(propertyResearchPropName, propertyResearchDocRef);
        }

        [StepDefinition(@"Research File View Form renders successfully")]
        public void VerifyResearchFileDetailsView()
        {
             /* TEST COVERAGE: PSP-3367 */

            //Verify Research File Details View Form
            researchFile.VerifyResearchFileMainFormView(researchFileRoadName, researchFileAliasName, researchFileVerifyRequestDate, researchFileRequester, researchFileVerifyResearchCompletedDate, researchFileExpropiationNo);
        }

        [StepDefinition(@"The create research file form is no longer displayed")]
        public void CancelCreateSuccessful()
        {
            /* TEST COVERAGE: PSP-3359 */
 
            Assert.True(researchFile.HeaderIsDisplayed().Equals(0));
        }

        [StepDefinition(@"Expected Content is displayed on Research File Table")]
        public void VerifyResearchFileTableContent()
        {
            /* TEST COVERAGE: PSP-3294  */

            searchResearchFile.VerifyResearchFileListView();
            searchResearchFile.VerifyResearchFileTableContent(researchFileName);

        }

        [StepDefinition(@"Research File Properties remain unchanged")]
        public void VerifyPropertiesCount()
        {
            /* TEST COVERAGE: PSP-3463 */

            Assert.False(researchFile.PropertiesCountChange());
        }

        [StepDefinition(@"Property Information is displayed correctly")]
        public void PropertyInformationViewDetailsSuccess()
        {
            /* TEST COVERAGE: PSP-4794 */
            propertyInformation.NavigatePropertyDetailsTab();
            propertyInformation.VerifyPropertyDetailsView("Research File");
        }
    }
}
