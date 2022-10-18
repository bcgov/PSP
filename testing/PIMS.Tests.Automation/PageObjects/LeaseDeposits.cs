

using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class LeaseDeposits : PageObjectBase
    {
        private By licenseDepositsLink = By.XPath("//a[contains(text(),'Deposit')]");


        private By licenseDepositModal = By.CssSelector("div[class='modal-content']");
        private By licenseDepositTypeSelect = By.Id("input-depositTypeCode");
        private By licenseDepositOtherTypeInput = By.Id("input-otherTypeDescription");
        private By licenseDepositDescriptionTextarea = By.Id("input-description");
        private By licenseDepositAmountInput = By.Id("input-amountPaid");
        private By licenseDepositPaidDateInput = By.Id("datepicker-depositDate");
        private By licenseDepositDepositHolderLabel = By.CssSelector("label[for='input-contactHolder']");
        private By licenseDepositContactButton = By.CssSelector("div[class='pl-0 col-auto'] button");

        private By licenseDepositContactSearchInput = By.Id("input-summary");
        private By licenseDepositContactSearchButton = By.Id("search-button");
        private By licenseDepositContactSearch1stResultRadioBttn = By.CssSelector("div[data-testid='contactsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div:nth-child(1) input");

        private By licenseDespositTable1stRow = By.CssSelector("div[data-testid='securityDepositsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1)");
        private By licenseDepositTable1stRowReturnBttn = By.Id("return-deposit-0");
        private By licenseDepositTable1stRowDeleteBttn = By.Id("delete-deposit-0");

        private By licenseDepositReturnTerminationDateInput = By.Id("datepicker-terminationDate");
        private By licenseDepositReturnClaimInput = By.Id("input-claimsAgainst");
        private By licenseDepositReturnAmountInput = By.Id("input-returnAmount");
        private By licenseDepositInterestPaidInput = By.Id("input-interestPaid");
        private By licenseDepositReturnDateInput = By.Id("datepicker-returnDate");
        private By licenseDepositContactReturn1stRow = By.CssSelector("div[data-testid='securityDepositReturnsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1)");

        private By licenseDepositEditNotesBttn = By.CssSelector("svg[data-testid='edit-notes']");
        private By licenseDepositNotesTextarea = By.Id("input-returnNotes");
        private By licenseDepositSaveButton = By.XPath("//button/div[contains(text(),'Save')]/ancestor::button");

        public LeaseDeposits(IWebDriver webDriver) : base(webDriver)
        {}

        //Navigates to Deposit Section
        public void NavigateToDepositSection()
        {
            Wait();
            webDriver.FindElement(licenseDepositsLink).Click();
        }

        public void AddDeposit(string description, string amount, string paidDate, string depositHolder)
        {
            ButtonElement("Add a deposit");

            WaitUntil(licenseDepositModal);
            ChooseRandomSelectOption(licenseDepositTypeSelect, "input-depositTypeCode", 2);

            Wait();
            if (webDriver.FindElements(licenseDepositOtherTypeInput).Count() > 0)
            {
                webDriver.FindElement(licenseDepositOtherTypeInput).SendKeys("Automation Test - Other Type Deposit");
            }

            webDriver.FindElement(licenseDepositDescriptionTextarea).SendKeys(description);
            webDriver.FindElement(licenseDepositAmountInput).SendKeys(amount);
            webDriver.FindElement(licenseDepositPaidDateInput).SendKeys(paidDate);
            webDriver.FindElement(licenseDepositDepositHolderLabel).Click();

            webDriver.FindElement(licenseDepositContactButton).Click();

            WaitUntil(licenseDepositContactSearchInput);

            webDriver.FindElement(licenseDepositContactSearchInput).SendKeys(depositHolder);
            webDriver.FindElement(licenseDepositContactSearchButton).Click();

            WaitUntil(licenseDepositContactSearch1stResultRadioBttn);

            webDriver.FindElement(licenseDepositContactSearch1stResultRadioBttn).Click();
            ButtonElement("Select");

            ButtonElement("Save");

            Wait();
            Assert.True(webDriver.FindElement(licenseDespositTable1stRow).Displayed);
        }

        public void AddReturn(string terminationDate, string claimsDeposit, string returnedAmount, string interestPaid, string returnedDate, string payeeName)
        {
            WaitUntil(licenseDespositTable1stRow);

            webDriver.FindElement(licenseDepositTable1stRowReturnBttn).Click();

            WaitUntil(licenseDepositModal);

            webDriver.FindElement(licenseDepositReturnTerminationDateInput).SendKeys(terminationDate);
            webDriver.FindElement(licenseDepositReturnClaimInput).SendKeys(claimsDeposit);
            webDriver.FindElement(licenseDepositReturnAmountInput).SendKeys(returnedAmount);
            webDriver.FindElement(licenseDepositInterestPaidInput).SendKeys(interestPaid);
            webDriver.FindElement(licenseDepositReturnDateInput).SendKeys(returnedDate);

            webDriver.FindElement(licenseDepositContactButton).Click();

            WaitUntil(licenseDepositContactSearchInput);

            webDriver.FindElement(licenseDepositContactSearchInput).SendKeys(payeeName);
            webDriver.FindElement(licenseDepositContactSearchButton).Click();

            WaitUntil(licenseDepositContactSearch1stResultRadioBttn);

            webDriver.FindElement(licenseDepositContactSearch1stResultRadioBttn).Click();
            ButtonElement("Select");

            ButtonElement("Save");

            Wait();
            Assert.True(webDriver.FindElement(licenseDepositContactReturn1stRow).Displayed);
        }

        public void AddNotes(string notes)
        {
            WaitUntil(licenseDepositEditNotesBttn);
            webDriver.FindElement(licenseDepositEditNotesBttn).Click();

            WaitUntil(licenseDepositNotesTextarea);
            webDriver.FindElement(licenseDepositNotesTextarea).SendKeys(notes);

            var saveButton = webDriver.FindElement(licenseDepositSaveButton);
            saveButton.Enabled.Should().BeTrue();

            Wait();
            saveButton.Click();

        }
    }
}
