

using PIMS.Tests.Automation.Classes;
using PIMS.Tests.Automation.Data;
using PIMS.Tests.Automation.PageObjects;
using System.Data;

namespace PIMS.Tests.Automation.StepDefinitions
{
    [Binding]
    public class PropertiesSteps
    {
        private readonly LoginSteps loginSteps;
        private SearchProperties searchProperties;
        private PropertyInformation propertyInformation;
        private readonly GenericSteps genericSteps;

        //private readonly string userName = "TRANPSP1";
        private readonly string userName = "sutairak";

        private string motiPropertyPID = "004-537-360";
        private string motiPropertyPIN = "90054791";
        private string propertyAddress = "2889 E 12th Ave";
        private string invalidPID = "000-000-000";

        private readonly string propertyDetailsAddressLine1 = "8989 Fake St.";
        private readonly string propertyDetailsAddressLine2 = "Apt 2305";
        private readonly string propertyDetailsCity = "Vancouver";
        private readonly string propertyDetailsPostalCode = "V6Z 8H9";
        private string municipalZone = "The Automated Zone";
        private string sqMts = "65";
        private string cubeMts = "103.59";
        private string PropertyInfoNotes = "Automation Test - Properties Edit";

        private Property property;


        public PropertiesSteps(BrowserDriver driver)
        {
            loginSteps = new LoginSteps(driver);
            searchProperties = new SearchProperties(driver.Current);
            propertyInformation = new PropertyInformation(driver.Current);
            genericSteps = new GenericSteps(driver);
            property = new Property();
        }

        [StepDefinition(@"I search for a Property in the Inventory by different filters")]
        public void SearchInventoryPropertyOnMap()
        {
            /* TEST COVERAGE:  PSP-1546, PSP-5090, PSP-5091, PSP-5092 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Search for a valid Address with the Search Bar
            searchProperties.SearchPropertyByAddress(propertyAddress);

            //Validate that the result gives only one pin
            Assert.True(searchProperties.PropertiesFoundCount() == 1);

            //Search for a valid PIN in Inventory
            searchProperties.SearchPropertyByPINPID(motiPropertyPIN);

            //Validate that the result gives only one pin
            Assert.True(searchProperties.PropertiesFoundCount() == 1);

            //Search for a valid PID in Inventory
            searchProperties.SearchPropertyByPINPID(motiPropertyPID);

            //Validate that the result gives only one pin
            Assert.True(searchProperties.PropertiesFoundCount() == 1);

        }

        [StepDefinition(@"I search for an Invalid Property")]
        public void SearchInvalidPropertyOnMap()
        {
            /* TEST COVERAGE: PSP-5004 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Search for an invalid Address with the Search Bar
            searchProperties.SearchPropertyByPINPID(invalidPID);

        }

        [StepDefinition(@"I review a Property's Information")]
        public void ReviewPropertyInformation()
        {
            /* TEST COVERAGE: PSP-1558, PSP-3589, PSP-3184, PSP-5163,  */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to the Inventory List View
            searchProperties.NavigatePropertyListView();

            //Validate List View Elements
            searchProperties.ValidatePropertyListView();

            //Select the first property from the list
            searchProperties.ChooseFirstPropertyFromList();

            //Validate Property Information Header
            propertyInformation.VerifyPropertyInformationHeader();

            //Validate the Property Details View
            propertyInformation.VerifyPropertyDetailsView("Property Information");

        }

        [StepDefinition(@"I search for a non MOTI property")]
        public void NonInventoryProperty()
        {
            /* TEST COVERAGE: PSP-3414 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Look for a non-inventory property
            searchProperties.SearchPropertyByAddress(propertyAddress);

            //Validate that the result gives only one pin
            Assert.True(searchProperties.PropertiesFoundCount() == 1);

            //Click on the founf property
            searchProperties.SelectFoundPin();
        }

        [StepDefinition(@"I make some changes on the selected property information")]
        public void EditPropertyInformationDetails()
        {
            /* TEST COVERAGE: PSP-3591, PSP-3590, PSP-5165, PSP-5163, PSP-5162, PSP-5164 */

            //Click on the Edit Property Information Button
            propertyInformation.EditPropertyInfoBttn();

            //Verify Property Information Edit Form
            propertyInformation.VerifyPropertyDetailsEditForm("Property Information");

            //Apply changes on the Property Information Form
            propertyInformation.UpdateMinPropertyDetails(PropertyInfoNotes);

            //Cancel changes
            propertyInformation.CancelPropertyDetails();

            //Click on the Edit Property Information Button
            propertyInformation.EditPropertyInfoBttn();

            //Apply changes on the Property Information Form
            propertyInformation.UpdateMaxPropertyDetails(propertyDetailsAddressLine1, propertyDetailsAddressLine2, propertyDetailsCity, propertyDetailsPostalCode, municipalZone, sqMts, cubeMts, PropertyInfoNotes);

            //Save changes
            propertyInformation.SavePropertyDetails();

        }

