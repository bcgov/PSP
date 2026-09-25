import { expect, test as base } from '@playwright/test';
import { ConsolidationSubdivisionHistoryPage,  ConSubHistory } from '../pages/consolidation-subdivision/consolidation-subdivision-history.page';
import { ConsolidationPage } from '../pages/consolidation-subdivision/consolidation.page';
import { SubdivisionPage } from '../pages/consolidation-subdivision/subdivision.page'

type ConsolidationFixtures = {
  consolidationPage: ConsolidationPage;
  consolidationCreated: ConsolidationCreated;
  consolidationSubdivisionHistoryPage: ConsolidationSubdivisionHistoryPage;

};

type SubdivisionFixtures = {
  subdivisionPage: SubdivisionPage;
  subdivisionCreated: SubdivisionCreated;
  consolidationSubdivisionHistoryPage: ConsolidationSubdivisionHistoryPage;
};

type ConsolidationCreated = {
  parentProperties: ConSubHistory[];
  childProperty: ConSubHistory;
};

type SubdivisionCreated = {
  parentProperty: ConSubHistory;
  childrenProperties: ConSubHistory[];
};

export const consolidationTest = base.extend<ConsolidationFixtures>({
  consolidationPage: async ({ page }, use) => {
    const consolidationCreatePage = new ConsolidationPage(page);

    await use(consolidationCreatePage);
  },

  consolidationCreated: async ({ page, consolidationPage }, use) => {
    //Test setup
    const parentProperties = [{ pid: '015-380-483', plan: 'NO_PLAN', status: 'RETIRED', area: '1,200.1212'}, {pid: '005-565-405', plan: 'NWP56954', status: 'RETIRED', area: '1,200.1212'}];
    const childProperty = { pid: '001-046-748', plan: 'NWP42089', status: 'ACTIVE', area: '1,200.1212'};

    await consolidationPage.goto();
    await consolidationPage.createConsolidation(
        parentProperties,
        childProperty
    );

    await consolidationPage.saveConsolidation();

     const responsePromise = page.waitForResponse(
      (response) =>
        response.url().includes('/api/property') && response.request().method() === 'POST'
    );

    const response = await responsePromise;
    if (!response.ok()) {
      const responseBody = await response.text();

      throw new Error(
        `Consolidation creation failed: ${response.status()} ${response.url()}\n${responseBody}`
      );
    }

    await consolidationPage.propertyDetailsTab.waitFor({
      state: 'visible',
      timeout: 15_000,
    });

    await use({
        parentProperties,
        childProperty,
    });

  },
});

export const subdivisionTest = base.extend<SubdivisionFixtures>({
  subdivisionPage: async ({ page }, use) => {
    const subdivisionCreatePage = new SubdivisionPage(page);

    await use(subdivisionCreatePage);
  },

  subdivisionCreated: async ({ page, subdivisionPage }, use) => {
    //Test setup
    const parentProperty = { pid: '001-046-748', plan: 'NWP42089', status: 'ACTIVE', area: '1,200.1212'};
    const childrenProperties = [{ pid: '015-380-483', plan: 'NO_PLAN', status: 'RETIRED', area: '1,200.1212'}, {pid: '005-565-405', plan: 'NWP56954', status: 'RETIRED', area: '1,200.1212'}];

    await subdivisionPage.goto();
    await subdivisionPage.createSubdivision(
        parentProperty,
        childrenProperties
    );

    await subdivisionPage.saveSubdivision();

     const responsePromise = page.waitForResponse(
      (response) =>
        response.url().includes('/api/property') && response.request().method() === 'POST'
    );

    const response = await responsePromise;
    if (!response.ok()) {
      const responseBody = await response.text();

      throw new Error(
        `Subdivision creation failed: ${response.status()} ${response.url()}\n${responseBody}`
      );
    }

    await subdivisionPage.propertyDetailsTab.waitFor({
      state: 'visible',
      timeout: 15_000,
    });

    await use({
        parentProperty,
        childrenProperties,
    });

  },
});

export { expect };
