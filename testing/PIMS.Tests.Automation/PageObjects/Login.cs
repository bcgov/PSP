using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PIMS.Tests.Automation.Classes;
using Protractor;
using SeleniumExtras.WaitHelpers;
using System.Collections.ObjectModel;

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

            string currentHandle = webDriver.CurrentWindowHandle;
            ReadOnlyCollection<string> originalHandles = webDriver.WindowHandles;

            WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(60));
            string popupWindowHandle = wait.Until<string>((d) =>
            {
                string foundHandle = null;

                // Subtract out the list of known handles. In the case of a single
                // popup, the newHandles list will only have one value.
                List<string> newHandles = webDriver.WindowHandles.Except(originalHandles).ToList();
                if (newHandles.Count > 0)
                    foundHandle = newHandles[0];
                

                return foundHandle;
            });

            var driver = webDriver is NgWebDriver ngDriver
               ? ngDriver.WrappedDriver
               : webDriver;

            //if (driver.WindowHandles.Count > 1)
            //{
            driver.SwitchTo().Window(popupWindowHandle);

            IWebElement element = driver.FindElements(By.TagName("button"))[1];
            element.Click();
            //element.SendKeys(Keys.Enter);

            driver.SwitchTo().Window(currentHandle);
            //}
            //else
            //{
            //    driver.SwitchTo().Window(driver.WindowHandles[1]);
            //    Actions action = new Actions(driver);
            //    action.SendKeys(Keys.Escape);
            //}

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
