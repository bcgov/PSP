class SharedPagination {
  constructor(page) {
    this.page = page;
  }

  async choosePaginationOption(pagination) {
    await this.page.locator("//span[contains(text(),'Entries')]").click();

    switch (pagination) {
      case 5:
        await this.page.locator("div[title='menu-item-5']").click();
        break;
      case 10:
        await this.page.locator("div[title='menu-item-10']").click();
        break;
      case 20:
        await this.page.locator("div[title='menu-item-20']").click();
        break;
      case 50:
        await this.page.locator("div[title='menu-item-50']").click();
        break;
      case 100:
        await this.page.locator("div[title='menu-item-100']").click();
        break;
    }
  }

  async goNextPage() {
    await this.page.locator("ul[class='pagination'] li:last-child").click();
  }

  async go1stPage() {
    await this.page.locator("ul[class='pagination'] li:nth-child(2)").click();
  }

  async resetSearch() {
    await this.page.locator("#reset-button").click();
  }
}

module.exports = SharedPagination;
