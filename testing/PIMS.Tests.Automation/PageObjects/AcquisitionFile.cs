

using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class AcquisitionFile : PageObjectBase
    {
        private By menuAcquisitionButton = By.XPath("//a/label[contains(text(),'Acquisition')]/parent::a");
        private By createAcquisitionFileButton = By.XPath("//a[contains(text(),'Create an Acquisition File')]");

        private By acquisitionFileAssignedDateInput = By.Id("datepicker-assignedDate");
        private By acquisitionFileDeliveryDateInput = By.Id("datepicker-deliveryDate");

        private By acquisitionFileNameInput = By.Id("input-fileName");
        private By acquisitionFilePhysicalStatusSelect = By.Id("input-acquisitionPhysFileStatusType");
        private By acquisitionFileTypeSelect = By.Id("input-acquisitionType");
        private By acquisitionRegionSelect = By.Id("input-region");

        private By acquisitionFileAddAnotherMemberLink = By.CssSelector("button[data-testid='add-team-member']");
        private By acquisitionFileTeamMembersGroup = By.CssSelector("div[class='collapse show'] div[class='py-3 row']");

        private By acquisitionFileEditButton = By.CssSelector("button[title='Edit acquisition file']");
        private By acquisitionFileEditPropertiesBttn = By.CssSelector("button[title='Edit acquisition properties']");

        private By acquisitionFileConfirmationModal = By.CssSelector("div[class='modal-content']");
        private By acquisitionFileModalOkButton = By.CssSelector("button[title='ok-modal']");

        private SharedSelectContact sharedSelectContact;


        public AcquisitionFile(IWebDriver webDriver) : base(webDriver)
        {
            sharedSelectContact = new SharedSelectContact(webDriver);
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
            webDriver.FindElement(acquisitionFileTypeSelect);

            Wait();
            ChooseRandomSelectOption(acquisitionFileTypeSelect,"input-acquisitionType", 2);

            Wait();
            ChooseRandomSelectOption(acquisitionRegionSelect, "input-region", 2);
        }

        public void AddAdditionalInformation(string deliveryDate, string teamMember1, string teamMember2)
        {
            WaitUntil(acquisitionFileEditButton);
            FocusAndClick(acquisitionFileEditButton);

            Wait();
            webDriver.FindElement(acquisitionFileDeliveryDateInput).SendKeys(deliveryDate);

            ChooseRandomSelectOption(acquisitionFilePhysicalStatusSelect, "input-acquisitionPhysFileStatusType", 2);

            AddTeamMembers(teamMember1);
            AddTeamMembers(teamMember2);
        }

        public void SaveAcquisitionFile()
        {
            Wait();
            ButtonElement("Save");

            WaitUntil(acquisitionFileEditButton);
            Assert.True(webDriver.FindElement(acquisitionFileEditButton).Displayed);
        }

        public void SaveAcquisitionFileProperties()
        {
            Wait();
            ButtonElement("Save");

            WaitUntil(acquisitionFileConfirmationModal);
            webDriver.FindElement(acquisitionFileModalOkButton).Click();

            WaitUntil(acquisitionFileEditButton);
            Assert.True(webDriver.FindElement(acquisitionFileEditButton).Displayed);
        }

        private void AddTeamMembers(string contactName)
        {
            Wait();
            FocusAndClick(acquisitionFileAddAnotherMemberLink);

            Wait();
            var teamMemberIndex = webDriver.FindElements(acquisitionFileTeamMembersGroup).Count() -1;
            var teamMemberCount = webDriver.FindElements(acquisitionFileTeamMembersGroup).Count();

            WaitUntil(By.CssSelector("select[id='input-team["+ teamMemberIndex +"].contactTypeCode']"));
            ChooseRandomSelectOption(By.CssSelector("select[id='input-team["+ teamMemberIndex +"].contactTypeCode']"), "input-team["+ teamMemberIndex +"].contactTypeCode", 2);
            FocusAndClick(By.CssSelector("div[class='collapse show'] div[class='py-3 row']:nth-child("+ teamMemberCount +") div[class='pl-0 col-auto'] button"));
            sharedSelectContact.SelectContact(contactName);

        }
    }
}
