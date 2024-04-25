using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PIMS.Tests.Automation.Classes;
using SeleniumExtras.WaitHelpers;

namespace PIMS.Tests.Automation.PageObjects
{
    public class AcquisitionAgreements : PageObjectBase
    {
        //Acquisition Files Agreements Initial Elements
        private By agreementsLinkTab = By.XPath("//a[contains(text(),'Agreements')]");

        private By agreementsSubtitle = By.XPath("//h2/div/div/div/div[contains(text(),'Agreements')]");
        private By agreementsCreateNewAgreementBttn = By.XPath("//h2/div/div/div/div[contains(text(),'Agreements')]/following-sibling::div/button");
        private By agreementsInitMessage = By.XPath("//p[contains(text(),'There are no agreements indicated in this acquisition file.')]");

        //Acquisition Files Agreement Create Form Elements
        private By AgreementsDetailsCreateSubtitle = By.XPath("//h2/div/div[contains(text(),'Agreement Details')]");
        private By agreementsStatusLabel = By.XPath("//label[contains(text(),'Agreement status')]");
        private By agreementsStatusInput = By.Id("input-agreementStatusTypeCode");
        private By agreementCancellationReasonInput = By.Id("input-cancellationNote");
        private By agreementsLegalSurveyPlanLabel = By.XPath("//label[contains(text(),'Legal survey plan')]");
        private By agreementsLegalSurveyPlanInput = By.Id("input-legalSurveyPlanNum");
        private By agreementsTypeLabel = By.XPath("//label[contains(text(),'Agreement type')]");
        private By agreementsTypeSelect = By.Id("input-agreementTypeCode");
        private By agreementsDateLabel = By.XPath("//label[contains(text(),'Agreement date')]");
        private By agreementsDateInput = By.Id("datepicker-agreementDate");
        private By agreementCommencementDateInput = By.Id("datepicker-commencementDate");
        private By agreementsCompletionDateLabel = By.XPath("//label[contains(text(),'Completion date')]");
        private By agreementsCompletionDateInput = By.Id("datepicker-completionDate");
        private By agreementsTerminationDateLabel = By.XPath("//label[contains(text(),'Termination date')]");
        private By agreementsTerminationDateInput = By.Id("datepicker-terminationDate");
        private By agreementsPossessionDateLabel = By.XPath("//label[contains(text(),'Possession date')]");
        private By agreementsPossessionDateInput = By.Id("datepicker-possessionDate");

        private By agreementFinancialSubtitle = By.XPath("//div[contains(text(),'Financial')]");
        private By agreementsPurchasePriceLabel = By.XPath("//label[contains(text(),'Purchase price')]");
        private By agreementsPurchasePriceInput = By.Id("input-purchasePrice");
        private By agreementsDepositNotLaterLabel = By.XPath("//label[contains(text(),'Deposit due no later than')]");
        private By agreementsDepositNotLaterInput = By.Id("input-noLaterThanDays");
        private By agreementsDepositAmountLabel = By.XPath("//label[contains(text(),'Deposit amount')]");
        private By agreementsDepositAmountInput = By.Id("input-depositAmount");

        private By agreementsTotalCount = By.XPath("//div[contains(text(),'Agreements')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div[@class='collapse show']/div");


        //Acquisition File Confirmation Modal Elements
        private By acquisitionFileConfirmationModal = By.CssSelector("div[class='modal-content']");

        private SharedModals sharedModals;


        public AcquisitionAgreements(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
        }

        public void NavigateAgreementsTab()
        {
            WaitUntilClickable(agreementsLinkTab);
            webDriver.FindElement(agreementsLinkTab).Click();
        }

        public void EditAgreementButton(int index)
        {
            Wait();
            var elementNumber = index + 1;
            webDriver.FindElement(By.CssSelector("button[data-testid='agreements["+ elementNumber +"].edit-btn']")).Click();
        }

