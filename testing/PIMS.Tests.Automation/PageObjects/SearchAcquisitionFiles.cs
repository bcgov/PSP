using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SearchAcquisitionFiles : PageObjectBase
    {
        private readonly By menuAcquisitionButton = By.CssSelector("div[data-testid='nav-tooltip-acquisition'] a");
        private readonly By searchAcquisitionButton = By.XPath("//a[contains(text(),'Manage Acquisition Files')]");

        //Acquisition File Search Filters Elements
        private readonly By searchAcquisitionFileTitle = By.XPath("//h3[contains(text(),'Acquisition Files')]");

        private readonly By searchAcquisitionFileSearchBySelect = By.Id("input-searchBy");
        private readonly By searchAcquisitionFileSearchByAddressInput = By.Id("input-address");
        private readonly By searchAcquisitionFileSearchByPIDInput = By.Id("input-pid");
        private readonly By searchAcquisitionFileSearchByPINInput = By.Id("input-pin");
        private readonly By searchAcquisitionFileNameInput = By.Id("input-acquisitionFileNameOrNumber");
        private readonly By searchAcquisitionFileTeamMemberSelect = By.Id("multiselect-acquisitionTeamMembers");
        private readonly By searchAcquisitionFileTeamMemberOptions = By.CssSelector("div[id='multiselect-acquisitionTeamMembers'] ul[class='optionContainer']");
        private readonly By searchAcquisitionFileTeamMember1stOption = By.CssSelector("div[id='multiselect-acquisitionTeamMembers'] ul[class='optionContainer'] li:nth-child(1)");
        private readonly By searchAcquisitionFileStatusSelect = By.Id("input-acquisitionFileStatusTypeCode");
        private readonly By searchAcquisitionFileProjectInput = By.Id("input-projectNameOrNumber");
        private readonly By searchAcquisitionFileSearchButton = By.Id("search-button");
        private readonly By searchAcquisitionFileResetButton = By.Id("reset-button");
        private readonly By searchAcquisitionFileCreateNewButton = By.XPath("//button[@type='button']/div[contains(text(),'Create an acquisition file')]/parent::button");

        //Acquisition Files List Elements
        private readonly By searchAcquisitionFileNumberHeader = By.XPath("//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Acquisition file #')]");
        private readonly By searchAcquisitionOrderFileNumberBttn = By.CssSelector("div[data-testid='sort-column-fileNumber']");
        private readonly By searchAcquisitionLegacyNumberHeader = By.XPath("//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Historical file #')]");
        private readonly By searchAcquisitionOrderLegacyNumberBttn = By.CssSelector("div[data-testid='sort-column-legacyFileNumber']");
        private readonly By searchAcquisitionFileNameHeader = By.XPath("//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Acquisition file name')]");
        private readonly By searchAcquisitionOrderFileNameBttn = By.CssSelector("div[data-testid='sort-column-fileName']");
        private readonly By searchAcquisitionFileMOTIRegionHeader = By.XPath("//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'MOTI Region')]");
        private readonly By searchAcquisitionFileProjectHeader = By.XPath("//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Project')]");
        private readonly By searchAcquisitionFileAddressHeader = By.XPath("//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Civic Address')]");
        private readonly By searchAcquisitionFileStatusHeader = By.XPath("//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Status')]");
        private readonly By searchAcquisitionFileTableContent = By.CssSelector("div[data-testid='acquisitionFilesTable'] div[class='tbody'] div[class='tr-wrapper']");

        //Search Acquisition File Pagination
        private readonly By searchAcquisitionFilePaginationMenu = By.CssSelector("div[class='Menu-root']");
        private readonly By searchAcquisitionPaginationList = By.CssSelector("ul[class='pagination']");

        //Acquisition File Sort and 1st Result Elements
        private readonly By searchAcquisitionFile1stResult = By.CssSelector("div[data-testid='acquisitionFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1)");
        private readonly By searchAcquisitionFile1stResultLink = By.CssSelector("div[data-testid='acquisitionFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable'] a");
        private readonly By searchAcquisitionFile1stResultHistoricalFile = By.CssSelector("div[data-testid='acquisitionFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable']:nth-child(2)");
        private readonly By searchAcquisitionFile1stResultName = By.CssSelector("div[data-testid='acquisitionFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable']:nth-child(3)");
        private readonly By searchAcquisitionFile1stResultMOTIRegion = By.CssSelector("div[data-testid='acquisitionFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable']:nth-child(4)");
        private readonly By searchAcquisitionFile1stResultProject = By.CssSelector("div[data-testid='acquisitionFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable']:nth-child(5)");
        private readonly By searchAcquisitionFile1stResultAddress = By.CssSelector("div[data-testid='acquisitionFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td'] div[class='w-100'] div");
        private readonly By searchAcquisitionFile1stResultStatus = By.CssSelector("div[data-testid='acquisitionFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable']:nth-child(8)");

        private readonly By searchAcquisitionFileHeaderCode = By.XPath("//label[contains(text(), 'File:')]/parent::div/following-sibling::div[1]");
       

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
            Wait(5000);
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
                ChooseSpecificSelectOption(searchAcquisitionFileSearchBySelect, "Civic Address");
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
            AssertTrueIsDisplayed(searchAcquisitionFileSearchByAddressInput);
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
                Assert.NotEmpty(webDriver.FindElements(searchAcquisitionFile1stResultAddress));

            AssertTrueContentEquals(searchAcquisitionFile1stResultStatus, acquisition.AcquisitionStatus);
        }
    }
}
