// AcquisitionChecklist.js
// Translated from AcquisitionChecklist.cs (C#) to JavaScript for Playwright
const { expect } = require('@playwright/test');

class AcquisitionChecklist {
  constructor(page) {
    this.page = page;
  }

  async navigateChecklistTab() {
    const checklistLinkTab = this.page.locator("//a[contains(text(),'Checklist')]");
    await checklistLinkTab.waitFor({ state: 'visible' });
    await checklistLinkTab.click();
  }

  async editChecklistButton() {
    const checklistEditBtn = this.page.locator("//button[@title='Edit checklist']");
    await checklistEditBtn.waitFor({ state: 'visible' });
    await checklistEditBtn.click();
  }

  async updateChecklistForm(checklist) {

    //File Initiation
    const checklistFileInitiationTitle = await this.page.locator("//h2/div/div[contains(text(),'File Initiation')]");
    await expect(checklistFileInitiationTitle).toBeVisible();

    for(var i = 0; i < checklist.AcqFileInitiation.length(); i++) {
      const labelElement = await this.page.getByRole("label").filter({ hasText: checklist.AcqFileInitiation[i].title });
      await expect(labelElement).toBeVisible();

      const selectElement = await this.page.locator(`#input-checklistSections[0].items[${i}].statusType`);
      await expect(selectElement).toBeVisible();
      await selectElement.selectOption({ label: checklist.AcqFileInitiation[i].status });
    }

    //Active File Management
    const checklistActiveFileMgmtTitle = await this.page.locator("//h2/div/div[contains(text(),'Active File Management')]");
    await expect(checklistActiveFileMgmtTitle).toBeVisible();

    for(var i = 0; i < checklist.AcqActiveFileManagement.length(); i++) {
      const labelElement = await this.page.getByRole("label").filter({ hasText: checklist.AcqActiveFileManagement[i].title });
      await expect(labelElement).toBeVisible();

      const selectElement = await this.page.locator(`#input-checklistSections[1].items[${i}].statusType`);
      await expect(selectElement).toBeVisible();
      await selectElement.selectOption({ label: checklist.AcqActiveFileManagement[i].status });
    }

    //Crown Land
    const checklistCrownLandTitle = await this.page.locator("//h2/div/div[contains(text(),'Crown Land')]");
    await expect(checklistCrownLandTitle).toBeVisible();

    for(var i = 0; i < checklist.AcqCrownLand.length(); i++) {
      const labelElement = await this.page.getByRole("label").filter({ hasText: checklist.AcqCrownLand[i].title });
      await expect(labelElement).toBeVisible();

      const selectElement = await this.page.locator(`#input-checklistSections[2].items[${i}].statusType`);
      await expect(selectElement).toBeVisible();
      await selectElement.selectOption({ label: checklist.AcqCrownLand[i].status });
    }

    //Section 3 - Agreement
    const checklistSection3Title = await this.page.locator("//h2/div/div[contains(text(),'Section 3 - Agreement')]");
    await expect(checklistSection3Title).toBeVisible();

    for(var i = 0; i < checklist.AcqSection3Agreement.length(); i++) {
      const labelElement = await this.page.getByRole("label").filter({ hasText: checklist.AcqSection3Agreement[i].title });
      await expect(labelElement).toBeVisible();

      const selectElement = await this.page.locator(`#input-checklistSections[3].items[${i}].statusType`);
      await expect(selectElement).toBeVisible();
      await selectElement.selectOption({ label: checklist.AcqSection3Agreement[i].status });
    }

    //Section 6 - Expropriation
    const checklistSection6Title = await this.page.locator("//h2/div/div[contains(text(),'Section 6 - Expropriation')]");
    await expect(checklistSection6Title).toBeVisible();

    for(var i = 0; i < checklist.AcqSection6Expropriation.length(); i++) {
      const labelElement = await this.page.getByRole("label").filter({ hasText: checklist.AcqSection6Expropriation[i].title });
      await expect(labelElement).toBeVisible();

      const selectElement = await this.page.locator(`#input-checklistSections[4].items[${i}].statusType`);
      await expect(selectElement).toBeVisible();
      await selectElement.selectOption({ label: checklist.AcqSection6Expropriation[i].status });
    }

    //Acquisition Completion
    const checklistAcqCompletionTitle = await this.page.locator("//h2/div/div[contains(text(),'Acquisition Completion')]");
    await expect(checklistAcqCompletionTitle).toBeVisible();

    for(var i = 0; i < checklist.AcqSection6Expropriation.length(); i++) {
      const labelElement = await this.page.getByRole("label").filter({ hasText: checklist.AcqSection6Expropriation[i].title });
      await expect(labelElement).toBeVisible();

      const selectElement = await this.page.locator(`#input-checklistSections[5].items[${i}].statusType`);
      await expect(selectElement).toBeVisible();
      await selectElement.selectOption({ label: checklist.AcqSection6Expropriation[i].status });
    }
  }

