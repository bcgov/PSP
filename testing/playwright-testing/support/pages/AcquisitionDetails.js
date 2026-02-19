const { expect } = require("@playwright/test");
const { clickAndWaitFor } = require("../../support/common.js");

class AcquisitionDetails {
  constructor(page) {
    this.page = page;
  }

  async navigateToAcquisitionFileListView() {
    clickAndWaitFor(
      this.page,
      "div[data-testid='nav-tooltip-acquisition'] a",
      "div[data-testid='side-tray']"
    );
    await this.page
      .locator("//a[contains(text(),'Manage Acquisition Files')]")
      .click();
  }

  async navigateToCreateNewAcquisitionFile() {
    clickAndWaitFor(
      this.page,
      "div[data-testid='nav-tooltip-acquisition'] a",
      "div[data-testid='side-tray']"
    );
    await this.page
      .locator("//a[contains(text(),'Create an Acquisition File')]")
      .click();
  }

  async createMinimumAcquisitionFile(acquisition) {
    const acquisitionName = await this.page.locator(
      "input[id='input-fileName']"
    );
    await acquisitionName.waitFor({ status: "visible" });
    await acquisitionName.fill(acquisition.AcquisitionFileName);

    await this.page
      .locator("select[id='input-acquisitionType']")
      .selectOption({ label: acquisition.AcquisitionType });
    await this.page
      .locator("select[id='input-region']")
      .selectOption({ label: acquisition.AcquisitionMOTIRegion });

    await this.page.getByTestId("save-button").click();
  }

