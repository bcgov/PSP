using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SharedNotesTab : PageObjectBase
    {
        private By notesTabLink = By.XPath("//nav[@role='tablist']/a[contains(text(),'Notes')]");
        private By notesTabTitle = By.XPath("//div[@class='tab-content']/div[@role='tabpanel']/div/div/div/h2/div/div/div/div[contains(text(),'Notes')]");
        private By notesTabAddBttn = By.XPath("//div[@class='tab-content']/div[@role='tabpanel']/div/div/div/h2/div/div/div/div/button");

        private By notesTabTableHeaderNoteColumn = By.XPath("//div[@data-testid='notesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Note')]");
        private By notesTabTableHeaderCreatedDateColumn = By.XPath("//div[@data-testid='notesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Created date')]");
        private By notesTabTableHeaderUpdatedByColumn = By.XPath("//div[@data-testid='notesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Last updated by')]");
        private By notesTabTableNoContent = By.XPath("//div[contains(text(),'No matching Notes found')]");
        private By notesTabTableContentTotal = By.XPath("//div[@data-testid='notesTable']/div[@class='tbody']/div[@class='tr-wrapper']");

        private By notesTab1stResultViewBttn = By.XPath("//div[@data-testid='notesTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[4]/div/button[@title='View Note']");
        private By notesTab1stResultDeleteBttn = By.XPath("//div[@data-testid='notesTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[4]/div/button[@title='Delete Note']");

        //Notes Delete pop-up Elements
        private By notesDeletePopupHeader = By.CssSelector("div[class='modal-header'] div");
        private By notesDeletePopupBody = By.CssSelector("div[class='modal-body']");
        private By notesDeleteOkBttn = By.XPath("//div[contains(text(),'Delete Note')]/parent::div/following-sibling::div[@class='modal-footer']/button[@title='ok-modal']");

        Notes notes;


        public SharedNotesTab(IWebDriver webDriver) : base(webDriver)
        {
            notes = new Notes(webDriver);
        }

        public void NavigateNotesTab()
        {
            Wait();
            webDriver.FindElement(notesTabLink).Click();
        }

        public void CreateNotesTabButton()
        {
            Wait();
            webDriver.FindElement(notesTabAddBttn).Click();
        }

        public void AddNewNoteDetails(string note)
        {
            notes.AddNewNoteDetails(note);
        }

        public void ViewFirstNoteDetails()
        {
            Wait();
            webDriver.FindElement(notesTab1stResultViewBttn).Click();
        }

        public void EditNote(string note)
        {
            notes.EditNote(note);
        }

        public void SaveNote()
        {
            notes.SaveNote();
        }

        public void CancelNote()
        {
            notes.CancelNote();
        }

        public void DeleteFirstNote()
        {
            Wait();
            webDriver.FindElement(notesTab1stResultDeleteBttn).Click();

            Wait();
            Assert.True(webDriver.FindElement(notesDeletePopupHeader).Text.Equals("Delete Note"));
            Assert.True(webDriver.FindElement(notesDeletePopupBody).Text.Equals("Are you sure you want to delete this note?"));
            webDriver.FindElement(notesDeleteOkBttn).Click();
        }

        public void VerifyNotesTabListView()
        {
            Wait();
            Assert.True(webDriver.FindElement(notesTabTitle).Displayed);
            Assert.True(webDriver.FindElement(notesTabAddBttn).Displayed);
            Assert.True(webDriver.FindElement(notesTabTableHeaderNoteColumn).Displayed);
            Assert.True(webDriver.FindElement(notesTabTableHeaderCreatedDateColumn).Displayed);
            Assert.True(webDriver.FindElement(notesTabTableHeaderUpdatedByColumn).Displayed);
            Assert.True(webDriver.FindElement(notesTabTableNoContent).Displayed);
        }

        public int NotesTabCount()
        {
            Wait();
            return webDriver.FindElements(notesTabTableContentTotal).Count();
        }
    }
}
