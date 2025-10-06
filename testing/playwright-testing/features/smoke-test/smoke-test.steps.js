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

Given("I navigate to the Map Layers Page", async function() {
  await this.mapLayers.navigateMapLayers();
});

When("I verify the Map Layers List View", async function() {
  await this.mapLayers.verifyMapLayersForm();
});

Then("The Map Layers section is rendered successfully", async function() {
  await this.mapLayers.navigateMapLayers();
});

Given ("I navigate to the Worklists Page", async function(){
  await this.workLists.navigateWorkLists();
});

When("I verify the Worklist view form", async function(){
  await this.workLists.verifyWorkListForm();
});

When("I insert a property in the Worklist", async function(){
  await this.searchProperties.navigateSearchProperties();
  await this.searchProperties.searchPropertyByPID("024-229-415");
  await this.searchProperties.selectNthSearchResult(0);
  await this.searchProperties.addPropertyInfoToWorklist();
});

Then("The Worklist section is rendered successfully", async function(){
  await this.workLists.verifyWorklistWithProps();
});

Given("I navigate to the Projects Page", async function(){
  await this.projects.navigateProjectListView();
});

When("I verify the Projects List View", async function() {
  await this.projects.verifyProjectsListView();
});

When("I verify the Projects Create Form fields", async function(){
  await this.projects.navigateCreateProject();
  await this.projects.verifyCreateProjectForm();
});

Then("The Projects section is rendered successfully", async function(){
  await this.projects.cancelCreateProject();
});

Given("I navigate to the Research Files Page", async function(){
  await this.researchFiles.navigateResearchListView();
});

When("I verify the Research Files List View", async function(){
  await this.researchFiles.verifyResearchListView();
});

When("I verify the Research Files Create Form fields", async function(){
  await this.researchFiles.navigateCreateResearch();
  await this.researchFiles.verifyCreateResearchFileForm();
});

Then("The Research Files section is rendered successfully", async function(){
  await this.researchFiles.cancelCreateResearchFile();
});

Given("I navigate to the Acquisition Files Page", async function(){
  await this.acquisitionDetails.navigateToAcquisitionFileListView();
});

When("I verify the Acquisition Files List View", async function(){
  await this.acquisitionDetails.verifyAcquisitionListView();
});

When("I verify the Acquisition Files Create Form fields", async function(){
  await this.acquisitionDetails.navigateToCreateNewAcquisitionFile();
  await this.acquisitionDetails.verifyAcquisitionFileCreateForm();
});

Then("The Acquisition Files section is rendered successfully", async function(){
  await this.acquisitionDetails.cancelCreateAcquisitionFile();
});

Given("I navigate to the Management Files Page", async function(){
  await this.managementFileDetails.navigateManagementMainMenu();
  await this.managementFileDetails.navigateManagementFileListView();
});

When("I verify the Management Files List View", async function(){
  await this.searchManagementFiles.verifySearchManagementListView();
});

When("I verify the Management Files Create Form fields", async function(){
  await this.managementFileDetails.navigateManagementMainMenu();
  await this.managementFileDetails.createManagementFileLink();
  await this.managementFileDetails.validateInitManagementFileDetailsPage();
});

Then("The Management Files section is rendered successfully", async function(){
  await this.managementFileDetails.cancelManagementFile();
});

Given("I navigate to the Lease and Licences Page", async function(){
  
});

When("I verify the Lease and Licences List View", async function(){});

When("I verify the Lease and Licences Create Form fields", async function(){});

Then("The Lease and Licences section is rendered successfully", async function(){});
