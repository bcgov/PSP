﻿using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.IO;
using System.Xml.Linq;

namespace PIMS.Tests.Automation.PageObjects
{
    public abstract class PageObjectBase
    {
        protected readonly IWebDriver webDriver;
        private WebDriverWait wait;
        private DefaultWait<IWebDriver> fluentWait;

        private By loadingSpinner = By.CssSelector("div[data-testid='filter-backdrop-loading']");

        protected PageObjectBase(IWebDriver webDriver)
        {
            this.webDriver = webDriver;
            wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(60));
            fluentWait = new DefaultWait<IWebDriver>(webDriver);
            fluentWait.Timeout = TimeSpan.FromSeconds(60);
            fluentWait.PollingInterval = TimeSpan.FromMilliseconds(100);
        }

        protected virtual void Wait(int milliseconds = 1000) => Thread.Sleep(milliseconds);

        protected void WaitUntilSpinnerDisappear()
        {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(loadingSpinner));
        }

        protected void WaitUntilStale(By element)
        {
            fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
            fluentWait.Until(ExpectedConditions.StalenessOf(webDriver.FindElement(element)));
        }

        protected void WaitUntilVisible(By element)
        {
            fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
            fluentWait.Until(ExpectedConditions.ElementIsVisible(element));
        }

        protected void WaitUntilClickable(By element)
        {
            fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException), typeof(ElementClickInterceptedException));
            wait.Until(ExpectedConditions.ElementToBeClickable(element));
        }

        public void WaitUntilVisibleText(By element, string text)
        {
            var webElement = webDriver.FindElement(element);
            fluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
            fluentWait.Until(ExpectedConditions.TextToBePresentInElement(webElement, text));
        }

        protected void ButtonElement(string btnContent)
        {
            Wait(5000);

            var js = (IJavaScriptExecutor)webDriver;

            var buttons = webDriver.FindElements(By.TagName("button"));

            var selectedBtn = buttons.Should().ContainSingle(b => b.Text.Contains(btnContent)).Subject;
            selectedBtn.Click();

            //try
            //{
            //    var selectedBtn = buttons.Should().ContainSingle(b => b.Text.Contains(btnContent)).Subject;
            //    selectedBtn.Click();
            //}
            //catch(Exception e)
            //{
            //    var selectedBtn = buttons.Should().ContainSingle(b => b.Text.Contains(btnContent)).Subject;
            //    selectedBtn.Click();
            //}
        }

        protected void FocusAndClick(By element)
        {
            Wait();

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
