﻿using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;
using System.Text.RegularExpressions;

namespace PIMS.Tests.Automation.PageObjects
{
    public class DispositionFileDetails : PageObjectBase
    {
        //Disposition Files Menu Elements
        private By menuDispositionButton = By.XPath("//body/div[@id='root']/div[2]/div[1]/div[1]/div[@data-testid='nav-tooltip-disposition']/a");
        private By createDispositionFileButton = By.XPath("//a[contains(text(),'Create a Disposition File')]");

        private By dispositionFileSummaryBttn = By.XPath("//div[contains(text(),'File Summary')]");
        private By dispositionFileDetailsTab = By.XPath("//a[contains(text(),'File Details')]");

        //Disposition Edit Details button
        private By dispositionFileEditButton = By.CssSelector("button[title='Edit disposition file']");

        //Disposition File Details View and Create Forms Elements
        private By dispositionFileViewTitle = By.XPath("//html[1]/body[1]/div[1]/div[2]/div[2]/div[1]/div[1]/div[1]/div[1]/div[1]/h1[contains(text(),'Disposition File')]");
        private By dispositionFileMainFormDiv = By.XPath("//h1[contains(text(),'Create Disposition File')]/parent::div/parent::div/parent::div/parent::div");

        private By dispositionFileCreateTitle = By.XPath("//*[contains(text(),'Create Disposition File')]");
        private By dispositionFileCloseFormBttn = By.XPath("//h1[contains(text(),'Disposition File')]/parent::div/following-sibling::div/*[3]");

        private By dispositionFileHeaderCodeLabel = By.XPath("//label[contains(text(), 'File:')]");
        private By dispositionFileHeaderCodeContent = By.XPath("//label[contains(text(), 'File:')]/parent::div/following-sibling::div");
        private By dispositionFileHistoricalLabel = By.XPath("//label[contains(text(), 'Historical file')]");
        private By dispositionFileHistoricalContent = By.XPath("//label[contains(text(), 'Historical file')]/parent::div/following-sibling::div/div/span");
        private By dispositionFileHeaderCreatedDateLabel = By.XPath("//strong[contains(text(), 'Created')]");
        private By dispositionFileHeaderCreatedDateContent = By.XPath("//strong[contains(text(), 'Created')]/parent::span");
        private By dispositionFileHeaderCreatedByContent = By.XPath("//strong[contains(text(),'Created')]/parent::span/span[@id='userNameTooltip']/strong");
        private By dispositionFileHeaderLastUpdateLabel = By.XPath("//strong[contains(text(), 'Updated')]");
        private By dispositionFileHeaderLastUpdateContent = By.XPath("//strong[contains(text(), 'Updated')]/parent::span");
        private By dispositionFileHeaderLastUpdateByContent = By.XPath("//strong[contains(text(), 'Updated')]/parent::span/span[@id='userNameTooltip']/strong");
        private By dispositionFileHeaderStatusContent = By.XPath("//b[contains(text(),'File')]/parent::span/following-sibling::div");

        private By dispositionFileStatusSelect = By.Id("input-fileStatusTypeCode");
        private By dispositionFileProjectSubtitle = By.XPath("//h2/div/div[contains(text(), 'Project')]");
        private By dispositionFileProjectLabel = By.XPath("//div[@class='collapse show']/div/div/label[contains(text(),'Ministry project')]");
        private By dispositionFileProjectInput = By.CssSelector("input[id='typeahead-project']");
        private By dispositionFileProject1stOption = By.CssSelector("div[id='typeahead-project'] a");
        private By dispositionFileProjectContent = By.XPath("//div[@class='collapse show']/div/div/label[contains(text(),'Ministry project')]/parent::div/following-sibling::div");
        private By dispositionFileProjectProductLabel = By.XPath("//label[contains(text(),'Product')]");
        private By dispositionFileProjectProductSelect = By.Id("input-productId");
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
        private By dispositionFileDetailsNameInput = By.Id("input-fileName");

        private By dispositionFileDetailsReferenceNumberLabel = By.XPath("//label[contains(text(),'Reference number')]");
        private By dispositionFileDetailsReferenceNumberTooltip = By.XPath("//label[contains(text(),'Reference number')]/span/span[@data-testid='tooltip-icon-section-field-tooltip']");
        private By dispositionFileDetailsReferenceNumberContent = By.XPath("//label[contains(text(),'Reference number')]/parent::div/following-sibling::div");
        private By dispositionFileDetailsReferenceNumberInput = By.Id("input-referenceNumber");
       
