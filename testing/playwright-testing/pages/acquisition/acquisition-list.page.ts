import { Page } from "@playwright/test";
import { LayoutPage } from "../layout/layout.page";

export class AcquisitionListPage extends LayoutPage {
  readonly page: Page;

  constructor(page: Page) {
    super(page);
    this.page = page;
  }

  async goto() {
    await this.page.goto("/acquisition/list");
  }
}
