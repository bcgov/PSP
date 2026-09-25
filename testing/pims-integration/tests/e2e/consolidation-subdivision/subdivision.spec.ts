import { BrowserContext, Page } from '@playwright/test';
import { SubdivisionPage } from '../../../pages/consolidation-subdivision/subdivision.page';
import { ConsolidationSubdivisionHistoryPage, ConSubHistory } from '../../../pages/consolidation-subdivision/consolidation-subdivision-history.page';
import { subdivisionTest } from '../../../fixtures/consolidation-subdivision.fixture';

type SubdivisionApiResponse = {
  sourceProperty: ConSubHistory;
  destinationProperty: ConSubHistory[];
};

let context: BrowserContext;
let page: Page;

let subdivisionPage: SubdivisionPage;
let consolidationHistoryPage: ConsolidationSubdivisionHistoryPage;

subdivisionTest.describe('Consolidation feature', () => {
  subdivisionTest.beforeAll(async ({ browser }) => {
    context = await browser.newContext();
    page = await context.newPage();
    subdivisionPage = new SubdivisionPage(page);
    consolidationHistoryPage = new ConsolidationSubdivisionHistoryPage(page);
  });

  subdivisionTest.afterAll(async () => {
    await context.close();
  });

  subdivisionTest('new consolidation', async () => {
    const parentProperty = { pid: '001-046-748', plan: 'NWP42089', status: 'ACTIVE', area: '1,200.1212'};
    const childrenProperties = [{ pid: '015-380-483', plan: 'NO_PLAN', status: 'RETIRED', area: '1,200.1212'}, {pid: '005-565-405', plan: 'NWP56954', status: 'RETIRED', area: '1,200.1212'}];

    let apiFeatureFileJson: SubdivisionApiResponse;
    const responsePromise = page.waitForResponse(
      (response: { url: () => string | string[]; status: () => number }) =>
        response.url().includes('/api/property') && response.status() === 200
    );

    await subdivisionTest.step('Create consolidation', async () => {
      //Navigate to consolidation and create a minimum viable consolidation
      await subdivisionPage.goto();
      await subdivisionPage.createSubdivision(parentProperty, childrenProperties);
      await subdivisionPage.saveSubdivision();

      const response = await responsePromise;
      apiFeatureFileJson = await response.json();

      await subdivisionPage.waitForPropertyPanel();

    });

    await subdivisionTest.step('Validate Subdivision history', async () => {
      await consolidationHistoryPage.verifySubdivisionHistory(apiFeatureFileJson.sourceProperty, apiFeatureFileJson.destinationProperty);
    });
  });
});
