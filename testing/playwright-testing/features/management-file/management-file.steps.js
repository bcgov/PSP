const { When } = require("@cucumber/cucumber");
const {
  ManagementFileDetails,
} = require("../../support/pages/ManagementFileDetails.js");
const mgmFiles = require("../../data/managementFiles.json");
const { clickSaveButton } = require("../../support/common.js");

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
    await managementFileDetails.createMinimumManagementFileDetails(
      mgmFiles[rowNbr]
    );

    //Save Management File
    await clickSaveButton(this.page);

    //Get Acquisition File code
    managementFileCode = await managementFileDetails.getManagementFileCode();
  }
);

When(
  "I add additional information to the Management File Details",
  async function () {
    //Enter to Edit mode of Management File
    managementFilesDetails.updateManagementFileButtonClick();

    //Verify Management file update init form
    managementFilesDetails.validateManagementUpdateForm();

    //Add Additional Optional information to the management file
    managementFilesDetails.updateManagementFileDetails(mgmFiles[rowNbr]);

    //Save Acquisition File
    await clickSaveButton(this.page);

    //Validate View File Details View Mode
    managementFilesDetails.validateManagementDetailsViewForm(mgmFiles[rowNbr]);

    //Verify automatic note created when status changes
    if (managementFile.ManagementStatus != "Active") {
      notes.navigateNotesTab();
      notes.verifyAutomaticNotes(
        "Management File",
        "Active",
        managementFile.ManagementStatus
      );
    }
  }
);
