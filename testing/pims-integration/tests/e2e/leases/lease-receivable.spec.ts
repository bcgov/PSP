import { Page, BrowserContext, test, expect } from '@playwright/test';
import { LeaseCreatePage } from '../../../pages/leases/lease-create.page';
import { LeaseDetailsPage } from '../../../pages/leases/lease-details.page';
import { LeaseEditPage } from '../../../pages/leases/lease-edit.page';
import { getTodayPrettyFormatted } from '../../../utils/utils';

let context: BrowserContext;
let page: Page;
let leaseCreatePage: LeaseCreatePage;
let leaseDetailsPage: LeaseDetailsPage;
let leaseEditPage: LeaseEditPage;

let leaseCreateResponseObject: {
  id: number;
  lFileNo: string;
};

test.describe.serial('Lease and Licence integration tests for RECEIVABLE lease.', () => {

  test.beforeAll(async ({ browser }) => {
    context = await browser.newContext();
    page = await context.newPage();
    leaseCreatePage = new LeaseCreatePage(page);
    leaseDetailsPage = new LeaseDetailsPage(page);
    leaseEditPage = new LeaseEditPage(page);
    await leaseCreatePage.goto();
  });

  test.afterAll(async () => {
    await context.close();
  });

  test("Creating a new Lease that's 'Receivable' with minimal data", async () => {
    const responsePromise = page.waitForResponse(
      response =>
        response.request().method() === 'POST' &&
        /\/leases(\?|$)/.test(response.url()) &&
        response.status() === 200
    );

    // await expect(leaseCreatePage.leaseCreatePageTitle).toBeVisible();

    await leaseCreatePage.setRegionOption('1');
    await leaseCreatePage.setLeaseProgramTypeOption('AGRIC');
    await leaseCreatePage.setLeaseTypeOption('AMNDAGREE');
    await leaseCreatePage.setLeasePurposeOption('Access');

    await leaseCreatePage.getLeaseIntendedUseInput().click();

    await leaseCreatePage.leaseSaveBtn.click();

    const response = await responsePromise;
    leaseCreateResponseObject = await response.json();

    await page.waitForURL(/\/mapview\/sidebar\/lease\/\d+/);
    expect(page.url().split('/').pop()).toBe(leaseCreateResponseObject.id.toString());
  });

  test("The user can add a 'renewal' to the lease", async () => {
    await expect(page.getByText(leaseCreateResponseObject.lFileNo)).toBeVisible();
    expect(await leaseDetailsPage.getEmptyRenewalsText()).toBe('No Renewal Information');

    await leaseDetailsPage.leaseEditBtn.click();
    await page.getByTestId('filter-backdrop-loading').waitFor({ state: 'hidden' });
    await leaseEditPage.leaseAddRenewalBtn.click();
    await page.waitForLoadState("domcontentloaded");

    expect(await leaseEditPage.getRenewalHeaderTextByIndex(0)).toBe('Renewal 1');

    await leaseEditPage.setRenewalCommencementDate();
    await leaseEditPage.leaseSaveBtn.click();

    // Assert
    const todayDateFormatted = getTodayPrettyFormatted();
    expect(await leaseDetailsPage.getRenewalHeaderTextByIndex()).toBe('Renewal 1');
    expect(await leaseDetailsPage.getRenewalExercisedTextByIndex()).toBe('No');
    expect(await leaseDetailsPage.getRenewalcommencementDateTextByIndex()).toBe(todayDateFormatted);
    expect(await leaseDetailsPage.getRenewalExpiryDateTextByIndex()).toBe('');
    expect(await leaseDetailsPage.getRenewalCommentsTextByIndex()).toBe('');
  });
});
