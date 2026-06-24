import { expect, test, type BrowserContext, type Page } from '@playwright/test';
import { HelpDeskPage } from '../../pages/help-desk.page';

let context: BrowserContext;
let page: Page;
let helpDeskPage: HelpDeskPage;

const userFullName = process.env.USER_TRANPSP1_FULLNAME;
const userEmail = process.env.USER_TRANPSP1_EMAIL;

test.describe.serial('Help desk modal', () => {
  test.beforeAll(async ({ browser }) => {
    context = await browser.newContext();
    page = await context.newPage();
    helpDeskPage = new HelpDeskPage(page);
    await helpDeskPage.goto();
    await helpDeskPage.openHelpDeskForm();
  });

  test.afterAll(async () => {
    await helpDeskPage.cancelButtonClick();
    await context.close();
  });

  test('displays the title properly', async () => {
    await expect(helpDeskPage.helpDeskTitle).toBeVisible();
    expect(await helpDeskPage.getTitleText()).toEqual('Help Desk');
  });

  test('displays the sub-title properly', async () => {
    await expect(helpDeskPage.helpDeskSubTitle).toBeVisible();
    expect(await helpDeskPage.getSubTitleText()).toEqual('Get started with PIMS');
  });

  test('displays the instructions properly', async () => {
    await expect(helpDeskPage.helpDeskInstructions).toBeVisible();
    expect(await helpDeskPage.getInstructionsText()).toEqual(
      'This overview has useful tools that will support you to start using the application. You can also watch the video demos.'
    );
  });

  test('displays the resources link', async () => {
    await expect(helpDeskPage.helpDeskPIMSResourcesLink).toBeVisible();
  });

  test('displays the contact us sub-title', async () => {
    await expect(helpDeskPage.helpDeskContactUsSubtitle).toBeVisible();
    expect(await helpDeskPage.getContactUsSubtitleText()).toEqual('Contact us:');
  });

  test('displays the user fullname and email in the form', async () => {
    await expect(helpDeskPage.helpDeskUserLabel).toBeVisible();
    await expect(helpDeskPage.helpDeskUserInput).toBeVisible();

    await expect(helpDeskPage.helpDeskEmailLabel).toBeVisible();
    await expect(helpDeskPage.helpDeskEmailInput).toBeVisible();

    expect(await helpDeskPage.getUserNameInputValue()).toEqual(userFullName);
    expect(await helpDeskPage.getUserEmailInputValue()).toEqual(userEmail);
  });
});
