

using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class Notes : PageObjectBase
    {
        //Notes Tab Elements
        private By notesTabLink = By.XPath("//nav[@role='tablist']/a[contains(text(),'Notes')]");
        private By notesTabTitle = By.XPath("//div[@class='tab-content']/div[@role='tabpanel']/div/div/div/h2/div/div/div/div[contains(text(),'Notes')]");
        private By notesTabAddBttn = By.XPath("//div[@class='tab-content']/div[@role='tabpanel']/div/div/div/h2/div/div/div/div/button");

        //Notes Tab Table Header
        private By notesTabTableHeaderNoteColumn = By.XPath("//div[@data-testid='notesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Note')]");
        private By notesTabTableHeaderCreatedDateColumn = By.XPath("//div[@data-testid='notesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Created date')]");
        private By notesTabTableHeaderUpdatedByColumn = By.XPath("//div[@data-testid='notesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Last updated by')]");
        private By notesTabTableNoContent = By.XPath("//div[contains(text(),'No matching Notes found')]");
        private By notesTabTableBody = By.XPath("//div[@data-testid='notesTable']/div[@class='tbody']");
        private By notesTabTableContentTotal = By.XPath("//div[@data-testid='notesTable']/div[@class='tbody']/div[@class='tr-wrapper']");

        private By notesTab2ndResultViewBttn = By.XPath("//div[@data-testid='notesTable']/div[@class='tbody']/div[@class='tr-wrapper'][2]/div/div[4]/div/div/button[@title='View Note']");
        private By notesTab1stResultDeleteBttn = By.XPath("//div[@data-testid='notesTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[4]/div/div/button[@title='Delete Note']");

        //Notes 1st result Elements
        private By note1stViewNoteBttn = By.CssSelector("div[data-testid='notesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[role='cell']:nth-child(4) div button[title='View Note']");
        private By note1stNoteContent = By.CssSelector("div[data-testid='notesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='tr'] div[class='td']:nth-child(1)");
        private By note2ndDeleteNoteBttn = By.CssSelector("div[data-testid='notesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(2) div[role='cell']:nth-child(4) div button[title='Delete Note']");
        private By note2ndNoteContent = By.CssSelector("div[data-testid='notesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='tr'] div[class='td']:nth-child(2)");

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
        private By notesCancelPopupContent = By.XPath("//div[contains(text(),'Confirm Changes')]/parent::div/parent::div");
        private By notesCancelPopupHeader = By.XPath("//div[contains(text(),'Confirm Changes')]");
        private By notesCancelPopupBody = By.XPath("//div[contains(text(),'Confirm Changes')]/parent::div/following-sibling::div[@class='modal-body']");
        private By notesCancelOkBttn = By.XPath("//div[contains(text(),'Confirm Changes')]/parent::div/parent::div/div/div[@class='button-wrap']/button[@title='ok-modal']");

        //Notes Delete pop-up Elements
        private By notesDeletePopupHeader = By.CssSelector("div[class='modal-header'] div[class='modal-title h4']");
        private By notesDeletePopupBody = By.CssSelector("div[class='modal-body']");
        private By notesDeleteOkBttn = By.XPath("//div[contains(text(),'Delete Note')]/parent::div/parent::div/div/div[@class='button-wrap']/button[@title='ok-modal']");

        SharedModals sharedModals;

        public Notes(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
        }

        public void NavigateNotesTab()
        {
            Wait(2000);

            WaitUntilClickable(notesTabLink);
            webDriver.FindElement(notesTabLink).Click();
        }

        public void CreateNotesTabButton()
        {
            Wait(3000);
            FocusAndClick(notesTabAddBttn);
        }

        public void AddNewNoteDetails(string note)
        {
            WaitUntilVisible(notesAddDetailsTextarea);
            webDriver.FindElement(notesAddDetailsTextarea).SendKeys(note);
        }

        public void ViewSecondNoteDetails()
        {
            Wait();
            webDriver.FindElement(notesTab2ndResultViewBttn).Click();
        }

        public void EditNote(string note)
        {
            WaitUntilClickable(notedEditBttn);
            webDriver.FindElement(notedEditBttn).Click();

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
            WaitUntilClickable(notesAddDetailsCancelBttn);
            webDriver.FindElement(notesAddDetailsCancelBttn).Click();

            Wait();
            if (webDriver.FindElements(notesCancelPopupContent).Count() > 0)
            {
                AssertTrueIsDisplayed(notesCancelPopupHeader);
                Assert.Contains("If you choose to cancel now, your changes will not be saved.", webDriver.FindElement(notesCancelPopupBody).Text);
                Assert.Contains("Do you want to proceed?", webDriver.FindElement(notesCancelPopupBody).Text);

                Wait(2000);
                webDriver.FindElement(notesCancelOkBttn).Click();
            }
        }

        public void DeleteLastSecondNote()
        {
            Wait(2000);
            webDriver.FindElement(note2ndDeleteNoteBttn).Click();

            WaitUntilVisible(notesDeletePopupHeader);
            AssertTrueContentEquals(notesDeletePopupHeader, "Delete Note");
            AssertTrueContentEquals(notesDeletePopupBody, "Are you sure you want to delete this note?");

            webDriver.FindElement(notesDeleteOkBttn).Click();
        }

        public void VerifyNotesAddNew()
        {
            AssertTrueIsDisplayed(notesAddDetailsHeader);
            AssertTrueContentEquals(notesAddDetailsHeader, "Notes");
            AssertTrueIsDisplayed(notesAddDetailsLabel);
            AssertTrueIsDisplayed(notesAddDetailsTextarea);
            AssertTrueIsDisplayed(notesAddDetailsSaveBttn);
            AssertTrueIsDisplayed(notesAddDetailsCancelBttn);
        }

        public void VerifyNotesEditForm()
        {
            AssertTrueIsDisplayed(notesEditCreatedLabel);
            AssertTrueIsDisplayed(notesEditCreatedDate);
            AssertTrueIsDisplayed(notesEditCreatedBy);

            if(webDriver.FindElements(notesEditUpdatedLabel).Count > 0)
                AssertTrueIsDisplayed(notesEditUpdatedDate);

            AssertTrueIsDisplayed(notesEditUpdatedBy);
            AssertTrueIsDisplayed(notedEditBttn);
            AssertTrueIsDisplayed(noteEditViewTextarea);
            AssertTrueIsDisplayed(notesAddDetailsSaveBttn);
        }

        public void VerifyNotesTabListView()
        {
            Wait(3000);

            AssertTrueIsDisplayed(notesTabTitle);
            AssertTrueIsDisplayed(notesTabAddBttn);
            AssertTrueIsDisplayed(notesTabTableHeaderNoteColumn);
            AssertTrueIsDisplayed(notesTabTableHeaderCreatedDateColumn);
            AssertTrueIsDisplayed(notesTabTableHeaderUpdatedByColumn);

            if (webDriver.FindElements(notesTabTableContentTotal).Count > 0)
                AssertTrueIsDisplayed(notesTabTableBody);
            else
                AssertTrueIsDisplayed(notesTabTableNoContent);
        }

        public int NotesTabCount()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElements(notesTabTableContentTotal).Count();
        }

        public void VerifyAutomaticNotes(string fileType, string fromStatus, string toStatus)
        {
            WaitUntilTableSpinnerDisappear();

            WaitUntilVisibleText(note1stNoteContent, webDriver.FindElement(note1stNoteContent).Text);
            AssertTrueContentEquals(note1stNoteContent, fileType + " status changed from "+ fromStatus +" to " + toStatus);
        }

        public void VerifyAutomaticNotesCompensation(string CompensationNbr, string fromStatus, string toStatus)
        {
            WaitUntilVisibleText(note1stNoteContent, webDriver.FindElement(note1stNoteContent).Text);
            AssertTrueContentEquals(note1stNoteContent, "Compensation Requisition with # " + CompensationNbr + ", changed status from '"+ fromStatus +"' to '" + toStatus + "'");
        }
    }
}
