

using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SearchLease : PageObjectBase
    {
        //Main Menu Elements
        private By menuManagementButton = By.CssSelector("div[data-testid='nav-tooltip-leases&licenses'] a");
        private By searchLicenseButton = By.XPath("//a[contains(text(),'Manage Lease/License Files')]");
        private By searchLicenseTitle = By.XPath("//h3[contains(text(),'Leases & Licenses')]");

        //Search Filter Elements
        private By searchBySelect = By.Id("input-searchBy");
        private By searchLicenseLFileInput = By.Id("input-lFileNo");
        private By searchLicensePIDInput = By.Id("input-pinOrPid");
        private By searchLicenseStatusInput = By.Id("status-selector_input");
        private By searchLicenseStatusOptions = By.XPath("//input[@id='status-selector_input']/parent::div/following-sibling::div/ul[@class='optionContainer']");
        private By searchLicenseActiveStatusDeleteBttn = By.CssSelector("div[class='search-wrapper searchWrapper '] span i");
        private By searchLicenseProgramInput = By.Id("properties-selector_input");
        private By searchLicenceTenantInput = By.Id("input-tenantName");
        private By searchLicenceFromDateInput = By.Id("datepicker-expiryStartDate");
        private By searchLicenseToDateInput = By.Id("datepicker-expiryStartDate");
        private By searchLicenseRegionsSelect = By.Id("input-regionType");
        private By searchLicenseKeywordInput = By.Id("input-details");
        private By searchLicenseKeywordTooltip = By.Id("lease-search-keyword-tooltip");
        private By searchLicenceExportExcelBttn = By.CssSelector("button:has(svg[data-testid='excel-icon'])");
        private By searchLicenseExportCsvIcon = By.CssSelector("button:has(svg[data-testid='csv-icon'])");
        private By searchLicenseSearchButton = By.Id("search-button");
        private By searchLicenseResetButton = By.Id("reset-button");
        private By searchLicenceCreateNewBttn = By.XPath("//div[@data-testid='leasesTable']/preceding-sibling::button");

        //Search Results Table Elements
        private By searchLicenceLFileColumnHeader = By.XPath("//div[@data-testid='leasesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'L-File Number')]");
        private By searchLicenseOrderByLFileBttn = By.CssSelector("div[data-testid='sort-column-lFileNo']");
        private By searchLicenceExpiryDateColumnHeader = By.XPath("//div[@data-testid='leasesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Expiry Date')]");
        private By searchLicenceProgramNameColumnHeader = By.XPath("//div[@data-testid='leasesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Program Name')]");
        private By searchLicenceTenantNameColumnHeader = By.XPath("//div[@data-testid='leasesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Tenant Names')]");
        private By searchLicencePropertiesColumnHeader = By.XPath("//div[@data-testid='leasesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Properties')]");
        private By searchLicenceStatusColumnHeader = By.XPath("//div[@data-testid='leasesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Status')]");
        private By searchLicenseResultsTable = By.CssSelector("div[data-testid='leasesTable'] div[class='tbody'] div[class='tr-wrapper']");

        //Search Table 1st Result Elements
        private By searchLicenseResultsTable1stResult = By.CssSelector("div[data-testid='leasesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1)");
        private By searchLicense1stResultLink = By.CssSelector("div[data-testid='leasesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) a");
        private By searchLicense1stResultExpiryDateContent = By.XPath("//div[@data-testid='leasesTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[2]/span");
        private By searchLicense1stResultProgramContent = By.XPath("//div[@data-testid='leasesTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[3]");
        private By searchLicense1stResultTenantsContent = By.XPath("//div[@data-testid='leasesTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[4]/div/div");
        private By searchLicense1stResultPropertiesContent = By.XPath("//div[@data-testid='leasesTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[5]/div/div");
        private By searchLicense1stResultStatusContent = By.XPath("//div[@data-testid='leasesTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[6]");

        private By searchLicenseFileHeaderCode = By.XPath("//label[contains(text(),'Lease/License #')]/parent::div/following-sibling::div/strong/div/span[1]");

        //Search Leases Pagination
        private By searchLeasesPaginationMenu = By.CssSelector("div[class='Menu-root']");
        private By searchLeasesPaginationList = By.CssSelector("ul[class='pagination']");

        public SearchLease(IWebDriver webDriver) : base(webDriver)
        {}

        //Navigates to Search a Lease/License
        public void NavigateToSearchLicense()
        {
            Wait();
            FocusAndClick(menuManagementButton);

            Wait();
            FocusAndClick(searchLicenseButton);
        }

        public void SearchLicenseByLFile(string lFile)
        {
            Wait(2000);
            webDriver.FindElement(searchLicenseLFileInput).SendKeys(lFile);
            webDriver.FindElement(searchLicenseActiveStatusDeleteBttn).Click();
            FocusAndClick(searchLicenseSearchButton);
        }

        public void SearchLastLease()
        {
            Wait(2000);
            webDriver.FindElement(searchLicenseResetButton).Click();

            Wait(2000);
            webDriver.FindElement(searchLicenseActiveStatusDeleteBttn).Click();
            webDriver.FindElement(searchLicenseSearchButton).Click();

            WaitUntilClickable(searchLicenseOrderByLFileBttn);
            webDriver.FindElement(searchLicenseOrderByLFileBttn).Click();

            Wait();
            webDriver.FindElement(searchLicenseOrderByLFileBttn).Click();
        }

        public void OrderByLastLease()
        {
            WaitUntilClickable(searchLicenseOrderByLFileBttn);
            webDriver.FindElement(searchLicenseOrderByLFileBttn).Click();

            Wait();
            webDriver.FindElement(searchLicenseOrderByLFileBttn).Click();
        }

        public void SelectFirstOption()
        {
            WaitUntilClickable(searchLicense1stResultLink);
            webDriver.FindElement(searchLicense1stResultLink).Click();

            WaitUntilVisible(searchLicenseFileHeaderCode);
            Assert.True(webDriver.FindElement(searchLicenseFileHeaderCode).Displayed);
        }

        public void FilterLeasesFiles(string pid, string expiryDate, string tenant, string status)
        {
            Wait();
            webDriver.FindElement(searchLicenseResetButton).Click();
            webDriver.FindElement(searchLicenseActiveStatusDeleteBttn).Click();

            WaitUntilClickable(searchBySelect);
            ChooseSpecificSelectOption(searchBySelect, "PID/PIN");
            webDriver.FindElement(searchLicensePIDInput).SendKeys(pid);
            webDriver.FindElement(searchLicenceFromDateInput).SendKeys(expiryDate);
            webDriver.FindElement(searchLicenceTenantInput).SendKeys(tenant);

            if (status != "")
            {
                webDriver.FindElement(searchLicenseStatusInput).Click();

                WaitUntilClickable(searchLicenseStatusOptions);
                ChooseMultiSelectSpecificOption(searchLicenseStatusOptions, status);
            }
             
            WaitUntilClickable(searchLicenseSearchButton);
            FocusAndClick(searchLicenseSearchButton);
        }

        public Boolean SearchFoundResults()
        {
            Wait(2000);
            return webDriver.FindElements(searchLicenseResultsTable1stResult).Count > 0;
        }

        public void VerifySearchLeasesView()
        {
            WaitUntilVisible(searchBySelect);

            //Search Leases Title
            AssertTrueIsDisplayed(searchLicenseTitle);

            //Search Leases Filters
            AssertTrueIsDisplayed(searchBySelect);
            AssertTrueIsDisplayed(searchLicensePIDInput);
            AssertTrueIsDisplayed(searchLicenseStatusInput);
            AssertTrueIsDisplayed(searchLicenseProgramInput);
            AssertTrueIsDisplayed(searchLicenceTenantInput);
            AssertTrueIsDisplayed(searchLicenceFromDateInput);
            AssertTrueIsDisplayed(searchLicenseToDateInput);
            AssertTrueIsDisplayed(searchLicenseRegionsSelect);
            AssertTrueIsDisplayed(searchLicenseKeywordInput);
            AssertTrueIsDisplayed(searchLicenseKeywordTooltip);
            AssertTrueIsDisplayed(searchLicenceExportExcelBttn);
            AssertTrueIsDisplayed(searchLicenseExportCsvIcon);
            AssertTrueIsDisplayed(searchLicenseSearchButton);
            AssertTrueIsDisplayed(searchLicenseResetButton);
            AssertTrueIsDisplayed(searchLicenceCreateNewBttn);

            //Search Leases Table Results
            AssertTrueIsDisplayed(searchLicenceLFileColumnHeader);
            AssertTrueIsDisplayed(searchLicenceExpiryDateColumnHeader);
            AssertTrueIsDisplayed(searchLicenceProgramNameColumnHeader);
            AssertTrueIsDisplayed(searchLicenceTenantNameColumnHeader);
            AssertTrueIsDisplayed(searchLicencePropertiesColumnHeader);
            AssertTrueIsDisplayed(searchLicenceStatusColumnHeader);
            AssertTrueIsDisplayed(searchLicenseResultsTable);

            //Search Leases Pagination
            AssertTrueIsDisplayed(searchLeasesPaginationMenu);
            AssertTrueIsDisplayed(searchLeasesPaginationList);
        }

        public void VerifyLeaseTableContent(string expiryDate, string program, string status)
        {
            WaitUntilVisibleText(searchLicense1stResultExpiryDateContent, webDriver.FindElement(searchLicense1stResultExpiryDateContent).Text);

            AssertTrueIsDisplayed(searchLicense1stResultLink);
            AssertTrueContentEquals(searchLicense1stResultExpiryDateContent, TransformDateFormat(expiryDate));
            AssertTrueContentEquals(searchLicense1stResultProgramContent, program);
            Assert.Equal(0, webDriver.FindElements(searchLicense1stResultTenantsContent).Count);
            Assert.True(webDriver.FindElements(searchLicense1stResultPropertiesContent).Count > 0);
            AssertTrueContentEquals(searchLicense1stResultStatusContent, status);
        }
    }
}
