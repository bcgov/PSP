using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PIMS.Tests.Automation.Classes;
using SeleniumExtras.WaitHelpers;

namespace PIMS.Tests.Automation.PageObjects
{
    public class AcquisitionAgreements : PageObjectBase
    {
        private By agreementsLinkTab = By.XPath("//a[contains(text(),'Agreements')]");

        private By agreementsEditBttn = By.CssSelector("button[title='Edit agreements file']");
        private By agreementsCreateNewAgreementBttn = By.XPath("//form/button");

        private By agreementsInitMessageP1 = By.XPath("//p[contains(text(),'There are no agreements associated with this file.')]");
        private By agreementsInitMessageP2 = By.XPath("//p[contains(text(),' To begin an agreement, click the edit button.')]");

        private By agreementsTotalCount = By.XPath("//form/button/following-sibling::div");

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

        public void EditAgreementButton()
        {
            Wait();
            webDriver.FindElement(agreementsEditBttn).Click();
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

            AssertTrueIsDisplayed(agreementsEditBttn);
        }

        public void CancelAcquisitionFileAgreement()
        {
            Wait();
            ButtonElement("Cancel");
           
            if (webDriver.FindElements(acquisitionFileConfirmationModal).Count() > 0)
            {
                Assert.Equal("Unsaved Changes", sharedModals.ModalHeader());
                Assert.Equal("You have made changes on this form. Do you wish to leave without saving?", sharedModals.ModalContent());

                sharedModals.ModalClickOKBttn();
            }

            AssertTrueIsDisplayed(agreementsEditBttn);
        }

        public void CreateNewAgreement(AcquisitionAgreement agreement, int index)
        {
            Wait();

            ChooseSpecificSelectOption(By.Id("input-agreements."+ index +".agreementStatusTypeCode"), agreement.AgreementStatus);

            if(agreement.AgreementLegalSurveyPlan != "")
                webDriver.FindElement(By.Id("input-agreements."+ index +".legalSurveyPlanNum")).SendKeys(agreement.AgreementLegalSurveyPlan);

            ChooseSpecificSelectOption(By.Id("input-agreements."+ index +".agreementTypeCode"), agreement.AgreementType);

            if (agreement.AgreementDate != "")
            {
                webDriver.FindElement(By.Id("datepicker-agreements."+ index +".agreementDate")).SendKeys(agreement.AgreementDate);
                webDriver.FindElement(By.Id("datepicker-agreements."+ index +".agreementDate")).SendKeys(Keys.Enter);
            }

            if (agreement.AgreementCommencementDate != "")
            {
                webDriver.FindElement(By.Id("datepicker-agreements."+ index +".commencementDate")).SendKeys(agreement.AgreementCommencementDate);
                webDriver.FindElement(By.Id("datepicker-agreements."+ index +".commencementDate")).SendKeys(Keys.Enter);
            }

            if (agreement.AgreementCompletionDate != "")
            {
                webDriver.FindElement(By.Id("datepicker-agreements."+ index +".completionDate")).SendKeys(agreement.AgreementCompletionDate);
                webDriver.FindElement(By.Id("datepicker-agreements."+ index +".completionDate")).SendKeys(Keys.Enter);
            }

            if (agreement.AgreementTerminationDate != "")
            {
                webDriver.FindElement(By.Id("datepicker-agreements."+ index +".terminationDate")).SendKeys(agreement.AgreementTerminationDate);
                webDriver.FindElement(By.Id("datepicker-agreements."+ index +".terminationDate")).SendKeys(Keys.Enter);
            }
                

            if (agreement.AgreementPurchasePrice != "")
                webDriver.FindElement(By.Id("input-agreements."+ index +".purchasePrice")).SendKeys(agreement.AgreementPurchasePrice);

            if (agreement.AgreementDepositDue != "")
                webDriver.FindElement(By.Id("input-agreements."+ index +".noLaterThanDays")).SendKeys(agreement.AgreementDepositDue);

            if (agreement.AgreementDepositAmount != "")
                webDriver.FindElement(By.Id("input-agreements."+ index +".depositAmount")).SendKeys(agreement.AgreementDepositAmount);
        }

