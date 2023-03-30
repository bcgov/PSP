using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class Contacts : PageObjectBase
    {
        private By menuContactsButton = By.XPath("//a/label[contains(text(),'Contacts')]/parent::a");
        private By createContactButton = By.XPath("//a[contains(text(),'Add a Contact')]");

        private By contactIndividualRadioBttn = By.Id("contact-individual");
        private By contactOrganizationRadioBttn = By.Id("contact-organization");

        private By contactIndFirstNameInput = By.Id("input-firstName");
        private By contactIndMiddleNameInput = By.Id("input-middleNames");
        private By contactIndLastNameInput = By.Id("input-surname");
        private By contactIndPrefNameInput = By.Id("input-preferredName");
        private By contactIndOrgInput = By.Id("typeahead-organization");
        private By contactIndOrgListOptions = By.CssSelector("div[id='typeahead-organization']");
        private By contactOrgNameSelect = By.Id("typeahead-organization-item-0");

        private By contactOrgNameInput = By.Id("input-name");
        private By contactOrgAliasInput = By.Id("input-alias");
        private By contactOrgIncNbrInput = By.Id("input-incorporationNumber");

        private By contactEmailInput1 = By.Id("input-emailContactMethods.0.value");
        private By contactEmailSelect1 = By.Id("input-emailContactMethods.0.contactMethodTypeCode");
        private By contactEmailAddBttn = By.XPath("//div[contains(text(), '+ Add another email address')]");
        private By contactEmailInput2 = By.Id("input-emailContactMethods.1.value");
        private By contactEmailSelect2 = By.Id("input-emailContactMethods.1.contactMethodTypeCode");

        private By contactPhoneInput1 = By.Id("input-phoneContactMethods.0.value");
        private By contactPhoneSelect1 = By.Id("input-phoneContactMethods.0.contactMethodTypeCode");
        private By contactPhoneAddBttn = By.XPath("//div[contains(text(), '+ Add another phone number')]");
        private By contactPhoneInput2 = By.Id("input-phoneContactMethods.1.value");
        private By contactPhoneSelect2 = By.Id("input-phoneContactMethods.1.contactMethodTypeCode");

        private By contactMailAddressLine1Input = By.Id("input-mailingAddress.streetAddress1");
        private By contactMailCountrySelect = By.Id("input-mailingAddress.countryId");
        private By contactMailCityInput = By.Id("input-mailingAddress.municipality");
        private By contactMailProvinceSelect = By.Id("input-mailingAddress.provinceId");
        private By contactMailPostalCodeInput = By.Id("input-mailingAddress.postal");

        private By contactPropertyAddressLine1Input = By.Id("input-propertyAddress.streetAddress1");
        private By contactPropertyCountrySelect = By.Id("input-propertyAddress.countryId");
        private By contactPropertyCityInput = By.Id("input-propertyAddress.municipality");
        private By contactPropertyProvinceSelect = By.Id("input-propertyAddress.provinceId");
        private By contactPropertyPostalCodeInput = By.Id("input-propertyAddress.postal");

        private By contactBillingAddressLine1Input = By.Id("input-billingAddress.streetAddress1");
        private By contactBillingCountrySelect = By.Id("input-billingAddress.countryId");
        private By contactBillingCityInput = By.Id("input-billingAddress.municipality");
        private By contactBillingOtherCountryInput = By.Id("input-billingAddress.countryOther");
        private By contactBillingPostalCodeInput = By.Id("input-billingAddress.postal");

        private By contactCommentTextarea = By.CssSelector("textarea[name='comment']");

        private By contactEditButton = By.CssSelector("button[title='Edit Contact']");
        private By contactOrgStatusSpan = By.CssSelector("span[data-testid='contact-organization-status']");
        private By contactIndStatusSpan = By.CssSelector("span[data-testid='contact-person-status']");

        private By contactDuplicateModal = By.CssSelector("div[class='modal-dialog']");
        private By contactsSearchTable = By.CssSelector("div[data-testid='contactsTable']");

        private SharedModals sharedModals;

        public Contacts(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
        }

        //Navigates to Create a new Contact
        public void NavigateToCreateNewContact()
        {

            Wait();
            webDriver.FindElement(menuContactsButton).Click();

            Wait();
            webDriver.FindElement(createContactButton).Click();
        }

        //Creates Individual Contact with minimum fields
        public void CreateIndividualContactMinFields(string firstName, string lastName, string orgName, string email, string country, string addressLine1, string province, string city, string postalCode)
        {
            Wait();

            //Choosing individual contact option
            FocusAndClick(contactIndividualRadioBttn);

            //Inserting individual personal details
            webDriver.FindElement(contactIndFirstNameInput).SendKeys(firstName);
            webDriver.FindElement(contactIndLastNameInput).SendKeys(lastName);
            webDriver.FindElement(contactIndOrgInput).SendKeys(orgName);

            Wait();
            webDriver.FindElement(contactOrgNameSelect).Click();

            //Inserting individual contact info
            webDriver.FindElement(contactEmailInput1).SendKeys(email);
            ChooseRandomSelectOption(contactEmailSelect1, 2);

            //Inserting contact mail address
            webDriver.FindElement(contactMailAddressLine1Input).SendKeys(addressLine1);

            var countryElement = webDriver.FindElement(contactMailCountrySelect);
            ChooseSpecificSelectOption(contactMailCountrySelect, country);

            webDriver.FindElement(contactMailCityInput).SendKeys(city);

            var provinceElement = webDriver.FindElement(contactMailProvinceSelect);
            ChooseSpecificSelectOption(contactMailProvinceSelect, province);

            webDriver.FindElement(contactMailPostalCodeInput).SendKeys(postalCode);
        }

        //Creates Individual Contact with all fields
        public void CreateIndividualContactMaxFields(string firstName, string middleName, string lastName, string prefName, string orgName, string email, string phone,
            string mailAddressLine1, string mailCountry, string mailProvince, string mailCity, string mailPostalCode,
            string propertyAddressLine1, string propertyCountry, string propertyProvince, string propertyCity, string propertyPostalCode,
            string billingAddressLine1, string billingCountry, string specifyCountry, string billingCity, string billingPostalCode, string comments)
        {
            Wait();

            //Choosing individual contact option
            FocusAndClick(contactIndividualRadioBttn);

            //Inserting individual personal details
            webDriver.FindElement(contactIndFirstNameInput).SendKeys(firstName);
            webDriver.FindElement(contactIndMiddleNameInput).SendKeys(middleName);
            webDriver.FindElement(contactIndLastNameInput).SendKeys(lastName);
            webDriver.FindElement(contactIndPrefNameInput).SendKeys(prefName);
            webDriver.FindElement(contactIndOrgInput).SendKeys(orgName);

            WaitUntil(contactIndOrgListOptions);
            Wait();
            webDriver.FindElement(contactOrgNameSelect).Click();

            //Inserting individual contact info
            webDriver.FindElement(contactEmailInput1).SendKeys(email);
            ChooseRandomSelectOption(contactEmailSelect1, 2);

            webDriver.FindElement(contactPhoneInput1).SendKeys(phone);
            ChooseRandomSelectOption(contactPhoneSelect1, 2);

            //Inserting contact mail address
            webDriver.FindElement(contactMailAddressLine1Input).SendKeys(mailAddressLine1);
            ChooseSpecificSelectOption(contactMailCountrySelect, mailCountry);
            webDriver.FindElement(contactMailCityInput).SendKeys(mailCity);
            ChooseSpecificSelectOption(contactMailProvinceSelect, mailProvince);
            webDriver.FindElement(contactMailPostalCodeInput).SendKeys(mailPostalCode);

            //Inserting contact property address
            webDriver.FindElement(contactPropertyAddressLine1Input).SendKeys(propertyAddressLine1);
            ChooseSpecificSelectOption(contactPropertyCountrySelect, propertyCountry);
            webDriver.FindElement(contactPropertyCityInput).SendKeys(propertyCity);
            ChooseSpecificSelectOption(contactPropertyProvinceSelect, propertyProvince);
            webDriver.FindElement(contactPropertyPostalCodeInput).SendKeys(propertyPostalCode);

            //Inserting contact billing address
            webDriver.FindElement(contactBillingAddressLine1Input).SendKeys(billingAddressLine1);

            ChooseSpecificSelectOption(contactBillingCountrySelect, billingCountry);

            webDriver.FindElement(contactBillingCityInput).SendKeys(billingCity);
            webDriver.FindElement(contactBillingOtherCountryInput).SendKeys(specifyCountry);
            webDriver.FindElement(contactBillingPostalCodeInput).SendKeys(billingPostalCode);

            //Inserting comments
            webDriver.FindElement(contactCommentTextarea).SendKeys(comments);
        }

        public void CreateOrganizationContactMinFields(string orgName, string phone, string addressLine1, string country, string province, string city, string postalCode)
        {
            Wait();

            //Choosing organization contact option
            FocusAndClick(contactOrganizationRadioBttn);

            //Inserting organization contact details
            webDriver.FindElement(contactOrgNameInput).SendKeys(orgName);

            //Inserting organization contact info
            webDriver.FindElement(contactPhoneInput1).SendKeys(phone);
            ChooseRandomSelectOption(contactPhoneSelect1, 2);

            //Inserting contact property address
            webDriver.FindElement(contactPropertyAddressLine1Input).SendKeys(addressLine1);

            ChooseSpecificSelectOption(contactPropertyCountrySelect, country);

            webDriver.FindElement(contactPropertyCityInput).SendKeys(city);

            ChooseSpecificSelectOption(contactPropertyProvinceSelect, province);

            webDriver.FindElement(contactPropertyPostalCodeInput).SendKeys(postalCode);
        }

        //Creates Organization Contact with all fields
        public void CreateOrganizationContactMaxFields(string orgName, string alias, string incNumber, string email, string phone,
            string mailAddressLine1, string mailCountry, string mailProvince, string mailCity, string mailPostalCode,
            string propertyAddressLine1, string propertyCountry, string propertyProvince, string propertyCity, string propertyPostalCode,
            string billingAddressLine1, string billingCountry, string billingSpecifyCountry, string billingCity, string billingPostalCode, string comments)
        {
            Wait();

            //Choosing organization contact option
            FocusAndClick(contactOrganizationRadioBttn);

            //Inserting individual personal details
            webDriver.FindElement(contactOrgNameInput).SendKeys(orgName);
            webDriver.FindElement(contactOrgAliasInput).SendKeys(alias);
            webDriver.FindElement(contactOrgIncNbrInput).SendKeys(incNumber);

            //Inserting individual contact info
            webDriver.FindElement(contactEmailInput1).SendKeys(email);
            ChooseRandomSelectOption(contactEmailSelect1, 2);

            webDriver.FindElement(contactPhoneInput1).SendKeys(phone);
            ChooseRandomSelectOption(contactPhoneSelect1, 2);

            //Inserting contact mail address
            webDriver.FindElement(contactMailAddressLine1Input).SendKeys(mailAddressLine1);
            ChooseSpecificSelectOption(contactMailCountrySelect, mailCountry);
            webDriver.FindElement(contactMailCityInput).SendKeys(mailCity);
            ChooseSpecificSelectOption(contactMailProvinceSelect, mailProvince);
            webDriver.FindElement(contactMailPostalCodeInput).SendKeys(mailPostalCode);

            //Inserting property mail address
            webDriver.FindElement(contactPropertyAddressLine1Input).SendKeys(propertyAddressLine1);
            ChooseSpecificSelectOption(contactPropertyCountrySelect, propertyCountry);
            webDriver.FindElement(contactPropertyCityInput).SendKeys(propertyCity);
            ChooseSpecificSelectOption(contactPropertyProvinceSelect, propertyProvince);
            webDriver.FindElement(contactPropertyPostalCodeInput).SendKeys(propertyPostalCode);

            //Inserting billing mail address
            webDriver.FindElement(contactBillingAddressLine1Input).SendKeys(billingAddressLine1);
            ChooseSpecificSelectOption(contactBillingCountrySelect, billingCountry);

            webDriver.FindElement(contactBillingCityInput).SendKeys(billingCity);
            webDriver.FindElement(contactBillingOtherCountryInput).SendKeys(billingSpecifyCountry);
            webDriver.FindElement(contactBillingPostalCodeInput).SendKeys(billingPostalCode);

            //Inserting comments
            webDriver.FindElement(contactCommentTextarea).SendKeys(comments);
        }

        //Update Contact
        public void UpdateContact(string email, string phone)
        {
            Wait();

            webDriver.FindElement(contactEditButton).Click();

            Wait();
            FocusAndClick(contactEmailAddBttn);
            ClearInput(contactEmailInput2);
            webDriver.FindElement(contactEmailInput2).SendKeys(email);
            ChooseRandomSelectOption(contactEmailSelect2, 2);

            FocusAndClick(contactPhoneAddBttn);
            ClearInput(contactPhoneInput2);
            webDriver.FindElement(contactPhoneInput2).SendKeys(phone);
            ChooseRandomSelectOption(contactPhoneSelect2, 2);

        }

        //Saves Contact
        public void SaveContact()
        {
            Wait();

            //Save
            ButtonElement("Save");

            Wait();
            if (webDriver.FindElements(contactDuplicateModal).Count > 0)
            {
                Assert.True(sharedModals.ModalHeader().Equals("Duplicate Contact"));
                Assert.True(sharedModals.ModalContent().Equals("A contact matching this information already exists in the system."));
                ButtonElement("Continue Save");
            }

            Wait();
            var editButtonElement = webDriver.FindElement(contactEditButton);
            Assert.True(editButtonElement.Displayed);

        }

        //Cancel Contact
        public void CancelContact()
        {
            Wait();

            ButtonElement("Cancel");

            Wait();
            ButtonElement("Confirm");

            Wait();
            var contactTableElement = webDriver.FindElement(contactsSearchTable);
            Assert.True(contactTableElement.Displayed);
        }

        // ASSERT FUNCTIONS
        public string GetContactOrgStatus()
        {
            Wait();
            return webDriver.FindElement(contactOrgStatusSpan).Text;
        }

        public string GetContactIndStatus()
        {
            Wait();
            return webDriver.FindElement(contactIndStatusSpan).Text;
        }

    }
}
