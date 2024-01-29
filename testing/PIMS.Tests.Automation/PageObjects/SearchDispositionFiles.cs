using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SearchDispositionFiles : PageObjectBase
    {
        private By menuDispositionButton = By.CssSelector("div[data-testid='nav-tooltip-disposition'] a");
        private By searcDispositionButton = By.XPath("//a[contains(text(),'Manage Disposition Files')]");

        //Disposition File Search Filters Elements
        private By searchAcquisitionFileTitle = By.XPath("//h3[contains(text(),'Disposition Files')]");

        private By searchDispositionFileSearchBySelect = By.Id("input-searchBy");
        private By searchDispositionFileSearchByAddressInput = By.Id("input-address");
        private By searchDispositionFileSearchByPIDInput = By.Id("input-pid");
        private By searchDispositionFileNameInput = By.Id("input-fileNameOrNumberOrReference");
        private By searchDispositionFileTeamMemberSelect = By.Id("typeahead-select-dispositionTeamMember");
        private By searchDispositionTeamMemberOptions = By.CssSelector("div[data-testid='typeahead-select-dispositionTeamMember'] div[aria-label='menu-options']");
        private By searchDispositionTeamMember1stOption = By.CssSelector("div[data-testid='typeahead-select-dispositionTeamMember'] div[aria-label='menu-options'] a:nth-child(1)");
        private By searchDispositionFileStatusSelect = By.Id("input-dispositionFileStatusCode");
        private By searchDispositionStatusSelect = By.Id("input-dispositionStatusCode");
        private By searchDispositionTypeSelect = By.Id("input-dispositionTypeCode");
        private By searchDispositionFileSearchButton = By.Id("search-button");
        private By searchDispositionFileResetButton = By.Id("reset-button");
        private By searchDispositionFileCreateNewButton = By.XPath("//button[@type='button']/div[contains(text(),'Add a Disposition File')]/parent::button");

        //Disposition Files List Elements
        private By searchDispositionFileNumberHeader = By.XPath("//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Disposition file #')]");
        private By searchDispositionOrderFileNumberBttn = By.CssSelector("div[data-testid='sort-column-fileNumber']");
        private By searchDispositionReferenceHeader = By.XPath("//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Reference #')]");
        private By searchDispositionOrderReferenceBttn = By.CssSelector("div[data-testid='sort-column-fileReference']");
        private By searchDispositionFileNameHeader = By.XPath("//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Disposition file name')]");
        private By searchDispositionOrderFileNameBttn = By.CssSelector("div[data-testid='sort-column-fileName']");
        private By searchDispositionFileTypeHeader = By.XPath("//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Disposition type')]");
        private By searchAcquisitionFileOrderTypeBttn = By.CssSelector("div[data-testid='sort-column-dispositionTypeCode']");
        private By searchDispositionMOTIRegionHeader = By.XPath("//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'MOTI Region')]");
        private By searchDispositionTeamMemberHeader = By.XPath("//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Team member')]");
        private By searchDispositionAddressHeader = By.XPath("//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Civic Address / PID / PIN')]");
        private By searchDispositionStatusHeader = By.XPath("//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Disposition status')]");
        private By searchDispositionFileStatusHeader = By.XPath("//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Status')]");
        private By searchAcquisitionFileOrderStatusBttn = By.CssSelector("div[data-testid='sort-column-dispositionFileStatusTypeCode']");
        private By searchDispositionFileTableContent = By.CssSelector("div[data-testid='dispositionFilesTable'] div[class='tbody'] div[class='tr-wrapper']");

        //Search Disposition File Pagination
        private By searchDispositionFilePaginationMenu = By.CssSelector("div[class='Menu-root']");
        private By searchDispositionPaginationList = By.CssSelector("ul[class='pagination']");

        //Disposition 1st Result Elements
        private By searchDispositionFile1stResult = By.CssSelector("div[data-testid='dispositionFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1)");
        private By searchDispositionFile1stResultLink = By.CssSelector("div[data-testid='dispositionFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable'] a");
        private By searchDispositionFile1stResultReference = By.CssSelector("div[data-testid='dispositionFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable']:nth-child(2)");
        private By searchDispositionFile1stResultName = By.CssSelector("div[data-testid='dispositionFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable']:nth-child(3)");
        private By searchDispositionFile1stResultType = By.CssSelector("div[data-testid='dispositionFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable']:nth-child(4)");
        private By searchDispositionFile1stResultMOTIRegion = By.CssSelector("div[data-testid='dispositionFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable']:nth-child(5)");
        private By searchDispositionFile1stResultTeamMember = By.CssSelector("div[data-testid='dispositionFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable']:nth-child(6) span");
        private By searchDispositionFile1stResultAddress = By.CssSelector("div[data-testid='dispositionFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td'] div[class='w-100'] div");
        private By searchDispositionFile1stResultStatus = By.CssSelector("div[data-testid='dispositionFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable']:nth-child(8)");
        private By searchDispositionFile1stResultStatusFile = By.CssSelector("div[data-testid='dispositionFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable']:nth-child(9)");

        private By searchDispositionFileHeaderCode = By.XPath("//label[contains(text(), 'File:')]/parent::div/following-sibling::div[1]/strong");

        public SearchDispositionFiles(IWebDriver webDriver) : base(webDriver)
        {}

        //Navigates to Search an DispositionFile
        public void NavigateToSearchDispositionFile()
        {
            Wait(3000);
            FocusAndClick(menuDispositionButton);

            Wait(3000);
            FocusAndClick(searcDispositionButton);
        }

        public void SearchDispositionFileByDFile(string DFile)
        {
            WaitUntilTableSpinnerDisappear();

            WaitUntilVisible(searchDispositionFileNameInput);
            webDriver.FindElement(searchDispositionFileNameInput).SendKeys(DFile);

            FocusAndClick(searchDispositionFileSearchButton);
        }

        public void SearchLastDispositionFile()
        {
            Wait(2000);
            webDriver.FindElement(searchDispositionFileResetButton).Click();

            Wait(2000);
            webDriver.FindElement(searchDispositionFileNameInput).SendKeys("Automated");
            webDriver.FindElement(searchDispositionFileSearchButton).Click();

            WaitUntilClickable(searchDispositionOrderFileNumberBttn);
            webDriver.FindElement(searchDispositionOrderFileNumberBttn).Click();

            Wait();
            webDriver.FindElement(searchDispositionOrderFileNumberBttn).Click();
        }

        public void OrderByDispositionFileNumber()
        {
            WaitUntilClickable(searchDispositionOrderFileNumberBttn);
            webDriver.FindElement(searchDispositionOrderFileNumberBttn).Click();
        }

        public void OrderByDispositionFileReferenceNumber()
        {
            WaitUntilClickable(searchDispositionOrderReferenceBttn);
            webDriver.FindElement(searchDispositionOrderReferenceBttn).Click();
        }

        public void OrderByDispositionFileName()
        {
            WaitUntilClickable(searchDispositionOrderFileNameBttn);
            webDriver.FindElement(searchDispositionOrderFileNameBttn).Click();
        }

        public void OrderByDispositionFileType()
        {
            WaitUntilClickable(searchAcquisitionFileOrderTypeBttn);
            webDriver.FindElement(searchAcquisitionFileOrderTypeBttn).Click();
        }

        public void OrderByDispositionFileStatus()
        {
            WaitUntilClickable(searchAcquisitionFileOrderStatusBttn);
            webDriver.FindElement(searchAcquisitionFileOrderStatusBttn).Click();
        }

        public void SelectFirstOption()
        {
            WaitUntilClickable(searchDispositionFile1stResultLink);
            webDriver.FindElement(searchDispositionFile1stResultLink).Click();

            WaitUntilClickable(searchDispositionFileHeaderCode);
            AssertTrueIsDisplayed(searchDispositionFileHeaderCode);
        }

        public void FilterDispositionFiles(string pid = "", string name = "", string teamMember = "", string status = "", string dispStatus = "", string type = "")
        {
            Wait(10000);
            webDriver.FindElement(searchDispositionFileResetButton).Click();

            Wait();
            if (pid != "")
            {
                ChooseSpecificSelectOption(searchDispositionFileSearchBySelect, "PID");
                webDriver.FindElement(searchDispositionFileSearchByPIDInput).SendKeys(pid);
            }

            if(name != "")
                webDriver.FindElement(searchDispositionFileNameInput).SendKeys(name);

            if (teamMember != "")
            {
                webDriver.FindElement(searchDispositionFileTeamMemberSelect).SendKeys(teamMember);
                WaitUntilVisible(searchDispositionTeamMemberOptions);
                webDriver.FindElement(searchDispositionTeamMember1stOption).Click();
            }

            if(status != "")
                ChooseSpecificSelectOption(searchDispositionFileStatusSelect, status);

            if (dispStatus != "")
                ChooseSpecificSelectOption(searchDispositionStatusSelect, dispStatus);

            if (type != "")
                ChooseSpecificSelectOption(searchDispositionTypeSelect, type);

            webDriver.FindElement(searchDispositionFileSearchButton).Click();
        }

        public Boolean SearchFoundResults()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElements(searchDispositionFile1stResult).Count > 0;
        }

        public string FirstDispositionFileNumber()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(searchDispositionFile1stResultLink).Text;
        }

        public string FirstDispositionReferenceNumber()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(searchDispositionFile1stResultReference).Text;
        }

        public string FirstDispositionFileName()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(searchDispositionFile1stResultName).Text;
        }

        public string FirstDispositionFileType()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(searchDispositionFile1stResultType).Text;
        }

        public string FirstDispositionStatus()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(searchDispositionFile1stResultStatus).Text;
        }

        public string FirstDispositionFileStatus()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(searchDispositionFile1stResultStatusFile).Text;
        }

        public int DispositionFileTableResultNumber()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElements(searchDispositionFileTableContent).Count;
        }

        public void VerifyAcquisitionFileListView()
        {
            Wait();

            //Acquisition File Title
            AssertTrueIsDisplayed(searchAcquisitionFileTitle);

            //Acquisition File Search Filters
            AssertTrueIsDisplayed(searchDispositionFileSearchBySelect);
            AssertTrueIsDisplayed(searchDispositionFileSearchByAddressInput);
            AssertTrueIsDisplayed(searchDispositionFileNameInput);
            AssertTrueIsDisplayed(searchDispositionFileTeamMemberSelect);
            AssertTrueIsDisplayed(searchDispositionFileStatusSelect);
            AssertTrueIsDisplayed(searchDispositionStatusSelect);
            AssertTrueIsDisplayed(searchDispositionTypeSelect);

            AssertTrueIsDisplayed(searchDispositionFileSearchButton);
            AssertTrueIsDisplayed(searchDispositionFileCreateNewButton);

            //Acquisition Files List View
            AssertTrueIsDisplayed(searchDispositionFileNumberHeader);
            AssertTrueIsDisplayed(searchDispositionOrderFileNumberBttn);
            AssertTrueIsDisplayed(searchDispositionReferenceHeader);
            AssertTrueIsDisplayed(searchDispositionOrderReferenceBttn);
            AssertTrueIsDisplayed(searchDispositionFileNameHeader);
            AssertTrueIsDisplayed(searchDispositionOrderFileNameBttn);
            AssertTrueIsDisplayed(searchDispositionFileTypeHeader);
            AssertTrueIsDisplayed(searchAcquisitionFileOrderTypeBttn);
            AssertTrueIsDisplayed(searchDispositionMOTIRegionHeader);
            AssertTrueIsDisplayed(searchDispositionTeamMemberHeader);
            AssertTrueIsDisplayed(searchDispositionAddressHeader);
            AssertTrueIsDisplayed(searchDispositionStatusHeader);
            AssertTrueIsDisplayed(searchDispositionFileStatusHeader);
            AssertTrueIsDisplayed(searchAcquisitionFileOrderStatusBttn);
            AssertTrueIsDisplayed(searchDispositionFileTableContent);

            //Acquisition File Pagination
            AssertTrueIsDisplayed(searchDispositionFilePaginationMenu);
            AssertTrueIsDisplayed(searchDispositionPaginationList);
        }

        public void VerifyAcquisitionFileTableContent(DispositionFile disposition)
        {
            AssertTrueIsDisplayed(searchDispositionFile1stResult);

            if(disposition.DispositionReferenceNumber != "")
                AssertTrueContentEquals(searchDispositionFile1stResultReference, disposition.DispositionReferenceNumber);

            AssertTrueContentEquals(searchDispositionFile1stResultName, disposition.DispositionFileName);
            AssertTrueContentEquals(searchDispositionFile1stResultType, disposition.DispositionType);
            AssertTrueContentEquals(searchDispositionFile1stResultMOTIRegion, disposition.DispositionMOTIRegion);

            if(disposition.DispositionTeam.Count > 0)
                Assert.NotEqual(0, webDriver.FindElements(searchDispositionFile1stResultTeamMember).Count());

            if (disposition.DispositionSearchPropertiesIndex != 0)
                Assert.NotEqual(0, webDriver.FindElements(searchDispositionFile1stResultAddress).Count());

            AssertTrueContentEquals(searchDispositionFile1stResultStatus, disposition.DispositionStatus);
            AssertTrueContentEquals(searchDispositionFile1stResultStatusFile, disposition.DispositionFileStatus);
        }
    }
}
