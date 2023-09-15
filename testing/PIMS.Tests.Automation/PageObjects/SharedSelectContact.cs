using OpenQA.Selenium;


namespace PIMS.Tests.Automation.PageObjects
{
    public class SharedSelectContact : PageObjectBase
    {
        private By selectContactOkButton = By.XPath("//button[@title='ok-modal']/div[contains(text(),'Select')]/ancestor::button");

        private By selectContactSearchInput = By.Id("input-summary");
        private By selectContactRadioBttnGroup = By.XPath("//input[@name='searchBy']");
        private By selectContactSearchButton = By.XPath("//div[contains(text(),'Select a contact')]/parent::div/following-sibling::div/div/div/div/form/div/div/div/div//button[@id='search-button']");
        private By selectContactSearch1stResultRadioBttn = By.CssSelector("div[data-testid='contactsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div:nth-child(1) input");

        public SharedSelectContact(IWebDriver webDriver) : base(webDriver)
        {}

        public void SelectContact(string contactSearchName, string contactType)
        {
            Wait(3000);

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

            webDriver.FindElement(selectContactSearchInput).SendKeys(contactSearchName);
            webDriver.FindElement(selectContactSearchButton).Click();

            WaitUntilVisible(selectContactSearch1stResultRadioBttn);
            webDriver.FindElement(selectContactSearch1stResultRadioBttn).Click();

            WaitUntilClickable(selectContactOkButton);
            webDriver.FindElement(selectContactOkButton).Click();
        }
    }
}
