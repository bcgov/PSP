using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SearchProjects : PageObjectBase
    {
        //Menu Elements
        private By projectMenuBttn = By.CssSelector("div[data-testid='nav-tooltip-project'] a");
        private By manageProjectButton = By.XPath("//a[contains(text(),'Manage Project')]");

        //Search Projects Filters Elements
        private By searchProjectSubtitle = By.XPath("//h1/div/div/span[contains(text(),'Projects')]");
        private By searchProjectNumberInput = By.Id("input-projectNumber");
        private By searchProjectNameInput = By.Id("input-projectName");
        private By searchProjectRegionSelect = By.Id("input-projectRegionCode");
        private By searchProjectStatusSelect = By.Id("input-projectStatusCode");
        private By searchProjectButton = By.Id("search-button");
        private By searchProjectResetButton = By.Id("reset-button");
        private By searchProjectAddProjectBttn = By.XPath("//div[contains(text(),'Create Project')]/parent::button");

        //Search Projects Table Column header Elements
        private By searchProjectNbrHeaderColumn = By.XPath("//div[@data-testid='projectsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Project #')]");
        private By searchProjectNbrOrderBttn = By.CssSelector("div[data-testid='sort-column-code']");
        private By searchProjectNameHeaderColumn = By.XPath("//div[@data-testid='projectsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Project name')]");
        private By searchProjectNameOrderBttn = By.CssSelector("div[data-testid='sort-column-description']");
        private By searchProjectRegionHeaderColumn = By.XPath("//div[@data-testid='projectsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Region')]");
        private By searchProjectStatusHeaderColumn = By.XPath("//div[@data-testid='projectsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Status')]");
        private By searchProjectLastUpdatedByHeaderColumn = By.XPath("//div[@data-testid='projectsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Last updated by')]");
        private By searchProjectLastUpdatedByOrderBttn = By.CssSelector("div[data-testid='sort-column-lastUpdatedBy']");
        private By searchProjectUpdatedDateHeaderColumn = By.XPath("//div[@data-testid='projectsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Updated date')]");
        private By searchProjectLastUpdatedDateOrderBttn = By.CssSelector("div[data-testid='sort-column-lastUpdatedDate']");

        //Search Projects Table 1st result Element
        private By searchProject1stResult = By.CssSelector("div[data-testid='projectsTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1)");
        private By searchProject1stResultNbrLink = By.XPath("//div[@data-testid='projectsTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[1]/a");
        private By searchProject1stResultNameLink = By.XPath("//div[@data-testid='projectsTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[2]/a");
        private By searchProject1stResultRegionContent = By.XPath("//div[@data-testid='projectsTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[3]");
        private By searchProject1stResultStatusContent = By.XPath("//div[@data-testid='projectsTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[4]");
        private By searchProject1stResultLastUpdatedByContent = By.XPath("//div[@data-testid='projectsTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[5]");
        private By searchProject1stResultLastUpdatedDateContent = By.XPath("//div[@data-testid='projectsTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[6]");
        private By searchProjectUpdatedDateSortBttn = By.CssSelector("div[data-testid='sort-column-lastUpdatedDate']");

        private By searchProjectTotalCount = By.XPath("//div[@data-testid='projectsTable']/div[@class='tbody']/div[@class='tr-wrapper']");

        //Search Projects Pagination elements
        private By searchProjectShowEntries = By.CssSelector("div[class='Menu-root']");
        private By searchProjectPagination = By.CssSelector("ul[class='pagination']");

        public SearchProjects(IWebDriver webDriver) : base(webDriver)
        {}

        //Navigates to Search a Project
        public void NavigateToSearchProject()
        {
            Wait(3000);

            WaitUntilClickable(projectMenuBttn);
            FocusAndClick(projectMenuBttn);

            WaitUntilClickable(manageProjectButton);
            FocusAndClick(manageProjectButton);
        }

        public void SearchProjectByName(string projectName)
        {
            WaitUntilVisible(searchProjectResetButton);
            webDriver.FindElement(searchProjectResetButton).Click();

            WaitUntilVisible(searchProjectNameInput);
            webDriver.FindElement(searchProjectNameInput).SendKeys(projectName);
            ChooseSpecificSelectOption(searchProjectStatusSelect, "All Statuses");
            
            webDriver.FindElement(searchProjectButton).Click();
        }

        public void SearchProjectByNumber(string projectNumber)
        {
            WaitUntilVisible(searchProjectResetButton);
            webDriver.FindElement(searchProjectResetButton).Click();

            WaitUntilVisible(searchProjectNumberInput);
            webDriver.FindElement(searchProjectNumberInput).SendKeys(projectNumber);
            ChooseSpecificSelectOption(searchProjectStatusSelect, "All Statuses");

            webDriver.FindElement(searchProjectButton).Click();
        }

        public void SearchProjectByRegion(string projectRegion)
        {
            WaitUntilVisible(searchProjectResetButton);
            webDriver.FindElement(searchProjectResetButton).Click();

            WaitUntilVisible(searchProjectRegionSelect);
            webDriver.FindElement(searchProjectRegionSelect).SendKeys(projectRegion);
            ChooseSpecificSelectOption(searchProjectStatusSelect, "All Statuses");

            webDriver.FindElement(searchProjectButton).Click();
        }

        public void SearchProjectByStatus(string projectStatus)
        {
            WaitUntilVisible(searchProjectResetButton);
            webDriver.FindElement(searchProjectResetButton).Click();

            WaitUntilVisible(searchProjectStatusSelect);
            ChooseSpecificSelectOption(searchProjectStatusSelect, projectStatus);

            webDriver.FindElement(searchProjectButton).Click();
        }

        public void SelectFirstResult()
        {
            WaitUntilVisible(searchProjectTotalCount);
            webDriver.FindElement(searchProject1stResultNbrLink).Click();
        }

        public void OrderByProjectNumber()
        {
            WaitUntilClickable(searchProjectNbrOrderBttn);
            webDriver.FindElement(searchProjectNbrOrderBttn).Click();
        }

        public void OrderByProjectName()
        {
            WaitUntilClickable(searchProjectNameOrderBttn);
            webDriver.FindElement(searchProjectNameOrderBttn).Click();
        }

        public void OrderByProjectLastUpdatedBy()
        {
            WaitUntilClickable(searchProjectLastUpdatedByOrderBttn);
            webDriver.FindElement(searchProjectLastUpdatedByOrderBttn).Click();
        }

        public void OrderByProjectLastUpdatedDate()
        {
            WaitUntilClickable(searchProjectLastUpdatedDateOrderBttn);
            webDriver.FindElement(searchProjectLastUpdatedDateOrderBttn).Click();
        }


        public void VerifySearchView()
        {
            WaitUntilVisible(searchProjectSubtitle);

            //Search Projects Filters Section
            AssertTrueIsDisplayed(searchProjectSubtitle);
            AssertTrueIsDisplayed(searchProjectNumberInput);
            AssertTrueIsDisplayed(searchProjectNameInput);
            AssertTrueIsDisplayed(searchProjectRegionSelect);
            AssertTrueIsDisplayed(searchProjectStatusSelect);
            AssertTrueIsDisplayed(searchProjectButton);
            AssertTrueIsDisplayed(searchProjectResetButton);
            AssertTrueIsDisplayed(searchProjectAddProjectBttn);

            //Search Projects Table Column header Elements
            AssertTrueIsDisplayed(searchProjectNbrHeaderColumn);
            AssertTrueIsDisplayed(searchProjectNameHeaderColumn);
            AssertTrueIsDisplayed(searchProjectRegionHeaderColumn);
            AssertTrueIsDisplayed(searchProjectStatusHeaderColumn);
            AssertTrueIsDisplayed(searchProjectLastUpdatedByHeaderColumn);
            AssertTrueIsDisplayed(searchProjectUpdatedDateHeaderColumn);

            AssertTrueIsDisplayed(searchProjectShowEntries);
            AssertTrueIsDisplayed(searchProjectPagination);
        }

        public void VerifyViewSearchResult(Project project)
        {
            DateTime thisDay = DateTime.Today;
            string today = thisDay.ToString("MMM d, yyyy");

            AssertTrueContentEquals(searchProject1stResultNbrLink, project.Number);
            AssertTrueContentEquals(searchProject1stResultNameLink, project.Name);
            AssertTrueContentEquals(searchProject1stResultRegionContent, project.ProjectMOTIRegion);
            AssertTrueContentEquals(searchProject1stResultStatusContent, project.ProjectStatus);
            AssertTrueContentEquals(searchProject1stResultLastUpdatedByContent, project.UpdatedBy);
            AssertTrueContentEquals(searchProject1stResultLastUpdatedDateContent, today);
        }

        public int ProjectsTableResultNumber()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElements(searchProjectTotalCount).Count();
        }

        public Boolean SearchFoundResults()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElements(searchProject1stResult).Count > 0;
        }

        public string FirstProjectCode()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(searchProject1stResultNbrLink).Text;
        }

        public string FirstProjectName()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(searchProject1stResultNameLink).Text;
        }

        public string FirstProjectLastUpdatedBy()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(searchProject1stResultLastUpdatedByContent).Text;
        }

        public string FirstProjectLastUpdatedDate()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(searchProject1stResultLastUpdatedDateContent).Text;
        }
    }
}
