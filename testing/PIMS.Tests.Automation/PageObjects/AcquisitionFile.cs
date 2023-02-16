using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Support.UI;

namespace PIMS.Tests.Automation.PageObjects
{
    public class AcquisitionFile : PageObjectBase
    {
        //Acquisition Files Menu Elements
        private By menuAcquisitionButton = By.XPath("//a/label[contains(text(),'Acquisition')]/parent::a");
        private By createAcquisitionFileButton = By.XPath("//a[contains(text(),'Create an Acquisition File')]");

        //Acquisition File Details View Form Elements
        private By acquisitionFileViewTitle = By.XPath("//h1[contains(text(),'Acquisition File')]");
        
        private By acquisitionFileCreateTitle = By.XPath("//h1[contains(text(),'Create Acquisition File')]");
        private By acquisitionFileHeaderCodeLabel = By.XPath("//label[contains(text(), 'File #:')]");
        private By acquisitionFileHeaderCodeContent = By.XPath("//label[contains(text(), 'File #:')]/parent::div/following-sibling::div[1]/strong");

        private By acquisitionFileHeaderProjectLabel = By.XPath("//label[contains(text(), 'Ministry project')]");
        private By acquisitionFileHeaderProjectContent = By.XPath("//label[contains(text(), 'Ministry project')]/parent::div/following-sibling::div[1]/strong");
        private By acquisitionFileHeaderCreatedDateLabel = By.XPath("//span[contains(text(), 'Created')]");
        private By acquisitionFileHeaderCreatedDateContent = By.XPath("//span[contains(text(), 'Created')]/strong");
        private By acquisitionFileHeaderCreatedByContent = By.XPath("//span[contains(text(),'Created')]/span[@id='userNameTooltip']/span");
        private By acquisitionFileHeaderLastUpdateLabel = By.XPath("//span[contains(text(), 'Last updated')]");
        private By acquisitionFileHeaderLastUpdateContent = By.XPath("//span[contains(text(), 'Last updated')]/strong");
        private By acquisitionFileHeaderLastUpdateByContent = By.XPath("//span[contains(text(), 'Last updated')]//span[@id='userNameTooltip']/span");
        private By acquisitionFileHeaderStatusLabel = By.XPath("//label[contains(text(),'Status')]");
        private By acquisitionFileHeaderStatusContent = By.XPath("//label[contains(text(),'Status')]/parent::div/following-sibling::div[1]/strong");

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
        private By acquisitionFileTeamMembersTotal = By.XPath("//h2/div/div[contains(text(),'Acquisition team')]/parent::div/parent::h2/following-sibling::div/div");

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
        private By acquisitionFileTeamMembersGroup = By.CssSelector("div[class='collapse show'] div[class='py-3 row']");

        private By acquisitionFileEditButton = By.CssSelector("button[title='Edit acquisition file']");
        private By acquisitionFileEditPropertiesBttn = By.CssSelector("button[title='Edit acquisition properties']");

        private By acquisitionFilePropertiesTotal = By.XPath("//h2/div/div[contains(text(),'Selected properties')]/parent::div/parent::h2/following-sibling::div/div");

        //Acquisition File - Properties Elements
        private By acquisitionProperty1stPropLink = By.CssSelector("div[data-testid='menu-item-row-1'] div:nth-child(3)");

        //Acquisition File Confirmation Modal Elements
        private By acquisitionFileConfirmationModal = By.CssSelector("div[class='modal-content']");

        private SharedSelectContact sharedSelectContact;
        private SharedModals sharedModals;
        private SharedSearchProperties sharedSearchProperties;

        public AcquisitionFile(IWebDriver webDriver) : base(webDriver)
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
            webDriver.FindElement(createAcquisitionFileButton).Click();
        }

        public void NavigateToAddPropertiesAcquisitionFile()
        {
            WaitUntil(acquisitionFileEditPropertiesBttn);
            webDriver.FindElement(acquisitionFileEditPropertiesBttn).Click();
        }

