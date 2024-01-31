using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Support.UI;
using System.Text.RegularExpressions;
using PIMS.Tests.Automation.Classes;
using System;

namespace PIMS.Tests.Automation.PageObjects
{
    public class AcquisitionDetails : PageObjectBase
    {
        //Acquisition Files Menu Elements
        private By menuAcquisitionButton = By.CssSelector("div[data-testid='nav-tooltip-acquisition'] a");
        private By createAcquisitionFileButton = By.XPath("//a[contains(text(),'Create an Acquisition File')]");

        private By acquisitionFileSummaryBttn = By.XPath("//div[contains(text(),'File Summary')]");
        private By acquisitionFileDetailsTab = By.XPath("//a[contains(text(),'File details')]");

        //Acquisition File Details View Form Elements
        private By acquisitionFileViewTitle = By.XPath("//h1[contains(text(),'Acquisition File')]");
        
        private By acquisitionFileCreateTitle = By.XPath("//h1[contains(text(),'Create Acquisition File')]");
        private By acquisitionFileHeaderCodeLabel = By.XPath("//label[contains(text(), 'File:')]");
        private By acquisitionFileHeaderCodeContent = By.XPath("//label[contains(text(), 'File:')]/parent::div/following-sibling::div[1]/strong");

        private By acquisitionFileHeaderProjectLabel = By.XPath("//label[contains(text(), 'Ministry project')]");
        private By acquisitionFileHeaderProjectContent = By.XPath("//label[contains(text(), 'Ministry project')]/parent::div/following-sibling::div[1]/strong");
        private By acquisitionFileHeaderCreatedDateLabel = By.XPath("//span[contains(text(), 'Created')]");
        private By acquisitionFileHeaderCreatedDateContent = By.XPath("//span[contains(text(), 'Created')]/strong");
        private By acquisitionFileHeaderCreatedByContent = By.XPath("//span[contains(text(),'Created')]/span[@id='userNameTooltip']/strong");
        private By acquisitionFileHeaderLastUpdateLabel = By.XPath("//span[contains(text(), 'Last updated')]");
        private By acquisitionFileHeaderLastUpdateContent = By.XPath("//span[contains(text(), 'Last updated')]/strong");
        private By acquisitionFileHeaderLastUpdateByContent = By.XPath("//span[contains(text(), 'Last updated')]//span[@id='userNameTooltip']/strong");
        private By acquisitionFileHeaderStatusLabel = By.XPath("//label[contains(text(),'Status')]");
        private By acquisitionFileStatusSelect = By.Id("input-fileStatusTypeCode");
        private By acquisitionFileHeaderStatusContent = By.XPath("//label[contains(text(),'Status')]/parent::div/following-sibling::div[1]/strong");

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
            WaitUntilClickable(acquisitionFileSummaryBttn);
            webDriver.FindElement(acquisitionFileSummaryBttn).Click();
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

