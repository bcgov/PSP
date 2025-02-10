using DotNetEnv;
using DotNetEnv.Configuration;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using PIMS.Tests.Automation.Reports;

namespace PIMS.Tests.Automation.StepDefinitions
{
    [Binding]
    public class GenericSteps
    {
        private readonly IWebDriver webDriver;

        public GenericSteps(IWebDriver driver)
        {
            this.webDriver = driver;
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

        public IConfiguration ReadConfiguration() =>
           new ConfigurationBuilder()
               .AddUserSecrets<TestHooks>()
               .AddDotNetEnv(".env", LoadOptions.TraversePath())
               .AddEnvironmentVariables()
               .Build();
    }
}

