﻿

using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SearchLease : PageObjectBase
    {
        //Main Menu Elements
        private readonly By menuManagementButton = By.CssSelector("div[data-testid='nav-tooltip-leases&licences'] a");
        private readonly By searchLicenseButton = By.XPath("//a[contains(text(),'Manage Lease/Licence Files')]");
        private readonly By searchLicenseTitle = By.XPath("//h1/div/div/span[contains(text(),'Leases & Licences')]");

        //Search Filter Elements
        private readonly By searchBySelect = By.Id("input-searchBy");
        private readonly By searchLicenseLFileInput = By.Id("input-lFileNo");
        private readonly By searchLicensePIDInput = By.Id("input-pid");
        private readonly By searchLicensePINInput = By.Id("input-pin");
        private readonly By searchLicenseAddressInput = By.Id("input-address");
        private readonly By searchLicenseHistoricalFile = By.Id("input-historical");
        private readonly By searchLicenseProgramSelect = By.Id("properties-selector_input");
        private readonly By searchLicenseStatusInput = By.Id("status-selector_input");
        private readonly By searchLicenseStatusOptions = By.XPath("//input[@id='status-selector_input']/parent::div/following-sibling::div/ul[@class='optionContainer']");
        private readonly By searchLicenseActiveStatusDeleteBttn = By.CssSelector("div[class='search-wrapper searchWrapper '] span i");
        private readonly By searchLicenseProgramOptions = By.XPath("//input[@id='program-selector_input']/parent::div/following-sibling::div/ul[@class='optionContainer']");
        private readonly By searchLicenceTenantInput = By.Id("input-tenantName");
        private readonly By searchLicenceFromDateInput = By.Id("datepicker-expiryStartDate");
        private readonly By searchLicenseToDateInput = By.Id("datepicker-expiryEndDate");
        private readonly By searchLicenseRegionsSelect = By.Id("input-regionType");
        private readonly By searchLicenseKeywordInput = By.Id("input-details");
        private readonly By searchLicenseKeywordTooltip = By.Id("lease-search-keyword-tooltip");
        private readonly By searchLicenceExportExcelBttn = By.CssSelector("button:has(svg[data-testid='excel-icon'])");
        private readonly By searchLicenseExportCsvIcon = By.CssSelector("button:has(svg[data-testid='csv-icon'])");
        private readonly By searchLicenseSearchButton = By.Id("search-button");
        private readonly By searchLicenseResetButton = By.Id("reset-button");
        private readonly By searchLicenceCreateNewBttn = By.XPath("//div[contains(text(),'Create a Lease/Licence')]/parent::button");

        //Search Results Table Elements
        private readonly By searchLicenceLFileColumnHeader = By.XPath("//div[@data-testid='leasesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'L-File Number')]");
        private readonly By searchLicenseOrderByLFileBttn = By.CssSelector("div[data-testid='sort-column-lFileNo']");
        private readonly By searchLicenceExpiryDateColumnHeader = By.XPath("//div[@data-testid='leasesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Expiry Date')]");
        private readonly By searchLicenseOrderByExpiryDateBttn = By.CssSelector("div[data-testid='sort-column-expiryDate']");
        private readonly By searchLicenceProgramNameColumnHeader = By.XPath("//div[@data-testid='leasesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Program Name')]");
        private readonly By searchLicenseOrderByProgramNameBttn = By.CssSelector("div[data-testid='sort-column-programName']");
        private readonly By searchLicenceTenantNameColumnHeader = By.XPath("//div[@data-testid='leasesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Tenant Names')]");
        private readonly By searchLicencePropertiesColumnHeader = By.XPath("//div[@data-testid='leasesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Properties')]");
        private readonly By searchLicenceStatusColumnHeader = By.XPath("//div[@data-testid='leasesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Status')]");
        private readonly By searchLicenseOrderByStatusBttn = By.CssSelector("div[data-testid='sort-column-fileStatusTypeCode']");

        private readonly By searchLicenseResultsTable = By.CssSelector("div[data-testid='leasesTable'] div[class='tbody'] div[class='tr-wrapper']");

        //Search Table 1st Result Elements
        private readonly By searchLicenseResultsTable1stResult = By.CssSelector("div[data-testid='leasesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1)");
        private readonly By searchLicense1stResultLink = By.CssSelector("div[data-testid='leasesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) a");
        private readonly By searchLicense1stResultExpiryDateContent = By.XPath("//div[@data-testid='leasesTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[2]/span[1]");
        private readonly By searchLicense1stResultProgramContent = By.XPath("//div[@data-testid='leasesTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[3]");
        private readonly By searchLicense1stResultTenantsContent = By.XPath("//div[@data-testid='leasesTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[4]/div/div");
        private readonly By searchLicense1stResultPropertiesContent = By.XPath("//div[@data-testid='leasesTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[5]/div/div");
        private readonly By searchLicense1stResultHistoricalFileContent = By.XPath("//div[@data-testid='leasesTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[6]");
        private readonly By searchLicense1stResultStatusContent = By.XPath("//div[@data-testid='leasesTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[7]");

        private readonly By searchLicenseFileHeaderCode = By.XPath("//label[contains(text(),'Lease/Licence #')]/parent::div/following-sibling::div/span[1]");

        //Search Leases Pagination
        private readonly By searchLeasesPaginationMenu = By.CssSelector("div[class='Menu-root']");
        private readonly By searchLeasesPaginationList = By.CssSelector("ul[class='pagination']");

        public SearchLease(IWebDriver webDriver) : base(webDriver)
        {}

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

        public void SearchAllLeases()
        {
            Wait(2000);
            webDriver.FindElement(searchLicenseResetButton).Click();

            Wait(2000);
            webDriver.FindElement(searchLicenseActiveStatusDeleteBttn).Click();
            webDriver.FindElement(searchLicenseSearchButton).Click();
        }

        public void OrderByLastLease()
        {
            WaitUntilClickable(searchLicenseOrderByLFileBttn);
            webDriver.FindElement(searchLicenseOrderByLFileBttn).Click();

            Wait();
            webDriver.FindElement(searchLicenseOrderByLFileBttn).Click();
        }

        public void OrderByLeaseFileNumber()
        {
            WaitUntilClickable(searchLicenseOrderByLFileBttn);
            webDriver.FindElement(searchLicenseOrderByLFileBttn).Click();
        }

        public void OrderByLeaseExpiryDate()
        {
            WaitUntilClickable(searchLicenseOrderByExpiryDateBttn);
            webDriver.FindElement(searchLicenseOrderByExpiryDateBttn).Click();
        }

        public void OrderByLeaseProgramName()
        {
            WaitUntilClickable(searchLicenseOrderByProgramNameBttn);
            webDriver.FindElement(searchLicenseOrderByProgramNameBttn).Click();
        }

        public void OrderByLeaseStatus()
        {
            WaitUntilClickable(searchLicenseOrderByStatusBttn);
            webDriver.FindElement(searchLicenseOrderByStatusBttn).Click();
        }

        public void SelectFirstOption()
        {
            WaitUntilClickable(searchLicense1stResultLink);
            webDriver.FindElement(searchLicense1stResultLink).Click();

            WaitUntilVisible(searchLicenseFileHeaderCode);
            Assert.True(webDriver.FindElement(searchLicenseFileHeaderCode).Displayed);
        }

        public string FirstLeaseFileNumber()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(searchLicense1stResultLink).Text;
        }

        public string FirstLeaseExpiryDate()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(searchLicense1stResultExpiryDateContent).Text;
        }

        public string FirstLeaseProgramName()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(searchLicense1stResultProgramContent).Text;
        }

        public string FirstLeaseStatus()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(searchLicense1stResultStatusContent).Text;
        }

        public void FilterLeasesFiles(string pid = "", string pin = "", string address = "", string lfile = "", string historicalFile = "", string program = "",
            string status = "", string tenant = "", string expiryDateFrom = "", string expiryDateTo = "", string region = "", string keyword = "")
        {
            Wait();
            webDriver.FindElement(searchLicenseResetButton).Click();
            webDriver.FindElement(searchLicenseActiveStatusDeleteBttn).Click();

            if (pid != "")
            {
                WaitUntilClickable(searchBySelect);
                ChooseSpecificSelectOption(searchBySelect, "PID");
                webDriver.FindElement(searchLicensePIDInput).SendKeys(pid);
            }

            if (pin != "")
            {
                WaitUntilClickable(searchBySelect);
                ChooseSpecificSelectOption(searchBySelect, "PIN");
                webDriver.FindElement(searchLicensePINInput).SendKeys(pin);
            }

            if (address != "")
            {
                WaitUntilClickable(searchBySelect);
                ChooseSpecificSelectOption(searchBySelect, "Address");
                webDriver.FindElement(searchLicenseAddressInput).SendKeys(address);
            }

            if (lfile != "")
            {
                WaitUntilClickable(searchBySelect);
                ChooseSpecificSelectOption(searchBySelect, "L-File #");
                webDriver.FindElement(searchLicenseLFileInput).SendKeys(lfile);
            }

            if (historicalFile != "")
            {
                WaitUntilClickable(searchBySelect);
                ChooseSpecificSelectOption(searchBySelect, "Historical File #");
                webDriver.FindElement(searchLicenseHistoricalFile).SendKeys(historicalFile);
            }

            if (program != "")
            {
                WaitUntilClickable(searchLicenseProgramSelect);

                webDriver.FindElement(searchLicenseProgramSelect).Click();
                WaitUntilClickable(searchLicenseProgramOptions);
                ChooseMultiSelectSpecificOption(searchLicenseProgramOptions, program);
            }

            if (status != "")
            {
                WaitUntilClickable(searchLicenseStatusInput);

                webDriver.FindElement(searchLicenseStatusInput).Click();
                WaitUntilClickable(searchLicenseStatusOptions);
                ChooseMultiSelectSpecificOption(searchLicenseStatusOptions, status);
            }

            if (tenant != "")
            {
                WaitUntilClickable(searchLicenceTenantInput);
                webDriver.FindElement(searchLicenceTenantInput).SendKeys(tenant);
            }

            if (expiryDateFrom != "")
            {
                WaitUntilClickable(searchLicenceFromDateInput);
                webDriver.FindElement(searchLicenceFromDateInput).SendKeys(expiryDateFrom);
            }

            if (expiryDateTo != "")
            {
                WaitUntilClickable(searchLicenseToDateInput);
                webDriver.FindElement(searchLicenseToDateInput).SendKeys(address);
            }

            if (region != "")
            {
                WaitUntilClickable(searchLicenseRegionsSelect);
                ChooseSpecificSelectOption(searchLicenseRegionsSelect, region);
            }

            if (keyword != "")
            {
                WaitUntilClickable(searchLicenseKeywordInput);
                webDriver.FindElement(searchLicenseKeywordInput).SendKeys(keyword);
            }
             
            WaitUntilClickable(searchLicenseSearchButton);
            FocusAndClick(searchLicenseSearchButton);
        }

        public Boolean SearchFoundResults()
        {
            Wait(2000);
            return webDriver.FindElements(searchLicenseResultsTable1stResult).Count > 0;
        }

        public int LeasesTableResultNumber()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElements(searchLicenseResultsTable).Count;
        }

        public void VerifySearchLeasesView()
        {
            WaitUntilVisible(searchBySelect);

            //Search Leases Title
            AssertTrueIsDisplayed(searchLicenseTitle);

            //Search Leases Filters
            AssertTrueIsDisplayed(searchBySelect);
            AssertTrueIsDisplayed(searchLicenseLFileInput);
            AssertTrueIsDisplayed(searchLicenseStatusInput);
            AssertTrueIsDisplayed(searchLicenseProgramSelect);
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

        public void VerifyLeaseTableContent(Lease lease)
        {
            WaitUntilVisibleText(searchLicense1stResultExpiryDateContent, webDriver.FindElement(searchLicense1stResultExpiryDateContent).Text);

            AssertTrueIsDisplayed(searchLicense1stResultLink);

            if(lease.LeaseExpiryDate != "")
                AssertTrueContentEquals(searchLicense1stResultExpiryDateContent, CalculateExpiryCurrentDate(lease.LeaseExpiryDate, lease.LeaseRenewals));

            if(lease.Program != "")
                AssertTrueContentEquals(searchLicense1stResultProgramContent, lease.Program);

            //if(lease.LeaseTenants.Count > 0)
            //    Assert.NotEmpty(webDriver.FindElements(searchLicense1stResultTenantsContent));

            if (lease.SearchPropertiesIndex!= 0)
                Assert.True(webDriver.FindElements(searchLicense1stResultPropertiesContent).Count > 0);

            //TO-DO: HISTORICAL FILE


            if(lease.LeaseStatus != "")
                AssertTrueContentEquals(searchLicense1stResultStatusContent, lease.LeaseStatus);
        }
    }
}
