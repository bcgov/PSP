import { test, expect } from '../../../fixtures/acquisition.fixtures';

const INITIAL_REMINDER_DATE = 'Aug 10, 2026';
const UPDATED_REMINDER_DATE = 'Aug 12, 2026';

// Each test drives a full create-acquisition-file + reminder workflow (navigation, async
// lookups, form fills, a save round-trip) which comfortably exceeds the default 30s secs.
test.describe.configure({ timeout: 60_000 });

test.describe('Acquisition reminder feature', () => {
  test('Manages a reminder for the Notice of Claim received date', async ({
    acquisitionSummaryPage,
    acquisitionWithNoticeOfClaim,
  }) => {
    expect(acquisitionWithNoticeOfClaim.fileName).toBeTruthy();

    await expect(
      acquisitionSummaryPage.noticeOfClaimReminder.reminderButton
    ).toBeVisible();

    expect(
      await acquisitionSummaryPage.noticeOfClaimReminder.isSet()
    ).toBe(false);

    await test.step('set the reminder', async () => {
      await acquisitionSummaryPage.noticeOfClaimReminder.setReminder(
        INITIAL_REMINDER_DATE
      );

      await expect(
        acquisitionSummaryPage.noticeOfClaimReminder.reminderButton
      ).toHaveAttribute(
        'title',
        `Reminder set for ${INITIAL_REMINDER_DATE}`
      );
    });

    await test.step('update the reminder', async () => {
      await acquisitionSummaryPage.noticeOfClaimReminder.openPopover();

      await acquisitionSummaryPage.noticeOfClaimReminder.setDate(
        UPDATED_REMINDER_DATE
      );

      await acquisitionSummaryPage.noticeOfClaimReminder.save();

      await expect(
        acquisitionSummaryPage.noticeOfClaimReminder.reminderButton
      ).toHaveAttribute(
        'title',
        `Reminder set for ${UPDATED_REMINDER_DATE}`
      );
    });

    await test.step('delete the reminder', async () => {
      await acquisitionSummaryPage.noticeOfClaimReminder.openPopover();

      await acquisitionSummaryPage.noticeOfClaimReminder.remove();

      await expect
        .poll(() => acquisitionSummaryPage.noticeOfClaimReminder.isSet())
        .toBe(false);
    });
  });
});