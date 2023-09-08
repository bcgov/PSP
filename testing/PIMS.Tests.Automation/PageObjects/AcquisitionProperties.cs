using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMS.Tests.Automation.PageObjects
{
    public class AcquisitionProperties : PageObjectBase
    {
        private By acquisitionFileEditPropertiesBttn = By.CssSelector("button[title='Change properties']");

        //Acquisition File - Properties Elements
        private By acquisitionProperty1stPropLink = By.CssSelector("div[data-testid='menu-item-row-1'] div:nth-child(3)");
        private By acquisitionFilePropertiesTotal = By.XPath("//h2/div/div[contains(text(),'Selected properties')]/parent::div/parent::h2/following-sibling::div/div");

        //Acquisition File Confirmation Modal Elements
        private By acquisitionFileConfirmationModal = By.CssSelector("div[class='modal-content']");

        private SharedModals sharedModals;

        public AcquisitionProperties(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
        }

        public void NavigateToAddPropertiesAcquisitionFile()
        {
            WaitUntilVisible(acquisitionFileEditPropertiesBttn);
            webDriver.FindElement(acquisitionFileEditPropertiesBttn).Click();
        }

        public void ChooseFirstPropertyOption()
        {
            WaitUntilClickable(acquisitionProperty1stPropLink);
            webDriver.FindElement(acquisitionProperty1stPropLink).Click();
        }

        public void DeleteLastProperty()
        {
            Wait();
            var propertyIndex = webDriver.FindElements(acquisitionFilePropertiesTotal).Count();

            WaitUntilClickable(By.XPath("//h2/div/div[contains(text(),'Selected properties')]/parent::div/parent::h2/following-sibling::div/div["+ propertyIndex +"]/div[3]/button"));
            webDriver.FindElement(By.XPath("//h2/div/div[contains(text(),'Selected properties')]/parent::div/parent::h2/following-sibling::div/div["+ propertyIndex +"]/div[3]/button")).Click();

            Wait();
            var propertiesAfterRemove = webDriver.FindElements(acquisitionFilePropertiesTotal).Count();
            Assert.True(propertiesAfterRemove == propertyIndex - 1);

        }

        public void SaveAcquisitionFileProperties()
        {
            Wait();
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
    }
}
