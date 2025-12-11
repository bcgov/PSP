using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class HelpDesk : PageObjectBase
    {
        //Help Desk Main Button
        private readonly By mainMenuHelpDeskBttn = By.CssSelector("header div[class='nav-item'] div");

        //Modal Header
        private readonly By mainMenuHeader = By.CssSelector("div[class='modal-title h4']");

        //Modal Content
        private readonly By helpDeskSubtitle = By.CssSelector("div[class='modal-body'] h3:first-child");
        private readonly By helpDeskDescription = By.XPath("//p[contains(text(),'This overview has useful tools that will support you to start using the application. You can also watch the video demos.')]");
        private readonly By helpDeskPIMSResourcesLink = By.XPath("//a[contains(text(),'PIMS Training Materials')]");

        private readonly By helpDeskContactUsSubtitle = By.XPath("//h3[contains(text(),'Contact us')]");
        private readonly By helpDeskUserLabel = By.XPath("//label[contains(text(),'Name')]");
        private readonly By helpDeskUserInput = By.Id("input-user");
        private readonly By helpDeskEmailLabel = By.XPath("//label[contains(text(),'Email')]");
        private readonly By helpDeskEmailInput = By.Id("input-email");
        private readonly By helpDeskDescriptionLabel = By.XPath("//div[@class='modal-body']/div/form/div/div/label[contains(text(),'Description')]");
        private readonly By helpDeskDescriptionInput = By.Id("input-description");
        private readonly By helpDeskFooterQuestion = By.XPath("//p[contains(text(),'Do you want to proceed and send the email?')]");

        //Help Desk Buttons Elements
        private readonly By noButton = By.CssSelector("button[data-testid='cancel-modal-button']");
        private readonly By yesButton = By.CssSelector("div[class='modal-footer'] a[data-testid='ok-modal-button']");

        public HelpDesk(IWebDriver webDriver) : base(webDriver)
        {}

        public void NavigateToHelpDesk()
        {
            Wait();
            FocusAndClick(mainMenuHelpDeskBttn);
        }

        public void VerifyHelpDeskModal()
        {
            Wait();

            AssertTrueContentEquals(mainMenuHeader, "Help Desk");

            AssertTrueContentEquals(helpDeskSubtitle, "Get started with PIMS");
            AssertTrueIsDisplayed(helpDeskDescription);
            AssertTrueIsDisplayed(helpDeskPIMSResourcesLink);

            AssertTrueIsDisplayed(helpDeskContactUsSubtitle);
            AssertTrueIsDisplayed(helpDeskUserLabel);
            AssertTrueIsDisplayed(helpDeskUserInput);
            AssertTrueIsDisplayed(helpDeskEmailLabel);
            AssertTrueIsDisplayed(helpDeskEmailInput);
            AssertTrueIsDisplayed(helpDeskDescriptionLabel);                           
            AssertTrueIsDisplayed(helpDeskDescriptionInput);
            AssertTrueIsDisplayed(helpDeskFooterQuestion);

            AssertTrueIsDisplayed(noButton);
            AssertTrueIsDisplayed(yesButton);
        }

        public void CancelHelpDeskModal()
        {
            WaitUntilClickable(noButton);
            webDriver.FindElement(noButton).Click();
        }
    }
}
