const { Before, After, setDefaultTimeout } = require("@cucumber/cucumber");
const { getUserCredential } = require("./common.js");

setDefaultTimeout(30000);

Before(async function () {
  if (!this.page) {
    await this.openBrowser();
  }

  await this.loginPage.navigate(global.baseURL);

  const username = getUserCredential("USER1_NAME");
  const password = getUserCredential("USER1_PASSWORD");

  await this.loginPage.login(username, password);
  await this.page
    .getByTestId("filter-backdrop-loading")
    .waitFor({ state: "hidden", timeout: 20000 });
  await this.page
    .locator("#layersControlButton")
    .waitFor({ state: "visible", timeout: 20000 });
});

After(async function () {
  //await this.closeBrowser();
});