        public void CreateMinimumAcquisitionFile(string acquisitionName)
        {
            Wait();
            webDriver.FindElement(acquisitionFileNameInput).SendKeys(acquisitionName);

            Wait();
            webDriver.FindElement(acquisitionFileDetailsTypeSelect);

            Wait();
            ChooseRandomSelectOption(acquisitionFileDetailsTypeSelect, 2);

            Wait();
            ChooseRandomSelectOption(acquisitionFileDetailsRegionSelect, 2);
        }

        public void EditAcquisitionFileDetails()
        {
            WaitUntil(acquisitionFileEditButton);
            FocusAndClick(acquisitionFileEditButton);
        }

        public void AddAdditionalInformation(string deliveryDate, string teamMember1, string teamMember2)
        {
            Wait();
            webDriver.FindElement(acquisitionFileDeliveryDateInput).SendKeys(deliveryDate);
            webDriver.FindElement(acquisitionFileScheduleDeliveryDateLabel).Click();

            ChooseRandomSelectOption(acquisitionFilePhysicalStatusSelect, 2);

            AddTeamMembers(teamMember1);
            AddTeamMembers(teamMember2);
        }

        public void ChangeStatus(string status)
        {
            Wait();
            ChooseSpecificSelectOption(acquisitionFileStatusSelect, status);
        }

        public void DeleteLastStaffMember()
        {
            Wait();
            var memberStaffIndex = webDriver.FindElements(acquisitionFileTeamMembersGroup).Count();
            webDriver.FindElement(By.XPath("//div[@class='collapse show']/div[@class='py-3 row']["+ memberStaffIndex +"]/div[3]/button")).Click();

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
        }

        public void DeleteLastProperty()
        {
            Wait();
            var propertyIndex = webDriver.FindElements(acquisitionFilePropertiesTotal).Count();
            webDriver.FindElement(By.XPath("//h2/div/div[contains(text(),'Selected properties')]/parent::div/parent::h2/following-sibling::div/div["+ propertyIndex +"]/div[3]/button")).Click();

            var propertiesAfterRemove = webDriver.FindElements(acquisitionFilePropertiesTotal).Count();

            Assert.True(propertiesAfterRemove == propertyIndex - 1);

        }

