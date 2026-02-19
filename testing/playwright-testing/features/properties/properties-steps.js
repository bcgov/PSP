const { Given, When, Then } = require("@cucumber/cucumber");
const searchPropertiesJson = require("../../data/SearchProperties.json");

let searchPropertiesData = [];
setDefaultTimeout(15000);

Given(
  "I search for a property by PID from row number {int}",
  async function (rowNbr) {
    searchPropertiesData = searchPropertiesJson[rowNbr];
    await searchPropertyByPID(searchPropertiesData.PID);
    await selectNthPMBCSearchResult(0);
  }
);

Given(
  "I search for a property by Plan number from row number {int}",
  async function (rowNbr) {
    searchPropertiesData = searchPropertiesJson[rowNbr];
    await this.searchProperties.searchPropertyByPlan(searchPropertiesData.Plan);
    await this.searchProperties.selectPinOnMap();
  }
);

When("I verify the Strata popup on map", async function () {
  await this.searchProperties.verifyMultiplePropertyPopup();
});

Then("The Property View on Map is rendered successfully", async function () {
  await this.searchProperties.resetSearch();
  await this.searchProperties.closeMultiplePropertyPopup();
});
