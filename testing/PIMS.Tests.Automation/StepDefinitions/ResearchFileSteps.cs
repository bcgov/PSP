

using PIMS.Tests.Automation.Classes;
using PIMS.Tests.Automation.Data;
using System.Data;

namespace PIMS.Tests.Automation.StepDefinitions
{
    [Binding]
    public class ResearchFileSteps
    {
        private readonly LoginSteps loginSteps;
        private readonly ResearchFiles researchFiles;
        private readonly SharedSearchProperties sharedSearchProperties;
        private readonly SearchResearchFiles searchResearchFile;
        private readonly PropertyInformation propertyInformation;
        private readonly SearchProperties searchProperties;
        private readonly Notes notes;
        private readonly GenericSteps genericSteps;

        private readonly string userName = "TRANPSP1";
        //private readonly string userName = "SUTAIRAK";

        private ResearchFile researchFile;
        protected string researchFileCode = "";

        public ResearchFileSteps(BrowserDriver driver)
        {
            loginSteps = new LoginSteps(driver);
            researchFiles = new ResearchFiles(driver.Current);
            sharedSearchProperties = new SharedSearchProperties(driver.Current);
            searchResearchFile = new SearchResearchFiles(driver.Current);
            propertyInformation = new PropertyInformation(driver.Current);
            searchProperties = new SearchProperties(driver.Current);
            genericSteps = new GenericSteps(driver);
            notes = new Notes(driver.Current);
            researchFile = new ResearchFile();
        }

