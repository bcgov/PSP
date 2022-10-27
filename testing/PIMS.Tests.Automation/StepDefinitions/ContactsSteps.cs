using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMS.Tests.Automation.StepDefinitions
{
    [Binding]
    public sealed class ContactsSteps
    {
        private readonly LoginSteps loginSteps;
        private readonly Contacts contacts;
        private readonly SearchContacts searchContacts;

        private readonly string userName = "TRANPSP1";

        private readonly string nonExistingContact = "A non existing contact";
        private readonly string legacyOrganizationName = "Bishop of Victoria";
        private readonly string organization1Name = "Automation Test Corp";

        private readonly string organization2Name = "Automation Test Corp II";
        private readonly string organization2Alias = "AutoCorpII";
        private readonly string organization2CorpNbr = "BC78990";
        private readonly string organization2Email1 = "info@testcorp.ca";
        private readonly string organization2Phone1 = "(778) 323-0988";
        private readonly string organization2Email2 = "contact@testcorp.ca";
        private readonly string organization2Phone2 = "(778) 323-0989";


        private readonly string contact1FirstName = "John";
        private readonly string contact1LastName = "Doe";
        private readonly string contact1Email = "john.doe@test.ca";
        private readonly string contact1Country = "Canada";
        private readonly string contact1AddressLine1 = "1234 Douglas St.";
        private readonly string contact1City = "Victoria";
        private readonly string contact1Province = "British Columbia";
        private readonly string contact1PostalCode = "V7B 1F6";

        private readonly string contact2FirstName = "Anne";
        private readonly string contact2MiddleName = "Elizabeth";
        private readonly string contact2LastName = "Lee";
        private readonly string contact2PrefName = "Anne Lee";
        private readonly string contact2Email = "anne.lee@test.ca";
        private readonly string contact2Phone = "(236) 323-0988";
        private readonly string contact2MailAddressLine1 = "123-5667 Main St.";
        private readonly string contact2MailCountry = "United States of America";
        private readonly string contact2MailState = "Washington";
        private readonly string contact2MailCity = "Bellevue";
        private readonly string contact2MailPostalCode = "98004";
        private readonly string contact2PropertyAddressLine1 = "Av. Fray Antonio Alcalde 10, Zona Centro";
        private readonly string contact2PropertyCountry = "Mexico";
        private readonly string contact2PropertyProvince = "Mexico";
        private readonly string contact2PropertyCity = "Guadalajara";
        private readonly string contact2PropertyPostalCode = "44100";
        private readonly string contact2BillingAddressLine1 = "136 Exford St.";
        private readonly string contact2BillingCountry = "Other";
        private readonly string contact2BillingOtherCountry = "Australia";
        private readonly string contact2BillingCity = "Brisbane";
        private readonly string contact2BillingPostalCode = "4000";

        private readonly string comments = "Automated Test for Contacts";

        public ContactsSteps(BrowserDriver driver)
        {
            loginSteps = new LoginSteps(driver);
            contacts = new Contacts(driver.Current);
            searchContacts = new SearchContacts(driver.Current);
        }

        [StepDefinition(@"I create a new Individual Contact with minimum fields")]
        public void MinimumIndividualContact()
        {
            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Create new contact form
            contacts.NavigateToCreateNewContact();

            //Create new Individual Contact with minimum fields
            contacts.CreateIndividualContactMinFields(contact1FirstName, contact1LastName, legacyOrganizationName, contact1Email, contact1Country, contact1AddressLine1, contact1Province, contact1City, contact1PostalCode);

            //Save Contact
            contacts.SaveContact();

        }

        [StepDefinition(@"I create a new Individual Contact with maximum fields")]
        public void MaximumIndividualContact()
        {
            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Create new contact form
            contacts.NavigateToCreateNewContact();

            //Create new Individual Contact with maximum fields
            contacts.CreateIndividualContactMaxFields(contact2FirstName, contact2MiddleName, contact2LastName, contact2PrefName, legacyOrganizationName, contact2Email, contact2Phone,
                 contact2MailAddressLine1, contact2MailCountry, contact2MailState, contact2MailCity, contact2MailPostalCode,
                 contact2PropertyAddressLine1, contact2PropertyCountry, contact2PropertyProvince, contact2PropertyCity, contact2PropertyPostalCode,
                 contact2BillingAddressLine1, contact2BillingCountry, contact2BillingOtherCountry, contact2BillingCity, contact2BillingPostalCode, comments);

            //Save Contact
            contacts.SaveContact();

        }

        [StepDefinition(@"I create a new Organization Contact with minimum fields")]
        public void MinimumOrganizationContact()
        {
            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Create new contact form
            contacts.NavigateToCreateNewContact();

            //Create new Organization Contact with minimum fields
            contacts.CreateOrganizationContactMinFields(organization1Name, contact2Phone, contact1AddressLine1, contact1Country, contact1Province, contact1City, contact1PostalCode);

            //Save Contact
            contacts.SaveContact();
        }

        [StepDefinition(@"I create a new Organization Contact with maximum fields")]
        public void MaximumOrganizationContact()
        {
            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Create new contact form
            contacts.NavigateToCreateNewContact();

            //Create new Organization Contact with maximum fields
            contacts.CreateOrganizationContactMaxFields(organization2Name, organization2Alias, organization2CorpNbr, organization2Email1, organization2Phone1,
                contact2MailAddressLine1, contact2MailCountry, contact2MailState, contact2MailCity, contact2MailPostalCode,
                contact2PropertyAddressLine1, contact2PropertyCountry, contact2PropertyProvince, contact2PropertyCity, contact2PropertyPostalCode,
                contact2BillingAddressLine1, contact2BillingCountry, contact2BillingOtherCountry, contact2BillingCity, contact2BillingPostalCode, comments);

            //Save Contact
            contacts.SaveContact();

        }

        [StepDefinition(@"I update an existing Organization Contact")]
        public void UpdateOrganizationContact()
        {
            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Search a Contact
            searchContacts.NavigateToSearchContact();

            //Search for a contact
            searchContacts.SearchOrganizationContact(organization2Name);

            //Update an Organization Contact
            contacts.UpdateContact(organization2Email2, organization2Phone2);

            //Save Contact
            contacts.SaveContact();

        }

        [StepDefinition(@"I search for an existing contact")]
        public void SearchExistingContact()
        {
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
            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Search a Contact
            searchContacts.NavigateToSearchContact();

            //Click on Create new Contact button
            searchContacts.CreateNewContactFromSearch();

            //Create new Individual Contact with minimum fields
            contacts.CreateIndividualContactMinFields(contact1FirstName, contact1LastName, legacyOrganizationName, contact1Email, contact1Country, contact1AddressLine1, contact1Province, contact1City, contact1PostalCode);

            //Cancel Contact
            contacts.CancelContact();
        }

        //ASSERT FUNCTIONS
        [StepDefinition(@"A new Organization contact is successfully created")]
        public void NewOrganizationContactCreated()
        {
            contacts.Wait();
            Assert.True(contacts.GetContactOrgStatus().Equals("ACTIVE"));
        }

        [StepDefinition(@"A new Individual contact is successfully created")]
        public void NewIndividualContactCreated()
        {
            contacts.Wait();
            Assert.True(contacts.GetContactIndStatus().Equals("ACTIVE"));
        }

        [StepDefinition(@"Search Contacts screen is correctly rendered")]
        public void CorrectSearchContact()
        {
            contacts.Wait();
            Assert.True(searchContacts.SearchContactRender());
        }

        [StepDefinition(@"No contacts results are found")]
        public void NoContactResults()
        {
            contacts.Wait();
            Assert.True(searchContacts.GetNoSearchMessage().Equals("No Contacts match the search criteria"));
        }

    }
}
