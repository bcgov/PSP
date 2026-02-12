const { expect } = require("@playwright/test");
const { clickAndWaitFor } = require("../common.js");

class ManagementFile {
  constructor(page) {
    this.page = page;
  }

  async navigateManagementMainMenu() {
    clickAndWaitFor(
      this.page,
      "div[data-testid='nav-tooltip-management'] a",
      "div[data-testid='side-tray']"
    );
  }

  async createManagementFileLink() {
    await this.page
      .locator("//a[contains(text(), 'Create Management File')]")
      .click();
  }

  async cancelManagementFile() {
    await this.page
      .locator("//div[contains(text(),'Cancel')]/parent::button")
      .click();
  }

  async createMinimumManagementDetails(mgmtFile) {
    //Management File Name
    const managementFileNameInput = await this.page.locator("#input-fileName");
    expect(managementFileNameInput).toBeVisible();
    await this.page(managementFileNameInput).fill(mgmtFile.ManagementName);

    //Purpose
    const managementFilePurposeSelect = await this.page.locator(
      "#input-purposeTypeCode"
    );
    expect(managementFilePurposeSelect).toBeVisible();
    await this.page(managementFilePurposeSelect).selectOption({
      label: mgmtFile.ManagementPurpose,
    });
  }

  async verifyInitManagementFileDetailsPage() {
    await this.page
      .getByRole("h1", { name: "Create Management File" })
      .isVisible();

    //Project
    await expect(this.page.locator('h2:has-text("Project")')).toBeVisible();
    await expect(
      this.page.locator("label:has-text('Ministry project')")
    ).toBeVisible();
    await expect(this.page.getByTestId("typeahead-project")).toBeVisible();
    await expect(
      this.page.locator("label", { hasText: "Funding" })
    ).toBeVisible();

    const fundingSelect = await this.page.locator("#input-fundingTypeCode");
    expect(fundingSelect).toBeVisible();

    const fundingOptions = await fundingSelect.locator("option").count();
    expect(fundingOptions).toBeGreaterThan(0);

    //Properties
    await expect(
      this.page.locator(
        "//h2/div/div[text()='Properties to include in this file:']"
      )
    ).toBeVisible();
    await expect(
      this.page.locator(
        "//div[contains(text(),'Select one or more properties that you want to include in this management file. You can choose a location from the map, or search by other criteria.')]"
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
      this.page.locator("//span[contains(text(),'No Properties selected')]")
    ).toBeVisible();

    //Management Details
    await expect(
      this.page.locator('h2:has-text("Management Details")')
    ).toBeVisible();
    await expect(this.page.getByText("File name")).toBeVisible();
    await expect(this.page.locator("#input-fileName")).toBeVisible();
    await expect(this.page.getByText("Historical file number")).toBeVisible();
    await expect(this.page.locator("#input-legacyFileNum")).toBeVisible();
    await expect(this.page.getByText("Purpose")).toBeVisible();
    await expect(this.page.locator("#input-purposeTypeCode")).toBeVisible();
    await expect(this.page.getByText("Additional details")).toBeVisible();
    await expect(this.page.locator("#input-additionalDetails")).toBeVisible();

    //Management Team
    await expect(
      this.page.locator('h2:has-text("Management Team")')
    ).toBeVisible();
    await expect(this.page.getByTestId("add-team-member")).toBeVisible();
  }

  async navigateToSearchManagement() {
    await clickAndWaitFor(
      this.page,
      "div[data-testid='nav-tooltip-project'] a",
      "div[data-testid='side-tray']"
    );
    await clickAndWaitFor(
      this.page,
      "div[data-testid='nav-tooltip-management'] a",
      "div[data-testid='side-tray']"
    );

    await this.page
      .getByRole("link", { name: "Manage Management Files" })
      .waitFor({ state: "visible" });
    await this.page
      .getByRole("link", { name: "Manage Management Files" })
      .click();
  }

  async navigateToSearchActivitiesManagement() {
    await clickAndWaitFor(
      this.page,
      "div[data-testid='nav-tooltip-project'] a",
      "div[data-testid='side-tray']"
    );
    await clickAndWaitFor(
      this.page,
      "div[data-testid='nav-tooltip-management'] a",
      "div[data-testid='side-tray']"
    );

    await this.page
      .getByRole("link", { name: "Manage Management Activities" })
      .waitFor({ state: "visible" });
    await this.page
      .getByRole("link", { name: "Manage Management Activities" })
      .click();
  }

  async verifySearchManagementListView() {
    //Search Management Title
    await this.page
      .locator("h1 span")
      .filter({ hasText: "Management Files" })
      .waitFor({ state: "visible" });
    expect(
      this.page.locator("h1 span").filter({ hasText: "Management Files" })
    ).toBeVisible();

    //Search Management Filters
    await this.page.locator("text=Search by:").waitFor({ state: "visible" });
    expect(this.page.locator("text=Search by:")).toBeVisible();

    await this.page.locator("#input-searchBy").waitFor({ state: "visible" });
    expect(this.page.locator("#input-searchBy")).toBeVisible();

    await this.page.locator("#input-address").waitFor({ state: "visible" });
    expect(this.page.locator("#input-address")).toBeVisible();

    await this.page
      .locator("#input-fileNameOrNumberOrReference")
      .waitFor({ state: "visible" });
    expect(
      this.page.locator("#input-fileNameOrNumberOrReference")
    ).toBeVisible();

    await this.page
      .locator("#typeahead-select-managementTeamMember")
      .waitFor({ state: "visible" });
    expect(
      this.page.locator("#typeahead-select-managementTeamMember")
    ).toBeVisible();

    await this.page
      .locator("#input-managementFileStatusCode")
      .waitFor({ status: "visible" });
    expect(this.page.locator("#input-managementFileStatusCode")).toBeVisible();

    await this.page
      .locator("#input-managementFilePurposeCode")
      .waitFor({ status: "visible" });
    expect(this.page.locator("#input-managementFilePurposeCode")).toBeVisible();

    await this.page
      .locator("#input-projectNameOrNumber")
      .waitFor({ status: "visible" });
    expect(this.page.locator("#input-projectNameOrNumber")).toBeVisible();

    await this.page.locator("#search-button").waitFor({ status: "visible" });
    expect(this.page.locator("#search-button")).toBeVisible();

    await this.page.locator("#reset-button").waitFor({ status: "visible" });
    expect(this.page.locator("#reset-button")).toBeVisible();

    //Search Management Column Headers
    const managementFileNumberColumn = await this.page
      .locator(
        "div[data-testid='managementFilesTable'] div[class='thead thead-light'] div:nth-child(1) div[class='sortable-column']"
      )
      .getByText("Management file #");
    expect(managementFileNumberColumn).toBeVisible();

    await this.page
      .getByTestId("sort-column-managementFileId")
      .waitFor({ status: "visible" });
    expect(this.page.getByTestId("sort-column-managementFileId")).toBeVisible();

    const fileNameColumn = await this.page.locator(
      "div[data-testid='managementFilesTable'] div[class='thead thead-light'] div:nth-child(2) div[class='sortable-column']"
    );
    expect(fileNameColumn).toHaveText("File name");

     await this.page
      .getByTestId("sort-column-fileName")
      .waitFor({ status: "visible" });
    expect(this.page.getByTestId("sort-column-fileName")).toBeVisible();

    const MOTTFileColumn = await this.page.locator(
      "div[data-testid='managementFilesTable'] div[class='thead thead-light'] div:nth-child(3) div[class='sortable-column']"
    );
    expect(MOTTFileColumn).toHaveText("MOTT region");

    await this.page
      .getByTestId("sort-column-fileName")
      .waitFor({ status: "visible" });
    expect(this.page.getByTestId("sort-column-fileName")).toBeVisible();

    const historyFileColumn = await this.page.locator(
      "div[data-testid='managementFilesTable'] div[class='thead thead-light'] div:nth-child(4) div[class='sortable-column']"
    );
    expect(historyFileColumn).toHaveText("Historical File #");

    await this.page
      .getByTestId("sort-column-legacyFileNum")
      .waitFor({ status: "visible" });
    expect(this.page.getByTestId("sort-column-legacyFileNum")).toBeVisible();

    const projectColumn = await this.page.locator(
      "div[data-testid='managementFilesTable'] div[class='thead thead-light'] div:nth-child(4) div[class='sortable-column']"
    );
    expect(projectColumn).toHaveText("Project");

    const purposeColumn = await this.page.locator(
      "div[data-testid='managementFilesTable'] div[class='thead thead-light'] div:nth-child(5) div[class='sortable-column']"
    );
    expect(purposeColumn).toHaveText("Purpose");

    await this.page
      .getByTestId("sort-column-managementFilePurposeTypeCode")
      .waitFor({ status: "visible" });
    expect(
      this.page.getByTestId("sort-column-managementFilePurposeTypeCode")
    ).toBeVisible();

    const teamMemberColumn = await this.page.locator(
      "div[data-testid='managementFilesTable'] div[class='thead thead-light'] div:nth-child(7) div[class='sortable-column']"
    );
    expect(teamMemberColumn).toHaveText("Team member");

    const civicAddressColumn = await this.page.locator(
      "div[data-testid='managementFilesTable'] div[class='thead thead-light'] div:nth-child(8) div[class='sortable-column']"
    );
    expect(civicAddressColumn).toHaveText("Civic Address / PID / PIN");

    const statusColumn = await this.page.locator(
      "div[data-testid='managementFilesTable'] div[class='thead thead-light'] div:nth-child(9) div[class='sortable-column']"
    );
    expect(statusColumn).toHaveText("Status");

    await this.page
      .getByTestId("sort-column-managementFileStatusTypeCode")
      .waitFor({ status: "visible" });
    expect(
      this.page.getByTestId("sort-column-managementFileStatusTypeCode")
    ).toBeVisible();

    //Search Management Pagination
    await this.page
      .locator("div[class='Menu-root']")
      .waitFor({ status: "visible" });
    expect(this.page.locator("div[class='Menu-root']")).toBeVisible();

    await this.page
      .locator("ul[class='pagination']")
      .waitFor({ status: "visible" });
    expect(this.page.locator("ul[class='pagination']")).toBeVisible();
  }

  async verifySearchManagementActivitiesListView() {
    //Search Management Activities Title
    await this.page
      .locator("h1 span")
      .filter({ hasText: "Management Activities" })
      .waitFor({ state: "visible" });
    expect(
      this.page.locator("h1 span").filter({ hasText: "Management Activities" })
    ).toBeVisible();

    //Search Management Filters
    await this.page.locator("text=Search by:").waitFor({ state: "visible" });
    expect(this.page.locator("text=Search by:")).toBeVisible();

    await this.page.locator("#input-searchBy").waitFor({ state: "visible" });
    expect(this.page.locator("#input-searchBy")).toBeVisible();

    await this.page.locator("#input-address").waitFor({ state: "visible" });
    expect(this.page.locator("#input-address")).toBeVisible();

    await this.page
      .locator("#input-fileNameOrNumberOrReference")
      .waitFor({ state: "visible" });
    expect(
      this.page.locator("#input-fileNameOrNumberOrReference")
    ).toBeVisible();

    await this.page
      .locator("#input-activityStatusCode")
      .waitFor({ state: "visible" });
    expect(this.page.locator("#input-activityStatusCode")).toBeVisible();

    await this.page
      .locator("#input-activityTypeCode")
      .waitFor({ status: "visible" });
    expect(this.page.locator("#input-activityTypeCode")).toBeVisible();

    await this.page
      .locator("#input-projectNameOrNumber")
      .waitFor({ status: "visible" });
    expect(this.page.locator("#input-projectNameOrNumber")).toBeVisible();

    await this.page
      .locator("#input-managementFileStatusCode")
      .waitFor({ status: "visible" });
    expect(this.page.locator("#input-managementFileStatusCode")).toBeVisible();

    await this.page
      .locator("#input-managementFilePurposeCode")
      .waitFor({ status: "visible" });
    expect(this.page.locator("#input-managementFilePurposeCode")).toBeVisible();

    await this.page.locator("#search-button").waitFor({ status: "visible" });
    expect(this.page.locator("#search-button")).toBeVisible();

    await this.page.locator("#reset-button").waitFor({ status: "visible" });
    expect(this.page.locator("#reset-button")).toBeVisible();

    await this.page
      .getByTestId("excel-icon-overview")
      .waitFor({ status: "visible" });
    expect(this.page.getByTestId("excel-icon-overview")).toBeVisible();

    await this.page
      .locator("//span[text()='Activity overview']")
      .waitFor({ status: "visible" });
    expect(
      this.page.locator("//span[text()='Activity overview']")
    ).toBeVisible();

    await this.page
      .getByTestId("excel-icon-invoices")
      .waitFor({ status: "visible" });
    expect(this.page.getByTestId("excel-icon-invoices")).toBeVisible();

    await this.page
      .locator("//span[text()='Invoice report']")
      .waitFor({ status: "visible" });
    expect(this.page.locator("//span[text()='Invoice report']")).toBeVisible();

    //Search Management Column Headers
    const managementActDescriptionColumn = await this.page
      .locator(
        "div[data-testid='managementActivitiesTable'] div[class='thead thead-light'] div:nth-child(1) div[class='sortable-column']"
      )
      .getByText("Description");
    expect(managementActDescriptionColumn).toBeVisible();

    await this.page
      .getByTestId("sort-column-description")
      .waitFor({ status: "visible" });
    expect(this.page.getByTestId("sort-column-description")).toBeVisible();

    const fileNameColumn = await this.page.locator(
      "div[data-testid='managementActivitiesTable'] div[class='thead thead-light'] div:nth-child(2) div[class='sortable-column']"
    );
    expect(fileNameColumn).toHaveText("File name");

    await this.page
      .getByTestId("sort-column-fileName")
      .waitFor({ status: "visible" });
    expect(this.page.getByTestId("sort-column-fileName")).toBeVisible();

    const historyFileColumn = await this.page.locator(
      "div[data-testid='managementActivitiesTable'] div[class='thead thead-light'] div:nth-child(3) div[class='sortable-column']"
    );
    expect(historyFileColumn).toHaveText("Historical File #");

    await this.page
      .getByTestId("sort-column-legacyFileNum")
      .waitFor({ status: "visible" });
    expect(this.page.getByTestId("sort-column-legacyFileNum")).toBeVisible();

    const addressColumn = await this.page.locator(
      "div[data-testid='managementActivitiesTable'] div[class='thead thead-light'] div:nth-child(4) div[class='sortable-column']"
    );
    expect(addressColumn).toHaveText("Civic Address / PID / PIN");

    const typeColumn = await this.page.locator(
      "div[data-testid='managementActivitiesTable'] div[class='thead thead-light'] div:nth-child(5) div[class='sortable-column']"
    );
    expect(typeColumn).toHaveText("Type");

    await this.page
      .getByTestId("sort-column-activityType")
      .waitFor({ status: "visible" });
    expect(this.page.getByTestId("sort-column-activityType")).toBeVisible();

    const subtypeColumn = await this.page.locator(
      "div[data-testid='managementActivitiesTable'] div[class='thead thead-light'] div:nth-child(6) div[class='sortable-column']"
    );
    expect(subtypeColumn).toHaveText("Sub-type");

    const statusColumn = await this.page.locator(
      "div[data-testid='managementActivitiesTable'] div[class='thead thead-light'] div:nth-child(7) div[class='sortable-column']"
    );
    expect(statusColumn).toHaveText("Status");

    await this.page
      .getByTestId("sort-column-activityStatus")
      .waitFor({ status: "visible" });
    expect(this.page.getByTestId("sort-column-activityStatus")).toBeVisible();

    //Search Management Pagination
    await this.page
      .locator("div[class='Menu-root']")
      .waitFor({ status: "visible" });
    expect(this.page.locator("div[class='Menu-root']")).toBeVisible();

    await this.page
      .locator("ul[class='pagination']")
      .waitFor({ status: "visible" });
    expect(this.page.locator("ul[class='pagination']")).toBeVisible();
  }
}
module.exports = ManagementFile;
