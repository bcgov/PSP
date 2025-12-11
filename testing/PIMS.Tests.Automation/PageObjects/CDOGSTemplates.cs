using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class CDOGSTemplates : PageObjectBase
    {
        //Admin Tools - CDOGS Menu Elements
        private readonly By adminToolsTemplatesLink = By.XPath("//a[contains(text(),'Manage Form Document Templates')]");
        private readonly By adminToolsTemplateTypeSelect = By.CssSelector("select[class='form-select form-control']");

        //Admin Tools - CDOGS List View Elements
        private readonly By CDOGSDocumentsTitle = By.XPath("//div[contains(text(),'Documents')]");
        private readonly By CDOGSAddTemplateBttn = By.XPath("//button[@data-testid='refresh-button']/preceding-sibling::button");

        //CDOGS List Filters
        private readonly By CDOGSFilterTypeSelect = By.CssSelector("select[data-testid='document-type']");
        private readonly By CDOGSFilterStatusSelect = By.CssSelector("select[data-testid='document-status']");
        private readonly By CDOGSFilterNameInput = By.CssSelector("input[data-testid='document-filename']");
        private readonly By CDOGSFilterSearchBttn = By.CssSelector("button[data-testid='search']");
        private readonly By CDOGSFilterResetBttn = By.CssSelector("button[data-testid='reset-button']");

        //Documents List Results
        private readonly By CDOGSTableResults = By.CssSelector("div[data-testid='documentsTable']");
        private readonly By CDOGSTableTypeColumn = By.XPath("//div[contains(text(),'Document type')]");
        private readonly By CDOGSTableNameColumn = By.XPath("//div[contains(text(),'Document name')]");
        private readonly By CDOGSTableDateColumn = By.XPath("//div[contains(text(),'Uploaded')]");
        private readonly By CDOGSTableStatusColumn = By.XPath("//div[contains(text(),'Status')]");
        private readonly By CDOGSTableActionsColumn = By.XPath("//div[contains(text(),'Actions')]");

        private readonly By CDOGSPagination = By.XPath("//ul[@class='pagination']");
        private readonly By CDOGSMenuPagination = By.XPath("//div[@class='Menu-root']");

        //Documents List 1st Result Elements
        private readonly By CDOGStTableResults1stDownloadBttn = By.XPath("//div[@data-testid='documentsTable']/div[@class='tbody']/div[1]/div/div[5]/div/div/div/button[@data-testid='document-download-button']");
        private readonly By CDOGSTableResults1stViewBttn = By.XPath("//div[@data-testid='documentsTable']/div[@class='tbody']/div[1]/div/div[5]/div/div/button[@data-testid='document-view-button']");
        private readonly By CDOGSTableResults1stDeleteBttn = By.Id("document-delete-0");

        //Document Delete Document Confirmation Modal Elements
        private readonly By documentDeleteHeader = By.CssSelector("div[class='modal-header'] div[class='modal-title h4']");
        private readonly By documentDeleteContent1 = By.CssSelector("div[class='modal-body'] div div:nth-child(1)");
        private readonly By documentDeteleContent2 = By.CssSelector("div[class='modal-body'] div:nth-child(3)");
        private readonly By documentDeleteContent3 = By.CssSelector("div[class='modal-body'] div strong");
        private readonly By documentDeleteOkBttn = By.CssSelector("button[title='ok-modal']");

        public CDOGSTemplates(IWebDriver webDriver) : base(webDriver)
        {}

        public void NavigateAdminTemplates()
        {
            Wait();

            WaitUntilClickable(adminToolsTemplatesLink);
            webDriver.FindElement(adminToolsTemplatesLink).Click();
        }

        public void SelectTemplateType(string type)
        {
            WaitUntilClickable(adminToolsTemplateTypeSelect);
            ChooseSpecificSelectOption(adminToolsTemplateTypeSelect, type);
        }

        public void AddNewTemplate()
        {
            WaitUntilClickable(CDOGSAddTemplateBttn);
            FocusAndClick(CDOGSAddTemplateBttn);
        }

        public void VerifyCDOGSListView()
        {
            WaitUntilVisible(CDOGSAddTemplateBttn);
            AssertTrueIsDisplayed(CDOGSDocumentsTitle);
            AssertTrueIsDisplayed(CDOGSAddTemplateBttn);

            AssertTrueIsDisplayed(CDOGSFilterTypeSelect);
            AssertTrueIsDisplayed(CDOGSFilterStatusSelect);
            AssertTrueIsDisplayed(CDOGSFilterNameInput);
            AssertTrueIsDisplayed(CDOGSFilterSearchBttn);
            AssertTrueIsDisplayed(CDOGSFilterResetBttn);

            AssertTrueIsDisplayed(CDOGSTableResults);
            AssertTrueIsDisplayed(CDOGSTableTypeColumn);
            AssertTrueIsDisplayed(CDOGSTableNameColumn);
            AssertTrueIsDisplayed(CDOGSTableDateColumn);
            AssertTrueIsDisplayed(CDOGSTableStatusColumn);
            AssertTrueIsDisplayed(CDOGSTableActionsColumn);
        }

        public void Delete1stTemplate()
        {
            Wait();
            webDriver.FindElement(CDOGSTableResults1stDeleteBttn).Click();
            
            WaitUntilVisible(documentDeleteHeader);
            Assert.Equal("Delete a document", webDriver.FindElement(documentDeleteHeader).Text);
            Assert.Equal("You have chosen to delete this document.", webDriver.FindElement(documentDeleteContent1).Text);
            Assert.Equal("If the document is linked to other files or entities in PIMS it will still be accessible from there, however if this the only instance then the file will be removed from the document store completely.", webDriver.FindElement(documentDeteleContent2).Text);
            Assert.Equal("Do you wish to continue deleting this document?", webDriver.FindElement(documentDeleteContent3).Text);

            webDriver.FindElement(documentDeleteOkBttn).Click();
        }

        public void VerifyTemplateExistence()
        {
            Wait();
            if (webDriver.FindElements(CDOGSTableResults1stDeleteBttn).Count == 1)
                Delete1stTemplate();   
        }
    }
}