        public void AddAdditionalInformation(AcquisitionFile acquisition)
        {
            if (acquisition.AcquisitionStatus != "")
            {
                WaitUntilClickable(acquisitionFileStatusSelect);
                ChooseSpecificSelectOption(acquisitionFileStatusSelect, acquisition.AcquisitionStatus);
            }

            if (acquisition.AcquisitionProject != "")
            {
                WaitUntilVisible(acquisitionFileProjectInput);

                webDriver.FindElement(acquisitionFileProjectInput).SendKeys(acquisition.AcquisitionProject);

                Wait();
                webDriver.FindElement(acquisitionFileProjectInput).SendKeys(Keys.Space);

                Wait();
                webDriver.FindElement(acquisitionFileProjectInput).SendKeys(Keys.Backspace);

                Wait();
                FocusAndClick(acquisitionFileProject1stOption); 
            }

            if (acquisition.AcquisitionProjProduct != "")
            {
                WaitUntilVisible(acquisitionFileProjectProductSelect);
                webDriver.FindElement(acquisitionFileProjectProductSelect).Click();

                ChooseSpecificSelectOption(acquisitionFileProjectProductSelect, acquisition.AcquisitionProjProductCode + " " + acquisition.AcquisitionProjProduct);
            }
                
            if(acquisition.AcquisitionProjFunding != "")
                ChooseSpecificSelectOption(acquisitionFileProjectFundingInput, acquisition.AcquisitionProjFunding);

            if (webDriver.FindElements(acquisitionFileProjectOtherFundingLabel).Count > 0)
            {
                webDriver.FindElement(acquisitionFileProjectOtherFundingInput).SendKeys(acquisition.AcquisitionFundingOther);
            }

            if (acquisition.DeliveryDate != "")
            {
                webDriver.FindElement(acquisitionFileDeliveryDateInput).SendKeys(acquisition.DeliveryDate);
                webDriver.FindElement(acquisitionFileDeliveryDateInput).SendKeys(Keys.Enter);
            }

            if (acquisition.AcquisitionCompletedDate != "")
            {
                webDriver.FindElement(acquisitionFileCompletedDateInput).SendKeys(acquisition.AcquisitionCompletedDate);
                webDriver.FindElement(acquisitionFileCompletedDateInput).SendKeys(Keys.Enter);
            }

            if (acquisition.HistoricalFileNumber != "")
                webDriver.FindElement(acquisitionFileHistoricalNumberInput).SendKeys(acquisition.HistoricalFileNumber);
           
            if(acquisition.PhysicalFileStatus != "")
                ChooseSpecificSelectOption(acquisitionFilePhysicalStatusSelect, acquisition.PhysicalFileStatus);

            if (acquisition.AcquisitionTeam.Count > 0)
            {
                for (var i = 0; i < acquisition.AcquisitionTeam.Count; i++)
                {
                    AddTeamMembers(acquisition.AcquisitionTeam[i]);
                }
            }

            if (acquisition.AcquisitionOwners.Count > 0)
            {
                for (var i = 0; i < acquisition.AcquisitionOwners.Count; i++)
                {
                    AddOwners(acquisition.AcquisitionOwners[i], i);
                }
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
                webDriver.FindElement(acquisitionFileOwnerCommentTextArea).Click();
                webDriver.FindElement(acquisitionFileOwnerCommentTextArea).SendKeys(acquisition.OwnerComment);
            }
        }

        public void UpdateAcquisitionFile(AcquisitionFile acquisition)
        {
            if (acquisition.AcquisitionStatus != "")
            {
                WaitUntilClickable(acquisitionFileStatusSelect);
                ChooseSpecificSelectOption(acquisitionFileStatusSelect, acquisition.AcquisitionStatus);
            }

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

            if (acquisition.AcquisitionFileName != "")
            {
                WaitUntilVisible(acquisitionFileNameInput);
                ClearInput(acquisitionFileNameInput);
                webDriver.FindElement(acquisitionFileNameInput).SendKeys(acquisition.AcquisitionFileName);
            }

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

            if (acquisition.AcquisitionCompletedDate != "")
            {
                WaitUntilClickable(acquisitionFileCompletedDateInput);
                ClearInput(acquisitionFileCompletedDateInput);
                webDriver.FindElement(acquisitionFileCompletedDateInput).SendKeys(acquisition.AcquisitionCompletedDate);
                webDriver.FindElement(acquisitionFileCompletedDateInput).SendKeys(Keys.Enter);
            }

            if (acquisition.HistoricalFileNumber != "")
            {
                WaitUntilClickable(acquisitionFileHistoricalNumberInput);
                ClearInput(acquisitionFileHistoricalNumberInput);
                webDriver.FindElement(acquisitionFileHistoricalNumberInput).SendKeys(acquisition.HistoricalFileNumber);
            }

            if (acquisition.PhysicalFileStatus != "") 
                ChooseSpecificSelectOption(acquisitionFilePhysicalStatusSelect, acquisition.PhysicalFileStatus);

            if (acquisition.AcquisitionTeam.Count > 0)
            {
                while (webDriver.FindElements(acquisitionFileTeamMembersGroup).Count > 0)
                    DeleteFirstStaffMember();

                for (var i = 0; i < acquisition.AcquisitionTeam.Count; i++)
                {
                    AddTeamMembers(acquisition.AcquisitionTeam[i]);
                }
            }

            if (acquisition.AcquisitionOwners.Count > 0)
            {
                while (webDriver.FindElements(acquisitionFileOwnersGroup).Count > 0)
                    DeleteOwner();

                for (var i = 0; i < acquisition.AcquisitionOwners.Count; i++)
                {
                    AddOwners(acquisition.AcquisitionOwners[i], i);
                }
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
                Assert.Equal("User Override Required", sharedModals.ModalHeader());

                if (sharedModals.ModalContent().Contains("The selected Ministry region is different from that associated to one or more selected properties"))
                {
                    Assert.Contains("The selected Ministry region is different from that associated to one or more selected properties", sharedModals.ModalContent());
                    Assert.Contains("Do you want to proceed?", sharedModals.ModalContent());
                }
                else
                {
                    Assert.Contains("The selected property already exists in the system's inventory. However, the record is missing spatial details.", sharedModals.ModalContent());
                    Assert.Contains("To add the property, the spatial details for this property will need to be updated. The system will attempt to update the property record with spatial information from the current selection.", sharedModals.ModalContent());
                }

                sharedModals.ModalClickOKBttn();
            }

            AssertTrueIsDisplayed(acquisitionFileEditButton);
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

            try
            {
                WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(3));
                if (wait.Until(ExpectedConditions.AlertIsPresent()) != null)
                {
                    webDriver.SwitchTo().Alert().Accept();
                }
            }
            catch (WebDriverTimeoutException e)
            {
                if (webDriver.FindElements(acquisitionFileConfirmationModal).Count() > 0)
                {
                    Assert.Equal("Unsaved Changes", sharedModals.ModalHeader());
                    Assert.Equal("You have made changes on this form. Do you wish to leave without saving?", sharedModals.ModalContent());
                    sharedModals.ModalClickOKBttn();
                }
            }
        }

