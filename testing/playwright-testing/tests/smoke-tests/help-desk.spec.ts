import { expect, test as base } from '@playwright/test';
import { HelpDeskPage } from '../../pages/help-desk.page';

// Extend basic test by providing a "HelpDeskPage" fixture.
const test = base.extend<{ helpDeskPage: HelpDeskPage }>({
  helpDeskPage: async ({ page }, use) => {
    const helpDeskPage = new HelpDeskPage(page);
    await helpDeskPage.goto();
    await helpDeskPage.openHelpDeskForm();
    await use(helpDeskPage);
  },
});

test.describe('Help desk modal', () => {
  //   test.beforeEach(async ({ page }) => {
  //     // Go to the starting url before each test.
  //     await page.goto('https://playwright.dev/');
  //   });

  test.afterEach(async ({ helpDeskPage }) => {
    // Close the modal after each test
    await helpDeskPage.cancelButtonClick();
  });

  // Pulling variables directly from the environment
  const userFullName = process.env.USER_TRANPSP1_FULLNAME;
  const userEmail = process.env.USER_TRANPSP1_EMAIL;

  test('displays the title properly', async ({ helpDeskPage }) => {
    // Assertions use the expect API.
    await expect(helpDeskPage.helpDeskTitle).toBeVisible();
    expect(await helpDeskPage.getTitleText()).toEqual("Help Desk");
  });

  test('displays the sub-title properly', async ({ helpDeskPage }) => {
    await expect(helpDeskPage.helpDeskSubTitle).toBeVisible();
    expect(await helpDeskPage.getSubTitleText()).toEqual("Get started with PIMS");
  });

  test('displays the instructions properly', async ({ helpDeskPage }) => {
    await expect(helpDeskPage.helpDeskInstructions).toBeVisible();
    expect(await helpDeskPage.getInstructionsText()).toEqual("This overview has useful tools that will support you to start using the application. You can also watch the video demos.");
  });

  test('displays the resources link', async ({ helpDeskPage }) => {
    await expect(helpDeskPage.helpDeskPIMSResourcesLink).toBeVisible();
  });

  test('displays the contact us sub-title', async ({ helpDeskPage }) => {
    await expect(helpDeskPage.helpDeskContactUsSubtitle).toBeVisible();
    expect(await helpDeskPage.getContactUsSubtitleText()).toEqual("Contact us:");
  });

  test('displays the user fullname and email in the form', async ({ helpDeskPage }) => {
    await expect(helpDeskPage.helpDeskUserLabel).toBeVisible();
    await expect(helpDeskPage.helpDeskUserInput).toBeVisible();

    await expect(helpDeskPage.helpDeskEmailLabel).toBeVisible();
    await expect(helpDeskPage.helpDeskEmailInput).toBeVisible();

    expect(await helpDeskPage.getUserNameInputValue()).toEqual(userFullName);
    expect(await helpDeskPage.getUserEmailInputValue()).toEqual(userEmail);
  });
});
