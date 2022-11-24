using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class PropertyInformation : PageObjectBase
    {
        private By propertyCloseWindowBttn = By.XPath("//h1[contains(text(),'Property Information')]/parent::div/following-sibling::div");
        private By propertyFlyOutEllipsis = By.CssSelector("button[data-testid='fly-out-ellipsis']");
        private By propertyMoreOptionsMenu = By.CssSelector("div[class='list-group list-group-flush']");


        public PropertyInformation(IWebDriver webDriver) : base(webDriver)
        {}

        public void ClosePropertyInfoModal()
        {
            Wait();
            webDriver.FindElement(propertyCloseWindowBttn).Click();
        }

        public void ChooseCreationOptionFromPin(string option)
        {
            Wait();
            webDriver.FindElement(propertyFlyOutEllipsis).Click();

            WaitUntil(propertyMoreOptionsMenu);
            ButtonElement(option);
        }
    }
}
