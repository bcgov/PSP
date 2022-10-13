

using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class LeaseInsurance : PageObjectBase
    {
        private By licenseInsuranceLink = By.XPath("//a[contains(text(),'Insurance')]");
        private By insuranceEditIcon = By.CssSelector("a[class='float-right']");

        private By insuranceAircraftCheckbox = By.Id("insurance-0");
        private By insuranceCGLCheckbox = By.Id("insurance-1");
        private By insuranceMarineCheckbox = By.Id("insurance-2");
        private By insuranceVehicleCheckbox = By.Id("insurance-3");
        private By insuranceOtherCheckbox = By.Id("insurance-4");

        private By insuranceAircraftInPlaceRadioBttnGroup = By.XPath("//input[@name='insurances.0.isInsuranceInPlaceRadio']");
        private By insuranceAircraftLimitInput = By.Id("input-insurances.0.coverageLimit");
        private By insuranceAircraftExpiryDateInput = By.Id("datepicker-insurances.0.expiryDate");
        private By insuranceAircraftDescriptionTextarea = By.Id("input-insurances.0.coverageDescription");

        private By insuranceCGLInPlaceRadioBttnGroup = By.XPath("//input[@name='insurances.1.isInsuranceInPlaceRadio']");
        private By insuranceCGLLimitInput = By.Id("input-insurances.1.coverageLimit");
        private By insuranceCGLExpiryDateInput = By.Id("datepicker-insurances.1.expiryDate");
        private By insuranceCGLDescriptionTextarea = By.Id("input-insurances.1.coverageDescription");

        private By insuranceMarineInPlaceRadioBttnGroup = By.XPath("//input[@name='insurances.2.isInsuranceInPlaceRadio']");
        private By insuranceMarineLimitInput = By.Id("input-insurances.2.coverageLimit");
        private By insuranceMarineExpiryDateInput = By.Id("datepicker-insurances.2.expiryDate");
        private By insuranceMarineDescriptionTextarea = By.Id("input-insurances.2.coverageDescription");

        private By insuranceVehicleInPlaceRadioBttnGroup = By.XPath("//input[@name='insurances.3.isInsuranceInPlaceRadio']");
        private By insuranceVehicleLimitInput = By.Id("input-insurances.3.coverageLimit");
        private By insuranceVehicleExpiryDateInput = By.Id("datepicker-insurances.3.expiryDate");
        private By insuranceVehicleDescriptionTextarea = By.Id("input-insurances.3.coverageDescription");

        private By insuranceOtherTypeInput = By.Id("input-insurances.4.otherInsuranceType");
        private By insuranceOtherInPlaceRadioBttnGroup = By.XPath("//input[@name='insurances.4.isInsuranceInPlaceRadio']");
        private By insuranceOtherLimitInput = By.Id("input-insurances.4.coverageLimit");
        private By insuranceOtherExpiryDateInput = By.Id("datepicker-insurances.4.expiryDate");
        private By insuranceOtherDescriptionTextarea = By.Id("input-insurances.4.coverageDescription");

        private By licenseInsuranceSaveButton = By.XPath("//button/div[contains(text(),'Save')]/ancestor::button");


        public LeaseInsurance(IWebDriver webDriver) : base(webDriver)
        {}

        //Navigate to Insurance section
        public void NavigateToInsuranceSection()
        {
            Wait();
            webDriver.FindElement(licenseInsuranceLink).Click();
        }

        //Edit Insurance section
        public void EditInsurance()
        {
            WaitUntil(insuranceEditIcon);

            webDriver.FindElement(insuranceEditIcon).Click();
        }

        //Add Aircraft Insurance
        public void AddAircraftInsurance(string limit, string expiryDate, string description)
        {
            Wait();
            FocusAndClick(insuranceAircraftCheckbox);

            Wait();
            ChooseRandomRadioButton(insuranceAircraftInPlaceRadioBttnGroup);

            webDriver.FindElement(insuranceAircraftLimitInput).SendKeys(limit);
            webDriver.FindElement(insuranceAircraftExpiryDateInput).Click();
            webDriver.FindElement(insuranceAircraftExpiryDateInput).SendKeys(expiryDate);
            webDriver.FindElement(insuranceAircraftDescriptionTextarea).Click();
            webDriver.FindElement(insuranceAircraftDescriptionTextarea).SendKeys(description);

        }

        //Add CGL Insurance
        public void AddCGLInsurance(string limit, string expiryDate, string description)
        {
            Wait();
            FocusAndClick(insuranceCGLCheckbox);

            Wait();
            ChooseRandomRadioButton(insuranceCGLInPlaceRadioBttnGroup);

            webDriver.FindElement(insuranceCGLLimitInput).SendKeys(limit);
            webDriver.FindElement(insuranceCGLExpiryDateInput).Click();
            webDriver.FindElement(insuranceCGLExpiryDateInput).SendKeys(expiryDate);
            webDriver.FindElement(insuranceCGLDescriptionTextarea).Click();
            webDriver.FindElement(insuranceCGLDescriptionTextarea).SendKeys(description);

        }

        //Add Marine Insurance
        public void AddMarineInsurance(string limit, string expiryDate, string description)
        {
            Wait();
            FocusAndClick(insuranceMarineCheckbox);

            Wait();
            ChooseRandomRadioButton(insuranceMarineInPlaceRadioBttnGroup);

            webDriver.FindElement(insuranceMarineLimitInput).SendKeys(limit);
            webDriver.FindElement(insuranceMarineExpiryDateInput).Click();
            webDriver.FindElement(insuranceMarineExpiryDateInput).SendKeys(expiryDate);
            webDriver.FindElement(insuranceMarineDescriptionTextarea).Click();
            webDriver.FindElement(insuranceMarineDescriptionTextarea).SendKeys(description);

        }

        //Add Vehicle Insurance
        public void AddVehicleInsurance(string limit, string expiryDate, string description)
        {
            Wait();
            FocusAndClick(insuranceVehicleCheckbox);

            Wait();
            ChooseRandomRadioButton(insuranceVehicleInPlaceRadioBttnGroup);

            webDriver.FindElement(insuranceVehicleLimitInput).SendKeys(limit);
            webDriver.FindElement(insuranceVehicleExpiryDateInput).Click();
            webDriver.FindElement(insuranceVehicleExpiryDateInput).SendKeys(expiryDate);
            webDriver.FindElement(insuranceVehicleDescriptionTextarea).Click();
            webDriver.FindElement(insuranceVehicleDescriptionTextarea).SendKeys(description);

        }

        //Add Other Insurance
        public void AddOtherInsurance(string type, string limit, string expiryDate, string description)
        {
            Wait();
            FocusAndClick(insuranceOtherCheckbox);

            Wait();
            webDriver.FindElement(insuranceOtherTypeInput).SendKeys(type);

            ChooseRandomRadioButton(insuranceOtherInPlaceRadioBttnGroup);

            webDriver.FindElement(insuranceVehicleLimitInput).SendKeys(limit);
            webDriver.FindElement(insuranceOtherExpiryDateInput).Click();
            webDriver.FindElement(insuranceOtherExpiryDateInput).SendKeys(expiryDate);
            webDriver.FindElement(insuranceOtherDescriptionTextarea).Click();
            webDriver.FindElement(insuranceOtherDescriptionTextarea).SendKeys(description);

        }

        //Save Insurance
        public void SaveInsurance()
        {
            Wait();

            ScrollToElement(licenseInsuranceSaveButton);

            var saveButton = webDriver.FindElement(licenseInsuranceSaveButton);
            saveButton.Enabled.Should().BeTrue();

            Wait();
            saveButton.Click();
        }
    }
}
