using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SearchContacts : PageObjectBase
    {
        private By menuContactsButton = By.XPath("//a/label[contains(text(),'Contacts')]/parent::a");
        private By searchContactButton = By.XPath("//a[contains(text(),'Manage Contacts')]");

        private By searchContactOrgRadioBttn = By.Id("input-organizations");
        private By searchContactIndRadioBttn = By.Id("input-persons");
        private By searchContactNameInput = By.Id("input-summary");
        private By searchContactResultsButton = By.Id("search-button");

        private By searchContactTable = By.CssSelector("div[data-testid='contactsTable']");
        private By searchContactMenuItems = By.CssSelector("div[class='Menu-root']");
        private By searchContactPaginationList = By.CssSelector("ul[class='pagination']");
        private By searchContactNoResults = By.CssSelector("div[class='no-rows-message']");


        private By searchContactFirstResultLink = By.CssSelector("div[class='tr-wrapper']:nth-child(1) div:nth-child(3) a");
        private By searchContactUpdateBttn = By.CssSelector("div[class='tr-wrapper']:nth-child(1) div:nth-child(11) button:nth-child(1)");
        private By searchContactViewBttn = By.CssSelector("div[class='tr-wrapper']:nth-child(1) div:nth-child(11) button:nth-child(2)");

        private By searchUpdateForm = By.Id("updateForm");
        private By searchContactViewContactInfoHeader = By.XPath("//h2[contains(text(), 'Contact info')]");
        private By searchContactBackLink = By.XPath("//a[contains(text(), 'Contact Search')]");



        public SearchContacts(IWebDriver webDriver) : base(webDriver)
        { }

        //Navigates to Search a Contact
        public void NavigateToSearchContact()
        {

            Wait();
            webDriver.FindElement(menuContactsButton).Click();

            Wait();
            webDriver.FindElement(searchContactButton).Click();
        }

        //Search For a general contact
        public void SearchGeneralContact(string searchCriteria)
        {
            Wait();
            webDriver.FindElement(searchContactNameInput).SendKeys(searchCriteria);
            webDriver.FindElement(searchContactResultsButton).Click();
        }

        //Search for an Individual Contact
        public void SearchIndividualContact(string searchCriteria)
        {
            Wait();
            FocusAndClick(searchContactIndRadioBttn);
            webDriver.FindElement(searchContactNameInput).SendKeys(searchCriteria);
            webDriver.FindElement(searchContactResultsButton).Click();

            Wait();
            webDriver.FindElement(searchContactFirstResultLink).Click();
        }

        //Search for an Organization Contact
        public void SearchOrganizationContact(string searchCriteria)
        {
            Wait();
            FocusAndClick(searchContactOrgRadioBttn);
            webDriver.FindElement(searchContactNameInput).SendKeys(searchCriteria);
            webDriver.FindElement(searchContactResultsButton).Click();

            Wait();
            webDriver.FindElement(searchContactFirstResultLink).Click();

        }

        //Click on Create new contact
        public void CreateNewContactFromSearch()
        {
            Wait();
            ButtonElement("Add new contact");

        }

        //Verify Links functionality on Search Contacts
        public void VerifySearchLinks(string searchCriteria)
        {
            Wait();
            webDriver.FindElement(searchContactNameInput).SendKeys(searchCriteria);
            webDriver.FindElement(searchContactResultsButton).Click();

            //Verify Update link
            Wait();
            webDriver.FindElement(searchContactUpdateBttn).Click();

            Wait();
            Assert.True(webDriver.FindElement(searchUpdateForm).Displayed);

            webDriver.FindElement(searchContactBackLink).Click();

            Wait();
            webDriver.FindElement(searchContactNameInput).SendKeys(searchCriteria);
            webDriver.FindElement(searchContactResultsButton).Click();

            //Verify View Link
            Wait();
            webDriver.FindElement(searchContactUpdateBttn).Click();

            Wait();
            Assert.True(webDriver.FindElement(searchContactViewContactInfoHeader).Displayed);

            webDriver.FindElement(searchContactBackLink).Click();

        }

        //Verify Search Contacts Main Screen
        public Boolean SearchContactRender()
        {
            Wait();
            ScrollToElement(searchContactMenuItems);

            return webDriver.FindElement(searchContactTable).Displayed
                    && webDriver.FindElement(searchContactMenuItems).Displayed
                    && webDriver.FindElement(searchContactPaginationList).Displayed;
        }


        public string GetNoSearchMessage()
        {
            Wait();
            return webDriver.FindElement(searchContactNoResults).Text;
        }

    }
}
