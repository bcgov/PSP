import { Locator, Page } from '@playwright/test';
import { LayoutPage } from '../layout/layout.page';
import { ReminderComponent } from '../reminder/reminder.component';

export class AcquisitionSummaryPage extends LayoutPage {
  readonly page: Page;

  readonly fileDetailsTab: Locator;
  readonly noticeOfClaimReceivedDateLabel: Locator;

  readonly noticeOfClaimReminder: ReminderComponent;

  constructor(page: Page) {
    super(page);

    this.page = page;

    this.fileDetailsTab = page.getByRole('tab', {
      name: 'File Details',
    });

    this.noticeOfClaimReceivedDateLabel = page.getByText(
      'Received date:',
      { exact: true }
    );

    this.noticeOfClaimReminder = new ReminderComponent(
      page,
      page.getByRole('button', {
        name: 'Reminder for Received date',
      })
    );
  }
}
