using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;
using Sprache;

namespace PIMS.Tests.Automation.PageObjects
{
    public class LeaseTenants: PageObjectBase
    {
        //Tenants Tab Element
        private readonly By licenseTenantLink = By.XPath("//a[contains(text(),'Tenant')]");
        private readonly By licensePayeeLink = By.XPath("//a[contains(text(),'Payee')]");

        //Tenants Edit Element
        private readonly By StakeholderEditButton = By.XPath("//div[@role='tabpanel']/div/div/div/button");

        //Tenants Add Tenant(s) button
        private readonly By stakeholderAddTenantsBttn = By.XPath("//div[contains(text(),'Tenants')]/following-sibling::div/button");
        private readonly By stakeholderAddPayeesBttn = By.XPath("//div[contains(text(),'Payee')]/following-sibling::div/button");

        //Tenant Add Tenant Modal Elements
        private readonly By stakeholderOrganizationRadioBttn = By.Id("input-organizations");
        private readonly By stakeholderSearchInput = By.Id("input-summary");
        private readonly By tenantSearchBttn = By.Id("search-button");
        private readonly By stakeholderFirstResultRadioBttn = By.CssSelector("div[data-testid='contactsTable'] div[class='tr-wrapper']:nth-child(1) div:nth-child(1) input");
        private readonly By stakeholderAddSelectedButton = By.XPath("//div[contains(text(), 'Select')]/parent::button[@title='ok-modal']");

        //Selected tenants
        private readonly By stakeholderPrimaryContact1stCell = By.CssSelector("div[data-testid='selected-items'] div[class='tr-wrapper']:nth-child(1) div[class='td']:nth-child(4) p");
        private readonly By stakeholderPrimaryContact1stSelect = By.Id("input-stakeholders.0.primaryContactId");
        private readonly By stakeholderType1stSelect = By.Id("input-stakeholders.0.stakeholderType");
        private readonly By stakeholder1stStakeholderList = By.CssSelector("div[data-testid='selected-items'] div[class='tr-wrapper']:nth-child(1) button[title='Click to remove']");

        //Total Tenants by Type Elements
        private readonly By stakeholderTotalAssigneeView = By.XPath("//div[contains(text(),'Assignee')]/parent::div/parent::h2/following-sibling::div/div");
        private readonly By stakeholderTotalTenantsView = By.XPath("//div[contains(text(),'Tenant')]/parent::div/parent::h2/following-sibling::div/div");
        private readonly By stakeholderTotalRepresentativeView = By.XPath("//div[contains(text(),'Property manager')]/parent::div/parent::h2/parent::div/following-sibling::div/h2/div/div[contains(text(),'Representative')]/parent::div/parent::h2/following-sibling::div/div");
        private readonly By stakeholderTotalManagerView = By.XPath("//div[contains(text(),'Property manager')]/parent::div/parent::h2/following-sibling::div/div");
        private readonly By stakeholderTotalUnknownView = By.XPath("//div[contains(text(),'Unknown')]/parent::div/parent::h2/following-sibling::div/div");
        private readonly By stakeholderTotalOwnerView = By.XPath("//div[@class='tab-content']/div/div/div/div[1]/div/div");
        private readonly By stakeholderTotalOwnerRepView = By.XPath("//div[contains(text(),'Owner Representative')]/parent::div/parent::h2/following-sibling::div/div");

        //Confirm Tenants Save Modal
        private readonly By stakeholderModal = By.CssSelector("div[class='modal-content']");
        private readonly By stakeholderModalSave2ndParagraph = By.CssSelector("div[class='modal-body'] p:nth-child(2)");

