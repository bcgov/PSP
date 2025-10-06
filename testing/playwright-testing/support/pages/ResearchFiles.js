const { expect } = require("@playwright/test");
const { clickAndWaitFor } = require("../../support/common.js");

class ResearchFiles {
    constructor(page) {
        this.page = page;
    }

    async navigateCreateResearch() {
        clickAndWaitFor(
        this.page,
        "div[data-testid='nav-tooltip-research'] a",
        "div[data-testid='side-tray']"
      );
      this.page.locator("a[text()='Create a Research File']").click();
    }

    async navigateResearchListView() {
    clickAndWaitFor(
        this.page,
        "div[data-testid='nav-tooltip-research'] a",
        "div[data-testid='side-tray']"
      );
      this.page.locator("a[text()='Manage Research Files']").click();
    }

    async verifyCreateResearchFileForm() {

      const formTitle = await this.page.getByTestId("form-title");
      expect(formTitle).toHaveText("Create Research File");

      await expect(this.page.locator("//strong[contains(text(),'Name this research file')]")).toBeVisible();
      await expect(this.page.locator("#input-name")).toBeVisible();
      await expect(this.page.locator("//div[contains(text(),'A unique file number will be generated for this research file on save.')]")).toBeVisible();
      await expect(this.page.locator("//div[contains(text(),'Help with choosing a name')]")).toBeVisible();

      await expect(this.page.locator("//h2/div/div[text()='Project']")).toBeVisible();
      await expect(this.page.getByTestId("add-project")).toBeVisible();

      await expect(this.page.locator("//h2/div/div[text()='Properties to include in this file:']")).toBeVisible();
      await expect(this.page.locator("//div[contains(text(),'New workflow')]")).toBeVisible();
      await expect(this.page.locator("//h2/div/div[text()='Selected Properties']")).toBeVisible();
      await expect(this.page.locator("//div[contains(text(),'Identifier')]")).toBeVisible();
      await expect(this.page.locator("//div[contains(text(),'Provide a descriptive name for this land')]")).toBeVisible();
      await expect(this.page.getByTestId("tooltip-icon-property-selector-tooltip")).toBeVisible();
      await expect(this.page.locator("//span[contains(text(),'No Properties selected')]")).toBeVisible();

      await expect(this.page.getByTestId("cancel-button")).toBeVisible();
      await expect(this.page.getByTestId("save-button")).toBeVisible();
    }

    async verifyResearchListView() {
      await expect(this.page.locator("//span[text()='Research Files']")).toBeVisible();
      await expect(this.page.locator("h1 button")).toBeVisible();

      await expect(this.page.locator("//string[text()='Search by:']")).toBeVisible();
      await expect(this.page.locator("#input-regionCode")).toBeVisible();
      await expect(this.page.locator("#input-researchSearchBy")).toBeVisible();
      await expect(this.page.locator("#input-pid")).toBeVisible();
      await expect(this.page.locator("#input-researchFileStatusTypeCode")).toBeVisible();
      await expect(this.page.locator("#input-roadOrAlias")).toBeVisible();
      await expect(this.page.locator("#input-createOrUpdateRange")).toBeVisible();
      await expect(this.page.locator("#datepicker-updatedOnStartDate")).toBeVisible();
      await expect(this.page.locator("#datepicker-updatedOnEndDate")).toBeVisible();
      await expect(this.page.locator("#input-createOrUpdateBy")).toBeVisible();
      await expect(this.page.locator("#input-appLastUpdateUserid")).toBeVisible();
      await expect(this.page.locator("#search-button")).toBeVisible();
      await expect(this.page.locator("#reset-button")).toBeVisible();

      await expect(this.page.locator("//div[contains(text(),'File #')]")).toBeVisible();
      await expect(this.page.getByTestId("sort-column-rfileNumber")).toBeVisible();
      await expect(this.page.locator("//div[contains(text(),'File name')]")).toBeVisible();
      await expect(this.page.getByTestId("sort-column-name")).toBeVisible();
      await expect(this.page.locator("//div[contains(text(),'MOTT region')]")).toBeVisible();
      await expect(this.page.locator("//div[contains(text(),'Created by')]")).toBeVisible();
      await expect(this.page.getByTestId("sort-column-appCreateUserid")).toBeVisible();
      await expect(this.page.locator("//div[contains(text(),'Created date')]")).toBeVisible();
      await expect(this.page.getByTestId("sort-column-appCreateTimestamp")).toBeVisible();
      await expect(this.page.locator("//div[contains(text(),'Last updated by')]")).toBeVisible();
      await expect(this.page.getByTestId("sort-column-appLastUpdateUserid")).toBeVisible();
      await expect(this.page.locator("//div[contains(text(),'Last updated date')]")).toBeVisible();
      await expect(this.page.getByTestId("sort-column-appLastUpdateTimestamp")).toBeVisible();
      await expect(this.page.locator("//div[contains(text(),'Status')]")).toBeVisible();
      await expect(this.page.getByTestId("sort-column-researchFileStatusTypeCode")).toBeVisible();

      const researchFileCount = await this.page.locator("div[data-testid='researchFilesTable'] div[class='tbody'] div[class='tr-wrapper']");
      expect(researchFileCount).toBeGreaterThan(0);

      await expect(this.page.getByTestId("input-page-size")).toBeVisible();
      await expect(this.page.locator("ul[class='pagination']")).toBeVisible();
    }

    async cancelCreateResearchFile() {
      await this.page.locator("//div[contains(text(),'Cancel')]/parent::button").click();
    }
}
module.exports = ResearchFiles;