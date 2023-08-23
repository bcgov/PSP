using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class ManageUsers : PageObjectBase
    {
        //Admin Tool Menu Elements
        private By mainMenuAdminToolLink = By.XPath("//label[contains(text(),'Admin Tools')]/parent::a");

        private By adminSubmenuManageUserLink = By.XPath("//a[contains(text(),'Manage Users')]");
        private By adminSubmenuManageAccessLink = By.XPath("//a[contains(text(),'Manage Access Requests')]");
        private By adminSubmenuCDOGSLink = By.XPath("//a[contains(text(),'Manage Activity Document Templates')]");
        private By adminSubmenuFinancialCodesLink = By.XPath("//a[contains(text(),'Manage Financial Codes')]");

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

        private By userManagerHeaderActiveColumn = By.XPath("//div[@data-testid='usersTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Active')]");
        private By userManagerHeaderIdirColumn = By.XPath("//div[@data-testid='usersTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'IDIR/BCeID')]");
        private By userManagerHeaderFirstNameColumn = By.XPath("//div[@data-testid='usersTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'First name')]");
        private By userManagerHeaderLastNameColumn = By.XPath("//div[@data-testid='usersTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Last name')]");
        private By userManagerHeaderEmailColumn = By.XPath("//div[@data-testid='usersTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Email')]");
        private By userManagerHeaderPositionColumn = By.XPath("//div[@data-testid='usersTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Position')]");
        private By userManagerHeaderRolesColumn = By.XPath("//div[@data-testid='usersTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Roles')]");
        private By userManagerHeaderRegionColumn = By.XPath("//div[@data-testid='usersTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'MoTI region(s)')]");
        private By userManagerHeaderLastLoginColumn = By.XPath("//div[@data-testid='usersTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Last login')]");
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

        public void FilterUsers(string idir, string region)
        {
            WaitUntilVisible(userManagementIdirInput);
            webDriver.FindElement(userManagementIdirInput).SendKeys(idir);
            webDriver.FindElement(userManagementSearchButton).Click();

            WaitUntilVisible(userManagerTableResults);
            Assert.True(webDriver.FindElements(userManagerTableResults).Count() > 0);
            webDriver.FindElement(userManagementResetButton).Click();

            WaitUntilClickable(userManagementRegionSelect);
            ChooseSpecificSelectOption(userManagementRegionSelect, region);
            webDriver.FindElement(userManagementSearchButton).Click();

            Assert.True(webDriver.FindElements(userManagerTableResults).Count() == 0);
            webDriver.FindElement(userManagementResetButton).Click();

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
    }
}
