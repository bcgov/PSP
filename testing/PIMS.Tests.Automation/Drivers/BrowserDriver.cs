using DotNetEnv;
using DotNetEnv.Configuration;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using Xunit.Abstractions;

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
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("start-maximized");
            options.AddExcludedArgument("enable-automation");
            options.AddArgument("--incognito");
            options.AddArgument("user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Firefox/91.0 Safari/537.36");

            if (runAutomationHeadless)
                options.AddArgument("--headless=new");

            var chromeDriver = new ChromeDriver(ChromeDriverService.CreateDefaultService(), options, TimeSpan.FromMinutes(2));
            chromeDriver.Url = Configuration.GetValue<string>("Base_url");

            return chromeDriver;
        }

        private IWebDriver CreateFirefoxWebDriver()
        {
            var options = new FirefoxOptions();
            if (runAutomationHeadless)
                options.AddArguments("--headless");
              

            var firefoxDriver = new FirefoxDriver(FirefoxDriverService.CreateDefaultService(), options);
            firefoxDriver.Manage().Window.Maximize();
            firefoxDriver.Url = Configuration.GetValue<string>("Base_url");

            return firefoxDriver;
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
                Current.Close();
                Current.Dispose();
                Current.Quit();

            }    
        }
    }
}
