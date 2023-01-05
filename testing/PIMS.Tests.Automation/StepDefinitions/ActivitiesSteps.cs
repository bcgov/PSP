

namespace PIMS.Tests.Automation.StepDefinitions
{
    [Binding]
    public class ActivitiesSteps
    {
        private readonly Activities activities;

        public ActivitiesSteps(BrowserDriver driver)
        {
            activities = new Activities(driver.Current);
        }

        [StepDefinition(@"I create a new activity")]
        public void CreateActivityWithproperties()
        {
            /* TEST COVERAGE:  PSP-4364, PSP-4363, PSP-4361, PSP-4360, PSP-4169, PSP-4170, PSP-4683, PSP-4478, PSP-4479 */

            //Acess the activity tab
            activities.AccessActivitiesTab();

            //Create new activity
            activities.CreateNewActivity();

            //Verify activities list view
            activities.VerifyActivityListView();

            //Verify activity Details view
            activities.VerifyActivityDetails();

            //Filter Activities
            activities.FilterActivities();

            //Select All Properties
            activities.SelectAllProperties();

            //Deselect All Properties
            activities.DesectAllProperties();
        }

        [StepDefinition(@"I create and delete an activity")]
        public void CreateDeleteActivity()
        {
            /* TEST COVERAGE:  PSP-4477, PSP-4784, PSP-4785, PSP-4362 */

            //Acess the activity tab
            activities.AccessActivitiesTab();

            //Create new activity
            activities.CreateNewActivity();

            //Verify activities list view
            activities.VerifyActivityListView();

            //Verify activity Details view
            activities.VerifyActivityDetails();

            //Check No Properties pop-up
            activities.NoProperties();

            //Change Status to Completed
            activities.ChangeStatus("Completed");
            Assert.False(activities.IsActivityBlocked());
            activities.CancelActivityChanges();

            //Change Status to Cancel
            activities.ChangeStatus("Cancelled");
            Assert.False(activities.IsActivityBlocked());
            activities.CancelActivityChanges();

            //Delete Activity
            activities.DeleteActivity();

        }

        [StepDefinition(@"An activity is created successfully")]
        public void ActivityCreatedSuccess()
        {
            Assert.True(activities.totalActivities() > 0);
        }

        [StepDefinition(@"An activity is deleted successfully")]
        public void ActivityDeletedSuccess()
        {
            Assert.True(activities.totalActivities() == 0);
        }

    }
}
