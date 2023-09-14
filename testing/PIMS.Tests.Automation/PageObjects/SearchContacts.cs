using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SearchContacts : PageObjectBase
    {
        private By menuContactsButton = By.XPath("//a/label[contains(text(),'Contacts')]/parent::a");
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

        private By searchContactTable = By.CssSelector("div[data-testid='contactsTable']");
        private By searchContactTableSummaryColumn = By.XPath("//div[contains(text(),'Summary')]");
        private By searchContactTableFirstNameColumn = By.XPath("//div[contains(text(),'First name')]");
        private By searchContactTableLastNameColumn = By.XPath("//div[contains(text(),'Last name')]");
        private By searchContactTableOrganizationColumn = By.XPath("//div[contains(text(),'Organization')]");
        private By searchContactTableEmailColumn = By.XPath("//div[contains(text(),'E-mail')]");
        private By searchContactTableMailingAddressColumn = By.XPath("//div[contains(text(),'Mailing address')]");
        private By searchContactTableCityColumn = By.XPath("//div[contains(text(),'City')]");
        private By searchContactTableProvinceColumn = By.XPath("//div[contains(text(),'Prov')]");
        private By searchContactTableUpdateViewColumn = By.XPath("//div[contains(text(),'Update/View')]");
        private By searchContactNoResults = By.CssSelector("div[class='no-rows-message']");

        private By searchContactMenuItems = By.CssSelector("div[class='Menu-root']");
        private By searchContactPaginationList = By.CssSelector("ul[class='pagination']");

        private By searchContactLoadingResultTable = By.CssSelector("div[data-testid='contactsTable'] div[class='table-loading'] div[title='table-loading']");
        private By searchContactFirstResultLink = By.CssSelector("div[class='tr-wrapper']:nth-child(1) div:nth-child(3) a");
        private By searchContactFirstNameDiv = By.CssSelector("div[class='tr-wrapper']:nth-child(1) div:nth-child(4)");
        private By searchContactFirstLastNameDiv = By.CssSelector("div[class='tr-wrapper']:nth-child(1) div:nth-child(5)");
        private By searchContactFirstOrganizationDiv = By.CssSelector("div[class='tr-wrapper']:nth-child(1) div:nth-child(6)");
        private By searchContactFirstEmailDiv = By.CssSelector("div[class='tr-wrapper']:nth-child(1) div:nth-child(7)");
        private By searchContactFirstMailAddressDiv = By.CssSelector("div[class='tr-wrapper']:nth-child(1) div:nth-child(8)");
        private By searchContactFirstCityDiv = By.CssSelector("div[class='tr-wrapper']:nth-child(1) div:nth-child(9)");
        private By searchContactFirstProvinceDiv = By.CssSelector("div[class='tr-wrapper']:nth-child(1) div:nth-child(10)");
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
            //Wait(3000);

            WaitUntilClickable(menuContactsButton);
            FocusAndClick(menuContactsButton);

            WaitUntilClickable(searchContactButton);
            FocusAndClick(searchContactButton);
        }

        //Search For a general contact
        public void SearchGeneralContact(string searchCriteria)
        {
            WaitUntilClickable(searchContactNameInput);
            webDriver.FindElement(searchContactNameInput).SendKeys(searchCriteria);
            webDriver.FindElement(searchContactResultsBttn).Click();
        }

        //Search for an Individual Contact
        public void SearchIndividualContact(string searchCriteria)
        {
            FocusAndClick(searchContactIndRadioBttn);
            webDriver.FindElement(searchContactNameInput).SendKeys(searchCriteria);

            Wait();
            webDriver.FindElement(searchContactResultsBttn).Click();
        }

        //Search for an Organization Contact
        public void SearchOrganizationContact(string searchCriteria)
        {
            Wait();

            FocusAndClick(searchContactOrgRadioBttn);
            webDriver.FindElement(searchContactNameInput).SendKeys(searchCriteria);
            webDriver.FindElement(searchContactResultsBttn).Click();
        }

        //Pick the first Search Result
        public void SelectFirstResultLink()
        {
            Wait(2000);
            webDriver.FindElement(searchContactFirstResultLink).Click();
        }

        //Click on Create new contact
        public void CreateNewContactFromSearch()
        {
            webDriver.FindElement(searchContactAddNewBttn).Click();
        }

        //Verify Links functionality on Search Contacts
        public void VerifySearchLinks(string searchCriteria)
        {
            WaitUntilClickable(searchContactNameInput);
            webDriver.FindElement(searchContactNameInput).SendKeys(searchCriteria);
            webDriver.FindElement(searchContactResultsBttn).Click();

            //Verify Update link
            WaitUntilClickable(searchContactUpdateBttn);
            webDriver.FindElement(searchContactUpdateBttn).Click();

            WaitUntilVisible(searchUpdateForm);
            Assert.True(webDriver.FindElement(searchUpdateForm).Displayed);

            webDriver.FindElement(searchContactBackLink).Click();

            WaitUntilVisible(searchContactNameInput);
            webDriver.FindElement(searchContactNameInput).SendKeys(searchCriteria);
            webDriver.FindElement(searchContactResultsBttn).Click();

            //Verify View Link
            WaitUntilClickable(searchContactUpdateBttn);
            webDriver.FindElement(searchContactUpdateBttn).Click();

            WaitUntilVisible(searchContactViewContactInfoHeader);
            Assert.True(webDriver.FindElement(searchContactViewContactInfoHeader).Displayed);

            webDriver.FindElement(searchContactBackLink).Click();

        }

        //Verify Search Contacts Main Screen
        public Boolean SearchContactRender()
        {
            WaitUntilVisible(searchContactMenuItems);
            ScrollToElement(searchContactMenuItems);

            return webDriver.FindElement(searchContactTable).Displayed
                    && webDriver.FindElement(searchContactMenuItems).Displayed
                    && webDriver.FindElement(searchContactPaginationList).Displayed;
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
            Assert.True(webDriver.FindElement(searchContactTable).Displayed);
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

        public void VerifyContactTableContent(string summary, string firstName, string lastName, string organization, string email, string address, string city, string province, string country)
        {
            //WaitUntilDisappear(searchContactLoadingResultTable);
            Wait(2000);

            AssertTrueContentEquals(searchContactFirstResultLink,summary);
            AssertTrueContentEquals(searchContactFirstNameDiv, firstName);
            AssertTrueContentEquals(searchContactFirstLastNameDiv, lastName);
            AssertTrueContentEquals(searchContactFirstOrganizationDiv, organization);
            AssertTrueContentEquals(searchContactFirstEmailDiv, email);
            AssertTrueContentEquals(searchContactFirstMailAddressDiv, address);
            AssertTrueContentEquals(searchContactFirstCityDiv, city);
            if (country == "Canada" || country == "United States of America")
            {
                AssertTrueContentEquals(searchContactFirstProvinceDiv, province);
            }
            else
            {
                AssertTrueContentEquals(searchContactFirstProvinceDiv, "");
            }
            AssertTrueIsDisplayed(searchContactUpdateBttn);
            AssertTrueIsDisplayed(searchContactViewBttn);
            
        }

    }
}