        [StepDefinition(@"I update a Property details from row number (.*)")]
        public void UpdatePropertyDetailsFromFile(int rowNumber)
        {
            /* TEST COVERAGE: PSP-3460, PSP-3599, PSP-3600, PSP-3612, PSP-3722, PSP-3462, PSP-4791 */

            //Navigate to Property Information Tab
            propertyInformation.NavigatePropertyDetailsTab();

            //Click on the Edit Property Information Button
            propertyInformation.EditPropertyInfoResearchBttn();

            //Insert some changes
            PopulateProperty(rowNumber);
            propertyInformation.UpdatePropertyDetails(property);

            //Save changes
            propertyInformation.SavePropertyDetails();
        }

        [StepDefinition(@"LTSA Pop-up Information validation is successful")]
        public void ValidateLTSAPopUp()
        {
            /* TEST COVERAGE: PSP-3186 */

            //Select found property on Map
            searchProperties.SelectFoundPin();

            //Validate LTSA Pop-up
            propertyInformation.VerifyPropertyMapPopUpView();

            //Close Property Details Form
            propertyInformation.ClosePropertyInfoModal();

            //Reset Map
            searchProperties.SearchPropertyReset();

            //Close LTSA Pop-up
            propertyInformation.CloseLTSAPopUp();
        }

        [StepDefinition(@"No Properties were found")]
        public void NonPropertyFound()
        {
            /* TEST COVERAGE: PSP-1548 */

            //Validate that the result gives only one pin
            Assert.True(searchProperties.PropertiesFoundCount() == 0);
        }

        [StepDefinition(@"A Property Information is saved successfully")]
        public void PropertyInfoSucess()
        {
            //Validate Property Information View Form after changes
            propertyInformation.VerifyPropertyDetailsView("Property Information");
        }

        [StepDefinition(@"Non-Inventory property renders correctly")]
        public void NonInventoryPropertySucess()
        {
            //Validate tabs counting
            Assert.True(propertyInformation.PropertyTabs() == 2);

            //Validate correct tabs are displayed
            propertyInformation.VerifyNonInventoryPropertyTabs();
        }

        [StepDefinition(@"Property Information is displayed correctly")]
        public void PropertyInformationViewDetailsSuccess()
        {
            /* TEST COVERAGE: PSP-4794 */
            propertyInformation.NavigatePropertyDetailsTab();
            propertyInformation.VerifyPropertyDetailsView("Research File");
        }

        private void PopulateProperty(int rowNumber)
        {
            DataTable propertiesSheet = ExcelDataContext.GetInstance().Sheets["Properties"];
            ExcelDataContext.PopulateInCollection(propertiesSheet);

            property.PropertyName = ExcelDataContext.ReadData(rowNumber, "PropertyName");
            property.Address.AddressLine1 = ExcelDataContext.ReadData(rowNumber, "AddressLine1");
            property.Address.AddressLine2 = ExcelDataContext.ReadData(rowNumber, "AddressLine2");
            property.Address.AddressLine3 = ExcelDataContext.ReadData(rowNumber, "AddressLine3");
            property.Address.City = ExcelDataContext.ReadData(rowNumber, "City");
            property.Address.PostalCode = ExcelDataContext.ReadData(rowNumber, "PostalCode");

            property.MOTIRegion = ExcelDataContext.ReadData(rowNumber, "MoTIRegion");
            property.HighwaysDistrict = ExcelDataContext.ReadData(rowNumber, "HighwaysDistrict");
            property.ElectoralDistrict = ExcelDataContext.ReadData(rowNumber, "ElectoralDistrict");
            property.AgriculturalLandReserve = ExcelDataContext.ReadData(rowNumber, "AgriculturalLandReserve");
            property.RailwayBelt = ExcelDataContext.ReadData(rowNumber, "RailwayBelt");
            property.LandParcelType = ExcelDataContext.ReadData(rowNumber, "LandParcelType");
            property.MunicipalZoning = ExcelDataContext.ReadData(rowNumber, "MunicipalZoning");
            property.Anomalies = genericSteps.PopulateLists(ExcelDataContext.ReadData(rowNumber, "Anomalies"));

            property.TenureStatus = genericSteps.PopulateLists(ExcelDataContext.ReadData(rowNumber, "TenureStatus"));
            property.ProvincialPublicHwy = ExcelDataContext.ReadData(rowNumber, "ProvincialPublicHwy");
            property.HighwayEstablishedBy = genericSteps.PopulateLists(ExcelDataContext.ReadData(rowNumber, "HighwayEstablishedBy"));
            property.AdjacentLandType = genericSteps.PopulateLists(ExcelDataContext.ReadData(rowNumber, "AdjacentLandType"));
            property.SqrMeters = ExcelDataContext.ReadData(rowNumber, "SqrMeters");
            property.IsVolumetric = bool.Parse(ExcelDataContext.ReadData(rowNumber, "IsVolumetric"));
            property.Volume = ExcelDataContext.ReadData(rowNumber, "Volume");
            property.VolumeType = ExcelDataContext.ReadData(rowNumber, "VolumeType");
            property.PropertyNotes = ExcelDataContext.ReadData(rowNumber, "Notes");
        }
    }
}
