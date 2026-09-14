import { Locator, Page } from '@playwright/test';
import { LayoutPage } from '../layout/layout.page';


export class LeaseDetailsPage extends LayoutPage {
  readonly page: Page;

  readonly leaseEditBtn: Locator;
  readonly leaseEmptyRenewals: Locator;

  constructor(page: Page) {
    super(page);

    this.page = page;

    this.leaseEditBtn = page.locator('#edit-details-btn');

    // Renewals
    this.leaseEmptyRenewals = page.getByTestId('empty-renewals');
  }

  // Renewals
  getEmptyRenewalsText(): Promise<string> {
    return this.page.getByTestId(`empty-renewals`).innerText();
  }

  getRenewalHeaderTextByIndex(index: number = 0): Promise<string> {
    return this.page.getByTestId(`renewal[${index}].header`).getByText('Renewal').innerText();
  }

  getRenewalExercisedTextByIndex(index: number = 0): Promise<string> {
    return this.page.getByTestId(`renewal[${index}].exercised`).innerText();
  }

  getRenewalcommencementDateTextByIndex(index: number = 0): Promise<string> {
    return this.page.getByTestId(`renewal[${index}].commencementDt`).innerText();
  }

  getRenewalExpiryDateTextByIndex(index: number = 0): Promise<string> {
    return this.page.getByTestId(`renewal[${index}].expiryDt`).innerText();
  }

  getRenewalCommentsTextByIndex(index: number = 0): Promise<string> {
    return this.page.getByTestId(`renewal[${index}].renewalNote`).innerText();
  }
}
