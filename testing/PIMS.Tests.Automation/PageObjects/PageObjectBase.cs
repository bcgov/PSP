using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Globalization;

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
        }

        protected virtual void Wait(int milliseconds = 3000) => Thread.Sleep(milliseconds);

        protected void WaitUntilSpinnerDisappear()
        {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(loadingSpinner));
            Wait();
        }

        protected void WaitUntilTableSpinnerDisappear()
        {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(tableLoadingSpinner));
            Wait();
        }

        protected void WaitUntilStale(By element) => wait.Until(ExpectedConditions.StalenessOf(webDriver.FindElement(element)));

        protected void WaitUntilDisappear(By element) => wait.Until(ExpectedConditions.InvisibilityOfElementLocated(element));

        protected void WaitUntilVisible(By element) => wait.Until(ExpectedConditions.ElementIsVisible(element));

        protected void WaitUntilExist(By element) => wait.Until(ExpectedConditions.ElementExists(element));

        protected void WaitUntilClickable(By element) => wait.Until(ExpectedConditions.ElementToBeClickable(element));

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
                wait.Until(ExpectedConditions.ElementToBeClickable(saveButton));
                webDriver.FindElement(saveButton).Click();
            }
            else
            {
                wait.Until(ExpectedConditions.ElementExists(cancelButton));
                wait.Until(ExpectedConditions.ElementToBeClickable(cancelButton));
                webDriver.FindElement(cancelButton).Click();
            }
        }

        protected void ButtonElement(By button)
        {
            Wait();

            var js = (IJavaScriptExecutor)webDriver;

            wait.Until(ExpectedConditions.ElementExists(button));
            wait.Until(ExpectedConditions.ElementToBeClickable(button));
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

        protected void ChooseSpecificSelectOption(By parentElement, string option)
        {
            Wait(2000);

            var js = (IJavaScriptExecutor)webDriver;
            
            var selectElement = webDriver.FindElement(parentElement);
            wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(selectElement.FindElements(By.TagName("option"))));

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
            js.ExecuteScript("arguments[0].click();", selectedOption);
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
            WaitUntilVisible(elementBy);

            var parentElement = webDriver.FindElement(elementBy);
            var childrenElements = parentElement.FindElements(By.TagName("span"));

            foreach (var element in childrenElements)
            {
                element.FindElement(By.TagName("i")).Click();
            }
        }

        protected void AssertTrueIsDisplayed(By elementBy)
        {
            WaitUntilVisible(elementBy);
            Assert.True(webDriver.FindElement(elementBy).Displayed);
        }

        protected void AssertTrueContentEquals(By elementBy, string text)
        {
            WaitUntilVisible(elementBy);
            Assert.Equal(text, webDriver.FindElement(elementBy).Text);
        }

        protected void AssertTrueElementValueEquals(By elementBy, string text)
        {
            WaitUntilVisible(elementBy);
            Assert.Equal(text, webDriver.FindElement(elementBy).GetAttribute("Value"));
        }

        protected void AssertTrueDoublesEquals(By elementBy, double number2)
        {
            WaitUntilVisible(elementBy);
            var numberFromElement = webDriver.FindElement(elementBy).GetAttribute("Value");
            var number1 = double.Parse(numberFromElement);

            Assert.True(number1.Equals(number2));
        }

        protected void AssertTrueContentNotEquals(By elementBy, string text)
        {
            WaitUntilVisible(elementBy);
            Assert.True(webDriver.FindElement(elementBy).Text != text);
        }

        protected void AssertTrueElementContains(By elementBy, string text)
        {
            WaitUntilVisible(elementBy);
            Assert.Contains(text, webDriver.FindElement(elementBy).Text);
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
            if (amount == "")
            {
                return "";
            }
            else
            {
                decimal value = decimal.Parse(amount);
                return "$" + value.ToString("#,##0.00");
            }
        }

        protected string TransformNumberFormat(string amount)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            

            if (amount == "")
            {
                return "";
            }
            else
            {
                decimal value = decimal.Parse(amount);
                return value.ToString("#,##0.##");        
            }
        }

        protected string TransformProjectFormat(string project)
        {
            var splittedProject = project.Split(' ', 2);
            return splittedProject[0] + " - " + splittedProject[1];
        }

        protected string TransformListToText(List<string> list)
        {
            string result = "";

            for (int i = 0; i < list.Count; i++)
            {
                if(i == list.Count -1)
                    result = result + list[i];
                else
                    result = result + list[i] + ", ";

            }

            return result;
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

        protected double TransformStringToDouble(string elementValue)
        {
            return double.Parse(elementValue, System.Globalization.CultureInfo.InvariantCulture);
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

        public void Dispose() => webDriver.Dispose();
    }
}
