using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class LeaseConsultations : PageObjectBase
    {
        //Insurance Menu Elements
        private By licenseInsuranceLink = By.XPath("//a[contains(text(),'Insurance')]");
        private By insuranceEditIcon = By.XPath("//div[@role='tabpanel']/div/div/div/button");

        //Insurance Create Form Elements


        public LeaseConsultations(IWebDriver webDriver) : base(webDriver)
        {
        }
    }
}
