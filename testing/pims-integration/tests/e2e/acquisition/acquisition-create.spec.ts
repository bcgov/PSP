import { test, expect } from '../../../fixtures/acquisition.fixtures';

test.describe('Acquisition file creation', () => {
  test('creates a new acquisition file with a Notice of Claim', async ({
    acquisitionSummaryPage,
    acquisitionWithNoticeOfClaim,
  }) => {
    await expect(acquisitionSummaryPage.fileDetailsTab).toBeVisible();

    await expect(
      acquisitionSummaryPage.noticeOfClaimReceivedDateLabel
    ).toBeVisible();

    expect(acquisitionWithNoticeOfClaim.fileName).toBeTruthy();
  });
});