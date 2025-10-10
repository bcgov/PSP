const { expect } = require("@playwright/test");
const { clickAndWaitFor } = require("../common.js");

class AdminTools {
  constructor(page) {
    this.page = page;
  }
  async navigateAdminUsers() {
    clickAndWaitFor(
      this.page,
      "div[data-testid='nav-tooltip-admintools'] a",
      "div[data-testid='side-tray']"
    );
    this.page.locator("a[text()='Manage Users']").click();
  }

  async navigateAdminUserRequests() {
    clickAndWaitFor(
      this.page,
      "div[data-testid='nav-tooltip-admintools'] a",
      "div[data-testid='side-tray']"
    );
    this.page.locator("a[text()='Manage Access Requests']").click();
  }

  async navigateCDOGS() {
    clickAndWaitFor(
      this.page,
      "div[data-testid='nav-tooltip-admintools'] a",
      "div[data-testid='side-tray']"
    );
    this.page.locator("a[text()='Manage Form Document Templates']").click();
  }

  async navigateFinancialCodes() {
    clickAndWaitFor(
      this.page,
      "div[data-testid='nav-tooltip-admintools'] a",
      "div[data-testid='side-tray']"
    );
    this.page.locator("a[text()='Manage Project and Financial Codes']").click();
  }

  async navigateBCFTAOwnershipPage() {
    clickAndWaitFor(
      this.page,
      "div[data-testid='nav-tooltip-admintools'] a",
      "div[data-testid='side-tray']"
    );
    this.page.locator("a[text()='Manage BCTFA Property Ownership']").click();
  }

  async navigateHome() {
    await this.page.getByTestId("nav-tooltip-mapview").click();
  }

