using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace PIMS.Tests.Automation.PageObjects
{
    public class Login: PageObjectBase
    {
        private By signInButton = By.XPath("//div[contains(text(), 'Sign In')]/parent::button");

        private By userIdirInput = By.Name("user");
        private By userPasswordInput = By.Name("password");
        private By submitUserBttn = By.Name("btnSubmit");

        private By initToastBody = By.CssSelector("div[class='Toastify__toast-body']");
        private WebDriverWait wait;


        public Login(IWebDriver webDriver) : base(webDriver)
        {
            wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(60));
        }

        public void LoginToPIMS()
        {
            WaitUntilSpinnerDisappear();
            WaitUntilVisible(signInButton);
            
            FocusAndClick(signInButton);
        }

        public void LoginUsingIDIR(string user, string password)
        {
            WaitUntilVisible(userIdirInput);
            webDriver.FindElement(userIdirInput).Clear();
            webDriver.FindElement(userIdirInput).SendKeys(user);

            webDriver.FindElement(userPasswordInput).Clear();
            webDriver.FindElement(userPasswordInput).SendKeys(password);

            WaitUntilClickable(submitUserBttn);
            webDriver.FindElement(submitUserBttn).Click();

            WaitUntilSpinnerDisappear();
        }
    }
}
