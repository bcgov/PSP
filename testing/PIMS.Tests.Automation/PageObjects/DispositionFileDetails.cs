

using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class DispositionFileDetails : PageObjectBase
    {
        //Disposition Files Menu Elements
        private By menuDispositionButton = By.CssSelector("div[data-testid='nav-tooltip-disposition'] a");
        private By createDispositionFileButton = By.XPath("//a[contains(text(),'Create a Disposition File')]");

        private By dispositionFileSummaryBttn = By.XPath("//div[contains(text(),'File Summary')]");
        private By dispositionFileDetailsTab = By.XPath("//a[contains(text(),'File details')]");

        //disposition File Details View Form Elements
        private By dispositionFileViewTitle = By.XPath("//h1[contains(text(),'Disposition File')]");

        private By dispositionFileCreateTitle = By.XPath("//h1[contains(text(),'Create Disposition File')]");
        private By dispositionFileHeaderCodeLabel = By.XPath("//label[contains(text(), 'File:')]");
        private By dispositionFileHeaderCodeContent = By.XPath("//label[contains(text(), 'File:')]/parent::div/following-sibling::div[1]/strong");

        private By dispositionFileHeaderProjectLabel = By.XPath("//label[contains(text(), 'Ministry project')]");
        private By dispositionFileHeaderProjectContent = By.XPath("//label[contains(text(), 'Ministry project')]/parent::div/following-sibling::div[1]/strong");
        private By dispositionFileHeaderCreatedDateLabel = By.XPath("//span[contains(text(), 'Created')]");
        private By dispositionFileHeaderCreatedDateContent = By.XPath("//span[contains(text(), 'Created')]/strong");
        private By dispositionFileHeaderCreatedByContent = By.XPath("//span[contains(text(),'Created')]/span[@id='userNameTooltip']/strong");
        private By dispositionFileHeaderLastUpdateLabel = By.XPath("//span[contains(text(), 'Last updated')]");
        private By dispositionFileHeaderLastUpdateContent = By.XPath("//span[contains(text(), 'Last updated')]/strong");
        private By dispositionFileHeaderLastUpdateByContent = By.XPath("//span[contains(text(), 'Last updated')]//span[@id='userNameTooltip']/strong");
        private By dispositionFileHeaderStatusLabel = By.XPath("//label[contains(text(),'Status')]");
        private By dispositionFileStatusSelect = By.Id("input-fileStatusTypeCode");
        private By dispositionFileHeaderStatusContent = By.XPath("//label[contains(text(),'Status')]/parent::div/following-sibling::div[1]/strong");

        private By dispositionFileProjectSubtitle = By.XPath("//h2/div/div[contains(text(), 'Project')]");
        private By dispositionFileProjectLabel = By.XPath("//div[@class='collapse show']/div/div/label[contains(text(),'Ministry project')]");
        private By dispositionFileProjectInput = By.CssSelector("input[id='typeahead-project']");
        private By dispositionFileProject1stOption = By.CssSelector("div[id='typeahead-project'] a");
        private By dispositionFileProjectContent = By.XPath("//div[@class='collapse show']/div/div/label[contains(text(),'Ministry project')]/parent::div/following-sibling::div");
        private By dispositionFileProjectProductLabel = By.XPath("//label[contains(text(),'Product')]");
        private By dispositionFileProjectProductSelect = By.Id("input-product");
        private By dispositionFileProjectProductContent = By.XPath("//label[contains(text(),'Product')]/parent::div/following-sibling::div");
        private By dispositionFileProjectFundingLabel = By.XPath("//label[contains(text(),'Funding')]");
        private By dispositionFileProjectFundingInput = By.Id("input-fundingTypeCode");
        private By dispositionFileProjectFundingContent = By.XPath("//label[contains(text(),'Funding')]/parent::div/following-sibling::div");
        private By dispositionFileProjectOtherFundingLabel = By.XPath("//label[contains(text(),'Other funding')]");
        private By dispositionFileProjectOtherFundingInput = By.Id("input-fundingTypeOtherDescription");
        private By dispositionFileProjectOtherFundingContent = By.XPath("//label[contains(text(),'Other funding')]/parent::div/following-sibling::div");

        private By dispositionFileScheduleSubtitle = By.XPath("//div[contains(text(),'Schedule')]");
        private By dispositionFileScheduleAssignedDateLabel = By.XPath("//label[contains(text(),'Assigned date')]");
        private By dispositionFileAssignedDateInput = By.Id("datepicker-assignedDate");
        private By dispositionFileScheduleAssignedDateContent = By.XPath("//label[contains(text(),'Assigned date')]/parent::div/following-sibling::div");
        private By dispositionFileScheduleCompletedDateLabel = By.XPath("//label[contains(text(),'Disposition completed date')]");
        private By dispositionFileCompletedDateLabelInput = By.Id("datepicker-completionDate");
        private By dispositionFileScheduleCompletedDateContent = By.XPath("//label[contains(text(),'sition completed date')]/parent::div/following-sibling::div");

        private By dispositionFileDetailsSubtitle = By.XPath("//div[contains(text(),'Disposition Details')]");
        private By dispositionFileDetailsNameLabel = By.XPath("//label[contains(text(),'Disposition file name')]");
        private By dispositionFileDetailsNameContent = By.XPath("//label[contains(text(),'Disposition file name')]/parent::div/following-sibling::div");
        private By dispositionFileDetailsReferenceNumberLabel = By.XPath("//label[contains(text(),'Reference number')]");
        private By dispositionFileDetailsReferenceNumberContent = By.XPath("//label[contains(text(),'Reference number')]/parent::div/following-sibling::div");
        private By dispositionFileDetailsDispositionStatusLabel = By.XPath("//label[contains(text(),'Disposition status')]");
        private By dispositionFileDetailsDispositionStatusContent = By.XPath("//label[contains(text(),'Disposition status')]/parent::div/following-sibling::div");
        private By dispositionFileDetailsDispositionTypeLabel = By.XPath("//label[contains(text(),'Disposition type')]");
        private By dispositionFileDetailsDispositionTypeContent = By.XPath("//label[contains(text(),'Disposition type')]/parent::div/following-sibling::div");
        private By dispositionFileDetailsInitiatingDocumentLabel = By.XPath("//label[contains(text(),'Initiating document')]");
        private By dispositionFileDetailsInitiatingDocumentContent = By.XPath("(//label[contains(text(),'Initiating document')]/parent::div/following-sibling::div)[1]");
        private By dispositionFileDetailsOtherInitiatingDocumentLabel = By.XPath("//label[contains(text(),'Other (Initiating Document)')]");
        private By dispositionFileDetailsOtherInitiatingDocumentLabelContent = By.XPath("//label[contains(text(),'Other (Initiating Document)')]");
        private By dispositionFileDetailsInitiatingDocumentDateLabel = By.XPath("//label[contains(text(),'Initiating document date')]");
        private By dispositionFileDetailsInitiatingDocumentDateContent = By.XPath("//label[contains(text(),'Initiating document date')]/parent::div/following-sibling::div");
        private By dispositionFileDetailsPhysicalFileLabel = By.XPath("//label[contains(text(),'Physical file status')]");
        private By dispositionFileDetailsPhysicalFileContent = By.XPath("//label[contains(text(),'Physical file status')]/parent::div/following-sibling::div");
        private By dispositionFileDetailsInitiatingBranchLabel = By.XPath("//label[contains(text(),'Initiating branch')]");
        private By dispositionFileDetailsInitiatingBranchContent = By.XPath("//label[contains(text(),'Initiating branch')]/parent::div/following-sibling::div");
        private By dispositionFileDetailsMOTIRegionLabel = By.XPath("//label[contains(text(),'Ministry region')]");
        private By dispositionFileDetailsMOTIRegionContent = By.XPath("//label[contains(text(),'Ministry region')]/parent::div/following-sibling::div");

        private By dispositionFileTeamSubtitle = By.XPath("//div[contains(text(),'Disposition Team')]");

        //Disposition File Main Form Input Elements
        private By dispositionFileMainFormDiv = By.XPath("//h1[contains(text(),'Create Disposition File')]/parent::div/parent::div/parent::div/parent::div");

        private By dispositionFileNameInput = By.Id("input-fileName");
        private By dispositionFileNameInvalidMessage = By.XPath("//div[contains(text(),'Disposition file name must be at most 200 characters')]");
        private By dispositionFileReferenceNumberInput = By.Id("input-referenceNumber");
        private By dispositionFileReferenceNumberInvalidMessage = By.XPath("//div[contains(text(),'Disposition reference number must be at most 200 characters')]");
        private By dispositionFileReferenceNumberTooltip = By.XPath("//label[contains(text(),'Reference number')]/span/span[@data-testid='tooltip-icon-section-field-tooltip']");
        private By dispositionFileDispositionStatusSelect = By.Id("input-dispositionStatusTypeCode");
        private By dispositionFileDispositionTypeSelect = By.Id("input-dispositionTypeCode");
        private By dispositionFileInitiatingDocumentSelect = By.Id("input-initiatingDocumentTypeCode");
        private By dispositionFileInitiatingDocumentTooltip = By.XPath("(//label[contains(text(),'Initiating document')]/span/span[@data-testid='tooltip-icon-section-field-tooltip'])[1]");
        private By dispositionFileInitiatingDocumentDateInput = By.Id("datepicker-initiatingDocumentDate");
        private By dispositionFilePhysicalStatusSelect = By.Id("input-physicalFileStatusTypeCode");
        private By dispositionFileInitiatingBranchSelect = By.Id("input-initiatingBranchTypeCode");
        private By dispositionFileDetailsRegionSelect = By.Id("input-regionCode");

        private By dispositionFileAddAnotherMemberLink = By.CssSelector("button[data-testid='add-team-member']");
        private By dispositionFileTeamMembersGroup = By.XPath("//div[contains(text(),'Disposition Team')]/parent::div/parent::h2/following-sibling::div/div[@class='py-3 row']");
        private By dispositionFileTeamFirstMemberDeleteBttn = By.XPath("//div[contains(text(),'Disposition Team')]/parent::div/parent::h2/following-sibling::div/div[@class='py-3 row'][1]/div[3]/button");
        private By dispositionFileTeamInvalidTeamMemberMessage = By.XPath("//div[contains(text(),'Select a team member')]");
        private By dispositionFileTeamInvalidProfileMessage = By.XPath("//div[contains(text(),'Select a profile')]");

        private By dispositionFileEditButton = By.CssSelector("button[title='Edit disposition file']");

        public DispositionFileDetails(IWebDriver webDriver) : base(webDriver)
        {
            //sharedSelectContact = new SharedSelectContact(webDriver);
        }

        public void NavigateToCreateNewDipositionFile()
        {
            Wait(3000);
            FocusAndClick(menuDispositionButton);

            WaitUntilVisible(createDispositionFileButton);
            FocusAndClick(createDispositionFileButton);
        }

        public void NavigateToFileSummary()
        {
            WaitUntilClickable(dispositionFileSummaryBttn);
            webDriver.FindElement(dispositionFileSummaryBttn).Click();
        }

        public void NavigateToFileDetailsTab()
        {
            WaitUntilClickable(dispositionFileDetailsTab);
            webDriver.FindElement(dispositionFileDetailsTab).Click();
        }

        public void CreateMinimumDispositionFile(DispositionFile disposition)
        {
            Wait(2000);

            webDriver.FindElement(dispositionFileNameInput).SendKeys(disposition.DispositionFileName);
            webDriver.FindElement(dispositionFileDispositionStatusSelect);
            ChooseSpecificSelectOption(dispositionFileDispositionStatusSelect, disposition.DispositionStatus);
            webDriver.FindElement(dispositionFileDispositionTypeSelect);
            ChooseSpecificSelectOption(dispositionFileDispositionTypeSelect, disposition.DispositionType);
            ChooseSpecificSelectOption(dispositionFileDetailsRegionSelect, disposition.DispositionMOTIRegion);
        }

        public void SaveDispositionFileDetails()
        {
            Wait();
            ButtonElement("Save");
            AssertTrueIsDisplayed(dispositionFileEditButton);
        }


        public void VerifyDispositionFileCreate()
        {
            AssertTrueIsDisplayed(dispositionFileCreateTitle);


            //Project
            AssertTrueIsDisplayed(dispositionFileProjectSubtitle);
            AssertTrueIsDisplayed(dispositionFileProjectLabel);
            //AssertTrueIsDisplayed(dispositionFileProjectInput);
            AssertTrueIsDisplayed(dispositionFileProjectProductLabel);
            AssertTrueIsDisplayed(dispositionFileProjectFundingLabel);
            AssertTrueIsDisplayed(dispositionFileProjectFundingInput);
            


            //Schedule
            AssertTrueIsDisplayed(dispositionFileScheduleSubtitle);
            AssertTrueIsDisplayed(dispositionFileScheduleAssignedDateLabel);
            AssertTrueIsDisplayed(dispositionFileAssignedDateInput);
            AssertTrueIsDisplayed(dispositionFileScheduleCompletedDateLabel);
            AssertTrueIsDisplayed(dispositionFileCompletedDateLabelInput);



            //Disposition Details
            AssertTrueIsDisplayed(dispositionFileDetailsSubtitle);
            AssertTrueIsDisplayed(dispositionFileDetailsNameLabel);
            AssertTrueIsDisplayed(dispositionFileNameInput);
            AssertTrueIsDisplayed(dispositionFileDetailsReferenceNumberLabel);
            AssertTrueIsDisplayed(dispositionFileReferenceNumberTooltip);
            AssertTrueIsDisplayed(dispositionFileReferenceNumberInput);
            AssertTrueIsDisplayed(dispositionFileDetailsDispositionStatusLabel);
            AssertTrueIsDisplayed(dispositionFileDispositionStatusSelect);
            AssertTrueIsDisplayed(dispositionFileDetailsDispositionTypeLabel);
            AssertTrueIsDisplayed(dispositionFileDispositionTypeSelect);
            AssertTrueIsDisplayed(dispositionFileDetailsInitiatingDocumentLabel);
            AssertTrueIsDisplayed(dispositionFileInitiatingDocumentTooltip);
            AssertTrueIsDisplayed(dispositionFileInitiatingDocumentSelect);
            AssertTrueIsDisplayed(dispositionFileDetailsInitiatingDocumentDateLabel);
            AssertTrueIsDisplayed(dispositionFileInitiatingDocumentDateInput);
            AssertTrueIsDisplayed(dispositionFileDetailsPhysicalFileLabel);
            AssertTrueIsDisplayed(dispositionFilePhysicalStatusSelect);
            AssertTrueIsDisplayed(dispositionFileDetailsInitiatingBranchLabel);
            AssertTrueIsDisplayed(dispositionFileInitiatingBranchSelect);
            AssertTrueIsDisplayed(dispositionFileDetailsMOTIRegionLabel);
            AssertTrueIsDisplayed(dispositionFileDetailsRegionSelect);

            VerifyMaximumFields();

            //Team members
            AssertTrueIsDisplayed(dispositionFileTeamSubtitle);
            AssertTrueIsDisplayed(dispositionFileAddAnotherMemberLink);

        }

        private void VerifyMaximumFields()
        {
            //Verify File Name Input
            webDriver.FindElement(dispositionFileNameInput).SendKeys("Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibus");
            webDriver.FindElement(dispositionFileDetailsNameLabel).Click();
            AssertTrueIsDisplayed(dispositionFileNameInvalidMessage);
            ClearInput(dispositionFileNameInput);

            //Verify Reference File Number Input
            webDriver.FindElement(dispositionFileReferenceNumberInput).SendKeys("Test Reference numberTest Reference numberTest Reference numberTest Reference numberTest Reference numberTest Reference numberTest Reference numberTest Reference numberTest Reference numberTest Referen");
            webDriver.FindElement(dispositionFileDetailsReferenceNumberLabel).Click();
            AssertTrueIsDisplayed(dispositionFileReferenceNumberInvalidMessage);
            ClearInput(dispositionFileReferenceNumberInput);
        }
    }
}
