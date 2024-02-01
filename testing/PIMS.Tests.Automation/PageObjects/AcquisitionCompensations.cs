using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;
using System;
using System.Text.RegularExpressions;

namespace PIMS.Tests.Automation.PageObjects
{
    public class AcquisitionCompensations : PageObjectBase
    {
        private By compensationLinkTab = By.XPath("//a[contains(text(),'Compensation')]");

        //Add Compensation Elements
        private By compensationAddSubtitle = By.XPath("//div[contains(text(),'Compensation Requisitions')]");
        private By compensationAddBttn = By.XPath("//div[contains(text(),'Compensation Requisitions')]/following-sibling::div/button");

        private By compensationTotalAllowableLabel = By.XPath("//label[contains(text(),'Total allowable compensation')]");
        private By compensationTotalAllowableContent = By.XPath("//label[contains(text(),'Total allowable compensation')]/parent::div/following-sibling::div");
        private By compensationTotalAllowableEdiBttn = By.XPath("//label[contains(text(),'Total allowable compensation')]/parent::div/following-sibling::div/button[@data-testid='edit-button']");
        private By compensationTotalAllowableInput = By.CssSelector("input[title='Enter a financial value']");
        private By compensationTotalAllowableSaveBttn = By.CssSelector("button[title='confirm']");

        private By compensationTotalPaymentsMadeLabel = By.XPath("//label[contains(text(),'Total payments made on this file')]");
        private By compensationTotalPaymentsMadeContent = By.XPath("//label[contains(text(),'Total payments made on this file')]/parent::div/following-sibling::div");

        private By compensationDraftsTotalLabel = By.XPath("//label[contains(text(),'Drafts')]");
        private By compensationDraftsTotalContent = By.XPath("//label[contains(text(),'Drafts')]/parent::div/following-sibling::div");

        private By compensationAddCompensationTooltips = By.XPath("//div[contains(text(),'Compensation Requisitions')]/parent::div/parent::div/parent::div/parent::h2/parent::div/div/div/div/label/span");

        //Requisition in this file (H120) Elements
        private By compensationH120Subtitle = By.XPath("//div[contains(text(),'Requisitions in this file (H120)')]");
        private By compensationH120Table = By.CssSelector("div[data-testid='AcquisitionCompensationTable']");
        private By compensationH120NoRowsMessage = By.CssSelector("div[data-testid='AcquisitionCompensationTable'] div[class='no-rows-message']");
        private By compensationH120TotalCount = By.CssSelector("div[data-testid='AcquisitionCompensationTable'] div[class='tbody'] div[class='tr-wrapper']");

        //Compensation Requisition Details Elements
        private By compensationDetailsTitle = By.XPath("//div[contains(text(),'Compensation Requisition (H120)')]");
        private By compensationClientLabel = By.XPath("//label[contains(text(),'Client')]");
        private By compensationClientContent = By.CssSelector("div[data-testid='compensation-client'] strong");

        private By compensationRequisitionNumberLabel = By.XPath("//label[contains(text(),'Requisition number')]");
        private By compensationRequisitionNumberContent = By.CssSelector("div[data-testid='compensation-number'] strong");
        private By compensationAmountLabel = By.XPath("//label[contains(text(),'Compensation amount')]");
        private By compensationAmountContent = By.CssSelector("div[data-testid='header-pretax-amount'] strong p");
        private By compensationGSTLabel = By.XPath("//label[contains(text(),'Applicable GST')]");
        private By compensationGSTContent = By.CssSelector("div[data-testid='header-tax-amount'] strong p");
        private By compensationTotalChequeAmountLabel = By.XPath("//label[contains(text(),'Total cheque amount')]");
        private By compensationTotalChequeAmountContent = By.CssSelector("div[data-testid='header-total-amount'] strong p");

        private By requisitionDetailsViewSubtitle = By.XPath("//div[contains(text(),'Requisition Details')]");
        private By requisitionDetailsCreateSubtitle = By.XPath("//div[contains(text(),'Requisition details')]");
        private By requisitionGenerateH120Bttn = By.XPath("//div[contains(text(),'Requisition Details')]/div/button[2]");
        private By requisitionEditBttn = By.XPath("//div[contains(text(),'Requisition Details')]/div/button[@title='Edit compensation requisition']");
        private By requisitionStatusLabel = By.XPath("//div[contains(text(),'Requisition Details')]/parent::div/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Status')]");
        private By requisitionStatusContent = By.XPath("//div[contains(text(),'Requisition Details')]/parent::div/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Status')]/parent::div/following-sibling::div");
        private By requisitionStatusSelect = By.XPath("//div[contains(text(),'Requisition Details')]/parent::div/parent::h2/following-sibling::div/div/div/div/select[@id='input-status']");
        private By requisitionAltProjectLabel = By.XPath("//div[contains(text(),'Requisition Details')]/parent::div/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Alternate project')]");
        private By requisitionAltProjectContent = By.XPath("//div[contains(text(),'Requisition Details')]/parent::div/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Alternate project')]/parent::div/following-sibling::div");
        private By requisitionAltProjectInput = By.Id("typeahead-alternateProject");
        private By requisitionAltProjectOptions = By.CssSelector("div[data-testid='typeahead-alternateProject'] div[aria-label='menu-options']");
        private By requisitionAltProject1stOption = By.CssSelector("div[data-testid='typeahead-alternateProject'] div[aria-label='menu-options'] a:nth-child(1)");
        private By requisitionFinalDateLabel = By.XPath("//label[contains(text(),'Final date')]");
        private By requisitionAgreementLabel = By.XPath("//div[contains(text(),'Requisition Details')]/parent::div/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Agreement date')]");
        private By requisitionAgreementContent = By.XPath("//div[contains(text(),'Requisition Details')]/parent::div/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Agreement date')]/parent::div/following-sibling::div");
        private By requisitionAgreementInput = By.Id("datepicker-agreementDateTime");
        private By requsitionExpropriationServedLabel = By.XPath("//label[contains(text(),'Expropriation notice served date')]");
        private By requsitionExpropriationServedContent = By.XPath("//label[contains(text(),'Expropriation notice served date')]/parent::div/following-sibling::div");
        private By requisitionExpropriationServedInput = By.Id("datepicker-expropriationNoticeServedDateTime");
        private By requisitionExpropriationVestingLabel = By.XPath("//label[contains(text(),'Expropriation vesting date')]");
        private By requisitionExpropriationVestingContent = By.XPath("//label[contains(text(),'Expropriation vesting date')]/parent::div/following-sibling::div");
        private By requisitionExpropriationVestingInput = By.Id("datepicker-expropriationVestingDateTime");
        private By requisitionAdvancePaymentLabel = By.XPath("//label[contains(text(),'Advanced payment served date')]");
        private By requisitionAdvancePaymentContent = By.XPath("//label[contains(text(),'Advanced payment served date')]/parent::div/following-sibling::div");
        private By requisitionAdvancePaymentInput = By.Id("datepicker-advancedPaymentServedDate");
        private By requisitionSpecialInstructionLabel = By.XPath("//label[contains(text(),'Special instructions')]");
        private By requisitionSpecialInstructionContent = By.XPath("//label[contains(text(),'Special instructions')]/parent::div/following-sibling::div");
        private By requisitionSpecialInstructionTextarea = By.Id("input-specialInstruction");

