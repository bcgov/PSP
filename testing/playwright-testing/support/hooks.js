// support/hooks.js

import { Before, After } from "@cucumber/cucumber";
import { getUserCredential } from "./common.js";

Before({ timeout: 60000 }, async function () {
  await this.openBrowser();

  // Login process before each test case
  await this.loginPage.navigate(global.baseURL);

  const username = getUserCredential("USER1_NAME");
  const password = getUserCredential("USER1_PASSWORD");

  await this.loginPage.login(username, password);

  // Wait for App Stability
  await this.page
    .getByTestId("filter-backdrop-loading")
    .first()
    .waitFor({ state: "hidden", timeout: 20000 });
  await this.page
    .getByTestId("filter-backdrop-loading")
    .nth(1)
    .waitFor({ state: "hidden", timeout: 20000 });
  await this.page
    .locator("#layersControlButton")
    .waitFor({ state: "visible", timeout: 20000 });
});

// --- AFTER HOOK ---
After(async function () {
  await this.closeBrowser();
});
