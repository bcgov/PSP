

using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class LeaseSurplus : PageObjectBase
    {
        private By licenseSurplusLink = By.XPath("//a[contains(text(),'Surplus Declaration')]");

        private By licenseSurplusTableBody = By.CssSelector("div[data-testid='leasesTable'] div[class='tbody']");

        private By licenseHeaderLeaseCode = By.XPath("//b[contains(text(),'L-')]");

        public LeaseSurplus(IWebDriver webDriver) : base(webDriver)
        { }

        //Navigates to Surplus Declaration Section
        public void NavigateToSurplusSection()
        {
            Wait();
            webDriver.FindElement(licenseSurplusLink).Click();

            Assert.True(webDriver.FindElement(licenseSurplusTableBody).Displayed);
        }

        //Get the lease number
        public string GetLeaseCode()
        {
            return webDriver.FindElement(licenseHeaderLeaseCode).Text;
        }
    }
}
