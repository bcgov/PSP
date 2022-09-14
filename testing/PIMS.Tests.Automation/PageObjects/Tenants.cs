using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class Tenants: PageObjectBase
    {
        private By licenseTenantLink = By.XPath("//a[contains(text(),'Tenant')]");
        private By licensePaymentsLink = By.XPath("//a[contains(text(),'Payments')]");

        private By licenseEditIcon = By.CssSelector("a[class='float-right']");

        public Tenants(IWebDriver webDriver) : base(webDriver)
        { }

        //Navigates to Tenants Section
        public void NavigateToTenantSection()
        {
            Wait();
            webDriver.FindElement(licenseTenantLink).Click();
        }

        //Edit Tenant section
        public void EditTenant()
        {
            Wait();
            webDriver.FindElement(licenseEditIcon).Click();
        }
    }
}
