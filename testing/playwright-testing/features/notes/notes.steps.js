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
    this.notes.navigateNotesTab();
    this.notes.verifyNotesTabListView();

    //Create a new note
    this.notes.createNotesTabButton();
    this.notes.verifyNotesAddNew();
    this.notes.addNewNoteDetails(notesList[0]);

    //Cancel new note
    this.notes.cancelNote();

    //Create a new note
    notesList.forEach((note) => {
      this.notes.createNotesTabButton();
      this.notes.addNewNoteDetails(note);
      this.notes.saveNote();
    });
  }
);

When(
  "I edit a Note on the Notes Tab with row number {int}",
  async function (rowNbr) {
    notesList = splitStringToArray(notesData[rowNbr].Notes);

    //Navigate to the Notes Tab
    this.notes.navigateNotesTab();

    //Edit note
    this.notes.viewSecondNoteDetails();
    this.notes.verifyNotesEditForm();
    this.notes.editNote(notesList[0]);

    //Cancel note's update
    this.notes.cancelNote();

    //Edit note
    this.notes.viewSecondNoteDetails();
    this.notes.editNote(notesList[0]);

    //Save changes
    this.notes.saveNote();

    //Verify Pagination on the list view
    this.sharedPagination.choosePaginationOption(5);
    const notesCount5 = await this.notes.notesTabCount();
    expect(notesCount5).toBe(5);

    this.sharedPagination.choosePaginationOption(10);
    const notesCount10 = await this.notes.notesTabCount();
    expect(notesCount10).toBe(10);

    //Delete Note
    this.notes.deleteLastSecondNote();
  }
);
