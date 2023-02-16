

namespace PIMS.Tests.Automation.StepDefinitions
{
    [Binding]
    public class PropertiesSteps
    {
        private readonly LoginSteps loginSteps;
        private SearchProperties searchProperties;
        private PropertyInformation propertyInformation;

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


        public PropertiesSteps(BrowserDriver driver)
        {
            loginSteps = new LoginSteps(driver);
            searchProperties = new SearchProperties(driver.Current);
            propertyInformation = new PropertyInformation(driver.Current);
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
    }
}
