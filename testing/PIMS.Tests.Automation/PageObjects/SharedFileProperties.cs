using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SharedFileProperties : PageObjectBase
    {
        //Search Properties Section Elements
        private readonly By searchSectionTitle = By.XPath("//div[contains(text(),'Properties to include in this file')]");
        private readonly By searchSectionSubfileTitle = By.XPath("//div[contains(text(),'Properties to include in this sub-file')]");
        private readonly By searchSectionInstructions = By.XPath("//div[contains(text(),'Properties to include in this file')]/parent::div/parent::h2/following-sibling::div/div[1]");
        private readonly By searchSectionSubfileInstructions = By.XPath("//div[contains(text(),'Properties to include in this sub-file')]/parent::div/parent::h2/following-sibling::div/div[1]");

        //Selected Properties Elements
        private readonly By searchPropertiesSelectedPropertiesSubtitle = By.XPath("//div[contains(text(),'Selected Properties')]");
        private readonly By searchPropertiesSelectedIdentifierHeader = By.XPath("//div[@class='collapse show']/div/div[contains(text(),'Identifier')]");
        private readonly By searchPropertiesSelectedDescriptiveNameHeader = By.XPath("//div[@class='collapse show']/div/div[contains(text(),'Provide a descriptive name for this land')]");
        private readonly By searchPropertiesSelectedToolTipIcon = By.CssSelector("span[data-testid='tooltip-icon-property-selector-tooltip']");
        private readonly By searchPropertiesSelectedDefault = By.XPath("//span[contains(text(),'No Properties selected')]");
        private readonly By searchPropertiesPropertiesInFileTotal = By.CssSelector("div[class='align-items-center mb-3 no-gutters row']");
        private readonly By searchPropertiesPropertiesInLeaseTotal = By.CssSelector("div[class='align-items-center my-3 no-gutters row']");

        //File - Edit Properties button
        private readonly By fileEditPropertiesBttn = By.CssSelector("button[title='Change properties']");

        //File - Properties Elements
        private readonly By acquisitionProperty1stPropLink = By.CssSelector("div[data-testid='menu-item-row-1'] div:nth-child(3)");

        //File Confirmation Modal Elements
        private readonly By propertiesFileConfirmationModal = By.CssSelector("div[class='modal-content']");

        //Toast Element
        //private readonly By duplicatePropToast = By.CssSelector("div[id='duplicate-property'] div[class='Toastify__toast-body']");

        private SharedModals sharedModals;

        public SharedFileProperties(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
        }

        public void AddNameSelectedProperty(string name, int index)
        {
            WaitUntilVisible(By.Id("input-properties." + index + ".name"));
            webDriver.FindElement(By.Id("input-properties." + index + ".name")).SendKeys(name);
        }

        public void VerifyLocateOnMapFeature(string fileType)
        {
            Wait(2000);

            if (fileType == "Subfile")
            {
                AssertTrueIsDisplayed(searchSectionSubfileTitle);
                AssertTrueIsDisplayed(searchSectionSubfileInstructions);
            }
            else
            {
                AssertTrueIsDisplayed(searchSectionTitle);
                AssertTrueIsDisplayed(searchSectionInstructions);
            }

            AssertTrueIsDisplayed(searchPropertiesSelectedPropertiesSubtitle);
            AssertTrueIsDisplayed(searchPropertiesSelectedIdentifierHeader);
            AssertTrueIsDisplayed(searchPropertiesSelectedDescriptiveNameHeader);
            AssertTrueIsDisplayed(searchPropertiesSelectedToolTipIcon);

            if (webDriver.FindElements(searchPropertiesSelectedDefault).Count > 0)
            {
                WaitUntilVisible(searchPropertiesSelectedDefault);
                AssertTrueIsDisplayed(searchPropertiesSelectedDefault);
            }
        }

        public void NavigateToAddPropertiesToFile()
        {
            Wait();
            webDriver.FindElement(fileEditPropertiesBttn).Click();
        }

        public void SelectFirstPropertyOptionFromFile()
        {
            WaitUntilClickable(acquisitionProperty1stPropLink);
            webDriver.FindElement(acquisitionProperty1stPropLink).Click();
        }

        public void SelectNthPropertyOptionFromFile(int index)
        {
            var elementOrder = index++;
            By chosenProperty = By.CssSelector("div[data-testid='menu-item-row-" + elementOrder + "'] div:nth-child(3)");

            WaitUntilClickable(chosenProperty);
            webDriver.FindElement(chosenProperty).Click();
        }

        public void DeleteLastPropertyFromFile()
        {
            Wait();
            var propertyIndex = webDriver.FindElements(searchPropertiesPropertiesInFileTotal).Count();

            WaitUntilClickable(By.XPath("//h2/div/div[contains(text(),'Selected properties')]/parent::div/parent::h2/following-sibling::div/div[@class='align-items-center mb-3 no-gutters row'][" + propertyIndex + "]/div[4]/button"));
            webDriver.FindElement(By.XPath("//h2/div/div[contains(text(),'Selected properties')]/parent::div/parent::h2/following-sibling::div/div[@class='align-items-center mb-3 no-gutters row'][" + propertyIndex + "]/div[4]/button")).Click();

            Wait();
            if (webDriver.FindElements(propertiesFileConfirmationModal).Count > 0)
            {
                Assert.True(sharedModals.ModalHeader() == "Removing Property from form");
                Assert.True(sharedModals.ModalContent() == "Are you sure you want to remove this property from this lease/license?");

                sharedModals.ModalClickOKBttn();
            }

            Wait();
            var propertiesAfterRemove = webDriver.FindElements(searchPropertiesPropertiesInFileTotal).Count();
            Assert.True(propertiesAfterRemove == propertyIndex - 1);

        }

        public void DeleteLastPropertyFromLease()
        {
            Wait();
            var propertyIndex = webDriver.FindElements(searchPropertiesPropertiesInLeaseTotal).Count();

            WaitUntilClickable(By.XPath("//h2/div/div[contains(text(),'Selected properties')]/parent::div/parent::h2/following-sibling::div/div[@class='align-items-center my-3 no-gutters row'][" + propertyIndex + "]/div[4]/button"));
            webDriver.FindElement(By.XPath("//h2/div/div[contains(text(),'Selected properties')]/parent::div/parent::h2/following-sibling::div/div[@class='align-items-center my-3 no-gutters row'][" + propertyIndex + "]/div[4]/button")).Click();

            Wait();
            if (webDriver.FindElements(propertiesFileConfirmationModal).Count > 0)
            {
                Assert.True(sharedModals.ModalHeader() == "Removing Property from Lease/Licence");
                Assert.True(sharedModals.ModalContent() == "Are you sure you want to remove this property from this lease/licence?");

                sharedModals.ModalClickOKBttn();
            }

            Wait();
            var propertiesAfterRemove = webDriver.FindElements(searchPropertiesPropertiesInLeaseTotal).Count();
            Assert.True(propertiesAfterRemove == propertyIndex - 1);

        }

        public void SaveFileProperties()
        {
            Wait();
            ButtonElement("Save");

            Assert.Equal("Confirm changes", sharedModals.ModalHeader());
            Assert.Equal("You have made changes to the properties in this file.", sharedModals.ConfirmationModalText1());
            Assert.Equal("Do you want to save these changes?", sharedModals.ConfirmationModalText2());

            sharedModals.ModalClickOKBttn();

            Wait();
            while (webDriver.FindElements(propertiesFileConfirmationModal).Count() > 0)
            {

                if (sharedModals.SecondaryModalContent().Contains("You have added one or more properties to the disposition file that are not in the MOTI Inventory"))
                {
                    Assert.Equal("User Override Required", sharedModals.SecondaryModalHeader());
                    Assert.Equal("You have added one or more properties to the disposition file that are not in the MOTI Inventory. Do you want to proceed?", sharedModals.SecondaryModalContent());
                }
                else if (sharedModals.SecondaryModalContent().Contains("You have added one or more properties to the management file that are not in the MOTI Inventory"))
                {
                    Assert.Equal("User Override Required", sharedModals.SecondaryModalHeader());
                    Assert.Equal("You have added one or more properties to the management file that are not in the MOTI Inventory. To acquire these properties, add them to an acquisition file. Do you want to proceed?", sharedModals.SecondaryModalContent());
                }
                else
                {
                    Assert.Equal("User Override Required", sharedModals.SecondaryModalHeader());
                    Assert.Contains("The selected property already exists in the system's inventory. However, the record is missing spatial details.", sharedModals.SecondaryModalContent());
                    Assert.Contains("To add the property, the spatial details for this property will need to be updated. The system will attempt to update the property record with spatial information from the current selection.", sharedModals.SecondaryModalContent());
                }
                sharedModals.SecondaryModalClickOKBttn();
                Wait();
            }
        }
    }
}
