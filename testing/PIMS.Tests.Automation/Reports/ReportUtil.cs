using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Config;
using OpenQA.Selenium;

namespace PIMS.Tests.Automation.Reports
{
    public class ReportUtil
    {
        public static ExtentReports _extentReports;
        public static ExtentTest _feature;
        public static ExtentTest _scenario;

        public static String dir = AppDomain.CurrentDomain.BaseDirectory;
        public static String reportPath = dir.Replace("bin\\Debug\\net8.0", "Reports\\Extent_Reports");


        public static void ExtentReportInit()
        {
            ExtentSparkReporter spark = new ExtentSparkReporter(reportPath + "\\PIMS_AutomationReport.html");
            spark.Config.ReportName = "Automation Status Report";
            spark.Config.DocumentTitle = "Automation Status Report";
            spark.Config.Theme = Theme.Standard;

            _extentReports = new ExtentReports();
            _extentReports.AttachReporter(spark);
            _extentReports.AddSystemInfo("Application", "MOTI PIMS");
            _extentReports.AddSystemInfo("Browser", "Chrome");
            _extentReports.AddSystemInfo("OS", "Windows");
        }

        public static void ExtentReportTearDown()
        {
            _extentReports.Flush();
        }

        public string addScreenshot(IWebDriver driver, ScenarioContext scenarioContext)
        {
            ITakesScreenshot takesScreenshot = (ITakesScreenshot)driver;
            Screenshot screenshot = takesScreenshot.GetScreenshot();
            string screenshotLocation = Path.Combine(reportPath, scenarioContext.ScenarioInfo.Title + ".png");
            screenshot.SaveAsFile(screenshotLocation);
            return screenshotLocation;
        }
    }
}
