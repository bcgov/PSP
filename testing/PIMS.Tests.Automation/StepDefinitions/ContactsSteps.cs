using Microsoft.Extensions.Configuration;

namespace PIMS.Tests.Automation.StepDefinitions
{
    [Binding]
    public sealed class ContactsSteps
    {
        private readonly LoginSteps loginSteps;
        private readonly Contacts contacts;
        private readonly SearchContacts searchContacts;
        private readonly IEnumerable<IndividualContact> individualContacts;
        private readonly IEnumerable<OrganizationContact> organizationContacts;

        private readonly string userName = "TRANPSP1";

        private readonly string nonExistingContact = "A non existing contact";
        private readonly string legacyOrganizationName = "BC Hydro and Telus";
        private readonly string organization1Name = "Automation Test Corp";

        private readonly string organization2Name = "Automation Test Corp II";
        private readonly string organization2Alias = "AutoCorpII";
        private readonly string organization2CorpNbr = "BC78990";
        private readonly string organization2Email1 = "info@testcorp.ca";
        private readonly string organization2Phone1 = "(778) 323-0988";
        private readonly string organization2Email2 = "contact@testcorp.ca";
        private readonly string organization2Phone2 = "(778) 323-0989";

        private readonly string comments = "Automated Test for Contacts";

        public ContactsSteps(BrowserDriver driver)
        {
            loginSteps = new LoginSteps(driver);
            contacts = new Contacts(driver.Current);
            searchContacts = new SearchContacts(driver.Current);
            individualContacts = driver.Configuration.GetSection("IndividualContacts").Get<IEnumerable<IndividualContact>>();
            organizationContacts = driver.Configuration.GetSection("OrganizationContacts").Get<IEnumerable<OrganizationContact>>();
        }

        [StepDefinition(@"I create a new Individual Contact with minimum fields (.*)")]
        public void MinimumIndividualContact(string lastName)
        {
            /* TEST COVERAGE: PSP-2705, PSP-2797, PSP-4559 */

            //Login to PIMS
            loginSteps.Idir(userName);

            var contact = individualContacts.SingleOrDefault(u => u.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase));
            if (contact == null) throw new InvalidOperationException($"Contact {lastName} not found in the test configuration");

            //Navigate to Create new contact form
            contacts.NavigateToCreateNewContact();

            //Create new Individual Contact with minimum fields
            contacts.CreateIndividualContactMinFields(contact.FirstName, contact.LastName, contact.Organization, contact.Email, contact.MailCountry, contact.MailAddressLine1, contact.MailProvince, contact.MailProvince, contact.MailPostalCode);

            //Save Contact
            contacts.SaveContact();

        }

