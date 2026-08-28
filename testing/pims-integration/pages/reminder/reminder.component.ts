import { expect, Locator, Page } from '@playwright/test';

export class ReminderComponent {
  readonly page: Page;

  readonly reminderButton: Locator;
  readonly reminderDatePickerInput: Locator;
  readonly setReminderButton: Locator;
  readonly removeReminderButton: Locator;

  constructor(page: Page, reminderButton: Locator) {
    this.page = page;

    this.reminderButton = reminderButton;
    this.reminderDatePickerInput = page.locator(
      '.react-datepicker-wrapper input.date-picker'
    );
    this.setReminderButton = page.getByRole('button', {
      name: 'Set reminder',
    });
    this.removeReminderButton = page.getByRole('button', {
      name: 'Remove reminder',
    });
  }

  async openPopover() {
    await this.reminderButton.click();
  }

  async setDate(formattedDate: string) {
    await this.reminderDatePickerInput.fill(formattedDate);
  }

  async setReminder(date: string) {
    await this.openPopover();
    await this.setDate(date);
    await this.save();

    await expect
        .poll(async () => await this.isSet())
        .toBe(true);
    }

  async save() {
    await this.setReminderButton.click();
  }

  async remove() {
    await this.removeReminderButton.click();
  }

  async isSet(): Promise<boolean> {
    const title = await this.reminderButton.getAttribute('title');

    return !!title && title.startsWith('Reminder set for');
  }
}