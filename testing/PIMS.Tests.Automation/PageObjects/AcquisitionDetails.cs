using OpenQA.Selenium;
using System.Text.RegularExpressions;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class AcquisitionDetails : PageObjectBase
    {
        //Acquisition Files Menu Elements
        private readonly By menuAcquisitionButton = By.CssSelector("div[data-testid='nav-tooltip-acquisition'] a");
        private readonly By createAcquisitionFileButton = By.XPath("//a[contains(text(),'Create an Acquisition File')]");

        private readonly By acquisitionFileSummaryBttn = By.XPath("//div[contains(text(),'File Summary')]");
        private readonly By acquisitionFileDetailsTab = By.XPath("//a[contains(text(),'File details')]");

        //Acquisition File Details View Form Elements
        private readonly By acquisitionFileViewTitle = By.XPath("//h1[contains(text(),'Acquisition File')]");
        
        private readonly By acquisitionFileCreateTitle = By.XPath("//h1[contains(text(),'Create Acquisition File')]");
        private readonly By acquisitionFileHeaderCodeLabel = By.XPath("//label[contains(text(), 'File:')]");
        private readonly By acquisitionFileHeaderCodeContent = By.XPath("//label[contains(text(), 'File:')]/parent::div/following-sibling::div[1]");

        private readonly By acquisitionFileHeaderProjectLabel = By.XPath("//label[contains(text(), 'Ministry project')]");
        private readonly By acquisitionFileHeaderProjectContent = By.XPath("//label[contains(text(), 'Ministry project')]/parent::div/following-sibling::div[1]");
        private readonly By acquisitionFileHeaderProductLabel = By.XPath("//label[contains(text(), 'Ministry product')]");
        private readonly By acquisitionFileHeaderProductContent = By.XPath("//label[contains(text(), 'Ministry product')]/parent::div/following-sibling::div[1]");
        private readonly By acquisitionFileHeaderCreatedDateLabel = By.XPath("//strong[contains(text(), 'Created')]");
        private readonly By acquisitionFileHeaderCreatedDateContent = By.XPath("//strong[contains(text(), 'Created')]/parent::span");
        private By acquisitionFileHeaderCreatedByContent = By.XPath("//strong[contains(text(),'Created')]/parent::span/span[@id='userNameTooltip']/strong");
        private By acquisitionFileHeaderLastUpdateLabel = By.XPath("//strong[contains(text(), 'Updated')]");
        private By acquisitionFileHeaderLastUpdateContent = By.XPath("//strong[contains(text(), 'Updated')]/parent::span");
        private By acquisitionFileHeaderLastUpdateByContent = By.XPath("//strong[contains(text(), 'Updated')]/parent::span/span[@id='userNameTooltip']/strong");
        private By acquisitionFileHeaderHistoricalFileLabel = By.XPath("//label[contains(text(),'Historical file')]");
        private By acquisitionFileHeaderHistoricalFileContent = By.XPath("//label[contains(text(),'Historical file #:')]/parent::div/following-sibling::div/div/span");
        private By acquisitionHeaderStatusContent = By.XPath("//b[contains(text(),'File')]/parent::span/following-sibling::div");

        private By acquisitionFileStatusSelect = By.Id("input-fileStatusTypeCode");
        private By acquisitionFileProjectSubtitle = By.XPath("//h2/div/div[contains(text(), 'Project')]");
        private By acquisitionFileProjectLabel = By.XPath("//div[@class='collapse show']/div/div/label[contains(text(),'Ministry project')]");
        private By acquisitionFileProjectInput = By.CssSelector("input[id='typeahead-project']");
        private By acquisitionFileProject1stOption = By.CssSelector("div[id='typeahead-project'] a");
        private By acquisitionFileProjectContent = By.XPath("//div[@class='collapse show']/div/div/label[contains(text(),'Ministry project')]/parent::div/following-sibling::div");
        private By acquisitionFileProjectProductLabel = By.XPath("//label[contains(text(),'Product')]");
        private By acquisitionFileProjectProductSelect = By.Id("input-product");
        private By acquisitionFileProjectProductContent = By.XPath("//label[contains(text(),'Product')]/parent::div/following-sibling::div");
        private By acquisitionFileProjectFundingLabel = By.XPath("//label[contains(text(),'Funding')]");
        private By acquisitionFileProjectFundingInput = By.Id("input-fundingTypeCode");
        private By acquisitionFileProjectFundingContent = By.XPath("//label[contains(text(),'Funding')]/parent::div/following-sibling::div");
        private By acquisitionFileProjectOtherFundingLabel = By.XPath("//label[contains(text(),'Other funding')]");
        private By acquisitionFileProjectOtherFundingInput = By.Id("input-fundingTypeOtherDescription");
        private By acquisitionFileProjectOtherFundingContent = By.XPath("//label[contains(text(),'Other funding')]/parent::div/following-sibling::div");

        private By acquisitionFileScheduleSubtitle = By.XPath("//div[contains(text(),'Schedule')]");
        private By acquisitionFileScheduleAssignedDateLabel = By.XPath("//label[contains(text(),'Assigned date')]");
        private By acquisitionFileAssignedDateInput = By.Id("datepicker-assignedDate");
        private By acquisitionFileScheduleAssignedDateContent = By.XPath("//label[contains(text(),'Assigned date')]/parent::div/following-sibling::div");
        private By acquisitionFileScheduleDeliveryDateLabel = By.XPath("//label[contains(text(),'Delivery date')]");
        private By acquisitionFileScheduleDeliveryDateContent = By.XPath("//label[contains(text(),'Delivery date')]/parent::div/following-sibling::div");

        private By acquisitionFileScheduleCompletedDateContent = By.XPath("//label[contains(text(),'Acquisition completed date')]/parent::div/following-sibling::div");

        private By acquisitionFileDetailsSubtitle = By.XPath("//div[contains(text(),'Acquisition Details')]");
        private By acquisitionFileDetailsNameLabel = By.XPath("//label[contains(text(),'Acquisition file name')]");
        private By acquisitionFileDetailsNameContent = By.XPath("//label[contains(text(),'Acquisition file name')]/parent::div/following-sibling::div");
        private By acquisitionFileDetailsPhysicalFileLabel = By.XPath("//label[contains(text(),'Physical file status')]");
        private By acquisitionFileDetailsPhysicalFileContent = By.XPath("//label[contains(text(),'Physical file status')]/parent::div/following-sibling::div");
        private By acquisitionFileDetailsTypeLabel = By.XPath("//label[contains(text(),'Acquisition type')]");
        private By acquisitionFileDetailsTypeContent = By.XPath("//label[contains(text(),'Acquisition type')]/parent::div/following-sibling::div");
        private By acquisitionFileDetailsMOTIRegionLabel = By.XPath("//label[contains(text(),'Ministry region')]");
        private By acquisitionFileDetailsMOTIRegionContent = By.XPath("//label[contains(text(),'Ministry region')]/parent::div/following-sibling::div");

        private By acquisitionFileTeamSubtitle = By.XPath("//div[contains(text(),'Acquisition Team')]");

        private By acquisitionFileOwnerSubtitle = By.XPath("//div[contains(text(),'Owner Information')]");

        //Acquisition File Main Form Input Elements
        private By acquisitionFileMainFormDiv = By.XPath("//h1[contains(text(),'Create Acquisition File')]/parent::div/parent::div/parent::div/parent::div");
        private By acquisitionFileDeliveryDateInput = By.Id("datepicker-deliveryDate");
        private By acquisitionFileCompletedDateInput = By.Id("datepicker-completionDate");

        private By acquisitionFileNameInput = By.Id("input-fileName");
        private By acquisitionFileNameInvalidMessage = By.XPath("//div[contains(text(),'Acquisition file name must be at most 500 characters')]");

        private By acquisitionFileHistoricalNumberLabel = By.XPath("//label[contains(text(),'Historical file number')]");
        private By acquisitionFileHistoricalNumberInput = By.Id("input-legacyFileNumber");
        private By acquisitionFileHistoricalInvalidMessage = By.XPath("//div[contains(text(),'Legacy file number must be at most 18 characters')]");
        private By acquisitionFileHistoricalNumberTooltip = By.XPath("//label[contains(text(),'Historical file number')]/span/span[@data-testid='tooltip-icon-section-field-tooltip']");

        private By acquisitionFilePhysicalStatusSelect = By.Id("input-acquisitionPhysFileStatusType");
        private By acquisitionFileDetailsTypeSelect = By.Id("input-acquisitionType");
        private By acquisitionFileDetailsRegionSelect = By.Id("input-region");

        private By acquisitionFileAddAnotherMemberLink = By.CssSelector("button[data-testid='add-team-member']");
        private By acquisitionFileTeamMembersGroup = By.XPath("//div[contains(text(),'Acquisition Team')]/parent::div/parent::h2/following-sibling::div/div[@class='py-3 row']");
        private By acquisitionFileViewTeamMembersGroup = By.XPath("//div[contains(text(),'Acquisition Team')]/parent::div/parent::h2/following-sibling::div/div");
        private By acquisitionFileTeamFirstMemberDeleteBttn = By.XPath("//div[contains(text(),'Acquisition Team')]/parent::div/parent::h2/following-sibling::div/div[@class='py-3 row'][1]/div[3]/button");
        private By acquisitionFileTeamInvalidTeamMemberMessage = By.XPath("//div[contains(text(),'Select a team member')]");
        private By acquisitionFileTeamInvalidProfileMessage = By.XPath("//div[contains(text(),'Select a profile')]");

        private By acquisitionFileCreateOwnerSubtitle = By.XPath("//div[contains(text(),'Owners')]");
        private By acquisitionFileOwnerInfo = By.XPath("//p[contains(text(),'Each property in this file should be owned by the owner(s) in this section')]");
        private By acquisitionFileAddOwnerLink = By.CssSelector("button[data-testid='add-file-owner']");
        private By acquisitionFileOwnersGroup = By.XPath("//div[contains(text(),'Owners')]/parent::div/parent::h2/following-sibling::div/div[@class='py-3 row']");
        private By acquisitionFileDeleteFirstOwnerBttn = By.XPath("//div[contains(text(),'Owners')]/parent::div/parent::h2/following-sibling::div/div[@class='py-3 row'][1]/div/div/button");
        
        private By acquisitionFileOwnerSolicitorLabel = By.XPath("//label[contains(text(),'Owner solicitor')]");
        private By acquisitionFileOwnerSolicitorButton = By.XPath("//label[contains(text(),'Owner solicitor')]/parent::div/following-sibling::div/div/div/div/button[@title='Select Contact']");
        private By acquisitionFileOwnerSolicitorContent = By.XPath("//label[contains(text(),'Owner solicitor')]/parent::div/following-sibling::div/a/span");
        private By acquisitionFileOwnerRepresentativeLabel = By.XPath("//label[contains(text(),'Owner representative')]");
        private By acquisitionFileOwnerRepresentativeButton = By.XPath("//label[contains(text(),'Owner representative')]/parent::div/following-sibling::div/div/div/div/button[@title='Select Contact']");
        private By acquisitionFileOwnerRepresentativeContent = By.XPath("//label[contains(text(),'Owner representative')]/parent::div/following-sibling::div/a/span");
        private By acquisitionFileOwnerCommentLabel = By.XPath("//label[contains(text(),'Comment')]");
        private By acquisitionFileOwnerCommentTextArea = By.Id("input-ownerRepresentative.comment");
        private By acquisitionFileOwnerCommentContent = By.XPath("//label[contains(text(),'Comment')]/parent::div/following-sibling::div");

        private By acquisitionFileEditButton = By.CssSelector("button[title='Edit acquisition file']");

        //Acquisition File Confirmation Modal Elements
        private By acquisitionFileConfirmationModal = By.CssSelector("div[class='modal-content']");

        private SharedSelectContact sharedSelectContact;
        private SharedModals sharedModals;
        private SharedFileProperties sharedSearchProperties;

        public AcquisitionDetails(IWebDriver webDriver) : base(webDriver)
        {
            sharedSelectContact = new SharedSelectContact(webDriver);
            sharedModals = new SharedModals(webDriver);
            sharedSearchProperties = new SharedFileProperties(webDriver);
        }

        public void NavigateToCreateNewAcquisitionFile()
        {
            Wait();
            FocusAndClick(menuAcquisitionButton);

            WaitUntilVisible(createAcquisitionFileButton);
            FocusAndClick(createAcquisitionFileButton);
        }

        public void NavigateToFileSummary()
        {
            WaitUntilVisible(acquisitionFileSummaryBttn);
            FocusAndClick(acquisitionFileSummaryBttn);
        }

        public void NavigateToFileDetailsTab()
        {
            WaitUntilClickable(acquisitionFileDetailsTab);
            webDriver.FindElement(acquisitionFileDetailsTab).Click();
        }

        public void CreateMinimumAcquisitionFile(AcquisitionFile acquisition)
        {
            Wait();

            webDriver.FindElement(acquisitionFileNameInput).SendKeys(acquisition.AcquisitionFileName);
            webDriver.FindElement(acquisitionFileDetailsTypeSelect);
            ChooseSpecificSelectOption(acquisitionFileDetailsTypeSelect, acquisition.AcquisitionType);
            ChooseSpecificSelectOption(acquisitionFileDetailsRegionSelect, acquisition.AcquisitionMOTIRegion);
        }

        public void EditAcquisitionFileBttn()
        {
            WaitUntilSpinnerDisappear();
            webDriver.FindElement(acquisitionFileEditButton).Click();
        }

        public void UpdateAcquisitionFile(AcquisitionFile acquisition)
        {
            //Status
            if (acquisition.AcquisitionStatus != "")
            {
                WaitUntilClickable(acquisitionFileStatusSelect);
                ChooseSpecificSelectOption(acquisitionFileStatusSelect, acquisition.AcquisitionStatus);
            }

            //Project
            if (acquisition.AcquisitionProject != "")
            {
                WaitUntilVisible(acquisitionFileProjectInput);

                ClearInput(acquisitionFileProjectInput);
                webDriver.FindElement(acquisitionFileProjectInput).SendKeys(acquisition.AcquisitionProject);
                webDriver.FindElement(acquisitionFileProjectInput).SendKeys(Keys.Space);
                webDriver.FindElement(acquisitionFileProjectInput).SendKeys(Keys.Backspace);

                Wait();
                webDriver.FindElement(acquisitionFileProject1stOption).Click();
            }

            if (acquisition.AcquisitionProjProduct != "")
            {
                WaitUntilClickable(acquisitionFileProjectProductSelect);
                webDriver.FindElement(acquisitionFileProjectProductSelect).Click();
                ChooseSpecificSelectOption(acquisitionFileProjectProductSelect, acquisition.AcquisitionProjProductCode + " " + acquisition.AcquisitionProjProduct);
            }

            if (acquisition.AcquisitionProjFunding != "")
            {
                WaitUntilClickable(acquisitionFileProjectFundingInput);
                ChooseSpecificSelectOption(acquisitionFileProjectFundingInput, acquisition.AcquisitionProjFunding);
            }

            if (webDriver.FindElements(acquisitionFileProjectOtherFundingLabel).Count > 0 && acquisition.AcquisitionFundingOther != "")
            {
                WaitUntilClickable(acquisitionFileProjectOtherFundingInput);
                ClearInput(acquisitionFileProjectOtherFundingInput);
                webDriver.FindElement(acquisitionFileProjectOtherFundingInput).SendKeys(acquisition.AcquisitionFundingOther);
            }

            //Schedule
            if (acquisition.AssignedDate != "")
            {
                WaitUntilClickable(acquisitionFileAssignedDateInput);
                ClearInput(acquisitionFileAssignedDateInput);
                webDriver.FindElement(acquisitionFileAssignedDateInput).SendKeys(acquisition.AssignedDate);
                webDriver.FindElement(acquisitionFileAssignedDateInput).SendKeys(Keys.Enter);
            }

            if (acquisition.DeliveryDate != "")
            {
                WaitUntilClickable(acquisitionFileDeliveryDateInput);
                ClearInput(acquisitionFileDeliveryDateInput);
                webDriver.FindElement(acquisitionFileDeliveryDateInput).SendKeys(acquisition.DeliveryDate);
                webDriver.FindElement(acquisitionFileDeliveryDateInput).SendKeys(Keys.Enter);
            }

            //Details
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
                ChooseSpecificSelectOption(acquisitionFilePhysicalStatusSelect, acquisition.PhysicalFileStatus);
            
            if (acquisition.AcquisitionType != "")
            {
                WaitUntilClickable(acquisitionFileDetailsTypeSelect);
                ChooseSpecificSelectOption(acquisitionFileDetailsTypeSelect, acquisition.AcquisitionType);
            }
 
            if (acquisition.AcquisitionMOTIRegion != "")
            {
                WaitUntilClickable(acquisitionFileDetailsRegionSelect);
                ChooseSpecificSelectOption(acquisitionFileDetailsRegionSelect, acquisition.AcquisitionMOTIRegion);
            }

            //Team
            if (acquisition.AcquisitionTeam!.Count > 0)
            {
                while (webDriver.FindElements(acquisitionFileTeamMembersGroup).Count > 0)
                    DeleteFirstStaffMember();

                for (var i = 0; i < acquisition.AcquisitionTeam.Count; i++)
                    AddTeamMembers(acquisition.AcquisitionTeam[i]);
                
            }

            //Owners
            if (acquisition.AcquisitionOwners!.Count > 0)
            {
                while (webDriver.FindElements(acquisitionFileOwnersGroup).Count > 0)
                    DeleteOwner();

                for (var i = 0; i < acquisition.AcquisitionOwners.Count; i++)
                    AddOwners(acquisition.AcquisitionOwners[i], i);
            }

            if (acquisition.OwnerSolicitor != "")
            {
                WaitUntilVisible(acquisitionFileOwnerSolicitorButton);
                webDriver.FindElement(acquisitionFileOwnerSolicitorButton).Click();
                sharedSelectContact.SelectContact(acquisition.OwnerSolicitor, "");
            }

            if (acquisition.OwnerRepresentative != "")
            {
                WaitUntilVisible(acquisitionFileOwnerRepresentativeButton);
                webDriver.FindElement(acquisitionFileOwnerRepresentativeButton).Click();
                sharedSelectContact.SelectContact(acquisition.OwnerRepresentative, "");
            }

            if (acquisition.OwnerComment != "")
            {
                Wait();
                
                ClearInput(acquisitionFileOwnerCommentTextArea);
                webDriver.FindElement(acquisitionFileOwnerCommentTextArea).SendKeys(acquisition.OwnerComment);
            }
        }

        public void SaveAcquisitionFileDetails()
        {
            Wait();
            ButtonElement("Save");

            Wait();
            if (webDriver.FindElements(acquisitionFileConfirmationModal).Count() > 0)
            {
                if (sharedModals.ModalContent().Contains("The selected Ministry region is different from that associated to one or more selected properties"))
                {
                    Assert.Equal("Different Ministry region", sharedModals.ModalHeader());
                    Assert.Contains("The selected Ministry region is different from that associated to one or more selected properties", sharedModals.ModalContent());
                    Assert.Contains("Do you want to proceed?", sharedModals.ModalContent());
                    sharedModals.ModalClickOKBttn();
                }
                 else if(sharedModals.ModalContent().Contains("The selected property already exists in the system's inventory."))
                {
                    Assert.Equal("User Override Required", sharedModals.ModalHeader());
                    Assert.Contains("The selected property already exists in the system's inventory. However, the record is missing spatial details.", sharedModals.ModalContent());
                    Assert.Contains("To add the property, the spatial details for this property will need to be updated. The system will attempt to update the property record with spatial information from the current selection.", sharedModals.ModalContent());
                    sharedModals.ModalClickOKBttn();
                }
            }
        }

        public void SaveAcquisitionFileDetailsWithExpectedErrors()
        {
            Wait();
            ButtonElement("Save");

            sharedModals.IsToastyPresent();
        }

        public void CancelAcquisitionFile()
        {
            Wait();
            ButtonElement("Cancel");

            sharedModals.CancelActionModal();
        }

        public string GetAcquisitionFileCode()
        {
            Wait();

            var totalFileName = webDriver.FindElement(acquisitionFileHeaderCodeContent).Text;
            return Regex.Match(totalFileName, "^[^ ]+").Value;
        }

        public int IsCreateAcquisitionFileFormVisible()
        {
            return webDriver.FindElements(acquisitionFileMainFormDiv).Count();
        }

        public void VerifyAcquisitionFileView(AcquisitionFile acquisition)
        {
            Wait();
            AssertTrueIsDisplayed(acquisitionFileViewTitle);

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
            //Assert.True(webDriver.FindElements(acquisitionFileHeaderHistoricalFileContent).Count > 0);

            AssertTrueIsDisplayed(acquisitionFileHeaderCreatedDateLabel);
            AssertTrueContentNotEquals(acquisitionFileHeaderCreatedDateContent, "");
            AssertTrueContentNotEquals(acquisitionFileHeaderCreatedByContent, "");
            AssertTrueIsDisplayed(acquisitionFileHeaderLastUpdateLabel);
            AssertTrueContentNotEquals(acquisitionFileHeaderLastUpdateContent, "");
            AssertTrueContentNotEquals(acquisitionFileHeaderLastUpdateByContent, "");  

            if (acquisition.AcquisitionStatus != "")
                AssertTrueContentEquals(acquisitionHeaderStatusContent, GetUppercaseString(acquisition.AcquisitionStatus));

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

            //Schedule
            AssertTrueIsDisplayed(acquisitionFileScheduleSubtitle);
            AssertTrueIsDisplayed(acquisitionFileScheduleAssignedDateLabel);

            if (acquisition.AssignedDate != "")
                AssertTrueContentEquals(acquisitionFileScheduleAssignedDateContent, TransformDateFormat(acquisition.AssignedDate));
            else
            {
                AssertTrueContentEquals(acquisitionFileScheduleAssignedDateContent, DateTime.Now.ToString("MMM d, yyyy"));
            }

            AssertTrueIsDisplayed(acquisitionFileScheduleDeliveryDateLabel);

            if(acquisition.DeliveryDate != "")
                AssertTrueContentEquals(acquisitionFileScheduleDeliveryDateContent, TransformDateFormat(acquisition.DeliveryDate));

            //Details
            AssertTrueIsDisplayed(acquisitionFileDetailsSubtitle);
            AssertTrueIsDisplayed(acquisitionFileDetailsNameLabel);

            if(acquisition.AcquisitionFileName != "")
                AssertTrueContentEquals(acquisitionFileDetailsNameContent, acquisition.AcquisitionFileName);

            AssertTrueIsDisplayed(acquisitionFileDetailsPhysicalFileLabel);

            if(acquisition.PhysicalFileStatus != "")
                AssertTrueContentEquals(acquisitionFileDetailsPhysicalFileContent, acquisition.PhysicalFileStatus);

            AssertTrueIsDisplayed(acquisitionFileDetailsTypeLabel);

            if(acquisition.AcquisitionType != "")
                AssertTrueContentEquals(acquisitionFileDetailsTypeContent, acquisition.AcquisitionType);

            AssertTrueIsDisplayed(acquisitionFileDetailsMOTIRegionLabel);
            AssertTrueContentNotEquals(acquisitionFileDetailsMOTIRegionContent, "");

            //Team members
            AssertTrueIsDisplayed(acquisitionFileTeamSubtitle);

            if (acquisition.AcquisitionTeam!.Count > 0)
            {
                var index = 1;

                for (var i = 0; i < acquisition.AcquisitionTeam.Count; i++)
                {
                     AssertTrueContentEquals(By.XPath("//h2/div/div[contains(text(),'Acquisition Team')]/parent::div/parent::h2/following-sibling::div/div[" + index + "]/div/label"), acquisition.AcquisitionTeam[i].TeamMemberRole + ":");
                     AssertTrueContentEquals(By.XPath("//h2/div/div[contains(text(),'Acquisition Team')]/parent::div/parent::h2/following-sibling::div/div[" + index + "]/div/a"), acquisition.AcquisitionTeam[i].TeamMemberContactName);

                    if (acquisition.AcquisitionTeam[i].TeamMemberPrimaryContact != "")
                    {
                        index ++;
                        AssertTrueContentEquals(By.XPath("//h2/div/div[contains(text(),'Acquisition Team')]/parent::div/parent::h2/following-sibling::div/div[" + index + "]/div/a"), acquisition.AcquisitionTeam[i].TeamMemberPrimaryContact);
                    }

                    index++;
                }
            }

            //Owners
            AssertTrueIsDisplayed(acquisitionFileOwnerSubtitle);

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

            if(acquisition.OwnerSolicitor != "")
                AssertTrueContentEquals(acquisitionFileOwnerSolicitorContent, acquisition.OwnerSolicitor);

            if (acquisition.OwnerRepresentative != "")
                AssertTrueContentEquals(acquisitionFileOwnerRepresentativeContent, acquisition.OwnerRepresentative);

            if (acquisition.OwnerComment != "")
                AssertTrueContentEquals(acquisitionFileOwnerCommentContent, acquisition.OwnerComment);
        }

        public void VerifyAcquisitionFileCreate()
        {
            AssertTrueIsDisplayed(acquisitionFileCreateTitle);

            //Project
            AssertTrueIsDisplayed(acquisitionFileProjectSubtitle);
            AssertTrueIsDisplayed(acquisitionFileProjectLabel);
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
            sharedSearchProperties.VerifyLocateOnMapFeature();

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
            AssertTrueIsDisplayed(acquisitionFileDetailsRegionSelect);

            //Team members
            AssertTrueIsDisplayed(acquisitionFileTeamSubtitle);
            AssertTrueIsDisplayed(acquisitionFileAddAnotherMemberLink);

            VerifyRequiredTeamMemberMessages();
            DeleteFirstStaffMember();

            //Owners
            AssertTrueIsDisplayed(acquisitionFileCreateOwnerSubtitle);
            AssertTrueIsDisplayed(acquisitionFileOwnerInfo);
            AssertTrueIsDisplayed(acquisitionFileAddOwnerLink);
            AssertTrueIsDisplayed(acquisitionFileOwnerSolicitorLabel);
            AssertTrueIsDisplayed(acquisitionFileOwnerSolicitorButton);
            AssertTrueIsDisplayed(acquisitionFileOwnerRepresentativeLabel);
            AssertTrueIsDisplayed(acquisitionFileOwnerRepresentativeButton);
            AssertTrueIsDisplayed(acquisitionFileOwnerCommentLabel);
            AssertTrueIsDisplayed(acquisitionFileOwnerCommentTextArea);
        }

        public void VerifyMaximumFields()
        {
            //Get previous inserted data
            var acquisitionFileName = webDriver.FindElement(acquisitionFileNameInput).GetAttribute("value");

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

        public void DeleteFirstStaffMember()
        {
            WaitUntilClickable(acquisitionFileTeamFirstMemberDeleteBttn);
            webDriver.FindElement(acquisitionFileTeamFirstMemberDeleteBttn).Click();

            WaitUntilVisible(acquisitionFileConfirmationModal);
            Assert.Equal("Remove Team Member", sharedModals.ModalHeader());
            Assert.Equal("Are you sure you want to remove this row?", sharedModals.ModalContent());

            sharedModals.ModalClickOKBttn();
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

        private void AddTeamMembers(TeamMember teamMember)
        {
            WaitUntilClickable(acquisitionFileAddAnotherMemberLink);
            FocusAndClick(acquisitionFileAddAnotherMemberLink);

            Wait();
            var teamMemberIndex = webDriver.FindElements(acquisitionFileTeamMembersGroup).Count() -1;
            //var teamMemberCount = webDriver.FindElements(acquisitionFileTeamMembersGroup).Count();

            WaitUntilVisible(By.CssSelector("select[id='input-team."+ teamMemberIndex +".contactTypeCode']"));
            ChooseSpecificSelectOption(By.CssSelector("select[id='input-team."+ teamMemberIndex +".contactTypeCode']"), teamMember.TeamMemberRole);
            FocusAndClick(By.CssSelector("div[data-testid='teamMemberRow["+ teamMemberIndex +"]'] div[data-testid='contact-input'] button[title='Select Contact']"));
            sharedSelectContact.SelectContact(teamMember.TeamMemberContactName, teamMember.TeamMemberContactType);

            Wait();
            if(webDriver.FindElements(By.Id("input-team."+ teamMemberIndex +".primaryContactId")).Count > 0)
                ChooseSpecificSelectOption(By.Id("input-team."+ teamMemberIndex +".primaryContactId"), teamMember.TeamMemberPrimaryContact);
        }

        private void AddOwners(AcquisitionOwner owner, int ownerIndex)
        {
            WaitUntilClickable(acquisitionFileAddOwnerLink);
            FocusAndClick(acquisitionFileAddOwnerLink);

            Wait();
            if (owner.OwnerContactType.Equals("Individual"))
            {
                FocusAndClick(By.CssSelector("input[data-testid='radio-owners["+ ownerIndex +"].isorganization-individual']"));

                if (owner.OwnerGivenNames != "")
                    webDriver.FindElement(By.Id("input-owners["+ ownerIndex +"].givenName")).SendKeys(owner.OwnerGivenNames);
                if (owner.OwnerLastName != "")
                    webDriver.FindElement(By.Id("input-owners["+ ownerIndex +"].lastNameAndCorpName")).SendKeys(owner.OwnerLastName);
                if (owner.OwnerOtherName != "")
                    webDriver.FindElement(By.Id("input-owners["+ ownerIndex +"].otherName")).SendKeys(owner.OwnerOtherName);
            }
            else
            {
                FocusAndClick(By.CssSelector("input[data-testid='radio-owners["+ ownerIndex +"].isorganization-corporation']"));

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
                ChooseSpecificSelectOption(By.Id("input-owners["+ ownerIndex +"].address.countryId"), owner.OwnerMailAddress.Country);
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

        private void VerifyRequiredTeamMemberMessages()
        {
            //Add a new Team member form
            Wait();
            WaitUntilClickable(acquisitionFileAddAnotherMemberLink);
            webDriver.FindElement(acquisitionFileAddAnotherMemberLink).Click();

            //Verify that invalid team member message is displayed
            ChooseSpecificSelectOption(By.Id("input-team.0.contactTypeCode"), "Expropriation agent");
            AssertTrueIsDisplayed(acquisitionFileTeamInvalidTeamMemberMessage);

            //verify that invalid profile message is displayed
            ChooseSpecificSelectOption(By.Id("input-team.0.contactTypeCode"), "Select profile...");
            webDriver.FindElement(By.CssSelector("div[data-testid='contact-input'] button[title='Select Contact']")).Click();
            sharedSelectContact.SelectContact("Test", "");
            AssertTrueIsDisplayed(acquisitionFileTeamInvalidProfileMessage);
        }
    }
}
