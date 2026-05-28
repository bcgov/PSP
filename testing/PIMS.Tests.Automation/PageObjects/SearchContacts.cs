using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SearchContacts : PageObjectBase
    {
        private readonly By menuContactsButton = By.CssSelector("div[data-testid='nav-tooltip-contacts'] a");
        private readonly By searchContactButton = By.XPath("//a[contains(text(),'Manage Contacts')]");
        private readonly By consolidationsBttn = By.CssSelector("div[data-testid='nav-tooltip-subdivision&consolidation']");

        private readonly By searchContactOrgRadioBttn = By.Id("input-organizations");
        private readonly By searchContactIndRadioBttn = By.Id("input-persons");
        private readonly By searchContactNameInput = By.Id("input-summary");
        private readonly By searchContactCityInput = By.Id("input-municipality");
        private readonly By searchContactResultsBttn = By.Id("search-button");
        private readonly By searchContactResetBttn = By.Id("reset-button");
        private readonly By searchContactActiveChckBox = By.Id("input-activeContactsOnly");
        private readonly By searchContactActiveSpan = By.XPath("//span[contains(text(),'Show active only')]");

        private readonly By searchContactTableSummaryColumn = By.XPath("//div[contains(text(),'Summary')]");
        private readonly By searchContactOrderBySummaryBttn = By.CssSelector("div[data-testid='sort-column-summary']");
        private readonly By searchContactTableFirstNameColumn = By.XPath("//div[contains(text(),'First name')]");
        private readonly By searchContactOrderByFirstNameBttn = By.CssSelector("div[data-testid='sort-column-firstName']");
        private readonly By searchContactTableLastNameColumn = By.XPath("//div[contains(text(),'Last name')]");
        private readonly By searchContactOrderByLastNameBttn = By.CssSelector("div[data-testid='sort-column-surname']");
        private readonly By searchContactTableOrganizationColumn = By.XPath("//div[contains(text(),'Organization')]");
        private readonly By searchContactOrderByOrganizationBttn = By.CssSelector("div[data-testid='sort-column-organizationName']");
        private readonly By searchContactTableEmailColumn = By.XPath("//div[contains(text(),'E-mail')]");
        private readonly By searchContactTableMailingAddressColumn = By.XPath("//div[contains(text(),'Mailing address')]");
        private readonly By searchContactTableCityColumn = By.XPath("//div[contains(text(),'City')]");
        private readonly By searchContactOrderByCityBttn = By.CssSelector("div[data-testid='sort-column-municipalityName']");
        private readonly By searchContactTableProvinceColumn = By.XPath("//div[contains(text(),'Prov')]");
        private readonly By searchContactTableUpdateViewColumn = By.XPath("//div[contains(text(),'Edit/View')]");
        private readonly By searchContactNoResults = By.XPath("//div[contains(text(),'No Contacts match the search criteria')]");

        private readonly By searchContactMenuItems = By.CssSelector("div[class='Menu-root']");
        private readonly By searchContactPaginationList = By.CssSelector("ul[class='pagination']");

        //First result cell elements
        private readonly By searchContactTableContent = By.CssSelector("div[data-testid='contactsTable'] div[class='tbody'] div[class='tr-wrapper']");
        private readonly By searchContact1stRowResult = By.CssSelector("div[data-testid='contactsTable'] div[class='tbody'] div[class='tr-wrapper']:first-child");
        private readonly By searchContact1stResultLink = By.CssSelector("div[class='tr-wrapper']:nth-child(1) div:nth-child(3) a");
        private readonly By searchContact1stFirstNameContent = By.CssSelector("div[class='tr-wrapper']:nth-child(1) div:nth-child(4)");
        private readonly By searchContact1stLastNameContent = By.CssSelector("div[class='tr-wrapper']:nth-child(1) div:nth-child(5)");
        private readonly By searchContact1stOrganizationContent = By.CssSelector("div[class='tr-wrapper']:nth-child(1) div:nth-child(6)");
        private readonly By searchContact1stEmailContent = By.CssSelector("div[class='tr-wrapper']:nth-child(1) div:nth-child(7)");
        private readonly By searchContact1stMailAddressContent = By.CssSelector("div[class='tr-wrapper']:nth-child(1) div:nth-child(8)");
        private readonly By searchContact1stCityContent = By.CssSelector("div[class='tr-wrapper']:nth-child(1) div:nth-child(9)");
        private readonly By searchContact1stProvinceContent = By.CssSelector("div[class='tr-wrapper']:nth-child(1) div:nth-child(10)");

        private readonly By searchContactUpdateBttn = By.CssSelector("div[class='tr-wrapper']:nth-child(1) div:nth-child(11) button:nth-child(1)");
        private readonly By searchContactViewBttn = By.CssSelector("div[class='tr-wrapper']:nth-child(1) div:nth-child(11) button:nth-child(2)");

        public SearchContacts(IWebDriver webDriver) : base(webDriver)
        { }

        //Navigates to Search a Contact
        public void NavigateToSearchContact()
        {
            SafeClick(consolidationsBttn);
            SafeClick(menuContactsButton);
            SafeClick(searchContactButton);
        }

        //Search For a general contact
        public void FilterContacts(string contactType = "", string summary = "", string city = "")
        {
            WaitUntilClickable(searchContactResetBttn);
            webDriver.FindElement(searchContactResetBttn).Click();

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
            WaitForTableToLoad();
        }

        //Pick the first Search Result
        public void SelectFirstResultLink()
        {
            WaitUntilClickable(searchContact1stResultLink);
            webDriver.FindElement(searchContact1stResultLink).Click();
        }

        public Boolean GetNoSearchMessage()
        {
            Wait();
            return webDriver.FindElements(searchContactNoResults).Count == 1;
        }

        public void VerifyContactsListView()
        {
            WaitUntilVisible(searchContactTableSummaryColumn);

            //Search Bar Elements
            AssertTrueIsDisplayed(searchContactOrgRadioBttn);
            AssertTrueIsDisplayed(searchContactIndRadioBttn);
            AssertTrueIsDisplayed(searchContactNameInput);
            AssertTrueIsDisplayed(searchContactCityInput);
            AssertTrueIsDisplayed(searchContactResultsBttn);
            AssertTrueIsDisplayed(searchContactResetBttn);
            AssertTrueIsDisplayed(searchContactActiveChckBox);
            AssertTrueIsDisplayed(searchContactActiveSpan);
//            AssertTrueIsDisplayed(searchContactAddNewBttn);

            //Table Elements
            AssertTrueIsDisplayed(searchContactTableSummaryColumn);
            AssertTrueIsDisplayed(searchContactTableFirstNameColumn);
            AssertTrueIsDisplayed(searchContactTableLastNameColumn);
            AssertTrueIsDisplayed(searchContactTableOrganizationColumn);
            AssertTrueIsDisplayed(searchContactTableEmailColumn);
            AssertTrueIsDisplayed(searchContactTableMailingAddressColumn);
            AssertTrueIsDisplayed(searchContactTableCityColumn);
            AssertTrueIsDisplayed(searchContactTableProvinceColumn);
            AssertTrueIsDisplayed(searchContactTableUpdateViewColumn);

            //Pagination Elements
            WaitUntilVisible(searchContactMenuItems);
            AssertTrueIsDisplayed(searchContactMenuItems);
            AssertTrueIsDisplayed(searchContactPaginationList);
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
            Wait();
            SafeClick(searchContactOrderByOrganizationBttn);
        }

        public void OrderByContactCity()
        {
            WaitUntilClickable(searchContactOrderByCityBttn);
            webDriver.FindElement(searchContactOrderByCityBttn).Click();
        }

        public string FirstContactSummary()
        {
            WaitForTableToLoad();
            return webDriver.FindElement(searchContact1stResultLink).Text;
        }

        public string FirstContactFirstName()
        {
            WaitForTableToLoad();
            return webDriver.FindElement(searchContact1stFirstNameContent).Text;
        }

        public string FirstContactLastName()
        {
            WaitForTableToLoad();
            return webDriver.FindElement(searchContact1stLastNameContent).Text;
        }

        public string FirstContactOrganization()
        {
            WaitForTableToLoad();
            return webDriver.FindElement(searchContact1stOrganizationContent).Text;
        }

        public string FirstContactCity()
        {
            WaitForTableToLoad();
            return webDriver.FindElement(searchContact1stCityContent).Text;
        }

        public int ContactsTableResultNumber()
        {
            WaitForTableToLoad();
            return webDriver.FindElements(searchContactTableContent).Count;
        }

        public Boolean SearchFoundResults()
        {
            WaitForTableToLoad();
            return webDriver.FindElements(searchContact1stRowResult).Count > 0;
        }

        public void VerifyContactTableContent(string summary, string firstName, string lastName, string organization, string email, string address, string city, string province, string country)
        {
            Wait();

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
