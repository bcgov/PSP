

using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class Notes : PageObjectBase
    {
        //Notes Tab Elements
        private readonly By notesTabLink = By.XPath("//nav[@role='tablist']/a[contains(text(),'Notes')]");
        private readonly By notesTabTitle = By.XPath("//h2/div/div/div/div[contains(text(),'Notes')]");
        private readonly By notesTabAddBttn = By.XPath("//div[@class='tab-content']/div[@role='tabpanel']/div/h2/div/div/div/div/button");

        //Notes Tab Table Header
        private readonly By notesTabTableHeaderNoteColumn = By.XPath("//div[@data-testid='main-notes-section']/div/div[@data-testid='notesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Note')]");
        private readonly By notesTabTableHeaderCreatedDateColumn = By.XPath("//div[@data-testid='main-notes-section']/div/div[@data-testid='notesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Created date')]");
        private readonly By notesTabTableHeaderUpdatedByColumn = By.XPath("//div[@data-testid='main-notes-section']/div/div[@data-testid='notesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Last updated by')]");
        private readonly By notesTabTableNoContent = By.XPath("//div[contains(text(),'No matching Notes found')]");
        private readonly By notesTabTableBody = By.XPath("//div[@data-testid='main-notes-section']/div/div[@data-testid='notesTable']/div[@class='tbody']");
        private readonly By notesTabTableContentTotal = By.XPath("//div[@data-testid='main-notes-section']/div/div[@data-testid='notesTable']/div[@class='tbody']/div[@class='tr-wrapper']");

        private readonly By notesTab2ndResultViewBttn = By.XPath("//div[@data-testid='notesTable']/div[@class='tbody']/div[@class='tr-wrapper'][2]/div/div[4]/div/button[@title='View Note']");

        //Notes 1st result Elements
        private readonly By note1stNoteContent = By.CssSelector("div[data-testid='notesTable'] div[class='tbody'] div[class='tr-wrapper'] div:nth-child(1) div:nth-child(1)");
        private readonly By note2ndDeleteNoteBttn = By.CssSelector("div[data-testid='notesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(2) div[role='cell']:nth-child(4) div button[title='Delete Note']");

        //Property Notes Table Elements
        private readonly By propNotesTableTitle = By.XPath("//div[@data-testid='property-notes-summary']/h2/div/div/div/div/div[text()='Property Notes']");
        private readonly By propNotesTooltip = By.CssSelector("span[data-testid='tooltip-icon-property-note-summary']");
        private readonly By propNotesPropNameColumn = By.CssSelector("div[data-testid='property-notes-summary'] div[data-testid='notesTable'] div[class='thead thead-light'] div[role='columnheader']:nth-child(1) div");
        private readonly By propNotesPropNoteColumn = By.CssSelector("div[data-testid='property-notes-summary'] div[data-testid='notesTable'] div[class='thead thead-light'] div[role='columnheader']:nth-child(2) div");
        private readonly By propNotesPropCreatedDateColumn = By.CssSelector("div[data-testid='property-notes-summary'] div[data-testid='notesTable'] div[class='thead thead-light'] div[role='columnheader']:nth-child(3) div");
        private readonly By propNotesPropCreatedSortBttn = By.CssSelector("div[data-testid='property-notes-summary'] div[data-testid='sort-column-appCreateTimestamp']");
        private readonly By propNotesPropUpdatedByColumn = By.CssSelector("div[data-testid='property-notes-summary'] div[data-testid='notesTable'] div[class='thead thead-light'] div[role='columnheader']:nth-child(4) div");
        private readonly By propNotesPropUpdatedBySortBttn = By.CssSelector("div[data-testid='sort-column-appLastUpdateUserid']");
        private readonly By propNotesPropActionsColumn = By.CssSelector("div[data-testid='property-notes-summary'] div[data-testid='notesTable'] div[class='thead thead-light'] div[role='columnheader']:nth-child(5) div");

        private readonly By propNotesTableName1stContent = By.CssSelector("div[data-testid='property-notes-summary'] div[data-testid='notesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div[role='cell']:nth-child(1) span");
        private readonly By propNotesTableNote1stContent = By.CssSelector("div[data-testid='property-notes-summary'] div[data-testid='notesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div[role='cell']:nth-child(2)");
        private readonly By propNotesTableCreatedDate1stContent = By.CssSelector("div[data-testid='property-notes-summary'] div[data-testid='notesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div[role='cell']:nth-child(3)");
        private readonly By propNotesTableLastUpdated1stContent = By.CssSelector("div[data-testid='property-notes-summary'] div[data-testid='notesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div[role='cell']:nth-child(4)");
        private readonly By propNotesTableViewBttn1stContent = By.CssSelector("div[data-testid='property-notes-summary'] div[data-testid='notesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div[role='cell']:nth-child(5) button[title='View Note']");

        //Management Notes Table Elements
        private readonly By mgmtNotesTableTitle = By.XPath("//div[@data-testid='management-notes-summary']/h2/div/div/div/div/div[text()='Management File Notes']");
        private readonly By mgmtNotesPropNameColumn = By.CssSelector("div[data-testid='management-notes-summary'] div[data-testid='notesTable'] div[class='thead thead-light'] div[role='columnheader']:nth-child(1) div");
        private readonly By mgmtNotesPropNoteColumn = By.CssSelector("div[data-testid='management-notes-summary'] div[data-testid='notesTable'] div[class='thead thead-light'] div[role='columnheader']:nth-child(2) div");
        private readonly By mgmtNotesPropCreatedDateColumn = By.CssSelector("div[data-testid='management-notes-summary'] div[data-testid='notesTable'] div[class='thead thead-light'] div[role='columnheader']:nth-child(3) div");
        private readonly By mgmtNotesPropCreatedSortBttn = By.CssSelector("div[data-testid='management-notes-summary'] div[data-testid='sort-column-appCreateTimestamp']");
        private readonly By mgmtNotesPropUpdatedByColumn = By.CssSelector("div[data-testid='management-notes-summary'] div[data-testid='notesTable'] div[class='thead thead-light'] div[role='columnheader']:nth-child(4) div");
        private readonly By mgmtNotesPropUpdatedBySortBttn = By.CssSelector("div[data-testid='management-notes-summary'] div[data-testid='sort-column-appLastUpdateUserid']");
        private readonly By mgmtNotesPropActionsColumn = By.CssSelector("div[data-testid='management-notes-summary'] div[data-testid='notesTable'] div[class='thead thead-light'] div[role='columnheader']:nth-child(5) div");

        private readonly By mgmtNotesTableName1stContent = By.CssSelector("div[data-testid='management-notes-summary'] div[data-testid='notesTable'] div[class='tbody'] div[role='cell']:first-child span");
        private readonly By mgmtNotesTableNote1stContent = By.CssSelector("div[data-testid='management-notes-summary'] div[data-testid='notesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div[role='cell']:nth-child(2)");
        private readonly By mgmtNotesTableCreatedDate1stContent = By.CssSelector("div[data-testid='management-notes-summary'] div[data-testid='notesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div[role='cell']:nth-child(3)");
        private readonly By mgmtNotesTableLastUpdated1stContent = By.CssSelector("div[data-testid='management-notes-summary'] div[data-testid='notesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div[role='cell']:nth-child(4)");
        private readonly By mgmtNotesTableViewBttn1stContent = By.CssSelector("div[data-testid='management-notes-summary'] div[data-testid='notesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div[role='cell']:nth-child(5) button[title='View Note']");

        //Notes Add new notes Details Elements
        private readonly By notesAddDetailsHeader = By.XPath("//div[@class='modal-title h4']");
        private readonly By notesAddDetailsLabel = By.XPath("//label[contains(text(),'Type a note')]");
        private readonly By notesAddDetailsTextarea = By.Id("input-note.note");
        private readonly By notesAddDetailsSaveBttn = By.CssSelector("button[title='ok-modal']");
        private readonly By notesAddDetailsCancelBttn = By.CssSelector("button[title='cancel-modal']");

        //Notes Edit Details Elements
        private readonly By notesEditCreatedLabel = By.XPath("//div[@class='modal-content']/div[@class='modal-body']/div/div/div[contains(text(),'Created')]");
        private readonly By notesEditCreatedDate = By.XPath("//div[@class='modal-content']/div[@class='modal-body']/div/div/div[contains(text(),'Created')]/following-sibling::div/span/strong");
        private readonly By notesEditCreatedBy = By.XPath("//div[@class='modal-content']/div[@class='modal-body']/div/div/div[contains(text(),'Created')]/following-sibling::div/span/span");
        private readonly By notesEditUpdatedLabel = By.XPath("//div[@class='modal-content']/div[@class='modal-body']/div/div/div[contains(text(),'Last updated')]");
        private readonly By notesEditUpdatedDate = By.XPath("//div[@class='modal-content']/div[@class='modal-body']/div/div/div[contains(text(),'Last updated')]/following-sibling::div/span/strong");
        private readonly By notesEditUpdatedBy = By.XPath("//div[@class='modal-content']/div[@class='modal-body']/div/div/div[contains(text(),'Last updated')]/following-sibling::div/span/span");
        private readonly By notedEditBttn = By.CssSelector("button[aria-label='edit']");
        private readonly By noteEditViewTextarea = By.CssSelector("textarea[title='Note']");
        private readonly By noteEditTextarea = By.Id("input-note");

        //Notes Cancel pop-up Elements
        private readonly By notesCancelPopupContent = By.XPath("//div[contains(text(),'Confirm Changes')]/parent::div/parent::div");
        private readonly By notesCancelPopupHeader = By.XPath("//div[contains(text(),'Confirm Changes')]");
        private readonly By notesCancelPopupBody = By.XPath("//div[contains(text(),'Confirm Changes')]/parent::div/following-sibling::div[@class='modal-body']");
        private readonly By notesCancelOkBttn = By.XPath("//div[contains(text(),'Confirm Changes')]/parent::div/parent::div/div/div[@class='button-wrap']/button[@title='ok-modal']");

        //Notes Delete pop-up Elements
        private readonly By notesDeletePopupHeader = By.CssSelector("div[class='modal-header'] div[class='modal-title h4']");
        private readonly By notesDeletePopupBody = By.CssSelector("div[class='modal-body']");
        private readonly By notesDeleteOkBttn = By.XPath("//div[contains(text(),'Delete Note')]/parent::div/parent::div/div/div[@class='button-wrap']/button[@title='ok-modal']");

        readonly SharedModals sharedModals;

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

        public void NavigateToFirstManagementNote()
        {
            var originalWindowHandle = webDriver.CurrentWindowHandle;

            Wait();
            webDriver.FindElement(mgmtNotesTableName1stContent).Click();

            Wait();
            var allWindowsHandle = webDriver.WindowHandles;
            var newWindowHandle = allWindowsHandle.Where(handle => handle != originalWindowHandle).First();
            webDriver.SwitchTo().Window(newWindowHandle);
        }

        public void NavigateToFirstPropertyNote()
        {
            var originalWindowHandle = webDriver.CurrentWindowHandle;

            Wait();
            webDriver.FindElement(propNotesTableName1stContent).Click();

            Wait();
            var allWindowsHandle = webDriver.WindowHandles;
            var newWindowHandle = allWindowsHandle.Where(handle => handle != originalWindowHandle).First();
            webDriver.SwitchTo().Window(newWindowHandle);
        }

        public void CreateNotesTabButton()
        {
            Wait();
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

        public void VerifyManagementNotesTab(string feature)
        {
            VerifyNotesTabListView();

            if (feature == "Property")
            {
                //Second Table from Management notes
                AssertTrueIsDisplayed(mgmtNotesTableTitle);
                AssertTrueIsDisplayed(propNotesTooltip);
                AssertTrueContentEquals(mgmtNotesPropNameColumn, "File Name");
                AssertTrueContentEquals(mgmtNotesPropNoteColumn, "Note");
                AssertTrueContentEquals(mgmtNotesPropCreatedDateColumn, "Created date");
                AssertTrueIsDisplayed(mgmtNotesPropCreatedSortBttn);
                AssertTrueContentEquals(mgmtNotesPropUpdatedByColumn, "Last updated by");
                AssertTrueIsDisplayed(mgmtNotesPropUpdatedBySortBttn);
                AssertTrueContentEquals(mgmtNotesPropActionsColumn, "Actions");
            }
            else
            {
                //Second Table from Properties notes
                AssertTrueIsDisplayed(propNotesTableTitle);
                AssertTrueIsDisplayed(propNotesTooltip);
                AssertTrueContentEquals(propNotesPropNameColumn, "Property Name");
                AssertTrueContentEquals(propNotesPropNoteColumn, "Note");
                AssertTrueContentEquals(propNotesPropCreatedDateColumn, "Created date");
                AssertTrueIsDisplayed(propNotesPropCreatedSortBttn);
                AssertTrueContentEquals(propNotesPropUpdatedByColumn, "Last updated by");
                AssertTrueIsDisplayed(propNotesPropUpdatedBySortBttn);
                AssertTrueContentEquals(propNotesPropActionsColumn, "Actions");
            }
        }

        public void VerifySecondaryNotesListContent(string feature, string note)
        {
            Wait();

            if (feature == "Property")
            {
                AssertTrueContentNotEquals(propNotesTableName1stContent, "");
                AssertTrueContentNotEquals(propNotesTableNote1stContent, "");
                AssertTrueContentNotEquals(propNotesTableCreatedDate1stContent, "");
                AssertTrueContentNotEquals(propNotesTableLastUpdated1stContent, "");
                AssertTrueIsDisplayed(propNotesTableViewBttn1stContent);
            }
            else
            {
                AssertTrueContentNotEquals(mgmtNotesTableName1stContent, "");
                AssertTrueContentNotEquals(mgmtNotesTableNote1stContent, note);
                AssertTrueContentNotEquals(mgmtNotesTableCreatedDate1stContent, "");
                AssertTrueContentNotEquals(mgmtNotesTableLastUpdated1stContent, "");
                AssertTrueIsDisplayed(mgmtNotesTableViewBttn1stContent);
            }
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
            Wait();
            AssertTrueContentEquals(note1stNoteContent, "Compensation Requisition with # " + CompensationNbr + ", changed status from '"+ fromStatus +"' to '" + toStatus + "'");
        }

        public void VerifyAutomaticNotesPropertyStatus(string status)
        {
            Wait();
            AssertTrueElementContains(note1stNoteContent, "Management File property");
            AssertTrueElementContains(note1stNoteContent, status);
        }
    }
}
