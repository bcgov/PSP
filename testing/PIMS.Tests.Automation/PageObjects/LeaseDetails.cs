using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PIMS.Tests.Automation.Classes;
using Sprache;

namespace PIMS.Tests.Automation.PageObjects
{
    public class LeaseDetails : PageObjectBase
    {
        //Main Menu links Elements
        private readonly By menuManagementButton = By.CssSelector("div[data-testid='nav-tooltip-leases&licences'] a");
        private readonly By createLicenseButton = By.XPath("//a[contains(text(),'Create a Lease/Licence File')]");

        //File Details Edit Icon
        private readonly By licenseDetailsEditIcon = By.XPath("//div[@role='tabpanel']/div/div/div/button");

        //Lease Header Elements
        private readonly By licenseHeaderNbrLabel = By.XPath("//label[contains(text(),'Lease/Licence #')]");
        private readonly By licenseHeaderNbrContent = By.XPath("//label[contains(text(),'Lease/Licence #')]/parent::div/following-sibling::div/span[1]");
        private readonly By licenseHeaderAccountType = By.XPath("//label[contains(text(),'Lease/Licence #')]/parent::div/following-sibling::div/span[2]");
        private readonly By licenseHeaderProperty = By.XPath("//label[contains(text(),'Property')]");
        private readonly By licenseHeaderPropertyContent = By.XPath("//label[contains(text(),'Property')]/parent::div/following-sibling::div/div/span");
        private readonly By licenseHeaderTenantLabel = By.XPath("//label[contains(text(),'Tenant')]");
        private readonly By licenseHeaderStartDateLabel = By.XPath("//h1/parent::div/parent::div/following-sibling::div[2]/div/div/div/div/div[4]/div/label[contains(text(),'Commencement')]");
        private readonly By licenseHeaderStartDateContent = By.XPath("//label[contains(text(),'Commencement')]/parent::div/following-sibling::div[1]");
        private readonly By licenseHeaderExpiryDateLabel = By.XPath("//label[contains(text(),'Commencement')]/parent::div/following-sibling::div[2]/label[contains(text(),'Expiry')]");
        private readonly By licenseHeaderExpiryDateContent = By.XPath("//label[contains(text(),'Commencement')]/parent::div/following-sibling::div[2]/label[contains(text(),'Expiry')]/parent::div/following-sibling::div[1]");
        private readonly By licenseHeaderHistoricalFileLabel = By.XPath("//label[contains(text(),'Historical file')]");
        private readonly By licenseHeaderHistoricalFileContent = By.XPath("//label[contains(text(),'Historical file #:')]/parent::div/following-sibling::div/div/span");

        private readonly By licenseHeaderCreatedLabel = By.XPath("//span/strong[contains(text(),'Created')]");
        private readonly By licenseHeaderCreatedContent = By.XPath("//strong[contains(text(),'Created')]/parent::span");
        private readonly By licenseHeaderCreatedByContent = By.XPath("//strong[contains(text(),'Created')]/parent::span/span[@data-testid='tooltip-icon-userNameTooltip']");
        private readonly By licenseHeaderLastUpdatedLabel = By.XPath("//span/strong[contains(text(),'Updated')]");
        private readonly By licenseHeaderLastUpdatedContent = By.XPath("//strong[contains(text(),'Updated')]/parent::span");
        private readonly By licenseHeaderLastUpdatedByContent = By.XPath("//strong[contains(text(),'Updated')]/parent::span/span[@data-testid='tooltip-icon-userNameTooltip']");
        private readonly By licenseHeaderStatusContent = By.XPath("//b[contains(text(),'File')]/parent::span/following-sibling::div");
        private readonly By licenseHeaderExpiredFlag = By.XPath("//label[contains(text(),'Commencement')]/parent::div/following-sibling::div[4]/div");

        //Create/View Lease Details Elements
        private readonly By licenseCreateTitle = By.XPath("//h1[contains(text(),'Create Lease/Licence')]");

        private readonly By licenseDetailsViewSubtitle = By.XPath("//h2/div/div[contains(text(), 'Details')]");
        private readonly By licenseDetailsCreateSubtitle = By.XPath("//h2/div/div[contains(text(),'Original Agreement')]");

        private readonly By licenseDetailsProjectLabel = By.XPath("//label[contains(text(),'Ministry project')]");
        private readonly By licenseDetailsProjectContent = By.XPath("//label[contains(text(),'Ministry project')]/parent::div/following-sibling::div");
        private readonly By licenseDetailsProjectInput = By.CssSelector("input[id='typeahead-project']");
        private readonly By licenseDetailsProject1stOption = By.CssSelector("div[id='typeahead-project'] a:nth-child(1)");

        private readonly By licenseDetailsStatusLabel = By.XPath("//div/div/div/div/label[contains(text(),'Status')]");
        private readonly By licenseDetailsStatusSelector = By.Id("input-statusTypeCode");
        private readonly By licenseDetailsStatusContent = By.XPath("//label[contains(text(),'Status')]/parent::div/following-sibling::div");

        private readonly By licenseDetailsAccountTypeLabel = By.XPath("//label[contains(text(),'Account type')]");
        private readonly By licenseDetailsAccountTypeSelector = By.Id("input-paymentReceivableTypeCode");
        private readonly By licenseDetailsAccountTypeContent = By.XPath("//div[contains(text(),'Details')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Account type')]/parent::div/following-sibling::div");

        private readonly By licenseDetailsCommencementDateLabel = By.XPath("//h2/following-sibling::div/div/div/div/div/label[contains(text(),'Commencement')]");
        private readonly By licenseDetailsCommencementDateInput = By.Id("datepicker-startDate");
        private readonly By licenseDetailsCommencementDateContent = By.XPath("//h2/following-sibling::div/div/div/div/div/label[contains(text(),'Commencement')]/parent::div/following-sibling::div");

        private readonly By licenseDetailsExpiryDateLabel = By.XPath("//h2/following-sibling::div/div/div/div/div/label[contains(text(),'Expiry')]");
        private readonly By licenseDetailsExpiryDateInput = By.Id("datepicker-expiryDate");
        private readonly By licenseDetailsExpiryDateContent = By.XPath("//h2/following-sibling::div/div/div/div/div/label[contains(text(),'Expiry')]/parent::div/following-sibling::div");

        private readonly By licenseDetailsCancelReasonLabel = By.XPath("//label[contains(text(),'Cancellation reason')]");
        private readonly By licenseDetailsCancelReasonInput = By.Id("input-cancellationReason");
        private readonly By licenseDetailsCancelContent = By.XPath("//label[contains(text(),'Cancellation reason')]/parent::div/following-sibling::div");

        private readonly By licenseDetailsViewTerminateDateLabel = By.XPath("//div[contains(text(),'Details')]/parent::div/parent::h2/following-sibling::div/div[5]/div/label[contains(text(),'Termination')]");
        private readonly By licenseDetailsTerminatedReasonLabel = By.XPath("//label[contains(text(),'Termination reason')]");
        private readonly By licenseDetailsTerminateReasonInput = By.Id("input-terminationReason");
        private readonly By licenseDetailsTerminateDateInput = By.Id("datepicker-terminationDate");
        private readonly By licenseDetailsTerminateReasonContent = By.XPath("//label[contains(text(),'Termination reason')]/parent::div/following-sibling::div");
        private readonly By licenseDetailsViewTerminateDateContent = By.XPath("//div[contains(text(),'Details')]/parent::div/parent::h2/following-sibling::div/div[5]/div[2]");
        private readonly By licenseDetailsViewTerminateCancelReason = By.XPath("//div[@role='tabpanel']/div/div/div/button/preceding-sibling::div");

        //Create/View Renewal Options Elements
        private readonly By licenseDetailsRenewalTitle = By.XPath("//h2/div/div[contains(text(),'Renewal Option')]");
        private readonly By licenseDetailsAddRenewButton = By.XPath("//div[contains(text(),'+ Add a Renew')]/parent::button");
        private readonly By licenceDetailsFirstRenewalDeleteBttn = By.XPath("//*[@data-testid='renewal.0.remove-button']/parent::div/parent::button");

