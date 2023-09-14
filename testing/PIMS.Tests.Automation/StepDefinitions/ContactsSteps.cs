using Microsoft.Extensions.Configuration;
using System.Data;
using PIMS.Tests.Automation.Data;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.StepDefinitions
{
    [Binding]
    public class ContactsSteps
    {
        private readonly LoginSteps loginSteps;
        private readonly Contacts contacts;
        private readonly SearchContacts searchContacts;

        private readonly string userName = "TRANPSP1";

        private IndividualContact individualContact;
        private OrganizationContact organizationContact;

        public ContactsSteps(BrowserDriver driver)
        {
            loginSteps = new LoginSteps(driver);
            contacts = new Contacts(driver.Current);
            searchContacts = new SearchContacts(driver.Current);
        }

        [StepDefinition(@"I create a new Individual Contact from row number (.*)")]
        public void IndividualContact(int rowNumber)
        {
            /* TEST COVERAGE: PSP-2705, PSP-2797, PSP-4559 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Create new contact form
            contacts.NavigateToCreateNewContact();

            PopulateIndividualContact(rowNumber);

            //Create new Individual Contact with maximum fields
            contacts.CreateIndividualContact(individualContact);

            //Save Contact
            contacts.SaveContact();

            //Verify Individual Contact Details View
            contacts.VerifyIndividualContactView(individualContact);
        }

        [StepDefinition(@"I create a new Organization Contact from row number (.*)")]
        public void OrganizationContact(int rowNumber)
        {
            /* TEST COVERAGE: PSP-4208, PSP-2797, PSP-4559 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Create new contact form
            contacts.NavigateToCreateNewContact();

            //Populate the organization Object with selected data
            PopulateOrganizationContact(rowNumber);

            //Create new Organization Contact with maximum fields
            contacts.CreateOrganizationContact(organizationContact);

            //Save Contact
            contacts.SaveContact();

            //Verify Organization Contact View Form
            contacts.VerifyOrganizationContactView(organizationContact);
        }

        [StepDefinition(@"I update an existing Organization Contact from row number (.*)")]
        public void UpdateOrganizationContact(int rowNumber)
        {
            /* TEST COVERAGE: PSP-3021, PSP-4200, PSP-4559 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Search a Contact
            searchContacts.NavigateToSearchContact();

            //Search for a contact
            PopulateOrganizationContact(rowNumber);
            searchContacts.SearchOrganizationContact(organizationContact.OrganizationName);

            //Select the first option from search
            searchContacts.SelectFirstResultLink();

            //Update an Organization Contact
            contacts.UpdateOrganizationContact(organizationContact);

            //Save Contact
            contacts.SaveContact();

        }

        [StepDefinition(@"I update an existing Individual Contact from row number (.*)")]
        public void UpdateIndividualContact(int rowNumber)
        {
            /* TEST COVERAGE: PSP-4207, PSP-4200, PSP-4559 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Search a Contact
            searchContacts.NavigateToSearchContact();
           
            PopulateIndividualContact(rowNumber);
            searchContacts.SearchIndividualContact(individualContact.FullName);

            //Select the first option from search
            searchContacts.SelectFirstResultLink();

            //Update an Organization Contact
            contacts.UpdateIndividualContact(individualContact);

            //Save Contact
            contacts.SaveContact();
        }

        [StepDefinition(@"I search for an existing contact from type ""(.*)"" row number (.*)")]
        public void SearchExistingContact(string contactType, int rowNumber)
        {
            /* TEST COVERAGE: PSP-4200, PSP-4559, PSP-4559 */

            //Navigate to Search a Contact
            searchContacts.NavigateToSearchContact();

            if (contactType == "Individual")
            {
                //PopulateIndividualContact(rowNumber);
                searchContacts.SearchGeneralContact(individualContact.FullName);
            }
            else
            {
                //PopulateOrganizationContact(rowNumber);
                searchContacts.SearchGeneralContact(organizationContact.OrganizationName);
            }
        }

        [StepDefinition(@"I search for an non-existing contact from type ""(.*)"" row number (.*)")]
        public void SearchNonExistantContact(string contactType, int rowNumber)
        {
            /* TEST COVERAGE: PSP-4200, PSP-4559 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Search a Contact
            searchContacts.NavigateToSearchContact();

            //Search for a non-existing contact
            if (contactType == "Individual")
            {
                PopulateIndividualContact(rowNumber);
                searchContacts.SearchGeneralContact(individualContact.FullName);
            }
            else
            {
                PopulateOrganizationContact(rowNumber);
                searchContacts.SearchGeneralContact(organizationContact.OrganizationName);
            }
        }

        [StepDefinition(@"I cancel creating a new contact from row number (.*)")]
        public void CancelContactCreation(int rowNumber)
        {
            /* TEST COVERAGE: PSP-2706, PSP-4559 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Search a Contact
            searchContacts.NavigateToSearchContact();

            //Click on Create new Contact button
            searchContacts.CreateNewContactFromSearch();

            PopulateIndividualContact(rowNumber);

            //Create new Individual Contact with minimum fields
            contacts.CreateIndividualContact(individualContact);

            //Cancel Contact
            contacts.CancelContact();
        }

        [StepDefinition(@"I verify the Contacts List View")]
        public void VerifyContactsListView()
        {
            /* TEST COVERAGE: PSP-2355, PSP-4559 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Search a Contact
            searchContacts.NavigateToSearchContact();

            //Verify List View
            searchContacts.VerifyContactsListView();
        }

        //ASSERT FUNCTIONS
        [StepDefinition(@"An Organization contact is successfully updated")]
        public void OrganizationContactUpdated()
        {
            /* TEST COVERAGE: PSP-4208 */

            contacts.VerifyOrganizationContactView(organizationContact);
        }

        [StepDefinition(@"An Individual contact is successfully updated")]
        public void IndividualContactUpdated()
        {
            /* TEST COVERAGE: PSP-2705 */
            contacts.VerifyIndividualContactView(individualContact);
        }

        [StepDefinition(@"Search Contacts screen is correctly rendered")]
        public void CorrectSearchContact()
        {
            /* TEST COVERAGE: PSP-2704, PSP-2541 */
            Assert.True(searchContacts.SearchContactRender());
        }

        [StepDefinition(@"No contacts results are found")]
        public void NoContactResults()
        {
            /* TEST COVERAGE: PSP-4200 */
            Assert.True(searchContacts.GetNoSearchMessage().Equals("No Contacts match the search criteria"));
        }

        [StepDefinition(@"Expected Content is displayed on Contacts Table from contact type ""(.*)""")]
        public void VerifyContactsTableContent(string contactType)
        {
            /* TEST COVERAGE: PSP-2355 */

            if (contactType == "Individual")
            {
                searchContacts.VerifyContactTableContent(individualContact.FullName, individualContact.FirstName, individualContact.LastName, individualContact.Organization, individualContact.IndEmail1, individualContact.IndMailAddressLine1, individualContact.IndMailCity, individualContact.IndMailProvDisplay, individualContact.IndMailCountry);
            }
            else
            {
                searchContacts.VerifyContactTableContent(organizationContact.OrganizationName, "", "", organizationContact.OrganizationName, organizationContact.OrgEmail1, organizationContact.OrgMailAddressLine1, organizationContact.OrgMailCity, organizationContact.OrgMailProvDisplay, organizationContact.OrgMailCountry);
            }
        }

        private void PopulateIndividualContact(int rowNumber)
        {
            DataTable individualContactSheet = ExcelDataContext.GetInstance().Sheets["IndividualContacts"];
            ExcelDataContext.PopulateInCollection(individualContactSheet);

            individualContact = new IndividualContact();
            individualContact.FirstName = ExcelDataContext.ReadData(rowNumber, "FirstName");
            individualContact.MiddleName = ExcelDataContext.ReadData(rowNumber, "MiddleName");
            individualContact.LastName = ExcelDataContext.ReadData(rowNumber, "LastName");
            individualContact.FullName = ExcelDataContext.ReadData(rowNumber, "FullName");
            individualContact.PreferableName = ExcelDataContext.ReadData(rowNumber, "PreferableName");
            individualContact.ContactStatus = ExcelDataContext.ReadData(rowNumber, "ContactStatus");
            individualContact.Organization = ExcelDataContext.ReadData(rowNumber, "Organization");

            individualContact.IndEmail1 = ExcelDataContext.ReadData(rowNumber, "IndEmail1");
            individualContact.IndEmailType1 = ExcelDataContext.ReadData(rowNumber, "IndEmailType1");
            individualContact.IndEmailTypeDisplay1 = ExcelDataContext.ReadData(rowNumber, "IndEmailTypeDisplay1");
            individualContact.IndEmail2 = ExcelDataContext.ReadData(rowNumber, "IndEmail2");
            individualContact.IndEmailType2 = ExcelDataContext.ReadData(rowNumber, "IndEmailType2");
            individualContact.IndEmailTypeDisplay2 = ExcelDataContext.ReadData(rowNumber, "IndEmailTypeDisplay2");
            individualContact.IndPhone1 = ExcelDataContext.ReadData(rowNumber, "IndPhone1");
            individualContact.IndPhoneType1 = ExcelDataContext.ReadData(rowNumber, "IndPhoneType1");
            individualContact.IndPhoneTypeDisplay1 = ExcelDataContext.ReadData(rowNumber, "IndPhoneTypeDisplay1");
            individualContact.IndPhone2 = ExcelDataContext.ReadData(rowNumber, "IndPhone2");
            individualContact.IndPhoneType2 = ExcelDataContext.ReadData(rowNumber, "IndPhoneType2");
            individualContact.IndPhoneTypeDisplay2 = ExcelDataContext.ReadData(rowNumber, "IndPhoneTypeDisplay2");

            individualContact.IndMailAddressLine1 = ExcelDataContext.ReadData(rowNumber, "IndMailAddressLine1");
            individualContact.IndMailAddressLine2 = ExcelDataContext.ReadData(rowNumber, "IndMailAddressLine2");
            individualContact.IndMailAddressLine3 = ExcelDataContext.ReadData(rowNumber, "IndMailAddressLine3");
            individualContact.IndMailCity = ExcelDataContext.ReadData(rowNumber, "IndMailCity");
            individualContact.IndMailProvince = ExcelDataContext.ReadData(rowNumber, "IndMailProvince");
            individualContact.IndMailProvDisplay = ExcelDataContext.ReadData(rowNumber, "IndMailProvDisplay");
            individualContact.IndMailCityProvinceView = ExcelDataContext.ReadData(rowNumber, "IndMailCityProvinceView");
            individualContact.IndMailCountry = ExcelDataContext.ReadData(rowNumber, "IndMailCountry");
            individualContact.IndMailOtherCountry = ExcelDataContext.ReadData(rowNumber, "IndMailOtherCountry");
            individualContact.IndMailPostalCode = ExcelDataContext.ReadData(rowNumber, "IndMailPostalCode");

            individualContact.IndPropertyAddressLine1 = ExcelDataContext.ReadData(rowNumber, "IndPropertyAddressLine1");
            individualContact.IndPropertyAddressLine2 = ExcelDataContext.ReadData(rowNumber, "IndPropertyAddressLine2");
            individualContact.IndPropertyAddressLine3 = ExcelDataContext.ReadData(rowNumber, "IndPropertyAddressLine3");
            individualContact.IndPropertyCity = ExcelDataContext.ReadData(rowNumber, "IndPropertyCity");
            individualContact.IndPropertyProvince = ExcelDataContext.ReadData(rowNumber, "IndPropertyProvince");
            individualContact.IndPropertyCityProvinceView = ExcelDataContext.ReadData(rowNumber, "IndPropertyCityProvinceView");
            individualContact.IndPropertyCountry = ExcelDataContext.ReadData(rowNumber, "IndPropertyCountry");
            individualContact.IndPropertyOtherCountry = ExcelDataContext.ReadData(rowNumber, "IndPropertyOtherCountry");
            individualContact.IndPropertyPostalCode = ExcelDataContext.ReadData(rowNumber, "IndPropertyPostalCode");

            individualContact.IndBillingAddressLine1 = ExcelDataContext.ReadData(rowNumber, "IndBillingAddressLine1");
            individualContact.IndBillingAddressLine2 = ExcelDataContext.ReadData(rowNumber, "IndBillingAddressLine2");
            individualContact.IndBillingAddressLine3 = ExcelDataContext.ReadData(rowNumber, "IndBillingAddressLine3");
            individualContact.IndBillingCity = ExcelDataContext.ReadData(rowNumber, "IndBillingCity");
            individualContact.IndBillingProvince = ExcelDataContext.ReadData(rowNumber, "IndBillingProvince");
            individualContact.IndBillingCityProvinceView = ExcelDataContext.ReadData(rowNumber, "IndBillingCityProvinceView");
            individualContact.IndBillingCountry = ExcelDataContext.ReadData(rowNumber, "IndBillingCountry");
            individualContact.IndBillingOtherCountry = ExcelDataContext.ReadData(rowNumber, "IndBillingOtherCountry");
            individualContact.IndBillingPostalCode = ExcelDataContext.ReadData(rowNumber, "IndBillingPostalCode");

            individualContact.IndComments = ExcelDataContext.ReadData(rowNumber, "IndComments");
        }

        private void PopulateOrganizationContact(int rowNumber)
        {
            DataTable organizationContactSheet = ExcelDataContext.GetInstance().Sheets["OrganizationContacts"];
            ExcelDataContext.PopulateInCollection(organizationContactSheet);

            organizationContact = new OrganizationContact();
            organizationContact.OrganizationName = ExcelDataContext.ReadData(rowNumber, "OrganizationName");
            organizationContact.Alias = ExcelDataContext.ReadData(rowNumber, "Alias");
            organizationContact.IncorporationNumber = ExcelDataContext.ReadData(rowNumber, "IncorporationNumber");
            organizationContact.ContactStatus = ExcelDataContext.ReadData(rowNumber, "ContactStatus");

            organizationContact.OrgEmail1 = ExcelDataContext.ReadData(rowNumber, "OrgEmail1");
            organizationContact.OrgEmailType1 = ExcelDataContext.ReadData(rowNumber, "OrgEmailType1");
            organizationContact.OrgEmailTypeDisplay1 = ExcelDataContext.ReadData(rowNumber, "OrgEmailTypeDisplay1");
            organizationContact.OrgEmail2 = ExcelDataContext.ReadData(rowNumber, "OrgEmail2");
            organizationContact.OrgEmailType2 = ExcelDataContext.ReadData(rowNumber, "OrgEmailType2");
            organizationContact.OrgEmailTypeDisplay2 = ExcelDataContext.ReadData(rowNumber, "OrgEmailTypeDisplay2");
            organizationContact.OrgPhone1 = ExcelDataContext.ReadData(rowNumber, "OrgPhone1");
            organizationContact.OrgPhoneType1 = ExcelDataContext.ReadData(rowNumber, "OrgPhoneType1");
            organizationContact.OrgPhoneTypeDisplay1 = ExcelDataContext.ReadData(rowNumber, "OrgPhoneTypeDisplay1");
            organizationContact.OrgPhone2 = ExcelDataContext.ReadData(rowNumber, "OrgPhone2");
            organizationContact.OrgPhoneType2 = ExcelDataContext.ReadData(rowNumber, "OrgPhoneType2");
            organizationContact.OrgPhoneTypeDisplay2 = ExcelDataContext.ReadData(rowNumber, "OrgPhoneTypeDisplay2");

            organizationContact.OrgMailAddressLine1 = ExcelDataContext.ReadData(rowNumber, "OrgMailAddressLine1");
            organizationContact.OrgMailAddressLine2 = ExcelDataContext.ReadData(rowNumber, "OrgMailAddressLine2");
            organizationContact.OrgMailAddressLine3 = ExcelDataContext.ReadData(rowNumber, "OrgMailAddressLine3");
            organizationContact.OrgMailCity = ExcelDataContext.ReadData(rowNumber, "OrgMailCity");
            organizationContact.OrgMailProvince = ExcelDataContext.ReadData(rowNumber, "OrgMailProvince");
            organizationContact.OrgMailProvDisplay = ExcelDataContext.ReadData(rowNumber, "OrgMailProvDisplay");
            organizationContact.OrgMailCityProvinceView = ExcelDataContext.ReadData(rowNumber, "OrgMailCityProvinceView");
            organizationContact.OrgMailCountry = ExcelDataContext.ReadData(rowNumber, "OrgMailCountry");
            organizationContact.OrgMailOtherCountry = ExcelDataContext.ReadData(rowNumber, "OrgMailOtherCountry");
            organizationContact.OrgMailPostalCode = ExcelDataContext.ReadData(rowNumber, "OrgMailPostalCode");

            organizationContact.OrgPropertyAddressLine1 = ExcelDataContext.ReadData(rowNumber, "OrgPropertyAddressLine1");
            organizationContact.OrgPropertyAddressLine2 = ExcelDataContext.ReadData(rowNumber, "OrgPropertyAddressLine2");
            organizationContact.OrgPropertyAddressLine3 = ExcelDataContext.ReadData(rowNumber, "OrgPropertyAddressLine3");
            organizationContact.OrgPropertyCity = ExcelDataContext.ReadData(rowNumber, "OrgPropertyCity");
            organizationContact.OrgPropertyProvince = ExcelDataContext.ReadData(rowNumber, "OrgPropertyProvince");
            organizationContact.OrgPropertyCityProvinceView = ExcelDataContext.ReadData(rowNumber, "OrgPropertyCityProvinceView");
            organizationContact.OrgPropertyCountry = ExcelDataContext.ReadData(rowNumber, "OrgPropertyCountry");
            organizationContact.OrgPropertyOtherCountry = ExcelDataContext.ReadData(rowNumber, "OrgPropertyOtherCountry");
            organizationContact.OrgPropertyPostalCode = ExcelDataContext.ReadData(rowNumber, "OrgPropertyPostalCode");

            organizationContact.OrgBillingAddressLine1 = ExcelDataContext.ReadData(rowNumber, "OrgBillingAddressLine1");
            organizationContact.OrgBillingAddressLine2 = ExcelDataContext.ReadData(rowNumber, "OrgBillingAddressLine2");
            organizationContact.OrgBillingAddressLine3 = ExcelDataContext.ReadData(rowNumber, "OrgBillingAddressLine3");
            organizationContact.OrgBillingCity = ExcelDataContext.ReadData(rowNumber, "OrgBillingCity");
            organizationContact.OrgBillingProvince = ExcelDataContext.ReadData(rowNumber, "OrgBillingProvince");
            organizationContact.OrgBillingCityProvinceView = ExcelDataContext.ReadData(rowNumber, "OrgBillingCityProvinceView");
            organizationContact.OrgBillingCountry = ExcelDataContext.ReadData(rowNumber, "OrgBillingCountry");
            organizationContact.OrgBillingOtherCountry = ExcelDataContext.ReadData(rowNumber, "OrgBillingOtherCountry");
            organizationContact.OrgBillingPostalCode = ExcelDataContext.ReadData(rowNumber, "OrgBillingPostalCode");

            organizationContact.OrgComments = ExcelDataContext.ReadData(rowNumber, "OrgComments");
        }
    }
}
