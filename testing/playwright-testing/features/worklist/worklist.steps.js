const { Given, When, Then, setDefaultTimeout } = require("@cucumber/cucumber");
const searchPropertiesJson = require("../../data/SearchProperties.json");
const { expect } = require("@playwright/test");

let searchPropertiesData = {};
setDefaultTimeout(100000);

When("I navigate to the Worklist", async function () {
  await this.workLists.navigateWorkLists();
});

When(
  "I verify the count of the worklist items from row number {int}",
  async function (rowNbr) {
    searchPropertiesData = searchPropertiesJson[rowNbr];
    const worklistCountingResult = await this.workLists.countItemsOnWorklist(
      searchPropertiesData.TotalPropertiesCount
    );
    expect(worklistCountingResult).toBe(true);
  }
);

When(
  "I verify the Worklist view form for Property Worklist",
  async function () {
    await this.workLists.verifyWorkListForm(
      searchPropertiesData.TotalPropertiesCount
    );
    await this.worklists.verifyWorklistMenuUptions();
  }
);

When("I delete properties on the worklist", async function () {
  await this.workLists.deleteNthElementWorklist(0);
});

When("I create a file from the worklist", async function () {
  const countOnPropDetails =
    await this.sharedFileProperties.countInsertedPropertiesOnFilePropDetails();
  expect(countOnPropDetails).toBe(searchPropertiesData.TotalPropertiesCount);

  await this.workLists.createFileWithProps();
});

When(" I verify the Worklist view form for Property Strata Worklist", async function(){
  await this.worklists.verifyWorklistStrata();
});

Then("The Worklist section is rendered successfully", async function () {
  const propsAfterDelete = searchPropertiesData.TotalPropertiesCount - 1;
  await this.worklLists.verifyWorkListForm(propsAfterDelete);
});

Then("The file is created successfully from the worklist", async function () {
  const countOnPropSidebar =
    await this.sharedFileProperties.countInsertedPropertiesOnFileSideBar();
  expect(countOnPropSidebar).toBe(searchPropertiesData.TotalPropertiesCount);
});
