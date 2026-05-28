const { expect } = require("@playwright/test");

class SharedFileProperties {
  constructor(page) {
    this.page = page;
  }

  async countInsertedPropertiesOnFilePropDetails() {
    const deleteBttns = await this.page.locator(
      "button[data-testid*='delete-property']"
    );
    await deleteBttns.waitFor({ state: "attached" });

    const count = await deleteBttns.count();
    return count;
  }

  async countInsertedPropertiesOnFileSideBar() {
    const propertiuesElements = await this.page.locator(
      "div[data-testid*='menu-item-property']]"
    );
    await propertiuesElements.waitFor({ state: "attached" });

    const count = await propertiuesElements.count();
    return count;
  }
}

module.exports = SharedFileProperties;
