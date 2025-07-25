const { Given, When, Then } = require("@cucumber/cucumber");
const {
  ManagementFileDetails,
} = require("../../support/pages/ManagementFileDetails.js");

let managementFileDetails;

When("I create a new Management File from row", async function () {
  managementFileDetails = new ManagementFileDetails(this.page);

  await managementFileDetails.managementMainMenu();
  await managementFileDetails.createManagementFileLink();
  await managementFileDetails.validateManagementFileDetailsPage();
});
