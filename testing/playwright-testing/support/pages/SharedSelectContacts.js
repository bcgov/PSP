const SharedModal = require("./SharedModal");

class SharedSelectContact {
  constructor(page) {
    this.page = page;
    this.sharedModal = new SharedModal(page);
  }

  async selectContact(contactSearchName, contactType) {
    switch (contactType) {
      case "Individual":
        await this.page
          .locator('input[name="searchBy"][value="persons"]')
          .check();
        break;
      case "Organization":
        await this.page
          .locator('input[name="searchBy"][value="organizations"]')
          .check();
        break;
      default:
        break;
    }

    await this.page.locator("#input-summary").fill(contactSearchName);
    await this.page.locator("#search-button").click();

    await this.page
      .locator('input[type="radio"][name="table-radio"]')
      .first()
      .check();
    this.sharedModal.mainModalClickOKBttn();
  }
}

module.exports = SharedSelectContact;
