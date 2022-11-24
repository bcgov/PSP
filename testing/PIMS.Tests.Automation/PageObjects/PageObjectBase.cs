using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace PIMS.Tests.Automation.PageObjects
{
    public abstract class PageObjectBase
    {
        protected readonly IWebDriver webDriver;

        protected By toastifyMessage = By.CssSelector("div[class='Toastify__toast-body']");

        protected PageObjectBase(IWebDriver webDriver)
        {
            this.webDriver = webDriver;
        }

        public virtual string CurrentLocation => new Uri(webDriver.Url).AbsolutePath;

        public virtual void Wait(int milliseconds = 1000) => Thread.Sleep(milliseconds);


        public void WaitUntil(By element)
        {
            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(30));
            wait.Until(ExpectedConditions.ElementIsVisible(element));
        }

        protected void ButtonElement(string btnContent)
        {
            Wait();

            var js = (IJavaScriptExecutor)webDriver;

            var buttons = webDriver.FindElements(By.TagName("button"));
            var selectedBtn = buttons.Should().ContainSingle(b => b.Text.Contains(btnContent)).Subject;
            selectedBtn.Click();
        }

        protected void FocusAndClick(By element)
        {
            Wait();

            var js = (IJavaScriptExecutor)webDriver;
            var selectedElement = webDriver.FindElement(element);

            js.ExecuteScript("arguments[0].scrollIntoView();", selectedElement);

            Wait();
            js.ExecuteScript("arguments[0].click();", selectedElement);
        }

        protected void ScrollToElement(By element)
        {
            Wait();

            var js = (IJavaScriptExecutor)webDriver;
            var selectedElement = webDriver.FindElement(element);

            js.ExecuteScript("arguments[0].scrollIntoView();", selectedElement);
        }

        protected void ChooseRandomSelectOption(By parentElementBy, string parentElementName, int fromOption)
        {
            Random random = new Random();
            var js = (IJavaScriptExecutor)webDriver;

            var parentElement = webDriver.FindElement(parentElementBy);
            var childrenElements = parentElement.FindElements(By.TagName("option"));
            int index = random.Next(fromOption, childrenElements.Count + 1);
            var selectedRadioBttnLocator = "select[id='" + parentElementName + "'] option:nth-child(" + index + ")";
            var selectedRadioBttn = webDriver.FindElement(By.CssSelector(selectedRadioBttnLocator));

            js.ExecuteScript("arguments[0].scrollIntoView();", selectedRadioBttn);
            Wait();
            selectedRadioBttn.Click();
        }

        protected void ChooseSpecificSelectOption(string parentElementName, string option)
        {
            var js = (IJavaScriptExecutor)webDriver;

            var selectedRadioBttnLocator = "//select[@id='"+ parentElementName +"']/option[contains(text(),'"+ option +"')]";
            var selectedOption = webDriver.FindElement(By.XPath(selectedRadioBttnLocator));

            js.ExecuteScript("arguments[0].scrollIntoView();", selectedOption);
            Wait();
            selectedOption.Click();
        }

        protected void ChooseRandomRadioButton(By parentName)
        {
            Random random = new Random();
            var js = (IJavaScriptExecutor)webDriver;

            var childrenElements = webDriver.FindElements(parentName);
            int index = random.Next(0, childrenElements.Count);
            var selectedRadioBttn = childrenElements[index];
            //var selectedRadioBttn = webDriver.FindElement(By.CssSelector(selectedRadioBttnLocator));

            js.ExecuteScript("arguments[0].scrollIntoView();", selectedRadioBttn);
            Wait();
            selectedRadioBttn.Click();
        }

        protected void ChooseSpecificRadioButton(string parentElementName, string option)
        {
            var js = (IJavaScriptExecutor)webDriver;

            var selectedRadioBttnLocator = "//input[@name='"+ parentElementName +"']/following-sibling::label[contains(text(),'"+ option +"')]";
            var selectedOption = webDriver.FindElement(By.XPath(selectedRadioBttnLocator));

            js.ExecuteScript("arguments[0].scrollIntoView();", selectedOption);
            Wait();
            selectedOption.Click();
        }

        protected void ChooseMultiSelectRandomOptions(By element, string optionsContainerName, int options)
        {
            Random random = new Random();
            var js = (IJavaScriptExecutor)webDriver;

            for (int i = 0; i < options; i++)
            {
                var parentElement = webDriver.FindElement(element);
                var childrenElements = parentElement.FindElements(By.TagName("li"));

                int index = random.Next(1, childrenElements.Count + 1);
                var selectedOptionBttnLocator = "ul[class='" + optionsContainerName + "'] li:nth-child(" + index + ")";
                var selectedOption = webDriver.FindElement(By.CssSelector(selectedOptionBttnLocator));

                js.ExecuteScript("arguments[0].scrollIntoView();", selectedOption);

                Wait();
                selectedOption.Click();
            } 
        }

        protected void ClearInput(By elementBy)
        {
            var element = webDriver.FindElement(elementBy);
            while (!element.GetAttribute("value").Equals(""))
            {
                element.SendKeys(Keys.Backspace);
            }
        }
    }

}
