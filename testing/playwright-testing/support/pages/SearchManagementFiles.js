const { expect } = require("@playwright/test");
const { clickAndWaitFor } = require("../../support/common.js");

class SearchManagementFiles {
  constructor(page) {
    this.page = page;
  }

  async navigateToSearchManagement() {
    clickAndWaitFor(
      this.page,
      "div[data-testid='nav-tooltip-project'] a",
      "div[data-testid='side-tray']"
    );
    clickAndWaitFor(
      this.page,
      "div[data-testid='nav-tooltip-management'] a",
      "div[data-testid='side-tray']"
    );
    await this.page
      .getByRole("link", { name: "Manage Management Files" })
      .click();
  }

  async orderByMgmtFileName() {
    await this.page.getByTestId("sort-column-fileName").click();
  }

  async orderByMgmtHistoricalFileNbr() {
    await this.page.getByTestId("sort-column-legacyFileNum").click();
  }

  async orderByMgmtPurpose() {
    await this.page
      .getByTestId("sort-column-managementFilePurposeTypeCode")
      .click();
  }

  async orderByMgmtStatus() {
    await this.page
      .getByTestId("sort-column-managementFileStatusTypeCode")
      .click();
  }

  async selectFirstOption() {
    await this.page
      .locator(
        "div[data-testid='managementFilesTable'] div[class='tr-wrapper']:first-child a"
      )
      .click();
    await expect(this.page.getByTestId("mgmt-fileId")).toBeVisible();
  }

  async firstMgmtFileName() {
    return await this.page
      .locator(
        "div[data-testid='managementFilesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div[class='td clickable']:nth-child(2)"
      )
      .textContent();
  }

  async firstMgmtHistoricalFile() {
    return await this.page
      .locator(
        "div[data-testid='managementFilesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div[class='td clickable']:nth-child(3)"
      )
      .textContent();
  }

  async firstMgmtPurpose() {
    return await this.page
      .locator(
        "div[data-testid='managementFilesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div[class='td clickable']:nth-child(5)"
      )
      .textContent();
  }

  async firstMgmtStatus() {
    return await this.page
      .locator(
        "div[data-testid='managementFilesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div[class='td clickable']:nth-child(8)"
      )
      .textContent();
  }

  async filterManagementFiles({
    pid = "",
    pin = "",
    address = "",
    mgmtfile = "",
    teamMember = "",
    status = "",
    purpose = "",
    project = "",
  }) {
    await this.page.locator("#reset-button").click();

    if (pid != "") {
      await this.page.locator("#input-searchBy").selectOption({ label: "PID" });
      await this.page.locator("#input-pid").fill(pid);
    }

    if (pin != "") {
      await this.page.locator("#input-searchBy").selectOption({ label: "PIN" });
      await this.page.locator("#input-pin").fill(pin);
    }

    if (address != "") {
      await this.page
        .locator("#input-searchBy")
        .selectOption({ label: "Address" });
      await this.page.locator("#input-address").fill(address);
    }

    if (mgmtfile != "") {
      await this.page
        .locator("#input-fileNameOrNumberOrReference")
        .fill(mgmtfile);
    }

    if (teamMember != "") {
      await this.page
        .locator("typeahead-select-managementTeamMember")
        .fill(pid);
      await this.page
        .locator(
          "div[id='typeahead-select-managementTeamMember'] a:first-child"
        )
        .click();
    }

    if (status != "") {
      await this.page
        .locator("#input-managementFileStatusCode")
        .selectOption({ label: status });
    }

    if (purpose != "") {
      await this.page
        .locator("#input-managementFilePurposeCode")
        .selectOption({ label: purpose });
    }

    if (project != "") {
      await this.page.locator("#input-projectNameOrNumber").fill(project);
    }

    await this.page.locator("#search-button").click();
  }

  async searchFoundResults() {
    return (
      (await this.page
        .locator(
          "div[data-testid='managementFilesTable'] div[class='tr-wrapper']:first-child a"
        )
        .count()) > 0
    );
  }

  async mgmtTableResultNumber() {
    return await this.page
      .locator(
        "div[data-testid='managementFilesTable'] div[class='tbody'] div[class='tr-wrapper']"
      )
      .count();
  }

