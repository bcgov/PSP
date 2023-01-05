

using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class DigitalDocuments : PageObjectBase
    {
        //Documents List View Elements
        //Documents List Header
        private By documentsTitle = By.XPath("//h2/div/div/div/div[contains(text(),'Documents')]");
        private By addDocumentBttn = By.XPath("//h2/div/div/div/div[contains(text(),'Documents')]/following-sibling::div/button");

        //Documents List Filters
        private By documentFilterTypeSelect = By.XPath("//div[@data-testid='activity-tray']/div[2]/div/div[2]/div[3]/div/form/div/div[2]/div/div[1]/div/select[@data-testid='document-type']");
        private By documentFilterStatusSelect = By.XPath("//div[@data-testid='activity-tray']/div[2]/div/div[2]/div[3]/div/form/div/div[2]/div/div[2]/div/select[@data-testid='document-status']");
        private By documentFilterNameInput = By.XPath("//div[@data-testid='activity-tray']/div[2]/div/div[2]/div[3]/div/form/div/div[2]/div/div[3]/div/input[@data-testid='document-filename']");
        private By documentFilterSearchBttn = By.XPath("//div[@data-testid='activity-tray']/div[2]/div/div[2]/div[3]/div/form/div/div[3]/div/div[1]/button[@data-testid='search']");
        private By documentFilterResetBttn = By.XPath("//div[@data-testid='activity-tray']/div[2]/div/div[2]/div[3]/div/form/div/div[3]/div/div[2]/button[@data-testid='reset-button']");

        //Documents List Results
        private By documentTableResults = By.XPath("//div[@data-testid='activity-tray']/div[2]/div/div[2]/div[3]/div/div[@data-testid='documentsTable']");
        private By documentTableTypeColumn = By.XPath("//div[@data-testid='activity-tray']/div[2]/div/div[2]/div[3]/div/div[@data-testid='documentsTable']/div/div/div/div[contains(text(),'Document type')]");
        private By documentTableNameColumn = By.XPath("//div[@data-testid='activity-tray']/div[2]/div/div[2]/div[3]/div/div[@data-testid='documentsTable']/div/div/div/div[contains(text(),'File name')]");
        private By documentTableDateColumn = By.XPath("//div[@data-testid='activity-tray']/div[2]/div/div[2]/div[3]/div/div[@data-testid='documentsTable']/div/div/div/div[contains(text(),'Uploaded')]");
        private By documentTableStatusColumn = By.XPath("//div[@data-testid='activity-tray']/div[2]/div/div[2]/div[3]/div/div[@data-testid='documentsTable']/div/div/div/div[contains(text(),'Status')]");
        private By documentTableActionsColumn = By.XPath("//div[@data-testid='activity-tray']/div[2]/div/div[2]/div[3]/div/div[@data-testid='documentsTable']/div/div/div/div[contains(text(),'Actions')]");

        //Documents Pagination
        private By documentPagination = By.XPath("(//ul[@class='pagination'])[2]");
        private By documentMenuPagination = By.XPath("div[class='Menu-button']");


        public DigitalDocuments(IWebDriver webDriver) : base(webDriver)
        {}

        public void VerifyDocumentsListView()
        {
            Wait();
            Assert.True(webDriver.FindElement(documentsTitle).Displayed);
            Assert.True(webDriver.FindElement(addDocumentBttn).Displayed);

            Assert.True(webDriver.FindElement(documentFilterTypeSelect).Displayed);
            Assert.True(webDriver.FindElement(documentFilterStatusSelect).Displayed);
            Assert.True(webDriver.FindElement(documentFilterNameInput).Displayed);
            Assert.True(webDriver.FindElement(documentFilterSearchBttn).Displayed);
            Assert.True(webDriver.FindElement(documentFilterResetBttn).Displayed);

            Assert.True(webDriver.FindElement(documentTableResults).Displayed);
            Assert.True(webDriver.FindElement(documentTableTypeColumn).Displayed);
            Assert.True(webDriver.FindElement(documentTableNameColumn).Displayed);
            Assert.True(webDriver.FindElement(documentTableDateColumn).Displayed);
            Assert.True(webDriver.FindElement(documentTableStatusColumn).Displayed);
            Assert.True(webDriver.FindElement(documentTableActionsColumn).Displayed);
        }
    }
}
