using AventStack.ExtentReports.Model;
using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        private readonly By managementAddActivityBttn = By.XPath("//div[contains(text(),'Activities List')]/following-sibling::div/button");
        private readonly By managementFileActivitiesTitle = By.CssSelector("div[data-testid='ad-hoc activities']");
        private readonly By managementActivitiesListTable = By.CssSelector("div[data-testid='adhoc-activity-list']");
        private readonly By managementActivitiesListTableActivityTypeColumn = By.CssSelector("div[data-testid='adhoc-activity-list'] div[class='thead thead-light'] div[role='columnheader']:nth-child(1) div");
        private readonly By managementActivitiesListActivityTypeSortBttn = By.CssSelector("div[data-testid='sort-column-activityType']");
        private readonly By managementActivitiesListTableActivitySubtypeColumn = By.CssSelector("div[data-testid='adhoc-activity-list'] div[class='thead thead-light'] div[role='columnheader']:nth-child(2) div");
        private readonly By managementActivitiesListActivitySubtypeSortBttn = By.CssSelector("div[data-testid='sort-column-activitySubTypes']");
        private readonly By managementActivitiesListTableActivityStatusColumn = By.CssSelector("div[data-testid='adhoc-activity-list'] div[class='thead thead-light'] div[role='columnheader']:nth-child(3) div");
        private readonly By managementActivitiesListActivityStatusSortBttn = By.CssSelector("div[data-testid='sort-column-activityStatusType']");
        private readonly By managementActivitiesListTableCommencementColumn = By.CssSelector("div[data-testid='adhoc-activity-list'] div[class='thead thead-light'] div[role='columnheader']:nth-child(4) div");
        private readonly By managementActivitiesListActivityCommencementSortBttn = By.CssSelector("div[data-testid='sort-column-requestedAddedDate]");
        private readonly By managementActivitiesListTableActionsColumn = By.CssSelector("div[data-testid='adhoc-activity-list'] div[class='thead thead-light'] div[role='columnheader']:nth-child(5) div");
        private readonly By managementActivitiesListTable1stActTypeContext = By.CssSelector("div[data-testid='adhoc-activity-list'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[role='cell']:nth-child(1)");
        private readonly By managementActivitiesListTable1stActSubtypeContext = By.CssSelector("div[data-testid='adhoc-activity-list'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[role='cell']:nth-child(2)");
        private readonly By managementActivitiesListTable1stActStatusContext = By.CssSelector("div[data-testid='adhoc-activity-list'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[role='cell']:nth-child(3)");
        private readonly By managementActivitiesListTable1stActCommencementContext = By.CssSelector("div[data-testid='adhoc-activity-list'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[role='cell']:nth-child(4)");
        private readonly By managementActivitiesListTable1stActViewBttn = By.CssSelector("div[data-testid='adhoc-activity-list'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[role='cell']:nth-child(5) button[title='property-activity view details']");
        private readonly By managementActivitiesListTable1stActDeleteBttn = By.CssSelector("div[data-testid='adhoc-activity-list'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[role='cell']:nth-child(5) button[title='Delete']");
        private readonly By managementActivitiesListTable1stActWarning = By.CssSelector("div[data-testid='adhoc-activity-list'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[role='cell']:nth-child(5) svg");
        private readonly By managementActivitiesBodyCount = By.CssSelector("div[data-testid='adhoc-activity-list'] div[class='tbody'] div[class='tr-wrapper']");
        private readonly By managementActivitiesDeleteBttns = By.CssSelector("button[title='Delete']");
        private readonly By managementActivityPaginationOptions = By.XPath("//div[contains(text(),'Ad-hoc Activities List')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div[@data-testid='PropertyManagementActivitiesTable']/following-sibling::div/div/ul[@class='pagination']/li");

        //View Property File Activity Summary Elements
        private readonly By activitiesFileListSubtitle = By.XPath("//div[contains(text(),'Property File Activity Summary')]");
        private readonly By activitiesFileListTooltip = By.CssSelector("span[data-testid='tooltip-icon-property-file-activity-summary']");
        private readonly By activitiesFileListTableExpandBttn = By.XPath("//div[contains(text(),'Property File Activity Summary')]/parent::div/parent::div/parent::div/following-sibling::div");
        private readonly By activitiesFileListTableActivityTypeColumn = By.CssSelector("div[data-testid='mgmt-activity-list-readonly'] div[class='thead thead-light'] div[role='columnheader']:nth-child(1) div");
        private readonly By activitiesFileListTableActivitySubtypeColumn = By.CssSelector("div[data-testid='mgmt-activity-list-readonly'] div[class='thead thead-light'] div[role='columnheader']:nth-child(2) div");
        private readonly By activitiesFileListTableActivityStatusColumn = By.CssSelector("div[data-testid='mgmt-activity-list-readonly'] div[class='thead thead-light'] div[role='columnheader']:nth-child(3) div");
        private readonly By activitiesFileListTableActivityCommencementColumn = By.CssSelector("div[data-testid='mgmt-activity-list-readonly'] div[class='thead thead-light'] div[role='columnheader']:nth-child(4) div");
        private readonly By activitiesFileListTableActivityNavigationColumn = By.CssSelector("div[data-testid='mgmt-activity-list-readonly'] div[class='thead thead-light'] div[role='columnheader']:nth-child(5) div");
        private readonly By activitiesFileListTable1stActTypeContext = By.CssSelector("div[data-testid='mgmt-activity-list-readonly'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[role='cell']:nth-child(1)");
        private readonly By activitiesFileListTable1stActSubtypeContext = By.XPath("div[data-testid='mgmt-activity-list-readonly'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[role='cell']:nth-child(1)");
        private readonly By activitiesFileListTable1stActStatusContext = By.XPath("div[data-testid='mgmt-activity-list-readonly'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[role='cell']:nth-child(1)");
        private readonly By activitiesFileListTable1stActCommencementContext = By.XPath("div[data-testid='mgmt-activity-list-readonly'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[role='cell']:nth-child(1)");
        private readonly By activitiesFileListTable1stActNavigationContext = By.XPath("div[data-testid='mgmt-activity-list-readonly'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[role='cell']:nth-child(1)");
        private readonly By activitiesFileActivityPaginationOptions = By.XPath("//div[contains(text(),'Property File Activity Summary')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div[@data-testid='PropertyManagementActivitiesTable']/following-sibling::div/div/ul[@class='pagination']/li");

        private readonly By activitiesTablesEmptyInfo = By.XPath("//div[contains(text(),'No property management activities found')]");

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

        public void AddNewPropertyContactButton()
        {
            WaitUntilClickable(managementeAddContactBttn);
            webDriver.FindElement(managementeAddContactBttn).Click();
        }

        public void AddNewPropertyActivityButton()
        {
            WaitUntilClickable(managementAddActivityBttn);
            webDriver.FindElement(managementAddActivityBttn).Click();
        }

        public void InsertManagementSummaryInformation(PropertyManagement managementProperty)
        {
            Wait();

            //Delete Property Purpose if any
            if (webDriver.FindElements(managementPropertyPurposeDeleteBttns).Count > 0)
            {
                while (webDriver.FindElements(managementPropertyPurposeDeleteBttns).Count > 0)
                    webDriver.FindElements(managementPropertyPurposeDeleteBttns)[0].Click();
                
                webDriver.FindElement(managementPropertyPurposeLabel).Click();
            }
            //Add Property Purpose
            if (managementProperty.ManagementPropertyPurpose.First() != "")
            {
                ClearMultiSelectInput(managementPropertyPurposeInput);
                foreach (string purpose in managementProperty.ManagementPropertyPurpose)
                {
                    Wait();
                    webDriver.FindElement(managementPropertyPurposeLabel).Click();
                    FocusAndClick(managementPropertyPurposeInput);

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

        public void SavePropertyManagement()
        {
            ButtonElement("Save");
            WaitUntilVisible(managementSummaryEditBttn);
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

            //Activiies List
            AssertTrueIsDisplayed(managementFileActivitiesTitle);
            AssertTrueIsDisplayed(managementActivitiesListTable);
            AssertTrueIsDisplayed(managementActivitiesListTableActivityTypeColumn);
            AssertTrueIsDisplayed(managementActivitiesListTableActivitySubtypeColumn);
            AssertTrueIsDisplayed(managementActivitiesListTableActivityStatusColumn);
            AssertTrueIsDisplayed(managementActivitiesListTableCommencementColumn);
            AssertTrueIsDisplayed(managementActivitiesListTableActionsColumn);

            //Property File Activity Summary
            AssertTrueIsDisplayed(activitiesFileListSubtitle);
            AssertTrueIsDisplayed(activitiesFileListTooltip);
            AssertTrueIsDisplayed(activitiesFileListTableExpandBttn);
            AssertTrueIsDisplayed(activitiesFileListTableActivityTypeColumn);
            AssertTrueIsDisplayed(activitiesFileListTableActivitySubtypeColumn);
            AssertTrueIsDisplayed(activitiesFileListTableActivityStatusColumn);
            AssertTrueIsDisplayed(activitiesFileListTableActivityCommencementColumn);
            AssertTrueIsDisplayed(activitiesFileListTableActivityNavigationColumn);
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

        public void VerifyInsertedSummaryForm(PropertyManagement managementProperty)
        {
            Wait();

            AssertTrueIsDisplayed(managementSummaryTitle);
            AssertTrueIsDisplayed(managementPropertyPurposeLabel);

            if (webDriver.FindElements(managementPropertyPurposeContent).Count > 0)
            {
                var selectedPurposes = webDriver.FindElements(managementPropertyPurposeContent);
                for (int i = 0; i < selectedPurposes.Count; i++)
                    Assert.Equal(managementProperty.ManagementPropertyPurpose[i], selectedPurposes[i].Text);    
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

        public void VerifyLastInsertedActivityTable(PropertyActivity activity)
        {
            Wait();

            var lastInsertedActivityIndex = webDriver.FindElements(managementActivitiesBodyCount).Count;

            AssertTrueContentEquals(By.XPath("//div[contains(text(),'Activities List')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div[@data-testid='PropertyManagementActivitiesTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ lastInsertedActivityIndex +"]/div/div[@role='cell'][1]"), activity.PropertyActivityType);

            foreach (string subtype in activity.PropertyActivitySubTypeList)
                AssertTrueContentEquals(By.XPath("//div[contains(text(),'Activities List')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div[@data-testid='PropertyManagementActivitiesTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ lastInsertedActivityIndex +"]/div/div[@role='cell'][2]"), subtype);

            AssertTrueContentEquals(By.XPath("//div[contains(text(),'Activities List')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div[@data-testid='PropertyManagementActivitiesTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ lastInsertedActivityIndex +"]/div/div[@role='cell'][3]"), activity.PropertyActivityStatus);
            AssertTrueContentEquals(By.XPath("//div[contains(text(),'Activities List')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div[@data-testid='PropertyManagementActivitiesTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ lastInsertedActivityIndex +"]/div/div[@role='cell'][4]"), TransformDateFormat(activity.PropertyActivityRequestedCommenceDate));
            Assert.True(webDriver.FindElements(By.XPath("//div[contains(text(),'Activities List')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div[@data-testid='PropertyManagementActivitiesTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ lastInsertedActivityIndex +"]/div/div[@role='cell'][5]/div/div")).Count > 0);
        }

        public void ViewLastActivityFromList()
        {
            var paginationLastPage = webDriver.FindElements(managementActivityPaginationOptions).Count() -1;

            webDriver.FindElement(By.XPath("//div[@data-testid='PropertyManagementActivitiesTable']/following-sibling::div/div/ul[@class='pagination']/li["+ paginationLastPage +"]")).Click();
        }

        public string VerifyLeaseActiveStatus()
        {
            Wait();
            return webDriver.FindElement(managementLeaseContent).Text;
        }
    }
}