        //Create/View Administration Elements
        private readonly By licenseDetailsAdmSubtitle = By.XPath("//div[contains(text(),'Administration')]");
        private readonly By licenseDetailsMotiContactViewLabel = By.XPath("//label[contains(text(),'MoTI contact')]");
        private readonly By licenseDetailsMotiContactLabel = By.XPath("//label[contains(text(),'MOTI contact')]");
        private readonly By licenseDetailsMotiContactInput = By.Id("input-motiName");
        private readonly By licenseDetailsMotiRegionLabel = By.XPath("//label[contains(text(),'MOTI region')]");
        private readonly By licenseDetailsMotiRegionSelector = By.Id("input-regionId");
        private readonly By licenseDetailsProgramViewLabel = By.XPath("//label[contains(text(),'Program')]");
        private readonly By licenseDetailsProgramContent = By.Id("input-programName");
        private readonly By licenseDetailsProgramLabel = By.XPath("//select[@id='input-programTypeCode']/parent::div/parent::div/preceding-sibling::div/label[contains(text(),'Program')]");
        private readonly By licenseDetailsProgramSelector = By.Id("input-programTypeCode");
        private readonly By licenseDetailsOtherProgramLabel = By.XPath("//label[contains(text(),'Other Program')]");
        private readonly By licenseDetailsOtherProgramInput = By.Id("input-otherProgramTypeDescription");
        private readonly By licenseDetailsOtherProgramContent = By.Id("input-otherProgramType");
        private readonly By licenseDetailsTypeLabel = By.XPath("//label[contains(text(),'Type')]");
        private readonly By licenseDetailsTypeSelector = By.Id("input-leaseTypeCode");
        private readonly By licenseDetailsViewTypeLabel = By.XPath("//label[contains(text(),'Account type')]");
        private readonly By licenseDetailsTypeContent = By.Id("input-type.description");
        private readonly By licenseDetailsOtherTypeLabel = By.XPath("//input[@id='input-otherLeaseTypeDescription']/parent::div/parent::div/preceding-sibling::div/label[contains(text(),'Describe other')]");
        private readonly By licenseDetailsOtherTypeInput = By.Id("input-otherLeaseTypeDescription");
        private readonly By licenseDetailsOtherTypeContent = By.Id("input-otherType");
        private readonly By licenseDetailsReceivableToLabel = By.XPath("//label[contains(text(),'Receivable to')]");
        private readonly By licenseDetailsReceivableToContent = By.Id("input-paymentReceivableType.description");
        private readonly By licenseDetailsCategoryLabel = By.XPath("//label[contains(text(),'Category')]");
        private readonly By licenseDetailsCategorySelector = By.Id("input-categoryTypeCode");
        private readonly By licenseDetailsCategoryOtherLabel = By.XPath("//input[@id='input-otherCategoryTypeDescription']/parent::div/parent::div/preceding-sibling::div/label[contains(text(),'Describe other')]");
        private readonly By licenseDetailsCategoryOtherInput = By.Id("input-otherCategoryTypeDescription");
        private readonly By licenseDetailsPurposeLabel = By.XPath("//label[contains(text(),'Purpose')]");
        private readonly By licenseDetailsPurposeMultiselector = By.Id("multiselect-purposes_input");
        private readonly By licenseDetailsPurposeDeleteBttns = By.CssSelector("div[id='multiselect-purposes'] i[class='custom-close']");
        private readonly By licenseDetailsPurposeOptions = By.XPath("//input[@id='multiselect-purposes_input']/parent::div/following-sibling::div/ul[@class='optionContainer']");

        private readonly By licenseDetailsPurposeContent = By.CssSelector("div[id='multiselectContainerReact'] div");
        private readonly By licenseDetailsOtherPurposeLabel = By.XPath("//input[@id='input-purposeOtherDescription']/parent::div/parent::div/preceding-sibling::div/label[contains(text(),'Describe other')]");
        private readonly By licenseDetailsOtherPurposeInput = By.Id("input-purposeOtherDescription");
        private readonly By licenseDetailsInitiatorLabel = By.XPath("//label[contains(text(),'Initiator')]");
        private readonly By licenseDetailsInitiatorTooltip = By.XPath("//label[contains(text(),'Initiator')]/span/span[@data-testid='tooltip-icon-section-field-tooltip']");
        private readonly By licenseDetailsInitiatorSelector = By.Id("input-initiatorTypeCode");
        private readonly By licenseDetailsInitiatorContent = By.Id("input-initiatorType.description");
        private readonly By licenseDetailsResponsibilityLabel = By.XPath("//label[contains(text(),'Responsibility')]");
        private readonly By licenseDetailsResponsibilityTooltip = By.XPath("//label[contains(text(),'Responsibility')]/span/span[@data-testid='tooltip-icon-section-field-tooltip']");
        private readonly By licenseDetailsResposibilitySelector = By.Id("input-responsibilityTypeCode");
        private readonly By licenseDetailsResponsibilityContent = By.Id("input-responsibilityType.description");
        private readonly By licenseDetailsEffectiveDateLabel = By.XPath("//label[contains(text(),'Effective date')]");
        private readonly By licenseDetailsEffectiveDateInput = By.Id("datepicker-responsibilityEffectiveDate");
        private readonly By licenseDetailsEffectiveDateContent = By.XPath("//label[contains(text(),'Effective date')]/parent::div/following-sibling::div");
        private readonly By licenseDetailsIntendedUseLabel = By.XPath("//label[contains(text(),'Intended use')]");
        private readonly By licenseDetailsIntendedUseTextarea = By.Id("input-description");
        private readonly By licenseDetailsCityArbitrationLabel = By.XPath("//label[contains(text(),'Primary arbitration city')]");
        private readonly By licenseDetailsCityArbitrationInput = By.Id("input-primaryArbitrationCity");
        private readonly By licenseDetailsCityArbitrationContent = By.XPath("//label[contains(text(),'Primary arbitration city')]/parent::div/following-sibling::div/div/input");

        private readonly By licenseDetailsFeeDeterminationSubtitle = By.XPath("//div[contains(text(),'Fee Determination')]");
        private readonly By licenseDetailsFeeDeterminationPublicBenefitLabel = By.XPath("//label[contains(text(),'Public benefit')]");
        private readonly By licenseDetailsFeeDeterminationPublicBenefitInput = By.Id("input-isPublicBenefit");
        private readonly By licenseDetailsFeeDeterminationFinancialGainLabel = By.XPath("//label[contains(text(),'Financial gain')]");
        private readonly By licenseDetailsFeeDeterminationFinancialGainInput = By.Id("input-isFinancialGain");
        private readonly By licenseDetailsFeeDeterminationSuggestedFeeLabel = By.XPath("//label[contains(text(),'Suggested fee')]");
        private readonly By licenseDetailsFeeDeterminationSuggestedFeeContent = By.CssSelector("span[data-testid='suggestedFee']");
        private readonly By licenseDetailsFeeDeterminationNotesLabel = By.XPath("//label[contains(text(),'Notes')]");
        private readonly By licenseDetailsFeeDeterminationNotesInput = By.Id("input-feeDeterminationNote");
        private readonly By licenseDetailsFeeDeterminationNotesContent = By.XPath("//div[contains(text(),'Fee Determination')]/parent::div/parent::h2/following-sibling::div/div[4]/div[2]");

        private readonly By licenseDetailsConsultationSubtitle = By.XPath("//div[contains(text(),'Consultation')]");
        private readonly By licenseDetailsFirstNationLabel = By.XPath("//label[contains(text(),'First nation')]");
        private readonly By licenseDetailsFirstNationSelect = By.Id("input-consultations.0.consultationStatusType");
        private readonly By licenseDetailsFirstNationContent = By.XPath("//label[contains(text(),'First nation')]/parent::div/following-sibling::div");
        private readonly By licenseDetailsSRELabel = By.XPath("//label[contains(text(),'Strategic Real Estate (SRE)')]");
        private readonly By licenseDetailsSRESelect = By.Id("input-consultations.1.consultationStatusType");
        private readonly By licenseDetailsSREContent = By.XPath("//label[contains(text(),'Strategic Real Estate (SRE)')]/parent::div/following-sibling::div");
        private readonly By licenceDetailsRegionalPlanningLabel = By.XPath("//label[contains(text(),'Regional planning')]");
        private readonly By licenseDetailsRegionalPlanningSelect = By.Id("input-consultations.2.consultationStatusType");
        private readonly By licenceDetailsRegionalPlanningContent = By.XPath("//label[contains(text(),'Regional planning')]/parent::div/following-sibling::div");
        private readonly By licenseDetailsRegionalPropertyServicesLabel = By.XPath("//label[contains(text(),'Regional property services')]");
        private readonly By licenseDetailsRegionalPropertyServicesSelect = By.Id("input-consultations.3.consultationStatusType");
        private readonly By licenseDetailsRegionalPropertyServicesContent = By.XPath("//label[contains(text(),'Regional property services')]/parent::div/following-sibling::div");
        private readonly By licenceDetailsDistrictLabel = By.XPath("//label[contains(text(),'District')]");
        private readonly By licenseDetailsDistrictSelect = By.Id("input-consultations.4.consultationStatusType");
        private readonly By licenceDetailsDistrictContent = By.XPath("//label[contains(text(),'District')]/parent::div/following-sibling::div");
        private readonly By licenseDetailsHeadquarterLabel = By.XPath("//label[contains(text(),'Headquarter (HQ)')]");
        private readonly By licenseDetailsHeadquarterSelect = By.Id("input-consultations.5.consultationStatusType");
        private readonly By licenseDetailsHeadquarterContent = By.XPath("//label[contains(text(),'Headquarter (HQ)')]/parent::div/following-sibling::div");
        private readonly By licenceDetailsOtherLabel = By.XPath("//label[contains(text(),'Other')]");
        private readonly By licenseDetailsOtherSelect = By.Id("input-consultations.6.consultationStatusType");
        private readonly By licenceDetailsOtherContent = By.XPath("//label[contains(text(),'Other')]/parent::div/following-sibling::div");
        private readonly By licenseDetailsOtherDetailsInput = By.Id("input-consultations.6.consultationTypeOtherDescription");

