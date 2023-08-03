using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Xml.Linq;

namespace PIMS.Tests.Automation.PageObjects
{
    public class HelpDesk : PageObjectBase
    {
        //Help Desk Main Button
        private By mainMenuHelpDeskBttn = By.CssSelector("div[class='nav-item'] svg");

        //Modal Header
        private By mainMenuHeader = By.XPath("//div[contains(text(),'Help Desk')]");
        private By mainMenuHelpIcon = By.CssSelector("span[data-testid='tooltip-icon-help-toolTip']");

        //Information Options
        private By helpDeskMapButton = By.XPath("//div[contains(text(),'Map')]/parent::button");
        private By helpDeskFilterButton = By.XPath("//div[contains(text(),'Filter')]/parent::button");
        private By helpDeskNavigationButton = By.XPath("//div[contains(text(),'Navigation')]/parent::button");
        private By helpDeskInfoContent = By.CssSelector("span[class='col-md-8'] p");

        //Help Desk Input Options Elements
        private By helpDeskQuestionInput = By.Id("ticket-Question");
        private By helpDeskBugInput = By.Id("ticket-Bug");
        private By helpDeskFeatureInput = By.Id("ticket-Feature Request");

        //Forms Elements
        private By userLabel = By.CssSelector("label[for='input-user']");
        private By userInput = By.Id("input-user");
        private By emailLabel = By.CssSelector("label[for='input-email']");
        private By emailInput = By.Id("input-email");
        private By pageLabel = By.CssSelector("label[for='input-page']");
        private By pageInput = By.Id("input-page");
        private By questionLabel = By.CssSelector("label[for='input-question']");
        private By questionInput = By.Id("input-question");
        private By stepsLabel = By.CssSelector("label[for='input-stepsToReproduce']");
        private By stepsInput = By.Id("input-stepsToReproduce");
        private By expectedResultsLabel = By.CssSelector("label[for='input-expectedResult']");
        private By expectedResultsInput = By.Id("input-expectedResult");
        private By actualResultLabel = By.CssSelector("label[for='input-actualResult']");
        private By actualResultInput = By.Id("input-actualResult");
        private By descriptionLabel = By.CssSelector("label[for='input-description']");
        private By descriptionInput = By.Id("input-description");

        //Help Desk Buttons Elements
        private By submitButton = By.XPath("//div[contains(text(), 'Submit')]/parent::a");
        private By cancelButton = By.XPath("//div[contains(text(), 'Cancel')]/parent::button");

        public HelpDesk(IWebDriver webDriver) : base(webDriver)
        {}

        public void NavigateToHelpDesk()
        {
            WaitUntilVisible(mainMenuHelpDeskBttn);
            webDriver.FindElement(mainMenuHelpDeskBttn).Click();
        }

        public void VerifyHelpDeskHeader()
        {
            WaitUntilVisible(mainMenuHeader);
            Assert.True(webDriver.FindElement(mainMenuHeader).Displayed);
            Assert.True(webDriver.FindElement(mainMenuHelpIcon).Displayed);
        }

        public void VerifyMapHelpDesk()
        {
            WaitUntilClickable(helpDeskMapButton);
            webDriver.FindElement(helpDeskMapButton).Click();
        }

        public void VerifyFilterHelpDesk()
        {
            WaitUntilClickable(helpDeskFilterButton);
            webDriver.FindElement(helpDeskFilterButton).Click();
        }

        public void VerifyNavigationHelpDesk()
        {
            WaitUntilClickable(helpDeskNavigationButton);
            webDriver.FindElement(helpDeskNavigationButton).Click();
        }

        public void VerifyQuestionForm()
        {
            WaitUntilClickable(helpDeskQuestionInput);
            webDriver.FindElement(helpDeskQuestionInput).Click();

            WaitUntilVisible(userLabel);
            Assert.True(webDriver.FindElement(userLabel).Displayed);
            Assert.True(webDriver.FindElement(userInput).Displayed);
            Assert.True(webDriver.FindElement(emailLabel).Displayed);
            Assert.True(webDriver.FindElement(emailInput).Displayed);
            Assert.True(webDriver.FindElement(pageLabel).Displayed);
            Assert.True(webDriver.FindElement(pageInput).Displayed);
            Assert.True(webDriver.FindElement(questionLabel).Displayed);
            Assert.True(webDriver.FindElement(questionInput).Displayed);
        }

        public void VerifyBugForm()
        {
            WaitUntilClickable(helpDeskBugInput);
            webDriver.FindElement(helpDeskBugInput).Click();

            WaitUntilVisible(userLabel);
            Assert.True(webDriver.FindElement(userLabel).Displayed);
            Assert.True(webDriver.FindElement(userInput).Displayed);
            Assert.True(webDriver.FindElement(emailLabel).Displayed);
            Assert.True(webDriver.FindElement(emailInput).Displayed);
            Assert.True(webDriver.FindElement(pageLabel).Displayed);
            Assert.True(webDriver.FindElement(pageInput).Displayed);
            Assert.True(webDriver.FindElement(stepsLabel).Displayed);
            Assert.True(webDriver.FindElement(stepsInput).Displayed);
            Assert.True(webDriver.FindElement(expectedResultsLabel).Displayed);
            Assert.True(webDriver.FindElement(expectedResultsInput).Displayed);
            Assert.True(webDriver.FindElement(actualResultLabel).Displayed);
            Assert.True(webDriver.FindElement(actualResultInput).Displayed);
        }

        public void VerifyFeatureForm()
        {
            WaitUntilClickable(helpDeskFeatureInput);
            webDriver.FindElement(helpDeskFeatureInput).Click();

            WaitUntilVisible(userLabel);
            Assert.True(webDriver.FindElement(userLabel).Displayed);
            Assert.True(webDriver.FindElement(userInput).Displayed);
            Assert.True(webDriver.FindElement(emailLabel).Displayed);
            Assert.True(webDriver.FindElement(emailInput).Displayed);
            Assert.True(webDriver.FindElement(descriptionLabel).Displayed);
            Assert.True(webDriver.FindElement(descriptionInput).Displayed);
        }

        public void VerifyButtons()
        {
            WaitUntilVisible(submitButton);
            Assert.True(webDriver.FindElement(submitButton).Displayed);
            Assert.True(webDriver.FindElement(cancelButton).Displayed);
        }

        public void CancelHelpDeskModal()
        {
            WaitUntilClickable(cancelButton);
            webDriver.FindElement(cancelButton).Click();
        }
    }
}
