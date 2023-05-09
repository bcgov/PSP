using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class Login: PageObjectBase
    {
        private By userIdirInput = By.Name("user");
        private By userPasswordInput = By.Name("password");
        private By submitUserBttn = By.Name("btnSubmit");

        public Login(IWebDriver webDriver) : base(webDriver)
        { }

        public void LoginToPIMS()
        {
            ButtonElement("Sign In");
            Wait();
        }

        public void LoginUsingIDIR(string user, string password)
        {
            Wait();
            webDriver.FindElement(userIdirInput).Clear();
            webDriver.FindElement(userIdirInput).SendKeys(user);

            webDriver.FindElement(userPasswordInput).Clear();
            webDriver.FindElement(userPasswordInput).SendKeys(password);

            Wait();

            webDriver.FindElement(submitUserBttn).Click();

        }
    }
}
