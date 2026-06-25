import { Page, Locator } from '@playwright/test';

export class LayoutPage {
  readonly page: Page;
  readonly helpDeskButton: Locator;

  constructor(page: Page) {
    this.page = page;
    this.helpDeskButton = page.getByTestId('help-desk-container-btn');
  }

  async openHelpDeskForm() {
    await this.helpDeskButton.click();
  }

  normalize(value: unknown): string {
    return value == null ? '' : String(value).trim();
  }

  formatApiDate(apiDate: string | null | undefined): string {
    if (!apiDate) {
      return '';
    }

    const date = new Date(apiDate);

    return date.toLocaleDateString('en-CA', {
      month: 'short',
      day: 'numeric',
      year: 'numeric',
      timeZone: 'UTC', // avoids timezone surprises
    });
  }

  formatApiBoolean(value: boolean | null | undefined): string {
    if (value == null) {
      return '';
    }

    return value ? 'Yes' : 'No';
  }
}
