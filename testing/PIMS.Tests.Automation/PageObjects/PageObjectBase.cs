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
        protected By propertiesSpinner = By.CssSelector("div[data-testid='filter-backdrop-loading']");
        protected By saveButton = By.XPath("//button/div[contains(text(),'Save')]");
        protected By cancelButton = By.XPath("//button/div[contains(text(),'Cancel')]");

        protected PageObjectBase(IWebDriver webDriver)
        {
            this.webDriver = webDriver;
            wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(60));
        }

        protected virtual void Wait(int milliseconds = 1000) => Thread.Sleep(milliseconds);

        protected void WaitUntilSpinnerDisappear()
        {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(loadingSpinner));
        }

        protected void WaitForTableToLoad(By rowLocator)
        {
            wait.IgnoreExceptionTypes(
                typeof(StaleElementReferenceException),
                typeof(NoSuchElementException)
            );

            // Wait until spinner disappears
            wait.Until(driver =>
            {
                var spinners = driver.FindElements(tableLoadingSpinner);
                return spinners.Count == 0 || spinners.All(s => !s.Displayed);
            });

            // Optional: wait until table rows are present or stable
            wait.Until(driver =>
            {
                var rows = driver.FindElements(rowLocator);
                return rows.Count >= 0;
            });
        }

        protected void WaitForTableToLoad()
        {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(tableLoadingSpinner));
        }

        protected void WaitUntilDisappear(By element) => wait.Until(ExpectedConditions.InvisibilityOfElementLocated(element));

        protected void WaitUntilVisible(By element)
        {
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(NoSuchElementException));

            wait.Until(d =>
            {
                var locator = webDriver.FindElement(element);

                if (locator.Displayed)
                {
                    return true;
                }

                return false;
            });
        }

        protected IWebElement WaitUntilClickable(By by)
        {
            wait.IgnoreExceptionTypes(
                typeof(StaleElementReferenceException),
                typeof(NoSuchElementException),
                typeof(ElementClickInterceptedException)
            );

            return wait.Until(webDriver =>
            {
                var element = webDriver.FindElement(by);
                return (element.Displayed && element.Enabled) ? element : null;
            });
        }

        protected void SafeClick(By by)
        {
            wait.PollingInterval = TimeSpan.FromMilliseconds(200);

            wait.IgnoreExceptionTypes(
                typeof(NoSuchElementException),
                typeof(StaleElementReferenceException)
            );

            var js = (IJavaScriptExecutor)webDriver;

            wait.Until(driver =>
            {
                var element = driver.FindElement(by);

                if (!element.Displayed || !element.Enabled)
                    return false;

                js.ExecuteScript(
                    @"arguments[0].scrollIntoView({ block: 'center', inline: 'center' });",
                    element);

                var clickable = (bool)js.ExecuteScript(@"
                    const el = arguments[0];
                    const rect = el.getBoundingClientRect();

                    if (rect.width === 0 || rect.height === 0) return false;

                    const x = rect.left + rect.width / 2;
                    const y = rect.top + rect.height / 2;

                    const topEl = document.elementFromPoint(x, y);
                    return topEl === el || el.contains(topEl);
                    ", element);

                if (!clickable)
                    return false;

                try
                {
                    element.Click();
                    return true;
                }
                catch (ElementClickInterceptedException)
                {
                    return false;
                }
            });
        }

        public void WaitUntilVisibleText(By element, string text)
        {
            wait.IgnoreExceptionTypes(
                typeof(StaleElementReferenceException),
                typeof(NoSuchElementException)
            );

            // Wait until the parent select is visible
            wait.Until(webDriver =>
            {
                var parent = webDriver.FindElement(element);
                return parent.Displayed ? parent : null;
            });
            var webElement = webDriver.FindElement(element);

            wait.Until(ExpectedConditions.TextToBePresentInElement(webElement, text));
        }

        protected void ButtonElement(string buttonName)
        {
            var js = (IJavaScriptExecutor)webDriver;

            if (buttonName == "Save")
            {
                SafeClick(saveButton);
            }
            else
            {
                SafeClick(cancelButton);
            }
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

        protected void ChooseSelectOption(By parentElement, string option)
        {
            wait.IgnoreExceptionTypes(
                typeof(StaleElementReferenceException),
                typeof(NoSuchElementException)
            );

            // Wait until the parent select is visible
            wait.Until(webDriver =>
            {
                var parent = webDriver.FindElement(parentElement);
                return parent.Displayed ? parent : null;
            });

            // Wait until the desired option exists under the parent
            wait.Until(webDriver =>
            {
                var parent = webDriver.FindElement(parentElement);
                var options = parent.FindElements(By.TagName("option"));

                return options.Any(o => o.Text.Trim().Equals(option, StringComparison.OrdinalIgnoreCase));
            });

            // Re-locate after the wait to avoid stale references
            var selectElement = webDriver.FindElement(parentElement);
            var selectedOption = selectElement
                .FindElements(By.TagName("option"))
                .Single(o => o.Text.Trim().Equals(option, StringComparison.OrdinalIgnoreCase));

            ((IJavaScriptExecutor)webDriver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", selectedOption);
            selectedOption.Click();
        }

        protected void ChooseRadioButton(By parentName, string option)
        {
            var js = (IJavaScriptExecutor)webDriver;

            var childrenElements = webDriver.FindElements(parentName);
            var selectedOption = childrenElements.Should().ContainSingle(o => o.GetDomProperty("value").Equals(option)).Subject;

            System.Diagnostics.Debug.WriteLine(selectedOption);

            js.ExecuteScript("arguments[0].scrollIntoView();", selectedOption);
            js.ExecuteScript("arguments[0].click();", selectedOption);
        }

        protected void ChooseMultiSelectOption(By openLocator, By optionsContainerLocator, By closeLocator, string optionText)
        {
            var js = (IJavaScriptExecutor)webDriver;

            SafeClick(closeLocator);

            Wait();
            SafeClick(openLocator);

            var optionElement = wait.Until(driver =>
            {
                var container = driver.FindElement(optionsContainerLocator);

                var items = container.FindElements(By.TagName("li"))
                    .Where(x => x.Displayed)
                    .ToList();

                return items.FirstOrDefault(x =>
                    !string.IsNullOrWhiteSpace(x.Text) &&
                    x.Text.Trim().Equals(optionText.Trim(), StringComparison.OrdinalIgnoreCase));
            });

            if (optionElement == null)
            {
                throw new NoSuchElementException(
                    $"Could not find option '{optionText}' in multiselect.");
            }

            js.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", optionElement);
            optionElement.Click();

            SafeClick(closeLocator);
        }

        protected void ClearInput(By elementBy)
        {
            WaitUntilClickable(elementBy);

            var element = webDriver.FindElement(elementBy);
            while (!element.GetDomProperty("value").Equals(""))
            {
                element.SendKeys(Keys.Backspace);
            }
        }

        protected void ClearDigitsInput(By elementBy)
        {
            var element = webDriver.FindElement(elementBy);
            while (!element.GetDomProperty("value").Equals("0"))
            {
                element.SendKeys(Keys.Backspace);
            }
        }

        protected void CleanUpCurrencyInput(By elementBy)
        {
            var element = webDriver.FindElement(elementBy);
            int charCounter = 1;

            //Validate if the field has a value
            if (element.GetAttribute("value") != "$0.00")
                charCounter = element.GetAttribute("value").Length;

            element.Click();

            for (var i = 0; i < charCounter; i++)
                element.SendKeys(Keys.Delete);
        }

        protected void SendKeysToCurrencyInput(By elementBy, string digits)
        {
            var digitCharacters = digits.ToCharArray();

            foreach (var digit in digitCharacters)
            {
                switch (digit)
                {
                    case '0':
                        webDriver.FindElement(elementBy).SendKeys(Keys.NumberPad0);
                        break;
                    case '1':
                        webDriver.FindElement(elementBy).SendKeys(Keys.NumberPad1);
                        break;
                    case '2':
                        webDriver.FindElement(elementBy).SendKeys(Keys.NumberPad2);
                        break;
                    case '3':
                        webDriver.FindElement(elementBy).SendKeys(Keys.NumberPad3);
                        break;
                    case '4':
                        webDriver.FindElement(elementBy).SendKeys(Keys.NumberPad4);
                        break;
                    case '5':
                        webDriver.FindElement(elementBy).SendKeys(Keys.NumberPad5);
                        break;
                    case '6':
                        webDriver.FindElement(elementBy).SendKeys(Keys.NumberPad6);
                        break;
                    case '7':
                        webDriver.FindElement(elementBy).SendKeys(Keys.NumberPad7);
                        break;
                    case '8':
                        webDriver.FindElement(elementBy).SendKeys(Keys.NumberPad8);
                        break;
                    case '9':
                        webDriver.FindElement(elementBy).SendKeys(Keys.NumberPad9);
                        break;
                    case '.':
                        webDriver.FindElement(elementBy).SendKeys(Keys.Decimal);
                        break;
                }
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

        protected void AssertTrueContentEquals(By elementBy, string expectedText = "")
        {
            wait.IgnoreExceptionTypes(
                typeof(StaleElementReferenceException),
                typeof(NoSuchElementException)
            );

            bool textMatched = wait.Until(webDriver =>
            {
                var element = webDriver.FindElement(elementBy);
                return element.Displayed && element.Text.Trim().Equals(expectedText.Trim(), StringComparison.Ordinal);
            });

            Assert.True(
                textMatched,
                $"Expected element '{elementBy}' text to equal '{expectedText}'."
            );
        }

        protected void AssertTrueElementValueEquals(By elementBy, string text = "")
        {
            WaitUntilVisible(elementBy);
            Assert.Equal(text, webDriver.FindElement(elementBy).GetDomProperty("value"));
        }

        protected void AssertTrueContentNotEquals(By elementBy, string text)
        {
            WaitUntilVisible(elementBy);
            Assert.True(webDriver.FindElement(elementBy).Text != text);
        }

        protected void AssertTrueDoublesEquals(By elementBy, double number2)
        {

            WaitUntilVisible(elementBy);
            var numberFromElement = webDriver.FindElement(elementBy).GetDomProperty("value");
            var number1 = Math.Round(double.Parse(numberFromElement), 4, MidpointRounding.ToEven).ToString();
            var roundedNumber2 = Math.Round(number2, 4, MidpointRounding.ToEven).ToString();

            Assert.Equal(roundedNumber2, number1);
        }

        protected void AssertTrueElementContains(By elementBy, string text)
        {
            WaitUntilVisible(elementBy);
            Assert.Contains(text, webDriver.FindElement(elementBy).Text);
        }

        protected void AssertTrueElementContainsAnyOf(By elementBy, IEnumerable<string> texts)
        {
            WaitUntilVisible(elementBy);

            string elementText = webDriver.FindElement(elementBy).Text;
            bool isContained = texts.Any(text => elementText.Contains(text));

            Assert.True(isContained);
        }

        protected string TransformDateFormat(string date)
        {
            if (date == "")
                return "";
            else
            {
                var dateObject = DateTime.Parse(date);
                return dateObject.ToString("MMM d, yyyy");
            }
        }

        protected string TransformCurrencyFormat(string amount)
        {
            if (amount == "")
                return "$0.00";
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

        protected static string TransformListToText(List<string> list)
        {
            string result = "";

            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count -1)
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
                result.Add(projectName.Text);
            
            result.Sort();
            return result;
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
                result.Add(childElement.Text);
            
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

            if (originExpiryDate == "" && renewals.Count == 0) return "";
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
    }
}   
