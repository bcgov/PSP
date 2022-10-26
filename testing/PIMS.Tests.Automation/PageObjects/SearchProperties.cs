

using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SearchProperties : PageObjectBase
    {
        private By searchPropertyInput = By.Id("input-pinOrPid");
        private By searchPropertySearchBttn = By.Id("search-button");
        private By searchPropertyFoundPin = By.CssSelector("div[class='leaflet-pane leaflet-marker-pane'] img");

        public SearchProperties(IWebDriver webDriver) : base(webDriver)
        {}

        public void SearchPropertyByPINPID(string PID)
        {
            Wait();
            webDriver.FindElement(searchPropertyInput).SendKeys(PID);
            webDriver.FindElement(searchPropertySearchBttn).Click();

            Wait();
            webDriver.FindElement(searchPropertyFoundPin).Click();
        }
    }
}
