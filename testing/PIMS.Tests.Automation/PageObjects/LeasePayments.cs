using OpenQA.Selenium;
<<<<<<< HEAD
<<<<<<< HEAD
=======
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
>>>>>>> 7f1c050a4 (Leases automation)
=======
>>>>>>> 55c310707 (Automation for Lease and Licenses)

namespace PIMS.Tests.Automation.PageObjects
{
    public class LeasePayments : PageObjectBase
    {
        private By licensePaymentsLink = By.XPath("//a[contains(text(),'Payments')]");
        private By licensePaymentsTotal = By.CssSelector("div[data-testid='leasePaymentsTable'] div[class='tr-wrapper'] div[class='td expander svg-btn']");

        private By licensePaymentsModal = By.CssSelector("div[class='modal-content']");
        private By licensePaymentTermStartDateInput = By.Id("datepicker-startDate");
<<<<<<< HEAD
<<<<<<< HEAD
=======
        private By licensePaymentTermStartCalPicker = By.XPath("//div[contains(@class,'--selected')]");
>>>>>>> 7f1c050a4 (Leases automation)
=======
>>>>>>> 55c310707 (Automation for Lease and Licenses)
        private By licensePaymentTermEndDateInput = By.Id("datepicker-expiryDate");
        private By licensePaymentFrequencySelect = By.Id("input-leasePmtFreqTypeCode.id");
        private By licensePaymentAgreedPaymentInput = By.Id("input-paymentAmount");
        private By licensePaymentDueInput = By.Id("input-paymentDueDate");
        private By licensePaymentGSTRadioBttns = By.Name("isGstEligible");
        private By licensePaymentTermSelect = By.Id("input-statusTypeCode.id");

<<<<<<< HEAD
<<<<<<< HEAD
=======
        
>>>>>>> 7f1c050a4 (Leases automation)
=======
>>>>>>> 55c310707 (Automation for Lease and Licenses)
        private By licensePaymentSendDateInput = By.Id("datepicker-receivedDate");
        private By licensePaymentMethodSelect = By.Id("input-leasePaymentMethodType.id");
        private By licensePaymentAmountReceivedInput = By.Id("input-amountTotal");
        private By licensePaymentExpPaymentInput = By.Id("input-amountPreTax");
        private By licensePaymentGSTInput = By.Id("input-amountGst");

<<<<<<< HEAD
<<<<<<< HEAD
        //private By licensePaymentsTermTable = By.CssSelector("div[data-testid='securityDepositsTable']");
        private By licencePaymentsTable = By.CssSelector("div[data-testid='securityDepositsTable'] div[class='tr-wrapper']");
=======
        private By licensePaymentsTermTable = By.CssSelector("div[data-testid='securityDepositsTable']");
>>>>>>> 7f1c050a4 (Leases automation)
=======
        //private By licensePaymentsTermTable = By.CssSelector("div[data-testid='securityDepositsTable']");
        private By licencePaymentsTable = By.CssSelector("div[data-testid='securityDepositsTable'] div[class='tr-wrapper']");
>>>>>>> 55c310707 (Automation for Lease and Licenses)

        private int totalTermsInLease;

        public LeasePayments(IWebDriver webDriver) : base(webDriver)
        { }

        //Navigates to Payments Section
<<<<<<< HEAD
<<<<<<< HEAD
        public void NavigateToPaymentSection()
=======
        public void NavigateToTenantSection()
>>>>>>> 7f1c050a4 (Leases automation)
=======
        public void NavigateToPaymentSection()
>>>>>>> 55c310707 (Automation for Lease and Licenses)
        {
            Wait();
            webDriver.FindElement(licensePaymentsLink).Click();
        }

        public void AddTerm(string startDate, string endDate, string agreedPayment, string paymentDue, string gst, string termStatus)
        {
            Wait();
            ButtonElement("Add a Term");

<<<<<<< HEAD
<<<<<<< HEAD
            WaitUntil(licensePaymentsModal);

            var startDateInputElement = webDriver.FindElement(licensePaymentTermStartDateInput);

            if (startDateInputElement.GetAttribute("value") == "")
            {
                startDateInputElement.Click();
                startDateInputElement.SendKeys(startDate);
            }

            Wait();
=======
            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(20));
            wait.Until(ExpectedConditions.ElementIsVisible(licensePaymentsModal));
=======
            WaitUntil(licensePaymentsModal);
>>>>>>> 55c310707 (Automation for Lease and Licenses)

            var startDateInputElement = webDriver.FindElement(licensePaymentTermStartDateInput);

