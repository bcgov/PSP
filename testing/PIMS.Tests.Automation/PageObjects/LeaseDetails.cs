using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class LeaseDetails : PageObjectBase
    {
        //Main Menu links Elements
        private By menuManagementButton = By.CssSelector("div[data-testid='nav-tooltip-leases&licences'] a");
        private By createLicenseButton = By.XPath("//a[contains(text(),'Create a Lease/Licence File')]");

        //File Details Edit Icon
        private By licenseDetailsEditIcon = By.XPath("//div[@role='tabpanel']/div/div/div/button");

        //Lease Header Elements
        private By licenseHeaderNbrLabel = By.XPath("//label[contains(text(),'Lease/Licence #')]");
        private By licenseHeaderNbrContent = By.XPath("//label[contains(text(),'Lease/Licence #')]/parent::strong/parent::div/following-sibling::div/span[1]");
        private By licenseHeaderAccountType = By.XPath("//label[contains(text(),'Lease/Licence #')]/parent::strong/parent::div/following-sibling::div/span[2]");
        private By licenseHeaderProperty = By.XPath("//label[contains(text(),'Property')]");
        private By licenseHeaderPropertyContent = By.XPath("//label[contains(text(),'Property')]/parent::strong/parent::div/following-sibling::div/div/span");
        private By licenseHeaderTenantLabel = By.XPath("//label[contains(text(),'Tenant')]");
        private By licenseHeaderStartDateLabel = By.XPath("//label[contains(text(),'Lease Start')]");
        private By licenseHeaderStartDateContent = By.XPath("//label[contains(text(),'Lease Start')]/parent::strong/parent::div/following-sibling::div[1]");
        private By licenseHeaderExpiryDateLabel = By.XPath("//label[contains(text(),'Lease Start')]/parent::strong/parent::div/following-sibling::div[2]/strong/label[contains(text(),'Expiry')]");
        private By licenseHeaderExpiryDateContent = By.XPath("//label[contains(text(),'Lease Start')]/parent::strong/parent::div/following-sibling::div[3]/span");
        private By licenseHeaderHistoricalFileLabel = By.XPath("//label[contains(text(),'Historical File')]");
        private By licenseHeaderHistoricalFileContent = By.XPath("//label[contains(text(),'Historical File #:')]/parent::strong/parent::div/following-sibling::div/div/span");

        private By licenseHeaderCreatedLabel = By.XPath("//span/strong[contains(text(),'Created')]");
        private By licenseHeaderCreatedContent = By.XPath("//strong[contains(text(),'Created')]/parent::span");
        private By licenseHeaderCreatedByContent = By.XPath("//strong[contains(text(),'Created')]/parent::span/span[@data-testid='tooltip-icon-userNameTooltip']");
        private By licenseHeaderLastUpdatedLabel = By.XPath("//span/strong[contains(text(),'Updated')]");
        private By licenseHeaderLastUpdatedContent = By.XPath("//strong[contains(text(),'Updated')]/parent::span");
        private By licenseHeaderLastUpdatedByContent = By.XPath("//strong[contains(text(),'Updated')]/parent::span/span[@data-testid='tooltip-icon-userNameTooltip']");
        private By licenseHeaderStatusContent = By.XPath("//b[contains(text(),'File')]/parent::span/following-sibling::div");
        private By licenseHeaderExpiredFlag = By.XPath("//label[contains(text(),'Lease Start')]/parent::strong/parent::div/following-sibling::div[4]/div");

        //Lease Details Elements
        private By licenseDetailsLeaseDateSubtitle = By.XPath("//form/div/div/div/div/h4[contains(text(),'Lease / Licence')]");
        private By licenseDetailsLeaseDateStartLabel = By.XPath("//form/div/div/div/div/h4[contains(text(),'Lease / Licence')]/parent::div/div/label[contains(text(),'Start date')]");
        private By licenseDetailsLeaseDateStartContent = By.XPath("//form/div/div/div/div/h4[contains(text(),'Lease / Licence')]/parent::div/div/label[contains(text(),'Start date')]/following-sibling::div");
        private By licenseDetailsLeaseDateEndLabel = By.XPath("//form/div/div/div/div/h4[contains(text(),'Lease / Licence')]/parent::div/div/label[contains(text(),'Expiry')]");
        private By licenseDetailsLeaseDateEndContent = By.XPath("//form/div/div/div/div/h4[contains(text(),'Lease / Licence')]/parent::div/div/label[contains(text(),'Expiry')]/following-sibling::div");
        private By licenseDetailsCurrentTermSubtitle = By.XPath("//form/div/div/div/div/h4[contains(text(),'Current Period')]");
        private By licenseDetailsCurrentTermStartLabel = By.XPath("//form/div/div/div/div/h4[contains(text(),'Current Period')]/parent::div/div/label[contains(text(),'Start date')]");
        private By licenseDetailsCurrentTermStartContent = By.XPath("//form/div/div/div/div/h4[contains(text(),'Current Period')]/parent::div/div/label[contains(text(),'Start date')]/following-sibling::div");
        private By licenseDetailsCurrentTermEndLabel = By.XPath("//form/div/div/div/div/h4[contains(text(),'Current Period')]/parent::div/div/label[contains(text(),'Expiry')]");
        private By licenseDetailsCurrentTermEndContent = By.XPath("//form/div/div/div/div/h4[contains(text(),'Current Period')]/parent::div/div/label[contains(text(),'Expiry')]/following-sibling::div");

        //Property Information Elements
        private By licenseDetailsPropertyInformationSubtitle = By.XPath("//h2/div/div[contains(text(),'Property Information')]");
        private By licenseDetailsPropertiesCount = By.XPath("//h2/div/div[contains(text(),'Property Information')]/parent::div/parent::h2/following-sibling::div/div");
        private By licenseDetailsProperty1DescriptiveNameLabel = By.XPath("//h2/div/div[contains(text(),'Property Information')]/parent::div/parent::h2/following-sibling::div/div[1]/div/div/label[contains(text(),'Descriptive name')]");
        private By licenseDetailsProperty1DescriptiveNameContent = By.Id("input-properties.0.propertyName");
        private By licenseDetailsProperty1AreaIncludedLabel = By.XPath("//h2/div/div[contains(text(),'Property Information')]/parent::div/parent::h2/following-sibling::div/div[1]/div/div/label[contains(text(),'Area included')]");
        private By licenseDetailsProperty1AreaIncludedContent = By.XPath("//h2/div/div[contains(text(),'Property Information')]/parent::div/parent::h2/following-sibling::div/div[1]/div/div/label[contains(text(),'Area included')]/parent::div/following-sibling::div");
        private By licenseDetailsProperty1AddressLabel = By.XPath("//h2/div/div[contains(text(),'Property Information')]/parent::div/parent::h2/following-sibling::div/div[1]/div/div[1]/label[contains(text(),'Address')]");
        private By licenseDetailsProperty1AddressNoContent = By.XPath("//h2/div/div[contains(text(),'Property Information')]/parent::div/parent::h2/following-sibling::div/div[1]/div/div[1]/label[contains(text(),'Address')]/parent::div/following-sibling::div/label");
        private By licenseDetailsProperty1AddressContent = By.Id("input-fileProperties.0.property.address.streetAddress1");
        private By licenseDetailsProperty1LegalDescripLabel = By.XPath("//h2/div/div[contains(text(),'Property Information')]/parent::div/parent::h2/following-sibling::div/div[1]/div/div/label[contains(text(),'Legal description')]");
        private By licenseDetailsProperty1LegalDescripContent = By.XPath("//h2/div/div[contains(text(),'Property Information')]/parent::div/parent::h2/following-sibling::div/div[1]/div/div/label[contains(text(),'Legal description')]/parent::div/following-sibling::div");

        //Create Lease Details Elements
        private By licenseCreateTitle = By.XPath("//h1[contains(text(),'Create Lease/Licence')]");

        private By licenseDetailsProjectSubtitle = By.XPath("//h2/div/div[contains(text(), 'Project')]");
        private By licenseDetailsProjectLabel = By.XPath("//label[contains(text(),'Ministry project')]");
        private By licenseDetailsProjectContent = By.XPath("//label[contains(text(),'Ministry project')]/parent::div/following-sibling::div");
        private By licenseDetailsProjectInput = By.CssSelector("input[id='typeahead-project']");
        private By licenseDetailsProject1stOption = By.CssSelector("div[id='typeahead-project'] a:nth-child(1)");

        private By licenseDetailsStatusLabel = By.XPath("//div/div/div/div/div/label[contains(text(),'Status')]");
        private By licenseDetailsStatusSelector = By.Id("input-statusTypeCode");
        private By licenseDetailsTerminateReasonInput = By.Id("input-terminationReason");
        private By licenseDetailsCancelReasonInput = By.Id("input-cancellationReason");
        private By licenseDetailsAccountTypeLabel = By.XPath("//label[contains(text(),'Account type')]");
        private By licenseDetailsAccountTypeSelector = By.Id("input-paymentReceivableTypeCode");
        private By licenseDetailsStartDateLabel = By.XPath("//label[contains(text(),'Start date')]");
        private By licenseDetailsStartDateInput = By.Id("datepicker-startDate");
        private By licenseDetailsExpiryDateLabel = By.XPath("//label[contains(text(),'Expiry date')]");
        private By licenseDetailsExpiryDateInput = By.Id("datepicker-expiryDate");

        private By licenseDetailsAdmSubtitle = By.XPath("//div[contains(text(),'Administration')]");
        private By licenseDetailsMotiContactViewLabel = By.XPath("//label[contains(text(),'MoTI contact')]");
        private By licenseDetailsMotiContactLabel = By.XPath("//label[contains(text(),'MOTI contact')]");
        private By licenseDetailsMotiContactInput = By.Id("input-motiName");
        private By licenseDetailsMotiRegionLabel = By.XPath("//label[contains(text(),'MOTI region')]");
        private By licenseDetailsMotiRegionSelector = By.Id("input-regionId");
        private By licenseDetailsProgramViewLabel = By.XPath("//label[contains(text(),'Program')]");
        private By licenseDetailsProgramContent = By.Id("input-programName");
        private By licenseDetailsProgramLabel = By.XPath("//select[@id='input-programTypeCode']/parent::div/parent::div/preceding-sibling::div/label[contains(text(),'Program')]");
        private By licenseDetailsProgramSelector = By.Id("input-programTypeCode");
        private By licenseDetailsOtherProgramLabel = By.XPath("//label[contains(text(),'Other Program')]");
        private By licenseDetailsOtherProgramInput = By.Id("input-otherProgramTypeDescription");
        private By licenseDetailsOtherProgramContent = By.Id("input-otherProgramType");
        private By licenseDetailsTypeLabel = By.XPath("//label[contains(text(),'Type')]");
        private By licenseDetailsTypeSelector = By.Id("input-leaseTypeCode");
        private By licenseDetailsViewTypeLabel = By.XPath("//label[contains(text(),'Account type')]");
        private By licenseDetailsTypeContent = By.Id("input-type.description");
        private By licenseDetailsOtherTypeLabel = By.XPath("//input[@id='input-otherLeaseTypeDescription']/parent::div/parent::div/preceding-sibling::div/label[contains(text(),'Describe other')]");
        private By licenseDetailsOtherTypeInput = By.Id("input-otherLeaseTypeDescription");
        private By licenseDetailsOtherTypeContent = By.Id("input-otherType");
        private By licenseDetailsReceivableToLabel = By.XPath("//label[contains(text(),'Receivable to')]");
        private By licenseDetailsReceivableToContent = By.Id("input-paymentReceivableType.description");
        private By licenseDetailsCategoryLabel = By.XPath("//label[contains(text(),'Category')]");
        private By licenseDetailsCategorySelector = By.Id("input-categoryTypeCode");
        private By licenseDetailsCategoryContent = By.Id("input-categoryType.description");
        private By licenseDetailsCategoryOtherLabel = By.XPath("//input[@id='input-otherCategoryTypeDescription']/parent::div/parent::div/preceding-sibling::div/label[contains(text(),'Describe other')]");
        private By licenseDetailsCategoryOtherInput = By.Id("input-otherCategoryTypeDescription");
        private By licenseDetailsCategoryOtherContent = By.Id("input-otherCategoryType");
        private By licenseDetailsPurposeLabel = By.XPath("//label[contains(text(),'Purpose')]");
        private By licenseDetailsPurposeSelector = By.Id("input-purposeTypeCode");
        private By licenseDetailsPurposeContent = By.Id("input-purposeType.description");
        private By licenseDetailsOtherPurposeLabel = By.XPath("//input[@id='input-otherPurposeTypeDescription']/parent::div/parent::div/preceding-sibling::div/label[contains(text(),'Describe other')]");
        private By licenseDetailsOtherPurposeInput = By.Id("input-otherPurposeTypeDescription");
        private By licenseDetailsOtherPurposeContent = By.Id("input-otherPurposeType");
        private By licenseDetailsInitiatorLabel = By.XPath("//label[contains(text(),'Initiator')]");
        private By licenseDetailsInitiatorTooltip = By.XPath("//label[contains(text(),'Initiator')]/span/span[@data-testid='tooltip-icon-section-field-tooltip']");
        private By licenseDetailsInitiatorSelector = By.Id("input-initiatorTypeCode");
        private By licenseDetailsInitiatorContent = By.Id("input-initiatorType.description");
        private By licenseDetailsResponsibilityLabel = By.XPath("//label[contains(text(),'Responsibility')]");
        private By licenseDetailsResponsibilityTooltip = By.XPath("//label[contains(text(),'Responsibility')]/span/span[@data-testid='tooltip-icon-section-field-tooltip']");
        private By licenseDetailsResposibilitySelector = By.Id("input-responsibilityTypeCode");
        private By licenseDetailsResponsibilityContent = By.Id("input-responsibilityType.description");
        private By licenseDetailsEffectiveDateLabel = By.XPath("//label[contains(text(),'Effective date')]");
        private By licenseDetailsEffectiveDateInput = By.Id("datepicker-responsibilityEffectiveDate");
        private By licenseDetailsEffectiveDateContent = By.XPath("//label[contains(text(),'Effective date')]/parent::div/following-sibling::div");
        private By licenseDetailsIntendedUseLabel = By.XPath("//label[contains(text(),'Intended use')]");
        private By licenseDetailsIntendedUseTextarea = By.Id("input-description");

        private By licenseDetailsConsultationSubtitle = By.XPath("//div[contains(text(),'Consultation')]");
        private By licenseDetailsFirstNationLabel = By.XPath("//label[contains(text(),'First nation')]");
        private By licenseDetailsFirstNationSelect = By.Id("input-consultations.0.consultationStatusType");
        private By licenseDetailsFirstNationContent = By.XPath("//label[contains(text(),'First nation')]/parent::div/following-sibling::div");
        private By licenseDetailsSRELabel = By.XPath("//label[contains(text(),'Strategic Real Estate (SRE)')]");
        private By licenseDetailsSRESelect = By.Id("input-consultations.1.consultationStatusType");
        private By licenseDetailsSREContent = By.XPath("//label[contains(text(),'Strategic Real Estate (SRE)')]/parent::div/following-sibling::div");
        private By licenceDetailsRegionalPlanningLabel = By.XPath("//label[contains(text(),'Regional planning')]");
        private By licenseDetailsRegionalPlanningSelect = By.Id("input-consultations.2.consultationStatusType");
        private By licenceDetailsRegionalPlanningContent = By.XPath("//label[contains(text(),'Regional planning')]/parent::div/following-sibling::div");
        private By licenseDetailsRegionalPropertyServicesLabel = By.XPath("//label[contains(text(),'Regional property services')]");
        private By licenseDetailsRegionalPropertyServicesSelect = By.Id("input-consultations.3.consultationStatusType");
        private By licenseDetailsRegionalPropertyServicesContent = By.XPath("//label[contains(text(),'Regional property services')]/parent::div/following-sibling::div");
        private By licenceDetailsDistrictLabel = By.XPath("//label[contains(text(),'District')]");
        private By licenseDetailsDistrictSelect = By.Id("input-consultations.4.consultationStatusType");
        private By licenceDetailsDistrictContent = By.XPath("//label[contains(text(),'District')]/parent::div/following-sibling::div");
        private By licenseDetailsHeadquarterLabel = By.XPath("//label[contains(text(),'Headquarter (HQ)')]");
        private By licenseDetailsHeadquarterSelect = By.Id("input-consultations.5.consultationStatusType");
        private By licenseDetailsHeadquarterContent = By.XPath("//label[contains(text(),'Headquarter (HQ)')]/parent::div/following-sibling::div");
        private By licenceDetailsOtherLabel = By.XPath("//label[contains(text(),'Other')]");
        private By licenseDetailsOtherSelect = By.Id("input-consultations.6.consultationStatusType");
        private By licenceDetailsOtherContent = By.XPath("//label[contains(text(),'Other')]/parent::div/following-sibling::div");
        private By licenseDetailsOtherDetailsInput = By.Id("input-consultations.6.consultationTypeOtherDescription");

        private By licenseDetailsDocsSubtitle = By.XPath("//div[contains(text(),'Documentation')]");
        private By licenseDetailsPhysicalLeaseExistViewLabel = By.XPath("//label[contains(text(),'Physical copy exists')]");
        private By licenseDetailsPhysicalLeaseExistLabel = By.XPath("//label[contains(text(),'Physical lease/licence exists')]");
        private By licenseDetailsPhysicalLeaseExistSelector = By.Id("input-hasPhysicalLicense");
        private By licenseDetailsDigitalLeaseExistViewLabel = By.XPath("//label[contains(text(),'Digital copy exists')]");
        private By licenseDetailsDigitalLeaseExistLabel = By.XPath("//label[contains(text(),'Digital lease/licence exists')]");
        private By licenseDetailsDigitalLeaseExistSelector = By.Id("input-hasDigitalLicense");
        private By licenseDetailsLocationDocsLabel = By.XPath("//label[contains(text(),'Document location')]");
        private By licenseDetailsLocationDocsTooltip = By.XPath("//label[contains(text(),'Document location')]/span/span[@data-testid='tooltip-icon-section-field-tooltip']");
        private By licenseDetailsLocationDocsTextarea = By.Id("input-documentationReference");
        private By licenseDetailsLocationDocsContent = By.XPath("//label[contains(text(),'Document location')]/parent::div/following-sibling::div");
        private By licenseDetailsNotesLabel = By.XPath("//label[contains(text(),'Lease notes')]");
        private By licenseDetailsNotesTextarea = By.Id("input-note");
        private By licenseDetailsNotesContent = By.XPath("//label[contains(text(),'Lease notes')]/parent::div/following-sibling::div");

        private By licenseDetailsSaveButton = By.XPath("//div[contains(text(),'Save')]/parent::button");
        private By licenseDetailsCancelButton = By.XPath("//div[contains(text(),'Cancel')]/parent::button");

        private By licenseDetailsModalPIDAttached = By.CssSelector("div[class='modal-content']");

        //Leases Modal Element
        private By licenseDetailsConfirmationModal = By.CssSelector("div[class='modal-content']");
        private By licenseDetailsConfirmationContent = By.CssSelector("div[class='modal-content'] p");

        private SharedFileProperties sharedSearchProperties;
        private SharedModals sharedModals;

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
                webDriver.FindElement(licenseDetailsStartDateInput).SendKeys(lease.LeaseStartDate);
                webDriver.FindElement(licenseDetailsStartDateInput).SendKeys(Keys.Enter);
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

            //Selecting Category if required
            if (webDriver.FindElements(licenseDetailsCategorySelector).Count > 0 && lease.Category != "")
            {
                Assert.True(webDriver.FindElement(licenseDetailsCategoryLabel).Displayed);
                ChooseSpecificSelectOption(licenseDetailsCategorySelector, lease.Category);
            }

            //If Other Category has been selected
            if (webDriver.FindElements(licenseDetailsCategoryOtherInput).Count > 0 && lease.CategoryOther != "")
            {
                Assert.True(webDriver.FindElement(licenseDetailsCategoryOtherLabel).Displayed);
                webDriver.FindElement(licenseDetailsCategoryOtherInput).SendKeys(lease.CategoryOther);
            }

            //Purpose
            if(lease.LeasePurpose != "")
                ChooseSpecificSelectOption(licenseDetailsPurposeSelector, lease.LeasePurpose);

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
                SelectElement selectStatusElement = new SelectElement(webDriver.FindElement(licenseDetailsStatusSelector));
                IWebElement selectedStatus = selectStatusElement.SelectedOption;
                string previousStatus = selectedStatus.Text;

                ChooseSpecificSelectOption(licenseDetailsStatusSelector, lease.LeaseStatus);

                Wait();
                if (webDriver.FindElements(licenseDetailsConfirmationModal).Count() > 0)
                {
                    Assert.Equal("Are you sure?", sharedModals.ModalHeader());
                    Assert.Contains("The lease is no longer in " + previousStatus + " state. The reason for doing so will be cleared from the file details and can only be viewed in the notes tab.", webDriver.FindElement(licenseDetailsConfirmationContent).Text);
                    Assert.Contains("Do you want to proceed?", webDriver.FindElement(licenseDetailsConfirmationContent).Text);
                    sharedModals.ModalClickOKBttn();
                }
            }
            
            //Termination reason
            if (lease.LeaseTerminationReason != "")
            {
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

            //Account Type
            if (lease.AccountType != "")
            {
                webDriver.FindElement(licenseDetailsAccountTypeSelector).Click();
                ChooseSpecificSelectOption(licenseDetailsAccountTypeSelector, lease.AccountType);
            }

            //Start Date
            if (lease.LeaseStartDate != "")
            {
                ClearInput(licenseDetailsStartDateInput);
                webDriver.FindElement(licenseDetailsStartDateInput).SendKeys(lease.LeaseStartDate);
                webDriver.FindElement(licenseDetailsStartDateInput).SendKeys(Keys.Enter);
            }
               
            //Expiry Date
            if (lease.LeaseExpiryDate != "")
            {
                ClearInput (licenseDetailsExpiryDateInput);
                webDriver.FindElement(licenseDetailsExpiryDateInput).Click();
                webDriver.FindElement(licenseDetailsExpiryDateInput).SendKeys(lease.LeaseExpiryDate);
                webDriver.FindElement(licenseDetailsExpiryDateInput).SendKeys(Keys.Enter);
            }

            //Administration Details
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

            //Selecting Category if required
            if (webDriver.FindElements(licenseDetailsCategorySelector).Count > 0 && lease.Category != "")
            {
                WaitUntilVisible(licenseDetailsCategorySelector);
                Assert.True(webDriver.FindElement(licenseDetailsCategoryLabel).Displayed);
                ChooseSpecificSelectOption(licenseDetailsCategorySelector, lease.Category);
            }

            //If Other Category has been selected
            if (webDriver.FindElements(licenseDetailsCategoryOtherInput).Count > 0 && lease.CategoryOther != "")
            {
                Assert.True(webDriver.FindElement(licenseDetailsCategoryOtherLabel).Displayed);
                webDriver.FindElement(licenseDetailsCategoryOtherInput).SendKeys(lease.CategoryOther);
            }

            //Purpose
            if (lease.LeasePurpose != "")
                ChooseSpecificSelectOption(licenseDetailsPurposeSelector, lease.LeasePurpose);

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
                if (webDriver.FindElements(licenseDetailsConfirmationModal).Count() > 0)
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
            WaitUntilVisible(licenseDetailsProjectLabel);

            //Create Title
            AssertTrueIsDisplayed(licenseCreateTitle);

            //Details
            AssertTrueIsDisplayed(licenseDetailsProjectLabel);
            AssertTrueIsDisplayed(licenseDetailsProjectInput);
            AssertTrueIsDisplayed(licenseDetailsStatusLabel);
            AssertTrueIsDisplayed(licenseDetailsStatusSelector);
            AssertTrueIsDisplayed(licenseDetailsAccountTypeLabel);
            AssertTrueIsDisplayed(licenseDetailsAccountTypeSelector);
            AssertTrueIsDisplayed(licenseDetailsStartDateLabel);
            AssertTrueIsDisplayed(licenseDetailsStartDateInput);
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
            AssertTrueIsDisplayed(licenseDetailsPurposeSelector);
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

        public void VerifyLicenseHeader(Lease lease)
        {
            AssertTrueIsDisplayed(licenseHeaderNbrLabel);
            AssertTrueContentNotEquals(licenseHeaderNbrContent, "");
            AssertTrueContentNotEquals(licenseHeaderAccountType, "");
            AssertTrueIsDisplayed(licenseHeaderProperty);
            AssertTrueIsDisplayed(licenseHeaderPropertyContent);
            AssertTrueIsDisplayed(licenseHeaderTenantLabel);
            AssertTrueIsDisplayed(licenseHeaderStartDateLabel);
            AssertTrueContentNotEquals(licenseHeaderStartDateContent, "");
            AssertTrueIsDisplayed(licenseHeaderExpiryDateLabel);
            AssertTrueIsDisplayed(licenseHeaderExpiryDateContent);
            AssertTrueIsDisplayed(licenseHeaderHistoricalFileLabel);
            //Assert.True(webDriver.FindElements(licenseHeaderHistoricalFileContent).Count > 0);
            AssertTrueIsDisplayed(licenseHeaderCreatedLabel);
            AssertTrueContentNotEquals(licenseHeaderCreatedContent, "");
            AssertTrueContentNotEquals(licenseHeaderCreatedByContent, "");
            AssertTrueIsDisplayed(licenseHeaderLastUpdatedLabel);
            AssertTrueContentNotEquals(licenseHeaderLastUpdatedContent, "");
            AssertTrueContentNotEquals(licenseHeaderLastUpdatedByContent, "");

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

        public void VerifyLicenseDetailsViewForm(Lease lease)
        {
            VerifyLicenseHeader(lease);

            //Edit Icon
            AssertTrueIsDisplayed(licenseDetailsEditIcon);

            //Project
            AssertTrueIsDisplayed(licenseDetailsProjectSubtitle);
            AssertTrueIsDisplayed(licenseDetailsProjectLabel);

            if (lease.MinistryProject != "")
                AssertTrueContentEquals(licenseDetailsProjectContent, lease.MinistryProjectCode + " - " + lease.MinistryProject);

            //Lease Start and End Date
            //To-Do - Calculate Start date and expiry date
            AssertTrueIsDisplayed(licenseDetailsLeaseDateSubtitle);
            AssertTrueIsDisplayed(licenseDetailsLeaseDateStartLabel);
            AssertTrueIsDisplayed(licenseDetailsLeaseDateStartContent);
            AssertTrueIsDisplayed(licenseDetailsLeaseDateEndLabel);

            if(lease.LeaseExpiryDate != "")
                AssertTrueIsDisplayed(licenseDetailsLeaseDateEndContent);

            //Lease Current Term
            //To-Do - Calculate Term Dates
            AssertTrueIsDisplayed(licenseDetailsCurrentTermSubtitle);
            AssertTrueIsDisplayed(licenseDetailsCurrentTermStartLabel);
            //Assert.True(webDriver.FindElement(licenseDetailsCurrentTermStartContent).Displayed);
            AssertTrueIsDisplayed(licenseDetailsCurrentTermEndLabel);
            //Assert.True(webDriver.FindElement(licenseDetailsCurrentTermEndContent).Displayed);

            //Lease 1st Property Information
            AssertTrueIsDisplayed(licenseDetailsPropertyInformationSubtitle);
            AssertTrueIsDisplayed(licenseDetailsProperty1DescriptiveNameLabel);
            //Assert.True(webDriver.FindElement(licenseDetailsProperty1DescriptiveNameContent).Displayed);
            AssertTrueIsDisplayed(licenseDetailsProperty1AreaIncludedLabel);
            AssertTrueIsDisplayed(licenseDetailsProperty1AreaIncludedContent);
            AssertTrueIsDisplayed(licenseDetailsProperty1AddressLabel);

            if (webDriver.FindElements(licenseDetailsProperty1AddressNoContent).Count() > 0)
                AssertTrueIsDisplayed(licenseDetailsProperty1AddressNoContent);
            else
                AssertTrueIsDisplayed(licenseDetailsProperty1AddressContent);

            AssertTrueIsDisplayed(licenseDetailsProperty1LegalDescripLabel);
            AssertTrueIsDisplayed(licenseDetailsProperty1LegalDescripContent);

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

            AssertTrueIsDisplayed(licenseDetailsCategoryLabel);

            if(lease.Category != "")
                AssertTrueElementValueEquals(licenseDetailsCategoryContent, lease.Category);

            if (lease.CategoryOther != "")
                AssertTrueElementValueEquals(licenseDetailsCategoryOtherContent, lease.CategoryOther);

            AssertTrueIsDisplayed(licenseDetailsPurposeLabel);

            if(lease.LeasePurpose != "")
                AssertTrueElementValueEquals(licenseDetailsPurposeContent, lease.LeasePurpose);

            if (lease.PurposeOther != "")
                AssertTrueElementValueEquals(licenseDetailsOtherPurposeContent, lease.PurposeOther);

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

            //Documentation
            AssertTrueIsDisplayed(licenseDetailsPhysicalLeaseExistViewLabel);

            if (lease.PhysicalLeaseExist != "")
            {
                IWebElement physicalDocumentation = webDriver.FindElement(licenseDetailsPhysicalLeaseExistSelector);
                SelectElement selectedValue = new SelectElement(physicalDocumentation);
                string selectedText = selectedValue.SelectedOption.Text;
                Assert.Equal(lease.PhysicalLeaseExist, selectedText);
            }

            AssertTrueIsDisplayed(licenseDetailsDigitalLeaseExistViewLabel);

            if (lease.DigitalLeaseExist != "")
            {
                IWebElement digitalDocumentation = webDriver.FindElement(licenseDetailsDigitalLeaseExistSelector);
                SelectElement selectedValue = new SelectElement(digitalDocumentation);
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

        public void VerifyLicenseDetailsUpdateForm()
        {
            WaitUntilVisible (licenseDetailsStatusLabel);

            //Details
            AssertTrueIsDisplayed(licenseDetailsStatusLabel);
            AssertTrueIsDisplayed(licenseDetailsStatusSelector);
            AssertTrueIsDisplayed(licenseDetailsAccountTypeLabel);
            AssertTrueIsDisplayed(licenseDetailsAccountTypeSelector);
            AssertTrueIsDisplayed(licenseDetailsStartDateLabel);
            AssertTrueIsDisplayed(licenseDetailsStartDateInput);
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

            if (webDriver.FindElements(licenseDetailsOtherProgramLabel).Count() > 0)
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
            AssertTrueIsDisplayed(licenseDetailsPurposeSelector);

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
    }
}
