using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SearchResearchFiles : PageObjectBase
    {
        private By menuResearchButton = By.XPath("//a/label[contains(text(),'Research')]/parent::a");
        private By searchResearchButton = By.XPath("//a[contains(text(),'Manage Research File')]");

        //Search Research File Elements
        private By searchResearchRegionInput = By.Id("input-regionCode");
        private By searchResearchBySelect = By.Id("input-researchSearchBy");
        private By searchResearchNameInput = By.Id("input-name");
        private By searchResearchFileNbrInput = By.Id("input-rfileNumber");
        private By searchResearchStatusSelect = By.Id("input-researchFileStatusTypeCode");
        private By searchResearchRoadInput = By.Id("input-roadOrAlias");
        private By searchResearchCreateUpdateDateSelect = By.Id("input-createOrUpdateRange");
        private By searchResearchFromDateInput = By.Id("datepicker-updatedOnStartDate");
        private By searchResearchToDateInput = By.Id("datepicker-updatedOnEndDate");
        private By searchResearchCreateUpdateBySelect = By.Id("input-createOrUpdateBy");
        private By searchResearchUserUpdatedIdirInput = By.Id("input-appLastUpdateUserid");
        private By searchResearchFileUserCreatedIdirInput = By.Id("input-appCreateUserid");
        private By searchResearchFileButton = By.Id("search-button");
        private By searchResearchFileResetButton = By.Id("reset-button");
        private By searchResearchCreateNewBttn = By.XPath("//div[contains(text(),'Create a Research File')]/parent::button");

        //Search Research File List Elements
        private By searchResearchFileNbrLabel = By.XPath("//div[contains(text(),'File #')]");
        private By searchResearchFileNameLabel = By.XPath("//div[contains(text(),'File name')]");
        private By searchResearchFileRegionLabel = By.XPath("//div[contains(text(),'MOTI Region')]");
        private By searchResearchFileCreatedByLabel = By.XPath("//div[contains(text(),'Created by')]");
        private By searchResearchFileCreatedDateLabel = By.XPath("//div[contains(text(),'Created date')]");
        private By searchResearchFileUpdatedByLabel = By.XPath("//div[contains(text(),'Last updated by')]");
        private By searchResearchFileUpdatedDateLabel = By.XPath("//div[contains(text(),'Last updated date')]");
        private By searchResearchFileStatusLabel = By.XPath("//div[contains(text(),'Status')]");
        private By searchResearchFileSortByRFileBttn = By.CssSelector("div[data-testid='sort-column-rfileNumber']");

        //Search Research Files 1st Result Elements
        private By searchResearchFile1stResult = By.CssSelector("div[data-testid='researchFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1)");
        private By searchResearchFile1stResultLink = By.CssSelector("div[data-testid='researchFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable'] a");
        private By searchResearchFile1stResultFileName = By.CssSelector("div[data-testid='researchFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable']:nth-child(2)");
        private By searchResearchFile1stResultRegion = By.CssSelector("div[data-testid='researchFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable']:nth-child(3)");
        private By searchResearchFile1stResultCreator = By.CssSelector("div[data-testid='researchFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable']:nth-child(4)");
        private By searchResearchFile1stResultCreateDate = By.CssSelector("div[data-testid='researchFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable']:nth-child(5)");
        private By searchResearchFile1stResultUpdatedBy = By.CssSelector("div[data-testid='researchFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable']:nth-child(6)");
        private By searchResearchFile1stResultUpdateDate = By.CssSelector("div[data-testid='researchFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable']:nth-child(7)");
        private By searchResearchFile1stResultStatus = By.CssSelector("div[data-testid='researchFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable']:nth-child(8)");

        //Search Research File Pagination
        private By searchResearchFilePaginationMenu = By.CssSelector("div[class='Menu-root']");
        private By searchResearchPaginationList = By.CssSelector("ul[class='pagination']");

        private By researchFileHeaderCode = By.XPath("//strong[contains(text(),'R-')]");

        public SearchResearchFiles(IWebDriver webDriver) : base(webDriver)
        {}

        //Navigates to Search a Research File
        public void NavigateToSearchResearchFile()
        {
            Wait();
            FocusAndClick(menuResearchButton);

            Wait();
            FocusAndClick(searchResearchButton);
        }

        public void SearchResearchFileByRFile(string RFile)
        {
            Wait(2000);
            ChooseSpecificSelectOption(searchResearchBySelect, "Research file #");
            webDriver.FindElement(searchResearchFileNbrInput).SendKeys(RFile);
            ChooseSpecificSelectOption(searchResearchStatusSelect, "All Status");

            WaitUntilClickable(searchResearchFileButton);
            webDriver.FindElement(searchResearchFileButton).Click();
        }

        public void SearchLastResearchFile()
        {
            WaitUntilClickable(searchResearchFileResetButton);
            webDriver.FindElement(searchResearchFileResetButton).Click();

            WaitUntilClickable(searchResearchFileSortByRFileBttn);
            webDriver.FindElement(searchResearchFileSortByRFileBttn).Click();
            webDriver.FindElement(searchResearchFileSortByRFileBttn).Click();

            WaitUntilClickable(searchResearchStatusSelect);
            ChooseSpecificSelectOption(searchResearchStatusSelect, "All Status");
            FocusAndClick(searchResearchFileButton);
        }

        public void SelectFirstResult()
        {
            WaitUntilClickable(searchResearchFile1stResultLink);
            webDriver.FindElement(searchResearchFile1stResultLink).Click();

            WaitUntilVisible(researchFileHeaderCode);
            Assert.True(webDriver.FindElement(researchFileHeaderCode).Displayed);
        }

        public void VerifyResearchFileListView()
        {
            WaitUntilVisible(searchResearchRegionInput);

            //Search Bar Elements
            Assert.True(webDriver.FindElement(searchResearchRegionInput).Displayed);
            Assert.True(webDriver.FindElement(searchResearchBySelect).Displayed);
            if (webDriver.FindElements(searchResearchFileNbrInput).Count > 0)
            {
                Assert.True(webDriver.FindElement(searchResearchFileNbrInput).Displayed);
            }
            else
            {
                Assert.True(webDriver.FindElement(searchResearchNameInput).Displayed);
            }
            Assert.True(webDriver.FindElement(searchResearchStatusSelect).Displayed);
            Assert.True(webDriver.FindElement(searchResearchRoadInput).Displayed);
            Assert.True(webDriver.FindElement(searchResearchCreateUpdateDateSelect).Displayed);
            Assert.True(webDriver.FindElement(searchResearchFromDateInput).Displayed);
            Assert.True(webDriver.FindElement(searchResearchToDateInput).Displayed);
            Assert.True(webDriver.FindElement(searchResearchCreateUpdateBySelect).Displayed);
            Assert.True(webDriver.FindElement(searchResearchUserUpdatedIdirInput).Displayed);
            Assert.True(webDriver.FindElement(searchResearchFileButton).Displayed);
            Assert.True(webDriver.FindElement(searchResearchFileResetButton).Displayed);
            Assert.True(webDriver.FindElement(searchResearchCreateNewBttn).Displayed);

            //Table Elements
            Assert.True(webDriver.FindElement(searchResearchFileNbrLabel).Displayed);
            Assert.True(webDriver.FindElement(searchResearchFileNameLabel).Displayed);
            Assert.True(webDriver.FindElement(searchResearchFileRegionLabel).Displayed);
            Assert.True(webDriver.FindElement(searchResearchFileCreatedByLabel).Displayed);
            Assert.True(webDriver.FindElement(searchResearchFileCreatedDateLabel).Displayed);
            Assert.True(webDriver.FindElement(searchResearchFileUpdatedByLabel).Displayed);
            Assert.True(webDriver.FindElement(searchResearchFileUpdatedDateLabel).Displayed);
            Assert.True(webDriver.FindElement(searchResearchFileStatusLabel).Displayed);
            Assert.True(webDriver.FindElement(searchResearchFileSortByRFileBttn).Displayed);

            //Pagination Elements
            WaitUntilVisible(searchResearchFilePaginationMenu);
            Assert.True(webDriver.FindElement(searchResearchFilePaginationMenu).Displayed);
            Assert.True(webDriver.FindElement(searchResearchPaginationList).Displayed);
        }

        public void VerifyResearchFileTableContent(ResearchFile researchFile, string user)
        {
            AssertTrueIsDisplayed(searchResearchFile1stResultLink);
            AssertTrueContentEquals(searchResearchFile1stResultFileName, researchFile.ResearchFileName);
            Assert.True(webDriver.FindElement(searchResearchFile1stResultRegion).Text != "");
            AssertTrueContentEquals(searchResearchFile1stResultCreator, user);
            AssertTrueContentEquals(searchResearchFile1stResultCreateDate, GetTodayFormattedDate());
            AssertTrueContentEquals(searchResearchFile1stResultUpdatedBy, user);
            AssertTrueContentEquals(searchResearchFile1stResultUpdateDate, GetTodayFormattedDate());
            AssertTrueContentEquals(searchResearchFile1stResultStatus, researchFile.Status);
        }

        public void FilterResearchFiles(string name, string status, string roadName, string idir)
        {
            WaitUntilClickable(searchResearchFileResetButton);
            webDriver.FindElement(searchResearchFileResetButton).Click();

            WaitUntilVisible(searchResearchNameInput);
            webDriver.FindElement(searchResearchNameInput).SendKeys(name);
            ChooseSpecificSelectOption(searchResearchStatusSelect, status);
            webDriver.FindElement(searchResearchRoadInput).SendKeys(roadName);
            ChooseSpecificSelectOption(searchResearchCreateUpdateBySelect, "Created by");
            webDriver.FindElement(searchResearchFileUserCreatedIdirInput).SendKeys(idir);

            webDriver.FindElement(searchResearchFileButton).Click();
        }

        public Boolean SearchFoundResults()
        {
            Wait(2000);
            return webDriver.FindElements(searchResearchFile1stResult).Count > 0;
        }
    }
}
