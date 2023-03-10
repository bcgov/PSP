using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace PIMS.Tests.Automation.PageObjects
{
    public class LeasePayments : PageObjectBase
    {
        //Payments Tab Link Element
        private By licensePaymentsLink = By.XPath("//a[contains(text(),'Payments')]");

        //Payment Init screen Elements
        private By licencePaymentsSubtitle = By.XPath("//div[contains(text(),'Payments by Term')]");
        private By licencePaymentAddBttn = By.XPath("//div[contains(text(),'Add a Term')]/parent::button");
        private By licencePaymentColumnStartEndDate = By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Start date - End date')]");
        private By licensePaymentColumnPaymentFreq = By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Payment frequency')]");
        private By licencePaymentColumnPaymentDue = By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Payment due')]");
        private By licensePaymentColumnExpectedPay = By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Expected payment ($)')]");
        private By licensePaymentExpectedPayTooltip = By.Id("expectedPaymentTooltip");
        private By licencePaymentColumnGSTBoolean = By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'GST?')]");
        private By licensePaymentColumnGSTTotal = By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'GST ($)')]");
        private By licensePaymentGSTTotalTooltip = By.Id("gstAmountTooltip");
        private By licencePaymentColumnExpectedTotal = By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Expected total ($)')]");
        private By licensePaymentExpectedTotalTooltip = By.Id("expectedTotalTooltip");
        private By licensePaymentColumnExpectedTerm = By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Expected term ($)')]");
        private By licensePaymentExpectedTermTooltip = By.Id("expectedTermTooltip");
        private By licencePaymentColumnActualTotal = By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Actual total ($)')]");
        private By licensePaymentActualTotalTooltip = By.Id("actualTotalTooltip");
        private By licensePaymentColumnExercised = By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Exercised?')]");
        private By licencePaymentColumnActions = By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Actions')]");
        private By licencePaymentsNoRows = By.CssSelector("div[data-testid='leasePaymentsTable'] div[class='no-rows-message']");

        //Payments Table Headers Elements
        private By licensePaymentsSendDateColumn = By.XPath("//div[@data-testid='leasePaymentsTable']/div/div/div/div/div/div[@data-testid='securityDepositsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Sent date')]");
        private By licensePaymentsPaymentMethodColumn = By.XPath("//div[@data-testid='leasePaymentsTable']/div/div/div/div/div/div[@data-testid='securityDepositsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Payment method')]");
        private By licensePaymentsSentPaymentColumn = By.XPath("//div[@data-testid='leasePaymentsTable']/div/div/div/div/div/div[@data-testid='securityDepositsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Sent payment ($)')]");
        private By licensePaymentsSendPaymentTooltip = By.Id("actualReceivedPaymentTooltip");
        private By licensePaymentsGSTColumn = By.XPath("//div[@data-testid='leasePaymentsTable']/div/div/div/div/div/div[@data-testid='securityDepositsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'GST ($)')]");
        private By licensePaymentsGSTTooltip = By.Id("actualGstTooltip");
        private By licensePaymentsSentTotalColumn = By.XPath("//div[@data-testid='leasePaymentsTable']/div/div/div/div/div/div[@data-testid='securityDepositsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Sent total ($)')]");
        private By licensePaymentsSendTotalTooltip = By.Id("receivedTotalTooltip");
        private By licensePaymentsPaymentStatusColumn = By.XPath("//div[@data-testid='leasePaymentsTable']/div/div/div/div/div/div[@data-testid='securityDepositsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Payment status')]");
        private By licensePaymentsPaymentStatusTooltip = By.Id("paymentStatusTooltip");
        private By licensePaymentsNotesColumn = By.XPath("//div[@data-testid='leasePaymentsTable']/div/div/div/div/div/div[@data-testid='securityDepositsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Notes')]");
        private By licensePaymentsActionsColumn = By.XPath("//div[@data-testid='leasePaymentsTable']/div/div/div/div/div/div[@data-testid='securityDepositsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Actions')]");

        //Payments Modal Element
        private By licensePaymentsModal = By.CssSelector("div[class='modal-content']");

        //Create Term Elements
        private By licensePaymentTermStartDateLabel = By.XPath("//label[contains(text(),'Start date')]");
        private By licensePaymentTermStartDateInput = By.Id("datepicker-startDate");
        private By licensePaymentTermEndDateLabel = By.XPath("//label[contains(text(),'End date')]");
        private By licensePaymentTermEndDateInput = By.Id("datepicker-expiryDate");
        private By licensePaymentTermFrequencySelectLabel = By.XPath("//label[contains(text(),'Payment frequency')]");
        private By licensePaymentTermFrequencySelect = By.Id("input-leasePmtFreqTypeCode.id");
        private By licensePaymentTermAgreedPaymentLabel = By.XPath("//label[contains(text(),'Agreed payment ($)')]");
        private By licensePaymentTermAgreedPaymentInput = By.Id("input-paymentAmount");
        private By licensePaymentTermDueLabel = By.XPath("//label[contains(text(),'Payments due')]");
        private By licensePaymentTermDueTooltip = By.Id("paymentDueDate-tooltip");
        private By licensePaymentTermDueInput = By.Id("input-paymentDueDate");
        private By licensePaymentTermGSTLabel = By.XPath("//label[contains(text(),'Subject to GST?')]");
        private By licensePaymentTermGSTRadioBttns = By.Name("isGstEligible");
        private By licensePaymentTermLabel = By.XPath("//label[contains(text(),'Term Status')]");
        private By licensePaymentTermSelect = By.Id("input-statusTypeCode.id");

        //Create Payment Elements
        private By licensePaymentSendDateLabel = By.XPath("//label[contains(text(),'Sent date')]");
        private By licensePaymentSendDateInput = By.Id("datepicker-receivedDate");
        private By licensePaymentMethodLabel = By.XPath("//label[contains(text(),'Method')]");
        private By licensePaymentMethodSelect = By.Id("input-leasePaymentMethodType.id");
        private By licensePaymentAmountReceivedLabel = By.XPath("//label[contains(text(),'Total received ($)')]");
        private By licensePaymentAmountReceivedInput = By.Id("input-amountTotal");
        private By licensePaymentExpPaymentLabel = By.XPath("//label[contains(text(),'Expected payment ($)')]");
        private By licensePaymentExpPaymentInput = By.Id("input-amountPreTax");
        private By licensePaymentGSTLabel = By.XPath("//label[contains(text(),'GST ($)')]");
        private By licensePaymentExpPaymentTolltip = By.Id("actual-calculation-tooltip");
        private By licensePaymentGSTInput = By.Id("input-amountGst");

        //Last Term Table Elements
        private int totalTermsInLease;
        private By licensePaymentsTermsTotal = By.CssSelector("div[data-testid='leasePaymentsTable'] div[class='tr-wrapper'] div[class='td expander svg-btn']");
        

        //Last Payment Elements
        private int totalPaymentInTerm;
        private By licencePaymentsTermTable = By.CssSelector("div[data-testid='securityDepositsTable'] div[class='tr-wrapper']");
        private By licenseTermPaymentsTableTotal = By.CssSelector("div[data-testid='leasePaymentsTable'] div[data-testid='securityDepositsTable'] div[class='tbody'] div[class='tr-wrapper']");
        private By licensePaymentDeleteTermBttn = By.CssSelector("button[title='delete term']");

        
        private SharedModals sharedModals;

        public LeasePayments(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
            totalTermsInLease = 0;
            totalPaymentInTerm = 0;
        }

        //Navigates to Payments Section
        public void NavigateToPaymentSection()
        {
            Wait();
            webDriver.FindElement(licensePaymentsLink).Click();
        }

        public void AddTermBttn()
        {
            Wait();
            ButtonElement("Add a Term");
        }

        public void AddTerm(string startDate, string endDate, string frequency, string agreedPayment, string paymentDue, string gst, string termStatus)
        {
            Wait();

            var startDateInputElement = webDriver.FindElement(licensePaymentTermStartDateInput);

            if (startDateInputElement.GetAttribute("value") == "")
            {
                startDateInputElement.Click();
                startDateInputElement.SendKeys(startDate);
            }

            Wait();

            webDriver.FindElement(licensePaymentTermEndDateInput).Click();
            webDriver.FindElement(licensePaymentTermEndDateInput).SendKeys(endDate);
            webDriver.FindElement(licensePaymentTermEndDateInput).SendKeys(Keys.Enter);

            ChooseSpecificSelectOption(licensePaymentTermFrequencySelect, frequency);

            webDriver.FindElement(licensePaymentTermAgreedPaymentInput).Click();
            webDriver.FindElement(licensePaymentTermAgreedPaymentInput).SendKeys(agreedPayment);
            webDriver.FindElement(licensePaymentTermDueInput).SendKeys(paymentDue);

            ChooseRandomRadioButton(licensePaymentTermGSTRadioBttns);
            ChooseSpecificSelectOption(licensePaymentTermSelect, termStatus); 
            ButtonElement("Save term");

            Wait();
            totalTermsInLease = webDriver.FindElements(licensePaymentsTermsTotal).Count;
        }

        public void OpenLastPaymentTab()
        {
            Wait();
            var selectedExpander = webDriver.FindElement(By.XPath("//div[@class='tr-wrapper']["+ totalTermsInLease +"]/div/div[@class='td expander svg-btn']"));
            selectedExpander.Click();
        }

        public void AddPaymentBttn()
        {
            Wait();
            ButtonElement("Record a Payment");
        }

        public void AddPayment(string sentDate, string totalReceived, string status)
        {
            Wait();

            webDriver.FindElement(licensePaymentSendDateInput).Click();
            webDriver.FindElement(licensePaymentSendDateInput).SendKeys(sentDate);
            webDriver.FindElement(licensePaymentSendDateInput).SendKeys(Keys.Enter);
            webDriver.FindElement(licensePaymentsModal).Click();

            ChooseRandomSelectOption(licensePaymentMethodSelect, 1);

            webDriver.FindElement(licensePaymentAmountReceivedInput).SendKeys(totalReceived);

            ButtonElement("Save payment");

            Wait();
            totalPaymentInTerm = webDriver.FindElements(licenseTermPaymentsTableTotal).Count;

            //var totalPayments = webDriver.FindElements(licencePaymentsTermTable).Count();
            //var paymentStatus = webDriver.FindElement(By.CssSelector("div[data-testid='securityDepositsTable'] div[class='tr-wrapper']:nth-child("+ totalPayments +") div:nth-child(6)")).Text;

            //Assert.True(paymentStatus.Equals(status));
        }

        public void DeleteLastTerm()
        {
            Wait();
            webDriver.FindElement(licensePaymentDeleteTermBttn).Click();

            WaitUntil(licensePaymentsModal);
            sharedModals.ModalClickOKBttn();
        }

        public void DeleteLastPayment()
        {
            Wait();
            var totalPayments = webDriver.FindElements(licenseTermPaymentsTableTotal).Count();
            var lastPaymentDeleteIcon = By.CssSelector("div[class='tbody'] div[class='tr-wrapper']:nth-child("+totalPayments+") button[title='delete actual']");
            webDriver.FindElement(lastPaymentDeleteIcon).Click();

            WaitUntil(licensePaymentsModal);
            sharedModals.ModalClickOKBttn();

        }

        public void VerifyPaymentsInitForm()
        {
            Wait();
            Assert.True(webDriver.FindElement(licencePaymentsSubtitle).Displayed);

            Assert.True(webDriver.FindElement(licencePaymentAddBttn).Displayed);
            Assert.True(webDriver.FindElement(licencePaymentColumnStartEndDate).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentColumnPaymentFreq).Displayed);
            Assert.True(webDriver.FindElement(licencePaymentColumnPaymentDue).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentColumnExpectedPay).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentExpectedPayTooltip).Displayed);
            Assert.True(webDriver.FindElement(licencePaymentColumnGSTBoolean).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentColumnGSTTotal).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentGSTTotalTooltip).Displayed);
            Assert.True(webDriver.FindElement(licencePaymentColumnExpectedTotal).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentExpectedTotalTooltip).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentColumnExpectedTerm).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentExpectedTermTooltip).Displayed);
            Assert.True(webDriver.FindElement(licencePaymentColumnActualTotal).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentActualTotalTooltip).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentColumnExercised).Displayed);
            Assert.True(webDriver.FindElement(licencePaymentColumnActions).Displayed);
            Assert.True(webDriver.FindElement(licencePaymentsNoRows).Displayed);
        }

        public void VerifyCreateTermForm()
        {
            Wait();
            Assert.True(sharedModals.ModalHeader() == "Add a Term");

            Assert.True(webDriver.FindElement(licensePaymentTermStartDateLabel).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentTermStartDateInput).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentTermEndDateLabel).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentTermEndDateInput).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentTermFrequencySelectLabel).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentTermFrequencySelect).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentTermAgreedPaymentLabel).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentTermAgreedPaymentInput).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentTermDueLabel).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentTermDueTooltip).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentTermDueInput).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentTermGSTLabel).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentTermGSTRadioBttns).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentTermLabel).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentTermSelect).Displayed);

            sharedModals.VerifyButtonsPresence();
        }

        public void VerifyInsertedTermTable(string termType, string startDate, string endDate, string paymentFrequency, string paymentDue, string expectedPayment, string gst, string exercised)
        {
            Wait();

            Assert.True(webDriver.FindElement(By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalTermsInLease +"]/div[@class='tr']/div[@class='td expander svg-btn']/following-sibling::div[1]")).Text == termType);
            Assert.True(webDriver.FindElement(By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalTermsInLease +"]/div[@class='tr']/div[@class='td expander svg-btn']/following-sibling::div[2]")).Text == ConcatenateDates(startDate, endDate));
            Assert.True(webDriver.FindElement(By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalTermsInLease +"]/div[@class='tr']/div[@class='td expander svg-btn']/following-sibling::div[3]")).Text == paymentFrequency);
            Assert.True(webDriver.FindElement(By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalTermsInLease +"]/div[@class='tr']/div[@class='td expander svg-btn']/following-sibling::div[4]")).Text == paymentDue);
            Assert.True(webDriver.FindElement(By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalTermsInLease +"]/div[@class='tr']/div[@class='td expander svg-btn']/following-sibling::div[5]")).Text == TransformCurrencyFormat(expectedPayment));
            Assert.True(webDriver.FindElement(By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalTermsInLease +"]/div[@class='tr']/div[@class='td expander svg-btn']/following-sibling::div[6]")).Text == gst);
            if (gst == "N")
            {
                Assert.True(webDriver.FindElement(By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalTermsInLease +"]/div[@class='tr']/div[@class='td expander svg-btn']/following-sibling::div[7]")).Text == "-");
                Assert.True(webDriver.FindElement(By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalTermsInLease +"]/div[@class='tr']/div[@class='td expander svg-btn']/following-sibling::div[11]")).Text == TransformCurrencyFormat(expectedPayment));
            }
            else
            {
                Assert.True(webDriver.FindElement(By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalTermsInLease +"]/div[@class='tr']/div[@class='td expander svg-btn']/following-sibling::div[7]")).Text == CalculateGST(expectedPayment));
                System.Diagnostics.Debug.WriteLine(webDriver.FindElement(By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalTermsInLease +"]/div[@class='tr']/div[@class='td expander svg-btn']/following-sibling::div[11]")).Text);
                System.Diagnostics.Debug.WriteLine(CalculateExpectedTotal(expectedPayment));
                Assert.True(webDriver.FindElement(By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalTermsInLease +"]/div[@class='tr']/div[@class='td expander svg-btn']/following-sibling::div[11]")).Text == CalculateExpectedTotal(expectedPayment));
            }
            
            Assert.True(webDriver.FindElement(By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalTermsInLease +"]/div[@class='tr']/div[@class='td expander svg-btn']/following-sibling::div[8]")).Text == CalculateExpectedTerm(paymentFrequency, gst, expectedPayment, startDate, endDate));
            Assert.True(webDriver.FindElement(By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalTermsInLease +"]/div[@class='tr']/div[@class='td expander svg-btn']/following-sibling::div[9]")).Text == "$0.00");
            Assert.True(webDriver.FindElement(By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalTermsInLease +"]/div[@class='tr']/div[@class='td expander svg-btn']/following-sibling::div[10]")).Text == exercised);  
        }

        public void VerifyCreatePaymentForm()
        {
            Wait();
            Assert.True(sharedModals.ModalHeader() == "Payment details");

            Assert.True(webDriver.FindElement(licensePaymentSendDateLabel).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentSendDateInput).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentMethodLabel).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentMethodSelect).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentAmountReceivedLabel).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentAmountReceivedInput).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentExpPaymentLabel).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentExpPaymentInput).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentGSTLabel).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentExpPaymentTolltip).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentGSTInput).Displayed);

            sharedModals.VerifyButtonsPresence();
        }

        public void VerifyPaymentTableHeader()
        {
            Wait();
            Assert.True(webDriver.FindElement(licensePaymentsSendDateColumn).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentsPaymentMethodColumn).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentsSentPaymentColumn).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentsSendPaymentTooltip).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentsGSTColumn).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentsGSTTooltip).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentsSentTotalColumn).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentsSendTotalTooltip).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentsPaymentStatusColumn).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentsPaymentStatusTooltip).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentsNotesColumn).Displayed);
            Assert.True(webDriver.FindElement(licensePaymentsActionsColumn).Displayed);
        }

        public void VerifyInsertedPayment()
        {
            Wait();
            //To-Do
        }

        private string ConcatenateDates(string startDate, string endDate)
        {
            var startDateFormat = DateTime.Parse(startDate);
            var endDateFormat = DateTime.Parse(endDate);

            return startDateFormat.ToString("MMM dd, yyyy") +" - "+ endDateFormat.ToString("MMM dd, yyyy");
        }

        private string CalculateGST(string amount)
        {
            decimal value = decimal.Parse(amount) * 0.05m;
            return "$" + value.ToString("#,##0.00");
        }

        private string CalculateExpectedTotal(string amount)
        {
            decimal expectedValue = decimal.Parse(amount);
            decimal gstValue = decimal.Parse(amount) * 0.05m;
            decimal total = expectedValue + gstValue;

            return "$" + total.ToString("#,##0.00");
        }

        private string CalculateExpectedTerm(string frequency, string gst, string amount, string startDate, string endDate)
        {
            var frequencyNumber = 0 ;
            var unitAmount = decimal.Parse(amount);
            var startDateFormat = DateTime.Parse(startDate);
            var endDateFormat = DateTime.Parse(endDate);

            switch (frequency)
            {
                case "Daily":
                    var numberOfDays = (endDateFormat - startDateFormat).TotalDays;
                    frequencyNumber = (int)Math.Ceiling(numberOfDays);
                    break;
                case "Weekly":
                    var numberOfWeeks = (endDateFormat - startDateFormat).TotalDays /7;
                    frequencyNumber = (int)Math.Ceiling(numberOfWeeks);
                    break;
                case "Monthly":
                    var numberOfMonths = (endDateFormat - startDateFormat).TotalDays /30;
                    frequencyNumber = (int)Math.Ceiling(numberOfMonths);
                    break;
                default:
                    frequencyNumber = 0;
                    break;
            }

            if (gst == "Y")
            {
                unitAmount += decimal.Parse(amount) * 0.05m;
            }

            var finalAmount = frequencyNumber * unitAmount;

            if (finalAmount == 0)
            {
                return "-";
            }
            else
            {
                return "$" + finalAmount.ToString("#,##0.00");
            } 
        }
    }
}
