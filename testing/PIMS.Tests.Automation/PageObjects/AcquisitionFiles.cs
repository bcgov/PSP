using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Support.UI;
using System.Text.RegularExpressions;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class AcquisitionFiles : PageObjectBase
    {
        //Acquisition Files Menu Elements
        private By menuAcquisitionButton = By.XPath("//a/label[contains(text(),'Acquisition')]/parent::a");
        private By createAcquisitionFileButton = By.XPath("//a[contains(text(),'Create an Acquisition File')]");

        private By acquisitionFileSummaryBttn = By.XPath("//div[contains(text(),'File Summary')]");

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
        private By acquisitionFileHeaderStatusContent = By.XPath("//label[contains(text(),'Status')]/parent::div/following-sibling::div[1]/strong");

        private By acquisitionFileProjectSubtitle = By.XPath("//div[contains(text(),'Project')]");
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
        private By acquisitionFileProjectOtherFundingContent = By.XPath("//label[contains(text(),'Other Funding')]/parent::div/following-sibling::div");

        private By acquisitionFileScheduleSubtitle = By.XPath("//div[contains(text(),'Schedule')]");
        private By acquisitionFileScheduleAssigneedDateLabel = By.XPath("//label[contains(text(),'Assigned date')]");
        private By acquisitionFileScheduleAssigneedDateContent = By.XPath("//label[contains(text(),'Assigned date')]/parent::div/following-sibling::div");
        private By acquisitionFileScheduleDeliveryDateLabel = By.XPath("//label[contains(text(),'Delivery date')]");
        private By acquisitionFileScheduleDeliveryDateContent = By.XPath("//label[contains(text(),'Delivery date')]/parent::div/following-sibling::div");

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
        private By acquisitionFileTeamMembersTotal = By.XPath("//h2/div/div[contains(text(),'Acquisition Team')]/parent::div/parent::h2/following-sibling::div/div");

        //Acquisition File Details Update Form Elements
        private By acquisitionFileUpdateTitle = By.XPath("//h1[contains(text(),'Update Acquisition File')]");
        private By acquisitionFileHeaderStatusUpdateLabel = By.XPath("//div[@class='justify-content-end row']/div/label[contains(text(),'Status')]");

        private By acquisitionFileStatusUpdateLabel = By.XPath("//div[@class='collapse show']/div/div/label[contains(text(),'Status')]");
        private By acquisitionFileStatusSelect = By.Id("input-fileStatusTypeCode");
        private By acquisitionFileAssignedDateInput = By.Id("datepicker-assignedDate");

        //Acquisition File Main Form Input Elements
        private By acquisitionFileMainFormDiv = By.XPath("//h1[contains(text(),'Create Acquisition File')]/parent::div/parent::div/parent::div/parent::div");
        private By acquisitionFileDeliveryDateInput = By.Id("datepicker-deliveryDate");

        private By acquisitionFileNameInput = By.Id("input-fileName");
        private By acquisitionFilePhysicalStatusSelect = By.Id("input-acquisitionPhysFileStatusType");
        private By acquisitionFileDetailsTypeSelect = By.Id("input-acquisitionType");
        private By acquisitionFileDetailsRegionSelect = By.Id("input-region");

        private By acquisitionFileAddAnotherMemberLink = By.CssSelector("button[data-testid='add-team-member']");
        private By acquisitionFileTeamMembersGroup = By.XPath("//div[contains(text(),'Acquisition Team')]/parent::div/parent::h2/following-sibling::div/div[@class='py-3 row']");

        private By acquisitionFileEditButton = By.CssSelector("button[title='Edit acquisition file']");
        private By acquisitionFileEditPropertiesBttn = By.CssSelector("button[title='Change properties']");

        private By acquisitionFilePropertiesTotal = By.XPath("//h2/div/div[contains(text(),'Selected properties')]/parent::div/parent::h2/following-sibling::div/div");

        //Acquisition File - Properties Elements
        private By acquisitionProperty1stPropLink = By.CssSelector("div[data-testid='menu-item-row-1'] div:nth-child(3)");

        //Acquisition File Confirmation Modal Elements
        private By acquisitionFileConfirmationModal = By.CssSelector("div[class='modal-content']");

        private SharedSelectContact sharedSelectContact;
        private SharedModals sharedModals;
        private SharedSearchProperties sharedSearchProperties;

        public AcquisitionFiles(IWebDriver webDriver) : base(webDriver)
        {
            sharedSelectContact = new SharedSelectContact(webDriver);
            sharedModals = new SharedModals(webDriver);
            sharedSearchProperties = new SharedSearchProperties(webDriver);
        }

        public void NavigateToCreateNewAcquisitionFile()
        {
            Wait();
            webDriver.FindElement(menuAcquisitionButton).Click();

            Wait();
            FocusAndClick(createAcquisitionFileButton);
        }

        public void NavigateToAddPropertiesAcquisitionFile()
        {
            WaitUntil(acquisitionFileEditPropertiesBttn);
            webDriver.FindElement(acquisitionFileEditPropertiesBttn).Click();
        }

        public void NavigateToFileSummary()
        {
            Wait();
            webDriver.FindElement(acquisitionFileSummaryBttn).Click();
        }

        public void CreateMinimumAcquisitionFile(AcquisitionFile acquisition)
        {
            Wait();
            webDriver.FindElement(acquisitionFileNameInput).SendKeys(acquisition.AcquisitionFileName);

            Wait();
            webDriver.FindElement(acquisitionFileDetailsTypeSelect);

            Wait();
            ChooseSpecificSelectOption(acquisitionFileDetailsTypeSelect, acquisition.AcquisitionType);

            Wait();
            ChooseSpecificSelectOption(acquisitionFileDetailsRegionSelect, acquisition.MOTIRegion);
        }

        public void EditAcquisitionFileDetails()
        {
            WaitUntil(acquisitionFileEditButton);
            FocusAndClick(acquisitionFileEditButton);
        }

        public void AddAdditionalInformation(AcquisitionFile acquisition)
        {
            Wait();
            ChooseSpecificSelectOption(acquisitionFileStatusSelect, acquisition.AcquisitionStatus);

            Wait();
            webDriver.FindElement(acquisitionFileProjectInput).SendKeys(acquisition.AcquisitionProject);
            FocusAndClick(acquisitionFileProject1stOption);

            Wait();
            webDriver.FindElement(acquisitionFileProjectProductSelect).Click();
            ChooseSpecificSelectOption(acquisitionFileProjectProductSelect, acquisition.AcquisitionProjProduct);

            ChooseSpecificSelectOption(acquisitionFileProjectFundingInput, acquisition.AcquisitionProjFunding);

            if (webDriver.FindElements(acquisitionFileProjectOtherFundingLabel).Count > 0)
            {
                webDriver.FindElement(acquisitionFileProjectOtherFundingInput).SendKeys(acquisition.AcquisitionFundingOther);
            }

            webDriver.FindElement(acquisitionFileDeliveryDateInput).SendKeys(acquisition.DeliveryDate);
            webDriver.FindElement(acquisitionFileDeliveryDateInput).SendKeys(Keys.Enter);

            ChooseSpecificSelectOption(acquisitionFilePhysicalStatusSelect, acquisition.PhysicalFileStatus);

            if (acquisition.AcquisitionTeam.First().ContactName != "")
            {
                for (var i = 0; i < acquisition.AcquisitionTeam.Count; i++)
                {
                    AddTeamMembers(acquisition.AcquisitionTeam[i].TeamRole, acquisition.AcquisitionTeam[i].ContactName);
                }
            } 
        }

        public void UpdateAcquisitionFile(AcquisitionFile acquisition)
        {
            Wait();
            if (acquisition.AcquisitionFileName != "")
            {
                ClearInput(acquisitionFileNameInput);
                webDriver.FindElement(acquisitionFileNameInput).SendKeys(acquisition.AcquisitionFileName);
            }

            Wait();
            if (acquisition.AcquisitionType != "")
                ChooseSpecificSelectOption(acquisitionFileDetailsTypeSelect, acquisition.AcquisitionType);

            Wait();
            if (acquisition.MOTIRegion != "")
                ChooseSpecificSelectOption(acquisitionFileDetailsRegionSelect, acquisition.MOTIRegion);

            Wait();
            if (acquisition.AcquisitionStatus != "")
                ChooseSpecificSelectOption(acquisitionFileStatusSelect, acquisition.AcquisitionStatus);

            Wait();
            if (acquisition.AcquisitionProject != "")
            {
                ClearInput(acquisitionFileProjectInput);
                webDriver.FindElement(acquisitionFileProjectInput).SendKeys(acquisition.AcquisitionProject);
                FocusAndClick(acquisitionFileProject1stOption);
            }
                
            Wait();
            if (acquisition.AcquisitionProjProduct != "")
            {
                webDriver.FindElement(acquisitionFileProjectProductSelect).Click();
                ChooseSpecificSelectOption(acquisitionFileProjectProductSelect, acquisition.AcquisitionProjProduct);
            }

            if (acquisition.AcquisitionProjFunding != "")
                ChooseSpecificSelectOption(acquisitionFileProjectFundingInput, acquisition.AcquisitionProjFunding);

            if (webDriver.FindElements(acquisitionFileProjectOtherFundingLabel).Count > 0 && acquisition.AcquisitionFundingOther != "")
                webDriver.FindElement(acquisitionFileProjectOtherFundingInput).SendKeys(acquisition.AcquisitionFundingOther);

            if (acquisition.DeliveryDate != "")
            {
                ClearInput(acquisitionFileDeliveryDateInput);
                webDriver.FindElement(acquisitionFileDeliveryDateInput).SendKeys(acquisition.DeliveryDate);
                webDriver.FindElement(acquisitionFileDeliveryDateInput).SendKeys(Keys.Enter);
            }
           
            if(acquisition.PhysicalFileStatus != "") 
                ChooseSpecificSelectOption(acquisitionFilePhysicalStatusSelect, acquisition.PhysicalFileStatus);

            if (acquisition.AcquisitionTeam.First().ContactName != "")
            {
                for (var i = 0; i < acquisition.AcquisitionTeam.Count; i++)
                {
                    AddTeamMembers(acquisition.AcquisitionTeam[i].TeamRole, acquisition.AcquisitionTeam[i].ContactName);
                }
            }
        
        }

        public void DeleteLastStaffMember()
        {
            Wait(4000);
            var memberStaffIndex = webDriver.FindElements(acquisitionFileTeamMembersGroup).Count();
            webDriver.FindElement(By.XPath("//div[contains(text(),'Acquisition Team')]/parent::div/parent::h2/following-sibling::div/div[@class='py-3 row'][" + memberStaffIndex +"]/div[3]/button")).Click();

            WaitUntil(acquisitionFileConfirmationModal);
            Assert.True(sharedModals.ModalHeader() == "Remove Team Member");
            Assert.True(sharedModals.ModalContent() == "Are you sure you want to remove this row?");

            sharedModals.ModalClickOKBttn();

            var memberStaffAfterRemove = webDriver.FindElements(acquisitionFileTeamMembersGroup).Count();

            Assert.True(memberStaffAfterRemove == memberStaffIndex - 1);

        }

        public void ChooseFirstPropertyOption()
        {
            Wait(5000);
            webDriver.FindElement(acquisitionProperty1stPropLink).Click();

            sharedModals.SiteMinderModal();
        }

        public void DeleteLastProperty()
        {
            Wait();
            var propertyIndex = webDriver.FindElements(acquisitionFilePropertiesTotal).Count();
            webDriver.FindElement(By.XPath("//h2/div/div[contains(text(),'Selected properties')]/parent::div/parent::h2/following-sibling::div/div["+ propertyIndex +"]/div[3]/button")).Click();

            Wait();
            var propertiesAfterRemove = webDriver.FindElements(acquisitionFilePropertiesTotal).Count();
            Assert.True(propertiesAfterRemove == propertyIndex - 1);

        }

        public void EditAcquisitionFile()
        {
            Wait(3000);
            webDriver.FindElement(acquisitionFileEditButton).Click();
        }

        public void SaveAcquisitionFile()
        {
            Wait();
            ButtonElement("Save");

            Wait();
            if (webDriver.FindElements(acquisitionFileConfirmationModal).Count() > 0)
            {
                Assert.True(sharedModals.ModalHeader().Equals("Different Ministry region"));
                Assert.True(sharedModals.ModalContent().Equals("Selected Ministry region is different from that of one or more selected properties. Do you wish to continue?"));
                sharedModals.ModalClickOKBttn();
            }

            WaitUntil(acquisitionFileEditButton);
            Assert.True(webDriver.FindElement(acquisitionFileEditButton).Displayed);
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
                    Assert.True(sharedModals.ModalHeader().Equals("Confirm changes"));
                    Assert.True(sharedModals.ConfirmationModalText1().Equals("If you cancel now, this form will not be saved."));
                    Assert.True(sharedModals.ConfirmationModalText2().Equals("Are you sure you want to Cancel?"));
                    sharedModals.ModalClickOKBttn();
                }
            }

            Wait();
            sharedModals.SiteMinderModal();
        }

        public void SaveAcquisitionFileProperties()
        {
            Wait(5000);
            ButtonElement("Save");

            Assert.True(sharedModals.ModalHeader().Equals("Confirm changes"));
            Assert.True(sharedModals.ConfirmationModalText1().Equals("You have made changes to the properties in this file."));
            Assert.True(sharedModals.ConfirmationModalText2().Equals("Do you want to save these changes?"));

            sharedModals.ModalClickOKBttn();

            Wait();
            if (webDriver.FindElements(acquisitionFileConfirmationModal).Count() > 1)
            {
                Assert.True(sharedModals.SecondaryModalHeader().Equals("User Override Required"));
                Assert.Contains("The selected property already exists in the system's inventory. However, the record is missing spatial details.", sharedModals.SecondaryModalContent());
                Assert.Contains("To add the property, the spatial details for this property will need to be updated. The system will attempt to update the property record with spatial information from the current selection.", sharedModals.SecondaryModalContent());
                sharedModals.SecondaryModalClickOKBttn();
            }
        }

        private void AddTeamMembers(string teamRole, string contactName)
        {
            Wait();
            FocusAndClick(acquisitionFileAddAnotherMemberLink);

            Wait();
            var teamMemberIndex = webDriver.FindElements(acquisitionFileTeamMembersGroup).Count() -1;
            var teamMemberCount = webDriver.FindElements(acquisitionFileTeamMembersGroup).Count();

            WaitUntil(By.CssSelector("select[id='input-team["+ teamMemberIndex +"].contactTypeCode']"));
            ChooseSpecificSelectOption(By.CssSelector("select[id='input-team["+ teamMemberIndex +"].contactTypeCode']"), teamRole);
            FocusAndClick(By.CssSelector("div[class='collapse show'] div[class='py-3 row']:nth-child("+ teamMemberCount +") div[class='pl-0 col-auto'] button"));
            sharedSelectContact.SelectContact(contactName);

        }

        public string GetAcquisitionFileCode()
        {
            WaitUntil(acquisitionFileHeaderCodeContent);

            var totalFileName = webDriver.FindElement(acquisitionFileHeaderCodeContent).Text;

            return Regex.Match(totalFileName, "^[^ ]+").Value;
        }

        public int IsCreateAcquisitionFileFormVisible()
        {
            Wait();
            return webDriver.FindElements(acquisitionFileMainFormDiv).Count();
        }

        public void VerifyAcquisitionFileView(AcquisitionFile acquisition)
        {
            Wait();
            Assert.True(webDriver.FindElement(acquisitionFileViewTitle).Displayed);

            //Header
            Assert.True(webDriver.FindElement(acquisitionFileHeaderCodeLabel).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileHeaderCodeContent).Text != "");
            Assert.True(webDriver.FindElement(acquisitionFileHeaderProjectLabel).Displayed);

            if(acquisition.AcquisitionProject != "")
                Assert.True(webDriver.FindElement(acquisitionFileHeaderProjectContent).Text.Equals(acquisition.AcquisitionProjCode + " - "  +acquisition.AcquisitionProject));

            Assert.True(webDriver.FindElement(acquisitionFileHeaderCreatedDateLabel).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileHeaderCreatedDateContent).Text != "");
            Assert.True(webDriver.FindElement(acquisitionFileHeaderCreatedByContent).Text != "");
            Assert.True(webDriver.FindElement(acquisitionFileHeaderLastUpdateLabel).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileHeaderLastUpdateContent).Text != "");
            Assert.True(webDriver.FindElement(acquisitionFileHeaderLastUpdateByContent).Text != "");
            Assert.True(webDriver.FindElement(acquisitionFileHeaderStatusLabel).Displayed);

            //Status
            if(acquisition.AcquisitionStatus != "")
                Assert.True(webDriver.FindElement(acquisitionFileHeaderStatusContent).Text.Equals(acquisition.AcquisitionStatus));

            //Project
            Assert.True(webDriver.FindElement(acquisitionFileProjectSubtitle).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileProjectLabel).Displayed);

            if (acquisition.AcquisitionProject != "")
                Assert.True(webDriver.FindElement(acquisitionFileProjectContent).Text.Equals(acquisition.AcquisitionProjCode + " - " + acquisition.AcquisitionProject));

            Assert.True(webDriver.FindElement(acquisitionFileProjectProductLabel).Displayed);

            if(acquisition.AcquisitionProjProduct != "")
                Assert.True(webDriver.FindElement(acquisitionFileProjectProductContent).Text.Equals(acquisition.AcquisitionProjProduct));

            Assert.True(webDriver.FindElement(acquisitionFileProjectFundingLabel).Displayed);

            if(acquisition.AcquisitionProjFunding != "")
                Assert.True(webDriver.FindElement(acquisitionFileProjectFundingContent).Text.Equals(acquisition.AcquisitionProjFunding));

            if (webDriver.FindElements(acquisitionFileProjectOtherFundingLabel).Count > 0 && acquisition.AcquisitionFundingOther != "")
                Assert.True(webDriver.FindElement(acquisitionFileProjectOtherFundingContent).Text.Equals(acquisition.AcquisitionFundingOther));
            

            //Schedule
            Assert.True(webDriver.FindElement(acquisitionFileScheduleSubtitle).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileScheduleAssigneedDateLabel).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileScheduleAssigneedDateContent).Text.Equals(DateTime.Now.ToString("MMM d, yyyy")));

            Assert.True(webDriver.FindElement(acquisitionFileScheduleDeliveryDateLabel).Displayed);

            if(acquisition.DeliveryDate != "")
                Assert.True(webDriver.FindElement(acquisitionFileScheduleDeliveryDateContent).Text.Equals(TransformDateFormat(acquisition.DeliveryDate)));

            //Details
            Assert.True(webDriver.FindElement(acquisitionFileDetailsSubtitle).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileDetailsNameLabel).Displayed);

            if(acquisition.AcquisitionFileName != "")
                Assert.True(webDriver.FindElement(acquisitionFileDetailsNameContent).Text.Equals(acquisition.AcquisitionFileName));

            Assert.True(webDriver.FindElement(acquisitionFileDetailsPhysicalFileLabel).Displayed);

            if(acquisition.PhysicalFileStatus != "")
                Assert.True(webDriver.FindElement(acquisitionFileDetailsPhysicalFileContent).Text.Equals(acquisition.PhysicalFileStatus));

            Assert.True(webDriver.FindElement(acquisitionFileDetailsTypeLabel).Displayed);

            if(acquisition.AcquisitionType != "")
                Assert.True(webDriver.FindElement(acquisitionFileDetailsTypeContent).Text.Equals(acquisition.AcquisitionType));

            Assert.True(webDriver.FindElement(acquisitionFileDetailsMOTIRegionLabel).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileDetailsMOTIRegionContent).Text != "");

            //Team members
            Assert.True(webDriver.FindElement(acquisitionFileTeamSubtitle).Displayed);

            if (acquisition.AcquisitionTeam.First().ContactName != "")
            {
                for(var i = 0;  i < acquisition.AcquisitionTeam.Count; i++)
                {
                    var index = i + 1;
                    Assert.True(webDriver.FindElement(By.XPath("//h2/div/div[contains(text(),'Acquisition Team')]/parent::div/parent::h2/following-sibling::div/div[" + index + "]/div/label")).Text.Equals(acquisition.AcquisitionTeam[i].TeamRole + ":"));
                    Assert.True(webDriver.FindElement(By.XPath("//h2/div/div[contains(text(),'Acquisition Team')]/parent::div/parent::h2/following-sibling::div/div[" + index + "]/div/a")).Text.Equals(acquisition.AcquisitionTeam[i].ContactName));
                }
            }
        }

        //public void VerifyAcquisitionFileUpdate()
        //{
        //    Wait();

        //    Assert.True(webDriver.FindElement(acquisitionFileUpdateTitle).Displayed);

        //    //Header
        //    Assert.True(webDriver.FindElement(acquisitionFileHeaderCodeLabel).Displayed);
        //    Assert.True(webDriver.FindElement(acquisitionFileHeaderCodeContent).Text != "");
        //    Assert.True(webDriver.FindElement(acquisitionFileHeaderProjectLabel).Displayed);
        //    Assert.True(webDriver.FindElement(acquisitionFileHeaderProjectContent).Text != "");
        //    Assert.True(webDriver.FindElement(acquisitionFileHeaderCreatedDateLabel).Displayed);
        //    Assert.True(webDriver.FindElement(acquisitionFileHeaderCreatedDateContent).Text != "");
        //    Assert.True(webDriver.FindElement(acquisitionFileHeaderCreatedByContent).Text != "");
        //    Assert.True(webDriver.FindElement(acquisitionFileHeaderLastUpdateLabel).Displayed);
        //    Assert.True(webDriver.FindElement(acquisitionFileHeaderLastUpdateContent).Text != "");
        //    Assert.True(webDriver.FindElement(acquisitionFileHeaderLastUpdateByContent).Text != "");
        //    Assert.True(webDriver.FindElement(acquisitionFileHeaderStatusUpdateLabel).Displayed);
        //    Assert.True(webDriver.FindElement(acquisitionFileHeaderStatusContent).Text != "");

        //    //Status
        //    Assert.True(webDriver.FindElement(acquisitionFileStatusUpdateLabel).Displayed);
        //    Assert.True(webDriver.FindElement(acquisitionFileStatusSelect).Displayed);

        //    //Project
        //    Assert.True(webDriver.FindElement(acquisitionFileProjectSubtitle).Displayed);
        //    Assert.True(webDriver.FindElement(acquisitionFileProjectLabel).Displayed);
        //    Assert.True(webDriver.FindElement(acquisitionFileProjectInput).Displayed);
        //    Assert.True(webDriver.FindElement(acquisitionFileProjectFundingLabel).Displayed);
        //    Assert.True(webDriver.FindElement(acquisitionFileProjectFundingInput).Displayed);

        //    //Schedule
        //    Assert.True(webDriver.FindElement(acquisitionFileScheduleSubtitle).Displayed);
        //    Assert.True(webDriver.FindElement(acquisitionFileScheduleAssigneedDateLabel).Displayed);
        //    Assert.True(webDriver.FindElement(acquisitionFileAssignedDateInput).Displayed);
        //    Assert.True(webDriver.FindElement(acquisitionFileScheduleDeliveryDateLabel).Displayed);
        //    Assert.True(webDriver.FindElement(acquisitionFileDeliveryDateInput).Displayed);

        //    //Details
        //    Assert.True(webDriver.FindElement(acquisitionFileDetailsSubtitle).Displayed);
        //    Assert.True(webDriver.FindElement(acquisitionFileDetailsNameLabel).Displayed);
        //    Assert.True(webDriver.FindElement(acquisitionFileNameInput).Displayed);
        //    Assert.True(webDriver.FindElement(acquisitionFileDetailsPhysicalFileLabel).Displayed);
        //    Assert.True(webDriver.FindElement(acquisitionFilePhysicalStatusSelect).Displayed);
        //    Assert.True(webDriver.FindElement(acquisitionFileDetailsTypeLabel).Displayed);
        //    Assert.True(webDriver.FindElement(acquisitionFileDetailsTypeSelect).Displayed);
        //    Assert.True(webDriver.FindElement(acquisitionFileDetailsMOTIRegionLabel).Displayed);
        //    Assert.True(webDriver.FindElement(acquisitionFileDetailsRegionSelect).Displayed);

        //    //Team members
        //    Assert.True(webDriver.FindElement(acquisitionFileTeamSubtitle).Displayed);
        //}

        public void VerifyAcquisitionFileCreate()
        {
            Wait();

            Assert.True(webDriver.FindElement(acquisitionFileCreateTitle).Displayed);

            //Project
            Assert.True(webDriver.FindElement(acquisitionFileProjectSubtitle).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileProjectLabel).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileProjectInput).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileProjectFundingLabel).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileProjectFundingInput).Displayed);

            //Schedule
            Assert.True(webDriver.FindElement(acquisitionFileScheduleSubtitle).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileScheduleAssigneedDateLabel).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileAssignedDateInput).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileScheduleDeliveryDateLabel).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileDeliveryDateInput).Displayed);

            //Search Property Component
            sharedSearchProperties.VerifyLocateOnMapFeature();

            //Details
            Assert.True(webDriver.FindElement(acquisitionFileDetailsSubtitle).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileDetailsNameLabel).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileNameInput).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileDetailsPhysicalFileLabel).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFilePhysicalStatusSelect).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileDetailsTypeLabel).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileDetailsTypeSelect).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileDetailsMOTIRegionLabel).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileDetailsRegionSelect).Displayed);

            //Team members
            Assert.True(webDriver.FindElement(acquisitionFileTeamSubtitle).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileAddAnotherMemberLink).Displayed);
        }
    }
}