        private By requisitionFinancialCodingSubtitle = By.XPath("//div[contains(text(),'Financial Coding')]");
        private By requisitionProductLabel = By.XPath("//div[contains(text(),'Financial Coding')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Product')]");
        private By requisitionProductContent = By.XPath("//div[contains(text(),'Financial Coding')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Product')]/parent::div/following-sibling::div");

        private By requisitionBusinessLabel = By.XPath("//label[contains(text(),'Business function')]");
        private By requisitionBusinessContent = By.XPath("//label[contains(text(),'Business function')]/parent::div/following-sibling::div");
        private By requisitionWorkActivityLabel = By.XPath("//label[contains(text(),'Work activity')]");
        private By requisitionWorkActivityContent = By.XPath("//label[contains(text(),'Work activity')]/parent::div/following-sibling::div");
        private By requisitionCostTypeLabel = By.XPath("//label[contains(text(),'Cost type')]");
        private By requisitionCostTypeContent = By.XPath("//label[contains(text(),'Cost type')]/parent::div/following-sibling::div");
        private By requisitionFiscalYearLabel = By.XPath("//label[contains(text(),'Fiscal year')]");
        private By requisitionFiscalYearContent = By.XPath("//label[contains(text(),'Fiscal year')]/parent::div/following-sibling::div");
        private By requisitionFiscalYearSelect = By.Id("input-fiscalYear");
        private By requisitionSTOBLabel = By.XPath("//label[contains(text(),'STOB')]");
        private By requisitionSTOBContent = By.XPath("//label[contains(text(),'STOB')]/parent::div/following-sibling::div");
        private By requisitionSTOBInput = By.Id("typeahead-select-stob");
        private By requisitionSTOBOptions = By.CssSelector("div[id='typeahead-select-stob']");
        private By requisitionSTOB1stOption = By.CssSelector("div[id='typeahead-select-stob'] a:nth-child(1)");
        private By requisitionServiceLineLabel = By.XPath("//label[contains(text(),'Service line')]");
        private By requisitionServiceLineContent = By.XPath("//label[contains(text(),'Service line')]/parent::div/following-sibling::div/label");
        private By requisitionServiceLineInput = By.Id("typeahead-select-serviceLine");
        private By requisitionServiceLineOptions = By.CssSelector("div[id='typeahead-select-serviceLine']");
        private By requisitionServiceLine1stOption = By.CssSelector("div[id='typeahead-select-serviceLine'] a:nth-child(1)");
        private By requisitionResponsibilityCentreLabel = By.XPath("//label[contains(text(),'Responsibility centre')]");
        private By requisitionResponsibilityCentreContent = By.XPath("//label[contains(text(),'Responsibility centre')]/parent::div/following-sibling::div/label");
        private By requisitionResponsibilityCentreInput = By.Id("typeahead-select-responsibilityCentre");
        private By requisitionResponsibilityCentreOptions = By.CssSelector("div[id='typeahead-select-responsibilityCentre']");
        private By requisitionResponsibilityCentre1stOption = By.CssSelector("div[id='typeahead-select-responsibilityCentre'] a:nth-child(1)");

        private By requisitionPaymentSubtitle = By.XPath("//h2/div/div[contains(text(),'Payment')]");
        private By requisitionPayeeLabel = By.XPath("//label[contains(text(),'Payee')]");
        private By requisitionPayeeContent = By.XPath("//label[contains(text(),'Payee')]/parent::div/following-sibling::div/div/label[1]");
        private By requisitionPayeeContentLink = By.XPath("//label[contains(text(),'Payee')]/parent::div/following-sibling::div/div/a/span");
        private By requisitionPayeeSelect = By.Id("input-payee.payeeKey");
        private By requisitionPaymentInTrustLabel = By.XPath("//label[contains(text(),'Payment in Trust?')]");
        private By requisitionPaymentInTrustContent1 = By.XPath("//label[contains(text(),'Payee')]/parent::div/following-sibling::div/div/label[1]");
        private By requisitionPaymentInTrustContent2 = By.XPath("//label[contains(text(),'Payee')]/parent::div/following-sibling::div/div/label[2]");
        private By requisitionPaymentInTrustCheckbox = By.Id("input-payee.isPaymentInTrust");
        private By requisitionGSTNumberLabel = By.XPath("//label[contains(text(),'GST number')]");
        private By requisitionGSTNumberInput = By.Id("input-payee.gstNumber");
        private By requisitionAmountLabel = By.XPath("//div[contains(text(),'Payment')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Amount (before tax)')]");
        private By requisitionAmountContent = By.XPath("//div[contains(text(),'Payment')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Amount (before tax)')]/parent::div/following-sibling::div");
        private By requisitionAmountInput = By.Id("input-payee.pretaxAmount");
        private By requisitionGSTApplicableLabel = By.XPath("//div[contains(text(),'Payment')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'GST applicable?')]");
        private By requisitionGSTApplicableContent = By.XPath("//div[contains(text(),'Payment')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'GST applicable?')]/parent::div/following-sibling::div");
        private By requisitionGSTAmountLabel = By.XPath("//div[contains(text(),'Payment')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'GST amount')]");
        private By requisitionGSTAmountContent = By.XPath("//div[contains(text(),'Payment')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'GST amount')]/parent::div/following-sibling::div");
        private By requisitionGSTAmountInput = By.Id("input-payee.taxAmount");
        private By requisitionTotalAmountLabel = By.XPath("//div[contains(text(),'Payment')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Total amount')]");
        private By requisitionTotalAmountContent = By.XPath("//div[contains(text(),'Payment')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Total amount')]/parent::div/following-sibling::div");
        private By requisitionTotalAmountInput = By.Id("input-payee.totalAmount");

