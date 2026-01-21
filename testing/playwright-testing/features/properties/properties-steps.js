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
  }
);

Given(
  "I search for a property by coordinates from row number {int}",
  async function (rowNbr) {
    searchPropertiesData = searchPropertiesJson[rowNbr];
  }
);

When("I verify the Property's PID details", async function () {});

When("I verify the Property's Plan number details", async function () {});

When("I verify the Property's coordinates details", async function () {});

Then("The Property View on Map is rendered successfully", async function () {});
