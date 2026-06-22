import test, { BrowserContext, expect, Page } from '@playwright/test';
import { ResearchCreatePage } from '../../pages/research/research-create.page';
import { DocumentsListPage } from '../../pages/documents/documents-list.page';
import { ResearchViewFileDetails } from '../../pages/research/research-view-file-details.page';
import { DocumentUploadModalPage } from '../../pages/documents/document-upload-modal.page';
import path from 'path';

let context: BrowserContext;
let page: Page;
let researchCreatePage: ResearchCreatePage;
let researchViewDetails: ResearchViewFileDetails;
let documentsListPage: DocumentsListPage;
let documentUploadModalPage: DocumentUploadModalPage;

test.describe('Research Files feature', () => {
  test.beforeAll(async ({ browser }) => {
    context = await browser.newContext();
    page = await context.newPage();
    researchCreatePage = new ResearchCreatePage(page);
    researchViewDetails = new ResearchViewFileDetails(page);
    documentsListPage = new DocumentsListPage(page);
    documentUploadModalPage = new DocumentUploadModalPage(page);
  });

  test.afterAll(async () => {
    await context.close();
  });

  test('verify documents invalid types', async () => {
    //Navigate to new research file and create a minimum viable research
    await researchCreatePage.goto();
    await researchCreatePage.fillMinimumResearchForm();
    await researchCreatePage.confirmButtonClick();
    await expect(researchViewDetails.researchDocumentTab).toBeVisible();

    //Navigate to the Document tab and create a new document
    await researchViewDetails.navigateDocumentsTab();
    await documentsListPage.addDocumentButtonClick();
    const documentTypes = await documentUploadModalPage.getSupportedFileExtensionsText();
    expect(documentTypes).toBe(
      'Supported file types include txt, pdf, docx, doc, xlsx, xls, html, odt, png, jpg, bmp, tif, tiff, jpeg, gif, shp, gml, kml, kmz, msg.'
    );

    //Insert an invalid document type
    const filePath = path.resolve(process.cwd(), 'fixtures', 'react-icon.svg');
    await documentUploadModalPage.uploadDocument(filePath);

    //Verify warning message appears
    const errorLabel = await documentUploadModalPage.getDocumentErrorLabel();
    expect(errorLabel).toContain('File');

    const fileName = await documentUploadModalPage.getDocumentErrorFilename();
    expect(fileName).toBe('react-icon.svg');
  });
});