        private readonly By licenseDetailsDocsSubtitle = By.XPath("//div[contains(text(),'Documentation')]");
        private readonly By licenseDetailsPhysicalLeaseExistViewLabel = By.XPath("//label[contains(text(),'Physical copy exists')]");
        private readonly By licenseDetailsPhysicalLeaseExistLabel = By.XPath("//label[contains(text(),'Physical lease/licence exists')]");
        private readonly By licenseDetailsPhysicalLeaseExistSelector = By.Id("input-hasPhysicalLicense");
        private readonly By licenseDetailsDigitalLeaseExistViewLabel = By.XPath("//label[contains(text(),'Digital copy exists')]");
        private readonly By licenseDetailsDigitalLeaseExistLabel = By.XPath("//label[contains(text(),'Digital lease/licence exists')]");
        private readonly By licenseDetailsDigitalLeaseExistSelector = By.Id("input-hasDigitalLicense");
        private readonly By licenseDetailsLocationDocsLabel = By.XPath("//label[contains(text(),'Document location')]");
        private readonly By licenseDetailsLocationDocsTooltip = By.XPath("//label[contains(text(),'Document location')]/span/span[@data-testid='tooltip-icon-section-field-tooltip']");
        private readonly By licenseDetailsLocationDocsTextarea = By.Id("input-documentationReference");
        private readonly By licenseDetailsLocationDocsContent = By.XPath("//label[contains(text(),'Document location')]/parent::div/following-sibling::div");
        private readonly By licenseDetailsNotesLabel = By.XPath("//label[contains(text(),'Lease notes')]");
        private readonly By licenseDetailsNotesTextarea = By.Id("input-note");
        private readonly By licenseDetailsNotesContent = By.XPath("//label[contains(text(),'Lease notes')]/parent::div/following-sibling::div");

        private readonly By licenseDetailsSaveButton = By.XPath("//div[contains(text(),'Save')]/parent::button");
        private readonly By licenseDetailsCancelButton = By.XPath("//div[contains(text(),'Cancel')]/parent::button");

        private readonly By licenseDetailsModalPIDAttached = By.CssSelector("div[class='modal-content']");

        //Lease Property Section Elements
        private readonly By leasePropertiesSubtitle = By.XPath("//div[contains(text(),'Property Information')]/parent::div/parent::h2");
        private readonly By leasePropertiesUpdateSubtitle = By.XPath("//div[contains(text(),'Selected properties')]/parent::div/parent::h2");

        //Leases Modal Element
        private readonly By licenseDetailsConfirmationModal = By.CssSelector("div[class='modal-content']");
        private readonly By licenseDetailsConfirmationContent = By.CssSelector("div[class='modal-content'] p");

        private readonly SharedFileProperties sharedSearchProperties;
        private readonly SharedModals sharedModals;

        public LeaseDetails(IWebDriver webDriver) : base(webDriver)
        {
            sharedSearchProperties = new SharedFileProperties(webDriver);
            sharedModals = new SharedModals(webDriver);
        }

        //Navigates to Create a new Lease/License
        public void NavigateToCreateNewLicense()
        {
            WaitUntilClickable(menuManagementButton);
            FocusAndClick(menuManagementButton);

            WaitUntilClickable(createLicenseButton);
            FocusAndClick(createLicenseButton);
        }

        public void CreateMinimumLicenseDetails(Lease lease)
        {
            Wait();

            //MAIN DETAILS
            //Start Date
            if (lease.LeaseStartDate != "")
            {
                webDriver.FindElement(licenseDetailsCommencementDateInput).SendKeys(lease.LeaseStartDate);
                webDriver.FindElement(licenseDetailsCommencementDateInput).SendKeys(Keys.Enter);
            }

            //Administration Details
            //MOTI Region
            if(lease.MOTIRegion != "")
                ChooseSpecificSelectOption(licenseDetailsMotiRegionSelector, lease.MOTIRegion);

            //Program
            if (lease.Program != "")
                ChooseSpecificSelectOption(licenseDetailsProgramSelector, lease.Program);

            //If other Program is selected, insert input
            if (webDriver.FindElements(licenseDetailsOtherProgramInput).Count > 0 && lease.ProgramOther != "")
            {
                WaitUntilVisible(licenseDetailsOtherProgramInput);
                Assert.True(webDriver.FindElement(licenseDetailsOtherProgramLabel).Displayed);
                webDriver.FindElement(licenseDetailsOtherProgramInput).SendKeys(lease.ProgramOther);
            }

            //Type
            if (lease.AdminType != "")
                ChooseSpecificSelectOption(licenseDetailsTypeSelector, lease.AdminType);

            //If other Type is selected, insert input
            if (webDriver.FindElements(licenseDetailsOtherTypeInput).Count > 0 && lease.TypeOther != "")
            {
                Assert.True(webDriver.FindElement(licenseDetailsOtherTypeLabel).Displayed);
                webDriver.FindElement(licenseDetailsOtherTypeInput).SendKeys(lease.TypeOther);
            }

            //Purpose
            if (webDriver.FindElements(licenseDetailsPurposeDeleteBttns).Count > 0)
            {
                while (webDriver.FindElements(licenseDetailsPurposeDeleteBttns).Count > 0)
                    webDriver.FindElements(licenseDetailsPurposeDeleteBttns)[0].Click();
            }

            if (lease.LeasePurpose.Count > 0)
            {
                foreach (string purpose in lease.LeasePurpose)
                {
                    FocusAndClick(licenseDetailsPurposeMultiselector);

                    Wait(5000);
                    ChooseMultiSelectSpecificOption(licenseDetailsPurposeOptions, purpose);
                }

                webDriver.FindElement(licenseDetailsPurposeLabel).Click();
            }

            //If other Purpose is selected, insert input
            if (webDriver.FindElements(licenseDetailsOtherPurposeInput).Count > 0 && lease.PurposeOther != "")
            {
                Assert.True(webDriver.FindElement(licenseDetailsOtherPurposeLabel).Displayed);
                webDriver.FindElement(licenseDetailsOtherPurposeInput).SendKeys(lease.PurposeOther);
            }
        }