        public void CreateNewAgreementBttn()
        {
            Wait();

            WaitUntilClickable(agreementsCreateNewAgreementBttn);
            webDriver.FindElement(agreementsCreateNewAgreementBttn).Click();
        }

        public void SaveAcquisitionFileAgreement()
        {
            Wait();
            ButtonElement("Save");

            AssertTrueIsDisplayed(agreementsCreateNewAgreementBttn);
        }

        public void CancelAcquisitionFileAgreement()
        {
            Wait();
            ButtonElement("Cancel");
           
            sharedModals.CancelActionModal();

            AssertTrueIsDisplayed(agreementsCreateNewAgreementBttn);
        }

        public void CreateUpdateAgreement(AcquisitionAgreement agreement)
        {
            Wait();

            ChooseSpecificSelectOption(agreementsStatusInput, agreement.AgreementStatus);

            if (agreement.AgreementCancellationReason != "")
            {
                ClearInput(agreementCancellationReasonInput);
                webDriver.FindElement(agreementCancellationReasonInput).SendKeys(agreement.AgreementCancellationReason);
            }

            if (agreement.AgreementLegalSurveyPlan != "")
            {
                ClearInput(agreementsLegalSurveyPlanInput);
                webDriver.FindElement(agreementsLegalSurveyPlanInput).SendKeys(agreement.AgreementLegalSurveyPlan);
            }

            if(agreement.AgreementType != "")
                ChooseSpecificSelectOption(agreementsTypeSelect, agreement.AgreementType);

            if (agreement.AgreementDate != "")
            {
                ClearInput(agreementsDateInput);
                webDriver.FindElement(agreementsDateInput).SendKeys(agreement.AgreementDate);
                webDriver.FindElement(agreementsDateInput).SendKeys(Keys.Enter);
            }

            if (agreement.AgreementCommencementDate != "")
            {
                ClearInput(agreementCommencementDateInput);
                webDriver.FindElement(agreementCommencementDateInput).SendKeys(agreement.AgreementCommencementDate);
                webDriver.FindElement(agreementCommencementDateInput).SendKeys(Keys.Enter);
            }

            if (agreement.AgreementCompletionDate != "")
            {
                ClearInput(agreementsCompletionDateInput);
                webDriver.FindElement(agreementsCompletionDateInput).SendKeys(agreement.AgreementCompletionDate);
                webDriver.FindElement(agreementsCompletionDateInput).SendKeys(Keys.Enter);
            }

            if (agreement.AgreementTerminationDate != "")
            {
                ClearInput(agreementsTerminationDateInput);
                webDriver.FindElement(agreementsTerminationDateInput).SendKeys(agreement.AgreementTerminationDate);
                webDriver.FindElement(agreementsTerminationDateInput).SendKeys(Keys.Enter);
            }

            if (agreement.AgreementPossessionDate != "")
            {
                ClearInput(agreementsPossessionDateInput);
                webDriver.FindElement(agreementsPossessionDateInput).SendKeys(agreement.AgreementPossessionDate);
                webDriver.FindElement(agreementsPossessionDateInput).SendKeys(Keys.Enter);
            }

            if (agreement.AgreementPurchasePrice != "")
            {
                ClearInput(agreementsPurchasePriceInput);
                webDriver.FindElement(agreementsPurchasePriceInput).SendKeys(agreement.AgreementPurchasePrice);
            }

            if (agreement.AgreementDepositDue != "")
            {
                ClearInput(agreementsDepositNotLaterInput);
                webDriver.FindElement(agreementsDepositNotLaterInput).SendKeys(agreement.AgreementDepositDue);
            }

            if (agreement.AgreementDepositAmount != "")
            {
                ClearInput(agreementsDepositAmountInput);
                webDriver.FindElement(agreementsDepositAmountInput).SendKeys(agreement.AgreementDepositAmount);
            }
        }

