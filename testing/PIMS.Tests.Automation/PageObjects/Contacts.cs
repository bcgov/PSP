using OpenQA.Selenium;

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
        public void CreateIndividualContact(string firstName, string middleName, string lastName, string prefName, string orgName, string email1, string emailType1, string phone1, string phoneType1,
            string mailAddressLine1, string mailAddressLine2, string mailAddressLine3, string mailCity, string mailProvince, string mailPostalCode, string mailCountry, string mailOtherCountry, 
            string propertyAddressLine1, string propertyAddressLine2, string propertyAddressLine3, string propertyCity, string propertyProvince, string propertyPostalCode, string propertyCountry, string propertyOtherCountry, 
            string billingAddressLine1, string billingAddressLine2, string billingAddressLine3, string billingCity, string billingProvince, string billingPostalCode, string billingCountry, string billingOtherCountry,
            string comments)
        {
            Wait();

            //Choosing individual contact option
            FocusAndClick(contactIndividualRadioBttn);

            //Inserting individual personal details
            webDriver.FindElement(contactIndFirstNameInput).SendKeys(firstName);
            webDriver.FindElement(contactIndMiddleNameInput).SendKeys(middleName);
            webDriver.FindElement(contactIndLastNameInput).SendKeys(lastName);
            webDriver.FindElement(contactIndPrefNameInput).SendKeys(prefName);
            if (orgName != "")
            {
                webDriver.FindElement(contactIndOrgInput).SendKeys(orgName);

                Wait();
                webDriver.FindElement(contactOrgNameSelect).Click();
            }
            //Inserting contact info
            if (email1 != "")
            {
                webDriver.FindElement(contactEmailInput1).SendKeys(email1);
                ChooseSpecificSelectOption(contactEmailSelect1, emailType1);
            }
            if (phone1 != "")
            {
                webDriver.FindElement(contactPhoneInput1).SendKeys(phone1);
                ChooseSpecificSelectOption(contactPhoneSelect1, phoneType1);
            }

            //Inserting contact mail address
            if (mailAddressLine1 != "")
            {
                webDriver.FindElement(contactMailAddressLine1Input).SendKeys(mailAddressLine1);
                if (mailAddressLine2 != "")
                {
                    webDriver.FindElement(contactMailAddAddressLineBttn).Click();
                    webDriver.FindElement(contactMailAddressLine2Input).SendKeys(mailAddressLine2);
                }
                if (mailAddressLine3 != "")
                {
                    webDriver.FindElement(contactMailAddAddressLineBttn).Click();
                    webDriver.FindElement(contactMailAddressLine3Input).SendKeys(mailAddressLine3);
                }

                ChooseSpecificSelectOption(contactMailCountrySelect, mailCountry);
                if (mailCountry == "Other")
                {
                    webDriver.FindElement(contactMailOtherCountryInput).SendKeys(mailOtherCountry);
                }
                else
                {
                    ChooseSpecificSelectOption(contactMailProvinceSelect, mailProvince);
                }
                webDriver.FindElement(contactMailCityInput).SendKeys(mailCity);
                webDriver.FindElement(contactMailPostalCodeInput).SendKeys(mailPostalCode);
            }


            //Inserting contact property address
            if (propertyAddressLine1 != "")
            {
                webDriver.FindElement(contactPropertyAddressLine1Input).SendKeys(propertyAddressLine1);
                if (propertyAddressLine2 != "")
                {
                    webDriver.FindElement(contactPropertyAddAddressLineBttn).Click();
                    webDriver.FindElement(contactPropertyAddressLine2Input).SendKeys(propertyAddressLine2);
                }
                if (propertyAddressLine3 != "")
                {
                    webDriver.FindElement(contactPropertyAddAddressLineBttn).Click();
                    webDriver.FindElement(contactPropertyAddressLine3Input).SendKeys(propertyAddressLine3);
                }

                ChooseSpecificSelectOption(contactPropertyCountrySelect, propertyCountry);
                if (propertyCountry == "Other")
                {
                    webDriver.FindElement(contactPropertyOtherCountryInput).SendKeys(propertyOtherCountry);
                }
                else
                {
                    ChooseSpecificSelectOption(contactPropertyProvinceSelect, propertyProvince);
                }
                webDriver.FindElement(contactPropertyCityInput).SendKeys(propertyCity);
                webDriver.FindElement(contactPropertyPostalCodeInput).SendKeys(propertyPostalCode);
            }

            //Inserting contact billing address
            if (billingAddressLine1 != "")
            {
                webDriver.FindElement(contactBillingAddressLine1Input).SendKeys(billingAddressLine1);
                if (billingAddressLine2 != "")
                {
                    webDriver.FindElement(contactBillingAddAddressLineBttn).Click();
                    webDriver.FindElement(contactBillingAddressLine2Input).SendKeys(billingAddressLine2);
                }
                if (billingAddressLine3 != "")
                {
                    webDriver.FindElement(contactBillingAddAddressLineBttn).Click();
                    webDriver.FindElement(contactBillingAddressLine3Input).SendKeys(billingAddressLine3);
                }
                ChooseSpecificSelectOption(contactBillingCountrySelect, billingCountry);
                if (billingCountry == "Other")
                {
                    webDriver.FindElement(contactBillingOtherCountryInput).SendKeys(billingOtherCountry);
                }
                else
                {
                    ChooseSpecificSelectOption(contactBillingProvinceSelect, billingProvince);
                }
                webDriver.FindElement(contactBillingCityInput).SendKeys(billingCity);
                webDriver.FindElement(contactBillingPostalCodeInput).SendKeys(billingPostalCode);
            }

            //Inserting comments
            webDriver.FindElement(contactCommentTextarea).SendKeys(comments);
        }

        //Creates Organization Contact with all fields
        public void CreateOrganizationContact(string orgName, string alias, string incNumber,
            string email1, string emailType1, string email2, string emailType2, string phone1, string phoneType1, string phone2, string phoneType2,
            string mailAddressLine1, string mailAddressLine2, string mailAddressLine3, string mailCity, string mailProvince, string mailPostalCode, string mailCountry, string mailOtherCountry,
            string propertyAddressLine1, string propertyAddressLine2, string propertyAddressLine3, string propertyCity, string propertyProvince, string propertyPostalCode, string propertyCountry, string propertyOtherCountry,
            string billingAddressLine1, string billingAddressLine2, string billingAddressLine3, string billingCity, string billingProvince, string billingPostalCode, string billingCountry, string billingOtherCountry,
            string comments)
        {
            Wait();

            //Choosing organization contact option
            FocusAndClick(contactOrganizationRadioBttn);

            //Inserting individual personal details
            webDriver.FindElement(contactOrgNameInput).SendKeys(orgName);
            if (alias != "")
            {
                webDriver.FindElement(contactOrgAliasInput).SendKeys(alias);
            }
            if(incNumber != "")
            {
                webDriver.FindElement(contactOrgIncNbrInput).SendKeys(incNumber);
            }

            //Inserting contact info
            if (email1 != "")
            {
                webDriver.FindElement(contactEmailInput1).SendKeys(email1);
                ChooseSpecificSelectOption(contactEmailSelect1, emailType1);
            }
            if (phone1 != "")
            {
                webDriver.FindElement(contactPhoneInput1).SendKeys(phone1);
                ChooseSpecificSelectOption(contactPhoneSelect1, phoneType1);
            }

            //Inserting contact mail address
            if (mailAddressLine1 != "")
            {
                webDriver.FindElement(contactMailAddressLine1Input).SendKeys(mailAddressLine1);
                if (mailAddressLine2 != "")
                {
                    webDriver.FindElement(contactMailAddAddressLineBttn).Click();
                    webDriver.FindElement(contactMailAddressLine2Input).SendKeys(mailAddressLine2);
                }
                if (mailAddressLine3 != "")
                {
                    webDriver.FindElement(contactMailAddAddressLineBttn).Click();
                    webDriver.FindElement(contactMailAddressLine3Input).SendKeys(mailAddressLine3);
                }

                ChooseSpecificSelectOption(contactMailCountrySelect, mailCountry);
                if (mailCountry == "Other")
                {
                    webDriver.FindElement(contactMailOtherCountryInput).SendKeys(mailOtherCountry);
                }
                else
                {
                    ChooseSpecificSelectOption(contactMailProvinceSelect, mailProvince);
                }
                webDriver.FindElement(contactMailCityInput).SendKeys(mailCity);
                webDriver.FindElement(contactMailPostalCodeInput).SendKeys(mailPostalCode);
            }

            //Inserting contact property address
            if (propertyAddressLine1 != "")
            {
                webDriver.FindElement(contactPropertyAddressLine1Input).SendKeys(propertyAddressLine1);
                if (propertyAddressLine2 != "")
                {
                    webDriver.FindElement(contactPropertyAddAddressLineBttn).Click();
                    webDriver.FindElement(contactPropertyAddressLine2Input).SendKeys(propertyAddressLine2);
                }
                if (propertyAddressLine3 != "")
                {
                    webDriver.FindElement(contactPropertyAddAddressLineBttn).Click();
                    webDriver.FindElement(contactPropertyAddressLine3Input).SendKeys(propertyAddressLine3);
                }

                ChooseSpecificSelectOption(contactPropertyCountrySelect, propertyCountry);
                if (propertyCountry == "Other")
                {
                    webDriver.FindElement(contactPropertyOtherCountryInput).SendKeys(propertyOtherCountry);
                }
                else
                {
                    ChooseSpecificSelectOption(contactPropertyProvinceSelect, propertyProvince);
                }
                webDriver.FindElement(contactPropertyCityInput).SendKeys(propertyCity);
                webDriver.FindElement(contactPropertyPostalCodeInput).SendKeys(propertyPostalCode);
            }

            //Inserting contact billing address
            if (billingAddressLine1 != "")
            {
                webDriver.FindElement(contactBillingAddressLine1Input).SendKeys(billingAddressLine1);
                if (billingAddressLine2 != "")
                {
                    webDriver.FindElement(contactBillingAddAddressLineBttn).Click();
                    webDriver.FindElement(contactBillingAddressLine2Input).SendKeys(billingAddressLine2);
                }
                if (billingAddressLine3 != "")
                {
                    webDriver.FindElement(contactBillingAddAddressLineBttn).Click();
                    webDriver.FindElement(contactBillingAddressLine3Input).SendKeys(billingAddressLine3);
                }
                ChooseSpecificSelectOption(contactBillingCountrySelect, billingCountry);
                if (billingCountry == "Other")
                {
                    webDriver.FindElement(contactBillingOtherCountryInput).SendKeys(billingOtherCountry);
                }
                else
                {
                    ChooseSpecificSelectOption(contactBillingProvinceSelect, billingProvince);
                }
                webDriver.FindElement(contactBillingCityInput).SendKeys(billingCity);
                webDriver.FindElement(contactBillingPostalCodeInput).SendKeys(billingPostalCode);
            }

            //Inserting comments
            webDriver.FindElement(contactCommentTextarea).SendKeys(comments);
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
        public void VerifyIndividualContactView(string fullName, string preferredName, string status, string organization,
            string email1, string emailTypeDisplay1, string email2, string emailTypeDisplay2, string phone1, string phoneTypeDisplay1, string phone2, string phoneTypeDisplay2,
            string mailAddressLine1, string mailAddressLine2, string mailAddressLine3, string mailCityProvince, string mailPostalCode, string mailCountry, string mailOtherCountry,
            string propertyAddressLine1, string propertyAddressLine2, string propertyAddressLine3, string propertyCityProvince, string propertyPostalCode, string propertyCountry, string propertyOtherCountry,
            string billingAddressLine1, string billingAddressLine2, string billingAddressLine3, string billingCityProvince, string billingPostalCode, string billingCountry, string billingOtherCountry,
            string comments)
        {
            Wait();
            Assert.True(webDriver.FindElement(contactTitle).Displayed);
            Assert.True(webDriver.FindElement(contactEditButton).Displayed);

            Assert.True(webDriver.FindElement(contactIndFullName).Text.Equals(fullName));
            Assert.True(webDriver.FindElement(contactIndPrefNameLabel).Displayed);
            Assert.True(webDriver.FindElement(contactIndPrefNameContent).Text.Equals(preferredName));
            Assert.True(webDriver.FindElement(contactIndStatusSpan).Text.Equals(status));

            Assert.True(webDriver.FindElement(contactIndOrganizationLabel).Displayed);
            if (organization != "")
            {
                Assert.True(webDriver.FindElement(contactIndOrganizationContent).Text.Equals(organization));
            }
 
            Assert.True(webDriver.FindElement(contactInfoSubtitle).Displayed);
            Assert.True(webDriver.FindElement(contactEmailLabel).Displayed);           
            if (email1 != "")
            {
                Assert.True(webDriver.FindElement(contactEmail1Content).Text.Equals(email1));
                Assert.True(webDriver.FindElement(contactEmailType1Content).Text.Equals(emailTypeDisplay1));
            }
            if (email2 != "")
            {
                Assert.True(webDriver.FindElement(contactEmail2Content).Text.Equals(email2));
                Assert.True(webDriver.FindElement(contactEmailType2Content).Text.Equals(emailTypeDisplay2));
            }
            Assert.True(webDriver.FindElement(contactPhoneLabel).Displayed);
            
            if (phone1 != "")
            {
                Assert.True(webDriver.FindElement(contactPhone1Content).Text.Equals(phone1));
                Assert.True(webDriver.FindElement(contactPhoneType1Content).Text.Equals(phoneTypeDisplay1));
            }
            if (phone2 != "")
            {
                Assert.True(webDriver.FindElement(contactPhone2Content).Text.Equals(phone2));
                Assert.True(webDriver.FindElement(contactPhoneType2Content).Text.Equals(phoneTypeDisplay2));
            }

            Assert.True(webDriver.FindElement(contactAddressSubtitle).Displayed);
            if (mailAddressLine1 != "")
            {
                Assert.True(webDriver.FindElement(contactAddressMailSubtitle).Displayed);

                var mailAddressTotalLines = webDriver.FindElements(contactAddressIndMailCounter).Count;
                switch (mailAddressTotalLines)
                {
                    case 4:
                        Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine1).Text.Equals(mailAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine2).Text.Equals(mailCityProvince));
                        Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine3).Text.Equals(mailPostalCode));
                        if (mailCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine4).Text.Equals(mailOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine4).Text.Equals(mailCountry));
                        }
                        break;
                    case 5:
                        Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine1).Text.Equals(mailAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine2).Text.Equals(mailAddressLine2));
                        Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine3).Text.Equals(mailCityProvince));
                        Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine4).Text.Equals(mailPostalCode));
                        if (mailCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine5).Text.Equals(mailOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine5).Text.Equals(mailCountry));
                        }
                        break;
                    case 6:
                        Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine1).Text.Equals(mailAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine2).Text.Equals(mailAddressLine2));
                        Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine3).Text.Equals(mailAddressLine3));
                        Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine4).Text.Equals(mailCityProvince));
                        Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine5).Text.Equals(mailPostalCode));
                        if (mailCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine6).Text.Equals(mailOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine6).Text.Equals(mailCountry));
                        }
                        break;
                }
            }
            if (propertyAddressLine1 != "")
            {
                Assert.True(webDriver.FindElement(contactAddressPropertySubtitle).Displayed);

                var propertyAddressTotalLines = webDriver.FindElements(contactAddressIndPropertyCounter).Count;
                switch (propertyAddressTotalLines)
                {
                    case 4:
                        Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine1).Text.Equals(propertyAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine2).Text.Equals(propertyCityProvince));
                        Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine3).Text.Equals(propertyPostalCode));
                        if (propertyCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine4).Text.Equals(propertyOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine4).Text.Equals(propertyCountry));
                        }
                        break;
                    case 5:
                        Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine1).Text.Equals(propertyAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine2).Text.Equals(propertyAddressLine2));
                        Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine3).Text.Equals(propertyCityProvince));
                        Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine4).Text.Equals(propertyPostalCode));
                        if (propertyCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine5).Text.Equals(propertyOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine5).Text.Equals(propertyCountry));
                        }
                        break;
                    case 6:
                        Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine1).Text.Equals(propertyAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine2).Text.Equals(propertyAddressLine2));
                        Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine3).Text.Equals(propertyAddressLine3));
                        Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine4).Text.Equals(propertyCityProvince));
                        Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine5).Text.Equals(propertyPostalCode));
                        if (propertyCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine6).Text.Equals(propertyOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine6).Text.Equals(propertyCountry));
                        }
                        break;
                }
            }
            if (billingAddressLine1 != "")
            {
                Assert.True(webDriver.FindElement(contactAddressBillingSubtitle).Displayed);

                var propertyAddressTotalLines = webDriver.FindElements(contactAddressIndBillingCounter).Count;
                switch (propertyAddressTotalLines)
                {
                    case 4:
                        Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine1).Text.Equals(billingAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine2).Text.Equals(billingCityProvince));
                        Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine3).Text.Equals(billingPostalCode));
                        if (billingCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine4).Text.Equals(billingOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine4).Text.Equals(billingCountry));
                        }
                        break;
                    case 5:
                        Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine1).Text.Equals(billingAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine2).Text.Equals(billingAddressLine2));
                        Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine3).Text.Equals(billingCityProvince));
                        Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine4).Text.Equals(billingPostalCode));
                        if (billingCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine5).Text.Equals(billingOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine5).Text.Equals(billingCountry));
                        }
                        break;
                    case 6:
                        Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine1).Text.Equals(billingAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine2).Text.Equals(billingAddressLine2));
                        Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine3).Text.Equals(billingAddressLine3));
                        Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine4).Text.Equals(billingCityProvince));
                        Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine5).Text.Equals(billingPostalCode));
                        if (billingCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine6).Text.Equals(billingOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine6).Text.Equals(billingCountry));
                        }
                        break;
                }
            }
            Assert.True(webDriver.FindElement(commentsSubtitle).Displayed);
            Assert.True(webDriver.FindElement(commentsIndividualContent).Text.Equals(comments));
        }

        public void VerifyOrganizationContactView(string organizationName, string alias, string status, string incorporationNbr,
            string email1, string emailTypeDisplay1, string email2, string emailTypeDisplay2, string phone1, string phoneTypeDisplay1, string phone2, string phoneTypeDisplay2,
            string mailAddressLine1, string mailAddressLine2, string mailAddressLine3, string mailCityProvince, string mailPostalCode, string mailCountry, string mailOtherCountry,
            string propertyAddressLine1, string propertyAddressLine2, string propertyAddressLine3, string propertyCityProvince, string propertyPostalCode, string propertyCountry, string propertyOtherCountry,
            string billingAddressLine1, string billingAddressLine2, string billingAddressLine3, string billingCityProvince, string billingPostalCode, string billingCountry, string billingOtherCountry,
            string comments)
        {
            Wait();
            Assert.True(webDriver.FindElement(contactTitle).Displayed);
            Assert.True(webDriver.FindElement(contactEditButton).Displayed);

            Assert.True(webDriver.FindElement(contactOrgName).Text.Equals(organizationName));
            Assert.True(webDriver.FindElement(contactOrgAliasLabel).Displayed);
            Assert.True(webDriver.FindElement(contactOrgAliasContent).Text.Equals(alias));
            Assert.True(webDriver.FindElement(contactOrgStatusSpan).Text.Equals(status));

            Assert.True(webDriver.FindElement(contactOrgIncorpNbrLabel).Displayed);
            Assert.True(webDriver.FindElement(contactOrgIncorpNbrContent).Text.Equals(incorporationNbr));

            Assert.True(webDriver.FindElement(contactInfoSubtitle).Displayed);
            Assert.True(webDriver.FindElement(contactEmailLabel).Displayed);
            if (email1 != "")
            {
                Assert.True(webDriver.FindElement(contactEmail1Content).Text.Equals(email1));
                Assert.True(webDriver.FindElement(contactEmailType1Content).Text.Equals(emailTypeDisplay1));
            }
            if (email2 != "")
            {
                Assert.True(webDriver.FindElement(contactEmail2Content).Text.Equals(email2));
                Assert.True(webDriver.FindElement(contactEmailType2Content).Text.Equals(emailTypeDisplay2));
            }

            Assert.True(webDriver.FindElement(contactPhoneLabel).Displayed);
            if (phone1 != "")
            {
                Assert.True(webDriver.FindElement(contactPhone1Content).Text.Equals(phone1));
                Assert.True(webDriver.FindElement(contactPhoneType1Content).Text.Equals(phoneTypeDisplay1));
            }
            if (phone2 != "")
            {
                Assert.True(webDriver.FindElement(contactPhone2Content).Text.Equals(phone2));
                Assert.True(webDriver.FindElement(contactPhoneType2Content).Text.Equals(phoneTypeDisplay2));
            }

            Assert.True(webDriver.FindElement(contactAddressSubtitle).Displayed);
            if (mailAddressLine1 != "")
            {
                Assert.True(webDriver.FindElement(contactAddressMailSubtitle).Displayed);

                var mailAddressTotalLines = webDriver.FindElements(contactAddressOrgMailCounter).Count;
                switch (mailAddressTotalLines)
                {
                    case 4:
                        Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine1).Text.Equals(mailAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine2).Text.Equals(mailCityProvince));
                        Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine3).Text.Equals(mailPostalCode));
                        if (mailCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine4).Text.Equals(mailOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine4).Text.Equals(mailCountry));
                        }
                        break;
                    case 5:
                        Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine1).Text.Equals(mailAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine2).Text.Equals(mailAddressLine2));
                        Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine3).Text.Equals(mailCityProvince));
                        Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine4).Text.Equals(mailPostalCode));
                        if (mailCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine5).Text.Equals(mailOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine5).Text.Equals(mailCountry));
                        }
                        break;
                    case 6:
                        Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine1).Text.Equals(mailAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine2).Text.Equals(mailAddressLine2));
                        Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine3).Text.Equals(mailAddressLine3));
                        Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine4).Text.Equals(mailCityProvince));
                        Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine5).Text.Equals(mailPostalCode));
                        if (mailCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine6).Text.Equals(mailOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine6).Text.Equals(mailCountry));
                        }
                        break;
                }
            }
            if (propertyAddressLine1 != "")
            {
                Assert.True(webDriver.FindElement(contactAddressPropertySubtitle).Displayed);

                var propertyAddressTotalLines = webDriver.FindElements(contactAddressOrgPropertyCounter).Count;
                switch (propertyAddressTotalLines)
                {
                    case 4:
                        Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine1).Text.Equals(propertyAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine2).Text.Equals(propertyCityProvince));
                        Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine3).Text.Equals(propertyPostalCode));
                        if (propertyCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine4).Text.Equals(propertyOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine4).Text.Equals(propertyCountry));
                        }
                        break;
                    case 5:
                        Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine1).Text.Equals(propertyAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine2).Text.Equals(propertyAddressLine2));
                        Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine3).Text.Equals(propertyCityProvince));
                        Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine4).Text.Equals(propertyPostalCode));
                        if (propertyCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine5).Text.Equals(propertyOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine5).Text.Equals(propertyCountry));
                        }
                        break;
                    case 6:
                        Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine1).Text.Equals(propertyAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine2).Text.Equals(propertyAddressLine2));
                        Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine3).Text.Equals(propertyAddressLine3));
                        Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine4).Text.Equals(propertyCityProvince));
                        Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine5).Text.Equals(propertyPostalCode));
                        if (propertyCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine6).Text.Equals(propertyOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine6).Text.Equals(propertyCountry));
                        }
                        break;
                }
            }
            if (billingAddressLine1 != "")
            {
                Assert.True(webDriver.FindElement(contactAddressBillingSubtitle).Displayed);

                var propertyAddressTotalLines = webDriver.FindElements(contactAddressOrgBillingCounter).Count;
                switch (propertyAddressTotalLines)
                {
                    case 4:
                        Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine1).Text.Equals(billingAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine2).Text.Equals(billingCityProvince));
                        Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine3).Text.Equals(billingPostalCode));
                        if (billingCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine4).Text.Equals(billingOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine4).Text.Equals(billingCountry));
                        }
                        break;
                    case 5:
                        Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine1).Text.Equals(billingAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine2).Text.Equals(billingAddressLine2));
                        Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine3).Text.Equals(billingCityProvince));
                        Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine4).Text.Equals(billingPostalCode));
                        if (billingCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine5).Text.Equals(billingOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine5).Text.Equals(billingCountry));
                        }
                        break;
                    case 6:
                        Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine1).Text.Equals(billingAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine2).Text.Equals(billingAddressLine2));
                        Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine3).Text.Equals(billingAddressLine3));
                        Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine4).Text.Equals(billingCityProvince));
                        Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine5).Text.Equals(billingPostalCode));
                        if (billingCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine6).Text.Equals(billingOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine6).Text.Equals(billingCountry));
                        }
                        break;
                }
            }
            Assert.True(webDriver.FindElement(commentsSubtitle).Displayed);
            Assert.True(webDriver.FindElement(commentsOrganizationContent).Text.Equals(comments));
        }

    }
}
