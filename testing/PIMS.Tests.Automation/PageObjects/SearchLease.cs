

using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SearchLease : PageObjectBase
    {
        private By menuManagementButton = By.XPath("//a/label[contains(text(),'Management')]/parent::a");
        private By searchLicenseButton = By.XPath("//a[contains(text(),'Search for a Lease or License')]");

        private By searchLicenseLFileInput = By.Id("input-lFileNo");
        private By searchLicenseActiveStatusDeleteBttn = By.CssSelector("div[class='search-wrapper searchWrapper '] span i");
        private By searchLicenseSearchButton = By.Id("search-button");
        private By searchLicenseResetButton = By.Id("reset-button");

        private By searchLicenseResultsTable1stResult = By.CssSelector("div[data-testid='leasesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1)");

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

        public Boolean SearchFoundResults()
        {
            return webDriver.FindElements(searchLicenseResultsTable1stResult).Count > 0;
        }
    }
}
