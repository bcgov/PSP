using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace PIMS.Tests.Automation.PageObjects
{
    public class LeaseDetails : PageObjectBase
    {
        //Main Menu links Elements
        private By menuManagementButton = By.XPath("//a/label[contains(text(),'Management')]/parent::a");
        private By createLicenseButton = By.XPath("//a[contains(text(),'Create a Lease/License File')]");

        //Leases Tabs
        private By licenseDetailsLink = By.XPath("//a[contains(text(),'File details')]");
        private By licenseDocumentsLink = By.XPath("//a[contains(text(),'Documents')]");

        //File Details Edit Icon
        private By licenseDetailsEditIcon = By.XPath("//div[@role='tabpanel'][1]/div/div/button");

        //Lease Header Elements
        private By licenseHeaderNbrLabel = By.XPath("//label[contains(text(),'Lease/License #')]");
        private By licenseHeaderNbrContent = By.XPath("//label[contains(text(),'Lease/License #')]/parent::div/following-sibling::div/strong/div/span[1]");
        private By licenseHeaderAccountType = By.XPath("//label[contains(text(),'Lease/License #')]/parent::div/following-sibling::div/strong/div/span[2]");
        private By licenseHeaderProperty = By.XPath("//label[contains(text(),'Property')]");
        private By licenseHeaderPropertyContent = By.XPath("//label[contains(text(),'Property')]/parent::div/following-sibling::div/strong");
        private By licenseHeaderTenantLabel = By.XPath("//label[contains(text(),'Tenant')]");
        private By licenseHeaderStartDateLabel = By.XPath("//div/div/div/div/div/div/div/div/div/div/div/div/label[contains(text(),'Start date')]");
        private By licenseHeaderStartDateContent = By.XPath("//div/div/div/div/div/div/div/div/div/div/div/div/label[contains(text(),'Start date')]/parent::div/following-sibling::div[1]");
        private By licenseHeaderExpiryDateLabel = By.XPath("//label[contains(text(),'Expiry date')]");
        private By licenseHeaderExpiryDateContent = By.XPath("//label[contains(text(),'Expiry date')]/parent::div/following-sibling::div");
        private By licenseHeaderCreatedLabel = By.XPath("//span[contains(text(),'Created')]");
        private By licenseHeaderCreatedContent = By.XPath("//span[contains(text(),'Created')]/strong");
        private By licenseHeaderCreatedByContent = By.XPath("//span[contains(text(),'Created')]/span");
        private By licenseHeaderLastUpdatedLabel = By.XPath("//span[contains(text(),'Last updated')]");
        private By licenseHeaderLastUpdatedContent = By.XPath("//span[contains(text(),'Last updated')]/strong");
        private By licenseHeaderLastUpdatedByContent = By.XPath("//span[contains(text(),'Last updated')]/span");
        private By licenseHeaderStatusLabel = By.XPath("//Label[contains(text(),'Status')]");
        private By licenseHeaderStatusContent = By.XPath("//Label[contains(text(),'Status')]/parent::div/following-sibling::div");
        private By licenseHeaderExpiredFlag = By.XPath("//label[contains(text(),'Expiry date')]/parent::div/parent::div/parent::div/parent::div/parent::div/following-sibling::div/div");

        //Lease and Current Term Info Elements
        private By licenseDetailsLeaseDateSubtitle = By.XPath("//form/div/div/div/div/h4[contains(text(),'Lease / License')]");
        private By licenseDetailsLeaseDateStartLabel = By.XPath("//form/div/div/div/div/h4[contains(text(),'Lease / License')]/parent::div/div/label[contains(text(),'Start date')]");
        private By licenseDetailsLeaseDateStartContent = By.XPath("//form/div/div/div/div/h4[contains(text(),'Lease / License')]/parent::div/div/label[contains(text(),'Start date')]/following-sibling::div");
        private By licenseDetailsLeaseDateEndLabel = By.XPath("//form/div/div/div/div/h4[contains(text(),'Lease / License')]/parent::div/div/label[contains(text(),'Expiry')]");
        private By licenseDetailsLeaseDateEndContent = By.XPath("//form/div/div/div/div/h4[contains(text(),'Lease / License')]/parent::div/div/label[contains(text(),'Expiry')]/following-sibling::div");
        private By licenseDetailsCurrentTermSubtitle = By.XPath("//form/div/div/div/div/h4[contains(text(),'Current Term')]");
        private By licenseDetailsCurrentTermStartLabel = By.XPath("//form/div/div/div/div/h4[contains(text(),'Current Term')]/parent::div/div/label[contains(text(),'Start date')]");
        private By licenseDetailsCurrentTermStartContent = By.XPath("//form/div/div/div/div/h4[contains(text(),'Current Term')]/parent::div/div/label[contains(text(),'Start date')]/following-sibling::div");
        private By licenseDetailsCurrentTermEndLabel = By.XPath("//form/div/div/div/div/h4[contains(text(),'Current Term')]/parent::div/div/label[contains(text(),'Expiry')]");
        private By licenseDetailsCurrentTermEndContent = By.XPath("//form/div/div/div/div/h4[contains(text(),'Current Term')]/parent::div/div/label[contains(text(),'Expiry')]/following-sibling::div");

        //Property Information Elements
        private By licenseDetailsPropertyInformationSubtitle = By.XPath("//h2/div/div[contains(text(),'Property Information')]");
        private By licenseDetailsPropertiesCount = By.XPath("//h2/div/div[contains(text(),'Property Information')]/parent::div/parent::h2/following-sibling::div/div");
        private By licenseDetailsProperty1DescriptiveNameLabel = By.XPath("//h2/div/div[contains(text(),'Property Information')]/parent::div/parent::h2/following-sibling::div/div[1]/div/div/label[contains(text(),'Descriptive name')]");
        private By licenseDetailsProperty1DescriptiveNameContent = By.Id("input-properties.0.propertyName");
        private By licenseDetailsProperty1AreaIncludedLabel = By.XPath("//h2/div/div[contains(text(),'Property Information')]/parent::div/parent::h2/following-sibling::div/div[1]/div/div/label[contains(text(),'Area included')]");
        private By licenseDetailsProperty1AreaIncludedContent = By.XPath("//h2/div/div[contains(text(),'Property Information')]/parent::div/parent::h2/following-sibling::div/div[1]/div/div/label[contains(text(),'Area included')]/parent::div/following-sibling::div");
        private By licenseDetailsProperty1AddressLabel = By.XPath("//h2/div/div[contains(text(),'Property Information')]/parent::div/parent::h2/following-sibling::div/div[1]/div/div[1]/label[contains(text(),'Address')]");
        private By licenseDetailsProperty1AddressNoContent = By.XPath("//h2/div/div[contains(text(),'Property Information')]/parent::div/parent::h2/following-sibling::div/div[1]/div/div[1]/label[contains(text(),'Address')]/parent::div/following-sibling::div/label");
        private By licenseDetailsProperty1AddressContent = By.Id("input-properties.0.address.streetAddress1");
        private By licenseDetailsProperty1LegalDescripLabel = By.XPath("//h2/div/div[contains(text(),'Property Information')]/parent::div/parent::h2/following-sibling::div/div[1]/div/div/label[contains(text(),'Legal description')]");
        private By licenseDetailsProperty1LegalDescripContent = By.XPath("//h2/div/div[contains(text(),'Property Information')]/parent::div/parent::h2/following-sibling::div/div[1]/div/div/label[contains(text(),'Legal description')]/parent::div/following-sibling::div");

        //Create Lease Details Elements
        private By licenseCreateTitle = By.XPath("//h1[contains(text(),'Create Lease/License')]");

        private By licenseDetailsProjectLabel = By.XPath("//label[contains(text(),'Ministry project')]");
        private By licenseDetailsProjectInput = By.CssSelector("input[id='typeahead-project']");
        private By licenseDetailsProjectOptions = By.CssSelector("div[id='typeahead-project']");
        private By licenseDetailsProject1stOption = By.CssSelector("div[id='typeahead-project'] a:nth-child(1)");
        private By licenseDetailsStatusLabel = By.XPath("//form/div/div/div/div/label[contains(text(),'Status')]");
        private By licenseDetailsStatusSelector = By.Id("input-statusTypeCode");
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
        private By licenseDetailsTypeLabel = By.XPath("//label[contains(text(),'Type')]");
        private By licenseDetailsTypeSelector = By.Id("input-leaseTypeCode");
        private By licenseDetailsViewTypeLabel = By.XPath("//label[contains(text(),'Account type')]");
        private By licenseDetailsTypeContent = By.Id("input-type.description");
        private By licenseDetailsOtherTypeLabel = By.XPath("//input[@id='input-otherLeaseTypeDescription']/parent::div/parent::div/preceding-sibling::div/label[contains(text(),'Describe other')]");
        private By licenseDetailsOtherTypeInput = By.Id("input-otherLeaseTypeDescription");
        private By licenseDetailsReceivableToLabel = By.XPath("//label[contains(text(),'Receivable to')]");
        private By licenseDetailsReceivableToContent = By.Id("input-paymentReceivableType.description");
        private By licenseDetailsCategoryLabel = By.XPath("//label[contains(text(),'Category')]");
        private By licenseDetailsCategorySelector = By.Id("input-categoryTypeCode");
        private By licenseDetailsCategoryContent = By.Id("input-categoryType.description");
        private By licenseDetailsCategoryOtherLabel = By.XPath("//input[@id='input-otherCategoryTypeDescription']/parent::div/parent::div/preceding-sibling::div/label[contains(text(),'Describe other')]");
        private By licenseDetailsCategoryOtherInput = By.Id("input-otherCategoryTypeDescription");
        private By licenseDetailsPurposeLabel = By.XPath("//label[contains(text(),'Purpose')]");
        private By licenseDetailsPurposeSelector = By.Id("input-purposeTypeCode");
        private By licenseDetailsPurposeContent = By.Id("input-purposeType.description");
        private By licenseDetailsOtherPurposeLabel = By.XPath("//input[@id='input-otherPurposeTypeDescription']/parent::div/parent::div/preceding-sibling::div/label[contains(text(),'Describe other')]");
        private By licenseDetailsOtherPurposeInput = By.Id("input-otherPurposeTypeDescription");
        private By licenseDetailsInitiatorLabel = By.XPath("//label[contains(text(),'Initiator')]");
        private By licenseDetailsInitiatorTooltip = By.XPath("//label[contains(text(),'Initiator')]/span/span[@data-testid='tooltip-icon-section-field-tooltip']");
        private By licenseDetailsInitiatorSelector = By.Id("input-initiatorTypeCode");
        private By licenseDetailsInitiatorContent = By.Id("input-responsibilityType.description");
        private By licenseDetailsResponsibilityLabel = By.XPath("//label[contains(text(),'Responsibility')]");
        private By licenseDetailsResponsibilityTooltip = By.XPath("//label[contains(text(),'Responsibility')]/span/span[@data-testid='tooltip-icon-section-field-tooltip']");
        private By licenseDetailsResposibilitySelector = By.Id("input-responsibilityTypeCode");
        private By licenseDetailsResponsibilityContent = By.Id("input-responsibilityType.description");
        private By licenseDetailsEffectiveDateLabel = By.XPath("//label[contains(text(),'Effective date')]");
        private By licenseDetailsEffectiveDateInput = By.Id("datepicker-responsibilityEffectiveDate");
        private By licenseDetailsEffectiveDateContent = By.XPath("//label[contains(text(),'Effective date')]/parent::div/following-sibling::div");
        private By licenseDetailsIntendedUseLabel = By.XPath("//label[contains(text(),'Intended use')]");
        private By licenseDetailsIntendedUseTextarea = By.Id("input-description");

        private By licenseDetailsDocsSutitle = By.XPath("//div[contains(text(),'Documentation')]");
        private By licenseDetailsPhysicalLeaseExistViewLabel = By.XPath("//label[contains(text(),'Physical copy exists')]");
        private By licenseDetailsPhysicalLeaseExistContent = By.Id("input-description");
        private By licenseDetailsPhysicalLeaseExistLabel = By.XPath("//label[contains(text(),'Physical lease/license exists')]");
        private By licenseDetailsPhysicalLeaseExistSelector = By.Id("input-hasPhysicalLicense");
        private By licenseDetailsDigitalLeaseExistViewLabel = By.XPath("//label[contains(text(),'Digital copy exists')]");
        private By licenseDetailsDigitalLeaseExistContent = By.Id("input-hasDigitalLicense");
        private By licenseDetailsDigitalLeaseExistLabel = By.XPath("//label[contains(text(),'Digital lease/license exists')]");
        private By licenseDetailsDigitalLeaseExistSelector = By.Id("input-hasDigitalLicense");
        private By licenseDetailsLocationDocsLabel = By.XPath("//label[contains(text(),'Document location')]");
        private By licenseDetailsLocationDocsTooltip = By.XPath("//label[contains(text(),'Document location')]/span/span[@data-testid='tooltip-icon-section-field-tooltip']");
        private By licenseDetailsLocationDocsTextarea = By.Id("input-documentationReference");
        private By licenseDetailsLocationDocsContent = By.XPath("//label[contains(text(),'Document location')]/parent::div/following-sibling::div");
        private By licenseDetailsLISNbrLabel = By.XPath("//label[contains(text(),'LIS #')]");
        private By licenseDetailsLISNbrInput = By.Id("input-tfaFileNumber");
        private By licenseDetailsPSNbrLabel = By.XPath("//label[contains(text(),'PS #')]");
        private By licenseDetailsPSNbrInput = By.Id("input-psFileNo");
        private By licenseDetailsNotesLabel = By.XPath("//label[contains(text(),'Lease notes')]");
        private By licenseDetailsNotesTextarea = By.Id("input-note");
        private By licenseDetailsNotesContent = By.XPath("//label[contains(text(),'Lease notes')]/parent::div/following-sibling::div");

        private By licenseDetailsSaveButton = By.XPath("//div[contains(text(),'Save')]/parent::button");
        private By licenseDetailsCancelButton = By.XPath("//div[contains(text(),'Cancel')]/parent::button");

        private By licenseDetailsModalPIDAttached = By.CssSelector("div[class='modal-content']");

        //Leases Modal Element
        private By licenseDetailsConfirmationModal = By.CssSelector("div[class='modal-content']");

        private SharedSearchProperties sharedSearchProperties;
        private SharedModals sharedModals;

        public LeaseDetails(IWebDriver webDriver) : base(webDriver)
        {
            sharedSearchProperties = new SharedSearchProperties(webDriver);
            sharedModals = new SharedModals(webDriver);
        }

        //Navigates to Create a new Lease/License
        public void NavigateToCreateNewLicense()
        {
            Wait();
            webDriver.FindElement(menuManagementButton).Click();

            Wait();
            webDriver.FindElement(createLicenseButton).Click();
        }

        //Covers only required fields on License Details
        public void LicenseDetailsMinFields(string status, string startDate, string expiryDate, string program)
        {
            Wait();

            //Change Status
            ChooseSpecificSelectOption(licenseDetailsStatusSelector, status);

            //Selecting Account Type
            var receivePayableTypeElement = webDriver.FindElement(licenseDetailsAccountTypeSelector);
            receivePayableTypeElement.Click();
            ChooseRandomSelectOption(licenseDetailsAccountTypeSelector, 0);

            //Insert Start Date
            webDriver.FindElement(licenseDetailsStartDateInput).SendKeys(startDate);
            webDriver.FindElement(licenseDetailsStartDateLabel).Click();

            //Insert Expiry Date
            webDriver.FindElement(licenseDetailsExpiryDateInput).SendKeys(expiryDate);
            webDriver.FindElement(licenseDetailsExpiryDateLabel).Click();

            //Selecting MOTI Region
            ChooseRandomSelectOption(licenseDetailsMotiRegionSelector, 1);

            //Selecting Program
            ChooseSpecificSelectOption(licenseDetailsProgramSelector, program);

            Wait();
            //If other Program is selected, insert input
            if (webDriver.FindElements(licenseDetailsOtherProgramInput).Count > 0)
            {
                Assert.True(webDriver.FindElement(licenseDetailsOtherProgramLabel).Displayed);
                webDriver.FindElement(licenseDetailsOtherProgramInput).SendKeys("Automation Test - Other Program");
            }

            //Selecting Type
            ChooseRandomSelectOption(licenseDetailsTypeSelector, 1);

            //Selecting other Type if required
            if (webDriver.FindElements(licenseDetailsOtherTypeInput).Count() > 0)
            {
                Assert.True(webDriver.FindElement(licenseDetailsOtherTypeLabel).Displayed);
                webDriver.FindElement(licenseDetailsOtherTypeInput).SendKeys("Automation Test - Other Type");
            }

            Wait();
            //Selecting Category if required
            if (webDriver.FindElements(licenseDetailsCategorySelector).Count() > 0)
            {
                Assert.True(webDriver.FindElement(licenseDetailsCategoryLabel).Displayed);
                ChooseRandomSelectOption(licenseDetailsCategorySelector, 1);
            }

            //If Other Category is selected
            if (webDriver.FindElements(licenseDetailsCategoryOtherInput).Count() > 0)
            {
                Assert.True(webDriver.FindElement(licenseDetailsCategoryOtherLabel).Displayed);
                webDriver.FindElement(licenseDetailsCategoryOtherInput).SendKeys("Automation Test - Other Category");
            }

            //Selecting Purpose
            ChooseRandomSelectOption(licenseDetailsPurposeSelector, 1);

            Wait();
            //If other Purpose is selected, insert input
            if (webDriver.FindElements(licenseDetailsOtherPurposeInput).Count > 0)
            {
                Assert.True(webDriver.FindElement(licenseDetailsOtherPurposeLabel).Displayed);
                webDriver.FindElement(licenseDetailsOtherPurposeInput).SendKeys("Automation Test - Other Purpose");
            }
        }

        //Covers all fields on License Details
        public void LicenseDetailsMaxFields(string ministryProject, string status, string startDate, string expiryDate, string motiContact, string program, string responsibilityDate, string locationOfDoc, string description, string lis, string ps, string notes)
        {
            Wait();
            
            //Insert Project
            webDriver.FindElement(licenseDetailsProjectInput).SendKeys(ministryProject);
            Wait();
            webDriver.FindElement(licenseDetailsProject1stOption).Click();

            //Change Status
            ChooseSpecificSelectOption(licenseDetailsStatusSelector, status);

            //Insert Start Date
            webDriver.FindElement(licenseDetailsStartDateInput).SendKeys(startDate);

            //Insert Expiry Date
            webDriver.FindElement(licenseDetailsExpiryDateInput).Click();
            webDriver.FindElement(licenseDetailsExpiryDateInput).SendKeys(expiryDate);

            //Selecting Account Type
            webDriver.FindElement(licenseDetailsAccountTypeSelector).Click();
            ChooseRandomSelectOption(licenseDetailsAccountTypeSelector, 0);

            //Inserting MOTI Contact
            webDriver.FindElement(licenseDetailsMotiContactInput).SendKeys(motiContact);

            //Selecting MOTI Region
            ChooseRandomSelectOption(licenseDetailsMotiRegionSelector, 1);

            //Selecting Program
            ChooseSpecificSelectOption(licenseDetailsProgramSelector, program);

            Wait();
            //If other Program is selected, insert input
            if (webDriver.FindElements(licenseDetailsOtherProgramInput).Count > 0)
            {
                Assert.True(webDriver.FindElement(licenseDetailsOtherProgramLabel).Displayed);
                webDriver.FindElement(licenseDetailsOtherProgramInput).SendKeys("Automation Test - Other Program");
            }

            //Selecting Type
            ChooseRandomSelectOption(licenseDetailsTypeSelector, 1);

            //If other Type is selected, insert input
            if (webDriver.FindElements(licenseDetailsOtherTypeInput).Count > 0)
            {
                Assert.True(webDriver.FindElement(licenseDetailsOtherTypeLabel).Displayed);
                webDriver.FindElement(licenseDetailsOtherTypeInput).SendKeys("Automation Test - Other Type");
            }

            Wait();
            //Selecting Category if required
            if (webDriver.FindElements(licenseDetailsCategorySelector).Count > 0)
            {
                Assert.True(webDriver.FindElement(licenseDetailsCategoryLabel).Displayed);
                ChooseRandomSelectOption(licenseDetailsCategorySelector, 1);
            }

            //If Other Category has been selected
            if (webDriver.FindElements(licenseDetailsCategoryOtherInput).Count > 0)
            {
                Assert.True(webDriver.FindElement(licenseDetailsCategoryOtherLabel).Displayed);
                webDriver.FindElement(licenseDetailsCategoryOtherInput).SendKeys("Automation Test - Other Category");
            }

            //Selecting Purpose
            ChooseRandomSelectOption(licenseDetailsPurposeSelector, 1);

            Wait();
            //If other Purpose is selected, insert input
            if (webDriver.FindElements(licenseDetailsOtherPurposeInput).Count > 0)
            {
                Assert.True(webDriver.FindElement(licenseDetailsOtherPurposeLabel).Displayed);
                webDriver.FindElement(licenseDetailsOtherPurposeInput).SendKeys("Automation Test - Other Purpose");
            }

            //Selecting a Initiator
            ChooseRandomSelectOption(licenseDetailsInitiatorSelector, 1);

            //Selecting a Responsibility
            ChooseRandomSelectOption(licenseDetailsResposibilitySelector, 1);

            //Inserting a Effective date of responsibility
            webDriver.FindElement(licenseDetailsEffectiveDateInput).SendKeys(responsibilityDate);

            //Selecting Physical lease exists
            ChooseRandomSelectOption(licenseDetailsPhysicalLeaseExistSelector, 0);

            //Selecting Digital lease exists
            ChooseRandomSelectOption(licenseDetailsDigitalLeaseExistSelector, 0);

            //Inserting Location of documents
            webDriver.FindElement(licenseDetailsLocationDocsTextarea).SendKeys(locationOfDoc);

            //Inserting LIS#
            webDriver.FindElement(licenseDetailsLISNbrInput).SendKeys(lis);
 
            //Inserting PS#
            webDriver.FindElement(licenseDetailsPSNbrInput).SendKeys(ps);

            //Inserting Intended use
            webDriver.FindElement(licenseDetailsIntendedUseTextarea).SendKeys(description);

            //Inserting Notes
            webDriver.FindElement(licenseDetailsNotesTextarea).SendKeys(notes);
        }

        public void UpdateLeaseFileDetails(string description, string notes)
        {
            Wait();

            ClearInput(licenseDetailsIntendedUseTextarea);
            webDriver.FindElement(licenseDetailsIntendedUseTextarea).SendKeys(description);

            ClearInput(licenseDetailsNotesTextarea);
            webDriver.FindElement(licenseDetailsNotesTextarea).SendKeys(notes);
        }

        public void EditLeaseFileDetails()
        {
            Wait();
            webDriver.FindElement(licenseDetailsEditIcon).Click();
        }

        public void SaveLicense()
        {
            Wait();

            //Save
            ButtonElement("Save");

            Wait();
            //If PID is already associated with another license
            if (webDriver.FindElements(licenseDetailsModalPIDAttached).Count > 0)
            {
                ButtonElement("Save Anyways");
            }
        }

        public void CancelLicense()
        {
            Wait();
            ButtonElement("Cancel");

            try
            {
                WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(3));
                if (wait.Until(ExpectedConditions.AlertIsPresent()) != null)
                {
                    webDriver.SwitchTo().Alert().Accept();
                }
            }
            catch (WebDriverTimeoutException e)
            {
                if (webDriver.FindElements(licenseDetailsConfirmationModal).Count() > 0)
                {
                    Assert.True(sharedModals.ModalHeader().Equals("Confirm changes"));
                    Assert.True(sharedModals.ConfirmationModalText1().Equals("If you cancel now, this Lease/License will not be saved."));
                    Assert.True(sharedModals.ConfirmationModalText2().Equals("Are you sure you want to Cancel?"));

                    sharedModals.ModalClickOKBttn();
                }
            }
        }

        public string GetLeaseCode()
        {
            Wait();
            return webDriver.FindElement(licenseHeaderNbrContent).Text;
        }

        public string GetLeaseAccountType()
        {
            Wait();
            return webDriver.FindElement(licenseHeaderAccountType).Text;
        }

        public void VerifyLicenseDetailsCreateForm()
        {
            Wait();

            //Create Title
            Assert.True(webDriver.FindElement(licenseCreateTitle).Displayed);

            //Details
            Assert.True(webDriver.FindElement(licenseDetailsProjectLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsProjectInput).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsStatusLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsStatusSelector).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsAccountTypeLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsAccountTypeSelector).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsStartDateLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsStartDateInput).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsExpiryDateLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsExpiryDateInput).Displayed);

            //Properties to include in this file
            sharedSearchProperties.VerifyLocateOnMapFeature();

            //Administration
            Assert.True(webDriver.FindElement(licenseDetailsAdmSubtitle).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsMotiContactLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsMotiContactInput).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsMotiRegionLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsMotiRegionSelector).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsProgramLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsProgramSelector).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsTypeLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsTypeSelector).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsPurposeLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsPurposeSelector).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsInitiatorLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsInitiatorTooltip).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsInitiatorSelector).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsResponsibilityLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsResponsibilityTooltip).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsResposibilitySelector).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsEffectiveDateLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsEffectiveDateInput).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsIntendedUseLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsIntendedUseTextarea).Displayed);

            //Documentation
            Assert.True(webDriver.FindElement(licenseDetailsDocsSutitle).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsPhysicalLeaseExistLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsPhysicalLeaseExistSelector).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsDigitalLeaseExistLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsDigitalLeaseExistSelector).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsLocationDocsLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsLocationDocsTooltip).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsLocationDocsTextarea).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsLISNbrLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsLISNbrInput).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsPSNbrLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsPSNbrInput).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsNotesLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsNotesTextarea).Displayed);

            //Buttons
            Assert.True(webDriver.FindElement(licenseDetailsSaveButton).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsCancelButton).Displayed);
        }

        public void VerifyLicenseHeader()
        {
            Wait();

            Assert.True(webDriver.FindElement(licenseHeaderNbrLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseHeaderNbrContent).Displayed);
            Assert.True(webDriver.FindElement(licenseHeaderAccountType).Displayed);
            Assert.True(webDriver.FindElement(licenseHeaderProperty).Displayed);
            Assert.True(webDriver.FindElement(licenseHeaderPropertyContent).Displayed);
            Assert.True(webDriver.FindElement(licenseHeaderTenantLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseHeaderStartDateLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseHeaderStartDateContent).Displayed);
            Assert.True(webDriver.FindElement(licenseHeaderExpiryDateLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseHeaderExpiryDateContent).Displayed);
            Assert.True(webDriver.FindElement(licenseHeaderCreatedLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseHeaderCreatedContent).Displayed);
            Assert.True(webDriver.FindElement(licenseHeaderCreatedByContent).Displayed);
            Assert.True(webDriver.FindElement(licenseHeaderLastUpdatedLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseHeaderLastUpdatedContent).Displayed);
            Assert.True(webDriver.FindElement(licenseHeaderLastUpdatedByContent).Displayed);
            Assert.True(webDriver.FindElement(licenseHeaderStatusLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseHeaderStatusContent).Displayed);

            //Verify Expired Flag on Header
            var expiryDateInput = Convert.ToDateTime(webDriver.FindElement(licenseHeaderExpiryDateContent).Text);
            if (expiryDateInput < DateTime.Now)
            {
                Assert.True(webDriver.FindElement(licenseHeaderExpiredFlag).Displayed);
            }
        }

        public void VerifyLicenseDetailsViewForm()
        {
            Wait();
            VerifyLicenseHeader();

            //Edit Icon
            Assert.True(webDriver.FindElement(licenseDetailsEditIcon).Displayed);

            //Lease Start and End Date
            Assert.True(webDriver.FindElement(licenseDetailsLeaseDateSubtitle).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsLeaseDateStartLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsLeaseDateStartContent).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsLeaseDateEndLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsLeaseDateEndContent).Displayed);

            //Lease Current Term
            Assert.True(webDriver.FindElement(licenseDetailsCurrentTermSubtitle).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsCurrentTermStartLabel).Displayed);
            //Assert.True(webDriver.FindElement(licenseDetailsCurrentTermStartContent).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsCurrentTermEndLabel).Displayed);
            //Assert.True(webDriver.FindElement(licenseDetailsCurrentTermEndContent).Displayed);

            //Lease 1st Property Information
            Assert.True(webDriver.FindElement(licenseDetailsPropertyInformationSubtitle).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsProperty1DescriptiveNameLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsProperty1DescriptiveNameContent).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsProperty1AreaIncludedLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsProperty1AreaIncludedContent).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsProperty1AddressLabel).Displayed);

            if (webDriver.FindElements(licenseDetailsProperty1AddressNoContent).Count() > 0)
            {
                Assert.True(webDriver.FindElement(licenseDetailsProperty1AddressNoContent).Displayed);
            }
            else
            {
                Assert.True(webDriver.FindElement(licenseDetailsProperty1AddressContent).Displayed);
            }
            
            Assert.True(webDriver.FindElement(licenseDetailsProperty1LegalDescripLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsProperty1LegalDescripContent).Displayed);

            //Lease Management
            Assert.True(webDriver.FindElement(licenseDetailsAdmSubtitle).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsProgramViewLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsProgramContent).GetAttribute("value") != "");
            Assert.True(webDriver.FindElement(licenseDetailsViewTypeLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsTypeContent).GetAttribute("value") != "");
            Assert.True(webDriver.FindElement(licenseDetailsReceivableToLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsReceivableToContent).GetAttribute("value") != "");
            Assert.True(webDriver.FindElement(licenseDetailsCategoryLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsCategoryContent).GetAttribute("value") != "");
            Assert.True(webDriver.FindElement(licenseDetailsPurposeLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsPurposeContent).GetAttribute("value") != "");
            Assert.True(webDriver.FindElement(licenseDetailsInitiatorLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsInitiatorContent).GetAttribute("value") != "");
            Assert.True(webDriver.FindElement(licenseDetailsResponsibilityLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsResponsibilityContent).GetAttribute("value") != "");
            Assert.True(webDriver.FindElement(licenseDetailsEffectiveDateLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsEffectiveDateContent).GetAttribute("value") != "");
            Assert.True(webDriver.FindElement(licenseDetailsMotiContactViewLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsMotiContactInput).GetAttribute("value") != "");
            Assert.True(webDriver.FindElement(licenseDetailsIntendedUseLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsIntendedUseTextarea).GetAttribute("value") != "");

            //Documentation
            Assert.True(webDriver.FindElement(licenseDetailsPhysicalLeaseExistViewLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsPhysicalLeaseExistContent).GetAttribute("value") != "");
            Assert.True(webDriver.FindElement(licenseDetailsDigitalLeaseExistViewLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsDigitalLeaseExistContent).GetAttribute("value") != "");
            Assert.True(webDriver.FindElement(licenseDetailsLocationDocsLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsLocationDocsContent).GetAttribute("value") != "");
            Assert.True(webDriver.FindElement(licenseDetailsLISNbrLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsLISNbrInput).GetAttribute("value") != "");
            Assert.True(webDriver.FindElement(licenseDetailsPSNbrLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsPSNbrInput).GetAttribute("value") != "");
            Assert.True(webDriver.FindElement(licenseDetailsNotesLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsNotesContent).Text != "");
        }

        public void VerifyLicenseDetailsUpdateForm()
        {
            Wait();
            VerifyLicenseHeader();

            //Details
            Assert.True(webDriver.FindElement(licenseDetailsStatusLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsStatusSelector).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsAccountTypeLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsAccountTypeSelector).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsStartDateLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsStartDateInput).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsExpiryDateLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsExpiryDateInput).Displayed);

            //Administration
            Assert.True(webDriver.FindElement(licenseDetailsAdmSubtitle).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsMotiContactLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsMotiContactInput).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsMotiRegionLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsMotiRegionSelector).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsProgramLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsProgramSelector).Displayed);
            if (webDriver.FindElements(licenseDetailsOtherProgramLabel).Count() > 0) { Assert.True(webDriver.FindElement(licenseDetailsOtherProgramInput).Displayed); }

            Assert.True(webDriver.FindElement(licenseDetailsTypeLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsTypeSelector).Displayed);
            if (webDriver.FindElements(licenseDetailsOtherTypeLabel).Count > 0) { Assert.True(webDriver.FindElement(licenseDetailsOtherTypeInput).Displayed); }
            if (webDriver.FindElements(licenseDetailsCategoryLabel).Count > 0) { Assert.True(webDriver.FindElement(licenseDetailsCategorySelector).Displayed); }
            if (webDriver.FindElements(licenseDetailsCategoryOtherLabel).Count > 0) { Assert.True(webDriver.FindElement(licenseDetailsCategoryOtherInput).Displayed); }

            Assert.True(webDriver.FindElement(licenseDetailsPurposeLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsPurposeSelector).Displayed);
            if (webDriver.FindElements(licenseDetailsOtherTypeLabel).Count > 0) { Assert.True(webDriver.FindElement(licenseDetailsOtherTypeInput).Displayed); }

            Assert.True(webDriver.FindElement(licenseDetailsInitiatorLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsInitiatorTooltip).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsInitiatorSelector).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsResponsibilityLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsResponsibilityTooltip).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsResposibilitySelector).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsEffectiveDateLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsEffectiveDateInput).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsIntendedUseLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsIntendedUseTextarea).Displayed);

            //Documentation
            Assert.True(webDriver.FindElement(licenseDetailsDocsSutitle).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsPhysicalLeaseExistLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsPhysicalLeaseExistSelector).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsDigitalLeaseExistLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsDigitalLeaseExistSelector).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsLocationDocsLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsLocationDocsTooltip).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsLocationDocsTextarea).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsLISNbrLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsLISNbrInput).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsPSNbrLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsPSNbrInput).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsNotesLabel).Displayed);
            Assert.True(webDriver.FindElement(licenseDetailsNotesTextarea).Displayed);
        }

    }
}
