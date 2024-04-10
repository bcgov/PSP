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

        private By dispositionAppraisalAndAssessmentCreateFormSubtitle = By.XPath("//h2/div/div[contains(text(), 'Appraisal and Assessment')]");
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

        private By dispositionOfferCreateSubtitle = By.XPath("//div[contains(text(),'Offer')]");
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
        private By dispositionSalesDetailsEditButton = By.CssSelector("button[title='Edit Sale']");
        private By dispositionSalesDetailsMessage = By.XPath("//p[contains(text(),'There are no sale details indicated with this disposition file.')]");

        private By dispositionSalesDetailsCreateSubtitle = By.XPath("//div[contains(text(), 'Sales Details')]");
        private By dispositionSalesDetailsPurchaserNameLabel = By.XPath("//label[contains(text(),'Purchaser name(s)')]");
        private By dispositionSalesDetailsPurchaserNameLink = By.XPath("//button/div[contains(text(),'+ Add another purchaser')]");
        private By dispositionSalesDetailsPurchaserNameContent = By.CssSelector("div[data-testid='disposition-sale.purchasers']");
        private By dispositionSalesDetails1stPurchaserNameButton = By.CssSelector("div[data-testid='purchaserRow[0]'] button[@title='Select Contact']");
        private By dispositionSalesDetails1stPurchaserNameDeleteBttn = By.CssSelector("div[data-testid='purchaserRow[0]'] button[title='Remove Purchaser']");

        private By dispositionSalesDetailsPurchaserAgentLabel = By.XPath("//label[contains(text(),'Purchaser agent')]");
        private By dispositionSalesDetailsPurchaserAgentButton = By.XPath("//label[contains(text(),'Purchaser agent')]/parent::div/following-sibling::div/div/div/div/button[@title='Select Contact']");
        private By dispositionSalesDetailsPurchaserAgentInput = By.XPath("//div/input[@id='input-dispositionPurchaserAgent.contact.id']/parent::div/parent::div");
        private By dispositionSalesDetailsPurchaserAgentPrimaryContactSelect = By.Id("input-dispositionPurchaserAgent.primaryContactId");

        private By dispositionSalesDetailsPurchaserSolicitorLabel = By.XPath("//label[contains(text(),'Purchaser solicitor')]");
        private By dispositionSalesDetailsPurchaserSolicitorInput = By.XPath("//div/input[@id='input-dispositionPurchaserSolicitor.contact.id']/parent::div/parent::div");
        private By dispositionSalesDetailsPurchaserSolicitorButton = By.XPath("//label[contains(text(),'Purchaser solicitor')]/parent::div/following-sibling::div/div/div/div/button[@title='Select Contact']");
        private By dispositionSalesDetailsPurchaserSolicitorPrimaryContactSelect = By.Id("input-dispositionPurchaserSolicitor.primaryContactId");

        private By dispositionSalesDetailsRemovalDateLabel = By.XPath("//label[contains(text(),'Last condition removal date')]");
        private By dispositionSalesDetailsRemovalDateContent = By.XPath("//label[contains(text(),'Last condition removal date')]/parent::div/following-sibling::div");
        private By dispositionSalesDetailsRemovalDateInput = By.Id("datepicker-finalConditionRemovalDate");
        private By dispositionSalesDetailsRemovalDateTooltip = By.XPath("//label[contains(text(),'Last condition removal date')]/span/span[@data-testid='tooltip-icon-section-field-tooltip']");

        private By dispositionSalesDetailsCompletionDateLabel = By.XPath("//label[contains(text(),'Sale completion date')]");
        private By dispositionSalesDetailsCompletionDateContent = By.XPath("//label[contains(text(),'Sale completion date')]/parent::div/following-sibling::div");
        private By dispositionSalesDetailsCompletionDateInput = By.Id("datepicker-saleCompletionDate");

        private By dispositionSalesDetailsFiscalYearLabel = By.XPath("//label[contains(text(),'Fiscal year of sale')]");
        private By dispositionSalesDetailsFiscalYearContent = By.XPath("//label[contains(text(),'Fiscal year of sale')]/parent::div/following-sibling::div");
        private By dispositionSalesDetailsFiscalYearInput = By.Id("datepicker-saleFiscalYear");

        private By dispositionSalesDetailsSalePriceLabel = By.XPath("//label[contains(text(),'Final sale price, incl. GST ($)')]");
        private By dispositionSalesDetailsSalePriceContent = By.XPath("//label[contains(text(),'Final sale price, incl. GST ($)')]/parent::div/following-sibling::div");
        private By dispositionSalesDetailsSalePriceInput = By.Id("input-finalSaleAmount");

        private By dispositionSalesDetailsRealtorCommissionLabel = By.XPath("//label[contains(text(),'Realtor commission ($)')]");
        private By dispositionSalesDetailsRealtorCommissionContent = By.XPath("//label[contains(text(),'Realtor commission ($)')]/parent::div/following-sibling::div");
        private By dispositionSalesDetailsRealtorCommissionInput = By.Id("input-realtorCommissionAmount");

        private By dispositionSalesDetailsGSTLabel = By.XPath("//label[contains(text(),'GST required')]");
        private By dispositionSalesDetailsGSTContent = By.XPath("//label[contains(text(),'GST required')]/parent::div/following-sibling::div");
        private By dispositionSalesDetailsGSTSelect = By.Id("input-isGstRequired");

        private By dispositionSalesDetailsGSTCollectedLabel = By.XPath("//label[contains(text(),'GST collected ($)')]");
        private By dispositionSalesDetailsGSTCollectedContent = By.XPath("//label[contains(text(),'GST collected ($)')]/parent::div/following-sibling::div");
        private By dispositionSalesDetailsGSTCollectedInput = By.Id("input-gstCollectedAmount");

        private By dispositionSalesDetailsNetBookValueLabel = By.XPath("//label[contains(text(),'Net Book Value ($)')]");
        private By dispositionSalesDetailsNetBookValueContent = By.XPath("//label[contains(text(),'Net Book Value ($)')]/parent::div/following-sibling::div");
        private By dispositionSalesDetailsNetBookValueInput = By.Id("input-netBookAmount");

        private By dispositionSalesDetailsTotalCostSaleLabel = By.XPath("//label[contains(text(),'Total cost of sales ($)')]");
        private By dispositionSalesDetailsTotalCostSaleContent = By.XPath("//label[contains(text(),'Total cost of sales ($)')]/parent::div/following-sibling::div");
        private By dispositionSalesDetailsTotalCostSaleInput = By.Id("input-totalCostAmount"); 
        private By dispositionSalesDetailsTotalCostSaleTooltip = By.XPath("//label[contains(text(),'Total cost of sales ($)')]/span/span[@data-testid='tooltip-icon-section-field-tooltip']");

        private By dispositionSalesDetailsBeforeSppLabel = By.XPath("//label[contains(text(),'Net proceeds before SPP cost ($)')]");
        private By dispositionSalesDetailsBeforeSppContent = By.XPath("//label[contains(text(),'Net proceeds before SPP cost ($)')]/parent::div/following-sibling::div");
        private By dispositionSalesDetailsBeforeSppInput = By.Id("input-netProceedsBeforeSppAmount");

        private By dispositionSalesDetailsSppAmountLabel = By.XPath("//label[contains(text(),'SPP Amount ($)')]");
        private By dispositionSalesDetailsSppAmountContent = By.XPath("//label[contains(text(),'SPP Amount ($)')]/parent::div/following-sibling::div");
        private By dispositionSalesDetailsSppAmountInput = By.Id("input-sppAmount");
        private By dispositionSalesDetailsSppAmountTooltip = By.XPath("//label[contains(text(),'SPP Amount ($)')]/span/span[@data-testid='tooltip-icon-section-field-tooltip']");

        private By dispositionSalesDetailsAfterSppLabel = By.XPath("//label[contains(text(),'Net proceeds after SPP cost ($)')]");
        private By dispositionSalesDetailsAfterSppContent = By.XPath("//label[contains(text(),'Net proceeds after SPP cost ($)')]/parent::div/following-sibling::div");
        private By dispositionSalesDetailsAfterSppInput = By.Id("input-netProceedsAfterSppAmount");
        private By dispositionSalesDetailsAfterSppTooltip = By.XPath("//label[contains(text(),'Net proceeds after SPP cost ($)')]/span/span[@data-testid='tooltip-icon-section-field-tooltip']");

        private By dispositionSalesDetailsRemediationCostLabel = By.XPath("//label[contains(text(),'Remediation cost ($)')]");
        private By dispositionSalesDetailsRemediationCostContent = By.XPath("//label[contains(text(),'Remediation cost ($)')]/parent::div/following-sibling::div");
        private By dispositionSalesDetailsRemediationCostInput = By.Id("input-remediationAmount");

        //Disposition File Confirmation Modal Elements
        private By dispositionFileConfirmationModal = By.CssSelector("div[class='modal-content']");

        private SharedSelectContact sharedSelectContact;
        private SharedModals sharedModals;

        public DispositionOfferSale(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
            sharedSelectContact = new SharedSelectContact(webDriver);
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

        public void EditSalesDetailsButton()
        {
            Wait(2000);
            webDriver.FindElement(dispositionSalesDetailsEditButton).Click();
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
                Assert.Equal("Confirm Changes", sharedModals.ModalHeader());
                Assert.Contains("If you choose to cancel now, your changes will not be saved.", sharedModals.ModalContent());
                Assert.Contains("Do you want to proceed?", sharedModals.ModalContent());

                sharedModals.ModalClickOKBttn();
            }

            AssertTrueIsDisplayed(dispositionAppraisalEditButton);
        }

        public void CreateNewAppraisalAndAssessment(DispositionFile dispositionFile)
        {
            Wait();

            webDriver.FindElement(dispositionAppraisalValueInput).SendKeys(dispositionFile.AppraisalAndAssessmentValue);

            if (dispositionFile.AppraisalAndAssessmentDate != "")
            {
                webDriver.FindElement(dispositionAppraisalDateInput).SendKeys(dispositionFile.AppraisalAndAssessmentDate);
                webDriver.FindElement(dispositionAppraisalDateInput).SendKeys(Keys.Enter);
            }

            webDriver.FindElement(dispositionBcAssessmentValueInput).SendKeys(dispositionFile.AppraisalAndAssessmentBcAssessmentValue);

            if (dispositionFile.AppraisalAndAssessmentBcAssessmentRollYear != "")
            {
                webDriver.FindElement(dispositionBcAssessmentRollYearInput).SendKeys(dispositionFile.AppraisalAndAssessmentBcAssessmentRollYear);
                webDriver.FindElement(dispositionBcAssessmentRollYearInput).SendKeys(Keys.Enter);
            }

            webDriver.FindElement(dispositionListPriceInput).SendKeys(dispositionFile.AppraisalAndAssessmentListPrice);

        }

        public void CreateNewOffer(DispositionOfferAndSale offer)
        {
            Wait();
            webDriver.FindElement(addOffersButton).Click();

            VerifyInitOfferForm();

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

        public void UpdateAppraisalAndAssessment(DispositionFile dispositionFile)
        {
            Wait();
            if (dispositionFile.AppraisalAndAssessmentValue != "")
            {
                ClearInput(dispositionAppraisalValueInput);
                webDriver.FindElement(dispositionAppraisalValueInput).SendKeys(dispositionFile.AppraisalAndAssessmentValue);
            }

            if (dispositionFile.AppraisalAndAssessmentDate != "")
            {
                ClearInput(dispositionAppraisalDateInput);
                webDriver.FindElement(dispositionAppraisalDateInput).SendKeys(dispositionFile.AppraisalAndAssessmentDate);
                webDriver.FindElement(dispositionAppraisalDateInput).SendKeys(Keys.Enter);
            }

            if (dispositionFile.AppraisalAndAssessmentBcAssessmentValue != "")
            {
                ClearInput(dispositionBcAssessmentValueInput);
                webDriver.FindElement(dispositionBcAssessmentValueInput).SendKeys(dispositionFile.AppraisalAndAssessmentBcAssessmentValue);
            }

            if (dispositionFile.AppraisalAndAssessmentBcAssessmentRollYear != "")
            {
                ClearInput(dispositionBcAssessmentRollYearInput);
                webDriver.FindElement(dispositionBcAssessmentRollYearInput).SendKeys(dispositionFile.AppraisalAndAssessmentBcAssessmentRollYear);
                webDriver.FindElement(dispositionBcAssessmentRollYearInput).SendKeys(Keys.Enter);
            }

            if (dispositionFile.AppraisalAndAssessmentListPrice != "")
            {
                ClearInput(dispositionListPriceInput);
                webDriver.FindElement(dispositionListPriceInput).SendKeys(dispositionFile.AppraisalAndAssessmentListPrice);
            }
        }

        public void UpdateOffers(DispositionOfferAndSale offerUpdate, int index) {

            Wait(5000);
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

        public void InsertSalesDetails(DispositionFile dispositionFile)
        {
            Wait();
            while (webDriver.FindElements(dispositionSalesDetails1stPurchaserNameDeleteBttn).Count > 0)
            {
                webDriver.FindElement(dispositionSalesDetails1stPurchaserNameDeleteBttn).Click();

                Wait();
                Assert.Equal("Remove Purchaser", sharedModals.ModalHeader());
                Assert.Equal("Do you wish to remove this purchaser?", sharedModals.ModalContent());

                sharedModals.ModalClickOKBttn();
            }

            if (dispositionFile.PurchaserNames!.Count > 0)
            {
                for (var i = 0; i < dispositionFile.PurchaserNames.Count; i++)
                    AddPurchaseNames(dispositionFile.PurchaserNames[i], i); 
            }

            if (dispositionFile.PurchaserAgent != "")
            {
                WaitUntilVisible(dispositionSalesDetailsPurchaserAgentButton);
                webDriver.FindElement(dispositionSalesDetailsPurchaserAgentButton).Click();
                sharedSelectContact.SelectContact(dispositionFile.PurchaserAgent, dispositionFile.PurchaserAgentType);
            }

            Wait();
            if (webDriver.FindElements(dispositionSalesDetailsPurchaserAgentPrimaryContactSelect).Count > 0)
                ChooseSpecificSelectOption(dispositionSalesDetailsPurchaserAgentPrimaryContactSelect, dispositionFile.PurchaserAgentPrimaryContact);

            if (dispositionFile.PurchaserSolicitor != "")
            {
                WaitUntilVisible(dispositionSalesDetailsPurchaserSolicitorButton);
                webDriver.FindElement(dispositionSalesDetailsPurchaserSolicitorButton).Click();
                sharedSelectContact.SelectContact(dispositionFile.PurchaserSolicitor, dispositionFile.PurchaserSolicitorType);
            }

            Wait();
            if (webDriver.FindElements(dispositionSalesDetailsPurchaserSolicitorPrimaryContactSelect).Count > 0)
                ChooseSpecificSelectOption(dispositionSalesDetailsPurchaserSolicitorPrimaryContactSelect, dispositionFile.PurchaserSolicitorPrimaryContact);

            if (dispositionFile.LastConditionRemovalDate != "")
            {
                ClearInput(dispositionSalesDetailsRemovalDateInput);
                webDriver.FindElement(dispositionSalesDetailsRemovalDateInput).SendKeys(dispositionFile.LastConditionRemovalDate);
                webDriver.FindElement(dispositionSalesDetailsRemovalDateInput).SendKeys(Keys.Enter);
            }
            if (dispositionFile.SaleCompletionDate != "")
            {
                ClearInput(dispositionSalesDetailsCompletionDateInput);
                webDriver.FindElement(dispositionSalesDetailsCompletionDateInput).SendKeys(dispositionFile.SaleCompletionDate);
                webDriver.FindElement(dispositionSalesDetailsCompletionDateInput).SendKeys(Keys.Enter);
            }
            if (dispositionFile.FiscalYearOfSale != "")
            {
                ClearInput(dispositionSalesDetailsFiscalYearInput);
                webDriver.FindElement(dispositionSalesDetailsFiscalYearInput).SendKeys(dispositionFile.FiscalYearOfSale);
                webDriver.FindElement(dispositionSalesDetailsFiscalYearInput).SendKeys(Keys.Enter);
            }

            if (dispositionFile.FinalSalePrice != "")
            {
                ClearInput(dispositionSalesDetailsSalePriceInput);
                webDriver.FindElement(dispositionSalesDetailsSalePriceInput).SendKeys(dispositionFile.FinalSalePrice);
            }

            if (dispositionFile.RealtorCommission != "")
            {
                ClearInput(dispositionSalesDetailsRealtorCommissionInput);
                webDriver.FindElement(dispositionSalesDetailsRealtorCommissionInput).SendKeys(dispositionFile.RealtorCommission);
            }

            ChooseSpecificSelectOption(dispositionSalesDetailsGSTSelect, dispositionFile.GSTRequired);

            Wait();
            if (webDriver.FindElements(dispositionFileConfirmationModal).Count > 0)
            {
                Assert.Equal("Confirm Change", sharedModals.ModalHeader());
                Assert.Equal("The GST, if provided, will be cleared. Do you wish to proceed?", sharedModals.ModalContent());
                sharedModals.ModalClickOKBttn();
            }

            if (dispositionFile.GSTRequired.Equals("Yes"))
            {
                AssertTrueIsDisplayed(dispositionSalesDetailsGSTCollectedLabel);
                AssertTrueIsDisplayed(dispositionSalesDetailsGSTCollectedInput);
            }

            if (dispositionFile.NetBookValue != "")
            {
                ClearInput(dispositionSalesDetailsNetBookValueInput);
                webDriver.FindElement(dispositionSalesDetailsNetBookValueInput).SendKeys(dispositionFile.NetBookValue);
            }

            if (dispositionFile.TotalCostOfSales != "")
            {
                ClearInput(dispositionSalesDetailsTotalCostSaleInput);
                webDriver.FindElement(dispositionSalesDetailsTotalCostSaleInput).SendKeys(dispositionFile.TotalCostOfSales);
            }

            if (dispositionFile.SPPAmount != "")
            {
                ClearInput(dispositionSalesDetailsSppAmountInput);
                webDriver.FindElement(dispositionSalesDetailsSppAmountInput).SendKeys(dispositionFile.SPPAmount);
            }

            if (dispositionFile.RemediationCost != "")
            {
                ClearInput(dispositionSalesDetailsRemediationCostInput);
                webDriver.FindElement(dispositionSalesDetailsRemediationCostInput).SendKeys(dispositionFile.RemediationCost);
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

        public void VerifyInitAppraisalAndAssessmentForm()
        {
            AssertTrueIsDisplayed(dispositionAppraisalAndAssessmentCreateFormSubtitle);

            AssertTrueIsDisplayed(dispositionAppraisalValueLabel);
            AssertTrueIsDisplayed(dispositionAppraisalValueInput);

            AssertTrueIsDisplayed(dispositionAppraisalDateLabel);
            AssertTrueIsDisplayed(dispositionAppraisalDateInput);

            AssertTrueIsDisplayed(dispositionBcAssessmentValueLabel);
            AssertTrueIsDisplayed(dispositionBcAssessmentValueInput);

            AssertTrueIsDisplayed(dispositionBcAssessmentRollYearLabel);
            AssertTrueIsDisplayed(dispositionBcAssessmentRollYearInput);

            AssertTrueIsDisplayed(dispositionListPriceLabel);
            AssertTrueIsDisplayed(dispositionListPriceInput);
        }

        public void VerifyInitSalesDetailsForm()
        {
            AssertTrueIsDisplayed(dispositionSalesDetailsCreateSubtitle);

            AssertTrueIsDisplayed(dispositionSalesDetailsPurchaserNameLabel);
            AssertTrueIsDisplayed(dispositionSalesDetailsPurchaserNameLink);

            AssertTrueIsDisplayed(dispositionSalesDetailsPurchaserAgentLabel);
            AssertTrueIsDisplayed(dispositionSalesDetailsPurchaserAgentInput);
            AssertTrueIsDisplayed(dispositionSalesDetailsPurchaserAgentButton);

            AssertTrueIsDisplayed(dispositionSalesDetailsPurchaserSolicitorLabel);
            AssertTrueIsDisplayed(dispositionSalesDetailsPurchaserSolicitorInput);
            AssertTrueIsDisplayed(dispositionSalesDetailsPurchaserSolicitorButton);

            AssertTrueIsDisplayed(dispositionSalesDetailsRemovalDateLabel);
            AssertTrueIsDisplayed(dispositionSalesDetailsRemovalDateTooltip);
            AssertTrueIsDisplayed(dispositionSalesDetailsRemovalDateInput);

            AssertTrueIsDisplayed(dispositionSalesDetailsCompletionDateLabel);
            AssertTrueIsDisplayed(dispositionSalesDetailsCompletionDateInput);

            AssertTrueIsDisplayed(dispositionSalesDetailsFiscalYearLabel);
            AssertTrueIsDisplayed(dispositionSalesDetailsFiscalYearInput);

            AssertTrueIsDisplayed(dispositionSalesDetailsSalePriceLabel);
            AssertTrueIsDisplayed(dispositionSalesDetailsSalePriceInput);

            AssertTrueIsDisplayed(dispositionSalesDetailsRealtorCommissionLabel);
            AssertTrueIsDisplayed(dispositionSalesDetailsRealtorCommissionInput);

            AssertTrueIsDisplayed(dispositionSalesDetailsGSTLabel);
            AssertTrueIsDisplayed(dispositionSalesDetailsGSTSelect);

            AssertTrueIsDisplayed(dispositionSalesDetailsNetBookValueLabel);
            AssertTrueIsDisplayed(dispositionSalesDetailsNetBookValueInput);

            AssertTrueIsDisplayed(dispositionSalesDetailsTotalCostSaleLabel);
            AssertTrueIsDisplayed(dispositionSalesDetailsTotalCostSaleTooltip);
            AssertTrueIsDisplayed(dispositionSalesDetailsTotalCostSaleInput);

            AssertTrueIsDisplayed(dispositionSalesDetailsBeforeSppLabel);
            AssertTrueIsDisplayed(dispositionSalesDetailsBeforeSppInput);

            AssertTrueIsDisplayed(dispositionSalesDetailsSppAmountLabel);
            AssertTrueIsDisplayed(dispositionSalesDetailsSppAmountTooltip);
            AssertTrueIsDisplayed(dispositionSalesDetailsSppAmountInput);

            AssertTrueIsDisplayed(dispositionSalesDetailsAfterSppLabel);
            AssertTrueIsDisplayed(dispositionSalesDetailsAfterSppTooltip);
            AssertTrueIsDisplayed(dispositionSalesDetailsAfterSppInput);

            AssertTrueIsDisplayed(dispositionSalesDetailsRemediationCostLabel);
            AssertTrueIsDisplayed(dispositionSalesDetailsRemediationCostInput);
        }

        public void VerifyCreatedAppraisalAndAssessment(DispositionFile disposition)
        {
            AssertTrueIsDisplayed(offersAndSaleAppraisalAndAssessmentSubtitle);

            AssertTrueIsDisplayed(dispositionAppraisalValueLabel);
            if(disposition.AppraisalAndAssessmentValue != "")
                AssertTrueContentEquals(dispositionAppraisalValueContent, TransformCurrencyFormat(disposition.AppraisalAndAssessmentValue));

            AssertTrueIsDisplayed(dispositionAppraisalDateLabel);
            System.Diagnostics.Debug.WriteLine(dispositionAppraisalDateContent);
            if(disposition.AppraisalAndAssessmentDate != "")
                AssertTrueContentEquals(dispositionAppraisalDateContent, TransformDateFormat(disposition.AppraisalAndAssessmentDate));

            AssertTrueIsDisplayed(dispositionBcAssessmentValueLabel);
            if(disposition.AppraisalAndAssessmentBcAssessmentValue != "")
                AssertTrueContentEquals(dispositionBcAssessmentValueContent, TransformCurrencyFormat(disposition.AppraisalAndAssessmentBcAssessmentValue));

            AssertTrueIsDisplayed(dispositionBcAssessmentRollYearLabel);
            if(disposition.AppraisalAndAssessmentBcAssessmentRollYear != "")
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

        public void VerifyCreatedSalesDetails(DispositionFile disposition)
        {
            AssertTrueIsDisplayed(offersAndSaleSalesDetailsSubtitle);

            AssertTrueIsDisplayed(dispositionSalesDetailsPurchaserNameLabel);
            if (disposition.PurchaserNames!.Count > 0)
            {
                var memberLinkCount = 0;
                var purchaserNames = webDriver.FindElement(dispositionSalesDetailsPurchaserNameContent).FindElements(By.TagName("a"));

                for (int i = 0; i < disposition.PurchaserNames.Count; i++)
                {
                    Assert.Equal(disposition.PurchaserNames[i].PurchaserName, purchaserNames[memberLinkCount].Text);
                    if (disposition.PurchaserNames[i].PurchaseMemberPrimaryContact != "")
                    {
                        memberLinkCount++;
                        Assert.Equal(disposition.PurchaserNames[i].PurchaseMemberPrimaryContact, purchaserNames[memberLinkCount].Text);
                    }

                    memberLinkCount++;
                }
            }

                AssertTrueIsDisplayed(dispositionSalesDetailsRemovalDateLabel);
            if (disposition.LastConditionRemovalDate != "")
                AssertTrueContentEquals(dispositionSalesDetailsRemovalDateContent, TransformDateFormat(disposition.LastConditionRemovalDate));

            AssertTrueIsDisplayed(dispositionSalesDetailsCompletionDateLabel);
            if (disposition.SaleCompletionDate != "")
                AssertTrueContentEquals(dispositionSalesDetailsCompletionDateContent, TransformDateFormat(disposition.SaleCompletionDate));

            AssertTrueIsDisplayed(dispositionSalesDetailsFiscalYearLabel);
            if(disposition.FiscalYearOfSale != "")
                AssertTrueContentEquals(dispositionSalesDetailsFiscalYearContent, disposition.FiscalYearOfSale);

            AssertTrueIsDisplayed(dispositionSalesDetailsSalePriceLabel);
            if (disposition.FinalSalePrice != "")
                AssertTrueContentEquals(dispositionSalesDetailsSalePriceContent, TransformCurrencyFormat(disposition.FinalSalePrice));

            AssertTrueIsDisplayed(dispositionSalesDetailsRealtorCommissionLabel);
            if (disposition.RealtorCommission != "")
                AssertTrueContentEquals(dispositionSalesDetailsRealtorCommissionContent, TransformCurrencyFormat(disposition.RealtorCommission));

            AssertTrueIsDisplayed(dispositionSalesDetailsGSTLabel);
            AssertTrueContentEquals(dispositionSalesDetailsGSTContent, disposition.GSTRequired);

            AssertTrueIsDisplayed(dispositionSalesDetailsNetBookValueLabel);
            if (disposition.NetBookValue != "")
                AssertTrueContentEquals(dispositionSalesDetailsNetBookValueContent, TransformCurrencyFormat(disposition.NetBookValue));

            AssertTrueIsDisplayed(dispositionSalesDetailsTotalCostSaleLabel);
            if (disposition.TotalCostOfSales != "")
                AssertTrueContentEquals(dispositionSalesDetailsTotalCostSaleContent, TransformCurrencyFormat(disposition.TotalCostOfSales));

            AssertTrueIsDisplayed(dispositionSalesDetailsBeforeSppLabel);
            if(disposition.NetProceedsBeforeSPP != "")
                AssertTrueContentEquals(dispositionSalesDetailsBeforeSppContent, TransformCurrencyFormat(disposition.NetProceedsBeforeSPP));

            AssertTrueIsDisplayed(dispositionSalesDetailsSppAmountLabel);
            if (disposition.SPPAmount != "")
                AssertTrueContentEquals(dispositionSalesDetailsSppAmountContent, TransformCurrencyFormat(disposition.SPPAmount));

            AssertTrueIsDisplayed(dispositionSalesDetailsAfterSppLabel);
            if (disposition.NetProceedsAfterSPP != "")
                AssertTrueContentEquals(dispositionSalesDetailsAfterSppContent, TransformCurrencyFormat(disposition.NetProceedsAfterSPP));

            AssertTrueIsDisplayed(dispositionSalesDetailsRemediationCostLabel);
            if (disposition.RemediationCost != "")
                AssertTrueContentEquals(dispositionSalesDetailsRemediationCostContent, TransformCurrencyFormat(disposition.RemediationCost));
        }

        private void VerifyInitOfferForm()
        {
            AssertTrueIsDisplayed(dispositionOfferCreateSubtitle);

            AssertTrueIsDisplayed(dispositionOfferStatusLabel);
            AssertTrueIsDisplayed(dispositionOfferStatusTooltip);
            AssertTrueIsDisplayed(dispositionOfferStatusSelect);

            AssertTrueIsDisplayed(dispositionOfferNameLabel);
            AssertTrueIsDisplayed(dispositionOfferNameInput);

            AssertTrueIsDisplayed(dispositionOfferDateLabel);
            AssertTrueIsDisplayed(dispositionOfferDateInput);

            AssertTrueIsDisplayed(dispositionOfferExpiryDateLabel);
            AssertTrueIsDisplayed(dispositionOfferExpiryDateInput);

            AssertTrueIsDisplayed(dispositionOfferPriceLabel);
            AssertTrueIsDisplayed(dispositionOfferPriceInput);

            AssertTrueIsDisplayed(dispositionOfferNotesLabel);
            AssertTrueIsDisplayed(dispositionOfferNotesInput);
            AssertTrueIsDisplayed(dispositionOfferNotesTooltip);
        }

        private void AddPurchaseNames(PurchaseMember purchaseMember, int index)
        {
            Wait();
            FocusAndClick(dispositionSalesDetailsPurchaserNameLink);

            FocusAndClick(By.CssSelector("div[data-testid='purchaserRow["+ index +"]'] button[title='Select Contact']"));
            sharedSelectContact.SelectContact(purchaseMember.PurchaserName, purchaseMember.PurchaseMemberContactType);

            Wait();
            if (webDriver.FindElements(By.Id("input-dispositionPurchasers."+ index +".primaryContactId")).Count > 0)
            {
                webDriver.FindElement(By.Id("input-dispositionPurchasers."+ index +".primaryContactId")).SendKeys(purchaseMember.PurchaseMemberPrimaryContact);
            }
        }
    }
}
