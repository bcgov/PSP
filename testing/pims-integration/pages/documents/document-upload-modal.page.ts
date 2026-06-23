import { Page, Locator } from '@playwright/test';

export class DocumentUploadModalPage {
  readonly page: Page;
  readonly documentUploadDropZone: Locator;
  readonly documentInput: Locator;
  readonly supportedFileExtensions: Locator;
  readonly cancelModalButton: Locator;
  readonly confirmOkModalButton: Locator;

  constructor(page: Page) {
    this.page = page;
    this.documentUploadDropZone = page.getByTestId('document-upload-drop-zone');
    this.supportedFileExtensions = page.getByTestId('supported-file-extensions');
    this.documentInput = page.getByTestId('upload-input');
    this.cancelModalButton = page.getByTestId('cancel-modal-button');
    this.confirmOkModalButton = page.getByTestId('ok-modal-button');
  }

  async uploadDocument(fileName: string) {
    await this.documentInput.setInputFiles(fileName);
  }

  async getDocumentHeaderLabel(index: number = 0): Promise<string> {
    const documentHeaderWrapper = this.page.getByTestId(`document[${index}]-header`);
    const documentLabel = documentHeaderWrapper.locator(':nth-match(span, 1)');

    return await documentLabel.innerText();
  }

  async getDocumentHeaderFilename(index: number = 0): Promise<string> {
    const documentHeaderWrapper = this.page.getByTestId(`document[${index}]-header`);
    const filename = documentHeaderWrapper.locator(':nth-match(span, 2)');

    return await filename.innerText();
  }

  async getDocumentErrorLabel(index: number = 0): Promise<string> {
    const documentHeaderWrapper = this.page.locator(
      `//div[@data-testid='document[${index}]-error']/parent::div/following-sibling::div`
    );
    return await documentHeaderWrapper.innerText();
  }

  async getDocumentErrorFilename(index: number = 0): Promise<string> {
    const documentHeaderWrapper = this.page.getByTestId(`document[${index}]-error`);
    const filename = documentHeaderWrapper.locator(':nth-match(span, 2)');

    return await filename.innerText();
  }

  async getSupportedFileExtensionsText(): Promise<string> {
    return await this.supportedFileExtensions.innerText();
  }
}
