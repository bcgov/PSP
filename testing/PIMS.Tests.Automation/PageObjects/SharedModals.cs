

using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SharedModals : PageObjectBase
    {
        private By generalModal = By.CssSelector("div[class='modal-content']");
        private By generalModalHeader = By.CssSelector("div[class='modal-header'] div");
        private By generalModalContent = By.CssSelector("div[class='modal-body']");
        private By generalModalOkBttn = By.CssSelector("button[title='ok-modal']");
        private By generalModalCancelBttn = By.CssSelector("button[title='cancel-modal']");

        private By secondaryModal = By.XPath("//div[@role='dialog'][2]/div[@class='modal-dialog']");
        private By secondaryModalHeader = By.XPath("//div[@role='dialog'][2]/div/div/div[@class='modal-header']/div");
        private By secondaryModalContent = By.XPath("//div[@role='dialog'][2]/div/div/div[@class='modal-body']");
        private By secondaryModalOkBttn = By.XPath("//div[@role='dialog'][2]/div/div/div[@class='modal-footer']/button[@title='ok-modal']");
        private By secondaryModalCancelBttn = By.XPath("//div[@role='dialog'][2]/div/div/div[@class='modal-footer']/button[@title='cancel-modal']");

        private By generalToastBody = By.CssSelector("div[class='Toastify__toast-body']");
        private By generalConfirmationModalBody1 = By.CssSelector("div[class='modal-body'] div");
        private By generalConfirmationModalBody2 = By.CssSelector("div[class='modal-body'] strong");

        public SharedModals(IWebDriver webDriver) : base(webDriver)
        {}

        public string ModalHeader()
        {
            WaitUntilVisible(generalModal);
            return (webDriver.FindElement(generalModalHeader).Text);
        }

        public string ModalContent()
        {
            WaitUntilVisible(generalModal);
            return (webDriver.FindElement(generalModalContent).Text);
        }

        public void ModalClickOKBttn()
        {
            WaitUntilVisible(generalModal);
            webDriver.FindElement(generalModalOkBttn).Click();
        }

        public string SecondaryModalHeader()
        {
            WaitUntilVisible(secondaryModal);
            return (webDriver.FindElement(secondaryModalHeader).Text);
        }

        public string SecondaryModalContent()
        {
            WaitUntilVisible(secondaryModal);
            return (webDriver.FindElement(secondaryModalContent).Text);
        }

        public void SecondaryModalClickOKBttn()
        {
            WaitUntilVisible(secondaryModal);
            webDriver.FindElement(secondaryModalOkBttn).Click();
        }

        public string ToastifyText()
        {
            WaitUntilVisible(generalToastBody);
            return (webDriver.FindElement(generalToastBody).Text);
        }

        public string ConfirmationModalText1()
        {
            WaitUntilVisible(generalConfirmationModalBody1);
            return (webDriver.FindElement(generalConfirmationModalBody1).Text);
        }

        public string ConfirmationModalText2()
        {
            WaitUntilVisible(generalConfirmationModalBody2);
            return (webDriver.FindElement(generalConfirmationModalBody2).Text);
        }

        public void VerifyButtonsPresence()
        {
            AssertTrueIsDisplayed(generalModalOkBttn);
            AssertTrueIsDisplayed(generalModalCancelBttn);
        }

        public void IsToastyPresent()
        {
            AssertTrueIsDisplayed(generalToastBody);
        }

        public void SiteMinderModal()
        {
            Wait();
            if (webDriver.FindElements(generalModal).Count > 0 && ModalHeader() == "SiteMinder Session Expired")
            {
                webDriver.FindElement(generalModalCancelBttn).Click();
            }
        }
    }
}
