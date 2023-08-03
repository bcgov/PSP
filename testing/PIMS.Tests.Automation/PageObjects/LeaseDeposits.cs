

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
        private By licenseDepositAddBttn = By.XPath("//button/div[contains(text(),'Add a deposit')]/ancestor::button");
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

        private By licenseDepositNotesSubtitle = By.XPath("//span[contains(text(),'Deposit Notes')]");
        private By licenseDepositEditNotesBttn = By.CssSelector("svg[data-testid='edit-notes']");

        //Deposit Add Deposit Elements
        private By licenseDepositAddTypeLabel = By.XPath("//label[contains(text(),'Deposit type')]");
        private By licenseDepositAddTypeSelect = By.Id("input-depositTypeCode");
        private By licenseDepositAddOtherTypeLabel = By.XPath("//label[contains(text(),'Describe other')]"); 
        private By licenseDepositAddOtherTypeInput = By.Id("input-otherTypeDescription");
        private By licenceDepositAddDescriptionLabel = By.XPath("//div[@class='modal-body']/form/div/div/div/label[contains(text(),'Description')]");
        private By licenseDepositAddDescriptionTextarea = By.CssSelector("textarea[id='input-description']");
        private By licenseDepositAddAmountLabel = By.XPath("//label[contains(text(),'Deposit Amount')]");
        private By licenseDepositAddAmountInput = By.Id("input-amountPaid");
        private By licenseDepositAddPaidDateLabel = By.XPath("//label[contains(text(),'Paid date')]");
        private By licenseDepositAddPaidDateInput = By.Id("datepicker-depositDate");
        private By licenseDepositAddDepositHolderLabel = By.XPath("//label[contains(text(),'Deposit Holder')]");
        private By licenseDepositAddDepositHolderInput = By.CssSelector("label[for='input-contactHolder']");
        private By licenseDepositAddContactButton = By.CssSelector("div[class='pl-0 col-auto'] button");

        //Deposit Add Return Elements
        private By licenseDepositReturnDepositTypeLabel = By.XPath("//strong[contains(text(),'Deposit type')]");
        private By licenseDepositReturnDepositTypeContent = By.XPath("//strong[contains(text(),'Deposit type')]/parent::div/following-sibling::div");
        private By licenseDepositReturnDepositAmountLabel = By.XPath("//strong[contains(text(),'Deposit amount')]");
        private By licenseDepositReturnDepositAmountContent = By.XPath("//strong[contains(text(),'Deposit amount')]/parent::div/following-sibling::div");
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
        private By licenseDepositTableTotal = By.XPath("//div[contains(text(),'Deposits Received')]/parent::div/parent::h2/parent::div/div/div[@data-testid='securityDepositsTable']/div[@class='tbody']/div[@class='tr-wrapper']");

        private By licenseDepositTable1stRowDepositTypeContent = By.CssSelector("div[data-testid='securityDepositsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td']:nth-child(1)");
        private By licenseDepositTable1stRowDescriptionContent = By.CssSelector("div[data-testid='securityDepositsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td']:nth-child(2)");
        private By licenseDepositTable1stRowAmountPaidContent = By.CssSelector("div[data-testid='securityDepositsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td']:nth-child(3)");
        private By licenseDepositTable1stRowPaidDateContent = By.CssSelector("div[data-testid='securityDepositsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td']:nth-child(4)");
        private By licenseDepositTable1stRowDepositHolderContent = By.CssSelector("div[data-testid='securityDepositsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td']:nth-child(5)");
        private By licenseDepositTable1stRowEditBttn = By.CssSelector("div[data-testid='securityDepositsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td']:nth-child(6) button[title='edit deposit']");
        private By licenseDepositTable1stRowReturnBttn = By.CssSelector("div[data-testid='securityDepositsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td']:nth-child(6) button[title='return deposit']");
        private By licenseDepositTable1stRowDeleteBttn = By.CssSelector("div[data-testid='securityDepositsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td']:nth-child(6) button[title='delete deposit']");
        private By licenseDepositTable1stRowTooltipBttn = By.CssSelector("div[data-testid='securityDepositsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td']:nth-child(6) span[class='tooltip-icon']");

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

        //Navigates to Deposit Section
        public void NavigateToDepositSection()
        {
            WaitUntilClickable(licenseDepositsLink);
            webDriver.FindElement(licenseDepositsLink).Click();
        }

        public void AddDepositBttn()
        {
            WaitUntilClickable(licenseDepositAddBttn);
            FocusAndClick(licenseDepositAddBttn);
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

            sharedSelectContact.SelectContact(deposit.DepositHolder);

            ButtonElement("Save");
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

            sharedSelectContact.SelectContact(deposit.ReturnPayeeName);

            ButtonElement("Save");
        }

        public void AddNotes(string notes)
        {
            WaitUntilClickable(licenseDepositEditNotesBttn);
            webDriver.FindElement(licenseDepositEditNotesBttn).Click();

            WaitUntilVisible(licenseDepositNotesTextarea);
            webDriver.FindElement(licenseDepositNotesTextarea).SendKeys(notes);
        }

        public void DeleteFirstReturn()
        {
            WaitUntilClickable(licenseDepositReturn1stRowDeleteBttn);
            FocusAndClick(licenseDepositReturn1stRowDeleteBttn);

            WaitUntilVisible(licenseDepositModal);
            Assert.True(sharedModals.ModalHeader() == "Delete Deposit Return");
            Assert.True(sharedModals.ModalContent() == "Are you sure you want to remove this deposit return?");
            sharedModals.ModalClickOKBttn();
        }

        public void EditLastDeposit(Deposit deposit)
        {
            WaitUntilVisible(licenseDepositTableTotal);
            var totalDeposits = webDriver.FindElements(licenseDepositTableTotal).Count;
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
            sharedSelectContact.SelectContact(deposit.DepositHolder);

            ButtonElement("Save");
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
            WaitUntilVisible(licenseDepositTypeColumn);

            Assert.True(webDriver.FindElement(licenseDepositsReceivedSubtitle).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositAddBttn).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositTypeColumn).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositDescriptionColumn).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositAmountPaidColumn).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositPaidDateColumn).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositDepositHolderColumn).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositActionsColumn).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositNoDepositData).Displayed);

            Assert.True(webDriver.FindElement(licenseDepositsReturnSubtitle).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositReturnTypeColumn).Displayed);
            Assert.True(webDriver.FindElement(licenseReturnTerminationColumn).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositAmountColumn).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositClaimsAgainstColumn).Displayed);

            Assert.True(webDriver.FindElement(licenseDepositReturnAmountColumn).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositInterestPaidColumn).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositReturnDateColumn).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositPayeeColumn).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositReturnActionsColumn).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositNoReturnsData).Displayed);

            Assert.True(webDriver.FindElement(licenseDepositNotesSubtitle).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositEditNotesBttn).Displayed);   
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
            Assert.True(webDriver.FindElement(licenceDepositAddDescriptionLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositAddDescriptionTextarea).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositAddAmountLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositAddAmountInput).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositAddPaidDateLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositAddPaidDateInput).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositAddDepositHolderLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositAddDepositHolderInput).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositAddContactButton).Displayed);
            
            sharedModals.VerifyButtonsPresence();
        }

        public void VerifyCreatedDepositTable(Deposit deposit)
        {
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

            WaitUntilVisible(licenseDepositTableLastRowDescriptionContent);
            if (deposit.DepositType == "Other deposit")
                Assert.True(webDriver.FindElement(licenseDepositTableLastRowDepositTypeContent).Text == deposit.DepositTypeOther + " (Other)");
            else
                Assert.True(webDriver.FindElement(licenseDepositTableLastRowDepositTypeContent).Text == deposit.DepositType);
           
            Assert.True(webDriver.FindElement(licenseDepositTableLastRowDescriptionContent).Text == deposit.DepositDescription);
            Assert.True(webDriver.FindElement(licenseDepositTableLastRowAmountPaidContent).Text == TransformCurrencyFormat(deposit.DepositAmount));
            Assert.True(webDriver.FindElement(licenseDepositTableLastRowPaidDateContent).Text == TransformDateFormat(deposit.DepositPaidDate));
            Assert.True(webDriver.FindElement(licenseDepositTableLastRowDepositHolderContent).Text == deposit.DepositHolder);
            Assert.True(webDriver.FindElement(licenseDepositTableLastRowEditBttn).Displayed);
            if(webDriver.FindElements(licenseDepositTableLastRowDeleteBttn).Count == 0)
                Assert.True(webDriver.FindElement(licenseDepositTableLastRowTooltipBttn).Displayed);
            else
                Assert.True(webDriver.FindElement(licenseDepositTableLastRowDeleteBttn).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositTableLastRowReturnBttn).Displayed);
        }

        public void VerifyCreateReturnForm(Deposit deposit)
        {
            WaitUntilVisible(licenseDepositReturnDepositTypeLabel);
            Assert.True(sharedModals.ModalHeader() == "Return a Deposit");

            Assert.True(webDriver.FindElement(licenseDepositReturnDepositTypeLabel).Displayed);

            if (deposit.DepositType == "Other deposit")
                Assert.True(webDriver.FindElement(licenseDepositReturnDepositTypeContent).Text == "Other - " + deposit.DepositTypeOther);
            else
                Assert.True(webDriver.FindElement(licenseDepositReturnDepositTypeContent).Text == deposit.DepositType);
          
            Assert.True(webDriver.FindElement(licenseDepositReturnDepositAmountLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositReturnDepositAmountContent).Text == TransformCurrencyFormat(deposit.DepositAmount));
            Assert.True(webDriver.FindElement(licenseDepositReturnTerminationDateLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositReturnTerminationDateInput).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositReturnClaimLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositReturnClaimInput).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositReturnAmountLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositReturnAmountInput).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositReturnInterestPaidLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositReturnInterestPaidInput).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositReturnDateLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositReturnDateInput).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositReturnPayeeNameLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositAddContactButton).Displayed);

            sharedModals.VerifyButtonsPresence();
        }

        public void VerifyCreatedReturnTable(Deposit deposit)
        {
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

            WaitUntilVisible(licenseDepositReturnTableLastRowTerminationContent);

            if (deposit.DepositType == "Other deposit")
                Assert.True(webDriver.FindElement(licenseDepositReturnTableLastRowDepositTypeContent).Text == deposit.DepositTypeOther + " (Other)");
            else
                Assert.True(webDriver.FindElement(licenseDepositReturnTableLastRowDepositTypeContent).Text == deposit.DepositType);

            Assert.True(webDriver.FindElement(licenseDepositReturnTableLastRowTerminationContent).Text == TransformDateFormat(deposit.ReturnTerminationDate));
            Assert.True(webDriver.FindElement(licenseDepositReturnTableLastRowAmountContent).Text == TransformCurrencyFormat(deposit.DepositAmount));
            Assert.True(webDriver.FindElement(licenseDepositReturnTableLastRowClaimContent).Text == TransformCurrencyFormat(deposit.TerminationClaimDeposit));
            Assert.True(webDriver.FindElement(licenseDepositReturnTableLastRowReturnedAmountContent).Text == TransformCurrencyFormat(deposit.ReturnedAmount));
            Assert.True(webDriver.FindElement(licenseDepositReturnTableLastRowInterestPaidContent).Text == TransformCurrencyFormat(deposit.ReturnInterestPaid));
            Assert.True(webDriver.FindElement(licenseDepositReturnTableLastRowReturnDateContent).Text == TransformDateFormat(deposit.ReturnedDate));
            Assert.True(webDriver.FindElement(licenseDepositReturnTableLastRowReturnedPayeeNameContent).Text == deposit.ReturnPayeeName);
            Assert.True(webDriver.FindElement(licenseDepositReturnTableLastRowDeleteBttn).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositReturnTableLastRowEditBttn).Displayed);
        }

        public int TotalDeposits()
        {
            WaitUntilVisible(licenseDepositTableTotal);
            return webDriver.FindElements(licenseDepositTableTotal).Count;
        }
    }
}
