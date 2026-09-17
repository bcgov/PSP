import { test, expect } from '../../../fixtures/management.fixtures';

const INITIAL_REMINDER_DATE = 'Sep 17, 2026'
const UPDATED_REMINDER_DATE = 'Oct 1, 2026'

// Each test drives a full create-acquisition-file + reminder workflow (navigation, async
// lookups, form fills, a save round-trip) which comfortably exceeds the default 30s secs.
test.describe.configure({ timeout: 60_000 });

test.describe('Management reminder feature', () => {
  test('Manages a reminder for the Notice of Claim received date', async ({
    managementSummaryPage,
    managementWithNoticeOfClaim,
  }) => {
    expect(managementWithNoticeOfClaim.fileName).toBeTruthy();

    await expect(managementSummaryPage.noticeOfClaimReminder.reminderButton).toBeVisible();

    expect(await managementSummaryPage.noticeOfClaimReminder.isSet()).toBe(false);

    await test.step('set the reminder', async () => {
      await managementSummaryPage.noticeOfClaimReminder.setReminder(INITIAL_REMINDER_DATE);

      await expect(managementSummaryPage.noticeOfClaimReminder.reminderButton).toHaveAttribute(
        'title',
        `Reminder set for ${INITIAL_REMINDER_DATE}`
      );
    });

    await test.step('update the reminder', async () => {
      await managementSummaryPage.noticeOfClaimReminder.openPopover();

      await managementSummaryPage.noticeOfClaimReminder.setDate(UPDATED_REMINDER_DATE);

      await managementSummaryPage.noticeOfClaimReminder.save();

      await expect(managementSummaryPage.noticeOfClaimReminder.reminderButton).toHaveAttribute(
        'title',
        `Reminder set for ${UPDATED_REMINDER_DATE}`
      );
    });

    await test.step('delete the reminder', async () => {
      await managementSummaryPage.noticeOfClaimReminder.openPopover();

      await managementSummaryPage.noticeOfClaimReminder.remove();

      await expect.poll(() => managementSummaryPage.noticeOfClaimReminder.isSet()).toBe(false);
    });
  });
});