        private By requisitionFinancialActivitiesSubtitle = By.XPath("//div[contains(text(),'Financial Activities')]");
        private By requisitionActivitiesSubtitle = By.XPath("//div[contains(text(),'Activities')]");
        private By requisitionAddActivityBttn = By.CssSelector("button[data-testid='add-financial-activity']");

        private By requisitionDetailRemarksLabel = By.XPath("//label[contains(text(),'Detailed remarks')]");
        private By requisitionDetailRemarksContent = By.XPath("//label[contains(text(),'Detailed remarks')]/parent::div/following-sibling::div");
        private By requisitionDetailRemarksTextarea = By.Id("input-detailedRemarks");

        private By requisitionFooterCompensationAmountLabel = By.XPath("//div[contains(text(),'Financial Activities')]/parent::div/parent::h2/parent::div/following-sibling::div/div/div/div/div/label[contains(text(),'Compensation amount')]");
        private By requisitionFooterCompensationAmountContent = By.XPath("//div[contains(text(),'Financial Activities')]/parent::div/parent::h2/parent::div/following-sibling::div/div/div/div/div/label[contains(text(),'Compensation amount')]/parent::div/following-sibling::div");
        private By requisitionFooterApplicableGSTLabel = By.XPath("//div[contains(text(),'Financial Activities')]/parent::div/parent::h2/parent::div/following-sibling::div/div/div/div/div/label[contains(text(),'Applicable GST')]");
        private By requisitionFooterApplicableGSTContent = By.XPath("//div[contains(text(),'Financial Activities')]/parent::div/parent::h2/parent::div/following-sibling::div/div/div/div/div/label[contains(text(),'Applicable GST')]/parent::div/following-sibling::div");
        private By requisitionFooterTotalChequeAmountLabel = By.XPath("//div[contains(text(),'Financial Activities')]/parent::div/parent::h2/parent::div/following-sibling::div/div/div/div/div/label[contains(text(),'Total cheque amount')]");
        private By requisitionFooterTotalChequeAmountContent = By.XPath("//div[contains(text(),'Financial Activities')]/parent::div/parent::h2/parent::div/following-sibling::div/div/div/div/div/label[contains(text(),'Total cheque amount')]/parent::div/following-sibling::div");

        //Activities Count
        private By requisitionFinancialActivitiesCount = By.XPath("//div[contains(text(),'Activities')]/parent::div/parent::h2/following-sibling::div/div/div/label");

        //Compensation Details Elements
        private By requisitionCancelBttn = By.XPath("//button[@id='close-tray']/parent::div/following-sibling::div/div/div/div/div/div/button/div[contains(text(),'Cancel')]");

        //Acquisition File Confirmation Modal Elements
        private By acquisitionFileConfirmationModal = By.CssSelector("div[class='modal-content']");

        private SharedModals sharedModals;

        public AcquisitionCompensations(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
        }

        public void NavigateCompensationTab()
        {
            WaitUntilClickable(compensationLinkTab);
            webDriver.FindElement(compensationLinkTab).Click();
        }

        public void UpdateTotalAllowableCompensation(string allowableAmount)
        {
            Wait();
            FocusAndClick(compensationTotalAllowableEdiBttn);

            WaitUntilVisible(compensationTotalAllowableInput);
            ClearInput(compensationTotalAllowableInput);
            webDriver.FindElement(compensationTotalAllowableInput).SendKeys(allowableAmount);

            FocusAndClick(compensationTotalAllowableSaveBttn);
        }

        public void AddCompensationBttn()
        {
            WaitUntilClickable(compensationAddBttn);
            webDriver.FindElement(compensationAddBttn).Click();

            WaitUntilSpinnerDisappear();
        }

        public void OpenCompensationDetails(int index)
        {
            Wait();

            WaitUntilClickable(By.CssSelector("div[data-testid='AcquisitionCompensationTable'] div[class='tbody'] div[class='tr-wrapper'] div button div svg[data-testid='compensation-view-"+ index +"']"));
            webDriver.FindElement(By.CssSelector("div[data-testid='AcquisitionCompensationTable'] div[class='tbody'] div[class='tr-wrapper'] div button div svg[data-testid='compensation-view-"+ index +"']")).Click();
        }

        public void DeleteCompensationRequisition(int index)
        {
            Wait();
            //WaitUntilVisible(compensationH120Table);
            FocusAndClick(By.CssSelector("div[data-testid='AcquisitionCompensationTable'] div[class='tbody'] div[class='tr-wrapper'] div button[data-testid='compensation-delete-"+ index +"']"));

            if (webDriver.FindElements(acquisitionFileConfirmationModal).Count() > 0)
            {
                Assert.Equal("Confirm Delete", sharedModals.ModalHeader());
                Assert.Equal("Are you sure you want to delete this item?", sharedModals.ModalContent());
                sharedModals.ModalClickOKBttn();
            }

            WaitUntilSpinnerDisappear();
        }

