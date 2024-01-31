

using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SearchAcquisitionFiles : PageObjectBase
    {
        private By menuAcquisitionButton = By.CssSelector("div[data-testid='nav-tooltip-acquisition'] a");
        private By searchAcquisitionButton = By.XPath("//a[contains(text(),'Manage Acquisition Files')]");

        //Acquisition File Search Filters Elements
        private By searchAcquisitionFileTitle = By.XPath("//h3[contains(text(),'Acquisition Files')]");

        private By searchAcquisitionFileSearchBySelect = By.Id("input-searchBy");
        private By searchAcquisitionFileSearchByAddressInput = By.Id("input-address");
        private By searchAcquisitionFileSearchByPIDInput = By.Id("input-pid");
        private By searchAcquisitionFileSearchByPINInput = By.Id("input-pin");
        private By searchAcquisitionFileNameInput = By.Id("input-acquisitionFileNameOrNumber");
        private By searchAcquisitionFileTeamMemberSelect = By.Id("multiselect-acquisitionTeamMembers");
        private By searchAcquisitionFileTeamMemberOptions = By.CssSelector("div[id='multiselect-acquisitionTeamMembers'] ul[class='optionContainer']");
        private By searchAcquisitionFileTeamMember1stOption = By.CssSelector("div[id='multiselect-acquisitionTeamMembers'] ul[class='optionContainer'] li:nth-child(1)");
        private By searchAcquisitionFileStatusSelect = By.Id("input-acquisitionFileStatusTypeCode");
        private By searchAcquisitionFileProjectInput = By.Id("input-projectNameOrNumber");
        private By searchAcquisitionFileSearchButton = By.Id("search-button");
        private By searchAcquisitionFileResetButton = By.Id("reset-button");
        private By searchAcquisitionFileCreateNewButton = By.XPath("//button[@type='button']/div[contains(text(),'Create an acquisition file')]/parent::button");

        //Acquisition Files List Elements
        private By searchAcquisitionFileNumberHeader = By.XPath("//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Acquisition file #')]");
        private By searchAcquisitionOrderFileNumberBttn = By.CssSelector("div[data-testid='sort-column-fileNumber']");
        private By searchAcquisitionLegacyNumberHeader = By.XPath("//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Historical file #')]");
        private By searchAcquisitionOrderLegacyNumberBttn = By.CssSelector("div[data-testid='sort-column-legacyFileNumber']");
        private By searchAcquisitionFileNameHeader = By.XPath("//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Acquisition file name')]");
        private By searchAcquisitionOrderFileNameBttn = By.CssSelector("div[data-testid='sort-column-fileName']");
        private By searchAcquisitionFileMOTIRegionHeader = By.XPath("//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'MOTI Region')]");
        private By searchAcquisitionFileProjectHeader = By.XPath("//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Project')]");
        private By searchAcquisitionFileAddressHeader = By.XPath("//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Civic Address')]");
        private By searchAcquisitionFileStatusHeader = By.XPath("//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Status')]");
        private By searchAcquisitionFileTableContent = By.CssSelector("div[data-testid='acquisitionFilesTable'] div[class='tbody'] div[class='tr-wrapper']");

        //Search Acquisition File Pagination
        private By searchAcquisitionFilePaginationMenu = By.CssSelector("div[class='Menu-root']");
        private By searchAcquisitionPaginationList = By.CssSelector("ul[class='pagination']");

        //Acquisition File Sort and 1st Result Elements
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
            WaitUntilTableSpinnerDisappear();

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

            WaitUntilClickable(searchAcquisitionOrderFileNumberBttn);
            webDriver.FindElement(searchAcquisitionOrderFileNumberBttn).Click();

            Wait();
            webDriver.FindElement(searchAcquisitionOrderFileNumberBttn).Click();
        }

        public void OrderByAcquisitionFileNumber()
        {
            WaitUntilClickable(searchAcquisitionOrderFileNumberBttn);
            webDriver.FindElement(searchAcquisitionOrderFileNumberBttn).Click();
        }

        public void OrderByAcquisitionFileHistoricalNumber()
        {
            WaitUntilClickable(searchAcquisitionOrderLegacyNumberBttn);
            webDriver.FindElement(searchAcquisitionOrderLegacyNumberBttn).Click();
        }

        public void OrderByAcquisitionFileName()
        {
            WaitUntilClickable(searchAcquisitionOrderFileNameBttn);
            webDriver.FindElement(searchAcquisitionOrderFileNameBttn).Click();
        }

        public void SelectFirstOption()
        {
            WaitUntilClickable(searchAcquisitionFile1stResultLink);
            webDriver.FindElement(searchAcquisitionFile1stResultLink).Click();

            WaitUntilClickable(searchAcquisitionFileHeaderCode);
            Assert.True(webDriver.FindElement(searchAcquisitionFileHeaderCode).Displayed);
        }

        public void FilterAcquisitionFiles(string pid = "", string pin = "", string address = "", string name = "", string teamMember = "", string status = "", string project = "")
        {
            Wait(10000);
            webDriver.FindElement(searchAcquisitionFileResetButton).Click();

            Wait();
            if (pid != "")
            {
                ChooseSpecificSelectOption(searchAcquisitionFileSearchBySelect, "PID");
                webDriver.FindElement(searchAcquisitionFileSearchByPIDInput).SendKeys(pid);
            }

            if (pin != "")
            {
                ChooseSpecificSelectOption(searchAcquisitionFileSearchBySelect, "PIN");
                webDriver.FindElement(searchAcquisitionFileSearchByPINInput).SendKeys(pin);
            }

            if (address != "")
            {
                ChooseSpecificSelectOption(searchAcquisitionFileSearchBySelect, "Address");
                webDriver.FindElement(searchAcquisitionFileSearchByAddressInput).SendKeys(address);
            }
            if (name != "")
                webDriver.FindElement(searchAcquisitionFileNameInput).SendKeys(name);

            if (teamMember != "")
            {
                webDriver.FindElement(searchAcquisitionFileTeamMemberSelect).SendKeys(teamMember);
                WaitUntilVisible(searchAcquisitionFileTeamMemberOptions);
                webDriver.FindElement(searchAcquisitionFileTeamMember1stOption).Click();
            }

            if(status != "")
                ChooseSpecificSelectOption(searchAcquisitionFileStatusSelect, status);

            if (project != "")
                webDriver.FindElement(searchAcquisitionFileProjectInput).SendKeys(project);

            webDriver.FindElement(searchAcquisitionFileSearchButton).Click();
        }

        public Boolean SearchFoundResults()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElements(searchAcquisitionFile1stResult).Count > 0;
        }

        public string FirstAcquisitionFileNumber()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(searchAcquisitionFile1stResultLink).Text;
        }

        public string FirstAcquisitionLegacyFileNumber()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(searchAcquisitionFile1stResultHistoricalFile).Text;
        }

        public string FirstAcquisitionFileName()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(searchAcquisitionFile1stResultName).Text;
        }

        public int AcquisitionFileTableResultNumber()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElements(searchAcquisitionFileTableContent).Count;
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
            AssertTrueIsDisplayed(searchAcquisitionOrderFileNumberBttn);
            AssertTrueIsDisplayed(searchAcquisitionLegacyNumberHeader);
            AssertTrueIsDisplayed(searchAcquisitionOrderLegacyNumberBttn);
            AssertTrueIsDisplayed(searchAcquisitionFileNameHeader);
            AssertTrueIsDisplayed(searchAcquisitionOrderFileNameBttn);
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

            if(acquisition.HistoricalFileNumber != "")
                AssertTrueContentEquals(searchAcquisitionFile1stResultHistoricalFile, acquisition.HistoricalFileNumber);

            AssertTrueContentEquals(searchAcquisitionFile1stResultName, acquisition.AcquisitionFileName);
            AssertTrueContentEquals(searchAcquisitionFile1stResultMOTIRegion, acquisition.AcquisitionMOTIRegion);

            if(acquisition.AcquisitionProjCode != "")
                AssertTrueContentEquals(searchAcquisitionFile1stResultProject, acquisition.AcquisitionProjCode + " " + acquisition.AcquisitionProject);

            if(acquisition.AcquisitionSearchPropertiesIndex != 0)
                Assert.NotEqual(0, webDriver.FindElements(searchAcquisitionFile1stResultAddress).Count());

            AssertTrueContentEquals(searchAcquisitionFile1stResultStatus, acquisition.AcquisitionStatus);
        }
    }
}
