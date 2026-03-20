using OpenQA.Selenium;
using Boolean = System.Boolean;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SearchActivities : PageObjectBase
    {
        //Main Menu Elements
        private readonly By managementMainMenu = By.XPath("//div[@data-testid= 'nav-tooltip-management']");
        private readonly By managementMainMenuActivitiesListViewLink = By.XPath("//a[contains(text(),'Manage Management Activities')]");

        //Search filters
        //Management List View - Search Section Elements
        private readonly By searchManagementFileTitle = By.XPath("//h1/div/div/span[contains(text(),'Management Activities')]");
        private readonly By managementListSearchByLabel = By.XPath("//strong[contains(text(),'Search by')]");
        private readonly By managementListSearchBySelect = By.Id("input-searchBy");
        private readonly By managementListSearchByAddressInput = By.Id("input-address");
        private readonly By managementListSearchByPIDInput = By.Id("input-pid");
        private readonly By managementListSearchByPINInput = By.Id("input-pin");
        private readonly By managementListSearchByActivityStatusSelect = By.Id("input-activityStatusCode");
        private readonly By managementListSearchByFileStatusSelect = By.Id("input-managementFileStatusCode");
        private readonly By managementListSearchByActivityTypeSelect = By.Id("input-activityTypeCode");
        private readonly By managementListSearchByFilePurposeSelect = By.Id("input-managementFilePurposeCode");
        private readonly By managementListSearchByFileNbrInput= By.Id("input-fileNameOrNumberOrReference");
        private readonly By managementListSearchByProjectInput = By.Id("input-projectNameOrNumber");
        private readonly By managementListSearchButton = By.Id("search-button");
        private readonly By managementListResetButton = By.Id("reset-button");

        private readonly By managementListImportActsOverview = By.CssSelector("*[data-testid='excel-icon-overview']");
        private readonly By managementListImportInvoiceReports = By.CssSelector("*[data-testid='excel-icon-invoices']");

        private readonly By managementListColumnDescription = By.CssSelector("div[data-testid='managementActivitiesTable'] div[class='thead thead-light'] div[role='columnheader']:nth-child(1)");
        private readonly By managementListDescriptionSort = By.CssSelector("div[data-testid='sort-column-description']");
        private readonly By managementListMgmtFileName = By.CssSelector("div[data-testid='managementActivitiesTable'] div[class='thead thead-light'] div[role='columnheader']:nth-child(2)");
        private readonly By managementListColumnFileName = By.CssSelector("div[data-testid='managementActivitiesTable'] div[class='thead thead-light'] div[role='columnheader']:nth-child(3)");
        private readonly By managementListFileNameSort = By.CssSelector("div[data-testid='sort-column-fileName']");
        private readonly By managementListColumnMOTTRegion = By.CssSelector("div[data-testid='managementActivitiesTable'] div[class='thead thead-light'] div[role='columnheader']:nth-child(4)");
        private readonly By managementListColumnHistoricalFile = By.CssSelector("div[data-testid='managementActivitiesTable'] div[class='thead thead-light'] div[role='columnheader']:nth-child(5)");
        private readonly By managementListHistoricalFileSort = By.CssSelector("div[data-testid='sort-column-legacyFileNum']");
        private readonly By managementListColumnAddress = By.CssSelector("div[data-testid='managementActivitiesTable'] div[class='thead thead-light'] div[role='columnheader']:nth-child(6)");
        private readonly By managementListColumnType = By.CssSelector("div[data-testid='managementActivitiesTable'] div[class='thead thead-light'] div[role='columnheader']:nth-child(7)");
        private readonly By managementListTypeSort = By.CssSelector("div[data-testid='sort-column-activityType']");
        private readonly By managementListColumnSubtype = By.CssSelector("div[data-testid='managementActivitiesTable'] div[class='thead thead-light'] div[role='columnheader']:nth-child(8)");
        private readonly By managementListColumnStatus= By.CssSelector("div[data-testid='managementActivitiesTable'] div[class='thead thead-light'] div[role='columnheader']:nth-child(9)");
        private readonly By managementListStatusSort = By.CssSelector("div[data-testid='sort-column-activityStatus']");

        private readonly By managementListViewDescription1stRecord = By.CssSelector("div[data-testid='managementActivitiesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div[role='cell']:nth-child(1)");
        private readonly By managementListViewName1stRecord = By.CssSelector("div[data-testid='managementActivitiesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div[role='cell']:nth-child(3)");
        private readonly By managementListViewHistoricalFile1stRecord = By.CssSelector("div[data-testid='managementActivitiesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div[role='cell']:nth-child(5)");
        private readonly By managementListViewType1stRecord = By.CssSelector("div[data-testid='managementActivitiesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div[role='cell']:nth-child(7)");
        private readonly By managementListViewStatus1stRecord = By.CssSelector("div[data-testid='managementActivitiesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div[role='cell']:nth-child(8)");

        private readonly By managementListViewViewMgmtFile1stRecord = By.CssSelector("div[data-testid='managementActivitiesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child");
        private readonly By managementFilesResultsTable = By.CssSelector("div[data-testid='managementActivitiesTable'] div[class='tbody'] div[class='tr-wrapper']");

        //Search  Activities Pagination
        private readonly By searchMgmtPaginationMenu = By.CssSelector("div[class='Menu-root']");
        private readonly By searchMgmtPaginationList = By.CssSelector("ul[class='pagination']");


        public SearchActivities(IWebDriver webDriver) : base(webDriver)
        {}

        public void NavigateToSearchActivities()
        {
            Wait();
            FocusAndClick(managementMainMenu);

            Wait();
            FocusAndClick(managementMainMenuActivitiesListViewLink);
        }

        public void OrderByActDescription()
        {
            WaitUntilClickable(managementListDescriptionSort);
            webDriver.FindElement(managementListDescriptionSort).Click();
        }

        public void OrderByActName()
        {
            WaitUntilClickable(managementListFileNameSort);
            webDriver.FindElement(managementListFileNameSort).Click();
        }

        public void OrderByActHistoricalFileNbr()
        {
            WaitUntilClickable(managementListHistoricalFileSort);
            webDriver.FindElement(managementListHistoricalFileSort).Click();
        }

        public void OrderByActType()
        {
            WaitUntilClickable(managementListTypeSort);
            webDriver.FindElement(managementListTypeSort).Click();
        }

        public void OrderByActStatus()
        {
            WaitUntilClickable(managementListStatusSort);
            webDriver.FindElement(managementListStatusSort).Click();
        }

        public string FirstActDescription()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(managementListViewDescription1stRecord).Text;
        }

        public string FirstActName()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(managementListViewName1stRecord).Text;
        }

        public string FirstActHistoricalFile()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(managementListViewHistoricalFile1stRecord).Text;
        }

        public string FirstActType()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(managementListViewType1stRecord).Text;
        }

        public string FirstActMgmtStatus()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(managementListViewStatus1stRecord).Text;
        }

        public void FilterManagementActivities(string pid = "", string pin = "", string address = "", string actName = "", string actStatus = "",
            string mgmtStatus = "", string actType = "", string mgmtPurpose = "", string project = "")
        {
            Wait();
            webDriver.FindElement(managementListResetButton).Click();

            if (pid != "")
            {
                WaitUntilClickable(managementListSearchBySelect);
                ChooseSpecificSelectOption(managementListSearchBySelect, "PID");
                webDriver.FindElement(managementListSearchByPIDInput).SendKeys(pid);
            }

            if (pin != "")
            {
                WaitUntilClickable(managementListSearchBySelect);
                ChooseSpecificSelectOption(managementListSearchBySelect, "PIN");
                webDriver.FindElement(managementListSearchByPINInput).SendKeys(pin);
            }

            if (address != "")
            {
                WaitUntilClickable(managementListSearchBySelect);
                ChooseSpecificSelectOption(managementListSearchBySelect, "Address");
                webDriver.FindElement(managementListSearchByAddressInput).SendKeys(address);
            }

            if (actName != "")
            {
                WaitUntilClickable(managementListSearchByFileNbrInput);
                webDriver.FindElement(managementListSearchByFileNbrInput).SendKeys(actName);
            }

            if (actStatus != "")
            {
                WaitUntilClickable(managementListSearchByActivityStatusSelect);
                ChooseSpecificSelectOption(managementListSearchByActivityStatusSelect, actStatus);
            }

            if (mgmtStatus != "")
            {
                WaitUntilClickable(managementListSearchByFileStatusSelect);
                ChooseSpecificSelectOption(managementListSearchByFileStatusSelect, mgmtStatus);
            }

            if (actType != "")
            {
                WaitUntilClickable(managementListSearchByActivityTypeSelect);
                ChooseSpecificSelectOption(managementListSearchByActivityTypeSelect, actType);
            }
            if (mgmtPurpose != "")
            {
                WaitUntilClickable(managementListSearchByFilePurposeSelect);
                webDriver.FindElement(managementListSearchByFilePurposeSelect).SendKeys(mgmtPurpose);
            }
            if (project != "")
            {
                WaitUntilClickable(managementListSearchByProjectInput);
                webDriver.FindElement(managementListSearchByProjectInput).SendKeys(project);
            }

            WaitUntilClickable(managementListSearchButton);
            FocusAndClick(managementListSearchButton);
        }

        public Boolean SearchFoundResults()
        {
            Wait(2000);
            return webDriver.FindElements(managementListViewViewMgmtFile1stRecord).Count > 0;
        }

        public int MgmtActsTableResultNumber()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElements(managementFilesResultsTable).Count;
        }

        public void VerifySearchManagementActivitiesListView()
        {
            WaitUntilVisible(managementListSearchByLabel);

            //Search Management Title
            AssertTrueIsDisplayed(searchManagementFileTitle);

            //Search Management Activities Filters
            AssertTrueIsDisplayed(managementListSearchByLabel);
            AssertTrueIsDisplayed(managementListSearchBySelect);
            AssertTrueIsDisplayed(managementListSearchByAddressInput);
            AssertTrueIsDisplayed(managementListSearchByActivityStatusSelect);
            AssertTrueIsDisplayed(managementListSearchByFileStatusSelect);
            AssertTrueIsDisplayed(managementListSearchByActivityTypeSelect);
            AssertTrueIsDisplayed(managementListSearchByFilePurposeSelect);
            AssertTrueIsDisplayed(managementListSearchByFileNbrInput);
            AssertTrueIsDisplayed(managementListSearchByProjectInput);
            AssertTrueIsDisplayed(managementListSearchButton);
            AssertTrueIsDisplayed(managementListResetButton);

            AssertTrueIsDisplayed(managementListImportActsOverview);
            AssertTrueIsDisplayed(managementListImportInvoiceReports);

            //Search Management Activities Column Headers
            AssertTrueIsDisplayed(managementListColumnDescription);
            AssertTrueIsDisplayed(managementListDescriptionSort);
            AssertTrueIsDisplayed(managementListMgmtFileName);
            AssertTrueIsDisplayed(managementListColumnFileName);
            AssertTrueIsDisplayed(managementListFileNameSort);
            AssertTrueIsDisplayed(managementListColumnMOTTRegion);
            AssertTrueIsDisplayed(managementListColumnHistoricalFile);
            AssertTrueIsDisplayed(managementListHistoricalFileSort);
            AssertTrueIsDisplayed(managementListColumnAddress);
            AssertTrueIsDisplayed(managementListColumnType);
            AssertTrueIsDisplayed(managementListTypeSort);
            AssertTrueIsDisplayed(managementListColumnSubtype);
            AssertTrueIsDisplayed(managementListColumnStatus);
            AssertTrueIsDisplayed(managementListStatusSort);

            //Search Management Activities Pagination
            AssertTrueIsDisplayed(searchMgmtPaginationMenu);
            AssertTrueIsDisplayed(searchMgmtPaginationList);
        }
    }
}
