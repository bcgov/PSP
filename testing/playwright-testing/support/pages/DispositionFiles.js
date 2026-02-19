const { expect } = require("@playwright/test");
const { clickAndWaitFor, clickSaveButton } = require("../../support/common.js");
const SharedModal = require("./SharedModal.js");

class DispositionFiles {
  constructor(page) {
    this.page = page;
    this.sharedModal = new SharedModal(page);
  }

  async navigateCreateDisposition() {
    clickAndWaitFor(
      this.page,
      "div[data-testid='nav-tooltip-disposition'] a",
      "div[data-testid='side-tray']"
    );
    await this.page.locator("//a[text()='Create a Disposition File']").click();
  }

  async navigateDispositionListView() {
    clickAndWaitFor(
      this.page,
      "div[data-testid='nav-tooltip-leases&licences'] a",
      "div[data-testid='side-tray']"
    );

    clickAndWaitFor(
      this.page,
      "div[data-testid='nav-tooltip-disposition'] a",
      "//a[text()='Manage Disposition Files']"
    );
    await this.page.locator("//a[text()='Manage Disposition Files']").click();
  }

  async createMinimumDispositionFile(dispositionFile) {
    const dispositionFileDetailsNameInput = await this.page.locator(
      "#input-fileName"
    );
    expect(dispositionFileDetailsNameInput).toBeVisible();
    await this.page(dispositionFileDetailsNameInput).fill(
      dispositionFile.DispositionFileName
    );

    const dispositionFileDetailsStatusSelect = await this.page.locator(
      "#input-dispositionStatusTypeCode"
    );
    expect(dispositionFileDetailsStatusSelect).toBeVisible();
    await this.page(dispositionFileDetailsStatusSelect).selectOption({
      label: dispositionFile.DispositionStatus,
    });

    const dispositionFileDetailsTypeSelect = await this.page.locator(
      "#input-dispositionTypeCode"
    );
    expect(dispositionFileDetailsTypeSelect).toBeVisible();
    await this.page(dispositionFileDetailsTypeSelect).selectOption({
      label: dispositionFile.DispositionType,
    });

    if (dispositionFile.DispositionType === "Other Transfer") {
      const dispositionFileDetailsOtherTransferTypeInput =
        await this.page.locator("#input-dispositionTypeOther");
      expect(dispositionFileDetailsOtherTransferTypeInput).toBeVisible();
      await this.page(dispositionFileDetailsOtherTransferTypeInput).fill(
        dispositionFile.DispositionOtherTransferType
      );
    }

    const dispositionFileDetailsMOTIRegionSelect = await this.page.locator(
      "#input-regionCode"
    );
    expect(dispositionFileDetailsMOTIRegionSelect).toBeVisible();
    await this.page(dispositionFileDetailsMOTIRegionSelect).selectOption({
      label: dispositionFile.DispositionMOTIRegion,
    });

    await this.page.getByTestId("save-button").click();
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
    const fundingSelectElement = await this.page.locator(
      "#input-fundingTypeCode"
    );
    expect(fundingSelectElement).toBeVisible();

    const fundingOptions = await fundingSelectElement.locator("option").count();
    expect(fundingOptions).toBeGreaterThan(0);

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
        "//div[contains(text(),'Select one or more properties that you want to include in this disposition. You can choose a location from the map, or search by other criteria.')]"
      )
    ).toBeVisible();
    await expect(
      this.page.locator("//h2/div/div/div/div[text()='Selected Properties']")
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
    const statusElement = await this.page.locator(
      "#input-dispositionStatusTypeCode"
    );
    expect(statusElement).toBeVisible();

    const statusOptions = await statusElement.locator("option").count();
    expect(statusOptions).toBeGreaterThan(0);

    await expect(
      this.page.locator("//label[contains(text(),'Disposition type')]")
    ).toBeVisible();
    const typeSelect = await this.page.locator("#input-dispositionTypeCode");
    expect(typeSelect).toBeVisible();

    const typeOptions = await typeSelect.locator("option").count();
    expect(typeOptions).toBeGreaterThan(0);

    await expect(this.page.getByText("Initiating document:")).toBeVisible();
    const initialDocSelect = await this.page.locator(
      "#input-initiatingDocumentTypeCode"
    );
    expect(initialDocSelect).toBeVisible();

    const initialDocOptions = await initialDocSelect.locator("option").count();
    expect(initialDocOptions).toBeGreaterThan(0);

    await expect(
      this.page.locator("//label[contains(text(),'Initiating document date')]")
    ).toBeVisible();
    await expect(
      this.page.locator("#datepicker-initiatingDocumentDate")
    ).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Physical file status')]")
    ).toBeVisible();
    const physicalFileSelect = await this.page.locator(
      "#input-physicalFileStatusTypeCode"
    );
    expect(physicalFileSelect).toBeVisible();

    const physicalFileOptions = await physicalFileSelect
      .locator("option")
      .count();
    expect(physicalFileOptions).toBeGreaterThan(0);

    await expect(
      this.page.locator("//label[contains(text(),'Initiating branch')]")
    ).toBeVisible();
    const initialBranchSelect = await this.page.locator(
      "#input-initiatingBranchTypeCode"
    );
    expect(initialBranchSelect).toBeVisible();
    const initialBranchOptions = await initialBranchSelect
      .locator("option")
      .count();
    expect(initialBranchOptions).toBeGreaterThan(0);

    await expect(
      this.page.locator("//label[contains(text(),'Ministry region')]")
    ).toBeVisible();
    const regionSelect = await this.page.locator("#input-regionCode");
    expect(regionSelect).toBeVisible();
    const regionsOptions = await regionSelect.locator("option").count();
    expect(regionsOptions).toBeGreaterThan(0);

    await expect(
      this.page.locator("//h2/div/div[contains(text(),'Disposition Team')]")
    ).toBeVisible();
    await expect(this.page.getByTestId("add-team-member")).toBeVisible();

    await this.page
      .getByTestId("tooltip-icon-section-field-tooltip")
      .first()
      .waitFor({
        state: "visible",
        timeout: 10000,
      });

    const dispositionTooltipsCount = await this.page
      .getByTestId("tooltip-icon-section-field-tooltip")
      .count();
    expect(dispositionTooltipsCount).toEqual(3);

    await expect(this.page.getByTestId("cancel-button")).toBeVisible();
    await expect(this.page.getByTestId("save-button")).toBeVisible();
  }

  async verifyDispositionListView() {
    await expect(
      this.page.locator("//span[text()='Disposition Files']")
    ).toBeVisible();
    await expect(this.page.locator("h1 button")).toBeVisible();

    await expect(
      this.page.locator("//strong[text()='Search by:']")
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

    await this.page
      .locator(
        "div[data-testid='dispositionFilesTable'] div[class='tbody'] div[class='tr-wrapper']"
      )
      .first()
      .waitFor({
        state: "visible",
        timeout: 10000,
      });

    const dispositionFileCount = await this.page
      .locator(
        "div[data-testid='dispositionFilesTable'] div[class='tbody'] div[class='tr-wrapper']"
      )
      .count();
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
