import { BrowserContext, Page } from '@playwright/test';
import { ConsolidationPage } from '../../../pages/consolidation-subdivision/consolidation.page';
import {
  ConsolidationSubdivisionHistoryPage,
  ConSubHistory,
} from '../../../pages/consolidation-subdivision/consolidation-subdivision-history.page';
import { consolidationTest } from '../../../fixtures/consolidation-subdivision.fixture';

type ConsolidationApiResponse = {
  sourceProperty: ConSubHistory[];
  destinationProperty: ConSubHistory;
};

let context: BrowserContext;
let page: Page;

let consolidationPage: ConsolidationPage;
let consolidationHistoryPage: ConsolidationSubdivisionHistoryPage;

consolidationTest.describe('Consolidation feature', () => {
  consolidationTest.beforeAll(async ({ browser }) => {
    context = await browser.newContext();
    page = await context.newPage();
    consolidationPage = new ConsolidationPage(page);
    consolidationHistoryPage = new ConsolidationSubdivisionHistoryPage(page);
  });

  consolidationTest.afterAll(async () => {
    await context.close();
  });

  consolidationTest('new consolidation', async () => {
    const parentsProperties = [
      { pid: '015-380-483', plan: 'NO_PLAN', status: 'RETIRED', area: '1,200.1212' },
      { pid: '005-565-405', plan: 'NWP56954', status: 'RETIRED', area: '1,200.1212' },
    ];
    const childProperty = {
      pid: '001-046-748',
      plan: 'NWP42089',
      status: 'ACTIVE',
      area: '1,200.1212',
    };

    let apiFeatureFileJson: ConsolidationApiResponse;
    const responsePromise = page.waitForResponse(
      (response: { url: () => string | string[]; status: () => number }) =>
        response.url().includes('/api/property') && response.status() === 200
    );

    await consolidationTest.step('Create consolidation', async () => {
      //Navigate to consolidation and create a minimum viable consolidation
      await consolidationPage.goto();
      await consolidationPage.createConsolidation(parentsProperties, childProperty);
      await consolidationPage.saveConsolidation();

      const response = await responsePromise;
      apiFeatureFileJson = await response.json();

      await consolidationPage.waitForPropertyPanel();
    });

    await consolidationTest.step('Validate consolidation history', async () => {
      await consolidationHistoryPage.verifyConsolidationHistory(
        apiFeatureFileJson.sourceProperty,
        apiFeatureFileJson.destinationProperty
      );
    });
  });
});
