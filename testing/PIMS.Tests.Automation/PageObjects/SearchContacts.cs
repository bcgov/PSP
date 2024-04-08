using OpenQA.Selenium;
using System.Security.Cryptography;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SearchContacts : PageObjectBase
    {
        private By menuContactsButton = By.CssSelector("div[data-testid='nav-tooltip-contacts'] a");
        private By searchContactButton = By.XPath("//a[contains(text(),'Manage Contacts')]");

        private By searchContactOrgRadioBttn = By.Id("input-organizations");
        private By searchContactIndRadioBttn = By.Id("input-persons");
        private By searchContactNameInput = By.Id("input-summary");
        private By searchContactCityInput = By.Id("input-municipality");
        private By searchContactResultsBttn = By.Id("search-button");
        private By searchContactResetBttn = By.Id("reset-button");
        private By searchContactActiveChckBox = By.Id("input-activeContactsOnly");
        private By searchContactActiveSpan = By.XPath("//span[contains(text(),'Show active contacts only')]");
        private By searchContactAddNewBttn = By.XPath("//span[contains(text(),'Add new contact')]/parent::div/parent::button");

        private By searchContactTableSummaryColumn = By.XPath("//div[contains(text(),'Summary')]");
        private By searchContactOrderBySummaryBttn = By.CssSelector("div[data-testid='sort-column-summary']");
        private By searchContactTableFirstNameColumn = By.XPath("//div[contains(text(),'First name')]");
        private By searchContactOrderByFirstNameBttn = By.CssSelector("div[data-testid='sort-column-firstName']");
        private By searchContactTableLastNameColumn = By.XPath("//div[contains(text(),'Last name')]");
        private By searchContactOrderByLastNameBttn = By.CssSelector("div[data-testid='sort-column-surname']");
        private By searchContactTableOrganizationColumn = By.XPath("//div[contains(text(),'Organization')]");
        private By searchContactOrderByOrganizationBttn = By.CssSelector("div[data-testid='sort-column-organizationName']");
        private By searchContactTableEmailColumn = By.XPath("//div[contains(text(),'E-mail')]");
        private By searchContactTableMailingAddressColumn = By.XPath("//div[contains(text(),'Mailing address')]");
        private By searchContactTableCityColumn = By.XPath("//div[contains(text(),'City')]");
        private By searchContactOrderByCityBttn = By.CssSelector("div[data-testid='sort-column-municipalityName']");
        private By searchContactTableProvinceColumn = By.XPath("//div[contains(text(),'Prov')]");
        private By searchContactTableUpdateViewColumn = By.XPath("//div[contains(text(),'Update/View')]");
        private By searchContactNoResults = By.CssSelector("div[class='no-rows-message']");

        private By searchContactMenuItems = By.CssSelector("div[class='Menu-root']");
        private By searchContactPaginationList = By.CssSelector("ul[class='pagination']");

        //First result cell elements
        private By searchContactTableContent = By.CssSelector("div[data-testid='contactsTable'] div[class='tbody'] div[class='tr-wrapper']");
        private By searchContact1stRowResult = By.CssSelector("div[data-testid='contactsTable'] div[class='tbody'] div[class='tr-wrapper']:first-child");
        private By searchContact1stResultLink = By.CssSelector("div[class='tr-wrapper']:nth-child(1) div:nth-child(3) a");
        private By searchContact1stFirstNameContent = By.CssSelector("div[class='tr-wrapper']:nth-child(1) div:nth-child(4)");
        private By searchContact1stLastNameContent = By.CssSelector("div[class='tr-wrapper']:nth-child(1) div:nth-child(5)");
        private By searchContact1stOrganizationContent = By.CssSelector("div[class='tr-wrapper']:nth-child(1) div:nth-child(6)");
        private By searchContact1stEmailContent = By.CssSelector("div[class='tr-wrapper']:nth-child(1) div:nth-child(7)");
        private By searchContact1stMailAddressContent = By.CssSelector("div[class='tr-wrapper']:nth-child(1) div:nth-child(8)");
        private By searchContact1stCityContent = By.CssSelector("div[class='tr-wrapper']:nth-child(1) div:nth-child(9)");
        private By searchContact1stProvinceContent = By.CssSelector("div[class='tr-wrapper']:nth-child(1) div:nth-child(10)");

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
            WaitUntilClickable(menuContactsButton);
            FocusAndClick(menuContactsButton);

            WaitUntilClickable(searchContactButton);
            FocusAndClick(searchContactButton);
        }

        //Search For a general contact
        public void FilterContacts(string contactType = "", string summary = "", string city = "")
        {
            Wait();

            webDriver.FindElement(searchContactResetBttn).Click();

            Wait();
            if (contactType != "")
            {
                if(contactType == "Individual")
                    FocusAndClick(searchContactIndRadioBttn);
                else
                    FocusAndClick(searchContactOrgRadioBttn);
            }

            if(summary != "")
                webDriver.FindElement(searchContactNameInput).SendKeys(summary);

            if(city != "")
                webDriver.FindElement(searchContactCityInput).SendKeys(city);

            webDriver.FindElement(searchContactResultsBttn).Click();
            WaitUntilTableSpinnerDisappear();
        }

        //Pick the first Search Result
        public void SelectFirstResultLink()
        {
            Wait(2000);
            webDriver.FindElement(searchContact1stResultLink).Click();
        }

        public string GetNoSearchMessage()
        {
            WaitUntilVisible(searchContactNoResults);
            return webDriver.FindElement(searchContactNoResults).Text;
        }

        public void VerifyContactsListView()
        {
            WaitUntilVisible(searchContactTableSummaryColumn);

            //Search Bar Elements
            Assert.True(webDriver.FindElement(searchContactOrgRadioBttn).Displayed);
            Assert.True(webDriver.FindElement(searchContactIndRadioBttn).Displayed);
            Assert.True(webDriver.FindElement(searchContactNameInput).Displayed);
            Assert.True(webDriver.FindElement(searchContactCityInput).Displayed);
            Assert.True(webDriver.FindElement(searchContactResultsBttn).Displayed);
            Assert.True(webDriver.FindElement(searchContactResetBttn).Displayed);
            Assert.True(webDriver.FindElement(searchContactActiveChckBox).Displayed);
            Assert.True(webDriver.FindElement(searchContactActiveSpan).Displayed);
            Assert.True(webDriver.FindElement(searchContactAddNewBttn).Displayed);

            //Table Elements
            Assert.True(webDriver.FindElement(searchContactTableSummaryColumn).Displayed);
            Assert.True(webDriver.FindElement(searchContactTableFirstNameColumn).Displayed);
            Assert.True(webDriver.FindElement(searchContactTableLastNameColumn).Displayed);
            Assert.True(webDriver.FindElement(searchContactTableOrganizationColumn).Displayed);
            Assert.True(webDriver.FindElement(searchContactTableEmailColumn).Displayed);
            Assert.True(webDriver.FindElement(searchContactTableMailingAddressColumn).Displayed);
            Assert.True(webDriver.FindElement(searchContactTableCityColumn).Displayed);
            Assert.True(webDriver.FindElement(searchContactTableProvinceColumn).Displayed);
            Assert.True(webDriver.FindElement(searchContactTableUpdateViewColumn).Displayed);

            //Pagination Elements
            WaitUntilVisible(searchContactMenuItems);
            Assert.True(webDriver.FindElement(searchContactMenuItems).Displayed);
            Assert.True(webDriver.FindElement(searchContactPaginationList).Displayed);
        }

        public void OrderByContactSummary()
        {
            WaitUntilClickable(searchContactOrderBySummaryBttn);
            webDriver.FindElement(searchContactOrderBySummaryBttn).Click();
        }

        public void OrderByContactFirstName()
        {
            WaitUntilClickable(searchContactOrderByFirstNameBttn);
            webDriver.FindElement(searchContactOrderByFirstNameBttn).Click();
        }

        public void OrderByContactLastName()
        {
            WaitUntilClickable(searchContactOrderByLastNameBttn);
            webDriver.FindElement(searchContactOrderByLastNameBttn).Click();
        }

        public void OrderByContactOrganization()
        {
            WaitUntilClickable(searchContactOrderByOrganizationBttn);
            webDriver.FindElement(searchContactOrderByOrganizationBttn).Click();
        }

        public void OrderByContactCity()
        {
            WaitUntilClickable(searchContactOrderByCityBttn);
            webDriver.FindElement(searchContactOrderByCityBttn).Click();
        }

        public string FirstContactSummary()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(searchContact1stResultLink).Text;
        }

        public string FirstContactFirstName()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(searchContact1stFirstNameContent).Text;
        }

        public string FirstContactLastName()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(searchContact1stLastNameContent).Text;
        }

        public string FirstContactOrganization()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(searchContact1stOrganizationContent).Text;
        }

        public string FirstContactCity()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(searchContact1stCityContent).Text;
        }

        public int ContactsTableResultNumber()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElements(searchContactTableContent).Count;
        }

        public Boolean SearchFoundResults()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElements(searchContact1stRowResult).Count > 0;
        }

        public void VerifyContactTableContent(string summary, string firstName, string lastName, string organization, string email, string address, string city, string province, string country)
        {
            Wait(2000);

            AssertTrueContentEquals(searchContact1stResultLink,summary);
            AssertTrueContentEquals(searchContact1stFirstNameContent, firstName);
            AssertTrueContentEquals(searchContact1stLastNameContent, lastName);
            AssertTrueContentEquals(searchContact1stOrganizationContent, organization);
            AssertTrueContentEquals(searchContact1stEmailContent, email);
            AssertTrueContentEquals(searchContact1stMailAddressContent, address);
            AssertTrueContentEquals(searchContact1stCityContent, city);

            if (country == "Canada" || country == "United States of America")
                AssertTrueContentEquals(searchContact1stProvinceContent, province);
            else
                AssertTrueContentEquals(searchContact1stProvinceContent, "");
            
            AssertTrueIsDisplayed(searchContactUpdateBttn);
            AssertTrueIsDisplayed(searchContactViewBttn);
            
        }

    }
}