        public void EditAcquisitionFile()
        {
            Wait();
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
                    Assert.True(sharedModals.ConfirmationModalText1().Equals("If you cancel now, this acquisition file will not be saved."));
                    Assert.True(sharedModals.ConfirmationModalText2().Equals("Are you sure you want to Cancel?"));
                    sharedModals.ModalClickOKBttn();
                }
            }           
        }

        public void SaveAcquisitionFileProperties()
        {
            Wait(5000);
            ButtonElement("Save");

            Assert.True(sharedModals.ModalHeader().Equals("Confirm changes"));
            Assert.True(sharedModals.ConfirmationModalText1().Equals("You have made changes to the properties in this file."));
            Assert.True(sharedModals.ConfirmationModalText2().Equals("Do you want to save these changes?"));

            sharedModals.ModalClickOKBttn();
        }

        private void AddTeamMembers(string contactName)
        {
            Wait();
            FocusAndClick(acquisitionFileAddAnotherMemberLink);

            Wait();
            var teamMemberIndex = webDriver.FindElements(acquisitionFileTeamMembersGroup).Count() -1;
            var teamMemberCount = webDriver.FindElements(acquisitionFileTeamMembersGroup).Count();

            WaitUntil(By.CssSelector("select[id='input-team["+ teamMemberIndex +"].contactTypeCode']"));
            ChooseRandomSelectOption(By.CssSelector("select[id='input-team["+ teamMemberIndex +"].contactTypeCode']"), 2);
            FocusAndClick(By.CssSelector("div[class='collapse show'] div[class='py-3 row']:nth-child("+ teamMemberCount +") div[class='pl-0 col-auto'] button"));
            sharedSelectContact.SelectContact(contactName);

        }

        //Get the acquisition file number
        public string GetAcquisitionFileCode()
        {
            WaitUntil(acquisitionFileHeaderCodeContent);
            return webDriver.FindElement(acquisitionFileHeaderCodeContent).Text;
        }

        public int IsCreateAcquisitionFileFormVisible()
        {
            Wait();
            return webDriver.FindElements(acquisitionFileMainFormDiv).Count();
        }

        public void VerifyAcquisitionFileView()
        {
            Wait();
            Assert.True(webDriver.FindElement(acquisitionFileViewTitle).Displayed);

            //Header
            Assert.True(webDriver.FindElement(acquisitionFileHeaderCodeLabel).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileHeaderCodeContent).Text != null);
            Assert.True(webDriver.FindElement(acquisitionFileHeaderProjectLabel).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileHeaderProjectContent).Text != null);
            Assert.True(webDriver.FindElement(acquisitionFileHeaderCreatedDateLabel).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileHeaderCreatedDateContent).Text != null);
            Assert.True(webDriver.FindElement(acquisitionFileHeaderCreatedByContent).Text != null);
            Assert.True(webDriver.FindElement(acquisitionFileHeaderLastUpdateLabel).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileHeaderLastUpdateContent).Text != null);
            Assert.True(webDriver.FindElement(acquisitionFileHeaderLastUpdateByContent).Text != null);
            Assert.True(webDriver.FindElement(acquisitionFileHeaderStatusLabel).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileHeaderStatusContent).Text != null);

            //Schedule
            Assert.True(webDriver.FindElement(acquisitionFileScheduleSubtitle).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileScheduleAssigneedDateLabel).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileScheduleAssigneedDateContent).Text != null);
            Assert.True(webDriver.FindElement(acquisitionFileScheduleDeliveryDateLabel).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileScheduleDeliveryDateContent).Text != null);

            //Details
            //Assert.True(webDriver.FindElement(acquisitionFileDetailsSubtitle).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileDetailsNameLabel).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileDetailsNameContent).Text != null);
            Assert.True(webDriver.FindElement(acquisitionFileDetailsPhysicalFileLabel).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileDetailsPhysicalFileContent).Text != null);
            Assert.True(webDriver.FindElement(acquisitionFileDetailsTypeLabel).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileDetailsTypeContent).Text != null);
            Assert.True(webDriver.FindElement(acquisitionFileDetailsMOTIRegionLabel).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileDetailsMOTIRegionContent).Text != null);

            //Team members
            //Assert.True(webDriver.FindElement(acquisitionFileTeamSubtitle).Displayed);
            Assert.True(webDriver.FindElements(acquisitionFileTeamMembersTotal).Count() > 0);

        }

        public void VerifyAcquisitionFileUpdate()
        {
            Wait();

            Assert.True(webDriver.FindElement(acquisitionFileUpdateTitle).Displayed);

            //Header
            Assert.True(webDriver.FindElement(acquisitionFileHeaderCodeLabel).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileHeaderCodeContent).Text != null);
            Assert.True(webDriver.FindElement(acquisitionFileHeaderProjectLabel).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileHeaderProjectContent).Text != null);
            Assert.True(webDriver.FindElement(acquisitionFileHeaderCreatedDateLabel).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileHeaderCreatedDateContent).Text != null);
            Assert.True(webDriver.FindElement(acquisitionFileHeaderCreatedByContent).Text != null);
            Assert.True(webDriver.FindElement(acquisitionFileHeaderLastUpdateLabel).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileHeaderLastUpdateContent).Text != null);
            Assert.True(webDriver.FindElement(acquisitionFileHeaderLastUpdateByContent).Text != null);
            Assert.True(webDriver.FindElement(acquisitionFileHeaderStatusUpdateLabel).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileHeaderStatusContent).Text != null);

            //Status
            Assert.True(webDriver.FindElement(acquisitionFileStatusUpdateLabel).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileStatusSelect).Displayed);

            //Schedule
            Assert.True(webDriver.FindElement(acquisitionFileScheduleSubtitle).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileScheduleAssigneedDateLabel).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileAssignedDateInput).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileScheduleDeliveryDateLabel).Displayed);
            Assert.True(webDriver.FindElement(acquisitionFileDeliveryDateInput).Displayed);

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
        }

        public void VerifyAcquisitionFileCreate()
        {
            Wait();

            Assert.True(webDriver.FindElement(acquisitionFileCreateTitle).Displayed);

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
