using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class ManageUsers : PageObjectBase
    {
        //Admin Tool Menu Elements
        private By mainMenuAdminToolLink = By.CssSelector("div[data-testid='nav-tooltip-admintools'] a");

        private By adminSubmenuManageUserLink = By.XPath("//a[contains(text(),'Manage Users')]");

        //User Management List View Elements
        private By userManagementRoleSelect = By.Id("input-role");
        private By userManagementIdirInput = By.Id("input-businessIdentifierValue");
        private By userManagementRegionSelect = By.Id("input-region");
        private By userManagementEmailInput = By.Id("input-email");
        private By userManagementSearchButton = By.Id("search-button");
        private By userManagementResetButton = By.Id("reset-button");
        private By userManagerActiveUserInput = By.Id("input-activeOnly");
        private By userManagerShowActiveUserLabel = By.XPath("//span[contains(text(),'Show active users only')]");
        private By userManagerExportExcelBttn = By.CssSelector("div[class='align-items-center d-flex col-md-4'] button");

        //Table Header Elements
        private By userManagerHeaderActiveColumn = By.XPath("//div[@data-testid='usersTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Active')]");
        private By userManagerHeaderIdirColumn = By.XPath("//div[@data-testid='usersTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'IDIR/BCeID')]");
        private By userManagerHeaderOrderByIDIRBttn = By.CssSelector("div[data-testid='sort-column-businessIdentifierValue']");
        private By userManagerHeaderFirstNameColumn = By.XPath("//div[@data-testid='usersTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'First name')]");
        private By userManagerHeaderOrderByFirstNameBttn = By.CssSelector("div[data-testid='sort-column-firstName']");
        private By userManagerHeaderLastNameColumn = By.XPath("//div[@data-testid='usersTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Last name')]");
        private By userManagerHeaderOrderByLastNameBttn = By.CssSelector("div[data-testid='sort-column-surname']");
        private By userManagerHeaderEmailColumn = By.XPath("//div[@data-testid='usersTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Email')]");
        private By userManagerHeaderOrderByEmailBttn = By.CssSelector("div[data-testid='sort-column-email']");
        private By userManagerHeaderPositionColumn = By.XPath("//div[@data-testid='usersTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Position')]");
        private By userManagerHeaderOrderByPositionBttn = By.CssSelector("div[data-testid='sort-column-position']");
        private By userManagerHeaderRolesColumn = By.XPath("//div[@data-testid='usersTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Roles')]");
        private By userManagerHeaderRegionColumn = By.XPath("//div[@data-testid='usersTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'MoTI region(s)')]");
        private By userManagerHeaderLastLoginColumn = By.XPath("//div[@data-testid='usersTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Last login')]");

        //Table 1st Result by Column Elements
        private By userManager1stIDIRContent = By.CssSelector("div[data-testid='usersTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div:nth-child(2) a");
        private By userManager1stFirstNameContent = By.CssSelector("div[data-testid='usersTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div[class='td clickable']:nth-child(3)");
        private By userManager1stLastNameContent = By.CssSelector("div[data-testid='usersTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div[class='td clickable']:nth-child(4)");
        private By userManager1stEmailContent = By.CssSelector("div[data-testid='usersTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div[class='td clickable']:nth-child(5)");
        private By userManager1stPositionContent = By.CssSelector("div[data-testid='usersTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div[class='td clickable']:nth-child(6)");
        private By userManager1stUserTypeContent = By.CssSelector("div[data-testid='usersTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div[class='td clickable']:nth-child(7)");
        private By userManager1stRolesContent = By.CssSelector("div[data-testid='usersTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div[class='td clickable']:nth-child(8)");
        private By userManager1stMOTIRegionContent = By.CssSelector("div[data-testid='usersTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div[class='td clickable']:nth-child(9)");
        private By userManager1stLastLoginContent = By.CssSelector("div[data-testid='usersTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div[class='td clickable']:nth-child(10)");

        private By userManagerTableResults = By.XPath("//div[@data-testid='usersTable']/div[@class='tbody']/div");

        private By userManagerEntriesByPage = By.CssSelector("div[class='align-self-center col-auto'] div[class='Menu-root']");
        private By userManagerPagination = By.CssSelector("ul[class='pagination']");


        public ManageUsers(IWebDriver webDriver) : base(webDriver)
        {}

        public void NavigateAdminTools()
        {
            Wait(3000);

            WaitUntilClickable(mainMenuAdminToolLink);
            FocusAndClick(mainMenuAdminToolLink);
        }

        public void NavigateUserManagement()
        {
            WaitUntilClickable(adminSubmenuManageUserLink);
            FocusAndClick(adminSubmenuManageUserLink);
        }

        public void ResetDefaultListView()
        {
            WaitUntilVisible(userManagementResetButton);
            webDriver.FindElement(userManagementResetButton).Click();
        }

        public void OrderByUserIDIR()
        {
            WaitUntilClickable(userManagerHeaderOrderByIDIRBttn);
            webDriver.FindElement(userManagerHeaderOrderByIDIRBttn).Click();
        }

        public void OrderByUserFirstName()
        {
            WaitUntilClickable(userManagerHeaderOrderByFirstNameBttn);
            webDriver.FindElement(userManagerHeaderOrderByFirstNameBttn).Click();
        }

        public void OrderByUserLastName()
        {
            WaitUntilClickable(userManagerHeaderOrderByLastNameBttn);
            webDriver.FindElement(userManagerHeaderOrderByLastNameBttn).Click();
        }

        public void OrderByUserMail()
        {
            WaitUntilClickable(userManagerHeaderOrderByEmailBttn);
            webDriver.FindElement(userManagerHeaderOrderByEmailBttn).Click();
        }

        public void OrderByUserPosition()
        {
            WaitUntilClickable(userManagerHeaderOrderByPositionBttn);
            webDriver.FindElement(userManagerHeaderOrderByPositionBttn).Click();
        }

        public string FirstUserIDIR()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(userManager1stIDIRContent).Text;
        }

        public string FirstUserFirstName()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(userManager1stFirstNameContent).Text;
        }

        public string FirstUserLastName()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(userManager1stLastNameContent).Text;
        }

        public string FirstUserEmail()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(userManager1stEmailContent).Text;
        }

        public string FirstUserPosition()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(userManager1stPositionContent).Text;
        }

        public void FilterUsers(string idir, string region)
        {
            WaitUntilVisible(userManagementResetButton);
            webDriver.FindElement(userManagementResetButton).Click();

            if (idir != "")
            {
                webDriver.FindElement(userManagementIdirInput).SendKeys(idir);
                webDriver.FindElement(userManagementSearchButton).Click();
            }

            if (region != "")
            {
                WaitUntilClickable(userManagementRegionSelect);
                ChooseSpecificSelectOption(userManagementRegionSelect, region);
            }
            
            webDriver.FindElement(userManagementSearchButton).Click();
        }

        public void VerifyManageUserListView()
        {
            WaitUntilVisible(userManagerTableResults);

            Assert.True(webDriver.FindElement(userManagementRoleSelect).Displayed);
            Assert.True(webDriver.FindElement(userManagementIdirInput).Displayed);
            Assert.True(webDriver.FindElement(userManagementRegionSelect).Displayed);
            Assert.True(webDriver.FindElement(userManagementEmailInput).Displayed);
            Assert.True(webDriver.FindElement(userManagementSearchButton).Displayed);
            Assert.True(webDriver.FindElement(userManagementResetButton).Displayed);
            Assert.True(webDriver.FindElement(userManagerActiveUserInput).Displayed);
            Assert.True(webDriver.FindElement(userManagerShowActiveUserLabel).Displayed);
            Assert.True(webDriver.FindElement(userManagerExportExcelBttn).Displayed);

            Assert.True(webDriver.FindElement(userManagerHeaderActiveColumn).Displayed);
            Assert.True(webDriver.FindElement(userManagerHeaderIdirColumn).Displayed);
            Assert.True(webDriver.FindElement(userManagerHeaderFirstNameColumn).Displayed);
            Assert.True(webDriver.FindElement(userManagerHeaderLastNameColumn).Displayed);
            Assert.True(webDriver.FindElement(userManagerHeaderEmailColumn).Displayed);
            Assert.True(webDriver.FindElement(userManagerHeaderPositionColumn).Displayed);
            Assert.True(webDriver.FindElement(userManagerHeaderRolesColumn).Displayed);
            Assert.True(webDriver.FindElement(userManagerHeaderRegionColumn).Displayed);
            Assert.True(webDriver.FindElement(userManagerHeaderLastLoginColumn).Displayed);
            Assert.True(webDriver.FindElement(userManagerTableResults).Displayed);

            Assert.True(webDriver.FindElement(userManagerEntriesByPage).Displayed);
            Assert.True(webDriver.FindElement(userManagerPagination).Displayed);
        }

        public int TotalUsersResult()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElements(userManagerTableResults).Count();
        }
    }
}
