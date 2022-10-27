using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SearchResearchFiles : PageObjectBase
    {
        private By menuResearchButton = By.XPath("//a/label[contains(text(),'Research')]/parent::a");
        private By searchResearchButton = By.XPath("//a[contains(text(),'Manage Research File')]");

        private By searchResearchFileNumberSelect = By.Id("input-researchSearchBy");
        private By searchResearchFileInput = By.Id("input-rfileNumber");
        private By searchResearchStatusSelect = By.Id("input-researchFileStatusTypeCode");
        private By searchResearchFileButton = By.Id("search-button");

        private By searchResearchFileSortByRFileBttn = By.CssSelector("div[data-testid='sort-column-fileNumber']");
        private By searchResearchFile1stResult = By.CssSelector("div[data-testid='researchFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1)");
        private By searchResearchFile1stResultLink = By.CssSelector("div[data-testid='researchFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable'] a");

        private By researchFileHeaderCode = By.XPath("//strong[contains(text(),'R-')]");

        public SearchResearchFiles(IWebDriver webDriver) : base(webDriver)
        {}

        //Navigates to Search a Research File
        public void NavigateToSearchResearchFile()
        {
            Wait();
            webDriver.FindElement(menuResearchButton).Click();

            Wait();
            webDriver.FindElement(searchResearchButton).Click();
        }

        public void SearchResearchFileByRFile(string RFile)
        {
            Wait();
            ChooseSpecificSelectOption("input-researchSearchBy", "Research file #");
            webDriver.FindElement(searchResearchFileInput).SendKeys(RFile);
            ChooseSpecificSelectOption("input-researchFileStatusTypeCode", "All Status");

            Wait(5000);
            webDriver.FindElement(searchResearchFileButton).Click();
        }

        public void SearchLastResearchFile()
        {
            Wait();
            webDriver.FindElement(searchResearchFileSortByRFileBttn).Click();

            Wait();
            webDriver.FindElement(searchResearchFileSortByRFileBttn).Click();

            Wait(1000);
            FocusAndClick(searchResearchFileButton);

            Wait();
            webDriver.FindElement(searchResearchFile1stResultLink).Click();

            Wait();
            Assert.True(webDriver.FindElement(researchFileHeaderCode).Displayed);

        }

        public Boolean SearchFoundResults()
        {
            Wait();
            return webDriver.FindElements(searchResearchFile1stResult).Count > 0;
        }
    }
}