        public void SaveAcquisitionFileCompensation()
        {
            Wait();
            ButtonElement("Save");

            if (webDriver.FindElements(acquisitionFileConfirmationModal).Count() > 0)
            {
                Assert.Equal("Confirm status change", sharedModals.ModalHeader());
                Assert.Contains("You have selected to change the status from DRAFT to FINAL.", sharedModals.ModalContent());
                Assert.Contains("We recommend that you only make this change status (draft to final) when printing the final version", sharedModals.ModalContent());
                Assert.Contains("without system administrator privileges. The compensation requisition cannot be changed again once it is saved as final.", sharedModals.ModalContent());
                
                sharedModals.ModalClickOKBttn();
            }

            AssertTrueIsDisplayed(requisitionEditBttn);
        }

        public void CancelAcquisitionFileCompensation()
        {
            Wait();
            FocusAndClick(requisitionCancelBttn);

            if (webDriver.FindElements(acquisitionFileConfirmationModal).Count() > 0)
            {
                Assert.Equal("Unsaved Changes", sharedModals.ModalHeader());
                Assert.Equal("You have made changes on this form. Do you wish to leave without saving?", sharedModals.ModalContent());
                sharedModals.ModalClickOKBttn();
            }

            AssertTrueIsDisplayed(requisitionEditBttn);
        }

        public string GetCompensationFileNumber(int compFileNbr)
        {
            Wait();
            WaitUntilVisible(By.CssSelector("div[data-testid='AcquisitionCompensationTable'] div[class='tr-wrapper']:nth-child("+ compFileNbr +") div:nth-child(2) div"));

            return webDriver.FindElement(By.CssSelector("div[data-testid='AcquisitionCompensationTable'] div[class='tr-wrapper']:nth-child("+ compFileNbr +") div:nth-child(2) div")).Text;
        }

        public void DeleteFirstActivity()
        {
            webDriver.FindElement(By.XPath("//div[@data-testid='finacialActivity[0]']/div/button[@title='Delete financial activity']")).Click();

            if (webDriver.FindElements(acquisitionFileConfirmationModal).Count() > 0)
            {
                Assert.Equal("Remove financial activity", sharedModals.ModalHeader());
                Assert.Equal("Are you sure you want to remove this financial activity?", sharedModals.ModalContent());
                sharedModals.ModalClickOKBttn();
            }
        }

        public void EditCompensationDetails()
        {
            WaitUntilVisible(requisitionEditBttn);
            FocusAndClick(requisitionEditBttn);
        }

        public void UpdateCompensationDetails(AcquisitionCompensation compensation)
        {
            //Requisition Details
            if(compensation.CompensationStatus != "")
                ChooseSpecificSelectOption(requisitionStatusSelect, compensation.CompensationStatus);

            if (compensation.CompensationAlternateProject != "")
            {
                ClearInput(requisitionAltProjectInput);
                webDriver.FindElement(requisitionAltProjectInput).SendKeys(compensation.CompensationAlternateProject);

                Wait();
                webDriver.FindElement(requisitionAltProjectInput).SendKeys(Keys.Space);

                Wait();
                webDriver.FindElement(requisitionAltProjectInput).SendKeys(Keys.Backspace);

                Wait();
                webDriver.FindElement(requisitionAltProject1stOption).Click();
            }

            if (compensation.CompensationAgreementDate != "")
            {
                ClearInput(requisitionAgreementInput);
                webDriver.FindElement(requisitionAgreementInput).SendKeys(compensation.CompensationAgreementDate);
                webDriver.FindElement(requisitionAgreementInput).SendKeys(Keys.Enter);
            }

            if (compensation.CompensationExpropriationNoticeDate != "")
            {
                ClearInput(requisitionExpropriationServedInput);
                webDriver.FindElement(requisitionExpropriationServedInput).SendKeys(compensation.CompensationExpropriationNoticeDate);
                webDriver.FindElement(requisitionExpropriationServedInput).SendKeys(Keys.Enter);
            }

            if (compensation.CompensationExpropriationVestingDate != "")
            {
                ClearInput(requisitionExpropriationVestingInput);
                webDriver.FindElement(requisitionExpropriationVestingInput).SendKeys(compensation.CompensationExpropriationVestingDate);
                webDriver.FindElement(requisitionExpropriationVestingInput).SendKeys(Keys.Enter);
            }

            if (compensation.CompensationAdvancedPaymentDate != "")
            {
                ClearInput(requisitionAdvancePaymentInput);
                webDriver.FindElement(requisitionAdvancePaymentInput).SendKeys(compensation.CompensationAdvancedPaymentDate);
                webDriver.FindElement(requisitionAdvancePaymentInput).SendKeys(Keys.Enter);
            }

            if (compensation.CompensationSpecialInstructions != "")
            {
                ClearInput(requisitionSpecialInstructionTextarea);
                webDriver.FindElement(requisitionSpecialInstructionTextarea).SendKeys(compensation.CompensationSpecialInstructions);
            }

            //Financial Coding
            if (compensation.CompensationFiscalYear != "")
                ChooseSpecificSelectOption(requisitionFiscalYearSelect, compensation.CompensationFiscalYear);

            if (compensation.CompensationSTOB != "")
            {
                webDriver.FindElement(requisitionSTOBInput).SendKeys(compensation.CompensationSTOB);
                WaitUntilVisible(requisitionSTOBOptions);
                FocusAndClick(requisitionSTOB1stOption);
            }

            if (compensation.CompensationServiceLine != "")
            {
                webDriver.FindElement(requisitionServiceLineInput).SendKeys(compensation.CompensationServiceLine);
                WaitUntilVisible(requisitionServiceLineOptions);
                FocusAndClick(requisitionServiceLine1stOption);
            }
                

            if (compensation.CompensationResponsibilityCentre != "")
            {
                webDriver.FindElement(requisitionResponsibilityCentreInput).SendKeys(compensation.CompensationResponsibilityCentre);
                WaitUntilVisible(requisitionResponsibilityCentreOptions);
                FocusAndClick(requisitionResponsibilityCentre1stOption);
            }

            //Payment
            if (compensation.CompensationPayee != "")
                ChooseSpecificSelectOption(requisitionPayeeSelect, compensation.CompensationPayee);

            if (compensation.CompensationPaymentInTrust)
                FocusAndClick(requisitionPaymentInTrustCheckbox);

            if (compensation.CompensationGSTNumber!= "")
            {
                ClearInput(requisitionGSTNumberInput);
                webDriver.FindElement(requisitionGSTNumberInput).SendKeys(compensation.CompensationGSTNumber);
            }

            //Financial Activities
            if (compensation.CompensationActivities.Count > 0)
            {
                for (int i = 0; i < compensation.CompensationActivities.Count; i++)
                    CreateFinancialActivity(compensation.CompensationActivities[i]);
            }

            //Detailed Remarks
            if (compensation.CompensationDetailedRemarks!= "")
            {
                ClearInput(requisitionDetailRemarksTextarea);
                webDriver.FindElement(requisitionDetailRemarksTextarea).SendKeys(compensation.CompensationDetailedRemarks);
            }
        }

