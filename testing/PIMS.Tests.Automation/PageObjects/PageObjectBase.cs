using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public abstract class PageObjectBase
    {
        protected readonly IWebDriver webDriver;

        protected PageObjectBase(IWebDriver webDriver)
        {
            this.webDriver = webDriver;
        }

        public virtual string CurrentLocation => new Uri(webDriver.Url).AbsolutePath;

        public virtual void Wait(int milliseconds = 700) => Thread.Sleep(milliseconds);

        protected void ButtonElement(string btnContent)
        {
            Wait();

            var js = (IJavaScriptExecutor)webDriver;

            var buttons = webDriver.FindElements(By.TagName("button"));
            var selectedBtn = buttons.Should().ContainSingle(b => b.Text.Contains(btnContent)).Subject;
            js.ExecuteScript("arguments[0].click()", selectedBtn);
        }

        protected void FocusAndClick(By element)
        {
            Wait();

            var js = (IJavaScriptExecutor)webDriver;
            var selectedElement = webDriver.FindElement(element);

            js.ExecuteScript("arguments[0].scrollIntoView();", selectedElement);

            Wait();
            selectedElement.Click();
        }

        protected void ScrollToElement(By element)
        {
            Wait();

            var js = (IJavaScriptExecutor)webDriver;
            var selectedElement = webDriver.FindElement(element);

            js.ExecuteScript("arguments[0].scrollIntoView();", selectedElement);
        }


        protected void ChooseRandomOption(IWebElement parentElement, string parentElementName, int fromOption)
        {
            Random random = new Random();
            var js = (IJavaScriptExecutor)webDriver;

            var childrenElements = parentElement.FindElements(By.TagName("option"));
            int index = random.Next(fromOption, childrenElements.Count + 1);
            var selectedRadioBttnLocator = "select[id='" + parentElementName + "'] option:nth-child(" + index + ")";
            var selectedRadioBttn = webDriver.FindElement(By.CssSelector(selectedRadioBttnLocator));

            js.ExecuteScript("arguments[0].scrollIntoView();", selectedRadioBttn);
            Wait();
            selectedRadioBttn.Click();
        }

        protected void ChooseSpecificOption(string parentElementName, string option)
        {
            var js = (IJavaScriptExecutor)webDriver;

            var selectedRadioBttnLocator = "//select[@id='"+ parentElementName +"']/option[contains(text(),'"+ option +"')]";
            var selectedOption = webDriver.FindElement(By.XPath(selectedRadioBttnLocator));

            js.ExecuteScript("arguments[0].scrollIntoView();", selectedOption);
            Wait();
            selectedOption.Click();
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
    }

}
