using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class LeaseDetails : PageObjectBase
    {
        private By menuManagementButton = By.XPath("//a/label[contains(text(),'Management')]/parent::a");
        private By createLicenseButton = By.XPath("//a[contains(text(),'Add a lease or license')]");
        private By searchLicenseButton = By.XPath("//a[contains(text(),'Search for a Lease or License')]");

        private By licenseDetailsLink = By.XPath("//a[contains(text(),'Details')]");
        private By licenseTenantLink = By.XPath("//a[contains(text(),'Tenant')]");
        
        private By licenseImprovementsLink = By.XPath("//a[contains(text(),'Improvements')]");
        private By licenseInsuranceLink = By.XPath("//a[contains(text(),'Insurance')]");
        private By licenseDepositLink = By.XPath("//a[contains(text(),'Deposit')]");
        private By licenseSurplusLink = By.XPath("//a[contains(text(),'Surplus Declaration')]");

        private By licenseEditIcon = By.CssSelector("a[class='float-right']");

        private By licenseDetailsStartDateInput = By.Id("datepicker-startDate");
        private By licenseDetailsExpiryDateInput = By.Id("datepicker-expiryDate");
        private By licenseDetailsStatusSelector = By.Id("input-statusType");
        private By licenseDetailsReceiveOrPaySelector = By.Id("input-paymentReceivableType");
        private By licenseDetailsMotiContactInput = By.Id("input-motiName");
        private By licenseDetailsMotiRegionSelector = By.Id("input-region");
        private By licenseDetailsProgramSelector = By.Id("input-programType");
        private By licenseDetailsOtherProgramInput = By.Id("input-otherProgramType");
        private By licenseDetailsTypeSelector = By.Id("input-type");
        private By licenseDetailsOtherTypeInput = By.Id("input-otherProgramType");
        private By licenseDetailsCategorySelector = By.Id("input-categoryType");
        private By licenseDetailsPurposeSelector = By.Id("input-purposeType");
        private By licenseDetailsOtherPurposeInput = By.Id("input-otherPurposeType");
        private By licenseDetailsInitiatorSelector = By.Id("input-initiatorType");
        private By licenseDetailsResposibilitySelector = By.Id("input-responsibilityType");
        private By licenseDetailsResponsibilityEffectDateInput = By.Id("datepicker-responsibilityEffectiveDate");
        private By licenseDetailsPhysicalLeaseExistSelector = By.Id("input-hasPhysicalLicense");
        private By licenseDetailsDigitalLeaseExistSelector = By.Id("input-hasDigitalLicense");
        private By licenseDetailsLocationDocsTextarea = By.Id("input-documentationReference");
        private By licenseDetailsPID1Input = By.Id("input-properties.0.pid"); //to-do
        private By licenseDetailsDescriptionTextarea = By.Id("input-description");
        private By licenseDetailsNotesTextarea = By.Id("input-note");
        private By licenseDetailsModalPIDAttached = By.CssSelector("div[class='modal-content']");

        public LeaseDetails(IWebDriver webDriver) : base(webDriver)
        { }

        //Navigates to Create a new Lease/License
        public void NavigateToCreateNewLicense()
        {
            Wait();
            webDriver.FindElement(menuManagementButton).Click();

            Wait();
            webDriver.FindElement(createLicenseButton).Click();
        }

        //Navigates to Search a Lease/License
        public void NavigateToSearchLicense()
        {
            Wait();
            webDriver.FindElement(menuManagementButton).Click();

            Wait();
            webDriver.FindElement(searchLicenseButton).Click();
        }

        //Covers only required fields on License Details
        public void LicenseDetailsMinFields(string startDate, string pid)
        {
            Wait();

            //Insert Start Date
            webDriver.FindElement(licenseDetailsStartDateInput).SendKeys(startDate);

            //Change Status
            var statusElement = webDriver.FindElement(licenseDetailsStatusSelector);
            ChooseRandomOption(statusElement, "input-statusType", 2);


            //Selecting Receive or Payable Type
            var receivePayableTypeElement = webDriver.FindElement(licenseDetailsReceiveOrPaySelector);
            receivePayableTypeElement.Click();
            ChooseRandomOption(receivePayableTypeElement, "input-paymentReceivableType", 2);

            //Selecting MOTI Region
            var motiRegionElement = webDriver.FindElement(licenseDetailsMotiRegionSelector);
            ChooseRandomOption(motiRegionElement, "input-region", 2);

            //Selecting Program
            var programElement = webDriver.FindElement(licenseDetailsProgramSelector);
            ChooseRandomOption(programElement, "input-programType", 2);

            //If other Program is selected, insert input
            if (webDriver.FindElements(licenseDetailsOtherProgramInput).Count > 0)
            {
                webDriver.FindElement(licenseDetailsOtherProgramInput).SendKeys("Automation Test - Other Program");
            }

            //Selecting Type
            var typeElement = webDriver.FindElement(licenseDetailsTypeSelector);
            ChooseRandomOption(typeElement, "input-type", 2);

            //Selecting Category if required
            if (webDriver.FindElements(licenseDetailsCategorySelector).Count > 0)
            {
                var categoryElement = webDriver.FindElement(licenseDetailsCategorySelector);
                ChooseRandomOption(categoryElement, "input-categoryType", 2);
            }

            //If other Program is selected, insert input
            if (webDriver.FindElements(licenseDetailsOtherPurposeInput).Count > 0)
            {
                webDriver.FindElement(licenseDetailsOtherPurposeInput).SendKeys("Automation Test - Other Program");
            }

            //Selecting Purpose
            var purposeElement = webDriver.FindElement(licenseDetailsPurposeSelector);
            ChooseRandomOption(purposeElement, "input-purposeType", 2);

            //If other Purpose is selected, insert input
            if (webDriver.FindElements(licenseDetailsOtherTypeInput).Count > 0)
            {
                webDriver.FindElement(licenseDetailsOtherTypeInput).SendKeys("Automation Test - Other Type");
            }

            //Inserting PID
            webDriver.FindElement(licenseDetailsPID1Input).SendKeys(pid);

        }

        //Covers all fields on License Details
        public void LicenseDetailsMaxFields(string startDate, string expiryDate, string motiContact, string responsibilityDate, string locationOfDoc, string pid, string description, string notes)
        {
            Wait();

            //Insert Start Date
            webDriver.FindElement(licenseDetailsStartDateInput).SendKeys(startDate);

            //Insert Expiry Date
            webDriver.FindElement(licenseDetailsExpiryDateInput).SendKeys(expiryDate);

            //Change Status
            var statusElement = webDriver.FindElement(licenseDetailsStatusSelector);
            ChooseRandomOption(statusElement, "input-statusType", 2);


            //Selecting Receive or Payable Type
            var receivePayableTypeElement = webDriver.FindElement(licenseDetailsReceiveOrPaySelector);
            receivePayableTypeElement.Click();
            ChooseRandomOption(receivePayableTypeElement, "input-paymentReceivableType", 2);

            //Inserting MOTI Contact
            webDriver.FindElement(licenseDetailsMotiContactInput).SendKeys(motiContact);

            //Selecting MOTI Region
            var motiRegionElement = webDriver.FindElement(licenseDetailsMotiRegionSelector);
            ChooseRandomOption(motiRegionElement, "input-region", 2);

            //Selecting Program
            var programElement = webDriver.FindElement(licenseDetailsProgramSelector);
            ChooseRandomOption(programElement, "input-programType", 2);

            //If other Program is selected, insert input
            if (webDriver.FindElements(licenseDetailsOtherProgramInput).Count > 0)
            {
                webDriver.FindElement(licenseDetailsOtherProgramInput).SendKeys("Automation Test - Other Program");
            }

            //Selecting Type
            var typeElement = webDriver.FindElement(licenseDetailsTypeSelector);
            ChooseRandomOption(typeElement, "input-type", 2);

            //Selecting Category if required
            if (webDriver.FindElements(licenseDetailsCategorySelector).Count > 0)
            {
                var categoryElement = webDriver.FindElement(licenseDetailsCategorySelector);
                ChooseRandomOption(categoryElement, "input-categoryType", 2);
            }

            //If other Program is selected, insert input
            if (webDriver.FindElements(licenseDetailsOtherPurposeInput).Count > 0)
            {
                webDriver.FindElement(licenseDetailsOtherPurposeInput).SendKeys("Automation Test - Other Program");
            }

            //Selecting Purpose
            var purposeElement = webDriver.FindElement(licenseDetailsPurposeSelector);
            ChooseRandomOption(purposeElement, "input-purposeType", 2);

            //If other Purpose is selected, insert input
            if (webDriver.FindElements(licenseDetailsOtherTypeInput).Count > 0)
            {
                webDriver.FindElement(licenseDetailsOtherTypeInput).SendKeys("Automation Test - Other Type");
            }

            //Selecting a Initiator
            var initiatorElement = webDriver.FindElement(licenseDetailsInitiatorSelector);
            ChooseRandomOption(initiatorElement, "input-initiatorType", 2);

            //Selecting a Responsibility
            var responsibilityElement = webDriver.FindElement(licenseDetailsResposibilitySelector);
            ChooseRandomOption(responsibilityElement, "input-responsibilityType", 2);

            //Inserting a Effective date of responsibility
            webDriver.FindElement(licenseDetailsResponsibilityEffectDateInput).SendKeys(responsibilityDate);

            //Selecting Physical lease exists
            var physicalLeaseElement = webDriver.FindElement(licenseDetailsPhysicalLeaseExistSelector);
            ChooseRandomOption(physicalLeaseElement, "input-hasPhysicalLicense", 1);

            //Selecting Digital lease exists
            var digitalLeaseElement = webDriver.FindElement(licenseDetailsDigitalLeaseExistSelector);
            ChooseRandomOption(digitalLeaseElement, "input-hasDigitalLicense", 1);

            //Inserting Location of documents
            webDriver.FindElement(licenseDetailsLocationDocsTextarea).SendKeys(locationOfDoc);

            //Inserting PID
            webDriver.FindElement(licenseDetailsPID1Input).SendKeys(pid);

            //Inserting Description
            webDriver.FindElement(licenseDetailsDescriptionTextarea).SendKeys(description);

            //Inserting Notes
            webDriver.FindElement(licenseDetailsNotesTextarea).SendKeys(notes);

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

            var tenantLinkElement = webDriver.FindElement(licenseTenantLink);
            Assert.True(tenantLinkElement.Enabled);

        }

        public void CancelLicense()
        {
            Wait();

            ButtonElement("Cancel");
            webDriver.SwitchTo().Alert().Accept();
        }



    }
}