        public void UpdateAgreement(AcquisitionAgreement agreement, int index)
        {
            Wait();

            ChooseSpecificSelectOption(By.Id("input-agreements."+ index +".agreementStatusTypeCode"), agreement.AgreementStatus);

            if (agreement.AgreementLegalSurveyPlan != "")
            {
                ClearInput(By.Id("input-agreements."+ index +".legalSurveyPlanNum"));
                webDriver.FindElement(By.Id("input-agreements."+ index +".legalSurveyPlanNum")).SendKeys(agreement.AgreementLegalSurveyPlan);
            }
            
            ChooseSpecificSelectOption(By.Id("input-agreements."+ index +".agreementTypeCode"), agreement.AgreementType);

            if (agreement.AgreementDate != "")
            {
                ClearInput(By.Id("datepicker-agreements."+ index +".agreementDate"));
                webDriver.FindElement(By.Id("datepicker-agreements."+ index +".agreementDate")).SendKeys(agreement.AgreementDate);
            }

            if (agreement.AgreementCommencementDate != "")
            {
                ClearInput(By.Id("datepicker-agreements."+ index +".commencementDate"));
                webDriver.FindElement(By.Id("datepicker-agreements."+ index +".commencementDate")).SendKeys(agreement.AgreementCommencementDate);
            }

            if (agreement.AgreementCompletionDate != "")
            {
                ClearInput(By.Id("datepicker-agreements."+ index +".completionDate"));
                webDriver.FindElement(By.Id("datepicker-agreements."+ index +".completionDate")).SendKeys(agreement.AgreementCompletionDate);
            }

            if (agreement.AgreementTerminationDate != "")
            {
                ClearInput(By.Id("datepicker-agreements."+ index +".terminationDate"));
                webDriver.FindElement(By.Id("datepicker-agreements."+ index +".terminationDate")).SendKeys(agreement.AgreementTerminationDate);
            }

            if (agreement.AgreementPurchasePrice != "")
            {
                ClearInput(By.Id("input-agreements."+ index +".purchasePrice"));
                webDriver.FindElement(By.Id("input-agreements."+ index +".purchasePrice")).SendKeys(agreement.AgreementPurchasePrice);
            }

            if (agreement.AgreementDepositDue != "")
            {
                ClearInput(By.Id("input-agreements."+ index +".noLaterThanDays"));
                webDriver.FindElement(By.Id("input-agreements."+ index +".noLaterThanDays")).SendKeys(agreement.AgreementDepositDue);
            }

            if (agreement.AgreementDepositAmount != "")
            {
                ClearInput(By.Id("input-agreements."+ index +".depositAmount"));
                webDriver.FindElement(By.Id("input-agreements."+ index +".depositAmount")).SendKeys(agreement.AgreementDepositAmount);
            }    
        }