        [StepDefinition(@"I create a new Individual Contact with maximum fields (.*)")]
        public void MaximumIndividualContact(string lastName)
        {
            /* TEST COVERAGE: PSP-2705, PSP-2797, PSP-4559 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Create new contact form
            contacts.NavigateToCreateNewContact();

            var contact = individualContacts.SingleOrDefault(u => u.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase));
            if (contact == null) throw new InvalidOperationException($"Contact {lastName} not found in the test configuration");

            //Create new Individual Contact with maximum fields
            contacts.CreateIndividualContactMaxFields(contact.FirstName, contact.MiddleName, contact.LastName, contact.PreferableName, contact.Organization, contact.Email, contact.Phone,
                 contact.MailAddressLine1, contact.MailCountry, contact.MailProvince, contact.MailCity, contact.MailPostalCode,
                 contact.PropertyAddressLine1, contact.PropertyCountry, contact.PropertyProvince, contact.PropertyCity, contact.PropertyPostalCode,
                 contact.BillingAddressLine1, contact.BillingCountry, contact.BillingOtherCountry, contact.BillingCity, contact.BillingPostalCode, comments);

            //Save Contact
            contacts.SaveContact();

        }

        [StepDefinition(@"I create a new Organization Contact with minimum fields (.*)")]
        public void MinimumOrganizationContact(string organization)
        {
            /* TEST COVERAGE: PSP-4208, PSP-2797, PSP-4559 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Create new contact form
            contacts.NavigateToCreateNewContact();

            var contact = organizationContacts.SingleOrDefault(u => u.OrganizationName.Equals(organization, StringComparison.OrdinalIgnoreCase));
            if (contact == null) throw new InvalidOperationException($"Contact {organization} not found in the test configuration");

            //Create new Organization Contact with minimum fields
            contacts.CreateOrganizationContactMinFields(contact.OrganizationName, contact.Phone, contact.MailAddressLine1, contact.MailCountry, contact.MailProvince, contact.MailCity, contact.MailPostalCode);

            //Save Contact
            contacts.SaveContact();
        }

        [StepDefinition(@"I create a new Organization Contact with maximum fields (.*)")]
        public void MaximumOrganizationContact(string organization)
        {
            /* TEST COVERAGE: PSP-4208, PSP-2797, PSP-4559 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Create new contact form
            contacts.NavigateToCreateNewContact();

            var contact = organizationContacts.SingleOrDefault(u => u.OrganizationName.Equals(organization, StringComparison.OrdinalIgnoreCase));
            if (contact == null) throw new InvalidOperationException($"Contact {organization} not found in the test configuration");

            //Create new Organization Contact with maximum fields
            contacts.CreateOrganizationContactMaxFields(contact.OrganizationName, contact.Alias, contact.IncorporationNumber, contact.Email, contact.Phone,
                contact.MailAddressLine1, contact.MailCountry, contact.MailProvince, contact.MailCity, contact.MailPostalCode,
                contact.PropertyAddressLine1, contact.PropertyCountry, contact.PropertyProvince, contact.PropertyCity, contact.PropertyPostalCode,
                contact.BillingAddressLine1, contact.BillingCountry, contact.BillingOtherCountry, contact.BillingCity, contact.BillingPostalCode, comments);

            //Save Contact
            contacts.SaveContact();

        }

        [StepDefinition(@"I update an existing Organization Contact")]
        public void UpdateOrganizationContact()
        {
            /* TEST COVERAGE: PSP-3021, PSP-4200, PSP-4559 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Search a Contact
            searchContacts.NavigateToSearchContact();

            //Search for a contact
            searchContacts.SearchOrganizationContact(organization2Name);

            //Select the first option from search
            searchContacts.SelectFirstResultLink();

            //Update an Organization Contact
            contacts.UpdateContact(organization2Email2, organization2Phone2);

            //Save Contact
            contacts.SaveContact();

        }

        [StepDefinition(@"I search for an existing contact (.*)")]
        public void SearchExistingContact(string searchCriteria)
        {
            /* TEST COVERAGE: PSP-4200, PSP-4559, PSP-4559 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Search a Contact
            searchContacts.NavigateToSearchContact();

            //Search for an existing contact
            searchContacts.VerifySearchLinks(organization2Name);
        }

        [StepDefinition(@"I search for an non-existing contact")]
        public void SearchNonExistantContact()
        {
            /* TEST COVERAGE: PSP-4200, PSP-4559 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Search a Contact
            searchContacts.NavigateToSearchContact();

            //Search for a non-existing contact
            searchContacts.SearchGeneralContact(nonExistingContact);
        }

        [StepDefinition(@"I cancel creating a new contact")]
        public void CancelContactCreation()
        {
            /* TEST COVERAGE: PSP-2706, PSP-4559 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Search a Contact
            searchContacts.NavigateToSearchContact();

            //Click on Create new Contact button
            searchContacts.CreateNewContactFromSearch();

            var contact = individualContacts.SingleOrDefault(u => u.LastName.Equals("Doe", StringComparison.OrdinalIgnoreCase));
            if (contact == null) throw new InvalidOperationException($"Contact Doe not found in the test configuration");

            //Create new Individual Contact with minimum fields
            contacts.CreateIndividualContactMinFields(contact.FirstName, contact.LastName, contact.Organization, contact.Email, contact.MailCountry, contact.MailAddressLine1, contact.MailProvince, contact.MailProvince, contact.MailPostalCode);

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
        [StepDefinition(@"A new Organization contact is successfully created")]
        public void NewOrganizationContactCreated()
        {
            /* TEST COVERAGE: PSP-4208 */

            contacts.Wait();
            Assert.True(contacts.GetContactOrgStatus().Equals("ACTIVE"));
        }

