using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SharedImprovementsTab : PageObjectBase
    {
        private readonly By sharedImprovementTab = By.CssSelector("a[data-rb-event-key='improvements']");
        private readonly By sharedImprovementTitle = By.XPath("//div[contains(text(),'Improvements')]");
        private readonly By sharedImprovementTooltip = By.XPath("//p[contains(text(),'Click on a property to edit that property improvements in a new tab')]");

        private readonly By sharedImprovementsPropertiesCount = By.CssSelector("div[data-testid*='property-improvements']");



        public SharedImprovementsTab(IWebDriver webDriver) : base(webDriver)
        {}

        public void NavigateImprovementTab()
        {
            Wait();
            webDriver.FindElement(sharedImprovementTab).Click();
        }

        public void VerifyImprovementsTab(List<string> propertiesList)
        {
            Wait();

            AssertTrueIsDisplayed(sharedImprovementTitle);
            AssertTrueIsDisplayed(sharedImprovementTooltip);

            for (int i = 0; i < propertiesList.Count; i++)
                AssertTrueContentEquals(By.CssSelector("div[data-testid*='property-improvements"+ i +"'] h2 div div div"), propertiesList[i]);
        }

        public int CountProperties()
        {
            return webDriver.FindElements(sharedImprovementsPropertiesCount).Count;
        }
    }
}
