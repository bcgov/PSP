using OpenQA.Selenium;
using System.Text.RegularExpressions;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class AcquisitionDetails : PageObjectBase
    {
        //Acquisition Files Menu Elements
        private readonly By menuAcquisitionButton = By.XPath("//body/div[@id='root']/div[2]/div[1]/div[1]/div[@data-testid='nav-tooltip-acquisition']/a");
        private readonly By createAcquisitionFileButton = By.XPath("//a[contains(text(),'Create an Acquisition File')]");

        private readonly By acquisitionFileSummaryBttn = By.CssSelector("div[data-testid='menu-item-summary'] button[title='File Details']");
        private readonly By acquisitionFileDetailsTab = By.CssSelector("a[data-rb-event-key='fileDetails']");
        private readonly By acquisitionSubfilesTab = By.XPath("//a[contains(text(),'Sub-Files')]");

        //Generate Forms Elements
        private readonly By acquisitionFileGenerateFormTitle = By.XPath("//span[text()='Generate a form:']");
        private readonly By acquisitionFileGenerateForm12Link = By.XPath("//div[text()='Generate Form 12']/parent::button");
        private readonly By acquisitionFileGenerateLetterLink = By.XPath("//div[text()='Generate Letter']/parent::button");
        private readonly By acquisitionFileGenerateH0443Link = By.XPath("//div[text()='Conditions of Entry (H0443)']/parent::button");
        private readonly By acquisitionFileGenerateNoticeEntryLink = By.XPath("//div[text()='Generate Notice of Entry']/parent::button");
        private readonly By acquisitionFileGenerateIntakeLink = By.XPath("//div[text()='Generate Intake']/parent::button");

        //Acquisition File Details View Form Elements
        private readonly By acquisitionFileTitle = By.CssSelector("div[data-testid='form-title']");
        private readonly By acquisitionFileHeaderCodeLabel = By.XPath("//label[contains(text(), 'File:')]");
        private readonly By acquisitionFileHeaderCodeContent = By.XPath("//label[contains(text(), 'File:')]/parent::div/following-sibling::div[1]");

        private readonly By acquisitionFileHeaderProjectLabel = By.XPath("//label[contains(text(), 'Ministry project')]");
        private readonly By acquisitionFileHeaderProjectContent = By.XPath("//label[contains(text(), 'Ministry project')]/parent::div/following-sibling::div[1]");
        private readonly By acquisitionFileHeaderProductLabel = By.XPath("//label[contains(text(), 'Ministry product')]");
        private readonly By acquisitionFileHeaderProductContent = By.XPath("//label[contains(text(), 'Ministry product')]/parent::div/following-sibling::div[1]");
        private readonly By acquisitionFileHeaderCreatedDateLabel = By.XPath("//strong[contains(text(), 'Created')]");
        private readonly By acquisitionFileHeaderCreatedDateContent = By.XPath("//strong[contains(text(), 'Created')]/parent::span");
        private readonly By acquisitionFileHeaderCreatedByContent = By.XPath("//strong[contains(text(),'Created')]/parent::span/span[@id='userNameTooltip']/strong");
        private readonly By acquisitionFileHeaderLastUpdateLabel = By.XPath("//strong[contains(text(), 'Updated')]");
        private readonly By acquisitionFileHeaderLastUpdateContent = By.XPath("//strong[contains(text(), 'Updated')]/parent::span");
        private readonly By acquisitionFileHeaderLastUpdateByContent = By.XPath("//strong[contains(text(), 'Updated')]/parent::span/span[@id='userNameTooltip']/strong");
        private readonly By acquisitionFileHeaderHistoricalFileLabel = By.XPath("//label[contains(text(),'Historical file')]");
        private readonly By acquisitionFileHeaderHistoricalFileContent = By.XPath("//label[contains(text(),'Historical file #:')]/parent::div/following-sibling::div/div/span");
        private readonly By acquisitionHeaderStatusContent = By.XPath("//b[contains(text(),'File')]/parent::span/following-sibling::div");

        private readonly By acquisitionFileProjectSubtitle = By.XPath("//h2/div/div[normalize-space(text())='Project']");
        private readonly By acquisitionFileProjectCreateLabel = By.XPath("//label[contains(text(),'Ministry project')]");
        private readonly By acquisitionFileProjectLabel = By.XPath("//div[@class='collapse show']/div/div/label[contains(text(),'Ministry project')]");
        private readonly By acquisitionFileProjectContent = By.XPath("//div[@class='collapse show']/div/div/label[contains(text(),'Ministry project')]/parent::div/following-sibling::div");
        private readonly By acquisitionFileProjectProductLabel = By.XPath("//label[contains(text(),'Product')]");
        private readonly By acquisitionFileProjectProductContent = By.XPath("//label[contains(text(),'Product')]/parent::div/following-sibling::div");
        private readonly By acquisitionFileProjectFundingLabel = By.XPath("//label[contains(text(),'Funding')]");
        private readonly By acquisitionFileProjectFundingInput = By.Id("input-fundingTypeCode");
        private readonly By acquisitionFileProjectFundingContent = By.XPath("//label[contains(text(),'Funding')]/parent::div/following-sibling::div");
        private readonly By acquisitionFileProjectOtherFundingLabel = By.XPath("//label[contains(text(),'Other funding')]");
        private readonly By acquisitionFileProjectOtherFundingContent = By.XPath("//label[contains(text(),'Other funding')]/parent::div/following-sibling::div");

        private readonly By acquisitionFileStatusesSubtitle = By.XPath("//div[contains(text(),'Progress Statuses')]/parent::div/parent::h2");
        private readonly By acquisitionFileStatusesFileProgressLabel = By.XPath("//label[contains(text(),'File progress(es)')]");
        private readonly By acquisitionFileStatusesFileAppraisalLabel = By.XPath("//label[contains(text(),'Appraisal')]");
        private readonly By acquisitionFileStatusesFileAppraisalContent = By.XPath("//label[contains(text(),'Appraisal')]/parent::div/following-sibling::div");
        private readonly By acquisitionFileStatusesFileLegalSurveyLabel = By.XPath("//label[contains(text(),'Legal survey')]");
        private readonly By acquisitionFileStatusesFileLegalSurveyContent = By.XPath("//label[contains(text(),'Legal survey')]/parent::div/following-sibling::div");
        private readonly By acquisitionFileStatusesFileTypeTakingLabel = By.XPath("//label[contains(text(),'Type of taking')]");
        private readonly By acquisitionFileStatusesFileExpropriationRiskLabel = By.XPath("//label[contains(text(),'Expropriation risk')]");
        private readonly By acquisitionFileStatusesFileExpropriationRiskContent = By.XPath("//label[contains(text(),'Expropriation risk')]/parent::div/following-sibling::div");

        private readonly By acquisitionFileScheduleSubtitle = By.XPath("//div[contains(text(),'Schedule')]");
        private readonly By acquisitionFileScheduleAssignedDateLabel = By.XPath("//label[contains(text(),'Assigned date')]");
        private readonly By acquisitionFileScheduleAssignedDateContent = By.XPath("//label[contains(text(),'Assigned date')]/parent::div/following-sibling::div");
        private readonly By acquisitionFileScheduleDeliveryDateLabel = By.XPath("//label[contains(text(),'Delivery date')]");
        private readonly By acquisitionFileScheduleDeliveryDateContent = By.XPath("//label[contains(text(),'Delivery date')]/parent::div/following-sibling::div");
        private readonly By acquisitionFileScheduleEstimatedDateLabel = By.XPath("//label[contains(text(),'Estimated date')]");
        private readonly By acquisitionFileScheduleEstimatedDateContent = By.XPath("//label[contains(text(),'Estimated date')]/parent::div/following-sibling::div");
        private readonly By acquisitionFileSchedulePossesionDateLabel = By.XPath("//label[contains(text(),'Possession date')]");
        private readonly By acquisitionFileSchedulePossesionDateContent = By.XPath("//label[contains(text(),'Possession date')]/parent::div/following-sibling::div");

        private readonly By acquisitionFileDetailsSubtitle = By.XPath("//div[contains(text(),'Acquisition Details')]");
        private readonly By acquisitionFileDetailsNameLabel = By.XPath("//label[contains(text(),'Acquisition file name')]");
        private readonly By acquisitionFileDetailsNameContent = By.XPath("//label[contains(text(),'Acquisition file name')]/parent::div/following-sibling::div");
        private readonly By acquisitionFileHistoricalNumberLabel = By.XPath("//label[contains(text(),'Historical file number')]");
        private readonly By acquisitionFileHistoricalNumberTooltip = By.XPath("//label[contains(text(),'Historical file number')]/span/span[@data-testid='tooltip-icon-section-field-tooltip']");
        private readonly By acquisitionFileDetailsPhysicalFileLabel = By.XPath("//label[contains(text(),'Physical file status')]");
        private readonly By acquisitionFileDetailsPhysicalFileContent = By.XPath("//label[contains(text(),'Physical file status')]/parent::div/following-sibling::div");
        private readonly By acquisitionFileDetailsPhysicalFileDetailsLabel = By.XPath("//label[contains(text(),'Physical file details')]");
        private readonly By acquisitionFileDetailsPhysicalFileDetailsContent = By.XPath("//label[contains(text(),'Physical file details')]/parent::div/following-sibling::div");
        private readonly By acquisitionFileDetailsTypeLabel = By.XPath("//label[contains(text(),'Acquisition type')]");
        private readonly By acquisitionFileDetailsTypeContent = By.XPath("//label[contains(text(),'Acquisition type')]/parent::div/following-sibling::div");
        private readonly By acquisitionFileDetailsSubfileInterestLabel = By.XPath("//label[contains(text(),'Sub-file interest')]");
        private readonly By acquisitionFileDetailsSubfileInterestContent = By.XPath("//label[contains(text(),'Sub-file interest')]/parent::div/following-sibling::div");
        private readonly By acquisitionFileDetailsMOTIRegionLabel = By.XPath("//label[contains(text(),'Ministry region')]");
        private readonly By acquisitionFileDetailsMOTIRegionContent = By.XPath("//label[contains(text(),'Ministry region')]/parent::div/following-sibling::div");

        private readonly By acquisitionFileTeamSubtitle = By.XPath("//div[contains(text(),'Acquisition Team')]");
        private readonly By acquisitionFileAddAnotherMemberLink = By.CssSelector("button[data-testid='add-team-member']");

        private readonly By acquisitionFileOwnerSubtitle = By.XPath("//div[contains(text(),'Owner Information')]");
        private readonly By acquisitionFileSubinterestSubtitle = By.XPath("//div[contains(text(),'Sub-Interest Information')]");
        private readonly By acquisitionFileOwnerRepresentativeLabel = By.XPath("//label[contains(text(),'Owner representative')]");

        private readonly By acquisitionFileOwnerCommentLabel = By.XPath("//div[text()='Owners']/parent::div/parent::h2/following-sibling::div/div/div/label[text()='Comment']");
        private readonly By acquistionFileOwnerViewCommentLabel = By.XPath("//div[text()='Owner Information']/parent::div/parent::h2/following-sibling::div/div/div/label[text()='Comment']");

        //Acquisition File Main Form Input Elements
        private readonly By acquisitionFileEditButton = By.CssSelector("button[title='Edit acquisition file']");
        private readonly By acquisitionFileStatusSelect = By.Id("input-fileStatusTypeCode");
        private readonly By acquisitionFileProjectInput = By.CssSelector("input[id='typeahead-project']");
        private readonly By acquisitionFileProjectDelete = By.CssSelector("div[data-testid='typeahead-project'] button[aria-label='Clear']");
        private readonly By acquisitionFileProject1stOption = By.CssSelector("div[id='typeahead-project'] a");
        private readonly By acquisitionFileProjectProductSelect = By.Id("input-product");
        private readonly By acquisitionFileProjectOtherFundingInput = By.Id("input-fundingTypeOtherDescription");

        private readonly By acquisitionFileStatusesFileProgressCreateLabel = By.XPath("//label[contains(text(),'File progress')]");
        private readonly By acquisitionFileStatusesFileProgressSelect = By.Id("multiselect-progressStatuses_input");
        private readonly By acquisitionFileStatusesDeleteBttns = By.CssSelector("div[id='multiselect-progressStatuses'] i[class='custom-close']");
        private readonly By acquisitionFileStatusesOptions = By.XPath("//input[@id='multiselect-progressStatuses_input']/parent::div/following-sibling::div[contains(@class,'optionListContainer')]");
        private readonly By acquisitionFileStatusesAppraisalSelect = By.Id("input-appraisalStatusType");
        private readonly By acquisitionFileStatusesLegalSurveySelect = By.Id("input-legalSurveyStatusType");
        private readonly By acquisitionFileStatusesTypeTakingSelect = By.Id("multiselect-takingStatuses_input");
        private readonly By acquisitionFileTypeTakingDeleteBttns = By.CssSelector("div[id='multiselect-takingStatuses'] i[class='custom-close']");
        private readonly By acquisitionFileTypeTakingOptions = By.XPath("//input[@id='multiselect-takingStatuses_input']/parent::div/following-sibling::div[contains(@class,'optionListContainer')]");
        private readonly By acquisitionFileStatusesExpropriationRiskSelect = By.Id("input-expropiationRiskStatusType");

        private readonly By acquisitionFileAssignedDateInput = By.Id("datepicker-assignedDate");
        private readonly By acquisitionFileDeliveryDateInput = By.Id("datepicker-deliveryDate");
        private readonly By acquisitionFileEstimatedDateInput = By.Id("datepicker-estimatedCompletionDate");
        private readonly By acquisitionFilePossesionDateInput = By.Id("datepicker-possessionDate");

        private readonly By acquisitionFileNameInput = By.CssSelector("input[id='input-fileName']");
        private readonly By acquisitionFileNameInvalidMessage = By.XPath("//div[contains(text(),'Acquisition file name must be at most 500 characters')]");
        private readonly By acquisitionFileHistoricalNumberInput = By.Id("input-legacyFileNumber");
        private readonly By acquisitionFileHistoricalInvalidMessage = By.XPath("//div[contains(text(),'Legacy file number must be at most 18 characters')]");
        private readonly By acquisitionFilePhysicalStatusSelect = By.Id("input-acquisitionPhysFileStatusType");
        private readonly By acquisitionFilePhysicalDetailsInput = By.Id("input-physicalFileDetails");
        private readonly By acquisitionFileDetailsTypeSelect = By.Id("input-acquisitionType");
        private readonly By acquisitionFileDetailsSubfileInterestSelect = By.Id("input-subfileInterestTypeCode");
        private readonly By acquisitionSubfileInterestOtherInput = By.Id("input-otherSubfileInterestType");
        private readonly By acquisitionFileDetailsRegionSelect = By.Id("input-region");

        private readonly By acquisitionFileTeamMembersGroup = By.XPath("//div[contains(text(),'Acquisition Team')]/parent::div/parent::h2/following-sibling::div/div[@class='py-3 row']");

        private readonly By acquisitionFileCreateOwnerSubtitle = By.XPath("//div[contains(text(),'Owners')]");
        private readonly By acquisitionFileOwnerInfo = By.XPath("//p[contains(text(),'Each property in this file should be owned by the owner(s) in this section')]");
        private readonly By acquisitionFileAddOwnerLink = By.CssSelector("button[data-testid='add-file-owner']");
        private readonly By acquisitionFileOwnersGroup = By.XPath("//div[contains(text(),'Owners')]/parent::div/parent::h2/following-sibling::div/div[@class='py-3 row']");
        private readonly By acquisitionFileDeleteFirstOwnerBttn = By.XPath("//div[contains(text(),'Owners')]/parent::div/parent::h2/following-sibling::div/div[@class='py-3 row'][1]/div/div/button");
        private readonly By acquisitionFileOwnerSolicitorLabel = By.XPath("//label[contains(text(),'Owner solicitor')]");
        private readonly By acquisitionFileOwnerSolicitorButton = By.XPath("//label[contains(text(),'Owner solicitor')]/parent::div/following-sibling::div/div/div/div/button[@title='Select Contact']");
        private readonly By acquisitionFileOwnerSolicitorContent = By.XPath("//label[contains(text(),'Owner solicitor')]/parent::div/following-sibling::div/a/span");
        private readonly By acquisitionFileOwnerRepresentativeButton = By.XPath("//label[contains(text(),'Owner representative')]/parent::div/following-sibling::div/div/div/div/button[@title='Select Contact']");
        private readonly By acquisitionFileOwnerRepresentativeContent = By.XPath("//label[contains(text(),'Owner representative')]/parent::div/following-sibling::div/a/span");
        private readonly By acquisitionFileOwnerCommentTextArea = By.Id("input-ownerRepresentative.comment");
        private readonly By acquisitionFileUpdateOwnerCommentTextArea = By.Id("input-ownerRepresentatives.0.comment");
        private readonly By acquisitionFileOwnerCommentContent = By.XPath("//label[contains(text(),'Comment')]/parent::div/following-sibling::div");
        private readonly By acquisitionFileMainFormDiv = By.XPath("//h1[contains(text(),'Create Acquisition File')]/parent::div/parent::div/parent::div/parent::div");

        private readonly By acquisitionFileNoticeClaimSubtitle = By.XPath("//h2/div/div[contains(text(),'Notice of Claim')]");
        private readonly By acquisitionFileNoticeClaimReceivedDateLabel = By.XPath("//label[contains(text(),'Received date')]");
        private readonly By acquisitionFileNoticeClaimReceivedDateInput = By.Id("datepicker-noticeOfClaim.receivedDate");
        private readonly By acquisitionFileNoticeClaimReceivedDateContent = By.XPath("//label[contains(text(),'Received date')]/parent::div/following-sibling::div");
        private readonly By acquisitionFileNoticeClaimCommentsLabel = By.XPath("//label[contains(text(),'Comment')]");
        private readonly By acquisitionFileNoticeClaimCommentsInput = By.Id("input-noticeOfClaim.comment");
        private readonly By acquisitionFileNoticeClaimCommentsContent = By.XPath("//div[text()='Notice of Claim']/parent::div/parent::h2/following-sibling::div/div/div/label[text()='Comment']/parent::div/following-sibling::div");

        //Acquisition Sub-files View Elements
        private readonly By acquisitionSubfileCreateTableLinkedFilesCode = By.CssSelector("div[data-testid='linked-files-header']");
        private readonly By acquisitionSubfileCreateButton = By.XPath("//div[contains(text(),'Add Sub-interest File')]/parent::button");
        private readonly By acquisitionSubfileCreateInsterestSubtitle = By.XPath("//div[contains(text(),'Sub-Interest')]");
        private readonly By acquisitionSubfileInterestInfo = By.XPath("//p[contains(text(),'Each property in this sub-file should be impacted by the sub-interest(s) in this section')]");
        private readonly By acquisitionSubfileOwnerSolicitorLabel = By.XPath("//label[contains(text(),'Sub-interest solicitor')]");
        private readonly By acquisitionSubfileOwnerSolicitorButton = By.XPath("//label[contains(text(),'Sub-interest solicitor')]/parent::div/following-sibling::div/div/div/div/button[@title='Select Contact']");
        private readonly By acquisitionSubfileOwnerSolicitorContent = By.XPath("//label[contains(text(),'Sub-interest solicitor')]/parent::div/following-sibling::div/a/span");
        private readonly By acquisitionSubfileOwnerRepresentativeLabel = By.XPath("//label[contains(text(),'Sub-interest representative')]");
        private readonly By acquisitionSubfileOwnerRepresentativeButton = By.XPath("//label[contains(text(),'Sub-interest representative')]/parent::div/following-sibling::div/div/div/div/button[@title='Select Contact']");
        private readonly By acquisitionSubfileOwnerRepresentativeContent = By.XPath("//label[contains(text(),'Sub-interest representative')]/parent::div/following-sibling::div/a/span");

        //Acquisition File Confirmation Modal Elements
        private readonly By acquisitionFileConfirmationModal = By.CssSelector("div[class='modal-content']");

        private SharedModals sharedModals;
        private SharedTeamMembers sharedTeams;
        private SharedFileProperties sharedSearchProperties;
        private SharedSelectContact sharedSelectContact;

        public AcquisitionDetails(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
            sharedTeams = new SharedTeamMembers(webDriver);
            sharedSearchProperties = new SharedFileProperties(webDriver);
            sharedSelectContact = new SharedSelectContact(webDriver);
        }

        public void NavigateToCreateNewAcquisitionFile()
        {
            WaitUntilClickable(menuAcquisitionButton);
            FocusAndClick(menuAcquisitionButton);

            WaitUntilClickable(createAcquisitionFileButton);
            FocusAndClick(createAcquisitionFileButton);
        }

        public void NavigateToFileSummary()
        {
            WaitUntilClickable(acquisitionFileSummaryBttn);
            if (webDriver.FindElements(acquisitionFileSummaryBttn).Count() > 0)
                SafeClick(acquisitionFileSummaryBttn);
        }

        public void NavigateToFileDetailsTab()
        {
            WaitUntilClickable(acquisitionFileDetailsTab);
            webDriver.FindElement(acquisitionFileDetailsTab).Click();
        }

        public void NavigateToSubfilesTab()
        {
            WaitUntilClickable(acquisitionSubfilesTab);
            webDriver.FindElement(acquisitionSubfilesTab).Click();
        }

        public void CreateMinimumAcquisitionFile(AcquisitionFile acquisition)
        {
            WaitUntilVisible(acquisitionFileNameInput);

            webDriver.FindElement(acquisitionFileNameInput).SendKeys(acquisition.AcquisitionFileName);
            webDriver.FindElement(acquisitionFileDetailsTypeSelect);
            ChooseSelectOption(acquisitionFileDetailsTypeSelect, acquisition.AcquisitionType);
            ChooseSelectOption(acquisitionFileDetailsRegionSelect, acquisition.AcquisitionMOTIRegion);
        }

        public void EditAcquisitionFileBttn()
        {
            WaitUntilSpinnerDisappear();
            webDriver.FindElement(acquisitionFileEditButton).Click();
        }

        public void AddAcquisitionSubfileBttn()
        {
            WaitUntilSpinnerDisappear();
            webDriver.FindElement(acquisitionSubfileCreateButton).Click();
        }

        public void UpdateAcquisitionFile(AcquisitionFile acquisition, string acquisitionType)
        {
            //STATUS
            if (acquisition.AcquisitionStatus != "" && acquisitionType == "Main")
            {
                WaitUntilClickable(acquisitionFileStatusSelect);
                ChooseSelectOption(acquisitionFileStatusSelect, acquisition.AcquisitionStatus);
            }

            //PROJECT
            if (acquisition.AcquisitionProject != "" && acquisitionType == "Main")
            {
                WaitUntilVisible(acquisitionFileProjectInput);

                if (webDriver.FindElements(acquisitionFileProjectDelete).Count >= 1)
                    webDriver.FindElement(acquisitionFileProjectDelete).Click();

                webDriver.FindElement(acquisitionFileProjectInput).SendKeys(acquisition.AcquisitionProject);
                webDriver.FindElement(acquisitionFileProjectInput).SendKeys(Keys.Space);
                webDriver.FindElement(acquisitionFileProjectInput).SendKeys(Keys.Backspace);

                Wait();
                FocusAndClick(acquisitionFileProject1stOption);
            }

            if (acquisition.AcquisitionProjProduct != "" && acquisitionType == "Main")
            {
                WaitUntilClickable(acquisitionFileProjectProductSelect);
                webDriver.FindElement(acquisitionFileProjectProductSelect).Click();

                WaitUntilClickable(acquisitionFileProjectProductSelect);
                ChooseSelectOption(acquisitionFileProjectProductSelect, acquisition.AcquisitionProjProductCode + " " + acquisition.AcquisitionProjProduct);
            }

            if (acquisition.AcquisitionProjFunding != "")
            {
                WaitUntilClickable(acquisitionFileProjectFundingInput);
                ChooseSelectOption(acquisitionFileProjectFundingInput, acquisition.AcquisitionProjFunding);
            }

            if (webDriver.FindElements(acquisitionFileProjectOtherFundingLabel).Count > 0 && acquisition.AcquisitionFundingOther != "")
            {
                WaitUntilClickable(acquisitionFileProjectOtherFundingInput);
                ClearInput(acquisitionFileProjectOtherFundingInput);
                webDriver.FindElement(acquisitionFileProjectOtherFundingInput).SendKeys(acquisition.AcquisitionFundingOther);
            }

            //PROGRESS STATUTES
            //Delete File Progress statuses previously selected if any
            if (webDriver.FindElements(acquisitionFileStatusesDeleteBttns).Count > 0)
            {
                WaitUntilClickable(acquisitionFileStatusesFileProgressSelect);
                FocusAndClick(acquisitionFileStatusesFileProgressSelect);
                while (webDriver.FindElements(acquisitionFileStatusesDeleteBttns).Count > 0)
                {
                    webDriver.FindElement(acquisitionFileStatusesFileProgressCreateLabel).Click();
                    webDriver.FindElements(acquisitionFileStatusesDeleteBttns)[0].Click();
                }
                webDriver.FindElement(acquisitionFileStatusesFileProgressCreateLabel).Click();
            }

            if (acquisition.AcquisitionFileProgressStatuses.First() != "")
            {
                foreach (string status in acquisition.AcquisitionFileProgressStatuses)
                    ChooseMultiSelectOption(acquisitionFileStatusesFileProgressSelect, acquisitionFileStatusesOptions, acquisitionFileStatusesFileProgressCreateLabel, status);
            }

            if (acquisition.AcquisitionAppraisalStatus != "")
                ChooseSelectOption(acquisitionFileStatusesAppraisalSelect, acquisition.AcquisitionAppraisalStatus);

            if (acquisition.AcquisitionLegalSurveyStatus != "")
                ChooseSelectOption(acquisitionFileStatusesLegalSurveySelect, acquisition.AcquisitionLegalSurveyStatus);

            //Delete Type of Taking statuses previously selected if any
            if (webDriver.FindElements(acquisitionFileTypeTakingDeleteBttns).Count > 0)
            {
                WaitUntilClickable(acquisitionFileStatusesTypeTakingSelect);
                FocusAndClick(acquisitionFileStatusesTypeTakingSelect);
                while (webDriver.FindElements(acquisitionFileTypeTakingDeleteBttns).Count > 0)
                {
                    webDriver.FindElement(acquisitionFileStatusesFileTypeTakingLabel).Click();
                    webDriver.FindElements(acquisitionFileTypeTakingDeleteBttns)[0].Click();
                }
                webDriver.FindElement(acquisitionFileStatusesFileTypeTakingLabel).Click();
            }

            if (acquisition.AcquisitionTypeTakingStatuses.First() != "")
            {
                foreach (string status in acquisition.AcquisitionTypeTakingStatuses) 
                    ChooseMultiSelectOption(acquisitionFileStatusesTypeTakingSelect, acquisitionFileTypeTakingOptions, acquisitionFileStatusesFileTypeTakingLabel, status);
            }

            if (acquisition.AcquisitionExpropriationRiskStatus != "")
                ChooseSelectOption(acquisitionFileStatusesExpropriationRiskSelect, acquisition.AcquisitionExpropriationRiskStatus);

            //SCHEDULE
            if (acquisition.AcquisitionAssignedDate != "")
            {
                WaitUntilClickable(acquisitionFileAssignedDateInput);
                ClearInput(acquisitionFileAssignedDateInput);
                webDriver.FindElement(acquisitionFileAssignedDateInput).SendKeys(acquisition.AcquisitionAssignedDate);
                webDriver.FindElement(acquisitionFileAssignedDateInput).SendKeys(Keys.Enter);
            }

            if (acquisition.AcquisitionDeliveryDate != "")
            {
                WaitUntilClickable(acquisitionFileDeliveryDateInput);
                ClearInput(acquisitionFileDeliveryDateInput);
                webDriver.FindElement(acquisitionFileDeliveryDateInput).SendKeys(acquisition.AcquisitionDeliveryDate);
                webDriver.FindElement(acquisitionFileDeliveryDateInput).SendKeys(Keys.Enter);
            }

            if (acquisition.AcquisitionEstimatedDate != "")
            {
                WaitUntilClickable(acquisitionFileEstimatedDateInput);
                ClearInput(acquisitionFileEstimatedDateInput);
                webDriver.FindElement(acquisitionFileEstimatedDateInput).SendKeys(acquisition.AcquisitionEstimatedDate);
                webDriver.FindElement(acquisitionFileEstimatedDateInput).SendKeys(Keys.Enter);
            }

            if (acquisition.AcquisitionPossesionDate != "")
            {
                WaitUntilClickable(acquisitionFilePossesionDateInput);
                ClearInput(acquisitionFilePossesionDateInput);
                webDriver.FindElement(acquisitionFilePossesionDateInput).SendKeys(acquisition.AcquisitionPossesionDate);
                webDriver.FindElement(acquisitionFilePossesionDateInput).SendKeys(Keys.Enter);
            }

            //DETAILS
            if (acquisition.AcquisitionFileName != "")
            {
                WaitUntilVisible(acquisitionFileNameInput);
                ClearInput(acquisitionFileNameInput);
                webDriver.FindElement(acquisitionFileNameInput).SendKeys(acquisition.AcquisitionFileName);
            }

            if (acquisition.HistoricalFileNumber != "")
            {
                WaitUntilClickable(acquisitionFileHistoricalNumberInput);
                ClearInput(acquisitionFileHistoricalNumberInput);
                webDriver.FindElement(acquisitionFileHistoricalNumberInput).SendKeys(acquisition.HistoricalFileNumber);
            }

            if (acquisition.PhysicalFileStatus != "")
                ChooseSelectOption(acquisitionFilePhysicalStatusSelect, acquisition.PhysicalFileStatus);

            if (acquisition.PhysicalFileDetails != "")
            {
                ClearInput(acquisitionFilePhysicalDetailsInput);
                webDriver.FindElement(acquisitionFilePhysicalDetailsInput).SendKeys(acquisition.PhysicalFileDetails);
            }

            if (acquisition.AcquisitionType != "")
            {
                WaitUntilClickable(acquisitionFileDetailsTypeSelect);
                ChooseSelectOption(acquisitionFileDetailsTypeSelect, acquisition.AcquisitionType);
            }

            if (acquisition.AcquisitionSubfileInterest != "" && acquisitionType == "Subfile")
                ChooseSelectOption(acquisitionFileDetailsSubfileInterestSelect, acquisition.AcquisitionSubfileInterest);

            if (acquisition.AcquisitionSubfileInterestOther != "" && acquisitionType == "Subfile")
            {
                WaitUntilVisible(acquisitionSubfileInterestOtherInput);
                webDriver.FindElement(acquisitionSubfileInterestOtherInput).SendKeys(acquisition.AcquisitionSubfileInterestOther);
            }

            if (acquisition.AcquisitionMOTIRegion != "")
            {
                WaitUntilClickable(acquisitionFileDetailsRegionSelect);
                ChooseSelectOption(acquisitionFileDetailsRegionSelect, acquisition.AcquisitionMOTIRegion);
            }

            //TEAM
            if (acquisition.AcquisitionTeam!.Count > 0)
            {
                while (webDriver.FindElements(acquisitionFileTeamMembersGroup).Count > 0)
                    sharedTeams.DeleteFirstStaffMember();

                for (var i = 0; i < acquisition.AcquisitionTeam.Count; i++)
                    sharedTeams.AddTeamMembers(acquisition.AcquisitionTeam[i]);

            }

            //OWNERS
            if (acquisition.AcquisitionOwners!.Count > 0)
            {
                while (webDriver.FindElements(acquisitionFileOwnersGroup).Count > 0)
                    DeleteOwner();

                for (var i = 0; i < acquisition.AcquisitionOwners.Count; i++)
                    AddOwners(acquisition.AcquisitionOwners[i], i);
            }

            if (acquisition.OwnerSolicitor != "" && acquisitionType == "Main")
            {
                WaitUntilVisible(acquisitionFileOwnerSolicitorButton);
                webDriver.FindElement(acquisitionFileOwnerSolicitorButton).Click();
                sharedSelectContact.SelectContact(acquisition.OwnerSolicitor, "");
            }

            if (acquisition.OwnerRepresentative != "" && acquisitionType == "Main")
            {
                WaitUntilClickable(acquisitionFileOwnerRepresentativeButton);
                webDriver.FindElement(acquisitionFileOwnerRepresentativeButton).Click();
                sharedSelectContact.SelectContact(acquisition.OwnerRepresentative, "");
            }

            if (acquisition.OwnerSolicitor != "" && acquisitionType == "Subfile")
            {
                WaitUntilClickable(acquisitionSubfileOwnerSolicitorButton);
                webDriver.FindElement(acquisitionSubfileOwnerSolicitorButton).Click();
                sharedSelectContact.SelectContact(acquisition.OwnerSolicitor, "");
            }

            if (acquisition.OwnerRepresentative != "" && acquisitionType == "Subfile")
            {
                WaitUntilVisible(acquisitionSubfileOwnerRepresentativeButton);
                webDriver.FindElement(acquisitionSubfileOwnerRepresentativeButton).Click();
                sharedSelectContact.SelectContact(acquisition.OwnerRepresentative, "");
            }

            if (acquisition.OwnerComment != "")
            {
                Wait();
                if (webDriver.FindElements(acquisitionFileOwnerCommentTextArea).Count > 0)
                {
                    ClearInput(acquisitionFileOwnerCommentTextArea);
                    webDriver.FindElement(acquisitionFileOwnerCommentTextArea).SendKeys(acquisition.OwnerComment);
                }
                else
                {
                    ClearInput(acquisitionFileUpdateOwnerCommentTextArea);
                    webDriver.FindElement(acquisitionFileUpdateOwnerCommentTextArea).SendKeys(acquisition.OwnerComment);
                }
            }
            
            //NOTICE OF CLAIMS
            AssertTrueIsDisplayed(acquisitionFileNoticeClaimSubtitle);

            //Date Received
            AssertTrueIsDisplayed(acquisitionFileNoticeClaimReceivedDateLabel);
            if (acquisition.AcquisitionNOCReceivedDate != "")
            {
                ClearInput(acquisitionFileNoticeClaimReceivedDateInput);
                webDriver.FindElement(acquisitionFileNoticeClaimReceivedDateInput).SendKeys(acquisition.AcquisitionNOCReceivedDate);
            }

            //Comments
            AssertTrueIsDisplayed(acquisitionFileNoticeClaimCommentsLabel);
            if (acquisition.AcquisitionNOCComments != "")
            {
                ClearInput(acquisitionFileNoticeClaimCommentsInput);
                webDriver.FindElement(acquisitionFileNoticeClaimCommentsInput).SendKeys(acquisition.AcquisitionNOCComments);
            }
        }

        public void SaveAcquisitionFileDetails()
        {
            ButtonElement("Save");

            Wait();
            while (webDriver.FindElements(acquisitionFileConfirmationModal).Count() > 0)
            {
                if (sharedModals.ModalContent().Contains("The selected Ministry region is different from that associated to one or more selected properties"))
                {
                    Assert.Equal("Different Ministry region", sharedModals.ModalHeader());
                    Assert.Contains("The selected Ministry region is different from that associated to one or more selected properties", sharedModals.ModalContent());
                    Assert.Contains("Do you want to proceed?", sharedModals.ModalContent());
                    sharedModals.ModalClickOKBttn();
                    Wait();
                }
                else if (sharedModals.ModalContent().Contains("The selected property already exists in the system's inventory."))
                {
                    Assert.Equal("User Override Required", sharedModals.ModalHeader());
                    Assert.Contains("The selected property already exists in the system's inventory. However, the record is missing spatial details.", sharedModals.ModalContent());
                    Assert.Contains("To add the property, the spatial details for this property will need to be updated. The system will attempt to update the property record with spatial information from the current selection.", sharedModals.ModalContent());
                    sharedModals.ModalClickOKBttn();
                    Wait();
                }
                else if (sharedModals.ModalContent().Contains("This change will be reflected on other related entities - generated documents, sub-files, etc."))
                {
                    Assert.Equal("Different Project or Product", sharedModals.ModalHeader());
                    Assert.Contains("This change will be reflected on other related entities - generated documents, sub-files, etc.", sharedModals.ModalContent());
                    Assert.Contains("Do you want to proceed?", sharedModals.ModalContent());
                    sharedModals.ModalClickOKBttn();
                    Wait();
                }
                else if (sharedModals.ModalHeader().Contains("Error"))
                {
                    return;
                }
                else
                {
                    sharedModals.ModalClickOKBttn();
                }
            }
        }

        public void SaveAcquisitionFileDetailsWithExpectedErrors()
        {
            ButtonElement("Save");

            WaitUntilVisible(acquisitionFileConfirmationModal);
            while (webDriver.FindElements(acquisitionFileConfirmationModal).Count() > 0)
            {
                if (sharedModals.ModalHeader().Contains("Error"))
                {
                    break;
                }
            }
        }

        public void CancelAcquisitionFile()
        {
            ButtonElement("Cancel");

            sharedModals.CancelActionModal();
        }

        public string GetAcquisitionFileCode()
        {
            WaitUntilVisible(acquisitionFileHeaderCodeContent);

            var totalFileName = webDriver.FindElement(acquisitionFileHeaderCodeContent).Text;
            return Regex.Match(totalFileName, "^[^ ]+").Value;
        }

        public string GetLinkedFilesCode()
        {
            WaitUntilVisible(acquisitionSubfileCreateTableLinkedFilesCode);
            return webDriver.FindElement(acquisitionSubfileCreateTableLinkedFilesCode).Text;
        }

        public int IsCreateAcquisitionFileFormVisible()
        {
            return webDriver.FindElements(acquisitionFileMainFormDiv).Count();
        }

        public void VerifyAcquisitionFileView(AcquisitionFile acquisition, string acquisitionType)
        {
            WaitUntilVisible(acquisitionFileHeaderCodeLabel);

            //Header
            AssertTrueIsDisplayed(acquisitionFileHeaderCodeLabel);
            AssertTrueContentNotEquals(acquisitionFileHeaderCodeContent, "");
            AssertTrueIsDisplayed(acquisitionFileHeaderProjectLabel);

            if(acquisition.AcquisitionProject != "")
                AssertTrueContentEquals(acquisitionFileHeaderProjectContent, acquisition.AcquisitionProjCode + " - "  + acquisition.AcquisitionProject);

            AssertTrueIsDisplayed(acquisitionFileHeaderProductLabel);
            if (acquisition.AcquisitionProject != "")
                AssertTrueContentEquals(acquisitionFileHeaderProductContent, acquisition.AcquisitionProjProductCode + " - " + acquisition.AcquisitionProjProduct);

            AssertTrueIsDisplayed(acquisitionFileHeaderHistoricalFileLabel);
            AssertTrueIsDisplayed(acquisitionFileHeaderCreatedDateLabel);
            AssertTrueContentNotEquals(acquisitionFileHeaderCreatedDateContent, "");
            AssertTrueContentNotEquals(acquisitionFileHeaderCreatedByContent, "");
            AssertTrueIsDisplayed(acquisitionFileHeaderLastUpdateLabel);
            AssertTrueContentNotEquals(acquisitionFileHeaderLastUpdateContent, "");
            AssertTrueContentNotEquals(acquisitionFileHeaderLastUpdateByContent, "");  

            if (acquisition.AcquisitionStatus != "")
                AssertTrueContentEquals(acquisitionHeaderStatusContent, GetUppercaseString(acquisition.AcquisitionStatus));

            //Links
            AssertTrueIsDisplayed(acquisitionFileGenerateFormTitle);
            AssertTrueIsDisplayed(acquisitionFileGenerateForm12Link);
            AssertTrueIsDisplayed(acquisitionFileGenerateLetterLink);
            AssertTrueIsDisplayed(acquisitionFileGenerateH0443Link);
            AssertTrueIsDisplayed(acquisitionFileGenerateNoticeEntryLink);
            AssertTrueIsDisplayed(acquisitionFileGenerateIntakeLink);

            //MAIN FORM
            //Project
            AssertTrueIsDisplayed(acquisitionFileProjectSubtitle);
            AssertTrueIsDisplayed(acquisitionFileProjectLabel);

            if (acquisition.AcquisitionProject != "")
                AssertTrueContentEquals(acquisitionFileProjectContent, acquisition.AcquisitionProjCode + " - " + acquisition.AcquisitionProject);

            AssertTrueIsDisplayed(acquisitionFileProjectProductLabel);

            if(acquisition.AcquisitionProjProduct != "")
                AssertTrueContentEquals(acquisitionFileProjectProductContent, acquisition.AcquisitionProjProductCode + " " +acquisition.AcquisitionProjProduct);

            AssertTrueIsDisplayed(acquisitionFileProjectFundingLabel);

            if(acquisition.AcquisitionProjFunding != "")
                AssertTrueContentEquals(acquisitionFileProjectFundingContent, acquisition.AcquisitionProjFunding);

            if (webDriver.FindElements(acquisitionFileProjectOtherFundingLabel).Count > 0 && acquisition.AcquisitionFundingOther != "")
                AssertTrueContentEquals(acquisitionFileProjectOtherFundingContent, acquisition.AcquisitionFundingOther);

            //Acquisition File Statuses
            AssertTrueIsDisplayed(acquisitionFileStatusesSubtitle);
            AssertTrueIsDisplayed(acquisitionFileStatusesFileProgressLabel);

            if (acquisition.AcquisitionFileProgressStatuses.First() != "")
            {
                for (var i = 0; i < acquisition.AcquisitionFileProgressStatuses.Count; i++)
                    AssertTrueContentEquals(By.XPath("//div[@data-testid='prg-file-progress-status']//span[" + (i + 1) + "]"), acquisition.AcquisitionFileProgressStatuses[i]);
            }

            AssertTrueIsDisplayed(acquisitionFileStatusesFileAppraisalLabel);
            AssertTrueContentEquals(acquisitionFileStatusesFileAppraisalContent, acquisition.AcquisitionAppraisalStatus);

            AssertTrueIsDisplayed(acquisitionFileStatusesFileLegalSurveyLabel);
            AssertTrueContentEquals(acquisitionFileStatusesFileLegalSurveyContent, acquisition.AcquisitionLegalSurveyStatus);

            AssertTrueIsDisplayed(acquisitionFileStatusesFileTypeTakingLabel);
            if (acquisition.AcquisitionTypeTakingStatuses.First() != "")
            {
                for (var i = 0; i < acquisition.AcquisitionTypeTakingStatuses.Count; i++)
                    AssertTrueContentEquals(By.XPath("//div[@data-testid='prg-taking-type-status']//span[" + (i + 1) + "]"), acquisition.AcquisitionTypeTakingStatuses[i]);
            }

            AssertTrueIsDisplayed(acquisitionFileStatusesFileExpropriationRiskLabel);
            AssertTrueContentEquals(acquisitionFileStatusesFileExpropriationRiskContent, acquisition.AcquisitionExpropriationRiskStatus);

            //Schedule
            AssertTrueIsDisplayed(acquisitionFileScheduleSubtitle);
            AssertTrueIsDisplayed(acquisitionFileScheduleAssignedDateLabel);

            if (acquisition.AcquisitionAssignedDate != "")
                AssertTrueContentEquals(acquisitionFileScheduleAssignedDateContent, TransformDateFormat(acquisition.AcquisitionAssignedDate));
            else
                AssertTrueContentEquals(acquisitionFileScheduleAssignedDateContent, DateTime.Now.ToString("MMM d, yyyy"));
            
            AssertTrueIsDisplayed(acquisitionFileScheduleDeliveryDateLabel);
            if(acquisition.AcquisitionDeliveryDate != "")
                AssertTrueContentEquals(acquisitionFileScheduleDeliveryDateContent, TransformDateFormat(acquisition.AcquisitionDeliveryDate));

            AssertTrueIsDisplayed(acquisitionFileScheduleEstimatedDateLabel);
            AssertTrueContentEquals(acquisitionFileScheduleEstimatedDateContent, TransformDateFormat(acquisition.AcquisitionEstimatedDate));

            AssertTrueIsDisplayed(acquisitionFileSchedulePossesionDateLabel);
            AssertTrueContentEquals(acquisitionFileSchedulePossesionDateContent, TransformDateFormat(acquisition.AcquisitionPossesionDate));

            //Details
            AssertTrueIsDisplayed(acquisitionFileDetailsSubtitle);
            AssertTrueIsDisplayed(acquisitionFileDetailsNameLabel);

            if(acquisition.AcquisitionFileName != "")
                AssertTrueContentEquals(acquisitionFileDetailsNameContent, acquisition.AcquisitionFileName);

            AssertTrueIsDisplayed(acquisitionFileDetailsPhysicalFileLabel);

            if(acquisition.PhysicalFileStatus != "")
                AssertTrueContentEquals(acquisitionFileDetailsPhysicalFileContent, acquisition.PhysicalFileStatus);

            AssertTrueIsDisplayed(acquisitionFileDetailsPhysicalFileDetailsLabel);

            if (acquisition.PhysicalFileDetails != "")
                AssertTrueContentEquals(acquisitionFileDetailsPhysicalFileDetailsContent, acquisition.PhysicalFileDetails);

            AssertTrueIsDisplayed(acquisitionFileDetailsTypeLabel);

            if(acquisition.AcquisitionType != "")
                AssertTrueContentEquals(acquisitionFileDetailsTypeContent, acquisition.AcquisitionType);

            if (acquisitionType == "Subfile")
            {
                AssertTrueIsDisplayed(acquisitionFileDetailsSubfileInterestLabel);
                AssertTrueContentEquals(acquisitionFileDetailsSubfileInterestContent, acquisition.AcquisitionSubfileInterest);
            }

            AssertTrueIsDisplayed(acquisitionFileDetailsMOTIRegionLabel);
            AssertTrueContentNotEquals(acquisitionFileDetailsMOTIRegionContent, "");

            //Team members
            AssertTrueIsDisplayed(acquisitionFileTeamSubtitle);

            if (acquisition.AcquisitionTeam!.Count > 0)
                sharedTeams.VerifyTeamMembersViewForm(acquisition.AcquisitionTeam);

            //Owners
            if (acquisitionType == "Main")
                AssertTrueIsDisplayed(acquisitionFileOwnerSubtitle);
            else
                AssertTrueIsDisplayed(acquisitionFileSubinterestSubtitle);

            if (acquisition.AcquisitionOwners!.Count > 0)
            {
                for (var i = 0; i < acquisition.AcquisitionOwners.Count; i++)
                {

                    if (acquisition.AcquisitionOwners[i].OwnerContactType.Equals("Individual"))
                    {
                        AssertTrueElementContains(By.XPath("//span[@data-testid='owner["+ i +"]']/div[2]/div[2]"), acquisition.AcquisitionOwners[i].OwnerGivenNames);
                        AssertTrueElementContains(By.XPath("//span[@data-testid='owner["+ i +"]']/div[2]/div[2]"), acquisition.AcquisitionOwners[i].OwnerLastName);
                        AssertTrueContentEquals(By.XPath("//span[@data-testid='owner["+ i +"]']/div[3]/div[2]"), acquisition.AcquisitionOwners[i].OwnerOtherName);
                    }
                    else
                    {
                        AssertTrueElementContains(By.XPath("//span[@data-testid='owner["+ i +"]']/div[2]/div[2]"), acquisition.AcquisitionOwners[i].OwnerCorporationName);

                        if (acquisition.AcquisitionOwners[i].OwnerCorporationName != "")
                            AssertTrueElementContains(By.XPath("//span[@data-testid='owner["+ i +"]']/div[2]/div[2]"), acquisition.AcquisitionOwners[i].OwnerCorporationName);
                        if (acquisition.AcquisitionOwners[i].OwnerRegistrationNumber != "")
                            AssertTrueElementContains(By.XPath("//span[@data-testid='owner["+ i +"]']/div[2]/div[2]"), acquisition.AcquisitionOwners[i].OwnerRegistrationNumber);

                        AssertTrueContentEquals(By.XPath("//span[@data-testid='owner["+ i +"]']/div[3]/div[2]"), acquisition.AcquisitionOwners[i].OwnerOtherName);
                    }

                    if (acquisition.AcquisitionOwners[i].OwnerMailAddress.AddressLine1 != "")
                        AssertTrueElementContains(By.XPath("//span[@data-testid='owner["+ i +"]']/div[4]/div[2]"), acquisition.AcquisitionOwners[i].OwnerMailAddress.AddressLine1);
                    if (acquisition.AcquisitionOwners[i].OwnerMailAddress.AddressLine2 != "")
                        AssertTrueElementContains(By.XPath("//span[@data-testid='owner["+ i +"]']/div[4]/div[2]"), acquisition.AcquisitionOwners[i].OwnerMailAddress.AddressLine2);
                    if (acquisition.AcquisitionOwners[i].OwnerMailAddress.AddressLine3 != "")
                        AssertTrueElementContains(By.XPath("//span[@data-testid='owner["+ i +"]']/div[4]/div[2]"), acquisition.AcquisitionOwners[i].OwnerMailAddress.AddressLine3);
                    if (acquisition.AcquisitionOwners[i].OwnerMailAddress.Country != "")
                        AssertTrueElementContains(By.XPath("//span[@data-testid='owner["+ i +"]']/div[4]/div[2]"), acquisition.AcquisitionOwners[i].OwnerMailAddress.Country);
                    if (acquisition.AcquisitionOwners[i].OwnerMailAddress.City != "")
                        AssertTrueElementContains(By.XPath("//span[@data-testid='owner["+ i +"]']/div[4]/div[2]"), acquisition.AcquisitionOwners[i].OwnerMailAddress.City);
                    if (acquisition.AcquisitionOwners[i].OwnerMailAddress.Province != "")
                        AssertTrueElementContains(By.XPath("//span[@data-testid='owner["+ i +"]']/div[4]/div[2]"), acquisition.AcquisitionOwners[i].OwnerMailAddress.Province);
                    if (acquisition.AcquisitionOwners[i].OwnerMailAddress.OtherCountry != "")
                        AssertTrueElementContains(By.XPath("//span[@data-testid='owner["+ i +"]']/div[4]/div[2]"), "Other - " + acquisition.AcquisitionOwners[i].OwnerMailAddress.OtherCountry);
                    if (acquisition.AcquisitionOwners[i].OwnerMailAddress.PostalCode != "")
                        AssertTrueElementContains(By.XPath("//span[@data-testid='owner["+ i +"]']/div[4]/div[2]"), acquisition.AcquisitionOwners[i].OwnerMailAddress.PostalCode);
                }
            }

            if (acquisition.OwnerSolicitor != "" && acquisitionType == "Main")
                AssertTrueContentEquals(acquisitionFileOwnerSolicitorContent, acquisition.OwnerSolicitor);
            
            if (acquisition.OwnerRepresentative != "" && acquisitionType == "Main")
                AssertTrueContentEquals(acquisitionFileOwnerRepresentativeContent, acquisition.OwnerRepresentative);

            if (acquisition.OwnerSolicitor != "" && acquisitionType == "Subfile")
                AssertTrueContentEquals(acquisitionSubfileOwnerSolicitorContent, acquisition.OwnerSolicitor);
            

            if (acquisition.OwnerRepresentative != "" && acquisitionType == "Subfile")
                AssertTrueContentEquals(acquisitionSubfileOwnerRepresentativeContent, acquisition.OwnerRepresentative);

            if (acquisition.OwnerComment != "")
            {
                AssertTrueIsDisplayed(acquistionFileOwnerViewCommentLabel);
                AssertTrueContentEquals(acquisitionFileOwnerCommentContent, acquisition.OwnerComment);
            }

            //NOTICE OF CLAIMS
            AssertTrueIsDisplayed(acquisitionFileNoticeClaimSubtitle);

            //NOC Received Date
            AssertTrueIsDisplayed(acquisitionFileNoticeClaimReceivedDateLabel);
            if (acquisition.AcquisitionNOCReceivedDate != "")
                AssertTrueContentEquals(acquisitionFileNoticeClaimReceivedDateContent, TransformDateFormat(acquisition.AcquisitionNOCReceivedDate));

            //NOC Comments
            AssertTrueIsDisplayed(acquisitionFileNoticeClaimCommentsLabel);
            if (acquisition.AcquisitionNOCReceivedDate != "")
                AssertTrueContentEquals(acquisitionFileNoticeClaimCommentsContent, acquisition.AcquisitionNOCComments);
        }

        public void VerifyAcquisitionFileCreate(string acquisitionType)
        {
            WaitUntilVisible(acquisitionFileProjectFundingInput);

            //if (acquisitionType == "Main")
            //    AssertTrueIsDisplayed(acquisitionFileTitle);
            //else
            //    AssertTrueContentEquals(acquisitionFileTitle, "Create Acquisition Sub-Interest File");

            //Project
            AssertTrueIsDisplayed(acquisitionFileProjectSubtitle);
            AssertTrueIsDisplayed(acquisitionFileProjectCreateLabel);
            if (acquisitionType == "Main")
                AssertTrueIsDisplayed(acquisitionFileProjectInput);
             
            AssertTrueIsDisplayed(acquisitionFileProjectFundingLabel);
            AssertTrueIsDisplayed(acquisitionFileProjectFundingInput);

            //Schedule
            AssertTrueIsDisplayed(acquisitionFileScheduleSubtitle);
            AssertTrueIsDisplayed(acquisitionFileScheduleAssignedDateLabel);
            AssertTrueIsDisplayed(acquisitionFileAssignedDateInput);
            AssertTrueIsDisplayed(acquisitionFileScheduleDeliveryDateLabel);
            AssertTrueIsDisplayed(acquisitionFileDeliveryDateInput);

            //Search Property Component
            sharedSearchProperties.VerifyLocateOnMapFeature(acquisitionType);

            //Details
            AssertTrueIsDisplayed(acquisitionFileDetailsSubtitle);
            AssertTrueIsDisplayed(acquisitionFileDetailsNameLabel);
            AssertTrueIsDisplayed(acquisitionFileNameInput);
            AssertTrueIsDisplayed(acquisitionFileDetailsPhysicalFileLabel);
            AssertTrueIsDisplayed(acquisitionFilePhysicalStatusSelect);
            AssertTrueIsDisplayed(acquisitionFileHistoricalNumberLabel);
            AssertTrueIsDisplayed(acquisitionFileHistoricalNumberTooltip);
            AssertTrueIsDisplayed(acquisitionFileHistoricalNumberInput);
            AssertTrueIsDisplayed(acquisitionFileDetailsTypeLabel);
            AssertTrueIsDisplayed(acquisitionFileDetailsTypeSelect);
            AssertTrueIsDisplayed(acquisitionFileDetailsMOTIRegionLabel);

            if (acquisitionType == "Subfile")
            {
                AssertTrueIsDisplayed(acquisitionFileDetailsSubfileInterestLabel);
                AssertTrueIsDisplayed(acquisitionFileDetailsSubfileInterestSelect);
            }
            AssertTrueIsDisplayed(acquisitionFileDetailsRegionSelect);

            //Team members
            AssertTrueIsDisplayed(acquisitionFileTeamSubtitle);
            AssertTrueIsDisplayed(acquisitionFileAddAnotherMemberLink);

            if (acquisitionType == "Main")
            {
                sharedTeams.VerifyRequiredTeamMemberMessages();
                sharedTeams.DeleteFirstStaffMember();
            }

            //Owners
            if (acquisitionType == "Main")
            {
                AssertTrueIsDisplayed(acquisitionFileCreateOwnerSubtitle);
                AssertTrueIsDisplayed(acquisitionFileOwnerInfo);
                AssertTrueIsDisplayed(acquisitionFileAddOwnerLink);
                AssertTrueIsDisplayed(acquisitionFileOwnerSolicitorLabel);
                AssertTrueIsDisplayed(acquisitionFileOwnerSolicitorButton);
                AssertTrueIsDisplayed(acquisitionFileOwnerRepresentativeLabel);
                AssertTrueIsDisplayed(acquisitionFileOwnerRepresentativeButton);
            }
            else
            {
                AssertTrueIsDisplayed(acquisitionSubfileCreateInsterestSubtitle);
                AssertTrueIsDisplayed(acquisitionSubfileInterestInfo);
                AssertTrueIsDisplayed(acquisitionFileAddOwnerLink);
                AssertTrueIsDisplayed(acquisitionSubfileOwnerSolicitorLabel);
                AssertTrueIsDisplayed(acquisitionSubfileOwnerSolicitorButton);
                AssertTrueIsDisplayed(acquisitionSubfileOwnerRepresentativeLabel);
                AssertTrueIsDisplayed(acquisitionSubfileOwnerRepresentativeButton);
            }
            AssertTrueIsDisplayed(acquisitionFileOwnerCommentLabel);
            AssertTrueIsDisplayed(acquisitionFileOwnerCommentTextArea);

            //Notice of Claims
            AssertTrueIsDisplayed(acquisitionFileNoticeClaimSubtitle);
            AssertTrueIsDisplayed(acquisitionFileNoticeClaimReceivedDateLabel);
            AssertTrueIsDisplayed(acquisitionFileNoticeClaimReceivedDateInput);
            AssertTrueIsDisplayed(acquisitionFileNoticeClaimCommentsLabel);
            AssertTrueIsDisplayed(acquisitionFileNoticeClaimCommentsInput);
        }

        public void VerifyMaximumFields()
        {
            //Get previous inserted data
            var acquisitionFileName = webDriver.FindElement(acquisitionFileNameInput).GetDomProperty("value");

            //Verify File Name Input
            webDriver.FindElement(acquisitionFileNameInput).SendKeys("Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibus");
            webDriver.FindElement(acquisitionFileDetailsNameLabel).Click();
            AssertTrueIsDisplayed(acquisitionFileNameInvalidMessage);
            ClearInput(acquisitionFileNameInput);

            //Verify Historical File Number Input
            webDriver.FindElement(acquisitionFileHistoricalNumberInput).SendKeys("Lorem ipsum dolor s");
            webDriver.FindElement(acquisitionFileHistoricalNumberLabel).Click();
            AssertTrueIsDisplayed(acquisitionFileHistoricalInvalidMessage);
            ClearInput(acquisitionFileHistoricalNumberInput);

            //Re-insert acquisition file name
            webDriver.FindElement(acquisitionFileNameInput).SendKeys(acquisitionFileName);
        }

        public void VerifyErrorMessageDraftItems()
        {
            WaitUntilVisible(acquisitionFileConfirmationModal);
            Assert.Contains("You cannot complete a file when there are one or more draft agreements, or one or more draft compensations requisitions.", sharedModals.ModalContent());
            Assert.Contains("Remove any draft compensations requisitions. Agreements should be set to final, cancelled, or removed.", sharedModals.ModalContent());
        }

        public void VerifyErrorCannotCompleteWithoutTakes()
        {
            WaitUntilVisible(acquisitionFileConfirmationModal);
            Assert.Equal("You cannot complete an acquisition file that has no takes.", sharedModals.ModalContent());
        }

        public void VerifyErrorCannotCompleteInProgressTakes()
        {
            WaitUntilVisible(acquisitionFileConfirmationModal);
            Assert.Equal("Please ensure all in-progress property takes have been completed or canceled before completing an Acquisition File.", sharedModals.ModalContent());
        }

        private void AddOwners(AcquisitionOwner owner, int ownerIndex)
        {
            WaitUntilClickable(acquisitionFileAddOwnerLink);
            SafeClick(acquisitionFileAddOwnerLink);

            if (owner.OwnerContactType.Equals("Individual"))
            {
                SafeClick(By.CssSelector("input[data-testid='radio-owners["+ ownerIndex +"].isorganization-individual']"));

                if (owner.OwnerGivenNames != "")
                    webDriver.FindElement(By.Id("input-owners["+ ownerIndex +"].givenName")).SendKeys(owner.OwnerGivenNames);
                if (owner.OwnerLastName != "")
                    webDriver.FindElement(By.Id("input-owners["+ ownerIndex +"].lastNameAndCorpName")).SendKeys(owner.OwnerLastName);
                if (owner.OwnerOtherName != "")
                    webDriver.FindElement(By.Id("input-owners["+ ownerIndex +"].otherName")).SendKeys(owner.OwnerOtherName);
            }
            else
            {
                SafeClick(By.CssSelector("input[data-testid='radio-owners["+ ownerIndex +"].isorganization-corporation']"));

                if (owner.OwnerCorporationName != "")
                    webDriver.FindElement(By.Id("input-owners["+ ownerIndex +"].lastNameAndCorpName")).SendKeys(owner.OwnerCorporationName);
                if (owner.OwnerOtherName != "")
                    webDriver.FindElement(By.Id("input-owners["+ ownerIndex +"].otherName")).SendKeys(owner.OwnerOtherName);
                if (owner.OwnerIncorporationNumber != "")
                    webDriver.FindElement(By.Id("input-owners["+ ownerIndex +"].incorporationNumber")).SendKeys(owner.OwnerIncorporationNumber);
                if (owner.OwnerRegistrationNumber != "")
                    webDriver.FindElement(By.Id("input-owners["+ ownerIndex +"].registrationNumber")).SendKeys(owner.OwnerRegistrationNumber);
            }   

            if(owner.OwnerIsPrimary)
                FocusAndClick(By.CssSelector("input[data-testid='radio-owners["+ ownerIndex +"].isprimarycontact-primary contact']"));

            if(owner.OwnerMailAddress.AddressLine1 != "")
                webDriver.FindElement(By.Id("input-owners["+ ownerIndex +"].address.streetAddress1")).SendKeys(owner.OwnerMailAddress.AddressLine1);
            if (owner.OwnerMailAddress.AddressLine2 != "")
            {
                webDriver.FindElement(By.XPath("//input[@id='input-owners["+ ownerIndex +"].address.streetAddress1']/parent::div/parent::div/parent::div/parent::div/parent::div /following-sibling::div/div/div/div/button")).Click();
                webDriver.FindElement(By.Id("input-owners["+ ownerIndex +"].address.streetAddress2")).SendKeys(owner.OwnerMailAddress.AddressLine2);
            }
            if (owner.OwnerMailAddress.AddressLine3 != "")
            {
                webDriver.FindElement(By.XPath("//input[@id='input-owners["+ ownerIndex +"].address.streetAddress2']/parent::div/parent::div/parent::div/parent::div/parent::div /following-sibling::div/div/div/div/button")).Click();
                webDriver.FindElement(By.Id("input-owners["+ ownerIndex +"].address.streetAddress3")).SendKeys(owner.OwnerMailAddress.AddressLine3);
            }
            if (owner.OwnerMailAddress.Country != "")
                ChooseSelectOption(By.Id("input-owners["+ ownerIndex +"].address.countryId"), owner.OwnerMailAddress.Country);
            if (owner.OwnerMailAddress.City != "")
                webDriver.FindElement(By.Id("input-owners["+ ownerIndex +"].address.municipality")).SendKeys(owner.OwnerMailAddress.City);
            if (owner.OwnerMailAddress.Province != "")
                webDriver.FindElement(By.Id("input-owners["+ ownerIndex +"].address.provinceId")).SendKeys(owner.OwnerMailAddress.Province);
            if (owner.OwnerMailAddress.OtherCountry != "")
                webDriver.FindElement(By.Id("input-owners["+ ownerIndex +"].address.countryOther")).SendKeys(owner.OwnerMailAddress.OtherCountry);
            if (owner.OwnerMailAddress.PostalCode != "")
                webDriver.FindElement(By.Id("input-owners["+ ownerIndex +"].address.postal")).SendKeys(owner.OwnerMailAddress.PostalCode);

            if (owner.OwnerEmail != "")
                webDriver.FindElement(By.Id("input-owners["+ ownerIndex +"].contactEmailAddress")).SendKeys(owner.OwnerEmail);
            if (owner.OwnerPhone != "")
                webDriver.FindElement(By.Id("input-owners["+ ownerIndex +"].contactPhoneNumber")).SendKeys(owner.OwnerPhone);

        }

        private void DeleteOwner()
        {
            WaitUntilClickable(acquisitionFileDeleteFirstOwnerBttn);
            webDriver.FindElement(acquisitionFileDeleteFirstOwnerBttn).Click();

            WaitUntilVisible(acquisitionFileConfirmationModal);
            Assert.True(sharedModals.ModalHeader() == "Remove Owner");
            Assert.True(sharedModals.ModalContent() == "Are you sure you want to remove this Owner?");

            sharedModals.ModalClickOKBttn();
        }
    }
}