        //Insert Tenants Form Elements
        private readonly By stakeholderTenantSubtittle = By.XPath("//div[contains(text(),'Tenants')]");
        private readonly By stakeholderPayeeSubtittle = By.XPath("//div[contains(text(),'Payees')]");
        private readonly By stakeholderTenantInstructions = By.XPath("//span[contains(text(),'Note: If the tenants you are trying to find were never added to the \"contact list\" it will not show up. Please add them to the contact list')]");
        private readonly By stakeholderPayeeInstructions = By.XPath("//span[contains(text(),'Note: If the payees you are trying to find were never added to the \"contact list\" it will not show up. Please add them to the contact list')]");
        private readonly By stakeholderCounter = By.XPath("//div[@data-testid='selected-items']/preceding-sibling::p");
        private readonly By stakeholderSelectedTableSummaryColumn = By.XPath("//div[@data-testid='selected-items']/div[@class='thead thead-light']/div/div/div[contains(text(),'Summary')]");
        private readonly By stakeholderSelectedTablePrimaryContactColumn = By.XPath("//div[@data-testid='selected-items']/div[@class='thead thead-light']/div/div/div[contains(text(),'Primary contact')]");
        private readonly By stakeholderSelectedTableContactInfoColumn = By.XPath("//div[@data-testid='selected-items']/div[@class='thead thead-light']/div/div/div[contains(text(),'Contact info')]");
        private readonly By stakeholderTenantSelectedTableTypeColumn = By.XPath("//div[@data-testid='selected-items']/div[@class='thead thead-light']/div/div/div[contains(text(),'Contact type')]");
        private readonly By stakeholderPayeeSelectedTableTypeColumn = By.XPath("//div[@data-testid='selected-items']/div[@class='thead thead-light']/div/div/div[contains(text(),'Payee type')]");
        private readonly By stakeholderSelectedNoRows = By.CssSelector("div[class='no-rows-message']");
        private readonly By stakeholderTotalSelected = By.CssSelector("div[data-testid='selected-items'] div[class='tr-wrapper']");

        SharedModals sharedModals;
        SharedSelectContact sharedSelectContact;

        public LeaseTenants(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
            sharedSelectContact = new SharedSelectContact(webDriver);
        }

        //Navigates to Tenants Section
        public void NavigateToStakeholderSection(string leaseType)
        {
            if (leaseType == "Receivable")
            {
                WaitUntilClickable(licenseTenantLink);
                FocusAndClick(licenseTenantLink);
            }
            else
            {
                WaitUntilClickable(licensePayeeLink);
                FocusAndClick(licensePayeeLink);
            } 
        }

        //Edit Tenant section
        public void EditStakeholderButton()
        {
            Wait();
            webDriver.FindElement(StakeholderEditButton).Click();
        }

        //Search and add a new tenant
        public void AddIndividualStakeholder(string leaseType, Stakeholder stakeholder)
        {
            Wait();

            if (leaseType == "Receivable")
                webDriver.FindElement(stakeholderAddTenantsBttn).Click();
            else
                webDriver.FindElement(stakeholderAddPayeesBttn).Click();

            Wait();
            sharedSelectContact.SelectContact(stakeholder.Summary, "Individual");

            //Choose stakeholder type
            Wait();
            ChooseSpecificSelectOption(stakeholderType1stSelect, stakeholder.StakeholderType);

            //Verify that the Primary Contact displays "Not applicable"
            Assert.Equal("Not applicable", webDriver.FindElement(stakeholderPrimaryContact1stCell).Text);
        }

        public void AddOrganizationTenant(string leaseType, Stakeholder stakeholder)
        {
            Wait();

            if (leaseType == "Receivable")
                webDriver.FindElement(stakeholderAddTenantsBttn).Click();
            else
                webDriver.FindElement(stakeholderAddPayeesBttn).Click();

            Wait();
            webDriver.FindElement(stakeholderSearchInput).SendKeys(stakeholder.Summary);
            webDriver.FindElement(stakeholderOrganizationRadioBttn).Click();
            webDriver.FindElement(tenantSearchBttn).Click();

            ScrollToElement(stakeholderSearchInput);
            WaitUntilClickable(stakeholderFirstResultRadioBttn);
            webDriver.FindElement(stakeholderFirstResultRadioBttn).Click();

            WaitUntilClickable(stakeholderAddSelectedButton);
            webDriver.FindElement(stakeholderAddSelectedButton).Click();

            //Choose a primary contact if there's the option
            if (webDriver.FindElements(stakeholderPrimaryContact1stSelect).Count > 0)
            {
                WaitUntilClickable(stakeholderPrimaryContact1stSelect);
                ChooseSpecificSelectOption(stakeholderPrimaryContact1stSelect, stakeholder.PrimaryContact);
            }

            //Choose stakeholder type
            ChooseSpecificSelectOption(stakeholderType1stSelect, stakeholder.StakeholderType);
        }

