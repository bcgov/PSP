﻿using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class PropertyManagementTab : PageObjectBase
    {
        private By managementTabLink = By.CssSelector("a[data-rb-event-key='management']");

        //View Summary Elements
        private readonly By managementSummaryTitle = By.XPath("//div[contains(text(),'Summary')]");
        private readonly By managementPropertyPurposeLabel = By.XPath("//label[contains(text(),'Property purpose')]");
        private readonly By managementPropertyPurposeContent = By.XPath("//div[contains(text(),'Summary')]/parent::div/parent::h2/parent::div/div/div/div/div//div[@id='multiselectContainerReact']/div/span");
        private readonly By managementLeaseLabel = By.XPath("//label[contains(text(),'Active Lease/License')]");
        private readonly By managementLeaseContent = By.XPath("//label[contains(text(),'Active Lease/License')]/parent::div/following-sibling::div");
        private readonly By managementUtilitiesPayableLabel = By.XPath("//label[contains(text(),'Utilities payable')]");
        private readonly By managementUtilitiesPayableContent = By.XPath("//label[contains(text(),'Utilities payable')]/parent::div/following-sibling::div");
        private readonly By managementTaxesPayableLabel = By.XPath("//label[contains(text(),'Taxes payable')]");
        private readonly By managementTaxesPayableContent = By.XPath("//label[contains(text(),'Taxes payable')]/parent::div/following-sibling::div");
        private readonly By managementAdditionalDetailsLabel = By.XPath("//label[contains(text(),'Additional details')]");
        private readonly By managementAdditionalDetailsContent = By.XPath("//label[contains(text(),'Additional details')]/parent::div/following-sibling::div");

        //Create Summary Elements
        private readonly By managementSummaryEditBttn = By.CssSelector("button[title='Edit property management information']");
        private readonly By managementPropertyPurposeInput = By.Id("multiselect-managementPurposes_input");
        private readonly By managementPropertyPurposeOptions = By.XPath("//input[@id='multiselect-managementPurposes_input']/parent::div/following-sibling::div/ul[@class='optionContainer']");
        private readonly By managementPropertyPurposeDeleteBttns = By.CssSelector("div[id='multiselect-managementPurposes'] i[class='custom-close']");
        private readonly By managementCreateLeaseLabel = By.XPath("//label[contains(text(),'Lease/Licensed')]");
        private readonly By managementUtilitiesPayableSelect = By.Id("input-isUtilitiesPayable");
        private readonly By managementTaxesPayableSelect = By.Id("input-isTaxesPayable");
        private readonly By managementAdditionalDetailsTextarea = By.Id("input-additionalDetails");

        //View Property Contacts List Elements
        private readonly By managementContactTitle = By.XPath("//div[contains(text(),'Property Contact')]");
        private readonly By managementeAddContactBttn = By.XPath("//div[contains(text(),'Property Contact')]/following-sibling::div/button");
        private readonly By managementContactsTable = By.CssSelector("div[data-testid='PropertyContactsTable']");
        private readonly By managementContactsBodyCount = By.CssSelector("div[data-testid='PropertyContactsTable'] div[class='tbody'] div[class='tr-wrapper']");
        private readonly By managementContactsDeleteBttns = By.CssSelector("button[title='Delete contact']");
        private readonly By managementContactsFirstDeleteBttn = By.CssSelector("div[data-testid='PropertyContactsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) [title='Delete contact']");

        //Create Property Contacts Elements
        private readonly By managementContactDetailsTitle = By.XPath("//div[contains(text(),'Contact Details')]");
        private readonly By managementContactLabel = By.XPath("//div[contains(text(),'Contact Details')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Contact')]");
        private readonly By managementContactsDiv = By.XPath("//div[contains(text(),'Select from contacts')]");
        private readonly By managementContactsButton = By.XPath("//div[contains(text(),'Select from contacts')]/parent::div/following-sibling::div/button");
        private readonly By managementContactsPrimaryContactLabel = By.XPath("//label[contains(text(),'Primary contact')]");
        private readonly By managementContactPrimaryContactSelect = By.Id("input-primaryContactId");
        private readonly By managementContactsPurposeDescriptionLabel = By.XPath("//label[contains(text(),'Purpose description')]");
        private readonly By managementContactsPurposeDescriptionTextarea = By.Id("input-purposeDescription");

        //View Activity List Elements
        private readonly By managementActivitiesTitle = By.XPath("//div[contains(text(),'Activities List')]");
        private readonly By managementeAddActivityBttn = By.XPath("//div[contains(text(),'Activities List')]/following-sibling::div/button");
        private readonly By managementActivitiesTable = By.CssSelector("div[data-testid='PropertyManagementActivitiesTable']");
        private readonly By managementActivitiesBodyCount = By.CssSelector("div[data-testid='PropertyManagementActivitiesTable'] div[class='tbody'] div[class='tr-wrapper']");
        private readonly By managementActivitiesDeleteBttns = By.CssSelector("button[title='Delete']");
        private readonly By managementActivityPaginationOptions = By.XPath("//div[@data-testid='PropertyManagementActivitiesTable']/following-sibling::div/div/ul[@class='pagination']/li");
        

        //Create Activity Elements
        //Activity Details
        private readonly By managementActCloseTrayBttn = By.CssSelector("button[id='close-tray']");
        private readonly By managementActEditButton = By.CssSelector("button[title='Edit property activity']");
        private readonly By managementActivityDetailsTitle = By.XPath("//div[contains(text(),'Activity Details')]");
        private readonly By managementActTypeLabel = By.XPath("//label[contains(text(),'Activity type')]");
        private readonly By managementActTypeInput = By.Id("input-activityTypeCode");
        private readonly By managementActTypeContent = By.XPath("//label[contains(text(),'Activity type')]/parent::div/following-sibling::div");
        private readonly By managementActSubTypeLabel = By.XPath("//label[contains(text(),'Sub-type')]");
        private readonly By managementActSubTypeInput = By.Id("input-activitySubtypeCode");
        private readonly By managementActSubTypeContent = By.XPath("//label[contains(text(),'Sub-type')]/parent::div/following-sibling::div");
        private readonly By managementActStatusLabel = By.XPath("//label[contains(text(),'Activity status')]");
        private readonly By managementActStatusInput = By.Id("input-activityStatusCode");
        private readonly By managementActStatusContent = By.XPath("//label[contains(text(),'Activity status')]/parent::div/following-sibling::div");
        private readonly By managementActRequestAddedDateLabel = By.XPath("//label[contains(text(),'Requested added date')]");
        private readonly By managementActRequestAddedDateInput = By.Id("datepicker-requestedDate");
        private readonly By managementActRequestAddedDateContent = By.XPath("//label[contains(text(),'Requested added date')]/parent::div/following-sibling::div");
        private readonly By managementActCompletionDateLabel = By.XPath("//label[contains(text(),'Completion date')]");
        private readonly By managementActCompletionDateInput = By.Id("datepicker-completionDate");
        private readonly By managementActCompletionDateContent = By.XPath("//label[contains(text(),'Completion date')]/parent::div/following-sibling::div");
        private readonly By managementActDescriptionLabel = By.XPath("//div[contains(text(),'Activity Details')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Description')]");
        private readonly By managementActDescriptionInput = By.Id("input-description");
        private readonly By managementActDescriptionContent = By.XPath("//div[contains(text(),'Activity Details')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Description')]/parent::div/following-sibling::div");
        private readonly By managementActMinistryContactLabel = By.XPath("//label[contains(text(),'Ministry contacts')]");
        private readonly By managementActMinistryContactInput = By.XPath("//label[contains(text(),'Ministry contacts')]/parent::div/following-sibling::div/div/div/div/div/div//div[contains(text(),'Select from contacts')]");
        private readonly By managementActMinistryContactBttn = By.XPath("//label[contains(text(),'Ministry contacts')]/parent::div/following-sibling::div/div/div/div/div/div/button");
        private readonly By managementActMinistryContactAddContactLink = By.XPath("//label[contains(text(),'Ministry contacts')]/parent::div/following-sibling::div/div/following-sibling::button");
        private readonly By managementActMinistryContactDeleteBttns = By.XPath("//label[contains(text(),'Ministry contacts')]/parent::div/following-sibling::div/div/div/button");
        private readonly By managementActMinistryContactContent = By.XPath("//label[contains(text(),'Ministry contacts')]/parent::div/following-sibling::div/a/span");
        private readonly By managementActRequestedSourceLabel = By.XPath("//label[contains(text(),'Requested source')]");
        private readonly By managementActRequestedSourceInput = By.Id("input-requestedSource");
        private readonly By managementActRequestedSourceContent = By.XPath("//label[contains(text(),'Requested source')]/parent::div/following-sibling::div");
        private readonly By managementActInvolvedPartiesLabel = By.XPath("//label[contains(text(),'Involved parties')]");
        private readonly By managementActInvolvedPartiesInput = By.XPath("//label[contains(text(),'Involved parties')]/parent::div/following-sibling::div/div/div/div/div/div//div[contains(text(),'Select from contacts')]");
        private readonly By managementActInvolvedPartiesBttn = By.XPath("//label[contains(text(),'Involved parties')]/parent::div/following-sibling::div/div/div/div/div/div/button");
        private readonly By managementActInvolvedPartiesAddContactLink = By.XPath("//label[contains(text(),'Involved parties')]/parent::div/following-sibling::div/div/following-sibling::button");
        private readonly By managementActInvolvedPartiesDeleteBttns = By.XPath("//label[contains(text(),'Involved parties')]/parent::div/following-sibling::div/div/div/button");
        private readonly By managementActInvolvedPartiesContent = By.XPath("//label[contains(text(),'Involved parties')]/parent::div/following-sibling::div/a/span");
        private readonly By managementActServiceProviderLabel = By.XPath("//label[contains(text(),'Service provider')]");
        private readonly By managementActServiceProviderInput = By.XPath("//label[contains(text(),'Service provider')]/parent::div/following-sibling::div/div/div/div/div[contains(text(),'Select from contacts')]");
        private readonly By managementActServiceProviderBttn = By.XPath("//label[contains(text(),'Service provider')]/parent::div/following-sibling::div/div/div/div/button");
        private readonly By managementActServiceProviderContent = By.XPath("//label[contains(text(),'Service provider')]/parent::div/following-sibling::div/a/span");

        private readonly By managementActInvoiceTotalsSubtitle = By.XPath("//div[contains(text(),'Invoices Total')]");
        private readonly By managementActInvoiceTotalPretaxLabel = By.XPath("//div[contains(text(),'Invoices Total')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Total (before tax)')]");
        private readonly By managementActInvoiceTotalPretaxContent = By.XPath("//div[contains(text(),'Invoices Total')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Total (before tax)')]/parent::div/following-sibling::div");
        private readonly By managementActInvoiceTotalGSTLabel = By.XPath("//div[contains(text(),'Invoices Total')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'GST amount')]");
        private readonly By managementActInvoiceTotalGSTContent = By.XPath("//div[contains(text(),'Invoices Total')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'GST amount')]/parent::div/following-sibling::div");
        private readonly By managementActInvoiceTotalPSTLabel = By.XPath("//div[contains(text(),'Invoices Total')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'PST amount')]");
        private readonly By managementActInvoiceTotalPSTContent = By.XPath("//div[contains(text(),'Invoices Total')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'PST amount')]/parent::div/following-sibling::div");
        private readonly By managementActInvoiceGrandTotalLabel = By.XPath("//div[contains(text(),'Invoices Total')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Total amount')]");
        private readonly By managementActInvoiceGrandTotalContent = By.XPath("//div[contains(text(),'Invoices Total')]/parent::div/parent::h2/following-sibling::div/div/div/label[contains(text(),'Total amount')]/parent::div/following-sibling::div");

        //Invoices View Element
        private readonly By managementAddInvoiceBttn = By.XPath("//div[contains(text(),'Invoices Total')]/following-sibling::div/button");
        private readonly By managementInvoicesTotalSubtitle = By.XPath("//div[contains(text(),'Invoices Total')]");
        private readonly By managementActPretaxAmountLabel = By.XPath("//label[contains(text(),'Total (before tax)')]");
        private readonly By managementActPretaxAmountInput = By.Id("input-pretaxAmount");
        private readonly By managementActGSTAmountLabel = By.XPath("//label[contains(text(),'GST amount')]");
        private readonly By managementActGSTAmountInput = By.Id("input-gstAmount");
        private readonly By managementActPSTAmountLabel = By.XPath("//label[contains(text(),'PST amount')]");
        private readonly By managementActPSTAmountInput = By.Id("input-pstAmount");
        private readonly By managementActTotalAmountLabel = By.XPath("//label[contains(text(),'Total amount')]");
        private readonly By managementActTotalAmountInput = By.Id("input-totalAmount");

        private readonly By managementActAddDocumentBttn = By.XPath("//div[contains(text(),'Documents')]/parent::div/div/button");

        private SharedModals sharedModals;
        private SharedSelectContact sharedSelectContact;

        public PropertyManagementTab(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
            sharedSelectContact = new SharedSelectContact(webDriver);
        }

        public void NavigateManagementTab()
        {
            WaitUntilSpinnerDisappear();
            WaitUntilClickable(managementTabLink);
            webDriver.FindElement(managementTabLink).Click();
        }

        public void UpdateManagementSummaryButton()
        {
            WaitUntilClickable(managementSummaryEditBttn);
            webDriver.FindElement(managementSummaryEditBttn).Click();
        }

        public void UpdateLastContactButton()
        {
            WaitUntilTableSpinnerDisappear();

            var lastInsertedContactIndex = webDriver.FindElements(managementContactsBodyCount).Count;
            webDriver.FindElement(By.CssSelector("div[data-testid='PropertyContactsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ lastInsertedContactIndex +") div[role='cell']:nth-child(4) button:nth-child(1)")).Click();

        }

        public void ViewLastActivityButton()
        {
            Wait();

            var lastInsertedActivityIndex = webDriver.FindElements(managementActivitiesBodyCount).Count;
            webDriver.FindElement(By.XPath("//div[@data-testid='PropertyManagementActivitiesTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ lastInsertedActivityIndex +"]/div/div[@role='cell'][5]/div/button[1]")).Click();

        }

        public void UpdateSelectedActivity()
        {
            WaitUntilClickable(managementActEditButton);
            webDriver.FindElement(managementActEditButton).Click();
            
        }

        public void AddNewPropertyContactButton()
        {
            WaitUntilClickable(managementeAddContactBttn);
            webDriver.FindElement(managementeAddContactBttn).Click();
        }

        public void AddNewPropertyActivityButton()
        {
            WaitUntilClickable(managementeAddActivityBttn);
            webDriver.FindElement(managementeAddActivityBttn).Click();
        }

        public void AddNewDocumentButton()
        {
            WaitUntilVisible(managementActAddDocumentBttn);
            webDriver.FindElement(managementActAddDocumentBttn).Click();
        }

        public void CloseActivityTray()
        {
            WaitUntilClickable(managementActCloseTrayBttn);
            webDriver.FindElement(managementActCloseTrayBttn).Click();
        }

        public void InsertManagementSummaryInformation(PropertyManagement managementProperty)
        {
            Wait();

            //Delete Property Purpose if any
            if (webDriver.FindElements(managementPropertyPurposeDeleteBttns).Count > 0)
            {
                while (webDriver.FindElements(managementPropertyPurposeDeleteBttns).Count > 0)
                {
                    webDriver.FindElements(managementPropertyPurposeDeleteBttns)[0].Click();
                }
                webDriver.FindElement(managementPropertyPurposeLabel).Click();
            }
            //Add Property Purpose
            if (managementProperty.ManagementPropertyPurpose.First() != "")
            {
                ClearMultiSelectInput(managementPropertyPurposeInput);
                foreach (string purpose in managementProperty.ManagementPropertyPurpose)
                {
                    webDriver.FindElement(managementPropertyPurposeLabel).Click();
                    FocusAndClick(managementPropertyPurposeInput);

                    WaitUntilClickable(managementPropertyPurposeOptions);
                    ChooseMultiSelectSpecificOption(managementPropertyPurposeOptions, purpose);
                    webDriver.FindElement(managementPropertyPurposeLabel).Click();
                }
            }
            if (managementProperty.ManagementUtilitiesPayable != "")
                ChooseSpecificSelectOption(managementUtilitiesPayableSelect, managementProperty.ManagementUtilitiesPayable);

            if (managementProperty.ManagementTaxesPayable != "")
                ChooseSpecificSelectOption(managementTaxesPayableSelect, managementProperty.ManagementTaxesPayable);

            ClearInput(managementAdditionalDetailsTextarea);
            if (managementProperty.ManagementPropertyAdditionalDetails != "")
                webDriver.FindElement(managementAdditionalDetailsTextarea).SendKeys(managementProperty.ManagementPropertyAdditionalDetails);
            
        }

        public void InsertNewPropertyContact(PropertyContact contact)
        {
            Wait();

            //Choosing a contact
            webDriver.FindElement(managementContactsButton).Click();
            sharedSelectContact.SelectContact(contact.PropertyContactFullName, contact.PropertyContactType);

            //Choosing Primary Contact
            Wait();
            if (contact.PropertyPrimaryContact != "" && webDriver.FindElements(managementContactPrimaryContactSelect).Count > 0)
                ChooseSpecificSelectOption(managementContactPrimaryContactSelect, contact.PropertyPrimaryContact);

            //Inserting Purpose Description
            webDriver.FindElement(managementContactsPurposeDescriptionTextarea).Click();
            ClearInput(managementContactsPurposeDescriptionTextarea);
            webDriver.FindElement(managementContactsPurposeDescriptionTextarea).SendKeys(contact.PropertyContactPurposeDescription);
        }

        public void UpdatePropertyContact(PropertyContact contact)
        {
            Wait();

            //Update Purpose Description
            webDriver.FindElement(managementContactsPurposeDescriptionTextarea).Click();
            ClearInput(managementContactsPurposeDescriptionTextarea);
            webDriver.FindElement(managementContactsPurposeDescriptionTextarea).SendKeys(contact.PropertyContactPurposeDescription);
        }

        public void InsertNewPropertyActivity(PropertyActivity activity)
        {
            Wait();

            //Choosing Activity type, Sub-type, status
            ChooseSpecificSelectOption(managementActTypeInput, activity.PropertyActivityType);

            Wait();
            ChooseSpecificSelectOption(managementActSubTypeInput, activity.PropertyActivitySubType);

            ChooseSpecificSelectOption(managementActStatusInput, activity.PropertyActivityStatus);

            //Inserting Requested Added Date
            ClearInput(managementActRequestAddedDateInput);
            webDriver.FindElement(managementActRequestAddedDateInput).Click();
            webDriver.FindElement(managementActRequestAddedDateInput).SendKeys(activity.PropertyActivityRequestedDate);
            webDriver.FindElement(managementActRequestAddedDateInput).SendKeys(Keys.Enter);

            //Inserting Completion Date
            if (activity.PropertyActivityCompletionDate != null)
            {
                webDriver.FindElement(managementActCompletionDateInput).SendKeys(activity.PropertyActivityCompletionDate);
                webDriver.FindElement(managementActCompletionDateInput).SendKeys(Keys.Enter);
            }

            //Inserting Description
            ClearInput(managementActDescriptionInput);
            webDriver.FindElement(managementActDescriptionInput).SendKeys(activity.PropertyActivityDescription);

            //Deleting previous Ministry Contacts and adding new
            while (webDriver.FindElements(managementActMinistryContactDeleteBttns).Count > 0)
                webDriver.FindElements(managementActMinistryContactDeleteBttns)[0].Click();

            if (activity.PropertyActivityMinistryContact.First() != "")
            {
                webDriver.FindElement(managementActMinistryContactBttn).Click();
                sharedSelectContact.SelectContact(activity.PropertyActivityMinistryContact[0], "");

                if (activity.PropertyActivityMinistryContact.Count > 1)
                {
                    for (int i = 1; i < activity.PropertyActivityMinistryContact.Count; i++)
                    {
                        var elementNumber = i + 1;
                        webDriver.FindElement(managementActMinistryContactAddContactLink).Click();

                        WaitUntilVisible(By.XPath("//label[contains(text(),'Ministry contacts')]/parent::div/following-sibling::div/div["+ elementNumber +"]/div/div/div/div/button"));
                        webDriver.FindElement(By.XPath("//label[contains(text(),'Ministry contacts')]/parent::div/following-sibling::div/div["+ elementNumber +"]/div/div/div/div/button")).Click();
                        sharedSelectContact.SelectContact(activity.PropertyActivityMinistryContact[i], "");
                    } 
                }
            }

            //Inserting Requested Source
            if (activity.PropertyActivityRequestedSource != string.Empty)
            {
                ClearInput(managementActRequestedSourceInput);
                webDriver.FindElement(managementActRequestedSourceInput).Click();
                webDriver.FindElement(managementActRequestedSourceInput).SendKeys(activity.PropertyActivityRequestedSource);
            }
                

            //Deleting Involved parties and adding new
            while (webDriver.FindElements(managementActInvolvedPartiesDeleteBttns).Count > 0)
                webDriver.FindElements(managementActInvolvedPartiesDeleteBttns)[0].Click();

            if (activity.PropertyActivityInvolvedParties.First() != "")
            {
                webDriver.FindElement(managementActInvolvedPartiesBttn).Click();
                sharedSelectContact.SelectContact(activity.PropertyActivityInvolvedParties[0], "");

                if (activity.PropertyActivityInvolvedParties.Count > 1)
                {
                    for (int i = 1; i < activity.PropertyActivityInvolvedParties.Count; i++)
                    {
                        var elementNumber = i + 1;
                        webDriver.FindElement(managementActInvolvedPartiesAddContactLink).Click();

                        WaitUntilVisible(By.XPath("//label[contains(text(),'Involved parties')]/parent::div/following-sibling::div/div["+ elementNumber +"]/div/div/div/div/button"));
                        webDriver.FindElement(By.XPath("//label[contains(text(),'Involved parties')]/parent::div/following-sibling::div/div["+ elementNumber +"]/div/div/div/div/button")).Click();
                        sharedSelectContact.SelectContact(activity.PropertyActivityInvolvedParties[i], "");
                    }
                }
            }

            if (activity.PropertyActivityServiceProvider != "")
            {
                webDriver.FindElement(managementActServiceProviderBttn).Click();
                sharedSelectContact.SelectContact(activity.PropertyActivityServiceProvider, "");
            }

            if (activity.ManagementPropertyActivityInvoices.Count > 0)
                for (int i = 0; i < activity.ManagementPropertyActivityInvoices.Count; i++)
                    AddInvoice(activity.ManagementPropertyActivityInvoices[i], i);
        }

        public void SavePropertyManagement()
        {
            ButtonElement("Save");
            WaitUntilVisible(managementSummaryEditBttn);
        }

        public void CancelPropertyManagement()
        {
            ButtonElement("Cancel");

            Wait();
            Assert.Equal("Confirm Changes", sharedModals.ModalHeader());
            Assert.Contains("If you choose to cancel now, your changes will not be saved.", sharedModals.ModalContent());
            Assert.Contains("Do you want to proceed?", sharedModals.ModalContent());

            sharedModals.ModalClickOKBttn();
        }

        public void DeleteAllContacts()
        {
            Wait();

            while (webDriver.FindElements(managementContactsDeleteBttns).Count > 0)
            {
                Wait();
                webDriver.FindElement(managementContactsFirstDeleteBttn).Click();

                Assert.Equal("Confirm delete", sharedModals.ModalHeader());
                Assert.Equal("This contact will be removed from the Property contacts. Do you wish to proceed?", sharedModals.ModalContent());
                sharedModals.ModalClickOKBttn();

                WaitUntilTableSpinnerDisappear();
            }
        }

        public void DeleteAllActivities()
        {
            WaitUntilTableSpinnerDisappear();

            while (webDriver.FindElements(managementActivitiesDeleteBttns).Count > 0)
            {
                Wait();
                webDriver.FindElements(managementActivitiesDeleteBttns)[0].Click();

                Wait();
                Assert.Equal("Confirm Delete", sharedModals.ModalHeader());
                Assert.Equal("Are you sure you want to delete this item?", sharedModals.ModalContent());
                sharedModals.ModalClickOKBttn();

                WaitUntilTableSpinnerDisappear();
            }
        }

        public void ViewLastActivityFromList()
        {
            var paginationLastPage = webDriver.FindElements(managementActivityPaginationOptions).Count() -1;

            webDriver.FindElement(By.XPath("//div[@data-testid='PropertyManagementActivitiesTable']/following-sibling::div/div/ul[@class='pagination']/li["+ paginationLastPage +"]")).Click();
        }

        public void VerifyInitManagementTabView()
        {
            //Summary
            AssertTrueIsDisplayed(managementSummaryTitle);
            AssertTrueIsDisplayed(managementPropertyPurposeLabel);
            AssertTrueIsDisplayed(managementLeaseLabel);
            AssertTrueContentEquals(managementLeaseContent, "No");
            AssertTrueIsDisplayed(managementUtilitiesPayableLabel);
            AssertTrueContentEquals(managementUtilitiesPayableContent, "Unknown");
            AssertTrueIsDisplayed(managementTaxesPayableLabel);
            AssertTrueContentEquals(managementTaxesPayableContent, "Unknown");
            AssertTrueIsDisplayed(managementAdditionalDetailsLabel);

            //Property Contacts
            AssertTrueIsDisplayed(managementContactTitle);
            AssertTrueIsDisplayed(managementeAddContactBttn);
            AssertTrueIsDisplayed(managementContactsTable);

            //Activities List
            AssertTrueIsDisplayed(managementActivitiesTitle);
            AssertTrueIsDisplayed(managementeAddActivityBttn);
            AssertTrueIsDisplayed(managementActivitiesTable);
        }

        public void VerifyCreateSummaryInitForm()
        {
            AssertTrueIsDisplayed(managementSummaryTitle);
            AssertTrueIsDisplayed(managementPropertyPurposeLabel);
            AssertTrueIsDisplayed(managementPropertyPurposeInput);
            AssertTrueIsDisplayed(managementCreateLeaseLabel);
            AssertTrueIsDisplayed(managementUtilitiesPayableLabel);
            AssertTrueIsDisplayed(managementUtilitiesPayableSelect);
            AssertTrueIsDisplayed(managementTaxesPayableLabel);
            AssertTrueIsDisplayed(managementTaxesPayableSelect);
            AssertTrueIsDisplayed(managementAdditionalDetailsLabel);
            AssertTrueIsDisplayed(managementAdditionalDetailsTextarea);
        }

        public void VerifyCreateContactsInitForm()
        {
            AssertTrueIsDisplayed(managementContactDetailsTitle);
            AssertTrueIsDisplayed(managementContactLabel);
            AssertTrueIsDisplayed(managementContactsDiv);
            AssertTrueIsDisplayed(managementContactsButton);
            AssertTrueIsDisplayed(managementContactsPrimaryContactLabel);
            AssertTrueIsDisplayed(managementContactsPurposeDescriptionLabel);
            AssertTrueIsDisplayed(managementContactsPurposeDescriptionTextarea);
        }

        public void VerifyCreateActivityInitForm()
        {
            //Activity Details
            AssertTrueIsDisplayed(managementActivityDetailsTitle);
            AssertTrueIsDisplayed(managementActTypeLabel);
            AssertTrueIsDisplayed(managementActTypeInput);
            AssertTrueIsDisplayed(managementActSubTypeLabel);
            AssertTrueIsDisplayed(managementActSubTypeInput);
            AssertTrueIsDisplayed(managementActStatusLabel);
            AssertTrueIsDisplayed(managementActStatusInput);
            AssertTrueIsDisplayed(managementActRequestAddedDateLabel);
            AssertTrueIsDisplayed(managementActRequestAddedDateInput);
            AssertTrueIsDisplayed(managementActCompletionDateLabel);
            AssertTrueIsDisplayed(managementActCompletionDateInput);
            AssertTrueIsDisplayed(managementActDescriptionLabel);
            AssertTrueIsDisplayed(managementActDescriptionInput);
            AssertTrueIsDisplayed(managementActMinistryContactLabel);
            AssertTrueIsDisplayed(managementActMinistryContactInput);
            AssertTrueIsDisplayed(managementActMinistryContactBttn);
            AssertTrueIsDisplayed(managementActMinistryContactAddContactLink);
            AssertTrueIsDisplayed(managementActRequestedSourceLabel);
            AssertTrueIsDisplayed(managementActRequestedSourceInput);
            AssertTrueIsDisplayed(managementActInvolvedPartiesLabel);
            AssertTrueIsDisplayed(managementActInvolvedPartiesInput);
            AssertTrueIsDisplayed(managementActInvolvedPartiesBttn);
            AssertTrueIsDisplayed(managementActInvolvedPartiesAddContactLink);
            AssertTrueIsDisplayed(managementActServiceProviderLabel);
            AssertTrueIsDisplayed(managementActServiceProviderInput);
            AssertTrueIsDisplayed(managementActServiceProviderBttn);

            //Invoice
            AssertTrueIsDisplayed(managementAddInvoiceBttn);
            AssertTrueIsDisplayed(managementInvoicesTotalSubtitle);
            AssertTrueIsDisplayed(managementActPretaxAmountLabel);
            AssertTrueIsDisplayed(managementActPretaxAmountInput);
            AssertTrueIsDisplayed(managementActGSTAmountLabel);
            AssertTrueIsDisplayed(managementActGSTAmountInput);
            AssertTrueIsDisplayed(managementActPSTAmountLabel);
            AssertTrueIsDisplayed(managementActPSTAmountInput);
            AssertTrueIsDisplayed(managementActTotalAmountLabel);
            AssertTrueIsDisplayed(managementActTotalAmountInput);
        }

        public void VerifyInsertedSummaryForm(PropertyManagement managementProperty)
        {
            Wait();

            AssertTrueIsDisplayed(managementSummaryTitle);
            AssertTrueIsDisplayed(managementPropertyPurposeLabel);

            if (webDriver.FindElements(managementPropertyPurposeContent).Count > 0)
            {
                var selectedPurposes = webDriver.FindElements(managementPropertyPurposeContent);
                for (int i = 0; i < selectedPurposes.Count; i++)
                {
                    Assert.Equal(managementProperty.ManagementPropertyPurpose[i], selectedPurposes[i].Text);
                }
            }

            AssertTrueIsDisplayed(managementLeaseLabel);
            AssertTrueIsDisplayed(managementUtilitiesPayableLabel);
            AssertTrueContentEquals(managementUtilitiesPayableContent, managementProperty.ManagementUtilitiesPayable);
            AssertTrueIsDisplayed(managementTaxesPayableLabel);
            AssertTrueContentEquals(managementTaxesPayableContent, managementProperty.ManagementTaxesPayable);
            AssertTrueIsDisplayed(managementAdditionalDetailsLabel);
            AssertTrueContentEquals(managementAdditionalDetailsContent, managementProperty.ManagementPropertyAdditionalDetails);
        }

        public void VerifyLastInsertedPropertyContactTable(PropertyContact contact)
        {
            Wait(3000);

            var lastInsertedContactIndex = webDriver.FindElements(managementContactsBodyCount).Count;

            AssertTrueContentEquals(By.CssSelector("div[data-testid='PropertyContactsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ lastInsertedContactIndex +") div[role='cell']:nth-child(1) a"), contact.PropertyContactFullName);

            if(webDriver.FindElements(By.CssSelector("div[data-testid='PropertyContactsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ lastInsertedContactIndex +") div[role='cell']:nth-child(2) a")).Count > 0 )
                AssertTrueContentEquals(By.CssSelector("div[data-testid='PropertyContactsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ lastInsertedContactIndex +") div[role='cell']:nth-child(2) a"), contact.PropertyPrimaryContact);
            else
                AssertTrueContentEquals(By.CssSelector("div[data-testid='PropertyContactsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ lastInsertedContactIndex +") div[role='cell']:nth-child(2)"), contact.PropertyPrimaryContact);

            AssertTrueContentEquals(By.CssSelector("div[data-testid='PropertyContactsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ lastInsertedContactIndex +") div[role='cell']:nth-child(3)"), contact.PropertyContactPurposeDescription);

            AssertTrueIsDisplayed(By.CssSelector("div[data-testid='PropertyContactsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ lastInsertedContactIndex +") div[role='cell']:nth-child(4) button:nth-child(1)"));
            AssertTrueIsDisplayed(By.CssSelector("div[data-testid='PropertyContactsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ lastInsertedContactIndex +") div[role='cell']:nth-child(4) button[title='Delete contact']"));
        }

        public void VerifyInsertedActivity(PropertyActivity activity)
        {
            Wait(2000);

            //Activity Details section
            AssertTrueIsDisplayed(managementActivityDetailsTitle);

            AssertTrueIsDisplayed(managementActTypeLabel);
            AssertTrueContentEquals(managementActTypeContent, activity.PropertyActivityType);

            AssertTrueIsDisplayed(managementActSubTypeLabel);
            AssertTrueContentEquals(managementActSubTypeContent, activity.PropertyActivitySubType);

            AssertTrueIsDisplayed(managementActStatusLabel);
            AssertTrueContentEquals(managementActStatusContent, activity.PropertyActivityStatus);

            AssertTrueIsDisplayed(managementActRequestAddedDateLabel);
            AssertTrueContentEquals(managementActRequestAddedDateContent,TransformDateFormat(activity.PropertyActivityRequestedDate));

            if (activity.PropertyActivityCompletionDate != null)
            {
                AssertTrueIsDisplayed(managementActCompletionDateLabel);
                AssertTrueContentEquals(managementActCompletionDateContent, TransformDateFormat(activity.PropertyActivityCompletionDate));
            }
            
            AssertTrueIsDisplayed(managementActDescriptionLabel);
            AssertTrueContentEquals(managementActDescriptionContent, activity.PropertyActivityDescription);

            AssertTrueIsDisplayed(managementActMinistryContactLabel);
            if (activity.PropertyActivityMinistryContact.First() != "")
                for(int i = 0; i < activity.PropertyActivityMinistryContact.Count; i++)
                    Assert.Equal(webDriver.FindElements(managementActMinistryContactContent)[i].Text, activity.PropertyActivityMinistryContact[i]);

            AssertTrueIsDisplayed(managementActRequestedSourceLabel);
            if (activity.PropertyActivityRequestedSource != "")
                AssertTrueContentEquals(managementActRequestedSourceContent, activity.PropertyActivityRequestedSource);

            AssertTrueIsDisplayed(managementActInvolvedPartiesLabel);
            if (activity.PropertyActivityInvolvedParties.First() != "")
                for (int i = 0; i < activity.PropertyActivityInvolvedParties.Count; i++)
                    Assert.Equal(webDriver.FindElements(managementActInvolvedPartiesContent)[i].Text, activity.PropertyActivityInvolvedParties[i]);

            AssertTrueIsDisplayed(managementActServiceProviderLabel);
            if(activity.PropertyActivityServiceProvider != "")
                AssertTrueContentEquals(managementActServiceProviderContent, activity.PropertyActivityServiceProvider);

            //Invoices section
            //AssertTrueIsDisplayed(managementActInvoiceTotalsSubtitle);

            //AssertTrueIsDisplayed(managementActInvoiceTotalPretaxLabel);
            //AssertTrueContentEquals(managementActInvoiceTotalPretaxContent, TransformCurrencyFormat(activity.ManagementPropertyActivityTotalPreTax));

            //AssertTrueIsDisplayed(managementActInvoiceTotalGSTLabel);
            //AssertTrueContentEquals(managementActInvoiceTotalGSTContent, TransformCurrencyFormat(activity.ManagementPropertyActivityTotalGST));

            //AssertTrueIsDisplayed(managementActInvoiceTotalPSTLabel);
            //AssertTrueContentEquals(managementActInvoiceTotalPSTContent, TransformCurrencyFormat(activity.ManagementPropertyActivityTotalPST));

            //AssertTrueIsDisplayed(managementActInvoiceGrandTotalLabel);
            //AssertTrueContentEquals(managementActInvoiceGrandTotalContent, TransformCurrencyFormat(activity.ManagementPropertyActivityGrandTotal)); 
        }

        public void VerifyLastInsertedActivityTable(PropertyActivity activity)
        {
            //WaitUntilSpinnerDisappear();

            var lastInsertedActivityIndex = webDriver.FindElements(managementActivitiesBodyCount).Count;

            AssertTrueContentEquals(By.CssSelector("div[data-testid='PropertyManagementActivitiesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ lastInsertedActivityIndex +") div[role='cell']:nth-child(1)"), activity.PropertyActivityType);
            AssertTrueContentEquals(By.CssSelector("div[data-testid='PropertyManagementActivitiesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ lastInsertedActivityIndex +") div[role='cell']:nth-child(2)"), activity.PropertyActivitySubType);
            AssertTrueContentEquals(By.CssSelector("div[data-testid='PropertyManagementActivitiesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ lastInsertedActivityIndex +") div[role='cell']:nth-child(3)"), activity.PropertyActivityStatus);
            AssertTrueContentEquals(By.CssSelector("div[data-testid='PropertyManagementActivitiesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ lastInsertedActivityIndex +") div[role='cell']:nth-child(4)"), TransformDateFormat(activity.PropertyActivityRequestedDate));
            Assert.True(webDriver.FindElements(By.CssSelector("div[data-testid='PropertyManagementActivitiesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child("+ lastInsertedActivityIndex +") div[role='cell']:nth-child(5) button")).Count > 0);
        }

        private void AddInvoice(ManagementPropertyActivityInvoice invoice, int index)
        {
            Wait();
            webDriver.FindElement(managementAddInvoiceBttn).Click();

            Wait();
            webDriver.FindElement(By.Id("input-invoices."+ index +".invoiceNum")).SendKeys(invoice.PropertyActivityInvoiceNumber);

            webDriver.FindElement(By.Id("datepicker-invoices."+ index +".invoiceDateTime")).SendKeys(invoice.PropertyActivityInvoiceDate);
            webDriver.FindElement(By.Id("datepicker-invoices."+ index +".invoiceDateTime")).SendKeys(Keys.Enter);

            webDriver.FindElement(By.Id("input-invoices."+ index +".description")).SendKeys(invoice.PropertyActivityInvoiceDescription);

            ClearInput(By.Id("input-invoices."+ index +".pretaxAmount"));
            webDriver.FindElement(By.Id("input-invoices."+ index +".pretaxAmount")).SendKeys(invoice.PropertyActivityInvoicePretaxAmount);

            ChooseSpecificSelectOption(By.Id("input-invoices."+ index +".isPstRequired"), invoice.PropertyActivityInvoicePSTApplicable);

            AssertTrueElementValueEquals(By.Id("input-invoices."+ index +".gstAmount"), TransformCurrencyFormat(invoice.PropertyActivityInvoiceGSTAmount));

            if(invoice.PropertyActivityInvoicePSTAmount != "0.00")
                AssertTrueElementValueEquals(By.Id("input-invoices."+ index +".pstAmount"), TransformCurrencyFormat(invoice.PropertyActivityInvoicePSTAmount));
        }
    }
}
