import { Locator, Page } from '@playwright/test';
import { LayoutPage } from './layout/layout.page';

export class MapViewPage extends LayoutPage {
  readonly page: Page;
  readonly searchPropertiesButton: Locator;
  readonly searchPropertiesPanel: Locator;

  constructor(page: Page) {
    super(page);

    this.page = page;
    this.searchPropertiesButton = page.locator('#searchControlButton');
    this.searchPropertiesPanel = page.locator('#search-sidebar');
  }

  async goto() {
    await this.page.goto('/mapview');
  }

  async searchPropertiesPanelToggle() {
    await this.searchPropertiesButton.click();
  }
}
