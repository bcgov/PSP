using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class CDOGSTemplates : PageObjectBase
    {
        //Admin Tools - CDOGS Menu Elements
        private By adminToolsTemplatesLink = By.XPath("//a[contains(text(),'Manage Form Document Templates')]");
        private By adminToolsTemplateTypeSelect = By.CssSelector("select[class='form-control']");

        //Admin Tools - CDOGS List View Elements
        private By CDOGSDocumentsTitle = By.XPath("//div[contains(text(),'Documents')]");
        private By CDOGSAddTemplateBttn = By.XPath("//div[contains(text(),'Documents')]/following-sibling::div/button");

        //CDOGS List Filters
        private By CDOGSFilterTypeSelect = By.CssSelector("select[data-testid='document-type']");
        private By CDOGSFilterStatusSelect = By.CssSelector("select[data-testid='document-status']");
        private By CDOGSFilterNameInput = By.CssSelector("input[data-testid='document-filename']");
        private By CDOGSFilterSearchBttn = By.CssSelector("button[data-testid='search']");
        private By CDOGSFilterResetBttn = By.CssSelector("button[data-testid='reset-button']");

        //Documents List Results
        private By CDOGSTableResults = By.CssSelector("div[data-testid='documentsTable']");
        private By CDOGSTableTypeColumn = By.XPath("//div[contains(text(),'Document type')]");
        private By CDOGSTableNameColumn = By.XPath("//div[contains(text(),'File name')]");
        private By CDOGSTableDateColumn = By.XPath("//div[contains(text(),'Uploaded')]");
        private By CDOGSTableStatusColumn = By.XPath("//div[contains(text(),'Status')]");
        private By CDOGSTableActionsColumn = By.XPath("//div[contains(text(),'Actions')]");

        private By CDOGSPagination = By.XPath("//ul[@class='pagination']");
        private By CDOGSMenuPagination = By.XPath("//div[@class='Menu-root']");

        //Documents List 1st Result Elements
        private By CDOGStTableResults1stDownloadBttn = By.XPath("//div[@data-testid='documentsTable']/div[@class='tbody']/div[1]/div/div[5]/div/div/div/button[@data-testid='document-download-button']");
        private By CDOGSTableResults1stViewBttn = By.XPath("//div[@data-testid='documentsTable']/div[@class='tbody']/div[1]/div/div[5]/div/div/button[@data-testid='document-view-button']");
        private By CDOGSTableResults1stDeleteBttn = By.XPath("//div[@data-testid='documentsTable']/div[@class='tbody']/div[1]/div/div[5]/div/div/button[@data-testid='document-delete-button']");

        //Document Delete Document Confirmation Modal Elements
        private By documentDeleteHeader = By.CssSelector("div[class='modal-header'] div[class='modal-title h4']");
        private By documentDeleteContent1 = By.CssSelector("div[class='modal-body'] div div:nth-child(1)");
        private By documentDeteleContent2 = By.CssSelector("div[class='modal-body'] div:nth-child(3)");
        private By documentDeleteContent3 = By.CssSelector("div[class='modal-body'] div strong");
        private By documentDeleteOkBttn = By.CssSelector("button[title='ok-modal']");

        public CDOGSTemplates(IWebDriver webDriver) : base(webDriver)
        {}

        public void NavigateAdminTemplates()
        {
            Wait(3000);

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
            WaitUntilClickable(CDOGSTableResults1stDeleteBttn);
            webDriver.FindElement(CDOGSTableResults1stDeleteBttn).Click();
            
            WaitUntilVisible(documentDeleteHeader);
            Assert.Equal("Delete a document", webDriver.FindElement(documentDeleteHeader).Text);
            Assert.Equal("You have chosen to delete this document.", webDriver.FindElement(documentDeleteContent1).Text);
            Assert.Equal("If the document is linked to other files or entities in PIMS it will still be accessible from there, however if this the only instance then the file will be removed from the document store completely.", webDriver.FindElement(documentDeteleContent2).Text);
            Assert.Equal("Do you wish to continue deleting this document?", webDriver.FindElement(documentDeleteContent3).Text);

            webDriver.FindElement(documentDeleteOkBttn).Click();
        }
    }
}
