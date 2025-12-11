const { expect } = require("@playwright/test");
const { clickAndWaitFor } = require("../../support/common.js");

class HelpDesk {
  constructor(page) {
    this.page = page;
  }

  async navigateHelpDeskForm() {
    clickAndWaitFor(
      this.page,
      "header div[class='nav-item'] div",
      "div[class='modal-content']"
    );
  }

  async verifyHelpDeskModal() {
    const helpDeskTitle =
      (
        await this.page.locator("div[class='modal-title h4']").textContent()
      )?.trim() || "";
    expect(helpDeskTitle).toEqual("Help Desk");

    const helpDeskSubtitle =
      (
        await this.page
          .locator("div[class='modal-body'] h3:first-child")
          .textContent()
      )?.trim() || "";
    expect(helpDeskSubtitle).toEqual("Get started with PIMS");

    const helpDeskInstructions = await this.page
      .locator("div[class='modal-body'] p")
      .first()
      .textContent();
    expect(helpDeskInstructions).toEqual(
      "This overview has useful tools that will support you to start using the application. You can also watch the video demos."
    );

    const helpDeskPIMSResourcesLink = await this.page.locator(
      "//a[contains(text(),'PIMS Training Materials')]"
    );
    expect(helpDeskPIMSResourcesLink).toBeVisible();

    const helpDeskContactUsSubtitle = await this.page.locator(
      "//h3[contains(text(),'Contact us')]"
    );
    expect(helpDeskContactUsSubtitle).toBeVisible();

    const helpDeskUserLabel = await this.page.locator(
      "//label[contains(text(),'Name')]"
    );
    expect(helpDeskUserLabel).toBeVisible();

    const helpDeskUserInput = await this.page.locator("#input-user");
    expect(helpDeskUserInput).toBeVisible();

    const helpDeskEmailLabel = await this.page.locator(
      "//label[contains(text(),'Email')]"
    );
    expect(helpDeskEmailLabel).toBeVisible();

    const helpDeskEmailInput = await this.page.locator("#input-email");
    expect(helpDeskEmailInput).toBeVisible();

    const helpDeskDescriptionLabel = await this.page.locator(
      "//div[@class='modal-body']/div/form/div/div/label[contains(text(),'Description')]"
    );
    expect(helpDeskDescriptionLabel).toBeVisible();

    const helpDeskDescriptionInput = await this.page.locator(
      "#input-description"
    );
    expect(helpDeskDescriptionInput).toBeVisible();

    const helpDeskFooterQuestion = await this.page
      .locator("div[class='modal-body'] p")
      .last()
      .textContent();
    expect(helpDeskFooterQuestion).toEqual(
      "Do you want to proceed and send the email?"
    );

    const noButton = await this.page.locator(
      "button[data-testid='cancel-modal-button']"
    );
    expect(noButton).toBeVisible();

    const yesButton = await this.page.locator(
      "div[class='modal-footer'] a[data-testid='ok-modal-button']"
    );
    expect(yesButton).toBeVisible();
  }

  async cancelHelpDeskModal() {
    await this.page
      .locator("button[data-testid='cancel-modal-button']")
      .click();
  }
}

module.exports = HelpDesk;