  async verifyChecklistViewForm(checklist) {

    //File Initiation
    const checklistFileInitiationTitle = await this.page.locator("//h2/div/div[contains(text(),'File Initiation')]");
    await expect(checklistFileInitiationTitle).toBeVisible();

    for(var i = 0; i < checklist.AcqFileInitiation.length(); i++) {
      const labelElement = await this.page.getByRole("label").filter({ hasText: checklist.AcqFileInitiation[i].title });
      await expect(labelElement).toBeVisible();

      const valueElement = await this.page.locator(`//label[contains(text(),'${checklist.AcqFileInitiation[i].title}')]/parent::div/following-sibling::div/div/div[2]/span`);
      await expect(valueElement).toHaveText(checklist.AcqFileInitiation[i].status);
    }

    //Active File Management
    const checklistActiveFileMgmtTitle = await this.page.locator("//h2/div/div[contains(text(),'Active File Management')]");
    await expect(checklistActiveFileMgmtTitle).toBeVisible();

    for(var i = 0; i < checklist.AcqActiveFileManagement.length(); i++) {
      const labelElement = await this.page.getByRole("label").filter({ hasText: checklist.AcqActiveFileManagement[i].title });
      await expect(labelElement).toBeVisible();

      const valueElement = await this.page.locator(`//label[contains(text(),'${checklist.AcqActiveFileManagement[i].title}')]/parent::div/following-sibling::div/div/div[2]/span`);
      await expect(valueElement).toHaveText(checklist.AcqActiveFileManagement[i].status);
    }

    //Crown Land
    const checklistCrownLandTitle = await this.page.locator("//h2/div/div[contains(text(),'Crown Land')]");
    await expect(checklistCrownLandTitle).toBeVisible();

    for(var i = 0; i < checklist.AcqCrownLand.length(); i++) {
      const labelElement = await this.page.getByRole("label").filter({ hasText: checklist.AcqCrownLand[i].title });
      await expect(labelElement).toBeVisible();

      const valueElement = await this.page.locator(`//label[contains(text(),'${checklist.AcqCrownLand[i].title}')]/parent::div/following-sibling::div/div/div[2]/span`);
      await expect(valueElement).toHaveText(checklist.AcqCrownLand[i].status);
    }

    //Section 3 - Agreement
    const checklistSection3Title = await this.page.locator("//h2/div/div[contains(text(),'Section 3 - Agreement')]");
    await expect(checklistSection3Title).toBeVisible();

    for(var i = 0; i < checklist.AcqSection3Agreement.length(); i++) {
      const labelElement = await this.page.getByRole("label").filter({ hasText: checklist.AcqSection3Agreement[i].title });
      await expect(labelElement).toBeVisible();

      const valueElement = await this.page.locator(`//label[contains(text(),'${checklist.AcqSection3Agreement[i].title}')]/parent::div/following-sibling::div/div/div[2]/span`);
      await expect(valueElement).toHaveText(checklist.AcqSection3Agreement[i].status);
    }

    //Section 6 - Expropriation
    const checklistSection6Title = await this.page.locator("//h2/div/div[contains(text(),'Section 6 - Expropriation')]");
    await expect(checklistSection6Title).toBeVisible();

    for(var i = 0; i < checklist.AcqSection6Expropriation.length(); i++) {
      const labelElement = await this.page.getByRole("label").filter({ hasText: checklist.AcqSection6Expropriation[i].title });
      await expect(labelElement).toBeVisible();

      const valueElement = await this.page.locator(`//label[contains(text(),'${checklist.AcqSection6Expropriation[i].title}')]/parent::div/following-sibling::div/div/div[2]/span`);
      await expect(valueElement).toHaveText(checklist.AcqSection6Expropriation[i].status);
    }

    //Acquisition Completion
    const checklistAcqCompletionTitle = await this.page.locator("//h2/div/div[contains(text(),'Acquisition Completion')]");
    await expect(checklistAcqCompletionTitle).toBeVisible();

    for(var i = 0; i < checklist.AcquisitionCompletion.length(); i++) {
      const labelElement = await this.page.getByRole("label").filter({ hasText: checklist.AcquisitionCompletion[i].title });
      await expect(labelElement).toBeVisible();

      const valueElement = await this.page.locator(`//label[contains(text(),'${checklist.AcquisitionCompletion[i].title}')]/parent::div/following-sibling::div/div/div[2]/span`);
      await expect(valueElement).toHaveText(checklist.AcquisitionCompletion[i].status);
    }
  }

  async saveAcquisitionFileChecklist() {
    await this.page.locator('button:has-text("Save")').click();

    const checklistEditBtn = this.page.locator("//button[@title='Edit checklist']");
    await expect(checklistEditBtn).toBeVisible();
  }
}

module.exports = { AcquisitionChecklist };
