import { expect, Page } from '@playwright/test';
import { AcquisitionCreatePage } from '../pages/acquisition/acquisition-create.page';
import { AcquisitionSummaryPage } from '../pages/acquisition/acquisition-summary.page';
import { generateFileName } from './utils';

/**
 * Creates a new acquisition file with a Notice of Claim received date and waits for the
 * File Details tab to render, leaving the browser on that file's summary page.
 */
export async function createAcquisitionWithNoticeOfClaim(
  page: Page,
  acquisitionCreatePage: AcquisitionCreatePage,
  acquisitionSummaryPage: AcquisitionSummaryPage,
  receivedDate: string = 'Aug 15, 2026'
): Promise<string> {
  const fileName = generateFileName('Acquisition');

  await acquisitionCreatePage.goto();

  await acquisitionCreatePage.setFileNameInput(fileName);
  await acquisitionCreatePage.selectAcquisitionType("CONSEN");
  await acquisitionCreatePage.selectRegion();
  await acquisitionCreatePage.setNoticeOfClaimReceivedDate(receivedDate);
  
  const responsePromise = page.waitForResponse(
    response =>
      response.url().includes('/api/acquisitionfiles') &&
      response.request().method() === 'POST'
  );

  await acquisitionCreatePage.confirmButtonClick();

  const response = await responsePromise;

  expect(
    response.status(),
    `Acquisition creation failed: ${response.status()} ${response.url()}`
  ).toBe(200);

  // Wait until we've reached the acquisition summary.
  await acquisitionSummaryPage.fileDetailsTab.waitFor({
    state: 'visible',
    timeout: 15_000,
  });


  return fileName;
}