        [StepDefinition(@"I create a new complete Research File from row number (.*)")]
        public void CreateResearchFile(int rowNumber)
        {
            /* TEST COVERAGE: PSP-3266, PSP-3267, PSP-3357, PSP-3358, PSP-3367, PSP-3595, PSP-3596, PSP-3597, PSP-3598, PSP-3600, 
             * PSP-3721, PSP-3849, PSP-4333, PSP-4556, PSP-5360, PSP-5541, PSP-5545, PSP-6303 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Research File
            researchFiles.NavigateToCreateNewResearchFile();

            //Create basic Research File
            PopulateResearchFile(rowNumber);
            researchFiles.VerifyResearchFileCreateInitForm();
            researchFiles.CreateResearchFile(researchFile);

            //Save Research File
            researchFiles.SaveResearchFile();

            //Get Research File code
            researchFileCode = researchFiles.GetResearchFileCode();

            //Add additional info to the reseach File
            researchFiles.VerifyResearchFileEditInitForm(researchFile, userName);
            researchFiles.AddAdditionalResearchFileInfo(researchFile);

            //Save Research File
            researchFiles.SaveResearchFile();

            //Verify Research File Details View Form
            researchFiles.VerifyResearchFileMainFormView(researchFile, userName);

            //Verify automatic note created when
            if (researchFile.Status != "Active")
            {
                notes.NavigateNotesTab();
                notes.VerifyAutomaticNotes("Research File", "Active", researchFile.Status);
            }

            //Navigate to Edit Research File
            researchFiles.NavigateToAddPropertiesReseachFile();

            //Verify UI/ UX from Search By Component
            sharedSearchProperties.NavigateToSearchTab();
            sharedSearchProperties.VerifySearchPropertiesFeature();

            //Search for a property by PID
            if (researchFile.SearchProperties.PID != "")
            {
                sharedSearchProperties.SelectPropertyByPID(researchFile.SearchProperties.PID);
                sharedSearchProperties.SelectFirstOption();
            }
            //Search for a property by PIN
            if (researchFile.SearchProperties.PIN != "")
            {
                sharedSearchProperties.SelectPropertyByPIN(researchFile.SearchProperties.PIN);
                sharedSearchProperties.SelectFirstOption();
            }
            //Search for a property by Plan
            if (researchFile.SearchProperties.PlanNumber != "")
            {
                sharedSearchProperties.SelectPropertyByPlan(researchFile.SearchProperties.PlanNumber);
                sharedSearchProperties.SelectFirstOption();
            }
            //Search for a property by Address
            if (researchFile.SearchProperties.Address != "")
            {
                sharedSearchProperties.SelectPropertyByAddress(researchFile.SearchProperties.Address);
                sharedSearchProperties.SelectFirstOption();
            }
            //Search for a property by Legal Description
            if (researchFile.SearchProperties.LegalDescription != "")
            {
                sharedSearchProperties.SelectPropertyByLegalDescription(researchFile.SearchProperties.LegalDescription);
                sharedSearchProperties.SelectFirstOption();
            }

            //Save Research File
            researchFiles.SaveResearchFileProperties();

            //Add Property Research Information
            if (researchFile.PropertyResearchRowEnd != 0 && researchFile.PropertyResearchRowStart != 0)
            {
                for (int i = 0; i < researchFile.PropertyResearchRowEnd; i++)
                {
                    researchFiles.AddPropertyResearchInfo(researchFile.PropertyResearch[i], i);
                    researchFiles.SaveResearchFile();
                    researchFiles.VerifyPropResearchTabFormView(researchFile.PropertyResearch[i]);
                }
            }

            //Go back to Research File Details
            researchFiles.NavigateToFileSummary();
        }

        [StepDefinition(@"I create a Research File from a pin on map and from row number (.*)")]
        public void CreateResearchFileFromPin(int rowNumber)
        {
            /* TEST COVERAGE: PSP-3371, PSP-1546, PSP-1556 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Search for a property
            PopulateResearchFile(rowNumber);
            searchProperties.SearchPropertyByPINPID(researchFile.SearchProperties.PID);

            //Select found property on Map
            searchProperties.SelectFoundPin();

            //Close Property Information Modal
            propertyInformation.ClosePropertyInfoModal();

            //Open elipsis option
            propertyInformation.OpenMoreOptionsPopUp();
            propertyInformation.ChooseCreationOptionFromPin("Research File - Create new");

            //Fill basic Research File information
            researchFiles.CreateResearchFile(researchFile);

            //Fill name to selected property
            sharedSearchProperties.AddNameSelectedProperty("Automated Property from Pin");

            //Save Research File
            researchFiles.SaveResearchFile();

            //Get Research File code
            researchFileCode = researchFiles.GetResearchFileCode();

            //Add additional info to the reseach File
            researchFiles.AddAdditionalResearchFileInfo(researchFile);

            //Save Research File
            researchFiles.SaveResearchFile();
        }

        [StepDefinition(@"I cancel changes done on Research Files from row number (.*)")]
        public void CreateCancelResearchFile(int rowNumber)
        {
            /* TEST COVERAGE: PSP-3268, PSP-3294, PSP-3359, PSP-3463, PSP-3596, PSP-3597, PSP-3598, PSP-3601, PSP-3720, PSP-3721, PSP-4197, PSP-4556, PSP-5005 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Research File
            researchFiles.NavigateToCreateNewResearchFile();

            //Create basic Research File
            PopulateResearchFile(rowNumber);
            researchFiles.CreateResearchFile(researchFile);

            //Cancel Research File
            researchFiles.CancelResearchFile();

            //Navigate to Manage Research File
            searchResearchFile.NavigateToSearchResearchFile();

            //Create basic Research File
            searchResearchFile.SelectFirstResult();

            //Edit Research File
            researchFiles.EditResearchFileForm(researchFile);

            //Cancel Research File
            researchFiles.CancelResearchFile();

            //Add a Property to the research File
            researchFiles.NavigateToAddPropertiesReseachFile();

            //Search for a property by PID
            sharedSearchProperties.NavigateToSearchTab();
            sharedSearchProperties.SelectPropertyByPID(researchFile.SearchProperties.PID);

            //Verify PID doesn't exist
            Assert.True(sharedSearchProperties.noRowsResultsMessage().Equals("No results found for your search criteria."));

            //Search for a property by Legal Description
            sharedSearchProperties.NavigateToSearchTab();
            sharedSearchProperties.SelectPropertyByLegalDescription(researchFile.SearchProperties.LegalDescription);

            //Verify more than 15 properties found result
            Assert.True(sharedSearchProperties.noRowsResultsMessage().Equals("Too many results (more than 15) match this criteria. Please refine your search."));

            //Cancel changes on a Property Detail on Research File
            researchFiles.CancelResearchFile();

            //Insert changes on Property Information
            researchFiles.AddPropertyResearchInfo(researchFile.PropertyResearch[0], 0);

            //Cancel changes
            researchFiles.CancelResearchFile();

            //Verify PIMS Files Tab
            propertyInformation.VerifyPimsFiles();

            //Navigate to Edit Research File
            researchFiles.NavigateToAddPropertiesReseachFile();

            //Delete first property
            sharedSearchProperties.DeleteProperty();

            //Cancel changes
            researchFiles.CancelResearchFileProps();

            //Navigate to Research File Search
            searchResearchFile.NavigateToSearchResearchFile();

            //Filter research Files
            searchResearchFile.FilterResearchFiles(researchFile.ResearchFileName, researchFile.Status, researchFile.RoadName, "TRANPSP1");
            Assert.True(searchResearchFile.SearchFoundResults());

            searchResearchFile.FilterResearchFiles("Automated", "Closed", "Happy", "TRANPSP1");
        }

        [StepDefinition(@"I update an Existing Research File from row number (.*)")]
        public void UpdateExistingResearchFile(int rowNumber)
        {
            /* TEST COVERAGE: PSP-3460, PSP-3599, PSP-3600, PSP-3612, PSP-3722, PSP-3462, PSP-4791, PSP-5547 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Research File Search and look for the last research file created
            PopulateResearchFile(rowNumber);
            searchResearchFile.NavigateToSearchResearchFile();
            searchResearchFile.SearchLastResearchFile();

            //Select 1st Search Result
            searchResearchFile.SelectFirstResult();

            //Edit Research File Details
            researchFiles.EditResearchFileForm(researchFile);

            //Save Changes
            researchFiles.SaveResearchFile();

            //Navigate to Edit Research File
            researchFiles.NavigateToAddPropertiesReseachFile();

            //Add existing property again
            sharedSearchProperties.NavigateToSearchTab();

            sharedSearchProperties.VerifySearchPropertiesFeature();
            sharedSearchProperties.SelectPropertyByPID(researchFile.SearchProperties.PID);
            sharedSearchProperties.SelectFirstOption();

            //Delete first property
            sharedSearchProperties.DeleteProperty();

            //Save changes
            researchFiles.SaveResearchFileProperties();

            //Select 1st Property attached
            researchFiles.ChooseFirstPropertyOption();

            //Edit Information
            if (researchFile.PropertyResearchRowEnd != 0 && researchFile.PropertyResearchRowStart != 0)
            {
                for (int i = 0; i < researchFile.PropertyResearch.Count; i++)
                {
                    researchFiles.EditPropertyResearchInfo(researchFile.PropertyResearch[i], i);
                    researchFiles.SaveResearchFile();
                    researchFiles.VerifyPropResearchTabFormView(researchFile.PropertyResearch[i]);
                }
            }
        }

        [StepDefinition(@"I navigate back to the Research File Summary")]
        public void NavigateMainResearchFileSection()
        {
            //Navigate back to File Summary
            researchFiles.NavigateToFileSummary();
        }

       [StepDefinition(@"A new Research File is created successfully")]
       public void NewResearchFileCreated()
       {
           /* TEST COVERAGE: PSP-4556, PSP-3294 */

            searchResearchFile.NavigateToSearchResearchFile();
            searchResearchFile.SearchResearchFileByRFile(researchFileCode);

            searchResearchFile.VerifyResearchFileListView();
            searchResearchFile.VerifyResearchFileTableContent(researchFile, userName);
        }

