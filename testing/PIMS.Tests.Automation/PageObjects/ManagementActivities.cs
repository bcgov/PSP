using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class ManagementActivities : PageObjectBase
    {
        private readonly By activitiesListSubtitle = By.XPath("//div[contains(text(),'Activities List')]");

        private readonly By activitiesTabLink = By.CssSelector("a[data-rb-event-key='activities']");
        private readonly By activitiesBttn = By.CssSelector("button[data-testid='add-activity-button']");

        private readonly By activitiesParagraph = By.XPath("//p[contains(text(),' You can attach a document after creating the activity. Create, then edit and attach a file if needed.')]");
        private readonly By activitiesListTable = By.CssSelector("div[data-testid='mgmt-activity-list']");
        private readonly By activitiesListTableActivityTypeColumn = By.CssSelector("div[data-testid='mgmt-activity-list'] div[class='thead thead-light'] div[role='columnheader']:nth-child(1)");
        private readonly By activitiesListTableActTypeSortBttn = By.CssSelector("div[data-testid='mgmt-activity-list'] div[data-testid='sort-column-activityType']");
        private readonly By activitiesListTableActivitySubtypeColumn = By.CssSelector("div[data-testid='mgmt-activity-list'] div[class='thead thead-light'] div[role='columnheader']:nth-child(2)");
        private readonly By activitiesListTableActSubypeSortBttn = By.CssSelector("div[data-testid='mgmt-activity-list'] div[data-testid='sort-column-activitySubTypes']");
        private readonly By activitiesListTableActivityStatusColumn = By.CssSelector("div[data-testid='mgmt-activity-list'] div[class='thead thead-light'] div[role='columnheader']:nth-child(3)");
        private readonly By activitiesListTableActStatusSortBttn = By.CssSelector("div[data-testid='mgmt-activity-list'] div[data-testid='sort-column-activityStatusType']");
        private readonly By activitiesListTableCommencementColumn = By.CssSelector("div[data-testid='mgmt-activity-list'] div[class='thead thead-light'] div[role='columnheader']:nth-child(4)");
        private readonly By activitiesListTableActCommencementSortBttn = By.CssSelector("div[data-testid='mgmt-activity-list'] div[data-testid='sort-column-requestedAddedDate']");
        private readonly By activitiesListTableActionsColumn = By.CssSelector("div[data-testid='mgmt-activity-list'] div[class='thead thead-light'] div[role='columnheader']:nth-child(5)");
        private readonly By activitiesListTable1stActTypeContext = By.XPath("//div[contains(text(),'Activity List')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div[@data-testid='PropertyManagementActivitiesTable']/div[@class='tbody']/div[1]/div/div[1]");
        private readonly By activitiesListTable1stActSubtypeContext = By.XPath("//div[contains(text(),'Activity List')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div[@data-testid='PropertyManagementActivitiesTable']/div[@class='tbody']/div[1]/div/div[2]");
        private readonly By activitiesListTable1stActStatusContext = By.XPath("//div[contains(text(),'Activity List')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div[@data-testid='PropertyManagementActivitiesTable']/div[@class='tbody']/div[1]/div/div[3]");
        private readonly By activitiesListTable1stActCommencementContext = By.XPath("//div[contains(text(),'Activity List')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div[@data-testid='PropertyManagementActivitiesTable']/div[@class='tbody']/div[1]/div/div[4]");
        private readonly By activitiesListTable1stActViewBttn = By.XPath("//div[contains(text(),'Activity List')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div[@data-testid='PropertyManagementActivitiesTable']/div[@class='tbody']/div[1]/div/div[5]/div/button[@title='property-activity view details']");
        private readonly By activitiesListTable1stActDeleteBttn = By.XPath("//div[contains(text(),'Activity List')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div[@data-testid='PropertyManagementActivitiesTable']/div[@class='tbody']/div[1]/div/div[5]/div/button[@title='Delete']");
        private readonly By activitiesListTable1stActWarning = By.XPath("//div[contains(text(),'Activity List')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div[@data-testid='PropertyManagementActivitiesTable']/div[@class='tbody']/div[1]/div/div[5]/div/button/following-sibling::*");
        private readonly By activitiesListTableBodyCount = By.CssSelector("div[data-testid='mgmt-activity-list'] div[class='tbody'] div[class='tr-wrapper']");
        private readonly By activitiesListPaginationOptions = By.XPath("//div[@data-testid='mgmt-activity-list']/following-sibling::div/div/ul[@class='pagination']/li");

        private readonly By activitiesAdhocListSubtitle = By.XPath("//div[contains(text(),'Ad-hoc Activities List')]");
        private readonly By activitiesAdhocListTooltip = By.CssSelector("span[data-testid='tooltip-icon-property-file-activity-summary']");
        private readonly By activitiesAdhocListTableExpandBttn = By.XPath("//div[contains(text(),'Ad-hoc Activities List')]/parent::div/parent::div/parent::div/following-sibling::div");
        private readonly By activitiesAdhocListTableActivityTypeColumn = By.CssSelector("div[data-testid='adhoc-activity-list-readonly'] div[class='thead thead-light'] div[role='columnheader']:nth-child(1)");
        private readonly By activitiesAdhocListTableActTypeSortBttn = By.CssSelector("div[data-testid='adhoc-activity-list-readonly'] div[data-testid='sort-column-activityType']");

        private readonly By activitiesAdhocListTableActivitySubtypeColumn = By.CssSelector("div[data-testid='adhoc-activity-list-readonly'] div[class='thead thead-light'] div[role='columnheader']:nth-child(2)");
        private readonly By activitiesAdhocListTableActSubypeSortBttn = By.CssSelector("div[data-testid='adhoc-activity-list-readonly'] div[data-testid='sort-column-activitySubTypes']");

        private readonly By activitiesAdhocListTableActivityStatusColumn = By.CssSelector("div[data-testid='adhoc-activity-list-readonly'] div[class='thead thead-light'] div[role='columnheader']:nth-child(3)");
        private readonly By activitiesAdhocListTableActStatusSortBttn = By.CssSelector("div[data-testid='adhoc-activity-list-readonly'] div[data-testid='sort-column-activityStatusType']");

        private readonly By activitiesAdhocListTableActivityCommencementColumn = By.CssSelector("div[data-testid='adhoc-activity-list-readonly'] div[class='thead thead-light'] div[role='columnheader']:nth-child(4)");
        private readonly By activitiesAdhocListTableActCommencementSortBttn = By.CssSelector("div[data-testid='adhoc-activity-list-readonly'] div[data-testid='sort-column-requestedAddedDate']");

        private readonly By activitiesAdhocListTableActivityNavigationColumn = By.CssSelector("div[data-testid='adhoc-activity-list-readonly'] div[class='thead thead-light'] div[role='columnheader']:nth-child(5)");
        private readonly By activitiesAdhocListTable1stActTypeContext = By.XPath("//div[contains(text(),'Ad-Hoc Activity Summary')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div[@data-testid='PropertyManagementActivitiesTable']/div[@class='tbody']/div[1]/div/div[1]");
        private readonly By activitiesAdhocListTable1stActSubtypeContext = By.XPath("//div[contains(text(),'Ad-Hoc Activity Summary')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div[@data-testid='PropertyManagementActivitiesTable']/div[@class='tbody']/div[1]/div/div[2]");
        private readonly By activitiesAdhocListTable1stActStatusContext = By.XPath("//div[contains(text(),'Ad-Hoc Activity Summary')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div[@data-testid='PropertyManagementActivitiesTable']/div[@class='tbody']/div[1]/div/div[3]");
        private readonly By activitiesAdhocListTable1stActCommencementContext = By.XPath("//div[contains(text(),'Ad-Hoc Activity Summary')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div[@data-testid='PropertyManagementActivitiesTable']/div[@class='tbody']/div[1]/div/div[4]");
        private readonly By activitiesAdhocListTable1stActNavigationContext = By.XPath("//div[contains(text(),'Ad-Hoc Activity Summary')]/parent::div/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div[@data-testid='PropertyManagementActivitiesTable']/div[@class='tbody']/div[1]/div/div[5]/a");
        private readonly By activitiesTablesEmptyInfo = By.XPath("//div[contains(text(),'No property management activities found')]");


        private readonly By activityTrayEditBttn = By.CssSelector("button[title='Edit File Property Activity']");

        //Management Activity Confirmation Modal Elements
        private readonly By actibityConfirmationModal = By.CssSelector("div[class='modal-content']");

        private SharedModals sharedModals;
        private SharedSelectContact sharedSelectContact;

        public ManagementActivities(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
            sharedSelectContact = new SharedSelectContact(webDriver);
        }

        public void NavigateActivitiesTab()
        {
            WaitUntilClickable(activitiesTabLink);
            webDriver.FindElement(activitiesTabLink).Click();
        }

        public void AddActivityBttn()
        {
            Wait(4000);
            webDriver.FindElement(activitiesBttn).Click();

            WaitUntilSpinnerDisappear();
        }

        public void OpenActivityDetails(int index)
        {
            Wait();

            WaitUntilClickable(By.XPath("//div[contains(text(),'Activity List')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div[@data-testid='PropertyManagementActivitiesTable']/div[@class='tbody']/div["+ index +"]/div/div[5]/div/button[@title='property-activity view details']"));
            webDriver.FindElement(By.XPath("//div[contains(text(),'Activity List')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div[@data-testid='PropertyManagementActivitiesTable']/div[@class='tbody']/div["+ index +"]/div/div[5]/div/button[@title='property-activity view details']")).Click();
        }

        public void DeleteNthActivity(int index)
        {
            Wait();
            FocusAndClick(By.CssSelector("div[data-testid='AcquisitionCompensationTable'] div[class='tbody'] div[class='tr-wrapper'] div button[data-testid='compensation-delete-"+ index +"']"));

            if (webDriver.FindElements(actibityConfirmationModal).Count() > 0)
            {
                Assert.Equal("Confirm Delete", sharedModals.ModalHeader());
                Assert.Equal("Are you sure you want to delete this item?", sharedModals.ModalContent());
                sharedModals.ModalClickOKBttn();
            }

            WaitUntilSpinnerDisappear();
        }

        public void SaveActivity()
        {
            Wait();
            ButtonElement("Save");

            AssertTrueIsDisplayed(activityTrayEditBttn);
        }

        public void CancelActivity()
        {
            Wait();
            ButtonElement("Cancel");

            sharedModals.CancelActionModal();

            AssertTrueIsDisplayed(activityTrayEditBttn);
        }

        public void EditActivityDetails()
        {
            WaitUntilVisible(activityTrayEditBttn);
            FocusAndClick(activityTrayEditBttn);
        }

        public void VerifyActivitiesInitListsView()
        {
            //Activity List
            AssertTrueIsDisplayed(activitiesListSubtitle);
            AssertTrueIsDisplayed(activitiesBttn);
            AssertTrueIsDisplayed(activitiesParagraph);
            AssertTrueIsDisplayed(activitiesListTable);
            AssertTrueIsDisplayed(activitiesListTableActivityTypeColumn);
            AssertTrueIsDisplayed(activitiesListTableActTypeSortBttn);
            AssertTrueIsDisplayed(activitiesListTableActivitySubtypeColumn);
            AssertTrueIsDisplayed(activitiesListTableActSubypeSortBttn);
            AssertTrueIsDisplayed(activitiesListTableActivityStatusColumn);
            AssertTrueIsDisplayed(activitiesListTableActStatusSortBttn);
            AssertTrueIsDisplayed(activitiesListTableCommencementColumn);
            AssertTrueIsDisplayed(activitiesListTableActCommencementSortBttn);
            AssertTrueIsDisplayed(activitiesListTableActionsColumn);

            //Ad-Hoc Activity Summary
            AssertTrueIsDisplayed(activitiesAdhocListSubtitle);
            AssertTrueIsDisplayed(activitiesAdhocListTooltip);
            AssertTrueIsDisplayed(activitiesAdhocListTableExpandBttn);
            AssertTrueIsDisplayed(activitiesAdhocListTableActivityTypeColumn);
            AssertTrueIsDisplayed(activitiesAdhocListTableActTypeSortBttn);
            AssertTrueIsDisplayed(activitiesAdhocListTableActivitySubtypeColumn);
            AssertTrueIsDisplayed(activitiesAdhocListTableActSubypeSortBttn);
            AssertTrueIsDisplayed(activitiesAdhocListTableActivityStatusColumn);
            AssertTrueIsDisplayed(activitiesAdhocListTableActStatusSortBttn);
            AssertTrueIsDisplayed(activitiesAdhocListTableActivityCommencementColumn);
            AssertTrueIsDisplayed(activitiesAdhocListTableActCommencementSortBttn);
            AssertTrueIsDisplayed(activitiesAdhocListTableActivityNavigationColumn);

            Assert.True(webDriver.FindElements(activitiesTablesEmptyInfo).Count == 1);
        }

        public void ViewLastActivityFromList()
        {
            var paginationLastPage = webDriver.FindElements(activitiesListPaginationOptions).Count() -1;

            webDriver.FindElement(By.XPath("//div[@data-testid='mgmt-activity-list']/following-sibling::div/div/ul[@class='pagination']/li["+ paginationLastPage +"]")).Click();
        }

        public void ViewLastActivityButton()
        {
            Wait();
            webDriver.FindElement(By.CssSelector("div[data-testid='mgmt-activity-list'] div[class='tr-wrapper']:first-child button[title='property-activity view details']")).Click();
        }

        public void VerifyLastInsertedActivityTable(PropertyActivity activity)
        {
            Wait();

            AssertTrueContentEquals(By.CssSelector("div[data-testid='mgmt-activity-list'] div[class='tbody'] div[class='tr-wrapper']:first-child [role='cell']:first-child"), activity.PropertyActivityType);
//            AssertTrueContentEquals(By.XPath("//div[contains(text(),'Activity List')]/parent::div/parent::div/parent::div/parent::h2/following-sibling::div/div[@data-testid='PropertyManagementActivitiesTable']/div[@class='tbody']/div[@class='tr-wrapper']["+ lastInsertedActivityIndex +"]/div/div[@role='cell'][2]"), activity.PropertyActivitySubType);  TODO
            AssertTrueContentEquals(By.CssSelector("div[data-testid='mgmt-activity-list'] div[class='tbody'] div[class='tr-wrapper']:first-child [role='cell']:nth-child(3)"), activity.PropertyActivityStatus);
            AssertTrueContentEquals(By.CssSelector("div[data-testid='mgmt-activity-list'] div[class='tbody'] div[class='tr-wrapper']:first-child [role='cell']:nth-child(4)"), TransformDateFormat(activity.PropertyActivityCommenceDate));
            Assert.True(webDriver.FindElements(By.CssSelector("div[data-testid='mgmt-activity-list'] div[class='tbody'] div[class='tr-wrapper']:first-child [role='cell']:nth-child(5) div div")).Count > 0);
        }

    }
}
