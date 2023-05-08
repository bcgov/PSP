

using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class Activities : PageObjectBase
    {
        //Activity List View Elements
        //Add Activities Elements
    //    private By activitiesTab = By.CssSelector("a[data-rb-event-key='activities']");
    //    private By activitiesTitle = By.XPath("//div[contains(text(),'Activities')]");
    //    private By activityTypeSelect = By.Id("input-activityTypeId");
    //    private By activityAddBttn = By.CssSelector("div[class='col-md-4'] button");

    //    //Activities Filter Elements
    //    private By activityFilterLabel = By.XPath("//form/div/div/label[contains(text(),'Filter by')]");
    //    private By activityTypeFilterSelect = By.Id("input-activityTypeId");
    //    private By activityStatusFilterSelect = By.Id("input-status");
    //    private By activityFilterSearchBttn = By.XPath("//div[contains(text(),'Activities')]/parent::div/parent::h2/following-sibling::div/form/div/div[4]/button");
    //    private By activityFilterResetBttn = By.XPath("//div[contains(text(),'Activities')]/parent::div/parent::h2/following-sibling::div/form/div/div[5]/button");

    //    //Activity List View Results
    //    private By activityTable = By.CssSelector("div[data-testid='ActivityTable']");
    //    private By activityTypeColumn = By.XPath("//div[@data-testid='ActivityTable']/div/div/div/div[contains(text(),'Activity type')]");
    //    private By activityDescColumn = By.XPath("//div[@data-testid='ActivityTable']/div/div/div/div[contains(text(),'Description')]");
    //    private By activityPropertyColumn = By.XPath("//div[@data-testid='ActivityTable']/div/div/div/div[contains(text(),'Property')]");
    //    private By activityStatusColumn = By.XPath("//div[@data-testid='ActivityTable']/div/div/div/div[contains(text(),'Status')]");
    //    private By activityActionsColumn = By.XPath("//div[@data-testid='ActivityTable']/div/div/div/div[contains(text(),'Actions')]");
    //    private By activity1stGeneralActBttn = By.XPath("(//div[contains(text(),'General')]/parent::button)[1]");
    //    private By activity1stViewActionBttn = By.XPath("(//button[@title='View Activity'])[1]");
    //    private By activity1stDeleteActionBttn = By.XPath("(//button[@title='View Activity']/following-sibling::button)[1]");
    //    private By activity1stPropertyCell = By.CssSelector("div[data-testid='ActivityTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div:nth-child(3)");
    //    private By activity1stStatusCell = By.CssSelector("div[data-testid='ActivityTable'] div[class='tbody'] div[class='tr-wrapper']:nth-child(1) div:nth-child(4)");
    //    private By activityNoFound = By.XPath("//div[contains(text(),'No matching Activity found')]");
    //    private By activitiesListTotal = By.CssSelector("div[data-testid='ActivityTable'] div[class='tbody'] div[class='tr-wrapper']");

    //    //Activity Pagination Elements
    //    private By activityPaginationSpan = By.CssSelector("div[class='align-self-center col-auto'] span");
    //    private By activityPagination = By.CssSelector("ul[class='pagination']");

    //    //Activity Details Elements
    //    //Activity Details Header
    //    private By activityHeaderTittle = By.XPath("//div[@data-testid='activity-tray']/div[contains(text(),'Activity')]");
    //    private By activityHeaderFileLabel = By.XPath("//div[@data-testid='activity-tray']/div/div/div/div/div/div/div/div/label[contains(text(),'File')]");
    //    private By activityHeaderFileContent = By.XPath("//div[@data-testid='activity-tray']/div/div/div/div/div/div/div/div/label[contains(text(),'File')]/parent::div/following-sibling::div/strong");
    //    private By activityHeaderCreatedLabel = By.XPath("//div[@data-testid='activity-tray']/div/div/div/div/div/div[contains(text(),'Created')]");
    //    private By activityHeaderCreatedDate = By.XPath("//div[@data-testid='activity-tray']/div/div/div/div/div/div[contains(text(),'Created')]/strong");
    //    private By activityHeaderCreatedByUser = By.XPath("//div[@data-testid='activity-tray']/div/div/div/div/div/div[contains(text(),'Created')]/span");
    //    private By activityHeaderUpdatedLabel = By.XPath("//div[@data-testid='activity-tray']/div/div/div/div/div/div[contains(text(),'Last updated')]");
    //    private By activityHeaderUpdatedDate = By.XPath("//div[@data-testid='activity-tray']/div/div/div/div/div/div[contains(text(),'Last updated')]/strong");
    //    private By activityHeaderUpdateByUser = By.XPath("//div[@data-testid='activity-tray']/div/div/div/div/div/div[contains(text(),'Last updated')]/span");
    //    private By activityHeaderStatusLabel = By.XPath("//div[@data-testid='activity-tray']/div/div/div/div/div/div/div/div/label[contains(text(),'Status')]");
    //    private By activityHeaderStatusContent = By.XPath("//div[@data-testid='activity-tray']/div/div/div/div/div/div/div/div/label[contains(text(),'Status')]/parent::div/following-sibling::div/strong");

    //    //Activity Details Elements
    //    private By activityDetailsTitle = By.XPath("//div[@data-testid='activity-tray']/div/div/div[2]/div[1]/h2/div/div[contains(text(),'Details')]");
    //    private By activityDetailsRelatedPropsBttn = By.XPath("//div[contains(text(),'Related properties')]/parent::button");
    //    private By activityDescriptionLabel = By.XPath("//h2/div/div[contains(text(),'Description')]");
    //    private By activityDescriptionTextArea = By.Id("input-description");
    //    private By activityStatusSelect = By.Id("input-activityStatusTypeCode.id");
    //    private By activityDetailsEditBttn = By.XPath("//div[@data-testid='activity-tray']/div[2]/div/div[3]/div/button");

    //    //Related Properties Pop-up
    //    private By activityPropertiesModal = By.CssSelector("div[class='modal-content']");
    //    private By activityPropertiesAllInput = By.CssSelector("input[data-testid='selectrow-parent']");

    //    private SharedDocumentsTab digitalDocuments;
    //    private Notes notes;
    //    private SharedModals sharedModals;


        public Activities(IWebDriver webDriver) : base(webDriver)
        {
    //        digitalDocuments = new SharedDocumentsTab(webDriver);
    //        notes = new Notes(webDriver);
    //        sharedModals = new SharedModals(webDriver);
        }

    //    public void AccessActivitiesTab()
    //    {
    //        Wait(2000);
    //        webDriver.FindElement(activitiesTab).Click();
    //    }

    //    public void CreateNewActivity(string activityType)
    //    {
    //        Wait();
    //        ChooseSpecificSelectOption(activityTypeSelect, activityType);
    //        webDriver.FindElement(activityAddBttn).Click();

    //        Wait();
    //        var lastActivityIndex = webDriver.FindElements(activitiesListTotal).Count();
    //        webDriver.FindElement(By.CssSelector("div[class='tbody'] div[class='tr-wrapper']:nth-child("+ lastActivityIndex +") div[role='row'] div[role='cell']:nth-child(1) button")).Click();
    //    }

    //    public void SelectFirstActivity()
    //    {
    //        Wait();
    //        webDriver.FindElement(activity1stGeneralActBttn).Click();
    //    }

    //    public void VerifyActivityListView()
    //    {
    //        Wait();
    //        webDriver.FindElement(activity1stViewActionBttn).Click();

    //        Wait();
    //        Assert.True(webDriver.FindElement(activitiesTitle).Displayed);
    //        Assert.True(webDriver.FindElement(activityTypeSelect).Displayed);
    //        Assert.True(webDriver.FindElement(activityAddBttn).Displayed);

    //        Assert.True(webDriver.FindElement(activityFilterLabel).Displayed);
    //        Assert.True(webDriver.FindElement(activityTypeFilterSelect).Displayed);
    //        Assert.True(webDriver.FindElement(activityStatusFilterSelect).Displayed);
    //        Assert.True(webDriver.FindElement(activityFilterSearchBttn).Displayed);
    //        Assert.True(webDriver.FindElement(activityFilterResetBttn).Displayed);

    //        Assert.True(webDriver.FindElement(activityTable).Displayed);
    //        Assert.True(webDriver.FindElement(activityTypeColumn).Displayed);
    //        Assert.True(webDriver.FindElement(activityDescColumn).Displayed);
    //        Assert.True(webDriver.FindElement(activityPropertyColumn).Displayed);
    //        Assert.True(webDriver.FindElement(activityStatusColumn).Displayed);
    //        Assert.True(webDriver.FindElement(activityActionsColumn).Displayed);
    //        Assert.True(webDriver.FindElement(activity1stGeneralActBttn).Displayed);
    //        Assert.True(webDriver.FindElement(activity1stViewActionBttn).Displayed);
    //        Assert.True(webDriver.FindElement(activity1stDeleteActionBttn).Displayed);

    //        Assert.True(webDriver.FindElement(activityPaginationSpan).Displayed);
    //        Assert.True(webDriver.FindElement(activityPagination).Displayed);

    //    }

    //    public void VerifyActivityDetails()
    //    {
    //        Wait();

    //        //Header
    //        Assert.True(webDriver.FindElement(activityHeaderTittle).Displayed);
    //        Assert.True(webDriver.FindElement(activityHeaderFileLabel).Displayed);
    //        Assert.True(webDriver.FindElement(activityHeaderFileContent).Displayed);
    //        Assert.True(webDriver.FindElement(activityHeaderCreatedLabel).Displayed);
    //        Assert.True(webDriver.FindElement(activityHeaderCreatedDate).Displayed);
    //        Assert.True(webDriver.FindElement(activityHeaderCreatedByUser).Displayed);
    //        Assert.True(webDriver.FindElement(activityHeaderUpdatedLabel).Displayed);
    //        Assert.True(webDriver.FindElement(activityHeaderUpdatedDate).Displayed);
    //        Assert.True(webDriver.FindElement(activityHeaderUpdateByUser).Displayed);
    //        Assert.True(webDriver.FindElement(activityHeaderStatusLabel).Displayed);
    //        Assert.True(webDriver.FindElement(activityHeaderStatusContent).Displayed);

    //        //Details & Description
    //        Assert.True(webDriver.FindElement(activityDetailsTitle).Displayed);
    //        Assert.True(webDriver.FindElement(activityDetailsRelatedPropsBttn).Displayed);
    //        Assert.True(webDriver.FindElement(activityDescriptionLabel).Displayed);

    //        //Documents
    //        digitalDocuments.VerifyActivityDocumentsListView();

    //        //Notes
    //        notes.VerifyNotesListView();

    //        //Edit button
    //        Assert.True(webDriver.FindElement(activityDetailsEditBttn).Displayed);

    //    }

    //    public void FilterActivities()
    //    {
    //        Wait();
    //        ChooseSpecificSelectOption(activityStatusFilterSelect, "In Progress");
    //        webDriver.FindElement(activityFilterSearchBttn).Click();

    //        Wait();
    //        Assert.True(webDriver.FindElement(activityNoFound).Displayed);

    //        webDriver.FindElement(activityFilterResetBttn).Click();

    //        ChangeStatus("In Progress");

    //        ChooseSpecificSelectOption(activityStatusFilterSelect, "In Progress");
    //        webDriver.FindElement(activityFilterSearchBttn).Click();

    //        Wait();
    //        Assert.True(webDriver.FindElements(activitiesListTotal).Count() > 0);

    //    }

    //    public void SelectAllProperties()
    //    {
    //        Wait();
    //        Assert.True(webDriver.FindElement(activity1stPropertyCell).Text == "");

    //        Wait();
    //        ButtonElement("Related properties");

    //        WaitUntil(activityPropertiesModal);
    //        webDriver.FindElement(activityPropertiesAllInput).Click();
    //        sharedModals.ModalClickOKBttn();

    //        Wait();
    //        Assert.True(webDriver.FindElement(activity1stPropertyCell).Text == "All");

    //    }

    //    public void DesectAllProperties()
    //    {
    //        Wait();
    //        Assert.True(webDriver.FindElement(activity1stPropertyCell).Text == "All");

    //        Wait();
    //        ButtonElement("Related properties");

    //        WaitUntil(activityPropertiesModal);
    //        webDriver.FindElement(activityPropertiesAllInput).Click();
    //        sharedModals.ModalClickOKBttn();

    //        Wait();
    //        Assert.True(webDriver.FindElement(activity1stPropertyCell).Text == "");
    //    }

    //    public void NoProperties()
    //    {
    //        Wait();
    //        ButtonElement("Related properties");

    //        WaitUntil(activityPropertiesModal);
    //        Assert.True(sharedModals.ModalHeader() == "Related properties");
    //        Assert.True(sharedModals.ModalContent() == "To link activity to one or more properties, add properties to the parent file first");
    //        sharedModals.ModalClickOKBttn(); 
    //    }

    //    public void EditActivity()
    //    {
    //        Wait();
    //        webDriver.FindElement(activityDetailsEditBttn).Click();
    //    }

    //    public void ChangeStatus(string toStatus)
    //    {
    //        Wait();
    //        var previousStatus = webDriver.FindElement(activity1stStatusCell).Text;

    //        webDriver.FindElement(activityDetailsEditBttn).Click();

    //        ChooseSpecificSelectOption(activityStatusSelect, toStatus);
    //        ButtonElement("Save");

    //        Wait();
    //        notes.VerifyAutomaticNotes(previousStatus, toStatus);
    //    }

    //    public void InsertDescription(string description)
    //    {
    //        Wait();
    //        webDriver.FindElement(activityDetailsEditBttn).Click();

    //        webDriver.FindElement(activityDescriptionTextArea).SendKeys(description);

    //    }

    //    public void DeleteActivity()
    //    {
    //        Wait();
    //        webDriver.FindElement(activity1stDeleteActionBttn).Click();

    //        WaitUntil(activityPropertiesModal);
    //        sharedModals.ModalClickOKBttn();
    //    }

    //    public void SaveActivityChanges()
    //    {
    //        Wait();
    //        ButtonElement("Save");
    //    }

    //    public void CancelActivityChanges()
    //    {
    //        Wait();
    //        ButtonElement("Cancel");
    //    }

    //    public Boolean IsActivityBlocked()
    //    {
    //        Wait();
    //        webDriver.FindElement(activityDetailsEditBttn).Click();
    //        return webDriver.FindElements(activityDescriptionTextArea).Count() == 0;
    //    }

    //    public int totalActivities()
    //    {
    //        return webDriver.FindElements(activitiesListTotal).Count();
    //    }
    }
}
