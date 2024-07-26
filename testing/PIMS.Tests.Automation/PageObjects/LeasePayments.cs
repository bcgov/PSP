using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class LeasePayments : PageObjectBase
    {
        //Payments Tab Link Element
        private By licensePaymentsLink = By.XPath("//a[contains(text(),'Payments')]");

        //Payment Init screen Elements
        private readonly By licencePaymentsSubtitle = By.XPath("//div[contains(text(),'Payment Periods')]");
        private readonly By licencePaymentAddBttn = By.XPath("//div[contains(text(),'Payment Periods')]/following-sibling::div/button");

        private readonly By licencePaymentColumnStartEndDate = By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Start date - end date')]");
        private readonly By licensePaymentColumnPaymentFreq = By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Payment frequency')]");
        private readonly By licencePaymentColumnPaymentDue = By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Payment due')]");
        private readonly By licensePaymentColumnExpectedPay = By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Expected payment ($)')]");
        private readonly By licensePaymentExpectedPayTooltip = By.Id("expectedPaymentTooltip");
        private readonly By licencePaymentColumnGSTBoolean = By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'GST?')]");
        private readonly By licensePaymentColumnGSTTotal = By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'GST ($)')]");
        private readonly By licensePaymentGSTTotalTooltip = By.Id("gstAmountTooltip");
        private readonly By licencePaymentColumnExpectedTotal = By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Expected total ($)')]");
        private readonly By licensePaymentExpectedTotalTooltip = By.Id("expectedTotalTooltip");
        private readonly By licensePaymentColumnExpectedTerm = By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Expected period ($)')]");
        private readonly By licensePaymentExpectedTermTooltip = By.Id("expectedTermTooltip");
        private readonly By licencePaymentColumnActualTotal = By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Actual total ($)')]");
        private readonly By licensePaymentActualTotalTooltip = By.Id("actualTotalTooltip");
        private readonly By licensePaymentColumnExercised = By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Exercised?')]");
        private readonly By licencePaymentColumnActions = By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Actions')]");
        private readonly By licencePaymentsNoRows = By.CssSelector("div[data-testid='leasePaymentsTable'] div[class='no-rows-message']");

        //Category Table Header Elements
        private readonly By licenceCategoryPaymentsCategoryColumn = By.XPath("//div[@data-testid='variablePeriodTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Category')]");
        private readonly By licenceCategoryPaymentsPaymentFrequencyColumn = By.XPath("//div[@data-testid='variablePeriodTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Payment Fequency')]");
        private readonly By licenceCategoryPaymentsExpectedPaymentColumn = By.XPath("//div[@data-testid='variablePeriodTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Expected Payment ($)')]");
        private readonly By licenceCategoryPaymentsIsGSTColumn = By.XPath("//div[@data-testid='variablePeriodTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'GST?')]");
        private readonly By licenceCategoryPaymentsGSTColumn = By.XPath("//div[@data-testid='variablePeriodTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'GST ($)')]");
        private readonly By licenceCategoryPaymentsExpectedPeriodColumn = By.XPath("//div[@data-testid='variablePeriodTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Expected total ($)')]");
        private readonly By licenceCategoryPaymentsActualTotalColumn = By.XPath("//div[@data-testid='variablePeriodTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Actual total ($)')]");

        //Payments Table Headers Elements
        private readonly By licensePaymentsReceivedDateColumn = By.XPath("//div[@data-testid='paymentsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Date')]");
        private readonly By licensePaymentsSendDateColumn = By.XPath("//div[@data-testid='paymentsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Rent category')]");
        private readonly By licensePaymentsPaymentMethodColumn = By.XPath("//div[@data-testid='paymentsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Payment method')]");
        private readonly By licensePaymentsReceivedPaymentColumn = By.XPath("//div[@data-testid='paymentsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Received payment ($)')]");
        private readonly By licensePaymentsReceivedPaymentTooltip = By.Id("actualReceivedPaymentTooltip");
        private readonly By licensePaymentsGSTColumn = By.XPath("//div[@data-testid='paymentsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'GST ($)')]");
        private readonly By licensePaymentsGSTTooltip = By.Id("actualGstTooltip");
        private readonly By licensePaymentsTotalColumn = By.XPath("//div[@data-testid='paymentsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Total ($)')]");
        private readonly By licensePaymentsTotalTooltip = By.Id("receivedTotalTooltip");
        private readonly By licensePaymentsPaymentStatusColumn = By.XPath("//div[@data-testid='paymentsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Payment status')]");
        private readonly By licensePaymentsPaymentStatusTooltip = By.Id("paymentStatusTooltip");
        private readonly By licensePaymentsNotesColumn = By.XPath("//div[@data-testid='paymentsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Notes')]");
        private readonly By licensePaymentsActionsColumn = By.XPath("//div[@data-testid='paymentsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Actions')]");

        //Payments Modal Element
        private readonly By licensePaymentsModal = By.CssSelector("div[class='modal-content']");

        //Create Period Modal Elements
        private readonly By licensePaymentPeriodSelectTypeLabel = By.XPath("//label[contains(text(),'Select payment type')]");
        private readonly By licensePaymentPeriodSelectTooltip = By.Id("section-field-tooltip");
        private readonly By licensePaymentPredefinedRadioInput = By.CssSelector("input[data-testid='radio-isvariable-predetermined']");
        private readonly By licensePaymentVariableRadioInput = By.CssSelector("input[data-testid='radio-isvariable-variable']");

        private readonly By licensePaymentPeriodDurationLabel = By.XPath("//label[contains(text(),'Period duration')]");
        private readonly By licensePaymentPeriodDurationTooltip = By.Id("isFlexible-tooltip");
        private readonly By licensePaymentPeriodDurationSelect = By.Id("input-isFlexible");

        //Create Predefined Period Modal Elements
        private readonly By licensePaymentPeriodStartDateLabel = By.XPath("//label[contains(text(),'Start date')]");
        private readonly By licensePaymentPeriodStartDateTooltip = By.Id("startDate-tooltip");
        private readonly By licensePaymentPeriodStartDateInput = By.CssSelector("input[id='datepicker-startDate']");
        private readonly By licensePaymentPeriodEndDateLabel = By.XPath("//label[contains(text(),'End date')]");
        private readonly By licensePaymentPeriodEndDateInput = By.Id("datepicker-expiryDate");
        private readonly By licensePaymentPeriodFrequencySelectLabel = By.XPath("//label[contains(text(),'Payment frequency')]");
        private readonly By licensePaymentPeriodFrequencySelect = By.Id("input-leasePmtFreqTypeCode.id");
        private readonly By licensePaymentPeriodAgreedPaymentLabel = By.XPath("//label[contains(text(),'Agreed payment ($)')]");
        private readonly By licensePaymentPeriodAgreedPaymentInput = By.Id("input-paymentAmount");
        private readonly By licensePaymentPeriodDueLabel = By.XPath("//label[contains(text(),'Payments due')]");
        private readonly By licensePaymentPeriodDueTooltip = By.Id("paymentDueDateStr-tooltip");
        private readonly By licensePaymentPeriodDueInput = By.Id("input-paymentDueDateStr");
        private readonly By licensePaymentPeriodGSTLabel = By.XPath("//label[contains(text(),'Subject to GST?')]");
        private readonly By licensePaymentPeriodGSTRadioBttns = By.Name("isGstEligible");
        private readonly By licencePaymentPeriodGSTTrueRadioBttn = By.Id("input-isGstEligible");
        private readonly By licencePaymentPeriodGSTFalseRadioBttn = By.Id("input-isGstEligible-2");
        private readonly By licensePaymentPeriodLabel = By.XPath("//label[contains(text(),'Period Status')]");
        private readonly By licensePaymentPeriodSelect = By.Id("input-statusTypeCode.id");

        //Create Variable Period Modal Elements
        private readonly By licensePaymentPeriodVariableBaseSubtitle = By.XPath("//div[contains(text(),'Add Base Rent')]/parent::div/parent::h2");
        private readonly By licensePaymentPeriodVariableBaseSubtitleTooltip = By.Id("base-rent-tooltip");
        private readonly By licensePaymentPeriodBaseFrequencyLabel = By.XPath("//label[@for='input-leasePmtFreqTypeCode.id']/span[contains(text(),'Payment frequency')]");
        private readonly By licensePaymentPeriodBaseSelect = By.Id("input-leasePmtFreqTypeCode.id");
        private readonly By licensePaymentPeriodBaseAgreedPaymentLabel = By.XPath("//label[@for='input-paymentAmount']/span[contains(text(),'Agreed payment ($)')]");
        private readonly By licensePaymentPeriodBaseAgreedPaymentInput = By.Id("input-paymentAmount");
        private readonly By licensePaymentPeriodBaseGSTLabel = By.XPath("//div[contains(text(),'Add Base Rent')]/parent::div/parent::h2/following-sibling::div/div/div/div/div/label[contains(text(),'Subject to GST')]");
        private readonly By licensePaymentPeriodBaseGSTYesRadioBttn = By.Id("input-isGstEligible");
        private readonly By licensePaymentPeriodBaseGSTNoRadioBttn = By.Id("input-isGstEligible-2");

        private readonly By licensePaymentPeriodVariableAdditionalSubtitle = By.XPath("//div[contains(text(),'Add Additional Rent')]/parent::div/parent::h2");
        private readonly By licensePaymentPeriodVariableAdditionalTooltip = By.Id("additional-rent-tooltip");
        private readonly By licensePaymentPeriodAdditionalFrequencyLabel = By.XPath("//label[@for='input-additionalRentFreqTypeCode.id']/span[contains(text(),'Payment frequency')]");
        private readonly By licensePaymentPeriodAdditionalSelect = By.Id("input-additionalRentFreqTypeCode.id");
        private readonly By licensePaymentPeriodAdditionalAgreedPaymentLabel = By.XPath("//div[contains(text(),'Add Additional Rent')]/parent::div/parent::h2/following-sibling::div/div/div/div/label[contains(text(),'Agreed payment ($)')]");
        private readonly By licensePaymentPeriodAdditionalAgreedPaymentInput = By.Id("input-additionalRentPaymentAmount");
        private readonly By licensePaymentPeriodAdditionalGSTLabel = By.XPath("//div[contains(text(),'Add Additional Rent')]/parent::div/parent::h2/following-sibling::div/div/div/div/div/label[contains(text(),'Subject to GST')]");
        private readonly By licensePaymentPeriodAdditionalYesRadioBttn = By.Id("input-isAdditionalRentGstEligible");
        private readonly By licensePaymentPeriodAdditionalGSTNoRadioBttn = By.Id("input-isAdditionalRentGstEligible-2");

        private readonly By licensePaymentPeriodVariableVariableSubtitle = By.XPath("//div[contains(text(),'Add Variable Rent')]/parent::div/parent::h2");
        private readonly By licensePaymentPeriodVariableVariableTooltip = By.Id("variable-rent-tooltip");
        private readonly By licensePaymentPeriodVariableFrequencyLabel = By.XPath("//label[@for='input-variableRentFreqTypeCode.id']/span[contains(text(),'Payment frequency')]");
        private readonly By licensePaymentPeriodVariableSelect = By.Id("input-variableRentFreqTypeCode.id");
        private readonly By licensePaymentPeriodVariableAgreedPaymentLabel = By.XPath("//div[contains(text(),'Add Variable Rent')]/parent::div/parent::h2/following-sibling::div/div/div/div/label[contains(text(),'Agreed payment ($)')]");
        private readonly By licensePaymentPeriodVariableAgreedPaymentInput = By.Id("input-variableRentPaymentAmount");
        private readonly By licensePaymentPeriodVariableGSTLabel = By.XPath("//div[contains(text(),'Add Variable Rent')]/parent::div/parent::h2/following-sibling::div/div/div/div/div/label[contains(text(),'Subject to GST')]");
        private readonly By licensePaymentPeriodVariableYesRadioBttn = By.Id("input-isVariableRentGstEligible");
        private readonly By licensePaymentPeriodVariableGSTNoRadioBttn = By.Id("input-isVariableRentGstEligible-2");

        //Create Payment Elements
        private readonly By licensePaymentSendDateLabel = By.XPath("//label[contains(text(),'Sent date')]");
        private readonly By licensePaymentSendDateInput = By.Id("datepicker-receivedDate");
        private readonly By licensePaymentMethodLabel = By.XPath("//label[contains(text(),'Method')]");
        private readonly By licensePaymentMethodSelect = By.Id("input-leasePaymentMethodType.id");
        private readonly By licensePaymentAmountReceivedLabel = By.XPath("//label[contains(text(),'Total received ($)')]");
        private readonly By licensePaymentAmountReceivedInput = By.Id("input-amountTotal");
        private readonly By licensePaymentExpPaymentLabel = By.XPath("//label[contains(text(),'Expected payment ($)')]");
        private readonly By licensePaymentExpPaymentInput = By.Id("input-amountPreTax");
        private readonly By licensePaymentGSTLabel = By.XPath("//label[contains(text(),'GST ($)')]");
        private readonly By licensePaymentExpPaymentTolltip = By.Id("actual-calculation-tooltip");
        private readonly By licensePaymentGSTInput = By.Id("input-amountGst");
        private readonly By licensePaymentRecordBttn = By.XPath("//button/div[contains(text(),'Record a Payment')]");
        private readonly By licensePaymentSaveBttn = By.XPath("//button/div[contains(text(),'Save payment')]");

        //Last Term Table Elements
        private int totalTermsInLease;
        private readonly By licenseTermsTotal = By.CssSelector("div[data-testid='leasePaymentsTable'] div[class='tr-wrapper'] div[class='td expander svg-btn']");

        //Last Payment Elements
        private int totalPaymentInTerm;
        private readonly By licensePaymentsTableTotal = By.CssSelector("div[data-testid='leasePaymentsTable'] div[data-testid='securityDepositsTable'] div[class='tbody'] div[class='tr-wrapper']");
        private readonly By licensePaymentDeleteTermBttn = By.CssSelector("button[title='delete term']");

        
        private SharedModals sharedModals;
        private LeaseDetails leaseDetails;

        public LeasePayments(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
            leaseDetails = new LeaseDetails(webDriver);
            totalTermsInLease = 0;
            totalPaymentInTerm = 0;
        }

        //Navigates to Payments Section
        public void NavigateToPaymentSection()
        {
            WaitUntilClickable(licensePaymentsLink);
            webDriver.FindElement(licensePaymentsLink).Click();
        }

        public void AddPeriodBttn() => ButtonElement(licencePaymentAddBttn);

        public void AddPeriod(Period period)    
        {
        
        //private readonly By licensePaymentPeriodStartDateInput = By.CssSelector("input[id='datepicker-startDate']");
        //private readonly By licensePaymentPeriodEndDateLabel = By.XPath("//label[contains(text(),'End date')]");
        //private readonly By licensePaymentPeriodEndDateInput = By.Id("datepicker-expiryDate");
        //private readonly By licensePaymentPeriodFrequencySelectLabel = By.XPath("//label[contains(text(),'Payment frequency')]");
        //private readonly By licensePaymentPeriodFrequencySelect = By.Id("input-leasePmtFreqTypeCode.id");
        //private readonly By licensePaymentPeriodAgreedPaymentLabel = By.XPath("//label[contains(text(),'Agreed payment ($)')]");
        //private readonly By licensePaymentPeriodAgreedPaymentInput = By.Id("input-paymentAmount");
        //private readonly By licensePaymentPeriodDueLabel = By.XPath("//label[contains(text(),'Payments due')]");
        //private readonly By licensePaymentPeriodDueTooltip = By.Id("paymentDueDateStr-tooltip");
        //private readonly By licensePaymentPeriodDueInput = By.Id("input-paymentDueDateStr");
        //private readonly By licensePaymentPeriodGSTLabel = By.XPath("//label[contains(text(),'Subject to GST?')]");
        //private readonly By licensePaymentPeriodGSTRadioBttns = By.Name("isGstEligible");
        //private readonly By licencePaymentPeriodGSTTrueRadioBttn = By.Id("input-isGstEligible");
        //private readonly By licencePaymentPeriodGSTFalseRadioBttn = By.Id("input-isGstEligible-2");
        //private readonly By licensePaymentPeriodLabel = By.XPath("//label[contains(text(),'Period Status')]");
        //private readonly By licensePaymentPeriodSelect = By.Id("input-statusTypeCode.id");

        ////Create Variable Period Modal Elements
        //private readonly By licensePaymentPeriodVariableBaseSubtitle = By.XPath("//div[contains(text(),'Add Base Rent')]/parent::div/parent::h2");
        //private readonly By licensePaymentPeriodVariableBaseSubtitleTooltip = By.Id("base-rent-tooltip");
        //private readonly By licensePaymentPeriodBaseFrequencyLabel = By.XPath("//label[@for='input-leasePmtFreqTypeCode.id']/span[contains(text(),'Payment frequency')]");
        //private readonly By licensePaymentPeriodBaseSelect = By.Id("input-leasePmtFreqTypeCode.id");
        //private readonly By licensePaymentPeriodBaseAgreedPaymentLabel = By.XPath("//label[@for='input-paymentAmount']/span[contains(text(),'Agreed payment ($)')]");
        //private readonly By licensePaymentPeriodBaseAgreedPaymentInput = By.Id("input-paymentAmount");
        //private readonly By licensePaymentPeriodBaseGSTLabel = By.XPath("//div[contains(text(),'Add Base Rent')]/parent::div/parent::h2/following-sibling::div/div/div/div/div/label[contains(text(),'Subject to GST')]");
        //private readonly By licensePaymentPeriodBaseGSTYesRadioBttn = By.Id("input-isGstEligible");
        //private readonly By licensePaymentPeriodBaseGSTNoRadioBttn = By.Id("input-isGstEligible-2");

        //private readonly By licensePaymentPeriodVariableAdditionalSubtitle = By.XPath("//div[contains(text(),'Add Additional Rent')]/parent::div/parent::h2");
        //private readonly By licensePaymentPeriodVariableAdditionalTooltip = By.Id("additional-rent-tooltip");
        //private readonly By licensePaymentPeriodAdditionalFrequencyLabel = By.XPath("//label[@for='input-additionalRentFreqTypeCode.id']/span[contains(text(),'Payment frequency')]");
        //private readonly By licensePaymentPeriodAdditionalSelect = By.Id("input-additionalRentFreqTypeCode.id");
        //private readonly By licensePaymentPeriodAdditionalAgreedPaymentLabel = By.XPath("//div[contains(text(),'Add Additional Rent')]/parent::div/parent::h2/following-sibling::div/div/div/div/label[contains(text(),'Agreed payment ($)')]");
        //private readonly By licensePaymentPeriodAdditionalAgreedPaymentInput = By.Id("input-additionalRentPaymentAmount");
        //private readonly By licensePaymentPeriodAdditionalGSTLabel = By.XPath("//div[contains(text(),'Add Additional Rent')]/parent::div/parent::h2/following-sibling::div/div/div/div/div/label[contains(text(),'Subject to GST')]");
        //private readonly By licensePaymentPeriodAdditionalYesRadioBttn = By.Id("input-isAdditionalRentGstEligible");
        //private readonly By licensePaymentPeriodAdditionalGSTNoRadioBttn = By.Id("input-isAdditionalRentGstEligible-2");

        //private readonly By licensePaymentPeriodVariableVariableSubtitle = By.XPath("//div[contains(text(),'Add Variable Rent')]/parent::div/parent::h2");
        //private readonly By licensePaymentPeriodVariableVariableTooltip = By.Id("variable-rent-tooltip");
        //private readonly By licensePaymentPeriodVariableFrequencyLabel = By.XPath("//label[@for='input-variableRentFreqTypeCode.id']/span[contains(text(),'Payment frequency')]");
        //private readonly By licensePaymentPeriodVariableSelect = By.Id("input-variableRentFreqTypeCode.id");
        //private readonly By licensePaymentPeriodVariableAgreedPaymentLabel = By.XPath("//div[contains(text(),'Add Variable Rent')]/parent::div/parent::h2/following-sibling::div/div/div/div/label[contains(text(),'Agreed payment ($)')]");
        //private readonly By licensePaymentPeriodVariableAgreedPaymentInput = By.Id("input-variableRentPaymentAmount");
        //private readonly By licensePaymentPeriodVariableGSTLabel = By.XPath("//div[contains(text(),'Add Variable Rent')]/parent::div/parent::h2/following-sibling::div/div/div/div/div/label[contains(text(),'Subject to GST')]");
        //private readonly By licensePaymentPeriodVariableYesRadioBttn = By.Id("input-isVariableRentGstEligible");
        //private readonly By licensePaymentPeriodVariableGSTNoRadioBttn = By.Id("input-isVariableRentGstEligible-2");


            //Periods Baseline
            WaitUntilClickable(licensePaymentPeriodDurationSelect);

            //Payment Type
            AssertTrueIsDisplayed(licensePaymentPeriodSelectTypeLabel);
            AssertTrueIsDisplayed(licensePaymentPeriodSelectTooltip);
            if (period.PaymentType == "Predetermined")
                webDriver.FindElement(licensePaymentPredefinedRadioInput).Click();
            else
                webDriver.FindElement(licensePaymentVariableRadioInput).Click();

            //Period Duration
            AssertTrueIsDisplayed(licensePaymentPeriodDurationLabel);
            AssertTrueIsDisplayed(licensePaymentPeriodDurationTooltip);
            ChooseSpecificSelectOption(licensePaymentPeriodDurationSelect, period.PeriodDuration);

            //Start Date
            AssertTrueIsDisplayed(licensePaymentPeriodStartDateLabel);
            AssertTrueIsDisplayed(licensePaymentPeriodStartDateTooltip);
            var startDateInputElement = webDriver.FindElement(licensePaymentPeriodStartDateInput);

            if (startDateInputElement.GetAttribute("value") != "")
                ClearInput(licensePaymentPeriodStartDateInput);
            
            startDateInputElement.Click();
            startDateInputElement.SendKeys(period.PeriodStartDate);

            //End Date
            WaitUntilClickable(licensePaymentTermEndDateInput);

            webDriver.FindElement(licensePaymentTermEndDateInput).Click();
            webDriver.FindElement(licensePaymentTermEndDateInput).SendKeys(period.PeriodEndDate);
            webDriver.FindElement(licensePaymentTermEndDateInput).SendKeys(Keys.Enter);

            ChooseSpecificSelectOption(licensePaymentTermFrequencySelect, period.PeriodPaymentFrequency);

            webDriver.FindElement(licensePaymentTermAgreedPaymentInput).SendKeys(period.PeriodAgreedPayment);
            webDriver.FindElement(licensePaymentTermDueInput).SendKeys(period.PeriodPaymentsDue);

            if (period.IsGSTEligible)
                webDriver.FindElement(licencePyamentTermGSTTrueRadioBttn).Click();
            else
            webDriver.FindElement(licencePyamentTermGSTFalseRadioBttn).Click();

            ChooseSpecificSelectOption(licensePaymentTermSelect, period.PeriodStatus);

            sharedModals.ModalClickOKBttn();

            Wait();
            totalTermsInLease = webDriver.FindElements(licenseTermsTotal).Count;
        }

        public void OpenPaymentTab(int index)
        {
            Wait();
            var selectedExpander = webDriver.FindElement(By.XPath("//div[@class='tr-wrapper']["+ index +"]/div/div[@class='td expander svg-btn']"));

            WaitUntilClickable(By.XPath("//div[@class='tr-wrapper']["+ index +"]/div/div[@class='td expander svg-btn']"));
            selectedExpander.Click();
        }

        public void AddPaymentBttn() => ButtonElement(licensePaymentRecordBttn);

        public void AddPayment(Payment payment)
        {
            WaitUntilClickable(licensePaymentSendDateInput);

            webDriver.FindElement(licensePaymentSendDateInput).Click();
            webDriver.FindElement(licensePaymentSendDateInput).SendKeys(payment.PaymentSentDate);
            webDriver.FindElement(licensePaymentSendDateInput).SendKeys(Keys.Enter);
            webDriver.FindElement(licensePaymentsModal).Click();

            ChooseSpecificSelectOption(licensePaymentMethodSelect, payment.PaymentMethod);

            webDriver.FindElement(licensePaymentAmountReceivedInput).SendKeys(payment.PaymentTotalReceived);
            webDriver.FindElement(licensePaymentAmountReceivedInput).Click();
            webDriver.FindElement(licensePaymentExpPaymentInput).Click();
            webDriver.FindElement(licensePaymentGSTInput).Click();

            ButtonElement(licensePaymentSaveBttn);

            Wait();
            totalPaymentInTerm = webDriver.FindElements(licensePaymentsTableTotal).Count;
        }

        public void DeleteLastTerm()
        {
            Wait();
            webDriver.FindElement(licensePaymentDeleteTermBttn).Click();

            WaitUntilVisible(licensePaymentsModal);
            sharedModals.ModalClickOKBttn();

            totalTermsInLease = webDriver.FindElements(licenseTermsTotal).Count;
        }

        public void DeleteLastPayment()
        {
            WaitUntilVisible(licensePaymentsTableTotal);

            var totalPayments = webDriver.FindElements(licensePaymentsTableTotal).Count();
            var lastPaymentDeleteIcon = By.CssSelector("div[class='tbody'] div[class='tr-wrapper']:nth-child("+totalPayments+") button[title='delete actual']");
            webDriver.FindElement(lastPaymentDeleteIcon).Click();

            WaitUntilVisible(licensePaymentsModal);
            sharedModals.ModalClickOKBttn();

            totalPaymentInTerm = webDriver.FindElements(licensePaymentsTableTotal).Count;
        }

        public int TotalTerms()
        {
            WaitUntilVisible(licenseTermsTotal);
            return webDriver.FindElements(licenseTermsTotal).Count;
        }

        public int TotalPayments()
        {
            WaitUntilVisible(licensePaymentsTableTotal);
            return webDriver.FindElements(licensePaymentsTableTotal).Count;
        }

        public void VerifyCreateTermForm()
        {
            WaitUntilVisible(licensePaymentTermStartDateInput);
            Assert.True(sharedModals.ModalHeader() == "Add a Term");

            AssertTrueIsDisplayed(licensePaymentPeriodStartDateLabel);
            AssertTrueIsDisplayed(licensePaymentTermStartDateInput);
            AssertTrueIsDisplayed(licensePaymentTermEndDateLabel);
            AssertTrueIsDisplayed(licensePaymentTermEndDateInput);
            AssertTrueIsDisplayed(licensePaymentTermFrequencySelectLabel);
            AssertTrueIsDisplayed(licensePaymentTermFrequencySelect);
            AssertTrueIsDisplayed(licensePaymentTermAgreedPaymentLabel);
            AssertTrueIsDisplayed(licensePaymentTermAgreedPaymentInput);
            AssertTrueIsDisplayed(licensePaymentTermDueLabel);
            AssertTrueIsDisplayed(licensePaymentTermDueTooltip);
            AssertTrueIsDisplayed(licensePaymentTermDueInput);
            AssertTrueIsDisplayed(licensePaymentTermGSTLabel);
            AssertTrueIsDisplayed(licensePaymentTermGSTRadioBttns);
            AssertTrueIsDisplayed(licensePaymentTermLabel);
            AssertTrueIsDisplayed(licensePaymentTermSelect);

            sharedModals.VerifyButtonsPresence();
        }

        public void VerifyInsertedTermTable(Period term)
        {
            Wait();
            WaitUntilVisible(By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalTermsInLease +"]/div[@class='tr']/div[@class='td expander svg-btn']/following-sibling::div[2]"));

            AssertTrueContentEquals(By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalTermsInLease +"]/div[@class='tr']/div[@class='td expander svg-btn']/following-sibling::div[2]"), ConcatenateDates(term.PeriodStartDate, term.PeriodEndDate));
            AssertTrueContentEquals(By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalTermsInLease +"]/div[@class='tr']/div[@class='td expander svg-btn']/following-sibling::div[3]"), term.PeriodPaymentFrequency);
            AssertTrueContentEquals(By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalTermsInLease +"]/div[@class='tr']/div[@class='td expander svg-btn']/following-sibling::div[4]"), term.PeriodPaymentsDue);
            AssertTrueContentEquals(By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalTermsInLease +"]/div[@class='tr']/div[@class='td expander svg-btn']/following-sibling::div[5]"), TransformCurrencyFormat(term.PeriodAgreedPayment));
            AssertTrueContentEquals(By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalTermsInLease +"]/div[@class='tr']/div[@class='td expander svg-btn']/following-sibling::div[6]"), TransformBooleanLeaseFormat(term.IsGSTEligible));

            if (term.IsGSTEligible)
            {
                AssertTrueContentEquals(By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='tbody']/div[@class='tr-wrapper'][" + totalTermsInLease + "]/div[@class='tr']/div[@class='td expander svg-btn']/following-sibling::div[7]"), CalculateGST(term.PeriodAgreedPayment, term.IsGSTEligible));
                AssertTrueContentEquals(By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='tbody']/div[@class='tr-wrapper'][" + totalTermsInLease + "]/div[@class='tr']/div[@class='td expander svg-btn']/following-sibling::div[8]"), CalculateExpectedTotal(term.PeriodAgreedPayment));
            }
            else
            {
                AssertTrueContentEquals(By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='tbody']/div[@class='tr-wrapper'][" + totalTermsInLease + "]/div[@class='tr']/div[@class='td expander svg-btn']/following-sibling::div[7]"), "-");
                AssertTrueContentEquals(By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='tbody']/div[@class='tr-wrapper'][" + totalTermsInLease + "]/div[@class='tr']/div[@class='td expander svg-btn']/following-sibling::div[8]"), TransformCurrencyFormat(term.PeriodAgreedPayment));
            }

            AssertTrueContentEquals(By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalTermsInLease +"]/div[@class='tr']/div[@class='td expander svg-btn']/following-sibling::div[9]"), CalculateExpectedTerm(term.PeriodPaymentFrequency, term.IsGSTEligible, term.PeriodAgreedPayment, term.PeriodStartDate, term.PeriodEndDate));
            AssertTrueContentEquals(By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalTermsInLease +"]/div[@class='tr']/div[@class='td expander svg-btn']/following-sibling::div[10]"), DisplayActualTotal(term.IsGSTEligible));
            AssertTrueContentEquals(By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalTermsInLease +"]/div[@class='tr']/div[@class='td expander svg-btn']/following-sibling::div[11]"), DisplayTerm(term.PeriodStatus));
        }

        public void VerifyCreatePaymentForm()
        {
            WaitUntilVisible(licensePaymentSendDateInput);
            Assert.True(sharedModals.ModalHeader() == "Payment details");

            AssertTrueIsDisplayed(licensePaymentSendDateLabel);
            AssertTrueIsDisplayed(licensePaymentSendDateInput);
            AssertTrueIsDisplayed(licensePaymentMethodLabel);
            AssertTrueIsDisplayed(licensePaymentMethodSelect);
            AssertTrueIsDisplayed(licensePaymentAmountReceivedLabel);
            AssertTrueIsDisplayed(licensePaymentAmountReceivedInput);
            AssertTrueIsDisplayed(licensePaymentExpPaymentLabel);
            AssertTrueIsDisplayed(licensePaymentExpPaymentInput);
            AssertTrueIsDisplayed(licensePaymentGSTLabel);
            AssertTrueIsDisplayed(licensePaymentExpPaymentTolltip);
            AssertTrueIsDisplayed(licensePaymentGSTInput);

            sharedModals.VerifyButtonsPresence();
        }

        public void VerifyPaymentTableHeader()
        {
            Wait();

            if (leaseDetails.GetLeaseAccountType() == "Receivable")
            {
                AssertTrueIsDisplayed(licensePaymentsReceivedDateColumn);
                AssertTrueIsDisplayed(licensePaymentsReceivedPaymentColumn);
                AssertTrueIsDisplayed(licensePaymentsReceivedTotalColumn);
            }
            else
            {
                AssertTrueIsDisplayed(licensePaymentsSendDateColumn);
                AssertTrueIsDisplayed(licensePaymentsGSTColumn);
                AssertTrueIsDisplayed(licensePaymentsSentTotalColumn);
            }

            AssertTrueIsDisplayed(licensePaymentsPaymentMethodColumn);
            AssertTrueIsDisplayed(licensePaymentsGSTTooltip);
            AssertTrueIsDisplayed(licensePaymentsGSTColumn);
            AssertTrueIsDisplayed(licensePaymentsGSTTooltip);
            AssertTrueIsDisplayed(licensePaymentsSendTotalTooltip);
            AssertTrueIsDisplayed(licensePaymentsPaymentStatusColumn);
            AssertTrueIsDisplayed(licensePaymentsPaymentStatusTooltip);
            AssertTrueIsDisplayed(licensePaymentsNotesColumn);
            AssertTrueIsDisplayed(licensePaymentsActionsColumn);
        }

        public void VerifyInsertedPayment(Payment payment)
        {
            WaitUntilVisible(By.XPath("//div[@data-testid='leasePaymentsTable']/div/div/div/div/div/div[@data-testid='securityDepositsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalPaymentInTerm +"]/div/div[1]"));

            AssertTrueContentEquals(By.XPath("//div[@data-testid='leasePaymentsTable']/div/div/div/div/div/div[@data-testid='securityDepositsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalPaymentInTerm +"]/div/div[1]"), TransformDateFormat(payment.PaymentSentDate));
            AssertTrueContentEquals(By.XPath("//div[@data-testid='leasePaymentsTable']/div/div/div/div/div/div[@data-testid='securityDepositsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalPaymentInTerm +"]/div/div[2]"), payment.PaymentMethod);
            AssertTrueContentEquals(By.XPath("//div[@data-testid='leasePaymentsTable']/div/div/div/div/div/div[@data-testid='securityDepositsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalPaymentInTerm +"]/div/div[3]"), TransformCurrencyFormat(payment.PaymentExpectedPayment));
            AssertTrueContentEquals(By.XPath("//div[@data-testid='leasePaymentsTable']/div/div/div/div/div/div[@data-testid='securityDepositsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalPaymentInTerm +"]/div/div[4]"), TransformCurrencyFormat(payment.PaymentGST));
            AssertTrueContentEquals(By.XPath("//div[@data-testid='leasePaymentsTable']/div/div/div/div/div/div[@data-testid='securityDepositsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalPaymentInTerm +"]/div/div[5]"),TransformCurrencyFormat(payment.PaymentTotalReceived));
            AssertTrueContentEquals(By.XPath("//div[@data-testid='leasePaymentsTable']/div/div/div/div/div/div[@data-testid='securityDepositsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalPaymentInTerm +"]/div/div[6]"), payment.PaymentStatus);
            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='leasePaymentsTable']/div/div/div/div/div/div[@data-testid='securityDepositsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalPaymentInTerm +"]/div/div[7]/button[@title='notes']"));
            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='leasePaymentsTable']/div/div/div/div/div/div[@data-testid='securityDepositsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalPaymentInTerm +"]/div/div[8]/div/button[@title='edit actual']"));
            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='leasePaymentsTable']/div/div/div/div/div/div[@data-testid='securityDepositsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalPaymentInTerm +"]/div/div[8]/div/button[@title='delete actual']"));
        }

        private string ConcatenateDates(string startDate, string endDate)
        {
            var startDateFormat = DateTime.Parse(startDate);
            var endDateFormat = DateTime.Parse(endDate);

            return startDateFormat.ToString("MMM dd, yyyy") +" - "+ endDateFormat.ToString("MMM dd, yyyy");
        }

        private string CalculateGST(string amount, bool gst)
        {
            if (gst)
            {
                decimal value = decimal.Parse(amount) * 0.05m;
                return "$" + value.ToString("#,##0.00");
            }
            else
                return "-";
        }

        private string DisplayTerm(string termStatus)
        {
            if (termStatus == "Exercised")
                return "Y";
            else
                return "N";
        }

        private string CalculateExpectedTotal(string amount)
        {
            decimal expectedValue = decimal.Parse(amount);
            decimal gstValue = decimal.Parse(amount) * 0.05m;
            decimal total = expectedValue + gstValue;

            return "$" + total.ToString("#,##0.00");
        }

        private string CalculateExpectedTerm(string frequency, bool gst, string amount, string startDate, string endDate)
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
                    frequencyNumber = 1;
                    break;
            }

            if (gst)
                unitAmount += decimal.Parse(amount) * 0.05m;

            var finalAmount = frequencyNumber * unitAmount;

            if (finalAmount == 0)
                return "-";
            else
                return "$" + finalAmount.ToString("#,##0.00");
        }

        private string DisplayActualTotal(bool GST)
        {
            if (GST)
                return "$0.00";
            else
                return "-";
        }
    }
}