  async verifyManageUsersListView() {
    await expect(
      this.page.locator("//h1/span[contains(text(),'User Management')]")
    ).toBeVisible();
    await expect(
      this.page.getByRole("div").filter({ hasText: "Search By:" })
    ).toBeVisible();
    await expect(this.page.locator("#input-role")).toBeVisible();
    await expect(this.page.locator("#input-region")).toBeVisible();
    await expect(
      this.page.locator("#input-businessIdentifierValue")
    ).toBeVisible();
    await expect(this.page.locator("#input-email']")).toBeVisible();
    await expect(this.page.locator("#search-button")).toBeVisible();
    await expect(this.page.locator("#reset-button")).toBeVisible();
    await expect(
      this.page.getByRole("span").filter({ hasText: " users only" })
    ).toBeVisible();
    await expect(this.page.locator("#input-activeOnly")).toBeVisible();

    await expect(
      this.page.locator("//div[contains(text(),'Active')]")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'IDIR/BCeID')]")
    ).toBeVisible();
    await expect(
      this.page.getByTestId("sort-column-businessIdentifierValue")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'First name')]")
    ).toBeVisible();
    await expect(this.page.getByTestId("sort-column-firstName")).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Last name')]")
    ).toBeVisible();
    await expect(this.page.getByTestId("sort-column-surname")).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Position')]")
    ).toBeVisible();
    await expect(this.page.getByTestId("sort-column-position")).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'User type')]")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Roles')]")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'MOTT region(s)')]")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Last login')]")
    ).toBeVisible();

    const usersCount = await this.page.locator(
      "div[data-testid='usersTable'] div[class='tbody'] div[class='tr-wrapper']"
    );
    expect(usersCount).toBeGreaterThan(0);

    await expect(this.page.getByTestId("input-page-size")).toBeVisible();
    await expect(this.page.locator("ul[class='pagination']")).toBeVisible();
  }

  async verifyUserRequestsListView() {
    await expect(
      this.page.locator("//span[text()='PIMS User Access Requests']")
    ).toBeVisible();

    await expect(this.page.locator("//label[text()='Search:']")).toBeVisible();
    await expect(
      this.page.getByTitle("Search by IDIR/Last Name")
    ).toBeVisible();
    await expect(this.page.locator("#search-button")).toBeVisible();
    await expect(this.page.locator("#reset-button")).toBeVisible();

    await expect(
      this.page.locator("//div[contains(text(),'IDIR/BCeID')]")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'First name')]")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Last name')]")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Email')]")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Position')]")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Status')]")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Role')]")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'MOTT region')]")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Actions')]")
    ).toBeVisible();

    await expect(this.page.getByTestId("input-page-size")).toBeVisible();
    await expect(this.page.locator("ul[class='pagination']")).toBeVisible();
  }

  async verifyCDOGSUploadPage() {
    await expect(
      this.page.locator(
        "//h1/span[contains(text(),'PIMS Document Template Management')]"
      )
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Form Type:')]")
    ).toBeVisible();

    const cdogsTemplateSelect = await this.page.locator(
      "select[class='form-select form-control']"
    );
    expect(cdogsTemplateSelect).toBeVisible();

    const optionsLocator = cdogsTemplateSelect.locator("option");
    await expect(optionsLocator).toHaveCount(19);

    await cdogsTemplateSelect.selectOption({
      label: "Notice of Expropriation (Form 1)",
    });

    await expect(
      this.page.locator(
        "//h2/div/div/div/div/span[contains(text(),'Documents')]"
      )
    ).toBeVisible();
    await expect(this.page.locator("h2 button").first()).toBeVisible();
    await expect(this.page.getByTestId("refresh-button").first()).toBeVisible();

    await expect(
      this.page.getByRole("label").filter({ hasText: "Filter by:" })
    ).toBeVisible();
    await expect(this.page.getByTestId("document-type")).toBeVisible();
    await expect(this.page.getByTestId("document-status")).toBeVisible();
    await expect(this.page.getByTestId("document-filename")).toBeVisible();
    await expect(this.page.getByTestId("search")).toBeVisible();
    await expect(this.page.getByTestId("reset-button")).toBeVisible();

    await expect(
      this.page.locator("//div[contains(text(),'Document type')]")
    ).toBeVisible();
    await expect(
      this.page.getByTestId("sort-column-documentType")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Document name')]")
    ).toBeVisible();
    await expect(this.page.getByTestId("sort-column-fileName")).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Uploaded')]")
    ).toBeVisible();
    await expect(
      this.page.getByTestId("sort-column-appCreateTimestamp")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Status')]")
    ).toBeVisible();
    await expect(
      this.page.getByTestId("sort-column-statusTypeCode")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Actions')]")
    ).toBeVisible();
  }

  async verifyFinancialCodesListView() {
    await expect(
      this.page.getByRole("span").filter({ hasText: "Financial Codes" })
    ).toBeVisible();
    await expect(
      this.page.getByRole("button").filter({ hasText: "Add a Financial Code" })
    ).toBeVisible();
    await expect(
      this.page.getByRole("div").filter({ hasText: "Search By:" })
    ).toBeVisible();
    await expect(this.page.locator("#input-financialCodeType")).toBeVisible();
    await expect(
      this.page.locator("#input-codeValueOrDescription")
    ).toBeVisible();
    await expect(this.page.locator("#input-showExpiredCodes")).toBeVisible();
    await expect(
      this.page.getByRole("span").filter({ hasText: "Show expired codes" })
    ).toBeVisible();
    await expect(this.page.locator("#search-button")).toBeVisible();
    await expect(this.page.locator("#reset-button")).toBeVisible();

    await expect(
      this.page.locator("//div[contains(text(),'Code value')]")
    ).toBeVisible();
    await expect(this.page.getByTestId("sort-column-code")).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Code description')]")
    ).toBeVisible();
    await expect(
      this.page.getByTestId("sort-column-description")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Code type')]")
    ).toBeVisible();
    await expect(this.page.getByTestId("sort-column-type")).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Effective date')]")
    ).toBeVisible();
    await expect(
      this.page.getByTestId("sort-column-effectiveDate")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Expiry date')]")
    ).toBeVisible();
    await expect(this.page.getByTestId("sort-column-expiryDate")).toBeVisible();

    const financialCodesCount = await this.page.locator(
      "div[data-testid='FinancialCodeTable'] div[class='tbody'] div[class='tr-wrapper']"
    );
    expect(financialCodesCount).toBeGreaterThan(0);

    await expect(this.page.getByTestId("input-page-size")).toBeVisible();
    await expect(this.page.locator("ul[class='pagination']")).toBeVisible();
  }

  async verifyFinancialCodeCreateForm() {
    await this.page.locator("h1 button").click();

    expect(
      await this.page
        .getByRole("h1")
        .filter({ hasText: "Create Financial Code" })
    ).toBeVisible();
    expect(
      await this.page.getByRole("label").filter({ hasText: "Code type" })
    ).toBeVisible();

    const codeTypeSelect = await this.page.locator("#input-type");
    expect(codeTypeSelect).toBeVisible();

    const codeTypesOptions = await codeTypeSelect.locator("options");
    expect(codeTypesOptions).toHaveCount(8);

    expect(
      await this.page.getByRole("label").filter({ hasText: "Code value" })
    ).toBeVisible();
    expect(await this.page.locator("#input-code")).toBeVisible();

    expect(
      await this.page.getByRole("label").filter({ hasText: "Code description" })
    ).toBeVisible();
    expect(await this.page.locator("#input-description")).toBeVisible();

    expect(
      await this.page.getByRole("label").filter({ hasText: "Effective date" })
    ).toBeVisible();
    expect(await this.page.locator("#datepicker-effectiveDate")).toBeVisible();

    expect(
      await this.page.getByRole("label").filter({ hasText: "Expiry date" })
    ).toBeVisible();
    expect(await this.page.locator("#datepicker-expiryDate")).toBeVisible();

    expect(
      await this.page.getByRole("label").filter({ hasText: "Display order" })
    ).toBeVisible();
    expect(await this.page.locator("#input-displayOrder")).toBeVisible();

    const tooltips = await this.page.getByTestId(
      "tooltip-icon-section-field-tooltip"
    );
    expect(tooltips).toHaveCount(2);

    await expect(
      this.page.locator("//div[text()='Cancel']:parent::button")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[text()='Save']:parent::button")
    ).toBeVisible();
  }

  async verifyBCFTAPage() {
    await expect(
      this.page.getByRole("div").filter({ hasText: "Update BCTFA Ownership" })
    ).toBeVisible();
    await expect(
      this.page
        .getByRole("div")
        .filter({
          hasText:
            "Upload a csv file, that contains the list of all PIDs currently owned by BCTFA, as provided by LTSA. Uploading this file here will update the BCTFA ownership layer within PIMS to reflect the PIDS listed in the uploaded file.",
        })
    ).toBeVisible();
    await expect(
      this.page
        .getByRole("div")
        .filter({ hasText: "Drag files here to attach or" })
    ).toBeVisible();
    await expect(
      this.page.getByRole("label").filter({ hasText: "Browse" })
    ).toBeVisible();
    await expect(this.page.getByTestId("upload-input")).toBeVisible();
  }
}
module.exports = AdminTools;
