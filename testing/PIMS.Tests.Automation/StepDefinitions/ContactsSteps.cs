using System.Data;
using PIMS.Tests.Automation.Data;
using PIMS.Tests.Automation.Classes;
using PIMS.Tests.Automation.PageObjects;

namespace PIMS.Tests.Automation.StepDefinitions
{
    [Binding]
    public class ContactsSteps
    {
        private readonly LoginSteps loginSteps;
        private readonly Contacts contacts;
        private readonly SearchContacts searchContacts;
        private readonly SharedPagination sharedPagination;

        private readonly string userName = "TRANPSP1";

        private IndividualContact individualContact;
        private OrganizationContact organizationContact;

        public ContactsSteps(BrowserDriver driver)
        {
            loginSteps = new LoginSteps(driver);
            contacts = new Contacts(driver.Current);
            searchContacts = new SearchContacts(driver.Current);
            sharedPagination = new SharedPagination(driver.Current);

            individualContact = new IndividualContact();
            organizationContact = new OrganizationContact();
        }

        [StepDefinition(@"I create a new Individual Contact from row number (.*)")]
        public void IndividualContact(int rowNumber)
        {
            /* TEST COVERAGE: PSP-2705, PSP-2797, PSP-4559, PSP-2706, PSP-4559 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Create new contact form
            contacts.NavigateToCreateNewContact();

            PopulateIndividualContact(rowNumber);

            //Create new Individual Contact
            contacts.CreateIndividualContact(individualContact);

            //Cancel Contact
            contacts.CancelContact();

            //Go to the create new contact form
            contacts.CreateNewContactBttn();

            //Create new Individual Contact
            contacts.CreateIndividualContact(individualContact);

            //Save Contact
            contacts.SaveContact();

            //Verify Individual Contact Details View
            contacts.VerifyIndividualContactView(individualContact);
        }

        [StepDefinition(@"I create a new Organization Contact from row number (.*)")]
        public void OrganizationContact(int rowNumber)
        {
            /* TEST COVERAGE: PSP-4208, PSP-2797, PSP-4559, PSP-2706, PSP-4559 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Create new contact form
            contacts.NavigateToCreateNewContact();

            //Populate the organization Object with selected data
            PopulateOrganizationContact(rowNumber);

            //Create new Organization Contact
            contacts.CreateOrganizationContact(organizationContact);

            //Cancel Contact
            contacts.CancelContact();

            //Go to the create new contact form
            contacts.CreateNewContactBttn();

            //Create new Organization Contact
            contacts.CreateOrganizationContact(organizationContact);

            //Save Contact
            contacts.SaveContact();

            //Verify Organization Contact View Form
            contacts.VerifyOrganizationContactView(organizationContact);
        }

        [StepDefinition(@"I update an existing Organization Contact from row number (.*)")]
        public void UpdateOrganizationContact(int rowNumber)
        {
            /* TEST COVERAGE: PSP-3021, PSP-4200, PSP-4559, PSP-4208 */

            //Update an Organization Contact
            PopulateOrganizationContact(rowNumber);
            contacts.UpdateOrganizationContact(organizationContact);

            //Save Contact
            contacts.SaveContact();

            //Verify updated contact
            contacts.VerifyOrganizationContactView(organizationContact);
        }

        [StepDefinition(@"I update an existing Individual Contact from row number (.*)")]
        public void UpdateIndividualContact(int rowNumber)
        {
            /* TEST COVERAGE: PSP-4207, PSP-4200, PSP-4559, PSP-2705 */

            //Update an Organization Contact
            PopulateIndividualContact(rowNumber);
            contacts.UpdateIndividualContact(individualContact);

            //Save Contact
            contacts.SaveContact();

            //Verify updated contact
            contacts.VerifyIndividualContactView(individualContact);
        }

        [StepDefinition(@"I search for an existing contact from type ""(.*)"" row number (.*)")]
        public void SearchExistingContact(string contactType, int rowNumber)
        {
            /* TEST COVERAGE: PSP-4200, PSP-4559, PSP-4559 */

            //Navigate to Search a Contact
            searchContacts.NavigateToSearchContact();

            if (contactType == "Individual")
            {
                PopulateIndividualContact(rowNumber);
                searchContacts.FilterContacts("Individual", individualContact.FullName, "");
            }
            else
            {
                PopulateOrganizationContact(rowNumber);
                searchContacts.FilterContacts("Organization", organizationContact.OrganizationName, "");
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
                searchContacts.FilterContacts("Individual",individualContact.FullName, "");
            }
            else
            {
                PopulateOrganizationContact(rowNumber);
                searchContacts.FilterContacts("Organization",organizationContact.OrganizationName, "");
            }
        }

