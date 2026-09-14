import { expect, Locator, Page } from '@playwright/test';
import { LayoutPage } from '../layout/layout.page';

export class AcquisitionCreatePage extends LayoutPage {
  readonly page: Page;

  readonly fileNameInput: Locator;
  readonly acquisitionTypeSelect: Locator;
  readonly regionSelect: Locator;
  readonly noticeOfClaimReceivedDateInput: Locator;
  readonly noticeOfClaimCommentInput: Locator;

  readonly cancelButton: Locator;
  readonly confirmButton: Locator;

  constructor(page: Page) {
    super(page);

    this.page = page;

    this.fileNameInput = page.locator('#input-fileName');
    this.acquisitionTypeSelect = page.locator('#input-acquisitionType');
    this.regionSelect = page.locator('#input-region');
    this.noticeOfClaimReceivedDateInput = page.locator('#datepicker-noticeOfClaim\\.receivedDate');
    this.noticeOfClaimCommentInput = page.locator('#input-noticeOfClaim\\.comment');

    this.cancelButton = page.locator("button[data-testid='cancel-button']");
    this.confirmButton = page.locator("button[data-testid='save-button']");
  }

  async goto() {
    await this.page.goto('/mapview/sidebar/acquisition/new', { waitUntil: 'domcontentloaded' });
  }

  async setFileNameInput(fileName: string) {
    await this.fileNameInput.fill(fileName);
  }

  async selectAcquisitionType(value: string) {
    await this.acquisitionTypeSelect.selectOption({ value });
  }

  async selectRegion(index: number = 1) {
    // The Region dropdown is filtered to only the logged-in user's assigned MOTT region(s), so an
    // account with none assigned (Admin > Manage Users > Edit User) will never have a real option.
    await expect
      .poll(async () => await this.regionSelect.locator('option').count(), {
        message:
          'Region dropdown has no real options. Assign a MOTT region to the test user via Admin > Manage Users.',
        timeout: 15_000,
      })
      .toBeGreaterThan(1);
    await this.regionSelect.selectOption({ index });
  }

  async setNoticeOfClaimReceivedDate(formattedDate: string) {
    await this.noticeOfClaimReceivedDateInput.fill(formattedDate);
    await this.noticeOfClaimReceivedDateInput.press('Escape');
  }

  async confirmButtonClick() {
    await this.confirmButton.click();
  }
}
