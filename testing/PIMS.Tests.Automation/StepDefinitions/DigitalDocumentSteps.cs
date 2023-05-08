using Microsoft.Extensions.Configuration;

namespace PIMS.Tests.Automation.StepDefinitions
{
    [Binding]
    public class DigitalDocumentSteps
    {
        private readonly SharedDocumentsTab sharedDocumentsTab;
        private readonly IEnumerable<DocumentFile> documentFiles;

        private string dateSigned = "03/10/2020";
        private string motiFile = "1245-6778";
        private string pid = "004-537-360";
        private string roadName = "Automated Test Road";
        private string transferNbr = "9090876-1234";

        private string gazettePage = "67";
        private string publishedDate = "04/15/2020";
        private string gazetteType = "Testing Gazette";
        private string legalPlan = "334-090980";
        private string LTSASchedule = "23-0909";
        private string motiPlan = "234-898999";

        protected int totalDigitalDocumentsUploaded = 0;

        public DigitalDocumentSteps(BrowserDriver driver)
        {
            sharedDocumentsTab = new SharedDocumentsTab(driver.Current);
            documentFiles = driver.Configuration.GetSection("UploadDocuments").Get<IEnumerable<DocumentFile>>();
        }


        [StepDefinition(@"I manage Documents Tab within a Research File")]
        public void DocumentTabResearchFile()
        {
            /* TEST COVERAGE: PSP-4159, PSP-4339, PSP-4340, PSP-4341 PSP-4342, PSP-4343, PSP-4344, PSP-4345, PSP-4346, PSP-4347, PSP-4348, PSP-4349, PSP-4350, PSP-4351, PSP-4352, PSP-4353, PSP-4354, PSP-4355, PSP-4356, PSP-4357 */

            //Access the documents tab
            sharedDocumentsTab.NavigateDocumentsTab();

            //Get total uploaded documents
            totalDigitalDocumentsUploaded = sharedDocumentsTab.GetTotalUploadedDocuments();

            //Add a New Document
            sharedDocumentsTab.AddNewDocument();

            //Verify different types of document types
            sharedDocumentsTab.VerifyDocumentFields("BC assessment search");
            sharedDocumentsTab.VerifyDocumentFields("Privy council");
            sharedDocumentsTab.VerifyDocumentFields("Photos / Images/ Video");
            sharedDocumentsTab.VerifyDocumentFields("PA plans / Design drawings");
            sharedDocumentsTab.VerifyDocumentFields("Other");
            sharedDocumentsTab.VerifyDocumentFields("OIC");
            sharedDocumentsTab.VerifyDocumentFields("MoTI plan");

            //Upload one digital document
            Random random = new Random();
            var index = random.Next(0, documentFiles.Count());
            var document = documentFiles.ElementAt(index);

            sharedDocumentsTab.UploadDocument(document.Url);
            sharedDocumentsTab.UploadTransferAdminFile(dateSigned, motiFile, pid, roadName, transferNbr);

            //Save digital document
            sharedDocumentsTab.SaveDigitalDocument();
        }

        [StepDefinition(@"I manage Documents Tab within an Acquisition File")]
        public void DocumentTabAcquisitionFile()
        {
            /* TEST COVERAGE: PSP-4159, PSP-4339, PSP-4340, PSP-4341 PSP-4342, PSP-4343, PSP-4344, PSP-4345, PSP-4346, PSP-4347, PSP-4348, PSP-4349, PSP-4350, PSP-4351, PSP-4352, PSP-4353, PSP-4354, PSP-4355, PSP-4356, PSP-4357 */

            //Access the documents tab
            sharedDocumentsTab.NavigateDocumentsTab();

            //Get total uploaded documents
            totalDigitalDocumentsUploaded = sharedDocumentsTab.GetTotalUploadedDocuments();

            //Add a New Document
            sharedDocumentsTab.AddNewDocument();

            //Verify different types of document types
            sharedDocumentsTab.VerifyDocumentFields("BC assessment search");
            sharedDocumentsTab.VerifyDocumentFields("Privy council");
            sharedDocumentsTab.VerifyDocumentFields("Photos / Images/ Video");
            sharedDocumentsTab.VerifyDocumentFields("PA plans / Design drawings");
            sharedDocumentsTab.VerifyDocumentFields("Other");
            sharedDocumentsTab.VerifyDocumentFields("OIC");
            sharedDocumentsTab.VerifyDocumentFields("MoTI plan");
            sharedDocumentsTab.VerifyDocumentFields("Miscellaneous notes (LTSA)");
            sharedDocumentsTab.VerifyDocumentFields("Ministerial order");
            sharedDocumentsTab.VerifyDocumentFields("Title search / Historical title");
            sharedDocumentsTab.VerifyDocumentFields("Legal survey plan");
            sharedDocumentsTab.VerifyDocumentFields("Historical file");
            sharedDocumentsTab.VerifyDocumentFields("Gazette");
            sharedDocumentsTab.VerifyDocumentFields("Field notes");
            sharedDocumentsTab.VerifyDocumentFields("District road register");
            sharedDocumentsTab.VerifyDocumentFields("Crown grant");
            sharedDocumentsTab.VerifyDocumentFields("Correspondence");
            sharedDocumentsTab.VerifyDocumentFields("Canada lands survey");
            sharedDocumentsTab.VerifyDocumentFields("Transfer of administration");

            //Upload one digital document
            Random random = new Random();
            var index = random.Next(0, documentFiles.Count());
            var document = documentFiles.ElementAt(index);

            sharedDocumentsTab.UploadDocument(document.Url);
            sharedDocumentsTab.UploadTransferAdminFile(dateSigned, motiFile, pid, roadName, transferNbr);

            //Save digital document
            sharedDocumentsTab.SaveDigitalDocument();
        }

