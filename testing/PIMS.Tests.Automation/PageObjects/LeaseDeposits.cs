

using OpenQA.Selenium;
using System;
using System.Globalization;

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
        private By licenseDespositTable1stRow = By.CssSelector("div[data-testid='securityDepositsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1)");
        private By licenseDepositTable1stRowDepositTypeContent = By.CssSelector("div[data-testid='securityDepositsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td']:nth-child(1)");
        private By licenseDepositTable1stRowDescriptionContent = By.CssSelector("div[data-testid='securityDepositsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td']:nth-child(2)");
        private By licenseDepositTable1stRowAmountPaidContent = By.CssSelector("div[data-testid='securityDepositsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td']:nth-child(3)");
        private By licenseDepositTable1stRowPaidDateContent = By.CssSelector("div[data-testid='securityDepositsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td']:nth-child(4)");
        private By licenseDepositTable1stRowDepositHolderContent = By.CssSelector("div[data-testid='securityDepositsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td']:nth-child(5)");
        private By licenseDepositTable1stRowTooltip = By.Id("no-delete-tooltip-325");
        private By licenseDepositTable1stRowEditBttn = By.Id("edit-deposit-0");
        private By licenseDepositTable1stRowReturnBttn = By.Id("return-deposit-0");
        private By licenseDepositTable1stRowDeleteBttn = By.Id("delete-deposit-0");

        private By licenseDepositTableTotal = By.CssSelector("div[data-testid='securityDepositsTable'] div[class='tbody'] div[class='tr-wrapper']");

        //Return Table Results Elements
        private By licenseDepositReturn1stRow = By.CssSelector("div[data-testid='securityDepositReturnsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1)");
        private By licenseDepositReturnTable1stRowDepositTypeContent = By.CssSelector("div[data-testid='securityDepositReturnsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td']:nth-child(1)");
        private By licenseDepositReturnTable1stRowTerminationContent = By.CssSelector("div[data-testid='securityDepositReturnsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td']:nth-child(2)");
        private By licenseDepositReturnTable1stRowAmountContent = By.CssSelector("div[data-testid='securityDepositReturnsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td']:nth-child(3)");
        private By licenseDepositReturnTable1stRowClaimContent = By.CssSelector("div[data-testid='securityDepositReturnsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td']:nth-child(4)");
        private By licenseDepositReturnTable1stRowReturnedAmountContent = By.CssSelector("div[data-testid='securityDepositReturnsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td']:nth-child(5)");
        private By licenseDepositReturnTable1stRowInterestPaidContent = By.CssSelector("div[data-testid='securityDepositReturnsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td']:nth-child(6)");
        private By licenseDepositReturnTable1stRowReturnDateContent = By.CssSelector("div[data-testid='securityDepositReturnsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td']:nth-child(7)");
        private By licenseDepositReturnTable1stRowReturnedPayeeNameContent = By.CssSelector("div[data-testid='securityDepositReturnsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td']:nth-child(8)");
        private By licenseDepositReturnTable1stRowDeleteBttn = By.CssSelector("button[title='delete deposit return']");
        private By licenseDepositReturnTable1stRowEditBttn = By.CssSelector("button[title='edit deposit return']");

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
            Wait();
            webDriver.FindElement(licenseDepositsLink).Click();
        }

        public void AddDepositBttn()
        {
            Wait();
            FocusAndClick(licenseDepositAddBttn);
        }

        public void AddDeposit(string depositType, string description, string amount, string paidDate, string depositHolder)
        {
            Wait();
            ChooseSpecificSelectOption(licenseDepositAddTypeSelect, depositType);

            Wait();
            if (webDriver.FindElements(licenseDepositAddOtherTypeInput).Count() > 0)
            {
                webDriver.FindElement(licenseDepositAddOtherTypeInput).SendKeys("Automation Test - Other Type Deposit");
            }

            webDriver.FindElement(licenseDepositAddDescriptionTextarea).SendKeys(description);
            webDriver.FindElement(licenseDepositAddAmountInput).SendKeys(amount);
            webDriver.FindElement(licenseDepositAddPaidDateInput).SendKeys(paidDate);
            webDriver.FindElement(licenseDepositAddDepositHolderLabel).Click();

            webDriver.FindElement(licenseDepositAddContactButton).Click();

            sharedSelectContact.SelectContact(depositHolder);

            ButtonElement("Save");
        }

        public void AddReturnBttn()
        {
            Wait();
            webDriver.FindElement(licenseDepositTable1stRowReturnBttn).Click();
        }

        public void AddReturn(string terminationDate, string claimsDeposit, string returnedAmount, string interestPaid, string returnedDate, string payeeName)
        {
            Wait();

            webDriver.FindElement(licenseDepositReturnTerminationDateInput).SendKeys(terminationDate);
            webDriver.FindElement(licenseDepositReturnClaimInput).SendKeys(claimsDeposit);
            webDriver.FindElement(licenseDepositReturnAmountInput).SendKeys(returnedAmount);
            webDriver.FindElement(licenseDepositReturnInterestPaidInput).SendKeys(interestPaid);
            webDriver.FindElement(licenseDepositReturnDateInput).SendKeys(returnedDate);

            webDriver.FindElement(licenseDepositAddContactButton).Click();

            sharedSelectContact.SelectContact(payeeName);

            ButtonElement("Save");
        }

        public void AddNotes(string notes)
        {
            Wait();
            webDriver.FindElement(licenseDepositEditNotesBttn).Click();

            WaitUntil(licenseDepositNotesTextarea);
            webDriver.FindElement(licenseDepositNotesTextarea).SendKeys(notes);
        }

        public void DeleteFirstReturn()
        {
            Wait();
            FocusAndClick(licenseDepositReturnTable1stRowDeleteBttn);

            WaitUntil(licenseDepositModal);
            Assert.True(sharedModals.ModalHeader() == "Delete Deposit Return");
            Assert.True(sharedModals.ModalContent() == "Are you sure you want to remove this deposit return?");
            sharedModals.ModalClickOKBttn();
        }

        public void DeleteLastDeposit()
        {
            Wait();
            var totalDeposits = webDriver.FindElements(licenseDepositTableTotal).Count();
            var lastDepositDeleteIcon = By.CssSelector("div[data-testid='securityDepositsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ totalDeposits +") button[title='delete deposit']");
            FocusAndClick(lastDepositDeleteIcon);

            WaitUntil(licenseDepositModal);
            Assert.True(sharedModals.ModalHeader() == "Delete Deposit");
            Assert.True(sharedModals.ModalContent() == "Are you sure you want to remove the deposit?");
            sharedModals.ModalClickOKBttn();

        }

        public void VerifyDepositInitForm()
        {
            Wait();
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
            Wait();

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

        public void VerifyCreatedDepositTable(string depositType, string description, string amountPaid, string paidDate, string depositHolder)
        {
            Wait();
            if (depositType == "Other deposit")
            {
                Assert.True(webDriver.FindElement(licenseDepositTable1stRowDepositTypeContent).Text == "Automation Test - Other Type Deposit (Other)");
            }
            else
            {
                Assert.True(webDriver.FindElement(licenseDepositTable1stRowDepositTypeContent).Text == depositType);
            }
            
            Assert.True(webDriver.FindElement(licenseDepositTable1stRowDescriptionContent).Text == description);
            Assert.True(webDriver.FindElement(licenseDepositTable1stRowAmountPaidContent).Text == TransformCurrencyFormat(amountPaid));
            Assert.True(webDriver.FindElement(licenseDepositTable1stRowPaidDateContent).Text == TransformDateFormat(paidDate));
            Assert.True(webDriver.FindElement(licenseDepositTable1stRowDepositHolderContent).Text == depositHolder);
            Assert.True(webDriver.FindElement(licenseDepositTable1stRowEditBttn).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositTable1stRowDeleteBttn).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositTable1stRowReturnBttn).Displayed);

        }

        public void VerifyCreateReturnForm(string depositType,string depositAmount)
        {
            Wait();
            Assert.True(sharedModals.ModalHeader() == "Return a Deposit");

            Assert.True(webDriver.FindElement(licenseDepositReturnDepositTypeLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositReturnDepositTypeContent).Text == depositType);
            Assert.True(webDriver.FindElement(licenseDepositReturnDepositAmountLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositReturnDepositAmountContent).Text == TransformCurrencyFormat(depositAmount));
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
            //Assert.True(webDriver.FindElement(licenseDepositRerturnPayeeNameInput).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositAddContactButton).Displayed);

            sharedModals.VerifyButtonsPresence();
        }

        public void VerifyCreatedReturnTable(string depositType, string terminationDate, string amountPaid, string claim, string returnedAmount, string interestPaid, string returnDate, string payeeName)
        {
            Wait();
            Assert.True(webDriver.FindElement(licenseDepositReturnTable1stRowDepositTypeContent).Text == depositType);
            Assert.True(webDriver.FindElement(licenseDepositReturnTable1stRowTerminationContent).Text == TransformDateFormat(terminationDate));
            Assert.True(webDriver.FindElement(licenseDepositReturnTable1stRowAmountContent).Text == TransformCurrencyFormat(amountPaid));
            Assert.True(webDriver.FindElement(licenseDepositReturnTable1stRowClaimContent).Text == TransformCurrencyFormat(claim));
            Assert.True(webDriver.FindElement(licenseDepositReturnTable1stRowReturnedAmountContent).Text == TransformCurrencyFormat(returnedAmount));
            Assert.True(webDriver.FindElement(licenseDepositReturnTable1stRowInterestPaidContent).Text == TransformCurrencyFormat(interestPaid));
            Assert.True(webDriver.FindElement(licenseDepositReturnTable1stRowReturnDateContent).Text == TransformDateFormat(returnDate));
            Assert.True(webDriver.FindElement(licenseDepositReturnTable1stRowReturnedPayeeNameContent).Text == payeeName);
            Assert.True(webDriver.FindElement(licenseDepositReturnTable1stRowDeleteBttn).Displayed);
            Assert.True(webDriver.FindElement(licenseDepositReturnTable1stRowEditBttn).Displayed);
        }
    }
}
