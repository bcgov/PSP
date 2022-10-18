using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class LeaseDetails : PageObjectBase
    {
        private By menuManagementButton = By.XPath("//a/label[contains(text(),'Management')]/parent::a");
        private By createLicenseButton = By.XPath("//a[contains(text(),'Create a Lease/License File')]");

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
        private By licenseDetailsOtherTypeInput = By.Id("input-otherType");
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

        

        //Covers only required fields on License Details
        public void LicenseDetailsMinFields(string startDate, string pid)
        {
            Wait();

            //Insert Start Date
            webDriver.FindElement(licenseDetailsStartDateInput).SendKeys(startDate);

            //Change Status
            ChooseRandomSelectOption(licenseDetailsStatusSelector, "input-statusType", 2);


            //Selecting Receive or Payable Type
            var receivePayableTypeElement = webDriver.FindElement(licenseDetailsReceiveOrPaySelector);
            receivePayableTypeElement.Click();
            ChooseRandomSelectOption(licenseDetailsReceiveOrPaySelector, "input-paymentReceivableType", 2);

            //Selecting MOTI Region
            ChooseRandomSelectOption(licenseDetailsMotiRegionSelector, "input-region", 2);

            //Selecting Program
            ChooseRandomSelectOption(licenseDetailsProgramSelector, "input-programType", 2);

            Wait();
            //If other Program is selected, insert input
            if (webDriver.FindElements(licenseDetailsOtherProgramInput).Count > 0)
            {
                webDriver.FindElement(licenseDetailsOtherProgramInput).SendKeys("Automation Test - Other Program");
            }

            //Selecting Type
            ChooseRandomSelectOption(licenseDetailsTypeSelector, "input-type", 2);

            //Selecting other Type if required
            if (webDriver.FindElements(licenseDetailsOtherTypeInput).Count > 0)
            {
                webDriver.FindElement(licenseDetailsOtherTypeInput).SendKeys("Automation Test - Other Type");
            }

            Wait();
            //Selecting Category if required
            if (webDriver.FindElements(licenseDetailsCategorySelector).Count > 0)
            {
                ChooseRandomSelectOption(licenseDetailsCategorySelector, "input-categoryType", 2);
            }

            //Selecting Purpose
            ChooseRandomSelectOption(licenseDetailsPurposeSelector, "input-purposeType", 2);

            Wait();
            //If other Purpose is selected, insert input
            if (webDriver.FindElements(licenseDetailsOtherPurposeInput).Count > 0)
            {
                webDriver.FindElement(licenseDetailsOtherPurposeInput).SendKeys("Automation Test - Other Purpose");
            }

            Wait();

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
            webDriver.FindElement(licenseDetailsExpiryDateInput).Click();
            webDriver.FindElement(licenseDetailsExpiryDateInput).SendKeys(expiryDate);

            //Change Status
            webDriver.FindElement(licenseDetailsStatusSelector).Click();
            ChooseRandomSelectOption(licenseDetailsStatusSelector, "input-statusType", 2);


            //Selecting Receive or Payable Type
            webDriver.FindElement(licenseDetailsReceiveOrPaySelector).Click();
            ChooseRandomSelectOption(licenseDetailsReceiveOrPaySelector, "input-paymentReceivableType", 2);

            //Inserting MOTI Contact
            webDriver.FindElement(licenseDetailsMotiContactInput).SendKeys(motiContact);

            //Selecting MOTI Region
            ChooseRandomSelectOption(licenseDetailsMotiRegionSelector, "input-region", 2);

            //Selecting Program
            ChooseRandomSelectOption(licenseDetailsProgramSelector, "input-programType", 2);

            Wait();
            //If other Program is selected, insert input
            if (webDriver.FindElements(licenseDetailsOtherProgramInput).Count > 0)
            {
                webDriver.FindElement(licenseDetailsOtherProgramInput).SendKeys("Automation Test - Other Program");
            }

            //Selecting Type
            ChooseRandomSelectOption(licenseDetailsTypeSelector, "input-type", 2);

            Wait();
            //Selecting Category if required
            if (webDriver.FindElements(licenseDetailsCategorySelector).Count > 0)
            {
                ChooseRandomSelectOption(licenseDetailsCategorySelector, "input-categoryType", 2);
            }

            Wait();
            //If other Program is selected, insert input
            if (webDriver.FindElements(licenseDetailsOtherPurposeInput).Count > 0)
            {
                webDriver.FindElement(licenseDetailsOtherPurposeInput).SendKeys("Automation Test - Other Purpose");
            }

            //Selecting Purpose
            ChooseRandomSelectOption(licenseDetailsPurposeSelector, "input-purposeType", 2);

            Wait();
            //If other Purpose is selected, insert input
            if (webDriver.FindElements(licenseDetailsOtherTypeInput).Count > 0)
            {
                webDriver.FindElement(licenseDetailsOtherTypeInput).SendKeys("Automation Test - Other Type");
            }

            //Selecting a Initiator
            ChooseRandomSelectOption(licenseDetailsInitiatorSelector, "input-initiatorType", 2);

            //Selecting a Responsibility
            ChooseRandomSelectOption(licenseDetailsResposibilitySelector, "input-responsibilityType", 2);

            //Inserting a Effective date of responsibility
            webDriver.FindElement(licenseDetailsResponsibilityEffectDateInput).SendKeys(responsibilityDate);

            //Selecting Physical lease exists
            ChooseRandomSelectOption(licenseDetailsPhysicalLeaseExistSelector, "input-hasPhysicalLicense", 1);

            //Selecting Digital lease exists
            ChooseRandomSelectOption(licenseDetailsDigitalLeaseExistSelector, "input-hasDigitalLicense", 1);

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
