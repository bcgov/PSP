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
            WaitUntilTableSpinnerDisappear();

            switch (contactType)
            {
                case "Individual":
                    ChooseSpecificRadioButton(selectContactRadioBttnGroup, "persons");
                    break;
                case "Organization":
                    ChooseSpecificRadioButton(selectContactRadioBttnGroup, "organizations");
                    break;
                default:
                    break;
            }

            Wait(2000);
            webDriver.FindElement(selectContactSearchInput).SendKeys(contactSearchName);
            webDriver.FindElement(selectContactSearchButton).Click();

            Wait(5000);
            webDriver.FindElement(selectContactSearch1stResultRadioBttn).Click();

            WaitUntilClickable(selectContactOkButton);
            webDriver.FindElement(selectContactOkButton).Click();
        }
    }
}
