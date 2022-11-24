

using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class Activities : PageObjectBase
    {
        private By activityTypeSelect = By.CssSelector("a[data-rb-event-key='activities']");
        private By activityAddBttn = By.CssSelector("div[class='col-md-4'] button");
        private By activity1stGeneralActBttn = By.XPath("//div[contains(text(),'General')]/parent::button");
        private By activitiesListTotal = By.CssSelector("div[class='tbody'] div[class='tr-wrapper']");


        public Activities(IWebDriver webDriver) : base(webDriver)
        {}

        public void CreateNewActivity()
        {
            Wait();
            ChooseSpecificSelectOption("input-activityTypeId", "General");
            webDriver.FindElement(activityAddBttn).Click();

            Wait();
            var lastActivityIndex = webDriver.FindElements(activitiesListTotal).Count();
            webDriver.FindElement(By.CssSelector("div[class='tbody'] div[class='tr-wrapper']:nth-child("+ lastActivityIndex +") div[role='row'] div[role='cell']:nth-child(1) button")).Click();
        }
            
    }
}
