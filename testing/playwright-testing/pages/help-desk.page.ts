import { Locator, type Page } from '@playwright/test';
import { LayoutPage } from './layout/layout.page';

export class HelpDeskPage extends LayoutPage {
  readonly page: Page;
  readonly helpDeskTitle: Locator;

  constructor(page: Page) {
    super(page);

    this.page = page;
    this.helpDeskTitle = page.locator("div[class='modal-title h4']", {hasText: 'Help Desk'});
  }

  async goto() {
    await this.page.goto('/mapview');
  }

}
