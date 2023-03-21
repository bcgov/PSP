﻿

using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SearchLease : PageObjectBase
    {
        //Main Menu Elements
        private By menuManagementButton = By.XPath("//a/label[contains(text(),'Management')]/parent::a");
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
            webDriver.FindElement(menuManagementButton).Click();

            Wait();
            webDriver.FindElement(searchLicenseButton).Click();
        }

        public void SearchLicenseByLFile(string lFile)
        {
            Wait();
            webDriver.FindElement(searchLicenseLFileInput).SendKeys(lFile);
            webDriver.FindElement(searchLicenseActiveStatusDeleteBttn).Click();
            webDriver.FindElement(searchLicenseSearchButton).Click();
        }

        public void SearchLastLease()
        {
            Wait();
            webDriver.FindElement(searchLicenseResetButton).Click();

            Wait();
            webDriver.FindElement(searchLicenseActiveStatusDeleteBttn).Click();
            webDriver.FindElement(searchLicenseSearchButton).Click();

            Wait();
            webDriver.FindElement(searchLicenseOrderByLFileBttn).Click();
            webDriver.FindElement(searchLicenseOrderByLFileBttn).Click();
        }

        public void SelectFirstOption()
        {
            Wait();
            webDriver.FindElement(searchLicense1stResultLink).Click();

            Wait();
            Assert.True(webDriver.FindElement(searchLicenseFileHeaderCode).Displayed);
        }

        public void FilterLeasesFiles(string pid, string expiryDate, string tenant, string status)
        {
            Wait();
            webDriver.FindElement(searchLicenseResetButton).Click();
            webDriver.FindElement(searchLicenseActiveStatusDeleteBttn).Click();

            Wait();
            ChooseSpecificSelectOption(searchBySelect, "PID/PIN");
            webDriver.FindElement(searchLicensePIDInput).SendKeys(pid);
            webDriver.FindElement(searchLicenceFromDateInput).SendKeys(expiryDate);
            webDriver.FindElement(searchLicenceTenantInput).SendKeys(tenant);
            webDriver.FindElement(searchLicenseStatusInput).Click();

            Wait();
            ChooseMultiSelectSpecificOption(searchLicenseStatusOptions, status);

            webDriver.FindElement(searchLicenseSearchButton).Click();
        }

        public Boolean SearchFoundResults()
        {
            Wait();
            return webDriver.FindElements(searchLicenseResultsTable1stResult).Count > 0;
        }

        public void VerifySearchLeasesView()
        {
            Wait();

            //Search Leases Title
            Assert.True(webDriver.FindElement(searchLicenseTitle).Displayed);

            //Search Leases Filters
            Assert.True(webDriver.FindElement(searchBySelect).Displayed);
            Assert.True(webDriver.FindElement(searchLicenseLFileInput).Displayed);
            Assert.True(webDriver.FindElement(searchLicenseStatusInput).Displayed);
            Assert.True(webDriver.FindElement(searchLicenseProgramInput).Displayed);
            Assert.True(webDriver.FindElement(searchLicenceTenantInput).Displayed);
            Assert.True(webDriver.FindElement(searchLicenceFromDateInput).Displayed);
            Assert.True(webDriver.FindElement(searchLicenseToDateInput).Displayed);
            Assert.True(webDriver.FindElement(searchLicenseRegionsSelect).Displayed);
            Assert.True(webDriver.FindElement(searchLicenseKeywordInput).Displayed);
            Assert.True(webDriver.FindElement(searchLicenseKeywordTooltip).Displayed);
            Assert.True(webDriver.FindElement(searchLicenceExportExcelBttn).Displayed);
            Assert.True(webDriver.FindElement(searchLicenseExportCsvIcon).Displayed);
            Assert.True(webDriver.FindElement(searchLicenseSearchButton).Displayed);
            Assert.True(webDriver.FindElement(searchLicenseResetButton).Displayed);
            Assert.True(webDriver.FindElement(searchLicenceCreateNewBttn).Displayed);

            //Search Leases Table Results
            Assert.True(webDriver.FindElement(searchLicenceLFileColumnHeader).Displayed);
            Assert.True(webDriver.FindElement(searchLicenceExpiryDateColumnHeader).Displayed);
            Assert.True(webDriver.FindElement(searchLicenceProgramNameColumnHeader).Displayed);
            Assert.True(webDriver.FindElement(searchLicenceTenantNameColumnHeader).Displayed);
            Assert.True(webDriver.FindElement(searchLicencePropertiesColumnHeader).Displayed);
            Assert.True(webDriver.FindElement(searchLicenceStatusColumnHeader).Displayed);
            Assert.True(webDriver.FindElement(searchLicenseResultsTable).Displayed);

            //Search Leases Pagination
            Assert.True(webDriver.FindElement(searchLeasesPaginationMenu).Displayed);
            Assert.True(webDriver.FindElement(searchLeasesPaginationList).Displayed);
        }

        public void VerifyLeaseTableContent(string expiryDate, string program, string status)
        {
            Wait(1500);

            Assert.True(webDriver.FindElement(searchLicense1stResultLink).Displayed);
            Assert.True(webDriver.FindElement(searchLicense1stResultExpiryDateContent).Text.Equals(TransformDateFormat(expiryDate)));
            Assert.True(webDriver.FindElement(searchLicense1stResultProgramContent).Text.Equals(program));
            Assert.True(webDriver.FindElements(searchLicense1stResultTenantsContent).Count > 0);
            Assert.True(webDriver.FindElements(searchLicense1stResultPropertiesContent).Count > 0);
            Assert.True(webDriver.FindElement(searchLicense1stResultStatusContent).Text.Equals(status));
        }
    }
}
