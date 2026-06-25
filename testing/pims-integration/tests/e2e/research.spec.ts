import test, { BrowserContext, expect, Page } from '@playwright/test';
import { ResearchCreatePage } from '../../pages/research/research-create.page';
import { DocumentsListPage } from '../../pages/documents/documents-list.page';
import { ResearchListPage } from '../../pages/research/research-list.page';
import { ResearchViewFileDetails } from '../../pages/research/research-view-file-details.page';
import { DocumentUploadModalPage } from '../../pages/documents/document-upload-modal.page';
import path from 'path';

let context: BrowserContext;
let page: Page;
let researchListPage: ResearchListPage;
let researchCreatePage: ResearchCreatePage;
let researchViewDetails: ResearchViewFileDetails;
let documentsListPage: DocumentsListPage;
let documentUploadModalPage: DocumentUploadModalPage;

test.describe('Research Files feature', () => {
  test.beforeAll(async ({ browser }) => {
    context = await browser.newContext();
    page = await context.newPage();
    researchListPage = new ResearchListPage(page);
    researchCreatePage = new ResearchCreatePage(page);
    researchViewDetails = new ResearchViewFileDetails(page);
    documentsListPage = new DocumentsListPage(page);
    documentUploadModalPage = new DocumentUploadModalPage(page);
  });

  test.afterAll(async () => {
    await context.close();
  });

  test('create new research file', async () => {
    //Navigate to new research file and create a minimum viable research
    await researchCreatePage.goto();
    await researchCreatePage.setResearchNameValue('researchFile');
    await researchCreatePage.confirmButtonClick();
    await expect(researchViewDetails.researchDocumentTab).toBeVisible();
  });

  test('verify research view form', async () => {
    //Navigate to Research list view
    await researchListPage.goto();

    //Search researh file by name
    await researchListPage.searchByName('Bubba BBQ Whirled');

    const [newPage] = await Promise.all([
      context.waitForEvent('page'),
      researchListPage.clickResearchFileResult(1),
    ]);

    const responsePromise = newPage.waitForResponse(
      (response: { url: () => string | string[]; status: () => number }) =>
        response.url().includes('/api/researchFiles/') && response.status() === 200
    );

    await newPage.waitForLoadState('domcontentloaded');

    const response = await responsePromise;
    const apiResearchFile = await response.json();

    const researchViewDetails = new ResearchViewFileDetails(newPage);

    console.log('############################');
    console.log('FOUND THE API RESPONSE');
    console.log(response.url());
    console.log(JSON.stringify(apiResearchFile, null, 2));
    console.log('############################');

    await expect(researchViewDetails.researchDocumentTab).toBeVisible();
    const fieldsToCompare = [
      { label: 'Ministry project', apiValue: apiResearchFile.researchFileProjects[0] },
      { label: 'Road name', apiValue: apiResearchFile.roadName },
      { label: 'Road alias', apiValue: apiResearchFile.roadAlias },
      {
        label: 'Research purpose',
        apiValue: apiResearchFile.researchFilePurposes[0].researchPurposeTypeCode.description,
      },
      { label: 'Source of request', apiValue: apiResearchFile.requestSourceType.description },
      { label: 'Requester', apiValue: apiResearchFile.requestorPerson },
    ];

    const datesToCompare = [
      { label: 'Request date', apiValue: apiResearchFile.requestDate },
      { label: 'Research completed on', apiValue: apiResearchFile.researchCompletionDate },
    ];

    for (const field of fieldsToCompare) {
      const uiValue = await researchViewDetails.getFieldValueByLabel(field.label);
      expect(researchViewDetails.normalize(uiValue)).toBe(
        researchViewDetails.normalize(field.apiValue)
      );
    }

    for (const date of datesToCompare) {
      const uiValue = await researchViewDetails.getFieldValueByLabel(date.label);
      const changedFormat = researchViewDetails.formatApiDate(date.apiValue);
      expect(researchViewDetails.normalize(uiValue)).toBe(changedFormat);
    }

    const isExpropriation = await researchViewDetails.getFieldValueByLabel('Expropriation?');
    const formatedBoolean = researchViewDetails.formatApiBoolean(apiResearchFile.isExpropriation);
    expect(researchViewDetails.normalize(isExpropriation)).toBe(formatedBoolean);

    const description = await researchViewDetails.getResearchDescription();
    expect(description).toBe(apiResearchFile.requestDescription ?? '');

    const result = await researchViewDetails.getResearchResult();
    expect(result).toBe(apiResearchFile.researchResult ?? '');

    const exproNotes = await researchViewDetails.getResearchExpropriationNotes();
    expect(exproNotes).toBe(apiResearchFile.expropriationNotes ?? '');
  });

  test('verify documents invalid types', async () => {
    //Navigate to research list view and pick an existing research file
    await researchListPage.goto();
    const newPage = await researchListPage.openResearchFileInNewTab(1);
    researchViewDetails = new ResearchViewFileDetails(newPage);
    documentsListPage = new DocumentsListPage(newPage);
    documentUploadModalPage = new DocumentUploadModalPage(newPage);

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
    expect(errorLabel).toBe('File type not supported!');

    const fileName = await documentUploadModalPage.getDocumentErrorFilename();
    expect(fileName).toBe('react-icon.svg');
  });
});
