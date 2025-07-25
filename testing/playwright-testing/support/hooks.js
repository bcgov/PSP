const { After, setDefaultTimeout } = require('@cucumber/cucumber');

setDefaultTimeout(20000);

After(async function () {
  if (this.browser) {
    //await this.browser.close();
  }
});
