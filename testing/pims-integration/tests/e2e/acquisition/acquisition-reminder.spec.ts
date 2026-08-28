import test, { expect, Page } from '@playwright/test';
import { AcquisitionCreatePage } from '../../../pages/acquisition/acquisition-create.page';
import { AcquisitionSummaryPage } from '../../../pages/acquisition/acquisition-summary.page';
import {
  createAcquisitionWithNoticeOfClaim,
} from '../../../utils/acquisition.workflows';

const INITIAL_REMINDER_DATE = 'Aug 10, 2026';
const UPDATED_REMINDER_DATE = 'Aug 12, 2026';

// Each test drives a full create-acquisition-file + reminder workflow (navigation, async
// lookups, form fills, a save round-trip) which comfortably exceeds the default 30s secs.
test.describe.configure({ timeout: 60_000 });

let acquisitionCreatePage: AcquisitionCreatePage;
let acquisitionSummaryPage: AcquisitionSummaryPage;

test.describe('Acquisition reminder feature', () => {
  // Uses Playwright's own per-test `page` fixture (isolated context per test) instead of a
  // shared page across tests, so one test's failure/timeout can't affect the others.
  test.beforeEach(async ({ page }: { page: Page }) => {
    acquisitionCreatePage = new AcquisitionCreatePage(page);
    acquisitionSummaryPage = new AcquisitionSummaryPage(page);
  });

  test('set a reminder for the Notice of Claim received date', async ({ page }) => {
    await createAcquisitionWithNoticeOfClaim(page, acquisitionCreatePage, acquisitionSummaryPage);

    await expect(
      acquisitionSummaryPage.noticeOfClaimReminder.reminderButton
    ).toBeVisible();

    expect(
      await acquisitionSummaryPage.noticeOfClaimReminder.isSet()
    ).toBe(false);

    await acquisitionSummaryPage.noticeOfClaimReminder.setReminder(INITIAL_REMINDER_DATE)

    await expect(acquisitionSummaryPage.noticeOfClaimReminder.reminderButton).toHaveAttribute(
      'title',
      `Reminder set for ${INITIAL_REMINDER_DATE}`
    );
  });

  test('update a reminder for the Notice of Claim received date', async ({ page }) => {
    await createAcquisitionWithNoticeOfClaim(page, acquisitionCreatePage, acquisitionSummaryPage);
    
    await expect(acquisitionSummaryPage.noticeOfClaimReminder.reminderButton).toBeVisible();
    expect(await acquisitionSummaryPage.noticeOfClaimReminder.isSet()).toBe(false);

    await acquisitionSummaryPage.noticeOfClaimReminder.setReminder(INITIAL_REMINDER_DATE);

    await acquisitionSummaryPage.noticeOfClaimReminder.openPopover();

    await acquisitionSummaryPage.noticeOfClaimReminder.setDate(
      UPDATED_REMINDER_DATE
    );
    await acquisitionSummaryPage.noticeOfClaimReminder.save();

    await expect(acquisitionSummaryPage.noticeOfClaimReminder.reminderButton).toHaveAttribute(
      'title',
      `Reminder set for ${UPDATED_REMINDER_DATE}`
    );
  });

  test('delete a reminder for the Notice of Claim received date', async ({ page }) => {
    await createAcquisitionWithNoticeOfClaim(page, acquisitionCreatePage, acquisitionSummaryPage);

    await expect(acquisitionSummaryPage.noticeOfClaimReminder.reminderButton).toBeVisible();
    expect(await acquisitionSummaryPage.noticeOfClaimReminder.isSet()).toBe(false);

    await acquisitionSummaryPage.noticeOfClaimReminder.setReminder(INITIAL_REMINDER_DATE);

    await acquisitionSummaryPage.noticeOfClaimReminder.openPopover();
    await acquisitionSummaryPage.noticeOfClaimReminder.remove();

    await expect.poll(async () => await acquisitionSummaryPage.noticeOfClaimReminder.isSet()).toBe(false);
  });

});
