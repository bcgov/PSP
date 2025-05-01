

using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SharedModals : PageObjectBase
    {
        private By generalModal = By.CssSelector("div[class='modal-content']");
        private By generalModalHeader = By.CssSelector("div[class='modal-header'] div[class='modal-title h4']");
        private By generalModalContent = By.CssSelector("div[class='modal-body']");
        private By generalModalOkBttn = By.CssSelector("button[title='ok-modal']");
        private By generalModalCancelBttn = By.CssSelector("button[title='cancel-modal']");

        private By secondaryModal = By.XPath("//div[@role='dialog'][2]/div/div[@class='modal-content']");
        private By secondaryModalHeader = By.XPath("//div[@role='dialog'][2]/div/div/div[@class='modal-header']/div[2]");
        private By secondaryModalContent = By.XPath("//div[@role='dialog'][2]/div/div/div[@class='modal-body']");
        private By secondaryModalOkBttn = By.XPath("//div[@role='dialog'][2]/div/div/div[@class='modal-footer']/div/button[@title='ok-modal']");
        private By secondaryModalCancelBttn = By.XPath("//div[@role='dialog'][2]/div/div/div[@class='modal-footer']/div/button[@title='cancel-modal']");

        private By generalToastBody = By.CssSelector("div[class='Toastify__toast-body']");
        private By generalConfirmationModalBody1 = By.CssSelector("div[class='modal-body'] div");
        private By generalConfirmationModalBody2 = By.CssSelector("div[class='modal-body'] strong");
        private By generalConfirmationModalParagraph1 = By.CssSelector("div[class='modal-body'] p:first-child");
        private By generalConfirmationModalParagraph2 = By.CssSelector("div[class='modal-body'] p:last-child");

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
            Wait();
            webDriver.FindElement(generalModalOkBttn).Click();
        }

        public void ModalClickCancelBttn()
        {
            Wait();
            webDriver.FindElement(generalModalCancelBttn).Click();
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

        public string ConfirmationModalParagraph1()
        {
            WaitUntilVisible(generalConfirmationModalParagraph1);
            return (webDriver.FindElement(generalConfirmationModalParagraph1).Text);
        }

        public string ConfirmationModalParagraph2()
        {
            WaitUntilVisible(generalConfirmationModalParagraph2);
            return (webDriver.FindElement(generalConfirmationModalParagraph2).Text);
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

        public void CancelActionModal()
        {
            if (webDriver.FindElements(generalModalContent).Count() > 0)
            {
                Assert.Equal("Confirm Changes", ModalHeader());
                Assert.Contains("If you choose to cancel now, your changes will not be saved.", ModalContent());
                Assert.Contains("Do you want to proceed?", ModalContent());
                ModalClickOKBttn();
            }
        }
    }
}