        public void VerifyCompensationInitTabView()
        {
            AssertTrueIsDisplayed(compensationAddSubtitle);
            AssertTrueIsDisplayed(compensationAddBttn);

            AssertTrueIsDisplayed(compensationTotalAllowableLabel);
            AssertTrueIsDisplayed(compensationTotalAllowableContent);
            AssertTrueIsDisplayed(compensationTotalAllowableEdiBttn);

            AssertTrueIsDisplayed(compensationTotalPaymentsMadeLabel);
            AssertTrueIsDisplayed(compensationTotalPaymentsMadeContent);

            AssertTrueIsDisplayed(compensationDraftsTotalLabel);
            AssertTrueIsDisplayed(compensationDraftsTotalContent);
            AssertTrueIsDisplayed(compensationAddCompensationTooltips);

            AssertTrueIsDisplayed(compensationH120Subtitle);
            AssertTrueIsDisplayed(compensationH120Table);
            AssertTrueIsDisplayed(compensationH120NoRowsMessage);
        }

        public int TotalActivitiesCount()
        {
            Wait();
            return webDriver.FindElements(requisitionFinancialActivitiesCount).Count();
        }

        public int TotalCompensationCount()
        {
            Wait();
            return webDriver.FindElements(compensationH120TotalCount).Count();
        }

        public void VerifyCompensationDetailsInitViewForm()
        {
            //Title
            AssertTrueIsDisplayed(compensationDetailsTitle);

            //Header
            AssertTrueIsDisplayed(compensationClientLabel);
            AssertTrueIsDisplayed(compensationClientContent);
            AssertTrueIsDisplayed(compensationRequisitionNumberLabel);
            AssertTrueIsDisplayed(compensationRequisitionNumberContent);
            AssertTrueIsDisplayed(compensationAmountLabel);
            AssertTrueIsDisplayed(compensationAmountContent);
            AssertTrueIsDisplayed(compensationGSTLabel);
            AssertTrueIsDisplayed(compensationGSTContent);
            AssertTrueIsDisplayed(compensationTotalChequeAmountLabel);
            AssertTrueIsDisplayed(compensationTotalChequeAmountContent);

            //Requisition Details
            AssertTrueIsDisplayed(requisitionDetailsViewSubtitle);
            AssertTrueIsDisplayed(requisitionGenerateH120Bttn);
            AssertTrueIsDisplayed(requisitionEditBttn);
            AssertTrueIsDisplayed(requisitionStatusLabel);
            AssertTrueIsDisplayed(requisitionAltProjectLabel);
            AssertTrueContentEquals(requisitionStatusContent, "Draft");
            AssertTrueIsDisplayed(requisitionFinalDateLabel);
            AssertTrueIsDisplayed(requisitionAgreementLabel);
            AssertTrueIsDisplayed(requsitionExpropriationServedLabel);
            AssertTrueIsDisplayed(requisitionExpropriationVestingLabel);
            AssertTrueIsDisplayed(requisitionAdvancePaymentLabel);
            AssertTrueIsDisplayed(requisitionSpecialInstructionLabel);

            //Financial Coding
            AssertTrueIsDisplayed(requisitionFinancialCodingSubtitle);
            AssertTrueIsDisplayed(requisitionProductLabel);
            AssertTrueIsDisplayed(requisitionBusinessLabel);
            AssertTrueIsDisplayed(requisitionWorkActivityLabel);
            AssertTrueIsDisplayed(requisitionCostTypeLabel);
            AssertTrueIsDisplayed(requisitionFiscalYearLabel);
            AssertTrueIsDisplayed(requisitionSTOBLabel);
            AssertTrueIsDisplayed(requisitionServiceLineLabel);
            AssertTrueIsDisplayed(requisitionResponsibilityCentreLabel);

            //Payment
            AssertTrueIsDisplayed(requisitionPaymentSubtitle);
            AssertTrueIsDisplayed(requisitionPayeeLabel);
            AssertTrueIsDisplayed(requisitionAmountLabel);
            AssertTrueIsDisplayed(requisitionAmountContent);
            AssertTrueIsDisplayed(requisitionGSTApplicableLabel);
            AssertTrueIsDisplayed(requisitionGSTApplicableContent);
            AssertTrueIsDisplayed(requisitionTotalAmountLabel);
            AssertTrueIsDisplayed(requisitionTotalAmountContent);

            //Financial Activities
            AssertTrueIsDisplayed(requisitionFinancialActivitiesSubtitle);
            AssertTrueIsDisplayed(requisitionDetailRemarksLabel);

            //Footer
            AssertTrueIsDisplayed(requisitionFooterCompensationAmountLabel);
            AssertTrueIsDisplayed(requisitionFooterCompensationAmountContent);
            AssertTrueIsDisplayed(requisitionFooterApplicableGSTLabel);
            AssertTrueIsDisplayed(requisitionFooterApplicableGSTContent);
            AssertTrueIsDisplayed(requisitionFooterTotalChequeAmountLabel);
            AssertTrueIsDisplayed(requisitionFooterTotalChequeAmountContent);
        }

