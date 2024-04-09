
using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class PropertyPIMSFiles : PageObjectBase
    {
        //PIMS Files Elements
        private By propertyPimsFilesLinkTab = By.XPath("//a[contains(text(),'PIMS Files')]");
        private By propertyPimsFiles = By.XPath("//div[contains(text(),'This property is associated with the following files.')]");

        private By propertyResearchFileSubtitle = By.XPath("//div[contains(text(),'Research Files')]");
        private By propertyResearchCountLabel = By.XPath("//div[contains(text(),'Research Files')]/following-sibling::div[@class='my-1 col-auto']/div");
        private By propertyResearchTable = By.XPath("//div[contains(text(),'Research Files')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div[@data-testid='associationFiles']");
        private By propertyResearchExpandTableBttn = By.XPath("//div[contains(text(),'Research Files')]/parent::div/parent::div/following-sibling::div/*[1]");

        private By propertyAcquisitionFileSubtitle = By.XPath("//div[contains(text(),'Acquisition Files')]");
        private By propertyAcquisitionCountLabel = By.XPath("//div[contains(text(),'Acquisition Files')]/following-sibling::div[@class='my-1 col-auto']/div");
        private By propertyAcquisitionTable = By.XPath("//div[contains(text(),'Acquisition Files')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div[@data-testid='associationFiles']");
        private By propertyAcquisitionExpandTableBttn = By.XPath("//div[contains(text(),'Acquisition Files')]/parent::div/parent::div/following-sibling::div/*[1]");

        private By propertyLeasesSubtitle = By.XPath("//div[contains(text(),'Leases/Licenses')]");
        private By propertyLeaseCountLabel = By.XPath("//div[contains(text(),'Leases/Licenses')]/following-sibling::div[@class='my-1 col-auto']/div");
        private By propertyLeaseTable = By.XPath("//div[contains(text(),'Leases/Licenses')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div[@data-testid='associationFiles']");
        private By propertyLeaseExpandTableBttn = By.XPath("//div[contains(text(),'Leases/Licenses')]/parent::div/parent::div/following-sibling::div/*[1]");

        private By propertyDispositionFileSubtitle = By.XPath("//div[contains(text(),'Disposition Files')]");
        private By propertyDispositionCountLabel = By.XPath("//div[contains(text(),'Disposition Files')]/following-sibling::div[@class='my-1 col-auto']/div");
        private By propertyDispositionTable = By.XPath("//div[contains(text(),'Disposition Files')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div[@data-testid='associationFiles']");
        private By propertyDispositionExpandTableBttn = By.XPath("//div[contains(text(),'Disposition Files')]/parent::div/parent::div/following-sibling::div/*[1]");

        public PropertyPIMSFiles(IWebDriver webDriver) : base(webDriver)
        {}

        public void NavigatePIMSFiles()
        {
            WaitUntilSpinnerDisappear();
            webDriver.FindElement(propertyPimsFilesLinkTab).Click();
        }

        public void VerifyPimsFiles()
        {
            Wait();

            WaitUntilClickable(propertyPimsFilesLinkTab);
            webDriver.FindElement(propertyPimsFilesLinkTab).Click();

            AssertTrueIsDisplayed(propertyPimsFiles);

            //Research Files
            AssertTrueIsDisplayed(propertyResearchFileSubtitle);
            AssertTrueIsDisplayed(propertyResearchCountLabel);

            WaitUntilClickable(propertyResearchExpandTableBttn);
            webDriver.FindElement(propertyResearchExpandTableBttn).Click();

            AssertTrueIsDisplayed(propertyResearchTable);

            //Acquisition Files
            AssertTrueIsDisplayed(propertyAcquisitionFileSubtitle);
            AssertTrueIsDisplayed(propertyAcquisitionCountLabel);

            Wait();
            webDriver.FindElement(propertyAcquisitionExpandTableBttn).Click();

            AssertTrueIsDisplayed(propertyAcquisitionTable);

            //Leases
            AssertTrueIsDisplayed(propertyLeasesSubtitle);
            AssertTrueIsDisplayed(propertyLeaseCountLabel);

            Wait();
            webDriver.FindElement(propertyLeaseExpandTableBttn).Click();

            AssertTrueIsDisplayed(propertyLeaseTable);

            //Disposition Files
            AssertTrueIsDisplayed(propertyDispositionFileSubtitle);
            AssertTrueIsDisplayed(propertyDispositionCountLabel);

            WaitUntilVisible(propertyDispositionExpandTableBttn);
            webDriver.FindElement(propertyDispositionExpandTableBttn).Click();

            AssertTrueIsDisplayed(propertyDispositionTable);
        }

        public int GetResearchFilesCount()
        {
            WaitUntilVisible(propertyResearchCountLabel);
            return int.Parse(webDriver.FindElement(propertyResearchCountLabel).Text);
        }

        public int GetAcquisitionFilesCount()
        {
            WaitUntilVisible(propertyAcquisitionCountLabel);
            return int.Parse(webDriver.FindElement(propertyAcquisitionCountLabel).Text);
        }

        public int GetLeasesCount()
        {
            WaitUntilVisible(propertyLeaseCountLabel);
            return int.Parse(webDriver.FindElement(propertyLeaseCountLabel).Text);
        }

        public int GetDispositionFilesCount()
        {
            WaitUntilVisible(propertyDispositionCountLabel);
            return int.Parse(webDriver.FindElement(propertyDispositionCountLabel).Text);
        }




    }
}
