using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SearchProperties : PageObjectBase
    {
        //Search Bar Elements
        private By searchPropertyTypeSelect = By.Id("input-searchBy");
        private By searchPropertyByPIDPINInput = By.Id("input-pinOrPid");
        private By searchPropertyByAddressInput = By.Id("input-address");
        private By searchPropertyByPlanInput = By.Id("input-planNumber");
        private By searchPropertyAddressSuggestionsGroup = By.CssSelector("div[class='suggestionList']");
        private By searchPropertyAddressSuggestions1stOption = By.CssSelector("div[class='suggestionList'] option:nth-child(1)");
        private By searchPropertySearchBttn = By.Id("search-button");
        private By searchPropertyResetBttn = By.Id("reset-button");

        private By searchPropertyListViewIcon = By.CssSelector("div[class='bar-item col-auto'] div div:nth-child(2)");

        //Map Pin element
        private By searchPropertyFoundPin = By.XPath("//div[@class='leaflet-pane leaflet-marker-pane']/img[1]");
        private By searchPropertyFoundCluster = By.CssSelector("div[class='leaflet-marker-icon marker-cluster marker-cluster-small leaflet-zoom-animated leaflet-interactive']");

        //Properties List View Elements
        private By searchPropertyListViewTitle = By.XPath("//h3[contains(text(),'Search Results')]");
        private By searchPropertyViewByLabel = By.XPath("//div/strong[contains(text(),'View by')]");
        private By searchViewByContainer = By.CssSelector("div[id='properties-selector']");
        private By searchPropertyListHeaderPid = By.XPath("//div[@data-testid='propertiesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'PID')]");
        private By searchPropertyListHeaderPin = By.XPath("//div[@data-testid='propertiesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'PIN')]");
        private By searchPropertyListHeaderAddress = By.XPath("//div[@data-testid='propertiesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Civic Address')]");
        private By searchPropertyListHeaderLocation = By.XPath("//div[@data-testid='propertiesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Location')]");
        private By searchPropertyListLocationSortBttn = By.CssSelector("div[data-testid='sort-column-Location']");
        private By searchPropertyListHeaderLotSize = By.XPath("//div[@data-testid='propertiesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Lot Size')]");
        private By searchPropertyListLotSizeSortBttn = By.XPath("//div[@data-testid='propertiesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Lot Size')]/div");
        private By searchPropertyListHeaderOwnership = By.XPath("//div[@data-testid='propertiesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Ownership')]");
        private By searchPropertyListOwnershipSortBttn = By.CssSelector("div[data-testid='sort-column-Ownership']");
        private By searchPropertyListContent = By.XPath("//div[@data-testid='propertiesTable']/form/div/div");
        private By searchPropertyListContent1stProp = By.XPath("//div[@data-testid='propertiesTable']/form/div/div[1]");
        private By searchPropertyListContent1stViewTabBttn = By.XPath("//div[@data-testid='propertiesTable']/form/div/div[1]/div/div[7]/div/button[@data-testid='view-prop-tab']");
        private By searchPropertyListContent1stViewWindowBttn = By.XPath("//div[@data-testid='propertiesTable']/form/div/div[1]/div/div[7]/div/button[@data-testid='view-prop-ext']");
        private By searchPropertyListPaginationMenu = By.CssSelector("div[class='Menu-root']");
        private By searchPropertyListPaginationMenuBttn = By.CssSelector("div[class='Menu-button']");
        private By searchPropertyListPagination = By.CssSelector("ul[class='pagination']");

        private SharedModals sharedModals;

        public SearchProperties(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
        }

        public void SearchPropertyByPINPID(string PID)
        {
            Wait();

            WaitUntilClickable(searchPropertyTypeSelect);
            ChooseSpecificSelectOption(searchPropertyTypeSelect, "PID/PIN");
            ClearInput(searchPropertyByPIDPINInput);
            webDriver.FindElement(searchPropertyByPIDPINInput).SendKeys(PID);
            FocusAndClick(searchPropertySearchBttn);

            WaitUntilSpinnerDisappear();
        }

        public void SearchPropertyByAddress(string address)
        {
            Wait(3000);

            WaitUntilClickable(searchPropertyTypeSelect);
            ChooseSpecificSelectOption(searchPropertyTypeSelect, "Address");
            webDriver.FindElement(searchPropertyByAddressInput).SendKeys(address);

            WaitUntilVisible(searchPropertyAddressSuggestionsGroup);
            FocusAndClick(searchPropertyAddressSuggestions1stOption);
            
            webDriver.FindElement(searchPropertySearchBttn).Click();
            WaitUntilSpinnerDisappear();
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

        public void SearchPropertyReset()
        {
            WaitUntilClickable(searchPropertyResetBttn);
            webDriver.FindElement(searchPropertyResetBttn).Click();

            WaitUntilSpinnerDisappear();
        }

        public void SelectFoundPin()
        {
            WaitUntilSpinnerDisappear();

            while (webDriver.FindElements(searchPropertyFoundPin).Count.Equals(0))
                FocusAndClick(searchPropertyFoundCluster);

            FocusAndClick(searchPropertyFoundPin);
        }

        public void NavigatePropertyListView()
        {
            Wait(2000);
            webDriver.FindElement(searchPropertyListViewIcon).Click();
        }

        public void ChooseFirstPropertyFromList()
        {
            WaitUntilClickable(searchPropertyListContent1stViewTabBttn);
            webDriver.FindElement(searchPropertyListContent1stViewTabBttn).Click();
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

        public int PropertiesFoundCount()
        {
            Wait(2000);
            return webDriver.FindElements(searchPropertyFoundPin).Count();
        }

        public int PropertiesClustersFoundCount()
        {
            Wait(2000);
            return webDriver.FindElements(searchPropertyFoundCluster).Count();
        }

        
    }
}
