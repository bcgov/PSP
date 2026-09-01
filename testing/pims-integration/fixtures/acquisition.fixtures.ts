import { expect, test as base } from '@playwright/test';
import { AcquisitionCreatePage } from '../pages/acquisition/acquisition-create.page';
import { AcquisitionSummaryPage } from '../pages/acquisition/acquisition-summary.page';
import { generateFileName } from '../utils/utils';

type AcquisitionWithNoticeOfClaim = {
  fileName: string;
  receivedDate: string;
};

type AcquisitionFixtures = {
  acquisitionCreatePage: AcquisitionCreatePage;
  acquisitionSummaryPage: AcquisitionSummaryPage;
  acquisitionWithNoticeOfClaim: AcquisitionWithNoticeOfClaim;
};

export const test = base.extend<AcquisitionFixtures>({
  acquisitionCreatePage: async ({ page }, use) => {
    const acquisitionCreatePage = new AcquisitionCreatePage(page);

    await use(acquisitionCreatePage);
  },

  acquisitionSummaryPage: async ({ page }, use) => {
    const acquisitionSummaryPage = new AcquisitionSummaryPage(page);

    await use(acquisitionSummaryPage);
  },

  acquisitionWithNoticeOfClaim: async (
    {
      page,
      acquisitionCreatePage,
      acquisitionSummaryPage,
    },
    use
  ) => {
    const receivedDate = 'Aug 15, 2026';
    const fileName = generateFileName('Acquisition');

    // Test setup
    await acquisitionCreatePage.goto();

    await acquisitionCreatePage.setFileNameInput(fileName);
    await acquisitionCreatePage.selectAcquisitionType('CONSEN');
    await acquisitionCreatePage.selectRegion();
    await acquisitionCreatePage.setNoticeOfClaimReceivedDate(receivedDate);

    const responsePromise = page.waitForResponse(
      response =>
        response.url().includes('/api/acquisitionfiles') &&
        response.request().method() === 'POST'
    );

    await acquisitionCreatePage.confirmButtonClick();

    const response = await responsePromise;

    if (!response.ok()) {
        const responseBody = await response.text();

        throw new Error(
            `Acquisition creation failed: ${response.status()} ${response.url()}\n${responseBody}`
        );
    }

    await acquisitionSummaryPage.fileDetailsTab.waitFor({
      state: 'visible',
      timeout: 15_000,
    });

    await use({
      fileName,
      receivedDate,
    });
  },
});

export { expect };