        public void UpdateLeaseFileDetails(Lease lease)
        {
            //MAIN DETAILS
            //Project
            Wait();
            AssertTrueIsDisplayed(licenseDetailsCreateSubtitle);

            if (lease.MinistryProject != "")
            {
                ClearInput(licenseDetailsProjectInput);
                webDriver.FindElement(licenseDetailsProjectInput).SendKeys(lease.MinistryProject);
                webDriver.FindElement(licenseDetailsProjectInput).SendKeys(Keys.Enter);
                webDriver.FindElement(licenseDetailsProjectInput).SendKeys(Keys.Backspace);

                Wait();
                webDriver.FindElement(licenseDetailsProject1stOption).Click();
            }

            //Status
            if (lease.LeaseStatus != "")
            {
                SelectElement selectStatusElement = new(webDriver.FindElement(licenseDetailsStatusSelector));
                IWebElement selectedStatus = selectStatusElement.SelectedOption;
                string previousStatus = selectedStatus.Text;

                ChooseSpecificSelectOption(licenseDetailsStatusSelector, lease.LeaseStatus);

                Wait();
                if (webDriver.FindElements(licenseDetailsConfirmationModal).Count > 0)
                {
                    Assert.Equal("Are you sure?", sharedModals.ModalHeader());
                    Assert.Contains("The lease is no longer in " + previousStatus + " state. The reason for doing so will be cleared from the file details and can only be viewed in the notes tab.", webDriver.FindElement(licenseDetailsConfirmationContent).Text);
                    Assert.Contains("Do you want to proceed?", webDriver.FindElement(licenseDetailsConfirmationContent).Text);
                    sharedModals.ModalClickOKBttn();
                }
            }

            //Account Type
            if (lease.AccountType != "")
            {
                webDriver.FindElement(licenseDetailsAccountTypeSelector).Click();
                ChooseSpecificSelectOption(licenseDetailsAccountTypeSelector, lease.AccountType);
            }

            //Start Date
            if (lease.LeaseStartDate != "")
            {
                ClearInput(licenseDetailsCommencementDateInput);
                webDriver.FindElement(licenseDetailsCommencementDateInput).SendKeys(lease.LeaseStartDate);
                webDriver.FindElement(licenseDetailsCommencementDateInput).SendKeys(Keys.Enter);
            }

            //Expiry Date
            if (lease.LeaseExpiryDate != "")
            {
                ClearInput(licenseDetailsExpiryDateInput);
                webDriver.FindElement(licenseDetailsExpiryDateInput).Click();
                webDriver.FindElement(licenseDetailsExpiryDateInput).SendKeys(lease.LeaseExpiryDate);
                webDriver.FindElement(licenseDetailsExpiryDateInput).SendKeys(Keys.Enter);
            }

            //Termination reason
            if (lease.LeaseTerminationReason != "")
            {
                ClearInput(licenseDetailsTerminateDateInput);
                webDriver.FindElement(licenseDetailsTerminateDateInput).SendKeys(lease.LeaseTerminationDate);
                webDriver.FindElement(licenseDetailsTerminateDateInput).SendKeys(Keys.Enter);

                ClearInput(licenseDetailsTerminateReasonInput);
                webDriver.FindElement(licenseDetailsTerminateReasonInput).Click();
                webDriver.FindElement(licenseDetailsTerminateReasonInput).SendKeys(lease.LeaseTerminationReason);
            }

            //Cancellation reason
            if (lease.LeaseCancellationReason != "")
            {
                ClearInput(licenseDetailsCancelReasonInput);
                webDriver.FindElement(licenseDetailsCancelReasonInput).Click();
                webDriver.FindElement(licenseDetailsCancelReasonInput).SendKeys(lease.LeaseCancellationReason);
            }

            //RENEWAL OPTIONS
            if (lease.LeaseRenewals.Count > 0)
            {
                //Delete previous created renewals is any
                while (webDriver.FindElements(licenceDetailsFirstRenewalDeleteBttn).Count > 0)
                {
                    webDriver.FindElement(licenceDetailsFirstRenewalDeleteBttn).Click();

                    Wait();
                    if (webDriver.FindElements(licenseDetailsConfirmationModal).Count > 0)
                    {
                        Assert.Equal("Remove Renewal", sharedModals.ModalHeader());
                        Assert.Contains("Removing a renewal will also delete any renewal data.", sharedModals.ModalContent());
                        Assert.Contains("Do you want to proceed?", sharedModals.ModalContent());
                        sharedModals.ModalClickOKBttn();
                    }
                }

                for (var i = 0; i < lease.LeaseRenewals.Count; i++)
                {
                    webDriver.FindElement(licenseDetailsAddRenewButton).Click();
                    ChooseSpecificSelectOption(By.Id("input-renewals."+ i +".isExercised"), lease.LeaseRenewals[i].RenewalIsExercised);

                    webDriver.FindElement(By.Id("datepicker-renewals."+ i +".commencementDt")).SendKeys(lease.LeaseRenewals[i].RenewalCommencementDate);
                    webDriver.FindElement(By.Id("datepicker-renewals."+ i +".commencementDt")).SendKeys(Keys.Enter);

                    webDriver.FindElement(By.Id("datepicker-renewals."+ i +".expiryDt")).SendKeys(lease.LeaseRenewals[i].RenewalExpiryDate);
                    webDriver.FindElement(By.Id("datepicker-renewals."+ i +".expiryDt")).SendKeys(Keys.Enter);

                    webDriver.FindElement(By.Id("input-renewals."+ i +".renewalNote")).SendKeys(lease.LeaseRenewals[i].RenewalNotes);
                }
            }

            //ADMINISTRATION
            //MOTI Contact
            if (lease.MOTIContact != "")
                webDriver.FindElement(licenseDetailsMotiContactInput).SendKeys(lease.MOTIContact);

            //MOTI Region
            if (lease.MOTIRegion != "")
                ChooseSpecificSelectOption(licenseDetailsMotiRegionSelector, lease.MOTIRegion);

            //Program
            if (lease.Program != "")
                ChooseSpecificSelectOption(licenseDetailsProgramSelector, lease.Program);

            //If other Program is selected, insert input
            if (webDriver.FindElements(licenseDetailsOtherProgramInput).Count > 0 && lease.ProgramOther != "")
            {
                WaitUntilVisible(licenseDetailsOtherProgramInput);
                Assert.True(webDriver.FindElement(licenseDetailsOtherProgramLabel).Displayed);
                ClearInput(licenseDetailsOtherProgramInput);
                webDriver.FindElement(licenseDetailsOtherProgramInput).SendKeys(lease.ProgramOther);
            }

            //Type
            if (lease.AdminType != "")
                ChooseSpecificSelectOption(licenseDetailsTypeSelector, lease.AdminType);

            //If other Type is selected, insert input
            if (webDriver.FindElements(licenseDetailsOtherTypeInput).Count > 0 && lease.TypeOther != "")
            {
                Assert.True(webDriver.FindElement(licenseDetailsOtherTypeLabel).Displayed);
                ClearInput(licenseDetailsOtherTypeInput);
                webDriver.FindElement(licenseDetailsOtherTypeInput).SendKeys(lease.TypeOther);
            }

            //Purpose
            if (webDriver.FindElements(licenseDetailsPurposeDeleteBttns).Count > 0)
            {
                while (webDriver.FindElements(licenseDetailsPurposeDeleteBttns).Count > 0)
                    webDriver.FindElements(licenseDetailsPurposeDeleteBttns)[0].Click();
            }

            if (lease.LeasePurpose.Count > 0)
            {
                foreach (string purpose in lease.LeasePurpose)
                {
                    FocusAndClick(licenseDetailsPurposeMultiselector);

                    WaitUntilClickable(licenseDetailsPurposeOptions);
                    ChooseMultiSelectSpecificOption(licenseDetailsPurposeOptions, purpose);
                }
            }

            //If other Purpose is selected, insert input
            if (webDriver.FindElements(licenseDetailsOtherPurposeInput).Count > 0 && lease.PurposeOther != "")
            {
                WaitUntilVisible(licenseDetailsOtherPurposeInput);
                Assert.True(webDriver.FindElement(licenseDetailsOtherPurposeLabel).Displayed);
                ClearInput(licenseDetailsOtherPurposeInput);
                webDriver.FindElement(licenseDetailsOtherPurposeInput).SendKeys(lease.PurposeOther);
            }

            //Initiator
            if (lease.Initiator != "")
                ChooseSpecificSelectOption(licenseDetailsInitiatorSelector, lease.Initiator);

            //Responsibility
            if (lease.Responsibility != "")
                ChooseSpecificSelectOption(licenseDetailsResposibilitySelector, lease.Responsibility);

            //Effective date of responsibility
            if (lease.EffectiveDate != "")
            {
                ClearInput(licenseDetailsEffectiveDateInput);
                webDriver.FindElement(licenseDetailsEffectiveDateInput).SendKeys(lease.EffectiveDate);
                webDriver.FindElement(licenseDetailsEffectiveDateInput).SendKeys(Keys.Enter);
            }

            //Intended use
            if (lease.IntendedUse != "")
            {
                ClearInput(licenseDetailsIntendedUseTextarea);
                webDriver.FindElement(licenseDetailsIntendedUseTextarea).SendKeys(lease.IntendedUse);
            }

            //Arbitration City
            if (lease.ArbitrationCity != "")
            {
                ClearInput(licenseDetailsCityArbitrationInput);
                webDriver.FindElement(licenseDetailsCityArbitrationInput).SendKeys(lease.ArbitrationCity);
            }

            //CONSULTATION DETAILS
            //First Nation
            if (lease.FirstNation != "")
                ChooseSpecificSelectOption(licenseDetailsFirstNationSelect, lease.FirstNation);

            //Startegic Real Estate
            if (lease.StrategicRealEstate != "")
                ChooseSpecificSelectOption(licenseDetailsSRESelect, lease.StrategicRealEstate);

            //Regional planning
            if (lease.RegionalPlanning != "")
                ChooseSpecificSelectOption(licenseDetailsRegionalPlanningSelect, lease.RegionalPlanning);

            //Regional property services
            if (lease.RegionalPropertyService != "")
                ChooseSpecificSelectOption(licenseDetailsRegionalPropertyServicesSelect, lease.RegionalPropertyService);

            //District
            if (lease.District != "")
                ChooseSpecificSelectOption(licenseDetailsDistrictSelect, lease.District);

            //Headquarters
            if (lease.Headquarter != "")
                ChooseSpecificSelectOption(licenseDetailsHeadquarterSelect, lease.Headquarter);

            //Other
            if (lease.ConsultationOther != "")
                ChooseSpecificSelectOption(licenseDetailsOtherSelect, lease.ConsultationOther);

            //Describe other
            if (lease.ConsultationOtherDetails != "")
                webDriver.FindElement(licenseDetailsOtherDetailsInput).SendKeys(lease.ConsultationOtherDetails);

            //FEE DETERMINATION
            if (lease.FeeDeterminationPublicBenefit != "")
                ChooseSpecificSelectOption(licenseDetailsFeeDeterminationPublicBenefitInput, lease.FeeDeterminationPublicBenefit);

            if(lease.FeeDeterminationFinancialGain != "")
                ChooseSpecificSelectOption(licenseDetailsFeeDeterminationFinancialGainInput, lease.FeeDeterminationFinancialGain);

            AssertTrueElementContains(licenseDetailsFeeDeterminationSuggestedFeeContent, lease.FeeDeterminationSuggestedFee);

            if (lease.FeeDeterminationNotes != "")
            {
                ClearInput(licenseDetailsFeeDeterminationNotesInput);
                webDriver.FindElement(licenseDetailsFeeDeterminationNotesInput).SendKeys(lease.FeeDeterminationNotes);
            }

            //DOCUMENTATION
            //Selecting Physical lease exists
            if (lease.PhysicalLeaseExist != "")
                ChooseSpecificSelectOption(licenseDetailsPhysicalLeaseExistSelector, lease.PhysicalLeaseExist);

            //Selecting Digital lease exists
            if (lease.DigitalLeaseExist != "")
                ChooseSpecificSelectOption(licenseDetailsDigitalLeaseExistSelector, lease.DigitalLeaseExist);

            //Inserting Location of documents
            if (lease.DocumentLocation != "")
                webDriver.FindElement(licenseDetailsLocationDocsTextarea).SendKeys(lease.DocumentLocation);

            //Inserting Notes
            if (lease.LeaseNotes != "")
            {
                ClearInput(licenseDetailsNotesTextarea);
                webDriver.FindElement(licenseDetailsNotesTextarea).SendKeys(lease.LeaseNotes);
            } 
        }