  async verifyAcquisitionFileCreateForm() {
    const formTitle = await this.page.getByTestId("form-title");
    await expect(formTitle).toContainText("Create Acquisition File");

    await expect(
      this.page.locator("//h2/div/div[text()='Project']")
    ).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Ministry project')]")
    ).toBeVisible();
    await expect(this.page.locator("#typeahead-project")).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Funding')]")
    ).toBeVisible();

    const fundingSelect = await this.page.locator("#input-fundingTypeCode");
    expect(fundingSelect).toBeVisible();
    const fundingSelectOptions = await fundingSelect.locator("option").count();
    expect(fundingSelectOptions).toBeGreaterThan(0);

    await expect(
      this.page.locator("//h2/div/div[text()='Progress Statuses']")
    ).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'File progress')]")
    ).toBeVisible();
    await expect(
      this.page.locator("#multiselect-progressStatuses_input")
    ).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Appraisal')]")
    ).toBeVisible();

    const appraisalSelect = await this.page.locator(
      "#input-appraisalStatusType"
    );
    expect(appraisalSelect).toBeVisible();
    const appraisalSelectOptions = await appraisalSelect
      .locator("option")
      .count();
    expect(appraisalSelectOptions).toBeGreaterThan(0);

    await expect(
      this.page.locator("//label[contains(text(),'Legal survey')]")
    ).toBeVisible();

    const legalSurveySelect = await this.page.locator(
      "#input-legalSurveyStatusType"
    );
    expect(legalSurveySelect).toBeVisible();
    const legalSurveySelectSelectOptions = await legalSurveySelect
      .locator("option")
      .count();
    expect(legalSurveySelectSelectOptions).toBeGreaterThan(0);

    await expect(
      this.page.locator("//label[contains(text(),'Type of taking')]")
    ).toBeVisible();
    await expect(
      this.page.locator("#multiselect-takingStatuses_input")
    ).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Expropriation risk')]")
    ).toBeVisible();

    const expropriationRiskSelect = await this.page.locator(
      "#input-expropiationRiskStatusType"
    );
    expect(expropriationRiskSelect).toBeVisible();
    const expropriationRiskSelectOptions = await expropriationRiskSelect
      .locator("option")
      .count();
    expect(expropriationRiskSelectOptions).toBeGreaterThan(0);

    await expect(
      this.page.locator("//h2/div/div[text()='Schedule']")
    ).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Assigned date')]")
    ).toBeVisible();
    await expect(this.page.locator("#datepicker-assignedDate")).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Delivery date')]")
    ).toBeVisible();
    await expect(this.page.locator("#datepicker-deliveryDate")).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Estimated date')]")
    ).toBeVisible();
    await expect(
      this.page.locator("#datepicker-estimatedCompletionDate")
    ).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Possession date')]")
    ).toBeVisible();
    await expect(this.page.locator("#datepicker-possessionDate")).toBeVisible();

    await expect(
      this.page.locator(
        "//h2/div/div[text()='Properties to include in this file:']"
      )
    ).toBeVisible();
    await expect(
      this.page.locator(
        "//div[contains(text(),'Select one or more properties that you want to include in this acquisition. You can choose a location from the map, or search by other criteria.')]"
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
      this.page.locator("//span[contains(text(),'No Properties selected')]")
    ).toBeVisible();

    await expect(
      this.page.locator("//h2/div/div[text()='Acquisition Details']")
    ).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Acquisition file name')]")
    ).toBeVisible();
    await expect(this.page.locator("#input-fileName")).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Historical file number')]")
    ).toBeVisible();
    await expect(this.page.locator("#input-legacyFileNumber")).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Physical file status')]")
    ).toBeVisible();

    const physicalFileSelect = await this.page.locator(
      "#input-acquisitionPhysFileStatusType"
    );
    expect(physicalFileSelect).toBeVisible();
    const physicalFileSelectOptions = await physicalFileSelect
      .locator("option")
      .count();
    expect(physicalFileSelectOptions).toBeGreaterThan(0);

    await expect(
      this.page.locator("//label[contains(text(),'Physical file details')]")
    ).toBeVisible();
    await expect(this.page.locator("#input-physicalFileDetails")).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Acquisition type')]")
    ).toBeVisible();

    const acquisitionTypeSelect = await this.page.locator(
      "#input-acquisitionType"
    );
    expect(acquisitionTypeSelect).toBeVisible();
    const acquisitionTypeSelectOptions = await acquisitionTypeSelect
      .locator("option")
      .count();
    expect(acquisitionTypeSelectOptions).toBeGreaterThan(0);

    await expect(
      this.page.locator("//label[contains(text(),'Ministry region')]")
    ).toBeVisible();
    const ministryRegionSelect = await this.page.locator("#input-region");
    expect(ministryRegionSelect).toBeVisible();
    const ministryRegionSelectOptions = await ministryRegionSelect
      .locator("option")
      .count();
    expect(ministryRegionSelectOptions).toBeGreaterThan(0);

    await expect(
      this.page.locator("//h2/div/div[text()='Acquisition Team']")
    ).toBeVisible();
    await expect(this.page.getByTestId("add-team-member")).toBeVisible();

    await expect(
      this.page.locator("//h2/div/div[text()='Owners']")
    ).toBeVisible();
    await expect(
      this.page.locator(
        "//p[contains(text(),'Each property in this file should be owned by the owner(s) in this section')]"
      )
    ).toBeVisible();
    await expect(this.page.getByTestId("add-file-owner")).toBeVisible();

    await expect(
      this.page.locator("//label[contains(text(),'Owner solicitor')]")
    ).toBeVisible();
    await expect(
      this.page.locator(
        "//input[@id='input-ownerSolicitor.contact.id']/parent::div/preceding-sibling::div[text()='Select from contacts']"
      )
    ).toBeVisible();
    await expect(
      this.page.locator("//label[contains(text(),'Owner representative')]")
    ).toBeVisible();
    await expect(
      this.page.locator(
        "//input[@id='input-ownerRepresentative.contact.id']/parent::div/preceding-sibling::div[text()='Select from contacts']"
      )
    ).toBeVisible();
    await expect(
      this.page.locator("label").filter({ hasText: "Comment:" }).first()
    ).toBeVisible();
    await expect(
      this.page.locator("textarea[id='input-ownerRepresentative.comment']")
    ).toBeVisible();
    const contactManagerBttns = await this.page.locator(
      "button[title='Select Contact']"
    );
    expect(contactManagerBttns).toHaveCount(2);

    await expect(
      this.page.locator("//h2/div/div[text()='Notice of Claim']")
    ).toBeVisible();

    await expect(
      this.page.locator("//label[contains(text(),'Received date')]")
    ).toBeVisible();
    await expect(
      this.page.locator("input[id='datepicker-noticeOfClaim.receivedDate']")
    ).toBeVisible();
    await expect(
      this.page.locator("label").filter({ hasText: "Comment:" }).last()
    ).toBeVisible();
    await expect(
      this.page.locator("textarea[id='input-noticeOfClaim.comment']")
    ).toBeVisible();

    const tooltips = await this.page.getByTestId(
      "tooltip-icon-section-field-tooltip"
    );
    expect(tooltips).toHaveCount(4);

    await expect(this.page.getByTestId("cancel-button")).toBeVisible();
    await expect(this.page.getByTestId("save-button")).toBeVisible();
  }

  async verifyAcquisitionListView() {
    await expect(
      this.page.locator(
        "//h1/div/div/span[contains(text(),'Acquisition Files')]"
      )
    ).toBeVisible();
    await expect(this.page.locator("h1 button")).toBeVisible();

    await expect(this.page.locator("#input-searchBy")).toBeVisible();
    await expect(this.page.locator("#input-address")).toBeVisible();
    await expect(
      this.page.locator("#multiselect-acquisitionTeamMembers_input")
    ).toBeVisible();
    await expect(this.page.locator("#input-ownerName")).toBeVisible();
    await expect(
      this.page.locator("#input-acquisitionFileStatusTypeCode")
    ).toBeVisible();
    await expect(this.page.locator("#input-projectNameOrNumber")).toBeVisible();
    await expect(this.page.getByTestId("search")).toBeVisible();
    await expect(this.page.getByTestId("reset-button")).toBeVisible();

    await expect(
      this.page.locator(
        "//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Acquisition file #')]"
      )
    ).toBeVisible();
    await expect(this.page.getByTestId("sort-column-fileNumber")).toBeVisible();
    await expect(
      this.page.locator(
        "//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Historical file #')]"
      )
    ).toBeVisible();
    await expect(
      this.page.getByTestId("sort-column-legacyFileNumber")
    ).toBeVisible();
    await expect(
      this.page.locator(
        "//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Acquisition file name')]"
      )
    ).toBeVisible();
    await expect(this.page.getByTestId("sort-column-fileName")).toBeVisible();
    await expect(
      this.page.locator(
        "//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'MOTT region')]"
      )
    ).toBeVisible();
    await expect(
      this.page.locator(
        "//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Projects')]"
      )
    ).toBeVisible();
    await expect(
      this.page.locator(
        "//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Team member')]"
      )
    ).toBeVisible();
    await expect(
      this.page.locator(
        "//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Owner')]"
      )
    ).toBeVisible();
    await expect(
      this.page.locator(
        "//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Civic Address / PID / PIN')]"
      )
    ).toBeVisible();
    await expect(
      this.page.locator(
        "//div[@role='table']/div[@class='thead thead-light']/div/div/div[contains(text(),'Status')]"
      )
    ).toBeVisible();
    await expect(
      this.page.getByTestId("sort-column-acquisitionFileStatusTypeCode")
    ).toBeVisible();

    await this.page
      .locator(
        "div[data-testid='acquisitionFilesTable'] div[class='tbody'] div[class='tr-wrapper']"
      )
      .first()
      .waitFor({
        state: "visible",
        timeout: 10000,
      });

    const acquisitionTableCount = await this.page
      .locator(
        "div[data-testid='acquisitionFilesTable'] div[class='tbody'] div[class='tr-wrapper']"
      )
      .count();
    expect(acquisitionTableCount).toBeGreaterThan(0);

    await expect(this.page.locator("div[class='Menu-root']")).toBeVisible();
    await expect(this.page.locator("ul[class='pagination']")).toBeVisible();
  }

  async cancelCreateAcquisitionFile() {
    await this.page
      .locator("//div[contains(text(),'Cancel')]/parent::button")
      .click();
  }
}

module.exports = AcquisitionDetails;
