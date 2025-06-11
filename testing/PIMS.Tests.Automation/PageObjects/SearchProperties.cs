using OpenQA.Selenium;
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

        private readonly By searchPropertyListViewIcon = By.CssSelector("button[title='list-view']");

        //Map Pin element
        private readonly By searchPropertyFoundLocationPin = By.XPath("//div[@class='leaflet-pane leaflet-marker-pane']/img[1]");
        private readonly By searchPropertyFoundLocationPopup = By.CssSelector("div[class='leaflet-popup-content']");
        private readonly By searchPropertyFoundCluster = By.CssSelector("div[class='leaflet-marker-icon marker-cluster marker-cluster-small leaflet-zoom-animated leaflet-interactive']");

        //Properties List View Elements
        private readonly By searchPropertyViewByInput = By.Id("properties-selector_input");
        private readonly By searchPropertyViewByInputOptions = By.CssSelector("ul[class='optionContainer']");
        private readonly By searchPropertyViewByFirstOption = By.CssSelector("ul[class='optionContainer'] li:nth-child(1)");

        private readonly By searchPropertyListViewTitle = By.XPath("//h3[contains(text(),'Search Results')]");
        private readonly By searchPropertyViewByLabel = By.XPath("//div/strong[contains(text(),'View by')]");
        private readonly By searchViewByContainer = By.CssSelector("div[id='properties-selector']");
        private readonly By searchPropertyListHeaderPid = By.XPath("//div[@data-testid='propertiesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'PID')]");
        private readonly By searchPropertyListHeaderPin = By.XPath("//div[@data-testid='propertiesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'PIN')]");
        private readonly By searchPropertyListHeaderAddress = By.XPath("//div[@data-testid='propertiesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Civic Address')]");
        private readonly By searchPropertyListHeaderLocation = By.XPath("//div[@data-testid='propertiesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Location')]");
        private readonly By searchPropertyListLocationSortBttn = By.CssSelector("div[data-testid='sort-column-Location']");
        private readonly By searchPropertyListHeaderLotSize = By.XPath("//div[@data-testid='propertiesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Lot Size')]");
        private readonly By searchPropertyListLotSizeSortBttn = By.XPath("//div[@data-testid='propertiesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Lot Size')]/div");
        private readonly By searchPropertyListHeaderOwnership = By.XPath("//div[@data-testid='propertiesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Ownership')]");
        private readonly By searchPropertyListOwnershipSortBttn = By.CssSelector("div[data-testid='sort-column-Ownership']");
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

        private SharedModals sharedModals;

        public SearchProperties(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
        }

        public void NavigateToHomePage()
        {
            Wait();
            webDriver.FindElement(homePageBttn).Click();
        }

        public void SearchPropertyByPID(string PID)
        {
            Wait();

            WaitUntilClickable(searchPropertyTypeSelect);
            ChooseSpecificSelectOption(searchPropertyTypeSelect, "PID");
            ClearInput(searchPropertyByPIDInput);
            webDriver.FindElement(searchPropertyByPIDInput).SendKeys(PID);
            FocusAndClick(searchPropertySearchBttn);

            WaitUntilSpinnerDisappear();
        }

        public void SearchPropertyByPIN(string PIN)
        {
            Wait();

            WaitUntilClickable(searchPropertyTypeSelect);
            ChooseSpecificSelectOption(searchPropertyTypeSelect, "PIN");
            ClearInput(searchPropertyByPINInput);
            webDriver.FindElement(searchPropertyByPINInput).SendKeys(PIN);
            FocusAndClick(searchPropertySearchBttn);

            WaitUntilSpinnerDisappear();
        }

        public void SearchPropertyByAddressMap(string address)
        {
            Wait();

            WaitUntilClickable(searchPropertyTypeSelect);
            ChooseSpecificSelectOption(searchPropertyTypeSelect, "Address");
            webDriver.FindElement(searchPropertyByAddressInput).SendKeys(address);

            WaitUntilVisible(searchPropertyAddressSuggestionsGroup);
            FocusAndClick(searchPropertyAddressSuggestions1stOption);

            WaitUntilClickable(searchPropertySearchBttn);
            webDriver.FindElement(searchPropertySearchBttn).Click();
            WaitUntilSpinnerDisappear();
        }

        public void SearchPropertyByAddressList(string address)
        {
            Wait();

            WaitUntilClickable(searchPropertyTypeSelect);
            ChooseSpecificSelectOption(searchPropertyTypeSelect, "Address");
            webDriver.FindElement(searchPropertyByAddressInput).SendKeys(address);

            webDriver.FindElement(searchPropertySearchBttn).Click();
            WaitUntilTableSpinnerDisappear();
        }

        public void SearchPropertyByPlan(string plan)
        {
            Wait();

            WaitUntilClickable(searchPropertyTypeSelect);
            ChooseSpecificSelectOption(searchPropertyTypeSelect, "Plan #");
            webDriver.FindElement(searchPropertyByPlanInput).SendKeys(plan);

            FocusAndClick(searchPropertySearchBttn);
            WaitUntilSpinnerDisappear();
        }

        public void SearchPropertyByLatLong(PropertyLatitudeLongitude coordinates)
        {
            Wait();

            WaitUntilClickable(searchPropertyTypeSelect);
            ChooseSpecificSelectOption(searchPropertyTypeSelect, "Lat/Long");

            webDriver.FindElement(searchPropertyByLatDegreesInput).SendKeys(coordinates.LatitudeDegree);
            webDriver.FindElement(searchPropertyByLatMinsInput).SendKeys(coordinates.LatitudeMinutes);
            webDriver.FindElement(searchPropertyByLatSecsInput).SendKeys(coordinates.LatitudeSeconds);
            webDriver.FindElement(searchPropertyByLatDirectionSelect).SendKeys(coordinates.LatitudeDirection);

            webDriver.FindElement(searchPropertyByLongDegreesInput).SendKeys(coordinates.LongitudeDegree);
            webDriver.FindElement(searchPropertyByLongMinsInput).SendKeys(coordinates.LongitudeMinutes);
            webDriver.FindElement(searchPropertyByLongSecsInput).SendKeys(coordinates.LongitudeSeconds);
            webDriver.FindElement(searchPropertyByLongDirectionSelect).SendKeys(coordinates.LongitudeDirection);

            WaitUntilClickable(searchPropertySearchBttn);
            webDriver.FindElement(searchPropertySearchBttn).Click();
            WaitUntilSpinnerDisappear();
        }

        public void IncludeAllPropertyOwnershipSearch()
        {
            Wait();
            webDriver.FindElement(searchPropertyViewByInput).Click();

            WaitUntilVisible(searchPropertyViewByInputOptions);
            while (webDriver.FindElements(searchPropertyViewByFirstOption).Count == 1)
                webDriver.FindElement(searchPropertyViewByFirstOption).Click();
        }

        public void SearchPropertyReset()
        {
            Wait();
            WaitUntilClickable(searchPropertyResetBttn);
            webDriver.FindElement(searchPropertyResetBttn).Click();

            WaitUntilSpinnerDisappear();
        }

        public void SelectFoundPin()
        {
            WaitUntilSpinnerDisappear();

            while (webDriver.FindElements(searchPropertyFoundLocationPopup).Count.Equals(0))
                FocusAndClick(searchPropertyFoundCluster);

            FocusAndClick(searchPropertyFoundLocationPopup);
        }

        public void SelectFirstFoundPropertyList()
        {
            WaitUntilTableSpinnerDisappear();
            webDriver.FindElement(searchPropertyListContent1stViewTabBttn).Click();
        }

        public void NavigatePropertyListView()
        {
            Wait(10000);
            webDriver.FindElement(searchPropertyListViewIcon).Click();
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
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(searchPropertyListContent1stPID).Text;
        }

        public string FirstPropertyLocation()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(searchPropertyListContent1stLocation).Text;
        }

        public string FirstPropertyLotSize()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(searchPropertyListContent1stLotSize).Text;
        }

        public string FirstPropertyOwnership()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(searchPropertyListContent1stOwnership).Text;
        }

        public void ValidatePropertyListView()
        {
            WaitUntilVisible(searchPropertyListContent);

            AssertTrueIsDisplayed(searchPropertyListViewTitle);
            AssertTrueIsDisplayed(searchPropertyViewByLabel);
            AssertTrueIsDisplayed(searchViewByContainer);

            AssertTrueIsDisplayed(searchPropertyListHeaderPid);
            AssertTrueIsDisplayed(searchPropertyListHeaderPin);
            AssertTrueIsDisplayed(searchPropertyListHeaderAddress);
            AssertTrueIsDisplayed(searchPropertyListHeaderLocation);
            AssertTrueIsDisplayed(searchPropertyListLocationSortBttn);
            AssertTrueIsDisplayed(searchPropertyListHeaderLotSize);
            AssertTrueIsDisplayed(searchPropertyListLotSizeSortBttn);
            AssertTrueIsDisplayed(searchPropertyListHeaderOwnership);
            AssertTrueIsDisplayed(searchPropertyListOwnershipSortBttn);

            AssertTrueIsDisplayed(searchPropertyListContent);
            AssertTrueIsDisplayed(searchPropertyListContent1stProp);
            AssertTrueIsDisplayed(searchPropertyListContent1stViewTabBttn);
            AssertTrueIsDisplayed(searchPropertyListContent1stViewWindowBttn);

            AssertTrueIsDisplayed(searchPropertyListPaginationMenu);
            AssertTrueIsDisplayed(searchPropertyListPaginationMenuBttn);
            AssertTrueIsDisplayed(searchPropertyListPagination);
        }

        public Boolean PropertiesMapFoundCount()
        {
            Wait();
            return webDriver.FindElements(searchPropertyFoundLocationPopup).Count == 1;
        }

        public int PropertiesPinMapFoundCount()
        {
            Wait();
            return webDriver.FindElements(searchPropertyFoundLocationPin).Count;
        }

        public int PropertiesListFoundCount()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElements(searchPropertyListContent).Count();
        }

        public int PropertiesClustersFoundCount()
        {
            Wait();
            return webDriver.FindElements(searchPropertyFoundCluster).Count();
        }
    }
}