        public void DeleteLastAgreement()
        {
            var lastAgreement = webDriver.FindElements(agreementsTotalCount).Count();

            webDriver.FindElement(By.CssSelector("button[data-testid='agreements["+ lastAgreement +"].delete-btn']")).Click();

            if (webDriver.FindElements(acquisitionFileConfirmationModal).Count() > 0)
            {
                Assert.Equal("Delete Agreement", sharedModals.ModalHeader());
                Assert.Equal("You have selected to delete this Agreement.\r\n Do you want to proceed?", sharedModals.ModalContent());
                sharedModals.ModalClickOKBttn();
            }

        }

        public int TotalAgreementsCount()
        {
            Wait();
            return webDriver.FindElements(agreementsTotalCount).Count();
        }

        public void VerifyInitAgreementTab()
        {
            AssertTrueIsDisplayed(agreementsSubtitle);
            AssertTrueIsDisplayed(agreementsCreateNewAgreementBttn);
            AssertTrueIsDisplayed(agreementsInitMessage);
        }

        public void VerifyCreateAgreementForm()
        {
            AssertTrueIsDisplayed(AgreementsDetailsCreateSubtitle);

            AssertTrueIsDisplayed(agreementsStatusLabel);
            AssertTrueIsDisplayed(agreementsStatusInput);

            AssertTrueIsDisplayed(agreementsLegalSurveyPlanLabel);
            AssertTrueIsDisplayed(agreementsLegalSurveyPlanInput);

            AssertTrueIsDisplayed(agreementsTypeLabel);
            AssertTrueIsDisplayed(agreementsTypeSelect);

            AssertTrueIsDisplayed(agreementsDateLabel);
            AssertTrueIsDisplayed(agreementsDateInput);

            AssertTrueIsDisplayed(agreementsCompletionDateLabel);
            AssertTrueIsDisplayed(agreementsCompletionDateInput);

            AssertTrueIsDisplayed(agreementsTerminationDateLabel);
            AssertTrueIsDisplayed(agreementsTerminationDateInput);

            AssertTrueIsDisplayed(agreementsPossessionDateLabel);
            AssertTrueIsDisplayed(agreementsPossessionDateInput);

            AssertTrueIsDisplayed(agreementFinancialSubtitle);

            AssertTrueIsDisplayed(agreementsPurchasePriceLabel);
            AssertTrueIsDisplayed(agreementsPurchasePriceInput);

            AssertTrueIsDisplayed(agreementsDepositNotLaterLabel);
            AssertTrueIsDisplayed(agreementsDepositNotLaterInput);

            AssertTrueIsDisplayed(agreementsDepositAmountLabel);
            AssertTrueIsDisplayed(agreementsDepositAmountInput);
        }