        public void VerifyCompensationDetailsInitCreateForm()
        {
            //Requisition Details
            AssertTrueIsDisplayed(requisitionDetailsCreateSubtitle);
            
            AssertTrueIsDisplayed(requisitionStatusLabel);
            AssertTrueIsDisplayed(requisitionStatusSelect);
            AssertTrueIsDisplayed(requisitionAltProjectLabel);
            AssertTrueIsDisplayed(requisitionAltProjectInput);
            AssertTrueIsDisplayed(requisitionFinalDateLabel);
            AssertTrueIsDisplayed(requisitionAgreementLabel);
            AssertTrueIsDisplayed(requisitionAgreementInput);
            AssertTrueIsDisplayed(requsitionExpropriationServedLabel);
            AssertTrueIsDisplayed(requisitionExpropriationServedInput);
            AssertTrueIsDisplayed(requisitionExpropriationVestingLabel);
            AssertTrueIsDisplayed(requisitionExpropriationVestingInput);
            AssertTrueIsDisplayed(requisitionAdvancePaymentLabel);
            AssertTrueIsDisplayed(requisitionAdvancePaymentInput);
            AssertTrueIsDisplayed(requisitionSpecialInstructionLabel);
            AssertTrueIsDisplayed(requisitionSpecialInstructionTextarea);

            //Financial Coding
            AssertTrueIsDisplayed(requisitionFinancialCodingSubtitle);
            AssertTrueIsDisplayed(requisitionProductLabel);
            AssertTrueIsDisplayed(requisitionBusinessLabel);
            AssertTrueIsDisplayed(requisitionWorkActivityLabel);
            AssertTrueIsDisplayed(requisitionCostTypeLabel);
            AssertTrueIsDisplayed(requisitionFiscalYearLabel);
            AssertTrueIsDisplayed(requisitionFiscalYearSelect);
            AssertTrueIsDisplayed(requisitionSTOBLabel);
            AssertTrueIsDisplayed(requisitionSTOBInput);
            AssertTrueIsDisplayed(requisitionServiceLineLabel);
            AssertTrueIsDisplayed(requisitionServiceLineInput);
            AssertTrueIsDisplayed(requisitionResponsibilityCentreLabel);
            AssertTrueIsDisplayed(requisitionResponsibilityCentreInput);

            //Payment
            AssertTrueIsDisplayed(requisitionPaymentSubtitle);
            AssertTrueIsDisplayed(requisitionPayeeLabel);
            AssertTrueIsDisplayed(requisitionPayeeSelect);
            AssertTrueIsDisplayed(requisitionPaymentInTrustLabel);
            AssertTrueIsDisplayed(requisitionPaymentInTrustCheckbox);
            AssertTrueIsDisplayed(requisitionGSTNumberLabel);
            AssertTrueIsDisplayed(requisitionGSTNumberInput);
            AssertTrueIsDisplayed(requisitionAmountLabel);
            AssertTrueIsDisplayed(requisitionAmountInput);
            AssertTrueIsDisplayed(requisitionGSTAmountLabel);
            AssertTrueIsDisplayed(requisitionGSTAmountInput);
            AssertTrueIsDisplayed(requisitionTotalAmountLabel);
            AssertTrueIsDisplayed(requisitionTotalAmountInput);

            //Financial Activities
            AssertTrueIsDisplayed(requisitionActivitiesSubtitle);
            AssertTrueIsDisplayed(requisitionAddActivityBttn);

            //Detailed Remarks
            AssertTrueIsDisplayed(requisitionDetailRemarksLabel);
            AssertTrueIsDisplayed(requisitionDetailRemarksTextarea);
            
        }

