import { Page } from '@playwright/test';

export class AcquisitionCreatePage {
  readonly page: Page;

  constructor(page: Page) {
    this.page = page;
  }

  async goto() {
    await this.page.goto('/mapview/sidebar/acquisition/new');
  }
}
