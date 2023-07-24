

using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class LeaseInsurance : PageObjectBase
    {
        //Insurance Menu Elements
        private By licenseInsuranceLink = By.XPath("//a[contains(text(),'Insurance')]");
        private By insuranceEditIcon = By.XPath("//div[@role='tabpanel']/div/div/button");

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
        private By insuranceEditTotal = By.XPath("//h2[contains(text(),'Coverage details')]/following-sibling::div/div");
        private By insuranceViewTotal = By.XPath("//div[@data-testid='insurance-section']/div[@data-testid='insurance-section']");

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
        public void EditInsuranceButton()
        {
            Wait();
            webDriver.FindElement(insuranceEditIcon).Click();
        }

        //Add Aircraft Insurance
        public void AddAircraftInsurance(Lease lease)
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

            if(lease.AircraftInsuranceInPlace != "")
                ChooseSpecificRadioButton(insuranceAircraftInPlaceRadioBttnGroup, lease.AircraftInsuranceInPlace);

            if (lease.AircraftLimit != "")
                webDriver.FindElement(insuranceAircraftLimitInput).SendKeys(lease.AircraftLimit);

            webDriver.FindElement(insuranceAircraftExpiryDateInput).Click();

            if(lease.AircraftPolicyExpiryDate != "")
                webDriver.FindElement(insuranceAircraftExpiryDateInput).SendKeys(lease.AircraftPolicyExpiryDate);

            webDriver.FindElement(insuranceAircraftDescriptionTextarea).Click();

            if(lease.AircraftDescriptionCoverage != "")
                webDriver.FindElement(insuranceAircraftDescriptionTextarea).SendKeys(lease.AircraftDescriptionCoverage);
        }

        //Add CGL Insurance
        public void AddCGLInsurance(Lease lease)
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
            if(lease.CGLInsuranceInPlace != "")
                ChooseSpecificRadioButton(insuranceCGLInPlaceRadioBttnGroup, lease.CGLInsuranceInPlace);
            if(lease.CGLLimit != "")
                webDriver.FindElement(insuranceCGLLimitInput).SendKeys(lease.CGLLimit);
            webDriver.FindElement(insuranceCGLExpiryDateInput).Click();
            if (lease.CGLPolicyExpiryDate != "")
                webDriver.FindElement(insuranceCGLExpiryDateInput).SendKeys(lease.CGLPolicyExpiryDate);
            webDriver.FindElement(insuranceCGLDescriptionTextarea).Click();
            if (lease.CGLDescriptionCoverage != "")
                webDriver.FindElement(insuranceCGLDescriptionTextarea).SendKeys(lease.CGLDescriptionCoverage);
        }

        //Add Marine Insurance
        public void AddMarineInsurance(Lease lease)
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
            if (lease.MarineInsuranceInPlace != "")
                ChooseSpecificRadioButton(insuranceMarineInPlaceRadioBttnGroup, lease.MarineInsuranceInPlace);
            if (lease.MarineLimit != "")
                webDriver.FindElement(insuranceMarineLimitInput).SendKeys(lease.MarineLimit);
            webDriver.FindElement(insuranceMarineExpiryDateInput).Click();
            if (lease.MarinePolicyExpiryDate != "")
                webDriver.FindElement(insuranceMarineExpiryDateInput).SendKeys(lease.MarinePolicyExpiryDate);
            webDriver.FindElement(insuranceMarineDescriptionTextarea).Click();
            if (lease.MarineDescriptionCoverage != "")
                webDriver.FindElement(insuranceMarineDescriptionTextarea).SendKeys(lease.MarineDescriptionCoverage);
        }

        //Add Vehicle Insurance
        public void AddVehicleInsurance(Lease lease)
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
            if (lease.VehicleInsuranceInPlace != "")
                ChooseSpecificRadioButton(insuranceVehicleInPlaceRadioBttnGroup, lease.VehicleInsuranceInPlace);
            if (lease.VehicleLimit != "")
                webDriver.FindElement(insuranceVehicleLimitInput).SendKeys(lease.VehicleLimit);
            webDriver.FindElement(insuranceVehicleExpiryDateInput).Click();
            if (lease.VehiclePolicyExpiryDate != "")
                webDriver.FindElement(insuranceVehicleExpiryDateInput).SendKeys(lease.VehiclePolicyExpiryDate);
            webDriver.FindElement(insuranceVehicleDescriptionTextarea).Click();
            if (lease.VehicleDescriptionCoverage != "")
                webDriver.FindElement(insuranceVehicleDescriptionTextarea).SendKeys(lease.VehicleDescriptionCoverage);

        }

        //Add Other Insurance
        public void AddOtherInsurance(Lease lease)
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
            if (lease.OtherInsuranceType != "")
                webDriver.FindElement(insuranceOtherTypeInput).SendKeys(lease.OtherInsuranceType);
            if (lease.OtherInsuranceInPlace != "")
                ChooseSpecificRadioButton(insuranceOtherInPlaceRadioBttnGroup, lease.OtherInsuranceInPlace);
            if (lease.OtherLimit != "")
                webDriver.FindElement(insuranceOtherLimitInput).SendKeys(lease.OtherLimit);
            webDriver.FindElement(insuranceOtherExpiryDateInput).Click();
            if (lease.OtherPolicyExpiryDate != "")
                webDriver.FindElement(insuranceOtherExpiryDateInput).SendKeys(lease.OtherPolicyExpiryDate);
            webDriver.FindElement(insuranceOtherDescriptionTextarea).Click();
            if (lease.OtherDescriptionCoverage != "")
                webDriver.FindElement(insuranceOtherDescriptionTextarea).SendKeys(lease.OtherDescriptionCoverage);
        }

        public void DeleteLastInsurance()
        {
            Wait();
            var totalInsertedInsurance = webDriver.FindElements(insuranceEditTotal).Count;
            var lastInsuranceInserted = webDriver.FindElement(By.XPath("//h2[contains(text(),'Coverage details')]/following-sibling::div/div[" + totalInsertedInsurance + "]/h2")).Text;
            
            switch (lastInsuranceInserted)
            {
                case "Aircraft Liability Coverage":
                    FocusAndClick(insuranceAircraftCheckbox);
                    break;
                case "Commercial General Liability (CGL)":
                    FocusAndClick(insuranceCGLCheckbox);
                    break;
                case "Marine Liability Coverage":
                    FocusAndClick(insuranceMarineCheckbox);
                    break;
                case "Vehicle Liability Coverage":
                    FocusAndClick(insuranceVehicleCheckbox);
                    break;
                case "Other Insurance Coverage":
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

        public void VerifyInsuranceViewForm(Lease lease)
        {
            Wait();

            Assert.True(webDriver.FindElement(insuranceRequiredLabel).Displayed);

            if (webDriver.FindElements(insuranceAircraftListOption).Count > 1)
            {
                Assert.True(webDriver.FindElement(insuranceAircraftViewSubtitle).Displayed);
                Assert.True(webDriver.FindElement(insuranceAircraftViewInPlaceLabel).Displayed);

                if (lease.AircraftInsuranceInPlace != "")
                    Assert.True(webDriver.FindElement(insuranceAircraftViewInPlaceContent).Text.Equals(lease.AircraftInsuranceInPlace));

                Assert.True(webDriver.FindElement(insuranceAircraftViewLimitLabel).Displayed);

                if (lease.AircraftLimit != "")
                    Assert.True(webDriver.FindElement(insuranceAircraftViewLimitContent).Text.Equals(lease.AircraftLimit));

                Assert.True(webDriver.FindElement(insuranceAircraftViewExpiryDateLabel).Displayed);

                if (lease.AircraftPolicyExpiryDate != "")
                    Assert.True(webDriver.FindElement(insuranceAircraftViewExpiryDateContent).Text.Equals(TransformDateFormat(lease.AircraftPolicyExpiryDate)));
              
                if (lease.AircraftDescriptionCoverage != "")
                    Assert.True(webDriver.FindElement(insuranceAircraftViewDescriptionLabel).Displayed);

                Assert.True(webDriver.FindElement(insuranceAircraftViewDescriptionContent).Text.Equals(lease.AircraftInsuranceInPlace));
            }

            if (webDriver.FindElements(insuranceCGLListOption).Count > 1)
            {
                Assert.True(webDriver.FindElement(insuranceCGLViewSubtitle).Displayed);
                Assert.True(webDriver.FindElement(insuranceCGLViewInPlaceLabel).Displayed);

                if (lease.CGLInsuranceInPlace != "")
                    Assert.True(webDriver.FindElement(insuranceCGLViewInPlaceContent).Text.Equals(lease.CGLInsuranceInPlace));

                Assert.True(webDriver.FindElement(insuranceCGLViewLimitLabel).Displayed);

                if(lease.CGLLimit != "")
                    Assert.True(webDriver.FindElement(insuranceCGLViewLimitContent).Text.Equals(lease.CGLLimit));

                Assert.True(webDriver.FindElement(insuranceCGLViewExpiryDateLabel).Displayed);

                if(lease.CGLPolicyExpiryDate != "")
                    Assert.True(webDriver.FindElement(insuranceCGLViewExpiryDateContent).Text.Equals(TransformDateFormat(lease.CGLPolicyExpiryDate)));

                Assert.True(webDriver.FindElement(insuranceCGLViewDescriptionLabel).Displayed);

                if(lease.CGLDescriptionCoverage != "")
                    Assert.True(webDriver.FindElement(insuranceCGLViewDescriptionContent).Text.Equals(lease.CGLDescriptionCoverage));
            }

            if (webDriver.FindElements(insuranceMarineListOption).Count > 1)
            {
                Assert.True(webDriver.FindElement(insuranceMarineViewSubtitle).Displayed);
                Assert.True(webDriver.FindElement(insuranceMarineViewInPlaceLabel).Displayed);

                if (lease.MarineInsuranceInPlace != "")
                    Assert.True(webDriver.FindElement(insuranceMarineViewInPlaceContent).Text.Equals(lease.MarineInsuranceInPlace));

                Assert.True(webDriver.FindElement(insuranceMarineViewLimitLabel).Displayed);

                if(lease.MarineLimit != "")
                    Assert.True(webDriver.FindElement(insuranceMarineViewLimitContent).Text.Equals(lease.MarineLimit));
                Assert.True(webDriver.FindElement(insuranceMarineViewExpiryDateLabel).Displayed);

                if(lease.MarinePolicyExpiryDate != "")
                    Assert.True(webDriver.FindElement(insuranceMarineViewExpiryDateContent).Text.Equals(TransformDateFormat(lease.MarinePolicyExpiryDate)));
                Assert.True(webDriver.FindElement(insuranceMarineViewDescriptionLabel).Displayed);

                if(lease.MarineDescriptionCoverage != "")
                    Assert.True(webDriver.FindElement(insuranceMarineViewDescriptionContent).Text.Equals(lease.MarineDescriptionCoverage));
            }

            if (webDriver.FindElements(insuranceVehicleListOption).Count > 1)
            {
                Assert.True(webDriver.FindElement(insuranceVehicleViewSubtitle).Displayed);
                Assert.True(webDriver.FindElement(insuranceVehicleViewInPlaceLabel).Displayed);

                if (lease.VehicleInsuranceInPlace != "")
                    Assert.True(webDriver.FindElement(insuranceVehicleViewInPlaceContent).Text.Equals(lease.VehicleInsuranceInPlace));

                Assert.True(webDriver.FindElement(insuranceVehicleViewLimitLabel).Displayed);

                if(lease.VehicleLimit != "")
                    Assert.True(webDriver.FindElement(insuranceVehicleViewLimitContent).Text.Equals(lease.VehicleLimit));

                Assert.True(webDriver.FindElement(insuranceVehicleViewExpiryDateLabel).Displayed);

                if (lease.VehiclePolicyExpiryDate != "")
                    Assert.True(webDriver.FindElement(insuranceVehicleViewExpiryDateContent).Text.Equals(TransformDateFormat(lease.VehiclePolicyExpiryDate)));

                Assert.True(webDriver.FindElement(insuranceVehicleViewDescriptionLabel).Displayed);

                if (lease.VehicleDescriptionCoverage != "")
                    Assert.True(webDriver.FindElement(insuranceVehicleViewDescriptionContent).Text.Equals(lease.VehicleDescriptionCoverage));
            }

            if (webDriver.FindElements(insuranceOtherListOption).Count > 1)
            {
                Assert.True(webDriver.FindElement(insuranceOtherViewSubtitle).Displayed);
                Assert.True(webDriver.FindElement(insuranceOtherViewInPlaceLabel).Displayed);

                if(lease.OtherInsuranceInPlace != "")
                    Assert.True(webDriver.FindElement(insuranceOtherViewInPlaceContent).Text.Equals(lease.OtherInsuranceInPlace));

                Assert.True(webDriver.FindElement(insuranceOtherViewLimitLabel).Displayed);

                if (lease.OtherLimit != "")
                    Assert.True(webDriver.FindElement(insuranceOtherViewLimitContent).Text.Equals(lease.OtherLimit));

                Assert.True(webDriver.FindElement(insuranceOtherViewExpiryDateLabel).Displayed);

                if(lease.OtherPolicyExpiryDate != "")
                    Assert.True(webDriver.FindElement(insuranceOtherViewExpiryDateContent).Text.Equals(TransformDateFormat(lease.OtherPolicyExpiryDate)));

                Assert.True(webDriver.FindElement(insuranceOtherViewDescriptionLabel).Displayed);

                if(lease.OtherDescriptionCoverage != "")
                    Assert.True(webDriver.FindElement(insuranceOtherViewDescriptionContent).Text.Equals(lease.OtherDescriptionCoverage));
            }
        }

        public int TotalInsuranceCount()
        {
            return webDriver.FindElements(insuranceViewTotal).Count;
        }
            
    }
}
