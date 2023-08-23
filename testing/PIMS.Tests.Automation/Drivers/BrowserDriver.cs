using DotNetEnv;
using DotNetEnv.Configuration;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;

namespace PIMS.Tests.Automation.Drivers
{
    public class BrowserDriver : IDisposable
    {
        private readonly Lazy<IWebDriver> currentWebDriverLazy;
        private readonly Lazy<IConfiguration> configurationLazy;
        private readonly bool closeBrowserOnDispose;
        private readonly bool runAutomationHeadless;

        public BrowserDriver()
        {
            currentWebDriverLazy = new Lazy<IWebDriver>(CreateChromeWebDriver);
            configurationLazy = new Lazy<IConfiguration>(ReadConfiguration);
            closeBrowserOnDispose = Configuration.GetValue("CloseBrowserAfterEachTest", true);
            runAutomationHeadless = Configuration.GetValue("RunHeadless", true);
        }

        public IWebDriver Current => currentWebDriverLazy.Value;

        public IConfiguration Configuration => configurationLazy.Value;

        private IWebDriver CreateChromeWebDriver()
        {
            var options = new ChromeOptions();
            if (runAutomationHeadless)
            {
                options.AddArguments("window-size=1920,1080", "headless", "no-sandbox");
            }
            else
            {
                options.AddArguments("start-maximized");
            }
            var chromeDriver = new ChromeDriver(ChromeDriverService.CreateDefaultService(), options);
            chromeDriver.Url = Configuration.GetValue<string>("Base_url");

            return chromeDriver;
        }

        private IWebDriver CreateEdgeWebDriver()
        {
            var options = new EdgeOptions();
            if (runAutomationHeadless)
            {
                options.AddArguments("window-size=1920,1080", "headless");
            }
            else
            {
                options.AddArguments("start-maximized");
            }

            var edgeDriver = new EdgeDriver(EdgeDriverService.CreateDefaultService(), options, TimeSpan.FromMinutes(3));
            edgeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(120);
            edgeDriver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(120);
            edgeDriver.Url = Configuration.GetValue<string>("Base_url");

            return edgeDriver;
        }

        private IConfiguration ReadConfiguration() =>
            new ConfigurationBuilder()
                .AddUserSecrets<BrowserDriver>()
                .AddDotNetEnv(".env", LoadOptions.TraversePath())
                .AddEnvironmentVariables()
                .Build();

        public void Dispose()
        {
            if (currentWebDriverLazy.IsValueCreated && closeBrowserOnDispose)
            {
                Current.Quit();
            }
        }
    }
}
