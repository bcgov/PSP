import { Locator, Page } from '@playwright/test';
import { LayoutPage } from '../layout/layout.page';

export class LeaseCreatePage extends LayoutPage {
  readonly page: Page;

  readonly leaseCreatePageTitle: Locator;
  readonly leaseStatusDropDown: Locator;
  readonly leaseAccountTypeDropDown: Locator;
  readonly leaseProgramTypeDropDown: Locator;
  readonly leaseRegionTypeDropDown: Locator;
  readonly leaseTypeTypeDropDown: Locator;
  readonly leasePurposeTypeDropDown: Locator;
  readonly leaseIntendedUseInput: Locator;
  readonly leaseSaveBtn: Locator;

  constructor(page: Page) {
    super(page);

    this.page = page;

    this.leaseCreatePageTitle = page.getByTestId('form-title');
    this.leaseStatusDropDown = page.locator('#input-statusTypeCode');
    this.leaseAccountTypeDropDown = page.locator('#input-paymentReceivableTypeCode');
    this.leaseRegionTypeDropDown = page.locator('#input-regionId');
    this.leaseTypeTypeDropDown = page.locator('#input-leaseTypeCode');
    this.leasePurposeTypeDropDown = page.locator('#multiselect-purposes_input');
    this.leaseProgramTypeDropDown = page.locator('#input-programTypeCode');
    this.leaseIntendedUseInput = page.locator('#input-description');
    this.leaseSaveBtn = page.getByTestId('save-button');
  }

  async goto() {
    await this.page.goto('/mapview/sidebar/lease/new', { waitUntil: 'domcontentloaded' });
  }

  getLeaseIntendedUseInput() {
    return this.leaseIntendedUseInput;
  }

  async setRegionOption(option: string) {
    await this.leaseRegionTypeDropDown.selectOption(option);
  }

  async setLeaseProgramTypeOption(option: string) {
    await this.leaseProgramTypeDropDown.selectOption(option);
  }

  async setLeaseTypeOption(option: string) {
    await this.leaseTypeTypeDropDown.selectOption(option);
  }

  async setLeasePurposeOption(option: string) {
    await this.page.locator('.search-wrapper').click();
    await this.page.getByText(option, { exact: true }).click();
  }
}