        [StepDefinition(@"A new Individual contact is successfully created")]
        public void NewIndividualContactCreated()
        {
            /* TEST COVERAGE: PSP-2705 */

            contacts.Wait();
            Assert.True(contacts.GetContactIndStatus().Equals("ACTIVE"));
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

        [StepDefinition(@"Expected Content is displayed on Contacts Table (.*) (.*)")]
        public void VerifyContactsTableContent(string contactType, string searchCriteria)
        {
            /* TEST COVERAGE: PSP-2355 */

            if (contactType == "Individual")
            {
                var contact = individualContacts.SingleOrDefault(u => u.LastName.Equals(searchCriteria, StringComparison.OrdinalIgnoreCase));
                if (contact == null) throw new InvalidOperationException($"Contact {searchCriteria} not found in the test configuration");
                searchContacts.SearchIndividualContact(contact.Summary);
                searchContacts.VerifyContactTableContent(contact.Summary, contact.FirstName, contact.LastName, contact.Organization, contact.Email, contact.MailAddressLine1, contact.MailCity, contact.MailProvDisplay);
            }
            else
            {
                var contact = organizationContacts.SingleOrDefault(u => u.OrganizationName.Equals(searchCriteria, StringComparison.OrdinalIgnoreCase));
                if (contact == null) throw new InvalidOperationException($"Contact {searchCriteria} not found in the test configuration");
                searchContacts.SearchOrganizationContact(searchCriteria);
                searchContacts.VerifyContactTableContent(contact.OrganizationName, "", "", contact.OrganizationName, contact.Email, contact.MailAddressLine1, contact.MailCity, contact.MailProvDisplay);
            }
            
        }

        public class IndividualContact
        {
            public string Summary { get; set; } = null!;
            public string FirstName { get; set; } = null!;
            public string MiddleName { get; set; } = null!;
            public string LastName { get; set; } = null!;
            public string PreferableName { get; set; } = null!;
            public string Organization { get; set; } = null!;
            public string Email { get; set; } = null!;
            public string Phone { get; set; } = null!;
            public string MailAddressLine1 { get; set; } = null!;
            public string MailCountry { get; set; } = null!;
            public string MailProvince { get; set; } = null!;
            public string MailProvDisplay { get; set; } = null!; 
            public string MailCity { get; set; } = null!;
            public string MailPostalCode { get; set; } = null!;
            public string PropertyAddressLine1 { get; set; } = null!;
            public string PropertyCountry { get; set; } = null!;
            public string PropertyProvince { get; set; } = null!;
            public string PropertyCity { get; set; } = null!;
            public string PropertyPostalCode { get; set; } = null!;
            public string BillingAddressLine1 { get; set; } = null!;
            public string BillingCountry { get; set; } = null!;
            public string BillingOtherCountry { get; set; } = null!;
            public string BillingCity { get; set; } = null!;
            public string BillingPostalCode { get; set; } = null!;
        }

        public class OrganizationContact
        {
            public string OrganizationName { get; set; } = null!;
            public string Alias { get; set; } = null!;
            public string IncorporationNumber { get; set; } = null!;
            public string Email { get; set; } = null!;
            public string Phone { get; set; } = null!;
            public string MailAddressLine1 { get; set; } = null!;
            public string MailCountry { get; set; } = null!;
            public string MailProvince { get; set; } = null!;
            public string MailProvDisplay { get; set; } = null!;
            public string MailCity { get; set; } = null!;
            public string MailPostalCode { get; set; } = null!;
            public string PropertyAddressLine1 { get; set; } = null!;
            public string PropertyCountry { get; set; } = null!;
            public string PropertyProvince { get; set; } = null!;
            public string PropertyCity { get; set; } = null!;
            public string PropertyPostalCode { get; set; } = null!;
            public string BillingAddressLine1 { get; set; } = null!;
            public string BillingCountry { get; set; } = null!;
            public string BillingOtherCountry { get; set; } = null!;
            public string BillingCity { get; set; } = null!;
            public string BillingPostalCode { get; set; } = null!;
        }

    }
}
