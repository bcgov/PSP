const { SharedFileProperties } = require("./SharedFileProperties");
const { SharedTeamMembers } = require("./SharedTeamMembers");
const { SharedModal } = require("./SharedModal");
const { expect } = require("@playwright/test");

class ManagementFileDetails {
  constructor(page) {
    this.page = page;
    this.sharedFileProperties = new SharedFileProperties(page);
    this.sharedTeamMembers = new SharedTeamMembers(page);
    this.sharedModal = new SharedModal(page);
  }
  async navigateManagementMainMenu() {
    await this.page
      .locator('div[data-testid="nav-tooltip-management"] a')
      .click();
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

  async updateManagementFileButtonClick() {
    await this.page.locator("button[title='Edit management file']").click();
  }

  async saveManagementFile() {
    while ((await this.managementFileConfirmationModal.count()) > 0) {
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

  async createMinimumManagementFileDetails(data) {
    await this.page.locator("#input-fileName").fill(data.ManagementName);
    await this.page
      .locator("#input-purposeTypeCode")
      .selectOption({ label: data.ManagementPurpose });
  }

  async updateManagementFileDetails(data) {
    // Status
    await expect(
      page.locator('label[for="managementFileStatusInput"]')
    ).toBeVisible();
    if (data.ManagementStatus !== null) {
      await page
        .locator("#managementFileStatusInput")
        .selectOption({ label: data.ManagementStatus });
    }

    // Project
    await expect(
      page.locator('label[for="managementFileProjectInput"]')
    ).toBeVisible();
    if (mgmtFile.ManagementMinistryProject !== null) {
      const input = page.locator("#managementFileProjectInput");
      await input.type(data.ManagementMinistryProject);
      await input.press("Enter");
      //await input.press("Backspace");
      //await page.waitForTimeout(500); // Replace with better wait, if possible
      await page.locator("#managementFileProject1stOption").click();
    }

    // Product
    await expect(
      page.locator('label[for="managementFileProjectProductSelect"]')
    ).toBeVisible();
    if (data.ManagementMinistryProduct !== null) {
      await page
        .locator("#managementFileProjectProductSelect")
        .selectOption({ label: data.ManagementMinistryProduct });
    }

    // Funding
    await expect(
      page.locator('label[for="managementFileProjectFundingInput"]')
    ).toBeVisible();
    if (data.ManagementMinistryFunding !== null) {
      await page
        .locator("#managementFileProjectFundingInput")
        .selectOption({ label: data.ManagementMinistryFunding });
    }

    // MANAGEMENT DETAILS
    // File Name
    await expect(
      page.locator('label[for="managementFileNameInput"]')
    ).toBeVisible();
    if (data.ManagementName !== null) {
      await page.locator("#managementFileNameInput").fill(data.ManagementName);
    }

    // Historical File Number
    await expect(
      page.locator('label[for="managementFileHistoricalFileInput"]')
    ).toBeVisible();
    if (data.ManagementHistoricalFile !== null) {
      await page
        .locator("#managementFileHistoricalFileInput")
        .fill(data.ManagementHistoricalFile);
    }

    // Purpose
    await expect(
      page.locator('label[for="managementFilePurposeSelect"]')
    ).toBeVisible();
    if (data.ManagementPurpose !== null) {
      await page
        .locator("#managementFilePurposeSelect")
        .selectOption({ label: data.ManagementPurpose });
    }

    // Additional Details
    await expect(
      page.locator('label[for="managementFileAdditionalDetailsInput"]')
    ).toBeVisible();
    if (data.ManagementAdditionalDetails !== null) {
      await page
        .locator("#managementFileAdditionalDetailsInput")
        .fill(data.ManagementAdditionalDetails);
    }

    // MANAGEMENT TEAM
    if (mgmtFile.ManagementTeam.length > 0) {
      // Delete all existing members
      while (
        (await page.locator(".managementFileViewTeamMembersGroup").count()) > 0
      ) {
        await sharedTeamMembers.deleteFirstStaffMember();
      }

      // Add each team member
      for (let i = 0; i < data.ManagementTeam.length; i++) {
        await sharedTeamMembers.addMgmtTeamMember(data.ManagementTeam[i]);
      }
    }
  }

  async getManagementFileCode() {
    return await this.page.getByTestId("mgmt-fileId").textContent();
  }

  async validateInitManagementFileDetailsPage() {
    await this.page
      .getByRole("h1", { name: "Create Management File" })
      .isVisible();

    //Project
    await expect(this.page.locator('h2:has-text("Project")')).toBeVisible();
    await expect(
      this.page.locator("label", { hasText: "Ministry project" })
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
    await expect(page.locator("#input-fileStatusTypeCode")).toBeVisible();

    // Project
    await expect(page.locator('h2:has-text("Project")')).toBeVisible();
    await expect(
      page.locator("label", { hasText: "Ministry project" })
    ).toBeVisible();
    await expect(page.locator("#typeahead-project")).toBeVisible();
    await expect(page.locator("label", { hasText: "Funding" })).toBeVisible();
    await expect(page.locator("#input-fundingTypeCode")).toBeVisible();

    // Management Details
    await expect(
      page.locator('h2:has-text("Management Details")')
    ).toBeVisible();
    await expect(page.locator("label", { hasText: "File name" })).toBeVisible();
    await expect(page.locator("#input-fileName")).toBeVisible();
    await expect(
      page.locator("label", { hasText: "Historical file number" })
    ).toBeVisible();
    await expect(page.locator("#input-legacyFileNum")).toBeVisible();
    await expect(page.locator("label", { hasText: "Purpose" })).toBeVisible();
    await expect(page.locator("#input-purposeTypeCode")).toBeVisible();
    await expect(
      page.locator("label", { hasText: "Additional details" })
    ).toBeVisible();
    await expect(page.locator("#input-additionalDetails")).toBeVisible();

    // Management Team
    await expect(page.locator('h2:has-text("Management Team")')).toBeVisible();
    await expect(page.getByTestId("add-team-member")).toBeVisible();
  }

  async validateManagementDetailsViewForm(data) {
    const mgmtFile =
      await this.sharedFileProperties.getManagementFileProperties();
    expect(
      this.page.locator("h1", { hasText: "Management File" })
    ).toBeVisible();

    //Status
    expect(this.page.locator("label", { hasText: "Status" })).toBeVisible();
    const actualStatus =
      (
        await this.page.getByTestId("management-status").textContent()
      )?.trim() || "";
    const expectedStatus = data.ManagementStatus?.trim() || "";
    expect(actualStatus).toEqual(expectedStatus);

    //Project
    expect(this.page.locator("h2", { hasText: "Project" })).toBeVisible();

    expect(
      this.page.locator("label", { hasText: "Ministry project" })
    ).toBeVisible();
    const actualProject =
      (
        await this.page.getByTestId("management-project").textContent()
      )?.trim() || "";
    const expectedProject = data.ManagementMinistryProject?.trim() || "";
    expect(actualProject).toEqual(expectedProject);

    expect(this.page.locator("label", { hasText: "Product" })).toBeVisible();
    const actualProduct =
      (
        await this.page.getByTestId("management-product").textContent()
      )?.trim() || "";
    const expectedProduct = data.ManagementMinistryProduct?.trim() || "";
    expect(actualProduct).toEqual(expectedProduct);

    expect(this.page.locator("label", { hasText: "Funding" })).toBeVisible();
    const actualFunding =
      (
        await this.page.getByTestId("management-funding").textContent()
      )?.trim() || "";
    const expectedFunding = data.ManagementMinistryFunding?.trim() || "";
    expect(actualFunding).toEqual(expectedFunding);

    //Management Details
    expect(
      this.page.locator("label", { hasText: "Management file name" })
    ).toBeVisible();
    const actualName =
      (
        await this.page.getByTestId("management-file-name").textContent()
      )?.trim() || "";
    const expectedName = data.ManagementName?.trim() || "";
    expect(actualName).toEqual(expectedName);

    expect(
      this.page.locator("label", { hasText: "Historical file number" })
    ).toBeVisible();
    const actualHistorical =
      (
        await this.page
          .getByTestId("management-legacy-file-number")
          .textContent()
      )?.trim() || "";
    const expectedHistorical = data.ManagementHistoricalFile?.trim() || "";
    expect(actualHistorical).toEqual(expectedHistorical);

    expect(this.page.locator("label", { hasText: "Purpose" })).toBeVisible();
    const actualPurpose =
      (
        await this.page.getByTestId("management-purpose").textContent()
      )?.trim() || "";
    const expectedPurpose = data.ManagementPurpose?.trim() || "";
    expect(actualPurpose).toEqual(expectedPurpose);

    expect(
      this.page.locator("label", { hasText: "Additional details" })
    ).toBeVisible();
    const actualDetails =
      (
        await this.page
          .getByTestId("management-additional-details")
          .textContent()
      )?.trim() || "";
    const expectedDetails = data.ManagementAdditionalDetails?.trim() || "";
    expect(actualDetails).toEqual(expectedDetails);

    //Management Team
    if (data.ManagementTeam.length > 0) {
      while (
        webDriver.FindElements(managementFileViewTeamMembersGroup).Count > 0
      ) {
        sharedTeamMembers.DeleteFirstStaffMember();
      }

      for (var i = 0; i < mgmtFile.ManagementTeam.Count; i++) {
        sharedTeamMembers.AddMgmtTeamMembers(mgmtFile.ManagementTeam[i]);
      }
    }
  }
}

module.exports = { ManagementFileDetails };
