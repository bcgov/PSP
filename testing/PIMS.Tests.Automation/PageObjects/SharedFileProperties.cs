using OpenQA.Selenium;


namespace PIMS.Tests.Automation.PageObjects
{
    public class SharedFileProperties : PageObjectBase
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
        private By searchPropertiesPropertiesInFileTotal = By.CssSelector("div[class='align-items-center mb-3 no-gutters row']");

        //File - Edit Properties button
        private By fileEditPropertiesBttn = By.CssSelector("button[title='Change properties']");

        //File - Properties Elements
        private By acquisitionProperty1stPropLink = By.CssSelector("div[data-testid='menu-item-row-1'] div:nth-child(3)");

        //File Confirmation Modal Elements
        private By propertiesFileConfirmationModal = By.CssSelector("div[class='modal-content']");
        //private By propertiesFileMOTIInventoryModal = By.XPath("//div[@role='dialog'][2]/div/div/div[contains(text(),'You have added one or more properties to the disposition file that are not in the MoTI Inventory. Do you want to proceed?')]");

        //Toast Element
        private By duplicatePropToast = By.CssSelector("div[id='duplicate-property'] div[class='Toastify__toast-body']");

        private SharedModals sharedModals;

        public SharedFileProperties(IWebDriver webDriver) : base(webDriver)
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

        public void AddNameSelectedProperty(string name, int index)
        {
            WaitUntilVisible(By.Id("input-properties."+ index +".name"));
            webDriver.FindElement(By.Id("input-properties."+ index +".name")).SendKeys(name);
        }

        public void SelectFirstOptionFromSearch()
        {
            Wait();
            FocusAndClick(searchProperties1stResultPropCheckbox);

            webDriver.FindElement(searchPropertiesAddSelectionBttn).Click();


            Wait(2000);
            if (webDriver.FindElements(duplicatePropToast).Count() == 1)
            {
                WaitUntilVisible(duplicatePropToast);
                Assert.Equal("A property that the user is trying to select has already been added to the selected properties list", webDriver.FindElement(duplicatePropToast).Text);
            }

            if (webDriver.FindElements(propertiesFileConfirmationModal).Count > 0)
            {
                Assert.Equal("Not inventory property", sharedModals.ModalHeader());
                Assert.Equal("You have selected a property not previously in the inventory. Do you want to add this property to the lease?", sharedModals.ModalContent());

                sharedModals.ModalClickOKBttn();
            }
        }

        public string noRowsResultsMessageFromSearch()
        {
            WaitUntilDisappear(tableLoadingSpinner);
            return webDriver.FindElement(searchPropertiesNoRowsResult).Text;
        }

        public void VerifyLocateOnMapFeature()
        {
            WaitUntilVisible(searchSectionTitle);

            AssertTrueIsDisplayed(searchSectionTitle);
            AssertTrueIsDisplayed(searchSectionInstructions);

            AssertTrueIsDisplayed(locateOnMapTab);
            AssertTrueIsDisplayed(searchByTab);

            AssertTrueIsDisplayed(locateOnMapSubtitle);
            AssertTrueIsDisplayed(locateOnMapBlueIcon);
            AssertTrueIsDisplayed(locateOnMapInstuction1);
            AssertTrueIsDisplayed(locateOnMapInstuction2);
            AssertTrueIsDisplayed(locateOnMapInstuction3);
            AssertTrueIsDisplayed(locateOnMapSelectedLabel);
            AssertTrueIsDisplayed(locateOnMapPIDLabel);
            AssertTrueIsDisplayed(locateOnMapPlanLabel);
            AssertTrueIsDisplayed(locateOnMapAddressLabel);
            AssertTrueIsDisplayed(locateOnMapRegionLabel);
            AssertTrueIsDisplayed(locateOnMapDistrictLabel);
            //AssertTrueIsDisplayed(locateOnMapLegalDescriptionLabel);

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

        public void VerifySearchPropertiesFeature()
        {
            WaitUntilSpinnerDisappear();

            AssertTrueIsDisplayed(searchByTab);
            AssertTrueIsDisplayed(searchBySubtitle);
            AssertTrueIsDisplayed(searchBySelect);
            AssertTrueIsDisplayed(searchByPIDInput);
            AssertTrueIsDisplayed(searchByButton);
            AssertTrueIsDisplayed(searchResetButton);
            AssertTrueIsDisplayed(searchPropertiesResultTableHeader);
            AssertTrueIsDisplayed(searchPropsResultSelectAllInput);
            AssertTrueIsDisplayed(searchPropResultsPIDHeader);
            AssertTrueIsDisplayed(searchPropResultsPINHeader);
            AssertTrueIsDisplayed(searchPropResultsPlanHeader);
            AssertTrueIsDisplayed(searchPropResultsAddressHeader);
            //AssertTrueIsDisplayed(searchPropResultsLegalDescriptHeader);
        }

        public void NavigateToAddPropertiesToFile()
        {
            WaitUntilVisible(fileEditPropertiesBttn);
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
            By chosenProperty = By.CssSelector("div[data-testid='menu-item-row-"+ elementOrder +"'] div:nth-child(3)");

            WaitUntilClickable(chosenProperty);
            webDriver.FindElement(chosenProperty).Click();
        }

        public void DeleteLastPropertyFromFile()
        {
            Wait();
            var propertyIndex = webDriver.FindElements(searchPropertiesPropertiesInFileTotal).Count();

            WaitUntilClickable(By.XPath("//h2/div/div[contains(text(),'Selected properties')]/parent::div/parent::h2/following-sibling::div/div[@class='align-items-center mb-3 no-gutters row']["+ propertyIndex +"]/div[3]/button"));
            webDriver.FindElement(By.XPath("//h2/div/div[contains(text(),'Selected properties')]/parent::div/parent::h2/following-sibling::div/div[@class='align-items-center mb-3 no-gutters row']["+ propertyIndex +"]/div[3]/button")).Click();

            Wait(2000);
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
            var propertyIndex = webDriver.FindElements(searchPropertiesPropertiesInFileTotal).Count();

            WaitUntilClickable(By.XPath("//h2/div/div[contains(text(),'Selected properties')]/parent::div/parent::h2/following-sibling::div/div[@class='align-items-center mb-3 no-gutters row']["+ propertyIndex +"]/div[4]/button"));
            webDriver.FindElement(By.XPath("//h2/div/div[contains(text(),'Selected properties')]/parent::div/parent::h2/following-sibling::div/div[@class='align-items-center mb-3 no-gutters row']["+ propertyIndex +"]/div[4]/button")).Click();

            Wait(2000);
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
                Assert.Equal("User Override Required", sharedModals.SecondaryModalHeader());
                if (sharedModals.SecondaryModalContent().Contains("You have added one or more properties to the disposition file that are not in the MoTI Inventory"))
                {
                    Assert.Contains("You have added one or more properties to the disposition file that are not in the MoTI Inventory. Do you want to proceed?", sharedModals.SecondaryModalContent());
                }
                else
                {
                    Assert.Contains("The selected property already exists in the system's inventory. However, the record is missing spatial details.", sharedModals.SecondaryModalContent());
                    Assert.Contains("To add the property, the spatial details for this property will need to be updated. The system will attempt to update the property record with spatial information from the current selection.", sharedModals.SecondaryModalContent());
                }
                sharedModals.SecondaryModalClickOKBttn();
                Wait();
            }
        }
    }
}
