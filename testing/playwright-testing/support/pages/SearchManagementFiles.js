const { expect } = require("@playwright/test");
const { clickAndWaitFor } = require("../../support/common.js");

class SearchManagementFiles {
  constructor(page) {
    this.page = page;
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
    await this.page.locator(
      "div[data-testid='managementFilesTable'] div[class='tbody'] div[class='tr-wrapper']"
    );
    const tableNumbers = await this.page
      .locator(
        "div[data-testid='managementFilesTable'] div[class='tbody'] div[class='tr-wrapper']"
      )
      .count();
    return tableNumbers;
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

    const historyFileColumn = await this.page.locator(
      "div[data-testid='managementFilesTable'] div[class='thead thead-light'] div:nth-child(3) div[class='sortable-column']"
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
      "div[data-testid='managementFilesTable'] div[class='thead thead-light'] div:nth-child(6) div[class='sortable-column']"
    );
    expect(teamMemberColumn).toHaveText("Team member");

    const civicAddressColumn = await this.page.locator(
      "div[data-testid='managementFilesTable'] div[class='thead thead-light'] div:nth-child(7) div[class='sortable-column']"
    );
    expect(civicAddressColumn).toHaveText("Civic Address / PID / PIN");

    const statusColumn = await this.page.locator(
      "div[data-testid='managementFilesTable'] div[class='thead thead-light'] div:nth-child(8) div[class='sortable-column']"
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

  async verifyManagementTableContent(managementFile) {
    await this.page
      .locator(
        "div[data-testid='managementFilesTable'] div[class='tr-wrapper']:first-child"
      )
      .waitFor({ status: "visible" });
    expect(
      this.page.locator(
        "div[data-testid='managementFilesTable'] div[class='tr-wrapper']:first-child"
      )
    ).toBeVisible();

    const managementNameContent = await this.page.locator(
      "div[data-testid='managementFilesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div div:nth-child(2)"
    );
    expect(managementNameContent).toHaveText(managementFile.ManagementName);

    if (managementFile.ManagementHistoricalFile != null) {
      const historicalFileContent = await this.page.locator(
        "div[data-testid='managementFilesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div div:nth-child(3)"
      );
      expect(historicalFileContent).toHaveText(
        managementFile.ManagementHistoricalFile
      );
    }

    if (managementFile.ManagementMinistryProject != null) {
      const ministryProjectContent = await this.page.locator(
        "div[data-testid='managementFilesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div div:nth-child(4)"
      );
      await expect(ministryProjectContent).toHaveText(
        managementFile.ManagementMinistryProjectCode +
          " " +
          managementFile.ManagementMinistryProject
      );
    }

    const managementPurposeContent = await this.page.locator(
      "div[data-testid='managementFilesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div div:nth-child(5)"
    );
    await expect(managementPurposeContent).toHaveText(
      managementFile.ManagementPurpose
    );

    if (managementFile.ManagementTeam.length > 0) {
      await this.page
        .locator(
          "div[data-testid='managementFilesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div div:nth-child(6) span"
        )
        .waitFor({ status: "visible" });
      const managementTeamCount = await this.page
        .locator(
          "div[data-testid='managementFilesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div div:nth-child(6) span"
        )
        .count();
      expect(managementTeamCount).toBeGreaterThan(0);
    }

    // if (managementFile.ManagementSearchProperties != {}) {
    //   await this.page.locator("div[data-testid='managementFilesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div div:nth-child(7) div div").waitFor({status: 'visible'});
    //   const propertiesCount = await this.page.locator("div[data-testid='managementFilesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div div:nth-child(7) div div").count()
    //   expect(propertiesCount).toBeGreaterThan(0);
    // }

    const managementStatus = await this.page.locator(
      "div[data-testid='managementFilesTable'] div[class='tbody'] div[class='tr-wrapper']:first-child div div:nth-child(8)"
    );
    expect(managementStatus).toHaveText(managementFile.ManagementStatus);
  }
}

module.exports = SearchManagementFiles;
