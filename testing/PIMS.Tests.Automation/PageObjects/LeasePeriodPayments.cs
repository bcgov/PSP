using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class LeasePeriodPayments : PageObjectBase
    {
        //Payments Tab Link Element
        private readonly By licensePaymentsLink = By.XPath("//a[contains(text(),'Payments')]");

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
        private readonly By licensePaymentExpectedTermTooltip = By.Id("expectedPeriodTooltip");
        private readonly By licencePaymentColumnActualTotal = By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Actual total ($)')]");
        private readonly By licensePaymentActualTotalTooltip = By.Id("actualTotalTooltip");
        private readonly By licensePaymentColumnExercised = By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Exercised?')]");
        private readonly By licencePaymentColumnActions = By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Actions')]");
        private readonly By licencePaymentsNoRows = By.CssSelector("div[data-testid='leasePaymentsTable'] div[class='no-rows-message']");

        //Category Table Header & Static Elements
        private readonly By licenceCategoryPaymentsCategoryColumn = By.XPath("//div[@data-testid='variablePeriodTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Category')]");
        private readonly By licenceCategoryPaymentsPaymentFrequencyColumn = By.XPath("//div[@data-testid='variablePeriodTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Payment Fequency')]");
        private readonly By licenceCategoryPaymentsExpectedPaymentColumn = By.XPath("//div[@data-testid='variablePeriodTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Expected Payment ($)')]");
        private readonly By licenceCategoryPaymentsIsGSTColumn = By.XPath("//div[@data-testid='variablePeriodTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'GST?')]");
        private readonly By licenceCategoryPaymentsGSTColumn = By.XPath("//div[@data-testid='variablePeriodTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'GST ($)')]");
        private readonly By licenceCategoryPaymentsExpectedPeriodColumn = By.XPath("//div[@data-testid='variablePeriodTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Expected total ($)')]");
        private readonly By licenceCategoryPaymentsActualTotalColumn = By.XPath("//div[@data-testid='variablePeriodTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Actual total ($)')]");

        private readonly By licenseCategoryBaseRentRow = By.XPath("//div[@data-testid='variablePeriodTable']/div[@class='tbody']/div[@class='tr-wrapper']/div/div[contains(text(),'Base Rent')]");
        private readonly By licenseCategoryBaseRentTooltip = By.Id("variable-period-BASE");
        private readonly By licenseCategoryBaseFrequencyTableContent = By.XPath("//div[@data-testid='variablePeriodTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[2]");
        private readonly By licenseCategoryBaseExpectedPaymentTableContent = By.XPath("//div[@data-testid='variablePeriodTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[3]");
        private readonly By licenseCategoryBaseIsGSTTableContent = By.XPath("//div[@data-testid='variablePeriodTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[4]");
        private readonly By licenseCategoryBaseGSTTotalTableContent = By.XPath("//div[@data-testid='variablePeriodTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[5]");
        private readonly By licenseCategoryBaseExpectedTotalTableContent = By.XPath("//div[@data-testid='variablePeriodTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[6]");
        private readonly By licenseCategoryBaseActualTotalTableContent = By.XPath("//div[@data-testid='variablePeriodTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[7]");


        private readonly By licenseCategoryAdditionalRentRow = By.XPath("//div[@data-testid='variablePeriodTable']/div[@class='tbody']/div[@class='tr-wrapper']/div/div[contains(text(),'Additional Rent')]");
        private readonly By licenseCategoryAdditionalRentTooltip = By.Id("variable-period-ADDL");
        private readonly By licenseCategoryAdditionalFrequencyTableContent = By.XPath("//div[@data-testid='variablePeriodTable']/div[@class='tbody']/div[@class='tr-wrapper'][2]/div/div[2]");
        private readonly By licenseCategoryAdditionalExpectedPaymentTableContent = By.XPath("//div[@data-testid='variablePeriodTable']/div[@class='tbody']/div[@class='tr-wrapper'][2]/div/div[3]");
        private readonly By licenseCategoryAdditionalIsGSTTableContent = By.XPath("//div[@data-testid='variablePeriodTable']/div[@class='tbody']/div[@class='tr-wrapper'][2]/div/div[4]");
        private readonly By licenseCategoryAdditionalGSTTotalTableContent = By.XPath("//div[@data-testid='variablePeriodTable']/div[@class='tbody']/div[@class='tr-wrapper'][2]/div/div[5]");
        private readonly By licenseCategoryAdditionalExpectedTotalTableContent = By.XPath("//div[@data-testid='variablePeriodTable']/div[@class='tbody']/div[@class='tr-wrapper'][2]/div/div[6]");
        private readonly By licenseCategoryAdditionalActualTotalTableContent = By.XPath("//div[@data-testid='variablePeriodTable']/div[@class='tbody']/div[@class='tr-wrapper'][2]/div/div[7]");

        private readonly By licenseCategoryVariableRentRow = By.XPath("//div[@data-testid='variablePeriodTable']/div[@class='tbody']/div[@class='tr-wrapper']/div/div[contains(text(),'Variable Rent')]");
        private readonly By licenseCategoryVariableRentTooltip = By.Id("variable-period-VBL");
        private readonly By licenseCategoryVariableFrequencyTableContent = By.XPath("//div[@data-testid='variablePeriodTable']/div[@class='tbody']/div[@class='tr-wrapper'][3]/div/div[2]");
        private readonly By licenseCategoryVariableExpectedPaymentTableContent = By.XPath("//div[@data-testid='variablePeriodTable']/div[@class='tbody']/div[@class='tr-wrapper'][3]/div/div[3]");
        private readonly By licenseCategoryVariableIsGSTTableContent = By.XPath("//div[@data-testid='variablePeriodTable']/div[@class='tbody']/div[@class='tr-wrapper'][3]/div/div[4]");
        private readonly By licenseCategoryVariableGSTTotalTableContent = By.XPath("//div[@data-testid='variablePeriodTable']/div[@class='tbody']/div[@class='tr-wrapper'][3]/div/div[5]");
        private readonly By licenseCategoryVariableExpectedTotalTableContent = By.XPath("//div[@data-testid='variablePeriodTable']/div[@class='tbody']/div[@class='tr-wrapper'][3]/div/div[6]");
        private readonly By licenseCategoryVariableActualTotalTableContent = By.XPath("//div[@data-testid='variablePeriodTable']/div[@class='tbody']/div[@class='tr-wrapper'][3]/div/div[7]");

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

        //Create Period Modal Common Elements
        private readonly By licensePaymentPeriodSelectTypeLabel = By.XPath("//label[contains(text(),'Select payment type')]");
        private readonly By licensePaymentPeriodSelectTooltip = By.Id("section-field-tooltip");
        private readonly By licensePaymentPredefinedRadioInput = By.CssSelector("input[data-testid='radio-isvariable-predetermined']");
        private readonly By licensePaymentVariableRadioInput = By.CssSelector("input[data-testid='radio-isvariable-variable']");

        private readonly By licensePaymentPeriodDurationLabel = By.XPath("//span[contains(text(),'Period duration')]");
        private readonly By licensePaymentPeriodDurationTooltip = By.Id("isFlexible-tooltip");
        private readonly By licensePaymentPeriodDurationSelect = By.Id("input-isFlexible");

        private readonly By licensePaymentPeriodStartDateLabel = By.XPath("//span[contains(text(),'Start date')]");
        private readonly By licensePaymentPeriodStartDateTooltip = By.Id("startDate-tooltip");
        private readonly By licensePaymentPeriodStartDateInput = By.CssSelector("input[id='datepicker-startDate']");

        private readonly By licensePaymentPeriodEndDateLabel = By.XPath("//span[contains(text(),'End date')]");
        private readonly By licensePaymentPeriodEndDateTooltip = By.Id("expiryDate-tooltip");
        private readonly By licensePaymentPeriodEndDateInput = By.Id("datepicker-expiryDate");

        private readonly By licensePaymentPeriodDueLabel = By.XPath("//label[contains(text(),'Payment due')]");
        private readonly By licensePaymentPeriodDueTooltip = By.Id("paymentDueDateStr-tooltip");
        private readonly By licensePaymentPeriodDueInput = By.Id("input-paymentDueDateStr");

        private readonly By licensePaymentPeriodStatusLabel = By.XPath("//span[contains(text(),'Period status')]");
        private readonly By licensePaymentPeriodStatusTooltip = By.Id("statusTypeCode.id-tooltip");
        private readonly By licensePaymentPeriodStatusSelect = By.Id("input-statusTypeCode.id");

        //Create Predefined Period Modal Elements
        private readonly By licensePaymentPeriodFrequencySelectLabel = By.XPath("//span[contains(text(),'Payment frequency')]");
        private readonly By licensePaymentPeriodFrequencySelect = By.Id("input-leasePmtFreqTypeCode.id");
        private readonly By licensePaymentPeriodAgreedPaymentLabel = By.XPath("//label[contains(text(),'Payment (before tax)')]");
        private readonly By licensePaymentPeriodAgreedPaymentInput = By.Id("input-paymentAmount");
        private readonly By licensePaymentPeriodGSTLabel = By.XPath("//label[contains(text(),'Subject to GST?')]");
        private readonly By licensePaymentPeriodGSTRadioBttns = By.Name("isGstEligible");
        private readonly By licensePaymentPeriodGSTAmountLabel = By.XPath("//label[contains(text(),'GST Amount')]");
        private readonly By licensePaymentPeriodGSTAmountInput = By.Id("input-gstAmount");
        private readonly By licensePaymentPeriodTotalAmountLabel = By.XPath("//label[contains(text(),'Total Payment')]");
        private readonly By licensePaymentPeriodTotalAmountContent = By.XPath("//label[contains(text(),'Total Payment')]/parent::div/following-sibling::div");

        //Create Variable Period Modal Elements
        private readonly By licensePaymentPeriodVariableBaseSubtitle = By.XPath("//div[contains(text(),'Add Base Rent')]/parent::div/parent::h2");
        private readonly By licensePaymentPeriodVariableBaseSubtitleTooltip = By.Id("base-rent-tooltip");
        private readonly By licensePaymentPeriodBaseFrequencyLabel = By.XPath("//label[@for='input-leasePmtFreqTypeCode.id']/span[contains(text(),'Payment frequency')]");
        private readonly By licensePaymentPeriodBaseAgreedPaymentLabel = By.XPath("//label[contains(text(),'Payment (before tax)')]");
        private readonly By licensePaymentPeriodBaseGSTLabel = By.XPath("//div[contains(text(),'Add Base Rent')]/parent::div/parent::h2/following-sibling::div/div/div/div/div/label[contains(text(),'Subject to GST')]");
        private readonly By licensePaymentPeriodBaseGSTAmountLabel = By.XPath("//div[contains(text(),'Add Base Rent')]/parent::div/parent::h2/following-sibling::div/div/div/div/label[contains(text(),'GST Amount')]");
        private readonly By licensePaymentPeriodBaseTotalAmountLabel = By.XPath("//div[contains(text(),'Add Base Rent')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Total Payment')]");
        private readonly By licensePaymentPeriodBaseTotalAmountContent = By.XPath("//div[contains(text(),'Add Base Rent')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Total Payment')]/parent::div/following-sibling::div");

        private readonly By licensePaymentPeriodVariableAdditionalSubtitle = By.XPath("//div[contains(text(),'Add Additional Rent')]/parent::div/parent::h2");
        private readonly By licensePaymentPeriodVariableAdditionalTooltip = By.Id("additional-rent-tooltip");
        private readonly By licensePaymentPeriodAdditionalFrequencyLabel = By.XPath("//label[@for='input-additionalRentFreqTypeCode.id']/span[contains(text(),'Payment frequency')]");
        private readonly By licensePaymentPeriodAdditionalSelect = By.Id("input-additionalRentFreqTypeCode.id");
        private readonly By licensePaymentPeriodAdditionalAgreedPaymentLabel = By.XPath("//div[contains(text(),'Add Additional Rent')]/parent::div/parent::h2/following-sibling::div/div/div/div/label[contains(text(),'Payment (before tax)')]");
        private readonly By licensePaymentPeriodAdditionalAgreedPaymentInput = By.Id("input-additionalRentPaymentAmount");
        private readonly By licensePaymentPeriodAdditionalGSTLabel = By.XPath("//div[contains(text(),'Add Additional Rent')]/parent::div/parent::h2/following-sibling::div/div/div/div/div/label[contains(text(),'Subject to GST')]");
        private readonly By licensePaymentPeriodAdditionalGSTRadioBttns = By.Name("isAdditionalRentGstEligible");
        private readonly By licensePaymentPeriodAdditionalGSTAmountLabel = By.XPath("//div[contains(text(),'Add Additional Rent')]/parent::div/parent::h2/following-sibling::div/div/div/div/label[contains(text(),'GST Amount')]");
        private readonly By licensePaymentPeriodAdditionalGSTAmountInput = By.Id("input-additionalGstAmount");
        private readonly By licensePaymentPeriodAdditionalTotalAmountLabel = By.XPath("//div[contains(text(),'Add Additional Rent')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Total Payment')]");
        private readonly By licensePaymentPeriodAdditionalTotalAmountContent = By.XPath("//div[contains(text(),'Add Additional Rent')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Total Payment')]/parent::div/following-sibling::div");

        private readonly By licensePaymentPeriodVariableVariableSubtitle = By.XPath("//div[contains(text(),'Add Variable Rent')]/parent::div/parent::h2");
        private readonly By licensePaymentPeriodVariableVariableTooltip = By.Id("variable-rent-tooltip");
        private readonly By licensePaymentPeriodVariableFrequencyLabel = By.XPath("//label[@for='input-variableRentFreqTypeCode.id']/span[contains(text(),'Payment frequency')]");
        private readonly By licensePaymentPeriodVariableSelect = By.Id("input-variableRentFreqTypeCode.id");
        private readonly By licensePaymentPeriodVariableAgreedPaymentLabel = By.XPath("//div[contains(text(),'Add Variable Rent')]/parent::div/parent::h2/following-sibling::div/div/div/div/label[contains(text(),'Payment (before tax)')]");
        private readonly By licensePaymentPeriodVariableAgreedPaymentInput = By.Id("input-variableRentPaymentAmount");
        private readonly By licensePaymentPeriodVariableGSTLabel = By.XPath("//div[contains(text(),'Add Variable Rent')]/parent::div/parent::h2/following-sibling::div/div/div/div/div/label[contains(text(),'Subject to GST')]");
        private readonly By licensePaymentPeriodVariableGSTRadioBttns = By.Name("isVariableRentGstEligible");
        private readonly By licensePaymentPeriodVariableGSTAmountLabel = By.XPath("//div[contains(text(),'Add Variable Rent')]/parent::div/parent::h2/following-sibling::div/div/div/div/label[contains(text(),'GST Amount')]");
        private readonly By licensePaymentPeriodVariableGSTAmountInput = By.Id("input-variableRentGstAmount");
        private readonly By licensePaymentPeriodVariableTotalAmountLabel = By.XPath("//div[contains(text(),'Add Variable Rent')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Total Payment')]");
        private readonly By licensePaymentPeriodVariableTotalAmountContent = By.XPath("//div[contains(text(),'Add Variable Rent')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Total Payment')]/parent::div/following-sibling::div");

        //Create Payment Elements
        private readonly By licenseAddPaymentBttn = By.XPath("//div[contains(text(),'Payments')]/following-sibling::div/button");
        private readonly By licensePaymentSendDateLabel = By.XPath("//span[contains(text(),'Sent date')]");
        private readonly By licensePaymentSendDateInput = By.Id("datepicker-receivedDate");
        private readonly By licensePaymentMethodLabel = By.XPath("//span[contains(text(),'Method')]");
        private readonly By licensePaymentMethodSelect = By.Id("input-leasePaymentMethodType.id");
        private readonly By licensePaymentCategoryLabel = By.XPath("//span[contains(text(),'Payment category')]/parent::label");
        private readonly By licensePaymentCategorySelect = By.Id("input-leasePaymentCategoryTypeCode.id");
        private readonly By licensePaymentAmountReceivedLabel = By.XPath("//label[contains(text(),'Total received ($)')]");
        private readonly By licensePaymentAmountReceivedInput = By.Id("input-amountTotal");
        private readonly By licensePaymentExpPaymentLabel = By.XPath("//label[contains(text(),'Expected payment ($)')]");
        private readonly By licensePaymentExpPaymentInput = By.Id("input-amountPreTax");
        private readonly By licensePaymentGSTLabel = By.XPath("//label[contains(text(),'GST ($)')]");
        private readonly By licensePaymentExpPaymentTooltip = By.Id("actual-calculation-tooltip");
        private readonly By licensePaymentGSTInput = By.Id("input-amountGst");
        private readonly By licensePaymentSaveBttn = By.XPath("//button/div[contains(text(),'Save payment')]");

        //Last Term Table Elements
        private int totalPeriodsInLease;
        private readonly By licenseTermsTotal = By.CssSelector("div[data-testid='leasePaymentsTable'] div[class='tr-wrapper'] div[class='td expander svg-btn']");

        //Last Payment Elements
        private int totalPaymentInPeriod;
        private readonly By licensePaymentDeleteTermBttn = By.CssSelector("button[title='delete period']");

        private readonly SharedModals sharedModals;

        public LeasePeriodPayments(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
            totalPeriodsInLease = 0;
            totalPaymentInPeriod = 0;
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
            //Periods Baseline
            WaitUntilClickable(licensePaymentPeriodDurationSelect);

            //Payment Type
            AssertTrueIsDisplayed(licensePaymentPeriodSelectTypeLabel);
            AssertTrueIsDisplayed(licensePaymentPeriodSelectTooltip);
            if (period.PeriodPaymentType == "Predetermined")
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
            AssertTrueIsDisplayed(licensePaymentPeriodEndDateLabel);
            AssertTrueIsDisplayed(licensePaymentPeriodEndDateTooltip);

            webDriver.FindElement(licensePaymentPeriodEndDateInput).Click();
            webDriver.FindElement(licensePaymentPeriodEndDateInput).SendKeys(period.PeriodEndDate);
            webDriver.FindElement(licensePaymentPeriodEndDateInput).SendKeys(Keys.Enter);

            //Period Due
            AssertTrueIsDisplayed(licensePaymentPeriodDueLabel);
            AssertTrueIsDisplayed(licensePaymentPeriodDueTooltip);
            if (period.PeriodPaymentsDue != "")
                webDriver.FindElement(licensePaymentPeriodDueInput).SendKeys(period.PeriodPaymentsDue);

            //Period Status
            AssertTrueIsDisplayed(licensePaymentPeriodStatusLabel);
            AssertTrueIsDisplayed(licensePaymentPeriodStatusTooltip);
            ChooseSpecificSelectOption(licensePaymentPeriodStatusSelect, period.PeriodStatus);

            //Predetermined Base Rent
            if (period.PeriodPaymentType == "Predetermined")
            {
                //Payment Frequency
                AssertTrueIsDisplayed(licensePaymentPeriodFrequencySelectLabel);
                ChooseSpecificSelectOption(licensePaymentPeriodFrequencySelect, period.PeriodBasePaymentFrequency);

                //Agreed Payment
                AssertTrueIsDisplayed(licensePaymentPeriodAgreedPaymentLabel);
                webDriver.FindElement(licensePaymentPeriodAgreedPaymentInput).SendKeys(period.PeriodBaseAgreedPayment);

                //Is GST Eligible
                AssertTrueIsDisplayed(licensePaymentPeriodGSTLabel);
                ChooseSpecificRadioButton(licensePaymentPeriodGSTRadioBttns, period.PeriodBaseIsGSTEligible);

                //GST Amount
                if (period.PeriodBaseIsGSTEligible == "true")
                {
                    AssertTrueIsDisplayed(licensePaymentPeriodGSTAmountLabel);
                    webDriver.FindElement(licensePaymentPeriodGSTAmountInput).SendKeys(period.PeriodBaseGSTAmount);
                }

                //Total Payment
                AssertTrueIsDisplayed(licensePaymentPeriodTotalAmountLabel);
                AssertTrueContentEquals(licensePaymentPeriodTotalAmountContent, TransformCurrencyFormat(period.PeriodBaseTotalPaymentAmount));
            }
            else
            {
                //BASE RENT
                AssertTrueIsDisplayed(licensePaymentPeriodVariableBaseSubtitle);
                AssertTrueIsDisplayed(licensePaymentPeriodVariableBaseSubtitleTooltip);

                //Payment Frequency
                AssertTrueIsDisplayed(licensePaymentPeriodBaseFrequencyLabel);
                ChooseSpecificSelectOption(licensePaymentPeriodFrequencySelect, period.PeriodBasePaymentFrequency);

                //Agreed Payment
                AssertTrueIsDisplayed(licensePaymentPeriodBaseAgreedPaymentLabel);
                webDriver.FindElement(licensePaymentPeriodAgreedPaymentInput).SendKeys(period.PeriodBaseAgreedPayment);

                //Is GST Eligible
                AssertTrueIsDisplayed(licensePaymentPeriodBaseGSTLabel);
                ChooseSpecificRadioButton(licensePaymentPeriodGSTRadioBttns, period.PeriodBaseIsGSTEligible);

                //GST Amount
                if (period.PeriodBaseIsGSTEligible == "true")
                {
                    AssertTrueIsDisplayed(licensePaymentPeriodBaseGSTAmountLabel);
                    webDriver.FindElement(licensePaymentPeriodGSTAmountInput).SendKeys(period.PeriodBaseGSTAmount);
                } 

                //Total Payment
                AssertTrueIsDisplayed(licensePaymentPeriodBaseTotalAmountLabel);
                if(period.PeriodBaseTotalPaymentAmount != "")
                    AssertTrueContentEquals(licensePaymentPeriodBaseTotalAmountContent, TransformCurrencyFormat(period.PeriodBaseTotalPaymentAmount));


                //ADDITIONAL RENT
                AssertTrueIsDisplayed(licensePaymentPeriodVariableAdditionalSubtitle);
                AssertTrueIsDisplayed(licensePaymentPeriodVariableAdditionalTooltip);

                //Additional Payment Frequency
                AssertTrueIsDisplayed(licensePaymentPeriodAdditionalFrequencyLabel);
                if (period.PeriodAdditionalPaymentFrequency != "")
                    ChooseSpecificSelectOption(licensePaymentPeriodAdditionalSelect, period.PeriodAdditionalPaymentFrequency);

                //Additional Agreed Payment
                AssertTrueIsDisplayed(licensePaymentPeriodAdditionalAgreedPaymentLabel);
                if(period.PeriodAdditionalAgreedPayment != "")
                    webDriver.FindElement(licensePaymentPeriodAdditionalAgreedPaymentInput).SendKeys(period.PeriodAdditionalAgreedPayment);

                //Additional GST Eligible
                AssertTrueIsDisplayed(licensePaymentPeriodAdditionalGSTLabel);
                if (period.PeriodAdditionalIsGSTEligible != "")
                    ChooseSpecificRadioButton(licensePaymentPeriodAdditionalGSTRadioBttns, period.PeriodAdditionalIsGSTEligible);

                //GST Amount
                if (period.PeriodAdditionalIsGSTEligible == "true")
                {
                    AssertTrueIsDisplayed(licensePaymentPeriodAdditionalGSTAmountLabel);
                    webDriver.FindElement(licensePaymentPeriodAdditionalGSTAmountInput).SendKeys(period.PeriodAdditionalGSTAmount);
                }   

                //Total Payment
                AssertTrueIsDisplayed(licensePaymentPeriodAdditionalTotalAmountLabel);
                if(period.PeriodAdditionalTotalPaymentAmount != "")
                    AssertTrueContentEquals(licensePaymentPeriodAdditionalTotalAmountContent, TransformCurrencyFormat(period.PeriodAdditionalTotalPaymentAmount));


                //VARIABLE RENT
                AssertTrueIsDisplayed(licensePaymentPeriodVariableVariableSubtitle);
                AssertTrueIsDisplayed(licensePaymentPeriodVariableVariableTooltip);

                //Variable Payment Frequency
                AssertTrueIsDisplayed(licensePaymentPeriodVariableFrequencyLabel);
                if (period.PeriodVariablePaymentFrequency != "")
                    ChooseSpecificSelectOption(licensePaymentPeriodVariableSelect, period.PeriodVariablePaymentFrequency);

                //Variable Agreed Payment
                AssertTrueIsDisplayed(licensePaymentPeriodVariableAgreedPaymentLabel);
                if (period.PeriodVariableAgreedPayment != "")
                    webDriver.FindElement(licensePaymentPeriodVariableAgreedPaymentInput).SendKeys(period.PeriodVariableAgreedPayment);

                //Additional GST Eligible
                AssertTrueIsDisplayed(licensePaymentPeriodVariableGSTLabel);
                if (period.PeriodVariableIsGSTEligible != "")
                    ChooseSpecificRadioButton(licensePaymentPeriodVariableGSTRadioBttns, period.PeriodVariableIsGSTEligible);

                //GST Amount
                if (period.PeriodVariableIsGSTEligible == "true")
                {
                    AssertTrueIsDisplayed(licensePaymentPeriodVariableGSTAmountLabel);
                    ClearInput(licensePaymentPeriodVariableGSTAmountInput);
                    webDriver.FindElement(licensePaymentPeriodVariableGSTAmountInput).SendKeys(period.PeriodVariableGSTAmount);
                }

                //Total Payment
                AssertTrueIsDisplayed(licensePaymentPeriodVariableTotalAmountLabel);
                if(period.PeriodVariableTotalPaymentAmount != "")
                    AssertTrueContentEquals(licensePaymentPeriodVariableTotalAmountContent, TransformCurrencyFormat(period.PeriodVariableTotalPaymentAmount));
            }

            sharedModals.ModalClickOKBttn();

            Wait();
            totalPeriodsInLease = webDriver.FindElements(licenseTermsTotal).Count;
        }

        public void OpenClosePeriodCategoryPayments(int elementIdx)
        {
            Wait();
            var selectedExpander = webDriver.FindElement(By.XPath("//div[@class='tr-wrapper']["+ elementIdx +"]/div/div[@class='td expander svg-btn']"));

            WaitUntilClickable(By.XPath("//div[@class='tr-wrapper']["+ elementIdx +"]/div/div[@class='td expander svg-btn']"));
            selectedExpander.Click();
        }

        public void AddPaymentBttn(int parentIdx)
        {
            By addPaymentButton = By.XPath("//b[contains(text(),'Period "+ parentIdx +"')]/parent::div/parent::div/parent::div/following-sibling::div/div/h2/div/div/div/div/button");
            ButtonElement(addPaymentButton);
        }

        public void AddPayment(Payment payment, string periodType)
        {
            Wait();

            //Sent Date
            AssertTrueIsDisplayed(licensePaymentSendDateLabel);
            ClearInput(licensePaymentSendDateInput);
            webDriver.FindElement(licensePaymentSendDateInput).Click();
            webDriver.FindElement(licensePaymentSendDateInput).SendKeys(payment.PaymentSentDate);
            webDriver.FindElement(licensePaymentSendDateInput).SendKeys(Keys.Enter);
            webDriver.FindElement(licensePaymentsModal).Click();

            //Method
            AssertTrueIsDisplayed(licensePaymentMethodLabel);
            ChooseSpecificSelectOption(licensePaymentMethodSelect, payment.PaymentMethod);

            //Payment Category - for variable payments
            if (periodType ==  "Variable")
            {
                AssertTrueIsDisplayed(licensePaymentCategoryLabel);
                ChooseSpecificSelectOption(licensePaymentCategorySelect, payment.PaymentCategory);  
            }

            //Total Received
            AssertTrueIsDisplayed(licensePaymentAmountReceivedLabel);
            ClearInput(licensePaymentAmountReceivedInput);
            webDriver.FindElement(licensePaymentAmountReceivedInput).SendKeys(payment.PaymentTotalReceived);
            webDriver.FindElement(licensePaymentAmountReceivedInput).Click();
            webDriver.FindElement(licensePaymentExpPaymentInput).Click();
            webDriver.FindElement(licensePaymentGSTInput).Click();

            if (payment.PaymentIsGSTApplicable == "true")
            {
                //Expected Payment
                AssertTrueIsDisplayed(licensePaymentExpPaymentLabel);
                AssertTrueIsDisplayed(licensePaymentExpPaymentTooltip);
                AssertTrueElementValueEquals(licensePaymentExpPaymentInput, TransformCurrencyFormat(payment.PaymentExpectedPayment));

                //GST
                AssertTrueIsDisplayed(licensePaymentGSTLabel);
                AssertTrueElementValueEquals(licensePaymentGSTInput, TransformCurrencyFormat(payment.PaymentGST));
            }
            else
            {
                //Expected Payment
                AssertTrueIsDisplayed(licensePaymentExpPaymentLabel);
                AssertTrueIsDisplayed(licensePaymentExpPaymentTooltip);
                ClearInput(licensePaymentExpPaymentInput);
                webDriver.FindElement(licensePaymentExpPaymentInput).SendKeys(payment.PaymentExpectedPayment);

                //GST
                AssertTrueIsDisplayed(licensePaymentGSTLabel);
                ClearInput(licensePaymentGSTInput);
            }
           

            //Save Button
            ButtonElement(licensePaymentSaveBttn);

            Wait();
            totalPaymentInPeriod = webDriver.FindElements(By.XPath("//b[contains(text(), 'Period "+ payment.PeriodParentIndex +"')]/parent::div/parent::div/parent::div/following-sibling::div/div/div/div[@data-testid='paymentsTable']/div[@class='tbody']/div/div")).Count;
        }

        public void DeleteLastPeriod()
        {
            Wait();
            webDriver.FindElement(licensePaymentDeleteTermBttn).Click();

            WaitUntilVisible(licensePaymentsModal);
            sharedModals.ModalClickOKBttn();

            totalPeriodsInLease = webDriver.FindElements(licenseTermsTotal).Count;
        }

        public void DeleteLastPayment(int periodIdx)
        {
            WaitUntilVisible(By.XPath("//b[contains(text(), 'Period "+ periodIdx +"')]/parent::div/parent::div/parent::div/following-sibling::div/div/div/div[@data-testid='paymentsTable']/div[@class='tbody']/div/div"));

            var totalPayments = webDriver.FindElements(By.XPath("//b[contains(text(), 'Period "+ periodIdx +"')]/parent::div/parent::div/parent::div/following-sibling::div/div/div/div[@data-testid='paymentsTable']/div[@class='tbody']/div/div")).Count;
            var lastPaymentDeleteIcon = By.CssSelector("div[class='tbody'] div[class='tr-wrapper']:nth-child("+ totalPayments +") button[title='delete actual']");
            webDriver.FindElement(lastPaymentDeleteIcon).Click();

            WaitUntilVisible(licensePaymentsModal);
            sharedModals.ModalClickOKBttn();

            totalPaymentInPeriod = webDriver.FindElements(By.XPath("//b[contains(text(), 'Period "+ periodIdx +"')]/parent::div/parent::div/parent::div/following-sibling::div/div/div/div[@data-testid='paymentsTable']/div[@class='tbody']/div/div")).Count;
        }

        public void VerifyPeriodsTabInit()
        {
            Wait();

            AssertTrueIsDisplayed(licencePaymentsSubtitle);
            AssertTrueIsDisplayed(licencePaymentAddBttn);

            AssertTrueIsDisplayed(licencePaymentColumnStartEndDate);
            AssertTrueIsDisplayed(licensePaymentColumnPaymentFreq);
            AssertTrueIsDisplayed(licencePaymentColumnPaymentDue);
            AssertTrueIsDisplayed(licensePaymentColumnExpectedPay);
            AssertTrueIsDisplayed(licensePaymentExpectedPayTooltip);
            AssertTrueIsDisplayed(licencePaymentColumnGSTBoolean);
            AssertTrueIsDisplayed(licensePaymentColumnGSTTotal);
            AssertTrueIsDisplayed(licensePaymentGSTTotalTooltip);
            AssertTrueIsDisplayed(licencePaymentColumnExpectedTotal);
            AssertTrueIsDisplayed(licensePaymentExpectedTotalTooltip);
            AssertTrueIsDisplayed(licensePaymentColumnExpectedTerm);
            AssertTrueIsDisplayed(licensePaymentExpectedTermTooltip);
            AssertTrueIsDisplayed(licencePaymentColumnActualTotal);
            AssertTrueIsDisplayed(licensePaymentActualTotalTooltip);
            AssertTrueIsDisplayed(licensePaymentColumnExercised);
            AssertTrueIsDisplayed(licencePaymentColumnActions);

            AssertTrueIsDisplayed(licencePaymentsNoRows);
        }

        public void VerifyInsertedPeriodTable(Period period)
        {
            Wait();
            WaitUntilVisible(By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalPeriodsInLease +"]/div[@class='tr']/div[@class='td expander svg-btn']/following-sibling::div[2]"));

            AssertTrueElementContains(By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalPeriodsInLease +"]/div[@class='tr']/div[@class='td expander svg-btn']/following-sibling::div[2]"), ConcatenateDates(period.PeriodStartDate, period.PeriodEndDate));
            AssertTrueContentEquals(By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalPeriodsInLease +"]/div[@class='tr']/div[@class='td expander svg-btn']/following-sibling::div[3]"), period.PeriodBasePaymentFrequency);
            AssertTrueContentEquals(By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalPeriodsInLease +"]/div[@class='tr']/div[@class='td expander svg-btn']/following-sibling::div[4]"), period.PeriodPaymentsDue);
            AssertTrueContentEquals(By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalPeriodsInLease +"]/div[@class='tr']/div[@class='td expander svg-btn']/following-sibling::div[5]"), TransformCurrencyFormat(period.PeriodBaseAgreedPayment));
            AssertTrueContentEquals(By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalPeriodsInLease +"]/div[@class='tr']/div[@class='td expander svg-btn']/following-sibling::div[6]"), CalculateGSTDisplay(period.PeriodBaseIsGSTEligible));

            if (period.PeriodBaseIsGSTEligible == "true")
                AssertTrueContentEquals(By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='tbody']/div[@class='tr-wrapper'][" + totalPeriodsInLease + "]/div[@class='tr']/div[@class='td expander svg-btn']/following-sibling::div[7]"), CalculateGST(period.PeriodBaseAgreedPayment, period.PeriodBaseIsGSTEligible));
            else
                AssertTrueContentEquals(By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='tbody']/div[@class='tr-wrapper'][" + totalPeriodsInLease + "]/div[@class='tr']/div[@class='td expander svg-btn']/following-sibling::div[7]"), "-");

            AssertTrueContentEquals(By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='tbody']/div[@class='tr-wrapper'][" + totalPeriodsInLease + "]/div[@class='tr']/div[@class='td expander svg-btn']/following-sibling::div[8]"), TransformCurrencyFormat(period.PeriodBaseTotalPaymentAmount));
            
            AssertTrueContentEquals(By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalPeriodsInLease +"]/div[@class='tr']/div[@class='td expander svg-btn']/following-sibling::div[9]"), CalculateExpectedPeriod(period.PeriodBasePaymentFrequency, period.PeriodBaseIsGSTEligible, period.PeriodBaseAgreedPayment, period.PeriodStartDate, period.PeriodEndDate));
            AssertTrueContentEquals(By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalPeriodsInLease +"]/div[@class='tr']/div[@class='td expander svg-btn']/following-sibling::div[10]"), DisplayActualTotal(period.PeriodStatus));
            AssertTrueContentEquals(By.XPath("//div[@data-testid='leasePaymentsTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ totalPeriodsInLease +"]/div[@class='tr']/div[@class='td expander svg-btn']/following-sibling::div[11]"), DisplayTerm(period.PeriodStatus));

            if (period.PeriodPaymentType == "Variable")
            {
                //Category Header
                AssertTrueIsDisplayed(licenceCategoryPaymentsCategoryColumn);
                AssertTrueIsDisplayed(licenceCategoryPaymentsPaymentFrequencyColumn);
                AssertTrueIsDisplayed(licenceCategoryPaymentsExpectedPaymentColumn);
                AssertTrueIsDisplayed(licenceCategoryPaymentsIsGSTColumn);
                AssertTrueIsDisplayed(licenceCategoryPaymentsGSTColumn);
                AssertTrueIsDisplayed(licenceCategoryPaymentsExpectedPeriodColumn);
                AssertTrueIsDisplayed(licenceCategoryPaymentsActualTotalColumn);

                //Base Rent Row
                AssertTrueIsDisplayed(licenseCategoryBaseRentRow);
                AssertTrueIsDisplayed(licenseCategoryBaseRentTooltip);

                AssertTrueContentEquals(licenseCategoryBaseFrequencyTableContent, period.PeriodBasePaymentFrequency);

                if(period.PeriodBaseAgreedPayment == "")
                    AssertTrueContentEquals(licenseCategoryBaseExpectedPaymentTableContent, "$0.00");
                else
                    AssertTrueContentEquals(licenseCategoryBaseExpectedPaymentTableContent, TransformCurrencyFormat(period.PeriodBaseAgreedPayment));

                AssertTrueContentEquals(licenseCategoryBaseIsGSTTableContent, CalculateGSTDisplay(period.PeriodBaseIsGSTEligible));
                //if(period.PeriodBaseIsGSTEligible == "true")
                //    AssertTrueContentEquals(licenseCategoryBaseGSTTotalTableContent, TransformCurrencyFormat(period.PeriodBaseGSTAmount));
                //else
                //    AssertTrueContentEquals(licenseCategoryBaseGSTTotalTableContent, "$0.00");

                AssertTrueContentEquals(licenseCategoryBaseExpectedTotalTableContent, TransformCurrencyFormat(period.PeriodBaseTotalPaymentAmount));
                //AssertTrueContentEquals(licenseCategoryBaseActualTotalTableContent, "$0.00");

                //Additional Rent Row
                AssertTrueIsDisplayed(licenseCategoryAdditionalRentRow);
                AssertTrueIsDisplayed(licenseCategoryAdditionalRentTooltip);

                AssertTrueContentEquals(licenseCategoryAdditionalFrequencyTableContent, period.PeriodAdditionalPaymentFrequency);

                if(period.PeriodAdditionalAgreedPayment == "")
                    AssertTrueContentEquals(licenseCategoryAdditionalExpectedPaymentTableContent, "$0.00");
                else
                    AssertTrueContentEquals(licenseCategoryAdditionalExpectedPaymentTableContent, TransformCurrencyFormat(period.PeriodAdditionalAgreedPayment));

                AssertTrueContentEquals(licenseCategoryAdditionalIsGSTTableContent, CalculateGSTDisplay(period.PeriodAdditionalIsGSTEligible));
                //if (period.PeriodAdditionalIsGSTEligible == "true")
                //    AssertTrueContentEquals(licenseCategoryAdditionalGSTTotalTableContent, TransformCurrencyFormat(period.PeriodAdditionalGSTAmount));
                //else
                //    AssertTrueContentEquals(licenseCategoryAdditionalGSTTotalTableContent, "$0.00");
                
                //AssertTrueContentEquals(licenseCategoryAdditionalExpectedTotalTableContent, TransformCurrencyFormat(period.PeriodAdditionalTotalPaymentAmount));
                //AssertTrueContentEquals(licenseCategoryAdditionalActualTotalTableContent, "$0.00");

                //Variable Rent Row
                AssertTrueIsDisplayed(licenseCategoryVariableRentRow);
                AssertTrueIsDisplayed(licenseCategoryVariableRentTooltip);

                AssertTrueContentEquals(licenseCategoryVariableFrequencyTableContent, period.PeriodVariablePaymentFrequency);

                if(period.PeriodVariableAgreedPayment == "")
                    AssertTrueContentEquals(licenseCategoryVariableExpectedPaymentTableContent, "$0.00");
                else
                    AssertTrueContentEquals(licenseCategoryVariableExpectedPaymentTableContent, TransformCurrencyFormat(period.PeriodVariableAgreedPayment));
                AssertTrueContentEquals(licenseCategoryVariableIsGSTTableContent, CalculateGSTDisplay(period.PeriodVariableIsGSTEligible));

                //if (period.PeriodAdditionalIsGSTEligible == "true")
                //    AssertTrueContentEquals(licenseCategoryVariableGSTTotalTableContent, TransformCurrencyFormat(period.PeriodVariableGSTAmount));
                //else
                //    AssertTrueContentEquals(licenseCategoryVariableGSTTotalTableContent, "$0.00");

                //AssertTrueContentEquals(licenseCategoryVariableExpectedTotalTableContent, TransformCurrencyFormat(period.PeriodVariableTotalPaymentAmount));
                //AssertTrueContentEquals(licenseCategoryVariableActualTotalTableContent, "$0.00");
            }
        }

        public void VerifyInsertedPaymentTable(Payment payment, int periodIdx)
        {
            AssertTrueIsDisplayed(licensePaymentsReceivedDateColumn);
            AssertTrueIsDisplayed(licensePaymentsSendDateColumn);
            AssertTrueIsDisplayed(licensePaymentsPaymentMethodColumn);
            AssertTrueIsDisplayed(licensePaymentsReceivedPaymentColumn);
            AssertTrueIsDisplayed(licensePaymentsReceivedPaymentTooltip);
            AssertTrueIsDisplayed(licensePaymentsGSTColumn);
            AssertTrueIsDisplayed(licensePaymentsGSTTooltip);
            AssertTrueIsDisplayed(licensePaymentsTotalColumn);
            AssertTrueIsDisplayed(licensePaymentsTotalTooltip);
            AssertTrueIsDisplayed(licensePaymentsPaymentStatusColumn);
            AssertTrueIsDisplayed(licensePaymentsPaymentStatusTooltip);
            AssertTrueIsDisplayed(licensePaymentsNotesColumn);
            AssertTrueIsDisplayed(licensePaymentsActionsColumn);

            WaitUntilVisible(By.XPath("//b[contains(text(), 'Period "+ periodIdx +"')]/parent::div/parent::div/parent::div/following-sibling::div/div/div/div[@data-testid='paymentsTable']"));

            AssertTrueContentEquals(By.XPath("//b[contains(text(),'Period "+ periodIdx +"')]/parent::div/parent::div/parent::div/following-sibling::div/div/div/div[@data-testid='paymentsTable']/div[@class='tbody']/div["+ totalPaymentInPeriod +"]/div/div[1]"), TransformDateFormat(payment.PaymentSentDate));
            AssertTrueContentEquals(By.XPath("//b[contains(text(),'Period "+ periodIdx +"')]/parent::div/parent::div/parent::div/following-sibling::div/div/div/div[@data-testid='paymentsTable']/div[@class='tbody']/div["+ totalPaymentInPeriod +"]/div/div[2]"), payment.PaymentCategory);
            AssertTrueContentEquals(By.XPath("//b[contains(text(),'Period "+ periodIdx +"')]/parent::div/parent::div/parent::div/following-sibling::div/div/div/div[@data-testid='paymentsTable']/div[@class='tbody']/div["+ totalPaymentInPeriod +"]/div/div[3]"), payment.PaymentMethod);
            AssertTrueContentEquals(By.XPath("//b[contains(text(),'Period "+ periodIdx +"')]/parent::div/parent::div/parent::div/following-sibling::div/div/div/div[@data-testid='paymentsTable']/div[@class='tbody']/div["+ totalPaymentInPeriod +"]/div/div[4]"), TransformCurrencyFormat(payment.PaymentExpectedPayment));
            //if(payment.PaymentIsGSTApplicable == "true")
            //    AssertTrueContentEquals(By.XPath("//b[contains(text(),'Period "+ periodIdx +"')]/parent::div/parent::div/parent::div/following-sibling::div/div/div/div[@data-testid='paymentsTable']/div[@class='tbody']/div["+ totalPaymentInPeriod +"]/div/div[5]"), TransformCurrencyFormat(payment.PaymentGST));
            //else
            //    AssertTrueContentEquals(By.XPath("//b[contains(text(),'Period "+ periodIdx +"')]/parent::div/parent::div/parent::div/following-sibling::div/div/div/div[@data-testid='paymentsTable']/div[@class='tbody']/div["+ totalPaymentInPeriod +"]/div/div[5]"), "-");

            AssertTrueContentEquals(By.XPath("//b[contains(text(),'Period "+ periodIdx +"')]/parent::div/parent::div/parent::div/following-sibling::div/div/div/div[@data-testid='paymentsTable']/div[@class='tbody']/div["+ totalPaymentInPeriod +"]/div/div[6]"),TransformCurrencyFormat(payment.PaymentTotalReceived));
            AssertTrueContentEquals(By.XPath("//b[contains(text(),'Period "+ periodIdx +"')]/parent::div/parent::div/parent::div/following-sibling::div/div/div/div[@data-testid='paymentsTable']/div[@class='tbody']/div["+ totalPaymentInPeriod +"]/div/div[7]"), payment.PaymentStatus);
            AssertTrueIsDisplayed(By.XPath("//b[contains(text(),'Period "+ periodIdx +"')]/parent::div/parent::div/parent::div/following-sibling::div/div/div/div[@data-testid='paymentsTable']/div[@class='tbody']/div["+ totalPaymentInPeriod +"]/div/div[8]/button[@title='notes']"));
            AssertTrueIsDisplayed(By.XPath("//b[contains(text(),'Period "+ periodIdx +"')]/parent::div/parent::div/parent::div/following-sibling::div/div/div/div[@data-testid='paymentsTable']/div[@class='tbody']/div["+ totalPaymentInPeriod +"]/div/div[9]/div/button[@title='edit actual']"));
            AssertTrueIsDisplayed(By.XPath("//b[contains(text(),'Period "+ periodIdx +"')]/parent::div/parent::div/parent::div/following-sibling::div/div/div/div[@data-testid='paymentsTable']/div[@class='tbody']/div["+ totalPaymentInPeriod +"]/div/div[9]/div/button[@title='delete actual']"));
        }

        private static string ConcatenateDates(string startDate, string endDate)
        {
            var startDateFormat = DateTime.Parse(startDate);
            var endDateFormat = DateTime.Parse(endDate);

            return startDateFormat.ToString("MMM d, yyyy") +" - "+ endDateFormat.ToString("MMM d, yyyy");
        }

        private static string CalculateGST(string amount, string gst)
        {
            if (gst == "true")
            {
                decimal value = decimal.Parse(amount) * 0.05m;
                return "$" + value.ToString("#,##0.00");
            }
            else
                return "$0.00";
        }

        private static string DisplayTerm(string termStatus)
        {
            if (termStatus == "Exercised")
                return "Y";
            else
                return "N";
        }

        private static string CalculateExpectedPeriod(string frequency, string gst, string amount, string startDate, string endDate)
        {
            var unitAmount = decimal.Parse(amount);
            var startDateFormat = DateTime.Parse(startDate);
            var endDateFormat = DateTime.Parse(endDate);

            int frequencyNumber;
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

            if (gst == "true")
                unitAmount += decimal.Parse(amount) * 0.05m;

            var finalAmount = frequencyNumber * unitAmount;

            if (finalAmount == 0)
                return "-";
            else
                return "$" + finalAmount.ToString("#,##0.00");
        }

        private static string DisplayActualTotal(string isExercised)
        {
            if (isExercised == "Exercised")
                return "$0.00";
            else
                return "-";
        }
    }
}
