using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace PIMS.Tests.Automation.PageObjects
{
    public class Tenants: PageObjectBase
    {
        private By licenseTenantLink = By.XPath("//a[contains(text(),'Tenant')]");

        private By tenantEditIcon = By.CssSelector("a[class='float-right']");
        private By tenantSelectionSectionH2 = By.XPath("//h2[contains(text(),'Step 1 - Select Tenant(s)')]");

        private By tenantIndividualRadioBttn = By.Id("input-persons");
        private By tenantOrganizationRadioBttn = By.Id("input-organizations");

        private By tenantSearchInput = By.Id("input-summary");
        private By tenantSearchBttn = By.Id("search-button");
        private By tenantFirstResultRadioBttn = By.CssSelector("div[class='tr-wrapper']:nth-child(1) div:nth-child(1) input");
        private By tenantSelectedTenantsTable = By.CssSelector("div[data-testid='selected-items']");
        private By tenantSelectedTenantsRows = By.CssSelector("div[data-testid='selected-items'] div[class='tr-wrapper']");

        private By tenantsTotalTenantsView = By.CssSelector("form[id='leaseForm'] div li");

        private int totalTenantsInLease;
   

        public Tenants(IWebDriver webDriver) : base(webDriver)
        { }

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
        public void AddIndividualTenant(string searchCriteria)
        {
            Wait();

            ScrollToElement(tenantSelectionSectionH2);
            webDriver.FindElement(tenantSearchInput).SendKeys(searchCriteria);
            webDriver.FindElement(tenantIndividualRadioBttn).Click();
            webDriver.FindElement(tenantSearchBttn).Click();

            Wait();
            ScrollToElement(tenantSearchInput);
            webDriver.FindElement(tenantFirstResultRadioBttn).Click();
            ButtonElement("Add selected tenants");

            ScrollToElement(tenantSelectedTenantsTable);

            //Count how many tenants are to check if the recently added one has a selection to make
            totalTenantsInLease = webDriver.FindElements(tenantSelectedTenantsRows).Count();
            var primaryContactCellElement = "div[data-testid='selected-items'] div[class='tr-wrapper']:nth-child("+ totalTenantsInLease +") div[class='td']:nth-child(5) p";
            Assert.True(webDriver.FindElement(By.CssSelector(primaryContactCellElement)).Text.Equals("Not applicable"));

        }

        public void AddOrganizationTenant(string searchCriteria)
        {
            Wait();

            ScrollToElement(tenantSelectionSectionH2);
            

            webDriver.FindElement(tenantSearchInput).SendKeys(searchCriteria);
            webDriver.FindElement(tenantOrganizationRadioBttn).Click();
            webDriver.FindElement(tenantSearchBttn).Click();

            Wait();
            ScrollToElement(tenantSearchInput);
            webDriver.FindElement(tenantFirstResultRadioBttn).Click();
            ButtonElement("Add selected tenants");

            ScrollToElement(tenantSelectedTenantsTable);

            //Count how many tenants are to check if the recently added one has a selection to make
            totalTenantsInLease = webDriver.FindElements(tenantSelectedTenantsRows).Count();
            var currentTenantIndex = totalTenantsInLease -1;

            Wait();

            var selectElementId = "input-tenants." + currentTenantIndex +".primaryContactId";

            if (webDriver.FindElements(By.Id(selectElementId)).Count() > 0)
            {
                var selectContactElement = webDriver.FindElement(By.Id(selectElementId));
                ChooseRandomOption(selectContactElement, selectElementId, 2);
            }
        }

        public void SaveTenant()
        {
            Wait();
            ButtonElement("Save");
        }

        public void CancelTenant()
        {
            Wait();

            ButtonElement("Cancel");
            webDriver.SwitchTo().Alert().Accept();
        }

        public Boolean TotalTenants()
        {
            Wait();
            var totalTenantsinView = webDriver.FindElements(tenantsTotalTenantsView).Count();
            return totalTenantsInLease.Equals(totalTenantsinView);
        }
    }
}
