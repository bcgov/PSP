const { expect } = require("@playwright/test");
const { clickSaveButton } = require("../../support/common.js");

class Notes {
  constructor(page) {
    this.page = page;
  }

  async navigateNotesTab() {
    await this.page
      .locator("nav[role='tablist'] a[data-rb-event-key='notes']")
      .click();
  }

  async createNotesTabButton() {
    await this.page.getByTestId("note-add-button").click();
  }

  async addNewNoteDetails(note) {
    await this.page.getByTestId("note-field").fill(note);
  }

  async viewSecondNoteDetails() {
    await this.page.getByTestId("view-note-1").click();
  }

  async editNoteButton() {
    await this.page.locator("button[aria-label='edit']").click();
  }

  async editNote(note) {
    await this.page.getByTestId("note-field").fill("");
    await this.page.getByTestId("note-field").fill(note);
  }

  async saveNote() {
    await this.page.getByTestId("ok-modal-button").click();
  }

  async cancelNote() {
    await this.page.getByTestId("cancel-modal-button").click();

    if ((await this.page.locator("div[class='modal-content']").count()) == 2) {
      await expect(
        this.page.locator("div[class='modal-title h4']").last()
      ).toHaveText("Confirm Changes");
      await expect(
        this.page.locator("div[class='modal-body'] p").first()
      ).toHaveText(
        "If you choose to cancel now, your changes will not be saved."
      );
      await expect(
        this.page.locator("div[class='modal-body'] p").last()
      ).toHaveText("Do you want to proceed?");

      await this.page.getByTestId("ok-modal-button").last().click();
    }
  }

  async deleteLastSecondNote() {
    await this.page.getByTestId("remove-note-1").click();

    await expect(this.page.locator("div[class='modal-title h4']")).toHaveText(
      "Delete Note"
    );
    expect(this.page.locator("div[class='modal-body']")).toHaveText(
      "Are you sure you want to delete this note?"
    );
    await this.page.getByTestId("ok-modal-button").click();
  }

  async verifyNotesAddNew() {
    const titleText =
      (
        await this.page.locator("div[class='modal-title h4']").textContent()
      )?.trim() || "";
    expect(titleText).toEqual("Notes");

    const subtitleText =
      (
        await this.page.locator("label[for='input-note.note']").textContent()
      )?.trim() || "";
    expect(subtitleText).toEqual("Type a note:");

    await expect(this.page.getByTestId("note-field")).toBeVisible();
    await expect(this.page.getByTestId("cancel-modal-button")).toBeVisible();
    await expect(this.page.getByTestId("ok-modal-button")).toBeVisible();
  }

  async verifyNotesEditForm() {
    expect(
      this.page.locator("//div[normalize-space()='Created:']")
    ).toBeVisible();
    expect(
      this.page.locator(
        "//div[normalize-space()='Created:']/following-sibling::div/span/strong"
      )
    ).toBeVisible();
    expect(
      this.page.locator("//div[normalize-space()='Last updated:']")
    ).toBeVisible();
    expect(
      this.page.locator(
        "//div[normalize-space()='Last updated:']/following-sibling::div/span/strong"
      )
    ).toBeVisible();

    await this.page
      .locator("div[class='modal-body']")
      .waitFor({ state: "visible" });
    const username = await this.page
      .locator(
        "div[class='modal-body'] span[data-testid='tooltip-icon-userNameTooltip']"
      )
      .count();

    expect(username).toBe(2);

    const labelText =
      (
        await this.page.locator("label[for='input-note']").textContent()
      )?.trim() || "";
    expect(labelText).toEqual("Type a note:");

    expect(this.page.getByTestId("note-field")).toBeVisible();
    expect(this.page.getByTestId("cancel-modal-button")).toBeVisible();
    expect(this.page.getByTestId("ok-modal-button")).toBeVisible();
  }

  async verifyNotesTabListView() {
    const notesHeader = await this.page
      .getByTestId("notes-header")
      .textContent();
    expect(notesHeader).toEqual("Notes");
    expect(this.page.getByTestId("note-add-button")).toBeVisible();

    await expect(
      this.page
        .locator(
          "//div[@data-testid='notesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Note')]"
        )
        .first()
    ).toBeVisible();
    await expect(
      this.page
        .locator(
          "//div[@data-testid='notesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Created date')]"
        )
        .first()
    ).toBeVisible();
    await expect(
      this.page
        .locator(
          "//div[@data-testid='notesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Last updated by')]"
        )
        .first()
    ).toBeVisible();
    await expect(
      this.page
        .locator(
          "//div[@data-testid='notesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Actions')]"
        )
        .first()
    ).toBeVisible();

    if (
      (await this.page
        .locator(
          "//div[@data-testid='notesTable']/div[@class='tbody']/div[@class='tr-wrapper']"
        )
        .count()) > 0
    ) {
      await expect(
        this.page
          .locator("//div[@data-testid='notesTable']/div[@class='tbody']")
          .first()
      ).toBeVisible();
    } else {
      await expect(
        this.page
          .locator("//div[contains(text(),'No matching Notes found')]")
          .first()
      ).toBeVisible();
    }
  }

  async notesTabCount() {
    await this.page
      .locator(
        "//div[@data-testid='notesTable']/div[@class='tbody']/div[@class='tr-wrapper']"
      )
      .first()
      .waitFor({ state: "visible" });
    const totalRecords = this.page
      .locator(
        "//div[@data-testid='notesTable']/div[@class='tbody']/div[@class='tr-wrapper']"
      )
      .count();
    return totalRecords;
  }

  async verifyAutomaticNotes(fileType, fromStatus, toStatus) {
    await expect(
      this.page.locator(
        "div[data-testid='notesTable'] div[class='tbody'] div[class='tr-wrapper'] div:first-child div[role='cell']:first-child"
      )
    ).toHaveText(
      fileType + " status changed from " + fromStatus + " to " + toStatus
    );
  }

  async verifyAutomaticNotesCompensation(
    compensationNbr,
    fromStatus,
    toStatus
  ) {
    await expect(
      this.page.locator(
        "div[data-testid='notesTable'] div[class='tbody'] div[class='tr-wrapper'] div:first-child div[role='cell']:first-child"
      )
    ).toHaveText(
      "Compensation Requisition with # " +
        compensationNbr +
        ", changed status from '" +
        fromStatus +
        "' to '" +
        toStatus +
        "'"
    );
  }
}

module.exports = Notes;
