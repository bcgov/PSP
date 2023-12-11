
using PIMS.Tests.Automation.Classes;
using PIMS.Tests.Automation.Data;
using System.Data;

namespace PIMS.Tests.Automation.StepDefinitions
{
    [Binding]
    public class NotesSteps
    {
        private readonly Notes notes;
        private readonly GenericSteps genericSteps;

        private List<string> notesData;
        private int notesCount;

        public NotesSteps(BrowserDriver driver)
        {
            notes = new Notes(driver.Current);
            genericSteps = new GenericSteps(driver);
            notesData = new List<string>();
            notesCount = 0;
        }

        [StepDefinition(@"I create a new Note on the Notes Tab from row number (.*)")]
        public void CreateNotesTab(int rowNumber)
        {
            /* TEST COVERAGE: PSP-5332, PSP-5505 */

            //Navigate to the Notes Tab
            notes.NavigateNotesTab();
            notes.VerifyNotesTabListView();

            //Create a new note
            PopulateNotes(rowNumber);
            notes.CreateNotesTabButton();
            notes.VerifyNotesAddNew();
            notes.AddNewNoteDetails(notesData[0]);

            //Cancel new note
            notes.CancelNote();

            //Create a new note
            for (var i = 0; i < notesData.Count; i++)
            {
                notes.CreateNotesTabButton();
                notes.AddNewNoteDetails(notesData[i]);
                notes.SaveNote();
            }
        }

        [StepDefinition(@"I edit a Note on the Notes Tab from row number (.*)")]
        public void EditNotesTab(int rowNumber)
        {
            /* TEST COVERAGE: PSP-5506, PSP-5507 */

            //Navigate to the Notes Tab
            notes.NavigateNotesTab();

            //Edit note
            PopulateNotes(rowNumber);
            notes.ViewSecondLastNoteDetails();
            notes.VerifyNotesEditForm();
            notes.EditNote(notesData[0]);

            //Cancel note's update
            notes.CancelNote();

            //Edit note
            notes.ViewSecondLastNoteDetails();
            notes.EditNote(notesData[0]);

            //Save changes
            notes.SaveNote();

            //Delete Note
            notesCount = notes.NotesTabCount();
            notes.DeleteLastSecondNote();
        }

        [StepDefinition(@"Notes update have been done successfully")]
        public void NoteUpdateSuccess()
        {
            
            Assert.True(notes.NotesTabCount() == notesCount -1);
        }

        private void PopulateNotes(int rowNumber)
        {
            DataTable notesSheet = ExcelDataContext.GetInstance().Sheets["Notes"];
            ExcelDataContext.PopulateInCollection(notesSheet);

            notesData = genericSteps.PopulateLists(ExcelDataContext.ReadData(rowNumber, "Notes"));
        }
    }
}
