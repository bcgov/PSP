const { When } = require("@cucumber/cucumber");
const {
  ManagementFileDetails,
} = require("../../support/pages/ManagementFileDetails.js");
const mgmFiles = require("../../data/managementFiles.json");

let managementFileDetails;

When(
  "I create a new Management File with row number {int}",
  async function (rowNbr) {
    managementFileDetails = new ManagementFileDetails(this.page);

    await managementFileDetails.managementMainMenu();
    await managementFileDetails.createManagementFileLink();
    await managementFileDetails.validateManagementFileDetailsPage();

    await managementFileDetails.fillManagementFileDetails(mgmFiles[rowNbr]);
  }
);
