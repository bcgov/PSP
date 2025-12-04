const { Given, When, Then } = require("@cucumber/cucumber");

Given("I navigate to the Worklist", async function () {
  await this.helpDesk.navigateHelpDeskForm();
});

When("I verify the count of the worklist items", async function () {
  await this.worklists.countItemsOnWorklist();
});

When("I delete properties on the worklist", async function () {
  await this.worklists.deleteNthElementWorklist();
});