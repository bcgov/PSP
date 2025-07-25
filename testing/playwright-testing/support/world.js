require("dotenv").config();
const { setWorldConstructor } = require("@cucumber/cucumber");

const BASE_URL = process.env.BASE_URL;

function getUserCredential(envKey) {
  return process.env[envKey];
}

class CustomWorld {
  constructor() {
    this.browser = null;
    this.context = null;
    this.page = null;
  }

  async openBrowser() {
    this.browser = await chromium.launch({ headless: false });
    this.context = await this.browser.newContext();
    this.page = await this.context.newPage();
  }

  async closeBrowser() {
    await this.browser?.close();
  }
}

setWorldConstructor(CustomWorld);

module.exports = {
  BASE_URL,
  getUserCredential,
};