        [StepDefinition(@"I verify the Contacts List View from row number (.*)")]
        public void VerifyContactsListView(int rowNumber)
        {
            /* TEST COVERAGE: PSP-2355, PSP-4559 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Populate contacts data
            PopulateOrganizationContact(rowNumber);

            //Navigate to Search a Contact
            searchContacts.NavigateToSearchContact();

            //Verify List View
            searchContacts.VerifyContactsListView();

            //Verify Pagination
            sharedPagination.ChoosePaginationOption(5);
            Assert.Equal(5, searchContacts.ContactsTableResultNumber());

            sharedPagination.ChoosePaginationOption(10);
            Assert.Equal(10, searchContacts.ContactsTableResultNumber());

            sharedPagination.ChoosePaginationOption(20);
            Assert.Equal(20, searchContacts.ContactsTableResultNumber());

            sharedPagination.ChoosePaginationOption(50);
            Assert.Equal(50, searchContacts.ContactsTableResultNumber());

            sharedPagination.ChoosePaginationOption(100);
            Assert.Equal(100, searchContacts.ContactsTableResultNumber());

            //Verify Column Sorting by Contact Summary
            searchContacts.OrderByContactSummary();
            var firstSummaryDescResult = searchContacts.FirstContactSummary();

            searchContacts.OrderByContactSummary();
            var firstSummaryAscResult = searchContacts.FirstContactSummary();

            Assert.NotEqual(firstSummaryDescResult, firstSummaryAscResult);

            //Verify Column Sorting by First Name
            searchContacts.OrderByContactFirstName();
            var firstNameDescResult = searchContacts.FirstContactFirstName();

            searchContacts.OrderByContactFirstName();
            var firstNameAscResult = searchContacts.FirstContactFirstName();

            Assert.NotEqual(firstNameDescResult, firstNameAscResult);

            //Verify Column Sorting by Last Name
            searchContacts.OrderByContactLastName();
            var firstLastNameDescResult = searchContacts.FirstContactLastName();

            searchContacts.OrderByContactLastName();
            var firstLastNameAscResult = searchContacts.FirstContactLastName();

            Assert.NotEqual(firstLastNameDescResult, firstLastNameAscResult);

            //Verify Column Sorting by Organization
            searchContacts.OrderByContactOrganization();
            var firstOrganizationDescResult = searchContacts.FirstContactOrganization();

            searchContacts.OrderByContactOrganization();
            var firstOrganizationAscResult = searchContacts.FirstContactOrganization();

            Assert.NotEqual(firstOrganizationDescResult, firstOrganizationAscResult);

            //Verify Column Sorting by City
            searchContacts.OrderByContactCity();
            var firstCityDescResult = searchContacts.FirstContactCity();

            searchContacts.OrderByContactCity();
            var firstCityAscResult = searchContacts.FirstContactCity();

            Assert.NotEqual(firstCityDescResult, firstCityAscResult);

            //Verify Pagination display different set of results
            sharedPagination.ResetSearch();

            var firstContactPage1 = searchContacts.FirstContactSummary();
            sharedPagination.GoNextPage();
            var firstContactPage2 = searchContacts.FirstContactSummary();
            Assert.NotEqual(firstContactPage1, firstContactPage2);

            sharedPagination.ResetSearch();

            //Filter Acquisition Files
            searchContacts.FilterContacts("Individual", "Maria Johnson", "California");
            Assert.False(searchContacts.SearchFoundResults());

            //Look for the last created Acquisition File
            searchContacts.FilterContacts("Organization", organizationContact.OrganizationName, organizationContact.OrgMailAddress.City);
        }

        [StepDefinition(@"No contacts results are found")]
        public void NoContactResults()
        {
            /* TEST COVERAGE: PSP-4200 */
            Assert.Equal("No Contacts match the search criteria", searchContacts.GetNoSearchMessage());
            searchContacts.Dispose();
        }

