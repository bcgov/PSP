using OpenQA.Selenium;
using OpenQA.Selenium.Support.Events;
using System.Diagnostics;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SearchProperties : PageObjectBase
    {
        //Search Bar Elements
        private By searchPropertyTypeSelect = By.Id("input-searchBy");
        private By searchPropertyByPIDPINInput = By.Id("input-pinOrPid");
        private By searchPropertyByAddressInput = By.Id("input-address");
        private By searchPropertyAddressSuggestionsGroup = By.CssSelector("div[class='suggestionList']");
        private By searchPropertyAddressSuggestions1stOption = By.CssSelector("div[class='suggestionList'] option:nth-child(1)");
        private By searchPropertySearchBttn = By.Id("search-button");
        private By searchPropertyResetBttn = By.Id("reset-button");

        private By searchPropertyListViewIcon = By.CssSelector("div[class='bar-item col-auto'] div div:nth-child(2)");

        //Map Pin element
        private By searchPropertyFoundPin = By.CssSelector("div[class='leaflet-pane leaflet-marker-pane'] img");

        //Properties List View Elements
        private By searchPropertyListViewTitle = By.XPath("//h3[contains(text(),'Property Information')]");
        private By searchPropertyListHeaderPid = By.XPath("//div[@data-testid='propertiesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'PID')]");
        private By searchPropertyListHeaderPin = By.XPath("//div[@data-testid='propertiesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'PIN')]");
        private By searchPropertyListHeaderAddress = By.XPath("//div[@data-testid='propertiesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Civic Address')]");
        private By searchPropertyListHeaderLocation = By.XPath("//div[@data-testid='propertiesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Location')]");
        private By searchPropertyListHeaderLotSize = By.XPath("//div[@data-testid='propertiesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Lot Size')]");
        private By searchPropertyListContent = By.XPath("//div[@data-testid='propertiesTable']/form/div/div");
        private By searchPropertyListContent1stProp = By.XPath("//div[@data-testid='propertiesTable']/form/div/div[1]");
        private By searchPropertyListContent1stViewTabBttn = By.XPath("//div[@data-testid='propertiesTable']/form/div/div[1]/div/div[6]/div/button[@data-testid='view-prop-tab']");
        private By searchPropertyListContent1stViewWindowBttn = By.XPath("//div[@data-testid='propertiesTable']/form/div/div[1]/div/div[6]/div/button[@data-testid='view-prop-ext']");
        private By searchPropertyListPaginationMenu = By.CssSelector("div[class='Menu-root']");
        private By searchPropertyListPaginationMenuBttn = By.CssSelector("div[class='Menu-button']");
        private By searchPropertyListPaginationMenuItems = By.CssSelector("div[class='Menu-items']");
        private By searchPropertyListPagination = By.CssSelector("ul[class='pagination']");

        private SharedModals sharedModals;

        public SearchProperties(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
        }

        public void SearchPropertyByPINPID(string PID)
        {
            Wait(3000);

            WaitUntilClickable(searchPropertyTypeSelect);
            ChooseSpecificSelectOption(searchPropertyTypeSelect, "PID/PIN");
            ClearInput(searchPropertyByPIDPINInput);
            webDriver.FindElement(searchPropertyByPIDPINInput).SendKeys(PID);
            FocusAndClick(searchPropertySearchBttn);

            WaitUntilSpinnerDisappear();
        }

        public void SearchPropertyByAddress(string Address)
        {
            Wait(3000);

            WaitUntilClickable(searchPropertyTypeSelect);
            ChooseSpecificSelectOption(searchPropertyTypeSelect, "Address");
            webDriver.FindElement(searchPropertyByAddressInput).SendKeys(Address);

            WaitUntilVisible(searchPropertyAddressSuggestionsGroup);
            FocusAndClick(searchPropertyAddressSuggestions1stOption);
            
            webDriver.FindElement(searchPropertySearchBttn).Click();
        }

        public void SearchPropertyReset()
        {
            WaitUntilClickable(searchPropertyResetBttn);
            webDriver.FindElement(searchPropertyResetBttn).Click();
        }

        public void SelectFoundPin()
        {
            Wait(5000);
            FocusAndClick(searchPropertyFoundPin);

            //sharedModals.SiteMinderModal();
        }

        public void NavigatePropertyListView()
        {
            WaitUntilClickable(searchPropertyListViewIcon);
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

            Assert.True(webDriver.FindElement(searchPropertyListViewTitle).Displayed);
            Assert.True(webDriver.FindElement(searchPropertyListHeaderPid).Displayed);
            Assert.True(webDriver.FindElement(searchPropertyListHeaderPin).Displayed);
            Assert.True(webDriver.FindElement(searchPropertyListHeaderAddress).Displayed);
            Assert.True(webDriver.FindElement(searchPropertyListHeaderLocation).Displayed);
            Assert.True(webDriver.FindElement(searchPropertyListHeaderLotSize).Displayed);
            Assert.True(webDriver.FindElement(searchPropertyListContent).Displayed);
            Assert.True(webDriver.FindElement(searchPropertyListContent1stProp).Displayed);
            Assert.True(webDriver.FindElement(searchPropertyListContent1stViewTabBttn).Displayed);
            Assert.True(webDriver.FindElement(searchPropertyListContent1stViewWindowBttn).Displayed);
            Assert.True(webDriver.FindElement(searchPropertyListPaginationMenu).Displayed);
            Assert.True(webDriver.FindElement(searchPropertyListPaginationMenuBttn).Displayed);
            //Assert.True(webDriver.FindElement(searchPropertyListPaginationMenuItems).Displayed);
            Assert.True(webDriver.FindElement(searchPropertyListPagination).Displayed);

        }

        public int PropertiesFoundCount()
        {
            Wait(2000);
            return webDriver.FindElements(searchPropertyFoundPin).Count();
        }
    }
}