        public void VerifyViewAgreementForm(AcquisitionAgreement agreement, int index)
        {
            var agreementNbr = index + 1;

            AssertTrueContentEquals(By.XPath("//button[@data-testid='agreements["+ agreementNbr +"].edit-btn']/parent::div/parent::div/preceding-sibling::div"), "Agreement " + agreementNbr);

            AssertTrueIsDisplayed(By.XPath("//button[@data-testid='agreements["+ agreementNbr +"].edit-btn']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Agreement status')]"));
            AssertTrueContentEquals(By.XPath("//button[@data-testid='agreements["+ agreementNbr +"].edit-btn']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Agreement status')]/parent::div/following-sibling::div"), agreement.AgreementStatus);

            if (agreement.AgreementCancellationReason != "")
            {
                AssertTrueIsDisplayed(By.XPath("//button[@data-testid='agreements["+ agreementNbr +"].edit-btn']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Cancellation reason')]"));
                AssertTrueContentEquals(By.XPath("//button[@data-testid='agreements["+ agreementNbr +"].edit-btn']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Cancellation reason')]/parent::div/following-sibling::div"), agreement.AgreementCancellationReason);
            }

            AssertTrueIsDisplayed(By.XPath("//button[@data-testid='agreements["+ agreementNbr +"].edit-btn']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Legal survey plan')]"));
            AssertTrueContentEquals(By.XPath("//button[@data-testid='agreements["+ agreementNbr +"].edit-btn']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Legal survey plan')]/parent::div/following-sibling::div"), agreement.AgreementLegalSurveyPlan);

            AssertTrueIsDisplayed(By.XPath("//button[@data-testid='agreements["+ agreementNbr +"].edit-btn']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Agreement type')]"));
            AssertTrueContentEquals(By.XPath("//button[@data-testid='agreements["+ agreementNbr +"].edit-btn']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Agreement type')]/parent::div/following-sibling::div"), agreement.AgreementType);

            AssertTrueIsDisplayed(By.XPath("//button[@data-testid='agreements["+ agreementNbr +"].edit-btn']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Agreement date')]"));
            AssertTrueContentEquals(By.XPath("//button[@data-testid='agreements["+ agreementNbr +"].edit-btn']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Agreement date')]/parent::div/following-sibling::div"), TransformDateFormat(agreement.AgreementDate));

            if (agreement.AgreementCommencementDate != "")
            {
                AssertTrueIsDisplayed(By.XPath("//button[@data-testid='agreements["+ agreementNbr +"].edit-btn']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Commencement date')]"));
                AssertTrueContentEquals(By.XPath("//button[@data-testid='agreements["+ agreementNbr +"].edit-btn']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Commencement date')]/parent::div/following-sibling::div"), TransformDateFormat(agreement.AgreementCommencementDate));
            }

            AssertTrueIsDisplayed(By.XPath("//button[@data-testid='agreements["+ agreementNbr +"].edit-btn']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Completion date')]"));
            AssertTrueContentEquals(By.XPath("//button[@data-testid='agreements["+ agreementNbr +"].edit-btn']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Completion date')]/parent::div/following-sibling::div"), TransformDateFormat(agreement.AgreementCompletionDate));

            AssertTrueIsDisplayed(By.XPath("//button[@data-testid='agreements["+ agreementNbr +"].edit-btn']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Termination date')]"));
            AssertTrueContentEquals(By.XPath("//button[@data-testid='agreements["+ agreementNbr +"].edit-btn']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Termination date')]/parent::div/following-sibling::div"), TransformDateFormat(agreement.AgreementTerminationDate));

            AssertTrueIsDisplayed(By.XPath("//button[@data-testid='agreements["+ agreementNbr +"].edit-btn']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Possession date')]"));
            AssertTrueContentEquals(By.XPath("//button[@data-testid='agreements["+ agreementNbr +"].edit-btn']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Possession date')]/parent::div/following-sibling::div"), TransformDateFormat(agreement.AgreementPossessionDate));

            AssertTrueIsDisplayed(By.XPath("//button[@data-testid='agreements["+ agreementNbr +"].edit-btn']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div[contains(text(),'Financial')]"));

            AssertTrueIsDisplayed(By.XPath("//button[@data-testid='agreements["+ agreementNbr +"].edit-btn']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Purchase price')]"));
            AssertTrueContentEquals(By.XPath("//button[@data-testid='agreements["+ agreementNbr +"].edit-btn']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Purchase price')]/parent::div/following-sibling::div"), TransformCurrencyFormat(agreement.AgreementPurchasePrice));

            AssertTrueIsDisplayed(By.XPath("//button[@data-testid='agreements["+ agreementNbr +"].edit-btn']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Deposit due no later than')]"));
            AssertTrueContentEquals(By.XPath("//button[@data-testid='agreements["+ agreementNbr +"].edit-btn']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Deposit due no later than')]/parent::div/following-sibling::div"), agreement.AgreementDepositDue + " days");

            AssertTrueIsDisplayed(By.XPath("//button[@data-testid='agreements["+ agreementNbr +"].edit-btn']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Deposit amount')]"));
            AssertTrueContentEquals(By.XPath("//button[@data-testid='agreements["+ agreementNbr +"].edit-btn']/parent::div/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Deposit amount')]/parent::div/following-sibling::div"), TransformCurrencyFormat(agreement.AgreementDepositAmount));
        }
    }
}
