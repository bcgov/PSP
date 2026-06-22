import test, { BrowserContext, expect, Page } from '@playwright/test';
import { ResearchCreatePage } from '../../pages/research/research-create.page';
import { DocumentListPage } from '../../pages/document/document-list.page';

let context: BrowserContext;
let page: Page;
let researchCreatePage: ResearchCreatePage;
let documentListPage: DocumentListPage;

test.describe('Research Files feature', () => {
  test.beforeAll(async ({ browser }) => {
    context = await browser.newContext();
    page = await context.newPage();
    researchCreatePage = new ResearchCreatePage(page);
    documentListPage = new DocumentListPage(page);
  });

  test.afterAll(async () => {
    await context.close();
  });

  test('verify documents invalid types', async () => {
    //Navigate to new research file and create a minimum viable research
    await researchCreatePage.goto();
    await researchCreatePage.fillMinimumResearchForm();
    await researchCreatePage.confirmButtonClick();
    await expect(documentListPage.documentTabLink).toBeVisible();

    //Navigate to the Document tab and create a new document
    await documentListPage.navigateDocumentsTab();
    await documentListPage.addNewDocument();
  });
});
