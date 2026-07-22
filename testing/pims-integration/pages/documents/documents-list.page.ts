import { Page, Locator } from '@playwright/test';

export class DocumentsListPage {
  readonly page: Page;
  readonly addDocumentButton: Locator;
  readonly refreshDocumentListButton: Locator;
  readonly documentTypesDropDownList: Locator;
  readonly documentStatusesDropDownList: Locator;
  readonly documentFileNameInput: Locator;
  readonly documentSearchButton: Locator;
  readonly documentSearchResetButton: Locator;

  constructor(page: Page) {
    this.page = page;
    this.addDocumentButton = page.getByTestId('add-document-btn');
    this.refreshDocumentListButton = page.getByTestId('refresh-button');
    this.documentTypesDropDownList = page.getByTestId('document-type');
    this.documentStatusesDropDownList = page.getByTestId('document-status');
    this.documentFileNameInput = page.getByTestId('document-filename');
    this.documentSearchButton = page.getByTestId('search');
    this.documentSearchResetButton = page.getByTestId('reset-button');
  }

  async addDocumentButtonClick() {
    await this.addDocumentButton.click();
  }

  async refreshDocumentListClick() {
    this.refreshDocumentListButton.click();
  }

  async documentSearchButtonClick() {
    this.documentSearchButton.click();
  }

  async documentSearchResetButtonClick() {
    this.documentSearchResetButton.click();
  }
}
