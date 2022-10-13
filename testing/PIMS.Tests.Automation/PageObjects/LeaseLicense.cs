using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class LeaseLicense : PageObjectBase
    {
        private By menuManagementButton = By.XPath("//a/label[contains(text(),'Management')]");
        private By createLicenseButton = By.XPath("//a[contains(text(),'Add a lease or license')]");
        private By searchLicenseButton = By.XPath("//a[contains(text(),'Search for a Lease or License')]");

        private By licenseDetailsLink = By.XPath("//a[contains(text(),'Details')]");
        private By licenseTenantLink = By.XPath("//a[contains(text(),'Tenant')]");
        private By licensePaymentsLink = By.XPath("//a[contains(text(),'Payments')]");
        private By licensImprovementsLink = By.XPath("//a[contains(text(),'Improvements')]");
        private By licenseInsuranceLink = By.XPath("//a[contains(text(),'Insurance')]");
        private By licenseDepositLink = By.XPath("//a[contains(text(),'Deposit')]");
        private By licenseSurplusLink = By.XPath("//a[contains(text(),'Surplus Declaration')]");

        private By licenseEditIcon = By.CssSelector("a[class='float-right']");

        private By licenseDetailsStartDateInput = By.Id("datepicker-startDate");
        private By licenseDetailsExpiryDateInput = By.Id("datepicker-expiryDate");
        private By licenseStatusSelector = By.Id("input-statusType");
        private By licenseReceiveOrPaySelector = By.Id("input-paymentReceivableType");
        private By licenseMotiContactInput = By.Id("input-motiName");
        private By licenseMotiRegionSelector = By.Id("input-region");
        private By licenseProgramSelector = By.Id("input-programType");
        private By licenseTypeSelector = By.Id("input-type");
        private By licensePurposeSelector = By.Id("input-purposeType");
        private By licenseInitiatorSelector = By.Id("input-initiatorType");
        private By licenseResposibilitySelector = By.Id("input-responsibilityType");
        private By licenseResponsibilityEffectDateInput = By.Id("datepicker-responsibilityEffectiveDate");
        private By licensePhysicalLeaseExistSelector = By.Id("input-hasPhysicalLicense");
        private By licenseDigitalLeaseExistSelector = By.Id("input-hasDigitalLicense");
        private By licenseLocationDocsTextarea = By.Id("input-documentationReference");





            
            
            





        public LeaseLicense(IWebDriver webDriver) : base(webDriver)
        { }
    }
}
