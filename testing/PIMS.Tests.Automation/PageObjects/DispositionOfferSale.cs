using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;
using System;
using System.Diagnostics;

namespace PIMS.Tests.Automation.PageObjects
{
    public class DispositionOfferSale : PageObjectBase

    {
        private By offersAndSaleTab = By.XPath("//a[contains(text(),'Offers & Sale')]");

        //Appraisal and Assessment section view elements 
        private By offersAndSaleAppraisalAndAssessmentSubtitle = By.XPath("//h2/div/div/div/label[contains(text(), 'Appraisal and Assessment')]");
        private By dispositionAppraisalEditButton = By.CssSelector("button[title='Edit Appraisal']");
        private By dispositionAppraisalAndAssessmentMessage = By.XPath("//p[contains(text(),'There are no value details indicated with this disposition file.')]");
        private By dispositionAppraisalValueLabel = By.XPath("//label[contains(text(),'Appraisal value ($)')]");
        private By dispositionAppraisalValueInput = By.Id("input-appraisedValueAmount");
        private By dispositionAppraisalDateLabel = By.XPath("//label[contains(text(),'Appraisal date')]");
        private By dispositionAppraisalDateInput = By.Id("datepicker-appraisalDate");
        private By dispositionBcAssessmentValueLabel = By.XPath("//label[contains(text(),'BC assessment value ($)')]");
        private By dispositionBcAssessmentValueInput = By.Id("input-bcaValueAmount");
        private By dispositionBcAssessmentRollYearLabel = By.XPath("//label[contains(text(),'BC assessment roll year')]");
        private By dispositionBcAssessmentRollYearInput = By.Id("datepicker-bcaRollYear");
        private By dispositionListPriceLabel = By.XPath("//label[contains(text(),'List price ($)')]");
        private By dispositionListPriceInput = By.Id("input-listPriceAmount");

        //Offers section view elements
        private By offersAndSaleOffersSubtitle = By.XPath("//div[contains(text(), 'Offers')]");
        private By addOffersButton = By.XPath("//div[contains(text(),'Offers')]/following-sibling::div/button");
        private By dispositionOffersMessage = By.XPath("//p[contains(text(),'There are no offers indicated with this disposition file.')]");
        private By dispositionOfferStatusLabel = By.XPath("//label[contains(text(),'Offer status')]");
        private By dispositionOfferStatusInput = By.Id("input-dispositionOfferStatusTypeCode");
        private By dispositionOfferStatusTooltip = By.XPath("//label[contains(text(),'Offer status')]/span/span[@data-testid='tooltip-icon-section-field-tooltip']");
        private By dispositionOfferNameLabel = By.XPath("//label[contains(text(),'Offer name(s)')]");
        private By dispositionOfferNameInput = By.Id("input-offerName");
        private By dispositionOfferDateLabel = By.XPath("//label[contains(text(),'Offer date')]");
        private By dispositionOfferDateInput = By.Id("datepicker-offerDate");
        private By dispositionOfferExpiryDateLabel = By.XPath("//label[contains(text(),'Offer expiry date')]");
        private By dispositionOfferExpiryDateInput = By.Id("datepicker-offerExpiryDate");
        private By dispositionOfferPriceLabel = By.XPath("//label[contains(text(),'Offer price ($)')]");
        private By dispositionOfferPriceInput = By.Id("input-offerAmount");
        private By dispositionOfferNotesLabel = By.XPath("//label[contains(text(),'Notes')]");
        private By dispositionOfferNotesInput = By.Id("input-offerNote");
        private By dispositionOfferNotesTooltip = By.XPath("//label[contains(text(),'Notes')]/span/span[@data-testid='tooltip-icon-section-field-tooltip']");

        //Sales Details section view elements
        private By offersAndSaleSalesDetailsSubtitle = By.XPath("//div[contains(text(), 'Sales Details')]");
        private By dispositionSalesDetailsMessage = By.XPath("//p[contains(text(),'There are no sale details indicated with this disposition file.')]");

        //Disposition File Confirmation Modal Elements
        private By dispositionFileConfirmationModal = By.CssSelector("div[class='modal-content']");

        private SharedModals sharedModals;

        public DispositionOfferSale(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
        }
        public void NavigateoffersAndSaleTab()
        {
            WaitUntilClickable(offersAndSaleTab);
            webDriver.FindElement(offersAndSaleTab).Click();
        }
        public void VerifyInitOffersAndSaleTab()
        {
            AssertTrueIsDisplayed(offersAndSaleAppraisalAndAssessmentSubtitle);
            AssertTrueIsDisplayed(dispositionAppraisalAndAssessmentMessage);
            AssertTrueIsDisplayed(offersAndSaleOffersSubtitle);
            AssertTrueIsDisplayed(dispositionOffersMessage);
            AssertTrueIsDisplayed(offersAndSaleSalesDetailsSubtitle);
            AssertTrueIsDisplayed(dispositionSalesDetailsMessage);
        }

        public void EditAppraisalAndAssessmentButton()
        {
            Wait(2000);
            webDriver.FindElement(dispositionAppraisalEditButton).Click();
        }

        public void SaveDispositionFileOffersAndSale()
        {
            Wait();
            ButtonElement("Save");

        }

        public void CancelDispositionFileOffersAndSale()
        {
            Wait();
            ButtonElement("Cancel");

            if (webDriver.FindElements(dispositionFileConfirmationModal).Count() > 0)
            {
                Assert.Equal("Unsaved Changes", sharedModals.ModalHeader());
                Assert.Equal("You have made changes on this form. Do you wish to leave without saving?", sharedModals.ModalContent());

                sharedModals.ModalClickOKBttn();
            }

            AssertTrueIsDisplayed(dispositionAppraisalEditButton);
        }

        public void CreateNewAppraisalAndAssessment(DispositionOfferAndSale appraisalandassessment)
        {
            Wait();

            webDriver.FindElement(dispositionAppraisalValueInput).SendKeys(appraisalandassessment.AppraisalAndAssessmentAppraisalValue);

            if (appraisalandassessment.AppraisalAndAssessmentAppraisalDate != "")
            {
                webDriver.FindElement(dispositionAppraisalDateInput).SendKeys(appraisalandassessment.AppraisalAndAssessmentAppraisalDate);
                webDriver.FindElement(dispositionAppraisalDateInput).SendKeys(Keys.Enter);
            }

            webDriver.FindElement(dispositionBcAssessmentValueInput).SendKeys(appraisalandassessment.AppraisalAndAssessmentBcAssessmentValue);

            if (appraisalandassessment.AppraisalAndAssessmentBcAssessmentRollYear != "")
            {
                webDriver.FindElement(dispositionBcAssessmentRollYearInput).SendKeys(appraisalandassessment.AppraisalAndAssessmentBcAssessmentRollYear);
                webDriver.FindElement(dispositionBcAssessmentRollYearInput).SendKeys(Keys.Enter);
            }

            webDriver.FindElement(dispositionListPriceInput).SendKeys(appraisalandassessment.AppraisalAndAssessmentListPrice);

        }

    }
}