        public void EditLeaseFileDetailsBttn()
        {
            Wait();
            webDriver.FindElement(licenseDetailsEditIcon).Click();
        }

        public void SaveLicense()
        {
            //Save
            ButtonElement("Save");

            Wait();
            //If PID is already associated with another license
            if (webDriver.FindElements(licenseDetailsModalPIDAttached).Count > 0)
            {
                Wait();
                sharedModals.ModalClickOKBttn();

                Wait(5000);
                if (webDriver.FindElements(licenseDetailsConfirmationModal).Count > 0)
                {
                    Assert.Equal("User Override Required", sharedModals.ModalHeader());

                    if (sharedModals.ModalContent().Contains("he selected property already exists in the system's inventory"))
                    {
                        Assert.Contains("The selected property already exists in the system's inventory. However, the record is missing spatial details.", sharedModals.ModalContent());
                        Assert.Contains("To add the property, the spatial details for this property will need to be updated. The system will attempt to update the property record with spatial information from the current selection.", sharedModals.ModalContent());
                    }
                    else
                        Assert.Contains("is attached to L-File #", sharedModals.ModalContent());
                    
                    sharedModals.ModalClickOKBttn();
                }
            }
        }

        public void CancelLicense()
        {
            ButtonElement("Cancel");
            sharedModals.CancelActionModal();
        }

        public string GetLeaseCode()
        {
            WaitUntilVisible(licenseHeaderNbrContent);
            return webDriver.FindElement(licenseHeaderNbrContent).Text;
        }

        public string GetLeaseAccountType()
        {
            WaitUntilVisible(licenseHeaderAccountType);
            return webDriver.FindElement(licenseHeaderAccountType).Text;
        }

        public void VerifyLicenseDetailsCreateForm()
        {
            Wait();

            //Create Title
            AssertTrueIsDisplayed(licenseCreateTitle);

            //Details
            AssertTrueIsDisplayed(licenseDetailsProjectLabel);
            AssertTrueIsDisplayed(licenseDetailsProjectInput);
            AssertTrueIsDisplayed(licenseDetailsStatusLabel);
            AssertTrueIsDisplayed(licenseDetailsStatusSelector);
            AssertTrueIsDisplayed(licenseDetailsAccountTypeLabel);
            AssertTrueIsDisplayed(licenseDetailsAccountTypeSelector);
            AssertTrueIsDisplayed(licenseDetailsCommencementDateLabel);
            AssertTrueIsDisplayed(licenseDetailsCommencementDateInput);
            AssertTrueIsDisplayed(licenseDetailsExpiryDateLabel);
            AssertTrueIsDisplayed(licenseDetailsExpiryDateInput);

            //Properties to include in this file
            sharedSearchProperties.VerifyLocateOnMapFeature();

            //Administration
            AssertTrueIsDisplayed(licenseDetailsAdmSubtitle);
            AssertTrueIsDisplayed(licenseDetailsMotiContactLabel);
            AssertTrueIsDisplayed(licenseDetailsMotiContactInput);
            AssertTrueIsDisplayed(licenseDetailsMotiRegionLabel);
            AssertTrueIsDisplayed(licenseDetailsMotiRegionSelector);
            AssertTrueIsDisplayed(licenseDetailsProgramLabel);
            AssertTrueIsDisplayed(licenseDetailsProgramSelector);
            AssertTrueIsDisplayed(licenseDetailsTypeLabel);
            AssertTrueIsDisplayed(licenseDetailsTypeSelector);
            AssertTrueIsDisplayed(licenseDetailsPurposeLabel);
            AssertTrueIsDisplayed(licenseDetailsPurposeMultiselector);
            AssertTrueIsDisplayed(licenseDetailsInitiatorLabel);
            AssertTrueIsDisplayed(licenseDetailsInitiatorTooltip);
            AssertTrueIsDisplayed(licenseDetailsInitiatorSelector);
            AssertTrueIsDisplayed(licenseDetailsResponsibilityLabel);
            AssertTrueIsDisplayed(licenseDetailsResponsibilityTooltip);
            AssertTrueIsDisplayed(licenseDetailsResposibilitySelector);
            AssertTrueIsDisplayed(licenseDetailsEffectiveDateLabel);
            AssertTrueIsDisplayed(licenseDetailsEffectiveDateInput);
            AssertTrueIsDisplayed(licenseDetailsIntendedUseLabel);
            AssertTrueIsDisplayed(licenseDetailsIntendedUseTextarea);

            //Consultation
            AssertTrueIsDisplayed(licenseDetailsConsultationSubtitle);
            AssertTrueIsDisplayed(licenseDetailsFirstNationLabel);
            AssertTrueIsDisplayed(licenseDetailsFirstNationSelect);
            AssertTrueIsDisplayed(licenseDetailsSRELabel);
            AssertTrueIsDisplayed(licenseDetailsSRESelect);
            AssertTrueIsDisplayed(licenceDetailsRegionalPlanningLabel);
            AssertTrueIsDisplayed(licenseDetailsRegionalPlanningSelect);
            AssertTrueIsDisplayed(licenseDetailsRegionalPropertyServicesLabel);
            AssertTrueIsDisplayed(licenseDetailsRegionalPropertyServicesSelect);
            AssertTrueIsDisplayed(licenceDetailsDistrictLabel);
            AssertTrueIsDisplayed(licenseDetailsDistrictSelect);
            AssertTrueIsDisplayed(licenseDetailsHeadquarterLabel);
            AssertTrueIsDisplayed(licenseDetailsHeadquarterSelect);
            AssertTrueIsDisplayed(licenceDetailsOtherLabel);
            AssertTrueIsDisplayed(licenseDetailsOtherSelect);
            AssertTrueIsDisplayed(licenseDetailsOtherDetailsInput);

            //Documentation
            AssertTrueIsDisplayed(licenseDetailsDocsSubtitle);
            AssertTrueIsDisplayed(licenseDetailsPhysicalLeaseExistLabel);
            AssertTrueIsDisplayed(licenseDetailsPhysicalLeaseExistSelector);
            AssertTrueIsDisplayed(licenseDetailsDigitalLeaseExistLabel);
            AssertTrueIsDisplayed(licenseDetailsDigitalLeaseExistSelector);
            AssertTrueIsDisplayed(licenseDetailsLocationDocsLabel);
            AssertTrueIsDisplayed(licenseDetailsLocationDocsTooltip);
            AssertTrueIsDisplayed(licenseDetailsLocationDocsTextarea);
            AssertTrueIsDisplayed(licenseDetailsNotesLabel);
            AssertTrueIsDisplayed(licenseDetailsNotesTextarea);

            //Buttons
            AssertTrueIsDisplayed(licenseDetailsSaveButton);
            AssertTrueIsDisplayed(licenseDetailsCancelButton);
        }