        [StepDefinition(@"Research File Properties remain unchanged")]
        public void SearchResearchFileResult()
        {
            Assert.False(searchResearchFile.SearchFoundResults());
        }

        private void PopulateResearchFile(int rowNumber)
        {
            DataTable researchFileSheet = ExcelDataContext.GetInstance().Sheets["ResearchFiles"];
            ExcelDataContext.PopulateInCollection(researchFileSheet);

            researchFile.ResearchFileName = ExcelDataContext.ReadData(rowNumber, "ResearchFileName");
            researchFile.Status = ExcelDataContext.ReadData(rowNumber, "Status");
            researchFile.Projects = genericSteps.PopulateLists(ExcelDataContext.ReadData(rowNumber, "Projects"));

            researchFile.RoadName = ExcelDataContext.ReadData(rowNumber, "RoadName");
            researchFile.RoadAlias = ExcelDataContext.ReadData(rowNumber, "RoadAlias");

            researchFile.ResearchPurpose = genericSteps.PopulateLists(ExcelDataContext.ReadData(rowNumber, "ResearchPurpose"));
            researchFile.RequestDate = ExcelDataContext.ReadData(rowNumber, "RequestDate");
            researchFile.RequestSource = ExcelDataContext.ReadData(rowNumber, "RequestSource");
            researchFile.Requester = ExcelDataContext.ReadData(rowNumber, "Requester");
            researchFile.RequestDescription = ExcelDataContext.ReadData(rowNumber, "RequestDescription");

            researchFile.ResearchCompletedDate = ExcelDataContext.ReadData(rowNumber, "ResearchCompletedDate");
            researchFile.RequestResult = ExcelDataContext.ReadData(rowNumber, "RequestResult");

            researchFile.Expropriation = bool.Parse(ExcelDataContext.ReadData(rowNumber, "Expropriation"));
            researchFile.ExpropriationNotes = ExcelDataContext.ReadData(rowNumber, "ExpropriationNotes");

            researchFile.SearchPropertiesIndex = int.Parse(ExcelDataContext.ReadData(rowNumber, "SearchPropertiesIndex"));
            researchFile.PropertyResearchRowStart = int.Parse(ExcelDataContext.ReadData(rowNumber, "PropertyReasearchRowStart"));
            researchFile.PropertyResearchRowEnd = int.Parse(ExcelDataContext.ReadData(rowNumber, "PropertyResearchRowEnd"));

            if (researchFile.SearchPropertiesIndex > 0)
            {
                DataTable searchPropertiesSheet = ExcelDataContext.GetInstance().Sheets["SearchProperties"];
                ExcelDataContext.PopulateInCollection(searchPropertiesSheet);

                researchFile.SearchProperties.PID = ExcelDataContext.ReadData(researchFile.SearchPropertiesIndex, "PID");
                researchFile.SearchProperties.PIN = ExcelDataContext.ReadData(researchFile.SearchPropertiesIndex, "PIN");
                researchFile.SearchProperties.Address = ExcelDataContext.ReadData(researchFile.SearchPropertiesIndex, "Address");
                researchFile.SearchProperties.PlanNumber = ExcelDataContext.ReadData(researchFile.SearchPropertiesIndex, "PlanNumber");
                researchFile.SearchProperties.LegalDescription = ExcelDataContext.ReadData(researchFile.SearchPropertiesIndex, "LegalDescription");
            }
            if (researchFile.PropertyResearchRowEnd != 0 && researchFile.PropertyResearchRowStart != 0)
            {
                PopulatePropertyResearchCollection(researchFile.PropertyResearchRowStart, researchFile.PropertyResearchRowEnd);
            }
        }

        private void PopulatePropertyResearchCollection(int startRow, int rowsCount)
        {
            DataTable propertyResearchSheet = ExcelDataContext.GetInstance().Sheets["PropertyResearch"];
            ExcelDataContext.PopulateInCollection(propertyResearchSheet);

            for (int i = startRow; i <= rowsCount; i++)
            {
                PropertyResearch propertyResearch = new PropertyResearch();
                propertyResearch.DescriptiveName = ExcelDataContext.ReadData(i, "DescriptiveName");
                propertyResearch.Purpose = ExcelDataContext.ReadData(i, "Purpose");
                propertyResearch.LegalOpinionRequest = ExcelDataContext.ReadData(i, "LegalOpinionRequest");
                propertyResearch.LegalOpinionObtained = ExcelDataContext.ReadData(i, "LegalOpinionObtained");
                propertyResearch.DocumentReference = ExcelDataContext.ReadData(i, "DocumentReference");
                propertyResearch.SummaryNotes = ExcelDataContext.ReadData(i, "SummaryNotes");

                researchFile.PropertyResearch.Add(propertyResearch);
            }
        }
    }
}