        public string GetAcquisitionFileCode()
        {
            WaitUntilVisible(acquisitionFileHeaderCodeContent);

            var totalFileName = webDriver.FindElement(acquisitionFileHeaderCodeContent).Text;
            return Regex.Match(totalFileName, "^[^ ]+").Value;
        }

        public int IsCreateAcquisitionFileFormVisible()
        {
            return webDriver.FindElements(acquisitionFileMainFormDiv).Count();
        }

        public void VerifyAcquisitionFileView(AcquisitionFile acquisition)
        {
            AssertTrueIsDisplayed(acquisitionFileViewTitle);

            //Header
            AssertTrueIsDisplayed(acquisitionFileHeaderCodeLabel);
            AssertTrueContentNotEquals(acquisitionFileHeaderCodeContent, "");
            AssertTrueIsDisplayed(acquisitionFileHeaderProjectLabel);

            if(acquisition.AcquisitionProject != "")
                AssertTrueContentEquals(acquisitionFileHeaderProjectContent, acquisition.AcquisitionProjCode + " - "  + acquisition.AcquisitionProject);

            AssertTrueIsDisplayed(acquisitionFileHeaderCreatedDateLabel);
            AssertTrueContentNotEquals(acquisitionFileHeaderCreatedDateContent, "");
            AssertTrueContentNotEquals(acquisitionFileHeaderCreatedByContent, "");
            AssertTrueIsDisplayed(acquisitionFileHeaderLastUpdateLabel);
            AssertTrueContentNotEquals(acquisitionFileHeaderLastUpdateContent, "");
            AssertTrueContentNotEquals(acquisitionFileHeaderLastUpdateByContent, "");
            AssertTrueIsDisplayed(acquisitionFileHeaderStatusLabel);

            //Status
            if (acquisition.AcquisitionStatus != "")
                AssertTrueContentEquals(acquisitionFileHeaderStatusContent, acquisition.AcquisitionStatus);

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
            //AssertTrueContentEquals(acquisitionFileScheduleAssignedDateContent, DateTime.Now.ToString("MMM d, yyyy"));

            AssertTrueIsDisplayed(acquisitionFileScheduleDeliveryDateLabel);

            if(acquisition.DeliveryDate != "")
                AssertTrueContentEquals(acquisitionFileScheduleDeliveryDateContent, TransformDateFormat(acquisition.DeliveryDate));

            if (acquisition.AcquisitionCompletedDate != "")
                AssertTrueContentEquals(acquisitionFileScheduleCompletedDateContent, TransformDateFormat(acquisition.AcquisitionCompletedDate));

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

            if (acquisition.AcquisitionTeam.Count > 0)
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

            if (acquisition.AcquisitionOwners.Count > 0)
            {
                for (var i = 0; i < acquisition.AcquisitionOwners.Count; i++)
                {

                    if (acquisition.AcquisitionOwners[i].ContactType.Equals("Individual"))
                    {
                        AssertTrueElementContains(By.XPath("//span[@data-testid='owner["+ i +"]']/div[2]/div[2]"), acquisition.AcquisitionOwners[i].GivenNames);
                        AssertTrueElementContains(By.XPath("//span[@data-testid='owner["+ i +"]']/div[2]/div[2]"), acquisition.AcquisitionOwners[i].LastName);
                        AssertTrueContentEquals(By.XPath("//span[@data-testid='owner["+ i +"]']/div[3]/div[2]"), acquisition.AcquisitionOwners[i].OtherName);
                    }
                    else
                    {
                        AssertTrueElementContains(By.XPath("//span[@data-testid='owner["+ i +"]']/div[2]/div[2]"), acquisition.AcquisitionOwners[i].CorporationName);

                        if (acquisition.AcquisitionOwners[i].CorporationName != "")
                            AssertTrueElementContains(By.XPath("//span[@data-testid='owner["+ i +"]']/div[2]/div[2]"), acquisition.AcquisitionOwners[i].CorporationName);
                        if (acquisition.AcquisitionOwners[i].RegistrationNumber != "")
                            AssertTrueElementContains(By.XPath("//span[@data-testid='owner["+ i +"]']/div[2]/div[2]"), acquisition.AcquisitionOwners[i].RegistrationNumber);

                        AssertTrueContentEquals(By.XPath("//span[@data-testid='owner["+ i +"]']/div[3]/div[2]"), acquisition.AcquisitionOwners[i].OtherName);
                    }

                    if (acquisition.AcquisitionOwners[i].MailAddressLine1 != "")
                        AssertTrueElementContains(By.XPath("//span[@data-testid='owner["+ i +"]']/div[4]/div[2]"), acquisition.AcquisitionOwners[i].MailAddressLine1);
                    if (acquisition.AcquisitionOwners[i].MailAddressLine2 != "")
                        AssertTrueElementContains(By.XPath("//span[@data-testid='owner["+ i +"]']/div[4]/div[2]"), acquisition.AcquisitionOwners[i].MailAddressLine2);
                    if (acquisition.AcquisitionOwners[i].MailAddressLine3 != "")
                        AssertTrueElementContains(By.XPath("//span[@data-testid='owner["+ i +"]']/div[4]/div[2]"), acquisition.AcquisitionOwners[i].MailAddressLine3);
                    if (acquisition.AcquisitionOwners[i].MailCountry != "")
                        AssertTrueElementContains(By.XPath("//span[@data-testid='owner["+ i +"]']/div[4]/div[2]"), acquisition.AcquisitionOwners[i].MailCountry);
                    if (acquisition.AcquisitionOwners[i].MailCity != "")
                        AssertTrueElementContains(By.XPath("//span[@data-testid='owner["+ i +"]']/div[4]/div[2]"), acquisition.AcquisitionOwners[i].MailCity);
                    if (acquisition.AcquisitionOwners[i].MailProvince != "")
                        AssertTrueElementContains(By.XPath("//span[@data-testid='owner["+ i +"]']/div[4]/div[2]"), acquisition.AcquisitionOwners[i].MailProvince);
                    //if (acquisition.AcquisitionOwners[i].MailOtherCountry != "")
                        //AssertTrueElementContains(By.XPath("//span[@data-testid='owner["+ i +"]']/div[4]/div[2]"), acquisition.AcquisitionOwners[i].MailOtherCountry);
                    if (acquisition.AcquisitionOwners[i].MailPostalCode != "")
                        AssertTrueElementContains(By.XPath("//span[@data-testid='owner["+ i +"]']/div[4]/div[2]"), acquisition.AcquisitionOwners[i].MailPostalCode);
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

            VerifyMaximumFields();

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
            if (owner.ContactType.Equals("Individual"))
            {
                FocusAndClick(By.CssSelector("input[data-testid='radio-owners["+ ownerIndex +"].isorganization-individual']"));

                if (owner.GivenNames != "")
                    webDriver.FindElement(By.Id("input-owners["+ ownerIndex +"].givenName")).SendKeys(owner.GivenNames);
                if (owner.LastName != "")
                    webDriver.FindElement(By.Id("input-owners["+ ownerIndex +"].lastNameAndCorpName")).SendKeys(owner.LastName);
                if (owner.OtherName != "")
                    webDriver.FindElement(By.Id("input-owners["+ ownerIndex +"].otherName")).SendKeys(owner.OtherName);
            }
            else
            {
                FocusAndClick(By.CssSelector("input[data-testid='radio-owners["+ ownerIndex +"].isorganization-corporation']"));

                if (owner.CorporationName != "")
                    webDriver.FindElement(By.Id("input-owners["+ ownerIndex +"].lastNameAndCorpName")).SendKeys(owner.CorporationName);
                if (owner.OtherName != "")
                    webDriver.FindElement(By.Id("input-owners["+ ownerIndex +"].otherName")).SendKeys(owner.OtherName);
                if (owner.IncorporationNumber != "")
                    webDriver.FindElement(By.Id("input-owners["+ ownerIndex +"].incorporationNumber")).SendKeys(owner.IncorporationNumber);
                if (owner.RegistrationNumber != "")
                    webDriver.FindElement(By.Id("input-owners["+ ownerIndex +"].registrationNumber")).SendKeys(owner.RegistrationNumber);
            }   

            if(owner.isPrimary)
                FocusAndClick(By.CssSelector("input[data-testid='radio-owners["+ ownerIndex +"].isprimarycontact-primary contact']"));

            if(owner.MailAddressLine1 != "")
                webDriver.FindElement(By.Id("input-owners["+ ownerIndex +"].address.streetAddress1")).SendKeys(owner.MailAddressLine1);
            if (owner.MailAddressLine2 != "")
                webDriver.FindElement(By.Id("input-owners["+ ownerIndex +"].address.streetAddress2")).SendKeys(owner.MailAddressLine2);
            if (owner.MailAddressLine3 != "")
                webDriver.FindElement(By.Id("input-owners["+ ownerIndex +"].address.streetAddress3")).SendKeys(owner.MailAddressLine3);
            if (owner.MailCountry != "")
                ChooseSpecificSelectOption(By.Id("input-owners["+ ownerIndex +"].address.countryId"), owner.MailCountry);
            if (owner.MailCity != "")
                webDriver.FindElement(By.Id("input-owners["+ ownerIndex +"].address.municipality")).SendKeys(owner.MailCity);
            if (owner.MailProvince != "")
                webDriver.FindElement(By.Id("input-owners["+ ownerIndex +"].address.provinceId")).SendKeys(owner.MailProvince);
            if (owner.MailOtherCountry != "")
                webDriver.FindElement(By.Id("input-owners["+ ownerIndex +"].address.countryOther")).SendKeys(owner.MailOtherCountry);
            if (owner.MailPostalCode != "")
                webDriver.FindElement(By.Id("input-owners["+ ownerIndex +"].address.postal")).SendKeys(owner.MailPostalCode);

            if (owner.Email != "")
                webDriver.FindElement(By.Id("input-owners["+ ownerIndex +"].contactEmailAddress")).SendKeys(owner.Email);
            if (owner.Phone != "")
                webDriver.FindElement(By.Id("input-owners["+ ownerIndex +"].contactPhoneNumber")).SendKeys(owner.Phone);

        }

        public void DeleteFirstStaffMember()
        {
            WaitUntilClickable(acquisitionFileTeamFirstMemberDeleteBttn);
            webDriver.FindElement(acquisitionFileTeamFirstMemberDeleteBttn).Click();

            WaitUntilVisible(acquisitionFileConfirmationModal);
            Assert.True(sharedModals.ModalHeader() == "Remove Team Member");
            Assert.True(sharedModals.ModalContent() == "Are you sure you want to remove this row?");

            sharedModals.ModalClickOKBttn();
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

        private void VerifyMaximumFields()
        {
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
        }
    }
}
