import test, { BrowserContext, expect, Page } from '@playwright/test';
import { ResearchListPage } from '../../../pages/research/research-list.page';

let context: BrowserContext;
let page: Page;
let researchListPage: ResearchListPage;

test.describe('Research Files feature', () => {
  test.beforeAll(async ({ browser }) => {
    context = await browser.newContext();
    page = await context.newPage();
    researchListPage = new ResearchListPage(page);
  });

  test.afterAll(async () => {
    await context.close();
  });

  test('verify research list view', async () => {
    //Navigate to research list view and pick an existing research file
    await researchListPage.goto();

    //Filter Elements
    await expect(researchListPage.researchListTitle).toBeVisible();
    await expect(researchListPage.researchNewButton).toBeVisible();
    await expect(researchListPage.researcSearchByLabel).toBeVisible();
    await expect(researchListPage.researchSearchRoadInput).toBeVisible();
    await expect(researchListPage.researchSearchBySelect).toBeVisible();
    await expect(researchListPage.researchSearchPidInput).toBeVisible();
    await expect(researchListPage.researchSearchRegionsMultiSelect).toBeVisible();
    await expect(researchListPage.researchSearchDateSelect).toBeVisible();
    await expect(researchListPage.researchSearchDateToInput).toBeVisible();
    await expect(researchListPage.researchSearchDateFromInput).toBeVisible();
    await expect(researchListPage.researchSearchRegionsMultiSelect).toBeVisible();
    await expect(researchListPage.researchSearchUserSelect).toBeVisible();
    await expect(researchListPage.researchSearchUserInput).toBeVisible();
    await expect(researchListPage.researchSearchButton).toBeVisible();
    await expect(researchListPage.researchSearchResetButton).toBeVisible();

    //Research Files Table Columns
    await expect(researchListPage.researchTable).toBeVisible();
    await expect(researchListPage.researchTableFileNbrHeader).toBeVisible();
    await expect(researchListPage.researchTableOrderByFileNbr).toBeVisible();
    await expect(researchListPage.researchTableFileNameHeader).toBeVisible();
    await expect(researchListPage.researchTableOrderByName).toBeVisible();
    await expect(researchListPage.researchTableMotiRegionHeader).toBeVisible();
    await expect(researchListPage.researchTableCreatedByHeader).toBeVisible();
    await expect(researchListPage.researchTableOrderByCreatedBy).toBeVisible();
    await expect(researchListPage.researchTableOrderCreatedDate).toBeVisible();
    await expect(researchListPage.researchTableLastUpdatedByHeader).toBeVisible();
    await expect(researchListPage.researchTableOrderLastUpdated).toBeVisible();
    await expect(researchListPage.researchTableLastUpdatedDateHeader).toBeVisible();
    await expect(researchListPage.researchTableOrderUpdatedDate).toBeVisible();
    await expect(researchListPage.researchTableStatusHeader).toBeVisible();
    await expect(researchListPage.researchTableOrderStatus).toBeVisible();

    //Research File Table Content elements
    await expect(researchListPage.researchTableContent).toBeVisible();

    //Research File Pagination elements
    await expect(researchListPage.researchTableEntriesSpan).toBeVisible();
    await expect(researchListPage.researchTableNextPageButton).toBeVisible();
    await expect(researchListPage.researchTable1stPageButton).toBeVisible();
  });
});
