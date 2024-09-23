using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PIMS.Tests.Automation.Classes;
using SeleniumExtras.WaitHelpers;

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

        protected virtual void Wait(int milliseconds = 2000) => Thread.Sleep(milliseconds);

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
                FocusAndClick(saveButton);
            }
            else
            {
                wait.Until(ExpectedConditions.ElementExists(cancelButton));
                wait.Until(ExpectedConditions.ElementToBeClickable(cancelButton));
                FocusAndClick(cancelButton);
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

        protected void AssertTrueContentEquals(By elementBy, string text = "")
        {
            WaitUntilVisible(elementBy);
            Assert.Equal(text, webDriver.FindElement(elementBy).Text);
        }

        protected void AssertTrueElementValueEquals(By elementBy, string text = "")
        {
            WaitUntilVisible(elementBy);
            Assert.Equal(text, webDriver.FindElement(elementBy).GetAttribute("Value"));
        }

        protected void AssertTrueContentNotEquals(By elementBy, string text)
        {
            WaitUntilVisible(elementBy);
            Assert.True(webDriver.FindElement(elementBy).Text != text);
        }

        protected void AssertTrueDoublesEquals(By elementBy, double number2)
        {
            
            WaitUntilVisible(elementBy);
            var numberFromElement = webDriver.FindElement(elementBy).GetAttribute("Value");
            var number1 = Math.Round(double.Parse(numberFromElement), 4, MidpointRounding.ToEven).ToString();
            var roundedNumber2 = Math.Round(number2, 4, MidpointRounding.ToEven).ToString();

            Assert.Equal(roundedNumber2, number1);
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

        protected string TransformAreaNumberFormat(string amount)
        {
            if (amount == "")
                return "";
            else
            {
                decimal value = decimal.Parse(amount);
                return value.ToString("#,##0.0000");        
            }
        }

        protected string TranformSqMtsFormat(string area)
        {
            if (area == "")
                return "";
            else
            {
                decimal value = decimal.Parse(area);
                return value.ToString("#,##0.####") + " m\r\n2";
            }
        }

        protected double TransformSqMtToSqFt(string sqmt)
        {
            double sqmtNbr = double.Parse(sqmt) * 10.76391041671;
            double sqftRounded = Math.Round(sqmtNbr, 4, MidpointRounding.ToEven);

            if (sqftRounded.Equals(0.0000))
                return 0;
            else
                return sqftRounded;
        }

        protected double TransformSqMtToHectares(string sqmt)
        {
            double sqmtNbr = double.Parse(sqmt) * 0.0001;
            double hectaresRounded = Math.Round(sqmtNbr, 4, MidpointRounding.ToEven);

            if (hectaresRounded.Equals(0.0000))
                return 0;
            else
                return hectaresRounded;
        }

        protected double TransformSqMtToAcres(string sqmt)
        {
            double sqmtNbr = double.Parse(sqmt) * 0.000247110891123302;
            double acresRounded = Math.Round(sqmtNbr, 4, MidpointRounding.ToEven);

            if (acresRounded.Equals(0.0000))
                return 0;
            else
                return acresRounded;
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

        protected string CalculateGSTDisplay(string GST)
        {
            return GST == "true" || GST == "" ? "Y" : "N";
        }

        protected string TransformBooleanFormat(string elementValue)
        {
            bool boolElementValue = bool.Parse(elementValue);
            return boolElementValue ? "Yes" : "No";
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

        protected string GetUppercaseString(string elementValue)
        {
            return elementValue.ToUpper();
        }

        protected string GetSubstring(string input, int startIndex, int endIndex)
        {
            return input.Substring(startIndex, endIndex - startIndex);
        }

        protected string CalculateExpiryCurrentDate(string originExpiryDate, List<LeaseRenewal> renewals)
        {
            var expiryDates = new List<DateTime>();
            if (originExpiryDate != "")
            {
                var originExpiryDateElement = DateTime.Parse(originExpiryDate);
                expiryDates.Add(originExpiryDateElement);

            }

            if (renewals.Count > 0)
            {
                for (var i = 0; i < renewals.Count; i++)
                {
                    if (renewals[i].RenewalIsExercised == "Yes")
                    {
                        var renewalExpiryDate = DateTime.Parse(renewals[i].RenewalExpiryDate);
                        expiryDates.Add(renewalExpiryDate);
                    }
                }
            }

            expiryDates.Sort((x, y) => y.CompareTo(x));
            System.Diagnostics.Debug.WriteLine(expiryDates);
            return expiryDates[0].ToString("MMM d, yyyy");
        }

        public void Dispose()
        {
            webDriver.Close();
            webDriver.Quit();
            webDriver.Dispose();
        }
    }
}
