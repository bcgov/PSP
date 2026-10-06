import { Page, Locator } from '@playwright/test';

export class NotesListPage {
  readonly page: Page;
  readonly notesTabLink: Locator;
  readonly addNoteButton: Locator;

  readonly noteListTable: Locator;
  readonly noteListColumn: Locator;
  readonly noteListCreateDateColumn: Locator;
  readonly noteListUpdatedColumn: Locator;
  readonly noteListActionsColumn: Locator;

  constructor(page: Page) {
    this.page = page;
    this.notesTabLink = page.locator("a[data-rb-event-key='notes']");
    this.addNoteButton = page.getByTestId('add-document-btn');

    this.noteListTable = page.getByTestId('main-notes-section');
    this.noteListColumn = page.locator(
      "div[data-testid='notesTable'] div[class='thead thead-light'] div[role='columnheader']:first-child div"
    );
    this.noteListCreateDateColumn = page.locator(
      "div[data-testid='notesTable'] div[class='thead thead-light'] div[role='columnheader']:nth-child(2) div"
    );
    this.noteListUpdatedColumn = page.locator(
      "div[data-testid='notesTable'] div[class='thead thead-light'] div[role='columnheader']:nth-child(3) div"
    );
    this.noteListActionsColumn = page.locator(
      "div[data-testid='notesTable'] div[class='thead thead-light'] div[role='columnheader']:nth-child(4) div"
    );
  }

  async navigateNotesTab() {
    await this.notesTabLink.click();
  }
}
