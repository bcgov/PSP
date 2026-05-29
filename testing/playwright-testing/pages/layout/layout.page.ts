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
}