            if (startDateInputElement.GetAttribute("value") == "")
            {
                startDateInputElement.Click();
                startDateInputElement.SendKeys(startDate);
            }

            Wait();
<<<<<<< HEAD
            webDriver.FindElement(licensePaymentTermStartCalPicker).Click();
>>>>>>> 7f1c050a4 (Leases automation)
=======
>>>>>>> 55c310707 (Automation for Lease and Licenses)

            webDriver.FindElement(licensePaymentTermEndDateInput).Click();
            webDriver.FindElement(licensePaymentTermEndDateInput).SendKeys(endDate);

            var paymentFrequencyElement = webDriver.FindElement(licensePaymentFrequencySelect);
            ChooseRandomOption(paymentFrequencyElement, "input-leasePmtFreqTypeCode.id", 2);

            webDriver.FindElement(licensePaymentAgreedPaymentInput).Click();
            webDriver.FindElement(licensePaymentAgreedPaymentInput).SendKeys(agreedPayment);
            webDriver.FindElement(licensePaymentDueInput).SendKeys(paymentDue);

            ChooseSpecificRadioButton("isGstEligible", gst);
<<<<<<< HEAD
<<<<<<< HEAD
            ChooseSpecificOption("input-statusTypeCode.id", termStatus); 
=======
            ChooseSpecificOption("input-statusTypeCode.id", termStatus);

            
>>>>>>> 7f1c050a4 (Leases automation)
=======
            ChooseSpecificOption("input-statusTypeCode.id", termStatus); 
>>>>>>> 55c310707 (Automation for Lease and Licenses)

            ButtonElement("Save term");
        }

<<<<<<< HEAD
<<<<<<< HEAD
        public void OpenLastPaymentTab()
=======
        public void AddPayment(string sentDate, string totalReceived)
>>>>>>> 7f1c050a4 (Leases automation)
=======
        public void OpenLastPaymentTab()
>>>>>>> 55c310707 (Automation for Lease and Licenses)
        {
            Wait();

            totalTermsInLease = webDriver.FindElements(licensePaymentsTotal).Count();

            var selectedExpander = webDriver.FindElement(By.XPath("//div[@class='tr-wrapper']["+ totalTermsInLease +"]/div/div[@class='td expander svg-btn']"));
            selectedExpander.Click();
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> 55c310707 (Automation for Lease and Licenses)
        }

        public void AddPayment(string sentDate, string totalReceived, string status)
        {
<<<<<<< HEAD
=======
>>>>>>> 7f1c050a4 (Leases automation)
=======
>>>>>>> 55c310707 (Automation for Lease and Licenses)

            Wait();
            ButtonElement("Record a Payment");

<<<<<<< HEAD
<<<<<<< HEAD
            WaitUntil(licensePaymentsModal);
=======
            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(20));
            wait.Until(ExpectedConditions.ElementIsVisible(licensePaymentsModal));
>>>>>>> 7f1c050a4 (Leases automation)
=======
            WaitUntil(licensePaymentsModal);
>>>>>>> 55c310707 (Automation for Lease and Licenses)

            webDriver.FindElement(licensePaymentSendDateInput).Click();
            webDriver.FindElement(licensePaymentSendDateInput).SendKeys(sentDate);

            var paymentMethodElement = webDriver.FindElement(licensePaymentMethodSelect);
<<<<<<< HEAD
<<<<<<< HEAD
            webDriver.FindElement(licensePaymentsModal).Click();
=======
>>>>>>> 7f1c050a4 (Leases automation)
=======
            webDriver.FindElement(licensePaymentsModal).Click();
>>>>>>> 55c310707 (Automation for Lease and Licenses)
            ChooseRandomOption(paymentMethodElement, "input-leasePaymentMethodType.id", 1);

            webDriver.FindElement(licensePaymentAmountReceivedInput).SendKeys(totalReceived);

            ButtonElement("Save payment");
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> 55c310707 (Automation for Lease and Licenses)

            Wait();

            var totalPayments = webDriver.FindElements(licencePaymentsTable).Count();
            var paymentStatus = webDriver.FindElement(By.CssSelector("div[data-testid='securityDepositsTable'] div[class='tr-wrapper']:nth-child("+ totalPayments +") div:nth-child(6)")).Text;

            Assert.True(paymentStatus.Equals(status));
<<<<<<< HEAD
=======
>>>>>>> 7f1c050a4 (Leases automation)
=======
>>>>>>> 55c310707 (Automation for Lease and Licenses)
        }
    }
}
