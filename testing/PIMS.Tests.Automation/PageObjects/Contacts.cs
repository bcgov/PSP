using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class Contacts : PageObjectBase
    {
        //Contact Menu Elements
        private By menuContactsButton = By.CssSelector("div[data-testid='nav-tooltip-contacts'] a");
        private By createContactButton = By.XPath("//a[contains(text(),'Add a Contact')]");

        //Contacts Create Elements
        private By contactIndividualRadioBttn = By.Id("contact-individual");
        private By contactOrganizationRadioBttn = By.Id("contact-organization");

        private By contactIndFirstNameInput = By.Id("input-firstName");
        private By contactIndMiddleNameInput = By.Id("input-middleNames");
        private By contactIndLastNameInput = By.Id("input-surname");
        private By contactIndPrefNameInput = By.CssSelector("input[id='input-preferredName']");
        private By contactIndOrgInput = By.Id("typeahead-organization");
        private By contactIndOrgListOptions = By.CssSelector("div[id='typeahead-organization']");
        private By contactOrgName1stOption = By.CssSelector("div[id='typeahead-organization'] a:first-child");

        private By contactOrgNameInput = By.Id("input-name");
        private By contactOrgAliasInput = By.Id("input-alias");
        private By contactOrgIncNbrInput = By.Id("input-incorporationNumber");

        private By contactEmailInput1 = By.Id("input-emailContactMethods.0.value");
        private By contactEmailSelect1 = By.Id("input-emailContactMethods.0.contactMethodTypeCode");
        private By contactEmailAddBttn = By.XPath("//div[contains(text(), '+ Add email address')]");
        private By contactEmailInput2 = By.Id("input-emailContactMethods.1.value");
        private By contactEmailSelect2 = By.Id("input-emailContactMethods.1.contactMethodTypeCode");

        private By contactPhoneInput1 = By.Id("input-phoneContactMethods.0.value");
        private By contactPhoneSelect1 = By.Id("input-phoneContactMethods.0.contactMethodTypeCode");
        private By contactPhoneAddBttn = By.XPath("//div[contains(text(), '+ Add phone number')]");
        private By contactPhoneInput2 = By.Id("input-phoneContactMethods.1.value");
        private By contactPhoneSelect2 = By.Id("input-phoneContactMethods.1.contactMethodTypeCode");

        private By contactMailAddressLine1Input = By.Id("input-mailingAddress.streetAddress1");
        private By contactMailAddressLine2Input = By.Id("input-mailingAddress.streetAddress2");
        private By contactMailAddressLine3Input = By.Id("input-mailingAddress.streetAddress3");
        private By contactMailCountrySelect = By.Id("input-mailingAddress.countryId");
        private By contactMailOtherCountryInput = By.Id("input-mailingAddress.countryOther");
        private By contactMailCityInput = By.Id("input-mailingAddress.municipality");
        private By contactMailProvinceSelect = By.Id("input-mailingAddress.provinceId");
        private By contactMailPostalCodeInput = By.Id("input-mailingAddress.postal");
        private By contactIndMailAddAddressLineBttn = By.XPath("//span[contains(text(),'Mailing Address')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div/div/div/div/button/div[contains(text(),'+ Add an address line')]");
        private By contactOrgMailAddAddressLineBttn = By.XPath("//div[contains(text(),'Mailing Address')]/parent::div/parent::h2/following-sibling::div/div/div/div/div/button/div[contains(text(),'+ Add an address line')]");


        private By contactPropertyAddressLine1Input = By.Id("input-propertyAddress.streetAddress1");
        private By contactPropertyAddressLine2Input = By.Id("input-propertyAddress.streetAddress2");
        private By contactPropertyAddressLine3Input = By.Id("input-propertyAddress.streetAddress3");
        private By contactPropertyCountrySelect = By.Id("input-propertyAddress.countryId");
        private By contactPropertyOtherCountryInput = By.Id("input-propertyAddress.countryOther");
        private By contactPropertyCityInput = By.Id("input-propertyAddress.municipality");
        private By contactPropertyProvinceSelect = By.Id("input-propertyAddress.provinceId");
        private By contactPropertyPostalCodeInput = By.Id("input-propertyAddress.postal");
        //private By contactIndPropertyAddAddressLineBttn = By.XPath("//span[contains(text(),'Property Address')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div/div/div/div/button/div[contains(text(),'+ Add an address line')]");
        private By contactPropertyAddAddressLineBttn = By.XPath("//div[contains(text(),'Property Address')]/parent::div/parent::h2/following-sibling::div/div/div/div/div/button/div[contains(text(),'+ Add an address line')]");


        private By contactBillingAddressLine1Input = By.Id("input-billingAddress.streetAddress1");
        private By contactBillingAddressLine2Input = By.Id("input-billingAddress.streetAddress2");
        private By contactBillingAddressLine3Input = By.Id("input-billingAddress.streetAddress3");
        private By contactBillingCountrySelect = By.Id("input-billingAddress.countryId");
        private By contactBillingCityInput = By.Id("input-billingAddress.municipality");
        private By contactBillingOtherCountryInput = By.Id("input-billingAddress.countryOther");
        private By contactBillingProvinceSelect = By.Id("input-billingAddress.provinceId");
        private By contactBillingPostalCodeInput = By.Id("input-billingAddress.postal");
        //private By contactIndBillingAddAddressLineBttn = By.XPath("//span[contains(text(),'Billing Address')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div/div/div/div/button/div[contains(text(),'+ Add an address line')]");
        private By contactBillingAddAddressLineBttn = By.XPath("//div[contains(text(),'Billing Address')]/parent::div/parent::h2/following-sibling::div/div/div/div/div/button/div[contains(text(),'+ Add an address line')]");


        private By contactCommentTextarea = By.CssSelector("textarea[name='comment']");

        //Contacts Form View Elements
        private By contactTitle = By.XPath("//h1/div/div[contains(text(),'Contact')]");
        private By contactEditButton = By.CssSelector("button[title='Edit Contact']");
        private By contactDetailsSubtitle = By.XPath("//div[contains(text(),'Contact Details')]");

        private By contactIndStatusSpan = By.CssSelector("span[data-testid='contact-person-status']");
        private By contactIndFullName = By.CssSelector("div[data-testid='contact-person-fullname'] b");
        private By contactIndPrefNameLabel = By.XPath("//label[contains(text(),'Preferred name')]");
        private By contactIndPrefNameContent = By.CssSelector("div[data-testid='contact-person-preferred']");
        private By contactIndLinkedOrgsLabel = By.XPath("//label[contains(text(),'Linked organization')]");
        private By contactIndOrganizationContent = By.CssSelector("[data-testid='contact-person-organization'] a");
        private By contactIndContactInfoSubtitle = By.XPath("//h3[contains(text(),'Contact Info')]");

        private By contactOrgStatusSpan = By.CssSelector("span[data-testid='contact-organization-status']");
        private By contactOrgName = By.CssSelector("div[data-testid='contact-organization-organizationName'] b");
        private By contactOrgAliasLabel = By.XPath("//label[contains(text(),'Alias')]");
        private By contactOrgAliasContent = By.CssSelector("div[data-testid='contact-organization-alias']");
        private By contactOrgIncorpNbrLabel = By.XPath("//label[contains(text(),'Incorporation number')]");
        private By contactOrgIncorpNbrContent = By.CssSelector("div[data-testid='contact-organization-incorporationNumber']");
        private By contactOrgPrefContactSubtitle = By.XPath("//h3[contains(text(),'Preferred Contact')]");

        private By contactEmailLabel = By.XPath("//strong[contains(text(),'Email')]");
        private By contactEmail1Content = By.XPath("(//div[@data-testid='email-value'])[1]");
        private By contactEmailType1Content = By.XPath("(//div[@data-testid='email-value']/parent::div)[1]/div/em");
        private By contactEmail2Content = By.XPath("(//div[@data-testid='email-value'])[2]");
        private By contactEmailType2Content = By.XPath("(//div[@data-testid='email-value']/parent::div)[2]/div/em");
        private By contactPhoneLabel = By.XPath("//strong[contains(text(),'Phone')]");
        private By contactPhone1Content = By.XPath("(//div[@data-testid='phone-value'])[1]");
        private By contactPhoneType1Content = By.XPath("(//div[@data-testid='phone-value'])[1]/following-sibling::div/em");
        private By contactPhone2Content = By.XPath("(//div[@data-testid='phone-value'])[2]");
        private By contactPhoneType2Content = By.XPath("(//div[@data-testid='phone-value'])[2]/following-sibling::div/em");

        private By contactAddressSubtitle = By.XPath("//h2[contains(text(),'Address')]");
        private By contactAddressMailSubtitle = By.XPath("//h3[contains(text(),'Mailing address')]");
        private By contactAddressIndMailCounter = By.XPath("//div[1]/div/span[@data-testid='contact-person-address']/div");
        private By contactAddressIndMailAddressLine1 = By.XPath("//div[1]/div/span[@data-testid='contact-person-address']/div[1]");
        private By contactAddressIndMailAddressLine2 = By.XPath("//div[1]/div/span[@data-testid='contact-person-address']/div[2]");
        private By contactAddressIndMailAddressLine3 = By.XPath("//div[1]/div/span[@data-testid='contact-person-address']/div[3]");
        private By contactAddressIndMailAddressLine4 = By.XPath("//div[1]/div/span[@data-testid='contact-person-address']/div[4]");
        private By contactAddressIndMailAddressLine5 = By.XPath("//div[1]/div/span[@data-testid='contact-person-address']/div[5]");
        private By contactAddressIndMailAddressLine6 = By.XPath("//div[1]/div/span[@data-testid='contact-person-address']/div[6]");
        private By contactAddressMailAddressRemoveBttn = By.XPath("//h3[contains(text(),'Mailing Address')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div/div/div/div/button/div/span[contains(text(),'Remove')]/parent::div/parent::button");

        private By contactAddressOrgMailCounter = By.XPath("//div[1]/div[@data-testid='contact-organization-address']/div");
        private By contactAddressOrgMailAddressLine1 = By.XPath("//div[1]/div[@data-testid='contact-organization-address']/div[1]");
        private By contactAddressOrgMailAddressLine2 = By.XPath("//div[1]/div[@data-testid='contact-organization-address']/div[2]");
        private By contactAddressOrgMailAddressLine3 = By.XPath("//div[1]/div[@data-testid='contact-organization-address']/div[3]");
        private By contactAddressOrgMailAddressLine4 = By.XPath("//div[1]/div[@data-testid='contact-organization-address']/div[4]");
        private By contactAddressOrgMailAddressLine5 = By.XPath("//div[1]/div[@data-testid='contact-organization-address']/div[5]");
        private By contactAddressOrgMailAddressLine6 = By.XPath("//div[1]/div[@data-testid='contact-organization-address']/div[6]");
        private By contactAddressPropertyAddressRemoveBttn = By.XPath("//div[contains(text(),'Property Address')]/parent::div/parent::h2/following-sibling::div/div/div/div/div/button/div/span[contains(text(),'Remove')]/parent::div/parent::button");

        private By contactAddressPropertySubtitle = By.XPath("//h3[contains(text(),'Property address')]");
        private By contactAddressIndPropertyCounter = By.XPath("//div[2]/div/span[@data-testid='contact-person-address']/div");
        private By contactAddressIndPropertyAddressLine1 = By.XPath("//div[2]/div/span[@data-testid='contact-person-address']/div[1]");
        private By contactAddressIndPropertyAddressLine2 = By.XPath("//div[2]/div/span[@data-testid='contact-person-address']/div[2]");
        private By contactAddressIndPropertyAddressLine3 = By.XPath("//div[2]/div/span[@data-testid='contact-person-address']/div[3]");
        private By contactAddressIndPropertyAddressLine4 = By.XPath("//div[2]/div/span[@data-testid='contact-person-address']/div[4]");
        private By contactAddressIndPropertyAddressLine5 = By.XPath("//div[2]/div/span[@data-testid='contact-person-address']/div[5]");
        private By contactAddressIndPropertyAddressLine6 = By.XPath("//div[2]/div/span[@data-testid='contact-person-address']/div[6]");
        private By contactAddressBillingAddressRemoveBttn = By.XPath("//div[contains(text(),'Billing Address')]/parent::div/parent::h2/following-sibling::div/div/div/div/div/button/div/span[contains(text(),'Remove')]/parent::div/parent::button");

        private By contactAddressOrgPropertyCounter = By.XPath("//div[2]/div[@data-testid='contact-organization-address']/div");
        private By contactAddressOrgPropertyAddressLine1 = By.XPath("//div[2]/div[@data-testid='contact-organization-address']/div[1]");
        private By contactAddressOrgPropertyAddressLine2 = By.XPath("//div[2]/div[@data-testid='contact-organization-address']/div[2]");
        private By contactAddressOrgPropertyAddressLine3 = By.XPath("//div[2]/div[@data-testid='contact-organization-address']/div[3]");
        private By contactAddressOrgPropertyAddressLine4 = By.XPath("//div[2]/div[@data-testid='contact-organization-address']/div[4]");
        private By contactAddressOrgPropertyAddressLine5 = By.XPath("//div[2]/div[@data-testid='contact-organization-address']/div[5]");
        private By contactAddressOrgPropertyAddressLine6 = By.XPath("//div[2]/div[@data-testid='contact-organization-address']/div[6]");

        private By contactAddressBillingSubtitle = By.XPath("//h3[contains(text(),'Billing address')]");
        private By contactAddressIndBillingCounter = By.XPath("//div[3]/div/span[@data-testid='contact-person-address']/div");
        private By contactAddressIndBillingAddressLine1 = By.XPath("//div[3]/div/span[@data-testid='contact-person-address']/div[1]");
        private By contactAddressIndBillingAddressLine2 = By.XPath("//div[3]/div/span[@data-testid='contact-person-address']/div[2]");
        private By contactAddressIndBillingAddressLine3 = By.XPath("//div[3]/div/span[@data-testid='contact-person-address']/div[3]");
        private By contactAddressIndBillingAddressLine4 = By.XPath("//div[3]/div/span[@data-testid='contact-person-address']/div[4]");
        private By contactAddressIndBillingAddressLine5 = By.XPath("//div[3]/div/span[@data-testid='contact-person-address']/div[5]");
        private By contactAddressIndBillingAddressLine6 = By.XPath("//div[3]/div/span[@data-testid='contact-person-address']/div[6]");

        private By contactAddressOrgBillingCounter = By.XPath("//div[3]/div[@data-testid='contact-organization-address']/div");
        private By contactAddressOrgBillingAddressLine1 = By.XPath("//div[3]/div[@data-testid='contact-organization-address']/div[1]");
        private By contactAddressOrgBillingAddressLine2 = By.XPath("//div[3]/div[@data-testid='contact-organization-address']/div[2]");
        private By contactAddressOrgBillingAddressLine3 = By.XPath("//div[3]/div[@data-testid='contact-organization-address']/div[3]");
        private By contactAddressOrgBillingAddressLine4 = By.XPath("//div[3]/div[@data-testid='contact-organization-address']/div[4]");
        private By contactAddressOrgBillingAddressLine5 = By.XPath("//div[3]/div[@data-testid='contact-organization-address']/div[5]");
        private By contactAddressOrgBillingAddressLine6 = By.XPath("//div[3]/div[@data-testid='contact-organization-address']/div[6]");

        private By contactOrgIndividualContactsSubtitle = By.XPath("//h2[contains(text(),'Individual Contacts')]");
        private By contactOrgIndividualContactsCount = By.CssSelector("a[data-testid='contact-organization-person']");

        private By commentsSubtitle = By.XPath("//h2[contains(text(),'Comments')]");
        private By commentsIndividualContent = By.CssSelector("div[data-testid='contact-person-comment']");
        private By commentsOrganizationContent = By.CssSelector("div[data-testid='contact-organization-comment']");

        //Contact Modal Element
        private By contactModal = By.CssSelector("div[class='modal-content']");

        private By contactsSearchTable = By.CssSelector("div[data-testid='contactsTable']");
        private By contactModalContinueSaveBttn = By.XPath("//button/div[contains(text(),'Continue Save')]");

        private SharedModals sharedModals;

        public Contacts(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
        }

        //Navigates to Create a new Contact
        public void NavigateToCreateNewContact()
        {
            Wait();

            WaitUntilClickable(menuContactsButton);
            FocusAndClick(menuContactsButton);

            WaitUntilClickable(createContactButton);
            FocusAndClick(createContactButton);
        }

        //Create new Contact Button
        public void CreateNewContactBttn()
        {
            WaitUntilClickable(createContactButton);
            FocusAndClick(createContactButton);
        }

        //Creates Individual Contact with all fields
        public void CreateIndividualContact(IndividualContact contact)
        {
            WaitUntilClickable(contactIndividualRadioBttn);

            //Choosing individual contact option
            FocusAndClick(contactIndividualRadioBttn);

            //Inserting individual personal details
            webDriver.FindElement(contactIndFirstNameInput).SendKeys(contact.FirstName);
            webDriver.FindElement(contactIndMiddleNameInput).SendKeys(contact.MiddleName);
            webDriver.FindElement(contactIndLastNameInput).SendKeys(contact.LastName);
            webDriver.FindElement(contactIndPrefNameInput).SendKeys(contact.PreferableName);
            if (contact.Organization != "")
            {
                webDriver.FindElement(contactIndOrgInput).SendKeys(contact.Organization);
                webDriver.FindElement(contactIndOrgInput).SendKeys(Keys.Space);
                webDriver.FindElement(contactIndOrgInput).SendKeys(Keys.Backspace);

                Wait(); 
                webDriver.FindElement(contactOrgName1stOption).Click();
            }
            //Inserting contact info
            if (contact.IndEmail1 != "" && contact.IndEmailType1 != "")
            {
                webDriver.FindElement(contactEmailInput1).SendKeys(contact.IndEmail1);
                ChooseSpecificSelectOption(contactEmailSelect1, contact.IndEmailType1);
            }
            if (contact.IndEmail2 != "")
            {
                webDriver.FindElement(contactEmailAddBttn).Click();
                webDriver.FindElement(contactEmailInput2).SendKeys(contact.IndEmail2);
                ChooseSpecificSelectOption(contactEmailSelect2, contact.IndEmailType2);
            }
            if (contact.IndPhone1 != "")
            {
                webDriver.FindElement(contactPhoneInput1).SendKeys(contact.IndPhone1);
                ChooseSpecificSelectOption(contactPhoneSelect1, contact.IndPhoneType1);
            }
            if (contact.IndPhone2 != "")
            {
                webDriver.FindElement(contactPhoneAddBttn).Click();
                webDriver.FindElement(contactPhoneInput2).SendKeys(contact.IndPhone2);
                ChooseSpecificSelectOption(contactPhoneSelect2, contact.IndPhoneType2);
            }

            //Inserting contact mail address
            if (contact.IndMailAddress.AddressLine1 != "")
            {
                webDriver.FindElement(contactMailAddressLine1Input).SendKeys(contact.IndMailAddress.AddressLine1);
                if (contact.IndMailAddress.AddressLine2 != "")
                {
                    webDriver.FindElement(contactIndMailAddAddressLineBttn).Click();
                    webDriver.FindElement(contactMailAddressLine2Input).SendKeys(contact.IndMailAddress.AddressLine2);
                }
                if (contact.IndMailAddress.AddressLine3 != "")
                {
                    webDriver.FindElement(contactIndMailAddAddressLineBttn).Click();
                    webDriver.FindElement(contactMailAddressLine3Input).SendKeys(contact.IndMailAddress.AddressLine3);
                }

                ChooseSpecificSelectOption(contactMailCountrySelect, contact.IndMailAddress.Country);
                if (contact.IndMailAddress.Country == "Other")
                {
                    webDriver.FindElement(contactMailOtherCountryInput).SendKeys(contact.IndMailAddress.OtherCountry);
                }
                else
                {
                    ChooseSpecificSelectOption(contactMailProvinceSelect, contact.IndMailAddress.Province);
                }
                webDriver.FindElement(contactMailCityInput).SendKeys(contact.IndMailAddress.City);
                webDriver.FindElement(contactMailPostalCodeInput).SendKeys(contact.IndMailAddress.PostalCode);
            }

            //Inserting contact property address
            if (contact.IndPropertyAddress.AddressLine1 != "")
            {
                webDriver.FindElement(contactPropertyAddressLine1Input).SendKeys(contact.IndPropertyAddress.AddressLine1);
                if (contact.IndPropertyAddress.AddressLine2 != "")
                {
                    webDriver.FindElement(contactPropertyAddAddressLineBttn).Click();
                    webDriver.FindElement(contactPropertyAddressLine2Input).SendKeys(contact.IndPropertyAddress.AddressLine2);
                }
                if (contact.IndPropertyAddress.AddressLine3 != "")
                {
                    webDriver.FindElement(contactPropertyAddAddressLineBttn).Click();
                    webDriver.FindElement(contactPropertyAddressLine3Input).SendKeys(contact.IndPropertyAddress.AddressLine3);
                }

                ChooseSpecificSelectOption(contactPropertyCountrySelect, contact.IndPropertyAddress.Country);
                if (contact.IndPropertyAddress.Country == "Other")
                {
                    webDriver.FindElement(contactPropertyOtherCountryInput).SendKeys(contact.IndPropertyAddress.OtherCountry);
                }
                else
                {
                    ChooseSpecificSelectOption(contactPropertyProvinceSelect, contact.IndPropertyAddress.Province);
                }
                webDriver.FindElement(contactPropertyCityInput).SendKeys(contact.IndPropertyAddress.City);
                webDriver.FindElement(contactPropertyPostalCodeInput).SendKeys(contact.IndPropertyAddress.PostalCode);
            }

            //Inserting contact billing address
            if (contact.IndBillingAddress.AddressLine1 != "")
            {
                webDriver.FindElement(contactBillingAddressLine1Input).SendKeys(contact.IndBillingAddress.AddressLine1);
                if (contact.IndBillingAddress.AddressLine2 != "")
                {
                    webDriver.FindElement(contactBillingAddAddressLineBttn).Click();
                    webDriver.FindElement(contactBillingAddressLine2Input).SendKeys(contact.IndBillingAddress.AddressLine2);
                }
                if (contact.IndBillingAddress.AddressLine3 != "")
                {
                    webDriver.FindElement(contactBillingAddAddressLineBttn).Click();
                    webDriver.FindElement(contactBillingAddressLine3Input).SendKeys(contact.IndBillingAddress.AddressLine3);
                }
                ChooseSpecificSelectOption(contactBillingCountrySelect, contact.IndBillingAddress.Country);
                if (contact.IndBillingAddress.Country == "Other")
                {
                    webDriver.FindElement(contactBillingOtherCountryInput).SendKeys(contact.IndBillingAddress.OtherCountry);
                }
                else
                {
                    ChooseSpecificSelectOption(contactBillingProvinceSelect, contact.IndBillingAddress.Province);
                }
                webDriver.FindElement(contactBillingCityInput).SendKeys(contact.IndBillingAddress.City);
                webDriver.FindElement(contactBillingPostalCodeInput).SendKeys(contact.IndBillingAddress.PostalCode);
            }

            //Inserting comments
            webDriver.FindElement(contactCommentTextarea).SendKeys(contact.IndComments);
        }

        //Creates Organization Contact with all fields
        public void CreateOrganizationContact(OrganizationContact contact)
        {
            Wait();

            //Choosing organization contact option
            FocusAndClick(contactOrganizationRadioBttn);

            //Inserting individual personal details
            webDriver.FindElement(contactOrgNameInput).SendKeys(contact.OrganizationName);
            if (contact.Alias != "")
            {
                webDriver.FindElement(contactOrgAliasInput).SendKeys(contact.Alias);
            }
            if(contact.IncorporationNumber != "")
            {
                webDriver.FindElement(contactOrgIncNbrInput).SendKeys(contact.IncorporationNumber);
            }

            //Inserting contact info
            if (contact.OrgEmail1 != "")
            {
                webDriver.FindElement(contactEmailInput1).SendKeys(contact.OrgEmail1);
                ChooseSpecificSelectOption(contactEmailSelect1, contact.OrgEmailType1);
            }
            if (contact.OrgEmail2 != "")
            {
                webDriver.FindElement(contactEmailAddBttn).Click();
                webDriver.FindElement(contactEmailInput2).SendKeys(contact.OrgEmail2);
                ChooseSpecificSelectOption(contactEmailSelect2, contact.OrgEmailType2);
            }
            if (contact.OrgPhone1 != "")
            {
                webDriver.FindElement(contactPhoneInput1).SendKeys(contact.OrgPhone1);
                ChooseSpecificSelectOption(contactPhoneSelect1, contact.OrgPhoneType1);
            }
            if (contact.OrgPhone2 != "")
            {
                webDriver.FindElement(contactPhoneAddBttn).Click();
                webDriver.FindElement(contactPhoneInput2).SendKeys(contact.OrgPhone2);
                ChooseSpecificSelectOption(contactPhoneSelect2, contact.OrgPhoneType2);
            }

            //Inserting contact mail address
            if (contact.OrgMailAddress.AddressLine1 != "")
            {
                webDriver.FindElement(contactMailAddressLine1Input).SendKeys(contact.OrgMailAddress.AddressLine1);
                if (contact.OrgMailAddress.AddressLine2 != "")
                {
                    webDriver.FindElement(contactOrgMailAddAddressLineBttn).Click();
                    webDriver.FindElement(contactMailAddressLine2Input).SendKeys(contact.OrgMailAddress.AddressLine2);
                }
                if (contact.OrgMailAddress.AddressLine3 != "")
                {
                    webDriver.FindElement(contactOrgMailAddAddressLineBttn).Click();
                    webDriver.FindElement(contactMailAddressLine3Input).SendKeys(contact.OrgMailAddress.AddressLine3);
                }

                ChooseSpecificSelectOption(contactMailCountrySelect, contact.OrgMailAddress.Country);
                if (contact.OrgMailAddress.Country == "Other")
                {
                    webDriver.FindElement(contactMailOtherCountryInput).SendKeys(contact.OrgMailAddress.OtherCountry);
                }
                else
                {
                    ChooseSpecificSelectOption(contactMailProvinceSelect, contact.OrgMailAddress.Province);
                }
                webDriver.FindElement(contactMailCityInput).SendKeys(contact.OrgMailAddress.City);
                webDriver.FindElement(contactMailPostalCodeInput).SendKeys(contact.OrgMailAddress.PostalCode);
            }

            //Inserting contact property address
            if (contact.OrgPropertyAddress.AddressLine1 != "")
            {
                webDriver.FindElement(contactPropertyAddressLine1Input).SendKeys(contact.OrgPropertyAddress.AddressLine1);
                if (contact.OrgPropertyAddress.AddressLine2 != "")
                {
                    webDriver.FindElement(contactPropertyAddAddressLineBttn).Click();
                    webDriver.FindElement(contactPropertyAddressLine2Input).SendKeys(contact.OrgPropertyAddress.AddressLine2);
                }
                if (contact.OrgPropertyAddress.AddressLine3 != "")
                {
                    webDriver.FindElement(contactPropertyAddAddressLineBttn).Click();
                    webDriver.FindElement(contactPropertyAddressLine3Input).SendKeys(contact.OrgPropertyAddress.AddressLine3);
                }

                ChooseSpecificSelectOption(contactPropertyCountrySelect, contact.OrgPropertyAddress.Country);
                if (contact.OrgPropertyAddress.Country == "Other")
                {
                    webDriver.FindElement(contactPropertyOtherCountryInput).SendKeys(contact.OrgPropertyAddress.Country);
                }
                else
                {
                    ChooseSpecificSelectOption(contactPropertyProvinceSelect, contact.OrgPropertyAddress.Province);
                }
                webDriver.FindElement(contactPropertyCityInput).SendKeys(contact.OrgPropertyAddress.City);
                webDriver.FindElement(contactPropertyPostalCodeInput).SendKeys(contact.OrgPropertyAddress.PostalCode);
            }

            //Inserting contact billing address
            if (contact.OrgBillingAddress.AddressLine1 != "")
            {
                webDriver.FindElement(contactBillingAddressLine1Input).SendKeys(contact.OrgBillingAddress.AddressLine1);
                if (contact.OrgBillingAddress.AddressLine2 != "")
                {
                    webDriver.FindElement(contactBillingAddAddressLineBttn).Click();
                    webDriver.FindElement(contactBillingAddressLine2Input).SendKeys(contact.OrgBillingAddress.AddressLine2);
                }
                if (contact.OrgBillingAddress.AddressLine3 != "")
                {
                    webDriver.FindElement(contactBillingAddAddressLineBttn).Click();
                    webDriver.FindElement(contactBillingAddressLine3Input).SendKeys(contact.OrgBillingAddress.AddressLine3);
                }
                ChooseSpecificSelectOption(contactBillingCountrySelect, contact.OrgBillingAddress.Country);
                if (contact.OrgBillingAddress.Country == "Other")
                {
                    webDriver.FindElement(contactBillingOtherCountryInput).SendKeys(contact.OrgBillingAddress.OtherCountry);
                }
                else
                {
                    ChooseSpecificSelectOption(contactBillingProvinceSelect, contact.OrgBillingAddress.Province);
                }
                webDriver.FindElement(contactBillingCityInput).SendKeys(contact.OrgBillingAddress.City);
                webDriver.FindElement(contactBillingPostalCodeInput).SendKeys(contact.OrgBillingAddress.PostalCode);
            }

            //Inserting comments
            webDriver.FindElement(contactCommentTextarea).SendKeys(contact.OrgComments);
        }

        //Update Organization Contact
        public void UpdateOrganizationContact(OrganizationContact contact)
        {
            WaitUntilClickable(contactEditButton);
            webDriver.FindElement(contactEditButton).Click();

            //Updating organization details
            if (contact.Alias != "")
            {
                WaitUntilClickable(contactOrgAliasInput);
                ClearInput(contactOrgAliasInput);
                webDriver.FindElement(contactOrgAliasInput).SendKeys(contact.Alias);
            }
            if (contact.IncorporationNumber != "")
            {
                WaitUntilClickable(contactOrgIncNbrInput);
                ClearInput(contactOrgIncNbrInput);
                webDriver.FindElement(contactOrgIncNbrInput).SendKeys(contact.IncorporationNumber);
            }

            //Updating contact info
            if (contact.OrgEmail1 != "")
            {
                WaitUntilClickable(contactEmailInput1);
                ClearInput(contactEmailInput1);
                webDriver.FindElement(contactEmailInput1).SendKeys(contact.OrgEmail1);
                ChooseSpecificSelectOption(contactEmailSelect1, contact.OrgEmailType1);
            }
            if (contact.OrgEmail2 != "")
            {
                WaitUntilClickable(contactEmailInput2);
                ClearInput(contactEmailInput2);
                webDriver.FindElement(contactEmailInput2).SendKeys(contact.OrgEmail2);
                ChooseSpecificSelectOption(contactEmailSelect2, contact.OrgEmailType2);
            }
            if (contact.OrgPhone1 != "")
            {
                WaitUntilClickable(contactPhoneInput1);
                ClearInput(contactPhoneInput1);
                webDriver.FindElement(contactPhoneInput1).SendKeys(contact.OrgPhone1);
                ChooseSpecificSelectOption(contactPhoneSelect1, contact.OrgPhoneType1);
            }
            if (contact.OrgPhone2 != "")
            {
                WaitUntilClickable(contactPhoneInput2);
                ClearInput(contactPhoneInput2);
                webDriver.FindElement(contactPhoneInput2).SendKeys(contact.OrgPhone2);
                ChooseSpecificSelectOption(contactPhoneSelect2, contact.OrgPhoneType2);
            }

            //Updating contact mail address
            if (contact.OrgMailAddress.AddressLine1 != "")
            {
                while (webDriver.FindElements(contactAddressMailAddressRemoveBttn).Count > 0)
                {
                    FocusAndClick(contactAddressMailAddressRemoveBttn);
                }
                ClearInput(contactMailAddressLine1Input);

                webDriver.FindElement(contactMailAddressLine1Input).SendKeys(contact.OrgMailAddress.AddressLine1);

                if (contact.OrgMailAddress.AddressLine2 != "")
                {
                    webDriver.FindElement(contactIndMailAddAddressLineBttn).Click();
                    webDriver.FindElement(contactMailAddressLine2Input).SendKeys(contact.OrgMailAddress.AddressLine2);
                }
                if (contact.OrgMailAddress.AddressLine3 != "")
                {
                    webDriver.FindElement(contactIndMailAddAddressLineBttn).Click();
                    webDriver.FindElement(contactMailAddressLine3Input).SendKeys(contact.OrgMailAddress.AddressLine3);
                }
                if(contact.OrgMailAddress.Country != "")
                    ChooseSpecificSelectOption(contactMailCountrySelect, contact.OrgMailAddress.Country);
                if (contact.OrgMailAddress.Country == "Other")
                {
                    ClearInput(contactMailOtherCountryInput);
                    webDriver.FindElement(contactMailOtherCountryInput).SendKeys(contact.OrgMailAddress.OtherCountry);
                }
                if(contact.OrgMailAddress.Province != "")
                {
                    ChooseSpecificSelectOption(contactMailProvinceSelect, contact.OrgMailAddress.Province);
                }
                if (contact.OrgMailAddress.City != "")
                {
                    ClearInput(contactMailCityInput);
                    webDriver.FindElement(contactMailCityInput).SendKeys(contact.OrgMailAddress.City);
                }
                if (contact.OrgMailAddress.PostalCode != "")
                {
                    ClearInput(contactMailPostalCodeInput);
                    webDriver.FindElement(contactMailPostalCodeInput).SendKeys(contact.OrgMailAddress.PostalCode);
                }
            }

            //Updating contact property address
            if (contact.OrgPropertyAddress.AddressLine1 != "")
            {
                while (webDriver.FindElements(contactAddressPropertyAddressRemoveBttn).Count > 0)
                {
                    FocusAndClick(contactAddressPropertyAddressRemoveBttn);
                }
                ClearInput(contactPropertyAddressLine1Input);

                webDriver.FindElement(contactPropertyAddressLine1Input).SendKeys(contact.OrgPropertyAddress.AddressLine1);
                if (contact.OrgPropertyAddress.AddressLine2 != "")
                {
                    webDriver.FindElement(contactPropertyAddAddressLineBttn).Click();
                    webDriver.FindElement(contactPropertyAddressLine2Input).SendKeys(contact.OrgPropertyAddress.AddressLine2);
                }
                if (contact.OrgPropertyAddress.AddressLine3 != "")
                {
                    webDriver.FindElement(contactPropertyAddAddressLineBttn).Click();
                    webDriver.FindElement(contactPropertyAddressLine3Input).SendKeys(contact.OrgPropertyAddress.AddressLine3);
                }
                if(contact.OrgPropertyAddress.Country != "")
                    ChooseSpecificSelectOption(contactPropertyCountrySelect, contact.OrgPropertyAddress.Country);
                if (contact.OrgPropertyAddress.Country == "Other")
                {
                    ClearInput(contactPropertyOtherCountryInput);
                    webDriver.FindElement(contactPropertyOtherCountryInput).SendKeys(contact.OrgPropertyAddress.OtherCountry);
                }
                if(contact.OrgPropertyAddress.Province != "")
                    ChooseSpecificSelectOption(contactPropertyProvinceSelect, contact.OrgPropertyAddress.Province);

                if (contact.OrgPropertyAddress.City != "")
                {
                    ClearInput(contactPropertyCityInput);
                    webDriver.FindElement(contactPropertyCityInput).SendKeys(contact.OrgPropertyAddress.City);
                }
                if (contact.OrgPropertyAddress.PostalCode != "")
                {
                    ClearInput(contactPropertyPostalCodeInput);
                    webDriver.FindElement(contactPropertyPostalCodeInput).SendKeys(contact.OrgPropertyAddress.PostalCode);
                }
            }

            //Updating contact billing address
            if (contact.OrgBillingAddress.AddressLine1 != "")
            {
                while (webDriver.FindElements(contactAddressBillingAddressRemoveBttn).Count > 0)
                {
                    FocusAndClick(contactAddressBillingAddressRemoveBttn);
                }
                ClearInput(contactBillingAddressLine1Input);

                webDriver.FindElement(contactBillingAddressLine1Input).SendKeys(contact.OrgBillingAddress.AddressLine1);
                if (contact.OrgBillingAddress.AddressLine2 != "")
                {
                    webDriver.FindElement(contactBillingAddAddressLineBttn).Click();
                    webDriver.FindElement(contactBillingAddressLine2Input).SendKeys(contact.OrgBillingAddress.AddressLine2);
                }
                if (contact.OrgBillingAddress.AddressLine3 != "")
                {
                    webDriver.FindElement(contactBillingAddAddressLineBttn).Click();
                    webDriver.FindElement(contactBillingAddressLine3Input).SendKeys(contact.OrgBillingAddress.AddressLine3);
                }
                if(contact.OrgBillingAddress.Country != "")
                    ChooseSpecificSelectOption(contactBillingCountrySelect, contact.OrgBillingAddress.Country);
                if (contact.OrgBillingAddress.Country == "Other")
                {
                    ClearInput(contactBillingOtherCountryInput);
                    webDriver.FindElement(contactBillingOtherCountryInput).SendKeys(contact.OrgBillingAddress.OtherCountry);
                }
                if(contact.OrgBillingAddress.Province != "")
                {
                    ChooseSpecificSelectOption(contactBillingProvinceSelect, contact.OrgBillingAddress.Province);
                }
                if (contact.OrgBillingAddress.City != "")
                {
                    Wait();
                    ClearInput(contactBillingCityInput);
                    webDriver.FindElement(contactBillingCityInput).SendKeys(contact.OrgBillingAddress.City);
                }
                if (contact.OrgBillingAddress.PostalCode != "")
                {
                    Wait();
                    ClearInput(contactBillingPostalCodeInput);
                    webDriver.FindElement(contactBillingPostalCodeInput).SendKeys(contact.OrgBillingAddress.PostalCode);
                }
            }

            //Inserting comments
            ClearInput(contactCommentTextarea);
            webDriver.FindElement(contactCommentTextarea).SendKeys(contact.OrgComments);
        }

        //Update Individual Contact
        public void UpdateIndividualContact(IndividualContact contact)
        {
            WaitUntilClickable(contactEditButton);
            webDriver.FindElement(contactEditButton).Click();

            Wait();
            //Updating individual personal details
            if (contact.MiddleName != "")
            {
                WaitUntilClickable(contactIndMiddleNameInput);
                ClearInput(contactIndMiddleNameInput);
                webDriver.FindElement(contactIndMiddleNameInput).SendKeys(contact.MiddleName);
            }
            if (contact.PreferableName != "")
            {
                WaitUntilClickable(contactIndPrefNameInput);
                ClearInput(contactIndPrefNameInput);
                webDriver.FindElement(contactIndPrefNameInput).SendKeys(contact.PreferableName);
            }
            if (contact.Organization != "")
            {
                WaitUntilClickable(contactIndOrgInput);
                ClearInput(contactIndOrgInput);
                webDriver.FindElement(contactIndOrgInput).SendKeys(contact.Organization);

                WaitUntilVisible(contactIndOrgListOptions);
                webDriver.FindElement(contactOrgName1stOption).Click();
            }

            //Updating contact info
            if (contact.IndEmail1 != "")
            {
                ClearInput(contactEmailInput1);
                webDriver.FindElement(contactEmailInput1).SendKeys(contact.IndEmail1);
                ChooseSpecificSelectOption(contactEmailSelect1, contact.IndEmailType1);
            }
            if (contact.IndEmail2 != "" && webDriver.FindElement(contactEmailInput2).Displayed)
            {
                ClearInput(contactEmailInput2);
                webDriver.FindElement(contactEmailInput2).SendKeys(contact.IndEmail2);
                ChooseSpecificSelectOption(contactEmailSelect2, contact.IndEmailType2);
            }
            if (contact.IndPhone1 != "")
            {
                ClearInput(contactPhoneInput1);
                webDriver.FindElement(contactPhoneInput1).SendKeys(contact.IndPhone1);
                ChooseSpecificSelectOption(contactPhoneSelect1, contact.IndPhoneType1);
            }
            if (contact.IndPhone2 != "" && webDriver.FindElement(contactPhoneInput2).Displayed)
            {
                ClearInput(contactPhoneInput2);
                webDriver.FindElement(contactPhoneInput2).SendKeys(contact.IndPhone2);
                ChooseSpecificSelectOption(contactPhoneSelect2, contact.IndPhoneType2);
            }

            //Updating contact mail address
            if (contact.IndMailAddress.AddressLine1 != "")
            {
                while (webDriver.FindElements(contactAddressMailAddressRemoveBttn).Count > 0)
                {
                    FocusAndClick(contactAddressMailAddressRemoveBttn);
                }
                ClearInput(contactMailAddressLine1Input);

                webDriver.FindElement(contactMailAddressLine1Input).SendKeys(contact.IndMailAddress.AddressLine1);

                if (contact.IndMailAddress.AddressLine2 != "")
                {
                    webDriver.FindElement(contactIndMailAddAddressLineBttn).Click();
                    webDriver.FindElement(contactMailAddressLine2Input).SendKeys(contact.IndMailAddress.AddressLine2);
                }
                if (contact.IndMailAddress.AddressLine3 != "")
                {
                    webDriver.FindElement(contactIndMailAddAddressLineBttn).Click();
                    webDriver.FindElement(contactMailAddressLine3Input).SendKeys(contact.IndMailAddress.AddressLine3);
                }
                if (contact.IndMailAddress.Country != "")
                    ChooseSpecificSelectOption(contactMailCountrySelect, contact.IndMailAddress.Country);
                if (contact.IndMailAddress.Country == "Other")
                {
                    ClearInput(contactMailOtherCountryInput);
                    webDriver.FindElement(contactMailOtherCountryInput).SendKeys(contact.IndMailAddress.OtherCountry);
                }
                if (contact.IndMailAddress.Province != "")
                    ChooseSpecificSelectOption(contactMailProvinceSelect, contact.IndMailAddress.Province);
                
                if (contact.IndMailAddress.City != "")
                {
                    ClearInput(contactMailCityInput);
                    webDriver.FindElement(contactMailCityInput).SendKeys(contact.IndMailAddress.City);
                }
                if (contact.IndMailAddress.PostalCode != "")
                {
                    ClearInput(contactMailPostalCodeInput);
                    webDriver.FindElement(contactMailPostalCodeInput).SendKeys(contact.IndMailAddress.PostalCode);
                }
            }

            //Updating contact property address
            if (contact.IndPropertyAddress.AddressLine1 != "")
            {
                while (webDriver.FindElements(contactAddressPropertyAddressRemoveBttn).Count > 0)
                    FocusAndClick(contactAddressPropertyAddressRemoveBttn);
                
                ClearInput(contactPropertyAddressLine1Input);
                webDriver.FindElement(contactPropertyAddressLine1Input).SendKeys(contact.IndPropertyAddress.AddressLine1);

                if (contact.IndPropertyAddress.AddressLine2 != "")
                {
                    webDriver.FindElement(contactPropertyAddAddressLineBttn).Click();
                    webDriver.FindElement(contactPropertyAddressLine2Input).SendKeys(contact.IndPropertyAddress.AddressLine2);
                }
                if (contact.IndPropertyAddress.AddressLine3 != "")
                {
                    webDriver.FindElement(contactPropertyAddAddressLineBttn).Click();
                    webDriver.FindElement(contactPropertyAddressLine3Input).SendKeys(contact.IndPropertyAddress.AddressLine3);
                }
                if (contact.IndPropertyAddress.Country != "")
                    ChooseSpecificSelectOption(contactPropertyCountrySelect, contact.IndPropertyAddress.Country);
                if (contact.IndPropertyAddress.Country == "Other")
                {
                    ClearInput(contactPropertyOtherCountryInput);
                    webDriver.FindElement(contactPropertyOtherCountryInput).SendKeys(contact.IndPropertyAddress.OtherCountry);
                }
                if (contact.IndPropertyAddress.Province != "")
                    ChooseSpecificSelectOption(contactPropertyProvinceSelect, contact.IndPropertyAddress.Province);
                if (contact.IndPropertyAddress.City != "")
                {
                    ClearInput(contactPropertyCityInput);
                    webDriver.FindElement(contactPropertyCityInput).SendKeys(contact.IndPropertyAddress.City);
                }
                if (contact.IndPropertyAddress.PostalCode != "")
                {
                    ClearInput(contactPropertyPostalCodeInput);
                    webDriver.FindElement(contactPropertyPostalCodeInput).SendKeys(contact.IndPropertyAddress.PostalCode);
                }
            }

            //Updating contact billing address
            if (contact.IndBillingAddress.AddressLine1 != "")
            {
                while (webDriver.FindElements(contactAddressBillingAddressRemoveBttn).Count > 0)
                    FocusAndClick(contactAddressBillingAddressRemoveBttn);
                
                ClearInput(contactBillingAddressLine1Input);
                webDriver.FindElement(contactBillingAddressLine1Input).SendKeys(contact.IndBillingAddress.AddressLine1);

                if (contact.IndBillingAddress.AddressLine2 != "")
                {
                    webDriver.FindElement(contactBillingAddAddressLineBttn).Click();
                    webDriver.FindElement(contactBillingAddressLine2Input).SendKeys(contact.IndBillingAddress.AddressLine2);
                }
                if (contact.IndBillingAddress.AddressLine3 != "")
                {
                    webDriver.FindElement(contactBillingAddAddressLineBttn).Click();
                    webDriver.FindElement(contactBillingAddressLine3Input).SendKeys(contact.IndBillingAddress.AddressLine3);
                }
                if (contact.IndBillingAddress.Country != "")
                    ChooseSpecificSelectOption(contactBillingCountrySelect, contact.IndBillingAddress.Country);
                if (contact.IndBillingAddress.Country == "Other")
                {
                    ClearInput(contactBillingOtherCountryInput);
                    webDriver.FindElement(contactBillingOtherCountryInput).SendKeys(contact.IndBillingAddress.OtherCountry);
                }
                if (contact.IndBillingAddress.Province != "")
                {
                    ChooseSpecificSelectOption(contactBillingProvinceSelect, contact.IndBillingAddress.Province);
                }
                if (contact.IndBillingAddress.City != "")
                {
                    ClearInput(contactBillingCityInput);
                    webDriver.FindElement(contactBillingCityInput).SendKeys(contact.IndBillingAddress.City);
                }
                if (contact.IndBillingAddress.PostalCode != "")
                {
                    ClearInput(contactBillingPostalCodeInput);
                    webDriver.FindElement(contactBillingPostalCodeInput).SendKeys(contact.IndBillingAddress.PostalCode);
                }
            }

            //Inserting comments
            ClearInput(contactCommentTextarea);
            webDriver.FindElement(contactCommentTextarea).SendKeys(contact.IndComments);
        }

        //Saves Contact
        public void SaveContact()
        {
            ButtonElement("Save");

            Wait();
            if (webDriver.FindElements(contactModal).Count > 0)
            {
                Assert.Equal("Duplicate Contact", sharedModals.ModalHeader());
                Assert.Equal("A contact matching this information already exists in the system.", sharedModals.ModalContent());

                sharedModals.ModalClickOKBttn();
            }
        }

        //Cancel Contact
        public void CancelContact()
        {
            ButtonElement("Cancel");

            Wait();
            sharedModals.CancelActionModal();

            AssertTrueIsDisplayed(contactsSearchTable);
        }

        // ASSERT FUNCTIONS
        public void VerifyIndividualContactView(IndividualContact contact)
        {
            Wait();

            AssertTrueIsDisplayed(contactTitle);
            AssertTrueIsDisplayed(contactEditButton);
            AssertTrueIsDisplayed(contactDetailsSubtitle);

           AssertTrueContentEquals(contactIndStatusSpan, contact.ContactStatus);

            if (contact.FullName != "")
                AssertTrueContentEquals(contactIndFullName, contact.FullName);

            AssertTrueIsDisplayed(contactIndPrefNameLabel);
            if(contact.PreferableName != "")
                AssertTrueContentEquals(contactIndPrefNameContent, contact.PreferableName);

            AssertTrueIsDisplayed(contactIndLinkedOrgsLabel);
            if (contact.Organization != "")
                AssertTrueContentEquals(contactIndOrganizationContent, contact.Organization);

            AssertTrueIsDisplayed(contactIndContactInfoSubtitle);
            AssertTrueIsDisplayed(contactEmailLabel);
            
            if (contact.IndEmail1 != "")
            {
                AssertTrueContentEquals(contactEmail1Content, contact.IndEmail1);
                AssertTrueContentEquals(contactEmailType1Content, contact.IndEmailTypeDisplay1);
            }
            if (contact.IndEmail2 != "")
            {
                AssertTrueContentEquals(contactEmail2Content, contact.IndEmail2);
                AssertTrueContentEquals(contactEmailType2Content, contact.IndEmailTypeDisplay2);
            }
            AssertTrueIsDisplayed(contactPhoneLabel);
            
            if (contact.IndPhone1 != "")
            {
                AssertTrueContentEquals(contactPhone1Content, contact.IndPhone1);
                AssertTrueContentEquals(contactPhoneType1Content, contact.IndPhoneTypeDisplay1);
            }
            if (contact.IndPhone2 != "")
            {
                AssertTrueContentEquals(contactPhone2Content, contact.IndPhone2);
                AssertTrueContentEquals(contactPhoneType2Content, contact.IndPhoneTypeDisplay2);
            }

            AssertTrueIsDisplayed(contactAddressSubtitle);

            if (contact.IndMailAddress.AddressLine1 != "")
            {
                AssertTrueIsDisplayed(contactAddressMailSubtitle);

                var mailAddressTotalLines = webDriver.FindElements(contactAddressIndMailCounter).Count;
                switch (mailAddressTotalLines)
                {
                    case 4:
                        AssertTrueContentEquals(contactAddressIndMailAddressLine1, contact.IndMailAddress.AddressLine1);
                        AssertTrueContentEquals(contactAddressIndMailAddressLine2, contact.IndMailAddress.CityProvinceView);
                        AssertTrueContentEquals(contactAddressIndMailAddressLine3, contact.IndMailAddress.PostalCode);

                        if (contact.IndMailAddress.Country == "Other")
                            AssertTrueContentEquals(contactAddressIndMailAddressLine4, contact.IndMailAddress.OtherCountry);
                        else
                            AssertTrueContentEquals(contactAddressIndMailAddressLine4, contact.IndMailAddress.Country);
                        break;

                    case 5:
                        AssertTrueContentEquals(contactAddressIndMailAddressLine1, contact.IndMailAddress.AddressLine1);
                        AssertTrueContentEquals(contactAddressIndMailAddressLine2, contact.IndMailAddress.AddressLine2);
                        AssertTrueContentEquals(contactAddressIndMailAddressLine3, contact.IndMailAddress.CityProvinceView);
                        AssertTrueContentEquals(contactAddressIndMailAddressLine4, contact.IndMailAddress.PostalCode);

                        if (contact.IndMailAddress.Country == "Other")
                            AssertTrueContentEquals(contactAddressIndMailAddressLine5, contact.IndMailAddress.OtherCountry);
                        else
                            AssertTrueContentEquals(contactAddressIndMailAddressLine5, contact.IndMailAddress.Country);
                        break;

                    case 6:
                        AssertTrueContentEquals(contactAddressIndMailAddressLine1,contact.IndMailAddress.AddressLine1);
                        AssertTrueContentEquals(contactAddressIndMailAddressLine2,contact.IndMailAddress.AddressLine2);
                        AssertTrueContentEquals(contactAddressIndMailAddressLine3,contact.IndMailAddress.AddressLine3);
                        AssertTrueContentEquals(contactAddressIndMailAddressLine4,contact.IndMailAddress.CityProvinceView);
                        AssertTrueContentEquals(contactAddressIndMailAddressLine5,contact.IndMailAddress.PostalCode);

                        if (contact.IndMailAddress.Country == "Other")
                            AssertTrueContentEquals(contactAddressIndMailAddressLine6,contact.IndMailAddress.OtherCountry);
                        else
                            AssertTrueContentEquals(contactAddressIndMailAddressLine6,contact.IndMailAddress.Country);
                        break;
                }
            }
            if (contact.IndPropertyAddress.AddressLine1 != "")
            {
                AssertTrueIsDisplayed(contactAddressPropertySubtitle);

                var propertyAddressTotalLines = webDriver.FindElements(contactAddressIndPropertyCounter).Count;
                switch (propertyAddressTotalLines)
                {
                    case 4:
                        AssertTrueContentEquals(contactAddressIndPropertyAddressLine1,contact.IndPropertyAddress.AddressLine1);
                        AssertTrueContentEquals(contactAddressIndPropertyAddressLine2,contact.IndPropertyAddress.CityProvinceView);
                        AssertTrueContentEquals(contactAddressIndPropertyAddressLine3,contact.IndPropertyAddress.PostalCode);

                        if (contact.IndPropertyAddress.Country == "Other")
                            AssertTrueContentEquals(contactAddressIndPropertyAddressLine4, contact.IndPropertyAddress.OtherCountry);
                        else
                            AssertTrueContentEquals(contactAddressIndPropertyAddressLine4, contact.IndPropertyAddress.Country);
                        break;

                    case 5:
                        AssertTrueContentEquals(contactAddressIndPropertyAddressLine1, contact.IndPropertyAddress.AddressLine1);
                        AssertTrueContentEquals(contactAddressIndPropertyAddressLine2, contact.IndPropertyAddress.AddressLine2);
                        AssertTrueContentEquals(contactAddressIndPropertyAddressLine3, contact.IndPropertyAddress.CityProvinceView);
                        AssertTrueContentEquals(contactAddressIndPropertyAddressLine4, contact.IndPropertyAddress.PostalCode);

                        if (contact.IndPropertyAddress.Country == "Other")
                            AssertTrueContentEquals(contactAddressIndPropertyAddressLine5, contact.IndPropertyAddress.OtherCountry);
                        else
                            AssertTrueContentEquals(contactAddressIndPropertyAddressLine5, contact.IndPropertyAddress.Country);
                        break;

                    case 6:
                        AssertTrueContentEquals(contactAddressIndPropertyAddressLine1, contact.IndPropertyAddress.AddressLine1);
                        AssertTrueContentEquals(contactAddressIndPropertyAddressLine2, contact.IndPropertyAddress.AddressLine2);
                        AssertTrueContentEquals(contactAddressIndPropertyAddressLine3, contact.IndPropertyAddress.AddressLine3);
                        AssertTrueContentEquals(contactAddressIndPropertyAddressLine4, contact.IndPropertyAddress.CityProvinceView);
                        AssertTrueContentEquals(contactAddressIndPropertyAddressLine5, contact.IndPropertyAddress.PostalCode);

                        if (contact.IndPropertyAddress.Country == "Other")
                            AssertTrueContentEquals(contactAddressIndPropertyAddressLine6, contact.IndPropertyAddress.OtherCountry);
                        else
                            AssertTrueContentEquals(contactAddressIndPropertyAddressLine6, contact.IndPropertyAddress.Country);
                        break;
                }
            }
            if (contact.IndBillingAddress.AddressLine1 != "")
            {
                AssertTrueIsDisplayed(contactAddressBillingSubtitle);

                var propertyAddressTotalLines = webDriver.FindElements(contactAddressIndBillingCounter).Count;
                switch (propertyAddressTotalLines)
                {
                    case 4:
                        AssertTrueContentEquals(contactAddressIndBillingAddressLine1, contact.IndBillingAddress.AddressLine1);
                        AssertTrueContentEquals(contactAddressIndBillingAddressLine2, contact.IndBillingAddress.CityProvinceView);
                        AssertTrueContentEquals(contactAddressIndBillingAddressLine3, contact.IndBillingAddress.PostalCode);

                        if (contact.IndBillingAddress.Country == "Other")
                            AssertTrueContentEquals(contactAddressIndBillingAddressLine4, contact.IndBillingAddress.OtherCountry);
                        else
                            AssertTrueContentEquals(contactAddressIndBillingAddressLine4, contact.IndBillingAddress.Country);
                        break;

                    case 5:
                        AssertTrueContentEquals(contactAddressIndBillingAddressLine1, contact.IndBillingAddress.AddressLine1);
                        AssertTrueContentEquals(contactAddressIndBillingAddressLine2, contact.IndBillingAddress.AddressLine2);
                        AssertTrueContentEquals(contactAddressIndBillingAddressLine3, contact.IndBillingAddress.CityProvinceView);
                        AssertTrueContentEquals(contactAddressIndBillingAddressLine4, contact.IndBillingAddress.PostalCode);

                        if (contact.IndBillingAddress.Country == "Other")
                            AssertTrueContentEquals(contactAddressIndBillingAddressLine5, contact.IndBillingAddress.OtherCountry);
                        else
                            AssertTrueContentEquals(contactAddressIndBillingAddressLine5, contact.IndBillingAddress.Country);
                        break;

                    case 6:
                        AssertTrueContentEquals(contactAddressIndBillingAddressLine1, contact.IndBillingAddress.AddressLine1);
                        AssertTrueContentEquals(contactAddressIndBillingAddressLine2, contact.IndBillingAddress.AddressLine2);
                        AssertTrueContentEquals(contactAddressIndBillingAddressLine3, contact.IndBillingAddress.AddressLine3);
                        AssertTrueContentEquals(contactAddressIndBillingAddressLine4, contact.IndBillingAddress.CityProvinceView);
                        AssertTrueContentEquals(contactAddressIndBillingAddressLine5, contact.IndBillingAddress.PostalCode);

                        if (contact.IndBillingAddress.Country == "Other")
                            AssertTrueContentEquals(contactAddressIndBillingAddressLine6, contact.IndBillingAddress.OtherCountry);
                        else
                            AssertTrueContentEquals(contactAddressIndBillingAddressLine6, contact.IndBillingAddress.Country);
                        break;
                }
            }
            AssertTrueIsDisplayed(commentsSubtitle);
            AssertTrueContentEquals(commentsIndividualContent, contact.IndComments);
        }

        public void VerifyOrganizationContactView(OrganizationContact contact)
        {
            AssertTrueIsDisplayed(contactTitle);
            AssertTrueIsDisplayed(contactEditButton);
            AssertTrueIsDisplayed(contactDetailsSubtitle);

            AssertTrueContentEquals(contactOrgStatusSpan, contact.ContactStatus);

            if (contact.OrganizationName != "")
                AssertTrueContentEquals(contactOrgName, contact.OrganizationName);

            AssertTrueIsDisplayed(contactOrgAliasLabel);
            if(contact.Alias != "")
                AssertTrueContentEquals(contactOrgAliasContent, contact.Alias);

            AssertTrueIsDisplayed(contactOrgIncorpNbrLabel);
            if(contact.IncorporationNumber != "")
                AssertTrueContentEquals(contactOrgIncorpNbrContent,contact.IncorporationNumber);

            AssertTrueIsDisplayed(contactOrgPrefContactSubtitle);
            AssertTrueIsDisplayed(contactEmailLabel);

            if (contact.OrgEmail1 != "")
            {
                AssertTrueContentEquals(contactEmail1Content, contact.OrgEmail1);
                AssertTrueContentEquals(contactEmailType1Content, contact.OrgEmailTypeDisplay1);
            }
            if (contact.OrgEmail2 != "")
            {
                AssertTrueContentEquals(contactEmail2Content, contact.OrgEmail2);
                AssertTrueContentEquals(contactEmailType2Content, contact.OrgEmailTypeDisplay2);
            }

            AssertTrueIsDisplayed(contactPhoneLabel);
            if (contact.OrgPhone1 != "")
            {
                AssertTrueContentEquals(contactPhone1Content, contact.OrgPhone1);
                AssertTrueContentEquals(contactPhoneType1Content, contact.OrgPhoneTypeDisplay1);
            }
            if (contact.OrgPhone2 != "")
            {
                AssertTrueContentEquals(contactPhone2Content, contact.OrgPhone2);
                AssertTrueContentEquals(contactPhoneType2Content, contact.OrgPhoneTypeDisplay2);
            }

            AssertTrueIsDisplayed(contactAddressSubtitle);
            if (contact.OrgMailAddress.AddressLine1 != "")
            {
                AssertTrueIsDisplayed(contactAddressMailSubtitle);

                var mailAddressTotalLines = webDriver.FindElements(contactAddressOrgMailCounter).Count;
                switch (mailAddressTotalLines)
                {
                    case 4:
                        AssertTrueContentEquals(contactAddressOrgMailAddressLine1, contact.OrgMailAddress.AddressLine1);
                        AssertTrueContentEquals(contactAddressOrgMailAddressLine2, contact.OrgMailAddress.CityProvinceView);
                        AssertTrueContentEquals(contactAddressOrgMailAddressLine3, contact.OrgMailAddress.PostalCode);

                        if (contact.OrgMailAddress.Country == "Other")
                            AssertTrueContentEquals(contactAddressOrgMailAddressLine4, contact.OrgMailAddress.OtherCountry);
                        else
                            AssertTrueContentEquals(contactAddressOrgMailAddressLine4, contact.OrgMailAddress.Country);
                        break;

                    case 5:
                        AssertTrueContentEquals(contactAddressOrgMailAddressLine1, contact.OrgMailAddress.AddressLine1);
                        AssertTrueContentEquals(contactAddressOrgMailAddressLine2, contact.OrgMailAddress.AddressLine2);
                        AssertTrueContentEquals(contactAddressOrgMailAddressLine3, contact.OrgMailAddress.CityProvinceView);
                        AssertTrueContentEquals(contactAddressOrgMailAddressLine4, contact.OrgMailAddress.PostalCode);

                        if (contact.OrgMailAddress.Country == "Other")
                            AssertTrueContentEquals(contactAddressOrgMailAddressLine5, contact.OrgMailAddress.OtherCountry);
                        else
                            AssertTrueContentEquals(contactAddressOrgMailAddressLine5, contact.OrgMailAddress.Country);
                        break;

                    case 6:
                        AssertTrueContentEquals(contactAddressOrgMailAddressLine1, contact.OrgMailAddress.AddressLine1);
                        AssertTrueContentEquals(contactAddressOrgMailAddressLine2, contact.OrgMailAddress.AddressLine2);
                        AssertTrueContentEquals(contactAddressOrgMailAddressLine3, contact.OrgMailAddress.AddressLine3);
                        AssertTrueContentEquals(contactAddressOrgMailAddressLine4, contact.OrgMailAddress.CityProvinceView);
                        AssertTrueContentEquals(contactAddressOrgMailAddressLine5, contact.OrgMailAddress.PostalCode);

                        if (contact.OrgMailAddress.Country == "Other")
                            AssertTrueContentEquals(contactAddressOrgMailAddressLine6, contact.OrgMailAddress.OtherCountry);
                        else
                            AssertTrueContentEquals(contactAddressOrgMailAddressLine6, contact.OrgMailAddress.Country);
                        break;
                }
            }
            if (contact.OrgPropertyAddress.AddressLine1 != "")
            {
                AssertTrueIsDisplayed(contactAddressPropertySubtitle);

                var propertyAddressTotalLines = webDriver.FindElements(contactAddressOrgPropertyCounter).Count;
                switch (propertyAddressTotalLines)
                {
                    case 4:
                        AssertTrueContentEquals(contactAddressOrgPropertyAddressLine1, contact.OrgPropertyAddress.AddressLine1);
                        AssertTrueContentEquals(contactAddressOrgPropertyAddressLine2, contact.OrgPropertyAddress.CityProvinceView);
                        AssertTrueContentEquals(contactAddressOrgPropertyAddressLine3, contact.OrgPropertyAddress.PostalCode);

                        if (contact.OrgPropertyAddress.Country == "Other")
                            AssertTrueContentEquals(contactAddressOrgPropertyAddressLine4, contact.OrgPropertyAddress.OtherCountry);
                        else  
                            AssertTrueContentEquals(contactAddressOrgPropertyAddressLine4, contact.OrgPropertyAddress.Country);
                        break;

                    case 5:
                        AssertTrueContentEquals(contactAddressOrgPropertyAddressLine1,contact.OrgPropertyAddress.AddressLine1);
                        AssertTrueContentEquals(contactAddressOrgPropertyAddressLine2,contact.OrgPropertyAddress.AddressLine2);
                        AssertTrueContentEquals(contactAddressOrgPropertyAddressLine3,contact.OrgPropertyAddress.CityProvinceView);
                        AssertTrueContentEquals(contactAddressOrgPropertyAddressLine4,contact.OrgPropertyAddress.PostalCode);

                        if (contact.OrgPropertyAddress.Country == "Other")
                        {
                            AssertTrueContentEquals(contactAddressOrgPropertyAddressLine5,contact.OrgPropertyAddress.OtherCountry);
                        }
                        else
                        {
                            AssertTrueContentEquals(contactAddressOrgPropertyAddressLine5,contact.OrgPropertyAddress.Country);
                        }
                        break;

                    case 6:
                        AssertTrueContentEquals(contactAddressOrgPropertyAddressLine1,contact.OrgPropertyAddress.AddressLine1);
                        AssertTrueContentEquals(contactAddressOrgPropertyAddressLine2,contact.OrgPropertyAddress.AddressLine2);
                        AssertTrueContentEquals(contactAddressOrgPropertyAddressLine3,contact.OrgPropertyAddress.AddressLine3);
                        AssertTrueContentEquals(contactAddressOrgPropertyAddressLine4,contact.OrgPropertyAddress.CityProvinceView);
                        AssertTrueContentEquals(contactAddressOrgPropertyAddressLine5,contact.OrgPropertyAddress.PostalCode);

                        if (contact.OrgPropertyAddress.Country == "Other")
                            AssertTrueContentEquals(contactAddressOrgPropertyAddressLine6,contact.OrgPropertyAddress.OtherCountry);
                        else
                            AssertTrueContentEquals(contactAddressOrgPropertyAddressLine6,contact.OrgPropertyAddress.Country);
                        break;
                }
            }
            if (contact.OrgBillingAddress.AddressLine1 != "")
            {
                AssertTrueIsDisplayed(contactAddressBillingSubtitle);

                var propertyAddressTotalLines = webDriver.FindElements(contactAddressOrgBillingCounter).Count;
                switch (propertyAddressTotalLines)
                {
                    case 4:
                        AssertTrueContentEquals(contactAddressOrgBillingAddressLine1,contact.OrgBillingAddress.AddressLine1);
                        AssertTrueContentEquals(contactAddressOrgBillingAddressLine2,contact.OrgBillingAddress.CityProvinceView);
                        AssertTrueContentEquals(contactAddressOrgBillingAddressLine3,contact.OrgBillingAddress.PostalCode);
                        if (contact.OrgBillingAddress.Country == "Other")
                            AssertTrueContentEquals(contactAddressOrgBillingAddressLine4,contact.OrgBillingAddress.OtherCountry);
                        else
                            AssertTrueContentEquals(contactAddressOrgBillingAddressLine4,contact.OrgBillingAddress.Country);
                        break;

                    case 5:
                        AssertTrueContentEquals(contactAddressOrgBillingAddressLine1,contact.OrgBillingAddress.AddressLine1);
                        AssertTrueContentEquals(contactAddressOrgBillingAddressLine2,contact.OrgBillingAddress.AddressLine2);
                        AssertTrueContentEquals(contactAddressOrgBillingAddressLine3,contact.OrgBillingAddress.CityProvinceView);
                        AssertTrueContentEquals(contactAddressOrgBillingAddressLine4,contact.OrgBillingAddress.PostalCode);

                        if (contact.OrgBillingAddress.Country == "Other")
                            AssertTrueContentEquals(contactAddressOrgBillingAddressLine5, contact.OrgBillingAddress.OtherCountry);
                        else
                            AssertTrueContentEquals(contactAddressOrgBillingAddressLine5,contact.OrgBillingAddress.Country);
                        break;

                    case 6:
                        AssertTrueContentEquals(contactAddressOrgBillingAddressLine1,contact.OrgBillingAddress.AddressLine1);
                        AssertTrueContentEquals(contactAddressOrgBillingAddressLine2,contact.OrgBillingAddress.AddressLine2);
                        AssertTrueContentEquals(contactAddressOrgBillingAddressLine3,contact.OrgBillingAddress.AddressLine3);
                        AssertTrueContentEquals(contactAddressOrgBillingAddressLine4,contact.OrgBillingAddress.CityProvinceView);
                        AssertTrueContentEquals(contactAddressOrgBillingAddressLine5,contact.OrgBillingAddress.PostalCode);

                        if (contact.OrgBillingAddress.Country == "Other")
                            AssertTrueContentEquals(contactAddressOrgBillingAddressLine6,contact.OrgBillingAddress.OtherCountry);
                        else
                            AssertTrueContentEquals(contactAddressOrgBillingAddressLine6,contact.OrgBillingAddress.Country);
                        break;
                }
            }
            AssertTrueIsDisplayed(commentsSubtitle);
            AssertTrueContentEquals(commentsOrganizationContent,contact.OrgComments);
        }

    }
}