        public void VerifyCompensationDetailsViewForm(AcquisitionCompensation compensation)
        {
            Wait();

            //Title
            AssertTrueIsDisplayed(compensationDetailsTitle);

            //Header
            AssertTrueIsDisplayed(compensationClientLabel);
            AssertTrueContentEquals(compensationClientContent, "034");
            AssertTrueIsDisplayed(compensationRequisitionNumberLabel);
            AssertTrueIsDisplayed(compensationAmountLabel);
            AssertTrueContentEquals(compensationAmountContent, TransformCurrencyFormat(compensation.CompensationAmount));
            AssertTrueIsDisplayed(compensationGSTLabel);
            AssertTrueContentEquals(compensationGSTContent, TransformCurrencyFormat(compensation.CompensationGSTAmount));
            AssertTrueIsDisplayed(compensationTotalChequeAmountLabel);
            AssertTrueContentEquals(compensationTotalChequeAmountContent, TransformCurrencyFormat(compensation.CompensationTotalAmount));

            //Requisition Details
            AssertTrueIsDisplayed(requisitionDetailsViewSubtitle);
            AssertTrueIsDisplayed(requisitionGenerateH120Bttn);
            AssertTrueIsDisplayed(requisitionEditBttn);
            AssertTrueIsDisplayed(requisitionStatusLabel);
            AssertTrueContentEquals(requisitionStatusContent, compensation.CompensationStatus);
            AssertTrueIsDisplayed(requisitionAltProjectLabel);
            AssertTrueContentEquals(requisitionAltProjectContent, TransformProjectFormat(compensation.CompensationAlternateProject));
            AssertTrueIsDisplayed(requisitionFinalDateLabel);
            AssertTrueIsDisplayed(requisitionAgreementLabel);
            AssertTrueContentEquals(requisitionAgreementContent, TransformDateFormat(compensation.CompensationAgreementDate));
            AssertTrueIsDisplayed(requsitionExpropriationServedLabel);
            AssertTrueContentEquals(requsitionExpropriationServedContent, TransformDateFormat(compensation.CompensationExpropriationNoticeDate));
            AssertTrueIsDisplayed(requisitionExpropriationVestingLabel);
            AssertTrueContentEquals(requisitionExpropriationVestingContent, TransformDateFormat(compensation.CompensationExpropriationVestingDate));
            AssertTrueIsDisplayed(requisitionSpecialInstructionLabel);
            AssertTrueContentEquals(requisitionSpecialInstructionContent, compensation.CompensationSpecialInstructions);

            //Financial Coding
            AssertTrueIsDisplayed(requisitionFinancialCodingSubtitle);
            AssertTrueIsDisplayed(requisitionProductLabel);
            AssertTrueContentNotEquals(requisitionProductContent, "");
            AssertTrueIsDisplayed(requisitionBusinessLabel);
            AssertTrueContentNotEquals(requisitionBusinessContent, "");
            AssertTrueIsDisplayed(requisitionWorkActivityLabel);
            AssertTrueContentNotEquals(requisitionWorkActivityContent, "");
            AssertTrueIsDisplayed(requisitionCostTypeLabel);
            AssertTrueContentNotEquals(requisitionCostTypeContent, "");
            AssertTrueIsDisplayed(requisitionFiscalYearLabel);
            AssertTrueContentEquals(requisitionFiscalYearContent, compensation.CompensationFiscalYear);
            AssertTrueIsDisplayed(requisitionSTOBLabel);
            AssertTrueContentEquals(requisitionSTOBContent, compensation.CompensationSTOB);
            AssertTrueIsDisplayed(requisitionServiceLineLabel);
            AssertTrueContentEquals(requisitionServiceLineContent, compensation.CompensationServiceLine);
            AssertTrueIsDisplayed(requisitionResponsibilityCentreLabel);
            AssertTrueContentEquals(requisitionResponsibilityCentreContent, compensation.CompensationResponsibilityCentre);

            //Payment
            AssertTrueIsDisplayed(requisitionPaymentSubtitle);

            //Payee
            AssertTrueIsDisplayed(requisitionPayeeLabel);

            if (webDriver.FindElements(requisitionPayeeContentLink).Count > 0)
                AssertTrueContentEquals(requisitionPayeeContentLink, compensation.CompensationPayeeDisplay);

            else if(webDriver.FindElements(requisitionPayeeContent).Count > 0)
                AssertTrueElementContains(requisitionPayeeContent, compensation.CompensationPayeeDisplay);

            if (compensation.CompensationPaymentInTrust)
            {
                if(webDriver.FindElements(requisitionPaymentInTrustContent2).Count > 0)
                    AssertTrueContentEquals(requisitionPaymentInTrustContent2, ", in trust");
                else if(webDriver.FindElements(requisitionPaymentInTrustContent1).Count > 0)
                    AssertTrueContentEquals(requisitionPaymentInTrustContent1, ", in trust");
            }
                
            AssertTrueIsDisplayed(requisitionAmountLabel);
            AssertTrueContentEquals(requisitionAmountContent, TransformCurrencyFormat(compensation.CompensationAmount));

            AssertTrueIsDisplayed(requisitionGSTApplicableLabel);
            if (compensation.CompensationGSTAmount != "")
            {
                AssertTrueContentEquals(requisitionGSTApplicableContent, "Yes");

                AssertTrueIsDisplayed(requisitionGSTAmountLabel);
                AssertTrueContentEquals(requisitionGSTAmountContent, TransformCurrencyFormat(compensation.CompensationGSTAmount));
            }
            else
            {
                AssertTrueContentEquals(requisitionGSTApplicableContent, "No");
            }

            AssertTrueIsDisplayed(requisitionTotalAmountLabel);
            AssertTrueContentEquals(requisitionTotalAmountContent, TransformCurrencyFormat(compensation.CompensationTotalAmount));

            //Financial Activities
            AssertTrueIsDisplayed(requisitionFinancialActivitiesSubtitle);
            if (compensation.CompensationActivities.Count > 0)
            {
                for (int i = 0; i < compensation.CompensationActivities.Count; i++)
                    VerifyFinancialActivity(compensation.CompensationActivities[i], i);
            }

            //Details Remarks
            AssertTrueIsDisplayed(requisitionDetailRemarksLabel);
            AssertTrueContentEquals(requisitionDetailRemarksContent, compensation.CompensationDetailedRemarks);

            //Footer
            AssertTrueIsDisplayed(requisitionFooterCompensationAmountLabel);
            AssertTrueContentEquals(requisitionFooterCompensationAmountContent, TransformCurrencyFormat(compensation.CompensationAmount));
            AssertTrueIsDisplayed(requisitionFooterApplicableGSTLabel);
            AssertTrueContentEquals(requisitionFooterApplicableGSTContent, TransformCurrencyFormat(compensation.CompensationGSTAmount));
            AssertTrueIsDisplayed(requisitionFooterTotalChequeAmountLabel);
            AssertTrueContentEquals(requisitionFooterTotalChequeAmountContent, TransformCurrencyFormat(compensation.CompensationTotalAmount));
        }

