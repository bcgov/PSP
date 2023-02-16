using OpenQA.Selenium;


namespace PIMS.Tests.Automation.PageObjects
{
    public class SharedSelectContact : PageObjectBase
    {
        private By selectContactModal = By.CssSelector("div[class='modal-content']");
        private By selectContactOkButton = By.XPath("//button[@title='ok-modal']/div[contains(text(),'Select')]/ancestor::button");

        private By selectContactSearchInput = By.Id("input-summary");
        private By selectContactSearchButton = By.Id("search-button");
        private By selectContactSearch1stResultRadioBttn = By.CssSelector("div[data-testid='contactsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div:nth-child(1) input");



        public SharedSelectContact(IWebDriver webDriver) : base(webDriver)
        {}

        public void SelectContact(string contactSearchName)
        {
            Wait(5000);

            webDriver.FindElement(selectContactSearchInput).SendKeys(contactSearchName);
            webDriver.FindElement(selectContactSearchButton).Click();

            Wait(3000);
            webDriver.FindElement(selectContactSearch1stResultRadioBttn).Click();

            Wait();
            webDriver.FindElement(selectContactOkButton).Click();
        }
    }
}
