const { Given } = require("@cucumber/cucumber");
const { launchBrowser } = require("../../utils/browserSetup.js");
const { LoginPage } = require("../../support/pages/LoginPage.js");
const { BASE_URL, getUserCredential } = require("../../support/world.js");

Given(
  "I log in as {string} with password {string}",
  async function (userEnv, passEnv) {
    const { browser, context, page } = await launchBrowser(true); // Pass 'true' if using incognito

    this.browser = browser;
    this.context = context;
    this.page = page;

    const loginPage = new LoginPage(page);
    await loginPage.navigate(BASE_URL);
    const username = getUserCredential(userEnv);
    const password = getUserCredential(passEnv);
    await loginPage.login(username, password);

    await page.waitForLoadState("load", { timeout: 40000 });
  }
);