        public void VerifyLicenseDetailsUpdateForm()
        {
            WaitUntilVisible(licenseDetailsStatusLabel);

            //Details
            AssertTrueIsDisplayed(licenseDetailsStatusLabel);
            AssertTrueIsDisplayed(licenseDetailsStatusSelector);
            AssertTrueIsDisplayed(licenseDetailsAccountTypeLabel);
            AssertTrueIsDisplayed(licenseDetailsAccountTypeSelector);
            AssertTrueIsDisplayed(licenseDetailsCommencementDateLabel);
            AssertTrueIsDisplayed(licenseDetailsCommencementDateInput);
            AssertTrueIsDisplayed(licenseDetailsExpiryDateLabel);
            AssertTrueIsDisplayed(licenseDetailsExpiryDateInput);

            //Administration
            AssertTrueIsDisplayed(licenseDetailsAdmSubtitle);
            AssertTrueIsDisplayed(licenseDetailsMotiContactLabel);
            AssertTrueIsDisplayed(licenseDetailsMotiContactInput);
            AssertTrueIsDisplayed(licenseDetailsMotiRegionLabel);
            AssertTrueIsDisplayed(licenseDetailsMotiRegionSelector);
            AssertTrueIsDisplayed(licenseDetailsProgramLabel);
            AssertTrueIsDisplayed(licenseDetailsProgramSelector);

            if (webDriver.FindElements(licenseDetailsOtherProgramLabel).Count > 0)
                AssertTrueIsDisplayed(licenseDetailsOtherProgramInput);

            AssertTrueIsDisplayed(licenseDetailsTypeLabel);
            AssertTrueIsDisplayed(licenseDetailsTypeSelector);

            if (webDriver.FindElements(licenseDetailsOtherTypeLabel).Count > 0)
                AssertTrueIsDisplayed(licenseDetailsOtherTypeInput);

            if (webDriver.FindElements(licenseDetailsCategoryLabel).Count > 0)
                AssertTrueIsDisplayed(licenseDetailsCategorySelector);

            if (webDriver.FindElements(licenseDetailsCategoryOtherLabel).Count > 0)
                AssertTrueIsDisplayed(licenseDetailsCategoryOtherInput);

            AssertTrueIsDisplayed(licenseDetailsPurposeLabel);
            AssertTrueIsDisplayed(licenseDetailsPurposeMultiselector);

            if (webDriver.FindElements(licenseDetailsOtherTypeLabel).Count > 0)
                AssertTrueIsDisplayed(licenseDetailsOtherTypeInput);

            AssertTrueIsDisplayed(licenseDetailsInitiatorLabel);
            AssertTrueIsDisplayed(licenseDetailsInitiatorTooltip);
            AssertTrueIsDisplayed(licenseDetailsInitiatorSelector);
            AssertTrueIsDisplayed(licenseDetailsResponsibilityLabel);
            AssertTrueIsDisplayed(licenseDetailsResponsibilityTooltip);
            AssertTrueIsDisplayed(licenseDetailsResposibilitySelector);
            AssertTrueIsDisplayed(licenseDetailsEffectiveDateLabel);
            AssertTrueIsDisplayed(licenseDetailsEffectiveDateInput);
            AssertTrueIsDisplayed(licenseDetailsIntendedUseLabel);
            AssertTrueIsDisplayed(licenseDetailsIntendedUseTextarea);

            //Documentation
            AssertTrueIsDisplayed(licenseDetailsDocsSubtitle);
            AssertTrueIsDisplayed(licenseDetailsPhysicalLeaseExistLabel);
            AssertTrueIsDisplayed(licenseDetailsPhysicalLeaseExistSelector);
            AssertTrueIsDisplayed(licenseDetailsDigitalLeaseExistLabel);
            AssertTrueIsDisplayed(licenseDetailsDigitalLeaseExistSelector);
            AssertTrueIsDisplayed(licenseDetailsLocationDocsLabel);
            AssertTrueIsDisplayed(licenseDetailsLocationDocsTooltip);
            AssertTrueIsDisplayed(licenseDetailsLocationDocsTextarea);
            AssertTrueIsDisplayed(licenseDetailsNotesLabel);
            AssertTrueIsDisplayed(licenseDetailsNotesTextarea);
        }

        public void UpdateLicensePropertiesForm(LeaseProperty property, int propertyIdx)
        {
            AssertTrueIsDisplayed(leasePropertiesUpdateSubtitle);

            if (property.DescriptiveName != "")
            {
                ClearInput(By.Id("input-properties."+ propertyIdx +".name"));
                webDriver.FindElement(By.Id("input-properties."+ propertyIdx +".name")).SendKeys(property.DescriptiveName);

            }

            if (property.Area != "")
            {
                ClearDigitsInput(By.XPath("//input[@id='input-properties."+ propertyIdx +".name']/parent::div/parent::div/parent::div/following-sibling::div[1]/div/div/div[1]/div/div[1]/div/input"));
                webDriver.FindElement(By.XPath("//input[@id='input-properties."+ propertyIdx +".name']/parent::div/parent::div/parent::div/following-sibling::div[1]/div/div/div[1]/div/div[1]/div/input")).SendKeys(property.Area);

                AssertTrueDoublesEquals(By.XPath("//input[@id='input-properties."+ propertyIdx +".name']/parent::div/parent::div/parent::div/following-sibling::div[1]/div/div/div[1]/div/div[2]/div/input"), TransformSqMtToHectares(property.Area));
                AssertTrueIsDisplayed(By.XPath("//input[@id='input-properties."+ propertyIdx +".name']/parent::div/parent::div/parent::div/following-sibling::div[1]/div/div/div[1]/div/div[2]/div[contains(text(),'hectares')]"));

                AssertTrueDoublesEquals(By.XPath("//input[@id='input-properties."+ propertyIdx +".name']/parent::div/parent::div/parent::div/following-sibling::div[1]/div/div/div[2]/div/div[1]/div/input"), TransformSqMtToSqFt(property.Area));
                AssertTrueIsDisplayed(By.XPath("//input[@id='input-properties."+ propertyIdx +".name']/parent::div/parent::div/parent::div/following-sibling::div[1]/div/div/div[2]/div/div[1]/div[contains(text(),'sq. feet')]"));

                AssertTrueDoublesEquals(By.XPath("//input[@id='input-properties."+ propertyIdx +".name']/parent::div/parent::div/parent::div/following-sibling::div[1]/div/div/div[2]/div/div[2]/div/input"), TransformSqMtToAcres(property.Area));
                AssertTrueIsDisplayed(By.XPath("//input[@id='input-properties."+ propertyIdx +".name']/parent::div/parent::div/parent::div/following-sibling::div[1]/div/div/div[2]/div/div[2]/div[contains(text(),'acres')]"));
            }
        }

        public void VerifyLicenseHeader(Lease lease)
        {
            AssertTrueIsDisplayed(licenseHeaderNbrLabel);
            AssertTrueContentNotEquals(licenseHeaderNbrContent, "");
            AssertTrueContentNotEquals(licenseHeaderAccountType, "");
            AssertTrueIsDisplayed(licenseHeaderProperty);
            //AssertTrueIsDisplayed(licenseHeaderPropertyContent);
            AssertTrueIsDisplayed(licenseHeaderTenantLabel);
            AssertTrueIsDisplayed(licenseHeaderStartDateLabel);
            AssertTrueContentEquals(licenseHeaderStartDateContent, TransformDateFormat(lease.LeaseStartDate));
            AssertTrueIsDisplayed(licenseHeaderExpiryDateLabel);
            AssertTrueContentEquals(licenseHeaderExpiryDateContent, CalculateExpiryCurrentDate(lease.LeaseExpiryDate, lease.LeaseRenewals));
            AssertTrueIsDisplayed(licenseHeaderHistoricalFileLabel);
            //Assert.True(webDriver.FindElements(licenseHeaderHistoricalFileContent).Count > 0);
            AssertTrueIsDisplayed(licenseHeaderCreatedLabel);
            AssertTrueContentNotEquals(licenseHeaderCreatedContent, "");
            AssertTrueContentEquals(licenseHeaderCreatedByContent, "TRANPSP1");
            AssertTrueIsDisplayed(licenseHeaderLastUpdatedLabel);
            AssertTrueContentNotEquals(licenseHeaderLastUpdatedContent, "");
            AssertTrueContentEquals(licenseHeaderLastUpdatedByContent, "TRANPSP1");

            if (lease.LeaseStatus != "")
                AssertTrueContentEquals(licenseHeaderStatusContent, GetUppercaseString(lease.LeaseStatus));

            //Verify Expired Flag on Header
            if (webDriver.FindElement(licenseHeaderExpiryDateContent).Text != "")
            {
                var expiryDateInput = Convert.ToDateTime(webDriver.FindElement(licenseHeaderExpiryDateContent).Text);
                if (expiryDateInput < DateTime.Now)
                {
                    AssertTrueIsDisplayed(licenseHeaderExpiredFlag);
                }
            }  
        }

        public void VerifyLicensePropertiesHeader()
        {
            AssertTrueIsDisplayed(licenseHeaderNbrLabel);
            AssertTrueContentNotEquals(licenseHeaderNbrContent, "");
            AssertTrueContentNotEquals(licenseHeaderAccountType, "");
            AssertTrueIsDisplayed(licenseHeaderProperty);
            AssertTrueIsDisplayed(licenseHeaderPropertyContent);
            AssertTrueIsDisplayed(licenseHeaderTenantLabel);
            AssertTrueIsDisplayed(licenseHeaderStartDateLabel);
            AssertTrueIsDisplayed(licenseHeaderExpiryDateLabel);
            AssertTrueIsDisplayed(licenseHeaderHistoricalFileLabel);
            Assert.True(webDriver.FindElements(licenseHeaderHistoricalFileContent).Count > 0);
            AssertTrueIsDisplayed(licenseHeaderCreatedLabel);
            AssertTrueContentNotEquals(licenseHeaderCreatedContent, "");
            AssertTrueContentEquals(licenseHeaderCreatedByContent, "TRANPSP1");
            AssertTrueIsDisplayed(licenseHeaderLastUpdatedLabel);
            AssertTrueContentNotEquals(licenseHeaderLastUpdatedContent, "");
            AssertTrueContentEquals(licenseHeaderLastUpdatedByContent, "TRANPSP1");

            //Verify Expired Flag on Header
            if (webDriver.FindElement(licenseHeaderExpiryDateContent).Text != "")
            {
                var expiryDateInput = Convert.ToDateTime(webDriver.FindElement(licenseHeaderExpiryDateContent).Text);
                if (expiryDateInput < DateTime.Now)
                {
                    AssertTrueIsDisplayed(licenseHeaderExpiredFlag);
                }
            }
        }

