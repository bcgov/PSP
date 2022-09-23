using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace PIMS.Tests.Automation.PageObjects
{
    public class LeasePayments : PageObjectBase
    {
        private By licensePaymentsLink = By.XPath("//a[contains(text(),'Payments')]");
        private By licensePaymentsTotal = By.CssSelector("div[data-testid='leasePaymentsTable'] div[class='tr-wrapper'] div[class='td expander svg-btn']");

        private By licensePaymentsModal = By.CssSelector("div[class='modal-content']");
        private By licensePaymentTermStartDateInput = By.Id("datepicker-startDate");
        private By licensePaymentTermStartCalPicker = By.XPath("//div[contains(@class,'--selected')]");
        private By licensePaymentTermEndDateInput = By.Id("datepicker-expiryDate");
        private By licensePaymentFrequencySelect = By.Id("input-leasePmtFreqTypeCode.id");
        private By licensePaymentAgreedPaymentInput = By.Id("input-paymentAmount");
        private By licensePaymentDueInput = By.Id("input-paymentDueDate");
        private By licensePaymentGSTRadioBttns = By.Name("isGstEligible");
        private By licensePaymentTermSelect = By.Id("input-statusTypeCode.id");

        
        private By licensePaymentSendDateInput = By.Id("datepicker-receivedDate");
        private By licensePaymentMethodSelect = By.Id("input-leasePaymentMethodType.id");
        private By licensePaymentAmountReceivedInput = By.Id("input-amountTotal");
        private By licensePaymentExpPaymentInput = By.Id("input-amountPreTax");
        private By licensePaymentGSTInput = By.Id("input-amountGst");

        private By licensePaymentsTermTable = By.CssSelector("div[data-testid='securityDepositsTable']");

        private int totalTermsInLease;

        public LeasePayments(IWebDriver webDriver) : base(webDriver)
        { }

        //Navigates to Payments Section
        public void NavigateToTenantSection()
        {
            Wait();
            webDriver.FindElement(licensePaymentsLink).Click();
        }

        public void AddTerm(string startDate, string endDate, string agreedPayment, string paymentDue, string gst, string termStatus)
        {
            Wait();
            ButtonElement("Add a Term");

            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(20));
            wait.Until(ExpectedConditions.ElementIsVisible(licensePaymentsModal));

            webDriver.FindElement(licensePaymentTermStartDateInput).Click();
            webDriver.FindElement(licensePaymentTermStartDateInput).Clear();
            webDriver.FindElement(licensePaymentTermStartDateInput).SendKeys(startDate);

            Wait();
            webDriver.FindElement(licensePaymentTermStartCalPicker).Click();

            webDriver.FindElement(licensePaymentTermEndDateInput).Click();
            webDriver.FindElement(licensePaymentTermEndDateInput).SendKeys(endDate);

            var paymentFrequencyElement = webDriver.FindElement(licensePaymentFrequencySelect);
            ChooseRandomOption(paymentFrequencyElement, "input-leasePmtFreqTypeCode.id", 2);

            webDriver.FindElement(licensePaymentAgreedPaymentInput).Click();
            webDriver.FindElement(licensePaymentAgreedPaymentInput).SendKeys(agreedPayment);
            webDriver.FindElement(licensePaymentDueInput).SendKeys(paymentDue);

            ChooseSpecificRadioButton("isGstEligible", gst);
            ChooseSpecificOption("input-statusTypeCode.id", termStatus);

            

            ButtonElement("Save term");
        }

        public void AddPayment(string sentDate, string totalReceived)
        {
            Wait();

            totalTermsInLease = webDriver.FindElements(licensePaymentsTotal).Count();

            var selectedExpander = webDriver.FindElement(By.XPath("//div[@class='tr-wrapper']["+ totalTermsInLease +"]/div/div[@class='td expander svg-btn']"));
            selectedExpander.Click();

            Wait();
            ButtonElement("Record a Payment");

            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(20));
            wait.Until(ExpectedConditions.ElementIsVisible(licensePaymentsModal));

            webDriver.FindElement(licensePaymentSendDateInput).Click();
            webDriver.FindElement(licensePaymentSendDateInput).SendKeys(sentDate);

            var paymentMethodElement = webDriver.FindElement(licensePaymentMethodSelect);
            ChooseRandomOption(paymentMethodElement, "input-leasePaymentMethodType.id", 1);

            webDriver.FindElement(licensePaymentAmountReceivedInput).SendKeys(totalReceived);

            ButtonElement("Save payment");
        }
    }
}
