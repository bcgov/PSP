using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;
using System.Diagnostics.Contracts;

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
        private By contactIndPrefNameInput = By.CssSelector("input[id='input-preferredName']");
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
        private By contactAddressMailAddressRemoveBttn = By.XPath("//h3[contains(text(),'Mailing Address')]/parent::div/div/div/div/button/div/span[contains(text(),'Remove')]");

        private By contactAddressOrgMailCounter = By.XPath("//div[1]/div[@data-testid='contact-organization-address']/div");
        private By contactAddressOrgMailAddressLine1 = By.XPath("//div[1]/div[@data-testid='contact-organization-address']/div[1]");
        private By contactAddressOrgMailAddressLine2 = By.XPath("//div[1]/div[@data-testid='contact-organization-address']/div[2]");
        private By contactAddressOrgMailAddressLine3 = By.XPath("//div[1]/div[@data-testid='contact-organization-address']/div[3]");
        private By contactAddressOrgMailAddressLine4 = By.XPath("//div[1]/div[@data-testid='contact-organization-address']/div[4]");
        private By contactAddressOrgMailAddressLine5 = By.XPath("//div[1]/div[@data-testid='contact-organization-address']/div[5]");
        private By contactAddressOrgMailAddressLine6 = By.XPath("//div[1]/div[@data-testid='contact-organization-address']/div[6]");
        private By contactAddressPropertyAddressRemoveBttn = By.XPath("//h3[contains(text(),'Property Address')]/parent::div/div/div/div/button/div/span[contains(text(),'Remove')]");

        private By contactAddressPropertySubtitle = By.XPath("//strong[contains(text(),'Property address')]");
        private By contactAddressIndPropertyCounter = By.XPath("//div[2]/div/span[@data-testid='contact-person-address']/div");
        private By contactAddressIndPropertyAddressLine1 = By.XPath("//div[2]/div/span[@data-testid='contact-person-address']/div[1]");
        private By contactAddressIndPropertyAddressLine2 = By.XPath("//div[2]/div/span[@data-testid='contact-person-address']/div[2]");
        private By contactAddressIndPropertyAddressLine3 = By.XPath("//div[2]/div/span[@data-testid='contact-person-address']/div[3]");
        private By contactAddressIndPropertyAddressLine4 = By.XPath("//div[2]/div/span[@data-testid='contact-person-address']/div[4]");
        private By contactAddressIndPropertyAddressLine5 = By.XPath("//div[2]/div/span[@data-testid='contact-person-address']/div[5]");
        private By contactAddressIndPropertyAddressLine6 = By.XPath("//div[2]/div/span[@data-testid='contact-person-address']/div[6]");
        private By contactAddressBillingAddressRemoveBttn = By.XPath("//h3[contains(text(),'Billing Address')]/parent::div/div/div/div/button/div/span[contains(text(),'Remove')]");

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
        private By contactModalContinueSaveBttn = By.XPath("//button/div[contains(text(),'Continue Save')]");
        private By contactConfirmCancelBttn = By.XPath("//button/div[contains(text(),'Confirm')]");

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
                    webDriver.FindElement(contactMailAddAddressLineBttn).Click();
                    webDriver.FindElement(contactMailAddressLine2Input).SendKeys(contact.IndMailAddressLine2);
                }
                if (contact.IndMailAddressLine3 != "")
                {
                    webDriver.FindElement(contactMailAddAddressLineBttn).Click();
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
                    webDriver.FindElement(contactMailAddAddressLineBttn).Click();
                    webDriver.FindElement(contactMailAddressLine2Input).SendKeys(contact.OrgMailAddressLine2);
                }
                if (contact.OrgMailAddressLine3 != "")
                {
                    webDriver.FindElement(contactMailAddAddressLineBttn).Click();
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
                    webDriver.FindElement(contactMailAddAddressLineBttn).Click();
                    webDriver.FindElement(contactMailAddressLine2Input).SendKeys(contact.OrgMailAddressLine2);
                }
                if (contact.OrgMailAddressLine3 != "")
                {
                    webDriver.FindElement(contactMailAddAddressLineBttn).Click();
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
                    webDriver.FindElement(contactMailAddAddressLineBttn).Click();
                    webDriver.FindElement(contactMailAddressLine2Input).SendKeys(contact.IndMailAddressLine2);
                }
                if (contact.IndMailAddressLine3 != "")
                {
                    webDriver.FindElement(contactMailAddAddressLineBttn).Click();
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

            //Save
            ButtonElement("Save");

            Wait();
            if (webDriver.FindElements(contactDuplicateModal).Count > 0)
            {
                Assert.True(sharedModals.ModalHeader().Equals("Duplicate Contact"));
                Assert.True(sharedModals.ModalContent().Equals("A contact matching this information already exists in the system."));
                webDriver.FindElement(contactModalContinueSaveBttn).Click();
            }

            WaitUntilVisible(contactEditButton);
            Assert.True(webDriver.FindElement(contactEditButton).Displayed);

        }

        //Cancel Contact
        public void CancelContact()
        {

            ButtonElement("Cancel");

            ButtonElement(contactConfirmCancelBttn);

            WaitUntilVisible(contactsSearchTable);
            var contactTableElement = webDriver.FindElement(contactsSearchTable);
            Assert.True(contactTableElement.Displayed);
        }

        // ASSERT FUNCTIONS
        public void VerifyIndividualContactView(IndividualContact contact)
        {
            WaitUntilVisible(contactIndFullName);
            Assert.True(webDriver.FindElement(contactTitle).Displayed);
            Assert.True(webDriver.FindElement(contactEditButton).Displayed);

            Assert.True(webDriver.FindElement(contactIndFullName).Text.Equals(contact.FullName));
            Assert.True(webDriver.FindElement(contactIndPrefNameLabel).Displayed);
            Assert.True(webDriver.FindElement(contactIndPrefNameContent).Text.Equals(contact.PreferableName));
            Assert.True(webDriver.FindElement(contactIndStatusSpan).Text.Equals(contact.ContactStatus));

            Assert.True(webDriver.FindElement(contactIndOrganizationLabel).Displayed);
            if (contact.Organization != "")
            {
                Assert.True(webDriver.FindElement(contactIndOrganizationContent).Text.Equals(contact.Organization));
            }
 
            Assert.True(webDriver.FindElement(contactInfoSubtitle).Displayed);
            Assert.True(webDriver.FindElement(contactEmailLabel).Displayed);           
            if (contact.IndEmail1 != "")
            {
                Assert.True(webDriver.FindElement(contactEmail1Content).Text.Equals(contact.IndEmail1));
                Assert.True(webDriver.FindElement(contactEmailType1Content).Text.Equals(contact.IndEmailTypeDisplay1));
            }
            if (contact.IndEmail2 != "")
            {
                Assert.True(webDriver.FindElement(contactEmail2Content).Text.Equals(contact.IndEmail2));
                Assert.True(webDriver.FindElement(contactEmailType2Content).Text.Equals(contact.IndEmailTypeDisplay2));
            }
            Assert.True(webDriver.FindElement(contactPhoneLabel).Displayed);
            
            if (contact.IndPhone1 != "")
            {
                Assert.True(webDriver.FindElement(contactPhone1Content).Text.Equals(contact.IndPhone1));
                Assert.True(webDriver.FindElement(contactPhoneType1Content).Text.Equals(contact.IndPhoneTypeDisplay1));
            }
            if (contact.IndPhone2 != "")
            {
                Assert.True(webDriver.FindElement(contactPhone2Content).Text.Equals(contact.IndPhone2));
                Assert.True(webDriver.FindElement(contactPhoneType2Content).Text.Equals(contact.IndPhoneTypeDisplay2));
            }

            Assert.True(webDriver.FindElement(contactAddressSubtitle).Displayed);
            if (contact.IndMailAddressLine1 != "")
            {
                Assert.True(webDriver.FindElement(contactAddressMailSubtitle).Displayed);

                var mailAddressTotalLines = webDriver.FindElements(contactAddressIndMailCounter).Count;
                switch (mailAddressTotalLines)
                {
                    case 4:
                        Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine1).Text.Equals(contact.IndMailAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine2).Text.Equals(contact.IndMailCityProvinceView));
                        Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine3).Text.Equals(contact.IndMailPostalCode));
                        if (contact.IndMailCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine4).Text.Equals(contact.IndMailOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine4).Text.Equals(contact.IndMailCountry));
                        }
                        break;
                    case 5:
                        Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine1).Text.Equals(contact.IndMailAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine2).Text.Equals(contact.IndMailAddressLine2));
                        Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine3).Text.Equals(contact.IndMailCityProvinceView));
                        Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine4).Text.Equals(contact.IndMailPostalCode));
                        if (contact.IndMailCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine5).Text.Equals(contact.IndMailOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine5).Text.Equals(contact.IndMailCountry));
                        }
                        break;
                    case 6:
                        Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine1).Text.Equals(contact.IndMailAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine2).Text.Equals(contact.IndMailAddressLine2));
                        Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine3).Text.Equals(contact.IndMailAddressLine3));
                        Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine4).Text.Equals(contact.IndMailCityProvinceView));
                        Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine5).Text.Equals(contact.IndMailPostalCode));
                        if (contact.IndMailCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine6).Text.Equals(contact.IndMailOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine6).Text.Equals(contact.IndMailCountry));
                        }
                        break;
                }
            }
            if (contact.IndPropertyAddressLine1 != "")
            {
                Assert.True(webDriver.FindElement(contactAddressPropertySubtitle).Displayed);

                var propertyAddressTotalLines = webDriver.FindElements(contactAddressIndPropertyCounter).Count;
                switch (propertyAddressTotalLines)
                {
                    case 4:
                        Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine1).Text.Equals(contact.IndPropertyAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine2).Text.Equals(contact.IndPropertyCityProvinceView));
                        Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine3).Text.Equals(contact.IndPropertyPostalCode));
                        if (contact.IndPropertyCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine4).Text.Equals(contact.IndPropertyOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndMailAddressLine4).Text.Equals(contact.IndPropertyCountry));
                        }
                        break;
                    case 5:
                        Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine1).Text.Equals(contact.IndPropertyAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine2).Text.Equals(contact.IndPropertyAddressLine2));
                        Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine3).Text.Equals(contact.IndPropertyCityProvinceView));
                        Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine4).Text.Equals(contact.IndPropertyPostalCode));
                        if (contact.IndPropertyCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine5).Text.Equals(contact.IndPropertyOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine5).Text.Equals(contact.IndPropertyCountry));
                        }
                        break;
                    case 6:
                        Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine1).Text.Equals(contact.IndPropertyAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine2).Text.Equals(contact.IndPropertyAddressLine2));
                        Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine3).Text.Equals(contact.IndPropertyAddressLine3));
                        Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine4).Text.Equals(contact.IndPropertyCityProvinceView));
                        Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine5).Text.Equals(contact.IndPropertyPostalCode));
                        if (contact.IndPropertyCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine6).Text.Equals(contact.IndPropertyOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndPropertyAddressLine6).Text.Equals(contact.IndPropertyCountry));
                        }
                        break;
                }
            }
            if (contact.IndBillingAddressLine1 != "")
            {
                Assert.True(webDriver.FindElement(contactAddressBillingSubtitle).Displayed);

                var propertyAddressTotalLines = webDriver.FindElements(contactAddressIndBillingCounter).Count;
                switch (propertyAddressTotalLines)
                {
                    case 4:
                        Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine1).Text.Equals(contact.IndBillingAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine2).Text.Equals(contact.IndBillingCityProvinceView));
                        Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine3).Text.Equals(contact.IndBillingPostalCode));
                        if (contact.IndBillingCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine4).Text.Equals(contact.IndBillingOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine4).Text.Equals(contact.IndBillingCountry));
                        }
                        break;
                    case 5:
                        Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine1).Text.Equals(contact.IndBillingAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine2).Text.Equals(contact.IndBillingAddressLine2));
                        Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine3).Text.Equals(contact.IndBillingCityProvinceView));
                        Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine4).Text.Equals(contact.IndBillingPostalCode));
                        if (contact.IndBillingCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine5).Text.Equals(contact.IndBillingOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine5).Text.Equals(contact.IndBillingCountry));
                        }
                        break;
                    case 6:
                        Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine1).Text.Equals(contact.IndBillingAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine2).Text.Equals(contact.IndBillingAddressLine2));
                        Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine3).Text.Equals(contact.IndBillingAddressLine3));
                        Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine4).Text.Equals(contact.IndBillingCityProvinceView));
                        Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine5).Text.Equals(contact.IndBillingPostalCode));
                        if (contact.IndBillingCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine6).Text.Equals(contact.IndBillingOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressIndBillingAddressLine6).Text.Equals(contact.IndBillingCountry));
                        }
                        break;
                }
            }
            Assert.True(webDriver.FindElement(commentsSubtitle).Displayed);
            Assert.True(webDriver.FindElement(commentsIndividualContent).Text.Equals(contact.IndComments));
        }

        public void VerifyOrganizationContactView(OrganizationContact contact)
        {
            WaitUntilVisible(contactOrgName);
            Assert.True(webDriver.FindElement(contactTitle).Displayed);
            Assert.True(webDriver.FindElement(contactEditButton).Displayed);

            Assert.True(webDriver.FindElement(contactOrgName).Text.Equals(contact.OrganizationName));
            Assert.True(webDriver.FindElement(contactOrgAliasLabel).Displayed);

            if(contact.Alias != "")
                Assert.True(webDriver.FindElement(contactOrgAliasContent).Text.Equals(contact.Alias));

            Assert.True(webDriver.FindElement(contactOrgStatusSpan).Text.Equals(contact.ContactStatus));

            Assert.True(webDriver.FindElement(contactOrgIncorpNbrLabel).Displayed);

            if(contact.IncorporationNumber != "")
                Assert.True(webDriver.FindElement(contactOrgIncorpNbrContent).Text.Equals(contact.IncorporationNumber));

            Assert.True(webDriver.FindElement(contactInfoSubtitle).Displayed);
            Assert.True(webDriver.FindElement(contactEmailLabel).Displayed);
            if (contact.OrgEmail1 != "")
            {
                Assert.True(webDriver.FindElement(contactEmail1Content).Text.Equals(contact.OrgEmail1));
                Assert.True(webDriver.FindElement(contactEmailType1Content).Text.Equals(contact.OrgEmailTypeDisplay1));
            }
            if (contact.OrgEmail2 != "")
            {
                Assert.True(webDriver.FindElement(contactEmail2Content).Text.Equals(contact.OrgEmail2));
                Assert.True(webDriver.FindElement(contactEmailType2Content).Text.Equals(contact.OrgEmailTypeDisplay2));
            }

            Assert.True(webDriver.FindElement(contactPhoneLabel).Displayed);
            if (contact.OrgPhone1 != "")
            {
                Assert.True(webDriver.FindElement(contactPhone1Content).Text.Equals(contact.OrgPhone1));
                Assert.True(webDriver.FindElement(contactPhoneType1Content).Text.Equals(contact.OrgPhoneTypeDisplay1));
            }
            if (contact.OrgPhone2 != "")
            {
                Assert.True(webDriver.FindElement(contactPhone2Content).Text.Equals(contact.OrgPhone2));
                Assert.True(webDriver.FindElement(contactPhoneType2Content).Text.Equals(contact.OrgPhoneTypeDisplay2));
            }

            Assert.True(webDriver.FindElement(contactAddressSubtitle).Displayed);
            if (contact.OrgMailAddressLine1 != "")
            {
                Assert.True(webDriver.FindElement(contactAddressMailSubtitle).Displayed);

                var mailAddressTotalLines = webDriver.FindElements(contactAddressOrgMailCounter).Count;
                switch (mailAddressTotalLines)
                {
                    case 4:
                        Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine1).Text.Equals(contact.OrgMailAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine2).Text.Equals(contact.OrgMailCityProvinceView));
                        Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine3).Text.Equals(contact.OrgMailPostalCode));
                        if (contact.OrgMailCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine4).Text.Equals(contact.OrgMailOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine4).Text.Equals(contact.OrgMailCountry));
                        }
                        break;
                    case 5:
                        Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine1).Text.Equals(contact.OrgMailAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine2).Text.Equals(contact.OrgMailAddressLine2));
                        Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine3).Text.Equals(contact.OrgMailCityProvinceView));
                        Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine4).Text.Equals(contact.OrgMailPostalCode));
                        if (contact.OrgMailCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine5).Text.Equals(contact.OrgMailOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine5).Text.Equals(contact.OrgMailCountry));
                        }
                        break;
                    case 6:
                        Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine1).Text.Equals(contact.OrgMailAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine2).Text.Equals(contact.OrgMailAddressLine2));
                        Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine3).Text.Equals(contact.OrgMailAddressLine3));
                        Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine4).Text.Equals(contact.OrgMailCityProvinceView));
                        Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine5).Text.Equals(contact.OrgMailPostalCode));
                        if (contact.OrgMailCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine6).Text.Equals(contact.OrgMailOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgMailAddressLine6).Text.Equals(contact.OrgMailCountry));
                        }
                        break;
                }
            }
            if (contact.OrgPropertyAddressLine1 != "")
            {
                Assert.True(webDriver.FindElement(contactAddressPropertySubtitle).Displayed);

                var propertyAddressTotalLines = webDriver.FindElements(contactAddressOrgPropertyCounter).Count;
                switch (propertyAddressTotalLines)
                {
                    case 4:
                        Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine1).Text.Equals(contact.OrgPropertyAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine2).Text.Equals(contact.OrgPropertyCityProvinceView));
                        Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine3).Text.Equals(contact.OrgPropertyPostalCode));
                        if (contact.OrgPropertyCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine4).Text.Equals(contact.OrgPropertyOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine4).Text.Equals(contact.OrgPropertyCountry));
                        }
                        break;
                    case 5:
                        Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine1).Text.Equals(contact.OrgPropertyAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine2).Text.Equals(contact.OrgPropertyAddressLine2));
                        Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine3).Text.Equals(contact.OrgPropertyCityProvinceView));
                        Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine4).Text.Equals(contact.OrgPropertyPostalCode));
                        if (contact.OrgPropertyCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine5).Text.Equals(contact.OrgPropertyOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine5).Text.Equals(contact.OrgPropertyCountry));
                        }
                        break;
                    case 6:
                        Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine1).Text.Equals(contact.OrgPropertyAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine2).Text.Equals(contact.OrgPropertyAddressLine2));
                        Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine3).Text.Equals(contact.OrgPropertyAddressLine3));
                        Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine4).Text.Equals(contact.OrgPropertyCityProvinceView));
                        Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine5).Text.Equals(contact.OrgPropertyPostalCode));
                        if (contact.OrgPropertyCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine6).Text.Equals(contact.OrgPropertyOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgPropertyAddressLine6).Text.Equals(contact.OrgPropertyCountry));
                        }
                        break;
                }
            }
            if (contact.OrgBillingAddressLine1 != "")
            {
                Assert.True(webDriver.FindElement(contactAddressBillingSubtitle).Displayed);

                var propertyAddressTotalLines = webDriver.FindElements(contactAddressOrgBillingCounter).Count;
                switch (propertyAddressTotalLines)
                {
                    case 4:
                        Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine1).Text.Equals(contact.OrgBillingAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine2).Text.Equals(contact.OrgBillingCityProvinceView));
                        Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine3).Text.Equals(contact.OrgBillingPostalCode));
                        if (contact.OrgBillingCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine4).Text.Equals(contact.OrgBillingOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine4).Text.Equals(contact.OrgBillingCountry));
                        }
                        break;
                    case 5:
                        Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine1).Text.Equals(contact.OrgBillingAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine2).Text.Equals(contact.OrgBillingAddressLine2));
                        Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine3).Text.Equals(contact.OrgBillingCityProvinceView));
                        Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine4).Text.Equals(contact.OrgBillingPostalCode));
                        if (contact.OrgBillingCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine5).Text.Equals(contact.OrgBillingOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine5).Text.Equals(contact.OrgBillingCountry));
                        }
                        break;
                    case 6:
                        Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine1).Text.Equals(contact.OrgBillingAddressLine1));
                        Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine2).Text.Equals(contact.OrgBillingAddressLine2));
                        Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine3).Text.Equals(contact.OrgBillingAddressLine3));
                        Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine4).Text.Equals(contact.OrgBillingCityProvinceView));
                        Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine5).Text.Equals(contact.OrgBillingPostalCode));
                        if (contact.OrgBillingCountry == "Other")
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine6).Text.Equals(contact.OrgBillingOtherCountry));
                        }
                        else
                        {
                            Assert.True(webDriver.FindElement(contactAddressOrgBillingAddressLine6).Text.Equals(contact.OrgBillingCountry));
                        }
                        break;
                }
            }
            Assert.True(webDriver.FindElement(commentsSubtitle).Displayed);
            Assert.True(webDriver.FindElement(commentsOrganizationContent).Text.Equals(contact.OrgComments));
        }

    }
}
