using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace PIMS.Tests.Automation.PageObjects
{
    public class LeaseTenants: PageObjectBase
    {
        //Tenants Tab Element
        private By licenseTenantLink = By.XPath("//a[contains(text(),'Tenant')]");

        //Tenants Edit Element
        private By tenantEditIcon = By.XPath("//div[@role='tabpanel'][2]/div/div/button");

        //Tenants Add Tenant(s) button
        private By tenantAddTenantsBttn = By.XPath("//div[contains(text(),'Select Tenant(s)')]/parent::button");

        //Tenant Add Tenant Modal Elements
        private By tenantIndividualRadioBttn = By.Id("input-persons");
        private By tenantOrganizationRadioBttn = By.Id("input-organizations");
        private By tenantSearchInput = By.Id("input-summary");
        private By tenantSearchBttn = By.Id("search-button");
        private By tenantFirstResultRadioBttn = By.CssSelector("div[class='tr-wrapper']:nth-child(1) div:nth-child(1) input");
        private By tenantsAddSelectedButton = By.XPath("//div[contains(text(), 'Select')]/parent::button[@title='ok-modal']");

        //Selected tenants
        private By tenantSelectedTenantsRows = By.CssSelector("div[data-testid='selected-items'] div[class='tr-wrapper']");
        private By tenantPrimaryContact1stCell = By.CssSelector("div[data-testid='selected-items'] div[class='tr-wrapper']:nth-child(1) div[class='td']:nth-child(5) p");
        private By tenantPrimaryContact1stSelect = By.Id("input-tenants.0.primaryContactId");
        private By tenantType1stSelect = By.Id("input-tenants.0.tenantType");

        //Total Tenants by Type Elements
        private By tenantsTotalTenantsView = By.XPath("//div[contains(text(),'Tenant')]/parent::div/parent::h2/following-sibling::div/div");
        private By tenantsTotalRepresentativeView = By.XPath("//div[contains(text(),'Representative')]/parent::div/parent::h2/following-sibling::div/div");
        private By tenantsTotalManagerView = By.XPath("//div[contains(text(),'Property Manager')]/parent::div/parent::h2/following-sibling::div/div");
        private By tenantsTotalUnknownView = By.XPath("//div[contains(text(),'Unknown')]/parent::div/parent::h2/following-sibling::div/div");

        //Confirm Tenants Save Modal
        private By tenantsModal = By.CssSelector("div[class='modal-content']");
        private By tenantModalSave2ndParagraph = By.CssSelector("div[class='modal-body'] p:nth-child(2)");

        //Insert Tenants Form Elements
        private By tenantsSubtittle = By.XPath("//h2[contains(text(),'Add tenants & contacts to this Lease/License')]");
        private By tenantsInstructions = By.XPath("//p[contains(text(),'If the tenants are not already set up as contacts, you will have to add them first (under')]");
        private By tenantsCounter = By.XPath("//div[@data-testid='selected-items']/preceding-sibling::p");
        private By tenantsSelectedTableSummaryColumn = By.XPath("//div[@data-testid='selected-items']/div[@class='thead thead-light']/div/div/div[contains(text(),'Summary')]");
        private By tenantsSelectedTablePrimaryContactColumn = By.XPath("//div[@data-testid='selected-items']/div[@class='thead thead-light']/div/div/div[contains(text(),'Primary contact')]");
        private By tenantsSelectedTableContactInfoColumn = By.XPath("//div[@data-testid='selected-items']/div[@class='thead thead-light']/div/div/div[contains(text(),'Contact Info')]");
        private By tenantsSelectedTableTypeColumn = By.XPath("//div[@data-testid='selected-items']/div[@class='thead thead-light']/div/div/div[contains(text(),'Type')]");
        private By tenantsSelectedNoRows = By.CssSelector("div[class='no-rows-message']");

        SharedModals sharedModals;

        public LeaseTenants(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
        }

        //Navigates to Tenants Section
        public void NavigateToTenantSection()
        {
            Wait();
            webDriver.FindElement(licenseTenantLink).Click();
        }

        //Edit Tenant section
        public void EditTenant()
        {
            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(20));
            wait.Until(ExpectedConditions.ElementIsVisible(tenantEditIcon));

            webDriver.FindElement(tenantEditIcon).Click();
        }

        //Search and add a new tenant
        public void AddIndividualTenant(string searchCriteria, string tenantType)
        {
            Wait();

            webDriver.FindElement(tenantAddTenantsBttn).Click();

            Wait(5000);
            webDriver.FindElement(tenantSearchInput).SendKeys(searchCriteria);
            webDriver.FindElement(tenantIndividualRadioBttn).Click();
            webDriver.FindElement(tenantSearchBttn).Click();

            Wait();
            ScrollToElement(tenantSearchInput);
            webDriver.FindElement(tenantFirstResultRadioBttn).Click();

            Wait();
            webDriver.FindElement(tenantsAddSelectedButton).Click();

            //Choose tenant type
            Wait();
            ChooseSpecificSelectOption(tenantType1stSelect, tenantType);

            //Verify that the Primary Contact displays "Not applicable"
            Assert.True(webDriver.FindElement(tenantPrimaryContact1stCell).Text.Equals("Not applicable"));
        }

        public void AddOrganizationTenant(string searchCriteria, string tenantType)
        {
            Wait();

            webDriver.FindElement(tenantAddTenantsBttn).Click();

            Wait();
            webDriver.FindElement(tenantSearchInput).SendKeys(searchCriteria);
            webDriver.FindElement(tenantOrganizationRadioBttn).Click();
            webDriver.FindElement(tenantSearchBttn).Click();

            Wait();
            ScrollToElement(tenantSearchInput);
            webDriver.FindElement(tenantFirstResultRadioBttn).Click();

            Wait();
            webDriver.FindElement(tenantsAddSelectedButton).Click();

            Wait(5000);
            //Choose a primary contact if there's the option
            if (webDriver.FindElements(tenantPrimaryContact1stSelect).Count > 0)
            {
                ChooseRandomSelectOption(tenantPrimaryContact1stSelect, 1);
            }

            //Choose tenant type
            ChooseSpecificSelectOption(tenantType1stSelect, tenantType);
        }

        public void SaveTenant()
        {
            Wait();

            //Save
            ButtonElement("Save");

            Wait();
            //If primary contact hasn't been selected for any of the tenants
            if (webDriver.FindElements(tenantsModal).Count > 0)
            {
                Assert.True(sharedModals.ModalHeader() == "Confirm save");
                Assert.True(webDriver.FindElement(tenantModalSave2ndParagraph).Text == "Do you wish to save without providing a primary contact?");
                sharedModals.ModalClickOKBttn();
            }
        }

        public int TotalTenants()
        {
            Wait();
            return webDriver.FindElements(tenantsTotalTenantsView).Count;
            
        }

        public int TotalRepresentatives()
        {
            Wait();
            return webDriver.FindElements(tenantsTotalRepresentativeView).Count;
        }

        public int TotalManagers()
        {
            Wait();
            return webDriver.FindElements(tenantsTotalManagerView).Count;
        }

        public int TotalUnknown()
        {
            Wait();
            return webDriver.FindElements(tenantsTotalUnknownView).Count;
        }

        public void VerifyTenantsInitForm()
        {
            Wait();

            Assert.True(webDriver.FindElement(tenantsSubtittle).Displayed);
            Assert.True(webDriver.FindElement(tenantsInstructions).Displayed);
            Assert.True(webDriver.FindElement(tenantAddTenantsBttn).Displayed);
            Assert.True(webDriver.FindElement(tenantsCounter).Displayed);
            Assert.True(webDriver.FindElement(tenantsSelectedTableSummaryColumn).Displayed);
            Assert.True(webDriver.FindElement(tenantsSelectedTablePrimaryContactColumn).Displayed);
            Assert.True(webDriver.FindElement(tenantsSelectedTableContactInfoColumn).Displayed);
            Assert.True(webDriver.FindElement(tenantsSelectedTableTypeColumn).Displayed);
            Assert.True(webDriver.FindElement(tenantsSelectedNoRows).Displayed);
        }
    }
}
