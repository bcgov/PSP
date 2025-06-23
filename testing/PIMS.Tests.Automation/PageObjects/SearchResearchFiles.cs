using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SearchResearchFiles : PageObjectBase
    {
        private readonly By menuResearchButton = By.CssSelector("div[data-testid='nav-tooltip-research'] a");
        private readonly By searchResearchButton = By.XPath("//a[contains(text(),'Manage Research File')]");

        //Search Research File Elements
        private readonly By searchResearchRegionInput = By.Id("input-regionCode");
        private readonly By searchResearchBySelect = By.Id("input-researchSearchBy");
        private readonly By searchResearchByPID = By.Id("input-pid");
        private readonly By searchResearchByPIN = By.Id("input-pin");
        private readonly By searchResearchNameInput = By.Id("input-name");
        private readonly By searchResearchFileNbrInput = By.Id("input-rfileNumber");
        private readonly By searchResearchStatusSelect = By.Id("input-researchFileStatusTypeCode");
        private readonly By searchResearchRoadInput = By.Id("input-roadOrAlias");
        private readonly By searchResearchCreateUpdateDateSelect = By.Id("input-createOrUpdateRange");
        private readonly By searchResearchCreateDateInput = By.Id("datepicker-createdOnStartDate");
        private readonly By searchResearchUpdateDateInput = By.Id("datepicker-updatedOnStartDate");
        private readonly By searchResearchToDateInput = By.Id("datepicker-updatedOnEndDate");
        private readonly By searchResearchCreateUpdateBySelect = By.Id("input-createOrUpdateBy");
        private readonly By searchResearchUserUpdatedIdirInput = By.Id("input-appLastUpdateUserid");
        private readonly By searchResearchFileUserCreatedIdirInput = By.Id("input-appCreateUserid");
        private readonly By searchResearchFileButton = By.Id("search-button");
        private readonly By searchResearchFileResetButton = By.Id("reset-button");
        private readonly By searchResearchCreateNewBttn = By.XPath("//div[contains(text(),'Create a Research File')]/parent::button");

        //Search Research File List Elements
        private readonly By searchResearchFileNbrLabel = By.XPath("//div[contains(text(),'File #')]");
        private readonly By searchResearchFileNameLabel = By.XPath("//div[contains(text(),'File name')]");
        private readonly By searchResearchFileRegionLabel = By.XPath("//div[contains(text(),'MOTI Region')]");
        private readonly By searchResearchFileCreatedByLabel = By.XPath("//div[contains(text(),'Created by')]");
        private readonly By searchResearchFileCreatedDateLabel = By.XPath("//div[contains(text(),'Created date')]");
        private readonly By searchResearchFileUpdatedByLabel = By.XPath("//div[contains(text(),'Last updated by')]");
        private readonly By searchResearchFileUpdatedDateLabel = By.XPath("//div[contains(text(),'Last updated date')]");
        private readonly By searchResearchFileStatusLabel = By.XPath("//div[contains(text(),'Status')]");
        private readonly By searchResearchFileSortByRFileBttn = By.CssSelector("div[data-testid='sort-column-rfileNumber']");

        //Search Research Files 1st Result Elements
        private readonly By searchResearchFile1stResult = By.CssSelector("div[data-testid='researchFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1)");
        private readonly By searchResearchFile1stResultLink = By.CssSelector("div[data-testid='researchFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable'] a");
        private readonly By searchResearchFileOrderFileNumberBttn = By.CssSelector("div[data-testid='sort-column-rfileNumber']");
        private readonly By searchResearchFile1stResultFileName = By.CssSelector("div[data-testid='researchFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable']:nth-child(2)");
        private readonly By searchResearchFileOrderFileNameBttn = By.CssSelector("div[data-testid='sort-column-name']");
        private readonly By searchResearchFile1stResultRegion = By.CssSelector("div[data-testid='researchFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable']:nth-child(3)");
        private readonly By searchResearchFile1stResultCreator = By.CssSelector("div[data-testid='researchFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable']:nth-child(4)");
        private readonly By searchResearchFileOrderCreatorNameBttn = By.CssSelector("div[data-testid='sort-column-appCreateUserid']");
        private readonly By searchResearchFile1stResultCreateDate = By.CssSelector("div[data-testid='researchFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable']:nth-child(5)");
        private readonly By searchResearchFileOrderCreateDateBttn = By.CssSelector("div[data-testid='sort-column-appCreateTimestamp']");
        private readonly By searchResearchFile1stResultUpdatedBy = By.CssSelector("div[data-testid='researchFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable']:nth-child(6)");
        private readonly By searchResearchFileOrderLastUpdatedByBttn = By.CssSelector("div[data-testid='sort-column-appLastUpdateUserid']");
        private readonly By searchResearchFile1stResultUpdateDate = By.CssSelector("div[data-testid='researchFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable']:nth-child(7)");
        private readonly By searchResearchFileOrderUpdatedDateBttn = By.CssSelector("div[data-testid='sort-column-appLastUpdateTimestamp']");
        private readonly By searchResearchFile1stResultStatus = By.CssSelector("div[data-testid='researchFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable']:nth-child(8)");
        private readonly By searchResearchFileStatusNameBttn = By.CssSelector("div[data-testid='sort-column-researchFileStatusTypeCode']");

        private readonly By searchResearchFileTableContent = By.CssSelector("div[data-testid='researchFilesTable'] div[class='tbody'] div[class='tr-wrapper']");

        //Search Research File Pagination
        private readonly By searchResearchFilePaginationMenu = By.CssSelector("div[class='Menu-root']");
        private readonly By searchResearchPaginationList = By.CssSelector("ul[class='pagination']");

        private readonly By researchFileHeaderCode = By.XPath("//strong[contains(text(),'R-')]");

        public SearchResearchFiles(IWebDriver webDriver) : base(webDriver)
        {}

        //Navigates to Search a Research File
        public void NavigateToSearchResearchFile()
        {
            Wait(4000);
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
            FocusAndClick(searchResearchFileButton);
        }

        public void SearchAllResearchFiles()
        {
            Wait(2000);
            ChooseSpecificSelectOption(searchResearchStatusSelect, "All Status");

            WaitUntilClickable(searchResearchFileButton);
            webDriver.FindElement(searchResearchFileButton).Click();
        }

        public void OrderByResearchFileNumber()
        {
            WaitUntilClickable(searchResearchFileOrderFileNumberBttn);
            webDriver.FindElement(searchResearchFileOrderFileNumberBttn).Click();
        }

        public void OrderByResearchFileName()
        {
            WaitUntilClickable(searchResearchFileOrderFileNameBttn);
            webDriver.FindElement(searchResearchFileOrderFileNameBttn).Click();
        }

        public void OrderByResearchFileCreatedBy()
        {
            WaitUntilClickable(searchResearchFileOrderCreatorNameBttn);
            webDriver.FindElement(searchResearchFileOrderCreatorNameBttn).Click();
        }

        public void OrderByResearchCreatedDate()
        {
            WaitUntilClickable(searchResearchFileOrderCreateDateBttn);
            webDriver.FindElement(searchResearchFileOrderCreateDateBttn).Click();
        }

        public void OrderByResearchLastUpdatedBy()
        {
            WaitUntilClickable(searchResearchFileOrderLastUpdatedByBttn);
            webDriver.FindElement(searchResearchFileOrderLastUpdatedByBttn).Click();
        }

        public void OrderByResearchUpdatedDate()
        {
            WaitUntilClickable(searchResearchFileOrderUpdatedDateBttn);
            webDriver.FindElement(searchResearchFileOrderUpdatedDateBttn).Click();
        }

        public void OrderByResearchStatus()
        {
            WaitUntilClickable(searchResearchFileStatusNameBttn);
            webDriver.FindElement(searchResearchFileStatusNameBttn).Click();
        }

        public string FirstResearchFileNumber()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(searchResearchFile1stResultLink).Text;
        }

        public string FirstResearchFileName()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(searchResearchFile1stResultFileName).Text;
        }

        public string FirstResearchCreatedBy()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(searchResearchFile1stResultCreator).Text;
        }

        public string FirstResearchCreatedDate()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(searchResearchFile1stResultCreateDate).Text;
        }

        public string FirstResearchUpdatedBy()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(searchResearchFile1stResultUpdatedBy).Text;
        }

        public string FirstResearchUpdatedDate()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(searchResearchFile1stResultUpdateDate).Text;
        }

        public string FirstResearchFileStatus()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElement(searchResearchFile1stResultStatus).Text;
        }

        public void VerifyResearchFileListView()
        {
            WaitUntilVisible(searchResearchRegionInput);

            //Search Bar Elements
            AssertTrueIsDisplayed(searchResearchRegionInput);
            AssertTrueIsDisplayed(searchResearchBySelect);

            if (webDriver.FindElements(searchResearchFileNbrInput).Count > 0)
                AssertTrueIsDisplayed(searchResearchFileNbrInput);
            else
                AssertTrueIsDisplayed(searchResearchNameInput);

            AssertTrueIsDisplayed(searchResearchStatusSelect);
            AssertTrueIsDisplayed(searchResearchRoadInput);
            AssertTrueIsDisplayed(searchResearchCreateUpdateDateSelect);
            AssertTrueIsDisplayed(searchResearchToDateInput);
            AssertTrueIsDisplayed(searchResearchCreateUpdateBySelect);
            AssertTrueIsDisplayed(searchResearchUserUpdatedIdirInput);
            AssertTrueIsDisplayed(searchResearchFileButton);
            AssertTrueIsDisplayed(searchResearchFileResetButton);
            AssertTrueIsDisplayed(searchResearchCreateNewBttn);

            //Table Elements
            AssertTrueIsDisplayed(searchResearchFileNbrLabel);
            AssertTrueIsDisplayed(searchResearchFileNameLabel);
            AssertTrueIsDisplayed(searchResearchFileRegionLabel);
            AssertTrueIsDisplayed(searchResearchFileCreatedByLabel);
            AssertTrueIsDisplayed(searchResearchFileCreatedDateLabel);
            AssertTrueIsDisplayed(searchResearchFileUpdatedByLabel);
            AssertTrueIsDisplayed(searchResearchFileUpdatedDateLabel);
            AssertTrueIsDisplayed(searchResearchFileStatusLabel);
            AssertTrueIsDisplayed(searchResearchFileSortByRFileBttn);

            //Pagination Elements
            WaitUntilVisible(searchResearchFilePaginationMenu);
            AssertTrueIsDisplayed(searchResearchFilePaginationMenu);
            AssertTrueIsDisplayed(searchResearchPaginationList);
        }

        public void VerifyResearchFileTableContent(ResearchFile researchFile, string user)
        {
            Wait(2000);

            AssertTrueIsDisplayed(searchResearchFile1stResultLink);
            AssertTrueContentEquals(searchResearchFile1stResultFileName, researchFile.ResearchFileName);
            AssertTrueContentEquals(searchResearchFile1stResultRegion, researchFile.ResearchFileMOTIRegion);
            AssertTrueContentEquals(searchResearchFile1stResultCreator, user);
            AssertTrueContentEquals(searchResearchFile1stResultCreateDate, GetTodayFormattedDate());
            AssertTrueContentEquals(searchResearchFile1stResultUpdatedBy, user);
            AssertTrueContentEquals(searchResearchFile1stResultUpdateDate, GetTodayFormattedDate());
            AssertTrueContentEquals(searchResearchFile1stResultStatus, researchFile.Status);
        }

        public void FilterResearchFiles(string region = "", string status  = "", string pid = "", string pin = "", string name = "", string fileNumber = "", string roadName = "",
            string createdDate = "", string updatedDate = "", string createdBy = "", string updatedBy = "")
        {
            WaitUntilClickable(searchResearchFileResetButton);
            webDriver.FindElement(searchResearchFileResetButton).Click();

            if(region != "")
                ChooseSpecificSelectOption(searchResearchRegionInput, region);

            if(status != "")
                ChooseSpecificSelectOption(searchResearchStatusSelect, status);

            if (pid != "")
            {
                ChooseSpecificSelectOption(searchResearchBySelect, "PID");
                webDriver.FindElement(searchResearchByPID).SendKeys(pid);
            }
            else if (pin != "")
            {
                ChooseSpecificSelectOption(searchResearchBySelect, "PIN");
                webDriver.FindElement(searchResearchByPIN).SendKeys(pin);
            }
            else if (name != "")
            {
                ChooseSpecificSelectOption(searchResearchBySelect, "Research file name");
                webDriver.FindElement(searchResearchNameInput).SendKeys(name);
            }
            else if (fileNumber != "")
            {
                ChooseSpecificSelectOption(searchResearchBySelect, "Research file #");
                webDriver.FindElement(searchResearchFileNbrInput).SendKeys(fileNumber);
            }
            if (roadName != "")
            {
                webDriver.FindElement(searchResearchRoadInput).SendKeys(roadName);
            }
            if (createdDate != "")
            {
                ChooseSpecificSelectOption(searchResearchCreateUpdateDateSelect, "Created date");
                webDriver.FindElement(searchResearchCreateDateInput).SendKeys(createdDate);
            }
            else if (updatedDate != "")
            {
                ChooseSpecificSelectOption(searchResearchCreateUpdateDateSelect, "Updated date");
                webDriver.FindElement(searchResearchUpdateDateInput).SendKeys(updatedDate);
            }

            if (createdBy != "")
            {
                ChooseSpecificSelectOption(searchResearchCreateUpdateBySelect, "Created by");
                webDriver.FindElement(searchResearchFileUserCreatedIdirInput).SendKeys(createdBy);
            }
            else if (updatedBy != "")
            {
                ChooseSpecificSelectOption(searchResearchCreateUpdateBySelect, "Updated by");
                webDriver.FindElement(searchResearchUserUpdatedIdirInput).SendKeys(createdBy);
            }

            webDriver.FindElement(searchResearchFileButton).Click();
        }

        public Boolean SearchFoundResults()
        {
            Wait(2000);
            return webDriver.FindElements(searchResearchFile1stResult).Count > 0;
        }

        public int ResearchFileTableResultNumber()
        {
            WaitUntilTableSpinnerDisappear();
            return webDriver.FindElements(searchResearchFileTableContent).Count;
        }
    }
}
