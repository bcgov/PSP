import test, { expect } from '@playwright/test';
import { AcquisitionCreatePage } from '../../../pages/acquisition/acquisition-create.page';
import { AcquisitionSummaryPage } from '../../../pages/acquisition/acquisition-summary.page';
import { createAcquisitionWithNoticeOfClaim } from '../../../utils/acquisition.workflows';

test.describe('Acquisition file creation', () => {
  test('creates a new acquisition file with a Notice of Claim', async ({ page }) => {
    const acquisitionCreatePage = new AcquisitionCreatePage(page);
    const acquisitionSummaryPage = new AcquisitionSummaryPage(page);

    await createAcquisitionWithNoticeOfClaim(
    page,
    acquisitionCreatePage,
    acquisitionSummaryPage
  );

  await expect(acquisitionSummaryPage.fileDetailsTab).toBeVisible();

  await expect(
    acquisitionSummaryPage.noticeOfClaimReceivedDateLabel
  ).toBeVisible();
  });
});