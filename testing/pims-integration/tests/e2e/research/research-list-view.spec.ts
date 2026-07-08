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
    await expect(researchListPage.researchListTitle).toBeVisible();
  });
});
