const { Given, When, Then } = require("@cucumber/cucumber");
const mgmFiles = require("../../data/managementFiles.json");
const {
  splitStringToArray,
  clickCancelButton,
} = require("../../support/common.js");
const { expect } = require("@playwright/test");

let managementFile = {};
let managementFileCode = "";

Given(
  "I create a new Management File with row number {int}",
  async function (rowNbr) {
    managementFile = mgmFiles[rowNbr];

    //Navigate to Management File
    await this.managementFileDetails.navigateManagementMainMenu();
    await this.managementFileDetails.createManagementFileLink();

    //Validate Management File Details Create Form
    await this.managementFileDetails.validateInitManagementFileDetailsPage();

    //Create basic Management File
    await this.managementFileDetails.createMinimumManagementFileDetails(
      managementFile
    );

    //Save Management File
    await this.managementFileDetails.saveManagementFile();

    //Get Acquisition File code
    managementFileCode =
      await this.managementFileDetails.getManagementFileCode();
  }
);

When(
  "I add additional information to the Management File Details",
  async function () {
    //Enter to Edit mode of Management File
    await this.managementFileDetails.updateManagementFileButton();

    //Verify Management file update init form
    await this.managementFileDetails.validateManagementUpdateForm();

    //Add Additional optional information to the management file
    await this.managementFileDetails.updateManagementFileDetails(
      managementFile
    );

    //Save Management File
    await this.managementFileDetails.saveManagementFile();

    //Validate View File Details View Mode
    await this.managementFileDetails.validateManagementDetailsViewForm(
      managementFile
    );

    //Verify automatic note created when status changes
    if (managementFile.ManagementStatus != "Active") {
      await this.notes.navigateNotesTab();
      await this.notes.verifyAutomaticNotes(
        "Management File",
        "Active",
        managementFile.ManagementStatus
      );
    }
  }
);

When(
  "I update the File details from an existing Management File from row number {int}",
  async function (rowNbr) {
    managementFile = mgmFiles[rowNbr];

    //Search for an existing Management File
    await this.searchManagementFiles.navigateToSearchManagement();
    await this.searchManagementFiles.filterManagementFiles({
      mgmtfile: managementFileCode,
    });
    await this.searchManagementFiles.selectFirstOption();

    //Update existing Management file
    await this.managementFileDetails.updateManagementFileButton();
    await this.managementFileDetails.updateManagementFileDetails(
      managementFile
    );

    //Cancel changes
    clickCancelButton(this.page);
    await this.managementFileDetails.cancel;

    //Edit Management File
    await this.managementFileDetails.updateManagementFileButton();
    await this.managementFileDetails.updateManagementFileDetails(
      managementFile
    );

    //Save Management File
    await this.managementFileDetails.saveManagementFile();

    //Get Mangement File code
    managementFileCode =
      await this.managementFileDetails.getManagementFileCode();

    //Validate View File Details View Mode
    await this.managementFileDetails.validateManagementDetailsViewForm(
      managementFile
    );

    //Verify automatic note created when
    if (managementFile.ManagementStatus != "Active") {
      await this.notes.navigateNotesTab();
      await this.notes.verifyAutomaticNotes(
        "Management File",
        "Hold",
        managementFile.ManagementStatus
      );
    }
  }
);

