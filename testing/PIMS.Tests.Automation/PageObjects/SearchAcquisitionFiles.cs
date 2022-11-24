

using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SearchAcquisitionFiles : PageObjectBase
    {
        private By menuAcquisitionButton = By.XPath("//a/label[contains(text(),'Acquisition')]/parent::a");
        private By searchAcquisitionButton = By.XPath("//a[contains(text(),'Manage Acquisition Files')]");

        private By searchAcquisitionFileInput = By.Id("input-acquisitionFileNameOrNumber");
        private By searchAcquisitionFileButton = By.Id("search-button");

        private By searchAcquisitionFileSortByAFileBttn = By.CssSelector("div[data-testid='sort-column-fileNumber']");

        private By searchAcquisitionFile1stResult = By.CssSelector("div[data-testid='acquisitionFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1)");
        private By searchAcquisitionFile1stResultLink = By.CssSelector("div[data-testid='acquisitionFilesTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div[class='td clickable'] a");

        private By searchAcquisitionFileHeaderCode = By.XPath("//label[contains(text(), 'File #:')]/parent::div/following-sibling::div[1]/strong");
       


        public SearchAcquisitionFiles(IWebDriver webDriver) : base(webDriver)
        {}

        //Navigates to Search an Acquisition File
        public void NavigateToSearchAcquisitionFile()
        {
            Wait();
            webDriver.FindElement(menuAcquisitionButton).Click();

            Wait();
            webDriver.FindElement(searchAcquisitionButton).Click();
        }

        public void SearchAcquisitionFileByAFile(string AFile)
        {
            Wait();
            
            webDriver.FindElement(searchAcquisitionFileInput).SendKeys(AFile);
            ChooseSpecificSelectOption("input-acquisitionFileStatusTypeCode", "All Status");

            Wait(5000);
            webDriver.FindElement(searchAcquisitionFileButton).Click();
        }

        public void SearchLastAcquisitionFile()
        {
            Wait();
            ChooseSpecificSelectOption("input-acquisitionFileStatusTypeCode", "All Status");
            webDriver.FindElement(searchAcquisitionFileButton).Click();

            Wait(1000);
            webDriver.FindElement(searchAcquisitionFileSortByAFileBttn).Click();

            Wait();
            webDriver.FindElement(searchAcquisitionFileSortByAFileBttn).Click();


            Wait();
            webDriver.FindElement(searchAcquisitionFile1stResultLink).Click();

            Wait();
            Assert.True(webDriver.FindElement(searchAcquisitionFileHeaderCode).Displayed);

        }

        public Boolean SearchFoundResults()
        {
            Wait();
            return webDriver.FindElements(searchAcquisitionFile1stResult).Count > 0;
        }
    }
}
