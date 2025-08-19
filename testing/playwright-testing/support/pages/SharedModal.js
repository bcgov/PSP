class SharedModal {
  constructor(page) {
    this.page = page;
  }

  async isModalContainerVisible() {
    return await this.page.locator("div[class='modal-content']").isVisible();
  }

  async isMoreThanOneModalPresent() {
    return await this.page.locator("div[class='modal-content']").count();
  }

  async mainModalHeader() {
    await this.page
      .locator("div[class='modal-header'] div[class='modal-title h4']")
      .waitFor({ state: "visible" });
    const title = await this.page.locator("div.modal-title.h4").textContent();
    return title;
  }

  async secondaryModalHeader() {
    return await this.page
      .locator("div[class='modal-title h4']")
      .nth(1)
      .textContent();
  }

  async mainModalContent() {
    return this.page.locator("div[class='modal-body']").textContent();
  }

  async secondaryModalContent() {
    return await this.page
      .locator("div[class='modal-body']")
      .nth(1)
      .textContent();
  }

  async mainModalClickOKBttn() {
    await this.page.getByTestId("ok-modal-button").click();
  }

  async secondaryModalClickOKBttn() {
    await this.page.getByTestId("ok-modal-button").nth(1).click();
  }

  async mainModalClickCancelBttn() {
    await this.page.getByTestId("cancel-modal-button").click();
  }

  async toastText() {
    return await this.page
      .locator("div[class='Toastify__toast-body']")
      .textContent();
  }

  async modalText1() {
    return await this.page.locator("div[class='modal-body'] div").textContent();
  }

  async modalText2() {
    return await this.page
      .locator("div[class='modal-body'] strong")
      .textContent();
  }

  async modalParagraph1() {
    return await this.page
      .locator("div[class='modal-body'] p:first-child")
      .textContent();
  }

  async modalParagraph2() {
    return await this.page
      .locator("div[class='modal-body'] p:last-child")
      .textContent();
  }

  async verifyModalButtonsPresence() {
    await expect(this.page.getByTestId("ok-modal-button")).toBeVisible();
    await expect(this.page.getByTestId("cancel-modal-button")).toBeVisible();
  }

  async cancelActionModal() {
    if (await this.isModalContainerVisible()) {
      await expect(this.mainModalHeaderContent()).resolves.toBe(
        "Confirm Changes"
      );
      await expect(this.mainModalContent()).resolves.toBe(
        "If you choose to cancel now, your changes will not be saved."
      );
      await expect(this.mainModalContent()).resolves.toContain(
        "Do you want to proceed?"
      );
      await this.modalClickOKBttn();
    }
  }
}

module.exports = SharedModal;
