using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SearchProperties : PageObjectBase
    {
        //Homepage Button
        private readonly By homePageBttn = By.CssSelector("div[data-testid='nav-tooltip-mapview'] a");

        //Search Bar Elements
        private readonly By searchPropertyTypeSelect = By.Id("input-searchBy");
        private readonly By searchPropertyByPIDInput = By.Id("input-pid");
        private readonly By searchPropertyByPINInput = By.Id("input-pin");
        private readonly By searchPropertyByAddressInput = By.Id("input-address");
        private readonly By searchPropertyByPlanInput = By.Id("input-planNumber");
        private readonly By searchPropertyByHistoricalFileInput = By.Id("input-historical");
        private readonly By searchPropertyByPOINameInput = By.Id("input-name");
        private readonly By searchSurveyDistricSelect = By.Id("input-district");
        private readonly By searchSurveySectionInput = By.Id("input-section");
        private readonly By searchSurveyTownshipInput = By.Id("input-township");
        private readonly By searchSurveyRangeInput = By.Id("input-range");

        private readonly By searchPropertyByLatDegreesInput = By.Id("number-input-coordinates.latitude.degrees");
        private readonly By searchPropertyByLatMinsInput = By.Id("number-input-coordinates.latitude.minutes");
        private readonly By searchPropertyByLatSecsInput = By.Id("number-input-coordinates.latitude.seconds");
        private readonly By searchPropertyByLatDirectionSelect = By.Id("input-coordinates.latitude.direction");

        private readonly By searchPropertyByLongDegreesInput = By.Id("number-input-coordinates.longitude.degrees");
        private readonly By searchPropertyByLongMinsInput = By.Id("number-input-coordinates.longitude.minutes");
        private readonly By searchPropertyByLongSecsInput = By.Id("number-input-coordinates.longitude.seconds");
        private readonly By searchPropertyByLongDirectionSelect = By.Id("input-coordinates.longitude.direction");

        private readonly By searchPropertyAddressSuggestionsGroup = By.CssSelector("ul[class='suggestionList']");
        private readonly By searchPropertyAddressSuggestions1stOption = By.CssSelector("ul[class='suggestionList'] li:nth-child(1)");
        private readonly By searchPropertySearchBttn = By.Id("search-button");
        private readonly By searchPropertyResetBttn = By.Id("reset-button");

        private readonly By searchPropertyPOINameFirstOption = By.CssSelector("div[data-placement='bottom-start'] ul[class='suggestionList'] li:first-child");

        private readonly By searchPropertyProjectInput = By.Id("typeahead-project");
        private readonly By searchProject1stOption = By.CssSelector("div[id='typeahead-project'] a");

        private readonly By search1stPMBCResult = By.CssSelector("div[data-testid='pmbc-search-results-section'] div[data-testid='search-property-0']");
        private readonly By searchInfoEllipsisBttn = By.CssSelector("div[data-testid='quick-info-header'] button[id='dropdown-ellipsis']");
        private readonly By search2ndPMBCResult = By.XPath("//div[text()='Results (PMBC)']/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div/div[@data-testid='search-property-1']/div[1]/div");
        private readonly By search2ndPMBCResultEllipsisBttn = By.XPath("//div[text()='Results (PMBC)']/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div/div[@data-testid='search-property-1']/div[2]/div/div/button");

        private readonly By search1stPMBCResultCreateResearchOption = By.CssSelector("div[aria-labelledby='dropdown-ellipsis'] a[aria-label='Create Research File']");
        private readonly By search1stPMBCResultCreateAcquisitionOption = By.CssSelector("div[aria-labelledby='dropdown-ellipsis'] a[aria-label='Create Acquisition File']");
        private readonly By search1stPMBCResultCreateManagementOption = By.CssSelector("div[aria-labelledby='dropdown-ellipsis'] a[aria-label='Create Management File']");
        private readonly By search1stPMBCResultCreateLeaseOption = By.CssSelector("div[aria-labelledby='dropdown-ellipsis'] a[aria-label='Create Lease File']");
        private readonly By search1stPMBCResultCreateDispositionOption = By.CssSelector("div[aria-labelledby='dropdown-ellipsis'] a[aria-label='Create Disposition File']");
        private readonly By search1stPMBCResultAddToFileOption = By.CssSelector("div[aria-labelledby='dropdown-ellipsis'] a[aria-label='Add to Open File']");

        private readonly By search1stPIMSResult = By.XPath("//div[text()='Results (PIMS)']/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div/div[@data-testid='search-property-0']/div[1]/div");

        private readonly By searchPropertyMoreOptionsBttn = By.CssSelector("button[data-testid='quick-info-more-options']");
        private readonly By searchPropertyAddToFileOption = By.CssSelector("div[aria-labelledby='dropdown-ellipsis'] a[aria-label='Add to Open File']");

        private readonly By searchPropertyListViewBttn = By.CssSelector("div[data-testid='nav-tooltip-pimspropertylistview']");

        //Map Pin element
        private readonly By searchPropertyFoundLocationPin = By.CssSelector("div[class='leaflet-pane leaflet-marker-pane'] img:first-child");

        //Properties List View Elements
        private readonly By searchPropertyListViewTitle = By.XPath("//h3[contains(text(),'PIMS Property Search')]");
        private readonly By searchPropertyViewByLabel = By.XPath("//label[contains(text(),'Search By')]");

        private readonly By searchPropertyListViewOwnershipLabel = By.XPath("//label[text()='Ownership']");
        private readonly By searchPropertyListViewOwnershipInput = By.Id("ownership-selector");
        private readonly By searchPropertyListOwnershipOptions = By.CssSelector("div[id='ownership-selector'] div[class='optionListContainer displayBlock']");
        private readonly By searchPropertyOwnershipFirstOption = By.CssSelector("div[id='ownership-selector'] div[class='optionListContainer displayBlock'] ul[class='optionContainer'] li:nth-child(1)");
        private readonly By searchPropertyOwnershipDeleteBttns = By.CssSelector("div[id='ownership-selector'] i[class='custom-close']");

        private readonly By searchPropertyListViewTenureCleanupLabel = By.XPath("//label[text()='Tenure Cleanup']");
        private readonly By searchPropertyListViewTenureCleanupInput = By.Id("tenure-cleanup-selector_input");

        private readonly By searchPropertyListHeaderPid = By.XPath("//div[@data-testid='propertiesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'PID')]");
        private readonly By searchPropertyListHeaderPin = By.XPath("//div[@data-testid='propertiesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'PIN')]");
        private readonly By searchPropertyListHeaderHistoricalFile = By.XPath("//div[@data-testid='propertiesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Historical file')]");
        private readonly By searchPropertyListHeaderAddress = By.XPath("//div[@data-testid='propertiesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Civic address')]");
        private readonly By searchPropertyListHeaderLocation = By.XPath("//div[@data-testid='propertiesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Location')]");
        private readonly By searchPropertyListLocationSortBttn = By.CssSelector("div[data-testid='sort-column-Location']");
        private readonly By searchPropertyListHeaderLotSize = By.XPath("//div[@data-testid='propertiesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Lot size')]");
        private readonly By searchPropertyListLotSizeSortBttn = By.XPath("//div[contains(text(),'Lot size')]/div");
        private readonly By searchPropertyListHeaderOwnership = By.XPath("//div[@data-testid='propertiesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Ownership')]");
        private readonly By searchPropertyListOwnershipSortBttn = By.CssSelector("div[data-testid='sort-column-Ownership']");
        private readonly By searchPropertyListHeaderTenureCleanup = By.XPath("//div[@data-testid='propertiesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Tenure cleanup')]");
        private readonly By searchPropertyListContent = By.CssSelector("div[data-testid='propertiesTable'] div[class='tbody'] div[class='tr-wrapper']");
        private readonly By searchPropertyListContent1stProp = By.CssSelector("div[data-testid='propertiesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child");
        private readonly By searchPropertyListContent1stPID = By.CssSelector("div[data-testid='propertiesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div[role='cell']:nth-child(1)");
        private readonly By searchPropertyListContent1stLocation = By.CssSelector("div[data-testid='propertiesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div[role='cell']:nth-child(5)");
        private readonly By searchPropertyListContent1stLotSize = By.CssSelector("div[data-testid='propertiesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div[role='cell']:nth-child(6)");
        private readonly By searchPropertyListContent1stOwnership = By.CssSelector("div[data-testid='propertiesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div[role='cell']:nth-child(7)");
        private readonly By searchPropertyListContent1stViewTabBttn = By.CssSelector("div[data-testid='propertiesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child button[data-testid='view-prop-tab']");
        private readonly By searchPropertyListContent1stViewWindowBttn = By.CssSelector("div[data-testid='propertiesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child button[data-testid='view-prop-ext']");
        private readonly By searchPropertyListPaginationMenu = By.CssSelector("div[class='Menu-root']");
        private readonly By searchPropertyListPaginationMenuBttn = By.CssSelector("div[class='Menu-button']");
        private readonly By searchPropertyListPagination = By.CssSelector("ul[class='pagination']");

        private readonly By searchPropertyConfirmationModal = By.CssSelector("div[class='modal-content']");

        private readonly SharedModals sharedModals;

        public SearchProperties(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
        }

        public void NavigateToHomePage()
        {
            WaitUntilClickable(homePageBttn);
            webDriver.FindElement(homePageBttn).Click();
        }

        public void SearchProperty(string PID = "", string PIN = "", string address = "", string plan = "", string historicFile = "", string POIName = "",
            PropertyLatitudeLongitude? coordinates = null, SurveyParcel? surveyParcel = null, string project = "")
        {
            WaitUntilVisible(searchPropertyTypeSelect);

            if (PID != "")
            {
                ChooseSelectOption(searchPropertyTypeSelect, "PID");
                ClearInput(searchPropertyByPIDInput);
                webDriver.FindElement(searchPropertyByPIDInput).SendKeys(PID);
            }

            if (PIN != "")
            {
                ChooseSelectOption(searchPropertyTypeSelect, "PIN");
                ClearInput(searchPropertyByPINInput);
                webDriver.FindElement(searchPropertyByPINInput).SendKeys(PIN);
            }

            if (address != "")
            {
                ChooseSelectOption(searchPropertyTypeSelect, "Address");
                webDriver.FindElement(searchPropertyByAddressInput).SendKeys(address);

                Wait();
                SafeClick(searchPropertyAddressSuggestions1stOption);
            }

            if (plan != "")
            {
                ChooseSelectOption(searchPropertyTypeSelect, "Plan #");
                webDriver.FindElement(searchPropertyByPlanInput).SendKeys(plan);
            }

            if (historicFile != "")
            {
                ChooseSelectOption(searchPropertyTypeSelect, "Historical File #");
                webDriver.FindElement(searchPropertyByHistoricalFileInput).SendKeys(historicFile);
                
            }

            if (POIName != "")
            {
                ChooseSelectOption(searchPropertyTypeSelect, "POI Name");
                webDriver.FindElement(searchPropertyByPOINameInput).SendKeys(POIName);

                WaitUntilVisible(searchPropertyPOINameFirstOption);
                webDriver.FindElement(searchPropertyPOINameFirstOption).Click();
            }

            if (coordinates != null)
            {
                ChooseSelectOption(searchPropertyTypeSelect, "Lat/Long");

                webDriver.FindElement(searchPropertyByLatDegreesInput).SendKeys(coordinates.LatitudeDegree);
                webDriver.FindElement(searchPropertyByLatMinsInput).SendKeys(coordinates.LatitudeMinutes);
                webDriver.FindElement(searchPropertyByLatSecsInput).SendKeys(coordinates.LatitudeSeconds);
                webDriver.FindElement(searchPropertyByLatDirectionSelect).SendKeys(coordinates.LatitudeDirection);

                webDriver.FindElement(searchPropertyByLongDegreesInput).SendKeys(coordinates.LongitudeDegree);
                webDriver.FindElement(searchPropertyByLongMinsInput).SendKeys(coordinates.LongitudeMinutes);
                webDriver.FindElement(searchPropertyByLongSecsInput).SendKeys(coordinates.LongitudeSeconds);
                webDriver.FindElement(searchPropertyByLongDirectionSelect).SendKeys(coordinates.LongitudeDirection);
            }

            if (surveyParcel != null)
            {
                ChooseSelectOption(searchPropertyTypeSelect, "Survey Parcel");
                ChooseSelectOption(searchSurveyDistricSelect, surveyParcel.District);
                webDriver.FindElement(searchSurveySectionInput).SendKeys(surveyParcel.Section);
                webDriver.FindElement(searchSurveyTownshipInput).SendKeys(surveyParcel.Township);
                webDriver.FindElement(searchSurveyRangeInput).SendKeys(surveyParcel.Range);
            }

            if (project != "")
            {
                ChooseSelectOption(searchPropertyTypeSelect, "Project");

                webDriver.FindElement(searchPropertyProjectInput).SendKeys(project);
                webDriver.FindElement(searchPropertyProjectInput).SendKeys(Keys.Space);
                webDriver.FindElement(searchPropertyProjectInput).SendKeys(Keys.Backspace);

                WaitUntilClickable(searchProject1stOption);
                FocusAndClick(searchProject1stOption);
            }

            SafeClick(searchPropertySearchBttn);
            WaitUntilSpinnerDisappear();
        }

        public void SearchPropertyByAddressList(string address)
        {
            Wait();

            WaitUntilClickable(searchPropertyTypeSelect);
            ChooseSelectOption(searchPropertyTypeSelect, "Address");
            webDriver.FindElement(searchPropertyByAddressInput).SendKeys(address);

            webDriver.FindElement(searchPropertySearchBttn).Click();
            WaitForTableToLoad();
        }

        public void SelectOwnershipSearch(string ownership)
        {
            while (webDriver.FindElements(searchPropertyOwnershipDeleteBttns).Count > 0)
                webDriver.FindElements(searchPropertyOwnershipDeleteBttns)[0].Click();

            ChooseMultiSelectOption(searchPropertyListViewOwnershipInput, searchPropertyListOwnershipOptions, searchPropertyListHeaderAddress, ownership);
        }

        public void ResetPropertySearch()
        {
            WaitUntilClickable(searchPropertyResetBttn);
            webDriver.FindElement(searchPropertyResetBttn).Click();

            WaitUntilSpinnerDisappear();
        }

        public void SelectFound1stPropFromMap()
        {
            WaitUntilClickable(searchPropertyFoundLocationPin);
            webDriver.FindElement(searchPropertyFoundLocationPin).Click();
        }

        public void SelectFound1stPropAddToFile()
        {
            WaitUntilClickable(searchPropertyFoundLocationPin);
            FocusAndClick(searchPropertyFoundLocationPin);

            WaitUntilClickable(searchPropertyMoreOptionsBttn);
            SafeClick(searchPropertyMoreOptionsBttn);

            WaitUntilVisible(searchPropertyAddToFileOption);
            webDriver.FindElement(searchPropertyAddToFileOption).Click();

            if (webDriver.FindElements(searchPropertyConfirmationModal).Count > 0 && sharedModals.ModalContent().Contains("You have selected a property not previously in the inventory"))
            {
                Assert.Equal("Not inventory property", sharedModals.ModalHeader());
                Assert.Contains("You have selected a property not previously in the inventory. Do you want to add this property to the lease?", sharedModals.ModalContent());
                sharedModals.ModalClickOKBttn();
            }
        }

        public void SelectFirstFoundPropertyList()
        {
            WaitForTableToLoad();
            webDriver.FindElement(searchPropertyListContent1stViewTabBttn).Click();
        }

        public void SelectFirstPMBCResult(string action = "")
        {
            Wait();
            webDriver.FindElement(search1stPMBCResult).Click();

            WaitUntilClickable(searchInfoEllipsisBttn);
            SafeClick(searchInfoEllipsisBttn);

            switch (action)
            {
                case "Create Research":
                    webDriver.FindElement(search1stPMBCResultCreateResearchOption).Click();
                    break;
                case "Create Acquisition":
                    webDriver.FindElement(search1stPMBCResultCreateAcquisitionOption).Click();
                    break;
                case "Create Management":
                    webDriver.FindElement(search1stPMBCResultCreateManagementOption).Click();
                    break;
                case "Create Lease":
                    webDriver.FindElement(search1stPMBCResultCreateLeaseOption).Click();
                    break;
                case "Create Disposition":
                    webDriver.FindElement(search1stPMBCResultCreateDispositionOption).Click();
                    break;
                default:
                    webDriver.FindElement(search1stPMBCResultAddToFileOption).Click();
                    break;
            }

            Wait();
            if (webDriver.FindElements(searchPropertyConfirmationModal).Count > 0 && sharedModals.ModalContent().Contains("You have selected a property not previously in the inventory"))
            {
                Assert.Equal("Not inventory property", sharedModals.ModalHeader());
                Assert.Contains("You have selected a property not previously in the inventory. Do you want to add this property to the lease?", sharedModals.ModalContent());
                sharedModals.ModalClickOKBttn();
            }
        }

        public void SelectSecondPMBCResult(string action = "")
        {
            Wait();
            webDriver.FindElement(search2ndPMBCResult).Click();

            WaitUntilClickable(searchInfoEllipsisBttn);
            SafeClick(searchInfoEllipsisBttn);

            switch (action)
            {
                case "Create Research":
                    webDriver.FindElement(search1stPMBCResultCreateResearchOption).Click();
                    break;
                case "Create Acquisition":
                    webDriver.FindElement(search1stPMBCResultCreateAcquisitionOption).Click();
                    break;
                case "Create Management":
                    webDriver.FindElement(search1stPMBCResultCreateManagementOption).Click();
                    break;
                case "Create Lease":
                    webDriver.FindElement(search1stPMBCResultCreateLeaseOption).Click();
                    break;
                case "Create Disposition":
                    webDriver.FindElement(search1stPMBCResultCreateDispositionOption).Click();
                    break;
                default:
                    webDriver.FindElement(search1stPMBCResultAddToFileOption).Click();
                    break;
            }
        }

        public void SelectFirstPIMSResult()
        {
           SafeClick(search1stPIMSResult);
        }

        public void NavigatePropertyListView()
        {
            SafeClick(searchPropertyListViewBttn);
        }

        public void ChooseFirstPropertyFromList()
        {
            WaitUntilClickable(searchPropertyListContent1stViewTabBttn);
            webDriver.FindElement(searchPropertyListContent1stViewTabBttn).Click();
        }

        public void OrderByPropertyLocation()
        {
            WaitUntilClickable(searchPropertyListLocationSortBttn);
            webDriver.FindElement(searchPropertyListLocationSortBttn).Click();
        }

        public void OrderByPropertyLotSize()
        {
            WaitUntilClickable(searchPropertyListLotSizeSortBttn);
            FocusAndClick(searchPropertyListLotSizeSortBttn);
        }

        public void OrderByPropertyOwnership()
        {
            WaitUntilClickable(searchPropertyListOwnershipSortBttn);
            webDriver.FindElement(searchPropertyListOwnershipSortBttn).Click();
        }

        public string FirstPropertyPID()
        {
            WaitForTableToLoad();
            return webDriver.FindElement(searchPropertyListContent1stPID).Text;
        }

        public string FirstPropertyLocation()
        {
            WaitForTableToLoad();
            return webDriver.FindElement(searchPropertyListContent1stLocation).Text;
        }

        public string FirstPropertyLotSize()
        {
            WaitForTableToLoad();
            return webDriver.FindElement(searchPropertyListContent1stLotSize).Text;
        }

        public string FirstPropertyOwnership()
        {
            WaitForTableToLoad();
            return webDriver.FindElement(searchPropertyListContent1stOwnership).Text;
        }

        public void ValidatePropertyListView()
        {
            WaitUntilVisible(searchPropertyListContent);

            AssertTrueIsDisplayed(searchPropertyListViewTitle);
            AssertTrueIsDisplayed(searchPropertyViewByLabel);

            AssertTrueIsDisplayed(searchPropertyTypeSelect);
            AssertTrueIsDisplayed(searchPropertyByPIDInput);
            AssertTrueIsDisplayed(searchPropertySearchBttn);
            AssertTrueIsDisplayed(searchPropertyResetBttn);

            AssertTrueIsDisplayed(searchPropertyListViewOwnershipLabel);
            AssertTrueIsDisplayed(searchPropertyListViewOwnershipInput);
            AssertTrueIsDisplayed(searchPropertyListViewTenureCleanupLabel);
            AssertTrueIsDisplayed(searchPropertyListViewTenureCleanupInput);

            AssertTrueIsDisplayed(searchPropertyListHeaderPid);
            AssertTrueIsDisplayed(searchPropertyListHeaderPin);
            AssertTrueIsDisplayed(searchPropertyListHeaderHistoricalFile);
            AssertTrueIsDisplayed(searchPropertyListHeaderAddress);
            AssertTrueIsDisplayed(searchPropertyListHeaderLocation);
            AssertTrueIsDisplayed(searchPropertyListLocationSortBttn);
            AssertTrueIsDisplayed(searchPropertyListHeaderLotSize);
            AssertTrueIsDisplayed(searchPropertyListLotSizeSortBttn);
            AssertTrueIsDisplayed(searchPropertyListHeaderOwnership);
            AssertTrueIsDisplayed(searchPropertyListHeaderTenureCleanup);
            AssertTrueIsDisplayed(searchPropertyListOwnershipSortBttn);

            AssertTrueIsDisplayed(searchPropertyListContent);
            AssertTrueIsDisplayed(searchPropertyListContent1stProp);
            AssertTrueIsDisplayed(searchPropertyListContent1stViewTabBttn);
            AssertTrueIsDisplayed(searchPropertyListContent1stViewWindowBttn);

            AssertTrueIsDisplayed(searchPropertyListPaginationMenu);
            AssertTrueIsDisplayed(searchPropertyListPaginationMenuBttn);
            AssertTrueIsDisplayed(searchPropertyListPagination);
        }

        public int PropertiesPinMapFoundCount()
        {
            WaitUntilVisible(searchPropertyFoundLocationPin);
            return webDriver.FindElements(searchPropertyFoundLocationPin).Count;
        }

        public int PropertiesListFoundCount()
        {
            WaitForTableToLoad();
            return webDriver.FindElements(searchPropertyListContent).Count();
        }

        public void NoPropertiesFound()
        {
            Assert.True(sharedModals.ToastifyMainText() == "No search results found");
        }

        public void DuplicatePropertyInserted()
        {
            WaitUntilClickable(search1stPMBCResult);
            SafeClick(search1stPMBCResult);

            WaitUntilClickable(searchInfoEllipsisBttn);
            SafeClick(searchInfoEllipsisBttn);

            WaitUntilClickable(search1stPMBCResultAddToFileOption);
            SafeClick(search1stPMBCResultAddToFileOption);

            Wait();
            Assert.Equal("Skipped 1 duplicate property(s).", sharedModals.ToastifyMainText());
        }
    }
}
