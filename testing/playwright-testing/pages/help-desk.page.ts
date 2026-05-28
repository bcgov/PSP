import { Locator, Page } from '@playwright/test';
import { LayoutPage } from './layout/layout.page';

export class HelpDeskPage extends LayoutPage {
  readonly page: Page;
  readonly helpDeskTitle: Locator;
  readonly helpDeskSubTitle: Locator;
  readonly helpDeskInstructions: Locator;
  readonly helpDeskPIMSResourcesLink: Locator;
  readonly helpDeskContactUsSubtitle: Locator;
  readonly helpDeskUserLabel: Locator;
  readonly helpDeskUserInput: Locator;
  readonly helpDeskEmailLabel: Locator;
  readonly helpDeskEmailInput: Locator;
  readonly helpDeskDescriptionLabel: Locator;
  readonly helpDeskDescriptionInput: Locator;
  readonly helpDeskFooterQuestion: Locator;
  readonly cancelButton: Locator;
  readonly confirmButton: Locator;

  constructor(page: Page) {
    super(page);

    this.page = page;
    this.helpDeskTitle = page.locator("div[class='modal-title h4']", { hasText: 'Help Desk' });
    this.helpDeskSubTitle = page.locator("div[class='modal-body'] h3:first-child", { hasText: 'Get started with PIMS' });
    this.helpDeskInstructions = page.locator("div[class='modal-body'] p").first();
    this.helpDeskPIMSResourcesLink = page.locator("//a[contains(text(),'PIMS Training Materials')]");
    this.helpDeskContactUsSubtitle = page.locator("//h3[contains(text(),'Contact us')]");
    this.helpDeskUserLabel = page.locator("//label[contains(text(),'Name')]");
    this.helpDeskUserInput = page.locator("#input-user");
    this.helpDeskEmailLabel = page.locator("//label[contains(text(),'Email')]");
    this.helpDeskEmailInput = page.locator("#input-email");
    this.helpDeskDescriptionLabel = page.locator("//div[@class='modal-body']/div/form/div/div/label[contains(text(),'Description')]");
    this.helpDeskDescriptionInput = page.locator("#input-description");
    this.helpDeskFooterQuestion = page.locator("div[class='modal-body'] p").last();
    this.cancelButton = page.locator("button[data-testid='cancel-modal-button']");
    this.confirmButton = page.locator("div[class='modal-footer'] a[data-testid='ok-modal-button']");
  }

  async goto() {
    await this.page.goto('/mapview');
  }

  async getTitleText(): Promise<string> {
    return await this.helpDeskTitle.innerText();
  }

  async getSubTitleText(): Promise<string> {
    return await this.helpDeskSubTitle.innerText();
  }

  async getInstructionsText(): Promise<string> {
    return await this.helpDeskInstructions.innerText();
  }

  async getContactUsSubtitleText(): Promise<string> {
    return await this.helpDeskContactUsSubtitle.innerText();
  }

  async getFooterQuestionText(): Promise<string> {
    return await this.helpDeskFooterQuestion.innerText();
  }

  async getUserNameInputValue(): Promise<string> {
    return await this.helpDeskUserInput.inputValue();
  }

  async getUserEmailInputValue(): Promise<string> {
    return await this.helpDeskEmailInput.inputValue();
  }

  async cancelButtonClick() {
    await this.cancelButton.click();
  }

  async confirmButtonClick() {
    await this.confirmButton.click();
  }
}
