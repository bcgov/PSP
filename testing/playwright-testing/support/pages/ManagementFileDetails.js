const SharedFileProperties = require("./SharedFileProperties");
const SharedTeamMembers = require("./SharedTeamMembers");
const SharedModal = require("./SharedModal");
const { expect } = require("@playwright/test");
const {
  clickSaveButton,
  clickAndWaitFor,
  fillTypeahead,
} = require("../../support/common.js");

class ManagementFileDetails {
  constructor(page) {
    this.page = page;
    this.sharedFileProperties = new SharedFileProperties(page);
    this.sharedTeamMembers = new SharedTeamMembers(page);
    this.sharedModal = new SharedModal(page);
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

  async navigateManagementFileListView() {
    await this.page
      .locator("//a[contains(text(), 'Manage Management File')]")
      .click();
  }

  async navigateManagementFileListActivities() {
    await this.page
      .locator("//a[contains(text(), 'Manage Management Activities')]")
      .click();
  }

  async updateManagementFileButton() {
    await this.page.locator("button[title='Edit management file']").click();
  }

  async saveManagementFile() {
    clickSaveButton(this.page);

    while (
      (await this.page.locator("div[class='modal-content']").count()) > 0
    ) {
      const header = await this.sharedModal.mainModalHeader();
      const content = await this.sharedModal.mainModalContent();

      if (
        content.includes(
          "The selected property already exists in the system's inventory"
        )
      ) {
        expect(header).toBe("User Override Required");
        expect(content).toContain(
          "The selected property already exists in the system's inventory. However, the record is missing spatial details."
        );
        expect(content).toContain(
          "To add the property, the spatial details for this property will need to be updated..."
        );
        await this.sharedModal.mainModalClickOKBttn();
      } else if (
        content.includes(
          "You have made changes to the properties in this file."
        )
      ) {
        expect(header).toBe("Confirm changes");
        expect(content).toContain(
          "You have made changes to the properties in this file."
        );
        expect(content).toContain("Do you want to save these changes?");
        await this.sharedModal.mainModalClickOKBttn();
      } else if (header === "Confirm status change") {
        expect(header).toBe("Confirm status change");
        const paragraph1 = await this.sharedModal.confirmationModalParagraph1();
        const paragraph2 = await this.sharedModal.confirmationModalParagraph2();
        expect(paragraph1).toContain(
          "If you save it, only the administrator can turn it back on..."
        );
        expect(paragraph2).toBe("Do you want to acknowledge and proceed?");
        await this.sharedModal.mainModalClickOKBttn();
      } else {
        break;
      }
    }
  }

  async createMinimumManagementFileDetails(managementFile) {
    await this.page
      .locator("#input-fileName")
      .fill(managementFile.ManagementName);
    await this.page
      .locator("#input-purposeTypeCode")
      .selectOption({ label: managementFile.ManagementPurpose });
  }

  async updateManagementFileDetails(managementFile) {
    // Status
    if (managementFile.ManagementStatus !== null) {
      await this.page
        .locator("#input-fileStatusTypeCode")
        .selectOption({ label: managementFile.ManagementStatus });
    }

    //Project
    if (managementFile.ManagementMinistryProject !== null) {
      fillTypeahead(
        this.page,
        "input[id='typeahead-project']",
        managementFile.ManagementMinistryProject,
        "div[id='typeahead-project']"
      );
    }

    // Product
    if (managementFile.ManagementMinistryProduct !== null) {
      await this.page
        .locator("#input-productId")
        .selectOption({ label: managementFile.ManagementMinistryProduct });
    }

    // Funding
    if (managementFile.ManagementMinistryFunding !== null) {
      await this.page
        .locator("#input-fundingTypeCode")
        .selectOption({ label: managementFile.ManagementMinistryFunding });
    }

    // MANAGEMENT DETAILS
    // File Name
    if (managementFile.ManagementName !== null) {
      await this.page
        .locator("#input-fileName")
        .fill(managementFile.ManagementName);
    }

    // Historical File Number
    if (managementFile.ManagementHistoricalFile !== null) {
      await this.page
        .locator("#input-legacyFileNum")
        .fill(managementFile.ManagementHistoricalFile);
    }

    // Purpose
    if (managementFile.ManagementPurpose !== null) {
      await this.page
        .locator("#input-purposeTypeCode")
        .selectOption({ label: managementFile.ManagementPurpose });
    }

    // Additional Details
    if (managementFile.ManagementAdditionalDetails !== null) {
      await this.page
        .locator("#input-additionalDetails")
        .fill(managementFile.ManagementAdditionalDetails);
    }

    // MANAGEMENT TEAM
    if (managementFile.ManagementTeam.length > 0) {
      // Delete all existing members
      while (
        (await this.page
          .locator(
            "//div[contains(text(),'Management Team')]/parent::div/parent::h2/following-sibling::div/div"
          )
          .count()) > 0
      ) {
        await this.sharedTeamMembers.deleteFirstStaffMember();
      }

      // Add each team member
      for (let i = 0; i < managementFile.ManagementTeam.length; i++) {
        await this.sharedTeamMembers.addMgmtTeamMembers(
          managementFile.ManagementTeam[i]
        );
      }
    }
  }

  async getManagementFileCode() {
    const locator = await this.page.getByTestId("mgmt-fileId");
    await expect(locator).toBeVisible({ timeout: 10000 });
    return await locator.textContent();
  }

  async validateInitManagementFileDetailsPage() {
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
    await expect(this.page.locator("#input-fundingTypeCode")).toBeVisible();

    //Properties
    await this.sharedFileProperties.verifyPropertiesToIncludeInFileInitForm();

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

  async validateManagementUpdateForm() {
    // Title
    const updateManagementTitle = await this.page
      .getByTestId("form-title")
      .textContent();
    expect(updateManagementTitle).toEqual("Update Management File");

    // Status
    await expect(
      this.page.locator("label", { hasText: "Status" })
    ).toBeVisible();
    await expect(this.page.locator("#input-fileStatusTypeCode")).toBeVisible();

    // Project
    await expect(this.page.locator('h2:has-text("Project")')).toBeVisible();
    await expect(
      this.page.locator("label:has-text('Ministry project')").nth(1)
    ).toBeVisible();
    await expect(this.page.locator("#typeahead-project")).toBeVisible();
    await expect(this.page.locator("label:has-text('Funding')")).toBeVisible();
    await expect(this.page.locator("#input-fundingTypeCode")).toBeVisible();

    // Management Details
    await expect(
      this.page.locator('h2:has-text("Management Details")')
    ).toBeVisible();
    await expect(
      this.page.locator("label:has-text('File name')").nth(1)
    ).toBeVisible();
    await expect(this.page.locator("#input-fileName")).toBeVisible();
    await expect(
      this.page.locator("label:has-text('Historical file number')")
    ).toBeVisible();
    await expect(this.page.locator("#input-legacyFileNum")).toBeVisible();
    await expect(this.page.locator("label:has-text('Purpose')")).toBeVisible();
    await expect(this.page.locator("#input-purposeTypeCode")).toBeVisible();
    await expect(
      this.page.locator("label:has-text('Additional details')")
    ).toBeVisible();
    await expect(this.page.locator("#input-additionalDetails")).toBeVisible();

    // Management Team
    await expect(
      this.page.locator('h2:has-text("Management Team")')
    ).toBeVisible();
    await expect(this.page.getByTestId("add-team-member")).toBeVisible();
  }

  async validateManagementDetailsViewForm(managementFile) {
    expect(this.page.locator("h1:has-text('Management File')")).toBeVisible();

    //Status
    expect(this.page.locator("label:has-text('Status')")).toBeVisible();
    const actualStatus =
      (
        await this.page.getByTestId("management-status").textContent()
      )?.trim() || "";
    const expectedStatus = managementFile.ManagementStatus?.trim() || "";
    expect(actualStatus).toEqual(expectedStatus);

    //Project
    expect(this.page.locator("h2:has-text('Project')")).toBeVisible();

    expect(
      this.page.locator("label:has-text('Ministry project')").nth(1)
    ).toBeVisible();
    const actualProject =
      (
        await this.page.getByTestId("management-project").textContent()
      )?.trim() || "";
    const expectedProject =
      (
        managementFile.ManagementMinistryProjectCode +
        " - " +
        managementFile.ManagementMinistryProject
      )?.trim() || "";
    expect(actualProject).toEqual(expectedProject);

    expect(this.page.getByText("Product:", { exact: true })).toBeVisible();
    const actualProduct =
      (
        await this.page.getByTestId("management-product").textContent()
      )?.trim() || "";
    const expectedProduct =
      managementFile.ManagementMinistryProduct?.trim() || "";
    expect(actualProduct).toEqual(expectedProduct);

    expect(this.page.locator("label:has-text('Funding')")).toBeVisible();
    const actualFunding =
      (
        await this.page.getByTestId("management-funding").textContent()
      )?.trim() || "";
    const expectedFunding =
      managementFile.ManagementMinistryFunding?.trim() || "";
    expect(actualFunding).toEqual(expectedFunding);

    //Management Details
    expect(
      this.page.locator("label:has-text('Management file name')")
    ).toBeVisible();
    const actualName =
      (
        await this.page.getByTestId("management-file-name").textContent()
      )?.trim() || "";
    const expectedName = managementFile.ManagementName?.trim() || "";
    expect(actualName).toEqual(expectedName);

    expect(
      this.page.locator("label:has-text('Historical file number')")
    ).toBeVisible();
    const actualHistorical =
      (
        await this.page
          .getByTestId("management-legacy-file-number")
          .textContent()
      )?.trim() || "";
    const expectedHistorical =
      managementFile.ManagementHistoricalFile?.trim() || "";
    expect(actualHistorical).toEqual(expectedHistorical);

    expect(this.page.locator("label:has-text('Purpose')")).toBeVisible();
    const actualPurpose =
      (
        await this.page.getByTestId("management-purpose").textContent()
      )?.trim() || "";
    const expectedPurpose = managementFile.ManagementPurpose?.trim() || "";
    expect(actualPurpose).toEqual(expectedPurpose);

    expect(
      this.page.locator("label:has-text('Additional details')")
    ).toBeVisible();
    const actualDetails =
      (
        await this.page
          .getByTestId("management-additional-details")
          .textContent()
      )?.trim() || "";
    const expectedDetails =
      managementFile.ManagementAdditionalDetails?.trim() || "";
    expect(actualDetails).toEqual(expectedDetails);

    //Management Team
    // if (managementFile.ManagementTeam.length > 0) {
    //   while (
    //     webDriver.FindElements(managementFileViewTeamMembersGroup).Count > 0
    //   ) {
    //     sharedTeamMembers.DeleteFirstStaffMember();
    //   }

    //   for (var i = 0; i < mgmtFile.ManagementTeam.Count; i++) {
    //     sharedTeamMembers.AddMgmtTeamMembers(mgmtFile.ManagementTeam[i]);
    //   }
    // }
  }
}

module.exports = ManagementFileDetails;
