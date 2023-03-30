

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

        private By generalToastBody = By.CssSelector("div[class='Toastify__toast-body']");
        private By generalConfirmationModalBody1 = By.CssSelector("div[class='modal-body'] div");
        private By generalConfirmationModalBody2 = By.CssSelector("div[class='modal-body'] strong");

        public SharedModals(IWebDriver webDriver) : base(webDriver)
        {}

        public string ModalHeader()
        {
            WaitUntil(generalModal);
            return (webDriver.FindElement(generalModalHeader).Text);
        }

        public string ModalContent()
        {
            WaitUntil(generalModal);
            return (webDriver.FindElement(generalModalContent).Text);
        }

        public void ModalClickOKBttn()
        {
            WaitUntil(generalModal);
            webDriver.FindElement(generalModalOkBttn).Click();
        }

        public string ToastifyText()
        {
            WaitUntil(generalToastBody);
            return (webDriver.FindElement(generalToastBody).Text);
        }

        public string ConfirmationModalText1()
        {
            WaitUntil(generalConfirmationModalBody1);
            return (webDriver.FindElement(generalConfirmationModalBody1).Text);
        }

        public string ConfirmationModalText2()
        {
            WaitUntil(generalConfirmationModalBody2);
            return (webDriver.FindElement(generalConfirmationModalBody2).Text);
        }

        public void VerifyButtonsPresence()
        {
            Wait();
            Assert.True(webDriver.FindElement(generalModalOkBttn).Displayed);
            Assert.True(webDriver.FindElement(generalModalCancelBttn).Displayed);
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