        private By dispositionFileDetailsStatusLabel = By.XPath("//label[contains(text(),'Disposition status')]");
        private By dispositionFileDetailsStatusContent = By.XPath("//label[contains(text(),'Disposition status')]/parent::div/following-sibling::div");
        private By dispositionFileDetailsStatusSelect = By.Id("input-dispositionStatusTypeCode");
        
        private By dispositionFileDetailsTypeLabel = By.XPath("//label[contains(text(),'Disposition type')]");
        private By dispositionFileDetailsTypeContent = By.XPath("//label[contains(text(),'Disposition type')]/parent::div/following-sibling::div");
        private By dispositionFileDetailsTypeSelect = By.Id("input-dispositionTypeCode");

        private By dispositionFileDetailsOtherTransferTypeLabel = By.XPath("//label[contains(text(),'Other (disposition type)')]");
        private By dispositionFileDetailsOtherTransferTypeContent = By.XPath("//label[contains(text(),'Other (disposition type)')]/parent::div/following-sibling::div");
        private By dispositionFileDetailsOtherTransferTypeInput = By.Id("input-dispositionTypeOther");

        private By dispositionFileDetailsInitiatingDocumentLabel = By.XPath("//label[contains(text(),'Initiating document')]");
        private By dispositionFileDetailsInitiatingDocumentContent = By.XPath("(//label[contains(text(),'Initiating document')]/parent::div/following-sibling::div)[1]");
        private By dispositionFileDetailsInitiatingDocumentSelect = By.Id("input-initiatingDocumentTypeCode");

        private By dispositionFileDetailsOtherInitiatingDocumentLabel = By.XPath("//label[contains(text(),'Other (initiating document)')]");
        private By dispositionFileDetailsInitiatingDocumentTooltip = By.XPath("(//label[contains(text(),'Initiating document')]/span/span[@data-testid='tooltip-icon-section-field-tooltip'])[1]");
        private By dispositionFileDetailsOtherInitiatingDocumentContent = By.XPath("//label[contains(text(),'Other (initiating document)')]/parent::div/following-sibling::div");
        private By dispositionFileDetailsOtherInitiatingDocumentInput = By.Id("input-initiatingDocumentTypeOther");

        private By dispositionFileDetailsInitiatingDocumentDateLabel = By.XPath("//label[contains(text(),'Initiating document date')]");
        private By dispositionFileDetailsInitiatingDocumentDateContent = By.XPath("//label[contains(text(),'Initiating document date')]/parent::div/following-sibling::div");
        private By dispositionFileDetailsInitiatingDocumentDateInput = By.Id("datepicker-initiatingDocumentDate");

        private By dispositionFileDetailsPhysicalFileLabel = By.XPath("//label[contains(text(),'Physical file status')]");
        private By dispositionFileDetailsPhysicalFileContent = By.XPath("//label[contains(text(),'Physical file status')]/parent::div/following-sibling::div");
        private By dispositionFileDetailsPhysicalFileSelect = By.Id("input-physicalFileStatusTypeCode");

        private By dispositionFileDetailsInitiatingBranchLabel = By.XPath("//label[contains(text(),'Initiating branch')]");
        private By dispositionFileDetailsInitiatingBranchContent = By.XPath("//label[contains(text(),'Initiating branch')]/parent::div/following-sibling::div");
        private By dispositionFileDetailsInitiatingBranchSelect = By.Id("input-initiatingBranchTypeCode");

        private By dispositionFileDetailsMOTIRegionLabel = By.XPath("//label[contains(text(),'Ministry region')]");
        private By dispositionFileDetailsMOTIRegionContent = By.XPath("//label[contains(text(),'Ministry region')]/parent::div/following-sibling::div");
        private By dispositionFileDetailsMOTIRegionSelect = By.Id("input-regionCode");

        //Invalid Error messages elements
        private By dispositionFileNameInvalidMessage = By.XPath("//div[contains(text(),'Disposition file name must be at most 200 characters')]");
        private By dispositionFileReferenceNumberInvalidMessage = By.XPath("//div[contains(text(),'Disposition reference number must be at most 200 characters')]");

        //Team members static elements
        private By dispositionFileTeamSubtitle = By.XPath("//div[contains(text(),'Disposition Team')]");

        private By dispositionFileAddAnotherMemberLink = By.CssSelector("button[data-testid='add-team-member']");
        private By dispositionFileTeamMembersGroup = By.XPath("//div[contains(text(),'Disposition Team')]/parent::div/parent::h2/following-sibling::div/div[@class='py-3 row']");
        private By dispositionFileTeamFirstMemberDeleteBttn = By.XPath("//div[contains(text(),'Disposition Team')]/parent::div/parent::h2/following-sibling::div/div[@class='py-3 row'][1]/div[3]/button");
        private By dispositionFileTeamInvalidTeamMemberMessage = By.XPath("//div[contains(text(),'Select a team member')]");
        private By dispositionFileTeamInvalidProfileMessage = By.XPath("//div[contains(text(),'Select a profile')]");

