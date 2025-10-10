const { Given, When, Then } = require("@cucumber/cucumber");

Given("I navigate to the Help Desk Form", async function () {
  await this.helpDesk.navigateHelpDeskForm();
});

When("I verify the Help Desk Form fields", async function () {
  await this.helpDesk.verifyHelpDeskModal();
});

Then("The Help Desk Form is rendered successfully", async function () {
  await this.helpDesk.cancelHelpDeskModal();
});

Given("I verify the Search Controls", async function () {
  await this.searchProperties.verifySearchControlForm();
});

When("I search for a property", async function () {
  await this.searchProperties.searchPropertyByPID("024-229-415");
  await this.searchProperties.selectNthSearchResult(0);
});

Then("The Search Control section is rendered successfully", async function () {
  await this.searchProperties.verifyPropertyQuickInfo();
});

Given("I navigate to the Map Layers Page", async function () {
  await this.mapLayers.navigateMapLayers();
});

When("I verify the Map Layers List View", async function () {
  await this.mapLayers.verifyMapLayersForm();
});

Then("The Map Layers section is rendered successfully", async function () {
  await this.mapLayers.navigateMapLayers();
});

Given("I navigate to the Worklists Page", async function () {
  await this.workLists.navigateWorkLists();
});

When("I verify the Worklist view form", async function () {
  await this.workLists.verifyWorkListForm();
});

When("I insert a property in the Worklist", async function () {
  await this.searchProperties.navigateSearchProperties();
  await this.searchProperties.searchPropertyByPID("024-229-415");
  await this.searchProperties.selectNthSearchResult(0);
  await this.searchProperties.addPropertyInfoToWorklist();
});

Then("The Worklist section is rendered successfully", async function () {
  await this.workLists.verifyWorklistWithProps();
});

Given("I navigate to the Projects Page", async function () {
  await this.projects.navigateProjectListView();
});

When("I verify the Projects List View", async function () {
  await this.projects.verifyProjectsListView();
});

When("I verify the Projects Create Form fields", async function () {
  await this.projects.navigateCreateProject();
  await this.projects.verifyCreateProjectForm();
});

Then("The Projects section is rendered successfully", async function () {
  await this.projects.cancelCreateProject();
});

Given("I navigate to the Research Files Page", async function () {
  await this.researchFiles.navigateResearchListView();
});

When("I verify the Research Files List View", async function () {
  await this.researchFiles.verifyResearchListView();
});

When("I verify the Research Files Create Form fields", async function () {
  await this.researchFiles.navigateCreateResearch();
  await this.researchFiles.verifyCreateResearchFileForm();
});

Then("The Research Files section is rendered successfully", async function () {
  await this.researchFiles.cancelCreateResearchFile();
});

Given("I navigate to the Acquisition Files Page", async function () {
  await this.acquisitionDetails.navigateToAcquisitionFileListView();
});

When("I verify the Acquisition Files List View", async function () {
  await this.acquisitionDetails.verifyAcquisitionListView();
});

When("I verify the Acquisition Files Create Form fields", async function () {
  await this.acquisitionDetails.navigateToCreateNewAcquisitionFile();
  await this.acquisitionDetails.verifyAcquisitionFileCreateForm();
});

Then(
  "The Acquisition Files section is rendered successfully",
  async function () {
    await this.acquisitionDetails.cancelCreateAcquisitionFile();
  }
);

Given("I navigate to the Management Files Page", async function () {
  await this.managementFileDetails.navigateManagementMainMenu();
  await this.managementFileDetails.navigateManagementFileListView();
});

When("I verify the Management Files List View", async function () {
  await this.searchManagementFiles.verifySearchManagementListView();
});

When("I verify the Management Files Create Form fields", async function () {
  await this.managementFileDetails.navigateManagementMainMenu();
  await this.managementFileDetails.createManagementFileLink();
  await this.managementFileDetails.validateInitManagementFileDetailsPage();
});

Then(
  "The Management Files section is rendered successfully",
  async function () {
    await this.managementFileDetails.cancelManagementFile();
  }
);

Given("I navigate to the Lease and Licences Page", async function () {
  await this.leaseLicence.navigateLeaseListView();
});

When("I verify the Lease and Licences List View", async function () {
  await this.leaseLicence.verifyLeaseListView();
});

When("I verify the Lease and Licences Create Form fields", async function () {
  await this.navigateCreateLease();
  await this.verifyCreateLeaseForm();
});

Then(
  "The Lease and Licences section is rendered successfully",
  async function () {
    await this.leaseLicence.cancelCreateLeaseFile();
  }
);

Given("I navigate to the Disposition Files Page", async function () {
  await this.dispositionFile.navigateDispositionListView();
});

When("I verify the Disposition Files List View", async function () {
  await this.dispositionFile.verifyDispositionListView();
});

When("I verify the Disposition Files Create Form fields", async function () {
  await this.dispositionFile.navigateCreateDisposition();
  await this.dispositionFile.verifyCreateDispositionForm();
});

Then(
  "The Disposition Files section is rendered successfully",
  async function () {
    await this.dispositionFile.cancelCreateDispositionFile();
  }
);

Given("I navigate to the Contact Manager Page", async function () {
  await this.contactManager.navigateContactsListView();
});

When("I verify the Contact Manager List View", async function () {
  await this.contactManager.verifyDispositionListView();
});

When("I verify the Contact Manager Create Form fields", async function () {
  await this.contactManager.navigateCreateContact();
  await this.contactManager.verifyCreateContactForm();
});

Then("The Contact Manager section is rendered successfully", async function () {
  await this.contactManager.cancelCreateContact();
});

Given("I navigate to the Admin Users Page", async function () {
  await this.adminTools.navigateAdminUsers();
});

When("I verify the Admin Users List View", async function () {
  await this.adminTools.verifyManageUsersListView();
});

When("I verify the Manage Users request list view", async function () {
  await this.adminTools.navigateAdminUserRequests();
  await this.adminTools.verifyUserRequestsListView();
});

Given("I navigate to the Admin CDOGS Templates Page", async function () {
  await this.adminTools.navigateCDOGS();
});

When("I verify the Admin CDOGS Templates List View", async function () {
  await this.adminTools.verifyCDOGSUploadPage();
});

Given("I navigate to the Admin Financial Codes Page", async function () {
  await this.adminTools.navigateFinancialCodes();
});

When("I verify the Admin Financial Codes List View", async function () {
  await this.adminTools.verifyFinancialCodesListView();
});

When(
  "I verify the Admin Financial Codes Create Form fields",
  async function () {
    await this.adminTools.verifyFinancialCodeCreateForm();
  }
);

Given("I navigate to the Admin BCFTA Ownership Page", async function () {
  await this.adminTools.navigateBCFTAOwnershipPage();
});

When("I verify Admin BCFTA Ownership Page", async function () {
  await this.adminTools.verifyBCFTAPage();
});

Then("The Admin Tools section is rendered successfully", async function () {
  await this.adminTools.navigateHome();
});