        public void VerifyLicenseDetailsViewForm(Lease lease)
        {
            VerifyLicenseHeader(lease);

            //Edit Icon
            AssertTrueIsDisplayed(licenseDetailsEditIcon);

            //Details
            AssertTrueIsDisplayed(licenseDetailsViewSubtitle);

            AssertTrueIsDisplayed(licenseDetailsProjectLabel);
            if (lease.MinistryProject != "")
                AssertTrueContentEquals(licenseDetailsProjectContent, lease.MinistryProjectCode + " - " + lease.MinistryProject);

            AssertTrueIsDisplayed(licenseDetailsStatusLabel);
            AssertTrueContentEquals(licenseDetailsStatusContent, lease.LeaseStatus);

            AssertTrueIsDisplayed(licenseDetailsAccountTypeLabel);
            AssertTrueContentEquals(licenseDetailsAccountTypeContent, lease.AccountType);

            AssertTrueIsDisplayed(licenseDetailsCommencementDateLabel);
            if (lease.LeaseStartDate != "")
                AssertTrueContentEquals(licenseDetailsCommencementDateContent, TransformDateFormat(lease.LeaseStartDate));

            AssertTrueIsDisplayed(licenseDetailsExpiryDateLabel);
            if (lease.LeaseExpiryDate != "" || lease.LeaseRenewals.Count > 0)
                AssertTrueContentEquals(licenseDetailsExpiryDateContent, TransformDateFormat(lease.LeaseExpiryDate));
            
            if (lease.LeaseTerminationReason != "")
            {
                AssertTrueIsDisplayed(licenseDetailsViewTerminateDateLabel);
                AssertTrueContentEquals(licenseDetailsViewTerminateDateContent, TransformDateFormat(lease.LeaseTerminationDate));
                AssertTrueIsDisplayed(licenseDetailsTerminatedReasonLabel);
                AssertTrueContentEquals(licenseDetailsTerminateReasonContent, lease.LeaseTerminationReason);
                AssertTrueContentEquals(licenseDetailsViewTerminateCancelReason, lease.LeaseTerminationReason);
            }

            if (lease.LeaseCancellationReason != "")
            {
                AssertTrueIsDisplayed(licenseDetailsCancelReasonLabel);
                AssertTrueContentEquals(licenseDetailsCancelContent, lease.LeaseCancellationReason);
                AssertTrueContentEquals(licenseDetailsViewTerminateCancelReason, lease.LeaseCancellationReason);
            }

            //Renewal Option
            AssertTrueIsDisplayed(licenseDetailsRenewalTitle);
            if (lease.LeaseRenewals.Count > 0)
            {
                for (var i = 0; i < lease.LeaseRenewals.Count; i++)
                {
                    var elementIdx = i + 1;
                    AssertTrueContentEquals(By.XPath("//div[contains(text(),'Renewal Option')]/parent::div/parent::h2/following-sibling::div/div["+ elementIdx +"]/h2/div/div"), "Renewal " + elementIdx);
                    AssertTrueIsDisplayed(By.XPath("//div[contains(text(),'Renewal Option')]/parent::div/parent::h2/following-sibling::div/div["+ elementIdx +"]/div/div/div/label[contains(text(),'Exercised')]"));
                    AssertTrueContentEquals(By.XPath("//div[contains(text(),'Renewal Option')]/parent::div/parent::h2/following-sibling::div/div["+ elementIdx +"]/div/div/div/label[contains(text(),'Exercised')]/parent::div/following-sibling::div"), lease.LeaseRenewals[i].RenewalIsExercised);

                    AssertTrueIsDisplayed(By.XPath("//div[contains(text(),'Renewal Option')]/parent::div/parent::h2/following-sibling::div/div["+ elementIdx +"]/div/div[2]/div[1]/div/div/label[contains(text(),'Commencement')]"));
                    AssertTrueContentEquals(By.XPath("//div[contains(text(),'Renewal Option')]/parent::div/parent::h2/following-sibling::div/div["+ elementIdx +"]/div/div[2]/div[1]/div/div/label[contains(text(),'Commencement')]/parent::div/following-sibling::div"), TransformDateFormat(lease.LeaseRenewals[i].RenewalCommencementDate));

                    AssertTrueIsDisplayed(By.XPath("//div[contains(text(),'Renewal Option')]/parent::div/parent::h2/following-sibling::div/div["+ elementIdx +"]/div/div[2]/div[2]/div/div/label[contains(text(),'Expiry')]"));
                    AssertTrueContentEquals(By.XPath("//div[contains(text(),'Renewal Option')]/parent::div/parent::h2/following-sibling::div/div["+ elementIdx +"]/div/div[2]/div[2]/div/div/label[contains(text(),'Expiry')]/parent::div/following-sibling::div"), TransformDateFormat(lease.LeaseRenewals[i].RenewalExpiryDate));

                    AssertTrueIsDisplayed(By.XPath("//div[contains(text(),'Renewal Option')]/parent::div/parent::h2/following-sibling::div/div["+ elementIdx +"]/div/div[3]/div/label[contains(text(),'Notes')]"));
                    AssertTrueContentEquals(By.XPath("//div[contains(text(),'Renewal Option')]/parent::div/parent::h2/following-sibling::div/div["+ elementIdx +"]/div/div[3]/div/label[contains(text(),'Notes')]/parent::div/following-sibling::div"), lease.LeaseRenewals[i].RenewalNotes);
                }
            }

            //Lease Administration
            AssertTrueIsDisplayed(licenseDetailsAdmSubtitle);
            AssertTrueIsDisplayed(licenseDetailsProgramViewLabel);

            if(lease.Program != "")
                AssertTrueElementValueEquals(licenseDetailsProgramContent, lease.Program);

            if (lease.ProgramOther != "")
                AssertTrueElementValueEquals(licenseDetailsOtherProgramContent, lease.ProgramOther);

            AssertTrueIsDisplayed(licenseDetailsViewTypeLabel);

            if(lease.AdminType != "")
                AssertTrueElementValueEquals(licenseDetailsTypeContent, lease.AdminType);

            if (lease.TypeOther != "")
                AssertTrueElementValueEquals(licenseDetailsOtherTypeContent, lease.TypeOther);

            AssertTrueIsDisplayed(licenseDetailsReceivableToLabel);

            if(lease.AccountType != "")
                AssertTrueElementValueEquals(licenseDetailsReceivableToContent, lease.AccountType);

            AssertTrueIsDisplayed(licenseDetailsPurposeLabel);
            if (lease.LeasePurpose.Count > 0)
            {
                var purposesUI = GetViewFieldListContent(licenseDetailsPurposeContent);
                Assert.True(Enumerable.SequenceEqual(purposesUI, lease.LeasePurpose));
            }

            if (lease.PurposeOther != "")
            {
                var otherIdx = lease.LeasePurpose.FindIndex(a => a.Equals("Other*"));
                By licenseDetailsOtherPurposeContent = By.Id("input-leasePurposes["+ otherIdx +"].purposeOtherDescription");
                AssertTrueElementValueEquals(licenseDetailsOtherPurposeContent, lease.PurposeOther);
            }
                
            AssertTrueIsDisplayed(licenseDetailsInitiatorLabel);

            if (lease.Initiator != "")
                AssertTrueElementValueEquals(licenseDetailsInitiatorContent, lease.Initiator);

            AssertTrueIsDisplayed(licenseDetailsResponsibilityLabel);

            if(lease.Responsibility != "")
                AssertTrueElementValueEquals(licenseDetailsResponsibilityContent, lease.Responsibility);

            AssertTrueIsDisplayed(licenseDetailsEffectiveDateLabel);

            if (lease.EffectiveDate != "")
                AssertTrueContentEquals(licenseDetailsEffectiveDateContent, TransformDateFormat(lease.EffectiveDate));

            AssertTrueIsDisplayed(licenseDetailsMotiContactViewLabel);

            if(lease.MOTIContact != "")
                AssertTrueElementValueEquals(licenseDetailsMotiContactInput,lease.MOTIContact);

            AssertTrueIsDisplayed(licenseDetailsIntendedUseLabel);

            if(lease.IntendedUse != "")
                AssertTrueElementValueEquals(licenseDetailsIntendedUseTextarea, lease.IntendedUse);

            AssertTrueIsDisplayed(licenseDetailsCityArbitrationLabel);

            if (lease.ArbitrationCity != "")
                AssertTrueElementValueEquals(licenseDetailsCityArbitrationContent, lease.ArbitrationCity); 

            //Consultation
            AssertTrueIsDisplayed(licenseDetailsConsultationSubtitle);
            AssertTrueIsDisplayed(licenseDetailsFirstNationLabel);

            if(lease.FirstNation != "")
                AssertTrueContentEquals(licenseDetailsFirstNationContent, lease.FirstNation);

            AssertTrueIsDisplayed(licenseDetailsSRELabel);

            if(lease.StrategicRealEstate != "")
                AssertTrueContentEquals(licenseDetailsSREContent, lease.StrategicRealEstate);

            AssertTrueIsDisplayed(licenceDetailsRegionalPlanningLabel);

            if(lease.RegionalPlanning != "")
                AssertTrueContentEquals(licenceDetailsRegionalPlanningContent, lease.RegionalPlanning);

            AssertTrueIsDisplayed(licenseDetailsRegionalPropertyServicesLabel);

            if(lease.RegionalPropertyService != "")
                AssertTrueContentEquals(licenseDetailsRegionalPropertyServicesContent, lease.RegionalPropertyService);

            AssertTrueIsDisplayed(licenceDetailsDistrictLabel);

            if(lease.District != "")
                AssertTrueContentEquals(licenceDetailsDistrictContent, lease.District);

            AssertTrueIsDisplayed(licenseDetailsHeadquarterLabel);

            if(lease.Headquarter != "")
                AssertTrueContentEquals(licenseDetailsHeadquarterContent,lease.Headquarter);

            AssertTrueIsDisplayed(licenceDetailsOtherLabel);

            if(lease.ConsultationOther != "")
                AssertTrueContentEquals(licenceDetailsOtherContent, lease.ConsultationOther);

            //Fee Determination
            AssertTrueIsDisplayed(licenseDetailsFeeDeterminationSubtitle);

            AssertTrueIsDisplayed(licenseDetailsFeeDeterminationPublicBenefitLabel);
            if (lease.FeeDeterminationPublicBenefit != "")
            {
                IWebElement publicBenefit = webDriver.FindElement(licenseDetailsFeeDeterminationPublicBenefitInput);
                SelectElement selectedValue = new(publicBenefit);
                string selectedText = selectedValue.SelectedOption.Text;
                Assert.Equal(lease.FeeDeterminationPublicBenefit, selectedText);
            }

            AssertTrueIsDisplayed(licenseDetailsFeeDeterminationFinancialGainLabel);
            if (lease.FeeDeterminationFinancialGain != "")
            {
                IWebElement financialGain = webDriver.FindElement(licenseDetailsFeeDeterminationFinancialGainInput);
                SelectElement selectedValue = new(element: financialGain);
                string selectedText = selectedValue.SelectedOption.Text;
                Assert.Equal(lease.FeeDeterminationFinancialGain, selectedText);
            }

            AssertTrueIsDisplayed(licenseDetailsFeeDeterminationSuggestedFeeLabel);
            AssertTrueContentEquals(licenseDetailsFeeDeterminationSuggestedFeeContent, lease.FeeDeterminationSuggestedFee);

            AssertTrueIsDisplayed(licenseDetailsFeeDeterminationNotesLabel);
            AssertTrueContentEquals(licenseDetailsFeeDeterminationNotesContent, lease.FeeDeterminationNotes);

            //Documentation
            AssertTrueIsDisplayed(licenseDetailsPhysicalLeaseExistViewLabel);

            if (lease.PhysicalLeaseExist != "")
            {
                IWebElement physicalDocumentation = webDriver.FindElement(licenseDetailsPhysicalLeaseExistSelector);
                SelectElement selectedValue = new(physicalDocumentation);
                string selectedText = selectedValue.SelectedOption.Text;
                Assert.Equal(lease.PhysicalLeaseExist, selectedText);
            }

            AssertTrueIsDisplayed(licenseDetailsDigitalLeaseExistViewLabel);

            if (lease.DigitalLeaseExist != "")
            {
                IWebElement digitalDocumentation = webDriver.FindElement(licenseDetailsDigitalLeaseExistSelector);
                SelectElement selectedValue = new(digitalDocumentation);
                string selectedText = selectedValue.SelectedOption.Text;
                Assert.Equal(lease.DigitalLeaseExist, selectedText);
            }
                
            AssertTrueIsDisplayed(licenseDetailsLocationDocsLabel);

            if (lease.DocumentLocation != "")
                AssertTrueContentEquals(licenseDetailsLocationDocsContent, lease.DocumentLocation);

            AssertTrueIsDisplayed(licenseDetailsNotesLabel);

            if (lease.LeaseNotes != "")
                AssertTrueContentEquals(licenseDetailsNotesContent, lease.LeaseNotes) ;
        }

