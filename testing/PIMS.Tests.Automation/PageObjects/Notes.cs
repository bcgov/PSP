

using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class Notes : PageObjectBase
    {
        //Notes Add New button Element
        private By notesAddNoteBttn = By.XPath("//div[@data-testid='activity-tray']/div[2]/div/div[2]/div[7]/div/div/h2/div/div/div/div[contains(text(),'Notes')]/following-sibling::div/button");

        //Notes List View Elements
        private By notesTitle = By.XPath("//div[@data-testid='activity-tray']/div[2]/div/div[2]/div[7]/div/div/h2/div/div/div/div[contains(text(),'Notes')]");
        private By notesTable = By.XPath("//div[@data-testid='activity-tray']/div[2]/div/div[2]/div[7]/div/div/div/div[@data-testid='notesTable']");
        private By notesNoteColumn = By.XPath("//div[@data-testid='activity-tray']/div[2]/div/div[2]/div[7]/div/div/div/div[@data-testid='notesTable']/div/div/div/div[contains(text(),'Note')]");
        private By notesCreatedDateColumn = By.XPath("//div[@data-testid='activity-tray']/div[2]/div/div[2]/div[7]/div/div/div/div[@data-testid='notesTable']/div/div/div/div[contains(text(),'Created date')]");
        private By notesCreatedByColumn = By.XPath("//div[@data-testid='activity-tray']/div[2]/div/div[2]/div[7]/div/div/div/div[@data-testid='notesTable']/div/div/div/div[contains(text(),'Last updated by')]");
        private By notesBodyRows = By.XPath("//div[@data-testid='activity-tray']/div[2]/div/div[2]/div[7]/div/div/div/div[@data-testid='notesTable']/div[@class='tbody']/div[@class='tr-wrapper']");

        //Notes 1st result Elements
        private By note1stViewNoteBttn = By.CssSelector("div[data-testid='notesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[role='cell']:nth-child(4) div button[title='View Note']");
        private By note1stDeleteNoteBttn = By.CssSelector("div[data-testid='notesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[role='cell']:nth-child(4) div button[title='Delete Note']");

        //Notes Add new notes Details Elements
        private By notesAddDetailsHeader = By.XPath("//div[@class='modal-title h4']");
        private By notesAddDetailsLabel = By.XPath("//label[contains(text(),'Type a note')]");
        private By notesAddDetailsTextarea = By.Id("input-note.note");
        private By notesAddDetailsSaveBttn = By.CssSelector("button[title='ok-modal']");
        private By notesAddDetailsCancelBttn = By.CssSelector("button[title='cancel-modal']");

        //Notes Edit Details Elements
        private By notesEditCreatedLabel = By.XPath("//div[@class='modal-content']/div[@class='modal-body']/div/div/div[contains(text(),'Created')]");
        private By notesEditCreatedDate = By.XPath("//div[@class='modal-content']/div[@class='modal-body']/div/div/div[contains(text(),'Created')]/following-sibling::div/span/strong");
        private By notesEditCreatedBy = By.XPath("//div[@class='modal-content']/div[@class='modal-body']/div/div/div[contains(text(),'Created')]/following-sibling::div/span/span");
        private By notesEditUpdatedLabel = By.XPath("//div[@class='modal-content']/div[@class='modal-body']/div/div/div[contains(text(),'Last updated')]");
        private By notesEditUpdatedDate = By.XPath("//div[@class='modal-content']/div[@class='modal-body']/div/div/div[contains(text(),'Last updated')]/following-sibling::div/span/strong");
        private By notesEditUpdatedBy = By.XPath("//div[@class='modal-content']/div[@class='modal-body']/div/div/div[contains(text(),'Last updated')]/following-sibling::div/span/span");
        private By notedEditBttn = By.CssSelector("button[aria-label='edit']");
        private By noteEditViewTextarea = By.CssSelector("textarea[title='Note']");
        private By noteEditTextarea = By.Id("input-note");

        //Notes Cancel pop-up Elements
        private By notesCancelPopupContent = By.XPath("//div[contains(text(),'Unsaved Changes')]/parent::div/parent::div");
        private By notesCancelPopupHeader = By.XPath("//div[contains(text(),'Unsaved Changes')]");
        private By notesCancelPopupBody = By.XPath("//div[contains(text(),'Unsaved Changes')]/parent::div/following-sibling::div[@class='modal-body']");
        private By notesCancelOkBttn = By.XPath("//div[contains(text(),'Unsaved Changes')]/parent::div/following-sibling::div[@class='modal-footer']/button[@title='ok-modal']");

        //Notes Delete pop-up Elements
        private By notesDeletePopupHeader = By.CssSelector("div[class='modal-header'] div");
        private By notesDeletePopupBody = By.CssSelector("div[class='modal-body']");
        private By notesDeleteOkBttn = By.XPath("//div[contains(text(),'Delete Note')]/parent::div/following-sibling::div[@class='modal-footer']/button[@title='ok-modal']");


        public Notes(IWebDriver webDriver) : base(webDriver)
        {}

        public void AddNewNote()
        {
            Wait();
            FocusAndClick(notesAddNoteBttn);
        }

        public void AddNewNoteDetails(string note)
        {
            Wait();
            webDriver.FindElement(notesAddDetailsTextarea).SendKeys(note);
        }

        public void ViewFirstNoteButton()
        {
            Wait();
            webDriver.FindElement(note1stViewNoteBttn).Click();
        }

        public void EditNote(string note)
        {
            Wait();
            webDriver.FindElement(notedEditBttn).Click();

            Wait();
            ClearInput(noteEditTextarea);
            webDriver.FindElement(noteEditTextarea).SendKeys(note);
        }

        public void SaveNote()
        {
            Wait();
            webDriver.FindElement(notesAddDetailsSaveBttn).Click();
        }

        public void CancelNote()
        {
            Wait();
            webDriver.FindElement(notesAddDetailsCancelBttn).Click();
           
            if (webDriver.FindElements(notesCancelPopupContent).Count() > 0)
            {
                Assert.True(webDriver.FindElement(notesCancelPopupHeader).Displayed);
                Assert.True(webDriver.FindElement(notesCancelPopupBody).Text.Equals("You have made changes on this form. Do you wish to leave without saving?"));
                webDriver.FindElement(notesCancelOkBttn).Click();
            }
        }

        public void DeleteFirstNote()
        {
            Wait();
            webDriver.FindElement(note1stDeleteNoteBttn).Click();

            Wait();
            Assert.True(webDriver.FindElement(notesDeletePopupHeader).Text.Equals("Delete Note"));
            Assert.True(webDriver.FindElement(notesDeletePopupBody).Text.Equals("Are you sure you want to delete this note?"));
            webDriver.FindElement(notesDeleteOkBttn).Click();
        }

        public void VerifyNotesListView()
        {
            Wait();
            Assert.True(webDriver.FindElement(notesTitle).Displayed);
            Assert.True(webDriver.FindElement(notesAddNoteBttn).Displayed);
            Assert.True(webDriver.FindElement(notesTable).Displayed);
            Assert.True(webDriver.FindElement(notesNoteColumn).Displayed);
            Assert.True(webDriver.FindElement(notesCreatedDateColumn).Displayed);
            Assert.True(webDriver.FindElement(notesCreatedByColumn).Displayed);
        }

        public void VerifyNotesAddNew()
        {
            Wait();
            Assert.True(webDriver.FindElement(notesAddDetailsHeader).Displayed);
            Assert.True(webDriver.FindElement(notesAddDetailsHeader).Text == "Notes");
            Assert.True(webDriver.FindElement(notesAddDetailsLabel).Displayed);
            Assert.True(webDriver.FindElement(notesAddDetailsTextarea).Displayed);
            Assert.True(webDriver.FindElement(notesAddDetailsSaveBttn).Displayed);
            Assert.True(webDriver.FindElement(notesAddDetailsCancelBttn).Displayed);
        }

        public void VerifyNotesEditForm()
        {
            Wait();
            Assert.True(webDriver.FindElement(notesEditCreatedLabel).Displayed);
            Assert.True(webDriver.FindElement(notesEditCreatedDate).Displayed);
            Assert.True(webDriver.FindElement(notesEditCreatedBy).Displayed);
            Assert.True(webDriver.FindElement(notesEditUpdatedLabel).Displayed);
            Assert.True(webDriver.FindElement(notesEditUpdatedDate).Displayed);
            Assert.True(webDriver.FindElement(notesEditUpdatedBy).Displayed);
            Assert.True(webDriver.FindElement(notedEditBttn).Displayed);
            Assert.True(webDriver.FindElement(noteEditViewTextarea).Displayed);
            Assert.True(webDriver.FindElement(notesAddDetailsSaveBttn).Displayed);
        }

        public void VerifyAutomaticNotes(string fromStatus, string toStatus)
        {
            Wait();
            var lastNoteIndex = webDriver.FindElements(notesBodyRows).Count();
            var lastNote = "//div[@data-testid='activity-tray']/div[2]/div/div[2]/div[7]/div/div/div/div[@data-testid='notesTable']/div[@class='tbody']/div[@class='tr-wrapper']["+lastNoteIndex+"]/div[@class='tr']/div[@class='td'][1]";
            //var lastNote = "div[data-testid='notesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ lastNoteIndex +") div[class='tr'] div[class='td']:nth-child(1)";
            Assert.True(webDriver.FindElement(By.XPath(lastNote)).Text == "Activity status changed from "+ fromStatus +" to " + toStatus);
        }

        public int NotesTotal()
        {
            Wait();
            return webDriver.FindElements(notesBodyRows).Count();
        }
    }
}
