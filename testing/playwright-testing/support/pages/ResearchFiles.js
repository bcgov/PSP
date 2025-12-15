const { expect } = require("@playwright/test");
const { clickAndWaitFor } = require("../../support/common.js");

class ResearchFiles {
  constructor(page) {
    this.page = page;
  }

  async navigateCreateResearch() {
    clickAndWaitFor(
      this.page,
      "div[data-testid='nav-tooltip-project'] a",
      "div[data-testid='side-tray']"
    );

    clickAndWaitFor(
      this.page,
      "div[data-testid='nav-tooltip-research'] a",
      "//a[text()='Create a Research File']"
    );
    await this.page.locator("//a[text()='Create a Research File']").click();
  }

  async navigateResearchListView() {
    clickAndWaitFor(
      this.page,
      "div[data-testid='nav-tooltip-research'] a",
      "div[data-testid='side-tray']"
    );
    await this.page.locator("//a[text()='Manage Research Files']").click();
  }

  async createMinimumResearchFile(fileName) {
    const researchFileName = await this.page.locator("#input-name");
    expect(researchFileName).toBeVisible();
    await this.page(researchFileName).fill(fileName);
  }

  async verifyCreateResearchFileForm() {
    const formTitle = await this.page.getByTestId("form-title");
    expect(formTitle).toHaveText("Create Research File");

    await expect(
      this.page.locator("//strong[contains(text(),'Name this research file')]")
    ).toBeVisible();
    await expect(this.page.locator("#input-name")).toBeVisible();
    await expect(
      this.page.locator(
        "//div[contains(text(),'A unique file number will be generated for this research file on save.')]"
      )
    ).toBeVisible();
    await expect(
      this.page.locator("//div[contains(text(),'Help with choosing a name')]")
    ).toBeVisible();

    await expect(
      this.page.locator("//h2/div/div[text()='Project']")
    ).toBeVisible();
    await expect(this.page.getByTestId("add-project")).toBeVisible();

    await expect(
      this.page.locator(
        "//h2/div/div[text()='Properties to include in this file:']"
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

    await expect(this.page.getByTestId("cancel-button")).toBeVisible();
    await expect(this.page.getByTestId("save-button")).toBeVisible();
  }

  async verifyResearchListView() {
    await expect(
      this.page.locator("//span[text()='Research Files']")
    ).toBeVisible();
    await expect(this.page.locator("h1 button")).toBeVisible();

    await expect(
      this.page.locator("//strong[text()='Search by:']")
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

    await this.page
      .locator(
        "div[data-testid='researchFilesTable'] div[class='tbody'] div[class='tr-wrapper']"
      )
      .first()
      .waitFor({
        state: "visible",
        timeout: 10000,
      });

    const researchFileCount = await this.page
      .locator(
        "div[data-testid='researchFilesTable'] div[class='tbody'] div[class='tr-wrapper']"
      )
      .count();
    expect(researchFileCount).toBeGreaterThan(0);

    await expect(this.page.getByTestId("input-page-size")).toBeVisible();
    await expect(this.page.locator("ul[class='pagination']")).toBeVisible();
  }

  async saveResearchFile()
  {
      Wait();
      ButtonElement("Save");

      Wait();
      while (webDriver.FindElements(researchFileConfirmationModal).Count() > 0)
      {
          if (sharedModals.ModalHeader() == "Confirm changes")
          {
              Assert.Equal("Confirm changes", sharedModals.ModalHeader());
              Assert.Equal("You have made changes to the properties in this file.", sharedModals.ConfirmationModalText1());
              Assert.Equal("Do you want to save these changes?", sharedModals.ConfirmationModalText2());
              sharedModals.ModalClickOKBttn();
          }
          else if (sharedModals.ModalHeader() == "User Override Required")
          {
              Assert.Equal("User Override Required", sharedModals.ModalHeader());
              Assert.Contains("The selected property already exists in the system's inventory. However, the record is missing spatial details.", sharedModals.ModalContent());
              Assert.Contains("To add the property, the spatial details for this property will need to be updated. The system will attempt to update the property record with spatial information from the current selection.", sharedModals.ModalContent());
              sharedModals.ModalClickOKBttn();
          }
          else if (sharedModals.ModalHeader() == "Confirm status change")
          {
              Assert.Equal("Confirm status change", sharedModals.ModalHeader());
              Assert.Contains("If you save it, only the administrator can turn it back on. You will still see it in the management table.", sharedModals.ConfirmationModalParagraph1());
              Assert.Equal("Do you want to acknowledge and proceed?", sharedModals.ConfirmationModalParagraph2());
              sharedModals.ModalClickOKBttn();
          }

          Wait();
      }
  }

  async cancelCreateResearchFile() {
    await this.page
      .locator("//div[contains(text(),'Cancel')]/parent::button")
      .click();
  }
}
module.exports = ResearchFiles;