        public void VerifyCompensationListView(AcquisitionCompensation compensation)
        {
            var lastCreatedCompensationReq = webDriver.FindElements(compensationH120TotalCount).Count;
            var elementIndex = lastCreatedCompensationReq -1;

            AssertTrueContentEquals(By.CssSelector("div[data-testid='AcquisitionCompensationTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+lastCreatedCompensationReq+")  div:nth-child(3)"), TransformCurrencyFormat(compensation.CompensationTotalAmount));
            AssertTrueContentEquals(By.CssSelector("div[data-testid='AcquisitionCompensationTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+lastCreatedCompensationReq+")  div:nth-child(4)"), compensation.CompensationStatus);
            AssertTrueIsDisplayed(By.CssSelector("button svg[data-testid='compensation-view-"+ elementIndex +"']"));
            AssertTrueIsDisplayed(By.CssSelector("button[data-testid='compensation-delete-"+ elementIndex +"']"));
        }

        private void CreateFinancialActivity(CompensationActivity activity)
        {
            Wait();

            //Get index by counting number of activities
            var index = webDriver.FindElements(requisitionFinancialActivitiesCount).Count;

            //Click to add a new activity form
            webDriver.FindElement(requisitionAddActivityBttn).Click();

            //Verify Form
            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='finacialActivity["+ index +"]']/div/label[contains(text(),'Activity')]"));
            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='finacialActivity["+ index +"]']/div/button[@title='Delete financial activity']"));

            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='finacialActivity["+ index +"]']/div/div/label[contains(text(),'Code & Description')]"));
            AssertTrueIsDisplayed(By.Id("typeahead-select-financials."+ index +".financialActivityCodeId"));

            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='finacialActivity["+ index +"]']/div/div/label[contains(text(),'Amount (before tax)')]"));
            AssertTrueIsDisplayed(By.Id("input-financials["+ index +"].pretaxAmount"));

            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='finacialActivity["+ index +"]']/div/div/label[contains(text(),'GST applicable?')]"));
            AssertTrueIsDisplayed(By.Id("input-financials["+ index +"].isGstRequired"));

            AssertTrueIsDisplayed(By.XPath("//div[@data-testid='finacialActivity["+ index +"]']/div/div/label[contains(text(),'Total amount')]"));
            AssertTrueIsDisplayed(By.Id("input-financials["+ index +"].totalAmount"));

            //Create a new Activity
            if (activity.ActCodeDescription != "")
            {
                webDriver.FindElement(By.Id("typeahead-select-financials."+ index +".financialActivityCodeId")).SendKeys(activity.ActCodeDescription);
                WaitUntilVisible(By.CssSelector("div[id='typeahead-select-financials."+ index +".financialActivityCodeId']"));
                webDriver.FindElement(By.CssSelector("div[id='typeahead-select-financials."+ index +".financialActivityCodeId'] a:nth-child(1)")).Click();
            }

            if (activity.ActAmount != "")
            {
                ClearInput(By.Id("input-financials["+ index +"].pretaxAmount"));
                webDriver.FindElement(By.Id("input-financials["+ index +"].pretaxAmount")).SendKeys(activity.ActAmount);
            }

            if (activity.ActGSTEligible!= "")
                ChooseSpecificSelectOption(By.Id("input-financials["+ index +"].isGstRequired"), activity.ActGSTEligible);

            if (activity.ActGSTAmount != "")
            {
                AssertTrueIsDisplayed(By.XPath("//div[@data-testid='finacialActivity["+ index +"]']/div/div/label[contains(text(),'GST amount')]"));
                AssertTrueElementValueEquals(By.Id("input-financials["+ index +"].taxAmount"),TransformCurrencyFormat(activity.ActGSTAmount));
            }

            if (activity.ActTotalAmount != "")
                AssertTrueElementValueEquals(By.Id("input-financials["+ index +"].totalAmount"),TransformCurrencyFormat(activity.ActTotalAmount));
        }

        private void VerifyFinancialActivity(CompensationActivity activity, int index)
        {
            var elementNbr = index + 1;
            AssertTrueIsDisplayed(By.XPath("(//div[contains(text(),'Financial Activities')]/parent::div/parent::h2/following-sibling::div/div/label[contains(text(),'Activity')]/parent::div/following-sibling::div/div/label[contains(text(),'Code & Description')])["+ elementNbr +"]"));
            AssertTrueContentEquals(By.XPath("(//div[contains(text(),'Financial Activities')]/parent::div/parent::h2/following-sibling::div/div/label[contains(text(),'Activity')]/parent::div/following-sibling::div/div/label[contains(text(),'Code & Description')])["+ elementNbr +"]/parent::div/following-sibling::div"), activity.ActCodeDescription);

            AssertTrueIsDisplayed(By.XPath("(//div[contains(text(),'Financial Activities')]/parent::div/parent::h2/following-sibling::div/div/label[contains(text(),'Activity')]/parent::div/following-sibling::div/div/label[contains(text(),'Amount (before tax)')])["+ elementNbr +"]"));
            AssertTrueContentEquals(By.XPath("(//div[contains(text(),'Financial Activities')]/parent::div/parent::h2/following-sibling::div/div/label[contains(text(),'Activity')]/parent::div/following-sibling::div/div/label[contains(text(),'Amount (before tax)')])["+ elementNbr +"]/parent::div/following-sibling::div"),TransformCurrencyFormat(activity.ActAmount));

            AssertTrueIsDisplayed(By.XPath("(//div[contains(text(),'Financial Activities')]/parent::div/parent::h2/following-sibling::div/div/label[contains(text(),'Activity')]/parent::div/following-sibling::div/div/label[contains(text(),'GST applicable')])["+ elementNbr +"]"));
            AssertTrueContentEquals(By.XPath("(//div[contains(text(),'Financial Activities')]/parent::div/parent::h2/following-sibling::div/div/label[contains(text(),'Activity')]/parent::div/following-sibling::div/div/label[contains(text(),'GST applicable')])["+ elementNbr +"]/parent::div/following-sibling::div"), activity.ActGSTEligible);

            if (activity.ActGSTAmount != "")
            {
                AssertTrueIsDisplayed(By.XPath("(//div[contains(text(),'Financial Activities')]/parent::div/parent::h2/following-sibling::div/div/label[contains(text(),'Activity')]/parent::div/following-sibling::div/div/label[contains(text(),'GST amount')])["+ elementNbr +"]"));
                AssertTrueContentEquals(By.XPath("(//div[contains(text(),'Financial Activities')]/parent::div/parent::h2/following-sibling::div/div/label[contains(text(),'Activity')]/parent::div/following-sibling::div/div/label[contains(text(),'GST amount')])["+ elementNbr +"]/parent::div/following-sibling::div"), TransformCurrencyFormat(activity.ActGSTAmount));
            }
            
            AssertTrueIsDisplayed(By.XPath("(//div[contains(text(),'Financial Activities')]/parent::div/parent::h2/following-sibling::div/div/label[contains(text(),'Activity')]/parent::div/following-sibling::div/div/label[contains(text(),'Total amount')])["+ elementNbr +"]"));
            AssertTrueContentEquals(By.XPath("(//div[contains(text(),'Financial Activities')]/parent::div/parent::h2/following-sibling::div/div/label[contains(text(),'Activity')]/parent::div/following-sibling::div/div/label[contains(text(),'Total amount')])["+ elementNbr +"]/parent::div/following-sibling::div"), TransformCurrencyFormat(activity.ActTotalAmount));
        }
    }
}
