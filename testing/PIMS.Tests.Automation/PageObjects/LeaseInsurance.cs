

using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class LeaseInsurance : PageObjectBase
    {
        //Insurance Menu Elements
        private By licenseInsuranceLink = By.XPath("//a[contains(text(),'Insurance')]");
        private By insuranceEditIcon = By.XPath("//div[@role='tabpanel']/div/div/div/button");

        //Insurance Create Form Elements
        private By insuranceSubtitle = By.XPath("//div[contains(text(),'Required Coverage')]");
        private By insuranceSelectionLabel = By.XPath("//label[contains(text(),'Select coverage types')]");

        private By insuranceAircraftLabel = By.XPath("//label[contains(text(),'Aircraft Liability Coverage')]");
        private By insuranceAircraftCheckbox = By.Id("insurance-0");
        private By insuranceCGLLabel = By.XPath("//label[contains(text(),'Commercial General Liability (CGL)')]");
        private By insuranceCGLCheckbox = By.Id("insurance-1");
        private By insuranceMarineLabel = By.XPath("//label[contains(text(),'Marine Liability Coverage')]");
        private By insuranceMarineCheckbox = By.Id("insurance-2");
        private By insuranceAccidentalLabel = By.XPath("//label[contains(text(),'Sudden and Accidental Coverage')]");
        private By insuranceAccidentalCheckbox = By.Id("insurance-3");
        private By insuranceUnmannedAirVehicleLabel = By.XPath("//label[contains(text(),'Unmanned Air Vehicle (UAV/Drone) Coverage')]");
        private By insuranceUnmannedAirVehicleCheckbox = By.Id("insurance-4");
        private By insuranceVehicleLabel = By.XPath("//label[contains(text(),'Vehicle Liability Coverage')]");
        private By insuranceVehicleCheckbox = By.Id("insurance-5");
        private By insuranceOtherLabel = By.XPath("//label[contains(text(),'Other Insurance Coverage')]");
        private By insuranceOtherCheckbox = By.Id("insurance-6");

        private By insuranceEditTotal = By.CssSelector("div[data-testid='insurance-form']");
        private By insuranceViewTotal = By.XPath("//div[@data-testid='insurance-section']/div[@data-testid='insurance-section']");

        private By insuranceAircraftSubtitle = By.XPath("//div[contains(text(),'Aircraft Liability Coverage')]");
        private By insuranceAircraftInPlaceLabel = By.XPath("//div[contains(text(),'Aircraft Liability Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(), 'Insurance in place')]");
        private By insuranceAircraftInPlaceSelect = By.Id("input-insurances.0.isInsuranceInPlaceSelect");
        private By insuranceAircraftLimitLabel = By.XPath("//div[contains(text(),'Aircraft Liability Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(), 'Limit ($)')]");
        private By insuranceAircraftLimitInput = By.Id("input-insurances.0.coverageLimit");
        private By insuranceAircraftExpiryDateLabel = By.XPath("//div[contains(text(),'Aircraft Liability Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(), 'Policy expiry')]");
        private By insuranceAircraftExpiryDateInput = By.Id("datepicker-insurances.0.expiryDate");
        private By insuranceAircraftDescriptionLabel = By.XPath("//div[contains(text(),'Aircraft Liability Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(), 'Description of coverage')]");
        private By insuranceAircraftDescriptionTextarea = By.Id("input-insurances.0.coverageDescription");

        private By insuranceCGLSubtitle = By.XPath("//div[contains(text(),'Commercial General Liability (CGL)')]");
        private By insuranceCGLInPlaceLabel = By.XPath("//div[contains(text(),'Commercial General Liability (CGL)')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(), 'Insurance in place')]");
        private By insuranceCGLInPlaceSelect = By.Id("input-insurances.1.isInsuranceInPlaceSelect");
        private By insuranceCGLLimitLabel = By.XPath("//div[contains(text(),'Commercial General Liability (CGL)')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(), 'Limit ($)')]");
        private By insuranceCGLLimitInput = By.Id("input-insurances.1.coverageLimit");
        private By insuranceCGLExpiryDateLabel = By.XPath("//div[contains(text(),'Commercial General Liability (CGL)')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(), 'Policy expiry')]");
        private By insuranceCGLExpiryDateInput = By.Id("datepicker-insurances.1.expiryDate");
        private By insuranceCGLDescriptionLabel = By.XPath("//div[contains(text(),'Commercial General Liability (CGL)')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(), 'Description of coverage')]");
        private By insuranceCGLDescriptionTextarea = By.Id("input-insurances.1.coverageDescription");

        private By insuranceMarineSubtitle = By.XPath("//div[contains(text(),'Marine Liability Coverage')]");
        private By insuranceMarineInPlaceLabel = By.XPath("//div[contains(text(),'Marine Liability Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(), 'Insurance in place')]");
        private By insuranceMarineInPlaceSelect = By.Id("input-insurances.2.isInsuranceInPlaceSelect");
        private By insuranceMarineLimitLabel = By.XPath("//div[contains(text(),'Marine Liability Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(), 'Limit ($)')]");
        private By insuranceMarineLimitInput = By.Id("input-insurances.2.coverageLimit");
        private By insuranceMarineExpiryDateLabel = By.XPath("//div[contains(text(),'Marine Liability Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(), 'Policy expiry')]");
        private By insuranceMarineExpiryDateInput = By.Id("datepicker-insurances.2.expiryDate");
        private By insuranceMarineDescriptionLabel = By.XPath("//div[contains(text(),'Marine Liability Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(), 'Description of coverage')]");
        private By insuranceMarineDescriptionTextarea = By.Id("input-insurances.2.coverageDescription");

        private By insuranceAccidentalSubtitle = By.XPath("//div[contains(text(),'Sudden and Accidental Coverage')]");
        private By insuranceAccidentalInPlaceLabel = By.XPath("//div[contains(text(),'Sudden and Accidental Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(), 'Insurance in place')]");
        private By insuranceAccidentalInPlaceSelect = By.Id("input-insurances.3.isInsuranceInPlaceSelect");
        private By insuranceAccidentalLimitLabel = By.XPath("//div[contains(text(),'Sudden and Accidental Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(), 'Limit ($)')]");
        private By insuranceAccidentalLimitInput = By.Id("input-insurances.3.coverageLimit");
        private By insuranceAccidentalExpiryDateLabel = By.XPath("//div[contains(text(),'Sudden and Accidental Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(), 'Policy expiry')]");
        private By insuranceAccidentalExpiryDateInput = By.Id("datepicker-insurances.3.expiryDate");
        private By insuranceAccidentalDescriptionLabel = By.XPath("//div[contains(text(),'Accidental Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(), 'Description of coverage')]");
        private By insuranceAccidentalDescriptionTextarea = By.Id("input-insurances.3.coverageDescription");

        private By insuranceUnmannedAirVehicleSubtitle = By.XPath("//div[contains(text(),'Unmanned Air Vehicle (UAV/Drone) Coverage')]");
        private By insuranceUnmannedAirVehicleInPlaceLabel = By.XPath("//div[contains(text(),'Unmanned Air Vehicle (UAV/Drone) Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(), 'Insurance in place')]");
        private By insuranceUnmannedAirVehicleInPlaceSelect = By.Id("input-insurances.4.isInsuranceInPlaceSelect");
        private By insuranceUnmannedAirVehicleLimitLabel = By.XPath("//div[contains(text(),'Unmanned Air Vehicle (UAV/Drone) Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(), 'Limit ($)')]");
        private By insuranceUnmannedAirVehicleLimitInput = By.Id("input-insurances.4.coverageLimit");
        private By insuranceUnmannedAirVehicleExpiryDateLabel = By.XPath("//div[contains(text(),'Unmanned Air Vehicle (UAV/Drone) Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(), 'Policy expiry')]");
        private By insuranceUnmannedAirVehicleExpiryDateInput = By.Id("datepicker-insurances.4.expiryDate");
        private By insuranceUnmannedAirVehicleDescriptionLabel = By.XPath("//div[contains(text(),'Unmanned Air Vehicle (UAV/Drone) Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(), 'Description of coverage')]");
        private By insuranceUnmannedAirVehicleDescriptionTextarea = By.Id("input-insurances.4.coverageDescription");

        private By insuranceVehicleSubtitle = By.XPath("//div[contains(text(),'Vehicle Liability Coverage')]");
        private By insuranceVehicleInPlaceLabel = By.XPath("//div[contains(text(),'Vehicle Liability Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(), 'Insurance in place')]");
        private By insuranceVehicleInPlaceSelect = By.Id("input-insurances.5.isInsuranceInPlaceSelect");
        private By insuranceVehicleLimitLabel = By.XPath("//div[contains(text(),'Vehicle Liability Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(), 'Limit ($)')]");
        private By insuranceVehicleLimitInput = By.Id("input-insurances.5.coverageLimit");
        private By insuranceVehicleExpiryDateLabel = By.XPath("//div[contains(text(),'Vehicle Liability Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(), 'Policy expiry')]");
        private By insuranceVehicleExpiryDateInput = By.Id("datepicker-insurances.5.expiryDate");
        private By insuranceVehicleDescriptionLabel = By.XPath("//div[contains(text(),'Vehicle Liability Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(), 'Description of coverage')]");
        private By insuranceVehicleDescriptionTextarea = By.Id("input-insurances.5.coverageDescription");

        private By insuranceOtherSubtitle = By.XPath("//div[contains(text(),'Other Insurance Coverage')]");
        private By insuranceOtherTypeLabel = By.XPath("//div[contains(text(),'Other Insurance Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(), 'Other insurance type')]");
        private By insuranceOtherTypeInput = By.Id("input-insurances.6.otherInsuranceType");
        private By insuranceOtherInPlaceLabel = By.XPath("//div[contains(text(),'Other Insurance Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(), 'Insurance in place')]");
        private By insuranceOtherInPlaceSelect = By.Id("input-insurances.6.isInsuranceInPlaceSelect");
        private By insuranceOtherLimitLabel = By.XPath("//div[contains(text(),'Other Insurance Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(), 'Limit ($)')]");
        private By insuranceOtherLimitInput = By.Id("input-insurances.6.coverageLimit");
        private By insuranceOtherExpiryDateLabel = By.XPath("//div[contains(text(),'Other Insurance Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(), 'Policy expiry')]");
        private By insuranceOtherExpiryDateInput = By.Id("datepicker-insurances.6.expiryDate");
        private By insuranceOtherDescriptionLabel = By.XPath("//div[contains(text(),'Other Insurance Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(), 'Description of coverage')]");
        private By insuranceOtherDescriptionTextarea = By.Id("input-insurances.6.coverageDescription");

        //Insurance View Form Elements
        private By insuranceRequiredLabel = By.XPath("//div[contains(text(),'Required insurance')]");
        private By insuranceAccidentalListOption = By.XPath("//li[contains(text(),'Accidental Coverage')]");
        private By insuranceAircraftListOption = By.XPath("//li[contains(text(),'Aircraft Liability Coverage')]");
        private By insuranceCGLListOption = By.XPath("//li[contains(text(),'Commercial General Liability (CGL)')]");
        private By insuranceMarineListOption = By.XPath("//li[contains(text(),'Marine Liability Coverage')]");
        private By insuranceUnmannedAirListOption = By.XPath("//li[contains(text(),'Unmanned Air Vehicle Coverage')]");
        private By insuranceVehicleListOption = By.XPath("//li[contains(text(),'Vehicle Liability Coverage')]");
        private By insuranceOtherListOption = By.XPath("//li[contains(text(),'Other Insurance Coverage')]");

        private By insuranceAccidentalViewSubtitle = By.XPath("//div[contains(text(),'Accidental Coverage')]");
        private By insuranceAccidentalViewInPlaceLabel = By.XPath("//div[contains(text(),'Accidental Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Insurance in place')]");
        private By insuranceAccidentalViewInPlaceContent = By.XPath("//div[contains(text(),'Accidental Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Insurance in place')]/parent::div/following-sibling::div");
        private By insuranceAccidentalViewLimitLabel = By.XPath("//div[contains(text(),'Accidental Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Limit')]");
        private By insuranceAccidentalViewLimitContent = By.XPath("//div[contains(text(),'Accidental Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Limit')]/parent::div/following-sibling::div");
        private By insuranceAccidentalViewExpiryDateLabel = By.XPath("//div[contains(text(),'Accidental Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Policy expiry date')]");
        private By insuranceAccidentalViewExpiryDateContent = By.XPath("//div[contains(text(),'Accidental Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Policy expiry date')]/parent::div/following-sibling::div");
        private By insuranceAccidentalViewDescriptionLabel = By.XPath("//div[contains(text(),'Accidental Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Description of Coverage')]");
        private By insuranceAccidentalViewDescriptionContent = By.XPath("//div[contains(text(),'Accidental Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Description of Coverage')]/parent::div/following-sibling::div");

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

        private By insuranceUnmmaedAirVehicleViewSubtitle = By.XPath("//div[contains(text(),'Unmanned Air Vehicle (UAV/Drone) Coverage')]");
        private By insuranceUnmmaedAirVehicleViewInPlaceLabel = By.XPath("//div[contains(text(),'Unmanned Air Vehicle (UAV/Drone) Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Insurance in place')]");
        private By insuranceUnmmaedAirVehicleViewInPlaceContent = By.XPath("//div[contains(text(),'Unmanned Air Vehicle (UAV/Drone) Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Insurance in place')]/parent::div/following-sibling::div");
        private By insuranceUnmmaedAirVehicleViewLimitLabel = By.XPath("//div[contains(text(),'Unmanned Air Vehicle (UAV/Drone) Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Limit')]");
        private By insuranceUnmmaedAirVehicleViewLimitContent = By.XPath("//div[contains(text(),'Unmanned Air Vehicle (UAV/Drone) Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Limit')]/parent::div/following-sibling::div");
        private By insuranceUnmmaedAirVehicleViewExpiryDateLabel = By.XPath("//div[contains(text(),'Unmanned Air Vehicle (UAV/Drone) Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Policy expiry date')]");
        private By insuranceUnmmaedAirVehicleViewExpiryDateContent = By.XPath("//div[contains(text(),'Unmanned Air Vehicle (UAV/Drone) Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Policy expiry date')]/parent::div/following-sibling::div");
        private By insuranceUnmmaedAirVehicleViewDescriptionLabel = By.XPath("//div[contains(text(),'Unmanned Air Vehicle (UAV/Drone) Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Description of Coverage')]");
        private By insuranceUnmmaedAirVehicleViewDescriptionContent = By.XPath("//div[contains(text(),'Unmanned Air Vehicle (UAV/Drone) Coverage')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Description of Coverage')]/parent::div/following-sibling::div");

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

        //Add Accidental Coverage
        public void AddAccidentalInsurance(Lease lease)
        {
            FocusAndClick(insuranceAccidentalCheckbox);

            //Verify Presence of elements on the form
            AssertTrueIsDisplayed(insuranceAccidentalSubtitle);
            AssertTrueIsDisplayed(insuranceAccidentalInPlaceLabel);
            AssertTrueIsDisplayed(insuranceAccidentalInPlaceSelect);
            AssertTrueIsDisplayed(insuranceAccidentalLimitLabel);
            AssertTrueIsDisplayed(insuranceAccidentalLimitInput);
            AssertTrueIsDisplayed(insuranceAccidentalExpiryDateLabel);
            AssertTrueIsDisplayed(insuranceAccidentalExpiryDateInput);
            AssertTrueIsDisplayed(insuranceAccidentalDescriptionLabel);
            AssertTrueIsDisplayed(insuranceAccidentalDescriptionTextarea); 

            //Fill out form
            if (lease.AccidentalInsuranceInPlace != "")
                ChooseSpecificSelectOption(insuranceAccidentalInPlaceSelect, lease.AccidentalInsuranceInPlace);

            if (lease.AccidentalLimit != "")
                webDriver.FindElement(insuranceAccidentalLimitInput).SendKeys(lease.AccidentalLimit);

            if (lease.AccidentalPolicyExpiryDate != "")
            {
                webDriver.FindElement(insuranceAccidentalExpiryDateInput).Click();
                webDriver.FindElement(insuranceAccidentalExpiryDateInput).SendKeys(lease.AccidentalPolicyExpiryDate);
                webDriver.FindElement(insuranceAccidentalExpiryDateInput).SendKeys(Keys.Enter);
            }                

            if (lease.AccidentalDescriptionCoverage != "")
            {
                webDriver.FindElement(insuranceAccidentalDescriptionTextarea).Click();
                webDriver.FindElement(insuranceAccidentalDescriptionTextarea).SendKeys(lease.AccidentalDescriptionCoverage);
            }    
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
                ChooseSpecificSelectOption(insuranceAircraftInPlaceSelect, lease.AircraftInsuranceInPlace);

            if (lease.AircraftLimit != "")
                webDriver.FindElement(insuranceAircraftLimitInput).SendKeys(lease.AircraftLimit);

            if (lease.AircraftPolicyExpiryDate != "")
            {
                webDriver.FindElement(insuranceAircraftExpiryDateInput).Click();
                webDriver.FindElement(insuranceAircraftExpiryDateInput).SendKeys(lease.AircraftPolicyExpiryDate);
                webDriver.FindElement(insuranceAircraftExpiryDateInput).SendKeys(Keys.Enter);
            }                
            if (lease.AircraftDescriptionCoverage != "")
            {
                webDriver.FindElement(insuranceAircraftDescriptionTextarea).Click();
                webDriver.FindElement(insuranceAircraftDescriptionTextarea).SendKeys(lease.AircraftDescriptionCoverage);
            }
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
                WaitUntilClickable(insuranceCGLInPlaceSelect);
                ChooseSpecificSelectOption(insuranceCGLInPlaceSelect, lease.CGLInsuranceInPlace);
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
                WaitUntilClickable(insuranceMarineInPlaceSelect);
                ChooseSpecificSelectOption(insuranceMarineInPlaceSelect, lease.MarineInsuranceInPlace);
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

        //Add Unmanned Air Vehicle Insurance
        public void AddUnmannedAirVehicleInsurance(Lease lease)
        {
            WaitUntilClickable(insuranceUnmannedAirVehicleCheckbox);
            FocusAndClick(insuranceUnmannedAirVehicleCheckbox);

            //Verify Presence of elements on the form
            AssertTrueIsDisplayed(insuranceUnmannedAirVehicleSubtitle);
            AssertTrueIsDisplayed(insuranceUnmannedAirVehicleInPlaceLabel);
            AssertTrueIsDisplayed(insuranceUnmannedAirVehicleInPlaceSelect);
            AssertTrueIsDisplayed(insuranceUnmannedAirVehicleLimitLabel);
            AssertTrueIsDisplayed(insuranceUnmannedAirVehicleLimitInput);
            AssertTrueIsDisplayed(insuranceUnmannedAirVehicleExpiryDateLabel);
            AssertTrueIsDisplayed(insuranceUnmannedAirVehicleExpiryDateInput);
            AssertTrueIsDisplayed(insuranceUnmannedAirVehicleDescriptionLabel);
            AssertTrueIsDisplayed(insuranceUnmannedAirVehicleDescriptionTextarea);  

            //Fill out form

            if (lease.UnmannedAirVehicleInsuranceInPlace != "")
            {
                WaitUntilClickable(insuranceUnmannedAirVehicleInPlaceSelect);
                ChooseSpecificSelectOption(insuranceUnmannedAirVehicleInPlaceSelect, lease.UnmannedAirVehicleInsuranceInPlace);
            }

            if (lease.UnmannedAirVehicleLimit != "")
            {
                WaitUntilClickable(insuranceUnmannedAirVehicleLimitInput);
                webDriver.FindElement(insuranceUnmannedAirVehicleLimitInput).SendKeys(lease.UnmannedAirVehicleLimit);
            }   

            if (lease.UnmannedAirVehiclePolicyExpiryDate != "")
            {
                WaitUntilClickable(insuranceUnmannedAirVehicleExpiryDateInput);
                webDriver.FindElement(insuranceUnmannedAirVehicleExpiryDateInput).Click();
                webDriver.FindElement(insuranceUnmannedAirVehicleExpiryDateInput).SendKeys(lease.UnmannedAirVehiclePolicyExpiryDate);
                webDriver.FindElement(insuranceUnmannedAirVehicleExpiryDateInput).SendKeys(Keys.Enter);
            }

            if (lease.UnmannedAirVehicleDescriptionCoverage != "")
            {
                WaitUntilClickable(insuranceUnmannedAirVehicleDescriptionTextarea);
                webDriver.FindElement(insuranceUnmannedAirVehicleDescriptionTextarea).Click();
                webDriver.FindElement(insuranceUnmannedAirVehicleDescriptionTextarea).SendKeys(lease.UnmannedAirVehicleDescriptionCoverage);
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
                WaitUntilClickable(insuranceVehicleInPlaceSelect);
                ChooseSpecificSelectOption(insuranceVehicleInPlaceSelect, lease.VehicleInsuranceInPlace);
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
            AssertTrueIsDisplayed(insuranceOtherTypeLabel);
            AssertTrueIsDisplayed(insuranceOtherTypeInput);
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
                WaitUntilVisible(insuranceOtherInPlaceSelect);
                ChooseSpecificSelectOption(insuranceOtherInPlaceSelect, lease.OtherInsuranceInPlace);
            }
                
            if (lease.OtherLimit != "")
            {
                WaitUntilVisible(insuranceOtherInPlaceSelect);
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
            var lastInsuranceInserted = webDriver.FindElement(By.CssSelector("div[data-testid='insurance-form']:last-child h2")).Text;
            
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
            AssertTrueIsDisplayed(insuranceSelectionLabel);

            AssertTrueIsDisplayed(insuranceAccidentalLabel);
            AssertTrueIsDisplayed(insuranceAccidentalCheckbox);
            AssertTrueIsDisplayed(insuranceAircraftLabel);
            AssertTrueIsDisplayed(insuranceAircraftCheckbox);
            AssertTrueIsDisplayed(insuranceCGLLabel);
            AssertTrueIsDisplayed(insuranceCGLCheckbox);
            AssertTrueIsDisplayed(insuranceMarineLabel);
            AssertTrueIsDisplayed(insuranceMarineCheckbox);
            AssertTrueIsDisplayed(insuranceUnmannedAirVehicleLabel);
            AssertTrueIsDisplayed(insuranceUnmannedAirVehicleCheckbox);
            AssertTrueIsDisplayed(insuranceVehicleLabel);
            AssertTrueIsDisplayed(insuranceVehicleCheckbox);
            AssertTrueIsDisplayed(insuranceOtherLabel);
            AssertTrueIsDisplayed(insuranceOtherCheckbox);
        }

        public void VerifyInsuranceViewForm(Lease lease)
        {
            AssertTrueIsDisplayed(insuranceRequiredLabel);

            //Accidental Coverage
            if (webDriver.FindElements(insuranceAccidentalListOption).Count > 1)
            {
                AssertTrueIsDisplayed(insuranceAccidentalViewSubtitle);
                AssertTrueIsDisplayed(insuranceAccidentalViewInPlaceLabel);

                if (lease.AccidentalInsuranceInPlace != "")
                    AssertTrueContentEquals(insuranceAccidentalViewInPlaceContent, lease.AccidentalInsuranceInPlace);

                AssertTrueIsDisplayed(insuranceAccidentalViewLimitLabel);

                if (lease.AccidentalLimit != "")
                    AssertTrueContentEquals(insuranceAccidentalViewLimitContent, lease.AccidentalLimit);

                AssertTrueIsDisplayed(insuranceAccidentalViewExpiryDateLabel);

                if (lease.AccidentalPolicyExpiryDate != "")
                    AssertTrueContentEquals(insuranceAccidentalViewExpiryDateContent, TransformDateFormat(lease.AccidentalPolicyExpiryDate));

                if (lease.AccidentalDescriptionCoverage != "")
                    AssertTrueIsDisplayed(insuranceAircraftViewDescriptionLabel);

                AssertTrueContentEquals(insuranceAccidentalViewDescriptionContent, lease.AccidentalDescriptionCoverage);
            }

            //Aircraft Liability Coverage
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

            //CGL Coverage
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

            //Marine Coverage
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

            //Unmanned Air Vehicle Coverage
            if (webDriver.FindElements(insuranceUnmannedAirListOption).Count > 1)
            { 
                AssertTrueIsDisplayed(insuranceUnmmaedAirVehicleViewSubtitle);
                AssertTrueIsDisplayed(insuranceUnmmaedAirVehicleViewInPlaceLabel);

                if (lease.UnmannedAirVehicleInsuranceInPlace != "")
                    AssertTrueContentEquals(insuranceUnmmaedAirVehicleViewInPlaceContent, lease.UnmannedAirVehicleInsuranceInPlace);

                AssertTrueIsDisplayed(insuranceUnmmaedAirVehicleViewLimitLabel);

                if (lease.UnmannedAirVehicleLimit != "")
                    AssertTrueContentEquals(insuranceUnmmaedAirVehicleViewLimitContent, lease.UnmannedAirVehicleLimit);

                AssertTrueIsDisplayed(insuranceUnmmaedAirVehicleViewExpiryDateLabel);

                if (lease.UnmannedAirVehiclePolicyExpiryDate != "")
                    AssertTrueContentEquals(insuranceUnmmaedAirVehicleViewExpiryDateContent, TransformDateFormat(lease.UnmannedAirVehiclePolicyExpiryDate));

                AssertTrueIsDisplayed(insuranceUnmmaedAirVehicleViewDescriptionLabel);

                if (lease.UnmannedAirVehicleDescriptionCoverage != "")
                    AssertTrueContentEquals(insuranceUnmmaedAirVehicleViewDescriptionContent, lease.UnmannedAirVehicleDescriptionCoverage);
            }

            //Vehicle Liability Coverage
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

            //Other Coverage
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
            Wait();
            System.Diagnostics.Debug.WriteLine(insuranceViewTotal);
            return webDriver.FindElements(insuranceViewTotal).Count;
        }
            
    }
}
