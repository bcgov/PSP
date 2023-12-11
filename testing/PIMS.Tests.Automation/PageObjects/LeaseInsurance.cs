

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
            WaitUntilClickable(licenseInsuranceLink);
            webDriver.FindElement(licenseInsuranceLink).Click();
        }

        //Edit Insurance section
        public void EditInsuranceButton()
        {
            WaitUntilClickable(insuranceEditIcon);
            webDriver.FindElement(insuranceEditIcon).Click();
        }

        //Add Aircraft Insurance
        public void AddAircraftInsurance(Lease lease)
        {
            FocusAndClick(insuranceAircraftCheckbox);

            //Verify Presence of elements on the form
            AssertTrueIsDisplayed(insuranceAircraftSubtitle);
            AssertTrueIsDisplayed(insuranceAircraftInPlaceLabel);
            AssertTrueIsDisplayed(insuranceAircraftLimitLabel);
            AssertTrueIsDisplayed(insuranceAircraftLimitInput);
            AssertTrueIsDisplayed(insuranceAircraftExpiryDateLabel);
            AssertTrueIsDisplayed(insuranceAircraftExpiryDateInput);
            AssertTrueIsDisplayed(insuranceAircraftDescriptionLabel);
            AssertTrueIsDisplayed(insuranceAircraftDescriptionTextarea);

            //Fill out form
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
            WaitUntilClickable(insuranceCGLCheckbox);
            FocusAndClick(insuranceCGLCheckbox);

            //Verify Presence of elements on the form
            WaitUntilVisible(insuranceCGLSubtitle);
            AssertTrueIsDisplayed(insuranceCGLSubtitle);
            AssertTrueIsDisplayed(insuranceCGLInPlaceLabel);
            AssertTrueIsDisplayed(insuranceCGLLimitLabel);
            AssertTrueIsDisplayed(insuranceCGLLimitInput);
            AssertTrueIsDisplayed(insuranceCGLExpiryDateLabel);
            AssertTrueIsDisplayed(insuranceCGLExpiryDateInput);
            AssertTrueIsDisplayed(insuranceCGLDescriptionLabel);
            AssertTrueIsDisplayed(insuranceCGLDescriptionTextarea);

            //Fill out form
            Wait();
            if (lease.CGLInsuranceInPlace != "")
            {
                WaitUntilClickable(insuranceCGLInPlaceRadioBttnGroup);
                ChooseSpecificRadioButton(insuranceCGLInPlaceRadioBttnGroup, lease.CGLInsuranceInPlace);
            }
            if (lease.CGLLimit != "")
            {
                WaitUntilClickable(insuranceCGLLimitInput);
                webDriver.FindElement(insuranceCGLLimitInput).SendKeys(lease.CGLLimit);
            }  
            if (lease.CGLPolicyExpiryDate != "")
            {
                WaitUntilClickable(insuranceCGLExpiryDateInput);
                webDriver.FindElement(insuranceCGLExpiryDateInput).Click();
                webDriver.FindElement(insuranceCGLExpiryDateInput).SendKeys(lease.CGLPolicyExpiryDate);
                webDriver.FindElement(insuranceCGLExpiryDateInput).SendKeys(Keys.Enter);
            }
                
            
            if (lease.CGLDescriptionCoverage != "")
            {
                WaitUntilClickable(insuranceCGLDescriptionTextarea);
                webDriver.FindElement(insuranceCGLDescriptionTextarea).Click();
                webDriver.FindElement(insuranceCGLDescriptionTextarea).SendKeys(lease.CGLDescriptionCoverage);
            }
        }

        //Add Marine Insurance
        public void AddMarineInsurance(Lease lease)
        {
            WaitUntilClickable(insuranceMarineCheckbox);
            FocusAndClick(insuranceMarineCheckbox);

            //Verify Presence of elements on the form
            AssertTrueIsDisplayed(insuranceMarineSubtitle);
            AssertTrueIsDisplayed(insuranceMarineInPlaceLabel);
            AssertTrueIsDisplayed(insuranceMarineLimitLabel);
            AssertTrueIsDisplayed(insuranceMarineLimitInput);
            AssertTrueIsDisplayed(insuranceMarineExpiryDateLabel);
            AssertTrueIsDisplayed(insuranceMarineExpiryDateInput);
            AssertTrueIsDisplayed(insuranceMarineDescriptionLabel);
            AssertTrueIsDisplayed(insuranceMarineDescriptionTextarea);

            //Fill out form

            if (lease.MarineInsuranceInPlace != "")
            {
                WaitUntilClickable(insuranceMarineInPlaceRadioBttnGroup);
                ChooseSpecificRadioButton(insuranceMarineInPlaceRadioBttnGroup, lease.MarineInsuranceInPlace);
            }
                
            if (lease.MarineLimit != "")
            {
                WaitUntilClickable(insuranceMarineLimitInput);
                webDriver.FindElement(insuranceMarineLimitInput).SendKeys(lease.MarineLimit);
            }
                
            if (lease.MarinePolicyExpiryDate != "")
            {
                WaitUntilClickable(insuranceMarineExpiryDateInput);
                webDriver.FindElement(insuranceMarineExpiryDateInput).Click();
                webDriver.FindElement(insuranceMarineExpiryDateInput).SendKeys(lease.MarinePolicyExpiryDate);
                webDriver.FindElement(insuranceMarineExpiryDateInput).SendKeys(Keys.Enter);
            }
                
            
            if (lease.MarineDescriptionCoverage != "")
            {
                WaitUntilClickable(insuranceMarineDescriptionTextarea);
                webDriver.FindElement(insuranceMarineDescriptionTextarea).Click();
                webDriver.FindElement(insuranceMarineDescriptionTextarea).SendKeys(lease.MarineDescriptionCoverage);
            }
                
        }

        //Add Vehicle Insurance
        public void AddVehicleInsurance(Lease lease)
        {
            FocusAndClick(insuranceVehicleCheckbox);

            //Verify Presence of elements on the form
            AssertTrueIsDisplayed(insuranceVehicleSubtitle);
            AssertTrueIsDisplayed(insuranceVehicleInPlaceLabel);
            AssertTrueIsDisplayed(insuranceVehicleLimitLabel);
            AssertTrueIsDisplayed(insuranceVehicleLimitInput);
            AssertTrueIsDisplayed(insuranceVehicleExpiryDateLabel);
            AssertTrueIsDisplayed(insuranceVehicleExpiryDateInput);
            AssertTrueIsDisplayed(insuranceVehicleDescriptionLabel);
            AssertTrueIsDisplayed(insuranceVehicleDescriptionTextarea);

            //Fill out form
            if (lease.VehicleInsuranceInPlace != "")
            {
                WaitUntilClickable(insuranceVehicleInPlaceRadioBttnGroup);
                ChooseSpecificRadioButton(insuranceVehicleInPlaceRadioBttnGroup, lease.VehicleInsuranceInPlace);
            }
                
            if (lease.VehicleLimit != "")
            {
                WaitUntilClickable(insuranceVehicleLimitInput);
                webDriver.FindElement(insuranceVehicleLimitInput).SendKeys(lease.VehicleLimit);
            } 
            
            if (lease.VehiclePolicyExpiryDate != "")
            {
                WaitUntilClickable(insuranceVehicleExpiryDateInput);
                webDriver.FindElement(insuranceVehicleExpiryDateInput).Click();
                webDriver.FindElement(insuranceVehicleExpiryDateInput).SendKeys(lease.VehiclePolicyExpiryDate);
                webDriver.FindElement(insuranceVehicleExpiryDateInput).SendKeys(Keys.Enter);
            }
                
            if (lease.VehicleDescriptionCoverage != "")
            {
                WaitUntilClickable(insuranceVehicleDescriptionTextarea);
                webDriver.FindElement(insuranceVehicleDescriptionTextarea).Click();
                webDriver.FindElement(insuranceVehicleDescriptionTextarea).SendKeys(lease.VehicleDescriptionCoverage);
            }
        }

        //Add Other Insurance
        public void AddOtherInsurance(Lease lease)
        {
            FocusAndClick(insuranceOtherCheckbox);

            //Verify Presence of elements on the form
            AssertTrueIsDisplayed(insuranceOtherSubtitle);
            AssertTrueIsDisplayed(insuranceOtherInPlaceLabel);
            AssertTrueIsDisplayed(insuranceOtherLimitLabel);
            AssertTrueIsDisplayed(insuranceOtherLimitInput);
            AssertTrueIsDisplayed(insuranceOtherExpiryDateLabel);
            AssertTrueIsDisplayed(insuranceOtherExpiryDateInput);
            AssertTrueIsDisplayed(insuranceOtherDescriptionLabel);
            AssertTrueIsDisplayed(insuranceOtherDescriptionTextarea);

            //Fill out form
            if (lease.OtherInsuranceType != "")
            {
                WaitUntilVisible(insuranceOtherTypeInput);
                webDriver.FindElement(insuranceOtherTypeInput).SendKeys(lease.OtherInsuranceType);
            }
                
            if (lease.OtherInsuranceInPlace != "")
            {
                WaitUntilVisible(insuranceOtherInPlaceRadioBttnGroup);
                ChooseSpecificRadioButton(insuranceOtherInPlaceRadioBttnGroup, lease.OtherInsuranceInPlace);
            }
                
            if (lease.OtherLimit != "")
            {
                WaitUntilVisible(insuranceOtherInPlaceRadioBttnGroup);
                webDriver.FindElement(insuranceOtherLimitInput).SendKeys(lease.OtherLimit);
            }
               
            if (lease.OtherPolicyExpiryDate != "")
            {
                WaitUntilVisible(insuranceOtherExpiryDateInput);
                webDriver.FindElement(insuranceOtherExpiryDateInput).SendKeys(lease.OtherPolicyExpiryDate);
                webDriver.FindElement(insuranceOtherExpiryDateInput).SendKeys(Keys.Enter);
            }
                
            if (lease.OtherDescriptionCoverage != "")
            {
                WaitUntilVisible(insuranceOtherDescriptionTextarea);
                webDriver.FindElement(insuranceOtherDescriptionTextarea).SendKeys(lease.OtherDescriptionCoverage);
            }  
        }

        public void DeleteLastInsurance()
        {
            WaitUntilVisible(insuranceEditTotal);
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
            AssertTrueIsDisplayed(insuranceSubtitle);
            AssertTrueIsDisplayed(insuranceInstructions);

            AssertTrueIsDisplayed(insuranceAircraftLabel);
            AssertTrueIsDisplayed(insuranceAircraftCheckbox);
            AssertTrueIsDisplayed(insuranceCGLLabel);
            AssertTrueIsDisplayed(insuranceCGLCheckbox);
            AssertTrueIsDisplayed(insuranceMarineLabel);
            AssertTrueIsDisplayed(insuranceMarineCheckbox);
            AssertTrueIsDisplayed(insuranceVehicleLabel);
            AssertTrueIsDisplayed(insuranceVehicleCheckbox);
            AssertTrueIsDisplayed(insuranceOtherLabel);
            AssertTrueIsDisplayed(insuranceOtherCheckbox);

            AssertTrueIsDisplayed(insuranceDetailsSubtitle);
        }

        public void VerifyInsuranceViewForm(Lease lease)
        {
            AssertTrueIsDisplayed(insuranceRequiredLabel);

            if (webDriver.FindElements(insuranceAircraftListOption).Count > 1)
            {
                AssertTrueIsDisplayed(insuranceAircraftViewSubtitle);
                AssertTrueIsDisplayed(insuranceAircraftViewInPlaceLabel);

                if (lease.AircraftInsuranceInPlace != "")
                    AssertTrueContentEquals(insuranceAircraftViewInPlaceContent, lease.AircraftInsuranceInPlace);

                AssertTrueIsDisplayed(insuranceAircraftViewLimitLabel);

                if (lease.AircraftLimit != "")
                    AssertTrueContentEquals(insuranceAircraftViewLimitContent,lease.AircraftLimit);

                AssertTrueIsDisplayed(insuranceAircraftViewExpiryDateLabel);

                if (lease.AircraftPolicyExpiryDate != "")
                    AssertTrueContentEquals(insuranceAircraftViewExpiryDateContent,TransformDateFormat(lease.AircraftPolicyExpiryDate));
              
                if (lease.AircraftDescriptionCoverage != "")
                    AssertTrueIsDisplayed(insuranceAircraftViewDescriptionLabel);

                AssertTrueContentEquals(insuranceAircraftViewDescriptionContent, lease.AircraftInsuranceInPlace);
            }

            if (webDriver.FindElements(insuranceCGLListOption).Count > 1)
            {
                AssertTrueIsDisplayed(insuranceCGLViewSubtitle);
                AssertTrueIsDisplayed(insuranceCGLViewInPlaceLabel);

                if (lease.CGLInsuranceInPlace != "")
                    AssertTrueContentEquals(insuranceCGLViewInPlaceContent, lease.CGLInsuranceInPlace);

                AssertTrueIsDisplayed(insuranceCGLViewLimitLabel);

                if (lease.CGLLimit != "")
                    AssertTrueContentEquals(insuranceCGLViewLimitContent, lease.CGLLimit);

                AssertTrueIsDisplayed(insuranceCGLViewExpiryDateLabel);

                if (lease.CGLPolicyExpiryDate != "")
                    AssertTrueContentEquals(insuranceCGLViewExpiryDateContent, TransformDateFormat(lease.CGLPolicyExpiryDate));

                AssertTrueIsDisplayed(insuranceCGLViewDescriptionLabel);

                if (lease.CGLDescriptionCoverage != "")
                    AssertTrueContentEquals(insuranceCGLViewDescriptionContent,lease.CGLDescriptionCoverage);
            }

            if (webDriver.FindElements(insuranceMarineListOption).Count > 1)
            {
                AssertTrueIsDisplayed(insuranceMarineViewSubtitle);
                AssertTrueIsDisplayed(insuranceMarineViewInPlaceLabel);

                if (lease.MarineInsuranceInPlace != "")
                    AssertTrueContentEquals(insuranceMarineViewInPlaceContent, lease.MarineInsuranceInPlace);

                AssertTrueIsDisplayed(insuranceMarineViewLimitLabel);

                if(lease.MarineLimit != "")
                    AssertTrueContentEquals(insuranceMarineViewLimitContent, lease.MarineLimit);

                AssertTrueIsDisplayed(insuranceMarineViewExpiryDateLabel);

                if(lease.MarinePolicyExpiryDate != "")
                    AssertTrueContentEquals(insuranceMarineViewExpiryDateContent, TransformDateFormat(lease.MarinePolicyExpiryDate));

                AssertTrueIsDisplayed(insuranceMarineViewDescriptionLabel);

                if(lease.MarineDescriptionCoverage != "")
                    AssertTrueContentEquals(insuranceMarineViewDescriptionContent, lease.MarineDescriptionCoverage);
            }

            if (webDriver.FindElements(insuranceVehicleListOption).Count > 1)
            {
                AssertTrueIsDisplayed(insuranceVehicleViewSubtitle);
                AssertTrueIsDisplayed(insuranceVehicleViewInPlaceLabel);

                if (lease.VehicleInsuranceInPlace != "")
                    AssertTrueContentEquals(insuranceVehicleViewInPlaceContent, lease.VehicleInsuranceInPlace);

                AssertTrueIsDisplayed(insuranceVehicleViewLimitLabel);

                if(lease.VehicleLimit != "")
                    AssertTrueContentEquals(insuranceVehicleViewLimitContent, lease.VehicleLimit);

                AssertTrueIsDisplayed(insuranceVehicleViewExpiryDateLabel);

                if (lease.VehiclePolicyExpiryDate != "")
                    AssertTrueContentEquals(insuranceVehicleViewExpiryDateContent, TransformDateFormat(lease.VehiclePolicyExpiryDate));

                AssertTrueIsDisplayed(insuranceVehicleViewDescriptionLabel);

                if (lease.VehicleDescriptionCoverage != "")
                    AssertTrueContentEquals(insuranceVehicleViewDescriptionContent,lease.VehicleDescriptionCoverage);
            }

            if (webDriver.FindElements(insuranceOtherListOption).Count > 1)
            {
                AssertTrueIsDisplayed(insuranceOtherViewSubtitle);
                AssertTrueIsDisplayed(insuranceOtherViewInPlaceLabel);

                if(lease.OtherInsuranceInPlace != "")
                    AssertTrueContentEquals(insuranceOtherViewInPlaceContent,lease.OtherInsuranceInPlace);

                AssertTrueIsDisplayed(insuranceOtherViewLimitLabel);

                if (lease.OtherLimit != "")
                    AssertTrueContentEquals(insuranceOtherViewLimitContent, lease.OtherLimit);

                AssertTrueIsDisplayed(insuranceOtherViewExpiryDateLabel);

                if(lease.OtherPolicyExpiryDate != "")
                    AssertTrueContentEquals(insuranceOtherViewExpiryDateContent, TransformDateFormat(lease.OtherPolicyExpiryDate));

                AssertTrueIsDisplayed(insuranceOtherViewDescriptionLabel);

                if (lease.OtherDescriptionCoverage != "")
                    AssertTrueContentEquals(insuranceOtherViewDescriptionContent, lease.OtherDescriptionCoverage);
            }
        }

        public int TotalInsuranceCount()
        {
            return webDriver.FindElements(insuranceViewTotal).Count;
        }
            
    }
}
