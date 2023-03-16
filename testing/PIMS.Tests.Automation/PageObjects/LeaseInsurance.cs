

using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class LeaseInsurance : PageObjectBase
    {
        //Insurance Menu Elements
        private By licenseInsuranceLink = By.XPath("//a[contains(text(),'Insurance')]");
        private By insuranceEditIcon = By.XPath("//div[@role='tabpanel'][4]/div/div/button");

        //Insurance Create Form Elements
        private By insuranceSubtitle = By.XPath("//h2[contains(text(),'Required coverage')]");
        private By insuranceInstructions = By.XPath("//div[contains(text(),'Select the coverage types that are required for this lease or license.')]");

        private By insuranceAircraftLabel = By.XPath("//label[contains(text(),'Aircraft Liability Coverage')]");
        private By insuranceAircraftCheckbox = By.Id("insurance-0");
        private By insuranceCGLLabel = By.XPath("//label[contains(text(),'Commercial General Liability (CGL)')]");
        private By insuranceCGLCheckbox = By.Id("insurance-1");
        private By insuranceMarineLabel = By.XPath("//label[contains(text(),'Marine Liability Coverage')]");
        private By insuranceMarineCheckbox = By.Id("insurance-2");
        private By insuranceVehicleLabel = By.XPath("//label[contains(text(),'Vehicle Liability Coverage')]");
        private By insuranceVehicleCheckbox = By.Id("insurance-3");
        private By insuranceOtherLabel = By.XPath("//label[contains(text(),'Other Insurance Coverage')]");
        private By insuranceOtherCheckbox = By.Id("insurance-4");

        private By insuranceDetailsSubtitle = By.XPath("//h2[contains(text(),'Coverage details')]");

        private By insuranceAircraftSubtitle = By.XPath("//h2[contains(text(),'Aircraft Liability Coverage')]");
        private By insuranceAircraftInPlaceLabel = By.XPath("//h2[contains(text(),'Aircraft Liability Coverage')]/following-sibling::div[1]/div/strong[contains(text(), 'Insurance In Place')]");
        private By insuranceAircraftInPlaceRadioBttnGroup = By.XPath("//input[@name='insurances.0.isInsuranceInPlaceRadio']");
        private By insuranceAircraftLimitLabel = By.XPath("//h2[contains(text(),'Aircraft Liability Coverage')]/following-sibling::div[2]/div/div/strong[contains(text(), 'Limit ($)')]");
        private By insuranceAircraftLimitInput = By.Id("input-insurances.0.coverageLimit");
        private By insuranceAircraftExpiryDateLabel = By.XPath("//h2[contains(text(),'Aircraft Liability Coverage')]/following-sibling::div[2]/div/div/strong[contains(text(), 'Policy Expiry date')]");
        private By insuranceAircraftExpiryDateInput = By.Id("datepicker-insurances.0.expiryDate");
        private By insuranceAircraftDescriptionLabel = By.XPath("//h2[contains(text(),'Aircraft Liability Coverage')]/following-sibling::div[2]/div/div/strong[contains(text(), 'Description of Coverage')]");
        private By insuranceAircraftDescriptionTextarea = By.Id("input-insurances.0.coverageDescription");

        private By insuranceCGLSubtitle = By.XPath("//h2[contains(text(),'Commercial General Liability (CGL)')]");
        private By insuranceCGLInPlaceLabel = By.XPath("//h2[contains(text(),'Commercial General Liability (CGL)')]/following-sibling::div[1]/div/strong[contains(text(), 'Insurance In Place')]");
        private By insuranceCGLInPlaceRadioBttnGroup = By.XPath("//input[@name='insurances.1.isInsuranceInPlaceRadio']");
        private By insuranceCGLLimitLabel = By.XPath("//h2[contains(text(),'Commercial General Liability (CGL)')]/following-sibling::div[2]/div/div/strong[contains(text(), 'Limit ($)')]");
        private By insuranceCGLLimitInput = By.Id("input-insurances.1.coverageLimit");
        private By insuranceCGLExpiryDateLabel = By.XPath("//h2[contains(text(),'Commercial General Liability (CGL)')]/following-sibling::div[2]/div/div/strong[contains(text(), 'Policy Expiry date')]");
        private By insuranceCGLExpiryDateInput = By.Id("datepicker-insurances.1.expiryDate");
        private By insuranceCGLDescriptionLabel = By.XPath("//h2[contains(text(),'Commercial General Liability (CGL)')]/following-sibling::div[2]/div/div/strong[contains(text(), 'Description of Coverage')]");
        private By insuranceCGLDescriptionTextarea = By.Id("input-insurances.1.coverageDescription");

        private By insuranceMarineSubtitle = By.XPath("//h2[contains(text(),'Marine Liability Coverage')]");
        private By insuranceMarineInPlaceLabel = By.XPath("//h2[contains(text(),'Marine Liability Coverage')]/following-sibling::div[1]/div/strong[contains(text(), 'Insurance In Place')]");
        private By insuranceMarineInPlaceRadioBttnGroup = By.XPath("//input[@name='insurances.2.isInsuranceInPlaceRadio']");
        private By insuranceMarineLimitLabel = By.XPath("//h2[contains(text(),'Marine Liability Coverage')]/following-sibling::div[2]/div/div/strong[contains(text(), 'Limit ($)')]");
        private By insuranceMarineLimitInput = By.Id("input-insurances.2.coverageLimit");
        private By insuranceMarineExpiryDateLabel = By.XPath("//h2[contains(text(),'Marine Liability Coverage')]/following-sibling::div[2]/div/div/strong[contains(text(), 'Policy Expiry date')]");
        private By insuranceMarineExpiryDateInput = By.Id("datepicker-insurances.2.expiryDate");
        private By insuranceMarineDescriptionLabel = By.XPath("//h2[contains(text(),'Marine Liability Coverage')]/following-sibling::div[2]/div/div/strong[contains(text(), 'Description of Coverage')]");
        private By insuranceMarineDescriptionTextarea = By.Id("input-insurances.2.coverageDescription");

        private By insuranceVehicleSubtitle = By.XPath("//h2[contains(text(),'Vehicle Liability Coverage')]");
        private By insuranceVehicleInPlaceLabel = By.XPath("//h2[contains(text(),'Vehicle Liability Coverage')]/following-sibling::div[1]/div/strong[contains(text(), 'Insurance In Place')]");
        private By insuranceVehicleInPlaceRadioBttnGroup = By.XPath("//input[@name='insurances.3.isInsuranceInPlaceRadio']");
        private By insuranceVehicleLimitLabel = By.XPath("//h2[contains(text(),'Vehicle Liability Coverage')]/following-sibling::div[2]/div/div/strong[contains(text(), 'Limit ($)')]");
        private By insuranceVehicleLimitInput = By.Id("input-insurances.3.coverageLimit");
        private By insuranceVehicleExpiryDateLabel = By.XPath("//h2[contains(text(),'Vehicle Liability Coverage')]/following-sibling::div[2]/div/div/strong[contains(text(), 'Policy Expiry date')]");
        private By insuranceVehicleExpiryDateInput = By.Id("datepicker-insurances.3.expiryDate");
        private By insuranceVehicleDescriptionLabel = By.XPath("//h2[contains(text(),'Vehicle Liability Coverage')]/following-sibling::div[2]/div/div/strong[contains(text(), 'Description of Coverage')]");
        private By insuranceVehicleDescriptionTextarea = By.Id("input-insurances.3.coverageDescription");

        private By insuranceOtherSubtitle = By.XPath("//h2[contains(text(),'Other Insurance Coverage')]");
        private By insuranceOtherTypeLabel = By.XPath("//h2[contains(text(),'Other Insurance Coverage')]/following-sibling::div[1]/div/div/strong[contains(text(), 'Insurance Type')]");
        private By insuranceOtherTypeInput = By.Id("input-insurances.4.otherInsuranceType");
        private By insuranceOtherInPlaceLabel = By.XPath("//h2[contains(text(),'Other Insurance Coverage')]/following-sibling::div[2]/div/strong[contains(text(), 'Insurance In Place')]");
        private By insuranceOtherInPlaceRadioBttnGroup = By.XPath("//input[@name='insurances.4.isInsuranceInPlaceRadio']");
        private By insuranceOtherLimitLabel = By.XPath("//h2[contains(text(),'Other Insurance Coverage')]/following-sibling::div[3]/div/div/strong[contains(text(), 'Limit ($)')]");
        private By insuranceOtherLimitInput = By.Id("input-insurances.4.coverageLimit");
        private By insuranceOtherExpiryDateLabel = By.XPath("//h2[contains(text(),'Other Insurance Coverage')]/following-sibling::div[3]/div/div/strong[contains(text(), 'Policy Expiry date:')]");
        private By insuranceOtherExpiryDateInput = By.Id("datepicker-insurances.4.expiryDate");
        private By insuranceOtherDescriptionLabel = By.XPath("//h2[contains(text(),'Other Insurance Coverage')]/following-sibling::div[3]/div/div/strong[contains(text(), 'Description of Coverage')]");
        private By insuranceOtherDescriptionTextarea = By.Id("input-insurances.4.coverageDescription");

        //Insurance View Form Elements
        private By insuranceRequiredLabel = By.XPath("//div[contains(text(),'Required insurance')]");
        private By insuranceAircraftListOption = By.XPath("//li[contains(text(),'Aircraft Liability Coverage')]");
        private By insuranceCGLListOption = By.XPath("//li[contains(text(),'Commercial General Liability (CGL)')]");
        private By insuranceMarineListOption = By.XPath("//li[contains(text(),'Marine Liability Coverage')]");
        private By insuranceVehicleListOption = By.XPath("//li[contains(text(),'Vehicle Liability Coverage')]");
        private By insuranceOtherListOption = By.XPath("//li[contains(text(),'Other Insurance Coverage')]");

        private By insuranceAircraftViewSubtitle = By.XPath("//div[contains(text(),'Aircraft Liability Coverage')]");
        private By insuranceAircraftViewInPlaceLabel = By.XPath("//div[contains(text(),'Aircraft Liability Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Insurance in place')]");
        private By insuranceAircraftViewInPlaceContent = By.XPath("//div[contains(text(),'Aircraft Liability Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Insurance in place')]/parent::div/following-sibling::div");
        private By insuranceAircraftViewLimitLabel = By.XPath("//div[contains(text(),'Aircraft Liability Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Limit')]");
        private By insuranceAircraftViewLimitContent = By.XPath("//div[contains(text(),'Aircraft Liability Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Limit')]/parent::div/following-sibling::div");
        private By insuranceAircraftViewExpiryDateLabel = By.XPath("//div[contains(text(),'Aircraft Liability Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Policy expiry date')]");
        private By insuranceAircraftViewExpiryDateContent = By.XPath("//div[contains(text(),'Aircraft Liability Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Policy expiry date')]/parent::div/following-sibling::div");
        private By insuranceAircraftViewDescriptionLabel = By.XPath("//div[contains(text(),'Aircraft Liability Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Description of Coverage')]");
        private By insuranceAircraftViewDescriptionContent = By.XPath("//div[contains(text(),'Aircraft Liability Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Description of Coverage')]/parent::div/following-sibling::div");

        private By insuranceCGLViewSubtitle = By.XPath("//div[contains(text(),'Commercial General Liability (CGL)')]");
        private By insuranceCGLViewInPlaceLabel = By.XPath("//div[contains(text(),'Commercial General Liability (CGL)')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Insurance in place')]");
        private By insuranceCGLViewInPlaceContent = By.XPath("//div[contains(text(),'Commercial General Liability (CGL)')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Insurance in place')]/parent::div/following-sibling::div");
        private By insuranceCGLViewLimitLabel = By.XPath("//div[contains(text(),'Commercial General Liability (CGL)')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Limit')]");
        private By insuranceCGLViewLimitContent = By.XPath("//div[contains(text(),'Commercial General Liability (CGL)')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Limit')]/parent::div/following-sibling::div");
        private By insuranceCGLViewExpiryDateLabel = By.XPath("//div[contains(text(),'Commercial General Liability (CGL)')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Policy expiry date')]");
        private By insuranceCGLViewExpiryDateContent = By.XPath("//div[contains(text(),'Commercial General Liability (CGL)')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Policy expiry date')]/parent::div/following-sibling::div");
        private By insuranceCGLViewDescriptionLabel = By.XPath("//div[contains(text(),'Commercial General Liability (CGL)')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Description of Coverage')]");
        private By insuranceCGLViewDescriptionContent = By.XPath("//div[contains(text(),'Commercial General Liability (CGL)')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Description of Coverage')]/parent::div/following-sibling::div");

        private By insuranceMarineViewSubtitle = By.XPath("//div[contains(text(),'Marine Liability Coverage')]");
        private By insuranceMarineViewInPlaceLabel = By.XPath("//div[contains(text(),'Marine Liability Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Insurance in place')]");
        private By insuranceMarineViewInPlaceContent = By.XPath("//div[contains(text(),'Marine Liability Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Insurance in place')]/parent::div/following-sibling::div");
        private By insuranceMarineViewLimitLabel = By.XPath("//div[contains(text(),'Marine Liability Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Limit')]");
        private By insuranceMarineViewLimitContent = By.XPath("//div[contains(text(),'Marine Liability Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Limit')]/parent::div/following-sibling::div");
        private By insuranceMarineViewExpiryDateLabel = By.XPath("//div[contains(text(),'Marine Liability Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Policy expiry date')]");
        private By insuranceMarineViewExpiryDateContent = By.XPath("//div[contains(text(),'Marine Liability Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Policy expiry date')]/parent::div/following-sibling::div");
        private By insuranceMarineViewDescriptionLabel = By.XPath("//div[contains(text(),'Marine Liability Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Description of Coverage')]");
        private By insuranceMarineViewDescriptionContent = By.XPath("//div[contains(text(),'Marine Liability Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Description of Coverage')]/parent::div/following-sibling::div");

        private By insuranceVehicleViewSubtitle = By.XPath("//div[contains(text(),'Vehicle Liability Coverage')]");
        private By insuranceVehicleViewInPlaceLabel = By.XPath("//div[contains(text(),'Vehicle Liability Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Insurance in place')]");
        private By insuranceVehicleViewInPlaceContent = By.XPath("//div[contains(text(),'Vehicle Liability Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Insurance in place')]/parent::div/following-sibling::div");
        private By insuranceVehicleViewLimitLabel = By.XPath("//div[contains(text(),'Vehicle Liability Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Limit')]");
        private By insuranceVehicleViewLimitContent = By.XPath("//div[contains(text(),'Vehicle Liability Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Limit')]/parent::div/following-sibling::div");
        private By insuranceVehicleViewExpiryDateLabel = By.XPath("//div[contains(text(),'Vehicle Liability Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Policy expiry date')]");
        private By insuranceVehicleViewExpiryDateContent = By.XPath("//div[contains(text(),'Vehicle Liability Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Policy expiry date')]/parent::div/following-sibling::div");
        private By insuranceVehicleViewDescriptionLabel = By.XPath("//div[contains(text(),'Vehicle Liability Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Description of Coverage')]");
        private By insuranceVehicleViewDescriptionContent = By.XPath("//div[contains(text(),'Vehicle Liability Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Description of Coverage')]/parent::div/following-sibling::div");

        private By insuranceOtherViewSubtitle = By.XPath("//div[contains(text(),'Other Insurance Coverage')]");
        private By insuranceOtherViewInPlaceLabel = By.XPath("//div[contains(text(),'Other Insurance Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Insurance in place')]");
        private By insuranceOtherViewInPlaceContent = By.XPath("//div[contains(text(),'Other Insurance Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Insurance in place')]/parent::div/following-sibling::div");
        private By insuranceOtherViewLimitLabel = By.XPath("//div[contains(text(),'Other Insurance Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Limit')]");
        private By insuranceOtherViewLimitContent = By.XPath("//div[contains(text(),'Other Insurance Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Limit')]/parent::div/following-sibling::div");
        private By insuranceOtherViewExpiryDateLabel = By.XPath("//div[contains(text(),'Other Insurance Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Policy expiry date')]");
        private By insuranceOtherViewExpiryDateContent = By.XPath("//div[contains(text(),'Other Insurance Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Policy expiry date')]/parent::div/following-sibling::div");
        private By insuranceOtherViewDescriptionLabel = By.XPath("//div[contains(text(),'Other Insurance Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Description of Coverage')]");
        private By insuranceOtherViewDescriptionContent = By.XPath("//div[contains(text(),'Other Insurance Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Description of Coverage')]/parent::div/following-sibling::div");

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
            Wait();
            webDriver.FindElement(insuranceEditIcon).Click();
        }

        //Add Aircraft Insurance
        public void AddAircraftInsurance(string isInPlace, string limit, string expiryDate, string description)
        {
            Wait();
            FocusAndClick(insuranceAircraftCheckbox);

            //Verify Presence of elements on the form
            Wait();
            Assert.True(webDriver.FindElement(insuranceAircraftSubtitle).Displayed);
            Assert.True(webDriver.FindElement(insuranceAircraftInPlaceLabel).Displayed);
            Assert.True(webDriver.FindElement(insuranceAircraftLimitLabel).Displayed);
            Assert.True(webDriver.FindElement(insuranceAircraftLimitInput).Displayed);
            Assert.True(webDriver.FindElement(insuranceAircraftExpiryDateLabel).Displayed);
            Assert.True(webDriver.FindElement(insuranceAircraftExpiryDateInput).Displayed);
            Assert.True(webDriver.FindElement(insuranceAircraftDescriptionLabel).Displayed);
            Assert.True(webDriver.FindElement(insuranceAircraftDescriptionTextarea).Displayed);

            //Fill out form
            Wait();
            ChooseSpecificRadioButton(insuranceAircraftInPlaceRadioBttnGroup, isInPlace);

            webDriver.FindElement(insuranceAircraftLimitInput).SendKeys(limit);
            webDriver.FindElement(insuranceAircraftExpiryDateInput).Click();
            webDriver.FindElement(insuranceAircraftExpiryDateInput).SendKeys(expiryDate);
            webDriver.FindElement(insuranceAircraftDescriptionTextarea).Click();
            webDriver.FindElement(insuranceAircraftDescriptionTextarea).SendKeys(description);
        }

        //Add CGL Insurance
        public void AddCGLInsurance(string isInPlace, string limit, string expiryDate, string description)
        {
            Wait();
            FocusAndClick(insuranceCGLCheckbox);

            //Verify Presence of elements on the form
            Wait();
            Assert.True(webDriver.FindElement(insuranceCGLSubtitle).Displayed);
            Assert.True(webDriver.FindElement(insuranceCGLInPlaceLabel).Displayed);
            Assert.True(webDriver.FindElement(insuranceCGLLimitLabel).Displayed);
            Assert.True(webDriver.FindElement(insuranceCGLLimitInput).Displayed);
            Assert.True(webDriver.FindElement(insuranceCGLExpiryDateLabel).Displayed);
            Assert.True(webDriver.FindElement(insuranceCGLExpiryDateInput).Displayed);
            Assert.True(webDriver.FindElement(insuranceCGLDescriptionLabel).Displayed);
            Assert.True(webDriver.FindElement(insuranceCGLDescriptionTextarea).Displayed);

            //Fill out form
            Wait();
            ChooseSpecificRadioButton(insuranceCGLInPlaceRadioBttnGroup, isInPlace);

            webDriver.FindElement(insuranceCGLLimitInput).SendKeys(limit);
            webDriver.FindElement(insuranceCGLExpiryDateInput).Click();
            webDriver.FindElement(insuranceCGLExpiryDateInput).SendKeys(expiryDate);
            webDriver.FindElement(insuranceCGLDescriptionTextarea).Click();
            webDriver.FindElement(insuranceCGLDescriptionTextarea).SendKeys(description);
        }

        //Add Marine Insurance
        public void AddMarineInsurance(string isInPlace, string limit, string expiryDate, string description)
        {
            Wait();
            FocusAndClick(insuranceMarineCheckbox);

            //Verify Presence of elements on the form
            Wait();
            Assert.True(webDriver.FindElement(insuranceMarineSubtitle).Displayed);
            Assert.True(webDriver.FindElement(insuranceMarineInPlaceLabel).Displayed);
            Assert.True(webDriver.FindElement(insuranceMarineLimitLabel).Displayed);
            Assert.True(webDriver.FindElement(insuranceMarineLimitInput).Displayed);
            Assert.True(webDriver.FindElement(insuranceMarineExpiryDateLabel).Displayed);
            Assert.True(webDriver.FindElement(insuranceMarineExpiryDateInput).Displayed);
            Assert.True(webDriver.FindElement(insuranceMarineDescriptionLabel).Displayed);
            Assert.True(webDriver.FindElement(insuranceMarineDescriptionTextarea).Displayed);

            //Fill out form
            Wait();
            ChooseSpecificRadioButton(insuranceMarineInPlaceRadioBttnGroup, isInPlace);

            webDriver.FindElement(insuranceMarineLimitInput).SendKeys(limit);
            webDriver.FindElement(insuranceMarineExpiryDateInput).Click();
            webDriver.FindElement(insuranceMarineExpiryDateInput).SendKeys(expiryDate);
            webDriver.FindElement(insuranceMarineDescriptionTextarea).Click();
            webDriver.FindElement(insuranceMarineDescriptionTextarea).SendKeys(description);
        }

        //Add Vehicle Insurance
        public void AddVehicleInsurance(string isInPlace, string limit, string expiryDate, string description)
        {
            Wait();
            FocusAndClick(insuranceVehicleCheckbox);

            //Verify Presence of elements on the form
            Wait();
            Assert.True(webDriver.FindElement(insuranceVehicleSubtitle).Displayed);
            Assert.True(webDriver.FindElement(insuranceVehicleInPlaceLabel).Displayed);
            Assert.True(webDriver.FindElement(insuranceVehicleLimitLabel).Displayed);
            Assert.True(webDriver.FindElement(insuranceVehicleLimitInput).Displayed);
            Assert.True(webDriver.FindElement(insuranceVehicleExpiryDateLabel).Displayed);
            Assert.True(webDriver.FindElement(insuranceVehicleExpiryDateInput).Displayed);
            Assert.True(webDriver.FindElement(insuranceVehicleDescriptionLabel).Displayed);
            Assert.True(webDriver.FindElement(insuranceVehicleDescriptionTextarea).Displayed);

            //Fill out form
            Wait();
            ChooseSpecificRadioButton(insuranceVehicleInPlaceRadioBttnGroup, isInPlace);

            webDriver.FindElement(insuranceVehicleLimitInput).SendKeys(limit);
            webDriver.FindElement(insuranceVehicleExpiryDateInput).Click();
            webDriver.FindElement(insuranceVehicleExpiryDateInput).SendKeys(expiryDate);
            webDriver.FindElement(insuranceVehicleDescriptionTextarea).Click();
            webDriver.FindElement(insuranceVehicleDescriptionTextarea).SendKeys(description);

        }

        //Add Other Insurance
        public void AddOtherInsurance(string isInPlace, string type, string limit, string expiryDate, string description)
        {
            Wait();
            FocusAndClick(insuranceOtherCheckbox);

            //Verify Presence of elements on the form
            Wait();
            Assert.True(webDriver.FindElement(insuranceOtherSubtitle).Displayed);
            Assert.True(webDriver.FindElement(insuranceOtherInPlaceLabel).Displayed);
            Assert.True(webDriver.FindElement(insuranceOtherLimitLabel).Displayed);
            Assert.True(webDriver.FindElement(insuranceOtherLimitInput).Displayed);
            Assert.True(webDriver.FindElement(insuranceOtherExpiryDateLabel).Displayed);
            Assert.True(webDriver.FindElement(insuranceOtherExpiryDateInput).Displayed);
            Assert.True(webDriver.FindElement(insuranceOtherDescriptionLabel).Displayed);
            Assert.True(webDriver.FindElement(insuranceOtherDescriptionTextarea).Displayed);

            //Fill out form
            Wait();
            webDriver.FindElement(insuranceOtherTypeInput).SendKeys(type);

            ChooseSpecificRadioButton(insuranceOtherInPlaceRadioBttnGroup, isInPlace);

            webDriver.FindElement(insuranceOtherLimitInput).SendKeys(limit);
            webDriver.FindElement(insuranceOtherExpiryDateInput).Click();
            webDriver.FindElement(insuranceOtherExpiryDateInput).SendKeys(expiryDate);
            webDriver.FindElement(insuranceOtherDescriptionTextarea).Click();
            webDriver.FindElement(insuranceOtherDescriptionTextarea).SendKeys(description);
        }

        public void DeleteInsurance(string insuranceType)
        {
            Wait();
            switch (insuranceType)
            {
                case "Aircraft":
                    FocusAndClick(insuranceAircraftCheckbox);
                    break;
                case "CGL":
                    FocusAndClick(insuranceCGLCheckbox);
                    break;
                case "Marine":
                    FocusAndClick(insuranceMarineCheckbox);
                    break;
                case "Vehicle":
                    FocusAndClick(insuranceVehicleCheckbox);
                    break;
                case "Other":
                    FocusAndClick(insuranceOtherCheckbox);
                    break;
            }
        }

        public void VerifyInsuranceInitForm()
        {
            Wait();
            Assert.True(webDriver.FindElement(insuranceSubtitle).Displayed);
            Assert.True(webDriver.FindElement(insuranceInstructions).Displayed);

            Assert.True(webDriver.FindElement(insuranceAircraftLabel).Displayed);
            Assert.True(webDriver.FindElement(insuranceAircraftCheckbox).Displayed);
            Assert.True(webDriver.FindElement(insuranceCGLLabel).Displayed);
            Assert.True(webDriver.FindElement(insuranceCGLCheckbox).Displayed);
            Assert.True(webDriver.FindElement(insuranceMarineLabel).Displayed);
            Assert.True(webDriver.FindElement(insuranceMarineCheckbox).Displayed);
            Assert.True(webDriver.FindElement(insuranceVehicleLabel).Displayed);
            Assert.True(webDriver.FindElement(insuranceVehicleCheckbox).Displayed);
            Assert.True(webDriver.FindElement(insuranceOtherLabel).Displayed);
            Assert.True(webDriver.FindElement(insuranceOtherCheckbox).Displayed);

            Assert.True(webDriver.FindElement(insuranceDetailsSubtitle).Displayed);
        }

        public void VerifyInsuranceViewForm(string aircraftIsInPlace, string aircraftLimit, string aircraftExpiryDate, string aircraftDescription, string CGLIsInPlace, string CGLLimit, string CGLExpiryDate, string CGLDescription,
            string marineIsInPlace, string marineLimit, string marineExpiryDate, string marineDescription, string vehicleIsInPlace, string vehicleLimit, string vehicleExpiryDate, string vehicleDescription,
            string otherIsInPlace, string otherLimit, string otherExpiryDate, string otherDescription)
        {
            Wait();

            Assert.True(webDriver.FindElement(insuranceRequiredLabel).Displayed);

            if (webDriver.FindElements(insuranceAircraftListOption).Count > 1)
            {
                Assert.True(webDriver.FindElement(insuranceAircraftViewSubtitle).Displayed);
                Assert.True(webDriver.FindElement(insuranceAircraftViewInPlaceLabel).Displayed);
                Assert.True(webDriver.FindElement(insuranceAircraftViewInPlaceContent).Text.Equals(aircraftIsInPlace));
                Assert.True(webDriver.FindElement(insuranceAircraftViewLimitLabel).Displayed);
                Assert.True(webDriver.FindElement(insuranceAircraftViewLimitContent).Text.Equals(aircraftLimit));
                Assert.True(webDriver.FindElement(insuranceAircraftViewExpiryDateLabel).Displayed);
                Assert.True(webDriver.FindElement(insuranceAircraftViewExpiryDateContent).Text.Equals(aircraftExpiryDate));
                Assert.True(webDriver.FindElement(insuranceAircraftViewDescriptionLabel).Displayed);
                Assert.True(webDriver.FindElement(insuranceAircraftViewDescriptionContent).Text.Equals(aircraftDescription));
            }

            if (webDriver.FindElements(insuranceCGLListOption).Count > 1)
            {
                Assert.True(webDriver.FindElement(insuranceCGLViewSubtitle).Displayed);
                Assert.True(webDriver.FindElement(insuranceCGLViewInPlaceLabel).Displayed);
                Assert.True(webDriver.FindElement(insuranceCGLViewInPlaceContent).Text.Equals(CGLIsInPlace));
                Assert.True(webDriver.FindElement(insuranceCGLViewLimitLabel).Displayed);
                Assert.True(webDriver.FindElement(insuranceCGLViewLimitContent).Text.Equals(CGLLimit));
                Assert.True(webDriver.FindElement(insuranceCGLViewExpiryDateLabel).Displayed);
                Assert.True(webDriver.FindElement(insuranceCGLViewExpiryDateContent).Text.Equals(CGLExpiryDate));
                Assert.True(webDriver.FindElement(insuranceCGLViewDescriptionLabel).Displayed);
                Assert.True(webDriver.FindElement(insuranceCGLViewDescriptionContent).Text.Equals(CGLDescription));
            }

            if (webDriver.FindElements(insuranceMarineListOption).Count > 1)
            {
                Assert.True(webDriver.FindElement(insuranceMarineViewSubtitle).Displayed);
                Assert.True(webDriver.FindElement(insuranceMarineViewInPlaceLabel).Displayed);
                Assert.True(webDriver.FindElement(insuranceMarineViewInPlaceContent).Text.Equals(marineIsInPlace));
                Assert.True(webDriver.FindElement(insuranceMarineViewLimitLabel).Displayed);
                Assert.True(webDriver.FindElement(insuranceMarineViewLimitContent).Text.Equals(marineLimit));
                Assert.True(webDriver.FindElement(insuranceMarineViewExpiryDateLabel).Displayed);
                Assert.True(webDriver.FindElement(insuranceMarineViewExpiryDateContent).Text.Equals(marineExpiryDate));
                Assert.True(webDriver.FindElement(insuranceMarineViewDescriptionLabel).Displayed);
                Assert.True(webDriver.FindElement(insuranceMarineViewDescriptionContent).Text.Equals(marineDescription));
            }

            if (webDriver.FindElements(insuranceVehicleListOption).Count > 1)
            {
                Assert.True(webDriver.FindElement(insuranceVehicleViewSubtitle).Displayed);
                Assert.True(webDriver.FindElement(insuranceVehicleViewInPlaceLabel).Displayed);
                Assert.True(webDriver.FindElement(insuranceVehicleViewInPlaceContent).Text.Equals(vehicleIsInPlace));
                Assert.True(webDriver.FindElement(insuranceVehicleViewLimitLabel).Displayed);
                Assert.True(webDriver.FindElement(insuranceVehicleViewLimitContent).Text.Equals(vehicleLimit));
                Assert.True(webDriver.FindElement(insuranceVehicleViewExpiryDateLabel).Displayed);
                Assert.True(webDriver.FindElement(insuranceVehicleViewExpiryDateContent).Text.Equals(vehicleExpiryDate));
                Assert.True(webDriver.FindElement(insuranceVehicleViewDescriptionLabel).Displayed);
                Assert.True(webDriver.FindElement(insuranceVehicleViewDescriptionContent).Text.Equals(vehicleDescription));
            }

            if (webDriver.FindElements(insuranceOtherListOption).Count > 1)
            {
                Assert.True(webDriver.FindElement(insuranceOtherViewSubtitle).Displayed);
                Assert.True(webDriver.FindElement(insuranceOtherViewInPlaceLabel).Displayed);
                Assert.True(webDriver.FindElement(insuranceOtherViewInPlaceContent).Text.Equals(otherIsInPlace));
                Assert.True(webDriver.FindElement(insuranceOtherViewLimitLabel).Displayed);
                Assert.True(webDriver.FindElement(insuranceOtherViewLimitContent).Text.Equals(otherLimit));
                Assert.True(webDriver.FindElement(insuranceOtherViewExpiryDateLabel).Displayed);
                Assert.True(webDriver.FindElement(insuranceOtherViewExpiryDateContent).Text.Equals(otherExpiryDate));
                Assert.True(webDriver.FindElement(insuranceOtherViewDescriptionLabel).Displayed);
                Assert.True(webDriver.FindElement(insuranceOtherViewDescriptionContent).Text.Equals(otherDescription));
            }
        }
    }
}