        [StepDefinition(@"I manage Documents Tab within a Lease")]
        public void DocumentTabLeaseLicense()
        {
            /* TEST COVERAGE: PSP-4159, PSP-4339, PSP-4340, PSP-4341 PSP-4342, PSP-4343, PSP-4344, PSP-4345, PSP-4346, PSP-4347, PSP-4348, PSP-4349, PSP-4350, PSP-4351, PSP-4352, PSP-4353, PSP-4354, PSP-4355, PSP-4356, PSP-4357 */

            //Access the documents tab
            sharedDocumentsTab.NavigateDocumentsTab();

            //Get total uploaded documents
            totalDigitalDocumentsUploaded = sharedDocumentsTab.GetTotalUploadedDocuments();

            //Add a New Document
            sharedDocumentsTab.AddNewDocument();

            //Verify different types of document types
            sharedDocumentsTab.VerifyDocumentFields("BC assessment search");
            sharedDocumentsTab.VerifyDocumentFields("Privy council");
            sharedDocumentsTab.VerifyDocumentFields("Photos / Images/ Video");
            sharedDocumentsTab.VerifyDocumentFields("PA plans / Design drawings");
            sharedDocumentsTab.VerifyDocumentFields("Other");
            sharedDocumentsTab.VerifyDocumentFields("OIC");
            sharedDocumentsTab.VerifyDocumentFields("MoTI plan");
            sharedDocumentsTab.VerifyDocumentFields("Miscellaneous notes (LTSA)");
            sharedDocumentsTab.VerifyDocumentFields("Ministerial order");
            sharedDocumentsTab.VerifyDocumentFields("Title search / Historical title");
            sharedDocumentsTab.VerifyDocumentFields("Legal survey plan");
            sharedDocumentsTab.VerifyDocumentFields("Historical file");
            sharedDocumentsTab.VerifyDocumentFields("Gazette");
            sharedDocumentsTab.VerifyDocumentFields("Field notes");
            sharedDocumentsTab.VerifyDocumentFields("District road register");
            sharedDocumentsTab.VerifyDocumentFields("Crown grant");
            sharedDocumentsTab.VerifyDocumentFields("Correspondence");
            sharedDocumentsTab.VerifyDocumentFields("Canada lands survey");
            sharedDocumentsTab.VerifyDocumentFields("Transfer of administration");

            //Upload one digital document
            Random random = new Random();
            var index = random.Next(0, documentFiles.Count());
            var document = documentFiles.ElementAt(index);

            sharedDocumentsTab.UploadDocument(document.Url);
            sharedDocumentsTab.UploadTransferAdminFile(dateSigned, motiFile, pid, roadName, transferNbr);

            //Save digital document
            sharedDocumentsTab.SaveDigitalDocument();
        }

        [StepDefinition(@"I edit an attached document")]
        public void ActivityWithDocumentEdited()
        {
            /* TEST COVERAGE:  PSP-4030, PSP-4168, PSP-4335, PSP-4336, PSP-4338 */

            //Access the documents tab
            sharedDocumentsTab.NavigateDocumentsTab();

            //Add a New Document
            sharedDocumentsTab.AddNewDocument();

            //Upload one digital document
            Random random = new Random();
            var index = random.Next(0, documentFiles.Count());
            var document = documentFiles.ElementAt(index);

            sharedDocumentsTab.UploadDocument(document.Url);
            sharedDocumentsTab.UploadGazetteFile(dateSigned, gazettePage, publishedDate, gazetteType, legalPlan, LTSASchedule, motiPlan, roadName);

            //Save digital document
            sharedDocumentsTab.SaveDigitalDocument();

            //Get total uploaded documents
            totalDigitalDocumentsUploaded = sharedDocumentsTab.GetTotalUploadedDocuments();

            //Add new digital document
            sharedDocumentsTab.AddNewDocument();
            var index2 = random.Next(0, documentFiles.Count());
            var document2 = documentFiles.ElementAt(index2);

            sharedDocumentsTab.UploadDocument(document2.Url);
            sharedDocumentsTab.UploadTransferAdminFile(dateSigned, motiFile, pid, roadName, transferNbr);

            //Cancel uploading a new document
            sharedDocumentsTab.CancelDigitalDocument();

            //Edit digital document's details
            sharedDocumentsTab.View1stDocument();
            sharedDocumentsTab.EditDocument();

            //Verify Edit Form
            sharedDocumentsTab.VerifyGazetteEditForm();

            //Make changes on the document details
            sharedDocumentsTab.ChangeStatus("Approved");

            //Save document's changes
            sharedDocumentsTab.SaveEditDigitalDocument();

            //Edit digital document's details
            sharedDocumentsTab.View1stDocument();
            sharedDocumentsTab.EditDocument();
            sharedDocumentsTab.ChangeStatus("None");

            //Cancel digital document's details
            sharedDocumentsTab.CancelEditDigitalDocument();

            //Close Digital Documents Details View
            sharedDocumentsTab.CloseDigitalDocumentViewDetails();

            //Delete digital document
            sharedDocumentsTab.Delete1stDocument();
        }

        [StepDefinition(@"A digital document has been uploaded successfully")]
        public void DocumentUploadSuccess()
        {
            Assert.True(sharedDocumentsTab.GetTotalUploadedDocuments() == totalDigitalDocumentsUploaded + 1);
        }

        [StepDefinition(@"A digital document has been deleted successfully")]
        public void DocumentDeleteSuccess()
        {
            Assert.True(sharedDocumentsTab.GetTotalUploadedDocuments() == 0);
        }
    }
}

public class DocumentFile
{
    public string Url { get; set; } = null!;
}

