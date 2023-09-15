

using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SharedSearchProperties : PageObjectBase
    {
        //Search Properties Section Elements
        private By searchSectionTitle = By.XPath("//div[contains(text(),'Properties to include in this file')]");
        private By searchSectionInstructions = By.XPath("//div[contains(text(),'Properties to include in this file')]/parent::div/parent::h2/following-sibling::div/div[1]");

        //Locate on Map Elements
        private By locateOnMapTab = By.XPath("//a[contains(text(),'Locate on Map')]");
        private By locateOnMapSubtitle = By.XPath("//h3[contains(text(), 'Select a property')]");
        private By locateOnMapBlueIcon = By.Id("Layer_2");
        private By locateOnMapInstuction1 = By.XPath("//li[contains(text(),'Single-click blue marker above')]");
        private By locateOnMapInstuction2 = By.XPath("//li[contains(text(),'Mouse to a parcel on the map')]");
        private By locateOnMapInstuction3 = By.XPath("//li[contains(text(),'Single-click on parcel to select it')]");
        private By locateOnMapSelectedLabel = By.XPath("//div[contains(text(),'Selected property attributes')]");
        private By locateOnMapPIDLabel = By.XPath("//label[contains(text(),'PID')]");
        private By locateOnMapPlanLabel = By.XPath("//label[contains(text(),'Plan #')]");
        private By locateOnMapAddressLabel = By.XPath("//label[contains(text(),'Address')]");
        private By locateOnMapRegionLabel = By.XPath("//label[contains(text(),'Region')]");
        private By locateOnMapDistrictLabel = By.XPath("//label[contains(text(),'District')]");
        private By locateOnMapLegalDescriptionLabel = By.XPath("//label[contains(text(),'Legal Description')]");

        //Search Tab Elements
        private By searchByTab = By.XPath("//a[contains(text(),'Search')]");
        private By searchBySubtitle = By.XPath("//h3[contains(text(), 'Search for a property')]");
        private By searchBySelect = By.Id("input-searchBy");
        private By searchByPIDInput = By.Id("input-pid");
        private By searchByPINInput = By.Id("input-pin");
        private By searchByAddressInput = By.Id("input-address");
        private By searchByAddressInputSuggestionList = By.CssSelector("div[class='suggestionList']");
        private By searchByAddressSuggestion1stOption = By.CssSelector("div[class='suggestionList'] option:nth-child(1)");
        private By searchByPlanInput = By.Id("input-planNumber");
        private By searchByLegalDescriptionInput = By.Id("input-legalDescription");
        private By searchByButton = By.Id("search-button");
        private By searchResetButton = By.Id("reset-button");

        //Search Results Elements
        private By searchPropertiesResultTableHeader = By.CssSelector("div[class='thead thead-light']");
        private By searchPropsResultSelectAllInput = By.CssSelector("input[data-testid='selectrow-parent']");
        private By searchPropResultsPIDHeader = By.XPath("//div[@class='th']/div[contains(text(), 'PID')]");
        private By searchPropResultsPINHeader = By.XPath("//div[@class='th']/div[contains(text(), 'PIN')]");
        private By searchPropResultsPlanHeader = By.XPath("//div[@class='th']/div[contains(text(), 'Plan #')]");
        private By searchPropResultsAddressHeader = By.XPath("//div[@class='th']/div[contains(text(), 'Address')]");
        private By searchPropResultsLegalDescriptHeader = By.XPath("//div[@class='th']/div[contains(text(), 'Legal Description')]");

        private By searchPropertiesNoRowsResult = By.CssSelector("div[data-testid='map-properties'] div[class='no-rows-message']");
        private By searchProperties1stResultPropDiv = By.CssSelector("div[data-testid='map-properties'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1)");
        private By searchProperties1stResultPropCheckbox = By.CssSelector("div[data-testid='map-properties'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td']:nth-child(1) input");

        private By searchPropertiesAddSelectionBttn = By.XPath("//button/div[contains(text(),'Add to selection')]");

        //Selected Properties Elements
        private By searchPropertiesSelectedPropertiesSubtitle = By.XPath("//div[contains(text(),'Selected properties')]");
        private By searchPropertiesSelectedIdentifierHeader = By.XPath("//div[@class='collapse show']/div/div[contains(text(),'Identifier')]");
        private By searchPropertiesSelectedDescriptiveNameHeader = By.XPath("//div[@class='collapse show']/div/div[contains(text(),'Provide a descriptive name for this land')]");
        private By searchPropertiesSelectedToolTipIcon = By.CssSelector("span[data-testid='tooltip-icon-property-selector-tooltip']");
        private By searchPropertiesSelectedDefault = By.XPath("//span[contains(text(),'No Properties selected')]");
        private By searchPropertiesSelectedPropertiesTotal = By.CssSelector("div[class='align-items-center mb-3 no-gutters row']");
        private By searchPropertiesName1stPropInput = By.Id("input-properties.0.name");
        private By searchPropertiesDelete1stPropBttn = By.XPath("(//span[contains(text(),'Remove')]/parent::div/parent::button)[1]");

        //Toast Element
        private By duplicatePropToast = By.CssSelector("div[id='duplicate-property'] div[class='Toastify__toast-body']");

        //Warning Message Modal
        private By searchPropertiesModal = By.CssSelector("div[class='modal-content']");

        private SharedModals sharedModals;


        public SharedSearchProperties(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
        }

        public void NavigateToSearchTab()
        {
            WaitUntilClickable(searchByTab);
            webDriver.FindElement(searchByTab).Click();
        }

        public void SelectPropertyByPID(string PID)
        {
            WaitUntilClickable(searchBySelect);
            ChooseSpecificSelectOption(searchBySelect, "PID");

            WaitUntilVisible(searchByPIDInput);
            if (webDriver.FindElement(searchByPIDInput).GetAttribute("value") != "")
            {
                ClearInput(searchByPIDInput);
            }
            webDriver.FindElement(searchByPIDInput).SendKeys(PID);

            FocusAndClick(searchByButton);
        }

        public void SelectPropertyByPIN(string PIN)
        {
            WaitUntilClickable(searchBySelect);
            ChooseSpecificSelectOption(searchBySelect, "PIN");

            WaitUntilVisible(searchByPINInput);
            if (webDriver.FindElement(searchByPINInput).GetAttribute("value") != "")
            {
                ClearInput(searchByPINInput);
            }
            webDriver.FindElement(searchByPINInput).SendKeys(PIN);

            FocusAndClick(searchByButton);
        }

        public void SelectPropertyByAddress(string address)
        {
            Wait();
            ChooseSpecificSelectOption(searchBySelect, "Address");

            WaitUntilVisible(searchByAddressInput);
            if (webDriver.FindElement(searchByAddressInput).GetAttribute("value") != "")
            {
                ClearInput(searchByAddressInput);
            }
            webDriver.FindElement(searchByAddressInput).SendKeys(address);

            WaitUntilVisible(searchByAddressInputSuggestionList);
            FocusAndClick(searchByAddressSuggestion1stOption);

        }

        public void SelectPropertyByPlan(string plan)
        {
            Wait();
            ChooseSpecificSelectOption(searchBySelect, "Plan #");

            WaitUntilVisible(searchByPlanInput);
            if (webDriver.FindElement(searchByPlanInput).GetAttribute("value") != "")
            {
                ClearInput(searchByPlanInput);
            }
            webDriver.FindElement(searchByPlanInput).SendKeys(plan);

            FocusAndClick(searchByButton);
        }

        public void SelectPropertyByLegalDescription(string legalDescription)
        {
            Wait();
            ChooseSpecificSelectOption(searchBySelect, "Legal Description");

            WaitUntilVisible(searchByLegalDescriptionInput);
            if (webDriver.FindElement(searchByLegalDescriptionInput).GetAttribute("value") != "")
            {
                ClearInput(searchByLegalDescriptionInput);
            }
            webDriver.FindElement(searchByLegalDescriptionInput).SendKeys(legalDescription);

            FocusAndClick(searchByButton);
        }

        public void AddNameSelectedProperty(string name)
        {
            WaitUntilVisible(searchPropertiesName1stPropInput);
            webDriver.FindElement(searchPropertiesName1stPropInput).SendKeys(name);
        }

        public void DeleteProperty()
        {
            Wait();
            var PropertiesTotal = webDriver.FindElements(searchPropertiesSelectedPropertiesTotal).Count();

            WaitUntilSpinnerDisappear();
            webDriver.FindElement(searchPropertiesDelete1stPropBttn).Click();

            Wait(2000);
            if (webDriver.FindElements(searchPropertiesModal).Count > 0)
            {
                Assert.True(sharedModals.ModalHeader() == "Removing Property from form");
                Assert.True(sharedModals.ModalContent() == "Are you sure you want to remove this property from this lease/license?");

                sharedModals.ModalClickOKBttn();
            }

            var PropertiesLeft = webDriver.FindElements(searchPropertiesSelectedPropertiesTotal).Count();

            Wait();
            Assert.True(PropertiesTotal - PropertiesLeft == 1);
        }

        public void SelectFirstOption()
        {
            WaitUntilVisible(searchProperties1stResultPropDiv);
            FocusAndClick(searchProperties1stResultPropCheckbox);

            webDriver.FindElement(searchPropertiesAddSelectionBttn).Click();

            
            if (webDriver.FindElements(duplicatePropToast).Count() == 1)
            {
                WaitUntilVisible(duplicatePropToast);
                Assert.True(webDriver.FindElement(duplicatePropToast).Text == "A property that the user is trying to select has already been added to the selected properties list");
            }

            if (webDriver.FindElements(searchPropertiesModal).Count > 0)
            {
                Assert.True(sharedModals.ModalHeader() == "Not inventory property");
                Assert.True(sharedModals.ModalContent() == "You have selected a property not previously in the inventory. Do you want to add this property to the lease?");

                sharedModals.ModalClickOKBttn();
            }
        }

        public string noRowsResultsMessage()
        {
            WaitUntilDisappear(tableLoadingSpinner);
            return webDriver.FindElement(searchPropertiesNoRowsResult).Text;
        }

        public void VerifyLocateOnMapFeature()
        {
            WaitUntilVisible(searchSectionTitle);

            Assert.True(webDriver.FindElement(searchSectionTitle).Displayed);
            Assert.True(webDriver.FindElement(searchSectionInstructions).Displayed);

            Assert.True(webDriver.FindElement(locateOnMapTab).Displayed);
            Assert.True(webDriver.FindElement(searchByTab).Displayed);

            Assert.True(webDriver.FindElement(locateOnMapSubtitle).Displayed);
            Assert.True(webDriver.FindElement(locateOnMapBlueIcon).Displayed);
            Assert.True(webDriver.FindElement(locateOnMapInstuction1).Displayed);
            Assert.True(webDriver.FindElement(locateOnMapInstuction2).Displayed);
            Assert.True(webDriver.FindElement(locateOnMapInstuction3).Displayed);
            Assert.True(webDriver.FindElement(locateOnMapSelectedLabel).Displayed);
            Assert.True(webDriver.FindElement(locateOnMapPIDLabel).Displayed);
            Assert.True(webDriver.FindElement(locateOnMapPlanLabel).Displayed);
            Assert.True(webDriver.FindElement(locateOnMapAddressLabel).Displayed);
            Assert.True(webDriver.FindElement(locateOnMapRegionLabel).Displayed);
            Assert.True(webDriver.FindElement(locateOnMapDistrictLabel).Displayed);
            Assert.True(webDriver.FindElement(locateOnMapLegalDescriptionLabel).Displayed);

            Assert.True(webDriver.FindElement(searchPropertiesSelectedPropertiesSubtitle).Displayed);
            Assert.True(webDriver.FindElement(searchPropertiesSelectedIdentifierHeader).Displayed);
            Assert.True(webDriver.FindElement(searchPropertiesSelectedDescriptiveNameHeader).Displayed);
            Assert.True(webDriver.FindElement(searchPropertiesSelectedToolTipIcon).Displayed);
            if (webDriver.FindElements(searchPropertiesSelectedDefault).Count > 0)
            {
                WaitUntilVisible(searchPropertiesSelectedDefault);
                Assert.True(webDriver.FindElement(searchPropertiesSelectedDefault).Displayed);
            }
        }

        public void VerifySearchPropertiesFeature()
        {
            WaitUntilSpinnerDisappear();

            Assert.True(webDriver.FindElement(searchByTab).Displayed);
            Assert.True(webDriver.FindElement(searchBySubtitle).Displayed);
            Assert.True(webDriver.FindElement(searchBySelect).Displayed);
            Assert.True(webDriver.FindElement(searchByPIDInput).Displayed);
            Assert.True(webDriver.FindElement(searchByButton).Displayed);
            Assert.True(webDriver.FindElement(searchResetButton).Displayed);
            Assert.True(webDriver.FindElement(searchPropertiesResultTableHeader).Displayed);
            Assert.True(webDriver.FindElement(searchPropsResultSelectAllInput).Displayed);
            Assert.True(webDriver.FindElement(searchPropResultsPIDHeader).Displayed);
            Assert.True(webDriver.FindElement(searchPropResultsPINHeader).Displayed);
            Assert.True(webDriver.FindElement(searchPropResultsPlanHeader).Displayed);
            Assert.True(webDriver.FindElement(searchPropResultsAddressHeader).Displayed);
            Assert.True(webDriver.FindElement(searchPropResultsLegalDescriptHeader).Displayed);
        }
        
    }
}
