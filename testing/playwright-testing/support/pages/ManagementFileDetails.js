const { SharedFileProperties } = require("./SharedFileProperties");

class ManagementFileDetails {
  constructor(page) {
    this.page = page;
    this.sharedFileProperties = new SharedFileProperties(page);
  }
  async managementMainMenu() {
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

  async validateManagementFileDetailsPage() {
    await this.page
      .getByRole("h1", { name: "Create Management File" })
      .isVisible();

    //Project
    await this.page.locator('h2:has-text("Project")').isVisible();
    await this.page.getByText("Ministry project").isVisible();
    await this.page.getByTestId("typeahead-project").isVisible();
    await this.page.locator("label", { hasText: "Funding" }).isVisible();
    await this.page.locator("#input-fundingTypeCode").isVisible();

    //Properties
    await this.sharedFileProperties.verifyPropertiesToIncludeInFileInitForm();

    //Management Details
    await this.page.locator('h2:has-text("Management Details")').isVisible();
    await this.page.getByText("File name").isVisible();
    await this.page.locator("#input-fileName").isVisible();
    await this.page.getByText("Historical file number").isVisible();
    await this.page.locator("#input-legacyFileNum").isVisible();
    await this.page.getByText("Purpose").isVisible();
    await this.page.locator("#input-purposeTypeCode").isVisible();
    await this.page.getByText("Additional details").isVisible();
    await this.page.locator("#input-additionalDetails").isVisible();

    //Management Team
    await this.page.locator('h2:has-text("Management Team")').isVisible();
    await this.page.getByTestId("add-team-member").isVisible();
  }

  async createMinimumManagementFileDetails() {
    await this.page.locator("#input-fileName").fill("Test Management File");
    await this.page
      .locator("#input-purposeTypeCode")
      .selectOption({ label: "PIN" });
  }

  async updateManagementFileDetails() {
    // Status
    await expect(
      page.locator('label[for="managementFileStatusInput"]')
    ).toBeVisible();
    if (mgmtFile.ManagementStatus !== "") {
      await page
        .locator("#managementFileStatusInput")
        .selectOption({ label: mgmtFile.ManagementStatus });
    }

    // Project
    await expect(
      page.locator('label[for="managementFileProjectInput"]')
    ).toBeVisible();
    if (mgmtFile.ManagementMinistryProject !== "") {
      const input = page.locator("#managementFileProjectInput");
      await input.fill("");
      await input.type(mgmtFile.ManagementMinistryProject);
      await input.press("Enter");
      await input.press("Backspace");
      await page.waitForTimeout(500); // Replace with better wait, if possible
      await page.locator("#managementFileProject1stOption").click();
    }

    // Product
    await expect(
      page.locator('label[for="managementFileProjectProductSelect"]')
    ).toBeVisible();
    if (mgmtFile.ManagementMinistryProduct !== "") {
      await page
        .locator("#managementFileProjectProductSelect")
        .selectOption({ label: mgmtFile.ManagementMinistryProduct });
    }

    // Funding
    await expect(
      page.locator('label[for="managementFileProjectFundingInput"]')
    ).toBeVisible();
    if (mgmtFile.ManagementMinistryFunding !== "") {
      await page
        .locator("#managementFileProjectFundingInput")
        .selectOption({ label: mgmtFile.ManagementMinistryFunding });
    }

    // MANAGEMENT DETAILS
    // File Name
    await expect(
      page.locator('label[for="managementFileNameInput"]')
    ).toBeVisible();
    if (mgmtFile.ManagementName !== "") {
      await page
        .locator("#managementFileNameInput")
        .fill(mgmtFile.ManagementName);
    }

    // Historical File Number
    await expect(
      page.locator('label[for="managementFileHistoricalFileInput"]')
    ).toBeVisible();
    if (mgmtFile.ManagementHistoricalFile !== "") {
      await page
        .locator("#managementFileHistoricalFileInput")
        .fill(mgmtFile.ManagementHistoricalFile);
    }

    // Purpose
    await expect(
      page.locator('label[for="managementFilePurposeSelect"]')
    ).toBeVisible();
    if (mgmtFile.ManagementPurpose !== "") {
      await page
        .locator("#managementFilePurposeSelect")
        .selectOption({ label: mgmtFile.ManagementPurpose });
    }

    // Additional Details
    await expect(
      page.locator('label[for="managementFileAdditionalDetailsInput"]')
    ).toBeVisible();
    if (mgmtFile.ManagementAdditionalDetails !== "") {
      await page
        .locator("#managementFileAdditionalDetailsInput")
        .fill(mgmtFile.ManagementAdditionalDetails);
    }

    // MANAGEMENT TEAM
    if (mgmtFile.ManagementTeam.length > 0) {
      // Delete all existing members
      while (
        (await page.locator(".managementFileViewTeamMembersGroup").count()) > 0
      ) {
        await sharedTeamMembers.deleteFirstStaffMember(); // Assuming this is your custom helper
      }

      // Add each team member
      for (let i = 0; i < mgmtFile.ManagementTeam.length; i++) {
        await sharedTeamMembers.addMgmtTeamMember(mgmtFile.ManagementTeam[i]); // Assuming this is your helper
      }
    }
  }
}

module.exports = { ManagementFileDetails };