        public void VerifyLicensePropertyViewForm(List<LeaseProperty> properties)
        {
            VerifyLicensePropertiesHeader();

            AssertTrueIsDisplayed(leasePropertiesSubtitle);

            for (var i = 0; i < properties.Count; i++)
            {
                var elementIdx = i + 1;
                AssertTrueIsDisplayed(By.XPath("//div[contains(text(),'Property Information')]/parent::div/parent::h2/following-sibling::div/div["+ elementIdx +"]/div/div/label[contains(text(),'PID')]"));
                AssertTrueElementContains(By.XPath("//div[contains(text(),'Property Information')]/parent::div/parent::h2/following-sibling::div/div["+ elementIdx +"]/div[1]/div[2]"), "PID: " + properties[i].PID);

                AssertTrueIsDisplayed(By.XPath("//div[contains(text(),'Property Information')]/parent::div/parent::h2/following-sibling::div/div["+ elementIdx +"]/div/div/label[contains(text(),'Descriptive name')]"));
                if (properties[i].DescriptiveName != "")
                    AssertTrueElementValueEquals(By.Id("input-fileProperties."+ i +".propertyName"), properties[i].DescriptiveName);

                AssertTrueIsDisplayed(By.XPath("//div[contains(text(),'Property Information')]/parent::div/parent::h2/following-sibling::div/div["+ elementIdx +"]/div/div/label[contains(text(),'Area included')]"));
                if (properties[i].Area != "")
                {
                    AssertTrueContentEquals(By.XPath("//div[contains(text(),'Property Information')]/parent::div/parent::h2/following-sibling::div/div["+ elementIdx +"]/div[3]/div/div/div[1]/div/div[1]/div[1]"), TransformAreaNumberFormat(properties[i].Area));
                    AssertTrueIsDisplayed(By.XPath("//div[contains(text(),'Property Information')]/parent::div/parent::h2/following-sibling::div/div[1]/div[3]/div/div/div[1]/div/div[1]/div[contains(text(),'sq. metres')]"));
                }

                AssertTrueIsDisplayed(By.XPath("//div[contains(text(),'Property Information')]/parent::div/parent::h2/following-sibling::div/div["+ elementIdx +"]/div/div[1]/label[contains(text(),'Address')]"));
                if (properties[i].Address.AddressLine1 != "")
                {
                    AssertTrueElementValueEquals(By.Id("input-fileProperties."+ i +".property.address.streetAddress1"), properties[i].Address.AddressLine1);

                    if(properties[i].Address.AddressLine2 != "")
                        AssertTrueElementValueEquals(By.Id("input-fileProperties."+ i +".property.address.streetAddress2"), properties[i].Address.AddressLine2);

                    if (properties[i].Address.AddressLine3 != "")
                        AssertTrueElementValueEquals(By.Id("input-fileProperties."+ i +".property.address.streetAddress3"), properties[i].Address.AddressLine3);

                    if (properties[i].Address.City != "")
                        AssertTrueElementValueEquals(By.Id("input-fileProperties."+ i +".property.address.municipality"), properties[i].Address.City);

                    if (properties[i].Address.PostalCode != "")
                        AssertTrueElementValueEquals(By.Id("input-fileProperties."+ i +".property.address.postal"), properties[i].Address.PostalCode);
                }
                else
                {
                    AssertTrueIsDisplayed(By.XPath("//div[contains(text(),'Property Information')]/parent::div/parent::h2/following-sibling::div/div["+ elementIdx +"]/div/div[2]/label[contains(text(),'Address')]"));
                    AssertTrueIsDisplayed(By.XPath("//div[contains(text(),'Property Information')]/parent::div/parent::h2/following-sibling::div/div["+ elementIdx +"]/div/div[2]/p[contains(text(),'Address not available in PIMS')]"));
                }

                AssertTrueIsDisplayed(By.XPath("//div[contains(text(),'Property Information')]/parent::div/parent::h2/following-sibling::div/div["+ elementIdx +"]/div[5]/div/label[contains(text(),'Legal description')]"));
                if (properties[i].LegalDescription != "")
                    AssertTrueContentEquals(By.XPath("//div[contains(text(),'Property Information')]/parent::div/parent::h2/following-sibling::div/div["+ elementIdx +"]/div[5]/div[2]"), properties[i].LegalDescription);

            }
        }

        
    }
}
