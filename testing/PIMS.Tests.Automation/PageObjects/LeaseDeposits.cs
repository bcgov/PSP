

using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class LeaseDeposits : PageObjectBase
    {
        //Deposit Navigation Elements
        private By licenseDepositsLink = By.XPath("//a[contains(text(),'Deposit')]");

        //Deposit Modal Elements
        private By licenseDepositModal = By.CssSelector("div[class='modal-content']");

        //Deposit Initial Form Elements
        private By licenseDepositsReceivedSubtitle = By.XPath("//div[contains(text(),'Deposits Received')]");
        private By licenseDepositAddBttn = By.XPath("//div[contains(text(),'Deposits Received')]/following-sibling::div/button");
        private By licenseDepositTypeColumn = By.XPath("//div[@data-testid='securityDepositsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Deposit type')]");
        private By licenseDepositDescriptionColumn = By.XPath("//div[@data-testid='securityDepositsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Description')]");
        private By licenseDepositAmountPaidColumn = By.XPath("//div[@data-testid='securityDepositsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Amount paid')]");
        private By licenseDepositPaidDateColumn = By.XPath("//div[@data-testid='securityDepositsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Paid date')]");
        private By licenseDepositDepositHolderColumn = By.XPath("//div[@data-testid='securityDepositsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Deposit holder')]");
        private By licenseDepositActionsColumn = By.XPath("//div[@data-testid='securityDepositsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Actions')]");
        private By licenseDepositNoDepositData = By.XPath("//div[@data-testid='securityDepositsTable']/div[contains(text(),'There is no corresponding data')]");

        private By licenseDepositsReturnSubtitle = By.XPath("//div[contains(text(),'Deposits Returned')]");
        private By licenseDepositReturnTypeColumn = By.XPath("//div[@data-testid='securityDepositReturnsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Deposit type')]");
        private By licenseReturnTerminationColumn = By.XPath("//div[@data-testid='securityDepositReturnsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Termination or Surrender date')]");
        private By licenseDepositAmountColumn = By.XPath("//div[@data-testid='securityDepositReturnsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Deposit amount')]");
        private By licenseDepositClaimsAgainstColumn = By.XPath("//div[@data-testid='securityDepositReturnsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Claims against deposit')]");
        private By licenseDepositReturnAmountColumn = By.XPath("//div[@data-testid='securityDepositReturnsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Returned amount (without interest)')]");
        private By licenseDepositInterestPaidColumn = By.XPath("//div[@data-testid='securityDepositReturnsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Interest paid')]");
        private By licenseDepositReturnDateColumn = By.XPath("//div[@data-testid='securityDepositReturnsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Return date')]");
        private By licenseDepositPayeeColumn = By.XPath("//div[@data-testid='securityDepositReturnsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Payee name')]");
        private By licenseDepositReturnActionsColumn = By.XPath("//div[@data-testid='securityDepositReturnsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Actions')]");
        private By licenseDepositNoReturnsData = By.XPath("//div[@data-testid='securityDepositReturnsTable']/div[contains(text(),'There is no corresponding data')]");

        private By licenseDepositNotesSubtitle = By.XPath("//div[contains(text(),'Deposit Comments')]");
        private By licenseDepositEditNotesBttn = By.CssSelector("button[data-testid='edit-comments']");

        //Deposit Add Deposit Elements
        private By licenseDepositAddTypeLabel = By.XPath("//label[contains(text(),'Deposit type')]");
        private By licenseDepositAddTypeSelect = By.Id("input-depositTypeCode");
        private By licenseDepositAddOtherTypeLabel = By.XPath("//label[contains(text(),'Describe other')]"); 
        private By licenseDepositAddOtherTypeInput = By.Id("input-otherTypeDescription");
        private By licenceDepositAddDescriptionLabel = By.XPath("//div[@class='modal-body']/form/div/div/label[contains(text(),'Description')]");
        private By licenseDepositAddDescriptionTextarea = By.CssSelector("textarea[id='input-description']");
        private By licenseDepositAddAmountLabel = By.XPath("//label[contains(text(),'Deposit amount')]");
        private By licenseDepositAddAmountInput = By.Id("input-amountPaid");
        private By licenseDepositAddPaidDateLabel = By.XPath("//label[contains(text(),'Paid date')]");
        private By licenseDepositAddPaidDateInput = By.Id("datepicker-depositDate");
        private By licenseDepositAddDepositHolderLabel = By.XPath("//label[contains(text(),'Deposit holder')]");
        private By licenseDepositAddDepositHolderInput = By.XPath("//input[@id='input-contactHolder.id']/parent::div/parent::div");
        private By licenseDepositAddContactButton = By.CssSelector("div[class='pl-0 col-auto'] button");

        //Deposit Add Return Elements
        private By licenseDepositReturnDepositTypeLabel = By.XPath("//label[contains(text(),'Deposit type')]");
        private By licenseDepositReturnDepositTypeContent = By.XPath("//label[contains(text(),'Deposit type')]/parent::div/following-sibling::div");
        private By licenseDepositReturnDepositAmountLabel = By.XPath("//label[contains(text(),'Deposit amount')]");
        private By licenseDepositReturnDepositAmountContent = By.XPath("//label[contains(text(),'Deposit amount')]/parent::div/following-sibling::div");
        private By licenseDepositReturnTerminationDateLabel = By.XPath("//label[contains(text(),'Termination or surrender date')]");
        private By licenseDepositReturnTerminationDateInput = By.Id("datepicker-terminationDate");
        private By licenseDepositReturnClaimLabel = By.XPath("//label[contains(text(),'Claims against deposit ($)')]");
        private By licenseDepositReturnClaimInput = By.Id("input-claimsAgainst");
        private By licenseDepositReturnAmountLabel = By.XPath("//label[contains(text(),'Returned amount ($) without interest')]");
        private By licenseDepositReturnAmountInput = By.Id("input-returnAmount");
        private By licenseDepositReturnInterestPaidLabel = By.XPath("//label[contains(text(),'Interest paid ($)')]");
        private By licenseDepositReturnInterestPaidInput = By.Id("input-interestPaid");
        private By licenseDepositReturnDateLabel = By.XPath("//label[contains(text(),'Returned date')]");
        private By licenseDepositReturnDateInput = By.Id("datepicker-returnDate");
        private By licenseDepositReturnPayeeNameLabel = By.XPath("//label[contains(text(),'Payee name')]");
        private By licenseDepositRerturnPayeeNameInput = By.Id("input-contactHolder.id");

        //Deposit Table Results Elements
        private By licenseDepositTableTotal = By.XPath("//div[@data-testid='securityDepositsTable']/div[@class='tbody']/div[@class='tr-wrapper']");

        //Return Table Results Elements
        private By licenseDepositReturnTableTotal = By.CssSelector("div[data-testid='securityDepositReturnsTable'] div[class='tbody'] div[class='tr-wrapper']");
        private By licenseDepositReturn1stRowDeleteBttn = By.CssSelector("div[data-testid='securityDepositReturnsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td']:nth-child(9) button[title='delete deposit return']");

        //Deposit Notes Elements
        private By licenseDepositNotesTextarea = By.Id("input-returnNotes");

        SharedSelectContact sharedSelectContact;
        SharedModals sharedModals;

        public LeaseDeposits(IWebDriver webDriver) : base(webDriver)
        {
            sharedSelectContact = new SharedSelectContact(webDriver);
            sharedModals = new SharedModals(webDriver);
        }

        public void NavigateToDepositSection()
        {
            Wait();
            webDriver.FindElement(licenseDepositsLink).Click();
        }

        public void AddDepositBttn()
        {
            WaitUntilClickable(licenseDepositAddBttn);
            FocusAndClick(licenseDepositAddBttn);
        }

        public void CancelDeposit()
        {
            sharedModals.ModalClickCancelBttn();
        }

        public void AddDeposit(Deposit deposit)
        {
            WaitUntilClickable(licenseDepositAddTypeSelect);
            ChooseSpecificSelectOption(licenseDepositAddTypeSelect, deposit.DepositType);

            WaitUntilVisible(licenseDepositAddDescriptionTextarea);
            if (webDriver.FindElements(licenseDepositAddOtherTypeInput).Count() > 0)
            {
                webDriver.FindElement(licenseDepositAddOtherTypeInput).SendKeys(deposit.DepositTypeOther);
            }

            webDriver.FindElement(licenseDepositAddDescriptionTextarea).SendKeys(deposit.DepositDescription);
            webDriver.FindElement(licenseDepositAddAmountInput).SendKeys(deposit.DepositAmount);
            webDriver.FindElement(licenseDepositAddPaidDateInput).SendKeys(deposit.DepositPaidDate);
            webDriver.FindElement(licenseDepositAddDepositHolderLabel).Click();

            webDriver.FindElement(licenseDepositAddContactButton).Click();

            sharedSelectContact.SelectContact(deposit.DepositHolder, "");

            sharedModals.ModalClickOKBttn();
        }

        public void AddReturnBttn()
        {
            WaitUntilVisible(licenseDepositTableTotal);
            var totalDeposits = webDriver.FindElements(licenseDepositTableTotal).Count;
            var licenseDepositTableLastRowReturnBttn = By.CssSelector("div[data-testid='securityDepositsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ totalDeposits +") div[class='td']:nth-child(6) button[title='return deposit']");

            webDriver.FindElement(licenseDepositTableLastRowReturnBttn).Click();
        }

        public void AddReturn(Deposit deposit)
        {
            WaitUntilVisible(licenseDepositReturnTerminationDateInput);

            webDriver.FindElement(licenseDepositReturnTerminationDateInput).SendKeys(deposit.ReturnTerminationDate);
            webDriver.FindElement(licenseDepositReturnClaimInput).SendKeys(deposit.TerminationClaimDeposit);
            webDriver.FindElement(licenseDepositReturnAmountInput).SendKeys(deposit.ReturnedAmount);
            webDriver.FindElement(licenseDepositReturnInterestPaidInput).SendKeys(deposit.ReturnInterestPaid);
            webDriver.FindElement(licenseDepositReturnDateInput).SendKeys(deposit.ReturnedDate);

            webDriver.FindElement(licenseDepositAddContactButton).Click();

            sharedSelectContact.SelectContact(deposit.ReturnPayeeName, "");

            ButtonElement("Save");
        }

        public void AddNotes(string notes)
        {
            Wait();
            FocusAndClick(licenseDepositEditNotesBttn);

            WaitUntilVisible(licenseDepositNotesTextarea);
            webDriver.FindElement(licenseDepositNotesTextarea).SendKeys(notes);
        }

        public void DeleteFirstReturn()
        {
            Wait();
            FocusAndClick(licenseDepositReturn1stRowDeleteBttn);

            WaitUntilVisible(licenseDepositModal);
            Assert.True(sharedModals.ModalHeader() == "Delete Deposit Return");
            Assert.True(sharedModals.ModalContent() == "Are you sure you want to remove this deposit return?");
            sharedModals.ModalClickOKBttn();
        }

        public void EditLastDeposit(Deposit deposit)
        {
            Wait();
            var totalDeposits = webDriver.FindElements(licenseDepositTableTotal).Count;

            Wait();
            webDriver.FindElement(By.CssSelector("div[data-testid='securityDepositsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ totalDeposits +") button[title='edit deposit']")).Click();

            ChooseSpecificSelectOption(licenseDepositAddTypeSelect, deposit.DepositType);

            WaitUntilVisible(licenseDepositAddDescriptionTextarea);
            if (webDriver.FindElements(licenseDepositAddOtherTypeInput).Count() > 0)
            {
                ClearInput(licenseDepositAddOtherTypeInput);
                webDriver.FindElement(licenseDepositAddOtherTypeInput).SendKeys(deposit.DepositTypeOther);
            }

            ClearInput(licenseDepositAddDescriptionTextarea);
            webDriver.FindElement(licenseDepositAddDescriptionTextarea).SendKeys(deposit.DepositDescription);

            ClearInput(licenseDepositAddAmountInput);
            webDriver.FindElement(licenseDepositAddAmountInput).SendKeys(deposit.DepositAmount);

            ClearInput(licenseDepositAddPaidDateInput);
            webDriver.FindElement(licenseDepositAddPaidDateInput).SendKeys(deposit.DepositPaidDate);
            webDriver.FindElement(licenseDepositAddPaidDateInput).SendKeys(Keys.Enter);

            webDriver.FindElement(licenseDepositAddContactButton).Click();
            sharedSelectContact.SelectContact(deposit.DepositHolder, "");

            sharedModals.ModalClickOKBttn();
        }

        public void DeleteFirstDeposit()
        {
            var lastDepositDeleteIcon = By.CssSelector("div[data-testid='securityDepositsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) button[title='delete deposit']");
            WaitUntilClickable(lastDepositDeleteIcon);
            FocusAndClick(lastDepositDeleteIcon);

            WaitUntilVisible(licenseDepositModal);
            Assert.True(sharedModals.ModalHeader() == "Delete Deposit");
            Assert.True(sharedModals.ModalContent() == "Are you sure you want to remove the deposit?");
            sharedModals.ModalClickOKBttn();
        }

        public void VerifyDepositInitForm()
        {
            AssertTrueIsDisplayed(licenseDepositsReceivedSubtitle);
            AssertTrueIsDisplayed(licenseDepositAddBttn);
            AssertTrueIsDisplayed(licenseDepositTypeColumn);
            AssertTrueIsDisplayed(licenseDepositDescriptionColumn);
            AssertTrueIsDisplayed(licenseDepositAmountPaidColumn);
            AssertTrueIsDisplayed(licenseDepositPaidDateColumn);
            AssertTrueIsDisplayed(licenseDepositDepositHolderColumn);
            AssertTrueIsDisplayed(licenseDepositActionsColumn);
            AssertTrueIsDisplayed(licenseDepositNoDepositData);

            AssertTrueIsDisplayed(licenseDepositsReturnSubtitle);
            AssertTrueIsDisplayed(licenseDepositReturnTypeColumn);
            AssertTrueIsDisplayed(licenseReturnTerminationColumn);
            AssertTrueIsDisplayed(licenseDepositAmountColumn);
            AssertTrueIsDisplayed(licenseDepositClaimsAgainstColumn);

            AssertTrueIsDisplayed(licenseDepositReturnAmountColumn);
            AssertTrueIsDisplayed(licenseDepositInterestPaidColumn);
            AssertTrueIsDisplayed(licenseDepositReturnDateColumn);
            AssertTrueIsDisplayed(licenseDepositPayeeColumn);
            AssertTrueIsDisplayed(licenseDepositReturnActionsColumn);
            AssertTrueIsDisplayed(licenseDepositNoReturnsData);

            AssertTrueIsDisplayed(licenseDepositNotesSubtitle);
            AssertTrueIsDisplayed(licenseDepositEditNotesBttn);   
        }

        public void VerifyCreateDepositForm()
        {
            WaitUntilVisible(licenseDepositAddTypeLabel);

            Assert.True(sharedModals.ModalHeader() == "Add a Deposit");

            Assert.True(webDriver.FindElement(licenseDepositAddTypeLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositAddTypeSelect).Displayed);
            if (webDriver.FindElements(licenseDepositAddOtherTypeLabel).Count > 0)
            {
                Assert.True(webDriver.FindElement(licenseDepositAddOtherTypeInput).Displayed);
            }
            AssertTrueIsDisplayed(licenceDepositAddDescriptionLabel);
            AssertTrueIsDisplayed(licenseDepositAddDescriptionTextarea);
            AssertTrueIsDisplayed(licenseDepositAddAmountLabel);
            AssertTrueIsDisplayed(licenseDepositAddAmountInput);
            AssertTrueIsDisplayed(licenseDepositAddPaidDateLabel);
            AssertTrueIsDisplayed(licenseDepositAddPaidDateInput);
            AssertTrueIsDisplayed(licenseDepositAddDepositHolderLabel);
            AssertTrueIsDisplayed(licenseDepositAddDepositHolderInput);
            AssertTrueIsDisplayed(licenseDepositAddContactButton);
            
            sharedModals.VerifyButtonsPresence();
        }

        public void VerifyCreatedDepositTable(Deposit deposit)
        {
            Wait(5000);
            var totalDeposits = webDriver.FindElements(licenseDepositTableTotal).Count;

            var licenseDepositTableLastRowDepositTypeContent = By.CssSelector("div[data-testid='securityDepositsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ totalDeposits +") div[class='td']:nth-child(1)");
            var licenseDepositTableLastRowDescriptionContent = By.CssSelector("div[data-testid='securityDepositsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ totalDeposits +") div[class='td']:nth-child(2)");
            var licenseDepositTableLastRowAmountPaidContent = By.CssSelector("div[data-testid='securityDepositsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ totalDeposits +") div[class='td']:nth-child(3)");
            var licenseDepositTableLastRowPaidDateContent = By.CssSelector("div[data-testid='securityDepositsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ totalDeposits +") div[class='td']:nth-child(4)");
            var licenseDepositTableLastRowDepositHolderContent = By.CssSelector("div[data-testid='securityDepositsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ totalDeposits +") div[class='td']:nth-child(5)");
            var licenseDepositTableLastRowEditBttn = By.CssSelector("div[data-testid='securityDepositsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ totalDeposits +") div[class='td']:nth-child(6) button[title='edit deposit']");
            var licenseDepositTableLastRowReturnBttn = By.CssSelector("div[data-testid='securityDepositsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ totalDeposits +") div[class='td']:nth-child(6) button[title='return deposit']");
            var licenseDepositTableLastRowDeleteBttn = By.CssSelector("div[data-testid='securityDepositsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ totalDeposits +") div[class='td']:nth-child(6) button[title='delete deposit']");
            var licenseDepositTableLastRowTooltipBttn = By.CssSelector("div[data-testid='securityDepositsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ totalDeposits +") div[class='td']:nth-child(6) span[class='tooltip-icon']");

            if (deposit.DepositType == "Other deposit")
                AssertTrueContentEquals(licenseDepositTableLastRowDepositTypeContent, deposit.DepositTypeOther + " (Other)");
            else
                AssertTrueContentEquals(licenseDepositTableLastRowDepositTypeContent, deposit.DepositType);

            AssertTrueContentEquals(licenseDepositTableLastRowDescriptionContent, deposit.DepositDescription);
            AssertTrueContentEquals(licenseDepositTableLastRowAmountPaidContent, TransformCurrencyFormat(deposit.DepositAmount));
            AssertTrueContentEquals(licenseDepositTableLastRowPaidDateContent, TransformDateFormat(deposit.DepositPaidDate));
            AssertTrueContentEquals(licenseDepositTableLastRowDepositHolderContent, deposit.DepositHolder);
            AssertTrueIsDisplayed(licenseDepositTableLastRowEditBttn);

            if(webDriver.FindElements(licenseDepositTableLastRowDeleteBttn).Count == 0)
                AssertTrueIsDisplayed(licenseDepositTableLastRowTooltipBttn);
            else
                AssertTrueIsDisplayed(licenseDepositTableLastRowDeleteBttn);

            AssertTrueIsDisplayed(licenseDepositTableLastRowReturnBttn);
        }

        public void VerifyCreateReturnForm(Deposit deposit)
        {
            WaitUntilVisible(licenseDepositReturnDepositTypeLabel);
            Assert.True(sharedModals.ModalHeader() == "Return a Deposit");

            Assert.True(webDriver.FindElement(licenseDepositReturnDepositTypeLabel).Displayed);

            if (deposit.DepositType == "Other deposit")
                AssertTrueContentEquals(licenseDepositReturnDepositTypeContent, "Other - " + deposit.DepositTypeOther);
            else
                AssertTrueContentEquals(licenseDepositReturnDepositTypeContent, deposit.DepositType);
          
            AssertTrueIsDisplayed(licenseDepositReturnDepositAmountLabel);
            AssertTrueContentEquals(licenseDepositReturnDepositAmountContent, TransformCurrencyFormat(deposit.DepositAmount));
            AssertTrueIsDisplayed(licenseDepositReturnTerminationDateLabel);
            AssertTrueIsDisplayed(licenseDepositReturnTerminationDateInput);
            AssertTrueIsDisplayed(licenseDepositReturnClaimLabel);
            AssertTrueIsDisplayed(licenseDepositReturnClaimInput);
            AssertTrueIsDisplayed(licenseDepositReturnAmountLabel);
            AssertTrueIsDisplayed(licenseDepositReturnAmountInput);
            AssertTrueIsDisplayed(licenseDepositReturnInterestPaidLabel);
            AssertTrueIsDisplayed(licenseDepositReturnInterestPaidInput);
            AssertTrueIsDisplayed(licenseDepositReturnDateLabel);
            AssertTrueIsDisplayed(licenseDepositReturnDateInput);
            AssertTrueIsDisplayed(licenseDepositReturnPayeeNameLabel);
            AssertTrueIsDisplayed(licenseDepositAddContactButton);

            sharedModals.VerifyButtonsPresence();
        }

        public void VerifyCreatedReturnTable(Deposit deposit)
        {
            Wait(3000);
            var totalReturns = webDriver.FindElements(licenseDepositReturnTableTotal).Count;

            var licenseDepositReturnTableLastRowDepositTypeContent = By.CssSelector("div[data-testid='securityDepositReturnsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ totalReturns + ") div[class='td']:nth-child(1)");
            var licenseDepositReturnTableLastRowTerminationContent = By.CssSelector("div[data-testid='securityDepositReturnsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ totalReturns + ") div[class='td']:nth-child(2)");
            var licenseDepositReturnTableLastRowAmountContent = By.CssSelector("div[data-testid='securityDepositReturnsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ totalReturns + ") div[class='td']:nth-child(3)");
            var licenseDepositReturnTableLastRowClaimContent = By.CssSelector("div[data-testid='securityDepositReturnsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ totalReturns + ") div[class='td']:nth-child(4)");
            var licenseDepositReturnTableLastRowReturnedAmountContent = By.CssSelector("div[data-testid='securityDepositReturnsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ totalReturns + ") div[class='td']:nth-child(5)");
            var licenseDepositReturnTableLastRowInterestPaidContent = By.CssSelector("div[data-testid='securityDepositReturnsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ totalReturns + ") div[class='td']:nth-child(6)");
            var licenseDepositReturnTableLastRowReturnDateContent = By.CssSelector("div[data-testid='securityDepositReturnsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ totalReturns + ") div[class='td']:nth-child(7)");
            var licenseDepositReturnTableLastRowReturnedPayeeNameContent = By.CssSelector("div[data-testid='securityDepositReturnsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ totalReturns + ") div[class='td']:nth-child(8)");
            var licenseDepositReturnTableLastRowDeleteBttn = By.CssSelector("div[data-testid='securityDepositReturnsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ totalReturns + ") div[class='td']:nth-child(9) button[title='delete deposit return']");
            var licenseDepositReturnTableLastRowEditBttn = By.CssSelector("div[data-testid='securityDepositReturnsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ totalReturns + ") div[class='td']:nth-child(9) button[title='edit deposit return']");

            if (deposit.DepositType == "Other deposit")
                AssertTrueContentEquals(licenseDepositReturnTableLastRowDepositTypeContent, deposit.DepositTypeOther + " (Other)");
            else
                AssertTrueContentEquals(licenseDepositReturnTableLastRowDepositTypeContent, deposit.DepositType);

            AssertTrueContentEquals(licenseDepositReturnTableLastRowTerminationContent,TransformDateFormat(deposit.ReturnTerminationDate));
            AssertTrueContentEquals(licenseDepositReturnTableLastRowAmountContent, TransformCurrencyFormat(deposit.DepositAmount));
            AssertTrueContentEquals(licenseDepositReturnTableLastRowClaimContent, TransformCurrencyFormat(deposit.TerminationClaimDeposit));
            AssertTrueContentEquals(licenseDepositReturnTableLastRowReturnedAmountContent, TransformCurrencyFormat(deposit.ReturnedAmount));
            AssertTrueContentEquals(licenseDepositReturnTableLastRowInterestPaidContent, TransformCurrencyFormat(deposit.ReturnInterestPaid));
            AssertTrueContentEquals(licenseDepositReturnTableLastRowReturnDateContent,TransformDateFormat(deposit.ReturnedDate));
            AssertTrueContentEquals(licenseDepositReturnTableLastRowReturnedPayeeNameContent, deposit.ReturnPayeeName);
            AssertTrueIsDisplayed(licenseDepositReturnTableLastRowDeleteBttn);
            AssertTrueIsDisplayed(licenseDepositReturnTableLastRowEditBttn);
        }

        public int TotalDeposits()
        {
            WaitUntilVisible(licenseDepositTableTotal);
            return webDriver.FindElements(licenseDepositTableTotal).Count;
        }
    }
}