When("I add Properties to the Management File", async function () {
  //Navigate to Properties for Management File
  await this.sharedFileProperties.navigateToAddPropertiesToFile();

  //Search for a property by PID
  if (managementFile.ManagementSearchProperties.PID) {
    await this.searchProperties.searchPropertyByPID(
      managementFile.ManagementSearchProperties.PID
    );
    await this.searchProperties.selectNthSearchResult(0);
    await this.searchProperties.addPropertyInfoToOpenFile();
    await this.searchProperties.resetSearch();
  }

  //Search for a property by PIN
  if (managementFile.ManagementSearchProperties.PIN) {
    await this.searchProperties.searchPropertyByPIN(
      managementFile.ManagementSearchProperties.PIN
    );
    await this.searchProperties.selectNthSearchResult(0);
    await this.searchProperties.addPropertyInfoToOpenFile();
    await this.searchProperties.resetSearch();
  }

  //Search for a property by Plan
  if (managementFile.ManagementSearchProperties.PlanNumber) {
    await this.searchProperties.searchPropertyByPlan(
      managementFile.ManagementSearchProperties.PlanNumber
    );
    await this.searchProperties.selectNthSearchResult(0);
    await this.searchProperties.addPropertyInfoToOpenFile();
    await this.searchProperties.resetSearch();
  }

  //Search for a property by Address
  if (managementFile.ManagementSearchProperties.Address) {
    await this.searchProperties.searchPropertyByAddress(
      managementFile.ManagementSearchProperties.Address
    );
    await this.searchProperties.selectPinOnMap();
    await this.searchProperties.addPropertyInfoToOpenFile();
    await this.searchProperties.resetSearch();
  }

  //Search for a property by Legal Description
  if (managementFile.ManagementSearchProperties.LegalDescription) {
    await this.sharedFileProperties.selectPropertyByLegalDescription(
      managementFile.ManagementSearchProperties.LegalDescription
    );
    await this.searchProperties.selectNthSearchResult(0);
    await this.searchProperties.addPropertyInfoToOpenFile();
    await this.searchProperties.resetSearch();
  }

  //Search for a property by Latitude and Longitude
  if (
    managementFile.ManagementSearchProperties.LatitudeLongitude.LatitudeDegree
  ) {
    await this.sharedFileProperties.selectPropertyByLongLant(
      managementFile.ManagementSearchProperties.LatitudeLongitude
    );
    await this.searchProperties.selectNthSearchResult(0);
    await this.searchProperties.addPropertyInfoToOpenFile();
    await this.searchProperties.resetSearch();
  }

  //Search for Multiple PIDs
  if (managementFile.ManagementSearchProperties.MultiplePIDS) {
    const givenPIDs = await splitStringToArray(
      managementFile.ManagementSearchProperties.MultiplePIDS
    );
    await givenPIDs.forEach((pid) => {
      this.searchProperties.searchPropertyByPID(pid);
      this.searchProperties.selectNthSearchResult(0);
      this.searchProperties.addPropertyInfoToOpenFile();
      this.sharedFileProperties.resetSearch();
    });
  }

  //Search for a duplicate property
  if (managementFile.ManagementSearchProperties.PID) {
    await this.searchProperties.searchPropertyByPID(
      managementFile.ManagementSearchProperties.PID
    );
    await this.searchProperties.selectNthSearchResult(0);
    await this.searchProperties.addPropertyInfoToOpenFile();
    await this.sharedFileProperties.resetSearch();
  }

  //Save Management File
  await this.sharedFileProperties.saveFileProperties();
});

When(
  "I update a Management File's Properties from row number {int}",
  async function (rowNbr) {
    managementFile = mgmFiles[rowNbr];

    //Search for an existing Management File
    await this.searchManagementFiles.navigateToSearchManagement();
    await this.searchManagementFiles.filterManagementFiles({
      mgmtfile: managementFileCode,
    });
    await this.searchManagementFiles.selectFirstOption();

    //Navigate to Edit Management File's Properties
    await this.sharedFileProperties.navigateToAddPropertiesToFile();

    //Search for a property by Legal Description
    await this.sharedFileProperties.navigateToSearchTab();
    await this.sharedFileProperties.selectPropertyByLegalDescription(
      managementFile.ManagementSearchProperties.LegalDescription
    );
    await this.sharedFileProperties.selectFirstOptionFromSearch();

    //Save changes
    await this.sharedFileProperties.saveFileProperties();

    //Delete Property
    await this.sharedFileProperties.navigateToAddPropertiesToFile();
    await this.sharedFileProperties.deleteLastPropertyFromFile();

    //Save Acquisition File changes
    await this.sharedFileProperties.saveFileProperties();

    //Select 1st Property
    await this.sharedFileProperties.selectFirstPropertyOptionFromFile();
  }
);

