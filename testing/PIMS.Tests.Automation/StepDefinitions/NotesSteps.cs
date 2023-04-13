
namespace PIMS.Tests.Automation.StepDefinitions
{
    public class NotesSteps
    {
        private readonly SharedNotesTab sharedNotesTab;

        private readonly string notesTabNote1 = "Testing notes tab from Acquisition File";
        private readonly string acquisitionFileNameNotes = "Automated Acquisition File - Testing Notes Tab";
        private readonly string acquisitionFileNameNotes2 = "Automated Acquisition File - Testing Notes Tab Update";

        public NotesSteps(BrowserDriver driver)
        {
            sharedNotesTab = new SharedNotesTab(driver.Current);
        }

        [StepDefinition(@"I create a new Note on the Notes Tab")]
        public void CreateNotesTab()
        {
            /* TEST COVERAGE: PSP-5332, PSP-5505, PSP-5506, PSP-5507  */

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

        [StepDefinition(@"The Notes Tab rendered successfully")]
        public void NotesTanSuccessful()
        {
            sharedNotesTab.VerifyNotesTabListView();
        }
    }
}
