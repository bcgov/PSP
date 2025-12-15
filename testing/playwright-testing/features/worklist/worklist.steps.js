const { Given, When, Then } = require("@cucumber/cucumber");
const searchPropertiesJson = require("../../data/SearchProperties.json");
const { expect } = require("@playwright/test");

let searchPropertiesData = [];

When("I navigate to the Worklist", async function () {
  await this.workLists.navigateWorkLists();
});

When("I verify the Worklist view form for Property Worklist", async function () {
  await this.workLists.verifyWorkListForm();
});

When("I verify the count of the worklist items from row number {int}", async function (rowNbr) {
  searchPropertiesData = searchPropertiesJson[rowNbr];
  const worklistCountingResult = await this.workLists.countItemsOnWorklist(searchPropertiesData.TotalPropertiesCount);
  expect(worklistCountingResult).toBe(true);
});

When("I delete properties on the worklist", async function () {
  await this.workLists.deleteNthElementWorklist();
});

