import { expect, type Locator, type Page } from '@playwright/test';

export class LoginPage {
  readonly page: Page;
  readonly loginButton: Locator;
  readonly loginHeader: Locator;


  constructor(page: Page) {
    this.page = page;
    this.loginButton = page.locator('button', { hasText: 'Sign In' });
    this.loginHeader = page.locator('h1', { hasText: 'MOTT Property Information Management System (PIMS)' });
  }

  async goto() {
    await this.page.goto('/login');
  }

  async getStarted() {
    await this.loginButton.first().click();
  }

  async pageObjectModel() {
    await this.getStarted();
  }
}
