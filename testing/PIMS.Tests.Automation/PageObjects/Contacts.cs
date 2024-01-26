using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;
using System.Diagnostics.Contracts;

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
        private By contactOrgNameSelect = By.Id("typeahead-organization-item-0");

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
        private By contactAddressMailAddressRemoveBttn = By.XPath("//span[contains(text(),'Mailing Address')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div/div/div/div/button/div/span[contains(text(),'Remove')]/parent::div/parent::button");

        private By contactAddressOrgMailCounter = By.XPath("//div[1]/div[@data-testid='contact-organization-address']/div");
        private By contactAddressOrgMailAddressLine1 = By.XPath("//div[1]/div[@data-testid='contact-organization-address']/div[1]");
        private By contactAddressOrgMailAddressLine2 = By.XPath("//div[1]/div[@data-testid='contact-organization-address']/div[2]");
        private By contactAddressOrgMailAddressLine3 = By.XPath("//div[1]/div[@data-testid='contact-organization-address']/div[3]");
        private By contactAddressOrgMailAddressLine4 = By.XPath("//div[1]/div[@data-testid='contact-organization-address']/div[4]");
        private By contactAddressOrgMailAddressLine5 = By.XPath("//div[1]/div[@data-testid='contact-organization-address']/div[5]");
        private By contactAddressOrgMailAddressLine6 = By.XPath("//div[1]/div[@data-testid='contact-organization-address']/div[6]");
        private By contactAddressPropertyAddressRemoveBttn = By.XPath("//div[contains(text(),'Property Address')]/parent::div/parent::h2/following-sibling::div/div/div/div/div/button/div/span[contains(text(),'Remove')]/parent::div/parent::button");

        private By contactAddressPropertySubtitle = By.XPath("//strong[contains(text(),'Property address')]");
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
        private By contactModal = By.CssSelector("div[class='modal-content']");

        private By contactsSearchTable = By.CssSelector("div[data-testid='contactsTable']");
        private By contactModalContinueSaveBttn = By.XPath("//button/div[contains(text(),'Continue Save')]");
        //private By contactConfirmCancelBttn = By.XPath("//button/div[contains(text(),'Confirm')]");

        private SharedModals sharedModals;

        public Contacts(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
        }

        //Navigates to Create a new Contact
        public void NavigateToCreateNewContact()
        {
            Wait(3000);

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

                WaitUntilClickable(contactOrgNameSelect);
                webDriver.FindElement(contactOrgNameSelect).Click();
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
            if (contact.IndMailAddressLine1 != "")
            {
                webDriver.FindElement(contactMailAddressLine1Input).SendKeys(contact.IndMailAddressLine1);
                if (contact.IndMailAddressLine2 != "")
                {
                    webDriver.FindElement(contactIndMailAddAddressLineBttn).Click();
                    webDriver.FindElement(contactMailAddressLine2Input).SendKeys(contact.IndMailAddressLine2);
                }
                if (contact.IndMailAddressLine3 != "")
                {
                    webDriver.FindElement(contactIndMailAddAddressLineBttn).Click();
                    webDriver.FindElement(contactMailAddressLine3Input).SendKeys(contact.IndMailAddressLine3);
                }

                ChooseSpecificSelectOption(contactMailCountrySelect, contact.IndMailCountry);
                if (contact.IndMailCountry == "Other")
                {
                    webDriver.FindElement(contactMailOtherCountryInput).SendKeys(contact.IndMailOtherCountry);
                }
                else
                {
                    ChooseSpecificSelectOption(contactMailProvinceSelect, contact.IndMailProvince);
                }
                webDriver.FindElement(contactMailCityInput).SendKeys(contact.IndMailCity);
                webDriver.FindElement(contactMailPostalCodeInput).SendKeys(contact.IndMailPostalCode);
            }

            //Inserting contact property address
            if (contact.IndPropertyAddressLine1 != "")
            {
                webDriver.FindElement(contactPropertyAddressLine1Input).SendKeys(contact.IndPropertyAddressLine1);
                if (contact.IndPropertyAddressLine2 != "")
                {
                    webDriver.FindElement(contactPropertyAddAddressLineBttn).Click();
                    webDriver.FindElement(contactPropertyAddressLine2Input).SendKeys(contact.IndPropertyAddressLine2);
                }
                if (contact.IndPropertyAddressLine3 != "")
                {
                    webDriver.FindElement(contactPropertyAddAddressLineBttn).Click();
                    webDriver.FindElement(contactPropertyAddressLine3Input).SendKeys(contact.IndPropertyAddressLine3);
                }

                ChooseSpecificSelectOption(contactPropertyCountrySelect, contact.IndPropertyCountry);
                if (contact.IndPropertyCountry == "Other")
                {
                    webDriver.FindElement(contactPropertyOtherCountryInput).SendKeys(contact.IndPropertyOtherCountry);
                }
                else
                {
                    ChooseSpecificSelectOption(contactPropertyProvinceSelect, contact.IndPropertyProvince);
                }
                webDriver.FindElement(contactPropertyCityInput).SendKeys(contact.IndPropertyCity);
                webDriver.FindElement(contactPropertyPostalCodeInput).SendKeys(contact.IndPropertyPostalCode);
            }

            //Inserting contact billing address
            if (contact.IndBillingAddressLine1 != "")
            {
                webDriver.FindElement(contactBillingAddressLine1Input).SendKeys(contact.IndBillingAddressLine1);
                if (contact.IndBillingAddressLine2 != "")
                {
                    webDriver.FindElement(contactBillingAddAddressLineBttn).Click();
                    webDriver.FindElement(contactBillingAddressLine2Input).SendKeys(contact.IndBillingAddressLine2);
                }
                if (contact.IndBillingAddressLine3 != "")
                {
                    webDriver.FindElement(contactBillingAddAddressLineBttn).Click();
                    webDriver.FindElement(contactBillingAddressLine3Input).SendKeys(contact.IndBillingAddressLine3);
                }
                ChooseSpecificSelectOption(contactBillingCountrySelect, contact.IndBillingCountry);
                if (contact.IndBillingCountry == "Other")
                {
                    webDriver.FindElement(contactBillingOtherCountryInput).SendKeys(contact.IndBillingOtherCountry);
                }
                else
                {
                    ChooseSpecificSelectOption(contactBillingProvinceSelect, contact.IndBillingProvince);
                }
                webDriver.FindElement(contactBillingCityInput).SendKeys(contact.IndBillingCity);
                webDriver.FindElement(contactBillingPostalCodeInput).SendKeys(contact.IndBillingPostalCode);
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
            if (contact.OrgMailAddressLine1 != "")
            {
                webDriver.FindElement(contactMailAddressLine1Input).SendKeys(contact.OrgMailAddressLine1);
                if (contact.OrgMailAddressLine2 != "")
                {
                    webDriver.FindElement(contactOrgMailAddAddressLineBttn).Click();
                    webDriver.FindElement(contactMailAddressLine2Input).SendKeys(contact.OrgMailAddressLine2);
                }
                if (contact.OrgMailAddressLine3 != "")
                {
                    webDriver.FindElement(contactOrgMailAddAddressLineBttn).Click();
                    webDriver.FindElement(contactMailAddressLine3Input).SendKeys(contact.OrgMailAddressLine3);
                }

                ChooseSpecificSelectOption(contactMailCountrySelect, contact.OrgMailCountry);
                if (contact.OrgMailCountry == "Other")
                {
                    webDriver.FindElement(contactMailOtherCountryInput).SendKeys(contact.OrgMailOtherCountry);
                }
                else
                {
                    ChooseSpecificSelectOption(contactMailProvinceSelect, contact.OrgMailProvince);
                }
                webDriver.FindElement(contactMailCityInput).SendKeys(contact.OrgMailCity);
                webDriver.FindElement(contactMailPostalCodeInput).SendKeys(contact.OrgMailPostalCode);
            }

            //Inserting contact property address
            if (contact.OrgPropertyAddressLine1 != "")
            {
                webDriver.FindElement(contactPropertyAddressLine1Input).SendKeys(contact.OrgPropertyAddressLine1);
                if (contact.OrgPropertyAddressLine2 != "")
                {
                    webDriver.FindElement(contactPropertyAddAddressLineBttn).Click();
                    webDriver.FindElement(contactPropertyAddressLine2Input).SendKeys(contact.OrgPropertyAddressLine2);
                }
                if (contact.OrgPropertyAddressLine3 != "")
                {
                    webDriver.FindElement(contactPropertyAddAddressLineBttn).Click();
                    webDriver.FindElement(contactPropertyAddressLine3Input).SendKeys(contact.OrgPropertyAddressLine3);
                }

                ChooseSpecificSelectOption(contactPropertyCountrySelect, contact.OrgPropertyCountry);
                if (contact.OrgPropertyCountry == "Other")
                {
                    webDriver.FindElement(contactPropertyOtherCountryInput).SendKeys(contact.OrgPropertyOtherCountry);
                }
                else
                {
                    ChooseSpecificSelectOption(contactPropertyProvinceSelect, contact.OrgPropertyProvince);
                }
                webDriver.FindElement(contactPropertyCityInput).SendKeys(contact.OrgPropertyCity);
                webDriver.FindElement(contactPropertyPostalCodeInput).SendKeys(contact.OrgPropertyPostalCode);
            }

            //Inserting contact billing address
            if (contact.OrgBillingAddressLine1 != "")
            {
                webDriver.FindElement(contactBillingAddressLine1Input).SendKeys(contact.OrgBillingAddressLine1);
                if (contact.OrgBillingAddressLine2 != "")
                {
                    webDriver.FindElement(contactBillingAddAddressLineBttn).Click();
                    webDriver.FindElement(contactBillingAddressLine2Input).SendKeys(contact.OrgBillingAddressLine2);
                }
                if (contact.OrgBillingAddressLine3 != "")
                {
                    webDriver.FindElement(contactBillingAddAddressLineBttn).Click();
                    webDriver.FindElement(contactBillingAddressLine3Input).SendKeys(contact.OrgBillingAddressLine3);
                }
                ChooseSpecificSelectOption(contactBillingCountrySelect, contact.OrgBillingCountry);
                if (contact.OrgBillingCountry == "Other")
                {
                    webDriver.FindElement(contactBillingOtherCountryInput).SendKeys(contact.OrgBillingOtherCountry);
                }
                else
                {
                    ChooseSpecificSelectOption(contactBillingProvinceSelect, contact.OrgBillingProvince);
                }
                webDriver.FindElement(contactBillingCityInput).SendKeys(contact.OrgBillingCity);
                webDriver.FindElement(contactBillingPostalCodeInput).SendKeys(contact.OrgBillingPostalCode);
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
            if (contact.OrgMailAddressLine1 != "")
            {
                while (webDriver.FindElements(contactAddressMailAddressRemoveBttn).Count > 0)
                {
                    FocusAndClick(contactAddressMailAddressRemoveBttn);
                }
                ClearInput(contactMailAddressLine1Input);

                webDriver.FindElement(contactMailAddressLine1Input).SendKeys(contact.OrgMailAddressLine1);

                if (contact.OrgMailAddressLine2 != "")
                {
                    webDriver.FindElement(contactIndMailAddAddressLineBttn).Click();
                    webDriver.FindElement(contactMailAddressLine2Input).SendKeys(contact.OrgMailAddressLine2);
                }
                if (contact.OrgMailAddressLine3 != "")
                {
                    webDriver.FindElement(contactIndMailAddAddressLineBttn).Click();
                    webDriver.FindElement(contactMailAddressLine3Input).SendKeys(contact.OrgMailAddressLine3);
                }
                if(contact.OrgMailCountry != "")
                    ChooseSpecificSelectOption(contactMailCountrySelect, contact.OrgMailCountry);
                if (contact.OrgMailCountry == "Other")
                {
                    ClearInput(contactMailOtherCountryInput);
                    webDriver.FindElement(contactMailOtherCountryInput).SendKeys(contact.OrgMailOtherCountry);
                }
                if(contact.OrgMailProvince != "")
                {
                    ChooseSpecificSelectOption(contactMailProvinceSelect, contact.OrgMailProvince);
                }
                if (contact.OrgMailCity != "")
                {
                    ClearInput(contactMailCityInput);
                    webDriver.FindElement(contactMailCityInput).SendKeys(contact.OrgMailCity);
                }
                if (contact.OrgMailPostalCode != "")
                {
                    ClearInput(contactMailPostalCodeInput);
                    webDriver.FindElement(contactMailPostalCodeInput).SendKeys(contact.OrgMailPostalCode);
                }
            }

            //Updating contact property address
            if (contact.OrgPropertyAddressLine1 != "")
            {
                while (webDriver.FindElements(contactAddressPropertyAddressRemoveBttn).Count > 0)
                {
                    FocusAndClick(contactAddressPropertyAddressRemoveBttn);
                }
                ClearInput(contactPropertyAddressLine1Input);

                webDriver.FindElement(contactPropertyAddressLine1Input).SendKeys(contact.OrgPropertyAddressLine1);
                if (contact.OrgPropertyAddressLine2 != "")
                {
                    webDriver.FindElement(contactPropertyAddAddressLineBttn).Click();
                    webDriver.FindElement(contactPropertyAddressLine2Input).SendKeys(contact.OrgPropertyAddressLine2);
                }
                if (contact.OrgPropertyAddressLine3 != "")
                {
                    webDriver.FindElement(contactPropertyAddAddressLineBttn).Click();
                    webDriver.FindElement(contactPropertyAddressLine3Input).SendKeys(contact.OrgPropertyAddressLine3);
                }
                if(contact.OrgPropertyCountry != "")
                    ChooseSpecificSelectOption(contactPropertyCountrySelect, contact.OrgPropertyCountry);
                if (contact.OrgPropertyCountry == "Other")
                {
                    ClearInput(contactPropertyOtherCountryInput);
                    webDriver.FindElement(contactPropertyOtherCountryInput).SendKeys(contact.OrgPropertyOtherCountry);
                }
                if(contact.OrgPropertyProvince != "")
                    ChooseSpecificSelectOption(contactPropertyProvinceSelect, contact.OrgPropertyProvince);

                if (contact.OrgPropertyCity != "")
                {
                    ClearInput(contactPropertyCityInput);
                    webDriver.FindElement(contactPropertyCityInput).SendKeys(contact.OrgPropertyCity);
                }
                if (contact.OrgPropertyPostalCode != "")
                {
                    ClearInput(contactPropertyPostalCodeInput);
                    webDriver.FindElement(contactPropertyPostalCodeInput).SendKeys(contact.OrgPropertyPostalCode);
                }
            }

            //Updating contact billing address
            if (contact.OrgBillingAddressLine1 != "")
            {
                while (webDriver.FindElements(contactAddressBillingAddressRemoveBttn).Count > 0)
                {
                    FocusAndClick(contactAddressBillingAddressRemoveBttn);
                }
                ClearInput(contactBillingAddressLine1Input);

                webDriver.FindElement(contactBillingAddressLine1Input).SendKeys(contact.OrgBillingAddressLine1);
                if (contact.OrgBillingAddressLine2 != "")
                {
                    webDriver.FindElement(contactBillingAddAddressLineBttn).Click();
                    webDriver.FindElement(contactBillingAddressLine2Input).SendKeys(contact.OrgBillingAddressLine2);
                }
                if (contact.OrgBillingAddressLine3 != "")
                {
                    webDriver.FindElement(contactBillingAddAddressLineBttn).Click();
                    webDriver.FindElement(contactBillingAddressLine3Input).SendKeys(contact.OrgBillingAddressLine3);
                }
                if(contact.OrgBillingCountry != "")
                    ChooseSpecificSelectOption(contactBillingCountrySelect, contact.OrgBillingCountry);
                if (contact.OrgBillingCountry == "Other")
                {
                    ClearInput(contactBillingOtherCountryInput);
                    webDriver.FindElement(contactBillingOtherCountryInput).SendKeys(contact.OrgBillingOtherCountry);
                }
                if(contact.OrgBillingProvince != "")
                {
                    ChooseSpecificSelectOption(contactBillingProvinceSelect, contact.OrgBillingProvince);
                }
                if (contact.OrgBillingCity != "")
                {
                    Wait();
                    ClearInput(contactBillingCityInput);
                    webDriver.FindElement(contactBillingCityInput).SendKeys(contact.OrgBillingCity);
                }
                if (contact.OrgBillingPostalCode != "")
                {
                    Wait();
                    ClearInput(contactBillingPostalCodeInput);
                    webDriver.FindElement(contactBillingPostalCodeInput).SendKeys(contact.OrgBillingPostalCode);
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

                WaitUntilClickable(contactOrgNameSelect);
                webDriver.FindElement(contactOrgNameSelect).Click();
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
            if (contact.IndMailAddressLine1 != "")
            {
                while (webDriver.FindElements(contactAddressMailAddressRemoveBttn).Count > 0)
                {
                    FocusAndClick(contactAddressMailAddressRemoveBttn);
                }
                ClearInput(contactMailAddressLine1Input);

                webDriver.FindElement(contactMailAddressLine1Input).SendKeys(contact.IndMailAddressLine1);

                if (contact.IndMailAddressLine2 != "")
                {
                    webDriver.FindElement(contactIndMailAddAddressLineBttn).Click();
                    webDriver.FindElement(contactMailAddressLine2Input).SendKeys(contact.IndMailAddressLine2);
                }
                if (contact.IndMailAddressLine3 != "")
                {
                    webDriver.FindElement(contactIndMailAddAddressLineBttn).Click();
                    webDriver.FindElement(contactMailAddressLine3Input).SendKeys(contact.IndMailAddressLine3);
                }
                if (contact.IndMailCountry != "")
                    ChooseSpecificSelectOption(contactMailCountrySelect, contact.IndMailCountry);
                if (contact.IndMailCountry == "Other")
                {
                    ClearInput(contactMailOtherCountryInput);
                    webDriver.FindElement(contactMailOtherCountryInput).SendKeys(contact.IndMailOtherCountry);
                }
                if (contact.IndMailProvince != "")
                {
                    ChooseSpecificSelectOption(contactMailProvinceSelect, contact.IndMailProvince);
                }
                if (contact.IndMailCity != "")
                {
                    ClearInput(contactMailCityInput);
                    webDriver.FindElement(contactMailCityInput).SendKeys(contact.IndMailCity);
                }
                if (contact.IndMailPostalCode != "")
                {
                    ClearInput(contactMailPostalCodeInput);
                    webDriver.FindElement(contactMailPostalCodeInput).SendKeys(contact.IndMailPostalCode);
                }
            }

            //Updating contact property address
            if (contact.IndPropertyAddressLine1 != "")
            {
                while (webDriver.FindElements(contactAddressPropertyAddressRemoveBttn).Count > 0)
                {
                    FocusAndClick(contactAddressPropertyAddressRemoveBttn);
                }
                ClearInput(contactPropertyAddressLine1Input);

                webDriver.FindElement(contactPropertyAddressLine1Input).SendKeys(contact.IndPropertyAddressLine1);
                if (contact.IndPropertyAddressLine2 != "")
                {
                    webDriver.FindElement(contactPropertyAddAddressLineBttn).Click();
                    webDriver.FindElement(contactPropertyAddressLine2Input).SendKeys(contact.IndPropertyAddressLine2);
                }
                if (contact.IndPropertyAddressLine3 != "")
                {
                    webDriver.FindElement(contactPropertyAddAddressLineBttn).Click();
                    webDriver.FindElement(contactPropertyAddressLine3Input).SendKeys(contact.IndPropertyAddressLine3);
                }
                if (contact.IndPropertyCountry != "")
                    ChooseSpecificSelectOption(contactPropertyCountrySelect, contact.IndPropertyCountry);
                if (contact.IndPropertyCountry == "Other")
                {
                    ClearInput(contactPropertyOtherCountryInput);
                    webDriver.FindElement(contactPropertyOtherCountryInput).SendKeys(contact.IndPropertyOtherCountry);
                }
                if (contact.IndPropertyProvince != "")
                    ChooseSpecificSelectOption(contactPropertyProvinceSelect, contact.IndPropertyProvince);
                if (contact.IndPropertyCity != "")
                {
                    ClearInput(contactPropertyCityInput);
                    webDriver.FindElement(contactPropertyCityInput).SendKeys(contact.IndPropertyCity);
                }
                if (contact.IndPropertyPostalCode != "")
                {
                    ClearInput(contactPropertyPostalCodeInput);
                    webDriver.FindElement(contactPropertyPostalCodeInput).SendKeys(contact.IndPropertyPostalCode);
                }
            }

            //Updating contact billing address
            if (contact.IndBillingAddressLine1 != "")
            {
                while (webDriver.FindElements(contactAddressBillingAddressRemoveBttn).Count > 0)
                {
                    FocusAndClick(contactAddressBillingAddressRemoveBttn);
                }
                ClearInput(contactBillingAddressLine1Input);

                webDriver.FindElement(contactBillingAddressLine1Input).SendKeys(contact.IndBillingAddressLine1);
                if (contact.IndBillingAddressLine2 != "")
                {
                    webDriver.FindElement(contactBillingAddAddressLineBttn).Click();
                    webDriver.FindElement(contactBillingAddressLine2Input).SendKeys(contact.IndBillingAddressLine2);
                }
                if (contact.IndBillingAddressLine3 != "")
                {
                    webDriver.FindElement(contactBillingAddAddressLineBttn).Click();
                    webDriver.FindElement(contactBillingAddressLine3Input).SendKeys(contact.IndBillingAddressLine3);
                }
                if (contact.IndBillingCountry != "")
                    ChooseSpecificSelectOption(contactBillingCountrySelect, contact.IndBillingCountry);
                if (contact.IndBillingCountry == "Other")
                {
                    ClearInput(contactBillingOtherCountryInput);
                    webDriver.FindElement(contactBillingOtherCountryInput).SendKeys(contact.IndBillingOtherCountry);
                }
                if (contact.IndBillingProvince != "")
                {
                    ChooseSpecificSelectOption(contactBillingProvinceSelect, contact.IndBillingProvince);
                }
                if (contact.IndBillingCity != "")
                {
                    ClearInput(contactBillingCityInput);
                    webDriver.FindElement(contactBillingCityInput).SendKeys(contact.IndBillingCity);
                }
                if (contact.IndBillingPostalCode != "")
                {
                    ClearInput(contactBillingPostalCodeInput);
                    webDriver.FindElement(contactBillingPostalCodeInput).SendKeys(contact.IndBillingPostalCode);
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

            AssertTrueIsDisplayed(contactEditButton);
        }

        //Cancel Contact
        public void CancelContact()
        {
            ButtonElement("Cancel");

            Wait();
            if (webDriver.FindElements(contactModal).Count > 0)
            {
                Assert.Equal("Unsaved Changes", sharedModals.ModalHeader());
                Assert.Equal("If you choose to cancel now, your changes will not be saved.", sharedModals.ConfirmationModalText1());
                Assert.Equal("Do you want to proceed?", sharedModals.ConfirmationModalText2());

                sharedModals.ModalClickOKBttn();
            }

            AssertTrueIsDisplayed(contactsSearchTable);
        }

        // ASSERT FUNCTIONS
        public void VerifyIndividualContactView(IndividualContact contact)
        {
            Wait(3000);

            AssertTrueIsDisplayed(contactTitle);
            AssertTrueIsDisplayed(contactEditButton);

            if(contact.FullName != "")
                AssertTrueContentEquals(contactIndFullName, contact.FullName);
            AssertTrueIsDisplayed(contactIndPrefNameLabel);

            if(contact.PreferableName != "")
                AssertTrueContentEquals(contactIndPrefNameContent, contact.PreferableName);

            if(contact.ContactStatus != "")
                AssertTrueContentEquals(contactIndStatusSpan, contact.ContactStatus);

            AssertTrueIsDisplayed(contactIndOrganizationLabel);

            if (contact.Organization != "")
                AssertTrueContentEquals(contactIndOrganizationContent, contact.Organization);

            AssertTrueIsDisplayed(contactInfoSubtitle);
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

            if (contact.IndMailAddressLine1 != "")
            {
                AssertTrueIsDisplayed(contactAddressMailSubtitle);

                var mailAddressTotalLines = webDriver.FindElements(contactAddressIndMailCounter).Count;
                switch (mailAddressTotalLines)
                {
                    case 4:
                        AssertTrueContentEquals(contactAddressIndMailAddressLine1, contact.IndMailAddressLine1);
                        AssertTrueContentEquals(contactAddressIndMailAddressLine2, contact.IndMailCityProvinceView);
                        AssertTrueContentEquals(contactAddressIndMailAddressLine3, contact.IndMailPostalCode);

                        if (contact.IndMailCountry == "Other")
                            AssertTrueContentEquals(contactAddressIndMailAddressLine4, contact.IndMailOtherCountry);
                        else
                            AssertTrueContentEquals(contactAddressIndMailAddressLine4, contact.IndMailCountry);
                        break;

                    case 5:
                        AssertTrueContentEquals(contactAddressIndMailAddressLine1, contact.IndMailAddressLine1);
                        AssertTrueContentEquals(contactAddressIndMailAddressLine2, contact.IndMailAddressLine2);
                        AssertTrueContentEquals(contactAddressIndMailAddressLine3, contact.IndMailCityProvinceView);
                        AssertTrueContentEquals(contactAddressIndMailAddressLine4, contact.IndMailPostalCode);

                        if (contact.IndMailCountry == "Other")
                            AssertTrueContentEquals(contactAddressIndMailAddressLine5, contact.IndMailOtherCountry);
                        else
                            AssertTrueContentEquals(contactAddressIndMailAddressLine5, contact.IndMailCountry);
                        break;

                    case 6:
                        AssertTrueContentEquals(contactAddressIndMailAddressLine1,contact.IndMailAddressLine1);
                        AssertTrueContentEquals(contactAddressIndMailAddressLine2,contact.IndMailAddressLine2);
                        AssertTrueContentEquals(contactAddressIndMailAddressLine3,contact.IndMailAddressLine3);
                        AssertTrueContentEquals(contactAddressIndMailAddressLine4,contact.IndMailCityProvinceView);
                        AssertTrueContentEquals(contactAddressIndMailAddressLine5,contact.IndMailPostalCode);

                        if (contact.IndMailCountry == "Other")
                            AssertTrueContentEquals(contactAddressIndMailAddressLine6,contact.IndMailOtherCountry);
                        else
                            AssertTrueContentEquals(contactAddressIndMailAddressLine6,contact.IndMailCountry);
                        break;
                }
            }
            if (contact.IndPropertyAddressLine1 != "")
            {
                AssertTrueIsDisplayed(contactAddressPropertySubtitle);

                var propertyAddressTotalLines = webDriver.FindElements(contactAddressIndPropertyCounter).Count;
                switch (propertyAddressTotalLines)
                {
                    case 4:
                        AssertTrueContentEquals(contactAddressIndPropertyAddressLine1,contact.IndPropertyAddressLine1);
                        AssertTrueContentEquals(contactAddressIndPropertyAddressLine2,contact.IndPropertyCityProvinceView);
                        AssertTrueContentEquals(contactAddressIndPropertyAddressLine3,contact.IndPropertyPostalCode);

                        if (contact.IndPropertyCountry == "Other")
                            AssertTrueContentEquals(contactAddressIndPropertyAddressLine4, contact.IndPropertyOtherCountry);
                        else
                            AssertTrueContentEquals(contactAddressIndPropertyAddressLine4, contact.IndPropertyCountry);
                        break;

                    case 5:
                        AssertTrueContentEquals(contactAddressIndPropertyAddressLine1, contact.IndPropertyAddressLine1);
                        AssertTrueContentEquals(contactAddressIndPropertyAddressLine2, contact.IndPropertyAddressLine2);
                        AssertTrueContentEquals(contactAddressIndPropertyAddressLine3, contact.IndPropertyCityProvinceView);
                        AssertTrueContentEquals(contactAddressIndPropertyAddressLine4, contact.IndPropertyPostalCode);

                        if (contact.IndPropertyCountry == "Other")
                            AssertTrueContentEquals(contactAddressIndPropertyAddressLine5, contact.IndPropertyOtherCountry);
                        else
                            AssertTrueContentEquals(contactAddressIndPropertyAddressLine5, contact.IndPropertyCountry);
                        break;

                    case 6:
                        AssertTrueContentEquals(contactAddressIndPropertyAddressLine1, contact.IndPropertyAddressLine1);
                        AssertTrueContentEquals(contactAddressIndPropertyAddressLine2, contact.IndPropertyAddressLine2);
                        AssertTrueContentEquals(contactAddressIndPropertyAddressLine3, contact.IndPropertyAddressLine3);
                        AssertTrueContentEquals(contactAddressIndPropertyAddressLine4, contact.IndPropertyCityProvinceView);
                        AssertTrueContentEquals(contactAddressIndPropertyAddressLine5, contact.IndPropertyPostalCode);

                        if (contact.IndPropertyCountry == "Other")
                            AssertTrueContentEquals(contactAddressIndPropertyAddressLine6, contact.IndPropertyOtherCountry);
                        else
                            AssertTrueContentEquals(contactAddressIndPropertyAddressLine6, contact.IndPropertyCountry);
                        break;
                }
            }
            if (contact.IndBillingAddressLine1 != "")
            {
                AssertTrueIsDisplayed(contactAddressBillingSubtitle);

                var propertyAddressTotalLines = webDriver.FindElements(contactAddressIndBillingCounter).Count;
                switch (propertyAddressTotalLines)
                {
                    case 4:
                        AssertTrueContentEquals(contactAddressIndBillingAddressLine1, contact.IndBillingAddressLine1);
                        AssertTrueContentEquals(contactAddressIndBillingAddressLine2, contact.IndBillingCityProvinceView);
                        AssertTrueContentEquals(contactAddressIndBillingAddressLine3, contact.IndBillingPostalCode);

                        if (contact.IndBillingCountry == "Other")
                            AssertTrueContentEquals(contactAddressIndBillingAddressLine4, contact.IndBillingOtherCountry);
                        else
                            AssertTrueContentEquals(contactAddressIndBillingAddressLine4, contact.IndBillingCountry);
                        break;

                    case 5:
                        AssertTrueContentEquals(contactAddressIndBillingAddressLine1, contact.IndBillingAddressLine1);
                        AssertTrueContentEquals(contactAddressIndBillingAddressLine2, contact.IndBillingAddressLine2);
                        AssertTrueContentEquals(contactAddressIndBillingAddressLine3, contact.IndBillingCityProvinceView);
                        AssertTrueContentEquals(contactAddressIndBillingAddressLine4, contact.IndBillingPostalCode);

                        if (contact.IndBillingCountry == "Other")
                            AssertTrueContentEquals(contactAddressIndBillingAddressLine5, contact.IndBillingOtherCountry);
                        else
                            AssertTrueContentEquals(contactAddressIndBillingAddressLine5, contact.IndBillingCountry);
                        break;

                    case 6:
                        AssertTrueContentEquals(contactAddressIndBillingAddressLine1, contact.IndBillingAddressLine1);
                        AssertTrueContentEquals(contactAddressIndBillingAddressLine2, contact.IndBillingAddressLine2);
                        AssertTrueContentEquals(contactAddressIndBillingAddressLine3, contact.IndBillingAddressLine3);
                        AssertTrueContentEquals(contactAddressIndBillingAddressLine4, contact.IndBillingCityProvinceView);
                        AssertTrueContentEquals(contactAddressIndBillingAddressLine5, contact.IndBillingPostalCode);

                        if (contact.IndBillingCountry == "Other")
                            AssertTrueContentEquals(contactAddressIndBillingAddressLine6, contact.IndBillingOtherCountry);
                        else
                            AssertTrueContentEquals(contactAddressIndBillingAddressLine6, contact.IndBillingCountry);
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

            if(contact.OrganizationName != "")
                AssertTrueContentEquals(contactOrgName, contact.OrganizationName);
            AssertTrueIsDisplayed(contactOrgAliasLabel);

            if(contact.Alias != "")
                AssertTrueContentEquals(contactOrgAliasContent, contact.Alias);

            AssertTrueContentEquals(contactOrgStatusSpan, contact.ContactStatus);
            AssertTrueIsDisplayed(contactOrgIncorpNbrLabel);

            if(contact.IncorporationNumber != "")
                AssertTrueContentEquals(contactOrgIncorpNbrContent,contact.IncorporationNumber);

            AssertTrueIsDisplayed(contactInfoSubtitle);
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
            if (contact.OrgMailAddressLine1 != "")
            {
                AssertTrueIsDisplayed(contactAddressMailSubtitle);

                var mailAddressTotalLines = webDriver.FindElements(contactAddressOrgMailCounter).Count;
                switch (mailAddressTotalLines)
                {
                    case 4:
                        AssertTrueContentEquals(contactAddressOrgMailAddressLine1, contact.OrgMailAddressLine1);
                        AssertTrueContentEquals(contactAddressOrgMailAddressLine2, contact.OrgMailCityProvinceView);
                        AssertTrueContentEquals(contactAddressOrgMailAddressLine3, contact.OrgMailPostalCode);

                        if (contact.OrgMailCountry == "Other")
                            AssertTrueContentEquals(contactAddressOrgMailAddressLine4, contact.OrgMailOtherCountry);
                        else
                            AssertTrueContentEquals(contactAddressOrgMailAddressLine4, contact.OrgMailCountry);
                        break;

                    case 5:
                        AssertTrueContentEquals(contactAddressOrgMailAddressLine1, contact.OrgMailAddressLine1);
                        AssertTrueContentEquals(contactAddressOrgMailAddressLine2, contact.OrgMailAddressLine2);
                        AssertTrueContentEquals(contactAddressOrgMailAddressLine3, contact.OrgMailCityProvinceView);
                        AssertTrueContentEquals(contactAddressOrgMailAddressLine4, contact.OrgMailPostalCode);

                        if (contact.OrgMailCountry == "Other")
                            AssertTrueContentEquals(contactAddressOrgMailAddressLine5, contact.OrgMailOtherCountry);
                        else
                            AssertTrueContentEquals(contactAddressOrgMailAddressLine5, contact.OrgMailCountry);
                        break;

                    case 6:
                        AssertTrueContentEquals(contactAddressOrgMailAddressLine1, contact.OrgMailAddressLine1);
                        AssertTrueContentEquals(contactAddressOrgMailAddressLine2, contact.OrgMailAddressLine2);
                        AssertTrueContentEquals(contactAddressOrgMailAddressLine3, contact.OrgMailAddressLine3);
                        AssertTrueContentEquals(contactAddressOrgMailAddressLine4, contact.OrgMailCityProvinceView);
                        AssertTrueContentEquals(contactAddressOrgMailAddressLine5, contact.OrgMailPostalCode);

                        if (contact.OrgMailCountry == "Other")
                            AssertTrueContentEquals(contactAddressOrgMailAddressLine6, contact.OrgMailOtherCountry);
                        else
                            AssertTrueContentEquals(contactAddressOrgMailAddressLine6, contact.OrgMailCountry);
                        break;
                }
            }
            if (contact.OrgPropertyAddressLine1 != "")
            {
                AssertTrueIsDisplayed(contactAddressPropertySubtitle);

                var propertyAddressTotalLines = webDriver.FindElements(contactAddressOrgPropertyCounter).Count;
                switch (propertyAddressTotalLines)
                {
                    case 4:
                        AssertTrueContentEquals(contactAddressOrgPropertyAddressLine1, contact.OrgPropertyAddressLine1);
                        AssertTrueContentEquals(contactAddressOrgPropertyAddressLine2, contact.OrgPropertyCityProvinceView);
                        AssertTrueContentEquals(contactAddressOrgPropertyAddressLine3, contact.OrgPropertyPostalCode);

                        if (contact.OrgPropertyCountry == "Other")
                            AssertTrueContentEquals(contactAddressOrgPropertyAddressLine4, contact.OrgPropertyOtherCountry);
                        else  
                            AssertTrueContentEquals(contactAddressOrgPropertyAddressLine4, contact.OrgPropertyCountry);
                        break;

                    case 5:
                        AssertTrueContentEquals(contactAddressOrgPropertyAddressLine1,contact.OrgPropertyAddressLine1);
                        AssertTrueContentEquals(contactAddressOrgPropertyAddressLine2,contact.OrgPropertyAddressLine2);
                        AssertTrueContentEquals(contactAddressOrgPropertyAddressLine3,contact.OrgPropertyCityProvinceView);
                        AssertTrueContentEquals(contactAddressOrgPropertyAddressLine4,contact.OrgPropertyPostalCode);

                        if (contact.OrgPropertyCountry == "Other")
                        {
                            AssertTrueContentEquals(contactAddressOrgPropertyAddressLine5,contact.OrgPropertyOtherCountry);
                        }
                        else
                        {
                            AssertTrueContentEquals(contactAddressOrgPropertyAddressLine5,contact.OrgPropertyCountry);
                        }
                        break;

                    case 6:
                        AssertTrueContentEquals(contactAddressOrgPropertyAddressLine1,contact.OrgPropertyAddressLine1);
                        AssertTrueContentEquals(contactAddressOrgPropertyAddressLine2,contact.OrgPropertyAddressLine2);
                        AssertTrueContentEquals(contactAddressOrgPropertyAddressLine3,contact.OrgPropertyAddressLine3);
                        AssertTrueContentEquals(contactAddressOrgPropertyAddressLine4,contact.OrgPropertyCityProvinceView);
                        AssertTrueContentEquals(contactAddressOrgPropertyAddressLine5,contact.OrgPropertyPostalCode);

                        if (contact.OrgPropertyCountry == "Other")
                            AssertTrueContentEquals(contactAddressOrgPropertyAddressLine6,contact.OrgPropertyOtherCountry);
                        else
                            AssertTrueContentEquals(contactAddressOrgPropertyAddressLine6,contact.OrgPropertyCountry);
                        break;
                }
            }
            if (contact.OrgBillingAddressLine1 != "")
            {
                AssertTrueIsDisplayed(contactAddressBillingSubtitle);

                var propertyAddressTotalLines = webDriver.FindElements(contactAddressOrgBillingCounter).Count;
                switch (propertyAddressTotalLines)
                {
                    case 4:
                        AssertTrueContentEquals(contactAddressOrgBillingAddressLine1,contact.OrgBillingAddressLine1);
                        AssertTrueContentEquals(contactAddressOrgBillingAddressLine2,contact.OrgBillingCityProvinceView);
                        AssertTrueContentEquals(contactAddressOrgBillingAddressLine3,contact.OrgBillingPostalCode);
                        if (contact.OrgBillingCountry == "Other")
                            AssertTrueContentEquals(contactAddressOrgBillingAddressLine4,contact.OrgBillingOtherCountry);
                        else
                            AssertTrueContentEquals(contactAddressOrgBillingAddressLine4,contact.OrgBillingCountry);
                        break;

                    case 5:
                        AssertTrueContentEquals(contactAddressOrgBillingAddressLine1,contact.OrgBillingAddressLine1);
                        AssertTrueContentEquals(contactAddressOrgBillingAddressLine2,contact.OrgBillingAddressLine2);
                        AssertTrueContentEquals(contactAddressOrgBillingAddressLine3,contact.OrgBillingCityProvinceView);
                        AssertTrueContentEquals(contactAddressOrgBillingAddressLine4,contact.OrgBillingPostalCode);

                        if (contact.OrgBillingCountry == "Other")
                            AssertTrueContentEquals(contactAddressOrgBillingAddressLine5, contact.OrgBillingOtherCountry);
                        else
                            AssertTrueContentEquals(contactAddressOrgBillingAddressLine5,contact.OrgBillingCountry);
                        break;

                    case 6:
                        AssertTrueContentEquals(contactAddressOrgBillingAddressLine1,contact.OrgBillingAddressLine1);
                        AssertTrueContentEquals(contactAddressOrgBillingAddressLine2,contact.OrgBillingAddressLine2);
                        AssertTrueContentEquals(contactAddressOrgBillingAddressLine3,contact.OrgBillingAddressLine3);
                        AssertTrueContentEquals(contactAddressOrgBillingAddressLine4,contact.OrgBillingCityProvinceView);
                        AssertTrueContentEquals(contactAddressOrgBillingAddressLine5,contact.OrgBillingPostalCode);

                        if (contact.OrgBillingCountry == "Other")
                            AssertTrueContentEquals(contactAddressOrgBillingAddressLine6,contact.OrgBillingOtherCountry);
                        else
                            AssertTrueContentEquals(contactAddressOrgBillingAddressLine6,contact.OrgBillingCountry);
                        break;
                }
            }
            AssertTrueIsDisplayed(commentsSubtitle);
            AssertTrueContentEquals(commentsOrganizationContent,contact.OrgComments);
        }

    }
}