        //Acquisition File Confirmation Modal Elements
        private By dispositionFileConfirmationModal = By.CssSelector("div[class='modal-content']");

        private SharedSelectContact sharedSelectContact;
        private SharedFileProperties sharedFileProperties;
        private SharedModals sharedModals;

        public DispositionFileDetails(IWebDriver webDriver) : base(webDriver)
        {
            sharedSelectContact = new SharedSelectContact(webDriver);
            sharedFileProperties = new SharedFileProperties(webDriver);
            sharedModals = new SharedModals(webDriver); 
        }

        public void NavigateToCreateNewDipositionFile()
        {
            Wait();
            FocusAndClick(menuDispositionButton);

            Wait();
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

        public void EditDispositionFileBttn()
        {
            WaitUntilSpinnerDisappear();
            webDriver.FindElement(dispositionFileEditButton).Click();
        }

        public void CreateMinimumDispositionFile(DispositionFile disposition)
        {
            Wait();

            webDriver.FindElement(dispositionFileDetailsNameInput).SendKeys(disposition.DispositionFileName);
            ChooseSpecificSelectOption(dispositionFileDetailsStatusSelect, disposition.DispositionStatus);
            ChooseSpecificSelectOption(dispositionFileDetailsTypeSelect, disposition.DispositionType);

            if(disposition.DispositionType.Equals("Other Transfer"))
                webDriver.FindElement(dispositionFileDetailsOtherTransferTypeInput).SendKeys(disposition.DispositionOtherTransferType);

            ChooseSpecificSelectOption(dispositionFileDetailsMOTIRegionSelect, disposition.DispositionMOTIRegion);
        }

        public void AddAdditionalInformation(DispositionFile disposition)
        {
            //Status
            if (disposition.DispositionFileStatus != "")
                ChooseSpecificSelectOption(dispositionFileStatusSelect, disposition.DispositionFileStatus);

            //Project
            if (disposition.DispositionProject != "")
            {
                WaitUntilVisible(dispositionFileProjectInput);
                webDriver.FindElement(dispositionFileProjectInput).SendKeys(disposition.DispositionProject);

                Wait();
                webDriver.FindElement(dispositionFileProjectInput).SendKeys(Keys.Space);

                Wait();
                webDriver.FindElement(dispositionFileProjectInput).SendKeys(Keys.Backspace);

                Wait(2000);
                FocusAndClick(dispositionFileProject1stOption);
            }

            if (disposition.DispositionProjProduct != "")
            {
                WaitUntilVisible(dispositionFileProjectProductSelect);
                webDriver.FindElement(dispositionFileProjectProductSelect).Click();

                ChooseSpecificSelectOption(dispositionFileProjectProductSelect, disposition.DispositionProjProduct);
            }

            if (disposition.DispositionProjFunding != "")
                ChooseSpecificSelectOption(dispositionFileProjectFundingInput, disposition.DispositionProjFunding);


            //Disposition Details
            if (disposition.DispositionAssignedDate != "")
            {
                ClearInput(dispositionFileAssignedDateInput);
                webDriver.FindElement(dispositionFileAssignedDateInput).SendKeys(disposition.DispositionAssignedDate);
                webDriver.FindElement(dispositionFileAssignedDateInput).SendKeys(Keys.Enter);
            }

            if (disposition.DispositionCompletedDate != "")
            {
                ClearInput(dispositionFileCompletedDateLabelInput);
                webDriver.FindElement(dispositionFileCompletedDateLabelInput).SendKeys(disposition.DispositionCompletedDate);
                webDriver.FindElement(dispositionFileCompletedDateLabelInput).SendKeys(Keys.Enter);
            }

            if (disposition.DispositionReferenceNumber != "")
            {
                ClearInput(dispositionFileDetailsReferenceNumberInput);
                webDriver.FindElement(dispositionFileDetailsReferenceNumberInput).SendKeys(disposition.DispositionReferenceNumber);
            }

            if (disposition.DispositionStatus != "")
                ChooseSpecificSelectOption(dispositionFileDetailsStatusSelect, disposition.DispositionStatus);

            if (disposition.DispositionType != "")
                ChooseSpecificSelectOption(dispositionFileDetailsTypeSelect, disposition.DispositionType);

            if (disposition.DispositionOtherTransferType != "")
            {
                ClearInput(dispositionFileDetailsOtherTransferTypeInput);
                webDriver.FindElement(dispositionFileDetailsOtherTransferTypeInput).SendKeys(disposition.DispositionOtherTransferType);
            }

            if (disposition.InitiatingDocument != "")
                ChooseSpecificSelectOption(dispositionFileDetailsInitiatingDocumentSelect, disposition.InitiatingDocument);

            if (disposition.OtherInitiatingDocument != "")
            {
                ClearInput(dispositionFileDetailsOtherInitiatingDocumentInput);
                webDriver.FindElement(dispositionFileDetailsOtherInitiatingDocumentInput).SendKeys(disposition.OtherInitiatingDocument);
            }

            if (disposition.InitiatingDocumentDate != "")
            {
                ClearInput(dispositionFileDetailsInitiatingDocumentDateInput);
                webDriver.FindElement(dispositionFileDetailsInitiatingDocumentDateInput).SendKeys(disposition.InitiatingDocumentDate);
                webDriver.FindElement(dispositionFileDetailsInitiatingDocumentDateInput).SendKeys(Keys.Enter);
            }

            if (disposition.DispositionPhysicalFileStatus != "")
                ChooseSpecificSelectOption(dispositionFileDetailsPhysicalFileSelect, disposition.DispositionPhysicalFileStatus);

            if (disposition.InitiatingBranch != "")
                ChooseSpecificSelectOption(dispositionFileDetailsInitiatingBranchSelect, disposition.InitiatingBranch);

            //Disposition File Team Members
            if (disposition.DispositionTeam.Count > 0)
            {
                for (var i = 0; i < disposition.DispositionTeam.Count; i++)
                {
                    AddTeamMembers(disposition.DispositionTeam[i]);
                }
            }
        }

        public void UpdateDispositionFile(DispositionFile disposition)
        {
            //Status
            if (disposition.DispositionFileStatus != "")
            {
                WaitUntilClickable(dispositionFileStatusSelect);
                ChooseSpecificSelectOption(dispositionFileStatusSelect, disposition.DispositionFileStatus);
            }

            //Project
            if (disposition.DispositionProject != "")
            {
                WaitUntilVisible(dispositionFileProjectInput);
                webDriver.FindElement(dispositionFileProjectInput).SendKeys(disposition.DispositionProject);

                Wait();
                webDriver.FindElement(dispositionFileProjectInput).SendKeys(Keys.Space);

                Wait();
                webDriver.FindElement(dispositionFileProjectInput).SendKeys(Keys.Backspace);

                Wait(2000);
                FocusAndClick(dispositionFileProject1stOption);
            }

            if (disposition.DispositionProjProduct != "")
            {
                WaitUntilVisible(dispositionFileProjectProductSelect);
                webDriver.FindElement(dispositionFileProjectProductSelect).Click();

                ChooseSpecificSelectOption(dispositionFileProjectProductSelect, disposition.DispositionProjProduct);
            }

            if (disposition.DispositionProjFunding != "")
                ChooseSpecificSelectOption(dispositionFileProjectFundingInput, disposition.DispositionProjFunding);

            //Schedule
            if (disposition.DispositionAssignedDate != "")
            {
                ClearInput(dispositionFileAssignedDateInput);
                webDriver.FindElement(dispositionFileAssignedDateInput).SendKeys(disposition.DispositionAssignedDate);
                webDriver.FindElement(dispositionFileAssignedDateInput).SendKeys(Keys.Enter);
            }

            if (disposition.DispositionCompletedDate != "")
            {
                ClearInput(dispositionFileCompletedDateLabelInput);
                webDriver.FindElement(dispositionFileCompletedDateLabelInput).SendKeys(disposition.DispositionCompletedDate);
                webDriver.FindElement(dispositionFileCompletedDateLabelInput).SendKeys(Keys.Enter);
            }

            //Disposition Details
            if (disposition.DispositionFileName!= "")
            {
                ClearInput(dispositionFileDetailsNameInput);
                webDriver.FindElement(dispositionFileDetailsNameInput).SendKeys(disposition.DispositionFileName);
            }

            if (disposition.DispositionReferenceNumber != "")
            {
                ClearInput(dispositionFileDetailsReferenceNumberInput);
                webDriver.FindElement(dispositionFileDetailsReferenceNumberInput).SendKeys(disposition.DispositionReferenceNumber);
            }

            if (disposition.DispositionStatus != "")
                ChooseSpecificSelectOption(dispositionFileDetailsStatusSelect, disposition.DispositionStatus);

            if (disposition.DispositionType != "")
                ChooseSpecificSelectOption(dispositionFileDetailsTypeSelect, disposition.DispositionType);

            if (disposition.InitiatingDocument != "")
                ChooseSpecificSelectOption(dispositionFileDetailsInitiatingDocumentSelect, disposition.InitiatingDocument);

            if (disposition.OtherInitiatingDocument != "")
            {
                ClearInput(dispositionFileDetailsOtherInitiatingDocumentInput);
                webDriver.FindElement(dispositionFileDetailsOtherInitiatingDocumentInput).SendKeys(disposition.OtherInitiatingDocument);
            }

            if (disposition.InitiatingDocumentDate != "")
            {
                ClearInput(dispositionFileDetailsInitiatingDocumentDateInput);
                webDriver.FindElement(dispositionFileDetailsInitiatingDocumentDateInput).SendKeys(disposition.InitiatingDocumentDate);
                webDriver.FindElement(dispositionFileDetailsInitiatingDocumentDateInput).SendKeys(Keys.Enter);
            }

            if (disposition.DispositionPhysicalFileStatus != "")
                ChooseSpecificSelectOption(dispositionFileDetailsPhysicalFileSelect, disposition.DispositionPhysicalFileStatus);

            if (disposition.InitiatingBranch != "")
                ChooseSpecificSelectOption(dispositionFileDetailsInitiatingBranchSelect, disposition.InitiatingBranch);

            if (disposition.DispositionMOTIRegion != "")
                ChooseSpecificSelectOption(dispositionFileDetailsMOTIRegionSelect, disposition.DispositionMOTIRegion);

            //Disposition File Team Members
            if (disposition.DispositionTeam.Count > 0)
            {
                while (webDriver.FindElements(dispositionFileTeamMembersGroup).Count > 0)
                    DeleteFirstStaffMember();

                for (var i = 0; i < disposition.DispositionTeam.Count; i++)
                {
                    AddTeamMembers(disposition.DispositionTeam[i]);
                }
            }
        }

        public void SaveDispositionFileDetails()
        {
            Wait();
            ButtonElement("Save");

            Wait();
            while (webDriver.FindElements(dispositionFileConfirmationModal).Count() > 0)
            {
                if (sharedModals.ModalContent().Contains("You are changing this file to a non-editable state"))
                {
                    Assert.Equal("User Override Required", sharedModals.ModalHeader());
                    Assert.Equal("You are changing this file to a non-editable state. (Only system administrators can edit the file when set to Archived, Cancelled or Completed state). Do you wish to continue?", sharedModals.ModalContent());
                    sharedModals.ModalClickOKBttn();

                }
                else if (sharedModals.ModalContent().Contains("The Ministry region has been changed"))
                {
                    Assert.Equal("User Override Required", sharedModals.ModalHeader());
                    Assert.Equal("The Ministry region has been changed, this will result in a change to the file's prefix. This requires user confirmation.", sharedModals.ModalContent());
                    sharedModals.ModalClickOKBttn();
                }
                else if (sharedModals.ModalHeader() == "Confirm status change")
                {
                    Assert.Equal("Confirm status change", sharedModals.ModalHeader());
                    Assert.Contains("If you save it, only the administrator can turn it back on. You will still see it in the management table.", sharedModals.ConfirmationModalParagraph1());
                    Assert.Equal("Do you want to acknowledge and proceed?", sharedModals.ConfirmationModalParagraph2());
                    sharedModals.ModalClickOKBttn();

                    Wait();
                }
                else if (sharedModals.ModalHeader() == "Error")
                    break;

                Wait();
            }

        }

        public void CancelDispositionFile()
        {
            Wait();
            ButtonElement("Cancel");

            sharedModals.CancelActionModal();
        }

        public void CloseDispositionForm()
        {
            Wait();
            webDriver.FindElement(dispositionFileCloseFormBttn).Click();

        }

        public int IsCreateDispositionFileFormVisible()
        {
            return webDriver.FindElements(dispositionFileMainFormDiv).Count();
        }

        //public string GetDispositionFileName()
        //{
        //    //WaitUntilVisible(dispositionFileHeaderCodeContent);

        //    //var totalFileName = webDriver.FindElement(dispositionFileHeaderCodeContent).Text;
        //    //return Regex.Match(totalFileName, "^[^ ]+").Value;

        //}

        public void VerifyDispositionFileInitCreate()
        {
            //AssertTrueIsDisplayed(dispositionFileCreateTitle);

            //Project
            AssertTrueIsDisplayed(dispositionFileProjectSubtitle);
            AssertTrueIsDisplayed(dispositionFileProjectLabel);
            AssertTrueIsDisplayed(dispositionFileProjectInput);
            AssertTrueIsDisplayed(dispositionFileProjectFundingLabel);
            AssertTrueIsDisplayed(dispositionFileProjectFundingInput);

            //Schedule
            AssertTrueIsDisplayed(dispositionFileScheduleSubtitle);
            AssertTrueIsDisplayed(dispositionFileScheduleAssignedDateLabel);
            AssertTrueIsDisplayed(dispositionFileAssignedDateInput);

            //Properties Selection
            sharedFileProperties.VerifyLocateOnMapFeature("Disposition");

            //Disposition Details
            AssertTrueIsDisplayed(dispositionFileDetailsSubtitle);
            AssertTrueIsDisplayed(dispositionFileDetailsNameLabel);
            AssertTrueIsDisplayed(dispositionFileDetailsNameInput);
            AssertTrueIsDisplayed(dispositionFileDetailsReferenceNumberLabel);
            AssertTrueIsDisplayed(dispositionFileDetailsReferenceNumberTooltip);
            AssertTrueIsDisplayed(dispositionFileDetailsReferenceNumberInput);
            AssertTrueIsDisplayed(dispositionFileDetailsStatusLabel);
            AssertTrueIsDisplayed(dispositionFileDetailsStatusSelect);
            AssertTrueIsDisplayed(dispositionFileDetailsTypeLabel);
            AssertTrueIsDisplayed(dispositionFileDetailsTypeSelect);
            AssertTrueIsDisplayed(dispositionFileDetailsInitiatingDocumentLabel);
            AssertTrueIsDisplayed(dispositionFileDetailsInitiatingDocumentTooltip);
            AssertTrueIsDisplayed(dispositionFileDetailsInitiatingDocumentSelect);
            AssertTrueIsDisplayed(dispositionFileDetailsInitiatingDocumentDateLabel);
            AssertTrueIsDisplayed(dispositionFileDetailsInitiatingDocumentDateInput);
            AssertTrueIsDisplayed(dispositionFileDetailsPhysicalFileLabel);
            AssertTrueIsDisplayed(dispositionFileDetailsPhysicalFileSelect);
            AssertTrueIsDisplayed(dispositionFileDetailsInitiatingBranchLabel);
            AssertTrueIsDisplayed(dispositionFileDetailsInitiatingBranchSelect);
            AssertTrueIsDisplayed(dispositionFileDetailsMOTIRegionLabel);
            AssertTrueIsDisplayed(dispositionFileDetailsMOTIRegionSelect);

            //Team members
            AssertTrueIsDisplayed(dispositionFileTeamSubtitle);
            AssertTrueIsDisplayed(dispositionFileAddAnotherMemberLink);
        }

        public void VerifyDispositionFileView(DispositionFile disposition)
        {
            AssertTrueIsDisplayed(dispositionFileViewTitle);

            //Header
            AssertTrueIsDisplayed(dispositionFileHeaderCodeLabel);
            AssertTrueContentNotEquals(dispositionFileHeaderCodeContent, "");

            AssertTrueIsDisplayed(dispositionFileHistoricalLabel);
            //Assert.True(webDriver.FindElements(dispositionFileHistoricalContent).Count > 0);

            AssertTrueIsDisplayed(dispositionFileHeaderCreatedDateLabel);
            AssertTrueContentNotEquals(dispositionFileHeaderCreatedDateContent, "");
            AssertTrueContentNotEquals(dispositionFileHeaderCreatedByContent, "");
            AssertTrueIsDisplayed(dispositionFileHeaderLastUpdateLabel);
            AssertTrueContentNotEquals(dispositionFileHeaderLastUpdateContent, "");
            AssertTrueContentNotEquals(dispositionFileHeaderLastUpdateByContent, "");

            AssertTrueIsDisplayed(dispositionFileHeaderStatusContent);

            //Status
            if (disposition.DispositionFileStatus != "")
                AssertTrueContentEquals(dispositionFileHeaderStatusContent, GetUppercaseString(disposition.DispositionFileStatus));

            //Project
            AssertTrueIsDisplayed(dispositionFileProjectSubtitle);
            AssertTrueIsDisplayed(dispositionFileProjectLabel);

            if (disposition.DispositionProject != "")
                AssertTrueContentEquals(dispositionFileProjectContent, TransformProjectFormat(disposition.DispositionProject));

            AssertTrueIsDisplayed(dispositionFileProjectProductLabel);

            if (disposition.DispositionProjProduct != "")
                AssertTrueContentEquals(dispositionFileProjectProductContent, disposition.DispositionProjProduct);

            AssertTrueIsDisplayed(dispositionFileProjectFundingLabel);

            if (disposition.DispositionProjFunding != "")
                AssertTrueContentEquals(dispositionFileProjectFundingContent, disposition.DispositionProjFunding);

            //Schedule
            AssertTrueIsDisplayed(dispositionFileScheduleSubtitle);
            AssertTrueIsDisplayed(dispositionFileScheduleAssignedDateLabel);
            AssertTrueContentEquals(dispositionFileScheduleAssignedDateContent, TransformDateFormat(disposition.DispositionAssignedDate));

            AssertTrueIsDisplayed(dispositionFileScheduleCompletedDateLabel);
            if (disposition.DispositionCompletedDate != "")
                AssertTrueContentEquals(dispositionFileScheduleCompletedDateContent, TransformDateFormat(disposition.DispositionCompletedDate));

            //Details
            AssertTrueIsDisplayed(dispositionFileDetailsSubtitle);

            //File name
            AssertTrueIsDisplayed(dispositionFileDetailsNameLabel);
            
            if (disposition.DispositionFileName != "")
                AssertTrueContentEquals(dispositionFileDetailsNameContent, disposition.DispositionFileName);

            //Reference Number
            AssertTrueIsDisplayed(dispositionFileDetailsReferenceNumberLabel);

            if (disposition.DispositionReferenceNumber != "")
                AssertTrueContentEquals(dispositionFileDetailsReferenceNumberContent, disposition.DispositionReferenceNumber);

            //Disposition Status
            AssertTrueIsDisplayed(dispositionFileDetailsStatusLabel);

            if (disposition.DispositionStatus != "")
                AssertTrueContentEquals(dispositionFileDetailsStatusContent, disposition.DispositionStatus);

            //Disposition Type
            AssertTrueIsDisplayed(dispositionFileDetailsTypeLabel);

            if (disposition.DispositionType != "")
                AssertTrueContentEquals(dispositionFileDetailsTypeContent, disposition.DispositionType);

            if (disposition.DispositionOtherTransferType != "")
            {
                AssertTrueIsDisplayed(dispositionFileDetailsOtherTransferTypeLabel);
                AssertTrueContentEquals(dispositionFileDetailsOtherTransferTypeContent, disposition.DispositionOtherTransferType);
            }

            //Initiating Document
            AssertTrueIsDisplayed(dispositionFileDetailsInitiatingDocumentLabel);

            if (disposition.InitiatingDocument != "")
                AssertTrueContentEquals(dispositionFileDetailsInitiatingDocumentContent, disposition.InitiatingDocument);

            if (disposition.OtherInitiatingDocument != "")
            {
                AssertTrueIsDisplayed(dispositionFileDetailsOtherInitiatingDocumentLabel);
                AssertTrueContentEquals(dispositionFileDetailsOtherInitiatingDocumentContent, disposition.OtherInitiatingDocument);
            }

            //Initiating Document Date
            AssertTrueIsDisplayed(dispositionFileDetailsInitiatingDocumentDateLabel);

            if (disposition.InitiatingDocumentDate != "")
                AssertTrueContentEquals(dispositionFileDetailsInitiatingDocumentDateContent, TransformDateFormat(disposition.InitiatingDocumentDate));

            //Physical File Status
            AssertTrueIsDisplayed(dispositionFileDetailsPhysicalFileLabel);

            if (disposition.DispositionPhysicalFileStatus != "")
                AssertTrueContentEquals(dispositionFileDetailsPhysicalFileContent, disposition.DispositionPhysicalFileStatus);

            //Initiating Branch
            AssertTrueIsDisplayed(dispositionFileDetailsInitiatingBranchLabel);

            if (disposition.InitiatingBranch != "")
                AssertTrueContentEquals(dispositionFileDetailsInitiatingBranchContent, disposition.InitiatingBranch);

            //MOTI Region
            AssertTrueIsDisplayed(dispositionFileDetailsMOTIRegionLabel);
            AssertTrueContentEquals(dispositionFileDetailsMOTIRegionContent, disposition.DispositionMOTIRegion);

            //Team members
            AssertTrueIsDisplayed(dispositionFileTeamSubtitle);
            if (disposition.DispositionTeam.Count > 0)
            {
                var index = 1;

                for (var i = 0; i < disposition.DispositionTeam.Count; i++)
                {
                    AssertTrueContentEquals(By.XPath("//h2/div/div[contains(text(),'Disposition Team')]/parent::div/parent::h2/following-sibling::div/div[" + index + "]/div/label"), disposition.DispositionTeam[i].TeamMemberRole + ":");
                    AssertTrueContentEquals(By.XPath("//h2/div/div[contains(text(),'Disposition Team')]/parent::div/parent::h2/following-sibling::div/div[" + index + "]/div/a"), disposition.DispositionTeam[i].TeamMemberContactName);

                    if (disposition.DispositionTeam[i].TeamMemberPrimaryContact != "")
                    {
                        index++;
                        if (webDriver.FindElements(By.XPath("//h2/div/div[contains(text(),'Disposition Team')]/parent::div/parent::h2/following-sibling::div/div[" + index + "]/div/a")).Count > 0)
                            AssertTrueContentEquals(By.XPath("//h2/div/div[contains(text(),'Disposition Team')]/parent::div/parent::h2/following-sibling::div/div[" + index + "]/div/a"), disposition.DispositionTeam[i].TeamMemberPrimaryContact);
                        else
                            AssertTrueContentEquals(By.XPath("//h2/div/div[contains(text(),'Disposition Team')]/parent::div/parent::h2/following-sibling::div/div[" + index + "]/div[2]"), disposition.DispositionTeam[i].TeamMemberPrimaryContact);
                    }
                    index++;
                }
            }
        }

        public string VerifyDispositionFileHeaderStatus()
        {
            Wait();
            return webDriver.FindElement(dispositionFileHeaderStatusContent).Text;
        }

        public void DeleteFirstStaffMember()
        {
            WaitUntilClickable(dispositionFileTeamFirstMemberDeleteBttn);
            webDriver.FindElement(dispositionFileTeamFirstMemberDeleteBttn).Click();

            WaitUntilVisible(dispositionFileConfirmationModal);
            Assert.Equal("Remove Team Member", sharedModals.ModalHeader());
            Assert.Equal("Do you wish to remove this team member?", sharedModals.ModalContent());

            sharedModals.ModalClickOKBttn();
        }

        public void VerifyMaximumFields()
        {
            //Capture fields data before verifying max fields capacity
            var fileName = webDriver.FindElement(dispositionFileDetailsNameInput).GetDomProperty("value");
            var referenceNumber = webDriver.FindElement(dispositionFileDetailsReferenceNumberInput).GetDomProperty("value");

            //Verify File Name Input
            webDriver.FindElement(dispositionFileDetailsNameInput).SendKeys("Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibus");
            webDriver.FindElement(dispositionFileDetailsNameLabel).Click();
            AssertTrueIsDisplayed(dispositionFileNameInvalidMessage);
            ClearInput(dispositionFileDetailsNameInput);

            //Verify Reference File Number Input
            webDriver.FindElement(dispositionFileDetailsReferenceNumberInput).SendKeys("Test Reference numberTest Reference numberTest Reference numberTest Reference numberTest Reference numberTest Reference numberTest Reference numberTest Reference numberTest Reference numberTest Referen");
            webDriver.FindElement(dispositionFileDetailsReferenceNumberLabel).Click();
            AssertTrueIsDisplayed(dispositionFileReferenceNumberInvalidMessage);
            ClearInput(dispositionFileDetailsReferenceNumberInput);

            //Insert back the data that was in the form
            webDriver.FindElement(dispositionFileDetailsNameInput).SendKeys(fileName);
            webDriver.FindElement(dispositionFileDetailsReferenceNumberInput).SendKeys(referenceNumber);

        }

        public void SoldStatusError()
        {
            Wait();
            Assert.Equal("File Disposition Status has not been set to SOLD, so the related file properties cannot be Disposed. To proceed, set file disposition status to SOLD, or cancel the Disposition file.", sharedModals.ModalContent());
        }

        public void NonCorePropertyError()
        {
            Wait(4000);
            Assert.Equal("You have one or more properties attached to this Disposition file that is NOT in the \"Core Inventory\" (i.e. owned by BCTFA and/or HMK). To complete this file you must either, remove these non \"Non-Core Inventory\" properties, OR make sure the property is added to the PIMS inventory first.", sharedModals.ModalContent());
        }

        private void AddTeamMembers(TeamMember teamMember)
        {
            WaitUntilClickable(dispositionFileAddAnotherMemberLink);
            FocusAndClick(dispositionFileAddAnotherMemberLink);

            Wait();
            var teamMemberIndex = webDriver.FindElements(dispositionFileTeamMembersGroup).Count() -1;

            WaitUntilVisible(By.CssSelector("select[id='input-team."+ teamMemberIndex +".teamProfileTypeCode']"));
            ChooseSpecificSelectOption(By.CssSelector("select[id='input-team."+ teamMemberIndex +".teamProfileTypeCode']"), teamMember.TeamMemberRole);
            FocusAndClick(By.CssSelector("div[data-testid='teamMemberRow["+ teamMemberIndex +"]'] div[data-testid='contact-input'] button[title='Select Contact']"));
            sharedSelectContact.SelectContact(teamMember.TeamMemberContactName, teamMember.TeamMemberContactType);

            Wait();
            if (webDriver.FindElements(By.Id("input-team."+ teamMemberIndex +".primaryContactId")).Count > 0)
                ChooseSpecificSelectOption(By.Id("input-team."+ teamMemberIndex +".primaryContactId"), teamMember.TeamMemberPrimaryContact);
        }
    }
}
