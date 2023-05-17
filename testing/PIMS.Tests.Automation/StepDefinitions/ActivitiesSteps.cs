using Microsoft.Extensions.Configuration;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.StepDefinitions
{
    [Binding]
    public class ActivitiesSteps
    {
        private readonly Activities activities;
        private readonly Notes notes;
        private readonly DigitalDocuments digitalDocuments;
        private readonly IEnumerable<DocumentFile> documentFiles;

        private string activityDescription = "Automated Test - Description on activity";
        private string note1 = "Automated Test - Inserting a new note";
        private string note2 = "Automated Test - Editing existing note";
        private string note3 = "Automated Test - Editing existing note for second time";

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

        public ActivitiesSteps(BrowserDriver driver)
        {
            //activities = new Activities(driver.Current);
            notes = new Notes(driver.Current);
            digitalDocuments = new DigitalDocuments(driver.Current);
            documentFiles = driver.Configuration.GetSection("UploadDocuments").Get<IEnumerable<DocumentFile>>();
        }

        //[StepDefinition(@"I create a new activity")]
        //public void CreateActivityWithproperties()
        //{
        //    /* TEST COVERAGE:  PSP-4364, PSP-4363, PSP-4361, PSP-4360, PSP-4169, PSP-4170, PSP-4683, PSP-4478, PSP-4479, PSP-4275 */

        //    //Acess the activity tab
        //    activities.AccessActivitiesTab();

        //    //Create new activity
        //    activities.CreateNewActivity("General");

        //    //Verify activities list view
        //    activities.VerifyActivityListView();

        //    //Verify activity Details view
        //    activities.VerifyActivityDetails();

        //    //Filter Activities
        //    activities.FilterActivities();

        //    //Select All Properties
        //    activities.SelectAllProperties();

        //    //Deselect All Properties
        //    activities.DesectAllProperties();
        //}

        //[StepDefinition(@"I create and delete an activity")]
        //public void CreateDeleteActivity()
        //{
        //    /* TEST COVERAGE:  PSP-4275, PSP-4362, PSP-4477, PSP-4784, PSP-4785  */

        //    //Acess the activity tab
        //    activities.AccessActivitiesTab();

        //    //Create new activity
        //    activities.CreateNewActivity("General");

        //    //Verify activities list view
        //    activities.VerifyActivityListView();

        //    //Verify activity Details view
        //    activities.VerifyActivityDetails();

        //    //Check No Properties pop-up
        //    activities.NoProperties();

        //    //Change Status to Completed
        //    activities.ChangeStatus("Completed");
        //    Assert.True(activities.IsActivityBlocked());
        //    activities.CancelActivityChanges();

        //    //Change Status to In Progress
        //    activities.ChangeStatus("In Progress");
        //    Assert.False(activities.IsActivityBlocked());
        //    activities.CancelActivityChanges();

        //    //Change Status to Cancel
        //    activities.ChangeStatus("Cancelled");
        //    Assert.True(activities.IsActivityBlocked());
        //    activities.CancelActivityChanges();

        //    //Delete Activity
        //    activities.DeleteActivity();

        //}

        //[StepDefinition(@"I create an activity with notes and delete notes")]
        //public void ActivityWithNotes()
        //{
        //    /* TEST COVERAGE:  PSP-4006, PSP-4008, PSP-4009, PSP-4010, PSP-4012, PSP-4013, PSP-4019, PSP-4021 */

        //    //Acess the activity tab
        //    activities.AccessActivitiesTab();

        //    //Create new activity
        //    activities.CreateNewActivity("File Document");

        //    //Add a new note
        //    notes.AddNewNote();

        //    //Verify New Note Create Form
        //    notes.VerifyNotesAddNew();

        //    //Create New Note
        //    notes.AddNewNoteDetails(note1);

        //    //Save New Note
        //    notes.SaveNote();

        //    //Edit existing note
        //    notes.ViewFirstNoteButton();

        //    //Verify edit note form
        //    notes.VerifyNotesEditForm();

        //    //Save edited note
        //    notes.EditNote(note2);
        //    notes.SaveNote();

        //    //Edit existed note
        //    notes.ViewFirstNoteButton();
        //    notes.EditNote(note3);

        //    //Cancel edited note
        //    notes.CancelNote();

        //    //Create new note
        //    notes.AddNewNote();
        //    notes.AddNewNoteDetails(note1);

        //    //Cancel new note
        //    notes.CancelNote();

        //    //Verify List view UI/UX
        //    notes.VerifyNotesListView();

        //    //Delete created note
        //    notes.DeleteFirstNote();
        //}

        //[StepDefinition(@"I create an Acquisition File with activity and a document attached")]
        //public void ActivityWithDocuments()
        //{
        //    /* TEST COVERAGE: PSP-4159, PSP-4339, PSP-4340, PSP-4341 PSP-4342, PSP-4343, PSP-4344, PSP-4345, PSP-4346, PSP-4347, PSP-4348, PSP-4349, PSP-4350, PSP-4351, PSP-4352, PSP-4353, PSP-4354, PSP-4355, PSP-4356, PSP-4357 */

        //    //Access the activity tab
        //    activities.AccessActivitiesTab();

        //    //Create new activity
        //    activities.CreateNewActivity("File Document");

        //    //Get total uploaded documents
        //    totalDigitalDocumentsUploaded = digitalDocuments.GetTotalUploadedDocuments();

        //    //Add a New Document
        //    digitalDocuments.AddNewDocument();

        //    //Verify different types of document types
        //    digitalDocuments.VerifyDocumentFields("BC assessment search");
        //    digitalDocuments.VerifyDocumentFields("Privy council");
        //    digitalDocuments.VerifyDocumentFields("Photos / Images/ Video");
        //    digitalDocuments.VerifyDocumentFields("PA plans / Design drawings");
        //    digitalDocuments.VerifyDocumentFields("Other");
        //    digitalDocuments.VerifyDocumentFields("OIC");
        //    digitalDocuments.VerifyDocumentFields("MoTI plan");
        //    digitalDocuments.VerifyDocumentFields("Miscellaneous notes (LTSA)");
        //    digitalDocuments.VerifyDocumentFields("Ministerial order");
        //    digitalDocuments.VerifyDocumentFields("Title search / Historical title");
        //    digitalDocuments.VerifyDocumentFields("Legal survey plan");
        //    digitalDocuments.VerifyDocumentFields("Historical file");
        //    digitalDocuments.VerifyDocumentFields("Gazette");
        //    digitalDocuments.VerifyDocumentFields("Field notes");
        //    digitalDocuments.VerifyDocumentFields("District road register");
        //    digitalDocuments.VerifyDocumentFields("Crown grant");
        //    digitalDocuments.VerifyDocumentFields("Correspondence");
        //    digitalDocuments.VerifyDocumentFields("Canada lands survey");
        //    digitalDocuments.VerifyDocumentFields("Transfer of administration");

        //    //Upload one digital document
        //    Random random = new Random();
        //    var index = random.Next(0, documentFiles.Count());
        //    var document = documentFiles.ElementAt(index);

        //    digitalDocuments.UploadDocument(document.Url);
        //    digitalDocuments.UploadTransferAdminFile(dateSigned, motiFile, pid, roadName, transferNbr);

        //    //Save digital document
        //    digitalDocuments.SaveDigitalDocument();
        //}

        //[StepDefinition(@"I create an Acquisition File with activity and edit attached document")]
        //public void ActivityWithDocumentEdited()
        //{
        //    /* TEST COVERAGE:  PSP-4030, PSP-4168, PSP-4335, PSP-4336, PSP-4338 */

        //    //Access the activity tab
        //    activities.AccessActivitiesTab();

        //    //Create new activity
        //    activities.CreateNewActivity("File Document");

        //    //Add a New Document
        //    digitalDocuments.AddNewDocument();

        //    //Upload one digital document
        //    Random random = new Random();
        //    var index = random.Next(0, documentFiles.Count());
        //    var document = documentFiles.ElementAt(index);

        //    digitalDocuments.UploadDocument(document.Url);
        //    digitalDocuments.UploadGazetteFile(dateSigned, gazettePage, publishedDate, gazetteType, legalPlan, LTSASchedule, motiPlan, roadName);

        //    //Save digital document
        //    digitalDocuments.SaveDigitalDocument();

        //    //Get total uploaded documents
        //    totalDigitalDocumentsUploaded = digitalDocuments.GetTotalUploadedDocuments();

        //    //Add new digital document
        //    digitalDocuments.AddNewDocument();
        //    var index2 = random.Next(0, documentFiles.Count());
        //    var document2 = documentFiles.ElementAt(index2);

        //    digitalDocuments.UploadDocument(document2.Url);
        //    digitalDocuments.UploadTransferAdminFile(dateSigned, motiFile, pid, roadName, transferNbr);

        //    //Cancel uploading a new document
        //    digitalDocuments.CancelDigitalDocument();

        //    //Edit digital document's details
        //    digitalDocuments.View1stDocument();
        //    digitalDocuments.EditDocument();

        //    //Verify Edit Form
        //    digitalDocuments.VerifyGazetteEditForm();

        //    //Make changes on the document details
        //    digitalDocuments.ChangeStatus("Approved");

        //    //Save document's changes
        //    digitalDocuments.SaveEditDigitalDocument();

        //    //Edit digital document's details
        //    digitalDocuments.View1stDocument();
        //    digitalDocuments.EditDocument();
        //    digitalDocuments.ChangeStatus("None");

        //    //Cancel digital document's details
        //    digitalDocuments.CancelEditDigitalDocument();

        //    //Close Digital Documents Details View
        //    digitalDocuments.CloseDigitalDocumentViewDetails();

        //    //Delete digital document
        //    digitalDocuments.Delete1stDocument();
        //}

        //[StepDefinition(@"An activity is created successfully")]
        //public void ActivityCreatedSuccess()
        //{
        //    Assert.True(activities.totalActivities() > 0);
        //}

        //[StepDefinition(@"An activity is deleted successfully")]
        //public void ActivityDeletedSuccess()
        //{
        //    Assert.True(activities.totalActivities() == 0);
        //}

        //[StepDefinition(@"A note has been deleted successfully")]
        //public void NoteDeletedSuccess()
        //{
        //    Assert.True(notes.NotesTotal() == 0);
        //}

        //[StepDefinition(@"A digital document has been uploaded successfully")]
        //public void DocumentUploadSuccess()
        //{
        //    Assert.True(digitalDocuments.GetTotalUploadedDocuments() == totalDigitalDocumentsUploaded + 1);
        //}

        //[StepDefinition(@"A digital document has been deleted successfully")]
        //public void DocumentDeleteSuccess()
        //{
        //    Assert.True(digitalDocuments.GetTotalUploadedDocuments() == 0);
        //}

    }
}

//public class DocumentFile
//{
//    public string Url { get; set; } = null!;
//}
