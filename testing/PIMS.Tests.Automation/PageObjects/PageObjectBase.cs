using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.IO;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace PIMS.Tests.Automation.PageObjects
{
    public abstract class PageObjectBase
    {
        protected readonly IWebDriver webDriver;
        private WebDriverWait wait;

        protected By loadingSpinner = By.CssSelector("div[data-testid='filter-backdrop-loading']");
        protected By tableLoadingSpinner = By.CssSelector("div[class='table-loading'] div[class='spinner-border']");
        protected By saveButton = By.XPath("//button/div[contains(text(),'Save')]");
        protected By cancelButton = By.XPath("//button/div[contains(text(),'Cancel')]");

        protected PageObjectBase(IWebDriver webDriver)
        {
            this.webDriver = webDriver;
            wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(120));
            wait.PollingInterval = TimeSpan.FromMilliseconds(100);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException), typeof(ElementClickInterceptedException));
        }

        protected virtual void Wait(int milliseconds = 1000) => Thread.Sleep(milliseconds);

        protected void WaitUntilSpinnerDisappear()
        {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(loadingSpinner));
            Wait();
        }

        protected void WaitUntilStale(By element)
        {
            wait.Until(ExpectedConditions.StalenessOf(webDriver.FindElement(element)));
        }

        protected void WaitUntilDisappear(By element)
        {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(element));
        }

        protected void WaitUntilVisible(By element)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(element));
        }

        protected void WaitUntilClickable(By element)
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(element));
        }

        public void WaitUntilVisibleText(By element, string text)
        {
            var webElement = webDriver.FindElement(element);
            wait.Until(ExpectedConditions.TextToBePresentInElement(webElement, text));
        }

        protected void ButtonElement(string buttonName)
        {
            var js = (IJavaScriptExecutor)webDriver;

            if (buttonName == "Save")
            {
                wait.Until(ExpectedConditions.ElementExists(saveButton));
                webDriver.FindElement(saveButton).Click();
            }
            else
            {
                wait.Until(ExpectedConditions.ElementExists(cancelButton));
                webDriver.FindElement(cancelButton).Click();
            }
        }

        protected void ButtonElement(By button)
        {
            Wait();

            var js = (IJavaScriptExecutor)webDriver;

            wait.Until(ExpectedConditions.ElementExists(button));
            webDriver.FindElement(button).Click();
        }

        protected void FocusAndClick(By element)
        {
            wait.Until(ExpectedConditions.ElementExists(element));

            var js = (IJavaScriptExecutor)webDriver;
            var selectedElement = webDriver.FindElement(element);

            js.ExecuteScript("arguments[0].scrollIntoView();", selectedElement);
            js.ExecuteScript("arguments[0].click();", selectedElement);
        }

        protected void ScrollToElement(By element)
        {
            var js = (IJavaScriptExecutor)webDriver;
            var selectedElement = webDriver.FindElement(element);

            WaitUntilClickable(element);
            js.ExecuteScript("arguments[0].scrollIntoView();", selectedElement);
        }

        protected void ChooseRandomSelectOption(By parentElementBy, int fromOption)
        {
            Random random = new Random();
            var js = (IJavaScriptExecutor)webDriver;

            var parentElement = webDriver.FindElement(parentElementBy);
            var childrenElements = parentElement.FindElements(By.TagName("option"));
            int index = random.Next(fromOption, childrenElements.Count);

            var selectedRadioBttn = childrenElements[index];

            js.ExecuteScript("arguments[0].scrollIntoView();", selectedRadioBttn);
            selectedRadioBttn.Click();
        }

        protected void ChooseSpecificSelectOption(By parentElement, string option)
        {
            Wait();

            var js = (IJavaScriptExecutor)webDriver;

            var selectElement = webDriver.FindElement(parentElement);
            var childrenElements = selectElement.FindElements(By.TagName("option"));
            var selectedOption = childrenElements.Should().ContainSingle(b => b.Text.Equals(option)).Subject;

            js.ExecuteScript("arguments[0].scrollIntoView();", selectedOption);

            selectedOption.Click();
        }

        protected void ChooseSpecificRadioButton(By parentName, string option)
        {
            var js = (IJavaScriptExecutor)webDriver;

            var childrenElements = webDriver.FindElements(parentName);
            var selectedOption = childrenElements.Should().ContainSingle(o => o.GetAttribute("value").Equals(option)).Subject;

            js.ExecuteScript("arguments[0].scrollIntoView();", selectedOption);

            selectedOption.Click();
        }

        protected void ChooseMultiSelectSpecificOption(By element, string option)
        {
            var js = (IJavaScriptExecutor)webDriver;

            var parentElement = webDriver.FindElement(element);
            var childrenElements = parentElement.FindElements(By.TagName("li"));
            var selectedOption = childrenElements.Should().ContainSingle(o => o.Text.Equals(option)).Subject;

            js.ExecuteScript("arguments[0].scrollIntoView();", selectedOption);
            selectedOption.Click();
        }

        protected void ChooseMultiSelectRandomOptions(By element, int options)
        {
            Random random = new Random();
            var js = (IJavaScriptExecutor)webDriver;

            for (int i = 0; i < options; i++)
            {
                var parentElement = webDriver.FindElement(element);
                var childrenElements = parentElement.FindElements(By.TagName("li"));

                int index = random.Next(0, childrenElements.Count);
                var selectedOption = childrenElements[index];

                js.ExecuteScript("arguments[0].scrollIntoView();", selectedOption);
                selectedOption.Click();
            } 
        }

        protected void ClearInput(By elementBy)
        {
            WaitUntilClickable(elementBy);

            var element = webDriver.FindElement(elementBy);
            while (!element.GetAttribute("value").Equals(""))
            {
                element.SendKeys(Keys.Backspace);
            }
        }

        protected void ClearDigitsInput(By elementBy)
        {
            var element = webDriver.FindElement(elementBy);
            while (!element.GetAttribute("value").Equals("0"))
            {
                element.SendKeys(Keys.Backspace);
            }
        }

        protected void ClearMultiSelectInput(By elementBy)
        {
            var parentElement = webDriver.FindElement(elementBy);
            var childrenElement = parentElement.FindElements(By.TagName("span"));

            foreach (var element in childrenElement)
            {
                element.FindElement(By.TagName("i")).Click();
            }
        }

        protected string TransformDateFormat(string date)
        {
            if (date == "")
            {
                return "";
            }
            else
            {
                var dateObject = DateTime.Parse(date);
                return dateObject.ToString("MMM d, yyyy");
            }
        }

        protected string TransformCurrencyFormat(string amount)
        {
            decimal value = decimal.Parse(amount);
            return "$" + value.ToString("#,##0.00");
        }

        protected List<string> GetProjects(By element)
        {
            var result = new List<string>();
            var projectNames = webDriver.FindElements(element);
            foreach (var projectName in projectNames)
            {
                result.Add(projectName.Text);
            }
            result.Sort();
            return result;
        }

        protected string TransformBooleanFormat(bool elementValue)
        {
            if (elementValue)
                { return "Y"; }
            else
                { return "N"; }
        }

        protected List<string> GetViewFieldListContent(By element)
        {
            var result = new List<string>();
            var parentElement = webDriver.FindElement(element);
            var childrenElements = parentElement.FindElements(By.TagName("span"));
            foreach (var childElement in childrenElements)
            {
                result.Add(childElement.Text);
            }
            result.Sort();
            return result;
        }

        protected string GetTodayFormattedDate()
        {
            DateTime thisDay = DateTime.Today;
            return thisDay.ToString("MMM d, yyyy");
        }

        protected string GetSubstring(string input, int startIndex, int endIndex)
        {
            return input.Substring(startIndex, endIndex);
        }
    }
}
