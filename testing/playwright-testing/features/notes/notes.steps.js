const { Given, When } = require("@cucumber/cucumber");
const { expect } = require("@playwright/test");
const { splitStringToArray } = require("../../support/common.js");
const notesData = require("../../data/Notes.json");

let notesList = [];

Given(
  "I create a new Note on the Notes Tab with row number {int}",
  async function (rowNbr) {
    notesList = splitStringToArray(notesData[rowNbr].Notes);

    //Navigate to the Notes Tab
    await this.notes.navigateNotesTab();
    await this.notes.verifyNotesTabListView();

    //Create a new note
    await this.notes.createNotesTabButton();
    await this.notes.verifyNotesAddNew();
    await this.notes.addNewNoteDetails(notesList[0]);

    //Cancel new note
    await this.notes.cancelNote();

    //Create a new note
    for (const note of notesList) {
      await this.notes.createNotesTabButton();
      await this.notes.addNewNoteDetails(note);
      await this.notes.saveNote();
    }
  }
);

When(
  "I edit a Note on the Notes Tab with row number {int}",
  async function (rowNbr) {
    notesList = splitStringToArray(notesData[rowNbr].Notes);

    //Navigate to the Notes Tab
    await this.notes.navigateNotesTab();

    //Edit note
    await this.notes.viewSecondNoteDetails();
    await this.notes.editNoteButton();
    await this.notes.verifyNotesEditForm();
    await this.notes.editNote(notesList[0]);

    //Cancel note's update
    await this.notes.cancelNote();

    //Edit note
    await this.notes.viewSecondNoteDetails();
    await this.notes.editNoteButton();
    await this.notes.editNote(notesList[0]);

    //Save changes
    await this.notes.saveNote();

    //Verify Pagination on the list view
    await this.sharedPagination.choosePaginationOption(5);
    const notesCount5 = await this.notes.notesTabCount();
    expect(notesCount5).toBe(5);

    await this.sharedPagination.choosePaginationOption(10);
    const notesCount10 = await this.notes.notesTabCount();
    expect(notesCount10).toBe(10);

    //Delete Note
    await this.notes.deleteLastSecondNote();
  }
);
