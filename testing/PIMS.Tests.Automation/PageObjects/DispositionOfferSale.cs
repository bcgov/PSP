using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class DispositionOfferSale : PageObjectBase
    {
        private By offersAndSaleTab = By.XPath("//a[contains(text(),'Offers & Sale')]");

        //Appraisal and Assessment section view elements 
        private By offersAndSaleAppraisalAndAssessmentSubtitle = By.XPath("//h2/div/div/div/label[contains(text(), 'Appraisal and Assessment')]");
        private By dispositionAppraisalEditButton = By.CssSelector("button[title='Edit Appraisal']");
        private By dispositionAppraisalAndAssessmentMessage = By.XPath("//p[contains(text(),'There are no Appraisal and Assessment details indicated with this disposition file.')]");
        private By dispositionAppraisalValueLabel = By.XPath("//label[contains(text(),'Appraisal value ($)')]");
        private By dispositionAppraisalValueContent = By.XPath("//label[contains(text(),'Appraisal value ($)')]/parent::div/following-sibling::div");
        private By dispositionAppraisalValueInput = By.Id("input-appraisedValueAmount");
        private By dispositionAppraisalDateLabel = By.XPath("//label[contains(text(),'Appraisal date')]");
        private By dispositionAppraisalDateContent = By.XPath("//label[contains(text(),'Appraisal date')]/parent::div/following-sibling::div");
        private By dispositionAppraisalDateInput = By.Id("datepicker-appraisalDate");
        private By dispositionBcAssessmentValueLabel = By.XPath("//label[contains(text(),'BC assessment value ($)')]");
        private By dispositionBcAssessmentValueContent = By.XPath("//label[contains(text(),'BC assessment value ($)')]/parent::div/following-sibling::div");
        private By dispositionBcAssessmentValueInput = By.Id("input-bcaValueAmount");
        private By dispositionBcAssessmentRollYearLabel = By.XPath("//label[contains(text(),'BC assessment roll year')]");
        private By dispositionBcAssessmentRollYearContent = By.XPath("//label[contains(text(),'BC assessment roll year')]/parent::div/following-sibling::div");
        private By dispositionBcAssessmentRollYearInput = By.Id("datepicker-bcaRollYear");
        private By dispositionListPriceLabel = By.XPath("//label[contains(text(),'List price ($)')]");
        private By dispositionListPriceContent = By.XPath("//label[contains(text(),'List price ($)')]/parent::div/following-sibling::div");
        private By dispositionListPriceInput = By.Id("input-listPriceAmount");

        //Offers section view elements
        private By offersAndSaleOffersSubtitle = By.XPath("//div[contains(text(), 'Offers')]");
        private By addOffersButton = By.XPath("//div[contains(text(),'Offers')]/following-sibling::div/button");
        private By dispositionOffersMessage = By.XPath("//p[contains(text(),'There are no offers indicated with this disposition file.')]");

        private By dispositionOfferStatusLabel = By.XPath("//label[contains(text(),'Offer status')]");
        private By dispositionOfferStatusSelect = By.Id("input-dispositionOfferStatusTypeCode");
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
        private By offersAndSaleSalesDetailsSubtitle = By.XPath("//label[contains(text(), 'Sales Details')]");
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

        public void DeleteOffer(int index)
        {
            Wait();
            webDriver.FindElement(By.CssSelector("button[data-testid='Offer["+ index +"].delete-btn']")).Click();

            Wait();
            if (webDriver.FindElements(dispositionFileConfirmationModal).Count() > 0)
            {
                Assert.Equal("Delete Offer", sharedModals.ModalHeader());
                Assert.Contains("You have selected to delete this offer.", sharedModals.ModalContent());
                Assert.Contains("Do you want to proceed?", sharedModals.ModalContent());

                sharedModals.ModalClickOKBttn();
            }
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

        public void CreateNewAppraisalAndAssessment(DispositionFile appraisalandassessment)
        {
            Wait();

            webDriver.FindElement(dispositionAppraisalValueInput).SendKeys(appraisalandassessment.AppraisalAndAssessmentValue);

            if (appraisalandassessment.AppraisalAndAssessmentDate != "")
            {
                webDriver.FindElement(dispositionAppraisalDateInput).SendKeys(appraisalandassessment.AppraisalAndAssessmentDate);
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

        public void CreateNewOffer(DispositionOfferAndSale offer)
        {
            Wait();
            webDriver.FindElement(addOffersButton).Click();

            Wait();
            if(offer.OfferOfferStatus != "")
                ChooseSpecificSelectOption(dispositionOfferStatusSelect, offer.OfferOfferStatus);

            if (offer.OfferOfferName != "")
                webDriver.FindElement(dispositionOfferNameInput).SendKeys(offer.OfferOfferName);

            if (offer.OfferOfferDate != "")
            {
                webDriver.FindElement(dispositionOfferDateInput).SendKeys(offer.OfferOfferDate);
                webDriver.FindElement(dispositionOfferDateInput).SendKeys(Keys.Enter);
            }

            if (offer.OfferOfferExpiryDate != "")
            {
                webDriver.FindElement(dispositionOfferExpiryDateInput).SendKeys(offer.OfferOfferExpiryDate);
                webDriver.FindElement(dispositionOfferExpiryDateInput).SendKeys(Keys.Enter);
            }

            if (offer.OfferPrice != "")
            {
                webDriver.FindElement(dispositionOfferPriceInput).SendKeys(offer.OfferPrice);
                webDriver.FindElement(dispositionOfferPriceInput).SendKeys(Keys.Enter);
            }

            if (offer.OfferNotes != "")
            {
                webDriver.FindElement(dispositionOfferNotesInput).SendKeys(offer.OfferNotes);
                webDriver.FindElement(dispositionOfferNotesInput).SendKeys(Keys.Enter);
            }
        }

        public void UpdateAppraisalAndAssessment(DispositionFile appraisalandassessmentUpdate)
        {
            Wait();
            webDriver.FindElement(dispositionAppraisalValueInput).SendKeys(appraisalandassessmentUpdate.AppraisalAndAssessmentValue);

            if (appraisalandassessmentUpdate.AppraisalAndAssessmentDate != "")
            {
                ClearInput(dispositionAppraisalDateInput);
                webDriver.FindElement(dispositionAppraisalDateInput).SendKeys(appraisalandassessmentUpdate.AppraisalAndAssessmentDate);
                webDriver.FindElement(dispositionAppraisalDateInput).SendKeys(Keys.Enter);
            }

            ClearInput(dispositionBcAssessmentValueInput);
            webDriver.FindElement(dispositionBcAssessmentValueInput).SendKeys(appraisalandassessmentUpdate.AppraisalAndAssessmentBcAssessmentValue);

            if (appraisalandassessmentUpdate.AppraisalAndAssessmentBcAssessmentRollYear != "")
            {
                ClearInput(dispositionBcAssessmentRollYearInput);
                webDriver.FindElement(dispositionBcAssessmentRollYearInput).SendKeys(appraisalandassessmentUpdate.AppraisalAndAssessmentBcAssessmentRollYear);
                webDriver.FindElement(dispositionBcAssessmentRollYearInput).SendKeys(Keys.Enter);
            }
            ClearInput(dispositionListPriceInput);
            webDriver.FindElement(dispositionListPriceInput).SendKeys(appraisalandassessmentUpdate.AppraisalAndAssessmentListPrice);

        }

        public void UpdateOffers(DispositionOfferAndSale offerUpdate, int index) {

            Wait();
            webDriver.FindElement(By.CssSelector("button[data-testid='Offer["+ index +"].edit-btn']")).Click();

            Wait();
            if(offerUpdate.OfferOfferStatus != "")
                ChooseSpecificSelectOption(dispositionOfferStatusSelect, offerUpdate.OfferOfferStatus);

            if (offerUpdate.OfferOfferName != "")
            {
                ClearInput(By.Id("input-offerName"));
                webDriver.FindElement(By.Id("input-offerName")).SendKeys(offerUpdate.OfferOfferName);
            }

            if (offerUpdate.OfferOfferDate != "")
            {
                ClearInput(dispositionOfferDateInput);
                webDriver.FindElement(dispositionOfferDateInput).SendKeys(offerUpdate.OfferOfferDate);
                webDriver.FindElement(dispositionOfferDateInput).SendKeys(Keys.Enter);
            }
            if (offerUpdate.OfferOfferExpiryDate != "")
            {
                ClearInput(dispositionOfferExpiryDateInput);
                webDriver.FindElement(dispositionOfferExpiryDateInput).SendKeys(offerUpdate.OfferOfferExpiryDate);
                webDriver.FindElement(dispositionOfferExpiryDateInput).SendKeys(Keys.Enter);
            }
            if (offerUpdate.OfferPrice != "")
            {
                ClearInput(dispositionOfferPriceInput);
                webDriver.FindElement(dispositionOfferPriceInput).SendKeys(offerUpdate.OfferPrice);
                webDriver.FindElement(dispositionOfferPriceInput).SendKeys(Keys.Enter);
            }
            if (offerUpdate.OfferNotes != "")
            {
                ClearInput(dispositionOfferNotesInput);
                webDriver.FindElement(dispositionOfferNotesInput).SendKeys(offerUpdate.OfferNotes);
                webDriver.FindElement(dispositionOfferNotesInput).SendKeys(Keys.Enter);
            }
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

        public void VerifyCreatedAppraisalAndAssessment(DispositionFile disposition)
        {
            AssertTrueIsDisplayed(offersAndSaleAppraisalAndAssessmentSubtitle);

            AssertTrueIsDisplayed(dispositionAppraisalValueLabel);
            if(disposition.AppraisalAndAssessmentValue != "")
                AssertTrueContentEquals(dispositionAppraisalValueContent, TransformCurrencyFormat(disposition.AppraisalAndAssessmentValue));

            AssertTrueIsDisplayed(dispositionAppraisalDateLabel);
            if(disposition.AppraisalAndAssessmentDate != "")
                AssertTrueContentEquals(dispositionAppraisalDateContent, TransformDateFormat(disposition.AppraisalAndAssessmentDate));

            AssertTrueIsDisplayed(dispositionBcAssessmentValueLabel);
            if(disposition.AppraisalAndAssessmentBcAssessmentValue != "")
                AssertTrueContentEquals(dispositionBcAssessmentValueContent, TransformCurrencyFormat(disposition.AppraisalAndAssessmentBcAssessmentValue));

            AssertTrueIsDisplayed(dispositionBcAssessmentRollYearLabel);
            AssertTrueContentEquals(dispositionBcAssessmentRollYearContent, disposition.AppraisalAndAssessmentBcAssessmentRollYear);

            AssertTrueIsDisplayed(dispositionListPriceLabel);
            if(disposition.AppraisalAndAssessmentListPrice != "")
                AssertTrueContentEquals(dispositionListPriceContent, TransformCurrencyFormat(disposition.AppraisalAndAssessmentListPrice));
        }

        public void VerifyCreatedOffer(DispositionOfferAndSale offer, int index)
        {
            Wait();
            var totalOffers = index + 1;

            AssertTrueIsDisplayed(By.XPath("//div[contains(text(),'Offers')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ totalOffers +"]/div/div/label[contains(text(),'Offer status')]"));
            if(offer.OfferOfferStatus != "")
                AssertTrueContentEquals(By.CssSelector("div[data-testid='offer["+ index +"].offerStatusTypeCode']"), offer.OfferOfferStatus);

            AssertTrueIsDisplayed(By.XPath("//div[contains(text(),'Offers')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ totalOffers +"]/div/div/label[contains(text(),'Offer name(s)')]"));
            AssertTrueContentEquals(By.CssSelector("div[data-testid='offer["+ index +"].offerName']"), offer.OfferOfferName);

            AssertTrueIsDisplayed(By.XPath("//div[contains(text(),'Offers')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ totalOffers +"]/div/div/label[contains(text(),'Offer date')]"));
            AssertTrueContentEquals(By.CssSelector("div[data-testid='offer["+ index +"].offerDate']"), TransformDateFormat(offer.OfferOfferDate));

            AssertTrueIsDisplayed(By.XPath("//div[contains(text(),'Offers')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ totalOffers +"]/div/div/label[contains(text(),'Offer expiry date')]"));
            if(offer.OfferOfferExpiryDate != "")
                AssertTrueContentEquals(By.CssSelector("div[data-testid='offer["+ index +"].offerExpiryDate']"), TransformDateFormat(offer.OfferOfferExpiryDate));

            AssertTrueIsDisplayed(By.XPath("//div[contains(text(),'Offers')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ totalOffers +"]/div/div/label[contains(text(),'Offer price ($)')]"));
            AssertTrueContentEquals(By.CssSelector("div[data-testid='offer["+ index +"].offerPrice']"), TransformCurrencyFormat(offer.OfferPrice));

            AssertTrueIsDisplayed(By.XPath("//div[contains(text(),'Offers')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div["+ totalOffers +"]/div/div/label[contains(text(),'Notes')]"));
            if(offer.OfferNotes != "")
                AssertTrueContentEquals(By.CssSelector("div[data-testid='offer["+ index +"].notes']"), offer.OfferNotes);


        }
    }
}
