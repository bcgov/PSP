using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class Contacts : PageObjectBase
    {
        //Contact Menu Elements
        private By menuContactsButton = By.XPath("//a/label[contains(text(),'Contacts')]/parent::a");
        private By createContactButton = By.XPath("//a[contains(text(),'Add a Contact')]");

        //Contacts Create Elements
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
        private By contactMailAddressLine2Input = By.Id("input-mailingAddress.streetAddress2");
        private By contactMailAddressLine3Input = By.Id("input-mailingAddress.streetAddress3");
        private By contactMailCountrySelect = By.Id("input-mailingAddress.countryId");
        private By contactMailOtherCountryInput = By.Id("input-mailingAddress.countryOther");
        private By contactMailCityInput = By.Id("input-mailingAddress.municipality");
        private By contactMailProvinceSelect = By.Id("input-mailingAddress.provinceId");
        private By contactMailPostalCodeInput = By.Id("input-mailingAddress.postal");
        private By contactMailAddAddressLineBttn = By.XPath("//h3[contains(text(),'Mailing Address')]/following-sibling::div/div/button");

        private By contactPropertyAddressLine1Input = By.Id("input-propertyAddress.streetAddress1");
        private By contactPropertyAddressLine2Input = By.Id("input-propertyAddress.streetAddress2");
        private By contactPropertyAddressLine3Input = By.Id("input-propertyAddress.streetAddress3");
        private By contactPropertyCountrySelect = By.Id("input-propertyAddress.countryId");
        private By contactPropertyOtherCountryInput = By.Id("input-propertyAddress.countryOther");
        private By contactPropertyCityInput = By.Id("input-propertyAddress.municipality");
        private By contactPropertyProvinceSelect = By.Id("input-propertyAddress.provinceId");
        private By contactPropertyPostalCodeInput = By.Id("input-propertyAddress.postal");
        private By contactPropertyAddAddressLineBttn = By.XPath("//h3[contains(text(),'Property Address')]/following-sibling::div[2]/div/button");

        private By contactBillingAddressLine1Input = By.Id("input-billingAddress.streetAddress1");
        private By contactBillingAddressLine2Input = By.Id("input-billingAddress.streetAddress2");
        private By contactBillingAddressLine3Input = By.Id("input-billingAddress.streetAddress3");
        private By contactBillingCountrySelect = By.Id("input-billingAddress.countryId");
        private By contactBillingCityInput = By.Id("input-billingAddress.municipality");
        private By contactBillingOtherCountryInput = By.Id("input-billingAddress.countryOther");
        private By contactBillingProvinceSelect = By.Id("input-billingAddress.provinceId");
        private By contactBillingPostalCodeInput = By.Id("input-billingAddress.postal");
        private By contactBillingAddAddressLineBttn = By.XPath("//h3[contains(text(),'Billing Address')]/following-sibling::div[2]/div/button");

        private By contactCommentTextarea = By.CssSelector("textarea[name='comment']");

        //Contacts Form View Elements
        private By contactTitle = By.XPath("//h1[contains(text(),'Contact')]");
        private By contactEditButton = By.CssSelector("button[title='Edit Contact']");

        private By contactIndFullName = By.CssSelector("h2[data-testid='contact-person-fullname']");
        private By contactIndPrefNameLabel = By.XPath("//strong[contains(text(),'Preferred name')]");
        private By contactIndPrefNameContent = By.CssSelector("span[data-testid='contact-person-preferred']");
        private By contactIndStatusSpan = By.CssSelector("span[data-testid='contact-person-status']");
        private By contactIndOrganizationLabel = By.XPath("//strong[contains(text(),'Organization(s)')]");
        private By contactIndOrganizationContent = By.CssSelector("a[data-testid='contact-person-organization']");

        private By contactOrgName = By.CssSelector("span[data-testid='contact-organization-fullname']");
        private By contactOrgAliasLabel = By.XPath("//strong[contains(text(),'Alias')]");
        private By contactOrgAliasContent = By.CssSelector("span[data-testid='contact-organization-alias']");
        private By contactOrgStatusSpan = By.CssSelector("span[data-testid='contact-organization-status']");
        private By contactOrgIncorpNbrLabel = By.XPath("//strong[contains(text(),'Incorporation Number')]");
        private By contactOrgIncorpNbrContent = By.CssSelector("span[data-testid='contact-organization-incorporationNumber']");


        private By contactInfoSubtitle = By.XPath("//h2[contains(text(),'Contact info')]");
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
        private By contactAddressMailSubtitle = By.XPath("//strong[contains(text(),'Mailing address')]");
        private By contactAddressIndMailCounter = By.XPath("//div[1]/div/span[@data-testid='contact-person-address']/div");
        private By contactAddressIndMailAddressLine1 = By.XPath("//div[1]/div/span[@data-testid='contact-person-address']/div[1]");
        private By contactAddressIndMailAddressLine2 = By.XPath("//div[1]/div/span[@data-testid='contact-person-address']/div[2]");
        private By contactAddressIndMailAddressLine3 = By.XPath("//div[1]/div/span[@data-testid='contact-person-address']/div[3]");
        private By contactAddressIndMailAddressLine4 = By.XPath("//div[1]/div/span[@data-testid='contact-person-address']/div[4]");
        private By contactAddressIndMailAddressLine5 = By.XPath("//div[1]/div/span[@data-testid='contact-person-address']/div[5]");
        private By contactAddressIndMailAddressLine6 = By.XPath("//div[1]/div/span[@data-testid='contact-person-address']/div[6]");

        private By contactAddressOrgMailCounter = By.XPath("//div[1]/div[@data-testid='contact-organization-address']/div");
        private By contactAddressOrgMailAddressLine1 = By.XPath("//div[1]/div[@data-testid='contact-organization-address']/div[1]");
        private By contactAddressOrgMailAddressLine2 = By.XPath("//div[1]/div[@data-testid='contact-organization-address']/div[2]");
        private By contactAddressOrgMailAddressLine3 = By.XPath("//div[1]/div[@data-testid='contact-organization-address']/div[3]");
        private By contactAddressOrgMailAddressLine4 = By.XPath("//div[1]/div[@data-testid='contact-organization-address']/div[4]");
        private By contactAddressOrgMailAddressLine5 = By.XPath("//div[1]/div[@data-testid='contact-organization-address']/div[5]");
        private By contactAddressOrgMailAddressLine6 = By.XPath("//div[1]/div[@data-testid='contact-organization-address']/div[6]");

        private By contactAddressPropertySubtitle = By.XPath("//strong[contains(text(),'Property address')]");
        private By contactAddressIndPropertyCounter = By.XPath("//div[2]/div/span[@data-testid='contact-person-address']/div");
        private By contactAddressIndPropertyAddressLine1 = By.XPath("//div[2]/div/span[@data-testid='contact-person-address']/div[1]");
        private By contactAddressIndPropertyAddressLine2 = By.XPath("//div[2]/div/span[@data-testid='contact-person-address']/div[2]");
        private By contactAddressIndPropertyAddressLine3 = By.XPath("//div[2]/div/span[@data-testid='contact-person-address']/div[3]");
        private By contactAddressIndPropertyAddressLine4 = By.XPath("//div[2]/div/span[@data-testid='contact-person-address']/div[4]");
        private By contactAddressIndPropertyAddressLine5 = By.XPath("//div[2]/div/span[@data-testid='contact-person-address']/div[5]");
        private By contactAddressIndPropertyAddressLine6 = By.XPath("//div[2]/div/span[@data-testid='contact-person-address']/div[6]");

        private By contactAddressOrgPropertyCounter = By.XPath("//div[2]/div[@data-testid='contact-organization-address']/div");
        private By contactAddressOrgPropertyAddressLine1 = By.XPath("//div[2]/div[@data-testid='contact-organization-address']/div[1]");
        private By contactAddressOrgPropertyAddressLine2 = By.XPath("//div[2]/div[@data-testid='contact-organization-address']/div[2]");
        private By contactAddressOrgPropertyAddressLine3 = By.XPath("//div[2]/div[@data-testid='contact-organization-address']/div[3]");
        private By contactAddressOrgPropertyAddressLine4 = By.XPath("//div[2]/div[@data-testid='contact-organization-address']/div[4]");
        private By contactAddressOrgPropertyAddressLine5 = By.XPath("//div[2]/div[@data-testid='contact-organization-address']/div[5]");
        private By contactAddressOrgPropertyAddressLine6 = By.XPath("//div[2]/div[@data-testid='contact-organization-address']/div[6]");

        private By contactAddressBillingSubtitle = By.XPath("//strong[contains(text(),'Billing address')]");
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

        private By commentsSubtitle = By.XPath("//strong[contains(text(),'Comments')]");
        private By commentsIndividualContent = By.CssSelector("span[data-testid='contact-person-comment']");
        private By commentsOrganizationContent = By.CssSelector("div[data-testid='contact-organization-comment']");

        //Contact Modal Element
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

        //Creates Individual Contact with all fields
        public void CreateIndividualContact(IndividualContact contact)
        {
            Wait();

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

                Wait();
                webDriver.FindElement(contactOrgNameSelect).Click();
            }
            //Inserting contact info
            if (contact.Email1 != "")
            {
                webDriver.FindElement(contactEmailInput1).SendKeys(contact.Email1);
                ChooseSpecificSelectOption(contactEmailSelect1, contact.EmailType1);
            }
            if (contact.Email2 != "")
            {
                webDriver.FindElement(contactEmailAddBttn).Click();
                webDriver.FindElement(contactEmailInput2).SendKeys(contact.Email2);
                ChooseSpecificSelectOption(contactEmailSelect2, contact.EmailType2);
            }
            if (contact.Phone1 != "")
            {
                webDriver.FindElement(contactPhoneInput1).SendKeys(contact.Phone1);
                ChooseSpecificSelectOption(contactPhoneSelect1, contact.PhoneType1);
            }
            if (contact.Phone2 != "")
            {
                webDriver.FindElement(contactPhoneAddBttn).Click();
                webDriver.FindElement(contactPhoneInput2).SendKeys(contact.Phone2);
                ChooseSpecificSelectOption(contactPhoneSelect2, contact.PhoneType2);
            }

            //Inserting contact mail address
            if (contact.MailAddressLine1 != "")
            {
                webDriver.FindElement(contactMailAddressLine1Input).SendKeys(contact.MailAddressLine1);
                if (contact.MailAddressLine2 != "")
                {
                    webDriver.FindElement(contactMailAddAddressLineBttn).Click();
                    webDriver.FindElement(contactMailAddressLine2Input).SendKeys(contact.MailAddressLine2);
                }
                if (contact.MailAddressLine3 != "")
                {
                    webDriver.FindElement(contactMailAddAddressLineBttn).Click();
                    webDriver.FindElement(contactMailAddressLine3Input).SendKeys(contact.MailAddressLine3);
                }

                ChooseSpecificSelectOption(contactMailCountrySelect, contact.MailCountry);
                if (contact.MailCountry == "Other")
                {
                    webDriver.FindElement(contactMailOtherCountryInput).SendKeys(contact.MailOtherCountry);
                }
                else
                {
                    ChooseSpecificSelectOption(contactMailProvinceSelect, contact.MailProvince);
                }
                webDriver.FindElement(contactMailCityInput).SendKeys(contact.MailCity);
                webDriver.FindElement(contactMailPostalCodeInput).SendKeys(contact.MailPostalCode);
            }


            //Inserting contact property address
            if (contact.PropertyAddressLine1 != "")
            {
                webDriver.FindElement(contactPropertyAddressLine1Input).SendKeys(contact.PropertyAddressLine1);
                if (contact.PropertyAddressLine2 != "")
                {
                    webDriver.FindElement(contactPropertyAddAddressLineBttn).Click();
                    webDriver.FindElement(contactPropertyAddressLine2Input).SendKeys(contact.PropertyAddressLine2);
                }
                if (contact.PropertyAddressLine3 != "")
                {
                    webDriver.FindElement(contactPropertyAddAddressLineBttn).Click();
                    webDriver.FindElement(contactPropertyAddressLine3Input).SendKeys(contact.PropertyAddressLine3);
                }

                ChooseSpecificSelectOption(contactPropertyCountrySelect, contact.PropertyCountry);
                if (contact.PropertyCountry == "Other")
                {
                    webDriver.FindElement(contactPropertyOtherCountryInput).SendKeys(contact.PropertyOtherCountry);
                }
                else
                {
                    ChooseSpecificSelectOption(contactPropertyProvinceSelect, contact.PropertyProvince);
                }
                webDriver.FindElement(contactPropertyCityInput).SendKeys(contact.PropertyCity);
                webDriver.FindElement(contactPropertyPostalCodeInput).SendKeys(contact.PropertyPostalCode);
            }

            //Inserting contact billing address
            if (contact.BillingAddressLine1 != "")
            {
                webDriver.FindElement(contactBillingAddressLine1Input).SendKeys(contact.BillingAddressLine1);
                if (contact.BillingAddressLine2 != "")
                {
                    webDriver.FindElement(contactBillingAddAddressLineBttn).Click();
                    webDriver.FindElement(contactBillingAddressLine2Input).SendKeys(contact.BillingAddressLine2);
                }
                if (contact.BillingAddressLine3 != "")
                {
                    webDriver.FindElement(contactBillingAddAddressLineBttn).Click();
                    webDriver.FindElement(contactBillingAddressLine3Input).SendKeys(contact.BillingAddressLine3);
                }
                ChooseSpecificSelectOption(contactBillingCountrySelect, contact.BillingCountry);
                if (contact.BillingCountry == "Other")
                {
                    webDriver.FindElement(contactBillingOtherCountryInput).SendKeys(contact.BillingOtherCountry);
                }
                else
                {
                    ChooseSpecificSelectOption(contactBillingProvinceSelect, contact.BillingProvince);
                }
                webDriver.FindElement(contactBillingCityInput).SendKeys(contact.BillingCity);
                webDriver.FindElement(contactBillingPostalCodeInput).SendKeys(contact.BillingPostalCode);
            }

            //Inserting comments
            webDriver.FindElement(contactCommentTextarea).SendKeys(contact.Comments);
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
            if (contact.Email1 != "")
            {
                webDriver.FindElement(contactEmailInput1).SendKeys(contact.Email1);
                ChooseSpecificSelectOption(contactEmailSelect1, contact.EmailType1);
            }
            if (contact.Email2 != "")
            {
                webDriver.FindElement(contactEmailAddBttn).Click();
                webDriver.FindElement(contactEmailInput2).SendKeys(contact.Email2);
                ChooseSpecificSelectOption(contactEmailSelect2, contact.EmailType2);
            }
            if (contact.Phone1 != "")
            {
                webDriver.FindElement(contactPhoneInput1).SendKeys(contact.Phone1);
                ChooseSpecificSelectOption(contactPhoneSelect1, contact.PhoneType1);
            }
            if (contact.Phone2 != "")
            {
                webDriver.FindElement(contactPhoneAddBttn).Click();
                webDriver.FindElement(contactPhoneInput2).SendKeys(contact.Phone2);
                ChooseSpecificSelectOption(contactPhoneSelect2, contact.PhoneType2);
            }

            //Inserting contact mail address
            if (contact.MailAddressLine1 != "")
            {
                webDriver.FindElement(contactMailAddressLine1Input).SendKeys(contact.MailAddressLine1);
                if (contact.MailAddressLine2 != "")
                {
                    webDriver.FindElement(contactMailAddAddressLineBttn).Click();
                    webDriver.FindElement(contactMailAddressLine2Input).SendKeys(contact.MailAddressLine2);
                }
                if (contact.MailAddressLine3 != "")
                {
                    webDriver.FindElement(contactMailAddAddressLineBttn).Click();
                    webDriver.FindElement(contactMailAddressLine3Input).SendKeys(contact.MailAddressLine3);
                }

                ChooseSpecificSelectOption(contactMailCountrySelect, contact.MailCountry);
                if (contact.MailCountry == "Other")
                {
                    webDriver.FindElement(contactMailOtherCountryInput).SendKeys(contact.MailOtherCountry);
                }
                else
                {
                    ChooseSpecificSelectOption(contactMailProvinceSelect, contact.MailProvince);
                }
                webDriver.FindElement(contactMailCityInput).SendKeys(contact.MailCity);
                webDriver.FindElement(contactMailPostalCodeInput).SendKeys(contact.MailPostalCode);
            }

            //Inserting contact property address
            if (contact.PropertyAddressLine1 != "")
            {
                webDriver.FindElement(contactPropertyAddressLine1Input).SendKeys(contact.PropertyAddressLine1);
                if (contact.PropertyAddressLine2 != "")
                {
                    webDriver.FindElement(contactPropertyAddAddressLineBttn).Click();
                    webDriver.FindElement(contactPropertyAddressLine2Input).SendKeys(contact.PropertyAddressLine2);
                }
                if (contact.PropertyAddressLine3 != "")
                {
                    webDriver.FindElement(contactPropertyAddAddressLineBttn).Click();
                    webDriver.FindElement(contactPropertyAddressLine3Input).SendKeys(contact.PropertyAddressLine3);
                }

                ChooseSpecificSelectOption(contactPropertyCountrySelect, contact.PropertyCountry);
                if (contact.PropertyCountry == "Other")
                {
                    webDriver.FindElement(contactPropertyOtherCountryInput).SendKeys(contact.PropertyOtherCountry);
                }
                else
                {
                    ChooseSpecificSelectOption(contactPropertyProvinceSelect, contact.PropertyProvince);
                }
                webDriver.FindElement(contactPropertyCityInput).SendKeys(contact.PropertyCity);
                webDriver.FindElement(contactPropertyPostalCodeInput).SendKeys(contact.PropertyPostalCode);
            }

            //Inserting contact billing address
            if (contact.BillingAddressLine1 != "")
            {
                webDriver.FindElement(contactBillingAddressLine1Input).SendKeys(contact.BillingAddressLine1);
                if (contact.BillingAddressLine2 != "")
                {
                    webDriver.FindElement(contactBillingAddAddressLineBttn).Click();
                    webDriver.FindElement(contactBillingAddressLine2Input).SendKeys(contact.BillingAddressLine2);
                }
                if (contact.BillingAddressLine3 != "")
                {
                    webDriver.FindElement(contactBillingAddAddressLineBttn).Click();
                    webDriver.FindElement(contactBillingAddressLine3Input).SendKeys(contact.BillingAddressLine3);
                }
                ChooseSpecificSelectOption(contactBillingCountrySelect, contact.BillingCountry);
                if (contact.BillingCountry == "Other")
                {
                    webDriver.FindElement(contactBillingOtherCountryInput).SendKeys(contact.BillingOtherCountry);
                }
                else
                {
                    ChooseSpecificSelectOption(contactBillingProvinceSelect, contact.BillingProvince);
                }
                webDriver.FindElement(contactBillingCityInput).SendKeys(contact.BillingCity);
                webDriver.FindElement(contactBillingPostalCodeInput).SendKeys(contact.BillingPostalCode);
            }

            //Inserting comments
            webDriver.FindElement(contactCommentTextarea).SendKeys(contact.Comments);
        }

        //Update Contact
        public void UpdateContact(string email, string emailType, string phone, string phoneType)
        {
            Wait();

            webDriver.FindElement(contactEditButton).Click();

            Wait();
            FocusAndClick(contactEmailAddBttn);
            ClearInput(contactEmailInput2);
            webDriver.FindElement(contactEmailInput2).SendKeys(email);
            ChooseSpecificSelectOption(contactEmailSelect2, emailType);

            FocusAndClick(contactPhoneAddBttn);
            ClearInput(contactPhoneInput2);
            webDriver.FindElement(contactPhoneInput2).SendKeys(phone);
            ChooseSpecificSelectOption(contactPhoneSelect2, phoneType);
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

            Wait(5000);
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
        public void VerifyIndividualContactView(IndividualContact contact)
        {
            Wait();
            Assert.True(webDriver.FindElement(contactTitle).Displayed);
            Assert.True(webDriver.FindElement(contactEditButton).Displayed);

            Assert.True(webDriver.FindElement(contactIndFullName).Text.Equals(contact.FullName));
            Assert.True(webDriver.FindElement(contactIndPrefNameLabel).Displayed);
            Assert.True(webDriver.FindElement(contactIndPrefNameContent).Text.Equals(contact.PreferableName));
            Assert.True(webDriver.FindElement(contactIndStatusSpan).Text.Equals(contact.Status));

            Assert.True(webDriver.FindElement(contactIndOrganizationLabel).Displayed);
            if (contact.Organization != "")
            {
                Assert.True(webDriver.FindElement(contactIndOrganizationContent).Text.Equals(contact.Organization));
            }
 
            Assert.True(webDriver.FindElement(contactInfoSubtitle).Displayed);
            Assert.True(webDriver.FindElement(contactEmailLabel).Displayed);           
            if (contact.Email1 != "")
            {
                Assert.True(webDriver.FindElement(contactEmail1Content).Text.Equals(contact.Email1));
                Assert.True(webDriver.FindElement(contactEmailType1Content).Text.Equals(contact.EmailTypeDisplay1));
            }
            if (contact.Email2 != "")
            {
                Assert.True(webDriver.FindElement(contactEmail2Content).Text.Equals(contact.Email2));
                Assert.True(webDriver.FindElement(contactEmailType2Content).Text.Equals(contact.EmailTypeDisplay2));
            }
            Assert.True(webDriver.FindElement(contactPhoneLabel).Displayed);
            
            if (contact.Phone1 != "")
            {
                Assert.True(webDriver.FindElement(contactPhone1Content).Text.Equals(contact.Phone1));
                Assert.True(webDriver.FindElement(contactPhoneType1Content).Text.Equals(contact.PhoneTypeDisplay1));
            }
            if (contact.Phone2 != "")
            {
                Assert.True(webDriver.FindElement(contactPhone2Content).Text.Equals(contact.Phone2));
                Assert.True(webDriver.FindElement(contactPhoneType2Content).Text.Equals(contact.PhoneTypeDisplay2));
            }

            Assert.True(webDriver.FindElement(contactAddressSubtitle).Displayed);
            if (contact.MailAddressLine1 != "")
            {
                Assert.True(webDriver.FindElement(contactAddressMailSubtitle).Displayed);

                var mailAddressTotalLines = webDriver.FindElements(contactAddressIndMailCounter).Count;
                switch (mailAddressTotalLines)
                {
                    case 4:
                        Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine1).Text.Equals(contact.MailAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine2).Text.Equals(contact.MailCityProvinceView));
                        Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine3).Text.Equals(contact.MailPostalCode));
                        if (contact.MailCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine4).Text.Equals(contact.MailOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine4).Text.Equals(contact.MailCountry));
                        }
                        break;
                    case 5:
                        Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine1).Text.Equals(contact.MailAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine2).Text.Equals(contact.MailAddressLine2));
                        Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine3).Text.Equals(contact.MailCityProvinceView));
                        Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine4).Text.Equals(contact.MailPostalCode));
                        if (contact.MailCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine5).Text.Equals(contact.MailOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine5).Text.Equals(contact.MailCountry));
                        }
                        break;
                    case 6:
                        Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine1).Text.Equals(contact.MailAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine2).Text.Equals(contact.MailAddressLine2));
                        Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine3).Text.Equals(contact.MailAddressLine3));
                        Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine4).Text.Equals(contact.MailCityProvinceView));
                        Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine5).Text.Equals(contact.MailPostalCode));
                        if (contact.MailCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine6).Text.Equals(contact.MailOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine6).Text.Equals(contact.MailCountry));
                        }
                        break;
                }
            }
            if (contact.PropertyAddressLine1 != "")
            {
                Assert.True(webDriver.FindElement(contactAddressPropertySubtitle).Displayed);

                var propertyAddressTotalLines = webDriver.FindElements(contactAddressIndPropertyCounter).Count;
                switch (propertyAddressTotalLines)
                {
                    case 4:
                        Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine1).Text.Equals(contact.PropertyAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine2).Text.Equals(contact.PropertyCityProvinceView));
                        Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine3).Text.Equals(contact.PropertyPostalCode));
                        if (contact.PropertyCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine4).Text.Equals(contact.PropertyOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine4).Text.Equals(contact.PropertyCountry));
                        }
                        break;
                    case 5:
                        Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine1).Text.Equals(contact.PropertyAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine2).Text.Equals(contact.PropertyAddressLine2));
                        Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine3).Text.Equals(contact.PropertyCityProvinceView));
                        Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine4).Text.Equals(contact.PropertyPostalCode));
                        if (contact.PropertyCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine5).Text.Equals(contact.PropertyOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine5).Text.Equals(contact.PropertyCountry));
                        }
                        break;
                    case 6:
                        Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine1).Text.Equals(contact.PropertyAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine2).Text.Equals(contact.PropertyAddressLine2));
                        Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine3).Text.Equals(contact.PropertyAddressLine3));
                        Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine4).Text.Equals(contact.PropertyCityProvinceView));
                        Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine5).Text.Equals(contact.PropertyPostalCode));
                        if (contact.PropertyCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine6).Text.Equals(contact.PropertyOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine6).Text.Equals(contact.PropertyCountry));
                        }
                        break;
                }
            }
            if (contact.BillingAddressLine1 != "")
            {
                Assert.True(webDriver.FindElement(contactAddressBillingSubtitle).Displayed);

                var propertyAddressTotalLines = webDriver.FindElements(contactAddressIndBillingCounter).Count;
                switch (propertyAddressTotalLines)
                {
                    case 4:
                        Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine1).Text.Equals(contact.BillingAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine2).Text.Equals(contact.BillingCityProvinceView));
                        Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine3).Text.Equals(contact.BillingPostalCode));
                        if (contact.BillingCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine4).Text.Equals(contact.BillingOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine4).Text.Equals(contact.BillingCountry));
                        }
                        break;
                    case 5:
                        Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine1).Text.Equals(contact.BillingAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine2).Text.Equals(contact.BillingAddressLine2));
                        Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine3).Text.Equals(contact.BillingCityProvinceView));
                        Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine4).Text.Equals(contact.BillingPostalCode));
                        if (contact.BillingCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine5).Text.Equals(contact.BillingOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine5).Text.Equals(contact.BillingCountry));
                        }
                        break;
                    case 6:
                        Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine1).Text.Equals(contact.BillingAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine2).Text.Equals(contact.BillingAddressLine2));
                        Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine3).Text.Equals(contact.BillingAddressLine3));
                        Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine4).Text.Equals(contact.BillingCityProvinceView));
                        Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine5).Text.Equals(contact.BillingPostalCode));
                        if (contact.BillingCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine6).Text.Equals(contact.BillingOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine6).Text.Equals(contact.BillingCountry));
                        }
                        break;
                }
            }
            Assert.True(webDriver.FindElement(commentsSubtitle).Displayed);
            Assert.True(webDriver.FindElement(commentsIndividualContent).Text.Equals(contact.Comments));
        }

        public void VerifyOrganizationContactView(OrganizationContact contact)
        {
            Wait();
            Assert.True(webDriver.FindElement(contactTitle).Displayed);
            Assert.True(webDriver.FindElement(contactEditButton).Displayed);

            Assert.True(webDriver.FindElement(contactOrgName).Text.Equals(contact.OrganizationName));
            Assert.True(webDriver.FindElement(contactOrgAliasLabel).Displayed);
            Assert.True(webDriver.FindElement(contactOrgAliasContent).Text.Equals(contact.Alias));
            Assert.True(webDriver.FindElement(contactOrgStatusSpan).Text.Equals(contact.Status));

            Assert.True(webDriver.FindElement(contactOrgIncorpNbrLabel).Displayed);
            Assert.True(webDriver.FindElement(contactOrgIncorpNbrContent).Text.Equals(contact.IncorporationNumber));

            Assert.True(webDriver.FindElement(contactInfoSubtitle).Displayed);
            Assert.True(webDriver.FindElement(contactEmailLabel).Displayed);
            if (contact.Email1 != "")
            {
                Assert.True(webDriver.FindElement(contactEmail1Content).Text.Equals(contact.Email1));
                Assert.True(webDriver.FindElement(contactEmailType1Content).Text.Equals(contact.EmailTypeDisplay1));
            }
            if (contact.Email2 != "")
            {
                Assert.True(webDriver.FindElement(contactEmail2Content).Text.Equals(contact.Email2));
                Assert.True(webDriver.FindElement(contactEmailType2Content).Text.Equals(contact.EmailTypeDisplay2));
            }

            Assert.True(webDriver.FindElement(contactPhoneLabel).Displayed);
            if (contact.Phone1 != "")
            {
                Assert.True(webDriver.FindElement(contactPhone1Content).Text.Equals(contact.Phone1));
                Assert.True(webDriver.FindElement(contactPhoneType1Content).Text.Equals(contact.PhoneTypeDisplay1));
            }
            if (contact.Phone2 != "")
            {
                Assert.True(webDriver.FindElement(contactPhone2Content).Text.Equals(contact.Phone2));
                Assert.True(webDriver.FindElement(contactPhoneType2Content).Text.Equals(contact.PhoneTypeDisplay2));
            }

            Assert.True(webDriver.FindElement(contactAddressSubtitle).Displayed);
            if (contact.MailAddressLine1 != "")
            {
                Assert.True(webDriver.FindElement(contactAddressMailSubtitle).Displayed);

                var mailAddressTotalLines = webDriver.FindElements(contactAddressOrgMailCounter).Count;
                switch (mailAddressTotalLines)
                {
                    case 4:
                        Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine1).Text.Equals(contact.MailAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine2).Text.Equals(contact.MailCityProvinceView));
                        Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine3).Text.Equals(contact.MailPostalCode));
                        if (contact.MailCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine4).Text.Equals(contact.MailOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine4).Text.Equals(contact.MailCountry));
                        }
                        break;
                    case 5:
                        Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine1).Text.Equals(contact.MailAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine2).Text.Equals(contact.MailAddressLine2));
                        Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine3).Text.Equals(contact.MailCityProvinceView));
                        Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine4).Text.Equals(contact.MailPostalCode));
                        if (contact.MailCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine5).Text.Equals(contact.MailOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine5).Text.Equals(contact.MailCountry));
                        }
                        break;
                    case 6:
                        Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine1).Text.Equals(contact.MailAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine2).Text.Equals(contact.MailAddressLine2));
                        Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine3).Text.Equals(contact.MailAddressLine3));
                        Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine4).Text.Equals(contact.MailCityProvinceView));
                        Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine5).Text.Equals(contact.MailPostalCode));
                        if (contact.MailCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine6).Text.Equals(contact.MailOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine6).Text.Equals(contact.MailCountry));
                        }
                        break;
                }
            }
            if (contact.PropertyAddressLine1 != "")
            {
                Assert.True(webDriver.FindElement(contactAddressPropertySubtitle).Displayed);

                var propertyAddressTotalLines = webDriver.FindElements(contactAddressOrgPropertyCounter).Count;
                switch (propertyAddressTotalLines)
                {
                    case 4:
                        Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine1).Text.Equals(contact.PropertyAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine2).Text.Equals(contact.PropertyCityProvinceView));
                        Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine3).Text.Equals(contact.PropertyPostalCode));
                        if (contact.PropertyCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine4).Text.Equals(contact.PropertyOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine4).Text.Equals(contact.PropertyCountry));
                        }
                        break;
                    case 5:
                        Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine1).Text.Equals(contact.PropertyAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine2).Text.Equals(contact.PropertyAddressLine2));
                        Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine3).Text.Equals(contact.PropertyCityProvinceView));
                        Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine4).Text.Equals(contact.PropertyPostalCode));
                        if (contact.PropertyCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine5).Text.Equals(contact.PropertyOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine5).Text.Equals(contact.PropertyCountry));
                        }
                        break;
                    case 6:
                        Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine1).Text.Equals(contact.PropertyAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine2).Text.Equals(contact.PropertyAddressLine2));
                        Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine3).Text.Equals(contact.PropertyAddressLine3));
                        Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine4).Text.Equals(contact.PropertyCityProvinceView));
                        Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine5).Text.Equals(contact.PropertyPostalCode));
                        if (contact.PropertyCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine6).Text.Equals(contact.PropertyOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine6).Text.Equals(contact.PropertyCountry));
                        }
                        break;
                }
            }
            if (contact.BillingAddressLine1 != "")
            {
                Assert.True(webDriver.FindElement(contactAddressBillingSubtitle).Displayed);

                var propertyAddressTotalLines = webDriver.FindElements(contactAddressOrgBillingCounter).Count;
                switch (propertyAddressTotalLines)
                {
                    case 4:
                        Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine1).Text.Equals(contact.BillingAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine2).Text.Equals(contact.BillingCityProvinceView));
                        Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine3).Text.Equals(contact.BillingPostalCode));
                        if (contact.BillingCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine4).Text.Equals(contact.BillingOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine4).Text.Equals(contact.BillingCountry));
                        }
                        break;
                    case 5:
                        Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine1).Text.Equals(contact.BillingAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine2).Text.Equals(contact.BillingAddressLine2));
                        Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine3).Text.Equals(contact.BillingCityProvinceView));
                        Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine4).Text.Equals(contact.BillingPostalCode));
                        if (contact.BillingCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine5).Text.Equals(contact.BillingOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine5).Text.Equals(contact.BillingCountry));
                        }
                        break;
                    case 6:
                        Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine1).Text.Equals(contact.BillingAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine2).Text.Equals(contact.BillingAddressLine2));
                        Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine3).Text.Equals(contact.BillingAddressLine3));
                        Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine4).Text.Equals(contact.BillingCityProvinceView));
                        Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine5).Text.Equals(contact.BillingPostalCode));
                        if (contact.BillingCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine6).Text.Equals(contact.BillingOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine6).Text.Equals(contact.BillingCountry));
                        }
                        break;
                }
            }
            Assert.True(webDriver.FindElement(commentsSubtitle).Displayed);
            Assert.True(webDriver.FindElement(commentsOrganizationContent).Text.Equals(contact.Comments));
        }

    }
}
