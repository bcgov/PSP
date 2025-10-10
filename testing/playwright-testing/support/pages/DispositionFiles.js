const { expect } = require("@playwright/test");
const { clickAndWaitFor } = require("../../support/common.js");

class DispositionFiles {
  constructor(page) {
    this.page = page;
  }

  async navigateCreateDisposition() {
    clickAndWaitFor(
      this.page,
      "div[data-testid='nav-tooltip-disposition'] a",
      "div[data-testid='side-tray']"
    );
    this.page.locator("a[text()='Create a Disposition File']").click();
  }

  async navigateDispositionListView() {
    clickAndWaitFor(
      this.page,
      "div[data-testid='nav-tooltip-disposition'] a",
      "div[data-testid='side-tray']"
    );
    this.page.locator("a[text()='Manage Disposition Files']").click();
  }

  async verifyCreateDispositionForm() {
    const formTitle = await this.page.getByTestId("form-title");
    expect(formTitle).toHaveText("Create Disposition File");

    await expect(
      this.page.locator("//h2/div/div[contains(text(),'Project')]")
    ).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Ministry project')]")
    ).toBeVisible();
    await expect(this.page.locator("#typeahead-project")).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Funding')]")
    ).toBeVisible();
    await expect(this.page.locator("#input-fundingTypeCode")).toBeVisible();

    await expect(
      this.page.locator("//h2/div/div[contains(text(),'Schedule')]")
    ).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Assigned date')]")
    ).toBeVisible();
    await expect(this.page.locator("#datepicker-assignedDate")).toBeVisible();

    await expect(
      this.page.locator(
        "//h2/div/div[text()='Properties to include in this file:']"
      )
    ).toBeVisible();
    await expect(
      this.page.locator(
        "//div[contains(text(),'Select one or more properties that you want to include in this disposition file. You can choose a location from the map, or search by other criteria.')]"
      )
    ).toBeVisible();
    await expect(
      this.page.locator("//h2/div/div[text()='Selected Properties']")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'New workflow')]")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Identifier')]")
    ).toBeVisible();
    await expect(
      this.page.locator(
        "//div[contains(text(),'Provide a descriptive name for this land')]"
      )
    ).toBeVisible();
    await expect(
      this.page.getByTestId("tooltip-icon-property-selector-tooltip")
    ).toBeVisible();
    await expect(
      this.page.locator("//span[contains(text(),'No Properties selected')]")
    ).toBeVisible();

    await expect(
      this.page.locator("//h2/div/div[contains(text(),'Disposition Details')]")
    ).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Disposition file name')]")
    ).toBeVisible();
    await expect(this.page.locator("#input-fileName")).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Reference number')]")
    ).toBeVisible();
    await expect(this.page.locator("#input-referenceNumber")).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Disposition status')]")
    ).toBeVisible();
    await expect(
      this.page.locator("#input-dispositionStatusTypeCode")
    ).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Disposition type')]")
    ).toBeVisible();
    await expect(this.page.locator("#input-dispositionTypeCode")).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Initiating document')]")
    ).toBeVisible();
    await expect(
      this.page.locator("#input-initiatingDocumentTypeCode")
    ).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Initiating document date')]")
    ).toBeVisible();
    await expect(
      this.page.locator("#datepicker-initiatingDocumentDate")
    ).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Physical file status')]")
    ).toBeVisible();
    await expect(
      this.page.locator("#input-physicalFileStatusTypeCode")
    ).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Initiating branch')]")
    ).toBeVisible();
    await expect(
      this.page.locator("#input-initiatingBranchTypeCode")
    ).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Ministry region')]")
    ).toBeVisible();
    await expect(this.page.locator("#input-regionCode")).toBeVisible();

    await expect(
      this.page.locator("//h2/div/div[contains(text(),'Disposition Team')]")
    ).toBeVisible();
    await expect(this.page.getByTestId("add-team-member")).toBeVisible();

    const tooltips = await this.page.getByTestId(
      "tooltip-icon-lease-status-tooltip"
    );
    expect(tooltips).toHaveCount(3);

    await expect(this.page.getByTestId("cancel-button")).toBeVisible();
    await expect(this.page.getByTestId("save-button")).toBeVisible();
  }

  async verifyDispositionListView() {
    await expect(
      this.page.locator("//span[text()='Dipsosition Files']")
    ).toBeVisible();
    await expect(this.page.locator("h1 button")).toBeVisible();

    await expect(
      this.page.locator("//string[text()='Search by:']")
    ).toBeVisible();
    await expect(this.page.locator("#input-searchBy")).toBeVisible();
    await expect(this.page.locator("#input-address")).toBeVisible();
    await expect(
      this.page.locator("#typeahead-select-dispositionTeamMember")
    ).toBeVisible();
    await expect(
      this.page.locator("#input-fileNameOrNumberOrReference")
    ).toBeVisible();
    await expect(
      this.page.locator("#input-dispositionStatusCode")
    ).toBeVisible();
    await expect(this.page.locator("#input-dispositionTypeCode")).toBeVisible();
    await expect(this.page.locator("#search-button")).toBeVisible();
    await expect(this.page.locator("#reset-button")).toBeVisible();

    await expect(
      this.page.locator("//div[contains(text(),'Disposition file #')]")
    ).toBeVisible();
    await expect(this.page.getByTestId("sort-column-fileNumber")).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Reference')]")
    ).toBeVisible();
    await expect(
      this.page.getByTestId("sort-column-fileReference")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Disposition file name')]")
    ).toBeVisible();
    await expect(this.page.getByTestId("sort-column-fileName")).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Disposition type')]")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'MOTT region')]")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Team member')]")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Civic Address / PID / PIN')]")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Disposition status')]")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Status')]")
    ).toBeVisible();
    await expect(
      this.page.getByTestId("sort-column-dispositionFileStatusTypeCode")
    ).toBeVisible();

    const dispositionFileCount = await this.page.locator(
      "div[data-testid='dispositionFilesTable'] div[class='tbody'] div[class='tr-wrapper']"
    );
    expect(dispositionFileCount).toBeGreaterThan(0);

    await expect(this.page.getByTestId("input-page-size")).toBeVisible();
    await expect(this.page.locator("ul[class='pagination']")).toBeVisible();
  }

  async cancelCreateDispositionFile() {
    await this.page
      .locator("//div[contains(text(),'Cancel')]/parent::button")
      .click();
  }
}

module.exports = DispositionFiles;
