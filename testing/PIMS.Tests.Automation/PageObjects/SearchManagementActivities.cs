
using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SearchManagementActivities : PageObjectBase
    {
        //Main Menu Elements
        private readonly By managementMainMenu = By.XPath("//div[@data-testid= 'nav-tooltip-management']");
        private readonly By managementMainMenuActsListViewLink = By.XPath("//a[contains(text(),'Manage Management Activities')]");

        //Management Activities List View - Search Section Elements
        private readonly By searchManagementActivityTitle = By.XPath("//h1/div/div/span[contains(text(),'Management Activities')]");
        private readonly By managementActListSearchByLabel = By.XPath("//strong[contains(text(),'Search by')]");
        private readonly By managementActListSearchBySelect = By.Id("input-searchBy");
        private readonly By managementActListSearchByAddressInput = By.Id("input-address");
        private readonly By managementActListSearchByPIDInput = By.Id("input-pid");
        private readonly By managementActListSearchByPINInput = By.Id("input-pin");
        private readonly By managementActListSearchByNameInput = By.Id("input-fileNameOrNumberOrReference");
        private readonly By managementActListSearchByActivityStatusSelect = By.Id("input-activityStatusCode");
        private readonly By managementActListSearchByActivityType = By.Id("input-activityTypeCode");
        private readonly By managementActListSearchByProjectInput = By.Id("input-projectNameOrNumber");
        private readonly By managementActListSearchByFileStatusSelect = By.Id("input-managementFileStatusCode");
        private readonly By managementActListSearchByPurposeSelect = By.Id("input-managementFilePurposeCode");
        private readonly By managementActListSearchButton = By.Id("search-button");
        private readonly By managementActListResetButton = By.Id("reset-button");

        //Management Activities List - Table Elements
        private readonly By managementActListViewMgmtDescriptionColumnHeader = By.CssSelector("div[data-testid='managementActivitiesTable'] div[class='thead thead-light'] div[role='columnheader']:nth-child(1)");
        private readonly By managementActListViewOrderByDescription = By.CssSelector("div[data-testid='sort-column-description']");
        private readonly By managementActListViewFileNameColumnHeader = By.XPath("div[data-testid='managementActivitiesTable'] div[class='thead thead-light'] div[role='columnheader']:nth-child(2)");
        private readonly By managementActListViewOrderByFileName = By.CssSelector("div[data-testid='sort-column-fileName']");
        private readonly By managementActListViewHistFileColumnHeader = By.XPath("div[data-testid='managementActivitiesTable'] div[class='thead thead-light'] div[role='columnheader']:nth-child(3)");
        private readonly By managementActListViewOrderByHistFile = By.CssSelector("div[data-testid='sort-column-legacyFileNum']");
        private readonly By managementActListViewAddressColumnHeader = By.XPath("div[data-testid='managementActivitiesTable'] div[class='thead thead-light'] div[role='columnheader']:nth-child(4)");
        private readonly By managementActListViewTypeColumnHeader = By.XPath("div[data-testid='managementActivitiesTable'] div[class='thead thead-light'] div[role='columnheader']:nth-child(5)");
        private readonly By managementActListViewOrderByType = By.CssSelector("div[data-testid='sort-column-activityType']");
        private readonly By managementActListViewSubtypeHeader = By.XPath("div[data-testid='managementActivitiesTable'] div[class='thead thead-light'] div[role='columnheader']:nth-child(6)");
        private readonly By managementActListViewStatusColumnHeader = By.XPath("div[data-testid='managementActivitiesTable'] div[class='thead thead-light'] div[role='columnheader']:nth-child(7)");
        private readonly By managementActListViewOrderByStatus = By.CssSelector("div[data-testid='sort-column-activityStatus']");

        private readonly By managementActListViewDescription1stRecord = By.CssSelector("div[data-testid='managementActivitiesTable'] div[class='tbody'] div[role='cell']:nth-child(1) a");
        private readonly By managementActListViewFileName1stRecord = By.CssSelector("div[data-testid='managementActivitiesTable'] div[class='tbody'] div[role='cell']:nth-child(2)");
        private readonly By managementActListViewHistFile1stRecord = By.CssSelector("div[data-testid='managementActivitiesTable'] div[class='tbody'] div[role='cell']:nth-child(3)");
        private readonly By managementActListViewAddress1stRecord = By.CssSelector("div[data-testid='managementActivitiesTable'] div[class='tbody'] div[role='cell']:nth-child(4) > div");
        private readonly By managementActListViewType1stRecord = By.CssSelector("div[data-testid='managementActivitiesTable'] div[class='tbody'] div[role='cell']:nth-child(5)");
        private readonly By managementActListViewSubtype1stRecord = By.CssSelector("div[data-testid='managementActivitiesTable'] div[class='tbody'] div[role='cell']:nth-child(6)");
        private readonly By managementActListViewStatus1stRecord = By.CssSelector("div[data-testid='managementActivitiesTable'] div[class='tbody'] div[role='cell']:nth-child(7)");

        private readonly By managementActivitiesResultsTable = By.CssSelector("div[data-testid='managementActivitiesTable'] div[class='tbody'] div[class='tr-wrapper']");

        //Search Management Pagination
        private readonly By searchMgmtPaginationMenu = By.CssSelector("div[class='Menu-root']");
        private readonly By searchMgmtPaginationList = By.CssSelector("ul[class='pagination']");

        public SearchManagementActivities(IWebDriver webDriver) : base(webDriver)
        {}

        public void NavigateToSearchManagementActivities()
        {
            WaitUntilClickable(managementMainMenu);
            FocusAndClick(managementMainMenu);

            WaitUntilClickable(managementMainMenuActsListViewLink);
            FocusAndClick(managementMainMenuActsListViewLink);
        }

        public void OrderByMgmtActDescription()
        {
            WaitUntilClickable(managementActListViewOrderByDescription);
            webDriver.FindElement(managementActListViewOrderByDescription).Click();
        }

        public void OrderByMgmtActFileName()
        {
            WaitUntilClickable(managementActListViewOrderByFileName);
            webDriver.FindElement(managementActListViewOrderByFileName).Click();
        }

        public void OrderByMgmtActHistoricalFileNbr()
        {
            WaitUntilClickable(managementActListViewOrderByHistFile);
            webDriver.FindElement(managementActListViewOrderByHistFile).Click();
        }

        public void OrderByMgmtActTypeNbr()
        {
            WaitUntilClickable(managementActListViewOrderByType);
            webDriver.FindElement(managementActListViewOrderByType).Click();
        }

        public void OrderByMgmtActStatus()
        {
            WaitUntilClickable(managementActListViewOrderByStatus);
            webDriver.FindElement(managementActListViewOrderByStatus).Click();
        }

        public string FirstMgmtActDescription()
        {
            WaitForTableToLoad();
            return webDriver.FindElement(managementActListViewDescription1stRecord).Text;
        }

        public string FirstMgmtActFileName()
        {
            WaitForTableToLoad();
            return webDriver.FindElement(managementActListViewFileName1stRecord).Text;
        }

        public string FirstMgmtActHistoricalFile()
        {
            WaitForTableToLoad();
            return webDriver.FindElement(managementActListViewHistFile1stRecord).Text;
        }

        public string FirstMgmtActType()
        {
            WaitForTableToLoad();
            return webDriver.FindElement(managementActListViewType1stRecord).Text;
        }

        public string FirstMgmtActStatus()
        {
            WaitForTableToLoad();
            return webDriver.FindElement(managementActListViewStatus1stRecord).Text;
        }

        public void FilterManagementActivities(string pid = "", string pin = "", string address = "", string mgmtfile = "", string activityStatus = "",
            string activityType = "", string project = "", string mgmtFileStatus = "", string purpose = "")
        {
            WaitUntilClickable(managementActListResetButton);
            webDriver.FindElement(managementActListResetButton).Click();

            if (pid != "")
            {
                WaitUntilClickable(managementActListSearchBySelect);
                ChooseSelectOption(managementActListSearchBySelect, "PID");
                webDriver.FindElement(managementActListSearchByPIDInput).SendKeys(pid);
            }

            if (pin != "")
            {
                WaitUntilClickable(managementActListSearchBySelect);
                ChooseSelectOption(managementActListSearchBySelect, "PIN");
                webDriver.FindElement(managementActListSearchByPINInput).SendKeys(pin);
            }

            if (address != "")
            {
                WaitUntilClickable(managementActListSearchBySelect);
                ChooseSelectOption(managementActListSearchBySelect, "Address");
                webDriver.FindElement(managementActListSearchByAddressInput).SendKeys(address);
            }

            if (mgmtfile != "")
            {
                WaitUntilClickable(managementActListSearchByNameInput);
                webDriver.FindElement(managementActListSearchByNameInput).SendKeys(mgmtfile);
            }

            if (activityStatus != "")
            {
                WaitUntilClickable(managementActListSearchByActivityStatusSelect);
                ChooseSelectOption(managementActListSearchByActivityStatusSelect, activityStatus);
            }

            if (activityType != "")
            {
                WaitUntilClickable(managementActListSearchByActivityType);
                ChooseSelectOption(managementActListSearchByActivityType, activityType);
            }

            if (project != "")
            {
                WaitUntilClickable(managementActListSearchByProjectInput);
                webDriver.FindElement(managementActListSearchByProjectInput).SendKeys(project);
            }

            if (mgmtFileStatus != "")
            {
                WaitUntilClickable(managementActListSearchByFileStatusSelect);
                ChooseSelectOption(managementActListSearchByFileStatusSelect, mgmtFileStatus);
            }

            if (purpose != "")
            {
                WaitUntilClickable(managementActListSearchByPurposeSelect);
                ChooseSelectOption(managementActListSearchByPurposeSelect, purpose);
            }

            WaitUntilClickable(managementActListSearchButton);
            FocusAndClick(managementActListSearchButton);
        }

        public Boolean SearchFoundResults()
        {
            WaitUntilVisible(managementActListViewDescription1stRecord);
            return webDriver.FindElements(managementActListViewDescription1stRecord).Count > 0;
        }

        public int MgmtActivitiesTableResultNumber()
        {
            WaitForTableToLoad();
            return webDriver.FindElements(managementActivitiesResultsTable).Count;
        }

        public void VerifySearchManagementActivitiesListView()
        {
            WaitUntilVisible(managementActListSearchByLabel);

            //Search Management Activities Title
            AssertTrueIsDisplayed(searchManagementActivityTitle);

            //Search Management Activities Filters
            AssertTrueIsDisplayed(managementActListSearchByLabel);
            AssertTrueIsDisplayed(managementActListSearchBySelect);
            AssertTrueIsDisplayed(managementActListSearchByAddressInput);
            AssertTrueIsDisplayed(managementActListSearchByNameInput);
            AssertTrueIsDisplayed(managementActListSearchByActivityStatusSelect);
            AssertTrueIsDisplayed(managementActListSearchByActivityType);
            AssertTrueIsDisplayed(managementActListSearchByProjectInput);
            AssertTrueIsDisplayed(managementActListSearchByFileStatusSelect);
            AssertTrueIsDisplayed(managementActListSearchByPurposeSelect);
            AssertTrueIsDisplayed(managementActListSearchButton);
            AssertTrueIsDisplayed(managementActListResetButton);

            //Search Management Activities Column Headers
            AssertTrueIsDisplayed(managementActListViewMgmtDescriptionColumnHeader);
            AssertTrueIsDisplayed(managementActListViewOrderByDescription);
            AssertTrueIsDisplayed(managementActListViewFileNameColumnHeader);
            AssertTrueIsDisplayed(managementActListViewOrderByFileName);
            AssertTrueIsDisplayed(managementActListViewHistFileColumnHeader);
            AssertTrueIsDisplayed(managementActListViewOrderByHistFile);
            AssertTrueIsDisplayed(managementActListViewAddressColumnHeader);
            AssertTrueIsDisplayed(managementActListViewTypeColumnHeader);
            AssertTrueIsDisplayed(managementActListViewOrderByType);
            AssertTrueIsDisplayed(managementActListViewSubtypeHeader);
            AssertTrueIsDisplayed(managementActListViewStatusColumnHeader);
            AssertTrueIsDisplayed(managementActListViewOrderByStatus); 

            //Search Management Pagination
            AssertTrueIsDisplayed(searchMgmtPaginationMenu);
            AssertTrueIsDisplayed(searchMgmtPaginationList);
        }
    }
}