  async verifySearchManagementListView() {
    //Search Management Title
    await expect(
      this.page.getByRole("h1").filter({ hasText: "Management Files" })
    ).toBeVisible();

    //Search Management Filters
    await expect(this.page.getByText("Search by:")).toBeVisible();
    await expect(this.page.locator("#input-searchBy")).toBeVisible();
    await expect(this.page.locator("#input-address")).toBeVisible();
    await expect(
      this.page.locator("#input-fileNameOrNumberOrReference")
    ).toBeVisible();
    await expect(
      this.page.locator("#typeahead-select-managementTeamMember")
    ).toBeVisible();
    await expect(
      this.page.locator("#input-managementFileStatusCode")
    ).toBeVisible();
    await expect(
      this.page.locator("#input-managementFilePurposeCode")
    ).toBeVisible();
    await expect(this.page.locator("#input-projectNameOrNumber")).toBeVisible();
    await expect(this.page.locator("#search-button")).toBeVisible();
    await expect(this.page.locator("#reset-button")).toBeVisible();

    //Search Management Column Headers
    await expect(
      this.page.locator(
        "div[data-testid='managementFilesTable'] div[class='thead thead-light'] div:nth-child(1) div[class='sortable-column']"
      )
    ).toHaveTextContent("Management file #");
    await expect(
      this.page.getByTestId("sort-column-managementFileId")
    ).toBeVisible();
    await expect(
      this.page.locator(
        "div[data-testid='managementFilesTable'] div[class='thead thead-light'] div:nth-child(2) div[class='sortable-column']"
      )
    ).toHaveTextContent("File name");
    await expect(this.page.getByTestId("sort-column-fileName")).toBeVisible();
    await expect(
      this.page.locator(
        "div[data-testid='managementFilesTable'] div[class='thead thead-light'] div:nth-child(3) div[class='sortable-column']"
      )
    ).toHaveTextContent("History File #");
    await expect(
      this.page.getByTestId("sort-column-legacyFileNum")
    ).toBeVisible();
    await expect(
      this.page.locator(
        "div[data-testid='managementFilesTable'] div[class='thead thead-light'] div:nth-child(4) div[class='sortable-column']"
      )
    ).toHaveTextContent("Project");
    await expect(
      this.page.locator(
        "div[data-testid='managementFilesTable'] div[class='thead thead-light'] div:nth-child(5) div[class='sortable-column']"
      )
    ).toHaveTextContent("Purpose");
    await expect(
      this.page.getByTestId("sort-column-managementFilePurposeTypeCode")
    ).toBeVisible();
    await expect(
      this.page.locator(
        "div[data-testid='managementFilesTable'] div[class='thead thead-light'] div:nth-child(6) div[class='sortable-column']"
      )
    ).toHaveTextContent("Team member");
    await expect(
      this.page.locator(
        "div[data-testid='managementFilesTable'] div[class='thead thead-light'] div:nth-child(7) div[class='sortable-column']"
      )
    ).toHaveTextContent("Civic Address / PID / PIN");
    await expect(
      this.page.locator(
        "div[data-testid='managementFilesTable'] div[class='thead thead-light'] div:nth-child(8) div[class='sortable-column']"
      )
    ).toHaveTextContent("Status");
    await expect(
      this.page.getByTestId("sort-column-managementFileStatusTypeCode")
    ).toBeVisible();
    AssertTrueIsDisplayed(managementListViewOrderByStatus);

    //Search Management Pagination
    await expect(this.page.locator("div[class='Menu-root']")).toBeVisible();
    await expect(this.page.locator("ul[class='pagination']")).toBeVisible();
  }

  async verifyManagementTableContent(managementFile) {
    await expect(
      this.page.locator(
        "div[data-testid='managementFilesTable'] div[class='tr-wrapper']:first-child"
      )
    ).toBeVisible();
    await expect(
      this.page.locator(
        "div[data-testid='managementFilesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div div:nth-child(2)"
      )
    ).toHaveTextContent(managementFile.ManagementName);

    if (managementFile.ManagementHistoricalFile != null) {
      await expect(
        this.page.locator(
          "div[data-testid='managementFilesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div div:nth-child(3)"
        )
      ).toHaveTextContent(managementFile.ManagementHistoricalFile);
    }

    if (managementFile.ManagementMinistryProject != null) {
      await expect(
        this.page.locator(
          "div[data-testid='managementFilesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div div:nth-child(4)"
        )
      ).toHaveTextContent(managementFile.ManagementMinistryProject);
    }

    await expect(
      this.page.locator(
        "div[data-testid='managementFilesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div div:nth-child(5)"
      )
    ).toHaveTextContent(managementFile.ManagementPurpose);

    if (managementFile.ManagementTeam.count() > 0) {
      await expect(
        this.page
          .locator(
            "div[data-testid='managementFilesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div div:nth-child(6) span"
          )
          .count()
      ).toBeMoreThan(0);
    }

    if (managementFile.ManagementSearchProperties != {}) {
      await expect(
        this.page
          .locator(
            "div[data-testid='managementFilesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div div:nth-child(7) div div"
          )
          .count()
      ).toBeMoreThan(0);
    }

    await expect(
      this.page.locator(
        "div[data-testid='managementFilesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div div:nth-child()"
      )
    ).toHaveTextContent(managementFile.ManagementStatus);
  }
}

module.exports = SearchManagementFiles;
