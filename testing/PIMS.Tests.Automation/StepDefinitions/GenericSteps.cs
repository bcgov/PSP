using OpenQA.Selenium;

namespace PIMS.Tests.Automation.StepDefinitions
{
    [Binding]
    public class GenericSteps
    {
        private readonly IWebDriver webDriver;

        public GenericSteps(BrowserDriver driver)
        {
            webDriver = driver.Current;
        }

        [Then(@"I am on path (.*)")]
        public void CurrentLocation(string path)
        {
            new Uri(webDriver.Url).AbsolutePath.Should().Be(path);
        }

        [When(@"I navigate to path (.*)")]
        public void Navigate(string path)
        {
            webDriver.Url = path;
            webDriver.Navigate();
        }

        public List<string> PopulateLists(string stringToList)
        {
            List<string> result = stringToList.Split(',').ToList();
            result.Sort();
            return result;
        }
    }
}
