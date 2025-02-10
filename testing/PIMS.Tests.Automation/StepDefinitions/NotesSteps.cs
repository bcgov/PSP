
using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;
using PIMS.Tests.Automation.Data;
using PIMS.Tests.Automation.PageObjects;
using System.Data;

namespace PIMS.Tests.Automation.StepDefinitions
{
    [Binding]
    public class NotesSteps
    {
        private readonly Notes notes;
        private readonly SharedPagination sharedPagination;
        private readonly GenericSteps genericSteps;

        private List<string> notesData;
        private int notesCount;

        public NotesSteps(IWebDriver driver)
        {
            notes = new Notes(driver);
            sharedPagination = new SharedPagination(driver);
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
            /* TEST COVERAGE: PSP-4020, PSP-5506, PSP-5507 */

            //Navigate to the Notes Tab
            notes.NavigateNotesTab();

            //Edit note
            PopulateNotes(rowNumber);
            notes.ViewSecondNoteDetails();
            notes.VerifyNotesEditForm();
            notes.EditNote(notesData[0]);

            //Cancel note's update
            notes.CancelNote();

            //Edit note
            notes.ViewSecondNoteDetails();
            notes.EditNote(notesData[0]);

            //Save changes
            notes.SaveNote();

            //Verify Pagination on the list view
            sharedPagination.ChoosePaginationOption(5);
            Assert.Equal(5, notes.NotesTabCount());

            sharedPagination.ChoosePaginationOption(10);
            Assert.Equal(10, notes.NotesTabCount());

            //Delete Note
            notesCount = notes.NotesTabCount();
            notes.DeleteLastSecondNote();
        }

        private void PopulateNotes(int rowNumber)
        {
            DataTable notesSheet = ExcelDataContext.GetInstance().Sheets["Notes"]!;
            ExcelDataContext.PopulateInCollection(notesSheet);

            notesData = genericSteps.PopulateLists(ExcelDataContext.ReadData(rowNumber, "Notes"));
        }
    }
}
