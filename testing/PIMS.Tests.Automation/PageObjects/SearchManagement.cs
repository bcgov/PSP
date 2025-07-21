using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SearchManagement: PageObjectBase
    {
        //Main Menu Elements
        private readonly By managementMainMenu = By.XPath("//div[@data-testid= 'nav-tooltip-management']");
        private readonly By managementMainMenuListViewLink = By.XPath("//a[contains(text(),'Manage Management Files')]");
        private readonly By managementMainMenuCreateLink = By.XPath("//a[contains(text(),'Create Management File')]");

        //Management List View - Search Section Elements
        private readonly By searchManagementFileTitle = By.XPath("//h1/div/div/span[contains(text(),'Management Files')]");
        private readonly By managementListSearchByLabel = By.XPath("//strong[contains(text(),'Search by')]");
        private readonly By managementListSearchBySelect = By.Id("input-searchBy");
        private readonly By managementListSearchByAddressInput = By.Id("input-address");
        private readonly By managementListSearchByPIDInput = By.Id("input-pid");
        private readonly By managementListSearchByPINInput = By.Id("input-pin");
        private readonly By managementListSearchByNameInput = By.Id("input-fileNameOrNumberOrReference");
        private readonly By managementListSearchByTeamInput = By.Id("typeahead-select-managementTeamMember");
        private readonly By managementListSearchByTeamOptions = By.CssSelector("div[id='typeahead-select-managementTeamMember']");
        private readonly By managementListSearchByTeam1stOption = By.CssSelector("div[id='typeahead-select-managementTeamMember'] a:first-child");
        private readonly By managementListSearchByStatusSelect = By.Id("input-managementFileStatusCode");
        private readonly By managementListSearchByPurposeSelect = By.Id("input-managementFilePurposeCode");
        private readonly By managementListSearchByProjectInput = By.Id("input-projectNameOrNumber");
        private readonly By managementListSearchButton = By.Id("search-button");
        private readonly By managementListResetButton = By.Id("reset-button");

        //Management List View - Table Elements
        private readonly By managementListViewTable = By.XPath("//div[@data-testid='managementFilesTable']");
        private readonly By managementListViewMgmtFileColumnHeader = By.XPath("//div[@data-testid='managementFilesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Management file #')]");
        private readonly By managementListViewFileNameColumnHeader = By.XPath("//div[@data-testid='managementFilesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'File name')]");
        private readonly By managementListViewOrderByFileName = By.CssSelector("div[data-testid='sort-column-fileName']");
        private readonly By managementListViewHistFileColumnHeader = By.XPath("//div[@data-testid='managementFilesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Historical File #')]");
        private readonly By managementListViewOrderByHistFile = By.CssSelector("div[data-testid='sort-column-legacyFileNum']");
        private readonly By managementListViewProjectColumnHeader = By.XPath("//div[@data-testid='managementFilesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Project')]");
        private readonly By managementListViewPurposeColumnHeader = By.XPath("//div[@data-testid='managementFilesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Purpose')]");
        private readonly By managementListViewOrderByPurpose = By.CssSelector("div[data-testid='sort-column-managementFilePurposeTypeCode']");
        private readonly By managementListViewTeamMemberColumnHeader = By.XPath("//div[@data-testid='managementFilesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Team member')]");
        private readonly By managementListViewAddressColumnHeader = By.XPath("//div[@data-testid='managementFilesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Civic Address / PID / PIN')]");
        private readonly By managementListViewStatusColumnHeader = By.XPath("//div[@data-testid='managementFilesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Status')]");
        private readonly By managementListViewOrderByStatus = By.CssSelector("div[data-testid='sort-column-managementFileStatusTypeCode']");

        private readonly By managementListViewViewMgmtFile1stRecord = By.XPath("//div[@data-testid='managementFilesTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[1]/a");
        private readonly By managementListViewFileName1stRecord = By.XPath("//div[@data-testid='managementFilesTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[2]");
        private readonly By managementListViewHistFile1stRecord = By.XPath("//div[@data-testid='managementFilesTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[3]");
        private readonly By managementListViewProject1stRecord = By.XPath("//div[@data-testid='managementFilesTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[4]");
        private readonly By managementListViewPurpose1stRecord = By.XPath("//div[@data-testid='managementFilesTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[5]");
        private readonly By managementListViewTeamMember1stRecord = By.XPath("//div[@data-testid='managementFilesTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[6]/span");
        private readonly By managementListViewAddress1stRecord = By.XPath("//div[@data-testid='managementFilesTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[7]/div/div");
        private readonly By managementListViewStatus1stRecord = By.XPath("//div[@data-testid='managementFilesTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[8]");

        private readonly By managementFilesResultsTable = By.CssSelector("div[data-testid='managementFilesTable'] div[class='tbody'] div[class='tr-wrapper']");

        private readonly By managementFileHeaderCode = By.XPath("//label[contains(text(),'File #')]/parent::div/following-sibling::div");

        //Search Leases Pagination
        private readonly By searchMgmtPaginationMenu = By.CssSelector("div[class='Menu-root']");
        private readonly By searchMgmtPaginationList = By.CssSelector("ul[class='pagination']");

        public SearchManagement(IWebDriver driver) : base(driver)
        {}

        public void NavigateToSearchManagement()
        {
            Wait();
            FocusAndClick(managementMainMenu);

            Wait();
            FocusAndClick(managementMainMenuListViewLink);
        }

        public void OrderByMgmtFileName()
        {
            WaitUntilClickable(managementListViewOrderByFileName);
            webDriver.FindElement(managementListViewOrderByFileName).Click();
        }

        public void OrderByMgmtHistoricalFileNbr()
        {
            WaitUntilClickable(managementListViewOrderByHistFile);
            webDriver.FindElement(managementListViewOrderByHistFile).Click();
        }

        public void OrderByMgmtPurpose()
        {
            WaitUntilClickable(managementListViewOrderByPurpose);
            webDriver.FindElement(managementListViewOrderByPurpose).Click();
        }

        public void OrderByMgmtStatus()
        {
            WaitUntilClickable(managementListViewOrderByStatus);
            webDriver.FindElement(managementListViewOrderByStatus).Click();
        }

        public void SelectFirstOption()
        {
            WaitUntilClickable(managementListViewViewMgmtFile1stRecord);
            webDriver.FindElement(managementListViewViewMgmtFile1stRecord).Click();

            WaitUntilVisible(managementFileHeaderCode);
            Assert.True(webDriver.FindElement(managementFileHeaderCode).Displayed);
        }

        public string FirstMgmtFileName()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(managementListViewFileName1stRecord).Text;
        }

        public string FirstMgmtHistoricalFile()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(managementListViewHistFile1stRecord).Text;
        }

        public string FirstMgmtPurpose()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(managementListViewPurpose1stRecord).Text;
        }

        public string FirstMgmtStatus()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(managementListViewStatus1stRecord).Text;
        }

        public void FilterManagementFiles(string pid = "", string pin = "", string address = "", string mgmtfile = "", string teamMember = "",
            string status = "", string purpose = "", string project = "")
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

            if (mgmtfile != "")
            {
                WaitUntilClickable(managementListSearchByNameInput);
                webDriver.FindElement(managementListSearchByNameInput).SendKeys(mgmtfile);
            }

            if (teamMember != "")
            {
                WaitUntilClickable(managementListSearchByTeamInput);
                webDriver.FindElement(managementListSearchByTeamInput).SendKeys(teamMember);

                WaitUntilVisible(managementListSearchByTeamOptions);
                webDriver.FindElement(managementListSearchByTeam1stOption).Click();
            }

            if (status != "")
            {
                WaitUntilClickable(managementListSearchByStatusSelect);
                ChooseSpecificSelectOption(managementListSearchByStatusSelect, status);
            }

            if (purpose != "")
            {
                WaitUntilClickable(managementListSearchByPurposeSelect);
                ChooseSpecificSelectOption(managementListSearchByPurposeSelect, purpose);
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

        public int MgmtTableResultNumber()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElements(managementFilesResultsTable).Count;
        }

        public void VerifySearchManagementListView()
        {
            WaitUntilVisible(managementListSearchByLabel);

            //Search Management Title
            AssertTrueIsDisplayed(searchManagementFileTitle);

            //Search Management Filters
            AssertTrueIsDisplayed(managementListSearchByLabel);
            AssertTrueIsDisplayed(managementListSearchBySelect);
            AssertTrueIsDisplayed(managementListSearchByAddressInput);
            AssertTrueIsDisplayed(managementListSearchByNameInput);
            AssertTrueIsDisplayed(managementListSearchByTeamInput);
            AssertTrueIsDisplayed(managementListSearchByStatusSelect);
            AssertTrueIsDisplayed(managementListSearchByPurposeSelect);
            AssertTrueIsDisplayed(managementListSearchByProjectInput);
            AssertTrueIsDisplayed(managementListSearchButton);
            AssertTrueIsDisplayed(managementListResetButton);

            //Search Management Column Headers
            AssertTrueIsDisplayed(managementListViewMgmtFileColumnHeader);
            AssertTrueIsDisplayed(managementListViewFileNameColumnHeader);
            AssertTrueIsDisplayed(managementListViewOrderByFileName);
            AssertTrueIsDisplayed(managementListViewHistFileColumnHeader);
            AssertTrueIsDisplayed(managementListViewOrderByHistFile);
            AssertTrueIsDisplayed(managementListViewProjectColumnHeader);
            AssertTrueIsDisplayed(managementListViewPurposeColumnHeader);
            AssertTrueIsDisplayed(managementListViewOrderByPurpose);
            AssertTrueIsDisplayed(managementListViewTeamMemberColumnHeader);
            AssertTrueIsDisplayed(managementListViewAddressColumnHeader);
            AssertTrueIsDisplayed(managementListViewStatusColumnHeader);
            AssertTrueIsDisplayed(managementListViewOrderByStatus);

            //Search Management Pagination
            AssertTrueIsDisplayed(searchMgmtPaginationMenu);
            AssertTrueIsDisplayed(searchMgmtPaginationList);
        }

        public void VerifyManagementTableContent(ManagementFile managementFile)
        {
            WaitUntilVisibleText(managementListViewFileName1stRecord, managementFile.ManagementName);

            AssertTrueIsDisplayed(managementListViewViewMgmtFile1stRecord);

            if (managementFile.ManagementHistoricalFile != "")
                AssertTrueContentEquals(managementListViewHistFile1stRecord, managementFile.ManagementHistoricalFile);

            if(managementFile.ManagementMinistryProject != "")
                AssertTrueContentEquals(managementListViewProject1stRecord, managementFile.ManagementMinistryProject);

            AssertTrueContentEquals(managementListViewPurpose1stRecord, managementFile.ManagementPurpose);

            if(managementFile.ManagementTeam.Count > 0)
               Assert.True(webDriver.FindElements(managementListViewTeamMember1stRecord).Count > 0);

            if (managementFile.ManagementSearchPropertiesIndex!= 0)
                Assert.True(webDriver.FindElements(managementListViewAddress1stRecord).Count > 0);

            AssertTrueContentEquals(managementListViewStatus1stRecord, managementFile.ManagementStatus); 
        }
    }
}
