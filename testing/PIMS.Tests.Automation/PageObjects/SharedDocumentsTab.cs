using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SharedDocumentsTab : PageObjectBase
    {
        private By documentsTabLink = By.XPath("//nav[@role='tablist']/a[contains(text(),'Documents')]");
        private By documentsTabDocsFileSubtitle = By.XPath("//div[@class='tab-content']/div[@role='tabpanel']/div/h2/div/div/div/div[contains(text(),'File Documents')]");
        private By documentTabDocsAddNewDocBttn = By.XPath("//div[@class='tab-content']/div[@role='tabpanel']/div/h2/div/div/div/div[contains(text(),'File Documents')]/following-sibling::div/button");



        public SharedDocumentsTab(IWebDriver webDriver) : base(webDriver)
        {}
    }
}
