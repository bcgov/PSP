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

  async ViewSecondNoteDetails() {
    Wait();
    webDriver.FindElement(notesTab2ndResultViewBttn).Click();
  }

  async EditNote(note) {
    WaitUntilClickable(notedEditBttn);
    webDriver.FindElement(notedEditBttn).Click();

    ClearInput(noteEditTextarea);
    webDriver.FindElement(noteEditTextarea).SendKeys(note);
  }

  async CancelNote() {
    WaitUntilClickable(notesAddDetailsCancelBttn);
    webDriver.FindElement(notesAddDetailsCancelBttn).Click();

    Wait();
    if (webDriver.FindElements(notesCancelPopupContent).Count() > 0) {
      AssertTrueIsDisplayed(notesCancelPopupHeader);
      Assert.Contains(
        "If you choose to cancel now, your changes will not be saved.",
        webDriver.FindElement(notesCancelPopupBody).Text
      );
      Assert.Contains(
        "Do you want to proceed?",
        webDriver.FindElement(notesCancelPopupBody).Text
      );

      Wait(2000);
      webDriver.FindElement(notesCancelOkBttn).Click();
    }
  }

  async DeleteLastSecondNote() {
    Wait(2000);
    webDriver.FindElement(note2ndDeleteNoteBttn).Click();

    WaitUntilVisible(notesDeletePopupHeader);
    AssertTrueContentEquals(notesDeletePopupHeader, "Delete Note");
    AssertTrueContentEquals(
      notesDeletePopupBody,
      "Are you sure you want to delete this note?"
    );

    webDriver.FindElement(notesDeleteOkBttn).Click();
  }

  async VerifyNotesAddNew() {
    AssertTrueIsDisplayed(notesAddDetailsHeader);
    AssertTrueContentEquals(notesAddDetailsHeader, "Notes");
    AssertTrueIsDisplayed(notesAddDetailsLabel);
    AssertTrueIsDisplayed(notesAddDetailsTextarea);
    AssertTrueIsDisplayed(notesAddDetailsSaveBttn);
    AssertTrueIsDisplayed(notesAddDetailsCancelBttn);
  }

  async VerifyNotesEditForm() {
    AssertTrueIsDisplayed(notesEditCreatedLabel);
    AssertTrueIsDisplayed(notesEditCreatedDate);
    AssertTrueIsDisplayed(notesEditCreatedBy);

    if (webDriver.FindElements(notesEditUpdatedLabel).Count > 0)
      AssertTrueIsDisplayed(notesEditUpdatedDate);

    AssertTrueIsDisplayed(notesEditUpdatedBy);
    AssertTrueIsDisplayed(notedEditBttn);
    AssertTrueIsDisplayed(noteEditViewTextarea);
    AssertTrueIsDisplayed(notesAddDetailsSaveBttn);
  }

  async VerifyNotesTabListView() {
    Wait(3000);

    AssertTrueIsDisplayed(notesTabTitle);
    AssertTrueIsDisplayed(notesTabAddBttn);
    AssertTrueIsDisplayed(notesTabTableHeaderNoteColumn);
    AssertTrueIsDisplayed(notesTabTableHeaderCreatedDateColumn);
    AssertTrueIsDisplayed(notesTabTableHeaderUpdatedByColumn);

    if (webDriver.FindElements(notesTabTableContentTotal).Count > 0)
      AssertTrueIsDisplayed(notesTabTableBody);
    else AssertTrueIsDisplayed(notesTabTableNoContent);
  }

  async NotesTabCount() {
    WaitUntilTableSpinnerDisappear();
    return webDriver.FindElements(notesTabTableContentTotal).Count();
  }

  async VerifyAutomaticNotes(fileType, fromStatus, toStatus) {
    WaitUntilTableSpinnerDisappear();

    WaitUntilVisibleText(
      note1stNoteContent,
      webDriver.FindElement(note1stNoteContent).Text
    );
    AssertTrueContentEquals(
      note1stNoteContent,
      fileType + " status changed from " + fromStatus + " to " + toStatus
    );
  }

  async VerifyAutomaticNotesCompensation(
    CompensationNbr,
    fromStatus,
    toStatus
  ) {
    Wait();
    AssertTrueContentEquals(
      note1stNoteContent,
      "Compensation Requisition with # " +
        CompensationNbr +
        ", changed status from '" +
        fromStatus +
        "' to '" +
        toStatus +
        "'"
    );
  }
}

module.exports = { Notes };
