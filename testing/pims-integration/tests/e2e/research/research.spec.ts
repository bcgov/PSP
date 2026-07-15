import test, { BrowserContext, expect, Page } from '@playwright/test';
import { ResearchCreatePage } from '../../../pages/research/research-create.page';
import { DocumentsListPage } from '../../../pages/documents/documents-list.page';

import { ResearchViewFileDetails } from '../../../pages/research/research-view-file-details.page';
import { DocumentUploadModalPage } from '../../../pages/documents/document-upload-modal.page';
import { generateFileName, normalize, formatApiDate, formatApiBoolean } from '../../../utils/utils';
import path from 'path';
import { ResearchImprovementsPage } from '../../../pages/research/research-improvements-page';

let context: BrowserContext;
let page: Page;

let researchCreatePage: ResearchCreatePage;
let researchViewDetails: ResearchViewFileDetails;
let researchImprovements: ResearchImprovementsPage;
let documentsListPage: DocumentsListPage;
let documentUploadModalPage: DocumentUploadModalPage;

test.describe('Research Files feature', () => {
  test.beforeAll(async ({ browser }) => {
    context = await browser.newContext();
    page = await context.newPage();
    researchCreatePage = new ResearchCreatePage(page);
    researchViewDetails = new ResearchViewFileDetails(page);
    researchImprovements = new ResearchImprovementsPage(page);
    documentsListPage = new DocumentsListPage(page);
    documentUploadModalPage = new DocumentUploadModalPage(page);
  });

  test.afterAll(async () => {
    await context.close();
  });

  test('new research file', async () => {
    let apiFeatureFileJson: {
      isExpropriation: boolean | null;
      expropriationNotes: string;
      researchResult: string;
      requestDescription: string;
      researchFileProjects: any[];
      roadName: any;
      roadAlias: any;
      researchFilePurposes: { researchPurposeTypeCode: { description: any } }[];
      requestSourceType: { description: any };
      requestorPerson: any;
      requestDate: any;
      researchCompletionDate: any;
    };
    const responsePromise = page.waitForResponse(
      (response: { url: () => string | string[]; status: () => number }) =>
        response.url().includes('/api/researchFiles/') && response.status() === 200
    );

    await test.step('Create file', async () => {
      //Navigate to new research file and create a minimum viable research
      await researchCreatePage.goto();
      const newFileName = generateFileName('researchFile');
      await researchCreatePage.fillInField('#input-name', newFileName);
      await researchCreatePage.confirmButtonClick();

      const response = await responsePromise;
      apiFeatureFileJson = await response.json();

      await expect(researchViewDetails.researchDocumentTab).toBeVisible();
    });

    await test.step('Validate Research Details', async () => {
      const fieldsToCompare = [
        { label: 'Ministry project', apiValue: apiFeatureFileJson.researchFileProjects[0] },
        { label: 'Road name', apiValue: apiFeatureFileJson.roadName },
        { label: 'Road alias', apiValue: apiFeatureFileJson.roadAlias },
        {
          label: 'Research purpose',
          apiValue: apiFeatureFileJson.researchFilePurposes?.[0]?.researchPurposeTypeCode?.description ?? '',
        },
        { label: 'Source of request', apiValue: apiFeatureFileJson.requestSourceType?.description ?? '' },
        { label: 'Requester', apiValue: apiFeatureFileJson.requestorPerson },
      ];

      const datesToCompare = [
        { label: 'Request date', apiValue: apiFeatureFileJson.requestDate },
        //{ label: 'Research completed on', apiValue: apiFeatureFileJson.researchCompletionDate },
      ];

      for (const field of fieldsToCompare) {
        const uiValue = await researchViewDetails.getFieldValueByLabel(field.label);
        expect(normalize(uiValue)).toBe(normalize(field.apiValue));
      }

      for (const date of datesToCompare) {
        const uiValue = await researchViewDetails.getFieldValueByLabel(date.label);
        const changedFormat = formatApiDate(date.apiValue);
        expect(normalize(uiValue)).toBe(changedFormat);
      }

      const isExpropriation = await researchViewDetails.getFieldValueByLabel('Expropriation?');
      const formatedBoolean = formatApiBoolean(apiFeatureFileJson.isExpropriation);
      expect(normalize(isExpropriation)).toBe(formatedBoolean);

      const description = await researchViewDetails.getResearchDescription();
      expect(description).toBe(apiFeatureFileJson.requestDescription ?? '');

      const result = await researchViewDetails.getResearchResult();
      expect(result).toBe(apiFeatureFileJson.researchResult ?? '');

      const exproNotes = await researchViewDetails.getResearchExpropriationNotes();
      expect(exproNotes).toBe(apiFeatureFileJson.expropriationNotes ?? '');
    });

    await test.step('Validate Improvements Tab', async() =>{
      //Navigate to the research file improvements tab
      researchImprovements.improvementTabClick();

      //Verify initial elements
      await expect(researchImprovements.reseachImprovementsTooltip).toBeVisible();
      await expect(researchImprovements.reseachImprovementsInstructions).toBeVisible();
      await expect(researchImprovements.researchImprovementsProperties).toBeVisible();
    });

    await test.step('Validate Document Upload', async () => {
      //Navigate to the Document tab and create a new document
      await researchViewDetails.navigateDocumentsTab();

      //Verify initial elements
      await expect(documentsListPage.addDocumentButton).toBeVisible();
      await expect(documentsListPage.refreshDocumentListButton).toBeVisible();
      await expect(documentsListPage.documentTypesDropDownList).toBeVisible();
      await expect(documentsListPage.documentStatusesDropDownList).toBeVisible();
      await expect(documentsListPage.documentFileNameInput).toBeVisible();
      await expect(documentsListPage.documentSearchButton).toBeVisible();
      await expect(documentsListPage.documentSearchResetButton).toBeVisible();

      //Add a new document
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

    await test.step('Validate Notes tab', async () =>{

    });
  });
});