        [StepDefinition(@"Expected Content is displayed on Contacts Table from contact type ""(.*)""")]
        public void VerifyContactsTableContent(string contactType)
        {
            /* TEST COVERAGE: PSP-2355 */

            if (contactType == "Individual")
                searchContacts.VerifyContactTableContent(individualContact.FullName, individualContact.FirstName, individualContact.LastName, individualContact.Organization, individualContact.IndEmail1, individualContact.IndMailAddress.AddressLine1, individualContact.IndMailAddress.City, individualContact.IndMailAddress.ProvinceView, individualContact.IndMailAddress.Country);
            else
                searchContacts.VerifyContactTableContent(organizationContact.OrganizationName, "", "", organizationContact.OrganizationName, organizationContact.OrgEmail1, organizationContact.OrgMailAddress.AddressLine1, organizationContact.OrgMailAddress.City, organizationContact.OrgMailAddress.ProvinceView, organizationContact.OrgMailAddress.Country);

            searchContacts.Dispose();
        }

        private void PopulateIndividualContact(int rowNumber)
        {
            DataTable individualContactSheet = ExcelDataContext.GetInstance().Sheets["IndividualContacts"]!;
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

            individualContact.IndMailAddress.AddressLine1 = ExcelDataContext.ReadData(rowNumber, "IndMailAddressLine1");
            individualContact.IndMailAddress.AddressLine2 = ExcelDataContext.ReadData(rowNumber, "IndMailAddressLine2");
            individualContact.IndMailAddress.AddressLine3 = ExcelDataContext.ReadData(rowNumber, "IndMailAddressLine3");
            individualContact.IndMailAddress.City = ExcelDataContext.ReadData(rowNumber, "IndMailCity");
            individualContact.IndMailAddress.Province = ExcelDataContext.ReadData(rowNumber, "IndMailProvince");
            individualContact.IndMailAddress.ProvinceView = ExcelDataContext.ReadData(rowNumber, "IndMailProvDisplay");
            individualContact.IndMailAddress.CityProvinceView = ExcelDataContext.ReadData(rowNumber, "IndMailCityProvinceView");
            individualContact.IndMailAddress.Country = ExcelDataContext.ReadData(rowNumber, "IndMailCountry");
            individualContact.IndMailAddress.OtherCountry = ExcelDataContext.ReadData(rowNumber, "IndMailOtherCountry");
            individualContact.IndMailAddress.PostalCode = ExcelDataContext.ReadData(rowNumber, "IndMailPostalCode");

            individualContact.IndPropertyAddress.AddressLine1 = ExcelDataContext.ReadData(rowNumber, "IndPropertyAddressLine1");
            individualContact.IndPropertyAddress.AddressLine2 = ExcelDataContext.ReadData(rowNumber, "IndPropertyAddressLine2");
            individualContact.IndPropertyAddress.AddressLine3 = ExcelDataContext.ReadData(rowNumber, "IndPropertyAddressLine3");
            individualContact.IndPropertyAddress.City = ExcelDataContext.ReadData(rowNumber, "IndPropertyCity");
            individualContact.IndPropertyAddress.Province = ExcelDataContext.ReadData(rowNumber, "IndPropertyProvince");
            individualContact.IndPropertyAddress.CityProvinceView = ExcelDataContext.ReadData(rowNumber, "IndPropertyCityProvinceView");
            individualContact.IndPropertyAddress.Country = ExcelDataContext.ReadData(rowNumber, "IndPropertyCountry");
            individualContact.IndPropertyAddress.OtherCountry = ExcelDataContext.ReadData(rowNumber, "IndPropertyOtherCountry");
            individualContact.IndPropertyAddress.PostalCode = ExcelDataContext.ReadData(rowNumber, "IndPropertyPostalCode");

            individualContact.IndBillingAddress.AddressLine1 = ExcelDataContext.ReadData(rowNumber, "IndBillingAddressLine1");
            individualContact.IndBillingAddress.AddressLine2 = ExcelDataContext.ReadData(rowNumber, "IndBillingAddressLine2");
            individualContact.IndBillingAddress.AddressLine3 = ExcelDataContext.ReadData(rowNumber, "IndBillingAddressLine3");
            individualContact.IndBillingAddress.City = ExcelDataContext.ReadData(rowNumber, "IndBillingCity");
            individualContact.IndBillingAddress.Province = ExcelDataContext.ReadData(rowNumber, "IndBillingProvince");
            individualContact.IndBillingAddress.CityProvinceView = ExcelDataContext.ReadData(rowNumber, "IndBillingCityProvinceView");
            individualContact.IndBillingAddress.Country = ExcelDataContext.ReadData(rowNumber, "IndBillingCountry");
            individualContact.IndBillingAddress.OtherCountry = ExcelDataContext.ReadData(rowNumber, "IndBillingOtherCountry");
            individualContact.IndBillingAddress.PostalCode = ExcelDataContext.ReadData(rowNumber, "IndBillingPostalCode");

            individualContact.IndComments = ExcelDataContext.ReadData(rowNumber, "IndComments");
        }