        public void DeleteFirstStakeholder()
        {
            Wait();
            webDriver.FindElement(stakeholder1stStakeholderList).Click();
        }

        public void DeleteLastStakeholder()
        {
            Wait();

            var totalStakeholderSelected = webDriver.FindElements(stakeholderTotalSelected).Count;
            var deleteLastTenant = By.CssSelector("div[data-testid='selected-items'] div[class='tr-wrapper']:nth-child("+ totalStakeholderSelected +") button[title='Click to remove']");
            webDriver.FindElement(deleteLastTenant).Click();
        }

        public void EditStakeholder(Stakeholder tenant)
        {
            Wait();
            var totalStakeholderIndex = webDriver.FindElements(stakeholderTotalSelected).Count -1;
            By lastStakeholderSelector = By.Id("input-stakeholders."+ totalStakeholderIndex +".stakeholderType");

            ChooseSpecificSelectOption(lastStakeholderSelector, tenant.StakeholderType);
        }

        public void SaveTenant()
        { 
            //Save
            ButtonElement("Save");

            Wait();
            //If primary contact hasn't been selected for any of the tenants
            if (webDriver.FindElements(stakeholderModal).Count > 0)
            {
                Assert.True(sharedModals.ModalHeader() == "Confirm save");
                Assert.True(webDriver.FindElement(stakeholderModalSave2ndParagraph).Text == "Do you wish to save without providing a primary contact?");
                sharedModals.ModalClickOKBttn();
            }
        }

        public int TotalAssignees()
        {
            Wait();
            return webDriver.FindElements(stakeholderTotalAssigneeView).Count;
        }

        public int TotalTenants()
        {
            Wait();
            return webDriver.FindElements(stakeholderTotalTenantsView).Count;
        }

        public int TotalRepresentatives()
        {
            Wait();
            return webDriver.FindElements(stakeholderTotalRepresentativeView).Count;
        }

        public int TotalManagers()
        {
            Wait();
            return webDriver.FindElements(stakeholderTotalManagerView).Count;
        }

        public int TotalUnknown()
        {
            Wait();
            return webDriver.FindElements(stakeholderTotalUnknownView).Count;
        }

        public int TotalOwners()
        {
            Wait();
            return webDriver.FindElements(stakeholderTotalOwnerView).Count;
        }

        public int TotalOwnerRepresentatives()
        {
            Wait();
            return webDriver.FindElements(stakeholderTotalOwnerRepView).Count;
        }

        public void VerifyStakeholdersInitForm(string accountType)
        {
            if (accountType == "Receivable")
            {
                AssertTrueIsDisplayed(stakeholderTenantSubtittle);
                AssertTrueIsDisplayed(stakeholderTenantInstructions);
                AssertTrueIsDisplayed(stakeholderAddTenantsBttn);
                AssertTrueIsDisplayed(stakeholderTenantSelectedTableTypeColumn);
            }
            else
            {
                AssertTrueIsDisplayed(stakeholderPayeeSubtittle);
                AssertTrueIsDisplayed(stakeholderPayeeInstructions);
                AssertTrueIsDisplayed(stakeholderAddPayeesBttn);
                AssertTrueIsDisplayed(stakeholderPayeeSelectedTableTypeColumn);
            }
            
            AssertTrueIsDisplayed(stakeholderCounter);
            AssertTrueIsDisplayed(stakeholderSelectedTableSummaryColumn);
            AssertTrueIsDisplayed(stakeholderSelectedTablePrimaryContactColumn);
            AssertTrueIsDisplayed(stakeholderSelectedTableContactInfoColumn);
            AssertTrueIsDisplayed(stakeholderSelectedNoRows);
        }
    }
}
