using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SharedSelectContact : PageObjectBase
    {
        private readonly By selectContactOkButton = By.XPath("//button[@title='ok-modal']/div[contains(text(),'Select')]/ancestor::button");

        private readonly By selectContactSearchInput = By.CssSelector("input[id='input-summary']");
        private readonly By selectContactRadioBttnGroup = By.XPath("//input[@name='searchBy']");
        private readonly By selectContactSearchButton = By.XPath("//div[contains(text(),'Select Contact')]/parent::div/following-sibling::div/div/div/div/form/div/div/div/div//button[@id='search-button']");
        private readonly By selectContactSearch1stResultRadioBttn = By.CssSelector("div[data-testid='contactsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div:nth-child(1) input");

        public SharedSelectContact(IWebDriver webDriver) : base(webDriver)
        {}

        public void SelectContact(string contactSearchName, string contactType)
        {
            WaitForTableToLoad();

            switch (contactType)
            {
                case "Individual":
                    ChooseRadioButton(selectContactRadioBttnGroup, "persons");
                    break;
                case "Organization":
                    ChooseRadioButton(selectContactRadioBttnGroup, "organizations");
                    break;
                default:
                    break;
            }

            Wait();
            webDriver.FindElement(selectContactSearchInput).SendKeys(contactSearchName);
            SafeClick(selectContactSearchButton);
            SafeClick(selectContactSearch1stResultRadioBttn);
            SafeClick(selectContactOkButton);
        }
    }
}