        public void DeleteLastAgreement()
        {
            var lastAgreement = webDriver.FindElements(agreementsTotalCount).Count();

            webDriver.FindElement(By.XPath("//form/button/following-sibling::div["+ lastAgreement +"]/div/div/div/button[@title='Delete Agreement']")).Click();

            if (webDriver.FindElements(acquisitionFileConfirmationModal).Count() > 0)
            {
                Assert.Equal("Confirm Delete", sharedModals.ModalHeader());
                Assert.Equal("Are you sure you want to delete this item?", sharedModals.ModalContent());
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
            AssertTrueIsDisplayed(agreementsInitMessageP1);
            AssertTrueIsDisplayed(agreementsInitMessageP2);
        }

        public void VerifyCreateAgreementForm(int index)
        {
            var agreementNbr = index + 1;

            AssertTrueIsDisplayed(By.XPath("//form/button/following-sibling::div["+ agreementNbr +"]/h2/div/div[contains(text(),'Agreement')]"));
            AssertTrueIsDisplayed(By.XPath("//form/button/following-sibling::div["+ agreementNbr +"]/div[@class='collapse show']/div[contains(text(),'Agreement details')]"));

            AssertTrueIsDisplayed(By.XPath("//form/button/following-sibling::div["+ agreementNbr +"]/div[@class='collapse show']/div/div/label[contains(text(),'Agreement status')]"));
            AssertTrueIsDisplayed(By.Id("input-agreements."+ index +".agreementStatusTypeCode"));

            AssertTrueIsDisplayed(By.XPath("//form/button/following-sibling::div["+ agreementNbr +"]/div[@class='collapse show']/div/div/label[contains(text(),'Legal survey plan')]"));
            AssertTrueIsDisplayed(By.Id("input-agreements."+ index +".legalSurveyPlanNum"));

            AssertTrueIsDisplayed(By.XPath("//form/button/following-sibling::div["+ agreementNbr +"]/div[@class='collapse show']/div/div/label[contains(text(),'Agreement type')]"));
            AssertTrueIsDisplayed(By.Id("input-agreements."+ index +".agreementTypeCode"));
            
            AssertTrueIsDisplayed(By.XPath("//form/button/following-sibling::div["+ agreementNbr +"]/div[@class='collapse show']/div/div/label[contains(text(),'Completion date')]"));
            AssertTrueIsDisplayed(By.Id("datepicker-agreements."+ index +".completionDate"));

            AssertTrueIsDisplayed(By.XPath("//form/button/following-sibling::div["+ agreementNbr +"]/div[@class='collapse show']/div/div/label[contains(text(),'Termination date')]"));
            AssertTrueIsDisplayed(By.Id("datepicker-agreements."+ index +".terminationDate"));

            AssertTrueIsDisplayed(By.XPath("//form/button/following-sibling::div["+ agreementNbr +"]/div[@class='collapse show']/div[contains(text(),'Financial')]"));
            AssertTrueIsDisplayed(By.XPath("//form/button/following-sibling::div["+ agreementNbr +"]/div[@class='collapse show']/div/div/label[contains(text(),'Purchase price')]"));
            AssertTrueIsDisplayed(By.Id("input-agreements."+ index +".purchasePrice"));

            AssertTrueIsDisplayed(By.XPath("//form/button/following-sibling::div["+ agreementNbr +"]/div[@class='collapse show']/div/div/label[contains(text(),'Deposit due no later than')]"));
            AssertTrueIsDisplayed(By.Id("input-agreements."+ index +".noLaterThanDays"));

            AssertTrueIsDisplayed(By.XPath("//form/button/following-sibling::div[1]/div[@class='collapse show']/div/div/label[contains(text(),'Deposit amount')]"));
            AssertTrueIsDisplayed(By.Id("input-agreements."+ index +".depositAmount"));
        }

        public void VerifyViewAgreementForm(AcquisitionAgreement agreement, int index)
        {
            var agreementNbr = index + 1;

            AssertTrueContentEquals(By.XPath("//button[@title='Edit agreements file']/parent::div/following-sibling::div["+ agreementNbr +"]/h2/div/div/div/div[1]"), "Agreement " + agreementNbr);
            AssertTrueIsDisplayed(By.XPath("//button[@title='Edit agreements file']/parent::div/following-sibling::div["+ agreementNbr +"]/div/div[contains(text(),'Agreement details')]"));

            AssertTrueIsDisplayed(By.XPath("//button[@title='Edit agreements file']/parent::div/following-sibling::div["+ agreementNbr +"]/div/div/div/label[contains(text(),'Agreement status')]"));
            AssertTrueContentEquals(By.XPath("//button[@title='Edit agreements file']/parent::div/following-sibling::div["+ agreementNbr +"]/div/div/div/label[contains(text(),'Agreement status')]/parent::div/following-sibling::div"), agreement.AgreementStatus);

            AssertTrueIsDisplayed(By.XPath("//button[@title='Edit agreements file']/parent::div/following-sibling::div["+ agreementNbr +"]/div/div/div/label[contains(text(),'Legal survey plan')]"));
            AssertTrueContentEquals(By.XPath("//button[@title='Edit agreements file']/parent::div/following-sibling::div["+ agreementNbr +"]/div/div/div/label[contains(text(),'Legal survey plan')]/parent::div/following-sibling::div"), agreement.AgreementLegalSurveyPlan);

            AssertTrueIsDisplayed(By.XPath("//button[@title='Edit agreements file']/parent::div/following-sibling::div["+ agreementNbr +"]/div/div/div/label[contains(text(),'Agreement type')]"));
            AssertTrueContentEquals(By.XPath("//button[@title='Edit agreements file']/parent::div/following-sibling::div["+ agreementNbr +"]/div/div/div/label[contains(text(),'Agreement type')]/parent::div/following-sibling::div"), agreement.AgreementType);

            AssertTrueIsDisplayed(By.XPath("//button[@title='Edit agreements file']/parent::div/following-sibling::div["+ agreementNbr +"]/div/div/div/label[contains(text(),'Agreement date')]"));
            AssertTrueContentEquals(By.XPath("//button[@title='Edit agreements file']/parent::div/following-sibling::div["+ agreementNbr +"]/div/div/div/label[contains(text(),'Agreement date')]/parent::div/following-sibling::div"), TransformDateFormat(agreement.AgreementDate));

            if (agreement.AgreementCommencementDate != "")
            {
                System.Diagnostics.Debug.WriteLine(webDriver.FindElement(By.XPath("//button[@title='Edit agreements file']/parent::div/following-sibling::div["+ agreementNbr +"]/div/div/div/label[contains(text(),'Commencement date')]/parent::div/following-sibling::div")).Text);
                System.Diagnostics.Debug.WriteLine("THIS OTHER ONE: " + TransformDateFormat(agreement.AgreementCommencementDate));
                AssertTrueIsDisplayed(By.XPath("//button[@title='Edit agreements file']/parent::div/following-sibling::div["+ agreementNbr +"]/div/div/div/label[contains(text(),'Commencement date')]"));
                AssertTrueContentEquals(By.XPath("//button[@title='Edit agreements file']/parent::div/following-sibling::div["+ agreementNbr +"]/div/div/div/label[contains(text(),'Commencement date')]/parent::div/following-sibling::div"), TransformDateFormat(agreement.AgreementCommencementDate));
            }

            AssertTrueIsDisplayed(By.XPath("//button[@title='Edit agreements file']/parent::div/following-sibling::div["+ agreementNbr +"]/div/div/div/label[contains(text(),'Completion date')]"));
            AssertTrueContentEquals(By.XPath("//button[@title='Edit agreements file']/parent::div/following-sibling::div["+ agreementNbr +"]/div/div/div/label[contains(text(),'Completion date')]/parent::div/following-sibling::div"), TransformDateFormat(agreement.AgreementCompletionDate));

            AssertTrueIsDisplayed(By.XPath("//button[@title='Edit agreements file']/parent::div/following-sibling::div["+ agreementNbr +"]/div/div/div/label[contains(text(),'Termination date')]"));
            AssertTrueContentEquals(By.XPath("//button[@title='Edit agreements file']/parent::div/following-sibling::div["+ agreementNbr +"]/div/div/div/label[contains(text(),'Termination date')]/parent::div/following-sibling::div"), TransformDateFormat(agreement.AgreementTerminationDate));


            AssertTrueIsDisplayed(By.XPath("//button[@title='Edit agreements file']/parent::div/following-sibling::div["+ agreementNbr +"]/div/div[contains(text(),'Financial')]"));

            AssertTrueIsDisplayed(By.XPath("//button[@title='Edit agreements file']/parent::div/following-sibling::div["+ agreementNbr +"]/div/div/div/label[contains(text(),'Purchase price')]"));
            AssertTrueContentEquals(By.XPath("//button[@title='Edit agreements file']/parent::div/following-sibling::div["+ agreementNbr +"]/div/div/div/label[contains(text(),'Purchase price')]/parent::div/following-sibling::div"), TransformCurrencyFormat(agreement.AgreementPurchasePrice));

            AssertTrueIsDisplayed(By.XPath("//button[@title='Edit agreements file']/parent::div/following-sibling::div["+ agreementNbr +"]/div/div/div/label[contains(text(),'Deposit due no later than')]"));
            AssertTrueContentEquals(By.XPath("//button[@title='Edit agreements file']/parent::div/following-sibling::div["+ agreementNbr +"]/div/div/div/label[contains(text(),'Deposit due no later than')]/parent::div/following-sibling::div"), agreement.AgreementDepositDue + " days");

            AssertTrueIsDisplayed(By.XPath("//button[@title='Edit agreements file']/parent::div/following-sibling::div["+ agreementNbr +"]/div/div/div/label[contains(text(),'Deposit amount')]"));
            AssertTrueContentEquals(By.XPath("//button[@title='Edit agreements file']/parent::div/following-sibling::div["+ agreementNbr +"]/div/div/div/label[contains(text(),'Deposit amount')]/parent::div/following-sibling::div"), TransformCurrencyFormat(agreement.AgreementDepositAmount));
        }
    }
}
