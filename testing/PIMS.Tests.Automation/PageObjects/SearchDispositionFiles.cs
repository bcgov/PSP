using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SearchDispositionFiles : PageObjectBase
    {
        private readonly By menuManagementLeaseButton = By.CssSelector("div[data-testid='nav-tooltip-leases&licences'] a");
        private readonly By menuDispositionButton = By.CssSelector("div[data-testid='nav-tooltip-disposition'] a");
        private readonly By searcDispositionButton = By.XPath("//a[contains(text(),'Manage Disposition Files')]");

        //Disposition File Search Filters Elements
        private readonly By searchAcquisitionFileTitle = By.XPath("//h1/div/div/span[contains(text(),'Disposition Files')]");

        private readonly By searchDispositionFileSearchBySelect = By.Id("input-searchBy");
        private readonly By searchDispositionFileSearchByAddressInput = By.Id("input-address");
        private readonly By searchDispositionFileSearchByPIDInput = By.Id("input-pid");
        private readonly By searchDispositionFileSearchByPINInput = By.Id("input-pin");
        private readonly By searchDispositionFileNameInput = By.Id("input-fileNameOrNumberOrReference");
        private readonly By searchDispositionFileTeamMemberSelect = By.Id("typeahead-select-dispositionTeamMember");
        private readonly By searchDispositionTeamMemberOptions = By.CssSelector("div[data-testid='typeahead-select-dispositionTeamMember'] div[aria-label='menu-options']");
        private readonly By searchDispositionTeamMember1stOption = By.CssSelector("div[data-testid='typeahead-select-dispositionTeamMember'] div[aria-label='menu-options'] a:nth-child(1)");
        private readonly By searchDispositionFileStatusSelect = By.Id("input-dispositionFileStatusCode");
        private readonly By searchDispositionStatusSelect = By.Id("input-dispositionStatusCode");
        private readonly By searchDispositionTypeSelect = By.Id("input-dispositionTypeCode");
        private readonly By searchDispositionFileSearchButton = By.Id("search-button");
        private readonly By searchDispositionFileResetButton = By.Id("reset-button");
        private readonly By searchDispositionFileCreateNewButton = By.XPath("//button[@type='button']/div[contains(text(),'Add a Disposition File')]/parent::button");

        //Disposition Files List Elements
        private readonly By searchDispositionFileNumberHeader = By.XPath("//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Disposition file #')]");
        private readonly By searchDispositionOrderFileNumberBttn = By.CssSelector("div[data-testid='sort-column-fileNumber']");
        private readonly By searchDispositionReferenceHeader = By.XPath("//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Reference #')]");
        private readonly By searchDispositionOrderReferenceBttn = By.CssSelector("div[data-testid='sort-column-fileReference']");
        private readonly By searchDispositionFileNameHeader = By.XPath("//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Disposition file name')]");
        private readonly By searchDispositionOrderFileNameBttn = By.CssSelector("div[data-testid='sort-column-fileName']");
        private readonly By searchDispositionFileTypeHeader = By.XPath("//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Disposition type')]");
        private readonly By searchDispositionFileOrderTypeBttn = By.CssSelector("div[data-testid='sort-column-dispositionTypeCode']");
        private readonly By searchDispositionMOTIRegionHeader = By.XPath("//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'MOTT region')]");
        private readonly By searchDispositionTeamMemberHeader = By.XPath("//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Team member')]");
        private readonly By searchDispositionAddressHeader = By.XPath("//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Civic address / PID / PIN')]");
        private readonly By searchDispositionDispositionStatusHeader = By.XPath("//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Disposition status')]");
        private readonly By searchDispositionFileOrderDispositionStatusBttn = By.CssSelector("div[data-testid='sort-column-dispositionStatusTypeCode']");
        private readonly By searchDispositionFileStatusHeader = By.XPath("//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Status')]");
        private readonly By searchDispositionFileOrderStatusBttn = By.CssSelector("div[data-testid='sort-column-dispositionFileStatusTypeCode']");
        
        private readonly By searchDispositionFileTableContent = By.CssSelector("div[data-testid='dispositionFilesTable'] div[class='tbody'] div[class='tr-wrapper']");

        //Search Disposition File Pagination
        private readonly By searchDispositionFilePaginationMenu = By.CssSelector("div[class='Menu-root']");
        private readonly By searchDispositionPaginationList = By.CssSelector("ul[class='pagination']");

        //Disposition 1st Result Elements
        private readonly By searchDispositionFile1stResult = By.CssSelector("div[data-testid='dispositionFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1)");
        private readonly By searchDispositionFile1stResultLink = By.CssSelector("div[data-testid='dispositionFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable'] a");
        private readonly By searchDispositionFile1stResultReference = By.CssSelector("div[data-testid='dispositionFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable']:nth-child(2)");
        private readonly By searchDispositionFile1stResultName = By.CssSelector("div[data-testid='dispositionFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable']:nth-child(3)");
        private readonly By searchDispositionFile1stResultType = By.CssSelector("div[data-testid='dispositionFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable']:nth-child(4)");
        private readonly By searchDispositionFile1stResultMOTIRegion = By.CssSelector("div[data-testid='dispositionFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div:nth-child(5)");
        private readonly By searchDispositionFile1stResultTeamMember = By.CssSelector("div[data-testid='dispositionFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable']:nth-child(6) span");
        private readonly By searchDispositionFile1stResultAddress = By.CssSelector("div[data-testid='dispositionFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td'] div[class='w-100'] div");
        private readonly By searchDispositionFile1stResultStatus = By.CssSelector("div[data-testid='dispositionFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable']:nth-child(8)");
        private readonly By searchDispositionFile1stResultStatusFile = By.CssSelector("div[data-testid='dispositionFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable']:nth-child(9)");

        private readonly By searchDispositionFileHeaderCode = By.XPath("//label[contains(text(), 'File:')]/parent::div/following-sibling::div");

        public SearchDispositionFiles(IWebDriver webDriver) : base(webDriver)
        {}

        //Navigates to Search an DispositionFile
        public void NavigateToSearchDispositionFile()
        {
            SafeClick(menuManagementLeaseButton);
            SafeClick(menuDispositionButton);
            SafeClick(searcDispositionButton);
        }

        public void SearchLastDispositionFile()
        {
            WaitUntilClickable(searchDispositionFileResetButton);
            webDriver.FindElement(searchDispositionFileResetButton).Click();

            WaitUntilClickable(searchDispositionFileNameInput);
            webDriver.FindElement(searchDispositionFileNameInput).SendKeys("Automated");
            webDriver.FindElement(searchDispositionFileSearchButton).Click();

            WaitUntilClickable(searchDispositionOrderFileNumberBttn);
            webDriver.FindElement(searchDispositionOrderFileNumberBttn).Click();

            WaitUntilClickable(searchDispositionOrderFileNumberBttn);
            webDriver.FindElement(searchDispositionOrderFileNumberBttn).Click();
        }

        public void OrderByDispositionFileNumber()
        {
            Wait();
            SafeClick(searchDispositionOrderFileNumberBttn);
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
            WaitUntilClickable(searchDispositionFileOrderTypeBttn);
            webDriver.FindElement(searchDispositionFileOrderTypeBttn).Click();
        }

        public void OrderByDispositionStatus()
        {
            WaitUntilClickable(searchDispositionFileOrderDispositionStatusBttn);
            FocusAndClick(searchDispositionFileOrderDispositionStatusBttn);
        }

        public void OrderByDispositionFileStatus()
        {
            WaitUntilClickable(searchDispositionFileOrderStatusBttn);
            webDriver.FindElement(searchDispositionFileOrderStatusBttn).Click();
        }

        public void SelectFirstOption()
        {

            WaitUntilClickable(searchDispositionFile1stResultLink);
            webDriver.FindElement(searchDispositionFile1stResultLink).Click();

            WaitUntilVisible(searchDispositionFileHeaderCode);
            AssertTrueIsDisplayed(searchDispositionFileHeaderCode);
        }

        public void SelectLastOption()
        {
            var originalWindowHandle = webDriver.CurrentWindowHandle;

            WaitUntilClickable(searchDispositionOrderFileNumberBttn);
            SafeClick(searchDispositionOrderFileNumberBttn);

            WaitUntilClickable(searchDispositionOrderFileNumberBttn);
            SafeClick(searchDispositionOrderFileNumberBttn);

            WaitUntilClickable(searchDispositionFile1stResultLink);
            SafeClick(searchDispositionFile1stResultLink);

            Wait();
            var allWindowsHandle = webDriver.WindowHandles;
            var newWindowHandle = allWindowsHandle.Where(handle => handle != originalWindowHandle).First();
            webDriver.SwitchTo().Window(newWindowHandle);

            WaitUntilVisible(searchDispositionFileHeaderCode);
            AssertTrueIsDisplayed(searchDispositionFileHeaderCode);
        }

        public void FilterDispositionFiles(string pid = "", string pin = "", string address = "", string name = "", string teamMember = "",
            string status = "", string dispStatus = "", string type = "")
        {
            WaitUntilClickable(searchDispositionFileResetButton);
            webDriver.FindElement(searchDispositionFileResetButton).Click();

            if (pid != "")
            {
                ChooseSelectOption(searchDispositionFileSearchBySelect, "PID");
                webDriver.FindElement(searchDispositionFileSearchByPIDInput).SendKeys(pid);
            }

            if (pin != "")
            {
                ChooseSelectOption(searchDispositionFileSearchBySelect, "PIN");
                webDriver.FindElement(searchDispositionFileSearchByPINInput).SendKeys(pin);
            }

            if (address != "")
            {
                ChooseSelectOption(searchDispositionFileSearchBySelect, "Address");
                webDriver.FindElement(searchDispositionFileSearchByAddressInput).SendKeys(address);
            }

            if (name != "")
                webDriver.FindElement(searchDispositionFileNameInput).SendKeys(name);

            if (teamMember != "")
            {
                webDriver.FindElement(searchDispositionFileTeamMemberSelect).SendKeys(teamMember);
                WaitUntilVisible(searchDispositionTeamMemberOptions);
                FocusAndClick(searchDispositionTeamMember1stOption);
            }

            if(status != "")
                ChooseSelectOption(searchDispositionFileStatusSelect, status);

            if (dispStatus != "")
                ChooseSelectOption(searchDispositionStatusSelect, dispStatus);

            if (type != "")
                ChooseSelectOption(searchDispositionTypeSelect, type);

            webDriver.FindElement(searchDispositionFileSearchButton).Click();
        }

        public Boolean SearchFoundResults()
        {
            WaitForTableToLoad();
            return webDriver.FindElements(searchDispositionFile1stResult).Count > 0;
        }

        public string FirstDispositionFileNumber()
        {
            WaitForTableToLoad();
            return webDriver.FindElement(searchDispositionFile1stResultLink).Text;
        }

        public string FirstDispositionReferenceNumber()
        {
            WaitForTableToLoad();
            return webDriver.FindElement(searchDispositionFile1stResultReference).Text;
        }

        public string FirstDispositionFileName()
        {
            WaitForTableToLoad();
            return webDriver.FindElement(searchDispositionFile1stResultName).Text;
        }

        public string FirstDispositionFileType()
        {
            WaitForTableToLoad();
            return webDriver.FindElement(searchDispositionFile1stResultType).Text;
        }

        public string FirstDispositionStatus()
        {
            WaitForTableToLoad();
            return webDriver.FindElement(searchDispositionFile1stResultStatus).Text;
        }

        public string FirstDispositionFileStatus()
        {
            WaitForTableToLoad();
            return webDriver.FindElement(searchDispositionFile1stResultStatusFile).Text;
        }

        public int DispositionFileTableResultNumber()
        {
            WaitForTableToLoad();
            return webDriver.FindElements(searchDispositionFileTableContent).Count;
        }

        public void VerifyDispositionFileListView()
        {
            WaitUntilVisible(searchAcquisitionFileTitle);

            //Disposition File Title
            AssertTrueIsDisplayed(searchAcquisitionFileTitle);

            //Disposition File Search Filters
            AssertTrueIsDisplayed(searchDispositionFileSearchBySelect);
            AssertTrueIsDisplayed(searchDispositionFileSearchByAddressInput);
            AssertTrueIsDisplayed(searchDispositionFileNameInput);
            AssertTrueIsDisplayed(searchDispositionFileTeamMemberSelect);
            AssertTrueIsDisplayed(searchDispositionFileStatusSelect);
            AssertTrueIsDisplayed(searchDispositionStatusSelect);
            AssertTrueIsDisplayed(searchDispositionTypeSelect);

            AssertTrueIsDisplayed(searchDispositionFileSearchButton);
            AssertTrueIsDisplayed(searchDispositionFileCreateNewButton);

            //Disposition Files List View
            AssertTrueIsDisplayed(searchDispositionFileNumberHeader);
            AssertTrueIsDisplayed(searchDispositionOrderFileNumberBttn);
            AssertTrueIsDisplayed(searchDispositionReferenceHeader);
            AssertTrueIsDisplayed(searchDispositionOrderReferenceBttn);
            AssertTrueIsDisplayed(searchDispositionFileNameHeader);
            AssertTrueIsDisplayed(searchDispositionOrderFileNameBttn);
            AssertTrueIsDisplayed(searchDispositionFileTypeHeader);
            AssertTrueIsDisplayed(searchDispositionFileOrderTypeBttn);
            AssertTrueIsDisplayed(searchDispositionMOTIRegionHeader);
            AssertTrueIsDisplayed(searchDispositionTeamMemberHeader);
            AssertTrueIsDisplayed(searchDispositionAddressHeader);
            AssertTrueIsDisplayed(searchDispositionDispositionStatusHeader);
            AssertTrueIsDisplayed(searchDispositionFileOrderDispositionStatusBttn);
            AssertTrueIsDisplayed(searchDispositionFileStatusHeader);
            AssertTrueIsDisplayed(searchDispositionFileOrderStatusBttn);
            AssertTrueIsDisplayed(searchDispositionFileTableContent);

            //Disposition File Pagination
            AssertTrueIsDisplayed(searchDispositionFilePaginationMenu);
            AssertTrueIsDisplayed(searchDispositionPaginationList);
        }

        public void VerifyDispositionFileTableContent(DispositionFile disposition)
        {
            AssertTrueIsDisplayed(searchDispositionFile1stResult);

            if(disposition.DispositionReferenceNumber != "")
                AssertTrueContentEquals(searchDispositionFile1stResultReference, disposition.DispositionReferenceNumber!);

            AssertTrueContentEquals(searchDispositionFile1stResultName, disposition.DispositionFileName!);
            AssertTrueContentEquals(searchDispositionFile1stResultType, disposition.DispositionType!);
            AssertTrueContentEquals(searchDispositionFile1stResultMOTIRegion, disposition.DispositionMOTIRegion);

            if(disposition.DispositionTeam.Count > 0)
                Assert.NotEmpty(webDriver.FindElements(searchDispositionFile1stResultTeamMember));

            if (disposition.DispositionSearchPropertiesIndex != 0)
                Assert.NotEmpty(webDriver.FindElements(searchDispositionFile1stResultAddress));

            AssertTrueContentEquals(searchDispositionFile1stResultStatus, disposition.DispositionStatus!);
            AssertTrueContentEquals(searchDispositionFile1stResultStatusFile, disposition.DispositionFileStatus!);
        }
    }
}
