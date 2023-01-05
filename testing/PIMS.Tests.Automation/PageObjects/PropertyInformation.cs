using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class PropertyInformation : PageObjectBase
    {
        private By propertyCloseWindowBttn = By.XPath("//h1[contains(text(),'Property Information')]/parent::div/following-sibling::div");
        private By propertyFlyOutEllipsis = By.CssSelector("button[data-testid='fly-out-ellipsis']");
        private By propertyMoreOptionsMenu = By.CssSelector("div[class='list-group list-group-flush']");

        //PIMS Files Elements
        private By propertyPimsFilesLinkTab = By.XPath("//a[contains(text(),'PIMS Files')]");
        private By propertyPimsFiles = By.XPath("//div[contains(text(),'This property is associated with the following files.')]");

        private By propertyResearchFileSubtitle = By.XPath("//div[contains(text(),'Research Files')]");
        private By propertyResearchCountLabel = By.XPath("//div[contains(text(),'Research Files')]/following-sibling::div[@class='my-1 col-auto']");
        private By propertyResearchTable = By.XPath("//div[contains(text(),'Research Files')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div[@data-testid='associationFiles']");
        private By propertyResearchExpandTableBttn = By.XPath("//div[contains(text(),'Research Files')]/parent::div/parent::div/following-sibling::div");

        private By propertyAcquisitionFileSubtitle = By.XPath("//div[contains(text(),'Acquisition Files')]");
        private By propertyAcquisitionCountLabel = By.XPath("//div[contains(text(),'Acquisition Files')]/following-sibling::div[@class='my-1 col-auto']");
        private By propertyAcquisitionTable = By.XPath("//div[contains(text(),'Acquisition Files')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div[@data-testid='associationFiles']");
        private By propertyAcquisitionExpandTableBttn = By.XPath("//div[contains(text(),'Acquisition Files')]/parent::div/parent::div/following-sibling::div");

        private By propertyLeasesSubtitle = By.XPath("//div[contains(text(),'Leases/Licenses')]");
        private By propertyLeaseCountLabel = By.XPath("//div[contains(text(),'Leases/Licenses')]/following-sibling::div[@class='my-1 col-auto']");
        private By propertyLeaseTable = By.XPath("//div[contains(text(),'Leases/Licenses')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div[@data-testid='associationFiles']");
        private By propertyLeaseExpandTableBttn = By.XPath("//div[contains(text(),'Leases/Licenses')]/parent::div/parent::div/following-sibling::div");

        private By propertyDispositionFileSubtitle = By.XPath("//div[contains(text(),'Disposition Files')]");
        private By propertyDispositionCountLabel = By.XPath("//div[contains(text(),'Disposition Files')]/following-sibling::div[@class='my-1 col-auto']");



        public PropertyInformation(IWebDriver webDriver) : base(webDriver)
        {}

        public void ClosePropertyInfoModal()
        {
            Wait();
            webDriver.FindElement(propertyCloseWindowBttn).Click();
        }

        public void ChooseCreationOptionFromPin(string option)
        {
            Wait();
            webDriver.FindElement(propertyFlyOutEllipsis).Click();

            WaitUntil(propertyMoreOptionsMenu);
            ButtonElement(option);
        }

        public void VerifyPimsFiles()
        {
            Wait();

            webDriver.FindElement(propertyPimsFilesLinkTab).Click();

            Wait();
            Assert.True(webDriver.FindElement(propertyPimsFiles).Displayed);
                
            Assert.True(webDriver.FindElement(propertyResearchFileSubtitle).Displayed);
            Assert.True(webDriver.FindElement(propertyResearchCountLabel).Displayed);
            webDriver.FindElement(propertyResearchExpandTableBttn).Click();

            Wait();
            Assert.True(webDriver.FindElement(propertyResearchTable).Displayed);

            Assert.True(webDriver.FindElement(propertyAcquisitionFileSubtitle).Displayed);
            Assert.True(webDriver.FindElement(propertyAcquisitionCountLabel).Displayed);
            webDriver.FindElement(propertyAcquisitionExpandTableBttn).Click();

            Wait();
            Assert.True(webDriver.FindElement(propertyAcquisitionTable).Displayed);

            Assert.True(webDriver.FindElement(propertyLeasesSubtitle).Displayed);
            Assert.True(webDriver.FindElement(propertyLeaseCountLabel).Displayed);
            webDriver.FindElement(propertyLeaseExpandTableBttn).Click();

            Wait();
            Assert.True(webDriver.FindElement(propertyLeaseTable).Displayed);

            Assert.True(webDriver.FindElement(propertyDispositionFileSubtitle).Displayed);
            Assert.True(webDriver.FindElement(propertyDispositionCountLabel).Displayed); 
        }
    }
}
