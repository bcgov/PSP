using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SharedImprovementsTab : PageObjectBase
    {
        private readonly By sharedImprovementTab = By.CssSelector("a[data-rb-event-key='improvements']");
        private readonly By sharedImprovementTitle = By.XPath("//div[contains(text(),'Improvements')]");
        private readonly By sharedImprovementTooltip = By.XPath("//p[contains(text(),'Click on a property to edit that property improvements in a new tab')]");
        private readonly By sharedImprovementExpandBttn = By.XPath("//div[contains(text(),'Improvements')]/following-sibling::div");

        private readonly By sharedImprovementsPropertiesCount = By.CssSelector("div[data-testid*='property-improvements']");



        public SharedImprovementsTab(IWebDriver webDriver) : base(webDriver)
        {}

        public void NavigateImprovementTab()
        {
            WaitUntilClickable(sharedImprovementTab);
            webDriver.FindElement(sharedImprovementTab).Click();
        }

        public void VerifyImprovementsTab(List<string> propertiesList)
        {
            WaitUntilVisible(sharedImprovementTitle);
            AssertTrueIsDisplayed(sharedImprovementTitle);

            webDriver.FindElement(sharedImprovementExpandBttn).Click();
            AssertTrueIsDisplayed(sharedImprovementTooltip);

            var propertiesSectors = webDriver.FindElements(sharedImprovementsPropertiesCount);

            for (int i = 0; i < propertiesList.Count; i++)
            {
                for (int n = 0; n < propertiesSectors.Count; n++)
                {
                    var propertySector = webDriver.FindElements(sharedImprovementsPropertiesCount)[n];
                    var propertySubtitle = propertySector.FindElement(By.CssSelector("h2 div div div"));
                    if (propertySubtitle.Text.Contains(propertiesList[i]))
                    {
                        Assert.Contains(propertiesList[i], propertySubtitle.Text);
                        break;
                    }
                        
                }
               
                
            }
                
        }

        public int CountProperties()
        {
            WaitUntilVisible(sharedImprovementsPropertiesCount);
            return webDriver.FindElements(sharedImprovementsPropertiesCount).Count;
        }
    }
}
