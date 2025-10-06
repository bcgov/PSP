const { expect } = require("@playwright/test");

class WorkLists {
    constructor(page) {
        this.page = page;
    }

    async navigateWorkLists() {
        await this.page.locator("#worklistControlButton").click();
    }

    async verifyWorkListForm() {
        await expect(this.page.locator("//p[contains(text(),'Working list')]")).toBeVisible();
        await expect(this.page.locator("//div[contains(text(),'CTRL + Click to add a property')]")).toBeVisible();
    }

    async verifyWorklistWithProps() {
        await expect(this.page.locator("//p[contains(text(),'Working list')]")).toBeVisible();
        await expect(this.page.getByTestId("search-property-0")).toBeVisible();
    }
}

module.exports = WorkLists;