When(
  "I search for an existing Management File from row number {int}",
  async function (rowNbr) {
    //Navigate to Management File Search
    managementFile = mgmFiles[rowNbr];
    await this.searchManagementFiles.navigateToSearchManagement();

    //Verify Pagination
    await this.sharedPagination.choosePaginationOption(5);
    const expectedPaginationCount5 =
      await this.searchManagementFiles.mgmtTableResultNumber();
    expect(expectedPaginationCount5).toBe(5);

    await this.sharedPagination.choosePaginationOption(10);
    const expectedPaginationCount10 =
      await this.searchManagementFiles.mgmtTableResultNumber();
    expect(expectedPaginationCount10).toBe(10);

    await this.sharedPagination.choosePaginationOption(20);
    const expectedPaginationCount20 =
      await this.searchManagementFiles.mgmtTableResultNumber();
    expect(expectedPaginationCount20).toBeLessThan(20);

    //await this.sharedPagination.choosePaginationOption(50);
    //const expectedPaginationCount50 = await this.searchManagementFiles.mgmtTableResultNumber();
    //expect(expectedPaginationCount50).toBe(50);

    //await this.sharedPagination.choosePaginationOption(100);
    //const expectedPaginationCount100 = await this.searchManagementFiles.mgmtTableResultNumber();
    //expect(expectedPaginationCount100).toBe(100);

    //Verify Column Sorting by File Name
    await this.searchManagementFiles.orderByMgmtFileName();
    const firstFileNameDescResult =
      await this.searchManagementFiles.firstMgmtFileName();

    await this.searchManagementFiles.orderByMgmtFileName();
    const firstFileNameAscResult =
      await this.searchManagementFiles.firstMgmtFileName();

    expect(firstFileNameDescResult).not.toEqual(firstFileNameAscResult);

    //Verify Column Sorting by Historical File Number
    await this.searchManagementFiles.orderByMgmtHistoricalFileNbr();
    const firstHistoricalDescResult =
      await this.searchManagementFiles.firstMgmtHistoricalFile();

    await this.searchManagementFiles.orderByMgmtHistoricalFileNbr();
    const firstHistoricalAscResult =
      await this.searchManagementFiles.firstMgmtHistoricalFile();

    expect(firstHistoricalDescResult).not.toEqual(firstHistoricalAscResult);

    //Verify Column Sorting by Purpose
    await this.searchManagementFiles.orderByMgmtPurpose();
    const firstFilePurposeDescResult =
      await this.searchManagementFiles.firstMgmtPurpose();

    await this.searchManagementFiles.orderByMgmtPurpose();
    const firstFilePurposeAscResult =
      await this.searchManagementFiles.firstMgmtPurpose();

    expect(firstFilePurposeDescResult).not.toEqual(firstFilePurposeAscResult);

    //Verify Column Sorting by Status
    await this.searchManagementFiles.orderByMgmtStatus();
    const firstFileStatusDescResult =
      await this.searchManagementFiles.firstMgmtStatus();

    await this.searchManagementFiles.orderByMgmtStatus();
    const firstFileStatusAscResult =
      await this.searchManagementFiles.firstMgmtStatus();

    expect(firstFileStatusDescResult).not.toEqual(firstFileStatusAscResult);

    //Verify Pagination display different set of results
    await this.sharedPagination.resetSearch();
    await this.sharedPagination.choosePaginationOption(5);

    const firstAcquisitionPage1 =
      await this.searchManagementFiles.firstMgmtFileName();
    await this.sharedPagination.goNextPage();

    const firstAcquisitionPage2 =
      await this.searchManagementFiles.firstMgmtFileName();
    expect(firstAcquisitionPage1).not.toEqual(firstAcquisitionPage2);

    await this.sharedPagination.resetSearch();

    //Filter Acquisition Files
    await this.searchManagementFiles.filterManagementFiles({
      pid: "003-549-551",
      mgmtfile: "Management from Jonathan Doe",
      status: "Cancelled",
    });
    expect(this.searchManagementFiles.searchFoundResults()).not.toBeTruthy;

    //Look for the last created Acquisition File
    await this.searchManagementFiles.filterManagementFiles({
      mgmtfile: managementFile.ManagementName,
      status: managementFile.ManagementStatus,
    });
  }
);

When("I insert activities to the Management Activities Tab", async function () {
  //Go to the Management Activity Tab
  await this.sharedActivities.navigateActivitiesTab();
  await this.sharedActivities.verifyActivitiesInitListsView();

  //Insert Activities
  await managementFile.ManagementPropertyActivities.forEach(
    async (activity) => {
      await this.sharedActivities.addActivityBttn();
      await this.sharedActivities.verifyCreateActivityInitForm(
        "Management File",
        activity.PropertyActivityPropsCount
      );
      await this.sharedActivities.insertNewPropertyActivity(activity);
      await this.sharedActivities.saveActivity();
      await this.sharedActivities.verifyInsertedActivity(
        activity,
        "Management File"
      );
      await this.sharedActivities.viewLastActivityFromList();
      await this.sharedActivities.verifyLastInsertedActivityTable(activity);
    }
  );
});

When("I update an activity from the Activities Tab", async function () {
  //Update an activity
  await this.sharedActivities.viewLastActivityFromList();
  await this.sharedActivities.viewLastActivityButton();
  await this.sharedActivities.updateSelectedActivityBttn();
  await this.sharedActivities.insertNewPropertyActivity(
    managementFile.ManagementPropertyActivities[0]
  );
  await this.sharedActivities.saveActivity();
  await this.sharedActivities.saveManagementActivity();
  await this.sharedActivities.verifyInsertedActivity(
    managementFile.ManagementPropertyActivities[0],
    "Management File"
  );
  await this.sharedActivities.viewLastActivityFromList();
  await this.sharedActivities.verifyLastInsertedActivityTable(
    managementFile.ManagementPropertyActivities[0]
  );
});

Then(
  "A new Management file is created or updated successfully",
  async function () {
    await this.searchManagementFiles.navigateToSearchManagement();
    await this.searchManagementFiles.filterManagementFiles({
      mgmtfile: managementFileCode,
    });
    expect(this.searchManagementFiles.searchFoundResults()).toBeTruthy();
  }
);

Then(
  "Expected Management File Content is displayed on Management File Table",
  async function () {
    //Verify List View
    await this.searchManagementFiles.verifySearchManagementListView();
    await this.searchManagementFiles.verifyManagementTableContent(
      managementFile
    );
  }
);
