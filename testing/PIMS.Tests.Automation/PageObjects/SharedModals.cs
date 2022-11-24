

using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SharedModals : PageObjectBase
    {
        private By generalModal = By.CssSelector("div[class='modal-content']");
        private By generalModalHeader = By.CssSelector("div[class='modal-header'] div");
        private By generalModalContent = By.CssSelector("div[class='modal-body']");
        private By generalModalOkBttn = By.CssSelector("button[title='ok-modal']");

        private By generalToastBody = By.CssSelector("div[class='Toastify__toast-body']");

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
    }
}
