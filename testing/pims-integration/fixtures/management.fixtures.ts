import { expect, test as base } from '@playwright/test';
import { ManagementCreatePage } from '../pages/management/management-create.page';
import { ManagementSummaryPage } from '../pages/management/management-summary.page';
import { generateFileName } from '../utils/utils';

type ManagementWithNoticeOfClaim = {
  fileName: string;
  receivedDate: string;
};

type ManagementFixtures = {
  managementCreatePage: ManagementCreatePage;
  managementSummaryPage: ManagementSummaryPage;
  managementWithNoticeOfClaim: ManagementWithNoticeOfClaim;
};

export const test = base.extend<ManagementFixtures>({
  managementCreatePage: async ({ page }, use) => {
    const managementCreatePage = new ManagementCreatePage(page);

    await use(managementCreatePage);
  },

  managementSummaryPage: async ({ page }, use) => {
    const managementSummaryPage = new ManagementSummaryPage(page);

    await use(managementSummaryPage);
  },

  managementWithNoticeOfClaim: async (
    { page, managementCreatePage, managementSummaryPage },
    use
  ) => {
    const receivedDate = 'Aug 15, 2026';
    const fileName = generateFileName('Management');

    // Test setup
    await managementCreatePage.goto();

    await managementCreatePage.setFileNameInput(fileName);
    await managementCreatePage.setFilePurposeInput('AGRICULT');
    await managementCreatePage.setNoticeOfClaimReceivedDateInput(receivedDate);

    const claimsDialog = page.getByRole('dialog').filter({
      hasText: 'Role claims mismatch',
    });

    await claimsDialog.getByRole('button', {
      name: 'Continue',
      exact: true,
    }).click();

    await expect(claimsDialog).toBeHidden();

    const responsePromise = page.waitForResponse(
      (response) =>
        response.url().includes('api/managementfiles') && response.request().method() === 'POST'
    );

    await managementCreatePage.confirmButtonClick();

    const response = await responsePromise;

    if (!response.ok()) {
      const responseBody = await response.text();

      throw new Error(
        `Management file creation failed: ${response.status()} ${response.url()}\n${responseBody}`
      );
    }

    await managementSummaryPage.fileDetailsTab.waitFor({
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
