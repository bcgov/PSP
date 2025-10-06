const { expect } = require("@playwright/test");
const {
  clickAndWaitFor,
  fillTypeahead,
  textEqualsIfNotEmpty,
  textNotEmpty,
  listEquals,
} = require("../../support/common.js");

class LeaseLicence {
  constructor(page) {
    this.page = page;
  }

  async navigateCreateLease() {
    clickAndWaitFor(
      this.page,
      "div[data-testid='nav-tooltip-leases&licences'] a",
      "div[data-testid='side-tray']"
    );
    this.page.locator("a[text()='Create a Lease/Licence File']").click();
  }

  async navigateLeaseListView() {
    clickAndWaitFor(
      this.page,
      "div[data-testid='nav-tooltip-leases&licences'] a",
      "div[data-testid='side-tray']"
    );
    this.page.locator("a[text()='Manage Lease/Licence Files']").click();
  }

  async verifyCreateLeaseForm() {
    const formTitle = await this.page.getByTestId("form-title");
    expect(formTitle).toHaveText("Create Lease/Licence");

    await expect(
      this.page.locator("//h2/div/div[contains(text(),'Original Agreement')]")
    ).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Ministry project')]")
    ).toBeVisible();
    await expect(this.page.locator("#typeahead-project")).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Status')]")
    ).toBeVisible();
    await expect(
      this.page.getByTestId("tooltip-icon-lease-status-tooltip")
    ).toBeVisible();
    await expect(this.page.locator("#input-statusTypeCode")).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Account type')]")
    ).toBeVisible();
    await expect(
      this.page.locator("#input-paymentReceivableTypeCode")
    ).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Commencement')]")
    ).toBeVisible();
    await expect(
      this.page.getByTestId("tooltip-icon-lease-commencement-tooltip")
    ).toBeVisible();
    await expect(this.page.locator("#datepicker-startDate")).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Expiry')]")
    ).toBeVisible();
    await expect(
      this.page.getByTestId("tooltip-icon-lease-expiry-tooltip")
    ).toBeVisible();
    await expect(this.page.locator("#datepicker-expiryDate")).toBeVisible();

    await expect(
      this.page.locator(
        "//h2/div/div[text()='Properties to include in this file:']"
      )
    ).toBeVisible();
    await expect(
      this.page.locator(
        "//div[contains(text(),'Select one or more properties that you want to include in this lease/licence file. You can choose a location from the map, or search by other criteria.')]"
      )
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'New workflow')]")
    ).toBeVisible();
    await expect(
      this.page.locator("//h2/div/div[text()='Selected Properties']")
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
      this.page.locator("//h2/div/div[contains(text(),'Administration')]")
    ).toBeVisible();
    await expect(
      this.page.locator(
        "//div[contains(text(),'Help with choosing the agreement Program, Type and Purpose')]"
      )
    ).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'MOTT contact')]")
    ).toBeVisible();
    await expect(this.page.locator("#input-motiName")).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'MOTT region')]")
    ).toBeVisible();
    await expect(this.page.locator("#input-regionId")).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Program')]")
    ).toBeVisible();
    await expect(this.page.locator("#input-programTypeCode")).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Type')]")
    ).toBeVisible();
    await expect(this.page.locator("#input-leaseTypeCode")).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Purpose')]")
    ).toBeVisible();
    await expect(
      this.page.locator("#multiselect-purposes_input")
    ).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Initiator')]")
    ).toBeVisible();
    await expect(this.page.locator("#input-initiatorTypeCode")).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Responsibility')]")
    ).toBeVisible();
    await expect(
      this.page.locator("#input-responsibilityTypeCode")
    ).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Effective date')]")
    ).toBeVisible();
    await expect(
      this.page.locator("#datepicker-responsibilityEffectiveDate")
    ).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Intended use')]")
    ).toBeVisible();
    await expect(this.page.locator("#input-description")).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Primary arbitration city')]")
    ).toBeVisible();
    await expect(
      this.page.locator("#input-primaryArbitrationCity")
    ).toBeVisible();

    await expect(
      this.page.locator("//h2/div/div[contains(text(),'Lease & Licence Team')]")
    ).toBeVisible();
    await expect(this.page.getByTestId("add-team-member")).toBeVisible();

    const tooltips = await this.page.getByTestId(
      "tooltip-icon-lease-status-tooltip"
    );
    expect(tooltips).toHaveCount(6);

    await expect(this.page.getByTestId("cancel-button")).toBeVisible();
    await expect(this.page.getByTestId("save-button")).toBeVisible();
  }

  async verifyLeaseListView() {
    await expect(
      this.page.locator("//span[text()='Research Files']")
    ).toBeVisible();
    await expect(this.page.locator("h1 button")).toBeVisible();

    await expect(
      this.page.locator("//string[text()='Search by:']")
    ).toBeVisible();
    await expect(this.page.locator("#input-regionCode")).toBeVisible();
    await expect(this.page.locator("#input-researchSearchBy")).toBeVisible();
    await expect(this.page.locator("#input-pid")).toBeVisible();
    await expect(
      this.page.locator("#input-researchFileStatusTypeCode")
    ).toBeVisible();
    await expect(this.page.locator("#input-roadOrAlias")).toBeVisible();
    await expect(this.page.locator("#input-createOrUpdateRange")).toBeVisible();
    await expect(
      this.page.locator("#datepicker-updatedOnStartDate")
    ).toBeVisible();
    await expect(
      this.page.locator("#datepicker-updatedOnEndDate")
    ).toBeVisible();
    await expect(this.page.locator("#input-createOrUpdateBy")).toBeVisible();
    await expect(this.page.locator("#input-appLastUpdateUserid")).toBeVisible();
    await expect(this.page.locator("#search-button")).toBeVisible();
    await expect(this.page.locator("#reset-button")).toBeVisible();

    await expect(
      this.page.locator("//div[contains(text(),'File #')]")
    ).toBeVisible();
    await expect(
      this.page.getByTestId("sort-column-rfileNumber")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'File name')]")
    ).toBeVisible();
    await expect(this.page.getByTestId("sort-column-name")).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'MOTT region')]")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Created by')]")
    ).toBeVisible();
    await expect(
      this.page.getByTestId("sort-column-appCreateUserid")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Created date')]")
    ).toBeVisible();
    await expect(
      this.page.getByTestId("sort-column-appCreateTimestamp")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Last updated by')]")
    ).toBeVisible();
    await expect(
      this.page.getByTestId("sort-column-appLastUpdateUserid")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Last updated date')]")
    ).toBeVisible();
    await expect(
      this.page.getByTestId("sort-column-appLastUpdateTimestamp")
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Status')]")
    ).toBeVisible();
    await expect(
      this.page.getByTestId("sort-column-researchFileStatusTypeCode")
    ).toBeVisible();

    const researchFileCount = await this.page.locator(
      "div[data-testid='researchFilesTable'] div[class='tbody'] div[class='tr-wrapper']"
    );
    expect(researchFileCount).toBeGreaterThan(0);

    await expect(this.page.getByTestId("input-page-size")).toBeVisible();
    await expect(this.page.locator("ul[class='pagination']")).toBeVisible();
  }

  async cancelCreateResearchFile() {
    await this.page
      .locator("//div[contains(text(),'Cancel')]/parent::button")
      .click();
  }
}

module.exports = LeaseLicence;
