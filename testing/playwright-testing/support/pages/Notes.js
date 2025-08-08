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

  async editNote(note) {
    await this.page.locator("button[aria-label='edit']").click();

    await this.page.getByTestId("note-field").fill("");
    await this.page.getByTestId("note-field").fill(note);
  }

  async cancelNote() {
    await this.page.getByTestId("cancel-modal-button").click();

    if ((await this.page.locator("div[class='modal-content']").count()) == 2) {
      await expect(
        this.page.locator("div[class='modal-title h4']").last()
      ).toHaveTextContent("Confirm Changes");
      await expect(
        this.page.locator("div[class='modal-body'] p").last()
      ).toHaveTextContent(
        "If you choose to cancel now, your changes will not be saved."
      );
      await expect(
        this.page.locator("div[class='modal-body'] p").last()
      ).toHaveTextContent("Do you want to proceed?");

      await this.page.getByTestId("cancel-modal-button").last().click();
    }
  }

  async deleteSecondNote() {
    await this.page.getByTestId("remove-note-1").click();

    await expect(
      this.page.locator("div[class='modal-title h4']")
    ).toHaveTextContent("Delete Note");
    await expect(
      this.page.locator("div[class='modal-body']")
    ).toHaveTextContent("Are you sure you want to delete this note?");
    await expect(this.page.getByTestId("ok-modal-button")).click();
  }

  async verifyNotesAddNew() {
    await expect(
      this.page.locator("div[class='modal-title h4']")
    ).toHaveTextContent("Notes");
    await expect(
      this.page.locator("label[for='input-note.note']")
    ).toHaveTextContent("Type a note:");
    await expect(this.page.getByTestId("note-field")).toBeVisible();
    await expect(this.page.getByTestId("cancel-modal-button")).toBeVisible();
    await expect(this.page.getByTest("ok-modal-button")).toBeVisible();
  }

  async verifyNotesEditForm() {
    await expect(this.page.getByTestId("notes-created-label")).toBeVisible();
    await expect(this.page.getByTestId("notes-created-date")).toBeVisible();
    await expect(this.page.getByTestId("notes-updated-label")).toBeVisible();
    await expect(this.page.getByTestId("notes-updated-date")).toBeVisible();
    await expect(
      this.page
        .locator(
          "div[class='modal-body'] span[data-testid='tooltip-icon-userNameTooltip']"
        )
        .count()
    ).toBe(2);

    await expect(
      this.page.locator("label[for='input-note.note']")
    ).toHaveTextContent("Type a note:");
    await expect(this.page.getByTestId("note-field")).toBeVisible();
    await expect(this.page.getByTestId("cancel-modal-button")).toBeVisible();
    await expect(this.page.getByTest("ok-modal-button")).toBeVisible();
  }

  async verifyNotesTabListView() {
    await expect(this.page.getByTestId("notes-header")).toHaveTextContent(
      "Notes"
    );
    await expect(this.page.getByTestId("note-add-button")).toBeVisible();

    await expect(
      this.page.locator(
        "//div[@data-testid='notesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Note')]"
      )
    ).toBeVisible();
    await expect(
      this.page.locator(
        "//div[@data-testid='notesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Created date')]"
      )
    ).toBeVisible();
    await expect(
      this.page.locator(
        "//div[@data-testid='notesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Last updated by')]"
      )
    ).toBeVisible();
    await expect(
      this.page.locator(
        "//div[@data-testid='notesTable']/div[@class='thead thead-light']/div/div/div[contains(text(),'Actions')]"
      )
    ).toBeVisible();

    if (
      (await this.page
        .locator(
          "//div[@data-testid='notesTable']/div[@class='tbody']/div[@class='tr-wrapper']"
        )
        .count()) > 0
    ) {
      await expect(
        this.page.locator(
          "//div[@data-testid='notesTable']/div[@class='tbody']"
        )
      ).toBeVisible();
    } else {
      await expect(
        this.page.locator("//div[contains(text(),'No matching Notes found')]")
      ).toBeVisible();
    }
  }

  async notesTabCount() {
    return await this.page
      .locator(
        "//div[@data-testid='notesTable']/div[@class='tbody']/div[@class='tr-wrapper']"
      )
      .count();
  }

  async verifyAutomaticNotes(fileType, fromStatus, toStatus) {
    await expect(
      this.page.locator(
        "div[data-testid='notesTable'] div[class='tbody'] div[class='tr-wrapper'] div:nth-child(1) div:nth-child(1)"
      )
    ).toHaveTextContent(
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
        "div[data-testid='notesTable'] div[class='tbody'] div[class='tr-wrapper'] div:nth-child(1) div:nth-child(1)"
      )
    ).toHaveTextContent(
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

module.exports = { Notes };
