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
            individualContact = new IndividualContact();
            organizationContact = new OrganizationContact();
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
            //var contact = organizationContacts.SingleOrDefault(u => u.OrganizationName.Equals("Automation Test Corp II", StringComparison.OrdinalIgnoreCase));
            PopulateOrganizationContact(rowNumber);
            searchContacts.SearchOrganizationContact(organizationContact.OrganizationName);

            //Select the first option from search
            searchContacts.SelectFirstResultLink();

            //Update an Organization Contact
            contacts.UpdateContact(organizationContact.Email2, organizationContact.EmailType2, organizationContact.Phone2, organizationContact.PhoneType2);

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
            contacts.UpdateContact(individualContact.Email2, individualContact.EmailType2, individualContact.Phone2, individualContact.PhoneType2);

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

            contacts.Wait();
            Assert.True(searchContacts.SearchContactRender());
        }

        [StepDefinition(@"No contacts results are found")]
        public void NoContactResults()
        {
            /* TEST COVERAGE: PSP-4200 */

            contacts.Wait();
            Assert.True(searchContacts.GetNoSearchMessage().Equals("No Contacts match the search criteria"));
        }

        [StepDefinition(@"Expected Content is displayed on Contacts Table from contact type ""(.*)""")]
        public void VerifyContactsTableContent(string contactType)
        {
            /* TEST COVERAGE: PSP-2355 */

            if (contactType == "Individual")
            {
                searchContacts.VerifyContactTableContent(individualContact.FullName, individualContact.FirstName, individualContact.LastName, individualContact.Organization, individualContact.Email1, individualContact.MailAddressLine1, individualContact.MailCity, individualContact.MailProvDisplay, individualContact.MailCountry);
            }
            else
            {
                searchContacts.VerifyContactTableContent(organizationContact.OrganizationName, "", "", organizationContact.OrganizationName, organizationContact.Email1, organizationContact.MailAddressLine1, organizationContact.MailCity, organizationContact.MailProvDisplay, organizationContact.MailCountry);
            }
        }

        private void PopulateIndividualContact(int rowNumber)
        {
            DataTable individualContactSheet = ExcelDataContext.GetInstance().Sheets["IndividualContacts"];
            ExcelDataContext.PopulateInCollection(individualContactSheet);

            individualContact.FirstName = ExcelDataContext.ReadData(rowNumber, "FirstName");
            individualContact.MiddleName = ExcelDataContext.ReadData(rowNumber, "MiddleName");
            individualContact.LastName = ExcelDataContext.ReadData(rowNumber, "LastName");
            individualContact.FullName = ExcelDataContext.ReadData(rowNumber, "FullName");
            individualContact.PreferableName = ExcelDataContext.ReadData(rowNumber, "PreferableName");
            individualContact.ContactStatus = ExcelDataContext.ReadData(rowNumber, "ContactStatus");
            individualContact.Organization = ExcelDataContext.ReadData(rowNumber, "Organization");

            individualContact.Email1 = ExcelDataContext.ReadData(rowNumber, "Email1");
            individualContact.EmailType1 = ExcelDataContext.ReadData(rowNumber, "EmailType1");
            individualContact.EmailTypeDisplay1 = ExcelDataContext.ReadData(rowNumber, "EmailTypeDisplay1");
            individualContact.Email2 = ExcelDataContext.ReadData(rowNumber, "Email2");
            individualContact.EmailType2 = ExcelDataContext.ReadData(rowNumber, "EmailType2");
            individualContact.EmailTypeDisplay2 = ExcelDataContext.ReadData(rowNumber, "EmailTypeDisplay2");
            individualContact.Phone1 = ExcelDataContext.ReadData(rowNumber, "Phone1");
            individualContact.PhoneType1 = ExcelDataContext.ReadData(rowNumber, "PhoneType1");
            individualContact.PhoneTypeDisplay1 = ExcelDataContext.ReadData(rowNumber, "PhoneTypeDisplay1");
            individualContact.Phone2 = ExcelDataContext.ReadData(rowNumber, "Phone2");
            individualContact.PhoneType2 = ExcelDataContext.ReadData(rowNumber, "PhoneType2");
            individualContact.PhoneTypeDisplay2 = ExcelDataContext.ReadData(rowNumber, "PhoneTypeDisplay2");

            individualContact.MailAddressLine1 = ExcelDataContext.ReadData(rowNumber, "MailAddressLine1");
            individualContact.MailAddressLine2 = ExcelDataContext.ReadData(rowNumber, "MailAddressLine2");
            individualContact.MailAddressLine3 = ExcelDataContext.ReadData(rowNumber, "MailAddressLine3");
            individualContact.MailCity = ExcelDataContext.ReadData(rowNumber, "MailCity");
            individualContact.MailProvince = ExcelDataContext.ReadData(rowNumber, "MailProvince");
            individualContact.MailProvDisplay = ExcelDataContext.ReadData(rowNumber, "MailProvDisplay");
            individualContact.MailCityProvinceView = ExcelDataContext.ReadData(rowNumber, "MailCityProvinceView");
            individualContact.MailCountry = ExcelDataContext.ReadData(rowNumber, "MailCountry");
            individualContact.MailOtherCountry = ExcelDataContext.ReadData(rowNumber, "MailOtherCountry");
            individualContact.MailPostalCode = ExcelDataContext.ReadData(rowNumber, "MailPostalCode");

            individualContact.PropertyAddressLine1 = ExcelDataContext.ReadData(rowNumber, "PropertyAddressLine1");
            individualContact.PropertyAddressLine2 = ExcelDataContext.ReadData(rowNumber, "PropertyAddressLine2");
            individualContact.PropertyAddressLine3 = ExcelDataContext.ReadData(rowNumber, "PropertyAddressLine3");
            individualContact.PropertyCity = ExcelDataContext.ReadData(rowNumber, "PropertyCity");
            individualContact.PropertyProvince = ExcelDataContext.ReadData(rowNumber, "PropertyProvince");
            individualContact.PropertyCityProvinceView = ExcelDataContext.ReadData(rowNumber, "PropertyCityProvinceView");
            individualContact.PropertyCountry = ExcelDataContext.ReadData(rowNumber, "PropertyCountry");
            individualContact.PropertyOtherCountry = ExcelDataContext.ReadData(rowNumber, "PropertyOtherCountry");
            individualContact.PropertyPostalCode = ExcelDataContext.ReadData(rowNumber, "PropertyPostalCode");

            individualContact.BillingAddressLine1 = ExcelDataContext.ReadData(rowNumber, "BillingAddressLine1");
            individualContact.BillingAddressLine2 = ExcelDataContext.ReadData(rowNumber, "BillingAddressLine2");
            individualContact.BillingAddressLine3 = ExcelDataContext.ReadData(rowNumber, "BillingAddressLine3");
            individualContact.BillingCity = ExcelDataContext.ReadData(rowNumber, "BillingCity");
            individualContact.BillingProvince = ExcelDataContext.ReadData(rowNumber, "BillingProvince");
            individualContact.BillingCityProvinceView = ExcelDataContext.ReadData(rowNumber, "BillingCityProvinceView");
            individualContact.BillingCountry = ExcelDataContext.ReadData(rowNumber, "BillingCountry");
            individualContact.BillingOtherCountry = ExcelDataContext.ReadData(rowNumber, "BillingOtherCountry");
            individualContact.BillingPostalCode = ExcelDataContext.ReadData(rowNumber, "BillingPostalCode");

            individualContact.Comments = ExcelDataContext.ReadData(rowNumber, "Comments");
        }

        private void PopulateOrganizationContact(int rowNumber)
        {
            DataTable organizationContactSheet = ExcelDataContext.GetInstance().Sheets["OrganizationContacts"];
            ExcelDataContext.PopulateInCollection(organizationContactSheet);

            organizationContact.OrganizationName = ExcelDataContext.ReadData(rowNumber, "OrganizationName");
            organizationContact.Alias = ExcelDataContext.ReadData(rowNumber, "Alias");
            organizationContact.IncorporationNumber = ExcelDataContext.ReadData(rowNumber, "IncorporationNumber");
            organizationContact.ContactStatus = ExcelDataContext.ReadData(rowNumber, "ContactStatus");

            organizationContact.Email1 = ExcelDataContext.ReadData(rowNumber, "Email1");
            organizationContact.EmailType1 = ExcelDataContext.ReadData(rowNumber, "EmailType1");
            organizationContact.EmailTypeDisplay1 = ExcelDataContext.ReadData(rowNumber, "EmailTypeDisplay1");
            organizationContact.Email2 = ExcelDataContext.ReadData(rowNumber, "Email2");
            organizationContact.EmailType2 = ExcelDataContext.ReadData(rowNumber, "EmailType2");
            organizationContact.EmailTypeDisplay2 = ExcelDataContext.ReadData(rowNumber, "EmailTypeDisplay2");
            organizationContact.Phone1 = ExcelDataContext.ReadData(rowNumber, "Phone1");
            organizationContact.PhoneType1 = ExcelDataContext.ReadData(rowNumber, "PhoneType1");
            organizationContact.PhoneTypeDisplay1 = ExcelDataContext.ReadData(rowNumber, "PhoneTypeDisplay1");
            organizationContact.Phone2 = ExcelDataContext.ReadData(rowNumber, "Phone2");
            organizationContact.PhoneType2 = ExcelDataContext.ReadData(rowNumber, "PhoneType2");
            organizationContact.PhoneTypeDisplay2 = ExcelDataContext.ReadData(rowNumber, "PhoneTypeDisplay2");

            organizationContact.MailAddressLine1 = ExcelDataContext.ReadData(rowNumber, "MailAddressLine1");
            organizationContact.MailAddressLine2 = ExcelDataContext.ReadData(rowNumber, "MailAddressLine2");
            organizationContact.MailAddressLine3 = ExcelDataContext.ReadData(rowNumber, "MailAddressLine3");
            organizationContact.MailCity = ExcelDataContext.ReadData(rowNumber, "MailCity");
            organizationContact.MailProvince = ExcelDataContext.ReadData(rowNumber, "MailProvince");
            organizationContact.MailProvDisplay = ExcelDataContext.ReadData(rowNumber, "MailProvDisplay");
            organizationContact.MailCityProvinceView = ExcelDataContext.ReadData(rowNumber, "MailCityProvinceView");
            organizationContact.MailCountry = ExcelDataContext.ReadData(rowNumber, "MailCountry");
            organizationContact.MailOtherCountry = ExcelDataContext.ReadData(rowNumber, "MailOtherCountry");
            organizationContact.MailPostalCode = ExcelDataContext.ReadData(rowNumber, "MailPostalCode");

            organizationContact.PropertyAddressLine1 = ExcelDataContext.ReadData(rowNumber, "PropertyAddressLine1");
            organizationContact.PropertyAddressLine2 = ExcelDataContext.ReadData(rowNumber, "PropertyAddressLine2");
            organizationContact.PropertyAddressLine3 = ExcelDataContext.ReadData(rowNumber, "PropertyAddressLine3");
            organizationContact.PropertyCity = ExcelDataContext.ReadData(rowNumber, "PropertyCity");
            organizationContact.PropertyProvince = ExcelDataContext.ReadData(rowNumber, "PropertyProvince");
            organizationContact.PropertyCityProvinceView = ExcelDataContext.ReadData(rowNumber, "PropertyCityProvinceView");
            organizationContact.PropertyCountry = ExcelDataContext.ReadData(rowNumber, "PropertyCountry");
            organizationContact.PropertyOtherCountry = ExcelDataContext.ReadData(rowNumber, "PropertyOtherCountry");
            organizationContact.PropertyPostalCode = ExcelDataContext.ReadData(rowNumber, "PropertyPostalCode");

            organizationContact.BillingAddressLine1 = ExcelDataContext.ReadData(rowNumber, "BillingAddressLine1");
            organizationContact.BillingAddressLine2 = ExcelDataContext.ReadData(rowNumber, "BillingAddressLine2");
            organizationContact.BillingAddressLine3 = ExcelDataContext.ReadData(rowNumber, "BillingAddressLine3");
            organizationContact.BillingCity = ExcelDataContext.ReadData(rowNumber, "BillingCity");
            organizationContact.BillingProvince = ExcelDataContext.ReadData(rowNumber, "BillingProvince");
            organizationContact.BillingCityProvinceView = ExcelDataContext.ReadData(rowNumber, "BillingCityProvinceView");
            organizationContact.BillingCountry = ExcelDataContext.ReadData(rowNumber, "BillingCountry");
            organizationContact.BillingOtherCountry = ExcelDataContext.ReadData(rowNumber, "BillingOtherCountry");
            organizationContact.BillingPostalCode = ExcelDataContext.ReadData(rowNumber, "BillingPostalCode");

            organizationContact.Comments = ExcelDataContext.ReadData(rowNumber, "Comments");
        }
    }
}