        private void PopulateOrganizationContact(int rowNumber)
        {
            DataTable organizationContactSheet = ExcelDataContext.GetInstance().Sheets["OrganizationContacts"]!;
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

            organizationContact.OrgMailAddress.AddressLine1 = ExcelDataContext.ReadData(rowNumber, "OrgMailAddressLine1");
            organizationContact.OrgMailAddress.AddressLine2 = ExcelDataContext.ReadData(rowNumber, "OrgMailAddressLine2");
            organizationContact.OrgMailAddress.AddressLine3 = ExcelDataContext.ReadData(rowNumber, "OrgMailAddressLine3");
            organizationContact.OrgMailAddress.City = ExcelDataContext.ReadData(rowNumber, "OrgMailCity");
            organizationContact.OrgMailAddress.Province = ExcelDataContext.ReadData(rowNumber, "OrgMailProvince");
            organizationContact.OrgMailAddress.ProvinceView = ExcelDataContext.ReadData(rowNumber, "OrgMailProvDisplay");
            organizationContact.OrgMailAddress.CityProvinceView = ExcelDataContext.ReadData(rowNumber, "OrgMailCityProvinceView");
            organizationContact.OrgMailAddress.Country = ExcelDataContext.ReadData(rowNumber, "OrgMailCountry");
            organizationContact.OrgMailAddress.OtherCountry = ExcelDataContext.ReadData(rowNumber, "OrgMailOtherCountry");
            organizationContact.OrgMailAddress.PostalCode = ExcelDataContext.ReadData(rowNumber, "OrgMailPostalCode");

            organizationContact.OrgPropertyAddress.AddressLine1 = ExcelDataContext.ReadData(rowNumber, "OrgPropertyAddressLine1");
            organizationContact.OrgPropertyAddress.AddressLine2 = ExcelDataContext.ReadData(rowNumber, "OrgPropertyAddressLine2");
            organizationContact.OrgPropertyAddress.AddressLine3 = ExcelDataContext.ReadData(rowNumber, "OrgPropertyAddressLine3");
            organizationContact.OrgPropertyAddress.City = ExcelDataContext.ReadData(rowNumber, "OrgPropertyCity");
            organizationContact.OrgPropertyAddress.Province = ExcelDataContext.ReadData(rowNumber, "OrgPropertyProvince");
            organizationContact.OrgPropertyAddress.CityProvinceView = ExcelDataContext.ReadData(rowNumber, "OrgPropertyCityProvinceView");
            organizationContact.OrgPropertyAddress.Country = ExcelDataContext.ReadData(rowNumber, "OrgPropertyCountry");
            organizationContact.OrgPropertyAddress.OtherCountry = ExcelDataContext.ReadData(rowNumber, "OrgPropertyOtherCountry");
            organizationContact.OrgPropertyAddress.PostalCode = ExcelDataContext.ReadData(rowNumber, "OrgPropertyPostalCode");

            organizationContact.OrgBillingAddress.AddressLine1 = ExcelDataContext.ReadData(rowNumber, "OrgBillingAddressLine1");
            organizationContact.OrgBillingAddress.AddressLine2 = ExcelDataContext.ReadData(rowNumber, "OrgBillingAddressLine2");
            organizationContact.OrgBillingAddress.AddressLine3 = ExcelDataContext.ReadData(rowNumber, "OrgBillingAddressLine3");
            organizationContact.OrgBillingAddress.City = ExcelDataContext.ReadData(rowNumber, "OrgBillingCity");
            organizationContact.OrgBillingAddress.Province = ExcelDataContext.ReadData(rowNumber, "OrgBillingProvince");
            organizationContact.OrgBillingAddress.CityProvinceView = ExcelDataContext.ReadData(rowNumber, "OrgBillingCityProvinceView");
            organizationContact.OrgBillingAddress.Country = ExcelDataContext.ReadData(rowNumber, "OrgBillingCountry");
            organizationContact.OrgBillingAddress.OtherCountry = ExcelDataContext.ReadData(rowNumber, "OrgBillingOtherCountry");
            organizationContact.OrgBillingAddress.PostalCode = ExcelDataContext.ReadData(rowNumber, "OrgBillingPostalCode");

            organizationContact.OrgComments = ExcelDataContext.ReadData(rowNumber, "OrgComments");
        }
    }
}
