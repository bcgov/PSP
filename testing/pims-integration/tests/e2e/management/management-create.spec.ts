import { test, expect } from '../../../fixtures/management.fixtures';

test.describe('Management file Creation', () => {
  test('should create a management file with notice of claim', async ({
    managementSummaryPage, 
    managementWithNoticeOfClaim 
  }) => {
    await expect(managementSummaryPage.fileDetailsTab).toBeVisible();
    await expect(managementSummaryPage.noticeOfClaimReceivedDateLabel).toBeVisible();
    expect(managementWithNoticeOfClaim.fileName).toBeTruthy();
  });
});