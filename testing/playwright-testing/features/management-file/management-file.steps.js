const { When } = require("@cucumber/cucumber");
const {
  ManagementFileDetails,
} = require("../../support/pages/ManagementFileDetails.js");
const mgmFiles = require("../../data/managementFiles.json");
const {clickSaveButton,  clickCancelButton } = require("../../support/common.js");

let managementFileDetails;
let managementFileCode;

When(
  "I create a new Management File with row number {int}",
  async function (rowNbr) {
    managementFileDetails = new ManagementFileDetails(this.page);

    //Navigate to Management File
    await managementFileDetails.navigateManagementMainMenu();
    await managementFileDetails.createManagementFileLink();

    //Validate Management File Details Create Form
    await managementFileDetails.validateInitManagementFileDetailsPage();

    //Create basic Management File
    await managementFileDetails.createMinimumManagementFileDetails(mgmFiles[rowNbr]);

    //Save Management File
    await clickSaveButton(this.page);

    //Get Acquisition File code
    managementFileCode = await managementFileDetails.getManagementFileCode();
    //await managementFileDetails.fillManagementFileDetails(mgmFiles[rowNbr]);
  }
);
