

using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SearchAcquisitionFiles : PageObjectBase
    {
        private By menuAcquisitionButton = By.XPath("//a/label[contains(text(),'Acquisition')]/parent::a");
        private By searchAcquisitionButton = By.XPath("//a[contains(text(),'Manage Acquisition Files')]");

        //Acquisition File Search Filters Elements
        private By searchAcquisitionFileTitle = By.XPath("//h3[contains(text(),'Acquisition Files')]");

        private By searchAcquisitionFileSearchBySelect = By.Id("input-searchBy");
        private By searchAcquisitionFileSearchByAddressInput = By.Id("input-address");
        private By searchAcquisitionFileSearchByPIDInput = By.Id("input-pid");
        private By searchAcquisitionFileStatusSelect = By.Id("input-acquisitionFileStatusTypeCode");
        private By searchAcquisitionFileNameInput = By.Id("input-acquisitionFileNameOrNumber");
        private By searchAcquisitionFileProjectInput = By.Id("input-projectNameOrNumber");
        private By searchAcquisitionFileSearchButton = By.Id("search-button");
        private By searchAcquisitionFileResetButton = By.Id("reset-button");
        private By searchAcquisitionFileCreateNewButton = By.XPath("//button[@type='button']/div[contains(text(),'Create an acquisition file')]/parent::button");

        //Acquisition Files List Elements
        private By searchAcquisitionFileNumberHeader = By.XPath("//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Acquisition file #')]");
        private By searchAcquisitionFileNameHeader = By.XPath("//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Acquisition file name')]");
        private By searchAcquisitionFileMOTIRegionHeader = By.XPath("//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'MOTI Region')]");
        private By searchAcquisitionFileProjectHeader = By.XPath("//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Project')]");
        private By searchAcquisitionFileAddressHeader = By.XPath("//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Civic Address')]");
        private By searchAcquisitionFileStatusHeader = By.XPath("//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Status')]");
        private By searchAcquisitionFileTableContent = By.XPath("//div[@role='table']/div[@class='tbody']/div");

        //Search Acquisition File Pagination
        private By searchAcquisitionFilePaginationMenu = By.CssSelector("div[class='Menu-root']");
        private By searchAcquisitionPaginationList = By.CssSelector("ul[class='pagination']");

        //Acquisition File Sort and 1st Result Elements
        private By searchAcquisitionFileSortByAFileBttn = By.CssSelector("div[data-testid='sort-column-fileNumber']");

        private By searchAcquisitionFile1stResult = By.CssSelector("div[data-testid='acquisitionFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1)");
        private By searchAcquisitionFile1stResultLink = By.CssSelector("div[data-testid='acquisitionFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable'] a");
        private By searchAcquisitionFile1stResultHistoricalFile = By.CssSelector("div[data-testid='acquisitionFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable']:nth-child(2)");
        private By searchAcquisitionFile1stResultName = By.CssSelector("div[data-testid='acquisitionFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable']:nth-child(3)");
        private By searchAcquisitionFile1stResultMOTIRegion = By.CssSelector("div[data-testid='acquisitionFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable']:nth-child(4)");
        private By searchAcquisitionFile1stResultProject = By.CssSelector("div[data-testid='acquisitionFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable']:nth-child(5)");
        private By searchAcquisitionFile1stResultAddress = By.CssSelector("div[data-testid='acquisitionFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td'] div[class='w-100'] div");
        private By searchAcquisitionFile1stResultStatus = By.CssSelector("div[data-testid='acquisitionFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable']:nth-child(8)");

        private By searchAcquisitionFileHeaderCode = By.XPath("//label[contains(text(), 'File:')]/parent::div/following-sibling::div[1]/strong");
       

        public SearchAcquisitionFiles(IWebDriver webDriver) : base(webDriver)
        {}

        //Navigates to Search an Acquisition File
        public void NavigateToSearchAcquisitionFile()
        {
            Wait(3000);
            FocusAndClick(menuAcquisitionButton);

            Wait(3000);
            FocusAndClick(searchAcquisitionButton);
        }

        public void SearchAcquisitionFileByAFile(string AFile)
        {
            Wait(2000);

            WaitUntilVisible(searchAcquisitionFileNameInput);
            webDriver.FindElement(searchAcquisitionFileNameInput).SendKeys(AFile);
            ChooseSpecificSelectOption(searchAcquisitionFileStatusSelect, "All Status");

            Wait(2000);
            FocusAndClick(searchAcquisitionFileSearchButton);
        }

        public void SearchLastAcquisitionFile()
        {
            Wait(2000);
            webDriver.FindElement(searchAcquisitionFileResetButton).Click();

            Wait(2000);
            ChooseSpecificSelectOption(searchAcquisitionFileStatusSelect, "All Status");
            webDriver.FindElement(searchAcquisitionFileNameInput).SendKeys("Automated");
            webDriver.FindElement(searchAcquisitionFileSearchButton).Click();

            WaitUntilClickable(searchAcquisitionFileSortByAFileBttn);
            webDriver.FindElement(searchAcquisitionFileSortByAFileBttn).Click();

            Wait();
            webDriver.FindElement(searchAcquisitionFileSortByAFileBttn).Click();
        }

        public void SelectFirstOption()
        {
            WaitUntilClickable(searchAcquisitionFile1stResultLink);
            webDriver.FindElement(searchAcquisitionFile1stResultLink).Click();

            WaitUntilClickable(searchAcquisitionFileHeaderCode);
            Assert.True(webDriver.FindElement(searchAcquisitionFileHeaderCode).Displayed);
        }

        public void FilterAcquisitionFiles(string pid, string name, string status)
        {
            Wait(10000);
            webDriver.FindElement(searchAcquisitionFileResetButton).Click();

            Wait();
            ChooseSpecificSelectOption(searchAcquisitionFileSearchBySelect, "PID");
            webDriver.FindElement(searchAcquisitionFileSearchByPIDInput).SendKeys(pid);
            ChooseSpecificSelectOption(searchAcquisitionFileStatusSelect, status);
            webDriver.FindElement(searchAcquisitionFileNameInput).SendKeys(name);

            webDriver.FindElement(searchAcquisitionFileSearchButton).Click();
        }

        public Boolean SearchFoundResults()
        {
            Wait();
            return webDriver.FindElements(searchAcquisitionFile1stResult).Count > 0;
        }

        public void VerifyAcquisitionFileListView()
        {
            Wait();

            //Acquisition File Title
            AssertTrueIsDisplayed(searchAcquisitionFileTitle);

            //Acquisition File Search Filters
            AssertTrueIsDisplayed(searchAcquisitionFileSearchBySelect);
            AssertTrueIsDisplayed(searchAcquisitionFileSearchByPIDInput);
            AssertTrueIsDisplayed(searchAcquisitionFileStatusSelect);
            AssertTrueIsDisplayed(searchAcquisitionFileNameInput);
            AssertTrueIsDisplayed(searchAcquisitionFileProjectInput);
            AssertTrueIsDisplayed(searchAcquisitionFileSearchButton);
            AssertTrueIsDisplayed(searchAcquisitionFileResetButton);
            AssertTrueIsDisplayed(searchAcquisitionFileCreateNewButton);

            //Acquisition Files List View
            AssertTrueIsDisplayed(searchAcquisitionFileNumberHeader);
            AssertTrueIsDisplayed(searchAcquisitionFileNameHeader);
            AssertTrueIsDisplayed(searchAcquisitionFileMOTIRegionHeader);
            AssertTrueIsDisplayed(searchAcquisitionFileProjectHeader);
            AssertTrueIsDisplayed(searchAcquisitionFileAddressHeader);
            AssertTrueIsDisplayed(searchAcquisitionFileStatusHeader);
            AssertTrueIsDisplayed(searchAcquisitionFileTableContent);

            //Acquisition File Pagination
            AssertTrueIsDisplayed(searchAcquisitionFilePaginationMenu);
            AssertTrueIsDisplayed(searchAcquisitionPaginationList);
        }

        public void VerifyAcquisitionFileTableContent(AcquisitionFile acquisition)
        {
            AssertTrueIsDisplayed(searchAcquisitionFile1stResultLink);
            AssertTrueContentEquals(searchAcquisitionFile1stResultHistoricalFile, acquisition.HistoricalFileNumber);
            AssertTrueContentEquals(searchAcquisitionFile1stResultName, acquisition.AcquisitionFileName);
            AssertTrueContentEquals(searchAcquisitionFile1stResultMOTIRegion, acquisition.AcquisitionMOTIRegion);
            AssertTrueContentEquals(searchAcquisitionFile1stResultProject, acquisition.AcquisitionProjCode + " " + acquisition.AcquisitionProject);
            Assert.True(webDriver.FindElements(searchAcquisitionFile1stResultAddress).Count() > 0);
            AssertTrueContentEquals(searchAcquisitionFile1stResultStatus, acquisition.AcquisitionStatus);
        }
    }
}
