using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using PIMS.Tests.Automation.Classes;
using SeleniumExtras.WaitHelpers;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SearchProjects : PageObjectBase
    {
        //Menu Elements
        private By projectMenuBttn = By.XPath("//a/label[contains(text(),'Project')]/parent::a");
        private By manageProjectButton = By.XPath("//a[contains(text(),'Manage Project')]");

        //Search Projects Filters Elements
        private By searchProjectSubtitle = By.XPath("//div[@data-testid='projectsTable']/preceding-sibling::h3[contains(text(),'Projects')]");
        private By searchProjectNumberInput = By.Id("input-projectNumber");
        private By searchProjectNameInput = By.Id("input-projectName");
        private By searchProjectRegionSelect = By.Id("input-projectRegionCode");
        private By searchProjectStatusSelect = By.Id("input-projectStatusCode");
        private By searchProjectButton = By.Id("search-button");
        private By searchProjectResetButton = By.Id("reset-button");
        private By searchProjectAddProjectBttn = By.XPath("//h3[contains(text(),'Projects')]/parent::div/button");

        //Search Projects Table Column header Elements
        private By searchProjectNbrHeaderColumn = By.XPath("//div[@data-testid='projectsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Project #')]");
        private By searchProjectNameHeaderColumn = By.XPath("//div[@data-testid='projectsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Project name')]");
        private By searchProjectRegionHeaderColumn = By.XPath("//div[@data-testid='projectsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Region')]");
        private By searchProjectStatusHeaderColumn = By.XPath("//div[@data-testid='projectsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Status')]");
        private By searchProjectLastUpdatedByHeaderColumn = By.XPath("//div[@data-testid='projectsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Last updated by')]");
        private By searchProjectUpdatedDateHeaderColumn = By.XPath("//div[@data-testid='projectsTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Updated date')]");

        //Search Projects Table 1st result Element
        private By searchProject1stResultNbrLink = By.XPath("//div[@data-testid='projectsTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[1]/a");
        private By searchProject1stResultNameLink = By.XPath("//div[@data-testid='projectsTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[2]/a");
        private By searchProject1stResultRegionContent = By.XPath("//div[@data-testid='projectsTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[3]");
        private By searchProject1stResultStatusContent = By.XPath("//div[@data-testid='projectsTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[4]");
        private By searchProject1stResultLastUpdatedByContent = By.XPath("//div[@data-testid='projectsTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[5]");
        private By searchProject1stResultLastUpdatedDateContent = By.XPath("//div[@data-testid='projectsTable']/div[@class='tbody']/div[@class='tr-wrapper'][1]/div/div[6]");
        private By searchProjectUpdatedDateSortBttn = By.CssSelector("div[data-testid='sort-column-lastUpdatedDate']");

        private By searchProjectTotalCount = By.XPath("//div[@data-testid='projectsTable']/div[@class='tbody']/div[@class='tr-wrapper']");

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

        public void SearchLastProject()
        {
            WaitUntilVisible(searchProjectResetButton);
            webDriver.FindElement(searchProjectResetButton).Click();

            WaitUntilVisible(searchProjectTotalCount);
            ChooseSpecificSelectOption(searchProjectStatusSelect, "All Statuses");
            FocusAndClick(searchProjectButton);

            WaitUntilVisible(searchProjectTotalCount);
            webDriver.FindElement(searchProjectUpdatedDateSortBttn).Click();
            webDriver.FindElement(searchProjectUpdatedDateSortBttn).Click();
        }

        public void SelectFirstResult()
        {
            WaitUntilVisible(searchProjectTotalCount);
            webDriver.FindElement(searchProject1stResultNbrLink).Click();
        }

        public void VerifySearchView()
        {
            WaitUntilVisible(searchProjectSubtitle);

            //Search Projects Filters Section
            Assert.True(webDriver.FindElement(searchProjectSubtitle).Displayed);
            Assert.True(webDriver.FindElement(searchProjectNumberInput).Displayed);
            Assert.True(webDriver.FindElement(searchProjectNameInput).Displayed);
            Assert.True(webDriver.FindElement(searchProjectRegionSelect).Displayed);
            Assert.True(webDriver.FindElement(searchProjectStatusSelect).Displayed);
            Assert.True(webDriver.FindElement(searchProjectButton).Displayed);
            Assert.True(webDriver.FindElement(searchProjectResetButton).Displayed);
            Assert.True(webDriver.FindElement(searchProjectAddProjectBttn).Displayed);

            //Search Projects Table Column header Elements
            Assert.True(webDriver.FindElement(searchProjectNbrHeaderColumn).Displayed);
            Assert.True(webDriver.FindElement(searchProjectNameHeaderColumn).Displayed);
            Assert.True(webDriver.FindElement(searchProjectRegionHeaderColumn).Displayed);
            Assert.True(webDriver.FindElement(searchProjectStatusHeaderColumn).Displayed);
            Assert.True(webDriver.FindElement(searchProjectLastUpdatedByHeaderColumn).Displayed);
            Assert.True(webDriver.FindElement(searchProjectUpdatedDateHeaderColumn).Displayed);
 
            Assert.True(webDriver.FindElement(searchProjectShowEntries).Displayed);
            Assert.True(webDriver.FindElement(searchProjectPagination).Displayed);
        }

        public void VerifyViewSearchResult(Project project)
        {
            DateTime thisDay = DateTime.Today;
            string today = thisDay.ToString("MMM d, yyyy");

            WaitUntilVisible(searchProject1stResultNbrLink);
            Assert.True(webDriver.FindElement(searchProject1stResultNbrLink).Text.Equals(project.Number));
            Assert.True(webDriver.FindElement(searchProject1stResultNameLink).Text.Equals(project.Name));
            Assert.True(webDriver.FindElement(searchProject1stResultRegionContent).Text.Equals(project.ProjectMOTIRegion));
            Assert.True(webDriver.FindElement(searchProject1stResultStatusContent).Text.Equals(project.ProjectStatus));
            Assert.True(webDriver.FindElement(searchProject1stResultLastUpdatedByContent).Text.Equals(project.UpdatedBy));
            Assert.True(webDriver.FindElement(searchProject1stResultLastUpdatedDateContent).Text.Equals(today));
        }

        public int TotalSearchedProjects()
        {
            Wait(2000);
            return webDriver.FindElements(searchProjectTotalCount).Count();
        }
    }
}
