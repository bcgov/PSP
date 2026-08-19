import { Page, Locator } from '@playwright/test';

export class NotesListPage {
    readonly page: Page;
    readonly addNoteButton: Locator;
    readonly noteListTable: Locator;
    readonly documentTypesDropDownList: Locator;
    readonly documentStatusesDropDownList: Locator;
    readonly documentFileNameInput: Locator;
    readonly documentSearchButton: Locator;
    readonly documentSearchResetButton: Locator;

    constructor(page: Page) {
        this.page = page;
        this.addNoteButton = page.getByTestId('add-document-btn');
        this.refreshNoteListButton = page.getByTestId('refresh-button');
        this.documentTypesDropDownList = page.getByTestId('document-type');
        this.documentStatusesDropDownList = page.getByTestId('document-status');
        this.documentFileNameInput = page.getByTestId('document-filename');
        this.documentSearchButton = page.getByTestId('document-file-search-button');
        this.documentSearchResetButton = page.getByTestId('document-file-search-reset-button');
    }
}