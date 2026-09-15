import { Locator, Page } from '@playwright/test';
import { LayoutPage } from '../layout/layout.page';

export class LeaseEditPage extends LayoutPage {
  readonly page: Page;

  // Renewals Section
  readonly leaseAddRenewalBtn: Locator;
  readonly leaseSaveBtn: Locator;

  constructor(page: Page) {
    super(page);

    this.page = page;

    this.leaseAddRenewalBtn = page.getByTestId('add-lease-renewal');
    this.leaseSaveBtn = page.getByTestId('save-button');
  }

  // Renewals Section
  getRenewalHeaderTextByIndex(index: number = 0): Promise<string> {
    return this.page.getByTestId(`renewals.${index}.header`).locator('h2').innerText();
  }

  getRenewalExercisedInputByIndex(index: number = 0) {
    return this.page.locator(`[id="input-renewals.${index}.isExercised"]`);
  }

  /** Sets the renewal's commencement date; uses today's date when `date` is null. `date` must be in 'YYYY-MM-DD' format. */
  async setRenewalCommencementDate(index: number = 0, date: string | null = null) {
    const targetDate = date ? new Date(`${date}T00:00:00`) : new Date();
    const formattedDate = targetDate.toLocaleDateString('en-US', {
      month: 'short',
      day: '2-digit',
      year: 'numeric',
    });

    const commencementDtInput = this.page.locator(
      `[id="datepicker-renewals.${index}.commencementDt"]`
    );
    await commencementDtInput.click();
    await commencementDtInput.fill(formattedDate);
    await commencementDtInput.press('Enter');
  }
}
